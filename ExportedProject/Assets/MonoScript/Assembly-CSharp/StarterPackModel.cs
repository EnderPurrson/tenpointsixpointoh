using Rilisoft;
using System;
using UnityEngine;

public class StarterPackModel
{
	public const int MaxCountShownWindow = 1;

	private const float HoursToShowWindow = 8f;

	public const string pathToCoinsImage = "Textures/Bank/Coins_Shop_5";

	public const string pathToGemsImage = "Textures/Bank/Coins_Shop_Gem_5";

	public const string pathToGemsPackImage = "Textures/Bank/StarterPack_Gem";

	public const string pathToCoinsPackImage = "Textures/Bank/StarterPack_Gold";

	public const string pathToItemsPackImage = "Textures/Bank/StarterPack_Weapon";

	public static TimeSpan TimeOutShownWindow;

	public static TimeSpan MaxLiveTimeEvent;

	public static TimeSpan CooldownTimeEvent;

	public static string[] packNameLocalizeKey;

	public static int[] savingMoneyForBuyPack;

	static StarterPackModel()
	{
		StarterPackModel.TimeOutShownWindow = TimeSpan.FromHours(8);
		StarterPackModel.MaxLiveTimeEvent = TimeSpan.FromDays(1);
		StarterPackModel.CooldownTimeEvent = TimeSpan.FromDays(1.5);
		StarterPackModel.packNameLocalizeKey = new string[] { "Key_1049", "Key_1050", "Key_1051", "Key_1052", "Key_1053", "Key_1054", "Key_1055", "Key_1056" };
		StarterPackModel.savingMoneyForBuyPack = new int[] { 7, 5, 17, 14, 27, 21, 22, 42 };
	}

	public StarterPackModel()
	{
	}

	public static DateTime GetCurrentTimeByUnixTime(int unixTime)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return dateTime.AddSeconds((double)unixTime);
	}

	public static DateTime GetTimeDataEvent(string timeEventKey)
	{
		DateTime dateTime = new DateTime();
		string str = Storager.getString(timeEventKey, false);
		DateTime.TryParse(str, out dateTime);
		return dateTime;
	}

	public static string GetUrlForDownloadEventData()
	{
		string str = "https://secure.pixelgunserver.com/pixelgun3d-config/starterPack/";
		string empty = string.Empty;
		if (Defs.IsDeveloperBuild)
		{
			empty = "starter_pack_test.json";
		}
		else if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			empty = (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 ? "starter_pack_ios.json" : "starter_pack_wp8.json");
		}
		else
		{
			empty = (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "starter_pack_android.json" : "starter_pack_amazon.json");
		}
		return string.Format("{0}{1}", str, empty);
	}

	public enum TypeCost
	{
		Money,
		Gems,
		InApp,
		None
	}

	public enum TypePack
	{
		Items,
		Coins,
		Gems,
		None
	}
}