using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class RespawnWindow : MonoBehaviour
{
	public UILabel killerLevelNicknameLabel;

	public UITexture killerRank;

	public UILabel killerClanNameLabel;

	public UITexture killerClanLogo;

	public UILabel autoRespawnTitleLabel;

	public UILabel autoRespawnTimerLabel;

	public GameObject characterViewHolder;

	public Camera characterViewCamera;

	public UITexture characterViewTexture;

	public CharacterView characterView;

	public RespawnWindowItemToBuy killerWeapon;

	public RespawnWindowItemToBuy recommendedWeapon;

	public RespawnWindowItemToBuy recommendedArmor;

	public GameObject coinsShopButton;

	public GameObject armorObj;

	public GameObject healthIcon;

	public GameObject mechIcon;

	public GameObject healthBackground;

	public GameObject healtharmorBackground;

	public UITable healthTable;

	public RespawnWindowEquipmentItem hatItem;

	public RespawnWindowEquipmentItem maskItem;

	public RespawnWindowEquipmentItem armorItem;

	public RespawnWindowEquipmentItem capeItem;

	public RespawnWindowEquipmentItem bootsItem;

	public UILabel armorCountLabel;

	public UILabel healthCountLabel;

	public GameObject characterDrag;

	public GameObject cameraDrag;

	private static RespawnWindow _instance;

	private float _originalTimeout;

	private float _remained;

	[NonSerialized]
	public RenderTexture respawnWindowRT;

	public static RespawnWindow Instance
	{
		get
		{
			return RespawnWindow._instance;
		}
	}

	public bool isShown
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	static RespawnWindow()
	{
	}

	public RespawnWindow()
	{
	}

	[DebuggerHidden]
	[Obfuscation(Exclude=true)]
	public IEnumerator CloseAfterDelay(float seconds)
	{
		RespawnWindow.u003cCloseAfterDelayu003ec__Iterator1BE variable = null;
		return variable;
	}

	private void FillEquipments(KillerInfo killerInfo)
	{
		this.hatItem.SetItemTag(killerInfo.hat, 6);
		this.maskItem.SetItemTag(killerInfo.mask, 12);
		this.armorItem.SetItemTag(killerInfo.armor, 7);
		this.capeItem.SetItemTag(killerInfo.cape, 9);
		this.bootsItem.SetItemTag(killerInfo.boots, 10);
	}

	private void FillItemsToBuy(KillerInfo killerInfo)
	{
		string item;
		string armorForBuy;
		string str;
		try
		{
			try
			{
				List<string> strs = (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 ? this.GetWeaponsForBuy() : new List<string>());
				if (strs.Count <= 0)
				{
					item = null;
				}
				else
				{
					item = strs[0];
				}
				string str1 = item;
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
				{
					armorForBuy = this.GetArmorForBuy();
				}
				else
				{
					armorForBuy = null;
				}
				string str2 = armorForBuy;
				if (string.IsNullOrEmpty(str2))
				{
					if (strs.Count <= 1)
					{
						str = null;
					}
					else
					{
						str = strs[1];
					}
					str2 = str;
				}
				if (killerInfo == null || killerInfo.weapon == null)
				{
					this.killerWeapon.SetWeaponTag(string.Empty, new int?(0));
				}
				else
				{
					string str3 = killerInfo.weapon;
					int? nullable = null;
					if (GearManager.IsItemGear(str3))
					{
						if (str3 == GearManager.Turret)
						{
							nullable = new int?(1 + killerInfo.turretUpgrade);
						}
						else if (str3 == GearManager.Mech)
						{
							nullable = new int?(1 + killerInfo.mechUpgrade);
						}
					}
					this.killerWeapon.SetWeaponTag(str3, nullable);
				}
				this.recommendedWeapon.SetWeaponTag(str1, null);
				this.recommendedArmor.SetWeaponTag(str2, null);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		finally
		{
		}
	}

	private void FillStats(KillerInfo killerInfo)
	{
		int armorCountFor = Wear.GetArmorCountFor(killerInfo.armor, killerInfo.hat);
		if (armorCountFor <= 0 || killerInfo.isMech)
		{
			this.armorObj.SetActive(false);
			this.healthBackground.SetActive(true);
			this.healtharmorBackground.SetActive(false);
			this.healthTable.repositionNow = true;
		}
		else
		{
			this.armorObj.SetActive(true);
			this.healthBackground.SetActive(false);
			this.healtharmorBackground.SetActive(true);
			this.armorCountLabel.text = string.Format("{0}/{1}", Mathf.Min(armorCountFor, killerInfo.armorValue), armorCountFor);
			this.healthTable.repositionNow = true;
		}
		this.mechIcon.SetActive(killerInfo.isMech);
		this.healthIcon.SetActive(!killerInfo.isMech);
		if (!killerInfo.isMech)
		{
			this.healthCountLabel.text = string.Format("{0}/{1}", killerInfo.healthValue, ExperienceController.HealthByLevel[killerInfo.rank]);
		}
		else
		{
			this.healthCountLabel.text = string.Format("{0}/{1}", killerInfo.healthValue, WeaponManager.sharedManager.myPlayerMoveC.liveMechByTier[killerInfo.mechUpgrade]);
		}
	}

	private string GetArmorForBuy()
	{
		List<string> strs = new List<string>();
		strs.AddRange(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0]);
		List<string> strs1 = PromoActionsGUIController.FilterPurchases(strs, true, true, false, true);
		foreach (string str in strs)
		{
			if (!TempItemsController.PriceCoefs.ContainsKey(str) || strs1.Contains(str))
			{
				continue;
			}
			strs1.Add(str);
		}
		foreach (string str1 in strs1)
		{
			strs.Remove(str1);
		}
		return strs.FirstOrDefault<string>();
	}

	private List<string> GetWeaponsForBuy()
	{
		List<string> weaponsForBuy = WeaponManager.GetWeaponsForBuy();
		this.SortWeaponsByDps(weaponsForBuy);
		return weaponsForBuy;
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
		this.characterViewHolder.SetActive(false);
		this.characterView.gameObject.SetActive(false);
		this.Reset();
		Defs.inRespawnWindow = false;
		RespawnWindow._instance = null;
	}

	private void OnBackFromBankClicked(object sender, EventArgs e)
	{
		BankController.Instance.BackRequested -= new EventHandler(this.OnBackFromBankClicked);
		BankController.Instance.InterfaceEnabled = false;
		if (this != null)
		{
			base.gameObject.SetActive(true);
		}
		this.SetPaused(false);
	}

	public void OnBtnGoBattleClick()
	{
		this.RespawnPlayer();
		this.Hide();
	}

	public void OnItemToBuyClick(RespawnWindowItemToBuy itemToBuy)
	{
		Action<string> action = null;
		if (itemToBuy.itemTag == null || itemToBuy.itemPrice == null)
		{
			return;
		}
		int price = itemToBuy.itemPrice.Price;
		string currency = itemToBuy.itemPrice.Currency;
		ShopNGUIController.TryToBuy(base.gameObject, itemToBuy.itemPrice, () => {
			if (Defs.isSoundFX)
			{
				UIPlaySound component = itemToBuy.btnBuy.GetComponent<UIPlaySound>();
				if (component != null)
				{
					component.Play();
				}
			}
			if (itemToBuy.itemCategory == 7 || ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)itemToBuy.itemCategory))
			{
				ShopNGUIController.FireWeaponOrArmorBought();
			}
			int num = 1;
			if (GearManager.IsItemGear(itemToBuy.itemTag))
			{
				num = GearManager.ItemsInPackForGear(itemToBuy.itemTag);
				if (itemToBuy.itemTag == GearManager.Grenade)
				{
					num = Mathf.Min(num, Defs2.MaxGrenadeCount - Storager.getInt(itemToBuy.itemTag, false));
				}
			}
			int num1 = itemToBuy.itemCategory;
			string str = itemToBuy.itemTag;
			int num2 = num;
			if (action == null)
			{
				action = (string item) => {
					if (ShopNGUIController.sharedShop != null)
					{
						ShopNGUIController.sharedShop.FireBuyAction(item);
					}
				};
			}
			ShopNGUIController.ProvideShopItemOnStarterPackBoguht((ShopNGUIController.CategoryNames)num1, str, num2, false, 0, action, null, true, true, false);
			this.killerWeapon.SetWeaponTag(this.killerWeapon.itemTag, null);
			this.recommendedWeapon.SetWeaponTag(this.recommendedWeapon.itemTag, null);
			this.recommendedArmor.SetWeaponTag(this.recommendedArmor.itemTag, null);
			try
			{
				string empty = string.Empty;
				string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(itemToBuy.itemTag) ?? WeaponManager.FirstUnboughtTag(itemToBuy.itemTag), empty, (ShopNGUIController.CategoryNames)itemToBuy.itemCategory, null);
				FlurryPluginWrapper.LogPurchaseByModes((ShopNGUIController.CategoryNames)itemToBuy.itemCategory, itemNameNonLocalized, 1, false);
				if (itemToBuy.itemCategory != 11)
				{
					FlurryPluginWrapper.LogPurchaseByPoints((ShopNGUIController.CategoryNames)itemToBuy.itemCategory, itemNameNonLocalized, 1);
					FlurryPluginWrapper.LogPurchasesPoints(ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)itemToBuy.itemCategory));
				}
				ShopNGUIController.CategoryNames categoryName = (ShopNGUIController.CategoryNames)itemToBuy.itemCategory;
				bool flag = false;
				if (ShopNGUIController.IsWeaponCategory(categoryName))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(itemToBuy.itemTag);
					flag = (weaponInfo == null ? false : weaponInfo.IsAvalibleFromFilter(3));
				}
				AnalyticsStuff.LogSales(itemNameNonLocalized, (!FlurryEvents.shopCategoryToLogSalesNamesMapping.ContainsKey(categoryName) ? categoryName.ToString() : FlurryEvents.shopCategoryToLogSalesNamesMapping[categoryName]), flag);
				AnalyticsFacade.InAppPurchase(itemNameNonLocalized, (!FlurryEvents.shopCategoryToLogSalesNamesMapping.ContainsKey(categoryName) ? categoryName.ToString() : FlurryEvents.shopCategoryToLogSalesNamesMapping[categoryName]), 1, price, currency);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in loggin in Respawn Window: ", exception));
			}
			string str1 = "Cooldown Screen Purchases Total";
			string str2 = string.Concat(str1, FlurryPluginWrapper.GetPayingSuffix());
			string str3 = (ShopNGUIController.CategoryNames)itemToBuy.itemCategory.ToString();
			string str4 = itemToBuy.nonLocalizedName;
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "All Categories", str3 },
				{ str3, str4 },
				{ "Item", str4 }
			};
			if (itemToBuy.itemCategory != 11)
			{
				strs.Add("Without Quick Shop", str4);
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs, true);
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str2, strs, true);
		}, null, null, null, () => this.SetPaused(true), () => this.SetPaused(false));
	}

	private void OnToBankClicked(object sender, EventArgs e)
	{
		this.ShowBankWindow();
	}

	private void Reset()
	{
		this.killerWeapon.Reset();
		this.recommendedWeapon.Reset();
		this.recommendedArmor.Reset();
		this.hatItem.itemImage.mainTexture = null;
		this.maskItem.itemImage.mainTexture = null;
		this.armorItem.itemImage.mainTexture = null;
		this.capeItem.itemImage.mainTexture = null;
		this.bootsItem.itemImage.mainTexture = null;
	}

	private void RespawnPlayer()
	{
		Player_move_c playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		if (playerMoveC != null)
		{
			playerMoveC.RespawnPlayer();
		}
	}

	private void SetKillerNameVisible(bool visible)
	{
		this.killerLevelNicknameLabel.gameObject.SetActive(visible);
		this.killerRank.gameObject.SetActive(visible);
		this.killerClanNameLabel.gameObject.SetActive(visible);
		this.killerClanLogo.gameObject.SetActive(visible);
	}

	private void SetPaused(bool paused)
	{
		if (paused)
		{
			base.StopCoroutine("CloseAfterDelay");
		}
		else if (this != null)
		{
			base.StartCoroutine("CloseAfterDelay", this._remained);
		}
	}

	public void Show(KillerInfo inKillerInfo)
	{
		KillerInfo killerInfo = new KillerInfo();
		inKillerInfo.CopyTo(killerInfo);
		if (killerInfo.isGrenade)
		{
			killerInfo.weapon = GearManager.Grenade;
		}
		if (killerInfo.isTurret)
		{
			killerInfo.weapon = GearManager.Turret;
		}
		if (killerInfo.isMech)
		{
			killerInfo.weapon = GearManager.Mech;
		}
		this.killerLevelNicknameLabel.text = killerInfo.nickname;
		this.killerRank.mainTexture = killerInfo.rankTex;
		this.killerClanLogo.mainTexture = killerInfo.clanLogoTex;
		this.killerClanNameLabel.text = killerInfo.clanName;
		this.FillItemsToBuy(killerInfo);
		this.FillEquipments(killerInfo);
		this.FillStats(killerInfo);
		base.gameObject.SetActive(true);
		Defs.inRespawnWindow = true;
		RespawnWindow._instance = this;
		this.characterViewHolder.SetActive(true);
		this.SetKillerNameVisible(false);
		this._originalTimeout = 30f;
		base.StartCoroutine("CloseAfterDelay", this._originalTimeout);
	}

	private void ShowBankWindow()
	{
		ButtonClickSound.Instance.PlayClick();
		BankController.Instance.BackRequested += new EventHandler(this.OnBackFromBankClicked);
		BankController.Instance.InterfaceEnabled = true;
		this.SetPaused(true);
		if (this != null)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void ShowCharacter(KillerInfo killerInfo)
	{
		if (killerInfo.isMech)
		{
			this.characterView.ShowCharacterType(CharacterView.CharacterType.Mech);
			this.characterView.UpdateMech(killerInfo.mechUpgrade);
		}
		else if (!killerInfo.isTurret)
		{
			this.characterView.ShowCharacterType(CharacterView.CharacterType.Player);
			if (!killerInfo.isGrenade)
			{
				this.characterView.SetWeaponAndSkin(killerInfo.weapon, killerInfo.skinTex, true);
			}
			else
			{
				this.characterView.SetWeaponAndSkin("WeaponGrenade", killerInfo.skinTex, true);
			}
			if (string.IsNullOrEmpty(killerInfo.hat))
			{
				this.characterView.RemoveHat();
			}
			else
			{
				this.characterView.UpdateHat(killerInfo.hat);
			}
			if (string.IsNullOrEmpty(killerInfo.cape))
			{
				this.characterView.RemoveCape();
			}
			else
			{
				this.characterView.UpdateCape(killerInfo.cape, killerInfo.capeTex);
			}
			if (string.IsNullOrEmpty(killerInfo.mask))
			{
				this.characterView.RemoveMask();
			}
			else
			{
				this.characterView.UpdateMask(killerInfo.mask);
			}
			if (string.IsNullOrEmpty(killerInfo.boots))
			{
				this.characterView.RemoveBoots();
			}
			else
			{
				this.characterView.UpdateBoots(killerInfo.boots);
			}
			if (string.IsNullOrEmpty(killerInfo.armor))
			{
				this.characterView.RemoveArmor();
			}
			else
			{
				this.characterView.UpdateArmor(killerInfo.armor);
			}
		}
		else
		{
			this.characterView.ShowCharacterType(CharacterView.CharacterType.Turret);
			this.characterView.UpdateTurret(killerInfo.turretUpgrade);
		}
		this.characterViewHolder.SetActive(true);
		this.characterViewCamera.gameObject.SetActive(true);
		this.characterView.gameObject.SetActive(true);
		this.SetKillerNameVisible(true);
	}

	private void SortWeaponsByDps(List<string> weaponTags)
	{
		weaponTags.Sort((string weaponTag1, string weaponTag2) => {
			WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(weaponTag1);
			if (weaponInfo == null)
			{
				return 1;
			}
			WeaponSounds weaponSound = ItemDb.GetWeaponInfo(weaponTag2);
			if (weaponSound == null)
			{
				return -1;
			}
			return weaponSound.DPS.CompareTo(weaponInfo.DPS);
		});
	}

	private void Start()
	{
		if (this.coinsShopButton != null)
		{
			ButtonHandler component = this.coinsShopButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += new EventHandler(this.OnToBankClicked);
			}
		}
	}
}