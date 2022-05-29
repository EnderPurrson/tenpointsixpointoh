using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class MarafonBonusController
{
	private SaltedInt _currentBonusIndex;

	private static MarafonBonusController _instance;

	private static List<List<SaltedInt>> countsForPremiumAccountLevels;

	public List<BonusMarafonItem> BonusItems
	{
		get;
		private set;
	}

	public BonusMarafonItem CurrentBonus
	{
		get;
		private set;
	}

	public static MarafonBonusController Get
	{
		get
		{
			if (MarafonBonusController._instance == null)
			{
				MarafonBonusController._instance = new MarafonBonusController();
			}
			return MarafonBonusController._instance;
		}
	}

	static MarafonBonusController()
	{
		List<List<SaltedInt>> lists = new List<List<SaltedInt>>();
		List<SaltedInt> saltedInts = new List<SaltedInt>()
		{
			6,
			6,
			6,
			6,
			6,
			6,
			2,
			6,
			6,
			6,
			6,
			6,
			6,
			2,
			6,
			6,
			6,
			6,
			6,
			6,
			2,
			6,
			6,
			6,
			6,
			12,
			6,
			2,
			6,
			6
		};
		lists.Add(saltedInts);
		saltedInts = new List<SaltedInt>()
		{
			7,
			7,
			7,
			7,
			7,
			7,
			3,
			7,
			7,
			7,
			7,
			7,
			7,
			3,
			7,
			7,
			7,
			7,
			7,
			7,
			3,
			7,
			7,
			7,
			7,
			14,
			7,
			3,
			7,
			7
		};
		lists.Add(saltedInts);
		saltedInts = new List<SaltedInt>()
		{
			8,
			8,
			8,
			8,
			8,
			8,
			4,
			8,
			8,
			8,
			8,
			8,
			8,
			4,
			8,
			8,
			8,
			8,
			8,
			8,
			4,
			8,
			8,
			8,
			8,
			16,
			8,
			4,
			8,
			8
		};
		lists.Add(saltedInts);
		saltedInts = new List<SaltedInt>()
		{
			10,
			10,
			10,
			10,
			10,
			10,
			5,
			10,
			10,
			10,
			10,
			10,
			10,
			5,
			10,
			10,
			10,
			10,
			10,
			10,
			5,
			10,
			10,
			10,
			10,
			20,
			10,
			5,
			10,
			10
		};
		lists.Add(saltedInts);
		saltedInts = new List<SaltedInt>()
		{
			5,
			5,
			5,
			5,
			5,
			5,
			1,
			5,
			5,
			5,
			5,
			5,
			5,
			1,
			5,
			5,
			5,
			5,
			5,
			5,
			1,
			5,
			5,
			5,
			5,
			10,
			5,
			1,
			5,
			5
		};
		lists.Add(saltedInts);
		MarafonBonusController.countsForPremiumAccountLevels = lists;
	}

	public MarafonBonusController()
	{
		this.CurrentBonus = null;
		this.InitializeBonusItems();
		this._currentBonusIndex = Storager.getInt(Defs.NextMarafonBonusIndex, false);
	}

	private void AddGearForPlayer(string gearId, int addCount)
	{
		int num = Storager.getInt(gearId, false);
		Storager.setInt(gearId, num + addCount, false);
	}

	public BonusMarafonItem GetBonusForIndex(int index)
	{
		if (this.BonusItems == null || this.BonusItems.Count == 0)
		{
			return null;
		}
		if (index < 0 || index >= this.BonusItems.Count)
		{
			return null;
		}
		return this.BonusItems[index];
	}

	private static int GetCountForDayForCurrentPremiumLevel(int day)
	{
		int currentAccount;
		if (PremiumAccountController.Instance == null)
		{
			currentAccount = 4;
		}
		else
		{
			currentAccount = (int)PremiumAccountController.Instance.GetCurrentAccount();
		}
		int num = currentAccount;
		day--;
		if (MarafonBonusController.countsForPremiumAccountLevels.Count <= num || MarafonBonusController.countsForPremiumAccountLevels[num].Count <= day || day < 0)
		{
			return 0;
		}
		return MarafonBonusController.countsForPremiumAccountLevels[num][day].Value;
	}

	public int GetCurrentBonusIndex()
	{
		return this._currentBonusIndex.Value;
	}

	public void InitializeBonusItems()
	{
		this.BonusItems = new List<BonusMarafonItem>()
		{
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(1), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(2), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Real, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(3), "bonus_gems", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(4), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Granade, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(5), "bonus_grenade", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(6), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.PotionInvisible, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(7), "bonus_potion", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(8), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(9), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Real, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(10), "bonus_gems", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(11), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Granade, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(12), "bonus_grenade", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(13), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.JetPack, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(14), "bonus_jetpack", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(15), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(16), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Real, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(17), "bonus_gems", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(18), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Granade, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(19), "bonus_grenade", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(20), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Turret, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(21), "bonus_turret", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(22), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(23), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Real, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(24), "bonus_gems", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(25), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Granade, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(26), "bonus_grenade", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(27), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Mech, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(28), "bonus_mech", null),
			new BonusMarafonItem(BonusItemType.Gold, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(29), "bonus_coins", null),
			new BonusMarafonItem(BonusItemType.Mech, MarafonBonusController.GetCountForDayForCurrentPremiumLevel(30), "bonus_mech", null)
		};
	}

	public bool IsAvailable()
	{
		return this._currentBonusIndex.Value != -1;
	}

	public bool IsBonusTemporaryWeapon()
	{
		BonusMarafonItem item = this.BonusItems[this._currentBonusIndex.Value];
		return item.type == BonusItemType.TemporaryWeapon;
	}

	public bool IsNeedShow()
	{
		return Storager.getInt(Defs.NeedTakeMarathonBonus, false) == 1;
	}

	public void ResetSessionState()
	{
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.ResetStateBannerShowed(BannerWindowType.GiftBonuse);
		}
	}

	private void TakeBonusPlayer(int indexBonus)
	{
		if (this.BonusItems.Count == 0)
		{
			return;
		}
		this.CurrentBonus = this.BonusItems[indexBonus];
		switch (this.CurrentBonus.type)
		{
			case BonusItemType.Gold:
			{
				BankController.AddCoins(this.CurrentBonus.count.Value, true, AnalyticsConstants.AccrualType.Earned);
				FlurryEvents.LogCoinsGained("Marathon bonus", this.CurrentBonus.count.Value);
				break;
			}
			case BonusItemType.Real:
			{
				BankController.AddGems(this.CurrentBonus.count.Value, true, AnalyticsConstants.AccrualType.Earned);
				FlurryEvents.LogGemsGained("Marathon bonus", this.CurrentBonus.count.Value);
				break;
			}
			case BonusItemType.PotionInvisible:
			{
				this.AddGearForPlayer(GearManager.InvisibilityPotion, this.CurrentBonus.count.Value);
				break;
			}
			case BonusItemType.JetPack:
			{
				this.AddGearForPlayer(GearManager.Jetpack, this.CurrentBonus.count.Value);
				break;
			}
			case BonusItemType.Granade:
			{
				this.AddGearForPlayer(GearManager.Grenade, this.CurrentBonus.count.Value);
				break;
			}
			case BonusItemType.Turret:
			{
				this.AddGearForPlayer(GearManager.Turret, this.CurrentBonus.count.Value);
				break;
			}
			case BonusItemType.Mech:
			{
				this.AddGearForPlayer(GearManager.Mech, this.CurrentBonus.count.Value);
				break;
			}
			case BonusItemType.TemporaryWeapon:
			{
				ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(this.CurrentBonus.tag);
				TempItemsController.sharedController.TakeTemporaryItemToPlayer(itemCategory, this.CurrentBonus.tag, TempItemsController.RentIndexFromDays(this.CurrentBonus.count.Value));
				break;
			}
		}
		if (indexBonus + 1 < this.BonusItems.Count)
		{
			Storager.setInt(Defs.NextMarafonBonusIndex, indexBonus + 1, false);
		}
		else
		{
			Storager.setInt(Defs.NextMarafonBonusIndex, 0, false);
		}
		this._currentBonusIndex.Value = Storager.getInt(Defs.NextMarafonBonusIndex, false);
		Storager.setInt(Defs.NeedTakeMarathonBonus, 0, false);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	public void TakeMarafonBonus()
	{
		this.TakeBonusPlayer(this._currentBonusIndex.Value);
	}
}