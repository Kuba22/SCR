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

		protected override bool OccupyLocation()
		{
			var field = FootballPitch.GetField(Location);
			if (field.Field.FieldType != OccupiedBy.None)
				return false;
			field.Field.FieldType = Team == Team.Light ? OccupiedBy.PlayerTeamLight : OccupiedBy.PlayerTeamDark;
			return true;
		}
	}

	public enum Team
	{
		Light,
		Dark
	}
}
