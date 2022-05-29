using EveryplayMiniJSON;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class EveryplayDictionaryExtensions
{
	public static Dictionary<string, object> JsonToDictionary(string json)
	{
		if (json == null || json.Length <= 0)
		{
			return null;
		}
		return Json.Deserialize(json) as Dictionary<string, object>;
	}

	public static bool TryGetValue<T>(this Dictionary<string, object> dict, string key, out T value)
	{
		bool flag;
		T t;
		if (dict != null && dict.ContainsKey(key))
		{
			if (dict[key].GetType() == typeof(T))
			{
				value = (T)dict[key];
				return true;
			}
			try
			{
				value = (T)Convert.ChangeType(dict[key], typeof(T));
				flag = true;
			}
			catch
			{
				t = default(T);
				value = t;
				return false;
			}
			return flag;
		}
		t = default(T);
		value = t;
		return false;
	}
}