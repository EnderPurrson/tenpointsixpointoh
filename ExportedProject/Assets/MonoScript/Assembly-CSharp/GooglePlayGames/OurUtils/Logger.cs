using System;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	public class Logger
	{
		private static bool debugLogEnabled;

		private static bool warningLogEnabled;

		public static bool DebugLogEnabled
		{
			get
			{
				return GooglePlayGames.OurUtils.Logger.debugLogEnabled;
			}
			set
			{
				GooglePlayGames.OurUtils.Logger.debugLogEnabled = value;
			}
		}

		public static bool WarningLogEnabled
		{
			get
			{
				return GooglePlayGames.OurUtils.Logger.warningLogEnabled;
			}
			set
			{
				GooglePlayGames.OurUtils.Logger.warningLogEnabled = value;
			}
		}

		static Logger()
		{
			GooglePlayGames.OurUtils.Logger.warningLogEnabled = true;
		}

		public Logger()
		{
		}

		public static void d(string msg)
		{
			if (GooglePlayGames.OurUtils.Logger.debugLogEnabled)
			{
				Debug.Log(GooglePlayGames.OurUtils.Logger.ToLogMessage(string.Empty, "DEBUG", msg));
			}
		}

		public static string describe(byte[] b)
		{
			return (b != null ? string.Concat("byte[", (int)b.Length, "]") : "(null)");
		}

		public static void e(string msg)
		{
			if (GooglePlayGames.OurUtils.Logger.warningLogEnabled)
			{
				Debug.LogWarning(GooglePlayGames.OurUtils.Logger.ToLogMessage("***", "ERROR", msg));
			}
		}

		private static string ToLogMessage(string prefix, string logType, string msg)
		{
			object[] str = new object[] { prefix, null, null, null };
			str[1] = DateTime.Now.ToString("MM/dd/yy H:mm:ss zzz");
			str[2] = logType;
			str[3] = msg;
			return string.Format("{0} [Play Games Plugin DLL] {1} {2}: {3}", str);
		}

		public static void w(string msg)
		{
			if (GooglePlayGames.OurUtils.Logger.warningLogEnabled)
			{
				Debug.LogWarning(GooglePlayGames.OurUtils.Logger.ToLogMessage("!!!", "WARNING", msg));
			}
		}
	}
}