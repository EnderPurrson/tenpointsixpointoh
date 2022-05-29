using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class Statistics
	{
		private readonly Dictionary<string, int> _weaponToPopularity;

		private readonly List<Dictionary<string, int>> _weaponToPopularityForTier;

		private readonly Dictionary<string, int> _armorToPopularity;

		private readonly List<Dictionary<string, int>> _armorToPopularityForTier;

		private readonly Dictionary<int, Dictionary<string, int>> _armorToPopularityForLevel;

		private static Statistics _instance;

		public static Statistics Instance
		{
			get
			{
				if (Statistics._instance == null)
				{
					Statistics._instance = new Statistics();
				}
				return Statistics._instance;
			}
		}

		public Statistics()
		{
			this._weaponToPopularity = this.LoadPopularityFromPlayerPrefs("Statistics.WeaponPopularity");
			this._weaponToPopularityForTier = this.LoadPopularityForTierFromPlayerPrefs("Statistics.WeaponPopularityForTier");
			this._armorToPopularity = this.LoadPopularityFromPlayerPrefs("Statistics.ArmorPopularity");
			this._armorToPopularityForTier = this.LoadPopularityForTierFromPlayerPrefs("Statistics.ArmorPopularityForTier");
			this._armorToPopularityForLevel = this.LoadPopularityForLevelFromPlayerPrefs("Statistics.ArmorPopularityForLevel");
		}

		public string[] GetMostPopularArmors()
		{
			return this.GetMostPopularFrom(this._armorToPopularity);
		}

		public string[] GetMostPopularArmorsForLevel(int level)
		{
			Dictionary<string, int> strs;
			if (!this._armorToPopularityForLevel.TryGetValue(level, out strs))
			{
				return new string[0];
			}
			return this.GetMostPopularFrom(strs);
		}

		public string[] GetMostPopularArmorsForTier(int tier)
		{
			return this.GetMostPopularFrom(this._armorToPopularityForTier[tier]);
		}

		private string[] GetMostPopularFrom(Dictionary<string, int> popularityMap)
		{
			int value = 0;
			foreach (KeyValuePair<string, int> keyValuePair in popularityMap)
			{
				if (keyValuePair.Value <= value)
				{
					continue;
				}
				value = keyValuePair.Value;
			}
			if (value == 0)
			{
				return new string[0];
			}
			List<string> strs = new List<string>();
			foreach (KeyValuePair<string, int> keyValuePair1 in popularityMap)
			{
				if (keyValuePair1.Value != value)
				{
					continue;
				}
				strs.Add(keyValuePair1.Key);
			}
			return strs.ToArray();
		}

		public string[] GetMostPopularWeapons()
		{
			return this.GetMostPopularFrom(this._weaponToPopularity);
		}

		public string[] GetMostPopularWeaponsForTier(int tier)
		{
			return this.GetMostPopularFrom(this._weaponToPopularityForTier[tier]);
		}

		public void IncrementArmorPopularity(string key, bool save = true)
		{
			Dictionary<string, int> strs;
			this.IncrementPopularity(this._armorToPopularity, key);
			int ourTier = ExpController.Instance.OurTier;
			this.IncrementPopularity(this._armorToPopularityForTier[ourTier], key);
			int num = ExperienceController.sharedController.currentLevel;
			if (!this._armorToPopularityForLevel.TryGetValue(num, out strs))
			{
				strs = new Dictionary<string, int>();
				this._armorToPopularityForLevel.Add(num, strs);
			}
			this.IncrementPopularity(strs, key);
			if (save)
			{
				this.SaveArmorPopularity();
			}
		}

		private void IncrementPopularity(Dictionary<string, int> popularityDict, string key)
		{
			int num;
			if (!popularityDict.TryGetValue(key, out num))
			{
				popularityDict.Add(key, 1);
			}
			else
			{
				popularityDict[key] = num + 1;
			}
		}

		public void IncrementWeaponPopularity(string key, bool save = true)
		{
			this.IncrementPopularity(this._weaponToPopularity, key);
			int ourTier = ExpController.Instance.OurTier;
			this.IncrementPopularity(this._weaponToPopularityForTier[ourTier], key);
			if (save)
			{
				this.SaveWeaponPopularity();
			}
		}

		private Dictionary<int, Dictionary<string, int>> LoadPopularityForLevelFromPlayerPrefs(string playerPrefsKey)
		{
			Dictionary<string, int> strs;
			Dictionary<int, Dictionary<string, int>> nums = new Dictionary<int, Dictionary<string, int>>();
			string str = PlayerPrefs.GetString(playerPrefsKey, "{}");
			Dictionary<string, object> strs1 = Json.Deserialize(str) as Dictionary<string, object>;
			if (strs1 == null)
			{
				return nums;
			}
			foreach (KeyValuePair<string, object> keyValuePair in strs1)
			{
				int num = Convert.ToInt32(keyValuePair.Key);
				if (!nums.TryGetValue(num, out strs))
				{
					strs = new Dictionary<string, int>();
					nums.Add(num, strs);
				}
				foreach (KeyValuePair<string, object> value in keyValuePair.Value as Dictionary<string, object>)
				{
					strs.Add(value.Key, Convert.ToInt32(value.Value));
				}
			}
			return nums;
		}

		private List<Dictionary<string, int>> LoadPopularityForTierFromPlayerPrefs(string playerPrefsKey)
		{
			List<Dictionary<string, int>> dictionaries = new List<Dictionary<string, int>>();
			for (int i = 0; i < (int)ExpController.LevelsForTiers.Length; i++)
			{
				dictionaries.Add(new Dictionary<string, int>());
			}
			string str = PlayerPrefs.GetString(playerPrefsKey, "{}");
			List<object> objs = Json.Deserialize(str) as List<object>;
			if (objs == null)
			{
				return dictionaries;
			}
			for (int j = 0; j < (int)ExpController.LevelsForTiers.Length; j++)
			{
				if (j < objs.Count)
				{
					foreach (KeyValuePair<string, object> item in objs[j] as Dictionary<string, object>)
					{
						dictionaries[j].Add(item.Key, Convert.ToInt32(item.Value));
					}
				}
			}
			return dictionaries;
		}

		private Dictionary<string, int> LoadPopularityFromPlayerPrefs(string playerPrefsKey)
		{
			Dictionary<string, int> strs = new Dictionary<string, int>();
			string str = PlayerPrefs.GetString(playerPrefsKey, "{}");
			Dictionary<string, object> strs1 = Json.Deserialize(str) as Dictionary<string, object>;
			if (strs1 == null)
			{
				return strs;
			}
			foreach (KeyValuePair<string, object> keyValuePair in strs1)
			{
				strs.Add(keyValuePair.Key, Convert.ToInt32(keyValuePair.Value));
			}
			return strs;
		}

		public void SaveArmorPopularity()
		{
			this.SavePopularityInfo("Statistics.ArmorPopularity", this._armorToPopularity);
			this.SavePopularityInfo("Statistics.ArmorPopularityForLevel", this._armorToPopularityForLevel);
			this.SavePopularityInfo("Statistics.ArmorPopularityForTier", this._armorToPopularityForTier);
			PlayerPrefs.Save();
		}

		private void SavePopularityInfo(string playerPrefsKey, object popularityInfo)
		{
			string str = Json.Serialize(popularityInfo);
			if (Debug.isDebugBuild)
			{
				Debug.Log(string.Format("Saving: playerPrefsKey: {0}, popularityInfo: {1}", playerPrefsKey, str));
			}
			PlayerPrefs.SetString(playerPrefsKey, str);
		}

		public void SaveWeaponPopularity()
		{
			this.SavePopularityInfo("Statistics.WeaponPopularity", this._weaponToPopularity);
			this.SavePopularityInfo("Statistics.WeaponPopularityForTier", this._weaponToPopularityForTier);
			PlayerPrefs.Save();
		}
	}
}