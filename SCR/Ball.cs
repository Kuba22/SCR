using System;
using System.Windows.Forms;

namespace SCR
{
	public class Ball: BaseMovingOnPitch
	{
		public Player PlayerWithBall = null;

		public Ball(Pitch pitch): base(pitch)
		{
		}

		public void SetOwner(Player player)
		{
			PlayerWithBall = player;
		}

		public bool IsControlledBy(Player player)
		{
			if (player == PlayerWithBall)
				return true;
			return false;
		}

		public void MoveBall(Location to, Location from)
		{
			if (to.X + 1 > FootballPitch.Width || to.X < 0 || to.Y + 1 > FootballPitch.Length || to.Y < 0)
			{
				Location = new Location(FootballPitch.Width/2, FootballPitch.Length/2);
				SetOwner(null);
			}
			else
				Location = to;
			if (OccupyLocation())
				FreeLocation(from);
			else
				Location = from;
		}

		public bool InGoal()
		{
			return Location.Y == FootballPitch.Length - 1 || Location.Y == 0;
		}

		protected override bool OccupyLocation()
		{
			var field = FootballPitch.GetField(Location);
			if (field.Field.FieldType != OccupiedBy.None)
				return false;
			field.Field.FieldType = OccupiedBy.Ball;
			return true;
		}
	}

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
		UpLeft,
		UpRight,
		DownLeft,
		DownRight,
	}
}
