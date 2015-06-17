using System;
using System.Drawing;

namespace SCR
{
	public abstract class BaseMovingOnPitch: IMovable
	{
		public object LocationLock = new object();

		public Pitch FootballPitch;
		protected const int Speed = 1;
		protected Direction _direction;
		protected bool KeepMoving = true;

		public Location Location { get; set; }

		public Bitmap Image;

		protected BaseMovingOnPitch(Pitch pitch)
		{
			FootballPitch = pitch;
		}

		public virtual void Move()
		{
		}

		protected virtual void FreeLocation(Location location)
		{
			FootballPitch.GetField(location).Field.FieldType = OccupiedBy.None;
		}

		protected virtual bool OccupyLocation()
		{
			throw new NotImplementedException();
		}

		public int GetSpeed()
		{
			return Speed;
		}

		protected void MoveInDirection(Direction direction)
		{
			if (direction == Direction.Up && Location.Y > 0)
				Location.Y--;
			else if (direction == Direction.Down && Location.Y + 1 < FootballPitch.Length)
				Location.Y++;
			else if (direction == Direction.Left && Location.X > 0)
				Location.X--;
			else if (direction == Direction.Right && Location.X + 1 < FootballPitch.Width)
				Location.X++;
			else if (direction == Direction.UpLeft &&
					Location.X - 1 > 0 &&
					Location.Y - 1 > 0)
			{
				Location.X--;
				Location.Y--;
			}
			else if (direction == Direction.UpRight &&
					Location.X + 1 < FootballPitch.Width &&
					Location.Y - 1 > 0)
			{
				Location.X++;
				Location.Y--;
			}
			else if (direction == Direction.DownLeft &&
					Location.X - 1 > 0 &&
					Location.Y + 1 < FootballPitch.Length)
			{
				Location.X--;
				Location.Y++;
			}
			else if (direction == Direction.DownRight &&
					Location.X + 1 < FootballPitch.Width &&
					Location.Y + 1 < FootballPitch.Length)
			{
				Location.X++;
				Location.Y++;
			}
		}
	}
}
