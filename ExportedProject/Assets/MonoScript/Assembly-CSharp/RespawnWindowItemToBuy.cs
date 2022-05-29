using I2.Loc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class RespawnWindowItemToBuy : MonoBehaviour
{
	public UITexture itemImage;

	public UILabel itemNameLabel;

	public GameObject itemPriceBtnBuyContainer;

	public UILabel needTierLabel;

	public UISprite itemPriceIcon;

	public UILabel itemPriceLabel;

	public UIButton btnBuy;

	[NonSerialized]
	public string itemTag;

	[NonSerialized]
	public int itemCategory;

	[NonSerialized]
	public ItemPrice itemPrice;

	[NonSerialized]
	public string nonLocalizedName;

	private static Color priceGemColor;

	private static Color priceCoinColor;

	static RespawnWindowItemToBuy()
	{
		RespawnWindowItemToBuy.priceGemColor = new Color32(100, 230, 255, 255);
		RespawnWindowItemToBuy.priceCoinColor = new Color32(255, 255, 0, 255);
	}

	public RespawnWindowItemToBuy()
	{
	}

	public static string GetItemName(string itemTag, ShopNGUIController.CategoryNames itemCategory, int upgradeNum = 0)
	{
		int num;
		string str = itemTag;
		if (str != null)
		{
			if (RespawnWindowItemToBuy.u003cu003ef__switchu0024map12 == null)
			{
				RespawnWindowItemToBuy.u003cu003ef__switchu0024map12 = new Dictionary<string, int>(1)
				{
					{ "LikeID", 0 }
				};
			}
			if (RespawnWindowItemToBuy.u003cu003ef__switchu0024map12.TryGetValue(str, out num))
			{
				if (num == 0)
				{
					return ScriptLocalization.Get("Key_1796");
				}
			}
		}
		if (GearManager.IsItemGear(itemTag))
		{
			upgradeNum = Mathf.Min(upgradeNum, GearManager.NumOfGearUpgrades);
			itemTag = GearManager.UpgradeIDForGear(itemTag, upgradeNum);
		}
		return ItemDb.GetItemName(itemTag, itemCategory);
	}

	public static string GetItemNonLocalizedName(string itemTag, ShopNGUIController.CategoryNames itemCategory, int upgradeNum = 0)
	{
		string shopIdByTag = itemTag;
		if (ShopNGUIController.IsWeaponCategory(itemCategory))
		{
			shopIdByTag = ItemDb.GetShopIdByTag(itemTag);
		}
		if (GearManager.IsItemGear(itemTag))
		{
			upgradeNum = Mathf.Min(upgradeNum, GearManager.NumOfGearUpgrades);
			shopIdByTag = GearManager.UpgradeIDForGear(itemTag, upgradeNum);
		}
		return ItemDb.GetItemNameNonLocalized(itemTag, shopIdByTag, itemCategory, itemTag);
	}

	private static bool IsCanBuy(string itemTag, ShopNGUIController.CategoryNames itemCategory)
	{
		bool flag;
		if (!ShopNGUIController.IsWeaponCategory(itemCategory))
		{
			if (GearManager.IsItemGear(itemTag))
			{
				return false;
			}
			if (itemCategory != ShopNGUIController.CategoryNames.ArmorCategory)
			{
				return false;
			}
			return (ItemDb.IsItemInInventory(itemTag) ? false : !TempItemsController.PriceCoefs.ContainsKey(itemTag));
		}
		bool flag1 = (!ItemDb.IsCanBuy(itemTag) ? false : !ItemDb.IsTemporaryGun(itemTag));
		bool flag2 = ItemDb.IsItemInInventory(itemTag);
		bool flag3 = ItemDb.HasWeaponNeedUpgradesForBuyNext(itemTag);
		string str = WeaponManager.FirstTagForOurTier(itemTag);
		if (!flag1 || flag2)
		{
			flag = false;
		}
		else if (flag3)
		{
			flag = true;
		}
		else
		{
			flag = (str == null ? false : str.Equals(itemTag));
		}
		return flag;
	}

	public void Reset()
	{
		this.itemImage.mainTexture = null;
		this.itemTag = string.Empty;
		this.nonLocalizedName = string.Empty;
	}

	private static void SetPrice(UISprite priceIcon, UILabel priceLabel, ItemPrice price)
	{
		bool currency = price.Currency == "GemsCurrency";
		priceIcon.spriteName = (!currency ? "ingame_coin" : "gem_znachek");
		priceIcon.width = (!currency ? 30 : 24);
		priceIcon.height = (!currency ? 30 : 24);
		priceLabel.text = price.Price.ToString();
		priceLabel.color = (!currency ? RespawnWindowItemToBuy.priceCoinColor : RespawnWindowItemToBuy.priceGemColor);
	}

	public void SetWeaponTag(string itemTag, int? upgradeNum = null)
	{
		if (string.IsNullOrEmpty(itemTag))
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		int itemCategory = ItemDb.GetItemCategory(itemTag);
		ShopNGUIController.CategoryNames categoryName = (ShopNGUIController.CategoryNames)itemCategory;
		this.itemTag = itemTag;
		this.itemCategory = itemCategory;
		this.itemPrice = null;
		this.itemImage.mainTexture = ItemDb.GetItemIcon(itemTag, categoryName, upgradeNum);
		this.itemNameLabel.text = RespawnWindowItemToBuy.GetItemName(itemTag, categoryName, (!upgradeNum.HasValue ? 0 : upgradeNum.Value));
		this.nonLocalizedName = RespawnWindowItemToBuy.GetItemNonLocalizedName(itemTag, categoryName, (!upgradeNum.HasValue ? 0 : upgradeNum.Value));
		if (RespawnWindowItemToBuy.IsCanBuy(itemTag, categoryName))
		{
			this.itemPriceBtnBuyContainer.SetActive(true);
			this.needTierLabel.gameObject.SetActive(false);
			if (ShopNGUIController.IsWeaponCategory(categoryName))
			{
				int weaponInfo = ItemDb.GetWeaponInfo(itemTag).tier;
				if ((ExpController.Instance == null ? false : ExpController.Instance.OurTier < weaponInfo))
				{
					this.itemPriceBtnBuyContainer.SetActive(false);
					this.needTierLabel.gameObject.SetActive(true);
					int num = (weaponInfo < 0 || weaponInfo >= (int)ExpController.LevelsForTiers.Length ? ExpController.LevelsForTiers[(int)ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[weaponInfo]);
					string str = string.Format("{0} {1} {2}", LocalizationStore.Key_0226, num, LocalizationStore.Get("Key_1022"));
					this.needTierLabel.text = str;
				}
			}
			this.itemPrice = ShopNGUIController.currentPrice(itemTag, categoryName, false, true);
			RespawnWindowItemToBuy.SetPrice(this.itemPriceIcon, this.itemPriceLabel, this.itemPrice);
		}
		else
		{
			this.itemPriceBtnBuyContainer.gameObject.SetActive(false);
			this.needTierLabel.gameObject.SetActive(false);
		}
	}
}