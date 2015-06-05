using System;
using System.Threading;

namespace SCR
{
	public abstract class BaseMovingOnPitch: IMovable
	{
		public object LocationLock = new object();

		public Pitch FootballPitch;
		private const int Speed = 1;
		private Direction _direction;
		protected bool KeepMoving = true;

		public Thread Thread;
		private readonly Random _random;

		public Location Location { get; set; }

		protected BaseMovingOnPitch(Random random, Pitch pitch)
		{
			FootballPitch = pitch;
			_random = random;
			_direction = (Direction) _random.Next(0, 8);
			Thread = new Thread(Move);
		}

		public void Move()
		{
			while (KeepMoving)
			{
				_direction = (Direction)_random.Next(0, 8);
				lock (LocationLock)
				{
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
				}

				Thread.Sleep(Speed * 500);
			}
		}

		public void Stop()
		{
			KeepMoving = false;
			if (Thread.IsAlive)
				Thread.Join();
		}

		public int GetSpeed()
		{
			return Speed;
		}
	}
}
