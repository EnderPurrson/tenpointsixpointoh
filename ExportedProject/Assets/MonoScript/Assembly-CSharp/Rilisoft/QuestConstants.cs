using Rilisoft.NullExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	internal static class QuestConstants
	{
		public const string AddFriend = "addFriend";

		public const string GetGotcha = "getGotcha";

		public const string BreakSeries = "breakSeries";

		public const string CaptureFlags = "captureFlags";

		public const string CapturePoints = "capturePoints";

		public const string JoinClan = "joinClan";

		public const string KillFlagCarriers = "killFlagCarriers";

		public const string KillInCampaign = "killInCampaign";

		public const string KillInMode = "killInMode";

		public const string KillNpcWithWeapon = "killNpcWithWeapon";

		public const string KillViaHeadshot = "killViaHeadshot";

		public const string KillWithGrenade = "killWithGrenade";

		public const string KillWithWeapon = "killWithWeapon";

		public const string LikeFacebook = "likeFacebook";

		public const string LoginFacebook = "loginFacebook";

		public const string LoginTwitter = "loginTwitter";

		public const string MakeSeries = "makeSeries";

		public const string Revenge = "revenge";

		public const string SurviveWavesInArena = "surviveWavesInArena";

		public const string WinInMap = "winInMap";

		public const string WinInMode = "winInMode";

		public const string AnalyticsEventName = "Daily Quests";

		private readonly static HashSet<string> _supportedQuests;

		private readonly static Dictionary<string, ShopNGUIController.CategoryNames> _weaponSlots;

		private readonly static Dictionary<string, string> localizationQuests;

		static QuestConstants()
		{
			QuestConstants._supportedQuests = new HashSet<string>(new string[] { "breakSeries", "killFlagCarriers", "killInCampaign", "killInMode", "killNpcWithWeapon", "killViaHeadshot", "killWithGrenade", "killWithWeapon", "makeSeries", "revenge", "surviveWavesInArena", "winInMap", "winInMode", "captureFlags", "capturePoints" });
			Dictionary<string, ShopNGUIController.CategoryNames> strs = new Dictionary<string, ShopNGUIController.CategoryNames>()
			{
				{ "Backup", ShopNGUIController.CategoryNames.BackupCategory },
				{ "Melee", ShopNGUIController.CategoryNames.MeleeCategory },
				{ "Premium", ShopNGUIController.CategoryNames.PremiumCategory },
				{ "Primary", ShopNGUIController.CategoryNames.PrimaryCategory },
				{ "Sniper", ShopNGUIController.CategoryNames.SniperCategory },
				{ "Special", ShopNGUIController.CategoryNames.SpecilCategory }
			};
			QuestConstants._weaponSlots = strs;
			Dictionary<string, string> strs1 = new Dictionary<string, string>()
			{
				{ "addFriend", "Key_1894" },
				{ "getGotcha", "Key_1890" },
				{ "breakSeries", "Key_1709" },
				{ "captureFlags", "Key_1704" },
				{ "capturePoints", "Key_1703" },
				{ "joinClan", "Key_1895" },
				{ "killFlagCarriers", "Key_1702" },
				{ "killInCampaign", "Key_1712" },
				{ "killInMode", "Key_1701" },
				{ "killNpcWithWeapon", "Key_1713" },
				{ "killViaHeadshot", "Key_1706" },
				{ "killWithGrenade", "Key_1707" },
				{ "killWithWeapon", "Key_1705" },
				{ "likeFacebook", "Key_1892" },
				{ "loginFacebook", "Key_1891" },
				{ "loginTwitter", "Key_1893" },
				{ "makeSeries", "Key_1710" },
				{ "revenge", "Key_1708" },
				{ "surviveWavesInArena", "Key_1711" },
				{ "winInMap", "Key_1700" },
				{ "winInMode", "Key_1699" }
			};
			QuestConstants.localizationQuests = strs1;
		}

		public static string GetAccumulativeQuestDescriptionByType(AccumulativeQuestBase quest)
		{
			string str;
			string str1;
			string str2;
			string str3;
			QuestConstants.localizationQuests.TryGetValue(quest.Id, out str);
			string str4 = str.Map<string, string>(new Func<string, string>(LocalizationStore.Get), "{0}");
			ModeAccumulativeQuest modeAccumulativeQuest = quest as ModeAccumulativeQuest;
			if (modeAccumulativeQuest != null)
			{
				if (!ConnectSceneNGUIController.gameModesLocalizeKey.TryGetValue(Convert.ToInt32(modeAccumulativeQuest.Mode).ToString(), out str1))
				{
					str1 = modeAccumulativeQuest.Mode.ToString();
					Debug.LogError(string.Concat("Couldnot find mode name for ", modeAccumulativeQuest.Mode));
				}
				return string.Format(str4, string.Format("[fff600]{0}[-]", modeAccumulativeQuest.RequiredCount), string.Format("[ff9600]{0}[-]", LocalizationStore.Get(str1)));
			}
			MapAccumulativeQuest mapAccumulativeQuest = quest as MapAccumulativeQuest;
			if (mapAccumulativeQuest != null)
			{
				SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(mapAccumulativeQuest.Map);
				string empty = string.Empty;
				if (infoScene != null)
				{
					empty = infoScene.TranslateName;
				}
				else
				{
					empty = mapAccumulativeQuest.Map;
					Debug.LogError(string.Concat("Couldnot find map name for ", mapAccumulativeQuest.Map));
				}
				return string.Format(str4, string.Format("[fff600]{0}[-]", mapAccumulativeQuest.RequiredCount), string.Format("[ff9600]{0}[-]", empty));
			}
			WeaponSlotAccumulativeQuest weaponSlotAccumulativeQuest = quest as WeaponSlotAccumulativeQuest;
			if (weaponSlotAccumulativeQuest == null)
			{
				try
				{
					str3 = string.Format(str4, string.Format("[fff600]{0}[-]", quest.RequiredCount));
				}
				catch (FormatException formatException)
				{
					str3 = str4;
				}
				return str3;
			}
			if (!ShopNGUIController.weaponCategoryLocKeys.TryGetValue(weaponSlotAccumulativeQuest.WeaponSlot.ToString(), out str2))
			{
				str2 = weaponSlotAccumulativeQuest.WeaponSlot.ToString().Replace("Category", string.Empty);
				Debug.LogError(string.Concat("Couldnot find weapon name for ", weaponSlotAccumulativeQuest.WeaponSlot));
			}
			return string.Format(str4, string.Format("[fff600]{0}[-]", weaponSlotAccumulativeQuest.RequiredCount), string.Format("[ff9600]{0}[-]", LocalizationStore.Get(str2)));
		}

		internal static string GetDifficultyKey(Difficulty difficulty)
		{
			return difficulty.ToString().ToLowerInvariant();
		}

		internal static string[] GetSupportedQuests()
		{
			return QuestConstants._supportedQuests.ToArray<string>();
		}

		internal static bool IsSupported(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			return QuestConstants._supportedQuests.Contains(id);
		}

		internal static ConnectSceneNGUIController.RegimGame? ParseMode(string mode)
		{
			ConnectSceneNGUIController.RegimGame? nullable;
			if (string.IsNullOrEmpty(mode))
			{
				return null;
			}
			try
			{
				ConnectSceneNGUIController.RegimGame regimGame = (ConnectSceneNGUIController.RegimGame)((int)Enum.Parse(typeof(ConnectSceneNGUIController.RegimGame), mode));
				nullable = new ConnectSceneNGUIController.RegimGame?(regimGame);
			}
			catch
			{
				nullable = null;
			}
			return nullable;
		}

		internal static ShopNGUIController.CategoryNames? ParseWeaponSlot(string weaponSlot)
		{
			ShopNGUIController.CategoryNames categoryName;
			ShopNGUIController.CategoryNames? nullable;
			if (string.IsNullOrEmpty(weaponSlot))
			{
				return null;
			}
			if (QuestConstants._weaponSlots.TryGetValue(weaponSlot, out categoryName))
			{
				return new ShopNGUIController.CategoryNames?(categoryName);
			}
			try
			{
				categoryName = (ShopNGUIController.CategoryNames)((int)Enum.Parse(typeof(ShopNGUIController.CategoryNames), weaponSlot));
				nullable = new ShopNGUIController.CategoryNames?(categoryName);
			}
			catch
			{
				nullable = null;
			}
			return nullable;
		}
	}
}