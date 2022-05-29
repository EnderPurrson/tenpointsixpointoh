using Rilisoft;
using System;
using UnityEngine;

internal sealed class ChestBonusModel
{
	public const string pathToCommonBonusItems = "Textures/Bank/StarterPack_Weapon";

	public ChestBonusModel()
	{
	}

	public static string GetUrlForDownloadBonusesData()
	{
		string empty = string.Empty;
		if (Defs.IsDeveloperBuild)
		{
			empty = "chest_bonus_test.json";
		}
		else if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			empty = (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 ? "chest_bonus_ios.json" : "chest_bonus_wp8.json");
		}
		else
		{
			empty = (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "chest_bonus_android.json" : "chest_bonus_amazon.json");
		}
		return string.Format("{0}{1}", "https://secure.pixelgunserver.com/pixelgun3d-config/chestBonus/", empty);
	}
}