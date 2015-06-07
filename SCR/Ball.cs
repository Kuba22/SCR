using System;

namespace SCR
{
	public class Ball: BaseMovingOnPitch
	{
		public Player PlayerWithBall = null;

		public object BallLock = new object();

		public Ball(Random random, Pitch pitch): base(pitch)
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
