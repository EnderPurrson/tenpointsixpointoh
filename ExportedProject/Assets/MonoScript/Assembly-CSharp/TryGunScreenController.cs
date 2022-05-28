using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class TryGunScreenController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHandleBuy_003Ec__AnonStorey356
	{
		internal int priceAmount;

		internal string priceCurrency;

		internal TryGunScreenController _003C_003Ef__this;

		private static Action<string> _003C_003Ef__am_0024cache3;

		internal void _003C_003Em__592()
		{
			if (Defs.isSoundFX)
			{
			}
			ShopNGUIController.FireWeaponOrArmorBought();
			ShopNGUIController.CategoryNames category = _003C_003Ef__this.category;
			string itemTag = _003C_003Ef__this.ItemTag;
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003C_003Em__596;
			}
			ShopNGUIController.ProvideShopItemOnStarterPackBoguht(category, itemTag, 1, false, 0, _003C_003Ef__am_0024cache3);
			try
			{
				string empty = string.Empty;
				string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(_003C_003Ef__this.ItemTag) ?? WeaponManager.FirstUnboughtTag(_003C_003Ef__this.ItemTag), empty, _003C_003Ef__this.category);
				FlurryPluginWrapper.LogPurchaseByModes(_003C_003Ef__this.category, itemNameNonLocalized, 1, false);
				if (_003C_003Ef__this.category != ShopNGUIController.CategoryNames.GearCategory)
				{
					FlurryPluginWrapper.LogPurchaseByPoints(_003C_003Ef__this.category, itemNameNonLocalized, 1);
					FlurryPluginWrapper.LogPurchasesPoints(ShopNGUIController.IsWeaponCategory(_003C_003Ef__this.category));
				}
				bool isDaterWeapon = false;
				if (ShopNGUIController.IsWeaponCategory(_003C_003Ef__this.category))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(_003C_003Ef__this.ItemTag);
					isDaterWeapon = weaponInfo != null && weaponInfo.IsAvalibleFromFilter(3);
				}
				string text = ((!FlurryEvents.shopCategoryToLogSalesNamesMapping.ContainsKey(_003C_003Ef__this.category)) ? _003C_003Ef__this.category.ToString() : FlurryEvents.shopCategoryToLogSalesNamesMapping[_003C_003Ef__this.category]);
				AnalyticsStuff.LogSales(itemNameNonLocalized, text, isDaterWeapon);
				AnalyticsFacade.InAppPurchase(itemNameNonLocalized, text, 1, priceAmount, priceCurrency);
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in loggin in Try Gun Screen Controller: " + ex);
			}
			_003C_003Ef__this.DestroyScreen();
		}

		private static void _003C_003Em__596(string item)
		{
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.FireBuyAction(item);
			}
		}
	}

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

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache17;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache18;

	public bool ExpiredTryGun
	{
		get
		{
			return _expiredTryGun;
		}
		set
		{
			try
			{
				_expiredTryGun = value;
				backButton.SetActive(value);
				buyPanel.SetActive(value);
				equipPanel.SetActive(!value);
				gemsPrice.SetActive(value && price.Currency == "GemsCurrency");
				gemsPriceOld.SetActive(value && price.Currency == "GemsCurrency");
				coinsPrice.SetActive(value && price.Currency == "Coins");
				coinsPriceOld.SetActive(value && price.Currency == "Coins");
				headSpecialOffer.SetActive(!value);
				headExpired.SetActive(value);
				if (value)
				{
					if (price.Currency == "GemsCurrency")
					{
						gemsPrice.GetComponent<UILabel>().text = price.Price.ToString();
						gemsPriceOld.GetComponent<UILabel>().text = priceWithoutPromo.Price.ToString();
					}
					if (price.Currency == "Coins")
					{
						coinsPrice.GetComponent<UILabel>().text = price.Price.ToString();
						coinsPriceOld.GetComponent<UILabel>().text = priceWithoutPromo.Price.ToString();
					}
					try
					{
						foreach (UILabel discountLabel in discountLabels)
						{
							bool onlyServerDiscount;
							discountLabel.text = string.Format(LocalizationStore.Get("Key_1996"), Mathf.RoundToInt(WeaponManager.TryGunPromoDuration() / 60f), ShopNGUIController.DiscountFor(ItemTag, out onlyServerDiscount));
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("Exception in setting up discount in try gun screen: " + ex);
					}
					AnalyticsStuff.LogWEaponsSpecialOffers_Conversion(true);
					return;
				}
				int num = ((!FriendsController.useBuffSystem) ? KillRateCheck.instance.GetRoundsForGun() : BuffSystem.instance.GetRoundsForGun());
				foreach (UILabel numberOfMatchesLabel in numberOfMatchesLabels)
				{
					numberOfMatchesLabel.text = string.Format(LocalizationStore.Get("Key_1995"), num);
				}
			}
			catch (Exception ex2)
			{
				Debug.LogError("Exception in ExpiredTryGun: " + ex2);
			}
		}
	}

	public string ItemTag
	{
		get
		{
			return _itemTag;
		}
		set
		{
			try
			{
				_itemTag = value;
				category = (ShopNGUIController.CategoryNames)PromoActionsGUIController.CatForTg(_itemTag);
				price = ShopNGUIController.currentPrice(_itemTag, category);
				priceWithoutPromo = ShopNGUIController.currentPrice(_itemTag, category, false, false);
				string text = PromoActionsGUIController.IconNameForKey(_itemTag, (int)category);
				Texture texture = Resources.Load<Texture>("OfferIcons/" + text);
				if (texture != null && itemImage != null)
				{
					itemImage.mainTexture = texture;
				}
				string itemNameByTag = ItemDb.GetItemNameByTag(_itemTag);
				foreach (UILabel itemNameLabel in itemNameLabels)
				{
					itemNameLabel.text = itemNameByTag;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in ItemTag: " + ex);
			}
		}
	}

	public void HandleEquip()
	{
		try
		{
			WeaponManager.sharedManager.AddTryGun(ItemTag);
			if (FriendsController.useBuffSystem)
			{
				BuffSystem.instance.SetGetTryGun(ItemDb.GetByTag(ItemTag).PrefabName);
			}
			else
			{
				KillRateCheck.instance.SetGetWeapon();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("TryGunScreenController HandleEquip exception: " + ex);
		}
	}

	public void HandleClose()
	{
		DestroyScreen();
	}

	private void DestroyScreen()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void HandleBuy()
	{
		_003CHandleBuy_003Ec__AnonStorey356 _003CHandleBuy_003Ec__AnonStorey = new _003CHandleBuy_003Ec__AnonStorey356();
		_003CHandleBuy_003Ec__AnonStorey._003C_003Ef__this = this;
		_003CHandleBuy_003Ec__AnonStorey.priceAmount = price.Price;
		_003CHandleBuy_003Ec__AnonStorey.priceCurrency = price.Currency;
		GameObject mainPanel = base.gameObject;
		ItemPrice itemPrice = price;
		Action onSuccess = _003CHandleBuy_003Ec__AnonStorey._003C_003Em__592;
		if (_003C_003Ef__am_0024cache17 == null)
		{
			_003C_003Ef__am_0024cache17 = _003CHandleBuy_003Em__593;
		}
		Action onEnterCoinsShopAction = _003C_003Ef__am_0024cache17;
		if (_003C_003Ef__am_0024cache18 == null)
		{
			_003C_003Ef__am_0024cache18 = _003CHandleBuy_003Em__594;
		}
		ShopNGUIController.TryToBuy(mainPanel, itemPrice, onSuccess, null, null, null, onEnterCoinsShopAction, _003C_003Ef__am_0024cache18);
	}

	private void Start()
	{
		_escapeSubscription = BackSystem.Instance.Register(_003CStart_003Em__595, "Try Gun Screen");
	}

	private void OnDestroy()
	{
		_escapeSubscription.Dispose();
	}

	[CompilerGenerated]
	private static void _003CHandleBuy_003Em__593()
	{
	}

	[CompilerGenerated]
	private static void _003CHandleBuy_003Em__594()
	{
	}

	[CompilerGenerated]
	private void _003CStart_003Em__595()
	{
		if (!ExpiredTryGun)
		{
			HandleEquip();
		}
		HandleClose();
	}
}
