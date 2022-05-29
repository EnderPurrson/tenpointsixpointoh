using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CampaignProgress
{
	public const string CampaignProgressKey = "CampaignProgress";

	public static Dictionary<string, Dictionary<string, int>> boxesLevelsAndStars;

	static CampaignProgress()
	{
		CampaignProgress.boxesLevelsAndStars = new Dictionary<string, Dictionary<string, int>>();
		Storager.hasKey("CampaignProgress");
		CampaignProgress.LoadCampaignProgress();
		if (CampaignProgress.boxesLevelsAndStars.Keys.Count == 0)
		{
			CampaignProgress.boxesLevelsAndStars.Add(LevelBox.campaignBoxes[0].name, new Dictionary<string, int>());
			CampaignProgress.SaveCampaignProgress();
		}
	}

	public CampaignProgress()
	{
	}

	public static void ActualizeComicsViews()
	{
		try
		{
			if (CampaignProgress.boxesLevelsAndStars != null)
			{
				IEnumerable<string> strs = CampaignProgress.boxesLevelsAndStars.SelectMany<KeyValuePair<string, Dictionary<string, int>>, string>((KeyValuePair<string, Dictionary<string, int>> boxKvp) => boxKvp.Value.Keys).Concat<string>(Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0]).Distinct<string>();
				Save.SaveStringArray(Defs.ArtLevsS, strs.ToArray<string>());
				IEnumerable<string> strs1 = RiliExtensions.WithoutLast<string>(
					from boxName in CampaignProgress.boxesLevelsAndStars.Keys
					orderby LevelBox.campaignBoxes.FindIndex((LevelBox levelBox) => levelBox.name == boxName)
					select boxName).Concat<string>(Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0]).Distinct<string>();
				Save.SaveStringArray(Defs.ArtBoxS, strs1.ToArray<string>());
			}
			else
			{
				Debug.LogError("ActualizeComicsViews: boxesLevelsAndStars = null");
			}
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("ActualizeComicsViews: exception: ", exception));
		}
	}

	internal static Dictionary<string, Dictionary<string, int>> DeserializeProgress(string serializedProgress)
	{
		Dictionary<string, Dictionary<string, int>> strs = new Dictionary<string, Dictionary<string, int>>();
		object obj = Json.Deserialize(serializedProgress);
		if (Debug.isDebugBuild && obj != null && BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			Debug.Log(string.Concat("Deserialized progress type: ", obj.GetType()));
			Debug.Log(string.Concat("##### Serialized campaign progress:\n", serializedProgress));
		}
		Dictionary<string, object> strs1 = obj as Dictionary<string, object>;
		if (strs1 != null)
		{
			foreach (KeyValuePair<string, object> keyValuePair in strs1)
			{
				Dictionary<string, int> strs2 = new Dictionary<string, int>();
				IDictionary<string, object> value = keyValuePair.Value as IDictionary<string, object>;
				if (value != null)
				{
					IEnumerator<KeyValuePair<string, object>> enumerator = value.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, object> current = enumerator.Current;
							try
							{
								int num = Convert.ToInt32(current.Value);
								strs2.Add(current.Key, num);
							}
							catch (InvalidCastException invalidCastException)
							{
								Debug.LogWarning(string.Concat("Cannot convert ", current.Value, " to int."));
							}
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
				else if (Debug.isDebugBuild)
				{
					Debug.LogWarning("boxProgressDictionary == null");
				}
				strs.Add(keyValuePair.Key, strs2);
			}
		}
		else if (Debug.isDebugBuild)
		{
			Debug.LogWarning(string.Concat("campaignProgressDictionary == null,    serializedProgress == ", serializedProgress));
		}
		return strs;
	}

	internal static Dictionary<string, Dictionary<string, int>> DeserializeTestDictionary()
	{
		return CampaignProgress.DeserializeProgress("{\"Box_11\": { \"Level_02\": 1, \"Level_05\": 3 },\"Box_13\": { \"Level_03\": 1, \"Level_08\": 3, \"Level_21\": 2 },\"Box_34\": { },\"Box_99\": { \"Level_55\": 2 },}");
	}

	public static bool FirstTimeCompletesLevel(string lev)
	{
		return !CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName].ContainsKey(lev);
	}

	public static string GetCampaignProgressString()
	{
		return Storager.getString("CampaignProgress", false);
	}

	private static Dictionary<string, Dictionary<string, int>> GetProgressDictionary(string key, bool useCloud)
	{
		return CampaignProgress.DeserializeProgress(Storager.getString(key, useCloud));
	}

	public static void LoadCampaignProgress()
	{
		CampaignProgress.boxesLevelsAndStars = CampaignProgress.GetProgressDictionary("CampaignProgress", false);
	}

	public static void OpenNewBoxIfPossible()
	{
		int num = 0;
		for (int i = 1; i < LevelBox.campaignBoxes.Count; i++)
		{
			LevelBox item = LevelBox.campaignBoxes[i];
			if (CampaignProgress.boxesLevelsAndStars.ContainsKey(item.name))
			{
				num = i;
			}
		}
		int value = 0;
		foreach (KeyValuePair<string, Dictionary<string, int>> boxesLevelsAndStar in CampaignProgress.boxesLevelsAndStars)
		{
			foreach (KeyValuePair<string, int> keyValuePair in boxesLevelsAndStar.Value)
			{
				value += keyValuePair.Value;
			}
		}
		int num1 = num + 1;
		if (num1 < LevelBox.campaignBoxes.Count)
		{
			string str = LevelBox.campaignBoxes[num1].name;
			if (LevelBox.campaignBoxes[num1].starsToOpen <= value && !CampaignProgress.boxesLevelsAndStars.ContainsKey(str))
			{
				CampaignProgress.boxesLevelsAndStars.Add(str, new Dictionary<string, int>());
				CampaignProgress.SaveCampaignProgress();
				FlurryPluginWrapper.LogBoxOpened(str);
			}
		}
		CampaignProgress.SaveCampaignProgress();
	}

	public static void SaveCampaignProgress()
	{
		CampaignProgress.SetProgressDictionary("CampaignProgress", CampaignProgress.boxesLevelsAndStars, false);
	}

	internal static string SerializeProgress(Dictionary<string, Dictionary<string, int>> progress)
	{
		string str;
		try
		{
			str = Json.Serialize(progress);
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			Debug.LogError(exception);
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Message", exception.Message }
			};
			FlurryPluginWrapper.LogEvent(exception.GetType().Name);
			str = "{ }";
		}
		return str;
	}

	private static void SetProgressDictionary(string key, Dictionary<string, Dictionary<string, int>> dictionary, bool useCloud)
	{
		Storager.setString(key, CampaignProgress.SerializeProgress(dictionary), useCloud);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}
}