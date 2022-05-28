using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace DevToDev.Core.Utils
{
	internal static class Log
	{
		private static readonly string tag;

		private static string DIRECTORY_NAME;

		private static readonly string FILE_NAME;

		private static readonly bool DEBUG_LOG_ENABLED;

		private static StringBuilder logBuffer;

		private static Timer timer;

		public static bool LogEnabled
		{
			[CompilerGenerated]
			get
			{
				return _003CLogEnabled_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLogEnabled_003Ek__BackingField = value;
			}
		}

		static Log()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			tag = "DevToDev";
			FILE_NAME = "log.txt";
			DEBUG_LOG_ENABLED = false;
			logBuffer = new StringBuilder();
			if (DIRECTORY_NAME == null)
			{
				DIRECTORY_NAME = LogPlatform.GetDirectoryPath();
			}
			if (DEBUG_LOG_ENABLED && !UnityPlayerPlatform.isUnityWebPlatform())
			{
				timer = new Timer(new TimerCallback(AppendLog), (object)default(object), 1000, 1000);
			}
		}

		public static void Resume()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			if (!UnityPlayerPlatform.isUnityWebPlatform() && DEBUG_LOG_ENABLED && timer == null)
			{
				timer = new Timer(new TimerCallback(AppendLog), (object)default(object), 1000, 1000);
			}
		}

		public static void Suspend()
		{
			if (timer != null)
			{
				timer.Dispose();
				timer = null;
			}
		}

		private static void AppendLog(object state)
		{
			if (logBuffer.get_Length() <= 0)
			{
				return;
			}
			Stream val = null;
			try
			{
				if (!Directory.Exists(DIRECTORY_NAME))
				{
					Directory.CreateDirectory(DIRECTORY_NAME);
				}
				string text = DIRECTORY_NAME + "\\" + FILE_NAME;
				Stream val2 = (val = (Stream)(object)File.Open(text, (FileMode)6));
				try
				{
					lock (logBuffer)
					{
						byte[] bytes = Encoding.get_UTF8().GetBytes(((object)logBuffer).ToString());
						val.Write(bytes, 0, bytes.Length);
						logBuffer.set_Length(0);
						logBuffer.set_Capacity(0);
					}
				}
				finally
				{
					if (val2 != null)
					{
						((global::System.IDisposable)val2).Dispose();
					}
				}
			}
			catch (global::System.Exception)
			{
			}
			finally
			{
				if (val != null)
				{
					LogPlatform.CloseStream(val);
				}
			}
		}

		public static void D(string logMessage)
		{
			D(logMessage, null);
		}

		public static void D(string format, params object[] args)
		{
			if (LogEnabled && DEBUG_LOG_ENABLED)
			{
				string text = string.Concat(new string[6]
				{
					"[",
					tag,
					"] ",
					global::System.DateTime.get_Now().ToString("yyyy/MM/dd HH:mm:ss"),
					" : ",
					(args != null) ? string.Format(format, args) : format
				});
				Debug.Log(text);
				lock (logBuffer)
				{
					logBuffer.Append(text).Append("\r\n");
				}
			}
		}

		public static void E(string logMessage)
		{
			E(logMessage, null);
		}

		public static void E(string format, params object[] args)
		{
			if (LogEnabled && DEBUG_LOG_ENABLED)
			{
				string text = string.Concat(new string[6]
				{
					"[",
					tag,
					"] ",
					global::System.DateTime.get_Now().ToString("yyyy/MM/dd HH:mm:ss"),
					" : ",
					(args != null) ? string.Format(format, args) : format
				});
				Debug.LogError(text);
				lock (logBuffer)
				{
					logBuffer.Append(text).Append("\r\n");
				}
			}
		}

		public static void R(string logMessage)
		{
			R(logMessage, null);
		}

		public static void R(string format, params object[] args)
		{
			if (!LogEnabled)
			{
				return;
			}
			string text = string.Concat(new string[6]
			{
				"[",
				tag,
				"] ",
				global::System.DateTime.get_Now().ToString("yyyy/MM/dd HH:mm:ss"),
				" : ",
				(args != null) ? string.Format(format, args) : format
			});
			Debug.Log(text);
			if (DEBUG_LOG_ENABLED)
			{
				lock (logBuffer)
				{
					logBuffer.Append(text).Append("\r\n");
				}
			}
		}
	}
}
