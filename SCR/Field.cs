using System;

namespace SCR
{
	public class Field
	{
		public Location Location;

		public OccupiedBy FieldType { get; set; }

		public Field(Location location)
		{
			Location = location;
			FieldType = OccupiedBy.None;
		}
	}

	public enum OccupiedBy
	{
		PlayerTeamLight,
		PlayerTeamDark,
		Ball,
		None
	}

	public class Location: ICloneable
	{
		public int X;
		public int Y;

		public Location(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return String.Format("{0}, {1}", X, Y);
		}

		public object Clone()
		{
			return new Location(X, Y);
		}
	}
}
