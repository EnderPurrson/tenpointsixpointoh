using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public sealed class FlurryPluginWrapper : MonoBehaviour
{
	public const string BackToMainMenu = "Back to Main Menu";

	public const string UnlockHungerMoney = "Enable_Deadly Games";

	public const string AppsFlyerInitiatedCheckout = "af_initiated_checkout";

	private const string TeamBattleForPurchasesAnalytics = "Team Battle";

	private const string DeathmatchForPurchasesAnalytics = "Deathmatch";

	public static string ModeEnteredEvent;

	public static string MapEnteredEvent;

	public static string MapNameParameter;

	public static string ModeParameter;

	public static string ModePressedEvent;

	public static string SocialEventName;

	public static string SocialParName;

	public static string AppVersionParameter;

	public static string MultiplayeLocalEvent;

	public static string MultiplayerWayDeaathmatchEvent;

	public static string MultiplayerWayCOOPEvent;

	public static string MultiplayerWayCompanyEvent;

	public static string WayName;

	public readonly static string HatsCapesShopPressedEvent;

	public static string FreeCoinsEv;

	public static string FreeCoinsParName;

	public static string RateUsEv;

	private float _startSession;

	private static bool _sessionStarted;

	private static Dictionary<string, int> antiCheatLimitsPaying;

	private static Dictionary<string, int> antiCheatLimitsNonPaying;

	public static Dictionary<string, string> LevelAndTierParameters
	{
		get
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Levels", ((ExperienceController.sharedController == null ? 0 : ExperienceController.sharedController.currentLevel)).ToString() },
				{ "Tiers", ((ExpController.Instance == null ? 0 : ExpController.Instance.OurTier) + 1).ToString() }
			};
			return strs;
		}
	}

	public static string MultiplayerWayEvent
	{
		get
		{
			string multiplayerWayCOOPEvent;
			if (!Defs.isCOOP)
			{
				multiplayerWayCOOPEvent = (!Defs.isCompany ? FlurryPluginWrapper.MultiplayerWayDeaathmatchEvent : FlurryPluginWrapper.MultiplayerWayCompanyEvent);
			}
			else
			{
				multiplayerWayCOOPEvent = FlurryPluginWrapper.MultiplayerWayCOOPEvent;
			}
			return multiplayerWayCOOPEvent;
		}
	}

	static FlurryPluginWrapper()
	{
		FlurryPluginWrapper.ModeEnteredEvent = "ModeEnteredEvent";
		FlurryPluginWrapper.MapEnteredEvent = "MapEnteredEvent";
		FlurryPluginWrapper.MapNameParameter = "MapName";
		FlurryPluginWrapper.ModeParameter = "Mode";
		FlurryPluginWrapper.ModePressedEvent = "ModePressed";
		FlurryPluginWrapper.SocialEventName = "Post to Social";
		FlurryPluginWrapper.SocialParName = "Service";
		FlurryPluginWrapper.AppVersionParameter = "App_version";
		FlurryPluginWrapper.MultiplayeLocalEvent = "Local Button Pressed";
		FlurryPluginWrapper.MultiplayerWayDeaathmatchEvent = "Way_to_start_multiplayer_DEATHMATCH";
		FlurryPluginWrapper.MultiplayerWayCOOPEvent = "Way_to_start_multiplayer_COOP";
		FlurryPluginWrapper.MultiplayerWayCompanyEvent = "Way_to_start_multiplayer_Company";
		FlurryPluginWrapper.WayName = "Button";
		FlurryPluginWrapper.HatsCapesShopPressedEvent = "Hats_Capes_Shop";
		FlurryPluginWrapper.FreeCoinsEv = "FreeCoins";
		FlurryPluginWrapper.FreeCoinsParName = "type";
		FlurryPluginWrapper.RateUsEv = "Rate_Us";
		FlurryPluginWrapper._sessionStarted = false;
		Dictionary<string, int> strs = new Dictionary<string, int>()
		{
			{ "Coins", 14000 },
			{ "GemsCurrency", 11000 }
		};
		FlurryPluginWrapper.antiCheatLimitsPaying = strs;
		strs = new Dictionary<string, int>()
		{
			{ "Coins", 50000 },
			{ "GemsCurrency", 900 }
		};
		FlurryPluginWrapper.antiCheatLimitsNonPaying = strs;
	}

	public FlurryPluginWrapper()
	{
	}

	private void CheckForEdnermanApp()
	{
	}

	public static string ConvertFromBase64(string s)
	{
		byte[] numArray = Convert.FromBase64String(s);
		return Encoding.UTF8.GetString(numArray, 0, (int)numArray.Length);
	}

	public static string ConvertToBase64(string s)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
	}

	public static void DoWithResponse(HttpWebRequest request, Action<HttpWebResponse> responseAction)
	{
		Action action = () => request.BeginGetResponse((IAsyncResult iar) => {
			HttpWebResponse httpWebResponse = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
			responseAction(httpWebResponse);
		}, request);
		action.BeginInvoke((IAsyncResult iar) => ((Action)iar.AsyncState).EndInvoke(iar), action);
	}

	private void EndSession()
	{
		float single = Time.realtimeSinceStartup - this._startSession;
		this._startSession += single;
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Duration (s)", single.ToString("F1", CultureInfo.InvariantCulture) },
			{ "Timestamp (UTC)", DateTime.UtcNow.ToString("s") }
		};
		FlurryPluginWrapper.LogEventToAppsFlyer("End session", strs);
	}

	public static void EndTimedEvent(string eventName)
	{
	}

	private static void FlurryLogEventCore(string eventName, bool isTimed = false)
	{
	}

	private static void FlurryLogEventWithParametersCore(string ev, Dictionary<string, string> parameters, bool isTimed = false)
	{
	}

	private void FriendsController_NewCheaterDetectParametersAvailable(int coinsLimitPaying, int gemsLimitPaying, int coinsLimitNonPaying, int gemsLimitNonPaying)
	{
		FlurryPluginWrapper.antiCheatLimitsPaying["Coins"] = coinsLimitPaying;
		FlurryPluginWrapper.antiCheatLimitsPaying["GemsCurrency"] = gemsLimitPaying;
		FlurryPluginWrapper.antiCheatLimitsNonPaying["Coins"] = coinsLimitNonPaying;
		FlurryPluginWrapper.antiCheatLimitsNonPaying["GemsCurrency"] = gemsLimitNonPaying;
	}

	public static string GetEventName(string eventName)
	{
		return string.Concat(eventName, FlurryPluginWrapper.GetPayingSuffix());
	}

	public static string GetEventX3State()
	{
		if (!PromoActionsManager.sharedManager.IsEventX3Active)
		{
			return "None";
		}
		if (PromoActionsManager.sharedManager.IsNewbieEventX3Active)
		{
			return "Newbie";
		}
		return "Common";
	}

	public static string GetPayingSuffix()
	{
		return FlurryPluginWrapper.GetPayingSuffixNo10();
	}

	public static string GetPayingSuffixNo10()
	{
		if (!FlurryPluginWrapper.IsPayingUser())
		{
			return " (Non Paying)";
		}
		return " (Paying)";
	}

	public static bool IsAdditionalLoggingAvailable()
	{
		bool flag;
		try
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				flag = (File.Exists(FlurryPluginWrapper.ConvertFromBase64("L0FwcGxpY2F0aW9ucy9DeWRpYS5hcHA=")) || File.Exists(FlurryPluginWrapper.ConvertFromBase64("L0xpYnJhcnkvTW9iaWxlU3Vic3RyYXRlL01vYmlsZVN1YnN0cmF0ZS5keWxpYg==")) || File.Exists(FlurryPluginWrapper.ConvertFromBase64("L2Jpbi9iYXNo")) || File.Exists(FlurryPluginWrapper.ConvertFromBase64("L3Vzci9zYmluL3NzaGQ=")) || File.Exists(FlurryPluginWrapper.ConvertFromBase64("L2V0Yy9hcHQ=")) ? true : Directory.Exists(FlurryPluginWrapper.ConvertFromBase64("L3ByaXZhdGUvdmFyL2xpYi9hcHQv")));
			}
			else
			{
				flag = false;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogWarning(string.Concat("Exception in IsAdditionalLoggingAvailable: ", exception));
			flag = false;
		}
		return flag;
	}

	public static bool IsLoggingFlurryAnalyticsSupported()
	{
		bool flag;
		try
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				string str = FlurryPluginWrapper.ConvertFromBase64("L0xpYnJhcnkvTW9iaWxlU3Vic3RyYXRlL0R5bmFtaWNMaWJyYXJpZXM=");
				if (File.Exists(Path.Combine(str, FlurryPluginWrapper.ConvertFromBase64("TG9jYWxJQVBTdG9yZS5keWxpYg=="))) || File.Exists(Path.Combine(str, FlurryPluginWrapper.ConvertFromBase64("TG9jYWxsQVBTdG9yZS5keWxpYg=="))))
				{
					UnityEngine.Debug.LogWarning("IsLoggingFlurryAnalyticsSupported: logging supported");
					flag = true;
				}
				else if (File.Exists(Path.Combine(str, FlurryPluginWrapper.ConvertFromBase64("aWFwLmR5bGli"))))
				{
					UnityEngine.Debug.LogWarning("IsLoggingFlurryAnalyticsSupported: logging_supported");
					flag = true;
				}
				else if (File.Exists(Path.Combine(str, FlurryPluginWrapper.ConvertFromBase64("aWFwZnJlZS5jb3JlLmR5bGli"))) || File.Exists(Path.Combine(str, FlurryPluginWrapper.ConvertFromBase64("SUFQRnJlZVNlcnZpY2UuZHlsaWI="))))
				{
					UnityEngine.Debug.LogWarning("IsLoggingFlurryAnalyticsSupported: logging__supported");
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogWarning(string.Concat("Exception in IsLoggingFlurryAnalyticsSupported: ", exception));
			flag = false;
		}
		return flag;
	}

	public static bool IsPayingUser()
	{
		return Storager.getInt("PayingUser", true) > 0;
	}

	public static void LogAddYourSkinTriedToBoughtEvent()
	{
		FlurryPluginWrapper.LogEvent("AddYourSkin_TriedToBought");
	}

	public static void LogAddYourSkinUsedEvent()
	{
		FlurryPluginWrapper.LogEvent("AddYourSkin_Used");
	}

	public static void LogBoxOpened(string nm)
	{
		FlurryPluginWrapper.LogEvent(string.Concat(nm, "_Box_Opened"));
	}

	public static void LogCampaignModePress()
	{
		FlurryPluginWrapper.LogModeEventWithValue("Survival");
		FlurryPluginWrapper.LogEvent("Campaign");
	}

	public static void LogCategoryEnteredEvent(string catName)
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue("Dhop_Category", "Category_name", catName);
	}

	public static void LogCoinEarned()
	{
		FlurryPluginWrapper.LogEvent("Earned Coin Survival");
	}

	public static void LogCoinEarned_COOP()
	{
		FlurryPluginWrapper.LogEvent("Earned Coin COOP");
	}

	public static void LogCoinEarned_Deathmatch()
	{
		FlurryPluginWrapper.LogEvent("Earned Coin Deathmatch");
	}

	public static void LogCooperativeModePress()
	{
		FlurryPluginWrapper.LogModeEventWithValue("COOP");
		FlurryPluginWrapper.LogEvent("Cooperative");
	}

	public static void LogDeathmatchModePress()
	{
		FlurryPluginWrapper.LogModeEventWithValue("Deathmatch");
		FlurryPluginWrapper.LogEvent("Deathmatch");
	}

	public static void LogEnteringMap(int typeConnect, string mapName)
	{
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ FlurryPluginWrapper.MapNameParameter, mapName }
		};
		FlurryPluginWrapper.FlurryLogEventWithParametersCore((Defs.isCOOP ? "COOP" : "Deathmatch_WorldWide"), strs, false);
	}

	public static void LogEvent(string eventName)
	{
		FlurryPluginWrapper.FlurryLogEventCore(eventName, false);
	}

	public static void LogEvent(string eventName, Dictionary<string, string> parameters, bool addPaying = true)
	{
		if (addPaying && !parameters.ContainsKey("Paying User"))
		{
			parameters.Add("Paying User", FlurryPluginWrapper.IsPayingUser().ToString());
		}
		FlurryPluginWrapper.FlurryLogEventWithParametersCore(eventName, parameters, false);
	}

	public static void LogEventAndDublicateToConsole(string eventName, Dictionary<string, string> parameters, bool addPaying = true)
	{
		FlurryPluginWrapper.LogEvent(eventName, parameters, addPaying);
		if (Defs.IsDeveloperBuild)
		{
			string str = (parameters == null ? "{}" : Json.Serialize(parameters));
			if (!Application.isEditor)
			{
				UnityEngine.Debug.LogFormat("{0}: {1}", new object[] { eventName, str });
			}
			else
			{
				UnityEngine.Debug.LogFormat("<color=lightblue>{0}: {1}</color>", new object[] { eventName, str });
			}
		}
	}

	public static void LogEventAndDublicateToEditor(string eventName, Dictionary<string, string> parameters, Color32 color = default(Color32))
	{
		FlurryPluginWrapper.LogEvent(eventName, parameters, true);
		if (Application.isEditor)
		{
			string str = (parameters == null ? "{ }" : Json.Serialize(parameters));
			Color color1 = color;
			Color color2 = new Color();
			UnityEngine.Debug.Log((color1 != color2 ? string.Format("<color=#{2:X2}{3:X2}{4:X2}>{0}: {1}</color>", new object[] { eventName, str, color.r, color.g, color.b }) : string.Concat(eventName, ": ", str)));
		}
	}

	internal static void LogEventToAppsFlyer(string eventName, Dictionary<string, string> attributes)
	{
		if (string.IsNullOrEmpty(eventName))
		{
			UnityEngine.Debug.LogError("Event name should not be empty.");
			return;
		}
		if (attributes == null)
		{
			UnityEngine.Debug.LogError("Event values should not be null.");
			return;
		}
		if (!attributes.ContainsKey("deviceModel"))
		{
			attributes["deviceModel"] = SystemInfo.deviceModel;
		}
		if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > 0 && !attributes.ContainsKey("level"))
		{
			attributes["level"] = ExperienceController.sharedController.currentLevel.ToString();
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			AppsFlyer.trackRichEvent(eventName, attributes);
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("{0}: {1}", new object[] { eventName, Json.Serialize(attributes) });
		}
	}

	public static void LogEventWithParameterAndValue(string ev, string pat, string val)
	{
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ pat, val },
			{ "Paying User", FlurryPluginWrapper.IsPayingUser().ToString() }
		};
		FlurryPluginWrapper.FlurryLogEventWithParametersCore(ev, strs, false);
	}

	public static void LogFacebook()
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.SocialEventName, FlurryPluginWrapper.SocialParName, "Facebook");
	}

	public static void LogFastPurchase(string purchaseKind)
	{
		if (ExperienceController.sharedController == null)
		{
			UnityEngine.Debug.LogWarning("ExperienceController.sharedController == null");
		}
		else
		{
			int num = ExperienceController.sharedController.currentLevel;
			int num1 = (num - 1) / 9;
			string str = string.Format("[{0}, {1})", num1 * 9 + 1, (num1 + 1) * 9 + 1);
			string str1 = string.Format("Shop Purchases On Level {0} ({1}){2}", str, (!FlurryPluginWrapper.IsPayingUser() ? "Non Paying" : "Paying"), string.Empty);
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ string.Concat("Level ", num), purchaseKind }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs, true);
		}
	}

	public static void LogFreeCoinsFacebook()
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.FreeCoinsEv, FlurryPluginWrapper.FreeCoinsParName, "Facebook");
		FlurryPluginWrapper.LogEvent("Facebook");
	}

	public static void LogFreeCoinsRateUs()
	{
		FlurryPluginWrapper.LogEvent(FlurryPluginWrapper.RateUsEv);
	}

	public static void LogFreeCoinsTwitter()
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.FreeCoinsEv, FlurryPluginWrapper.FreeCoinsParName, "Twitter");
		FlurryPluginWrapper.LogEvent("Twitter");
	}

	public static void LogFreeCoinsYoutube()
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.FreeCoinsEv, FlurryPluginWrapper.FreeCoinsParName, "Youtube");
		FlurryPluginWrapper.LogEvent("YouTube");
	}

	public static void LogGamecenter()
	{
		FlurryPluginWrapper.LogEvent("Game Center");
	}

	public static void LogGearPurchases(string gearId, int gearCount, bool fromBottomPanel)
	{
		try
		{
			if (gearId != null)
			{
				string str = string.Format("Gear Purchases{0}", FlurryPluginWrapper.GetPayingSuffixNo10());
				string str1 = FlurryPluginWrapper.PlaceForPurchasesAnalytics(fromBottomPanel);
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "Total", gearId }
				};
				if (!string.IsNullOrEmpty(str1))
				{
					strs.Add(str1, gearId);
				}
				for (int i = 0; i < gearCount; i++)
				{
					if (UnityEngine.Debug.isDebugBuild)
					{
						UnityEngine.Debug.Log(string.Concat(new string[] { "<color=green>GearPurchasesEventName = ", str, "</color>\n<color=white>parameters = ", strs.ToStringFull(), "</color>" }));
					}
				}
				for (int j = 0; j < gearCount; j++)
				{
					FlurryPluginWrapper.LogEventAndDublicateToConsole(str, strs, true);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("LogGearPurchases: gearId = null");
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in LogGearPurchases: ", exception));
		}
	}

	public static void LogLevelPressed(string n)
	{
		FlurryPluginWrapper.FlurryLogEventCore(string.Concat(n, "_Pressed"), false);
	}

	public static void LogMatchCompleted(string mode)
	{
		if (ExperienceController.sharedController != null)
		{
			string str = string.Format("Match Completed ({0})", (!FlurryPluginWrapper.IsPayingUser() ? "Non Paying" : "Paying"));
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Rank", ExperienceController.sharedController.currentLevel.ToString() },
				{ "Mode", mode }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str, strs, true);
		}
	}

	public static void LogModeEventWithValue(string val)
	{
		if (PlayerPrefs.HasKey("Mode Pressed First Time"))
		{
			FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.ModePressedEvent, FlurryPluginWrapper.ModeParameter, val);
		}
		else
		{
			PlayerPrefs.SetInt("Mode Pressed First Time", 0);
			FlurryPluginWrapper.LogEventWithParameterAndValue("Mode Pressed First Time", FlurryPluginWrapper.ModeParameter, val);
		}
	}

	public static void LogMultiplayeLocalEvent()
	{
		FlurryPluginWrapper.LogEvent(FlurryPluginWrapper.MultiplayeLocalEvent);
	}

	public static void LogMultiplayerWayCustom()
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.MultiplayerWayEvent, FlurryPluginWrapper.WayName, "Custom");
		FlurryPluginWrapper.LogEvent("Custom");
	}

	public static void LogMultiplayerWayQuckRandGame()
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.MultiplayerWayEvent, FlurryPluginWrapper.WayName, "Quick_rand_game");
		FlurryPluginWrapper.LogEvent("Random");
	}

	public static void LogMultiplayerWayStart()
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.MultiplayerWayEvent, FlurryPluginWrapper.WayName, "Start");
		FlurryPluginWrapper.LogEvent("Start");
	}

	public static void LogMultiplayeWorldwideEvent()
	{
		FlurryPluginWrapper.LogEvent("Worldwide");
	}

	public static void LogPurchaseByModes(ShopNGUIController.CategoryNames category, string itemId, int count, bool UNUSED_fromBottomPanel)
	{
		try
		{
			string str = FlurryPluginWrapper.ModeNameForPurchasesAnalytics(false);
			if (str != null && !(Application.loadedLevelName == Defs.MainMenuScene))
			{
				string str1 = string.Format("Purchases {0}{1}", str, FlurryPluginWrapper.GetPayingSuffixNo10());
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "All Categories", category.ToString() },
					{ category.ToString(), itemId }
				};
				Dictionary<string, string> strs1 = strs;
				for (int i = 0; i < count; i++)
				{
					if (UnityEngine.Debug.isDebugBuild)
					{
						UnityEngine.Debug.Log(string.Concat(new string[] { "<color=green>EventName = ", str1, "</color>\n<color=white>parameters = ", strs1.ToStringFull(), "</color>" }));
					}
				}
				for (int j = 0; j < count; j++)
				{
					FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs1, true);
				}
				if (!Defs.isInet && (str == "Team Battle" || str == "Deathmatch"))
				{
					string str2 = string.Format("Purchases Local{0}", FlurryPluginWrapper.GetPayingSuffixNo10());
					for (int k = 0; k < count; k++)
					{
						if (UnityEngine.Debug.isDebugBuild)
						{
							UnityEngine.Debug.Log(string.Concat(new string[] { "<color=green>EventName = ", str2, "</color>\n<color=white>parameters = ", strs1.ToStringFull(), "</color>" }));
						}
					}
					for (int l = 0; l < count; l++)
					{
						FlurryPluginWrapper.LogEventAndDublicateToConsole(str2, strs1, true);
					}
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in LogPurchaseByModes: ", exception));
		}
	}

	public static void LogPurchaseByPoints(ShopNGUIController.CategoryNames category, string itemId, int count)
	{
		try
		{
			string str = FlurryPluginWrapper.PlaceForPurchasesAnalytics(false).Replace("Single ", string.Empty);
			string str1 = string.Format("Purchases {0} {1}{2}", FlurryPluginWrapper.ModeNameForPurchasesAnalytics(true) ?? string.Empty, str, FlurryPluginWrapper.GetPayingSuffixNo10()).Replace("Multiplayer Lobby", "Lobby");
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "All Categories", category.ToString() },
				{ category.ToString(), itemId }
			};
			Dictionary<string, string> strs1 = strs;
			for (int i = 0; i < count; i++)
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log(string.Concat(new string[] { "<color=green>PurchaseInModeEventName = ", str1, "</color>\n<color=white>parameters = ", strs1.ToStringFull(), "</color>" }));
				}
			}
			for (int j = 0; j < count; j++)
			{
				FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs1, true);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in LogPurchaseByPoints: ", exception));
		}
	}

	public static void LogPurchasesPoints(bool isWeaponEvent)
	{
		try
		{
			string str = FlurryPluginWrapper.PlaceForPurchasesAnalytics(false);
			string str1 = FlurryPluginWrapper.PurchasesPointsEventName();
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Total", str }
			};
			if (isWeaponEvent)
			{
				strs.Add("Total Weapons", str);
			}
			string str2 = null;
			str2 = FlurryPluginWrapper.ModeNameForPurchasesAnalytics(false);
			if (str2 != null)
			{
				strs.Add(str2, str);
			}
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log(string.Concat(new string[] { "<color=green>PurchasesPointsEventName = ", str1, "</color>\n<color=white>parameters = ", strs.ToStringFull(), "</color>" }));
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs, true);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in LogPurchasesPoints: ", exception));
		}
	}

	public static void LogSkinsMakerEnteredEvent()
	{
		FlurryPluginWrapper.LogEvent("SkinsMaker");
	}

	public static void LogSkinsMakerModePress()
	{
		FlurryPluginWrapper.LogEvent("Skins Maker");
	}

	public static void LogTimedEvent(string eventName)
	{
		FlurryPluginWrapper.FlurryLogEventCore(eventName, true);
	}

	public static void LogTimedEvent(string eventName, Dictionary<string, string> parameters)
	{
		if (!parameters.ContainsKey("Paying User"))
		{
			parameters.Add("Paying User", FlurryPluginWrapper.IsPayingUser().ToString());
		}
		FlurryPluginWrapper.FlurryLogEventWithParametersCore(eventName, parameters, true);
	}

	public static void LogTimedEventAndDublicateToConsole(string eventName)
	{
		FlurryPluginWrapper.LogTimedEvent(eventName);
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(eventName);
		}
	}

	public static void LogTrueSurvivalModePress()
	{
		FlurryPluginWrapper.LogModeEventWithValue("Arena_Survival");
		FlurryPluginWrapper.LogEvent("Survival");
	}

	public static void LogTwitter()
	{
		FlurryPluginWrapper.LogEventWithParameterAndValue(FlurryPluginWrapper.SocialEventName, FlurryPluginWrapper.SocialParName, "Twitter");
	}

	public static void LogWinInMatch(string mode)
	{
		if (ExperienceController.sharedController != null)
		{
			string str = string.Format("Win In Match ({0})", (!FlurryPluginWrapper.IsPayingUser() ? "Non Paying" : "Paying"));
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Rank", ExperienceController.sharedController.currentLevel.ToString() },
				{ "Mode", mode }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str, strs, true);
		}
	}

	public static string ModeNameForPurchasesAnalytics(bool forNormalMultyModesUseMultyplayer = false)
	{
		string str = null;
		try
		{
			if ((Defs.IsSurvival ? false : !Defs.isMulti))
			{
				str = "Campaign";
			}
			else if (Defs.IsSurvival && !Defs.isMulti)
			{
				str = "Arena";
			}
			else if (Defs.isMulti && Application.loadedLevelName != Defs.MainMenuScene && Application.loadedLevelName != "Clans")
			{
				if (Defs.isDaterRegim)
				{
					str = "Sandbox";
				}
				else if (forNormalMultyModesUseMultyplayer)
				{
					str = "Multiplayer";
				}
				else if (Defs.isCompany)
				{
					str = "Team Battle";
				}
				else if (Defs.isCapturePoints)
				{
					str = "Point Capture";
				}
				else if (Defs.isCOOP)
				{
					str = "COOP Survival";
				}
				else if (!Defs.isFlag)
				{
					str = (!Defs.isHunger ? "Deathmatch" : "Deadly Games");
				}
				else
				{
					str = "Flag Capture";
				}
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ModeNameForPurchasesAnalytics: ", exception));
		}
		return str;
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pause)
	{
		FlurryPluginWrapper.u003cOnApplicationPauseu003ec__Iterator139 variable = null;
		return variable;
	}

	private void OnApplicationQuit()
	{
		this.EndSession();
	}

	private void OnDestroy()
	{
		FriendsController.NewCheaterDetectParametersAvailable -= new Action<int, int, int, int>(this.FriendsController_NewCheaterDetectParametersAvailable);
	}

	public static string PlaceForPurchasesAnalytics(bool fromBottomPanel = false)
	{
		string str = "None";
		try
		{
			if (fromBottomPanel)
			{
				if (!(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.myPlayerMoveC != null))
				{
					str = "Bottom Panel";
				}
				else
				{
					str = (Defs.isMulti ? "Bottom Panel" : "Single Bottom Panel");
				}
			}
			else if (Application.loadedLevelName == Defs.MainMenuScene)
			{
				str = "Lobby";
			}
			else if (Defs.inRespawnWindow)
			{
				str = "Killcam";
			}
			else if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				str = (Defs.isMulti ? "Battle" : "Single Battle");
			}
			else if (Application.loadedLevelName == "LevelComplete")
			{
				str = "Single Score (End)";
			}
			else if (NetworkStartTableNGUIController.IsEndInterfaceShown())
			{
				str = "Score (End)";
			}
			else if (Application.loadedLevelName == "ChooseLevel")
			{
				str = "Single Score (Start)";
			}
			else if (NetworkStartTableNGUIController.IsStartInterfaceShown())
			{
				str = "Score (Start)";
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in PlaceForPurchasesAnalytics: ", exception));
		}
		return str;
	}

	public static string PurchasesPointsEventName()
	{
		return string.Format("{0}{1}", "Purchases Points", FlurryPluginWrapper.GetPayingSuffixNo10());
	}

	public static HttpWebRequest RequestAppWithID(string _id)
	{
		return (HttpWebRequest)WebRequest.Create(string.Concat("http://itunes.apple.com/lookup?id=", _id));
	}

	private void Start()
	{
		FriendsController.NewCheaterDetectParametersAvailable += new Action<int, int, int, int>(this.FriendsController_NewCheaterDetectParametersAvailable);
		this.StartSession();
		this.CheckForEdnermanApp();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		FlurryPluginWrapper._sessionStarted = true;
	}

	private void StartSession()
	{
		this._startSession = Time.realtimeSinceStartup;
		int num = PlayerPrefs.GetInt("AppsFlyer.SessionIndex", 0) + 1;
		PlayerPrefs.SetInt("AppsFlyer.SessionIndex", num);
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Session count", num.ToString(CultureInfo.InvariantCulture) },
			{ "Timestamp (UTC)", DateTime.UtcNow.ToString("s") }
		};
		FlurryPluginWrapper.LogEventToAppsFlyer("Start session", strs);
	}

	private static bool UserIsCheaterByCurrenciesCount()
	{
		Dictionary<string, int> strs = (!FlurryPluginWrapper.IsPayingUser() ? FlurryPluginWrapper.antiCheatLimitsNonPaying : FlurryPluginWrapper.antiCheatLimitsPaying);
		return (Storager.getInt("Coins", false) >= strs["Coins"] ? true : Storager.getInt("GemsCurrency", false) >= strs["GemsCurrency"]);
	}
}