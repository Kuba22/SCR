using System;

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
	}
}
