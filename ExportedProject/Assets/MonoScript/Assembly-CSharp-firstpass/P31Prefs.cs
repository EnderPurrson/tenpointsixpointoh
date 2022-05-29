using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P31Prefs
{
	private static bool _iCloudDocumentStoreAvailable;

	public static bool iCloudDocumentStoreAvailable
	{
		get
		{
			return P31Prefs._iCloudDocumentStoreAvailable;
		}
	}

	public P31Prefs()
	{
	}

	public static List<object> allKeys()
	{
		return new List<object>();
	}

	public static bool getBool(string key)
	{
		return PlayerPrefs.GetInt(key, 0) == 1;
	}

	public static IDictionary getDictionary(string key)
	{
		return PlayerPrefs.GetString(key).dictionaryFromJson();
	}

	public static float getFloat(string key)
	{
		return PlayerPrefs.GetFloat(key);
	}

	public static int getInt(string key)
	{
		return PlayerPrefs.GetInt(key);
	}

	public static string getString(string key)
	{
		return PlayerPrefs.GetString(key);
	}

	public static bool hasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void removeAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public static void removeObjectForKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	public static void setBool(string key, bool val)
	{
		PlayerPrefs.SetInt(key, (!val ? 0 : 1));
	}

	public static void setDictionary(string key, Hashtable val)
	{
		PlayerPrefs.SetString(key, Json.encode(val));
	}

	public static void setFloat(string key, float val)
	{
		PlayerPrefs.SetFloat(key, val);
	}

	public static void setInt(string key, int val)
	{
		PlayerPrefs.SetInt(key, val);
	}

	public static void setString(string key, string val)
	{
		PlayerPrefs.SetString(key, val);
	}

	public static bool synchronize()
	{
		PlayerPrefs.Save();
		return true;
	}
}