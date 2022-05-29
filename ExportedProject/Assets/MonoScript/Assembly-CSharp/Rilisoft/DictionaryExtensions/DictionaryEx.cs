using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rilisoft.DictionaryExtensions
{
	internal static class DictionaryEx
	{
		internal static object TryGet(this Dictionary<string, object> dictionary, string key)
		{
			if (dictionary == null || key == null)
			{
				return null;
			}
			object obj = null;
			dictionary.TryGetValue(key, out obj);
			return obj;
		}
	}
}