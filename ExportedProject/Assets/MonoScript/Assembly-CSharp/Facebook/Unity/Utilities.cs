using Facebook.MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal static class Utilities
	{
		private const string WarningMissingParameter = "Did not find expected value '{0}' in dictionary";

		public static string AbsoluteUrlOrEmptyString(this Uri uri)
		{
			if (uri == null)
			{
				return string.Empty;
			}
			return uri.AbsoluteUri;
		}

		public static void AddAllKVPFrom<T1, T2>(this IDictionary<T1, T2> dest, IDictionary<T1, T2> source)
		{
			IEnumerator<T1> enumerator = source.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					T1 current = enumerator.Current;
					dest[current] = source[current];
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

		private static DateTime FromTimestamp(int timestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return dateTime.AddSeconds((double)timestamp);
		}

		public static string GetUserAgent(string productName, string productVersion)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[] { productName, productVersion });
		}

		public static T GetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key, bool logWarning = true)
		{
			T t;
			if (!dictionary.TryGetValue<T>(key, out t))
			{
				FacebookLogger.Warn("Did not find expected value '{0}' in dictionary", new string[] { key });
			}
			return t;
		}

		public static AccessToken ParseAccessTokenFromResult(IDictionary<string, object> resultDictionary)
		{
			string valueOrDefault = resultDictionary.GetValueOrDefault<string>(LoginResult.UserIdKey, true);
			string str = resultDictionary.GetValueOrDefault<string>(LoginResult.AccessTokenKey, true);
			DateTime dateTime = Utilities.ParseExpirationDateFromResult(resultDictionary);
			ICollection<string> strs = Utilities.ParsePermissionFromResult(resultDictionary);
			DateTime? nullable = Utilities.ParseLastRefreshFromResult(resultDictionary);
			return new AccessToken(str, valueOrDefault, dateTime, strs, nullable);
		}

		private static DateTime ParseExpirationDateFromResult(IDictionary<string, object> resultDictionary)
		{
			DateTime dateTime;
			int num;
			if (!Constants.IsWeb)
			{
				dateTime = (!int.TryParse(resultDictionary.GetValueOrDefault<string>(LoginResult.ExpirationTimestampKey, true), out num) || num <= 0 ? DateTime.MaxValue : Utilities.FromTimestamp(num));
			}
			else
			{
				DateTime now = DateTime.Now;
				dateTime = now.AddSeconds((double)resultDictionary.GetValueOrDefault<long>(LoginResult.ExpirationTimestampKey, true));
			}
			return dateTime;
		}

		private static DateTime? ParseLastRefreshFromResult(IDictionary<string, object> resultDictionary)
		{
			int num;
			if (int.TryParse(resultDictionary.GetValueOrDefault<string>(LoginResult.ExpirationTimestampKey, true), out num) && num > 0)
			{
				return new DateTime?(Utilities.FromTimestamp(num));
			}
			return null;
		}

		private static ICollection<string> ParsePermissionFromResult(IDictionary<string, object> resultDictionary)
		{
			string str;
			IEnumerable<object> objs;
			if (resultDictionary.TryGetValue<string>(LoginResult.PermissionsKey, out str))
			{
				objs = str.Split(new char[] { ',' });
			}
			else if (!resultDictionary.TryGetValue<IEnumerable<object>>(LoginResult.PermissionsKey, out objs))
			{
				objs = new string[0];
				FacebookLogger.Warn("Failed to find parameter '{0}' in login result", new string[] { LoginResult.PermissionsKey });
			}
			return (
				from permission in objs
				select permission.ToString()).ToList<string>();
		}

		public static string ToCommaSeparateList(this IEnumerable<string> list)
		{
			if (list == null)
			{
				return string.Empty;
			}
			return string.Join(",", list.ToArray<string>());
		}

		public static string ToJson(this IDictionary<string, object> dictionary)
		{
			return Json.Serialize(dictionary);
		}

		public static long TotalSeconds(this DateTime dateTime)
		{
			TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1);
			return (long)timeSpan.TotalSeconds;
		}

		public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
		{
			object obj;
			if (dictionary.TryGetValue(key, out obj) && obj is T)
			{
				value = (T)obj;
				return true;
			}
			value = default(T);
			return false;
		}
	}
}