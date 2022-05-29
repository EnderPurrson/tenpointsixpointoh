using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KillRateStatisticsManager
{
	public const string WeKillKey = "wekill";

	public const string WeWereKilledKey = "wewerekilled";

	private static Dictionary<string, Dictionary<int, int>> weKillOld;

	private static Dictionary<string, Dictionary<int, int>> weWereKilledOld;

	private static bool _initialized;

	public static Dictionary<string, Dictionary<int, int>> WeKillOld
	{
		get
		{
			if (!KillRateStatisticsManager._initialized)
			{
				KillRateStatisticsManager.Initialize();
			}
			return KillRateStatisticsManager.weKillOld;
		}
	}

	public static Dictionary<string, Dictionary<int, int>> WeWereKilledOld
	{
		get
		{
			if (!KillRateStatisticsManager._initialized)
			{
				KillRateStatisticsManager.Initialize();
			}
			return KillRateStatisticsManager.weWereKilledOld;
		}
	}

	static KillRateStatisticsManager()
	{
		KillRateStatisticsManager.weKillOld = new Dictionary<string, Dictionary<int, int>>();
		KillRateStatisticsManager.weWereKilledOld = new Dictionary<string, Dictionary<int, int>>();
		KillRateStatisticsManager._initialized = false;
	}

	public KillRateStatisticsManager()
	{
	}

	private static void Initialize()
	{
		KillRateStatisticsManager.ParseKillRate(ref KillRateStatisticsManager.weKillOld, ref KillRateStatisticsManager.weWereKilledOld);
		KillRateStatisticsManager._initialized = true;
	}

	private static void InitializeKillRateKey()
	{
		if (!Storager.hasKey("KillRateKeyStatistics"))
		{
			KillRateStatisticsManager.WriteDefaultJson();
		}
	}

	private static void ParseKillRate(ref Dictionary<string, Dictionary<int, int>> returnWeKill, ref Dictionary<string, Dictionary<int, int>> returnWeWereKilled)
	{
		KillRateStatisticsManager.InitializeKillRateKey();
		Dictionary<string, object> strs = Json.Deserialize(Storager.getString("KillRateKeyStatistics", false)) as Dictionary<string, object>;
		if (!strs.ContainsKey("version"))
		{
			Debug.LogError("ParseKillRate: no version key. Please clear your PlayerPrefs");
			KillRateStatisticsManager.WriteDefaultJson();
			strs = Json.Deserialize(Storager.getString("KillRateKeyStatistics", false)) as Dictionary<string, object>;
		}
		else if (!((string)strs["version"]).Equals(GlobalGameController.AppVersion))
		{
			KillRateStatisticsManager.WriteDefaultJson();
			strs = Json.Deserialize(Storager.getString("KillRateKeyStatistics", false)) as Dictionary<string, object>;
		}
		Dictionary<string, object> strs1 = (!strs.ContainsKey("wekill") ? new Dictionary<string, object>() : strs["wekill"] as Dictionary<string, object>);
		Dictionary<string, object> strs2 = (!strs.ContainsKey("wewerekilled") ? new Dictionary<string, object>() : strs["wewerekilled"] as Dictionary<string, object>);
		Action<Dictionary<string, object>, Dictionary<string, Dictionary<int, int>>> action = (Dictionary<string, object> savedDict, Dictionary<string, Dictionary<int, int>> dict) => {
			foreach (KeyValuePair<string, object> keyValuePair in savedDict)
			{
				Dictionary<string, object> value = keyValuePair.Value as Dictionary<string, object>;
				Dictionary<int, int> nums = new Dictionary<int, int>();
				foreach (KeyValuePair<string, object> keyValuePair1 in value)
				{
					nums.Add(int.Parse(keyValuePair1.Key), (int)((long)keyValuePair1.Value));
				}
				dict.Add(keyValuePair.Key, nums);
			}
		};
		action(strs1, returnWeKill);
		action(strs2, returnWeWereKilled);
	}

	private static void WriteDefaultJson()
	{
		Dictionary<string, object> strs = new Dictionary<string, object>()
		{
			{ "version", GlobalGameController.AppVersion }
		};
		Storager.setString("KillRateKeyStatistics", Json.Serialize(strs), false);
	}
}