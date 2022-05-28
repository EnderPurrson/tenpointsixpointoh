using System;
using System.Collections.Generic;

namespace DevToDev.Core.Utils.Extensions
{
	internal static class DictionaryExtensions
	{
		public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (collection == null)
			{
				throw new ArgumentNullException("Collection is null");
			}
			Enumerator<T, S> enumerator = collection.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<T, S> current = enumerator.get_Current();
					if (source.ContainsKey(current.get_Key()))
					{
						source.Remove(current.get_Key());
					}
					source.Add(current.get_Key(), current.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public static void AddWithReplace<T, S>(this Dictionary<T, S> source, T key, S value)
		{
			if (source.ContainsKey(key))
			{
				source.Remove(key);
			}
			source.Add(key, value);
		}

		public static void AddWithoutReplace<T, S>(this Dictionary<T, S> source, T key, S value)
		{
			if (!source.ContainsKey(key))
			{
				source.Add(key, value);
			}
		}
	}
}
