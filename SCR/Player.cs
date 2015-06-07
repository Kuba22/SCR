using System;
using System.Threading;

namespace SCR
{
	public class Player: BaseMovingOnPitch
	{
		public Team Team;

		public Ball Ball;

		public Thread Thread;

		private readonly Random _random;

		public Player(Random random, Pitch pitch, Ball ball) : base(pitch)
		{
			_random = random;
			Ball = ball;
			_direction = (Direction)_random.Next(0, 8);
			Thread = new Thread(Move);
		}

		public override void Move()
		{
			while (KeepMoving)
			{
				lock (Ball.BallLock)
				{
					if (NeighborsBall())
						Ball.SetOwner(this);
				}

				_direction = (Direction)_random.Next(0, 8);
				lock (LocationLock)
				{
					var previousLocation = (Location)FootballPitch.GetField(Location).Field.Location.Clone();
					if (_direction == Direction.Up && Location.Y > 0)
						Location.Y--;
					else if (_direction == Direction.Down && Location.Y + 1 < FootballPitch.Length)
						Location.Y++;
					else if (_direction == Direction.Left && Location.X > 0)
						Location.X--;
					else if (_direction == Direction.Right && Location.X + 1 < FootballPitch.Width)
						Location.X++;
					else if (_direction == Direction.UpLeft &&
							Location.X - 1 > 0 &&
							Location.Y - 1 > 0)
					{
						Location.X--;
						Location.Y--;
					}
					else if (_direction == Direction.UpRight &&
							Location.X + 1 < FootballPitch.Width &&
							Location.Y - 1 > 0)
					{
						Location.X++;
						Location.Y--;
					}
					else if (_direction == Direction.DownLeft &&
							Location.X - 1 > 0 &&
							Location.Y + 1 < FootballPitch.Length)
					{
						Location.X--;
						Location.Y++;
					}
					else if (_direction == Direction.DownRight &&
							Location.X + 1 < FootballPitch.Width &&
							Location.Y + 1 < FootballPitch.Length)
					{
						Location.X++;
						Location.Y++;
					}

					if (OccupyLocation())
						FreeLocation(previousLocation);
					else
						Location = previousLocation;
				}

				Thread.Sleep(Speed * 500);
			}
		}

		private bool NeighborsBall()
		{
			var result = Ball.Location.Neighbors(Location);
			if (result)
				Ball.PlayerWithBall = this;
			return result;
		}

		private bool HasBall()
		{
			return Ball.IsControlledBy(this);
		}

		protected override bool OccupyLocation()
		{
			var field = FootballPitch.GetField(Location);
			if (field.Field.FieldType != OccupiedBy.None)
				return false;
			field.Field.FieldType = Team == Team.Light ? OccupiedBy.PlayerTeamLight : OccupiedBy.PlayerTeamDark;
			return true;
		}

		public void Stop()
		{
			KeepMoving = false;
			if (Thread.IsAlive)
				Thread.Join();
		}
	}

	public enum Team
	{
		Light,
		Dark
	}
}
