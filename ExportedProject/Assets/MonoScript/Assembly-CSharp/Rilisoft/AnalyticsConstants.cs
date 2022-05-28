namespace Rilisoft
{
	public sealed class AnalyticsConstants
	{
		public enum AccrualType
		{
			Earned = 0,
			Purchased = 1
		}

		public enum TutorialState
		{
			Started = 0,
			Controls_Overview = 1,
			Controls_Move = 2,
			Controls_Jump = 3,
			Kill_Enemy = 4,
			Portal = 5,
			Rewards = 6,
			Open_Shop = 7,
			Category_Sniper = 8,
			Equip_Sniper = 9,
			Category_Armor = 10,
			Equip_Armor = 11,
			Back_Shop = 12,
			Connect_Scene = 13,
			Table_Deathmatch = 14,
			Play_Deathmatch = 15,
			Deathmatch_Completed = 16,
			Finished = 17
		}

		public const string LevelUp = "LevelUp";

		public const string ViralityEvent = "Virality";

		public const string SocialEvent = "Social";
	}
}
