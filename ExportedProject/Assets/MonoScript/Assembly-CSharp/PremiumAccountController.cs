using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PremiumAccountController : MonoBehaviour
{
	private const float PremInfoTimeout = 1200f;

	private DateTime _timeStart;

	private DateTime _timeEnd;

	private TimeSpan _timeToEndAccount;

	private float _lastCheckTime;

	private int _additionalAccountDays;

	private long _lastLoggedAccountTime;

	private int _countCeilDays;

	private bool _isGetPremInfoRunning;

	private float _premGetInfoStartTime;

	public static bool AccountHasExpired
	{
		get;
		set;
	}

	public static PremiumAccountController Instance
	{
		get;
		private set;
	}

	public bool isAccountActive
	{
		get;
		private set;
	}

	public int RewardCoeff
	{
		get
		{
			return (!this.isAccountActive ? 1 : 2);
		}
	}

	public static float VirtualCurrencyMultiplier
	{
		get
		{
			if (PremiumAccountController.Instance == null)
			{
				UnityEngine.Debug.LogError("VirtualCurrencyMultiplier Instance == null");
				return 1f;
			}
			PremiumAccountController.AccountType currentAccount = PremiumAccountController.Instance.GetCurrentAccount();
			if (currentAccount == PremiumAccountController.AccountType.SevenDays)
			{
				return 1.05f;
			}
			if (currentAccount == PremiumAccountController.AccountType.Month)
			{
				return 1.1f;
			}
			return 1f;
		}
	}

	public PremiumAccountController()
	{
	}

	private void AddBoughtAccountInHistory(PremiumAccountController.AccountType accountType)
	{
		string str = Storager.getString("BuyHistoryPremiumAccount", false);
		str = (!string.IsNullOrEmpty(str) ? string.Concat(str, string.Format(",{0}", (int)accountType)) : string.Format("{0}", (int)accountType));
		Storager.setString("BuyHistoryPremiumAccount", str, false);
	}

	public void BuyAccount(PremiumAccountController.AccountType accountType)
	{
		if (this.GetCurrentAccount() == PremiumAccountController.AccountType.None)
		{
			this.StartNewAccount(accountType);
		}
		this.AddBoughtAccountInHistory(accountType);
		this._additionalAccountDays = this.GetAllTimeOtherAccountFromHistory();
		PremiumAccountController.AccountHasExpired = false;
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	private bool ChangeAccountOnNext()
	{
		this.DeleteBoughtAccountFromHistory();
		this._additionalAccountDays = this.GetAllTimeOtherAccountFromHistory();
		PremiumAccountController.AccountType currentAccount = this.GetCurrentAccount();
		if (currentAccount == PremiumAccountController.AccountType.None)
		{
			return false;
		}
		this.StartNewAccount(currentAccount);
		if (PremiumAccountController.OnAccountChanged != null)
		{
			PremiumAccountController.OnAccountChanged();
		}
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		return true;
	}

	private void ChangeCurrentAccount()
	{
		if (!this.ChangeAccountOnNext())
		{
			this.StopAccountsWork();
		}
	}

	private bool CheckInitializeCurrentAccount()
	{
		DateTime dateTime = new DateTime();
		bool flag = Tools.ParseDateTimeFromPlayerPrefs("StartTimePremiumAccount", out dateTime);
		DateTime dateTime1 = new DateTime();
		bool flag1 = Tools.ParseDateTimeFromPlayerPrefs("EndTimePremiumAccount", out dateTime1);
		if (!flag || !flag1)
		{
			return false;
		}
		this._timeStart = dateTime;
		this._timeEnd = dateTime1;
		return true;
	}

	private void CheckTimeHack()
	{
		this._lastLoggedAccountTime = this.GetLastLoggedTime();
		if (this._lastLoggedAccountTime != 0 && PromoActionsManager.CurrentUnixTime < this._lastLoggedAccountTime)
		{
			this.StopAccountsWork();
		}
	}

	private void DeleteBoughtAccountFromHistory()
	{
		string str = Storager.getString("BuyHistoryPremiumAccount", false);
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		int num = str.IndexOf(',');
		str = (num <= 0 ? string.Empty : str.Remove(0, num + 1));
		Storager.setString("BuyHistoryPremiumAccount", str, false);
	}

	private void Destroy()
	{
		this.UpdateLastLoggedTime();
		PremiumAccountController.Instance = null;
	}

	[DebuggerHidden]
	private IEnumerator DownloadPremInfo()
	{
		PremiumAccountController.u003cDownloadPremInfou003ec__Iterator17A variable = null;
		return variable;
	}

	private int GetAllTimeOtherAccountFromHistory()
	{
		string str = Storager.getString("BuyHistoryPremiumAccount", false);
		if (string.IsNullOrEmpty(str))
		{
			return 0;
		}
		string[] strArrays = str.Split(new char[] { ',' });
		if ((int)strArrays.Length == 0)
		{
			return 0;
		}
		int num = 0;
		int daysAccountByType = 0;
		for (int i = 1; i < (int)strArrays.Length; i++)
		{
			int.TryParse(strArrays[i], out num);
			daysAccountByType += this.GetDaysAccountByType(num);
		}
		return daysAccountByType;
	}

	public PremiumAccountController.AccountType GetCurrentAccount()
	{
		string str = Storager.getString("BuyHistoryPremiumAccount", false);
		if (string.IsNullOrEmpty(str))
		{
			return PremiumAccountController.AccountType.None;
		}
		string[] strArrays = str.Split(new char[] { ',' });
		if ((int)strArrays.Length == 0)
		{
			return PremiumAccountController.AccountType.None;
		}
		int num = 0;
		if (!int.TryParse(strArrays[0], out num))
		{
			return PremiumAccountController.AccountType.None;
		}
		return (PremiumAccountController.AccountType)num;
	}

	private int GetDaysAccountByType(int codeAccount)
	{
		switch (codeAccount)
		{
			case 0:
			{
				return 1;
			}
			case 1:
			{
				return 3;
			}
			case 2:
			{
				return 7;
			}
			case 3:
			{
				return 30;
			}
		}
		return 0;
	}

	public int GetDaysToEndAllAccounts()
	{
		return this._countCeilDays + this._additionalAccountDays;
	}

	private long GetLastLoggedTime()
	{
		long num;
		if (!this.isAccountActive)
		{
			return (long)0;
		}
		if (!Storager.hasKey("LastLoggedTimePremiumAccount"))
		{
			return (long)0;
		}
		string str = Storager.getString("LastLoggedTimePremiumAccount", false);
		long.TryParse(str, out num);
		return num;
	}

	[DebuggerHidden]
	private IEnumerator GetPremInfoLoop()
	{
		PremiumAccountController.u003cGetPremInfoLoopu003ec__Iterator179 variable = null;
		return variable;
	}

	public int GetRewardCoeffByActiveOrActiveBeforeMatch()
	{
		return (!this.IsActiveOrWasActiveBeforeStartMatch() ? 1 : 2);
	}

	private DateTime GetTimeEndAccount(DateTime startTime, PremiumAccountController.AccountType accountType)
	{
		DateTime dateTime = startTime;
		switch (accountType)
		{
			case PremiumAccountController.AccountType.OneDay:
			{
				dateTime = dateTime.AddDays(1);
				break;
			}
			case PremiumAccountController.AccountType.ThreeDay:
			{
				dateTime = dateTime.AddDays(3);
				break;
			}
			case PremiumAccountController.AccountType.SevenDays:
			{
				dateTime = dateTime.AddDays(7);
				break;
			}
			case PremiumAccountController.AccountType.Month:
			{
				dateTime = dateTime.AddDays(30);
				break;
			}
		}
		return dateTime;
	}

	public string GetTimeToEndAllAccounts()
	{
		if (!this.isAccountActive)
		{
			return string.Empty;
		}
		TimeSpan timeSpan = this._timeToEndAccount.Add(TimeSpan.FromDays((double)this._additionalAccountDays));
		if (timeSpan.Days <= 0)
		{
			return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}
		string str = "Days";
		this._countCeilDays = Mathf.CeilToInt((float)this._timeToEndAccount.TotalDays) + this._additionalAccountDays;
		return string.Format("{0}: {1}", str, this._countCeilDays);
	}

	public bool IsActiveOrWasActiveBeforeStartMatch()
	{
		Player_move_c playerMoveC;
		if (this.isAccountActive)
		{
			return true;
		}
		if (WeaponManager.sharedManager != null)
		{
			playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		else
		{
			playerMoveC = null;
		}
		Player_move_c playerMoveC1 = playerMoveC;
		if (playerMoveC1 == null)
		{
			return false;
		}
		return playerMoveC1.isNeedTakePremiumAccountRewards;
	}

	public static bool MapAvailableDueToPremiumAccount(string mapName)
	{
		if (mapName == null || PremiumAccountController.Instance == null)
		{
			return false;
		}
		return (Defs.PremiumMaps == null || !Defs.PremiumMaps.ContainsKey(mapName) ? false : PremiumAccountController.Instance.isAccountActive);
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pause)
	{
		PremiumAccountController.u003cOnApplicationPauseu003ec__Iterator178 variable = null;
		return variable;
	}

	private void Start()
	{
		PremiumAccountController.Instance = this;
		this._timeToEndAccount = new TimeSpan();
		this._additionalAccountDays = this.GetAllTimeOtherAccountFromHistory();
		this.isAccountActive = this.CheckInitializeCurrentAccount();
		this.CheckTimeHack();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.StartCoroutine(this.GetPremInfoLoop());
	}

	private void StartNewAccount(PremiumAccountController.AccountType accountType)
	{
		this.isAccountActive = true;
		this._timeStart = DateTime.UtcNow;
		Storager.setString("StartTimePremiumAccount", this._timeStart.ToString("s"), false);
		this._timeEnd = this.GetTimeEndAccount(this._timeStart, accountType);
		Storager.setString("EndTimePremiumAccount", this._timeEnd.ToString("s"), false);
	}

	private void StopAccountsWork()
	{
		this.isAccountActive = false;
		if (PremiumAccountController.OnAccountChanged != null)
		{
			PremiumAccountController.OnAccountChanged();
		}
		Storager.setString("StartTimePremiumAccount", string.Empty, false);
		Storager.setString("EndTimePremiumAccount", string.Empty, false);
		Storager.setString("BuyHistoryPremiumAccount", string.Empty, false);
		this._timeToEndAccount = TimeSpan.FromMinutes(0);
		this._additionalAccountDays = 0;
		this._countCeilDays = 0;
		PremiumAccountController.AccountHasExpired = true;
	}

	private void Update()
	{
		if (!this.isAccountActive)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastCheckTime >= 1f)
		{
			this._timeToEndAccount = this._timeEnd - DateTime.UtcNow;
			this.isAccountActive = DateTime.UtcNow <= this._timeEnd;
			if (!this.isAccountActive)
			{
				this.ChangeCurrentAccount();
			}
			this._lastCheckTime = Time.realtimeSinceStartup;
		}
	}

	private void UpdateLastLoggedTime()
	{
		if (!this.isAccountActive)
		{
			return;
		}
		Storager.setString("LastLoggedTimePremiumAccount", PromoActionsManager.CurrentUnixTime.ToString(), false);
	}

	public static event PremiumAccountController.OnAccountChangedDelegate OnAccountChanged;

	public enum AccountType
	{
		OneDay,
		ThreeDay,
		SevenDays,
		Month,
		None
	}

	public delegate void OnAccountChangedDelegate();
}