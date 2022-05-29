using ExitGames.Client.Photon;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class NotificationController : MonoBehaviour
{
	private const string ScheduledNotificationsKey = "Scheduled Notifications";

	public static bool isGetEveryDayMoney;

	public static float timeStartApp;

	public bool pauserTemp;

	private float playTime;

	private float playTimeInMatch;

	public float savedPlayTime;

	public float savedPlayTimeInMatch;

	public static NotificationController instance;

	private static bool _paused;

	private readonly List<int> _notificationIds = new List<int>();

	public float currentPlayTime
	{
		get
		{
			return this.savedPlayTime + this.playTime;
		}
	}

	public float currentPlayTimeMatch
	{
		get
		{
			return this.savedPlayTimeInMatch + this.playTimeInMatch;
		}
	}

	internal static bool Paused
	{
		get
		{
			return NotificationController._paused;
		}
	}

	static NotificationController()
	{
	}

	public NotificationController()
	{
	}

	[DebuggerHidden]
	private IEnumerator appStart()
	{
		return new NotificationController.u003cappStartu003ec__Iterator171();
	}

	private void appStop()
	{
		bool flag = (BankController.Instance == null ? false : BankController.Instance.InterfaceEnabled);
		if (PhotonNetwork.connected)
		{
			NotificationController._paused = true;
		}
		int hour = DateTime.Now.Hour;
		int num = 82800;
		hour += 23;
		if (hour > 24)
		{
			hour -= 24;
		}
		num = num + (hour <= 16 ? 16 - hour : 24 - hour + 16) * 3600;
		DateTime now = DateTime.Now;
		DateTime dateTime = now + TimeSpan.FromHours(23);
		DateTime dateTime1 = (dateTime.Hour >= 16 ? dateTime.Date.AddHours(40) : dateTime.Date.AddHours(16));
		TimeSpan timeSpan = TimeSpan.FromDays(1);
		int num1 = 0;
		for (int i = 0; i < num1; i++)
		{
			int num2 = num + i * 86400;
			num2 = num2 - 1800 + UnityEngine.Random.Range(0, 3600);
			DateTime dateTime2 = dateTime1 + TimeSpan.FromTicks(timeSpan.Ticks * (long)i);
			TimeSpan timeSpan1 = dateTime2 - now;
			int totalSeconds = (int)timeSpan1.TotalSeconds + UnityEngine.Random.Range(-1800, 1800);
			string empty = string.Empty;
			int num3 = EtceteraAndroid.scheduleNotification((long)totalSeconds, "Challenge", LocalizationStore.Get("Key_1657"), LocalizationStore.Get("Key_0012"), empty, -1);
			this._notificationIds.Add(num3);
		}
		string str = Json.Serialize(this._notificationIds);
		UnityEngine.Debug.Log(string.Concat("Notifications to save: ", str));
		PlayerPrefs.SetString("Scheduled Notifications", str);
		PlayerPrefs.Save();
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pauseStatus)
	{
		NotificationController.u003cOnApplicationPauseu003ec__Iterator172 variable = null;
		return variable;
	}

	internal static void ResetPaused()
	{
		NotificationController._paused = false;
	}

	public void SaveTimeValues()
	{
		if (this.playTime > 0f)
		{
			this.savedPlayTime += this.playTime;
			UnityEngine.Debug.Log(string.Format("PlayTime saved: {0} (+{1})", this.savedPlayTime, this.playTime));
			this.playTime = 0f;
			Storager.setString("PlayTime", this.savedPlayTime.ToString(), false);
		}
		if (this.playTimeInMatch > 0f)
		{
			this.savedPlayTimeInMatch += this.playTimeInMatch;
			UnityEngine.Debug.Log(string.Format("PlayTimeInMatch saved: {0} (+{1})", this.savedPlayTimeInMatch, this.playTimeInMatch));
			this.playTimeInMatch = 0f;
			Storager.setString("PlayTimeInMatch", this.savedPlayTimeInMatch.ToString(), false);
		}
	}

	private void Start()
	{
		float single;
		float single1;
		ScopeLogger scopeLogger = new ScopeLogger("NotificationController.Start()", Defs.IsDeveloperBuild);
		try
		{
			base.gameObject.AddComponent<LocalNotificationController>();
			if (Load.LoadBool("bilZapuskKey"))
			{
				base.StartCoroutine(this.appStart());
			}
			else
			{
				Save.SaveBool("bilZapuskKey", true);
			}
			NotificationController.instance = this;
			if (Storager.hasKey("PlayTime") && float.TryParse(Storager.getString("PlayTime", false), out single))
			{
				this.savedPlayTime = single;
			}
			if (Storager.hasKey("PlayTimeInMatch") && float.TryParse(Storager.getString("PlayTimeInMatch", false), out single1))
			{
				this.savedPlayTimeInMatch = single1;
			}
		}
		finally
		{
			scopeLogger.Dispose();
		}
	}

	private void Update()
	{
		if (this.pauserTemp)
		{
			this.pauserTemp = false;
			NotificationController._paused = true;
			PhotonNetwork.Disconnect();
		}
		if (!FriendsController.sharedController.idle)
		{
			this.playTime += Time.deltaTime;
			if (Initializer.Instance != null && (PhotonNetwork.room == null || PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].Equals(string.Empty)) && !Defs.isDaterRegim && !Defs.isHunger && !Defs.isCOOP && !NetworkStartTable.LocalOrPasswordRoom())
			{
				this.playTimeInMatch += Time.deltaTime;
			}
		}
	}
}