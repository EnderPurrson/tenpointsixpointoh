using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TryGunScreenController : MonoBehaviour
{
	public GameObject buyPanel;

	public GameObject equipPanel;

	public GameObject backButton;

	public GameObject gemsPrice;

	public GameObject gemsPriceOld;

	public GameObject coinsPrice;

	public GameObject coinsPriceOld;

	public UITexture itemImage;

	public List<UILabel> itemNameLabels;

	public GameObject headSpecialOffer;

	public GameObject headExpired;

	public List<UILabel> numberOfMatchesLabels;

	public List<UILabel> discountLabels;

	public Action<string> onPurchaseCustomAction;

	public Action onEnterCoinsShopAdditionalAction;

	public Action onExitCoinsShopAdditionalAction;

	public Action<string> customEquipWearAction;

	private string _itemTag;

	private ShopNGUIController.CategoryNames category;

	private ItemPrice price;

	private ItemPrice priceWithoutPromo;

	private bool _expiredTryGun;

	private IDisposable _escapeSubscription;

	public bool ExpiredTryGun
	{
		get
		{
			return this._expiredTryGun;
		}
		set
		{
			bool flag;
			try
			{
				this._expiredTryGun = value;
				this.backButton.SetActive(value);
				this.buyPanel.SetActive(value);
				this.equipPanel.SetActive(!value);
				this.gemsPrice.SetActive((!value ? false : this.price.Currency == "GemsCurrency"));
				this.gemsPriceOld.SetActive((!value ? false : this.price.Currency == "GemsCurrency"));
				this.coinsPrice.SetActive((!value ? false : this.price.Currency == "Coins"));
				this.coinsPriceOld.SetActive((!value ? false : this.price.Currency == "Coins"));
				this.headSpecialOffer.SetActive(!value);
				this.headExpired.SetActive(value);
				if (!value)
				{
					int num = (!FriendsController.useBuffSystem ? KillRateCheck.instance.GetRoundsForGun() : BuffSystem.instance.GetRoundsForGun());
					foreach (UILabel numberOfMatchesLabel in this.numberOfMatchesLabels)
					{
						numberOfMatchesLabel.text = string.Format(LocalizationStore.Get("Key_1995"), num);
					}
				}
				else
				{
					if (this.price.Currency == "GemsCurrency")
					{
						this.gemsPrice.GetComponent<UILabel>().text = this.price.Price.ToString();
						this.gemsPriceOld.GetComponent<UILabel>().text = this.priceWithoutPromo.Price.ToString();
					}
					if (this.price.Currency == "Coins")
					{
						this.coinsPrice.GetComponent<UILabel>().text = this.price.Price.ToString();
						this.coinsPriceOld.GetComponent<UILabel>().text = this.priceWithoutPromo.Price.ToString();
					}
					try
					{
						foreach (UILabel discountLabel in this.discountLabels)
						{
							discountLabel.text = string.Format(LocalizationStore.Get("Key_1996"), Mathf.RoundToInt(WeaponManager.TryGunPromoDuration() / 60f), ShopNGUIController.DiscountFor(this.ItemTag, out flag));
						}
					}
					catch (Exception exception)
					{
						Debug.LogError(string.Concat("Exception in setting up discount in try gun screen: ", exception));
					}
					AnalyticsStuff.LogWEaponsSpecialOffers_Conversion(true, null);
				}
			}
			catch (Exception exception1)
			{
				Debug.LogError(string.Concat("Exception in ExpiredTryGun: ", exception1));
			}
		}
	}

	public string ItemTag
	{
		get
		{
			return this._itemTag;
		}
		set
		{
			try
			{
				this._itemTag = value;
				this.category = (ShopNGUIController.CategoryNames)PromoActionsGUIController.CatForTg(this._itemTag);
				this.price = ShopNGUIController.currentPrice(this._itemTag, this.category, false, true);
				this.priceWithoutPromo = ShopNGUIController.currentPrice(this._itemTag, this.category, false, false);
				string str = PromoActionsGUIController.IconNameForKey(this._itemTag, (int)this.category);
				Texture texture = Resources.Load<Texture>(string.Concat("OfferIcons/", str));
				if (texture != null && this.itemImage != null)
				{
					this.itemImage.mainTexture = texture;
				}
				string itemNameByTag = ItemDb.GetItemNameByTag(this._itemTag);
				foreach (UILabel itemNameLabel in this.itemNameLabels)
				{
					itemNameLabel.text = itemNameByTag;
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in ItemTag: ", exception));
			}
		}
	}

	public TryGunScreenController()
	{
	}

	private void DestroyScreen()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void HandleBuy()
	{
		Action<string> action = null;
		int price = this.price.Price;
		string currency = this.price.Currency;
		ShopNGUIController.TryToBuy(base.gameObject, this.price, () => {
			!Defs.isSoundFX;
			ShopNGUIController.FireWeaponOrArmorBought();
			ShopNGUIController.CategoryNames u003cu003ef_this = this.category;
			string itemTag = this.ItemTag;
			if (action == null)
			{
				action = (string item) => {
					if (ShopNGUIController.sharedShop != null)
					{
						ShopNGUIController.sharedShop.FireBuyAction(item);
					}
				};
			}
			ShopNGUIController.ProvideShopItemOnStarterPackBoguht(u003cu003ef_this, itemTag, 1, false, 0, action, null, true, true, true);
			try
			{
				string empty = string.Empty;
				string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(this.ItemTag) ?? WeaponManager.FirstUnboughtTag(this.ItemTag), empty, this.category, null);
				FlurryPluginWrapper.LogPurchaseByModes(this.category, itemNameNonLocalized, 1, false);
				if (this.category != ShopNGUIController.CategoryNames.GearCategory)
				{
					FlurryPluginWrapper.LogPurchaseByPoints(this.category, itemNameNonLocalized, 1);
					FlurryPluginWrapper.LogPurchasesPoints(ShopNGUIController.IsWeaponCategory(this.category));
				}
				bool flag = false;
				if (ShopNGUIController.IsWeaponCategory(this.category))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(this.ItemTag);
					flag = (weaponInfo == null ? false : weaponInfo.IsAvalibleFromFilter(3));
				}
				string str = (!FlurryEvents.shopCategoryToLogSalesNamesMapping.ContainsKey(this.category) ? this.category.ToString() : FlurryEvents.shopCategoryToLogSalesNamesMapping[this.category]);
				AnalyticsStuff.LogSales(itemNameNonLocalized, str, flag);
				AnalyticsFacade.InAppPurchase(itemNameNonLocalized, str, 1, price, currency);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in loggin in Try Gun Screen Controller: ", exception));
			}
			this.DestroyScreen();
		}, null, null, null, () => {
		}, () => {
		});
	}

	public void HandleClose()
	{
		this.DestroyScreen();
	}

	public void HandleEquip()
	{
		try
		{
			WeaponManager.sharedManager.AddTryGun(this.ItemTag);
			if (!FriendsController.useBuffSystem)
			{
				KillRateCheck.instance.SetGetWeapon();
			}
			else
			{
				BuffSystem.instance.SetGetTryGun(ItemDb.GetByTag(this.ItemTag).PrefabName);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("TryGunScreenController HandleEquip exception: ", exception));
		}
	}

	private void OnDestroy()
	{
		this._escapeSubscription.Dispose();
	}

	private void Start()
	{
		this._escapeSubscription = BackSystem.Instance.Register(() => {
			if (!this.ExpiredTryGun)
			{
				this.HandleEquip();
			}
			this.HandleClose();
		}, "Try Gun Screen");
	}
}