using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	internal sealed class LocalNotificationController : MonoBehaviour
	{
		private struct LocalNotification
		{
			private readonly string _title;

			private readonly string _subtitle;

			public string Title
			{
				get
				{
					return _title ?? string.Empty;
				}
			}

			public string Subtitle
			{
				get
				{
					return _subtitle ?? string.Empty;
				}
			}

			public LocalNotification(string title, string subtitle)
			{
				_title = title ?? string.Empty;
				_subtitle = subtitle ?? string.Empty;
			}

			public static LocalNotification FromLocalizationKeys(string titleKey, string subtitleKey)
			{
				string title = LocalizationStore.Get(titleKey ?? string.Empty);
				string subtitle = LocalizationStore.Get(subtitleKey ?? string.Empty);
				return new LocalNotification(title, subtitle);
			}
		}

		private const int GachaNotificationId = 1000;

		private const int ReturnNotificationId = 2000;

		private readonly LocalNotification[] returnNotifications = new LocalNotification[2]
		{
			LocalNotification.FromLocalizationKeys("Key_2225", "Key_2239"),
			LocalNotification.FromLocalizationKeys("Key_2225", "Key_2240")
		};

		private bool GachaNotificationEnabled
		{
			get
			{
				return false;
			}
		}

		private bool ReturnNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		private void Awake()
		{
			string callee = string.Format("{0}.Awake()", GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
			try
			{
				CancelNotifications();
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void Destroy()
		{
			string callee = string.Format("{0}.Destroy()", GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
			try
			{
				ScheduleNotifications();
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void OnApplicationQuit()
		{
			string callee = string.Format("{0}.OnApplicationQuit()", GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
			try
			{
				ScheduleNotifications();
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			string callee = string.Format("{0}.OnApplicationPause({1})", GetType().Name, pauseStatus);
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
			try
			{
				if (pauseStatus)
				{
					ScheduleNotifications();
				}
				else
				{
					CancelNotifications();
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void ScheduleNotifications()
		{
			string text = string.Format("{0}.ScheduleNotifications()", GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(text, Defs.IsDeveloperBuild);
			try
			{
				if (GachaNotificationEnabled && ExperienceController.GetCurrentLevel() >= 2)
				{
					TimeSpan freeGachaAvailableIn = GiftController.Instance.FreeGachaAvailableIn;
					string callee = string.Format(CultureInfo.InvariantCulture, "Scheduling gacha notification in {0}", freeGachaAvailableIn);
					ScopeLogger scopeLogger2 = new ScopeLogger(text, callee, true);
					try
					{
						int num = Convert.ToInt32(freeGachaAvailableIn.TotalSeconds);
						if (num > 0)
						{
							int num2 = SafeGetSdkLevel();
							if (num2 >= 21)
							{
								EtceteraAndroid.scheduleNotification(num, "Chest available!", "Claim your gift!", "Check free chest!", string.Empty, 1000);
							}
							else
							{
								EtceteraAndroid.scheduleNotification(num, "Chest available!", "Claim your gift!", "Check free chest!", string.Empty, "small_icon", "large_icon", 1000);
							}
						}
					}
					finally
					{
						scopeLogger2.Dispose();
					}
				}
				if (!ReturnNotificationEnabled)
				{
					return;
				}
				DateTime now = DateTime.Now;
				DateTime[] array = new DateTime[2]
				{
					ClampTimeOfTheDay(now.AddDays(3.0)),
					ClampTimeOfTheDay(now.AddDays(7.0))
				};
				if (Defs.IsDeveloperBuild)
				{
					array = new DateTime[1] { now.AddMinutes(1.0) }.Concat(array).ToArray();
				}
				int num3 = UnityEngine.Random.Range(0, returnNotifications.Length);
				for (int i = 0; i != array.Length; i++)
				{
					DateTime dateTime = array[i];
					int num4 = Convert.ToInt32((dateTime - now).TotalSeconds);
					int num5 = (num3 + i) % returnNotifications.Length;
					LocalNotification localNotification = returnNotifications[num5];
					int num6 = SafeGetSdkLevel();
					if (num6 >= 21)
					{
						EtceteraAndroid.scheduleNotification(num4, localNotification.Title, localNotification.Subtitle, localNotification.Title, string.Empty, "small_icon", "large_icon", 2000 + i);
					}
					else
					{
						EtceteraAndroid.scheduleNotification(num4, localNotification.Title, localNotification.Subtitle, localNotification.Title, string.Empty, 2000 + i);
					}
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void CancelNotifications()
		{
			string callee = string.Format("{0}.CancelNotifications()", GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
			try
			{
				EtceteraAndroid.cancelNotification(1000);
				for (int i = 0; i != 3; i++)
				{
					EtceteraAndroid.cancelNotification(2000 + i);
				}
				EtceteraAndroid.cancelAllNotifications();
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private DateTime ClampTimeOfTheDay(DateTime rawDateTime)
		{
			TimeSpan timeSpan = new TimeSpan(16, 0, 0);
			TimeSpan timeSpan2 = TimeSpan.FromMinutes(UnityEngine.Random.Range(-30f, 30f));
			return rawDateTime.Date + timeSpan + timeSpan2;
		}

		private int SafeGetSdkLevel()
		{
			//Discarded unreachable code: IL_000b, IL_001e
			try
			{
				return AndroidSystem.GetSdkVersion();
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				return 0;
			}
		}
	}
}
