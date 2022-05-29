using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public sealed class ChestBonusController : MonoBehaviour
{
	private const float BonusUpdateTimeout = 870f;

	public static bool chestBonusesObtainedOnceInCurrentRun;

	private ChestBonusesData _bonusesData;

	private float _lastCheckEventTime;

	private bool _lastBonusActive;

	private DateTime _timeStartBonus;

	private DateTime _timeEndBonus;

	private bool _isGetBonusInfoRunning;

	private float _eventGetBonusInfoStartTime;

	public static ChestBonusController Get
	{
		get;
		private set;
	}

	public bool IsBonusActive
	{
		get;
		private set;
	}

	static ChestBonusController()
	{
	}

	public ChestBonusController()
	{
	}

	[DebuggerHidden]
	private IEnumerator DownloadDataAboutBonuses()
	{
		ChestBonusController.u003cDownloadDataAboutBonusesu003ec__Iterator127 variable = null;
		return variable;
	}

	public ChestBonusData GetBonusData(PurchaseEventArgs purchaseInfo)
	{
		bool currency = purchaseInfo.Currency == "GemsCurrency";
		return this.GetBonusData(currency, purchaseInfo.Index);
	}

	private ChestBonusData GetBonusData(bool isGemsPack, int packOrder)
	{
		if (this._bonusesData == null || this._bonusesData.bonuses == null)
		{
			return null;
		}
		string str = (!isGemsPack ? "coins" : "gems");
		string str1 = string.Format("{0}_{1}", str, packOrder + 1);
		for (int i = 0; i < this._bonusesData.bonuses.Count; i++)
		{
			ChestBonusData item = this._bonusesData.bonuses[i];
			if (item.linkKey == str1)
			{
				return item;
			}
		}
		return null;
	}

	[DebuggerHidden]
	private IEnumerator GetEventBonusInfoLoop(Task futureToWait)
	{
		ChestBonusController.u003cGetEventBonusInfoLoopu003ec__Iterator126 variable = null;
		return variable;
	}

	private bool IsBonusActivate()
	{
		if (this._bonusesData.timeStart == 0 || this._bonusesData.duration == 0)
		{
			return false;
		}
		DateTime utcNow = DateTime.UtcNow;
		return (utcNow < this._timeStartBonus ? false : utcNow <= this._timeEndBonus);
	}

	public bool IsBonusActiveForItem(PurchaseEventArgs purchaseInfo)
	{
		if (!this.IsBonusActive)
		{
			return false;
		}
		ChestBonusData bonusData = this.GetBonusData(purchaseInfo);
		return (bonusData == null ? false : bonusData.isVisible);
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			base.StartCoroutine(this.DownloadDataAboutBonuses());
		}
	}

	private void OnDestroy()
	{
		ChestBonusController.Get = null;
	}

	public void ShowBonusWindowForItem(PurchaseEventArgs purchaseInfo)
	{
		ChestBonusData bonusData = this.GetBonusData(purchaseInfo);
		BankController instance = BankController.Instance;
		if (bonusData != null && instance != null)
		{
			instance.bonusDetailView.Show(bonusData);
		}
	}

	private void Start()
	{
		ChestBonusController.Get = this;
		this._bonusesData = new ChestBonusesData();
		this._timeStartBonus = new DateTime();
		this._timeEndBonus = new DateTime();
		Task firstResponse = PersistentCacheManager.Instance.FirstResponse;
		base.StartCoroutine(this.GetEventBonusInfoLoop(firstResponse));
	}

	public static bool TryTakeChestBonus(bool isGemsPack, int packOrder)
	{
		ChestBonusController get = ChestBonusController.Get;
		if (get == null)
		{
			return false;
		}
		if (!get.IsBonusActive)
		{
			return false;
		}
		ChestBonusData bonusData = get.GetBonusData(isGemsPack, packOrder);
		if (bonusData == null)
		{
			return false;
		}
		if (bonusData.items == null || bonusData.items.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < bonusData.items.Count; i++)
		{
			ChestBonusItemData item = bonusData.items[i];
			ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(item.tag);
			ShopNGUIController.ProvideAllTypeShopItem(itemCategory, item.tag, item.count, item.timeLife);
		}
		int currentLevel = ExperienceController.GetCurrentLevel();
		int num = (ExpController.Instance != null ? ExpController.Instance.OurTier : 0);
		string str = (!isGemsPack ? "coins" : "gems");
		string str1 = string.Format("{0}_{1}", str, packOrder + 1);
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Level", currentLevel.ToString() },
			{ "Tier", num.ToString() },
			{ "SKU", str1 }
		};
		Dictionary<string, string> strs1 = strs;
		FlurryPluginWrapper.LogEventAndDublicateToConsole((!isGemsPack ? "Bonus-Coins" : "Bonus-Gems"), strs1, true);
		return true;
	}

	private void Update()
	{
		if (!this.IsBonusActive)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastCheckEventTime >= 1f)
		{
			this.IsBonusActive = this.IsBonusActivate();
			if (this._lastBonusActive != this.IsBonusActive && ChestBonusController.OnChestBonusChange != null)
			{
				ChestBonusController.OnChestBonusChange();
				this._lastBonusActive = this.IsBonusActive;
			}
			this._lastCheckEventTime = Time.realtimeSinceStartup;
		}
	}

	public static event ChestBonusController.OnChestBonusEnabledDelegate OnChestBonusChange;

	public delegate void OnChestBonusEnabledDelegate();
}