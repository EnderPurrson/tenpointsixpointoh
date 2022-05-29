using System;
using System.Collections.Generic;
using UnityEngine;

public static class Localization
{
	public static Localization.LoadFunction loadFunction;

	public static Localization.OnLocalizeNotification onLocalize;

	public static bool localizationHasBeenSet;

	private static string[] mLanguages;

	private static Dictionary<string, string> mOldDictionary;

	private static Dictionary<string, string[]> mDictionary;

	private static Dictionary<string, string> mReplacement;

	private static int mLanguageIndex;

	private static string mLanguage;

	private static bool mMerging;

	public static Dictionary<string, string[]> dictionary
	{
		get
		{
			if (!Localization.localizationHasBeenSet)
			{
				Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
			}
			return Localization.mDictionary;
		}
		set
		{
			Localization.localizationHasBeenSet = value != null;
			Localization.mDictionary = value;
		}
	}

	[Obsolete("Localization is now always active. You no longer need to check this property.")]
	public static bool isActive
	{
		get
		{
			return true;
		}
	}

	public static string[] knownLanguages
	{
		get
		{
			if (!Localization.localizationHasBeenSet)
			{
				Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
			}
			return Localization.mLanguages;
		}
	}

	public static string language
	{
		get
		{
			if (string.IsNullOrEmpty(Localization.mLanguage))
			{
				Localization.mLanguage = PlayerPrefs.GetString("Language", "English");
				Localization.LoadAndSelect(Localization.mLanguage);
			}
			return Localization.mLanguage;
		}
		set
		{
			if (Localization.mLanguage != value)
			{
				Localization.mLanguage = value;
				Localization.LoadAndSelect(value);
			}
		}
	}

	static Localization()
	{
		Localization.localizationHasBeenSet = false;
		Localization.mLanguages = null;
		Localization.mOldDictionary = new Dictionary<string, string>();
		Localization.mDictionary = new Dictionary<string, string[]>();
		Localization.mReplacement = new Dictionary<string, string>();
		Localization.mLanguageIndex = -1;
		Localization.mMerging = false;
	}

	private static void AddCSV(BetterList<string> newValues, string[] newLanguages, Dictionary<string, int> languageIndices)
	{
		if (newValues.size < 2)
		{
			return;
		}
		string item = newValues[0];
		if (string.IsNullOrEmpty(item))
		{
			return;
		}
		string[] strArrays = Localization.ExtractStrings(newValues, newLanguages, languageIndices);
		if (!Localization.mDictionary.ContainsKey(item))
		{
			try
			{
				Localization.mDictionary.Add(item, strArrays);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Debug.LogError(string.Concat("Unable to add '", item, "' to the Localization dictionary.\n", exception.Message));
			}
		}
		else
		{
			Localization.mDictionary[item] = strArrays;
			if (newLanguages == null)
			{
				Debug.LogWarning(string.Concat("Localization key '", item, "' is already present"));
			}
		}
	}

	public static void ClearReplacements()
	{
		Localization.mReplacement.Clear();
	}

	public static bool Exists(string key)
	{
		if (!Localization.localizationHasBeenSet)
		{
			Localization.language = PlayerPrefs.GetString("Language", "English");
		}
		string str = string.Concat(key, " Mobile");
		if (Localization.mDictionary.ContainsKey(str))
		{
			return true;
		}
		if (Localization.mOldDictionary.ContainsKey(str))
		{
			return true;
		}
		return (Localization.mDictionary.ContainsKey(key) ? true : Localization.mOldDictionary.ContainsKey(key));
	}

	private static string[] ExtractStrings(BetterList<string> added, string[] newLanguages, Dictionary<string, int> languageIndices)
	{
		string[] item;
		if (newLanguages == null)
		{
			string[] strArrays = new string[(int)Localization.mLanguages.Length];
			int num = 1;
			int num1 = Mathf.Min(added.size, (int)strArrays.Length + 1);
			while (num < num1)
			{
				strArrays[num - 1] = added[num];
				num++;
			}
			return strArrays;
		}
		string str = added[0];
		if (!Localization.mDictionary.TryGetValue(str, out item))
		{
			item = new string[(int)Localization.mLanguages.Length];
		}
		int num2 = 0;
		int length = (int)newLanguages.Length;
		while (num2 < length)
		{
			int item1 = languageIndices[newLanguages[num2]];
			item[item1] = added[num2 + 1];
			num2++;
		}
		return item;
	}

	public static string Format(string key, params object[] parameters)
	{
		return string.Format(Localization.Get(key), parameters);
	}

	public static string Get(string key)
	{
		string str;
		string[] strArrays;
		if (!Localization.localizationHasBeenSet)
		{
			Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
		}
		if (Localization.mLanguages == null)
		{
			Debug.LogError("No localization data present");
			return null;
		}
		string str1 = Localization.language;
		if (Localization.mLanguageIndex == -1)
		{
			int num = 0;
			while (num < (int)Localization.mLanguages.Length)
			{
				if (Localization.mLanguages[num] != str1)
				{
					num++;
				}
				else
				{
					Localization.mLanguageIndex = num;
					break;
				}
			}
		}
		if (Localization.mLanguageIndex == -1)
		{
			Localization.mLanguageIndex = 0;
			Localization.mLanguage = Localization.mLanguages[0];
			Debug.LogWarning(string.Concat("Language not found: ", str1));
		}
		UICamera.ControlScheme controlScheme = UICamera.currentScheme;
		if (controlScheme == UICamera.ControlScheme.Touch)
		{
			string str2 = string.Concat(key, " Mobile");
			if (Localization.mReplacement.TryGetValue(str2, out str))
			{
				return str;
			}
			if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(str2, out strArrays) && Localization.mLanguageIndex < (int)strArrays.Length)
			{
				return strArrays[Localization.mLanguageIndex];
			}
			if (Localization.mOldDictionary.TryGetValue(str2, out str))
			{
				return str;
			}
		}
		else if (controlScheme == UICamera.ControlScheme.Controller)
		{
			string str3 = string.Concat(key, " Controller");
			if (Localization.mReplacement.TryGetValue(str3, out str))
			{
				return str;
			}
			if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(str3, out strArrays) && Localization.mLanguageIndex < (int)strArrays.Length)
			{
				return strArrays[Localization.mLanguageIndex];
			}
			if (Localization.mOldDictionary.TryGetValue(str3, out str))
			{
				return str;
			}
		}
		if (Localization.mReplacement.TryGetValue(key, out str))
		{
			return str;
		}
		if (Localization.mLanguageIndex == -1 || !Localization.mDictionary.TryGetValue(key, out strArrays))
		{
			if (Localization.mOldDictionary.TryGetValue(key, out str))
			{
				return str;
			}
			return key;
		}
		if (Localization.mLanguageIndex >= (int)strArrays.Length)
		{
			return strArrays[0];
		}
		string str4 = strArrays[Localization.mLanguageIndex];
		if (string.IsNullOrEmpty(str4))
		{
			str4 = strArrays[0];
		}
		return str4;
	}

	private static bool HasLanguage(string languageName)
	{
		int num = 0;
		int length = (int)Localization.mLanguages.Length;
		while (num < length)
		{
			if (Localization.mLanguages[num] == languageName)
			{
				return true;
			}
			num++;
		}
		return false;
	}

	public static void Load(TextAsset asset)
	{
		ByteReader byteReader = new ByteReader(asset);
		Localization.Set(asset.name, byteReader.ReadDictionary());
	}

	private static bool LoadAndSelect(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (Localization.mDictionary.Count == 0 && !Localization.LoadDictionary(value))
			{
				return false;
			}
			if (Localization.SelectLanguage(value))
			{
				return true;
			}
		}
		if (Localization.mOldDictionary.Count > 0)
		{
			return true;
		}
		Localization.mOldDictionary.Clear();
		Localization.mDictionary.Clear();
		if (string.IsNullOrEmpty(value))
		{
			PlayerPrefs.DeleteKey("Language");
		}
		return false;
	}

	public static bool LoadCSV(TextAsset asset, bool merge = false)
	{
		return Localization.LoadCSV(asset.bytes, asset, merge);
	}

	public static bool LoadCSV(byte[] bytes, bool merge = false)
	{
		return Localization.LoadCSV(bytes, null, merge);
	}

	private static bool LoadCSV(byte[] bytes, TextAsset asset, bool merge = false)
	{
		if (bytes == null)
		{
			return false;
		}
		ByteReader byteReader = new ByteReader(bytes);
		BetterList<string> betterList = byteReader.ReadCSV();
		if (betterList.size < 2)
		{
			return false;
		}
		betterList.RemoveAt(0);
		string[] item = null;
		if (string.IsNullOrEmpty(Localization.mLanguage))
		{
			Localization.localizationHasBeenSet = false;
		}
		if (!Localization.localizationHasBeenSet || !merge && !Localization.mMerging || Localization.mLanguages == null || (int)Localization.mLanguages.Length == 0)
		{
			Localization.mDictionary.Clear();
			Localization.mLanguages = new string[betterList.size];
			if (!Localization.localizationHasBeenSet)
			{
				Localization.mLanguage = PlayerPrefs.GetString("Language", betterList[0]);
				Localization.localizationHasBeenSet = true;
			}
			for (int i = 0; i < betterList.size; i++)
			{
				Localization.mLanguages[i] = betterList[i];
				if (Localization.mLanguages[i] == Localization.mLanguage)
				{
					Localization.mLanguageIndex = i;
				}
			}
		}
		else
		{
			item = new string[betterList.size];
			for (int j = 0; j < betterList.size; j++)
			{
				item[j] = betterList[j];
			}
			for (int k = 0; k < betterList.size; k++)
			{
				if (!Localization.HasLanguage(betterList[k]))
				{
					int length = (int)Localization.mLanguages.Length + 1;
					Array.Resize<string>(ref Localization.mLanguages, length);
					Localization.mLanguages[length - 1] = betterList[k];
					Dictionary<string, string[]> strs = new Dictionary<string, string[]>();
					foreach (KeyValuePair<string, string[]> keyValuePair in Localization.mDictionary)
					{
						string[] value = keyValuePair.Value;
						Array.Resize<string>(ref value, length);
						value[length - 1] = value[0];
						strs.Add(keyValuePair.Key, value);
					}
					Localization.mDictionary = strs;
				}
			}
		}
		Dictionary<string, int> strs1 = new Dictionary<string, int>();
		for (int l = 0; l < (int)Localization.mLanguages.Length; l++)
		{
			strs1.Add(Localization.mLanguages[l], l);
		}
		while (true)
		{
			BetterList<string> betterList1 = byteReader.ReadCSV();
			if (betterList1 == null || betterList1.size == 0)
			{
				break;
			}
			if (!string.IsNullOrEmpty(betterList1[0]))
			{
				Localization.AddCSV(betterList1, item, strs1);
			}
		}
		if (!Localization.mMerging && Localization.onLocalize != null)
		{
			Localization.mMerging = true;
			Localization.OnLocalizeNotification onLocalizeNotification = Localization.onLocalize;
			Localization.onLocalize = null;
			onLocalizeNotification();
			Localization.onLocalize = onLocalizeNotification;
			Localization.mMerging = false;
		}
		return true;
	}

	private static bool LoadDictionary(string value)
	{
		byte[] numArray = null;
		if (!Localization.localizationHasBeenSet)
		{
			if (Localization.loadFunction != null)
			{
				numArray = Localization.loadFunction("Localization");
			}
			else
			{
				TextAsset textAsset = Resources.Load<TextAsset>("Localization");
				if (textAsset != null)
				{
					numArray = textAsset.bytes;
				}
			}
			Localization.localizationHasBeenSet = true;
		}
		if (Localization.LoadCSV(numArray, false))
		{
			return true;
		}
		if (string.IsNullOrEmpty(value))
		{
			value = Localization.mLanguage;
		}
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		if (Localization.loadFunction != null)
		{
			numArray = Localization.loadFunction(value);
		}
		else
		{
			TextAsset textAsset1 = Resources.Load<TextAsset>(value);
			if (textAsset1 != null)
			{
				numArray = textAsset1.bytes;
			}
		}
		if (numArray == null)
		{
			return false;
		}
		Localization.Set(value, numArray);
		return true;
	}

	[Obsolete("Use Localization.Get instead")]
	public static string Localize(string key)
	{
		return Localization.Get(key);
	}

	public static void ReplaceKey(string key, string val)
	{
		if (string.IsNullOrEmpty(val))
		{
			Localization.mReplacement.Remove(key);
		}
		else
		{
			Localization.mReplacement[key] = val;
		}
	}

	private static bool SelectLanguage(string language)
	{
		Localization.mLanguageIndex = -1;
		if (Localization.mDictionary.Count == 0)
		{
			return false;
		}
		int num = 0;
		int length = (int)Localization.mLanguages.Length;
		while (num < length)
		{
			if (Localization.mLanguages[num] == language)
			{
				Localization.mOldDictionary.Clear();
				Localization.mLanguageIndex = num;
				Localization.mLanguage = language;
				PlayerPrefs.SetString("Language", Localization.mLanguage);
				if (Localization.onLocalize != null)
				{
					Localization.onLocalize();
				}
				UIRoot.Broadcast("OnLocalize");
				return true;
			}
			num++;
		}
		return false;
	}

	public static void Set(string languageName, byte[] bytes)
	{
		Localization.Set(languageName, (new ByteReader(bytes)).ReadDictionary());
	}

	public static void Set(string languageName, Dictionary<string, string> dictionary)
	{
		Localization.mLanguage = languageName;
		PlayerPrefs.SetString("Language", Localization.mLanguage);
		Localization.mOldDictionary = dictionary;
		Localization.localizationHasBeenSet = true;
		Localization.mLanguageIndex = -1;
		Localization.mLanguages = new string[] { languageName };
		if (Localization.onLocalize != null)
		{
			Localization.onLocalize();
		}
		UIRoot.Broadcast("OnLocalize");
	}

	public static void Set(string key, string value)
	{
		if (!Localization.mOldDictionary.ContainsKey(key))
		{
			Localization.mOldDictionary.Add(key, value);
		}
		else
		{
			Localization.mOldDictionary[key] = value;
		}
	}

	public delegate byte[] LoadFunction(string path);

	public delegate void OnLocalizeNotification();
}