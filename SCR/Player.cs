﻿using System;
using System.Threading;
using System.Windows.Forms;

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
					if (NeighborsBall() && !HasBall())
						Ball.SetOwner(this);
					if (HasBall())
					{
						_direction = Team == Team.Light ? Direction.Down : Direction.Up;
						var previousBallLoc = (Location) Ball.Location.Clone();
						Ball.MoveBall(new Location(Location.X, Team == Team.Light ? Location.Y + 1 : Location.Y - 1), previousBallLoc);
						Console.WriteLine(@"Ball: " + Ball.Location);
						if (Ball.InGoal())
						{
							MessageBox.Show(@"Team {0} scored", Team.ToString());
							Ball.MoveBall(new Location(FootballPitch.Width/2, FootballPitch.Length/2), previousBallLoc);
						}
					}
					else
						_direction = (Direction)_random.Next(0, 8);
				}

				lock (LocationLock)
				{
					var previousLocation = (Location)FootballPitch.GetField(Location).Field.Location.Clone();

					MoveInDirection(_direction);

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
