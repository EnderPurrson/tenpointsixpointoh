using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class RentScreenController : PropertyInfoScreenController
{
	[CompilerGenerated]
	private sealed class _003CHandleRentButton_003Ec__AnonStorey293
	{
		private sealed class _003CHandleRentButton_003Ec__AnonStorey294
		{
			internal string totalEvent;

			internal string totalPayingEvent;

			internal _003CHandleRentButton_003Ec__AnonStorey293 _003C_003Ef__ref_0024659;

			internal void _003C_003Em__238(int baseNumber)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>
				{
					{
						"Levels_" + baseNumber,
						((ExperienceController.sharedController != null) ? ExperienceController.sharedController.currentLevel : 0).ToString()
					},
					{
						"Tiers_" + baseNumber,
						(((ExpController.Instance != null) ? ExpController.Instance.OurTier : 0) + 1).ToString()
					}
				};
				FlurryPluginWrapper.LogEventAndDublicateToConsole(totalEvent, parameters);
				FlurryPluginWrapper.LogEventAndDublicateToConsole(totalPayingEvent, parameters);
			}
		}

		internal int ind;

		internal bool hasBefore;

		internal ItemPrice price;

		internal RentScreenController _003C_003Ef__this;

		private static Action<string> _003C_003Ef__am_0024cache4;

		internal void _003C_003Em__234()
		{
			ShopNGUIController.CategoryNames category = _003C_003Ef__this.category;
			string itemTag = _003C_003Ef__this._itemTag;
			int timeForRentIndex = ind;
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003C_003Em__237;
			}
			ShopNGUIController.ProvideShopItemOnStarterPackBoguht(category, itemTag, 1, false, timeForRentIndex, _003C_003Ef__am_0024cache4, _003C_003Ef__this.customEquipWearAction, true, true, false);
			bool flag = Wear.armorNumTemp.ContainsKey(_003C_003Ef__this._itemTag ?? string.Empty);
			bool flag2 = !flag && _003C_003Ef__this._itemTag != null && (_003C_003Ef__this._itemTag.Equals(WeaponTags.DragonGunRent_Tag) || _003C_003Ef__this._itemTag.Equals(WeaponTags.PumpkinGunRent_Tag) || _003C_003Ef__this._itemTag.Equals(WeaponTags.RayMinigunRent_Tag) || _003C_003Ef__this._itemTag.Equals(WeaponTags.Red_StoneRent_Tag) || _003C_003Ef__this._itemTag.Equals(WeaponTags.TwoBoltersRent_Tag));
			string format = (flag ? "Time Armor and Hat {0}" : ((!flag2) ? "Time Weapons {0}" : "Time Weapons (red test) {0}"));
			string eventName = string.Format(format, "Total");
			string[] array = new string[3] { "1", "3", "7" };
			string text = ((array.Length <= ind || ind < 0) ? string.Empty : array[ind]) + " day - ";
			string value = text + (_003C_003Ef__this._itemTag ?? string.Empty) + ((!hasBefore) ? " - First Purchase" : string.Empty);
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{
					"Levels",
					((ExperienceController.sharedController != null) ? ExperienceController.sharedController.currentLevel : 0).ToString()
				},
				{
					"Tiers",
					(((ExpController.Instance != null) ? ExpController.Instance.OurTier : 0) + 1).ToString()
				},
				{ "Time Limits", value }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, parameters);
			string payingSuffixNo = FlurryPluginWrapper.GetPayingSuffixNo10();
			string eventName2 = string.Format(format, payingSuffixNo);
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName2, parameters);
			_003CHandleRentButton_003Ec__AnonStorey294 _003CHandleRentButton_003Ec__AnonStorey = new _003CHandleRentButton_003Ec__AnonStorey294();
			_003CHandleRentButton_003Ec__AnonStorey._003C_003Ef__ref_0024659 = this;
			int num = price.Price;
			int num2 = num / 10;
			int num3 = num % 10;
			string format2 = ((price.Currency != null && price.Currency.Equals("GemsCurrency")) ? ((!flag) ? "Purchase for Gems {0}" : "Purchase for Gems TempArmor {0}") : ((!flag) ? "Purchase for Coins {0}" : "Purchase for Coins TempArmor {0}"));
			_003CHandleRentButton_003Ec__AnonStorey.totalEvent = string.Format(format2, "Total");
			string payingSuffixNo2 = FlurryPluginWrapper.GetPayingSuffixNo10();
			_003CHandleRentButton_003Ec__AnonStorey.totalPayingEvent = string.Format(format2, payingSuffixNo2);
			Action<int> action = _003CHandleRentButton_003Ec__AnonStorey._003C_003Em__238;
			for (int i = 0; i < num2; i++)
			{
				action(10);
			}
			for (int j = 0; j < num3; j++)
			{
				action(1);
			}
			Action<string> onPurchaseCustomAction = _003C_003Ef__this.onPurchaseCustomAction;
			if (onPurchaseCustomAction != null)
			{
				onPurchaseCustomAction(_003C_003Ef__this._itemTag);
			}
			if (TempItemsController.sharedController != null)
			{
				TempItemsController.sharedController.ExpiredItems.Remove(_003C_003Ef__this._itemTag);
			}
			_003C_003Ef__this.Hide();
		}

		internal void _003C_003Em__235()
		{
			Action onEnterCoinsShopAdditionalAction = _003C_003Ef__this.onEnterCoinsShopAdditionalAction;
			if (onEnterCoinsShopAdditionalAction != null)
			{
				onEnterCoinsShopAdditionalAction();
			}
		}

		internal void _003C_003Em__236()
		{
			Action onExitCoinsShopAdditionalAction = _003C_003Ef__this.onExitCoinsShopAdditionalAction;
			if (onExitCoinsShopAdditionalAction != null)
			{
				onExitCoinsShopAdditionalAction();
			}
		}

		private static void _003C_003Em__237(string item)
		{
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.FireBuyAction(item);
			}
		}
	}

	public GameObject viewButtonPanel;

	public GameObject rentButtonsPanel;

	public UIButton viewButton;

	public GameObject window;

	public UILabel[] header;

	public UILabel[] rentFor;

	public UILabel[] prices;

	public UILabel[] pricesCoins;

	public UIButton[] buttons;

	public UITexture itemImage;

	public Action<string> onPurchaseCustomAction;

	public Action onEnterCoinsShopAdditionalAction;

	public Action onExitCoinsShopAdditionalAction;

	public Action<string> customEquipWearAction;

	private string _itemTag;

	private ShopNGUIController.CategoryNames category;

	private Func<int, int> priceFormula
	{
		get
		{
			return _003Cget_priceFormula_003Em__233;
		}
	}

	public string Header
	{
		set
		{
			UILabel[] array = header;
			foreach (UILabel uILabel in array)
			{
				if (uILabel != null && value != null)
				{
					uILabel.text = value;
				}
			}
		}
	}

	public string RentFor
	{
		set
		{
			UILabel[] array = rentFor;
			foreach (UILabel uILabel in array)
			{
				if (uILabel != null && value != null && _itemTag != null)
				{
					uILabel.text = string.Format(value, ItemDb.GetItemNameByTag(_itemTag));
				}
			}
		}
	}

	public string ItemTag
	{
		set
		{
			_itemTag = value;
			if (_itemTag == null)
			{
				return;
			}
			string text = PromoActionsGUIController.IconNameForKey(cat: (int)(category = (ShopNGUIController.CategoryNames)PromoActionsGUIController.CatForTg(_itemTag)), key: _itemTag);
			Texture texture = Resources.Load<Texture>("OfferIcons/" + text);
			if (texture != null && itemImage != null)
			{
				itemImage.mainTexture = texture;
			}
			ItemRecord byTag = ItemDb.GetByTag(_itemTag);
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null) ? _itemTag : byTag.ShopId);
			bool flag = priceByShopId != null && priceByShopId.Currency != null && priceByShopId.Currency.Equals("Coins");
			UILabel[] array = ((!flag) ? prices : pricesCoins);
			foreach (UILabel uILabel in array)
			{
				if (uILabel != null)
				{
					uILabel.gameObject.SetActive(true);
					uILabel.text = priceFormula(Array.IndexOf((!flag) ? prices : pricesCoins, uILabel)).ToString();
				}
			}
			UILabel[] array2 = ((!flag) ? pricesCoins : prices);
			foreach (UILabel uILabel2 in array2)
			{
				if (uILabel2 != null)
				{
					uILabel2.gameObject.SetActive(false);
				}
			}
		}
	}

	public override void Hide()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void HandleRentButton(UIButton b)
	{
		_003CHandleRentButton_003Ec__AnonStorey293 _003CHandleRentButton_003Ec__AnonStorey = new _003CHandleRentButton_003Ec__AnonStorey293();
		_003CHandleRentButton_003Ec__AnonStorey._003C_003Ef__this = this;
		if (Defs.isSoundFX)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		_003CHandleRentButton_003Ec__AnonStorey.ind = Array.IndexOf(buttons, b);
		ItemRecord byTag = ItemDb.GetByTag(_itemTag);
		ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null) ? _itemTag : byTag.ShopId);
		_003CHandleRentButton_003Ec__AnonStorey.price = new ItemPrice(priceFormula(_003CHandleRentButton_003Ec__AnonStorey.ind), (priceByShopId == null) ? "GemsCurrency" : priceByShopId.Currency);
		_003CHandleRentButton_003Ec__AnonStorey.hasBefore = TempItemsController.sharedController != null && TempItemsController.sharedController.ContainsItem(_itemTag);
		ShopNGUIController.TryToBuy(window, _003CHandleRentButton_003Ec__AnonStorey.price, _003CHandleRentButton_003Ec__AnonStorey._003C_003Em__234, null, null, null, _003CHandleRentButton_003Ec__AnonStorey._003C_003Em__235, _003CHandleRentButton_003Ec__AnonStorey._003C_003Em__236);
	}

	public void HandleViewButton()
	{
		Hide();
		if (_itemTag == null || !TempItemsController.GunsMappingFromTempToConst.ContainsKey(_itemTag))
		{
			return;
		}
		string text = WeaponManager.FirstUnboughtOrForOurTier(TempItemsController.GunsMappingFromTempToConst[_itemTag]);
		if (text != null)
		{
			int num = PromoActionsGUIController.CatForTg(text);
			if (num != -1)
			{
				ShopNGUIController.GoToShop((ShopNGUIController.CategoryNames)num, text);
			}
		}
	}

	private void Awake()
	{
		rentButtonsPanel.SetActive(false);
		viewButtonPanel.SetActive(true);
	}

	public static void SetDepthForExpGUI(int newDepth)
	{
		ExpController instance = ExpController.Instance;
		if (instance != null)
		{
			instance.experienceView.experienceCamera.depth = newDepth;
		}
	}

	private void Start()
	{
		SetDepthForExpGUI(89);
	}

	private void OnDestroy()
	{
		SetDepthForExpGUI(99);
	}

	[CompilerGenerated]
	private int _003Cget_priceFormula_003Em__233(int ind)
	{
		int result = 10;
		if (_itemTag != null)
		{
			ItemRecord byTag = ItemDb.GetByTag(_itemTag);
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null) ? _itemTag : byTag.ShopId);
			if (priceByShopId != null)
			{
				int num = -1;
				if (num == -1)
				{
					result = Mathf.RoundToInt((float)priceByShopId.Price * TempItemsController.PriceCoefs[_itemTag][ind]);
				}
			}
		}
		return result;
	}
}
