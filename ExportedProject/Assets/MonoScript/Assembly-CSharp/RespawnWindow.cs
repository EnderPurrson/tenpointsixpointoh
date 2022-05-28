using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class RespawnWindow : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003COnItemToBuyClick_003Ec__AnonStorey34C
	{
		internal RespawnWindowItemToBuy itemToBuy;

		internal int priceAmount;

		internal string priceCurrency;

		internal RespawnWindow _003C_003Ef__this;

		private static Action<string> _003C_003Ef__am_0024cache4;

		internal void _003C_003Em__55E()
		{
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
					int b = Defs2.MaxGrenadeCount - Storager.getInt(itemToBuy.itemTag, false);
					num = Mathf.Min(num, b);
				}
			}
			int itemCategory = itemToBuy.itemCategory;
			string itemTag = itemToBuy.itemTag;
			int gearCount = num;
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003C_003Em__562;
			}
			ShopNGUIController.ProvideShopItemOnStarterPackBoguht((ShopNGUIController.CategoryNames)itemCategory, itemTag, gearCount, false, 0, _003C_003Ef__am_0024cache4, null, true, true, false);
			_003C_003Ef__this.killerWeapon.SetWeaponTag(_003C_003Ef__this.killerWeapon.itemTag);
			_003C_003Ef__this.recommendedWeapon.SetWeaponTag(_003C_003Ef__this.recommendedWeapon.itemTag);
			_003C_003Ef__this.recommendedArmor.SetWeaponTag(_003C_003Ef__this.recommendedArmor.itemTag);
			try
			{
				string empty = string.Empty;
				string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(itemToBuy.itemTag) ?? WeaponManager.FirstUnboughtTag(itemToBuy.itemTag), empty, (ShopNGUIController.CategoryNames)itemToBuy.itemCategory);
				FlurryPluginWrapper.LogPurchaseByModes((ShopNGUIController.CategoryNames)itemToBuy.itemCategory, itemNameNonLocalized, 1, false);
				if (itemToBuy.itemCategory != 11)
				{
					FlurryPluginWrapper.LogPurchaseByPoints((ShopNGUIController.CategoryNames)itemToBuy.itemCategory, itemNameNonLocalized, 1);
					FlurryPluginWrapper.LogPurchasesPoints(ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)itemToBuy.itemCategory));
				}
				ShopNGUIController.CategoryNames itemCategory2 = (ShopNGUIController.CategoryNames)itemToBuy.itemCategory;
				bool isDaterWeapon = false;
				if (ShopNGUIController.IsWeaponCategory(itemCategory2))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(itemToBuy.itemTag);
					isDaterWeapon = weaponInfo != null && weaponInfo.IsAvalibleFromFilter(3);
				}
				AnalyticsStuff.LogSales(itemNameNonLocalized, (!FlurryEvents.shopCategoryToLogSalesNamesMapping.ContainsKey(itemCategory2)) ? itemCategory2.ToString() : FlurryEvents.shopCategoryToLogSalesNamesMapping[itemCategory2], isDaterWeapon);
				AnalyticsFacade.InAppPurchase(itemNameNonLocalized, (!FlurryEvents.shopCategoryToLogSalesNamesMapping.ContainsKey(itemCategory2)) ? itemCategory2.ToString() : FlurryEvents.shopCategoryToLogSalesNamesMapping[itemCategory2], 1, priceAmount, priceCurrency);
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in loggin in Respawn Window: " + ex);
			}
			string text = "Cooldown Screen Purchases Total";
			string eventName = text + FlurryPluginWrapper.GetPayingSuffix();
			string text2 = ((ShopNGUIController.CategoryNames)itemToBuy.itemCategory).ToString();
			string nonLocalizedName = itemToBuy.nonLocalizedName;
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{ "All Categories", text2 },
				{ text2, nonLocalizedName },
				{ "Item", nonLocalizedName }
			};
			if (itemToBuy.itemCategory != 11)
			{
				dictionary.Add("Without Quick Shop", nonLocalizedName);
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole(text, dictionary);
			FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, dictionary);
		}

		internal void _003C_003Em__55F()
		{
			_003C_003Ef__this.SetPaused(true);
		}

		internal void _003C_003Em__560()
		{
			_003C_003Ef__this.SetPaused(false);
		}

		private static void _003C_003Em__562(string item)
		{
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.FireBuyAction(item);
			}
		}
	}

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

	[CompilerGenerated]
	private static Comparison<string> _003C_003Ef__am_0024cache21;

	public static RespawnWindow Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool isShown
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	private void Start()
	{
		if (coinsShopButton != null)
		{
			ButtonHandler component = coinsShopButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += OnToBankClicked;
			}
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
		killerLevelNicknameLabel.text = killerInfo.nickname;
		killerRank.mainTexture = killerInfo.rankTex;
		killerClanLogo.mainTexture = killerInfo.clanLogoTex;
		killerClanNameLabel.text = killerInfo.clanName;
		FillItemsToBuy(killerInfo);
		FillEquipments(killerInfo);
		FillStats(killerInfo);
		base.gameObject.SetActive(true);
		Defs.inRespawnWindow = true;
		_instance = this;
		characterViewHolder.SetActive(true);
		SetKillerNameVisible(false);
		_originalTimeout = 30f;
		StartCoroutine("CloseAfterDelay", _originalTimeout);
	}

	public void ShowCharacter(KillerInfo killerInfo)
	{
		if (killerInfo.isMech)
		{
			characterView.ShowCharacterType(CharacterView.CharacterType.Mech);
			characterView.UpdateMech(killerInfo.mechUpgrade);
		}
		else if (killerInfo.isTurret)
		{
			characterView.ShowCharacterType(CharacterView.CharacterType.Turret);
			characterView.UpdateTurret(killerInfo.turretUpgrade);
		}
		else
		{
			characterView.ShowCharacterType(CharacterView.CharacterType.Player);
			if (killerInfo.isGrenade)
			{
				characterView.SetWeaponAndSkin("WeaponGrenade", killerInfo.skinTex, true);
			}
			else
			{
				characterView.SetWeaponAndSkin(killerInfo.weapon, killerInfo.skinTex, true);
			}
			if (!string.IsNullOrEmpty(killerInfo.hat))
			{
				characterView.UpdateHat(killerInfo.hat);
			}
			else
			{
				characterView.RemoveHat();
			}
			if (!string.IsNullOrEmpty(killerInfo.cape))
			{
				characterView.UpdateCape(killerInfo.cape, killerInfo.capeTex);
			}
			else
			{
				characterView.RemoveCape();
			}
			if (!string.IsNullOrEmpty(killerInfo.mask))
			{
				characterView.UpdateMask(killerInfo.mask);
			}
			else
			{
				characterView.RemoveMask();
			}
			if (!string.IsNullOrEmpty(killerInfo.boots))
			{
				characterView.UpdateBoots(killerInfo.boots);
			}
			else
			{
				characterView.RemoveBoots();
			}
			if (!string.IsNullOrEmpty(killerInfo.armor))
			{
				characterView.UpdateArmor(killerInfo.armor);
			}
			else
			{
				characterView.RemoveArmor();
			}
		}
		characterViewHolder.SetActive(true);
		characterViewCamera.gameObject.SetActive(true);
		characterView.gameObject.SetActive(true);
		SetKillerNameVisible(true);
	}

	[Obfuscation(Exclude = true)]
	public IEnumerator CloseAfterDelay(float seconds)
	{
		float closeTime = Time.realtimeSinceStartup + seconds;
		_remained = 0f;
		do
		{
			_remained = closeTime - Time.realtimeSinceStartup;
			autoRespawnTimerLabel.text = Mathf.CeilToInt(_remained).ToString();
			yield return null;
		}
		while (_remained > 0f);
		if (isShown)
		{
			RespawnPlayer();
			Hide();
		}
	}

	private void RespawnPlayer()
	{
		Player_move_c myPlayerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		if (myPlayerMoveC != null)
		{
			myPlayerMoveC.RespawnPlayer();
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
		characterViewHolder.SetActive(false);
		characterView.gameObject.SetActive(false);
		Reset();
		Defs.inRespawnWindow = false;
		_instance = null;
	}

	public void OnBtnGoBattleClick()
	{
		RespawnPlayer();
		Hide();
	}

	private void FillEquipments(KillerInfo killerInfo)
	{
		hatItem.SetItemTag(killerInfo.hat, 6);
		maskItem.SetItemTag(killerInfo.mask, 12);
		armorItem.SetItemTag(killerInfo.armor, 7);
		capeItem.SetItemTag(killerInfo.cape, 9);
		bootsItem.SetItemTag(killerInfo.boots, 10);
	}

	private void FillItemsToBuy(KillerInfo killerInfo)
	{
		try
		{
			List<string> list = ((BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64) ? GetWeaponsForBuy() : new List<string>());
			string itemTag = ((list.Count <= 0) ? null : list[0]);
			string text = ((BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64) ? GetArmorForBuy() : null);
			if (string.IsNullOrEmpty(text))
			{
				text = ((list.Count <= 1) ? null : list[1]);
			}
			if (killerInfo != null && killerInfo.weapon != null)
			{
				string weapon = killerInfo.weapon;
				int? upgradeNum = null;
				if (GearManager.IsItemGear(weapon))
				{
					if (weapon == GearManager.Turret)
					{
						upgradeNum = 1 + killerInfo.turretUpgrade;
					}
					else if (weapon == GearManager.Mech)
					{
						upgradeNum = 1 + killerInfo.mechUpgrade;
					}
				}
				killerWeapon.SetWeaponTag(weapon, upgradeNum);
			}
			else
			{
				killerWeapon.SetWeaponTag(string.Empty, 0);
			}
			recommendedWeapon.SetWeaponTag(itemTag);
			recommendedArmor.SetWeaponTag(text);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		finally
		{
		}
	}

	public void OnItemToBuyClick(RespawnWindowItemToBuy itemToBuy)
	{
		_003COnItemToBuyClick_003Ec__AnonStorey34C _003COnItemToBuyClick_003Ec__AnonStorey34C = new _003COnItemToBuyClick_003Ec__AnonStorey34C();
		_003COnItemToBuyClick_003Ec__AnonStorey34C.itemToBuy = itemToBuy;
		_003COnItemToBuyClick_003Ec__AnonStorey34C._003C_003Ef__this = this;
		if (_003COnItemToBuyClick_003Ec__AnonStorey34C.itemToBuy.itemTag != null && _003COnItemToBuyClick_003Ec__AnonStorey34C.itemToBuy.itemPrice != null)
		{
			_003COnItemToBuyClick_003Ec__AnonStorey34C.priceAmount = _003COnItemToBuyClick_003Ec__AnonStorey34C.itemToBuy.itemPrice.Price;
			_003COnItemToBuyClick_003Ec__AnonStorey34C.priceCurrency = _003COnItemToBuyClick_003Ec__AnonStorey34C.itemToBuy.itemPrice.Currency;
			ShopNGUIController.TryToBuy(base.gameObject, _003COnItemToBuyClick_003Ec__AnonStorey34C.itemToBuy.itemPrice, _003COnItemToBuyClick_003Ec__AnonStorey34C._003C_003Em__55E, null, null, null, _003COnItemToBuyClick_003Ec__AnonStorey34C._003C_003Em__55F, _003COnItemToBuyClick_003Ec__AnonStorey34C._003C_003Em__560);
		}
	}

	private void FillStats(KillerInfo killerInfo)
	{
		int armorCountFor = Wear.GetArmorCountFor(killerInfo.armor, killerInfo.hat);
		if (armorCountFor > 0 && !killerInfo.isMech)
		{
			armorObj.SetActive(true);
			healthBackground.SetActive(false);
			healtharmorBackground.SetActive(true);
			armorCountLabel.text = string.Format("{0}/{1}", Mathf.Min(armorCountFor, killerInfo.armorValue), armorCountFor);
			healthTable.repositionNow = true;
		}
		else
		{
			armorObj.SetActive(false);
			healthBackground.SetActive(true);
			healtharmorBackground.SetActive(false);
			healthTable.repositionNow = true;
		}
		mechIcon.SetActive(killerInfo.isMech);
		healthIcon.SetActive(!killerInfo.isMech);
		if (killerInfo.isMech)
		{
			healthCountLabel.text = string.Format("{0}/{1}", killerInfo.healthValue, WeaponManager.sharedManager.myPlayerMoveC.liveMechByTier[killerInfo.mechUpgrade]);
		}
		else
		{
			healthCountLabel.text = string.Format("{0}/{1}", killerInfo.healthValue, ExperienceController.HealthByLevel[killerInfo.rank]);
		}
	}

	private void OnBackFromBankClicked(object sender, EventArgs e)
	{
		BankController.Instance.BackRequested -= OnBackFromBankClicked;
		BankController.Instance.InterfaceEnabled = false;
		if (this != null)
		{
			base.gameObject.SetActive(true);
		}
		SetPaused(false);
	}

	private void OnToBankClicked(object sender, EventArgs e)
	{
		ShowBankWindow();
	}

	private void ShowBankWindow()
	{
		ButtonClickSound.Instance.PlayClick();
		BankController.Instance.BackRequested += OnBackFromBankClicked;
		BankController.Instance.InterfaceEnabled = true;
		SetPaused(true);
		if (this != null)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void SetPaused(bool paused)
	{
		if (paused)
		{
			StopCoroutine("CloseAfterDelay");
		}
		else if (this != null)
		{
			StartCoroutine("CloseAfterDelay", _remained);
		}
	}

	private List<string> GetWeaponsForBuy()
	{
		List<string> weaponsForBuy = WeaponManager.GetWeaponsForBuy();
		SortWeaponsByDps(weaponsForBuy);
		return weaponsForBuy;
	}

	private string GetArmorForBuy()
	{
		List<string> list = new List<string>();
		list.AddRange(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0]);
		bool filterNextTierUpgrades = true;
		List<string> list2 = PromoActionsGUIController.FilterPurchases(list, filterNextTierUpgrades);
		foreach (string item in list)
		{
			if (TempItemsController.PriceCoefs.ContainsKey(item) && !list2.Contains(item))
			{
				list2.Add(item);
			}
		}
		foreach (string item2 in list2)
		{
			list.Remove(item2);
		}
		return list.FirstOrDefault();
	}

	private void SortWeaponsByDps(List<string> weaponTags)
	{
		if (_003C_003Ef__am_0024cache21 == null)
		{
			_003C_003Ef__am_0024cache21 = _003CSortWeaponsByDps_003Em__561;
		}
		weaponTags.Sort(_003C_003Ef__am_0024cache21);
	}

	private void SetKillerNameVisible(bool visible)
	{
		killerLevelNicknameLabel.gameObject.SetActive(visible);
		killerRank.gameObject.SetActive(visible);
		killerClanNameLabel.gameObject.SetActive(visible);
		killerClanLogo.gameObject.SetActive(visible);
	}

	private void Reset()
	{
		killerWeapon.Reset();
		recommendedWeapon.Reset();
		recommendedArmor.Reset();
		hatItem.itemImage.mainTexture = null;
		maskItem.itemImage.mainTexture = null;
		armorItem.itemImage.mainTexture = null;
		capeItem.itemImage.mainTexture = null;
		bootsItem.itemImage.mainTexture = null;
	}

	[CompilerGenerated]
	private static int _003CSortWeaponsByDps_003Em__561(string weaponTag1, string weaponTag2)
	{
		WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(weaponTag1);
		if (weaponInfo == null)
		{
			return 1;
		}
		WeaponSounds weaponInfo2 = ItemDb.GetWeaponInfo(weaponTag2);
		if (weaponInfo2 == null)
		{
			return -1;
		}
		return weaponInfo2.DPS.CompareTo(weaponInfo.DPS);
	}
}
