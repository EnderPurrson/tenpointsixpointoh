using System;
using System.Text;
using UnityEngine;

namespace Prime31
{
	public static class Utils
	{
		private static Random _random;

		private static Random random
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Expected O, but got Unknown
				if (_random == null)
				{
					_random = new Random();
				}
				return _random;
			}
		}

		public static string randomString(int size = 38)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			StringBuilder val = new StringBuilder();
			for (int i = 0; i < size; i++)
			{
				char c = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * random.NextDouble() + 65.0)));
				val.Append(c);
			}
			return ((object)val).ToString();
		}

		public static void logObject(object obj)
		{
			string json = Json.encode(obj);
			prettyPrintJson(json);
		}

		public static void prettyPrintJson(string json)
		{
			string text = string.Empty;
			if (json != null)
			{
				text = JsonFormatter.prettyPrint(json);
			}
			try
			{
				Debug.Log(text);
			}
			catch (global::System.Exception)
			{
				Console.WriteLine(text);
			}
		}
	}
}
