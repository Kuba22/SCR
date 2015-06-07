using System;

namespace SCR
{
	public class Ball: BaseMovingOnPitch
	{
		public Ball(Random random, Pitch pitch): base(pitch)
		{
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
