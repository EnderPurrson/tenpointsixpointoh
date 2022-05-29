using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public abstract class Jsonable
	{
		protected Jsonable()
		{
		}

		public static void CheckForErrors(Dictionary<string, object> jsonMap)
		{
			object obj;
			if (jsonMap.TryGetValue("error", out obj))
			{
				throw new AmazonException(obj as string);
			}
		}

		public abstract Dictionary<string, object> GetObjectDictionary();

		public static List<object> unrollObjectIntoList<T>(List<T> obj)
		where T : Jsonable
		{
			List<object> objs = new List<object>();
			foreach (T t in obj)
			{
				objs.Add(t.GetObjectDictionary());
			}
			return objs;
		}

		public static Dictionary<string, object> unrollObjectIntoMap<T>(Dictionary<string, T> obj)
		where T : Jsonable
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			foreach (KeyValuePair<string, T> keyValuePair in obj)
			{
				strs.Add(keyValuePair.Key, keyValuePair.Value.GetObjectDictionary());
			}
			return strs;
		}
	}
}