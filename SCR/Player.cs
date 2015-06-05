using System;

namespace SCR
{
	public class Player: BaseMovingOnPitch
	{
		public Team Team;

		public Player(Random random, Pitch pitch) : base(random, pitch)
		{
		}

		public void UpdateBallLocation()
		{
			
		}
	}

	public enum Team
	{
		Light,
		Dark
	}
}
