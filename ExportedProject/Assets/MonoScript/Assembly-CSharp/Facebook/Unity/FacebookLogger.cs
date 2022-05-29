using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facebook.Unity
{
	internal static class FacebookLogger
	{
		private const string UnityAndroidTag = "Facebook.Unity.FBDebug";

		internal static IFacebookLogger Instance
		{
			private get;
			set;
		}

		static FacebookLogger()
		{
			FacebookLogger.Instance = new FacebookLogger.CustomLogger();
		}

		public static void Error(string msg)
		{
			FacebookLogger.Instance.Error(msg);
		}

		public static void Error(string format, params string[] args)
		{
			FacebookLogger.Error(string.Format(format, args));
		}

		public static void Info(string msg)
		{
			FacebookLogger.Instance.Info(msg);
		}

		public static void Info(string format, params string[] args)
		{
			FacebookLogger.Info(string.Format(format, args));
		}

		public static void Log(string msg)
		{
			FacebookLogger.Instance.Log(msg);
		}

		public static void Log(string format, params string[] args)
		{
			FacebookLogger.Log(string.Format(format, args));
		}

		public static void Warn(string msg)
		{
			FacebookLogger.Instance.Warn(msg);
		}

		public static void Warn(string format, params string[] args)
		{
			FacebookLogger.Warn(string.Format(format, args));
		}

		private class AndroidLogger : IFacebookLogger
		{
			public AndroidLogger()
			{
			}

			public void Error(string msg)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("e", new object[] { "Facebook.Unity.FBDebug", msg });
				}
			}

			public void Info(string msg)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("i", new object[] { "Facebook.Unity.FBDebug", msg });
				}
			}

			public void Log(string msg)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("v", new object[] { "Facebook.Unity.FBDebug", msg });
				}
			}

			public void Warn(string msg)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("w", new object[] { "Facebook.Unity.FBDebug", msg });
				}
			}
		}

		private class CustomLogger : IFacebookLogger
		{
			private IFacebookLogger logger;

			public CustomLogger()
			{
				this.logger = new FacebookLogger.AndroidLogger();
			}

			public void Error(string msg)
			{
				Debug.LogError(msg);
				this.logger.Error(msg);
			}

			public void Info(string msg)
			{
				Debug.Log(msg);
				this.logger.Info(msg);
			}

			public void Log(string msg)
			{
				if (Debug.isDebugBuild)
				{
					Debug.Log(msg);
					this.logger.Log(msg);
				}
			}

			public void Warn(string msg)
			{
				Debug.LogWarning(msg);
				this.logger.Warn(msg);
			}
		}
	}
}