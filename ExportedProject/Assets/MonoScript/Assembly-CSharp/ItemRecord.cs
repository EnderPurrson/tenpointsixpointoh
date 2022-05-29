using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemRecord
{
	private readonly SaltedInt _mainPrice;

	private readonly SaltedInt _alternativePrice;

	private readonly static System.Random _prng;

	private List<ItemPrice> _pricesForDifferentTiers;

	protected string alternativeCurrency
	{
		get;
		set;
	}

	protected int alternativePrice
	{
		get
		{
			return this._alternativePrice.Value;
		}
	}

	public bool CanBuy
	{
		get;
		private set;
	}

	public bool Deactivated
	{
		get;
		private set;
	}

	public int Id
	{
		get;
		private set;
	}

	protected string mainCurrency
	{
		get;
		set;
	}

	private int mainPrice
	{
		get
		{
			return this._mainPrice.Value;
		}
	}

	public string PrefabName
	{
		get;
		private set;
	}

	public ItemPrice Price
	{
		get
		{
			ItemPrice itemPrice;
			if (Defs2.GunPricesFromServer != null && Defs2.GunPricesFromServer.TryGetValue(this.PrefabName, out itemPrice) && itemPrice != null)
			{
				return itemPrice;
			}
			if (this._pricesForDifferentTiers != null)
			{
				return this.PriceForTierForThisItem;
			}
			if (this.alternativePrice == -1)
			{
				return new ItemPrice(this.mainPrice, this.mainCurrency);
			}
			if (WeaponManager.AllWrapperPrefabs().Find((WeaponSounds w) => (this.PrefabName == null ? false : w.name == this.PrefabName)) == null)
			{
				return new ItemPrice(this.mainPrice, this.mainCurrency);
			}
			int num = (!this.mainCurrency.Equals("GemsCurrency") ? this.alternativePrice : this.mainPrice);
			int num1 = (!this.mainCurrency.Equals("Coins") ? this.alternativePrice : this.mainPrice);
			return new ItemPrice(num, "GemsCurrency");
		}
	}

	private ItemPrice PriceForTierForThisItem
	{
		get
		{
			List<string> strs = WeaponUpgrades.ChainForTag(this.Tag);
			if (strs != null)
			{
				string str = WeaponManager.FirstTagForOurTier(this.Tag);
				if (str != null)
				{
					int num = strs.IndexOf(str);
					if (num >= 0 && this._pricesForDifferentTiers.Count > num)
					{
						return this._pricesForDifferentTiers[num];
					}
				}
			}
			Debug.LogWarning(string.Concat("Error in PriceForTierForThisItem: tag = ", this.Tag ?? "null", "  PrefabName: ", this.PrefabName ?? "null"));
			return new ItemPrice(100, "Coins");
		}
	}

	public string ShopDisplayName
	{
		get;
		private set;
	}

	public string ShopId
	{
		get;
		private set;
	}

	public string StorageId
	{
		get;
		private set;
	}

	public string Tag
	{
		get;
		private set;
	}

	public bool TemporaryGun
	{
		get
		{
			return (this.ShopId == null ? false : this.StorageId == null);
		}
	}

	public bool UseImagesFromFirstUpgrade
	{
		get;
		private set;
	}

	static ItemRecord()
	{
		ItemRecord._prng = new System.Random(1879142401);
	}

	public ItemRecord(int id, string tag, string storageId, string prefabName, string shopId, string shopDisplayName, bool canBuy, bool deactivated, List<ItemPrice> pricesForDiffTiers, bool useImageOfFirstUpgrade = false)
	{
		this.SetMainFields(id, tag, storageId, prefabName, shopId, shopDisplayName, canBuy, deactivated, useImageOfFirstUpgrade);
		this._pricesForDifferentTiers = pricesForDiffTiers;
		if (this._pricesForDifferentTiers == null || this._pricesForDifferentTiers.Count < 3)
		{
			Debug.LogError("ItemRecord: _pricesForDifferentTiers is null, or Count < 3!");
		}
	}

	public ItemRecord(int id, string tag, string storageId, string prefabName, string shopId, string shopDisplayName, int price, bool canBuy, bool deactivated, string currency = "Coins", int secondCurrencyPrice = -1, bool useImageOfFirstUpgrade = false)
	{
		this.SetMainFields(id, tag, storageId, prefabName, shopId, shopDisplayName, canBuy, deactivated, useImageOfFirstUpgrade);
		this._mainPrice = new SaltedInt(ItemRecord._prng.Next(), price);
		this._alternativePrice = new SaltedInt(ItemRecord._prng.Next(), secondCurrencyPrice);
		this.mainCurrency = currency;
		this.alternativeCurrency = (!this.mainCurrency.Equals("Coins") ? "Coins" : "GemsCurrency");
	}

	private void SetMainFields(int id, string tag, string storageId, string prefabName, string shopId, string shopDisplayName, bool canBuy, bool deactivated, bool useImageOfFirstUpgrade)
	{
		this.Id = id;
		this.Tag = tag;
		this.StorageId = storageId;
		this.PrefabName = prefabName;
		this.ShopId = shopId;
		this.ShopDisplayName = shopDisplayName;
		this.CanBuy = canBuy;
		this.Deactivated = deactivated;
		this.UseImagesFromFirstUpgrade = useImageOfFirstUpgrade;
	}
}