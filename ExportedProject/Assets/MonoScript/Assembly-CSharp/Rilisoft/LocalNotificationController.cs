using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	internal sealed class LocalNotificationController : MonoBehaviour
	{
		private const int GachaNotificationId = 1000;

		private const int ReturnNotificationId = 2000;

		private readonly LocalNotificationController.LocalNotification[] returnNotifications = new LocalNotificationController.LocalNotification[] { LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2239"), LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2240") };

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

		public LocalNotificationController()
		{
		}

		private void Awake()
		{
			string str = string.Format("{0}.Awake()", base.GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				this.CancelNotifications();
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void CancelNotifications()
		{
			string str = string.Format("{0}.CancelNotifications()", base.GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
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
			TimeSpan timeSpan1 = TimeSpan.FromMinutes((double)UnityEngine.Random.Range(-30f, 30f));
			return (rawDateTime.Date + timeSpan) + timeSpan1;
		}

		private void Destroy()
		{
			string str = string.Format("{0}.Destroy()", base.GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				this.ScheduleNotifications();
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			string str = string.Format("{0}.OnApplicationPause({1})", base.GetType().Name, pauseStatus);
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				if (!pauseStatus)
				{
					this.CancelNotifications();
				}
				else
				{
					this.ScheduleNotifications();
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void OnApplicationQuit()
		{
			string str = string.Format("{0}.OnApplicationQuit()", base.GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				this.ScheduleNotifications();
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private int SafeGetSdkLevel()
		{
			int sdkVersion;
			try
			{
				sdkVersion = AndroidSystem.GetSdkVersion();
			}
			catch (Exception exception)
			{
				Debug.LogWarning(exception);
				sdkVersion = 0;
			}
			return sdkVersion;
		}

		private void ScheduleNotifications()
		{
			string str = string.Format("{0}.ScheduleNotifications()", base.GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				if (this.GachaNotificationEnabled && ExperienceController.GetCurrentLevel() >= 2)
				{
					TimeSpan freeGachaAvailableIn = GiftController.Instance.FreeGachaAvailableIn;
					string str1 = string.Format(CultureInfo.InvariantCulture, "Scheduling gacha notification in {0}", new object[] { freeGachaAvailableIn });
					ScopeLogger scopeLogger1 = new ScopeLogger(str, str1, true);
					try
					{
						int num = Convert.ToInt32(freeGachaAvailableIn.TotalSeconds);
						if (num > 0)
						{
							if (this.SafeGetSdkLevel() < 21)
							{
								EtceteraAndroid.scheduleNotification((long)num, "Chest available!", "Claim your gift!", "Check free chest!", string.Empty, "small_icon", "large_icon", 1000);
							}
							else
							{
								EtceteraAndroid.scheduleNotification((long)num, "Chest available!", "Claim your gift!", "Check free chest!", string.Empty, 1000);
							}
						}
					}
					finally
					{
						scopeLogger1.Dispose();
					}
				}
				if (this.ReturnNotificationEnabled)
				{
					DateTime now = DateTime.Now;
					DateTime[] array = new DateTime[] { this.ClampTimeOfTheDay(now.AddDays(3)), this.ClampTimeOfTheDay(now.AddDays(7)) };
					if (Defs.IsDeveloperBuild)
					{
						array = ((IEnumerable<DateTime>)(new DateTime[] { now.AddMinutes(1) })).Concat<DateTime>(array).ToArray<DateTime>();
					}
					int num1 = UnityEngine.Random.Range(0, (int)this.returnNotifications.Length);
					for (int i = 0; i != (int)array.Length; i++)
					{
						DateTime dateTime = array[i];
						int num2 = Convert.ToInt32((dateTime - now).TotalSeconds);
						int length = (num1 + i) % (int)this.returnNotifications.Length;
						LocalNotificationController.LocalNotification localNotification = this.returnNotifications[length];
						if (this.SafeGetSdkLevel() < 21)
						{
							EtceteraAndroid.scheduleNotification((long)num2, localNotification.Title, localNotification.Subtitle, localNotification.Title, string.Empty, 2000 + i);
						}
						else
						{
							EtceteraAndroid.scheduleNotification((long)num2, localNotification.Title, localNotification.Subtitle, localNotification.Title, string.Empty, "small_icon", "large_icon", 2000 + i);
						}
					}
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private struct LocalNotification
		{
			private readonly string _title;

			private readonly string _subtitle;

			public string Subtitle
			{
				get
				{
					return this._subtitle ?? string.Empty;
				}
			}

			public string Title
			{
				get
				{
					return this._title ?? string.Empty;
				}
			}

			public LocalNotification(string title, string subtitle)
			{
				this._title = title ?? string.Empty;
				this._subtitle = subtitle ?? string.Empty;
			}

			public static LocalNotificationController.LocalNotification FromLocalizationKeys(string titleKey, string subtitleKey)
			{
				string str = LocalizationStore.Get(titleKey ?? string.Empty);
				return new LocalNotificationController.LocalNotification(str, LocalizationStore.Get(subtitleKey ?? string.Empty));
			}
		}
	}
}