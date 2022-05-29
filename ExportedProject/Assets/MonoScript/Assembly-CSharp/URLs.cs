using Rilisoft;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class URLs
{
	public const string UrlForTwitterPost = "http://goo.gl/dQMf4n";

	public static string BanURL;

	private readonly static Lazy<string> _trafficForwardingConfigUrl;

	public static string Friends;

	public static string ABTestBalansActualCohortNameURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/test/bCohortNameActual.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/bCohortNameActual.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/amazom/bCohortNameActual.json");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/wp/bCohortNameActual.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/ios/bCohortNameActual.json";
		}
	}

	public static string ABTestBalansFolderURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/test/";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/" : "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/amazon/");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/wp/";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/ios/";
		}
	}

	public static string ABTestBalansURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/test/abTest.php?key=0LjZB3GmACx15N7HJPYm";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/abTest.php?key=0LjZB3GmACx15N7HJPYm" : "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/amazon/abTest.php?key=0LjZB3GmACx15N7HJPYm");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/wp/abTest.php?key=0LjZB3GmACx15N7HJPYm";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/ios/abTest.php?key=0LjZB3GmACx15N7HJPYm";
		}
	}

	public static string ABTestBuffSettingsURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/wp.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/android.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/amazon.json";
			}
			return string.Empty;
		}
	}

	public static string ABTestQuestSystemURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_amazon.json");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_ios.json";
		}
	}

	public static string ABTestSandBoxURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_amazon.json");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_ios.json";
		}
	}

	public static string ABTestSpecialOffersURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_amazon.json");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_ios.json";
		}
	}

	public static string ABTestViewBankURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/test2505/currentView.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/android2505/currentView.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/amazon/currentView.json");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/wp/currentView.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/ios/currentView.json";
		}
	}

	public static string Advert
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_ios_TEST.json";
				}
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
				{
					return string.Empty;
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_amazon_TEST.json";
				}
				return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_android_TEST.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_amazon.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_android.json";
		}
	}

	public static string AmazonEvent
	{
		get
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/amazonEvent/amazon-event-test.json";
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/amazonEvent/amazon-event-test.json";
			}
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/amazonEvent/amazon-event-test.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/amazonEvent/amazon-event.json";
		}
	}

	public static string BestBuy
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_wp8.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_android.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_amazon.json";
			}
			return string.Empty;
		}
	}

	public static string BuffSettings
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_test", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_ios", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
				{
					return string.Empty;
				}
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_WP", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_android", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return string.Empty;
			}
			return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_amazon", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
		}
	}

	public static string BuffSettings1031
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_test", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_ios", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
				{
					return string.Empty;
				}
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_WP", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_android", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return string.Empty;
			}
			return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_amazon", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
		}
	}

	public static string BuffSettings1050
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_test", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_ios", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
				{
					return string.Empty;
				}
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_WP", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_android", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return string.Empty;
			}
			return string.Concat("https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_amazon", (!FlurryPluginWrapper.IsPayingUser() ? ".json" : "_paying.json"));
		}
	}

	public static string DayOfValor
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_wp8.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_android.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_amazon.json";
			}
			return string.Empty;
		}
	}

	public static string EventX3
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/event_x3_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/event_x3_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/event_x3_wp8.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/event_x3_android.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/event_x3_amazon.json";
			}
			return string.Empty;
		}
	}

	public static string FilterBadWord
	{
		get
		{
			return "https://secure.pixelgunserver.com/pixelgun3d-config/FilterBadWord.json";
		}
	}

	public static string LobbyNews
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_wp.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_androd.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_amazon.json";
			}
			return string.Empty;
		}
	}

	public static string PixelbookSettings
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_wp.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_androd.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_amazon.json";
			}
			return string.Empty;
		}
	}

	public static string PopularityMapUrl
	{
		get
		{
			int ourTier = ExpController.GetOurTier() + 1;
			string[] multiplayerProtocolVersion = new string[] { "https://secure.pixelgunserver.com/mapstats/", GlobalGameController.MultiplayerProtocolVersion, "_", null, null, null, null };
			int num = (int)ConnectSceneNGUIController.myPlatformConnect - (int)ConnectSceneNGUIController.PlatformConnect.ios;
			multiplayerProtocolVersion[3] = num.ToString();
			multiplayerProtocolVersion[4] = "_";
			multiplayerProtocolVersion[5] = ourTier.ToString();
			multiplayerProtocolVersion[6] = "_mapstat.json";
			return string.Concat(multiplayerProtocolVersion);
		}
	}

	public static string PremiumAccount
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_wp8.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_android.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_amazon.json";
			}
			return string.Empty;
		}
	}

	public static string PromoActions
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_amazon.json");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_wp8.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions.json";
		}
	}

	public static string PromoActionsTest
	{
		get
		{
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_test.json";
		}
	}

	public static string QuestConfig
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-wp8.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-android.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-amazon.json";
			}
			return string.Empty;
		}
	}

	public static string RatingSystemConfigURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ratingSystem/rating_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon ? "https://secure.pixelgunserver.com/pixelgun3d-config/ratingSystem/rating_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ratingSystem/rating_amazon.json");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/ratingSystem/rating_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBalansConfig/ratingSystem/rating_ios.json";
		}
	}

	internal static string TrafficForwardingConfigUrl
	{
		get
		{
			return URLs._trafficForwardingConfigUrl.Value;
		}
	}

	public static string TutorialQuestConfig
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "test");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "ios");
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
				{
					return string.Empty;
				}
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "wp8");
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "android");
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return string.Empty;
			}
			return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "amazon");
		}
	}

	static URLs()
	{
		URLs.BanURL = "https://secure.pixelgunserver.com/pixelgun3d-config/getBanList.php";
		URLs._trafficForwardingConfigUrl = new Lazy<string>(new Func<string>(URLs.InitializeTrafficForwardingConfigUrl));
		URLs.Friends = "https://pixelgunserver.com/~rilisoft/action.php";
	}

	public URLs()
	{
	}

	private static string InitializeTrafficForwardingConfigUrl()
	{
		if (Defs.IsDeveloperBuild)
		{
			return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "test");
		}
		RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
		switch (buildTargetPlatform)
		{
			case RuntimePlatform.IPhonePlayer:
			{
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "ios");
			}
			case RuntimePlatform.Android:
			{
				Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
				if (androidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "amazon");
				}
				if (androidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return string.Empty;
				}
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "android");
			}
			default:
			{
				if (buildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					break;
				}
				else
				{
					return string.Empty;
				}
			}
		}
		return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "wp8");
	}

	internal static string Sanitize(WWW request)
	{
		string str;
		if (request == null)
		{
			return string.Empty;
		}
		if (!request.isDone)
		{
			throw new InvalidOperationException("Request should be done.");
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			return string.Empty;
		}
		UTF8Encoding uTF8Encoding = new UTF8Encoding(false);
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
		{
			string str1 = uTF8Encoding.GetString(request.bytes, 0, (int)request.bytes.Length).Trim();
			return str1;
		}
		using (StreamReader streamReader = new StreamReader(new MemoryStream(request.bytes), uTF8Encoding))
		{
			str = streamReader.ReadToEnd().Trim();
		}
		return str;
	}
}