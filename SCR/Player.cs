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
			Direction = (Direction)_random.Next(0, 8);
			Thread = new Thread(Move);
		}

		public override void Move()
		{
			while (KeepMoving)
			{
				lock (FootballPitch.PitchLock)
				{
					if (NeighborsBall() && !HasBall())
						Ball.SetOwner(this);
					if (HasBall())
					{
						var previousBallLocation = (Location) Ball.Location.Clone();
						var nextBallLocation = new Location(Location.X, Team == Team.Light ? Location.Y + 2 : Location.Y - 2);

						Ball.MoveBall(nextBallLocation, previousBallLocation);
						Console.WriteLine(@"Ball: " + Ball.Location);

						Direction = Team == Team.Light ? Direction.Down : Direction.Up;
					}
					else
						Direction = (Direction) _random.Next(0, 8);

					var previousLocation = (Location) FootballPitch.GetField(Location).Field.Location.Clone();

					MoveInDirection(Direction);

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
