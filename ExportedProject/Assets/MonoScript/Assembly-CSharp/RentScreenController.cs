using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class RentScreenController : PropertyInfoScreenController
{
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

	public string Header
	{
		set
		{
			UILabel[] uILabelArray = this.header;
			for (int i = 0; i < (int)uILabelArray.Length; i++)
			{
				UILabel uILabel = uILabelArray[i];
				if (uILabel != null && value != null)
				{
					uILabel.text = value;
				}
			}
		}
	}

	public string ItemTag
	{
		set
		{
			this._itemTag = value;
			if (this._itemTag == null)
			{
				return;
			}
			int num = PromoActionsGUIController.CatForTg(this._itemTag);
			this.category = (ShopNGUIController.CategoryNames)num;
			string str = PromoActionsGUIController.IconNameForKey(this._itemTag, num);
			Texture texture = Resources.Load<Texture>(string.Concat("OfferIcons/", str));
			if (texture != null && this.itemImage != null)
			{
				this.itemImage.mainTexture = texture;
			}
			ItemRecord byTag = ItemDb.GetByTag(this._itemTag);
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null ? this._itemTag : byTag.ShopId));
			bool flag = (priceByShopId == null || priceByShopId.Currency == null ? false : priceByShopId.Currency.Equals("Coins"));
			UILabel[] uILabelArray = (!flag ? this.prices : this.pricesCoins);
			for (int i = 0; i < (int)uILabelArray.Length; i++)
			{
				UILabel uILabel = uILabelArray[i];
				if (uILabel != null)
				{
					uILabel.gameObject.SetActive(true);
					UILabel str1 = uILabel;
					int num1 = this.priceFormula(Array.IndexOf<UILabel>((!flag ? this.prices : this.pricesCoins), uILabel));
					str1.text = num1.ToString();
				}
			}
			UILabel[] uILabelArray1 = (!flag ? this.pricesCoins : this.prices);
			for (int j = 0; j < (int)uILabelArray1.Length; j++)
			{
				UILabel uILabel1 = uILabelArray1[j];
				if (uILabel1 != null)
				{
					uILabel1.gameObject.SetActive(false);
				}
			}
		}
	}

	private Func<int, int> priceFormula
	{
		get
		{
			return (int ind) => {
				int num = 10;
				if (this._itemTag != null)
				{
					ItemRecord byTag = ItemDb.GetByTag(this._itemTag);
					ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null ? this._itemTag : byTag.ShopId));
					if (priceByShopId != null)
					{
						if (-1 == -1)
						{
							num = Mathf.RoundToInt((float)priceByShopId.Price * TempItemsController.PriceCoefs[this._itemTag][ind]);
						}
					}
				}
				return num;
			};
		}
	}

	public string RentFor
	{
		set
		{
			UILabel[] uILabelArray = this.rentFor;
			for (int i = 0; i < (int)uILabelArray.Length; i++)
			{
				UILabel uILabel = uILabelArray[i];
				if (uILabel != null && value != null && this._itemTag != null)
				{
					uILabel.text = string.Format(value, ItemDb.GetItemNameByTag(this._itemTag));
				}
			}
		}
	}

	public RentScreenController()
	{
	}

	private void Awake()
	{
		this.rentButtonsPanel.SetActive(false);
		this.viewButtonPanel.SetActive(true);
	}

	public void HandleRentButton(UIButton b)
	{
		Action<string> action1 = null;
		if (Defs.isSoundFX)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		int num3 = Array.IndexOf<UIButton>(this.buttons, b);
		ItemRecord byTag = ItemDb.GetByTag(this._itemTag);
		ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null ? this._itemTag : byTag.ShopId));
		ItemPrice itemPrice = new ItemPrice(this.priceFormula(num3), (priceByShopId == null ? "GemsCurrency" : priceByShopId.Currency));
		bool flag1 = (TempItemsController.sharedController == null ? false : TempItemsController.sharedController.ContainsItem(this._itemTag));
		ShopNGUIController.TryToBuy(this.window, itemPrice, () => {
			ShopNGUIController.CategoryNames u003cu003ef_this = this.category;
			string str = this._itemTag;
			int num = num3;
			if (action1 == null)
			{
				action1 = (string item) => {
					if (ShopNGUIController.sharedShop != null)
					{
						ShopNGUIController.sharedShop.FireBuyAction(item);
					}
				};
			}
			ShopNGUIController.ProvideShopItemOnStarterPackBoguht(u003cu003ef_this, str, 1, false, num, action1, this.customEquipWearAction, true, true, false);
			bool flag = Wear.armorNumTemp.ContainsKey(this._itemTag ?? string.Empty);
			string str1 = (!flag ? ((flag || this._itemTag == null ? true : (this._itemTag.Equals(WeaponTags.DragonGunRent_Tag) || this._itemTag.Equals(WeaponTags.PumpkinGunRent_Tag) || this._itemTag.Equals(WeaponTags.RayMinigunRent_Tag) || this._itemTag.Equals(WeaponTags.Red_StoneRent_Tag) ? false : !this._itemTag.Equals(WeaponTags.TwoBoltersRent_Tag))) ? "Time Weapons {0}" : "Time Weapons (red test) {0}") : "Time Armor and Hat {0}");
			string str2 = string.Format(str1, "Total");
			string[] strArrays = new string[] { "1", "3", "7" };
			string str3 = string.Concat(string.Concat(((int)strArrays.Length <= num3 || num3 < 0 ? string.Empty : strArrays[num3]), " day - "), this._itemTag ?? string.Empty, (!flag1 ? " - First Purchase" : string.Empty));
			Dictionary<string, string> strs1 = new Dictionary<string, string>()
			{
				{ "Levels", ((ExperienceController.sharedController == null ? 0 : ExperienceController.sharedController.currentLevel)).ToString() },
				{ "Tiers", ((ExpController.Instance == null ? 0 : ExpController.Instance.OurTier) + 1).ToString() },
				{ "Time Limits", str3 }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str2, strs1, true);
			FlurryPluginWrapper.LogEventAndDublicateToConsole(string.Format(str1, FlurryPluginWrapper.GetPayingSuffixNo10()), strs1, true);
			int price = itemPrice.Price;
			int num1 = price / 10;
			int num2 = price % 10;
			string str4 = (itemPrice.Currency == null || !itemPrice.Currency.Equals("GemsCurrency") ? (!flag ? "Purchase for Coins {0}" : "Purchase for Coins TempArmor {0}") : (!flag ? "Purchase for Gems {0}" : "Purchase for Gems TempArmor {0}"));
			string str5 = string.Format(str4, "Total");
			string str6 = string.Format(str4, FlurryPluginWrapper.GetPayingSuffixNo10());
			Action<int> action = (int baseNumber) => {
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ string.Concat("Levels_", baseNumber.ToString()), ((ExperienceController.sharedController == null ? 0 : ExperienceController.sharedController.currentLevel)).ToString() },
					{ string.Concat("Tiers_", baseNumber.ToString()), ((ExpController.Instance == null ? 0 : ExpController.Instance.OurTier) + 1).ToString() }
				};
				FlurryPluginWrapper.LogEventAndDublicateToConsole(str5, strs, true);
				FlurryPluginWrapper.LogEventAndDublicateToConsole(str6, strs, true);
			};
			for (int i = 0; i < num1; i++)
			{
				action(10);
			}
			for (int j = 0; j < num2; j++)
			{
				action(1);
			}
			Action<string> u003cu003ef_this1 = this.onPurchaseCustomAction;
			if (u003cu003ef_this1 != null)
			{
				u003cu003ef_this1(this._itemTag);
			}
			if (TempItemsController.sharedController != null)
			{
				TempItemsController.sharedController.ExpiredItems.Remove(this._itemTag);
			}
			this.Hide();
		}, null, null, null, () => {
			Action u003cu003ef_this = this.onEnterCoinsShopAdditionalAction;
			if (u003cu003ef_this != null)
			{
				u003cu003ef_this();
			}
		}, () => {
			Action u003cu003ef_this = this.onExitCoinsShopAdditionalAction;
			if (u003cu003ef_this != null)
			{
				u003cu003ef_this();
			}
		});
	}

	public void HandleViewButton()
	{
		this.Hide();
		if (this._itemTag != null && TempItemsController.GunsMappingFromTempToConst.ContainsKey(this._itemTag))
		{
			string str = WeaponManager.FirstUnboughtOrForOurTier(TempItemsController.GunsMappingFromTempToConst[this._itemTag]);
			if (str != null)
			{
				int num = PromoActionsGUIController.CatForTg(str);
				if (num != -1)
				{
					ShopNGUIController.GoToShop((ShopNGUIController.CategoryNames)num, str);
				}
			}
		}
	}

	public override void Hide()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		RentScreenController.SetDepthForExpGUI(99);
	}

	public static void SetDepthForExpGUI(int newDepth)
	{
		ExpController instance = ExpController.Instance;
		if (instance != null)
		{
			instance.experienceView.experienceCamera.depth = (float)newDepth;
		}
	}

	private void Start()
	{
		RentScreenController.SetDepthForExpGUI(89);
	}
}