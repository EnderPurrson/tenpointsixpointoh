using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelBox
{
	public static List<LevelBox> campaignBoxes;

	public List<CampaignLevel> levels = new List<CampaignLevel>(6);

	public int starsToOpen;

	public string name;

	public string mapName;

	public string PreviewNAme = string.Empty;

	public int gems;

	public int coins;

	public static Dictionary<string, string> weaponsFromBosses;

	public int CompletionExperienceAward
	{
		get;
		set;
	}

	static LevelBox()
	{
		LevelBox.campaignBoxes = new List<LevelBox>(4);
		LevelBox.weaponsFromBosses = new Dictionary<string, string>(20);
		LevelBox.InitializeWeaponsFromBosses(LevelBox.weaponsFromBosses);
		LevelBox levelBox = new LevelBox()
		{
			starsToOpen = 2147483647,
			PreviewNAme = "Box_coming_soon",
			name = "coming soon",
			CompletionExperienceAward = 0
		};
		LevelBox levelBox1 = levelBox;
		levelBox = new LevelBox()
		{
			starsToOpen = 30,
			name = "Crossed",
			mapName = string.Empty,
			PreviewNAme = "Box_3",
			CompletionExperienceAward = 100,
			gems = 15,
			coins = 20
		};
		LevelBox levelBox2 = levelBox;
		levelBox2.levels.Add(new CampaignLevel("Swamp_campaign3", "Key_0471", "in"));
		levelBox2.levels.Add(new CampaignLevel("Castle_campaign3", "Key_1317", "in"));
		levelBox2.levels.Add(new CampaignLevel("Space_campaign3", "Key_0519", "in"));
		levelBox2.levels.Add(new CampaignLevel("Parkour", "Key_1318", "in"));
		levelBox2.levels.Add(new CampaignLevel("Code_campaign3", "Key_0845", "in"));
		levelBox = new LevelBox()
		{
			starsToOpen = 20,
			name = "minecraft",
			mapName = string.Empty,
			PreviewNAme = "Box_2",
			CompletionExperienceAward = 70,
			gems = 10,
			coins = 15
		};
		LevelBox levelBox3 = levelBox;
		CampaignLevel campaignLevel = new CampaignLevel()
		{
			sceneName = "Utopia",
			localizeKeyForLevelMap = "Key_0841",
			predlog = "in"
		};
		CampaignLevel campaignLevel1 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Maze",
			localizeKeyForLevelMap = "Key_0842",
			predlog = "in"
		};
		CampaignLevel campaignLevel2 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Sky_islands",
			localizeKeyForLevelMap = "Key_0843",
			predlog = "on"
		};
		CampaignLevel campaignLevel3 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Winter",
			localizeKeyForLevelMap = "Key_0844",
			predlog = "on"
		};
		CampaignLevel campaignLevel4 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Castle",
			localizeKeyForLevelMap = "Key_0845",
			predlog = "in"
		};
		CampaignLevel campaignLevel5 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Gluk_2",
			localizeKeyForLevelMap = "Key_0846",
			predlog = "in"
		};
		CampaignLevel campaignLevel6 = campaignLevel;
		levelBox3.levels.Add(campaignLevel1);
		levelBox3.levels.Add(campaignLevel2);
		levelBox3.levels.Add(campaignLevel3);
		levelBox3.levels.Add(campaignLevel4);
		levelBox3.levels.Add(campaignLevel5);
		levelBox3.levels.Add(campaignLevel6);
		levelBox = new LevelBox()
		{
			starsToOpen = 0,
			name = "Real",
			mapName = string.Empty,
			PreviewNAme = "Box_1",
			CompletionExperienceAward = 50,
			gems = 5,
			coins = 15
		};
		LevelBox levelBox4 = levelBox;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Farm",
			localizeKeyForLevelMap = "Key_0832",
			predlog = "at"
		};
		CampaignLevel campaignLevel7 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Cementery",
			localizeKeyForLevelMap = "Key_0833",
			predlog = "in"
		};
		CampaignLevel campaignLevel8 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "City",
			localizeKeyForLevelMap = "Key_0834",
			predlog = "in"
		};
		CampaignLevel campaignLevel9 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Hospital",
			localizeKeyForLevelMap = "Key_0835",
			predlog = "in"
		};
		CampaignLevel campaignLevel10 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Bridge",
			localizeKeyForLevelMap = "Key_0836",
			predlog = "on"
		};
		CampaignLevel campaignLevel11 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Jail",
			localizeKeyForLevelMap = "Key_0837",
			predlog = "at"
		};
		CampaignLevel campaignLevel12 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Slender",
			localizeKeyForLevelMap = "Key_0838",
			predlog = "in"
		};
		CampaignLevel campaignLevel13 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "Area52",
			localizeKeyForLevelMap = "Key_0839",
			predlog = "at"
		};
		CampaignLevel campaignLevel14 = campaignLevel;
		campaignLevel = new CampaignLevel()
		{
			sceneName = "School",
			localizeKeyForLevelMap = "Key_0840",
			predlog = "in"
		};
		CampaignLevel campaignLevel15 = campaignLevel;
		levelBox4.levels.Add(campaignLevel7);
		levelBox4.levels.Add(campaignLevel8);
		levelBox4.levels.Add(campaignLevel9);
		levelBox4.levels.Add(campaignLevel10);
		levelBox4.levels.Add(campaignLevel11);
		levelBox4.levels.Add(campaignLevel12);
		levelBox4.levels.Add(campaignLevel13);
		levelBox4.levels.Add(campaignLevel14);
		levelBox4.levels.Add(campaignLevel15);
		LevelBox.campaignBoxes.Add(levelBox4);
		LevelBox.campaignBoxes.Add(levelBox3);
		LevelBox.campaignBoxes.Add(levelBox2);
		LevelBox.campaignBoxes.Add(levelBox1);
	}

	public LevelBox()
	{
	}

	public static CampaignLevel GetLevelBySceneName(string sceneName)
	{
		CampaignLevel item;
		try
		{
			item = LevelBox.campaignBoxes.SelectMany<LevelBox, CampaignLevel>((LevelBox levelBox) => levelBox.levels).FirstOrDefault<CampaignLevel>((CampaignLevel level) => level.sceneName == sceneName);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			item = LevelBox.campaignBoxes[0].levels[0];
		}
		return item;
	}

	private static void InitializeWeaponsFromBosses(Dictionary<string, string> weaponsFromBosses)
	{
		weaponsFromBosses.Add("Farm", WeaponManager.UZI_WN);
		weaponsFromBosses.Add("Cementery", WeaponManager.MP5WN);
		weaponsFromBosses.Add("City", "Weapon4");
		weaponsFromBosses.Add("Hospital", "Weapon8");
		weaponsFromBosses.Add("Jail", "Weapon5");
		weaponsFromBosses.Add("Slender", "Weapon51");
		weaponsFromBosses.Add("Area52", "Weapon52");
		weaponsFromBosses.Add("Bridge", WeaponManager.M16_2WN);
		weaponsFromBosses.Add("Utopia", WeaponManager.CampaignRifle_WN);
		weaponsFromBosses.Add("Maze", WeaponManager.SimpleFlamethrower_WN);
		weaponsFromBosses.Add("Sky_islands", WeaponManager.Rocketnitza_WN);
		weaponsFromBosses.Add("Code_campaign3", WeaponManager.BugGunWN);
	}
}