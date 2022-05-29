using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal static class FlurryEvents
	{
		public const string PurchasesPointEventName = "Purchases Points";

		public const string CoinsGained = "Coins Gained";

		public const string FeatureEnabled = "Feature Enabled";

		public const string ShopPurchasesFormat = "Shop Purchases {0}";

		public const string TimeWeaponsFormat = "Time Weapons {0}";

		public const string TimeWeaponsRedFormat = "Time Weapons (red test) {0}";

		public const string TimeArmorAndHatFormat = "Time Armor and Hat {0}";

		public const string GemsFormat = "Purchase for Gems {0}";

		public const string GemsTempArmorFormat = "Purchase for Gems TempArmor {0}";

		public const string CoinsFormat = "Purchase for Coins {0}";

		public const string CoinsTempArmorFormat = "Purchase for Coins TempArmor {0}";

		public const string TrainingProgress = "Training Progress";

		public const string PurchaseAfterPayment = "Purchase After Payment";

		public const string PurchaseAfterPaymentCumulative = "Purchase After Payment Cumulative";

		public const string FastPurchase = "Fast Purchase";

		public const string AfterTraining = "After Training";

		public static Dictionary<ShopNGUIController.CategoryNames, string> shopCategoryToLogSalesNamesMapping;

		public static float? PaymentTime
		{
			get;
			set;
		}

		static FlurryEvents()
		{
			Dictionary<ShopNGUIController.CategoryNames, string> categoryNames = new Dictionary<ShopNGUIController.CategoryNames, string>()
			{
				{ ShopNGUIController.CategoryNames.SkinsCategory, "Skins" },
				{ ShopNGUIController.CategoryNames.PrimaryCategory, "Primary" },
				{ ShopNGUIController.CategoryNames.BackupCategory, "Back Up" },
				{ ShopNGUIController.CategoryNames.MeleeCategory, "Melee" },
				{ ShopNGUIController.CategoryNames.SpecilCategory, "Special" },
				{ ShopNGUIController.CategoryNames.SniperCategory, "Sniper" },
				{ ShopNGUIController.CategoryNames.PremiumCategory, "Premium" },
				{ ShopNGUIController.CategoryNames.ArmorCategory, "Armor" },
				{ ShopNGUIController.CategoryNames.BootsCategory, "Boots" },
				{ ShopNGUIController.CategoryNames.CapesCategory, "Capes" },
				{ ShopNGUIController.CategoryNames.HatsCategory, "Hats" },
				{ ShopNGUIController.CategoryNames.GearCategory, "Gear" },
				{ ShopNGUIController.CategoryNames.MaskCategory, "Masks" }
			};
			FlurryEvents.shopCategoryToLogSalesNamesMapping = categoryNames;
		}

		private static string CurrentContextForNonePlaceInBecomePaying()
		{
			string empty = string.Empty;
			try
			{
				if (Defs.inRespawnWindow)
				{
					empty = string.Concat(empty, " Killcam");
				}
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
				{
					empty = string.Concat(empty, " PlayerExists");
				}
				if (NetworkStartTableNGUIController.IsEndInterfaceShown())
				{
					empty = string.Concat(empty, " NetworkStartTable_End");
				}
				if (NetworkStartTableNGUIController.IsStartInterfaceShown())
				{
					empty = string.Concat(empty, " NetworkStartTable_Start");
				}
				if (ShopNGUIController.GuiActive)
				{
					empty = string.Concat(empty, " InShop");
				}
				string str = (FlurryPluginWrapper.ModeNameForPurchasesAnalytics(false) ?? string.Empty).Replace(" ", string.Empty);
				if (str != string.Empty)
				{
					empty = string.Concat(empty, " ", str);
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in CurrentContextForNonePlaceInBecomePaying: ", exception));
			}
			return empty;
		}

		private static string GetGameModeEventName(string gameMode)
		{
			return string.Concat("Game Mode ", gameMode);
		}

		public static string GetPlayingMode()
		{
			if (Application.loadedLevelName == Defs.MainMenuScene)
			{
				return "Main Menu";
			}
			if (SceneLoader.ActiveSceneName.Equals("ConnectScene", StringComparison.OrdinalIgnoreCase))
			{
				return "Connect Scene";
			}
			if (SceneLoader.ActiveSceneName.Equals("ConnectSceneSandbox", StringComparison.OrdinalIgnoreCase))
			{
				return "Connect Scene Sandbox";
			}
			if (!Defs.IsSurvival && !Defs.isMulti)
			{
				return "Campaign";
			}
			if (Defs.IsSurvival)
			{
				return (!Defs.isMulti ? "Survival" : "Time Survival");
			}
			if (Defs.isCompany)
			{
				return "Team Battle";
			}
			if (Defs.isFlag)
			{
				return "Flag Capture";
			}
			if (Defs.isHunger)
			{
				return "Deadly Games";
			}
			if (Defs.isCapturePoints)
			{
				return "Capture Points";
			}
			return (!Defs.isInet ? "Deathmatch Local" : "Deathmatch Worldwide");
		}

		public static void LogAfterTraining(string action, bool trainingState)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ (!trainingState ? "Skipped" : "Completed"), action ?? string.Empty }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("After Training", strs, true);
		}

		public static void LogBecomePaying(string purchaseId)
		{
			DateTime dateTime;
			DateTime dateTime1;
			DateTime dateTime2;
			try
			{
				string str = PlayerPrefs.GetString(Defs.DateOfInstallAppForInAppPurchases041215, string.Empty);
				if (!string.IsNullOrEmpty(str) && DateTime.TryParse(str, out dateTime))
				{
					Dictionary<string, string> strs = new Dictionary<string, string>();
					string str1 = PlayerPrefs.GetString(Defs.FirstInAppPurchaseDate_041215, string.Empty);
					if (string.IsNullOrEmpty(str1) || !DateTime.TryParse(str1, out dateTime1))
					{
						DateTime utcNow = DateTime.UtcNow;
						PlayerPrefs.SetString(Defs.FirstInAppPurchaseDate_041215, utcNow.ToString("s"));
						int num = FlurryEvents.NumberOfDaysForBecomePaying(dateTime, utcNow);
						strs.Add("First Purchase (Day)", num.ToString());
						string empty = string.Empty;
						if (Application.loadedLevelName == Defs.MainMenuScene && MainMenuController.sharedController != null && MainMenuController.sharedController.InAdventureScreen)
						{
							empty = "Connect Scene Adventure";
						}
						else if (Application.loadedLevelName == "ConnectScene")
						{
							empty = "Connect Scene Multiplayer";
						}
						else if (Application.loadedLevelName == "ConnectSceneSandbox")
						{
							empty = "Connect Scene Sandbox";
						}
						else if (Application.loadedLevelName == "Clans")
						{
							empty = "Clans Premium Map";
						}
						else if (!(Initializer.Instance != null) || !Defs.isInet || !Defs.isMulti || PhotonNetwork.connected || NetworkStartTableNGUIController.IsStartInterfaceShown() || NetworkStartTableNGUIController.IsEndInterfaceShown())
						{
							empty = FlurryPluginWrapper.PlaceForPurchasesAnalytics(false);
							if (empty == "None")
							{
								empty = string.Concat(empty, " ", FlurryEvents.CurrentContextForNonePlaceInBecomePaying());
							}
						}
						else
						{
							empty = "Disconnected";
						}
						strs.Add("Point First Purchase", empty);
						strs.Add("First Purchase Total", AnalyticsStuff.ReadableNameForInApp(purchaseId));
					}
					else
					{
						string str2 = PlayerPrefs.GetString(Defs.SecondInAppPurchaseDate_041215, string.Empty);
						if (string.IsNullOrEmpty(str2) || !DateTime.TryParse(str2, out dateTime2))
						{
							DateTime utcNow1 = DateTime.UtcNow;
							PlayerPrefs.SetString(Defs.SecondInAppPurchaseDate_041215, utcNow1.ToString("s"));
							int num1 = FlurryEvents.NumberOfDaysForBecomePaying(dateTime1, utcNow1);
							strs.Add("Second Purchase (Day After First)", num1.ToString());
							int num2 = FlurryEvents.NumberOfDaysForBecomePaying(dateTime, utcNow1);
							strs.Add("Second Purchase (Day After Start)", num2.ToString());
						}
					}
					int num3 = FlurryEvents.NumberOfDaysForBecomePaying(dateTime, DateTime.UtcNow);
					strs.Add("Purchase Day Total", num3.ToString());
					if (Debug.isDebugBuild)
					{
						Debug.Log(string.Concat("<color=green>LogBecomePaying = Become Paying</color>\n<color=white>parameters = ", strs.ToStringFull(), "</color>"));
					}
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Become Paying", strs, true);
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogBecomePaying: ", exception));
			}
		}

		public static void LogCoinsGained(string mode, int coinCount)
		{
			mode = mode ?? string.Empty;
			string str = (ExperienceController.sharedController == null ? "Unknown" : ExperienceController.sharedController.currentLevel.ToString());
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Total", mode },
				{ string.Concat(mode, " (Rank)"), str }
			};
			Dictionary<string, string> strs1 = strs;
			if (coinCount >= 1000)
			{
				string str1 = "Coins Gained Suspiciously Large Amount";
				strs1.Add("Amount", coinCount.ToString());
				FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs1, true);
				return;
			}
			int num = coinCount;
			for (int i = 1; num > 0 && i < 100; i *= 10)
			{
				int num1 = num % 10;
				string str2 = string.Format("{0} x{1}", "Coins Gained", i);
				for (int j = 0; j < num1; j++)
				{
					FlurryPluginWrapper.LogEventAndDublicateToConsole(str2, strs1, true);
				}
				num /= 10;
			}
			if (num > 0)
			{
				string str3 = string.Format("{0} x{1}", "Coins Gained", 100);
				for (int k = 0; k < num; k++)
				{
					FlurryPluginWrapper.LogEventAndDublicateToConsole(str3, strs1, true);
				}
			}
		}

		public static void LogGemsGained(string mode, int gemsCount)
		{
		}

		public static void LogPurchaseStickers(string stickersPackId)
		{
			try
			{
				if (!string.IsNullOrEmpty(stickersPackId))
				{
					Dictionary<string, string> strs = new Dictionary<string, string>()
					{
						{ "Purchases", stickersPackId },
						{ "Points", BuySmileBannerController.GetCurrentBuySmileContextName() }
					};
					Dictionary<string, string> strs1 = strs;
					if (Debug.isDebugBuild)
					{
						Debug.Log(string.Concat("<color=green>LogPurchaseStickers = Purchases Stickers Total</color>\n<color=white>parameters = ", strs1.ToStringFull(), "</color>"));
						Debug.Log(string.Concat(new string[] { "<color=green>LogPurchaseStickers = Purchases Stickers", FlurryPluginWrapper.GetPayingSuffixNo10(), "</color>\n<color=white>parameters = ", strs1.ToStringFull(), "</color>" }));
					}
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchases Stickers Total", strs1, true);
					FlurryPluginWrapper.LogEventAndDublicateToConsole(string.Concat("Purchases Stickers", FlurryPluginWrapper.GetPayingSuffixNo10()), strs1, true);
				}
				else
				{
					Debug.LogError("LogPurchaseStickers: string.IsNullOrEmpty(stickersPackId)");
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogPurchaseStickers: ", exception));
			}
		}

		public static void LogRateUsFake(bool rate, int stars = 0, bool sendNegativFeedback = false)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Efficiency", (!rate ? "Later" : "Rate") }
			};
			if (rate)
			{
				strs.Add("Rating (Stars)", stars.ToString());
			}
			if (stars > 0 && stars < 4)
			{
				strs.Add("Negative Feedback", (!sendNegativFeedback ? "Not sended" : "Sended"));
			}
			if (Debug.isDebugBuild)
			{
				Debug.Log(string.Concat("<color=green>LogRateUsFake = Rate Us Fake</color>\n<color=white>parameters = ", strs.ToStringFull(), "</color>"));
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Rate Us Fake", strs, true);
		}

		public static void LogTrainingProgress(string kind)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Kind", kind }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Training Progress", strs, true);
		}

		private static int NumberOfDaysForBecomePaying(DateTime earlyDate, DateTime futureDate)
		{
			TimeSpan timeSpan = futureDate - earlyDate;
			return (int)(Math.Ceiling(timeSpan.TotalDays) + 0.5);
		}

		internal static void StartLoggingGameModeEvent()
		{
			FlurryEvents.StartLoggingGameModeEvent(FlurryEvents.GetGameModeEventName(FlurryEvents.GetPlayingMode()));
		}

		private static void StartLoggingGameModeEvent(string eventName)
		{
			FlurryPluginWrapper.LogTimedEventAndDublicateToConsole(eventName);
		}

		internal static void StopLoggingGameModeEvent()
		{
			string[] strArrays = new string[] { "Main Menu", "Connect Scene", "Campaign", "Time Survival", "Survival", "Team Battle", "Flag Capture", "Deadly Games", "Deathmatch Worldwide", "Deathmatch Local" };
			IEnumerable<string> strs = strArrays.Select<string, string>(new Func<string, string>(FlurryEvents.GetGameModeEventName));
			IEnumerator<string> enumerator = strs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					FlurryEvents.StopLoggingGameModeEvent(enumerator.Current);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}

		internal static void StopLoggingGameModeEvent(string eventName)
		{
			FlurryPluginWrapper.EndTimedEvent(eventName);
		}
	}
}