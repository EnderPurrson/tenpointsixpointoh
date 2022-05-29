using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class AnalyticsStuff
	{
		private const string eventNameBase = "Daily Gift";

		private const string WeaponsSpecialOffersEvent = "Weapons Special Offers";

		private static int trainingStep;

		private static bool trainingStepLoaded;

		private static string trainingProgressKey;

		private static string[] trainingCustomEventStepValues;

		public static int TrainingStep
		{
			get
			{
				AnalyticsStuff.LoadTrainingStep();
				return AnalyticsStuff.trainingStep;
			}
		}

		static AnalyticsStuff()
		{
			AnalyticsStuff.trainingStep = -1;
			AnalyticsStuff.trainingStepLoaded = false;
			AnalyticsStuff.trainingProgressKey = "TrainingStepKeyAnalytics";
			AnalyticsStuff.trainingCustomEventStepValues = new string[] { "1_First Launch", "2_Controls_Overview", "3_Controls_Move", "4_Controls_Jump", "5_Kill_Enemy", "6_Portal", "7_Rewards", "8_Open Shop", "9_Category_Sniper", "10_Equip Sniper", "11_Category_Armor", "12_Equip Armor", "13_Back Shop", "14_Connect Scene", "15_Table Deathmatch", "16_Play Deathmatch", "17(0)_Deathmatch Win", "18_Level Up (Finished)", "17(1)_Deathmatch Lose" };
		}

		public AnalyticsStuff()
		{
		}

		private static void LoadTrainingStep()
		{
			if (!AnalyticsStuff.trainingStepLoaded)
			{
				if (Storager.hasKey(AnalyticsStuff.trainingProgressKey))
				{
					AnalyticsStuff.trainingStep = Storager.getInt(AnalyticsStuff.trainingProgressKey, false);
				}
				else
				{
					AnalyticsStuff.trainingStep = -1;
					Storager.setInt(AnalyticsStuff.trainingProgressKey, AnalyticsStuff.trainingStep, false);
				}
				AnalyticsStuff.trainingStepLoaded = true;
			}
		}

		public static void LogABTest(string nameTest, string nameCohort, bool isStart = true)
		{
			try
			{
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ nameTest, (!isStart ? string.Concat("Excluded ", nameCohort) : nameCohort) }
				};
				AnalyticsFacade.SendCustomEvent("A/B Test", strs);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("A/B Test exception: ", exception));
			}
		}

		public static void LogArenaFirst(bool isPause, bool isMoreOneWave)
		{
			object obj;
			try
			{
				Dictionary<string, object> strs = new Dictionary<string, object>();
				Dictionary<string, object> strs1 = strs;
				if (!isPause)
				{
					obj = (!isMoreOneWave ? "Fail" : "Complete");
				}
				else
				{
					obj = "Quit";
				}
				strs1.Add("First", obj);
				AnalyticsFacade.SendCustomEvent("Arena", strs);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("ArenaFirst  exception: ", exception));
			}
		}

		public static void LogArenaWavesPassed(int countWaveComplite)
		{
			try
			{
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "Waves Passed", (countWaveComplite >= 9 ? ">=9" : countWaveComplite.ToString()) }
				};
				AnalyticsFacade.SendCustomEvent("Arena", strs);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("ArenaFirst  exception: ", exception));
			}
		}

		public static void LogCampaign(string map, string boxName)
		{
			try
			{
				if (!string.IsNullOrEmpty(map))
				{
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ "Maps", map }
					};
					if (boxName != null)
					{
						strs.Add("Boxes", boxName);
					}
					AnalyticsFacade.SendCustomEvent("Campaign", strs);
				}
				else
				{
					Debug.LogError("LogCampaign string.IsNullOrEmpty(map)");
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogCampaign: ", exception));
			}
		}

		public static void LogDailyGift(string giftId, int count, bool isForMoneyGift)
		{
			try
			{
				if (!string.IsNullOrEmpty(giftId))
				{
					if (SkinsController.shopKeyFromNameSkin.ContainsKey(giftId))
					{
						giftId = "Skin";
					}
					giftId = string.Concat(giftId, "_", count);
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ "Chance", giftId },
						{ "Spins", (!isForMoneyGift ? "Free" : "Paid") }
					};
					Dictionary<string, object> strs1 = strs;
					AnalyticsFacade.SendCustomEvent("Daily Gift Total", strs1);
					AnalyticsFacade.SendCustomEvent(string.Concat("Daily Gift", FlurryPluginWrapper.GetPayingSuffixNo10()), strs1);
				}
				else
				{
					Debug.LogError("LogDailyGift: string.IsNullOrEmpty(giftId)");
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogDailyGift: ", exception));
			}
		}

		public static void LogDailyGiftPurchases(string packId)
		{
			try
			{
				if (!string.IsNullOrEmpty(packId))
				{
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ "Purchases", AnalyticsStuff.ReadableNameForInApp(packId) }
					};
					Dictionary<string, object> strs1 = strs;
					AnalyticsFacade.SendCustomEvent("Daily Gift Total", strs1);
					AnalyticsFacade.SendCustomEvent(string.Concat("Daily Gift", FlurryPluginWrapper.GetPayingSuffixNo10()), strs1);
				}
				else
				{
					Debug.LogError("LogDailyGiftPurchases: string.IsNullOrEmpty(packId)");
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogDailyGiftPurchases: ", exception));
			}
		}

		public static void LogFirstBattlesKillRate(int battleIndex, float killRate)
		{
			try
			{
				string empty = string.Empty;
				if (killRate < 0.4f)
				{
					empty = "<0,4";
				}
				else if (killRate < 0.6f)
				{
					empty = "0,4 - 0,6";
				}
				else if (killRate < 0.8f)
				{
					empty = "0,6 - 0,8";
				}
				else if (killRate < 1f)
				{
					empty = "0,8 - 1";
				}
				else if (killRate < 1.2f)
				{
					empty = "1 - 1,2";
				}
				else if (killRate < 1.5f)
				{
					empty = "1,2 - 1,5";
				}
				else if (killRate >= 2f)
				{
					empty = (killRate >= 3f ? ">3" : "2 - 3");
				}
				else
				{
					empty = "1,5 - 2";
				}
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ string.Concat("Battle ", battleIndex.ToString()), empty }
				};
				AnalyticsFacade.SendCustomEvent("First Battles KillRate", strs);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogFirstBattlesKillRate: ", exception));
			}
		}

		public static void LogFirstBattlesResult(int battleIndex, bool winner)
		{
			try
			{
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ string.Concat("Battle ", battleIndex.ToString()), (!winner ? "Lose" : "Win") }
				};
				AnalyticsFacade.SendCustomEvent("First Battles Result", strs);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogFirstBattlesResult: ", exception));
			}
		}

		public static void LogMultiplayer()
		{
			try
			{
				string str = ConnectSceneNGUIController.regim.ToString();
				if (str != null)
				{
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ "Game Modes", str },
						{ string.Concat(str, " By Tier"), ExpController.OurTierForAnyPlace() + 1 }
					};
					Dictionary<string, object> strs1 = strs;
					AnalyticsFacade.SendCustomEvent("Multiplayer Total", strs1);
					AnalyticsFacade.SendCustomEvent(string.Concat("Multiplayer", FlurryPluginWrapper.GetPayingSuffixNo10()), strs1);
				}
				else
				{
					Debug.LogError("LogMultiplayer modeName == null");
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogMultiplayer: ", exception));
			}
		}

		public static void LogSales(string itemId, string categoryParameterName, bool isDaterWeapon = false)
		{
			try
			{
				if (string.IsNullOrEmpty(itemId))
				{
					Debug.LogError("LogSales: string.IsNullOrEmpty(itemId)");
				}
				else if (!string.IsNullOrEmpty(categoryParameterName))
				{
					string[] strArrays = new string[] { "Stickers", "Premium Maps", "Gear", "Premium Account", "Skins", "Armor", "Boots", "Capes", "Hats", "Starter Pack", "Masks" };
					string[] strArrays1 = new string[] { "Primary", "Back Up", "Melee", "Special", "Sniper", "Premium" };
					string str = (!strArrays.Contains<string>(categoryParameterName) ? "Weapons Sales" : "Equipment Sales");
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ categoryParameterName, itemId }
					};
					if (isDaterWeapon)
					{
						strs.Add("Dater Weapons", itemId);
					}
					AnalyticsFacade.SendCustomEvent(string.Concat(str, " Total"), strs);
					AnalyticsFacade.SendCustomEvent(string.Concat(str, FlurryPluginWrapper.GetPayingSuffixNo10()), strs);
				}
				else
				{
					Debug.LogError("LogSales: string.IsNullOrEmpty(categoryParameterName)");
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogSales: ", exception));
			}
		}

		public static void LogSandboxTimeGamePopularity(int timeGame, bool isStart)
		{
			try
			{
				string str = (timeGame == 5 || timeGame == 10 || timeGame == 15 ? string.Concat("Time ", timeGame.ToString()) : "Other");
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ str, (!isStart ? "End" : "Start") }
				};
				AnalyticsFacade.SendCustomEvent("Sandbox", strs);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Sandbox exception: ", exception));
			}
		}

		public static void LogSpecialOffersPanel(string efficiencyPArameter, string efficiencyValue, string additionalParameter = null, string additionalValue = null)
		{
			try
			{
				if (string.IsNullOrEmpty(efficiencyPArameter) || string.IsNullOrEmpty(efficiencyValue))
				{
					Debug.LogError("LogSpecialOffersPanel:  string.IsNullOrEmpty(efficiencyPArameter) || string.IsNullOrEmpty(efficiencyValue)");
				}
				else
				{
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ efficiencyPArameter, efficiencyValue }
					};
					if (additionalParameter != null && additionalValue != null)
					{
						strs.Add(additionalParameter, additionalValue);
					}
					AnalyticsFacade.SendCustomEvent("Special Offers Banner Total", strs);
					AnalyticsFacade.SendCustomEvent(string.Concat("Special Offers Banner", FlurryPluginWrapper.GetPayingSuffixNo10()), strs);
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogSpecialOffersPanel: ", exception));
			}
		}

		public static void LogTrafficForwarding(AnalyticsStuff.LogTrafficForwardingMode mode)
		{
			try
			{
				string str = (mode != AnalyticsStuff.LogTrafficForwardingMode.Show ? "Button Pressed" : "Button Show");
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "Conversion", str },
					{ string.Concat(str, " Levels"), (ExperienceController.sharedController == null ? 1 : ExperienceController.sharedController.currentLevel) },
					{ string.Concat(str, " Tiers"), ExpController.OurTierForAnyPlace() + 1 },
					{ string.Concat(str, " Paying"), (!FlurryPluginWrapper.IsPayingUser() ? "FALSE" : "TRUE") }
				};
				AnalyticsFacade.SendCustomEvent("Pereliv Button", strs);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogTrafficForwarding: ", exception));
			}
		}

		public static void LogWEaponsSpecialOffers_Conversion(bool show, string weaponId = null)
		{
			string str;
			try
			{
				if (show || !string.IsNullOrEmpty(weaponId))
				{
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ "Conversion", (!show ? "Buy" : "Show") }
					};
					Dictionary<string, object> strs1 = strs;
					try
					{
						float single = (!FriendsController.useBuffSystem ? KillRateCheck.instance.GetKillRate() : BuffSystem.instance.GetKillrateByInteractions());
						if (single > 0.5f)
						{
							str = (single > 1.2f ? "Strong" : "Normal");
						}
						else
						{
							str = "Weak";
						}
						string str1 = string.Format("Conversion {0} Players", str);
						if (show)
						{
							strs1.Add("Show (Tier)", ExpController.OurTierForAnyPlace() + 1);
							strs1.Add("Show (Level)", (ExperienceController.sharedController == null ? 1 : ExperienceController.sharedController.currentLevel));
							strs1.Add(str1, "Show");
						}
						else
						{
							strs1.Add("Currency Spended", weaponId);
							strs1.Add("Buy (Tier)", ExpController.OurTierForAnyPlace() + 1);
							strs1.Add("Buy (Level)", (ExperienceController.sharedController == null ? 1 : ExperienceController.sharedController.currentLevel));
							strs1.Add(str1, "Buy");
						}
					}
					catch (Exception exception)
					{
						Debug.LogError(string.Concat("Exception in LogWEaponsSpecialOffers_Conversion adding paramters: ", exception));
					}
					AnalyticsFacade.SendCustomEvent("Weapons Special Offers Total", strs1);
					AnalyticsFacade.SendCustomEvent(string.Concat("Weapons Special Offers", FlurryPluginWrapper.GetPayingSuffixNo10()), strs1);
				}
				else
				{
					Debug.LogError("LogWEaponsSpecialOffers_Conversion: string.IsNullOrEmpty(weaponId)");
				}
			}
			catch (Exception exception1)
			{
				Debug.LogError(string.Concat("Exception in LogWEaponsSpecialOffers_Conversion: ", exception1));
			}
		}

		public static void LogWEaponsSpecialOffers_MoneySpended(string packId)
		{
			try
			{
				if (!string.IsNullOrEmpty(packId))
				{
					Dictionary<string, object> strs = new Dictionary<string, object>()
					{
						{ "Money Spended", AnalyticsStuff.ReadableNameForInApp(packId) }
					};
					Dictionary<string, object> strs1 = strs;
					AnalyticsFacade.SendCustomEvent("Weapons Special Offers Total", strs1);
					AnalyticsFacade.SendCustomEvent(string.Concat("Weapons Special Offers", FlurryPluginWrapper.GetPayingSuffixNo10()), strs1);
				}
				else
				{
					Debug.LogError("LogWEaponsSpecialOffers_MoneySpended: string.IsNullOrEmpty(packId)");
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in LogWEaponsSpecialOffers_MoneySpended: ", exception));
			}
		}

		public static void RateUsFake(bool rate, int stars, bool sendNegativFeedback = false)
		{
			try
			{
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "Efficiency", (!rate ? "Later" : "Rate") }
				};
				if (rate)
				{
					strs.Add("Rating (Stars)", stars);
				}
				if (stars > 0 && stars < 4)
				{
					strs.Add("Negative Feedback", (!sendNegativFeedback ? "Not sended" : "Sended"));
				}
				AnalyticsFacade.SendCustomEvent("Rate Us Fake", strs);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in RateUsFake: ", exception));
			}
		}

		public static string ReadableNameForInApp(string purchaseId)
		{
			return (!StoreKitEventListener.inAppsReadableNames.ContainsKey(purchaseId) ? purchaseId : StoreKitEventListener.inAppsReadableNames[purchaseId]);
		}

		public static void SaveTrainingStep()
		{
			if (AnalyticsStuff.trainingStepLoaded)
			{
				Storager.setInt(AnalyticsStuff.trainingProgressKey, AnalyticsStuff.trainingStep, false);
			}
		}

		internal static void TrySendOnceToAppsFlyer(string eventName, Lazy<Dictionary<string, string>> eventParams, Version excludeVersion)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (excludeVersion == null)
			{
				throw new ArgumentNullException("excludeVersion");
			}
			try
			{
				if (new Version(Switcher.InitialAppVersion) <= excludeVersion)
				{
					return;
				}
			}
			catch
			{
				return;
			}
			string str = string.Concat("Analytics:", eventName);
			if (Storager.hasKey(str) && !string.IsNullOrEmpty(Storager.getString(str, false)))
			{
				return;
			}
			Storager.setString(str, Json.Serialize(eventParams), false);
			AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, eventParams.Value);
		}

		public static void TrySendOnceToAppsFlyer(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			string str = string.Concat("Analytics:", eventName);
			if (Storager.hasKey(str) && !string.IsNullOrEmpty(Storager.getString(str, false)))
			{
				return;
			}
			Storager.setString(str, "{}", false);
			AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, new Dictionary<string, string>());
		}

		public static void Tutorial(AnalyticsConstants.TutorialState step, bool winDeathmatch = true)
		{
			try
			{
				AnalyticsStuff.LoadTrainingStep();
				if ((int)step > AnalyticsStuff.trainingStep)
				{
					AnalyticsStuff.trainingStep = (int)step;
					AnalyticsFacade.Tutorial(step);
					AnalyticsFacade.SendCustomEvent("Tutorial", new Dictionary<string, object>()
					{
						{ "Progress", (!winDeathmatch ? AnalyticsStuff.trainingCustomEventStepValues[(int)AnalyticsStuff.trainingCustomEventStepValues.Length - 1] : AnalyticsStuff.trainingCustomEventStepValues[(int)step]) }
					});
					if (step > AnalyticsConstants.TutorialState.Portal)
					{
						AnalyticsStuff.SaveTrainingStep();
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in Tutorial: ", exception));
			}
		}

		public enum LogTrafficForwardingMode
		{
			Show,
			Press
		}
	}
}