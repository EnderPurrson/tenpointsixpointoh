using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopNGUIController : MonoBehaviour
{
	public const string BoughtCurrencsySettingBase = "BoughtCurrency";

	public const string TrainingShopStageStepKey = "ShopNGUIController.TrainingShopStageStepKey";

	public const string CustomSkinID = "CustomSkinID";

	private const string ShowArmorKey = "ShowArmorKeySetting";

	private const string ShowHatKey = "ShowHatKeySetting";

	[Header("Training After Removing Novice Armor")]
	public GameObject trainingRemoveNoviceArmorCollider;

	public List<GameObject> trainingTipsRemovedNoviceArmor = new List<GameObject>();

	[Header("Training")]
	public GameObject trainingColliders;

	public List<GameObject> trainingTips = new List<GameObject>();

	private bool _trainStateInitialized;

	private UISprite toBlink;

	private Dictionary<ShopNGUIController.TrainingState, Action> _setTrainingStateMethods;

	private ShopNGUIController.TrainingState _trainingState;

	private ShopNGUIController.TrainingState _trainingStateRemovedNoviceArmor;

	public static ShopNGUIController sharedShop;

	public GameObject tryGunPanel;

	public UILabel tryGunMatchesCount;

	public GameObject tryGunDiscountPanel;

	public UILabel tryGunDiscountTime;

	public UILabel nonArmorWearDEscription;

	public UILabel armorWearDescription;

	public UILabel armorCountLabel;

	public UIButton facebookLoginLockedSkinButton;

	public List<UISprite> effectsSprites;

	public List<UILabel> effectsLabels;

	public GameObject hatLock;

	public GameObject armorLock;

	public BoxCollider shopCarouselCollider;

	public ToggleButton showArmorButtonTempArmor;

	public ToggleButton showHatButtonTempHat;

	public GameObject prolongateRentText;

	public GameObject purchaseSuccessfulRent;

	public UILabel salePercRent;

	public GameObject saleRent;

	public GameObject rentProperties;

	public GameObject notRented;

	public UILabel daysLeftLabel;

	public UILabel timeLeftLabel;

	public UILabel daysLeftValueLabel;

	public UILabel timeLeftValueLabel;

	public GameObject redBackForTime;

	public UIButton rent;

	public Transform rentScreenPoint;

	public ToggleButton showArmorButton;

	public ToggleButton showHatButton;

	public PropertyInfoScreenController infoScreen;

	public GameObject stub;

	public UIButton upgradeGear;

	public UISprite currencyImagePrice;

	public UISprite currencyImagePriceGear;

	public UISprite currencyImagePriceUpgradeGear;

	public UILabel price2Gear;

	public UILabel priceUpgradeGear;

	public GameObject wholePrice2Gear;

	public GameObject wholePriceUpgradeGear;

	public UIButton buyGear;

	public UITexture wholePriceBG;

	public UITexture wholePriceBG_Discount;

	public UITexture wholePriceBG2Gear;

	public UITexture wholePriceBG2Gear_Discount;

	public UITexture wholePriceUpgradeBG2Gear;

	public UITexture wholePriceUpgradeBG2Gear_Discount;

	public UILabel fireRate;

	public UILabel fireRateMElee;

	public UILabel mobility;

	public UILabel mobilityMelee;

	public UILabel capacity;

	public UILabel damage;

	public UILabel damageMelee;

	public GameObject needTier;

	public UILabel needTierLabel;

	public GameObject purchaseSuccessful;

	public List<Light> mylights;

	[Range(-200f, 200f)]
	public float firstOFfset = -50f;

	[Range(-200f, 200f)]
	public float secondOffset = 50f;

	public GameObject nonArmorWearProperties;

	public GameObject armorWearProperties;

	public GameObject skinProperties;

	public GameObject gearProperties;

	public GameObject border;

	public Action OnBecomeActive;

	private string offerID;

	public ShopNGUIController.CategoryNames offerCategory;

	public float scaleCoef = 0.5f;

	public Transform maskPoint;

	public Transform hatPoint;

	public Transform capePoint;

	public Transform bootsPoint;

	public Transform armorPoint;

	public GameObject mainPanel;

	public UIButton backButton;

	public UIButton coinShopButton;

	public UIPanel scrollViewPanel;

	public UIGrid wrapContent;

	public MyCenterOnChild carouselCenter;

	public GameObject body;

	public GameObject weaponPointShop;

	public Transform MainMenu_Pers;

	public string viewedId;

	public string chosenId;

	public Action resumeAction;

	public Action wearResumeAction;

	public Action<ShopNGUIController.CategoryNames, string> wearUnequipAction;

	public Action<ShopNGUIController.CategoryNames, string, string> wearEquipAction;

	public Action<string> buyAction;

	public Action<string> equipAction;

	public Action<string> activatePotionAction;

	public Action<string> onEquipSkinAction;

	private GameObject weapon;

	private AnimationClip profile;

	public bool EnableConfigurePos;

	public MultipleToggleButton category;

	public UIButton[] equips;

	public UISprite[] equippeds;

	public UIButton create;

	public UIButton buy;

	public UIButton upgrade;

	public UIButton unequip;

	public UIButton edit;

	public UIButton enable;

	public UIButton delete;

	public GameObject weaponProperties;

	public GameObject meleeProperties;

	public GameObject upgradesAnchor;

	public GameObject upgrade_1;

	public GameObject upgrade_2;

	public GameObject upgrade_3;

	public GameObject SpecialParams;

	public GameObject[] upgradeSprites3;

	public GameObject[] upgradeSprites2;

	public GameObject[] upgradeSprites1;

	public GameObject wholePrice;

	public GameObject sale;

	public UILabel price;

	public UILabel caption;

	public UILabel salePerc;

	public ShopNGUIController.CategoryNames currentCategory;

	public bool inGame = true;

	public List<Camera> ourCameras;

	public AnimationCoroutineRunner animationCoroutineRunner;

	public GameObject ActiveObject;

	private List<ShopPositionParams> hats = new List<ShopPositionParams>();

	private List<ShopPositionParams> capes = new List<ShopPositionParams>();

	private List<ShopPositionParams> boots = new List<ShopPositionParams>();

	private List<ShopPositionParams> masks = new List<ShopPositionParams>();

	private List<ShopPositionParams> armor = new List<ShopPositionParams>();

	private Action<List<ShopPositionParams>, ShopNGUIController.CategoryNames> sort = new Action<List<ShopPositionParams>, ShopNGUIController.CategoryNames>((List<ShopPositionParams> prefabs, ShopNGUIController.CategoryNames c) => prefabs.Sort((ShopPositionParams go1, ShopPositionParams go2) => {
		List<string> strs = null;
		List<string> strs1 = null;
		foreach (List<string> item in Wear.wear[c])
		{
			if (item.Contains(go1.name))
			{
				strs = item;
			}
			if (!item.Contains(go2.name))
			{
				continue;
			}
			strs1 = item;
		}
		if (strs == null || strs1 == null)
		{
			return 0;
		}
		if (strs == strs1)
		{
			return strs.IndexOf(go1.name) - strs.IndexOf(go2.name);
		}
		return Wear.wear[c].IndexOf(strs) - Wear.wear[c].IndexOf(strs1);
	}));

	private GameObject pixlMan;

	private int numberOfLoadingModels;

	public readonly static Dictionary<string, string> weaponCategoryLocKeys;

	private Transform highlightedCarouselObject;

	public int itemIndex = -1;

	private GameObject _lastSelectedItem;

	private GameObject[] _onPersArmorRefs;

	private static bool _ShowArmor;

	private static bool _ShowHat;

	private float timeToUpdateTempGunTime;

	private bool _shouldShowRewardWindowSkin;

	private bool _shouldShowRewardWindowCape;

	private GameObject _refOnLowPolyArmor;

	private Material[] _refsOnLowPolyArmorMaterials;

	public static string[] gearOrder;

	private string _currentCape;

	private string _currentHat;

	private string _currentBoots;

	private string _currentArmor;

	private string _currentMask;

	private float lastTime;

	public static float IdleTimeoutPers;

	private float idleTimerLastTime;

	private float _timePurchaseSuccessfulShown;

	private float _timePurchaseRentSuccessfulShown;

	private IDisposable _backSubscription;

	private bool _escapeRequested;

	private float timeOfEnteringShopForProtectFromPressingCoinsButton;

	private Color? _storedAmbientLight;

	private bool? _storedFogEnabled;

	private bool _isFromPromoActions;

	private string _promoActionsIdClicked;

	private string _assignedWeaponTag = string.Empty;

	private bool InTrainingAfterNoviceArmorRemoved;

	private string _CurrentEquippedSN
	{
		get
		{
			return ShopNGUIController.SnForWearCategory(this.currentCategory);
		}
	}

	public string _CurrentEquippedWear
	{
		get
		{
			return this.WearForCat(this.currentCategory);
		}
	}

	private string _CurrentNoneEquipped
	{
		get
		{
			return ShopNGUIController.NoneEquippedForWearCategory(this.currentCategory);
		}
	}

	public static bool GuiActive
	{
		get
		{
			return (ShopNGUIController.sharedShop == null ? false : ShopNGUIController.sharedShop.ActiveObject.activeInHierarchy);
		}
		set
		{
			string hunterRifleTag;
			ShopNGUIController.CategoryNames categoryName;
			if (value)
			{
				if (ShopNGUIController.sharedShop._backSubscription != null)
				{
					ShopNGUIController.sharedShop._backSubscription.Dispose();
				}
				ShopNGUIController.sharedShop._backSubscription = BackSystem.Instance.Register(new Action(ShopNGUIController.sharedShop.HandleEscape), "Shop");
			}
			else if (ShopNGUIController.sharedShop._backSubscription != null)
			{
				ShopNGUIController.sharedShop._backSubscription.Dispose();
				ShopNGUIController.sharedShop._backSubscription = null;
				Storager.RefreshWeaponDigestIfDirty();
			}
			if (value)
			{
				Transform vector3 = ShopNGUIController.sharedShop.category.buttons[11].transform;
				float single = vector3.localPosition.y;
				Vector3 vector31 = vector3.localPosition;
				vector3.localPosition = new Vector3(-10000f, single, vector31.z);
			}
			if (ZombieCreator.sharedCreator != null)
			{
				ZombieCreator.sharedCreator.SuppressDrawingWaveMessage();
			}
			if (Tools.RuntimePlatform != RuntimePlatform.MetroPlayerX64)
			{
				QualitySettings.antiAliasing = (!value || Device.isWeakDevice ? 0 : 4);
			}
			else
			{
				QualitySettings.antiAliasing = 0;
			}
			FreeAwardShowHandler.CheckShowChest(value);
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.SetOtherCamerasEnabled(!value);
				if (!value)
				{
					Color? nullable = ShopNGUIController.sharedShop._storedAmbientLight;
					RenderSettings.ambientLight = (!nullable.HasValue ? RenderSettings.ambientLight : nullable.Value);
					bool? nullable1 = ShopNGUIController.sharedShop._storedFogEnabled;
					RenderSettings.fog = (!nullable1.HasValue ? RenderSettings.fog : nullable1.Value);
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
					{
						ShopNGUIController.sharedShop.HideAllTrainingInterface();
						string str = ShopNGUIController.TempGunOrHighestDPSGun(ShopNGUIController.CategoryNames.PrimaryCategory, out categoryName);
						str = WeaponManager.FirstUnboughtTag(str);
						ShopNGUIController.sharedShop.CategoryChosen(categoryName, str, true);
					}
					else if (ShopNGUIController.sharedShop.InTrainingAfterNoviceArmorRemoved)
					{
						ShopNGUIController.sharedShop.HideAllTrainingInterfaceRemovedNoviceArmor();
					}
					ShopNGUIController.sharedShop.carouselCenter.onFinished -= new SpringPanel.OnFinished(ShopNGUIController.sharedShop.HandleCarouselCentering);
					PromoActionsManager.ActionsUUpdated -= new Action(ShopNGUIController.sharedShop.HandleActionsUUpdated);
					ShopNGUIController.sharedShop.SetWeapon(null);
					ShopNGUIController.sharedShop.ActiveObject.SetActive(false);
					ShopNGUIController.sharedShop.carouselCenter.enabled = false;
					WeaponManager.ClearCachedInnerPrefabs();
				}
				else
				{
					ShopNGUIController.sharedShop.armorLock.SetActive(Defs.isHunger);
					ShopNGUIController.sharedShop.stub.SetActive(true);
					try
					{
						ShopNGUIController.sharedShop._storedAmbientLight = new Color?(RenderSettings.ambientLight);
						ShopNGUIController.sharedShop._storedFogEnabled = new bool?(RenderSettings.fog);
						RenderSettings.ambientLight = Defs.AmbientLightColorForShop();
						RenderSettings.fog = false;
						ShopNGUIController.sharedShop.timeOfEnteringShopForProtectFromPressingCoinsButton = Time.realtimeSinceStartup;
						ShopNGUIController.sharedShop.LoadCurrentWearToVars();
						ShopNGUIController.sharedShop.UpdateIcons();
						ShopNGUIController.CategoryNames categoryName1 = ShopNGUIController.CategoryNames.PrimaryCategory;
						if (ShopNGUIController.sharedShop.offerID == null)
						{
							ShopNGUIController.CategoryNames categoryName2 = ShopNGUIController.CategoryNames.PrimaryCategory;
							if (WeaponManager.sharedManager != null && WeaponManager.sharedManager._currentFilterMap == 1)
							{
								categoryName1 = ShopNGUIController.CategoryNames.MeleeCategory;
								categoryName2 = ShopNGUIController.CategoryNames.MeleeCategory;
							}
							else if (WeaponManager.sharedManager != null && WeaponManager.sharedManager._currentFilterMap == 2)
							{
								categoryName1 = ShopNGUIController.CategoryNames.SniperCategory;
								categoryName2 = ShopNGUIController.CategoryNames.SniperCategory;
							}
							hunterRifleTag = ShopNGUIController._CurrentWeaponSetIDs()[(int)categoryName2] ?? ShopNGUIController.TempGunOrHighestDPSGun(categoryName2, out categoryName1);
						}
						else
						{
							hunterRifleTag = ShopNGUIController.sharedShop.offerID;
							categoryName1 = ShopNGUIController.sharedShop.offerCategory;
							ShopNGUIController.sharedShop.offerID = null;
						}
						if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
						{
							ShopNGUIController.sharedShop.HideAllTrainingInterface();
							if (!ShopNGUIController.sharedShop.InTrainingAfterNoviceArmorRemoved)
							{
								ShopNGUIController.sharedShop.HideAllTrainingInterfaceRemovedNoviceArmor();
							}
						}
						else
						{
							AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Open_Shop, true);
							ShopNGUIController.sharedShop.trainingColliders.SetActive(true);
							if (ShopNGUIController.sharedShop.trainingState >= ShopNGUIController.TrainingState.OnArmor)
							{
								categoryName1 = ShopNGUIController.CategoryNames.ArmorCategory;
								hunterRifleTag = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
							}
							else if (ShopNGUIController.sharedShop.trainingState >= ShopNGUIController.TrainingState.OnSniperRifle)
							{
								categoryName1 = ShopNGUIController.CategoryNames.SniperCategory;
								hunterRifleTag = WeaponTags.HunterRifleTag;
							}
						}
						string str1 = WeaponManager.LastBoughtTag(hunterRifleTag);
						if (str1 != null)
						{
							hunterRifleTag = str1;
						}
						else if (ShopNGUIController.IsWearCategory(categoryName1))
						{
							foreach (List<List<string>> lists in Wear.wear.Values)
							{
								foreach (List<string> strs in lists)
								{
									if (!strs.Contains(hunterRifleTag))
									{
										continue;
									}
									hunterRifleTag = strs[0];
									break;
								}
							}
						}
						ShopNGUIController.sharedShop.CategoryChosen(categoryName1, hunterRifleTag, true);
						ShopNGUIController.SetIconChosen(categoryName1);
						ShopNGUIController.sharedShop.MakeACtiveAfterDelay(hunterRifleTag, categoryName1);
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.GuiActive: ", exception));
					}
					ShopNGUIController.sharedShop.StartCoroutine(ShopNGUIController.sharedShop.DisableStub());
				}
			}
		}
	}

	private string IDForCurrentGear
	{
		get
		{
			if (this.viewedId == null)
			{
				return null;
			}
			return GearManager.HolderQuantityForID(this.viewedId);
		}
	}

	public bool IsFromPromoActions
	{
		get
		{
			return this._isFromPromoActions;
		}
	}

	private string NextUpgradeIDForCurrentGear
	{
		get
		{
			if (this.viewedId == null)
			{
				return null;
			}
			return GearManager.UpgradeIDForGear(GearManager.HolderQuantityForID(this.viewedId), GearManager.CurrentNumberOfUphradesForGear(this.viewedId) + 1);
		}
	}

	public static bool NoviceArmorAvailable
	{
		get
		{
			bool flag;
			if (Storager.getInt("Training.NoviceArmorUsedKey", false) != 1)
			{
				flag = false;
			}
			else
			{
				flag = (!TrainingController.TrainingCompleted ? true : Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1);
			}
			return flag;
		}
	}

	public static bool ShowArmor
	{
		get
		{
			return ShopNGUIController._ShowArmor;
		}
		private set
		{
			if (ShopNGUIController._ShowArmor != value)
			{
				ShopNGUIController._ShowArmor = value;
				Action action = ShopNGUIController.ShowArmorChanged;
				if (action != null)
				{
					action();
				}
			}
		}
	}

	public static bool ShowHat
	{
		get
		{
			return ShopNGUIController._ShowHat;
		}
		private set
		{
			if (ShopNGUIController._ShowHat != value)
			{
				ShopNGUIController._ShowHat = value;
				Action action = ShopNGUIController.ShowArmorChanged;
				if (action != null)
				{
					action();
				}
			}
		}
	}

	private ShopNGUIController.TrainingState trainingState
	{
		get
		{
			if (!this._trainStateInitialized)
			{
				this._trainingState = (ShopNGUIController.TrainingState)Storager.getInt("ShopNGUIController.TrainingShopStageStepKey", false);
				this._trainStateInitialized = true;
			}
			return this._trainingState;
		}
		set
		{
			try
			{
				if (this._trainingState != value)
				{
					this._trainingState = value;
					if (this._trainingState == ShopNGUIController.TrainingState.NotInArmorCategory || this._trainingState == ShopNGUIController.TrainingState.BackBlinking)
					{
						Storager.setInt("ShopNGUIController.TrainingShopStageStepKey", (int)this._trainingState, false);
					}
				}
				this._trainStateInitialized = true;
				for (int i = 0; i < this.trainingTips.Count; i++)
				{
					this.trainingTips[i].SetActive(i == (int)this._trainingState);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in trainingState setter: ", exception));
			}
		}
	}

	private ShopNGUIController.TrainingState trainingStateRemoveNoviceArmor
	{
		get
		{
			return this._trainingStateRemovedNoviceArmor;
		}
		set
		{
			try
			{
				if (this._trainingStateRemovedNoviceArmor != value)
				{
					this._trainingStateRemovedNoviceArmor = value;
				}
				for (int i = 0; i < this.trainingTipsRemovedNoviceArmor.Count; i++)
				{
					this.trainingTipsRemovedNoviceArmor[i].SetActive(this.trainingTipsRemovedNoviceArmor[i].name == this._trainingStateRemovedNoviceArmor.ToString());
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in trainingStateRemoveNoviceArmor setter: ", exception));
			}
		}
	}

	public bool WeaponCategory
	{
		get
		{
			return ShopNGUIController.IsWeaponCategory(this.currentCategory);
		}
	}

	public bool WearCategory
	{
		get
		{
			return ShopNGUIController.IsWearCategory(this.currentCategory);
		}
	}

	static ShopNGUIController()
	{
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ ShopNGUIController.CategoryNames.PrimaryCategory.ToString(), "Key_0352" },
			{ ShopNGUIController.CategoryNames.BackupCategory.ToString(), "Key_0442" },
			{ ShopNGUIController.CategoryNames.MeleeCategory.ToString(), "Key_0441" },
			{ ShopNGUIController.CategoryNames.SpecilCategory.ToString(), "Key_0440" },
			{ ShopNGUIController.CategoryNames.SniperCategory.ToString(), "Key_1669" },
			{ ShopNGUIController.CategoryNames.PremiumCategory.ToString(), "Key_0093" }
		};
		ShopNGUIController.weaponCategoryLocKeys = strs;
		ShopNGUIController._ShowArmor = true;
		ShopNGUIController._ShowHat = true;
		ShopNGUIController.gearOrder = PotionsController.potions;
		ShopNGUIController.IdleTimeoutPers = 5f;
	}

	public ShopNGUIController()
	{
	}

	public static int _CurrentNumberOfUpgrades(string itemId, out bool maxUpgrade, ShopNGUIController.CategoryNames c, bool countTryGunsAsUpgrade = true)
	{
		List<string> strs = new List<string>();
		int count = 0;
		if (ShopNGUIController.IsWeaponCategory(c))
		{
			strs = WeaponUpgrades.ChainForTag(itemId) ?? new List<string>()
			{
				itemId
			};
			count = strs.Count;
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(itemId))
			{
				int num = strs.Count - 1;
				while (num >= 0)
				{
					string item = itemId;
					bool flag = ItemDb.IsTemporaryGun(itemId);
					if (!flag)
					{
						item = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[strs[num]]];
					}
					bool flag1 = ShopNGUIController.HasBoughtGood(item, flag);
					if (!flag1 && countTryGunsAsUpgrade && WeaponManager.sharedManager != null)
					{
						flag1 = WeaponManager.sharedManager.IsAvailableTryGun(strs[num]);
					}
					if (flag1)
					{
						break;
					}
					else
					{
						count--;
						num--;
					}
				}
			}
		}
		else if (ShopNGUIController.IsWearCategory(c))
		{
			count = (!ShopNGUIController.HasBoughtGood(itemId, TempItemsController.PriceCoefs.ContainsKey(itemId)) ? 0 : 1);
		}
		if (itemId.Equals(StoreKitEventListener.elixirID) && Defs.NumberOfElixirs > 0)
		{
			count++;
		}
		maxUpgrade = count == (strs.Count <= 0 ? 1 : strs.Count);
		return count;
	}

	private static int _CurrentNumberOfWearUpgrades(string id, out bool maxUpgrade, ShopNGUIController.CategoryNames c)
	{
		if (id == "Armor_Novice")
		{
			maxUpgrade = ShopNGUIController.NoviceArmorAvailable;
			return (!ShopNGUIController.NoviceArmorAvailable ? 0 : 1);
		}
		List<string> strs = Wear.wear[c].FirstOrDefault<List<string>>((List<string> l) => l.Contains(id));
		if (strs == null)
		{
			maxUpgrade = false;
			return 0;
		}
		for (int i = 0; i < strs.Count; i++)
		{
			if (Storager.getInt(strs[i], true) == 0)
			{
				maxUpgrade = false;
				return i;
			}
		}
		maxUpgrade = true;
		return strs.Count;
	}

	private static string[] _CurrentWeaponSetIDs()
	{
		string[] tag = new string[6];
		WeaponManager weaponManager = WeaponManager.sharedManager;
		int num = 0;
		for (int i = 0; i < (int)tag.Length; i++)
		{
			if (num < weaponManager.playerWeapons.Count)
			{
				Weapon item = weaponManager.playerWeapons[num] as Weapon;
				if (item.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 != i)
				{
					tag[i] = null;
				}
				else
				{
					num++;
					tag[i] = ItemDb.GetByPrefabName(item.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
				}
			}
			else
			{
				tag[i] = null;
			}
		}
		return tag;
	}

	private static string _TagForId(string id)
	{
		string item;
		string str = id;
		List<List<string>>.Enumerator enumerator = WeaponUpgrades.upgrades.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				List<string> current = enumerator.Current;
				if (!current.Contains(str))
				{
					continue;
				}
				item = current[0];
				return item;
			}
			return str;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return item;
	}

	private void ActualBuy(string id, string tg, ItemPrice itemPrice)
	{
		bool flag;
		Dictionary<string, string> strs;
		if (this.currentCategory == ShopNGUIController.CategoryNames.ArmorCategory || ShopNGUIController.IsWeaponCategory(this.currentCategory))
		{
			ShopNGUIController.FireWeaponOrArmorBought();
		}
		ShopNGUIController.CategoryNames categoryName = this.currentCategory;
		int num = ShopNGUIController.AddedNumberOfGearWhenBuyingPack(id);
		ShopNGUIController.ProvdeShopItemWithRightId(categoryName, id, tg, null, (string item) => this.EquipWear(item), (string item) => {
			if (ShopNGUIController.IsWeaponCategory(categoryName) || ShopNGUIController.IsWearCategory(categoryName))
			{
				this.FireBuyAction(item);
			}
			this.purchaseSuccessful.SetActive(true);
			this._timePurchaseSuccessfulShown = Time.realtimeSinceStartup;
		}, (string item) => this.SetSkinAsCurrent(item), false, 1, false, 0, true);
		if (WeaponManager.tagToStoreIDMapping.ContainsValue(id))
		{
			IEnumerable<string> value = 
				from item in WeaponManager.tagToStoreIDMapping
				where item.Value == id
				select item into kv
				select kv.Key;
			ShopNGUIController.SynchronizeAndroidPurchases(string.Concat("Weapon: ", value.FirstOrDefault<string>()));
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId(id);
			int? nullable = null;
			if (priceByShopId != null)
			{
				nullable = new int?(priceByShopId.Price);
			}
			if (nullable.HasValue && nullable.Value >= PlayerPrefs.GetInt(Defs.MostExpensiveWeapon, 0))
			{
				PlayerPrefs.SetInt(Defs.MostExpensiveWeapon, nullable.Value);
				PlayerPrefs.SetString(Defs.MenuPersWeaponTag, (value.Count<string>() <= 0 ? string.Empty : value.ElementAt<string>(0)));
				PlayerPrefs.Save();
			}
		}
		string str = id;
		try
		{
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(id))
			{
				str = WeaponManager.tagToStoreIDMapping[id];
			}
			if (this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory && str != null && SkinsController.shopKeyFromNameSkin.ContainsKey(str))
			{
				str = SkinsController.shopKeyFromNameSkin[str];
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in setting shopId: ", exception));
		}
		try
		{
			string str1 = WeaponManager.LastBoughtTag(this.viewedId) ?? WeaponManager.FirstUnboughtTag(this.viewedId);
			string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(str1, str, this.currentCategory, null);
			try
			{
				if (this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory)
				{
					itemNameNonLocalized = LocalizationStore.GetByDefault(SkinsController.skinsLocalizeKey[int.Parse(id)]);
				}
			}
			catch (Exception exception1)
			{
				UnityEngine.Debug.LogError(string.Concat("Shop: ActualBuy: get readable skin name: ", exception1));
			}
			FlurryPluginWrapper.LogPurchaseByModes(this.currentCategory, (this.currentCategory != ShopNGUIController.CategoryNames.GearCategory ? itemNameNonLocalized : GearManager.HolderQuantityForID(str)), (this.currentCategory != ShopNGUIController.CategoryNames.GearCategory ? 1 : num), false);
			if (this.currentCategory == ShopNGUIController.CategoryNames.GearCategory)
			{
				FlurryPluginWrapper.LogGearPurchases(GearManager.HolderQuantityForID(str), num, false);
				if (Application.loadedLevelName == Defs.MainMenuScene)
				{
					FlurryPluginWrapper.LogPurchaseByPoints(this.currentCategory, GearManager.HolderQuantityForID(str), num);
				}
			}
			else
			{
				FlurryPluginWrapper.LogPurchasesPoints(ShopNGUIController.IsWeaponCategory(this.currentCategory));
				FlurryPluginWrapper.LogPurchaseByPoints(this.currentCategory, itemNameNonLocalized, 1);
			}
			try
			{
				bool flag1 = false;
				if (ShopNGUIController.IsWeaponCategory(this.currentCategory))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(str1);
					flag1 = (weaponInfo == null ? false : weaponInfo.IsAvalibleFromFilter(3));
				}
				string str2 = (!FlurryEvents.shopCategoryToLogSalesNamesMapping.ContainsKey(this.currentCategory) ? this.currentCategory.ToString() : FlurryEvents.shopCategoryToLogSalesNamesMapping[this.currentCategory]);
				AnalyticsStuff.LogSales(itemNameNonLocalized, str2, flag1);
				AnalyticsFacade.InAppPurchase(itemNameNonLocalized, str2, 1, itemPrice.Price, itemPrice.Currency);
				if (this._isFromPromoActions && this._promoActionsIdClicked != null && str1 != null && this._promoActionsIdClicked == str1)
				{
					AnalyticsStuff.LogSpecialOffersPanel("Efficiency", "Buy", str2 ?? "Unknown", itemNameNonLocalized);
				}
				this._isFromPromoActions = false;
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in LogSales block in Shop: ", exception2));
			}
			int num1 = ShopNGUIController.DiscountFor(WeaponManager.LastBoughtTag(this.viewedId) ?? WeaponManager.FirstUnboughtTag(this.viewedId), out flag);
			if (num1 > 0)
			{
				string itemNameNonLocalized1 = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(this.viewedId) ?? WeaponManager.FirstUnboughtTag(this.viewedId), str, this.currentCategory, "Unknown");
				strs = new Dictionary<string, string>()
				{
					{ num1.ToString(), itemNameNonLocalized1 }
				};
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Offers Sale", strs, true);
			}
			if (this.currentCategory == ShopNGUIController.CategoryNames.GearCategory && itemNameNonLocalized != null && !itemNameNonLocalized.Contains(GearManager.UpgradeSuffix) && GearManager.AllGear.Contains(itemNameNonLocalized))
			{
				itemNameNonLocalized = GearManager.AnalyticsIDForOneItemOfGear(itemNameNonLocalized, false);
			}
			this.LogShopPurchasesTotalAndPayingNonPaying(itemNameNonLocalized);
			if (ExperienceController.sharedController != null)
			{
				int num2 = ExperienceController.sharedController.currentLevel;
				int num3 = (num2 - 1) / 9;
				string str3 = string.Format("[{0}, {1})", num3 * 9 + 1, (num3 + 1) * 9 + 1);
				string str4 = string.Format("Shop Purchases On Level {0} ({1}){2}", str3, FlurryPluginWrapper.GetPayingSuffix().Trim(), string.Empty);
				strs = new Dictionary<string, string>()
				{
					{ string.Concat("Level ", num2), itemNameNonLocalized }
				};
				FlurryPluginWrapper.LogEventAndDublicateToConsole(str4, strs, true);
			}
			this.LogPurchaseAfterPaymentAnalyticsEvent(itemNameNonLocalized);
		}
		catch (Exception exception3)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in Shop Logging: ", exception3));
		}
		this.chosenId = WeaponManager.LastBoughtTag(this.viewedId);
		this.viewedId = (this.currentCategory != ShopNGUIController.CategoryNames.GearCategory ? this.chosenId : GearManager.NameForUpgrade(GearManager.HolderQuantityForID(this.viewedId), GearManager.CurrentNumberOfUphradesForGear(GearManager.HolderQuantityForID(this.viewedId))));
		this.UpdateIcon(this.currentCategory, true);
		this.ReloadCarousel(null);
		this.ChooseCarouselItem(this.viewedId, false, true);
		Resources.UnloadUnusedAssets();
		if (!this.inGame && this.currentCategory == ShopNGUIController.CategoryNames.CapesCategory && this.viewedId.Equals("cape_Custom"))
		{
			FlurryPluginWrapper.LogEvent("Enable_Custom Cape");
			this.wholePrice.gameObject.SetActive(false);
			this.goToSM();
		}
	}

	public static void AddBoughtCurrency(string currency, int count)
	{
		if (currency == null)
		{
			UnityEngine.Debug.LogWarning("AddBoughtCurrency: currency == null");
			return;
		}
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log(string.Format("<color=#ff00ffff>AddBoughtCurrency {0} {1}</color>", currency, count));
		}
		Storager.setInt(string.Concat("BoughtCurrency", currency), Storager.getInt(string.Concat("BoughtCurrency", currency), false) + count, false);
	}

	private static int AddedNumberOfGearWhenBuyingPack(string id)
	{
		int num = GearManager.ItemsInPackForGear(GearManager.HolderQuantityForID(id));
		if (Storager.getInt(id, false) + num > GearManager.MaxCountForGear(id))
		{
			num = GearManager.MaxCountForGear(id) - Storager.getInt(id, false);
		}
		return num;
	}

	public static void AddModel(GameObject pref, ShopNGUIController.Action7<GameObject, Vector3, Vector3, string, float, int, int> act, ShopNGUIController.CategoryNames c, bool isButtonInGameGui = false, WeaponSounds wsForPos = null)
	{
		float single = 150f;
		Vector3 vector3 = Vector3.zero;
		Vector3 vector31 = Vector3.zero;
		GameObject child = null;
		int num = 0;
		int league = 0;
		string str = null;
		if (!ShopNGUIController.IsWeaponCategory(c))
		{
			switch (c)
			{
				case ShopNGUIController.CategoryNames.HatsCategory:
				case ShopNGUIController.CategoryNames.ArmorCategory:
				case ShopNGUIController.CategoryNames.CapesCategory:
				case ShopNGUIController.CategoryNames.BootsCategory:
				case ShopNGUIController.CategoryNames.GearCategory:
				case ShopNGUIController.CategoryNames.MaskCategory:
				{
					child = pref.transform.GetChild(0).gameObject;
					ShopPositionParams infoForNonWeaponItem = ItemDb.GetInfoForNonWeaponItem(pref.nameNoClone(), c);
					vector31 = infoForNonWeaponItem.rotationShop;
					single = infoForNonWeaponItem.scaleShop;
					vector3 = infoForNonWeaponItem.positionShop;
					str = infoForNonWeaponItem.shopName;
					num = infoForNonWeaponItem.tier;
					league = infoForNonWeaponItem.League;
					break;
				}
				case ShopNGUIController.CategoryNames.SkinsCategory:
				{
					child = UnityEngine.Object.Instantiate<GameObject>(pref);
					ShopPositionParams component = pref.GetComponent<ShopPositionParams>();
					vector31 = component.rotationShop;
					single = component.scaleShop;
					vector3 = component.positionShop;
					num = component.tier;
					break;
				}
			}
		}
		else
		{
			WeaponSounds weaponSound = wsForPos;
			single = weaponSound.scaleShop;
			vector3 = weaponSound.positionShop;
			vector31 = weaponSound.rotationShop;
			child = pref.GetComponent<InnerWeaponPars>().bonusPrefab;
			try
			{
				ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponSound.name.Replace("(Clone)", string.Empty));
				string str1 = WeaponUpgrades.TagOfFirstUpgrade(byPrefabName.Tag);
				ItemRecord byTag = ItemDb.GetByTag(str1);
				str = WeaponManager.AllWrapperPrefabs().First<WeaponSounds>((WeaponSounds weapon) => weapon.name == byTag.PrefabName).shopName;
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Error in getting shop name of first upgrade: ", exception));
				str = weaponSound.shopName;
			}
			num = weaponSound.tier;
		}
		Vector3 vector32 = Vector3.zero;
		GameObject gameObject = null;
		if (c == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			gameObject = child;
			vector32 = new Vector3(0f, -1f, 0f);
		}
		else if (child != null)
		{
			Material[] materialArray = null;
			Mesh mesh = null;
			SkinnedMeshRenderer skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();
			if (skinnedMeshRenderer == null)
			{
				SkinnedMeshRenderer[] componentsInChildren = child.GetComponentsInChildren<SkinnedMeshRenderer>(true);
				if (componentsInChildren != null && (int)componentsInChildren.Length > 0)
				{
					skinnedMeshRenderer = componentsInChildren[0];
				}
			}
			if (skinnedMeshRenderer == null)
			{
				MeshFilter meshFilter = child.GetComponent<MeshFilter>();
				MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
				if (meshFilter != null)
				{
					mesh = meshFilter.sharedMesh;
				}
				if (meshRenderer != null)
				{
					materialArray = meshRenderer.sharedMaterials;
				}
			}
			else
			{
				materialArray = skinnedMeshRenderer.sharedMaterials;
				mesh = skinnedMeshRenderer.sharedMesh;
			}
			if (materialArray != null && mesh != null)
			{
				gameObject = new GameObject();
				gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
				MeshRenderer meshRenderer1 = gameObject.AddComponent<MeshRenderer>();
				meshRenderer1.materials = materialArray;
				vector32 = -meshRenderer1.bounds.center;
			}
		}
		try
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject);
		}
		catch (Exception exception1)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception DisableLightProbesRecursively: ", exception1));
		}
		GameObject gameObject1 = new GameObject()
		{
			name = gameObject.name
		};
		gameObject.transform.localPosition = vector32;
		gameObject.transform.parent = gameObject1.transform;
		Player_move_c.SetLayerRecursively(gameObject1, LayerMask.NameToLayer("NGUIShop"));
		if (act != null)
		{
			act(gameObject1, vector3, vector31, str, single, num, league);
		}
	}

	private void AdjustCategoryButtonsForFilterMap()
	{
		List<int> nums;
		List<int> nums1 = new List<int>();
		if (SceneLoader.ActiveSceneName.Equals("Sniper"))
		{
			nums = new List<int>()
			{
				0,
				3,
				5
			};
			nums1 = nums;
		}
		else if (SceneLoader.ActiveSceneName.Equals("Knife"))
		{
			nums = new List<int>()
			{
				0,
				1,
				3,
				5,
				4
			};
			nums1 = nums;
		}
		else if (Defs.isHunger)
		{
			nums = new List<int>()
			{
				7
			};
			nums1 = nums;
		}
		for (int i = 0; i < (int)this.category.buttons.Length; i++)
		{
			this.category.buttons[i].onButton.GetComponent<BoxCollider>().enabled = (nums1.Contains(i) ? false : this.category.buttons[i].onButton.GetComponent<BoxCollider>().enabled);
			this.category.buttons[i].offButton.GetComponent<BoxCollider>().enabled = !nums1.Contains(i);
		}
	}

	private void Awake()
	{
		Action action2 = null;
		Action action3 = null;
		this.showHatButton.gameObject.SetActive(false);
		ShopNGUIController._ShowArmor = PlayerPrefs.GetInt("ShowArmorKeySetting", 1) == 1;
		ShopNGUIController._ShowHat = PlayerPrefs.GetInt("ShowHatKeySetting", 1) == 1;
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		this.timeToUpdateTempGunTime = Time.realtimeSinceStartup;
		if (this.category != null)
		{
			this.category.Clicked += new EventHandler<MultipleToggleEventArgs>((object sender, MultipleToggleEventArgs e) => {
				if (e != null)
				{
					if (Defs.isSoundFX && this.category.buttons != null && (int)this.category.buttons.Length > e.Num)
					{
						this.category.buttons[e.Num].offButton.GetComponent<UIPlaySound>().Play();
					}
					try
					{
						if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
						{
							if (e.Num == 4 && this.trainingState == ShopNGUIController.TrainingState.NotInSniperCategory)
							{
								AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Category_Sniper, true);
								this.setOnSniperRifle();
							}
							else if (e.Num != 4 && (this.trainingState == ShopNGUIController.TrainingState.OnSniperRifle || this.trainingState == ShopNGUIController.TrainingState.InSniperCategoryNotOnSniperRifle))
							{
								this.setNotInSniperCategory();
							}
							else if (e.Num == 7 && this.trainingState == ShopNGUIController.TrainingState.NotInArmorCategory)
							{
								this.setOnArmor();
								AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Category_Armor, true);
							}
							else if (e.Num != 7 && (this.trainingState == ShopNGUIController.TrainingState.OnArmor || this.trainingState == ShopNGUIController.TrainingState.InArmorCategoryNotOnArmor))
							{
								this.setNotInArmorCategory();
							}
						}
						if (this.InTrainingAfterNoviceArmorRemoved)
						{
							if (e.Num == 7 && this.trainingStateRemoveNoviceArmor == ShopNGUIController.TrainingState.NotInArmorCategory)
							{
								this.setOnArmorRemovedNoviceArmor();
								AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Category_Armor, true);
							}
							else if (e.Num != 7 && (this.trainingStateRemoveNoviceArmor == ShopNGUIController.TrainingState.OnArmor || this.trainingStateRemoveNoviceArmor == ShopNGUIController.TrainingState.InArmorCategoryNotOnArmor))
							{
								this.setNotInArmorCategoryRemovedNoviceArmor();
							}
						}
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(string.Concat("Exceptio in training in category.Clicked: ", exception));
					}
					this.CategoryChosen((ShopNGUIController.CategoryNames)e.Num, null, false);
				}
			});
		}
		if (this.buy != null)
		{
			this.buy.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.BuyOrUpgradeWeapon(false));
		}
		if (this.buyGear != null)
		{
			this.buyGear.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.BuyOrUpgradeWeapon(false));
		}
		if (this.upgrade != null)
		{
			this.upgrade.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.BuyOrUpgradeWeapon(true));
		}
		if (this.upgradeGear != null)
		{
			this.upgradeGear.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.BuyOrUpgradeWeapon(true));
		}
		this.showArmorButton.IsChecked = !ShopNGUIController.ShowArmor;
		this.showHatButton.IsChecked = !ShopNGUIController.ShowHat;
		this.showArmorButtonTempArmor.IsChecked = !ShopNGUIController.ShowArmor;
		this.showHatButtonTempHat.IsChecked = !ShopNGUIController.ShowHat;
		Action showArmor = () => {
			ShopNGUIController.ShowArmor = !ShopNGUIController.ShowArmor;
			ShopNGUIController.SetPersArmorVisible(this.armorPoint);
			PlayerPrefs.SetInt("ShowArmorKeySetting", (!ShopNGUIController.ShowArmor ? 0 : 1));
		};
		Action showHat = () => {
			ShopNGUIController.ShowHat = !ShopNGUIController.ShowHat;
			ShopNGUIController.SetPersHatVisible(this.hatPoint);
			PlayerPrefs.SetInt("ShowHatKeySetting", (!ShopNGUIController.ShowHat ? 0 : 1));
		};
		this.showArmorButton.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			showArmor();
			this.showArmorButtonTempArmor.SetCheckedWithoutEvent(!ShopNGUIController.ShowArmor);
		});
		this.showHatButton.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			showHat();
			this.showHatButtonTempHat.SetCheckedWithoutEvent(!ShopNGUIController.ShowHat);
		});
		this.showArmorButtonTempArmor.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			showArmor();
			this.showArmorButton.SetCheckedWithoutEvent(!ShopNGUIController.ShowArmor);
		});
		this.showHatButtonTempHat.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			showHat();
			this.showHatButton.SetCheckedWithoutEvent(!ShopNGUIController.ShowHat);
		});
		ShopNGUIController.sharedShop = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.ActiveObject.SetActive(false);
		if (this.coinShopButton != null)
		{
			this.coinShopButton.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
				if (Time.realtimeSinceStartup - this.timeOfEnteringShopForProtectFromPressingCoinsButton < 0.5f)
				{
					return;
				}
				if (BankController.Instance != null)
				{
					if (BankController.Instance.InterfaceEnabledCoroutineLocked)
					{
						UnityEngine.Debug.LogWarning("InterfaceEnabledCoroutineLocked");
						return;
					}
					EventHandler interfaceEnabledCoroutineLocked = null;
					interfaceEnabledCoroutineLocked = (object sender_, EventArgs e_) => {
						if (BankController.Instance.InterfaceEnabledCoroutineLocked)
						{
							UnityEngine.Debug.LogWarning("InterfaceEnabledCoroutineLocked");
							return;
						}
						BankController.Instance.BackRequested -= interfaceEnabledCoroutineLocked;
						BankController.Instance.InterfaceEnabled = false;
						ShopNGUIController.GuiActive = true;
					};
					BankController.Instance.BackRequested += interfaceEnabledCoroutineLocked;
					BankController.Instance.InterfaceEnabled = true;
					ShopNGUIController.GuiActive = false;
				}
			});
		}
		if (this.backButton != null)
		{
			this.backButton.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => base.StartCoroutine(this.BackAfterDelay()));
		}
		UIButton[] uIButtonArray = this.equips;
		for (int i = 0; i < (int)uIButtonArray.Length; i++)
		{
			UIButton uIButton = uIButtonArray[i];
			uIButton.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
				if (Defs.isSoundFX)
				{
					uIButton.GetComponent<UIPlaySound>().Play();
				}
				if (this.WeaponCategory)
				{
					string u003cu003ef_this = WeaponManager.LastBoughtTag(this.viewedId);
					if (u003cu003ef_this == null && WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId))
					{
						u003cu003ef_this = this.viewedId;
					}
					if (u003cu003ef_this == null)
					{
						return;
					}
					string prefabName = ItemDb.GetByTag(u003cu003ef_this).PrefabName;
					Weapon weapon1 = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().FirstOrDefault<Weapon>((Weapon weapon) => weapon.weaponPrefab.nameNoClone() == prefabName);
					WeaponManager.sharedManager.EquipWeapon(weapon1, true, true);
					WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
					if (this.equipAction != null)
					{
						this.equipAction(u003cu003ef_this);
					}
					this.chosenId = u003cu003ef_this;
					this.UpdateIcon(this.currentCategory, false);
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted && this.trainingState == ShopNGUIController.TrainingState.OnSniperRifle && u003cu003ef_this != null && u003cu003ef_this == WeaponTags.HunterRifleTag)
					{
						if (Storager.getInt("Training.NoviceArmorUsedKey", false) != 1)
						{
							this.setNotInArmorCategory();
							AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Equip_Sniper, true);
						}
						else
						{
							this.setBackBlinking();
						}
					}
					if (this.InTrainingAfterNoviceArmorRemoved)
					{
						this.HideAllTrainingInterfaceRemovedNoviceArmor();
						this.InTrainingAfterNoviceArmorRemoved = false;
						this.HandleActionsUUpdated();
					}
				}
				else if (this.WearCategory)
				{
					string str = WeaponManager.LastBoughtTag(this.viewedId);
					if (str != null)
					{
						this.EquipWear(str);
					}
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
					{
						if (this.trainingState == ShopNGUIController.TrainingState.OnArmor)
						{
							this.setBackBlinking();
							AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Equip_Armor, true);
						}
					}
					else if (this.InTrainingAfterNoviceArmorRemoved)
					{
						this.HideAllTrainingInterfaceRemovedNoviceArmor();
						this.InTrainingAfterNoviceArmorRemoved = false;
						this.HandleActionsUUpdated();
					}
				}
				else if (this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory)
				{
					this.SetSkinAsCurrent(this.viewedId);
					this.UpdateIcon(this.currentCategory, true);
				}
				this.UpdateButtons();
			});
		}
		this.unequip.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
			if (Defs.isSoundFX)
			{
				this.unequip.GetComponent<UIPlaySound>().Play();
			}
			if (this.WearCategory)
			{
				ShopNGUIController.UnequipCurrentWearInCategory(this.currentCategory, this.inGame);
			}
			this.UpdateButtons();
		});
		this.enable.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
			if (this.viewedId == null || !this.viewedId.Equals("CustomSkinID"))
			{
				if (this.viewedId != null && this.viewedId.Equals("cape_Custom"))
				{
					this.BuyOrUpgradeWeapon(false);
				}
				return;
			}
			ItemPrice itemPrice = ShopNGUIController.currentPrice(this.viewedId, this.currentCategory, false, true);
			int price = itemPrice.Price;
			string currency = itemPrice.Currency;
			GameObject u003cu003ef_this = this.mainPanel;
			ItemPrice itemPrice1 = itemPrice;
			Action action = () => {
				if (Defs.isSoundFX)
				{
					this.enable.GetComponent<UIPlaySound>().Play();
				}
				if (ShopNGUIController.GunBought != null)
				{
					ShopNGUIController.GunBought();
				}
				FlurryPluginWrapper.LogPurchaseByModes(ShopNGUIController.CategoryNames.SkinsCategory, Defs.SkinsMakerInProfileBought, 1, false);
				FlurryPluginWrapper.LogPurchasesPoints(false);
				FlurryPluginWrapper.LogPurchaseByPoints(ShopNGUIController.CategoryNames.SkinsCategory, Defs.SkinsMakerInProfileBought, 1);
				this.LogShopPurchasesTotalAndPayingNonPaying(Defs.SkinsMakerInProfileBought);
				string item = FlurryEvents.shopCategoryToLogSalesNamesMapping[ShopNGUIController.CategoryNames.SkinsCategory];
				AnalyticsStuff.LogSales(Defs.SkinsMakerInProfileBought, item, false);
				AnalyticsFacade.InAppPurchase(Defs.SkinsMakerInProfileBought, item, 1, price, currency);
				Storager.setInt(Defs.SkinsMakerInProfileBought, 1, true);
				ShopNGUIController.SynchronizeAndroidPurchases("Custom skin");
				FlurryPluginWrapper.LogEvent("Enable_Custom Skin");
				this.wholePrice.gameObject.SetActive(false);
				if (!this.inGame)
				{
					this.goToSM();
				}
			};
			if (action2 == null)
			{
				action2 = () => FlurryPluginWrapper.LogEvent("Try_Enable_Custom Skin");
			}
			Action u003cu003ef_amu0024cache3 = action2;
			Action action1 = () => this.PlayWeaponAnimation();
			if (action3 == null)
			{
				action3 = () => ShopNGUIController.SetBankCamerasEnabled();
			}
			ShopNGUIController.TryToBuy(u003cu003ef_this, itemPrice1, action, u003cu003ef_amu0024cache3, null, action1, action3, () => this.SetOtherCamerasEnabled(false));
			this.UpdateButtons();
		});
		this.create.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
			ButtonClickSound.Instance.PlayClick();
			if (!this.inGame)
			{
				this.goToSM();
			}
		});
		this.edit.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
			ButtonClickSound.Instance.PlayClick();
			if (!this.inGame)
			{
				this.goToSM();
			}
		});
		this.delete.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
			string u003cu003ef_this = this.viewedId;
			InfoWindowController.ShowDialogBox(LocalizationStore.Get("Key_1693"), () => {
				ButtonClickSound.Instance.PlayClick();
				string str = SkinsController.currentSkinNameForPers;
				if (u003cu003ef_this != null)
				{
					SkinsController.DeleteUserSkin(u003cu003ef_this);
					if (u003cu003ef_this.Equals(str))
					{
						this.SetSkinAsCurrent("0");
						this.UpdateIcon(ShopNGUIController.CategoryNames.SkinsCategory, false);
					}
				}
				this.ReloadCarousel(SkinsController.currentSkinNameForPers ?? "0");
			}, null);
		});
		this.hats.AddRange(Resources.LoadAll<ShopPositionParams>("Hats_Info"));
		this.sort(this.hats, 6);
		this.armor.AddRange(Resources.LoadAll<ShopPositionParams>("Armor_Info"));
		this.sort(this.armor, 7);
		this.capes.AddRange(Resources.LoadAll<ShopPositionParams>("Capes_Info"));
		this.sort(this.capes, 9);
		this.masks.AddRange(Resources.LoadAll<ShopPositionParams>("Masks_Info"));
		this.sort(this.masks, 12);
		this.boots.AddRange(Resources.LoadAll<ShopPositionParams>("Shop_Boots_Info"));
		this.sort(this.boots, 10);
		this.pixlMan = Resources.Load<GameObject>("PixlManForSkins");
		if (!Device.IsLoweMemoryDevice)
		{
			this._onPersArmorRefs = Resources.LoadAll<GameObject>("Armor_Shop");
		}
		if (Device.isPixelGunLow)
		{
			this._refOnLowPolyArmor = Resources.Load<GameObject>("Armor_Low");
			this._refsOnLowPolyArmorMaterials = Resources.LoadAll<Material>("LowPolyArmorMaterials");
		}
	}

	[DebuggerHidden]
	public IEnumerator BackAfterDelay()
	{
		ShopNGUIController.u003cBackAfterDelayu003ec__Iterator1AA variable = null;
		return variable;
	}

	private static List<Camera> BankRelatedCameras()
	{
		List<Camera> list = BankController.Instance.GetComponentsInChildren<Camera>(true).ToList<Camera>();
		if (FreeAwardController.Instance != null && FreeAwardController.Instance.renderCamera != null)
		{
			list.Add(FreeAwardController.Instance.renderCamera);
		}
		return list;
	}

	[DebuggerHidden]
	private IEnumerator Blink(string[] images)
	{
		ShopNGUIController.u003cBlinku003ec__Iterator1A7 variable = null;
		return variable;
	}

	private void BuyOrUpgradeWeapon(bool upgradeNotBuy = false)
	{
		string str;
		if (this.currentCategory != ShopNGUIController.CategoryNames.GearCategory)
		{
			str = this.viewedId;
		}
		else
		{
			str = (!upgradeNotBuy ? this.IDForCurrentGear : this.NextUpgradeIDForCurrentGear);
		}
		string item = str;
		string str1 = item;
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(str1))
		{
			item = WeaponManager.tagToStoreIDMapping[WeaponManager.FirstUnboughtOrForOurTier(str1)];
		}
		if (this.WearCategory)
		{
			item = WeaponManager.FirstUnboughtTag(item);
			str1 = item;
		}
		if (item == null)
		{
			return;
		}
		ItemPrice itemPrice = ShopNGUIController.currentPrice(this.viewedId, this.currentCategory, upgradeNotBuy, true);
		ShopNGUIController.TryToBuy(this.mainPanel, itemPrice, () => {
			if (Defs.isSoundFX)
			{
				((!upgradeNotBuy ? this.buy : this.upgrade)).GetComponent<UIPlaySound>().Play();
			}
			this.ActualBuy(item, str1, itemPrice);
		}, () => {
			if (this.currentCategory == ShopNGUIController.CategoryNames.CapesCategory && this.viewedId.Equals("cape_Custom"))
			{
				FlurryPluginWrapper.LogEvent("Try_Enable_Custom Cape");
			}
		}, null, () => this.PlayWeaponAnimation(), () => ShopNGUIController.SetBankCamerasEnabled(), () => {
			this.SetOtherCamerasEnabled(false);
			base.StartCoroutine(this.ReloadAfterEditing(this.viewedId, true));
		});
	}

	public void CategoryChosen(ShopNGUIController.CategoryNames i, string idToSet = null, bool initial = false)
	{
		ShopNGUIController.CategoryNames categoryName;
		WeaponManager.ClearCachedInnerPrefabs();
		if (!initial)
		{
			switch (this.currentCategory)
			{
				case ShopNGUIController.CategoryNames.HatsCategory:
				case ShopNGUIController.CategoryNames.ArmorCategory:
				case ShopNGUIController.CategoryNames.CapesCategory:
				case ShopNGUIController.CategoryNames.BootsCategory:
				case ShopNGUIController.CategoryNames.MaskCategory:
				{
					this.viewedId = this._CurrentEquippedWear;
					break;
				}
				case ShopNGUIController.CategoryNames.SkinsCategory:
				{
					if (SkinsController.currentSkinNameForPers != null)
					{
						this.viewedId = SkinsController.currentSkinNameForPers;
					}
					else if (SkinsController.skinsForPers != null && SkinsController.skinsForPers.Keys.Count > 0)
					{
						Dictionary<string, Texture2D>.KeyCollection.Enumerator enumerator = SkinsController.skinsForPers.Keys.GetEnumerator();
						try
						{
							if (enumerator.MoveNext())
							{
								this.viewedId = enumerator.Current;
							}
						}
						finally
						{
							((IDisposable)(object)enumerator).Dispose();
						}
					}
					break;
				}
				case ShopNGUIController.CategoryNames.GearCategory:
				{
					break;
				}
				default:
				{
					goto case ShopNGUIController.CategoryNames.GearCategory;
				}
			}
			if (this.WearCategory || this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory)
			{
				this.UpdatePersWithNewItem();
			}
		}
		this.currentCategory = i;
		!(this.highlightedCarouselObject != null);
		this.highlightedCarouselObject = null;
		if (!this.WeaponCategory)
		{
			switch (this.currentCategory)
			{
				case ShopNGUIController.CategoryNames.HatsCategory:
				{
					string str = null;
					try
					{
						Dictionary<Wear.LeagueItemState, List<string>> leagueItemStates = Wear.LeagueItems();
						str = (
							from item in leagueItemStates[Wear.LeagueItemState.Open].Union<string>(leagueItemStates[Wear.LeagueItemState.Purchased])
							orderby Wear.LeagueForWear(item, this.currentCategory)
							select item).FirstOrDefault<string>();
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(string.Concat("CategoryChoosen: exception in getting firstLeagueItem: ", exception));
					}
					string str1 = str ?? WeaponManager.FirstUnboughtTag("hat_Headphones");
					int num = this.hats.FindIndex((ShopPositionParams go) => go.name.Equals(str1));
					this.viewedId = (this._CurrentEquippedWear == null || this._CurrentNoneEquipped == null || this._CurrentEquippedWear.Equals(this._CurrentNoneEquipped) || WeaponManager.LastBoughtTag(this._CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(this._CurrentEquippedWear).Equals(this._CurrentEquippedWear) ? this.hats[(num <= -1 ? this.hats.Count - 1 : num)].name : this._CurrentEquippedWear);
					break;
				}
				case ShopNGUIController.CategoryNames.ArmorCategory:
				{
					this.viewedId = (this._CurrentEquippedWear == null || this._CurrentNoneEquipped == null || this._CurrentEquippedWear.Equals(this._CurrentNoneEquipped) || WeaponManager.LastBoughtTag(this._CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(this._CurrentEquippedWear).Equals(this._CurrentEquippedWear) ? WeaponManager.FirstUnboughtTag(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0][0]) : this._CurrentEquippedWear);
					this.scrollViewPanel.transform.localPosition = Vector3.zero;
					this.scrollViewPanel.clipOffset = new Vector2(0f, 0f);
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
					{
						this.viewedId = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
						idToSet = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
					}
					if (this.InTrainingAfterNoviceArmorRemoved)
					{
						this.viewedId = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
						idToSet = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
					}
					break;
				}
				case ShopNGUIController.CategoryNames.SkinsCategory:
				{
					if (SkinsController.currentSkinNameForPers != null)
					{
						this.viewedId = SkinsController.currentSkinNameForPers;
					}
					else if (SkinsController.skinsForPers != null && SkinsController.skinsForPers.Keys.Count > 0)
					{
						Dictionary<string, Texture2D>.KeyCollection.Enumerator enumerator1 = SkinsController.skinsForPers.Keys.GetEnumerator();
						try
						{
							if (enumerator1.MoveNext())
							{
								this.viewedId = enumerator1.Current;
							}
						}
						finally
						{
							((IDisposable)(object)enumerator1).Dispose();
						}
					}
					break;
				}
				case ShopNGUIController.CategoryNames.CapesCategory:
				{
					this.viewedId = (this._CurrentEquippedWear == null || this._CurrentNoneEquipped == null || this._CurrentEquippedWear.Equals(this._CurrentNoneEquipped) || WeaponManager.LastBoughtTag(this._CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(this._CurrentEquippedWear).Equals(this._CurrentEquippedWear) ? WeaponManager.LastBoughtTag(this.capes[1].name) ?? this.capes[1].name : this._CurrentEquippedWear);
					break;
				}
				case ShopNGUIController.CategoryNames.BootsCategory:
				{
					this.viewedId = (this._CurrentEquippedWear == null || this._CurrentNoneEquipped == null || this._CurrentEquippedWear.Equals(this._CurrentNoneEquipped) || WeaponManager.LastBoughtTag(this._CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(this._CurrentEquippedWear).Equals(this._CurrentEquippedWear) ? WeaponManager.LastBoughtTag(this.boots[0].name) ?? this.boots[0].name : this._CurrentEquippedWear);
					break;
				}
				case ShopNGUIController.CategoryNames.GearCategory:
				{
					int num1 = GearManager.CurrentNumberOfUphradesForGear(GearManager.InvisibilityPotion);
					this.viewedId = GearManager.NameForUpgrade(GearManager.InvisibilityPotion, (num1 >= GearManager.NumOfGearUpgrades ? num1 : num1));
					break;
				}
				case ShopNGUIController.CategoryNames.MaskCategory:
				{
					this.viewedId = (this._CurrentEquippedWear == null || this._CurrentNoneEquipped == null || this._CurrentEquippedWear.Equals(this._CurrentNoneEquipped) || WeaponManager.LastBoughtTag(this._CurrentEquippedWear) == null || !WeaponManager.LastBoughtTag(this._CurrentEquippedWear).Equals(this._CurrentEquippedWear) ? WeaponManager.LastBoughtTag(this.masks[0].name) ?? this.masks[0].name : this._CurrentEquippedWear);
					break;
				}
			}
		}
		else
		{
			this.chosenId = ShopNGUIController._CurrentWeaponSetIDs()[(int)i];
			this.viewedId = idToSet;
			if (this.viewedId != null && this.chosenId != null && this.viewedId.Equals(this.chosenId) && WeaponManager.sharedManager != null && WeaponManager.sharedManager.FilteredShopLists.Count > (int)i && !WeaponManager.sharedManager.FilteredShopLists[(Int32)i].Find((GameObject go) => go.name.Equals(this.viewedId)))
			{
				this.viewedId = null;
			}
			this.viewedId = this.chosenId;
			if (this.viewedId == null)
			{
				string str2 = ShopNGUIController.TempGunOrHighestDPSGun(i, out categoryName);
				if (i != categoryName)
				{
					this.viewedId = ShopNGUIController.TemppOrHighestDPSGunInCategory((int)i);
				}
				else
				{
					this.viewedId = str2;
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
			{
				if (this.trainingState > ShopNGUIController.TrainingState.OnArmor && this.currentCategory == ShopNGUIController.CategoryNames.ArmorCategory)
				{
					this.viewedId = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
					idToSet = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
				}
				else if (this.currentCategory == ShopNGUIController.CategoryNames.SniperCategory)
				{
					this.viewedId = WeaponTags.HunterRifleTag;
					idToSet = WeaponTags.HunterRifleTag;
				}
			}
			if (this.InTrainingAfterNoviceArmorRemoved && this.currentCategory == ShopNGUIController.CategoryNames.ArmorCategory)
			{
				this.viewedId = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
				idToSet = WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1";
			}
		}
		this.armorWearProperties.SetActive(this.currentCategory == ShopNGUIController.CategoryNames.ArmorCategory);
		this.nonArmorWearProperties.SetActive((!ShopNGUIController.IsWearCategory(this.currentCategory) ? false : this.currentCategory != ShopNGUIController.CategoryNames.ArmorCategory));
		this.gearProperties.SetActive(this.currentCategory == ShopNGUIController.CategoryNames.GearCategory);
		this.skinProperties.SetActive(this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory);
		this.border.SetActive(this.currentCategory != ShopNGUIController.CategoryNames.SkinsCategory);
		this.ReloadCarousel(idToSet);
		this.SetCamera();
		if (!ShopNGUIController.IsWeaponCategory(i) && this.weapon == null)
		{
			this.SetWeapon(ShopNGUIController._CurrentWeaponSetIDs()[0] ?? WeaponManager._initialWeaponName);
		}
		if (this.currentCategory != ShopNGUIController.CategoryNames.SkinsCategory)
		{
			this.shopCarouselCollider.center = new Vector3(0f, 0f, 0f);
			BoxCollider vector3 = this.shopCarouselCollider;
			float single = this.shopCarouselCollider.size.x;
			Vector3 vector31 = this.shopCarouselCollider.size;
			vector3.size = new Vector3(single, 252f, vector31.z);
		}
		else
		{
			this.shopCarouselCollider.center = new Vector3(0f, -40f, 0f);
			BoxCollider boxCollider = this.shopCarouselCollider;
			float single1 = this.shopCarouselCollider.size.x;
			Vector3 vector32 = this.shopCarouselCollider.size;
			boxCollider.size = new Vector3(single1, 363f, vector32.z);
		}
	}

	private void CheckCenterItemChanging()
	{
		if (!this.scrollViewPanel.cachedGameObject.activeInHierarchy)
		{
			return;
		}
		Transform transforms = this.scrollViewPanel.cachedTransform;
		this.itemIndex = -1;
		int num = (int)this.wrapContent.cellWidth;
		int num1 = this.wrapContent.transform.childCount;
		if (transforms.localPosition.x > 0f)
		{
			this.itemIndex = 0;
		}
		else if (transforms.localPosition.x >= (float)(-1 * num * num1))
		{
			float single = transforms.localPosition.x;
			Vector3 vector3 = transforms.localPosition;
			this.itemIndex = -1 * Mathf.RoundToInt((single - (float)(Mathf.CeilToInt(vector3.x / (float)num / (float)num1) * num * num1)) / (float)num);
		}
		else
		{
			this.itemIndex = num1 - 1;
		}
		this.itemIndex = Mathf.Clamp(this.itemIndex, 0, num1 - 1);
		if (this.itemIndex >= 0 && this.itemIndex < this.wrapContent.transform.childCount)
		{
			GameObject child = this.wrapContent.transform.GetChild(this.itemIndex).gameObject;
			if (!this.EnableConfigurePos)
			{
				this.HandleCarouselCentering(child);
			}
		}
	}

	public void ChooseCarouselItem(string itemID, bool moveCarousel = false, bool setManuallyToChosen = false)
	{
		if (itemID == null)
		{
			if (this.WeaponCategory)
			{
				this.UpdatePersWithNewItem();
			}
			return;
		}
		ShopCarouselElement[] componentsInChildren = this.wrapContent.GetComponentsInChildren<ShopCarouselElement>(true) ?? new ShopCarouselElement[0];
		ShopCarouselElement[] shopCarouselElementArray = componentsInChildren;
		int num = 0;
		while (num < (int)shopCarouselElementArray.Length)
		{
			ShopCarouselElement shopCarouselElement = shopCarouselElementArray[num];
			if (!shopCarouselElement.itemID.Equals(itemID))
			{
				num++;
			}
			else
			{
				if (moveCarousel || setManuallyToChosen)
				{
					SpringPanel component = this.scrollViewPanel.GetComponent<SpringPanel>();
					if (component != null)
					{
						UnityEngine.Object.Destroy(component);
					}
					if (this.scrollViewPanel.gameObject.activeInHierarchy)
					{
						UIScrollView uIScrollView = this.scrollViewPanel.GetComponent<UIScrollView>();
						Vector3 vector3 = shopCarouselElement.transform.localPosition;
						float single = -vector3.x - this.scrollViewPanel.transform.localPosition.x;
						float single1 = this.scrollViewPanel.transform.localPosition.y;
						Vector3 vector31 = this.scrollViewPanel.transform.localPosition;
						uIScrollView.MoveRelative(new Vector3(single, single1, vector31.z));
					}
					this.wrapContent.Reposition();
				}
				this.viewedId = itemID;
				this.UpdatePersWithNewItem();
				this.UpdateButtons();
				this.UpdateItemParameters();
				this.caption.text = shopCarouselElement.readableName ?? string.Empty;
				this.caption.applyGradient = TempItemsController.PriceCoefs.ContainsKey(itemID);
				try
				{
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
					{
						if (this.trainingState == ShopNGUIController.TrainingState.OnSniperRifle && this.viewedId != null && this.viewedId != WeaponTags.HunterRifleTag)
						{
							this.setInSniperCategoryNotOnSniperRifle();
						}
						else if (this.trainingState != ShopNGUIController.TrainingState.InSniperCategoryNotOnSniperRifle || this.viewedId == null || !(this.viewedId == WeaponTags.HunterRifleTag))
						{
							if (this.trainingState == ShopNGUIController.TrainingState.OnArmor && this.viewedId != null)
							{
								if (this.viewedId == (WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1"))
								{
									goto Label1;
								}
								this.setInArmorCategoryNotOnArmor();
								goto Label0;
							}
						Label1:
							if (this.trainingState == ShopNGUIController.TrainingState.InArmorCategoryNotOnArmor && this.viewedId != null)
							{
								if (this.viewedId == (WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1"))
								{
									this.setOnArmor();
								}
							}
						}
						else
						{
							this.setOnSniperRifle();
						}
					}
				Label0:
					if (this.InTrainingAfterNoviceArmorRemoved)
					{
						if (this.trainingStateRemoveNoviceArmor == ShopNGUIController.TrainingState.OnArmor && this.viewedId != null)
						{
							if (this.viewedId == (WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1"))
							{
								goto Label3;
							}
							this.setInArmorCategoryNotOnArmorRemovedNoviceArmor();
							goto Label2;
						}
					Label3:
						if (this.trainingStateRemoveNoviceArmor == ShopNGUIController.TrainingState.InArmorCategoryNotOnArmor && this.viewedId != null)
						{
							if (this.viewedId == (WeaponManager.LastBoughtTag("Armor_Army_1") ?? "Armor_Army_1"))
							{
								this.setOnArmorRemovedNoviceArmor();
							}
						}
					}
				Label2:
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(string.Concat("Exception in training in ChooseCarouselItem: ", exception));
				}
				break;
			}
		}
	}

	public static ItemPrice currentPrice(string itemId, ShopNGUIController.CategoryNames currentCategory, bool upgradeNotBuy = false, bool useDiscounts = true)
	{
		bool flag;
		ItemPrice itemPrice;
		try
		{
			if (itemId != null)
			{
				string item = itemId;
				if (itemId != null && WeaponManager.tagToStoreIDMapping.ContainsKey(itemId))
				{
					item = WeaponManager.tagToStoreIDMapping[WeaponManager.FirstUnboughtOrForOurTier(itemId)];
				}
				if (currentCategory == ShopNGUIController.CategoryNames.SkinsCategory && SkinsController.shopKeyFromNameSkin.ContainsKey(item))
				{
					item = SkinsController.shopKeyFromNameSkin[item];
				}
				if (currentCategory == ShopNGUIController.CategoryNames.GearCategory)
				{
					item = (!upgradeNotBuy ? GearManager.OneItemIDForGear(GearManager.HolderQuantityForID(item), GearManager.CurrentNumberOfUphradesForGear(item)) : GearManager.UpgradeIDForGear(GearManager.HolderQuantityForID(item), GearManager.CurrentNumberOfUphradesForGear(item) + 1));
				}
				if (ShopNGUIController.IsWearCategory(currentCategory))
				{
					item = WeaponManager.FirstUnboughtTag(item);
				}
				string str = (ShopNGUIController.IsWeaponCategory(currentCategory) || ShopNGUIController.IsWearCategory(currentCategory) ? WeaponManager.FirstUnboughtOrForOurTier(itemId) : itemId);
				ItemPrice priceByShopId = ItemDb.GetPriceByShopId(item) ?? new ItemPrice(10, "Coins");
				int price = priceByShopId.Price;
				if (useDiscounts)
				{
					int num = ShopNGUIController.DiscountFor(str, out flag);
					if (num > 0)
					{
						float single = (float)num;
						price = Math.Max((int)((float)price * 0.01f), Mathf.RoundToInt((float)price * (1f - single / 100f)));
						if (flag)
						{
							price = (price % 5 >= 3 ? price + (5 - price % 5) : price - price % 5);
						}
					}
				}
				if (currentCategory == ShopNGUIController.CategoryNames.GearCategory && !upgradeNotBuy)
				{
					price *= GearManager.ItemsInPackForGear(GearManager.HolderQuantityForID(item));
				}
				itemPrice = new ItemPrice(price, priceByShopId.Currency);
			}
			else
			{
				itemPrice = new ItemPrice(0, "Coins");
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in currentPrice: ", exception));
			itemPrice = new ItemPrice(0, "Coins");
		}
		return itemPrice;
	}

	private void DisableGunflashes(GameObject root)
	{
		if (root == null)
		{
			return;
		}
		if (root.name.Equals("GunFlash"))
		{
			root.SetActive(false);
		}
		IEnumerator enumerator = root.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (null != current)
				{
					this.DisableGunflashes(current.gameObject);
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public static void DisableLightProbesRecursively(GameObject w)
	{
		Player_move_c.PerformActionRecurs(w, (Transform t) => {
			MeshRenderer component = t.GetComponent<MeshRenderer>();
			SkinnedMeshRenderer skinnedMeshRenderer = t.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				component.useLightProbes = false;
			}
			if (skinnedMeshRenderer != null)
			{
				skinnedMeshRenderer.useLightProbes = false;
			}
		});
	}

	[DebuggerHidden]
	private IEnumerator DisableStub()
	{
		ShopNGUIController.u003cDisableStubu003ec__Iterator1AC variable = null;
		return variable;
	}

	public static int DiscountFor(string itemTag, out bool onlyServerDiscount)
	{
		int num;
		try
		{
			if (itemTag != null)
			{
				bool flag = false;
				bool flag1 = false;
				float single = 100f;
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(itemTag))
				{
					single -= (float)WeaponManager.sharedManager.DiscountForTryGun(itemTag);
					single = Math.Max(1f, single);
					single = Math.Min(100f, single);
					flag1 = true;
				}
				single /= 100f;
				float value = 100f;
				if (!flag1 && PromoActionsManager.sharedManager.discounts.ContainsKey(itemTag) && PromoActionsManager.sharedManager.discounts[itemTag].Count > 0)
				{
					SaltedInt item = PromoActionsManager.sharedManager.discounts[itemTag][0];
					value -= (float)item.Value;
					value = Math.Max(10f, value);
					value = Math.Min(100f, value);
					flag = true;
				}
				value /= 100f;
				onlyServerDiscount = (flag1 ? false : flag);
				if (flag1 || flag)
				{
					float single1 = Mathf.Clamp(single * value, 0.01f, 1f);
					int num1 = Mathf.RoundToInt((1f - single1) * 100f);
					if (onlyServerDiscount && num1 % 5 != 0)
					{
						num1 = 5 * (num1 / 5 + 1);
					}
					num1 = Math.Min(num1, 99);
					num = num1;
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				UnityEngine.Debug.LogError("DiscountFor: itemTag == null");
				onlyServerDiscount = false;
				num = 0;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in DiscountFor: ", exception));
			onlyServerDiscount = false;
			num = 0;
		}
		return num;
	}

	public void EquipWear(string tg)
	{
		ShopNGUIController.EquipWearInCategory(tg, this.currentCategory, this.inGame);
	}

	private static void EquipWearInCategory(string tg, ShopNGUIController.CategoryNames cat, bool inGameLocal)
	{
		bool flag = !Defs.isMulti;
		string str = Storager.getString(ShopNGUIController.SnForWearCategory(cat), false);
		Player_move_c component = null;
		if (inGameLocal)
		{
			if (flag)
			{
				if (!SceneLoader.ActiveSceneName.Equals("LevelComplete") && !SceneLoader.ActiveSceneName.Equals("ChooseLevel"))
				{
					component = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
				}
			}
			else if (WeaponManager.sharedManager.myPlayer != null)
			{
				component = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		ShopNGUIController.SetAsEquippedAndSendToServer(tg, cat);
		ShopNGUIController.sharedShop.SetWearForCategory(cat, tg);
		if (ShopNGUIController.sharedShop.wearEquipAction != null)
		{
			ShopNGUIController.sharedShop.wearEquipAction(cat, str ?? ShopNGUIController.NoneEquippedForWearCategory(cat), ShopNGUIController.sharedShop.WearForCat(cat));
		}
		if (cat == ShopNGUIController.CategoryNames.BootsCategory && inGameLocal && component != null)
		{
			if (!str.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.bootsMethods.ContainsKey(str))
			{
				Wear.bootsMethods[str].Value(component, new Dictionary<string, object>());
			}
			if (Wear.bootsMethods.ContainsKey(tg))
			{
				Wear.bootsMethods[tg].Key(component, new Dictionary<string, object>());
			}
		}
		if (cat == ShopNGUIController.CategoryNames.CapesCategory && inGameLocal && component != null)
		{
			if (!str.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.capesMethods.ContainsKey(str))
			{
				Wear.capesMethods[str].Value(component, new Dictionary<string, object>());
			}
			if (Wear.capesMethods.ContainsKey(tg))
			{
				Wear.capesMethods[tg].Key(component, new Dictionary<string, object>());
			}
		}
		if (cat == ShopNGUIController.CategoryNames.HatsCategory && inGameLocal && component != null)
		{
			if (!str.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.hatsMethods.ContainsKey(str))
			{
				Wear.hatsMethods[str].Value(component, new Dictionary<string, object>());
			}
			if (Wear.hatsMethods.ContainsKey(tg))
			{
				Wear.hatsMethods[tg].Key(component, new Dictionary<string, object>());
			}
		}
		if (cat == ShopNGUIController.CategoryNames.ArmorCategory && inGameLocal && component != null)
		{
			if (!str.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.armorMethods.ContainsKey(str))
			{
				Wear.armorMethods[str].Value(component, new Dictionary<string, object>());
			}
			if (Wear.armorMethods.ContainsKey(tg))
			{
				Wear.armorMethods[tg].Key(component, new Dictionary<string, object>());
			}
		}
		if (ShopNGUIController.GuiActive)
		{
			ShopNGUIController.sharedShop.UpdateButtons();
			ShopNGUIController.sharedShop.UpdateIcon(cat, true);
		}
	}

	public static void EquipWearInCategoryIfNotEquiped(string tg, ShopNGUIController.CategoryNames cat, bool inGameLocal)
	{
		if (!Storager.hasKey(ShopNGUIController.SnForWearCategory(cat)))
		{
			Storager.setString(ShopNGUIController.SnForWearCategory(cat), ShopNGUIController.NoneEquippedForWearCategory(cat), false);
		}
		if (!Storager.getString(ShopNGUIController.SnForWearCategory(cat), false).Equals(tg))
		{
			ShopNGUIController.EquipWearInCategory(tg, cat, inGameLocal);
		}
	}

	public List<GameObject> FillModelsList(ShopNGUIController.CategoryNames c)
	{
		Func<ShopNGUIController.CategoryNames, Comparison<GameObject>> func = (ShopNGUIController.CategoryNames cn) => (GameObject lh, GameObject rh) => {
			List<string> strs = null;
			List<string> strs1 = null;
			foreach (List<string> item in Wear.wear[cn])
			{
				if (item.Contains(lh.name))
				{
					strs = item;
				}
				if (!item.Contains(rh.name))
				{
					continue;
				}
				strs1 = item;
			}
			if (strs == null || strs1 == null)
			{
				return 0;
			}
			if (strs == strs1)
			{
				return strs.IndexOf(lh.name) - strs.IndexOf(rh.name);
			}
			return Wear.wear[cn].IndexOf(strs) - Wear.wear[cn].IndexOf(strs1);
		};
		List<GameObject> gameObjects = new List<GameObject>();
		if (ShopNGUIController.IsWeaponCategory(c))
		{
			gameObjects = WeaponManager.sharedManager.FilteredShopLists[(Int32)c];
		}
		else if (c == ShopNGUIController.CategoryNames.HatsCategory)
		{
			foreach (ShopPositionParams hat in this.hats)
			{
				this.FilterUpgrades(gameObjects, hat.gameObject, ShopNGUIController.CategoryNames.HatsCategory, Defs.VisualHatArmor);
			}
			gameObjects.Sort(func(6));
		}
		else if (c == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			if (Storager.getInt("Training.NoviceArmorUsedKey", false) != 1 || TrainingController.TrainingCompleted)
			{
				foreach (ShopPositionParams shopPositionParam in this.armor)
				{
					this.FilterUpgrades(gameObjects, shopPositionParam.gameObject, ShopNGUIController.CategoryNames.ArmorCategory, Defs.VisualArmor);
				}
			}
			else
			{
				GameObject gameObject = Resources.Load<GameObject>("Armor_Info/Armor_Novice");
				if (gameObject == null)
				{
					UnityEngine.Debug.LogError("No novice armor when Storager.getInt(Defs.NoviceArmorUsedKey,false) == 1 && !TrainingController.TrainingCompleted");
				}
				else
				{
					gameObjects.Add(gameObject);
				}
			}
			gameObjects.Sort(func(7));
		}
		else if (c == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			foreach (string key in SkinsController.skinsForPers.Keys)
			{
				gameObjects.Add(this.pixlMan);
			}
			if (ShopNGUIController.ShowLockedFacebookSkin())
			{
				gameObjects.Add(this.pixlMan);
			}
		}
		else if (c == ShopNGUIController.CategoryNames.CapesCategory)
		{
			foreach (ShopPositionParams cape in this.capes)
			{
				this.FilterUpgrades(gameObjects, cape.gameObject, ShopNGUIController.CategoryNames.CapesCategory, string.Empty);
			}
			gameObjects.Sort(func(9));
		}
		else if (c == ShopNGUIController.CategoryNames.BootsCategory)
		{
			foreach (ShopPositionParams boot in this.boots)
			{
				this.FilterUpgrades(gameObjects, boot.gameObject, ShopNGUIController.CategoryNames.BootsCategory, string.Empty);
			}
			gameObjects.Sort(func(10));
		}
		else if (c != ShopNGUIController.CategoryNames.MaskCategory)
		{
			c != ShopNGUIController.CategoryNames.GearCategory;
		}
		else
		{
			foreach (ShopPositionParams mask in this.masks)
			{
				this.FilterUpgrades(gameObjects, mask.gameObject, ShopNGUIController.CategoryNames.MaskCategory, string.Empty);
			}
			gameObjects.Sort(func(12));
		}
		return gameObjects;
	}

	private void FilterUpgrades(List<GameObject> modelsList, GameObject prefab, ShopNGUIController.CategoryNames category, string visualDefName)
	{
		if (prefab.name.Replace("(Clone)", string.Empty) == "Armor_Novice")
		{
			return;
		}
		if (prefab != null && TempItemsController.PriceCoefs.ContainsKey(prefab.name) && TempItemsController.sharedController != null)
		{
			if (TempItemsController.sharedController.ContainsItem(prefab.name))
			{
				modelsList.Add(prefab);
			}
			return;
		}
		List<List<string>> item = null;
		try
		{
			item = Wear.wear[category];
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			UnityEngine.Debug.LogError(string.Concat(new object[] { "Exception in FilterUpgrades ll = Wear.wear [category]: ", exception, " category = ", category.ToString() }));
		}
		if (item == null)
		{
			UnityEngine.Debug.LogError(string.Concat("FilterUpgrades: ll == null   category = ", category.ToString()));
			return;
		}
		List<string> list = item.FirstOrDefault<List<string>>((List<string> l) => l.Contains(prefab.name)).ToList<string>();
		if (list == null)
		{
			return;
		}
		string str = (!string.IsNullOrEmpty(visualDefName) ? Storager.getString(visualDefName, false) : string.Empty);
		int num = list.IndexOf(prefab.name);
		if (Storager.getInt(prefab.name, true) <= 0)
		{
			if (num == 0 && Wear.LeagueForWear(prefab.name, category) <= (int)RatingSystem.instance.currentLeague)
			{
				modelsList.Add(prefab);
			}
			if (num > 0)
			{
				if (Storager.getInt(list[num - 1], true) <= 0)
				{
					if (!string.IsNullOrEmpty(str) && prefab.name.Equals(str))
					{
						modelsList.Add(prefab);
					}
				}
				else if (string.IsNullOrEmpty(str))
				{
					modelsList.Add(prefab);
				}
				else if (list.IndexOf(str) <= num)
				{
					modelsList.Add(prefab);
				}
			}
		}
		else if (num == list.Count - 1)
		{
			modelsList.Add(prefab);
		}
		else if (num >= 0 && num < list.Count - 1 && Storager.getInt(list[num + 1], true) == 0)
		{
			modelsList.Add(prefab);
		}
	}

	public void FireBuyAction(string item)
	{
		if (this.buyAction != null)
		{
			this.buyAction(item);
		}
	}

	private void FireOnEquipSkin(string id)
	{
		if (this.onEquipSkinAction != null)
		{
			this.onEquipSkinAction(id);
		}
	}

	public static void FireWeaponOrArmorBought()
	{
		Action action = ShopNGUIController.GunOrArmorBought;
		if (action != null)
		{
			action();
		}
	}

	private void ForceResetTrainingState()
	{
		try
		{
			if (this._setTrainingStateMethods == null)
			{
				Dictionary<ShopNGUIController.TrainingState, Action> trainingStates = new Dictionary<ShopNGUIController.TrainingState, Action>()
				{
					{ ShopNGUIController.TrainingState.NotInSniperCategory, new Action(this.setNotInSniperCategory) },
					{ ShopNGUIController.TrainingState.OnSniperRifle, new Action(this.setOnSniperRifle) },
					{ ShopNGUIController.TrainingState.InSniperCategoryNotOnSniperRifle, new Action(this.setInSniperCategoryNotOnSniperRifle) },
					{ ShopNGUIController.TrainingState.NotInArmorCategory, new Action(this.setNotInArmorCategory) },
					{ ShopNGUIController.TrainingState.OnArmor, new Action(this.setOnArmor) },
					{ ShopNGUIController.TrainingState.InArmorCategoryNotOnArmor, new Action(this.setInArmorCategoryNotOnArmor) },
					{ ShopNGUIController.TrainingState.BackBlinking, new Action(this.setBackBlinking) }
				};
				this._setTrainingStateMethods = trainingStates;
			}
			this._setTrainingStateMethods[this.trainingState]();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ForceResetTrainingState: ", exception));
		}
	}

	private static string GetWeaponStatText(int currentValue, int nextValue)
	{
		if (nextValue - currentValue == 0)
		{
			return currentValue.ToString();
		}
		if (nextValue - currentValue > 0)
		{
			return string.Format("{0}[00ff00]+{1}[-]", currentValue, nextValue - currentValue);
		}
		return string.Format("{0}[FACC2E]{1}[-]", currentValue, nextValue - currentValue);
	}

	public static void GiveArmorArmy1OrNoviceArmor()
	{
		ShopNGUIController.ProvideShopItemOnStarterPackBoguht(ShopNGUIController.CategoryNames.ArmorCategory, (Storager.getInt("Training.NoviceArmorUsedKey", false) != 1 ? "Armor_Army_1" : "Armor_Novice"), 1, false, 0, null, null, true, Storager.getInt("Training.NoviceArmorUsedKey", false) == 1, false);
	}

	public static void GoToShop(ShopNGUIController.CategoryNames cat, string id)
	{
		ShopNGUIController.sharedShop.SetOfferID(id);
		ShopNGUIController.sharedShop.offerCategory = cat;
		if (ShopNGUIController.GuiActive)
		{
			ShopNGUIController.sharedShop.CategoryChosen(cat, id, false);
			ShopNGUIController.SetIconChosen(cat);
		}
		else if (!SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
		{
			ShopNGUIController.sharedShop.resumeAction = null;
			ShopNGUIController.GuiActive = true;
		}
		else if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.HandleShopClicked(null, EventArgs.Empty);
		}
	}

	public void goToSM()
	{
		string str;
		if (UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("SkinEditorController")).GetComponent<SkinEditorController>() != null)
		{
			Action<string> action = null;
			action = (string n) => {
				SkinEditorController.ExitFromSkinEditor -= this.backHandler;
				MenuBackgroundMusic.sharedMusic.StopCustomMusicFrom(SkinEditorController.sharedController.gameObject);
				this.u003cu003ef__this.mainPanel.SetActive(true);
				if (this.u003cu003ef__this.currentCategory == ShopNGUIController.CategoryNames.CapesCategory || n != null)
				{
					if (this.u003cu003ef__this.viewedId != null && this.u003cu003ef__this.viewedId.Equals("CustomSkinID"))
					{
						this.u003cu003ef__this.SetSkinAsCurrent(n);
					}
					if (this.u003cu003ef__this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory && this.u003cu003ef__this.viewedId != null && this.u003cu003ef__this.viewedId.Equals(SkinsController.currentSkinNameForPers))
					{
						this.u003cu003ef__this.FireOnEquipSkin(n);
					}
					if (this.u003cu003ef__this.viewedId != null && this.u003cu003ef__this.viewedId.Equals("cape_Custom"))
					{
						this.u003cu003ef__this.EquipWear("cape_Custom");
					}
					this.u003cu003ef__this.StartCoroutine(this.u003cu003ef__this.ReloadAfterEditing(n, true));
				}
				else
				{
					this.u003cu003ef__this.StartCoroutine(this.u003cu003ef__this.ReloadAfterEditing(n, n == null));
				}
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC == null)
				{
					if (this.u003cu003ef__this.currentCategory != ShopNGUIController.CategoryNames.CapesCategory && PlayerPrefs.GetInt(Defs.ShownRewardWindowForSkin, 0) == 0)
					{
						this.u003cu003ef__this._shouldShowRewardWindowSkin = true;
					}
					if (this.u003cu003ef__this.currentCategory == ShopNGUIController.CategoryNames.CapesCategory && PlayerPrefs.GetInt(Defs.ShownRewardWindowForCape, 0) == 0)
					{
						this.u003cu003ef__this._shouldShowRewardWindowCape = true;
					}
				}
			};
			SkinEditorController.ExitFromSkinEditor += action;
			if (!this.viewedId.Equals("CustomSkinID"))
			{
				str = this.viewedId;
			}
			else
			{
				str = null;
			}
			SkinEditorController.currentSkinName = str;
			SkinEditorController.modeEditor = (this.currentCategory != ShopNGUIController.CategoryNames.SkinsCategory ? SkinEditorController.ModeEditor.Cape : SkinEditorController.ModeEditor.SkinPers);
			this.mainPanel.SetActive(false);
		}
	}

	private void HandleActionsUUpdated()
	{
		this.UpdateButtons();
		this.UpdateItemParameters();
	}

	private void HandleCarouselCentering()
	{
		this.HandleCarouselCentering(this.carouselCenter.centeredObject);
	}

	private void HandleCarouselCentering(GameObject centeredObj)
	{
		if (centeredObj != null && centeredObj != this._lastSelectedItem)
		{
			this._lastSelectedItem = centeredObj;
			!(this.highlightedCarouselObject != null);
			this.highlightedCarouselObject = centeredObj.transform;
			!(this.highlightedCarouselObject != null);
			this.ChooseCarouselItem(centeredObj.GetComponent<ShopCarouselElement>().itemID, false, false);
		}
		if (this.EnableConfigurePos && centeredObj != null)
		{
			centeredObj.GetComponent<ShopCarouselElement>().SetPos(1f, 0f);
		}
	}

	private void HandleEscape()
	{
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("Ignoring [Escape] since Tutorial is not completed.");
			}
			return;
		}
		if (this.InTrainingAfterNoviceArmorRemoved)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("Ignoring [Escape] since Tutorial after removing Novice Armor is not completed.");
			}
			return;
		}
		if (ShopNGUIController.GuiActive)
		{
			this._escapeRequested = true;
			return;
		}
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(string.Concat(base.GetType().Name, ".LateUpdate():    Ignoring Escape because Shop GUI is not active."));
		}
	}

	public void HandleFacebookButton()
	{
		this._isFromPromoActions = false;
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(() => FacebookController.Login(() => {
			if (ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.UpdateButtons();
			}
		}, null, "Shop", null), () => FacebookController.Login(null, null, "Shop", null));
	}

	public void HandleProfileButton()
	{
		GameObject gameObject = GameObject.Find("MainMenuNGUI");
		if (gameObject)
		{
			gameObject.SetActive(false);
		}
		GameObject gameObject1 = GameObject.FindWithTag("InGameGUI");
		if (gameObject1)
		{
			gameObject1.SetActive(false);
		}
		GameObject gameObject2 = GameObject.FindWithTag("NetworkStartTableNGUI");
		if (gameObject2)
		{
			gameObject2.SetActive(false);
		}
		ShopNGUIController.GuiActive = false;
		Action action = () => {
		};
		ProfileController.Instance.DesiredWeaponTag = this._assignedWeaponTag;
		ProfileController.Instance.ShowInterface(new Action[] { action, new Action(() => {
			ShopNGUIController.GuiActive = true;
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
			if (gameObject1)
			{
				gameObject1.SetActive(true);
			}
			if (gameObject2)
			{
				gameObject2.SetActive(true);
			}
		}) });
	}

	public void HandlePropertiesInfoButton()
	{
		if (!this.WeaponCategory)
		{
			return;
		}
		if (Defs.isSoundFX)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.infoScreen.Show(this.currentCategory == ShopNGUIController.CategoryNames.MeleeCategory);
	}

	public void HandleRentButton()
	{
	}

	private static bool HasBoughtGood(string defName, bool tempGun = false)
	{
		return (!tempGun ? Storager.getInt(defName, true) != 0 : TempItemsController.sharedController.ContainsItem(defName));
	}

	private void HideAllTrainingInterface()
	{
		try
		{
			foreach (GameObject trainingTip in this.trainingTips)
			{
				trainingTip.SetActive(false);
			}
			this.trainingColliders.SetActive(false);
			base.StopCoroutine("Blink");
			this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
			this.equips[1].normalSprite = "or_btn";
			this.category.buttons[4].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
			this.category.buttons[4].offButton.normalSprite = "trans_btn";
			this.category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
			this.category.buttons[7].offButton.normalSprite = "trans_btn";
			this.backButton.tweenTarget.GetComponent<UISprite>().spriteName = "yell_btn";
			this.backButton.normalSprite = "yell_btn";
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in HideAllTrainingInterface: ", exception));
		}
	}

	private void HideAllTrainingInterfaceRemovedNoviceArmor()
	{
		try
		{
			foreach (GameObject gameObject in this.trainingTipsRemovedNoviceArmor)
			{
				gameObject.SetActive(false);
			}
			this.trainingColliders.SetActive(false);
			this.trainingRemoveNoviceArmorCollider.SetActive(false);
			base.StopCoroutine("Blink");
			this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
			this.equips[1].normalSprite = "or_btn";
			this.category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
			this.category.buttons[7].offButton.normalSprite = "trans_btn";
			this.backButton.tweenTarget.GetComponent<UISprite>().spriteName = "yell_btn";
			this.backButton.normalSprite = "yell_btn";
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in HideAllTrainingInterfaceRemovedNoviceArmor: ", exception));
		}
	}

	public void IsInShopFromPromoPanel(bool isFromPromoACtions, string tg)
	{
		this._isFromPromoActions = isFromPromoACtions;
		this._promoActionsIdClicked = tg;
	}

	public static bool IsWeaponCategory(ShopNGUIController.CategoryNames c)
	{
		return c < ShopNGUIController.CategoryNames.HatsCategory;
	}

	public static bool IsWearCategory(ShopNGUIController.CategoryNames c)
	{
		return Wear.wear.Keys.Contains<ShopNGUIController.CategoryNames>(c);
	}

	private static string ItemIDForPrefab(string name, ShopNGUIController.CategoryNames c)
	{
		if (c == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			string str = Storager.getString(Defs.VisualArmor, false);
			if (!string.IsNullOrEmpty(str) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(name) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(name) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(str))
			{
				return str;
			}
		}
		else if (c == ShopNGUIController.CategoryNames.HatsCategory)
		{
			string str1 = Storager.getString(Defs.VisualHatArmor, false);
			if (!string.IsNullOrEmpty(str1) && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(name) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(name) < Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(str1))
			{
				return str1;
			}
		}
		return name;
	}

	private static string ItemIDForPrefabReverse(string name, ShopNGUIController.CategoryNames c)
	{
		if (c == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			string str = Storager.getString(Defs.VisualArmor, false);
			if (!string.IsNullOrEmpty(str) && str.Equals(name) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(name) >= 0)
			{
				for (int i = 1; i < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Count; i++)
				{
					if (Storager.getInt(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0][i], true) == 0)
					{
						return Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0][i];
					}
				}
			}
		}
		else if (c == ShopNGUIController.CategoryNames.HatsCategory)
		{
			string str1 = Storager.getString(Defs.VisualHatArmor, false);
			if (!string.IsNullOrEmpty(str1) && str1.Equals(name) && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(name) >= 0)
			{
				for (int j = 1; j < Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Count; j++)
				{
					if (Storager.getInt(Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0][j], true) == 0)
					{
						return Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0][j];
					}
				}
			}
		}
		return name;
	}

	private void LateUpdate()
	{
		float viewSize = this.scrollViewPanel.GetViewSize().x / 2f;
		ShopCarouselElement[] componentsInChildren = this.wrapContent.GetComponentsInChildren<ShopCarouselElement>(false);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			ShopCarouselElement shopCarouselElement = componentsInChildren[i];
			Transform transforms = shopCarouselElement.transform;
			float single = this.scrollViewPanel.clipOffset.x;
			Vector3 vector3 = transforms.localPosition;
			float single1 = Mathf.Abs(vector3.x - single);
			float single2 = this.scaleCoef + (1f - this.scaleCoef) * (1f - single1 / viewSize);
			float single3 = 0.65f;
			single2 = (single1 > viewSize / 3f ? this.scaleCoef + (single3 - this.scaleCoef) * (1f - (single1 - viewSize / 3f) / (viewSize * 2f / 3f)) : single3 + (1f - single3) * (1f - single1 / (viewSize / 3f)));
			if (single1 >= viewSize * 0.9f)
			{
				single2 = 0f;
			}
			float single4 = transforms.localPosition.x - single;
			float single5 = 0f;
			float single6 = (float)((single4 > 0f ? -1 : 1));
			if (single4 != 0f)
			{
				if (Mathf.Abs(single4) > this.wrapContent.cellWidth)
				{
					single5 = (Mathf.Abs(single4) > 2f * this.wrapContent.cellWidth ? this.secondOffset * (1f - (Mathf.Abs(single4) - 2f * (float)this.wrapContent.cellWidth) / (float)this.wrapContent.cellWidth) : this.firstOFfset + (this.secondOffset - this.firstOFfset) * ((Mathf.Abs(single4) - (float)this.wrapContent.cellWidth) / (float)this.wrapContent.cellWidth));
				}
				else
				{
					single5 = this.firstOFfset * (Mathf.Abs(single4) / (float)this.wrapContent.cellWidth);
				}
			}
			single5 *= single6;
			if (!this.EnableConfigurePos || this.scrollViewPanel.GetComponent<UIScrollView>().isDragging || this.scrollViewPanel.GetComponent<UIScrollView>().currentMomentum.x > 0f)
			{
				shopCarouselElement.SetPos(single2, single5);
			}
			shopCarouselElement.topSeller.gameObject.SetActive((!shopCarouselElement.showTS ? false : Mathf.Abs(single1) <= this.wrapContent.cellWidth / 10f));
			shopCarouselElement.newnew.gameObject.SetActive((!shopCarouselElement.showNew ? false : Mathf.Abs(single1) <= this.wrapContent.cellWidth / 10f));
			shopCarouselElement.quantity.gameObject.SetActive((!shopCarouselElement.showQuantity ? false : Mathf.Abs(single1) <= this.wrapContent.cellWidth / 10f));
		}
		if (this._escapeRequested)
		{
			base.StartCoroutine(this.BackAfterDelay());
			this._escapeRequested = false;
		}
	}

	public void LoadCurrentWearToVars()
	{
		this._currentCape = Storager.getString(Defs.CapeEquppedSN, false);
		this._currentHat = Storager.getString(Defs.HatEquppedSN, false);
		this._currentBoots = Storager.getString(Defs.BootsEquppedSN, false);
		this._currentArmor = Storager.getString(Defs.ArmorNewEquppedSN, false);
		this._currentMask = Storager.getString("MaskEquippedSN", false);
	}

	[DebuggerHidden]
	private IEnumerator LoadModelAsync(Action<GameObject, ShopNGUIController.CategoryNames> onLoad, GameObject prototype, ShopNGUIController.CategoryNames category)
	{
		ShopNGUIController.u003cLoadModelAsyncu003ec__Iterator1A8 variable = null;
		return variable;
	}

	private void LogPurchaseAfterPaymentAnalyticsEvent(string itemName)
	{
		Dictionary<string, string> strs;
		float? nullable;
		float? nullable1;
		float? nullable2;
		float? nullable3;
		float? nullable4;
		float? nullable5;
		if (!FlurryEvents.PaymentTime.HasValue)
		{
			return;
		}
		float? paymentTime = FlurryEvents.PaymentTime;
		if (!paymentTime.HasValue)
		{
			nullable = null;
		}
		else
		{
			nullable = new float?(Time.realtimeSinceStartup - paymentTime.Value);
		}
		float? nullable6 = nullable;
		if ((!nullable6.HasValue ? true : nullable6.Value >= 30f))
		{
			float? paymentTime1 = FlurryEvents.PaymentTime;
			if (!paymentTime1.HasValue)
			{
				nullable1 = null;
			}
			else
			{
				nullable1 = new float?(Time.realtimeSinceStartup - paymentTime1.Value);
			}
			float? nullable7 = nullable1;
			if ((!nullable7.HasValue ? true : nullable7.Value >= 60f))
			{
				float? paymentTime2 = FlurryEvents.PaymentTime;
				if (!paymentTime2.HasValue)
				{
					nullable2 = null;
				}
				else
				{
					nullable2 = new float?(Time.realtimeSinceStartup - paymentTime2.Value);
				}
				float? nullable8 = nullable2;
				if ((!nullable8.HasValue ? true : nullable8.Value >= 90f))
				{
					strs = new Dictionary<string, string>()
					{
						{ "90+", itemName }
					};
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment", strs, true);
				}
				else
				{
					strs = new Dictionary<string, string>()
					{
						{ "60-90", itemName }
					};
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment", strs, true);
				}
			}
			else
			{
				strs = new Dictionary<string, string>()
				{
					{ "30-60", itemName }
				};
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment", strs, true);
			}
		}
		else
		{
			strs = new Dictionary<string, string>()
			{
				{ "0-30", itemName }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment", strs, true);
		}
		float? paymentTime3 = FlurryEvents.PaymentTime;
		if (!paymentTime3.HasValue)
		{
			nullable3 = null;
		}
		else
		{
			nullable3 = new float?(Time.realtimeSinceStartup - paymentTime3.Value);
		}
		float? nullable9 = nullable3;
		if ((!nullable9.HasValue ? false : nullable9.Value < 30f))
		{
			strs = new Dictionary<string, string>()
			{
				{ "0-30", itemName }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment Cumulative", strs, true);
		}
		float? paymentTime4 = FlurryEvents.PaymentTime;
		if (!paymentTime4.HasValue)
		{
			nullable4 = null;
		}
		else
		{
			nullable4 = new float?(Time.realtimeSinceStartup - paymentTime4.Value);
		}
		float? nullable10 = nullable4;
		if ((!nullable10.HasValue ? false : nullable10.Value < 60f))
		{
			strs = new Dictionary<string, string>()
			{
				{ "0-60", itemName }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment Cumulative", strs, true);
		}
		float? paymentTime5 = FlurryEvents.PaymentTime;
		if (!paymentTime5.HasValue)
		{
			nullable5 = null;
		}
		else
		{
			nullable5 = new float?(Time.realtimeSinceStartup - paymentTime5.Value);
		}
		float? nullable11 = nullable5;
		if ((!nullable11.HasValue ? false : nullable11.Value < 90f))
		{
			strs = new Dictionary<string, string>()
			{
				{ "0-90", itemName }
			};
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment Cumulative", strs, true);
		}
		strs = new Dictionary<string, string>()
		{
			{ "All", itemName }
		};
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Purchase After Payment Cumulative", strs, true);
	}

	private void LogShopPurchasesTotalAndPayingNonPaying(string itemName)
	{
		try
		{
			string str = this.currentCategory.ToString();
			string str1 = string.Format("Shop Purchases {0}", "Total");
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "All Categories", str },
				{ str, itemName },
				{ "Item", itemName }
			};
			Dictionary<string, string> strs1 = strs;
			if (this.currentCategory != ShopNGUIController.CategoryNames.GearCategory)
			{
				strs1.Add("Without Quick Shop", itemName);
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str1, strs1, true);
			string payingSuffix = FlurryPluginWrapper.GetPayingSuffix();
			string str2 = string.Format("Shop Purchases {0}", string.Concat("Total", payingSuffix));
			FlurryPluginWrapper.LogEventAndDublicateToConsole(str2, strs1, true);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("LogShopPurchasesTotalAndPayingNonPaying exception: ", exception));
		}
	}

	public void MakeACtiveAfterDelay(string idToSet, ShopNGUIController.CategoryNames cn)
	{
		Light[] lightArray = UnityEngine.Object.FindObjectsOfType<Light>() ?? new Light[0];
		for (int i = 0; i < (int)lightArray.Length; i++)
		{
			Light light = lightArray[i];
			if (!this.mylights.Contains(light))
			{
				Light layer = light;
				layer.cullingMask = layer.cullingMask & ~(1 << (LayerMask.NameToLayer("NGUIShop") & 31 & 31));
			}
		}
		ShopNGUIController.sharedShop.ActiveObject.SetActive(true);
		this.wrapContent.Reposition();
		if (ExperienceController.sharedController != null && ExpController.Instance != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
			ExpController.Instance.InterfaceEnabled = true;
		}
		this.UpdatePersHat(this._currentHat);
		this.UpdatePersCape(this._currentCape);
		this.UpdatePersArmor(this._currentArmor);
		this.UpdatePersBoots(this._currentBoots);
		this.UpdatePersMask(this._currentMask);
		this.UpdatePersSkin(SkinsController.currentSkinNameForPers);
		this.carouselCenter.onFinished += new SpringPanel.OnFinished(this.HandleCarouselCentering);
		PromoActionsManager.ActionsUUpdated += new Action(this.HandleActionsUUpdated);
		this.PlayWeaponAnimation();
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		if (idToSet != null)
		{
			ShopNGUIController.sharedShop.ChooseCarouselItem(idToSet, false, true);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			this.ForceResetTrainingState();
		}
		ShopNGUIController.sharedShop.carouselCenter.enabled = true;
		this.AdjustCategoryButtonsForFilterMap();
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		ShopNGUIController.u003cMyWaitForSecondsu003ec__Iterator1AD variable = null;
		return variable;
	}

	public static string NoneEquippedForWearCategory(ShopNGUIController.CategoryNames c)
	{
		string capeNoneEqupped;
		if (c == ShopNGUIController.CategoryNames.CapesCategory)
		{
			capeNoneEqupped = Defs.CapeNoneEqupped;
		}
		else if (c == ShopNGUIController.CategoryNames.BootsCategory)
		{
			capeNoneEqupped = Defs.BootsNoneEqupped;
		}
		else if (c != ShopNGUIController.CategoryNames.ArmorCategory)
		{
			capeNoneEqupped = (c != ShopNGUIController.CategoryNames.MaskCategory ? Defs.HatNoneEqupped : "MaskNoneEquipped");
		}
		else
		{
			capeNoneEqupped = Defs.ArmorNewNoneEqupped;
		}
		return capeNoneEqupped;
	}

	private void OnDestroy()
	{
		if (this.profile != null)
		{
			Resources.UnloadAsset(this.profile);
			this.profile = null;
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		if (ShopNGUIController.GuiActive)
		{
			this._storedAmbientLight = new Color?(RenderSettings.ambientLight);
			this._storedFogEnabled = new bool?(RenderSettings.fog);
			RenderSettings.ambientLight = Defs.AmbientLightColorForShop();
			RenderSettings.fog = false;
		}
	}

	public void PlayWeaponAnimation()
	{
		if (this.profile != null && this.weapon != null)
		{
			Animation component = this.weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>();
			if (Time.timeScale == 0f)
			{
				this.animationCoroutineRunner.StopAllCoroutines();
				if (component.GetClip("Profile") == null)
				{
					component.AddClip(this.profile, "Profile");
				}
				if (!this.animationCoroutineRunner.gameObject.activeInHierarchy)
				{
					UnityEngine.Debug.LogWarning("Coroutine couldn't be started because the the game object 'AnimationCoroutineRunner' is inactive!");
				}
				else
				{
					this.animationCoroutineRunner.StartPlay(component, "Profile", false, null);
				}
			}
			else
			{
				if (component.GetClip("Profile") == null)
				{
					component.AddClip(this.profile, "Profile");
				}
				component.Play("Profile");
			}
		}
		this.MainMenu_Pers.GetComponent<Animation>().Stop();
		this.MainMenu_Pers.GetComponent<Animation>().Play("Idle");
	}

	public static int PriceIfGunWillBeTryGun(string tg)
	{
		return Mathf.RoundToInt((float)ShopNGUIController.currentPrice(tg, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(tg), false, false).Price * ((float)WeaponManager.BaseTryGunDiscount() / 100f));
	}

	private static void ProvdeShopItemWithRightId(ShopNGUIController.CategoryNames c, string id, string tg, Action UNUSED_DO_NOT_SET_onTrainingAction, Action<string> onEquipWearAction, Action<string> contextSpecificAction, Action<string> onSkinBoughtAction, bool giveOneItemOfGear = false, int gearCount = 1, bool buyArmorAndHatsUpToTg = false, int timeForRentIndex = 0, bool doAndroidCloudSync = true)
	{
		if (ShopNGUIController.GunBought != null)
		{
			ShopNGUIController.GunBought();
		}
		if (ShopNGUIController.IsWearCategory(c))
		{
			if (buyArmorAndHatsUpToTg && Wear.wear.ContainsKey(c))
			{
				List<List<string>> item = Wear.wear[c];
				List<string> strs = null;
				foreach (List<string> strs1 in item)
				{
					if (!strs1.Contains(tg))
					{
						continue;
					}
					strs = strs1;
					break;
				}
				if (strs != null)
				{
					int num = 0;
					while (num < strs.Count)
					{
						Storager.setInt(strs[num], 1, true);
						if (!strs[num].Equals(tg))
						{
							num++;
						}
						else
						{
							break;
						}
					}
				}
			}
			else if (!TempItemsController.PriceCoefs.ContainsKey(tg))
			{
				Storager.setInt(tg, 1, true);
			}
			else
			{
				int num1 = TempItemsController.RentTimeForIndex(timeForRentIndex);
				TempItemsController.sharedController.AddTemporaryItem(tg, num1);
			}
			if ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) && doAndroidCloudSync)
			{
				ShopNGUIController.SynchronizeAndroidPurchases(string.Concat("Wear: ", tg));
			}
			if (onEquipWearAction != null)
			{
				onEquipWearAction(tg);
			}
		}
		if (ShopNGUIController.IsWeaponCategory(c) && !WeaponManager.FirstUnboughtTag(tg).Equals(tg))
		{
			List<string> strs2 = WeaponUpgrades.ChainForTag(tg);
			if (strs2 != null)
			{
				int num2 = strs2.IndexOf(tg) - 1;
				if (num2 >= 0)
				{
					for (int i = 0; i <= num2; i++)
					{
						try
						{
							Storager.setInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[strs2[i]]], 1, true);
						}
						catch
						{
							UnityEngine.Debug.LogError("Error filling chain in indexOfWeaponBeforeCurrentTg");
						}
					}
				}
			}
		}
		WeaponManager.sharedManager.AddMinerWeapon(id, timeForRentIndex);
		if (WeaponManager.sharedManager != null)
		{
			try
			{
				bool flag = WeaponManager.sharedManager.IsAvailableTryGun(WeaponManager.LastBoughtTag(tg));
				bool flag1 = WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(WeaponManager.LastBoughtTag(tg));
				WeaponManager.RemoveGunFromAllTryGunRelated(tg);
				if (flag1)
				{
					string empty = string.Empty;
					string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(tg), empty, c, null);
					AnalyticsStuff.LogWEaponsSpecialOffers_Conversion(false, itemNameNonLocalized);
				}
				if (flag1 || flag)
				{
					Action<string> action = ShopNGUIController.TryGunBought;
					if (action != null)
					{
						action(WeaponManager.LastBoughtTag(tg));
					}
					if (!FriendsController.useBuffSystem)
					{
						KillRateCheck.OnTryGunBuyed();
					}
					else
					{
						BuffSystem.instance.OnTryGunBuyed(ItemDb.GetByTag(tg).PrefabName);
					}
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(string.Concat("Exception in removeing TryGun structures: ", exception));
			}
		}
		if (c == ShopNGUIController.CategoryNames.GearCategory)
		{
			if (!id.Contains(GearManager.UpgradeSuffix))
			{
				int num3 = ShopNGUIController.AddedNumberOfGearWhenBuyingPack(id);
				Storager.setInt(id, Storager.getInt(id, false) + (!giveOneItemOfGear ? num3 : gearCount), false);
			}
			else
			{
				string str = GearManager.NameForUpgrade(GearManager.HolderQuantityForID(id), GearManager.CurrentNumberOfUphradesForGear(GearManager.HolderQuantityForID(id)) + 1);
				Storager.setInt(str, 1, false);
			}
		}
		if (contextSpecificAction != null)
		{
			contextSpecificAction(id);
		}
		if (c == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			if (id != null && SkinsController.shopKeyFromNameSkin.ContainsKey(id))
			{
				string item1 = SkinsController.shopKeyFromNameSkin[id];
				if (Array.IndexOf<string>(StoreKitEventListener.skinIDs, item1) >= 0)
				{
					foreach (KeyValuePair<string, string> value in InAppData.inAppData.Values)
					{
						if (value.Key == null || !value.Key.Equals(item1))
						{
							continue;
						}
						Storager.setInt(value.Value, 1, true);
						if (doAndroidCloudSync)
						{
							ShopNGUIController.SynchronizeAndroidPurchases(string.Concat("Skin: ", item1));
						}
						break;
					}
				}
			}
			if (onSkinBoughtAction != null)
			{
				onSkinBoughtAction(id);
			}
		}
	}

	public static void ProvideAllTypeShopItem(ShopNGUIController.CategoryNames category, string sourceTag, int gearCount, int timeForRent)
	{
		int num2 = 0;
		if (timeForRent != -1)
		{
			num2 = TempItemsController.RentIndexFromDays(timeForRent / 24);
		}
		ShopNGUIController.ProvideShopItemOnStarterPackBoguht(category, sourceTag, gearCount, false, num2, null, (string tg) => {
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.weaponsInGame != null)
			{
				if (!ShopNGUIController.GuiActive || !(ShopNGUIController.sharedShop != null))
				{
					int num = PromoActionsGUIController.CatForTg(tg);
					if (num != -1)
					{
						ShopNGUIController.SetAsEquippedAndSendToServer(tg, (ShopNGUIController.CategoryNames)num);
					}
				}
				else
				{
					int num1 = PromoActionsGUIController.CatForTg(tg);
					if (num1 != -1)
					{
						ShopNGUIController.EquipWearInCategoryIfNotEquiped(tg, (ShopNGUIController.CategoryNames)num1, (WeaponManager.sharedManager == null ? false : WeaponManager.sharedManager.myPlayerMoveC != null));
					}
				}
			}
		}, true, true, true);
		TempItemsController.sharedController.ExpiredItems.Remove(sourceTag);
	}

	public static void ProvideShopItemOnStarterPackBoguht(ShopNGUIController.CategoryNames c, string sourceTg, int gearCount = 1, bool buyArmorUpToSourceTg = false, int timeForRentIndex = 0, Action<string> contextSpecificAction = null, Action<string> customEquipWearAction = null, bool equipSkin = true, bool equipWear = true, bool doAndroidCloudSync = true)
	{
		string str = (c != ShopNGUIController.CategoryNames.GearCategory ? sourceTg : GearManager.HolderQuantityForID(sourceTg));
		string str1 = str;
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(str1))
		{
			str = WeaponManager.tagToStoreIDMapping[str1];
		}
		if (str == null)
		{
			return;
		}
		ShopNGUIController.ProvdeShopItemWithRightId(c, str, str1, null, (string item) => {
			if (customEquipWearAction != null)
			{
				customEquipWearAction(item);
			}
			else if (equipWear)
			{
				ShopNGUIController.SetAsEquippedAndSendToServer(item, c);
			}
		}, contextSpecificAction, (string item) => {
			if (equipSkin)
			{
				ShopNGUIController.SaveSkinAndSendToServer(item);
			}
		}, true, gearCount, buyArmorUpToSourceTg, timeForRentIndex, doAndroidCloudSync);
	}

	[DebuggerHidden]
	public IEnumerator ReloadAfterEditing(string n, bool shouldReload = true)
	{
		ShopNGUIController.u003cReloadAfterEditingu003ec__Iterator1A9 variable = null;
		return variable;
	}

	internal void ReloadCarousel(string idToChoose = null)
	{
		string tag;
		GameObject gameObject;
		string str2;
		string str3 = idToChoose;
		ShopCarouselElement[] componentsInChildren = this.wrapContent.GetComponentsInChildren<ShopCarouselElement>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			ShopCarouselElement shopCarouselElement = componentsInChildren[i];
			UnityEngine.Object.Destroy(shopCarouselElement.gameObject);
			shopCarouselElement.transform.parent = null;
		}
		this.wrapContent.Reposition();
		List<GameObject> gameObjects = this.FillModelsList(this.currentCategory);
		string[] strArrays = null;
		if (this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			strArrays = new string[gameObjects.Count];
			List<string> list = SkinsController.skinsForPers.Keys.ToList<string>();
			if (!ShopNGUIController.ShowLockedFacebookSkin())
			{
				list.Remove("61");
			}
			list.Sort((string kvp1, string kvp2) => {
				int num;
				try
				{
					if (kvp1 == "61" || kvp2 == "61")
					{
						string str = 4.ToString();
						string str1 = (kvp1 != "61" ? kvp1 : kvp2);
						if (str1 == str)
						{
							str1 = 6.ToString();
						}
						num = (kvp1 != "61" ? long.Parse(str1).CompareTo(long.Parse(str)) : long.Parse(str).CompareTo(long.Parse(str1)));
					}
					else
					{
						num = long.Parse(kvp1).CompareTo(long.Parse(kvp2));
					}
				}
				catch
				{
					num = 0;
				}
				return num;
			});
			int num1 = list.FindIndex((string kvp) => {
				long num;
				if (!long.TryParse(kvp, out num))
				{
					return false;
				}
				return num >= (long)1000000;
			});
			int count = 0;
			if (num1 < 0 || num1 >= list.Count)
			{
				strArrays[0] = "CustomSkinID";
				count++;
				list.CopyTo(strArrays, 1);
			}
			else
			{
				List<string> strs = new List<string>();
				strs.AddRange(list.GetRange(num1, list.Count - num1));
				strs.Reverse();
				strs.CopyTo(strArrays);
				count = strs.Count;
				strArrays[count] = "CustomSkinID";
				count++;
				list.CopyTo(0, strArrays, count, num1);
			}
		}
		if (this.EnableConfigurePos)
		{
			List<string> strs1 = new List<string>();
			List<GameObject> gameObjects1 = new List<GameObject>();
			for (int j = 0; j < gameObjects.Count; j++)
			{
				List<string> strs2 = null;
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (!upgrade.Contains((!this.WeaponCategory ? gameObjects[j].tag : ItemDb.GetByPrefabName(gameObjects[j].name.Replace("(Clone)", string.Empty)).Tag)))
					{
						continue;
					}
					strs2 = upgrade;
					break;
				}
				if (strs2 != null)
				{
					for (int k = 0; k < strs2.Count; k++)
					{
						UnityEngine.Object[] objArray = WeaponManager.sharedManager.weaponsInGame;
						int num2 = 0;
						while (num2 < (int)objArray.Length)
						{
							GameObject gameObject1 = (GameObject)objArray[num2];
							if (!ItemDb.GetByPrefabName(gameObject1.name).Tag.Equals(strs2[k]))
							{
								num2++;
							}
							else
							{
								gameObjects1.Add(gameObject1);
								break;
							}
						}
					}
				}
				else
				{
					gameObjects1.Add(gameObjects[j]);
				}
			}
			gameObjects = gameObjects1;
		}
		if (str3 == null)
		{
			str3 = this.viewedId;
		}
		int num3 = 10000;
		if (str3 != null)
		{
			if (this.WeaponCategory)
			{
				ItemRecord byTag = ItemDb.GetByTag(str3);
				num3 = (byTag == null ? -1 : gameObjects.FindIndex((GameObject go) => go.nameNoClone() == byTag.PrefabName));
			}
			else if (this.currentCategory != ShopNGUIController.CategoryNames.SkinsCategory)
			{
				num3 = gameObjects.FindIndex((GameObject go) => go.nameNoClone() == str3);
			}
		}
		for (int l = 0; l < gameObjects.Count; l++)
		{
			GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ShopCarouselElement"));
			vector3.transform.parent = this.wrapContent.transform;
			vector3.transform.localScale = new Vector3(1f, 1f, 1f);
			GameObject item = gameObjects[l];
			vector3.name = l.ToString("D7");
			if (this.WeaponCategory)
			{
				string str4 = vector3.name;
				int num4 = int.Parse(item.name.Substring("Weapon".Length));
				vector3.name = string.Concat(str4, "_", num4.ToString("D5"));
			}
			ShopCarouselElement component = vector3.GetComponent<ShopCarouselElement>();
			if (!this.WeaponCategory)
			{
				tag = (this.currentCategory != ShopNGUIController.CategoryNames.SkinsCategory ? ShopNGUIController.ItemIDForPrefabReverse(item.name, this.currentCategory) : strArrays[l]);
			}
			else
			{
				tag = ItemDb.GetByPrefabName(item.name.Replace("(Clone)", string.Empty)).Tag;
			}
			string str5 = tag;
			component.itemID = str5;
			if (this.currentCategory == ShopNGUIController.CategoryNames.GearCategory)
			{
				component.showQuantity = true;
				component.SetQuantity();
			}
			bool flag = false;
			if (this.WeaponCategory && WeaponManager.tagToStoreIDMapping.ContainsKey(component.itemID) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[component.itemID]))
			{
				flag = Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[component.itemID]], true) > 0;
			}
			if (this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory)
			{
				ShopCarouselElement shopCarouselElement1 = component;
				if (!str5.Equals("CustomSkinID"))
				{
					str2 = (!SkinsController.skinsNamesForPers.ContainsKey(str5) ? string.Empty : SkinsController.skinsNamesForPers[str5]);
				}
				else
				{
					str2 = LocalizationStore.Get("Key_1090");
				}
				shopCarouselElement1.readableName = str2;
			}
			if (PromoActionsManager.sharedManager.topSellers.Contains(str5))
			{
				component.showTS = true;
			}
			if (PromoActionsManager.sharedManager.news.Contains(str5))
			{
				component.showNew = true;
			}
			Action<GameObject, ShopNGUIController.CategoryNames> action = (GameObject loadedOBject, ShopNGUIController.CategoryNames category) => ShopNGUIController.AddModel(loadedOBject, (GameObject manipulateObject, Vector3 positionShop, Vector3 rotationShop, string readableName, float scaleCoefShop, int tier, int league) => {
				if (component == null)
				{
					UnityEngine.Object.Destroy(manipulateObject);
					return;
				}
				if (category != ShopNGUIController.CategoryNames.SkinsCategory)
				{
					component.readableName = readableName ?? string.Empty;
				}
				manipulateObject.transform.parent = component.transform;
				component.baseScale = new Vector3(scaleCoefShop, scaleCoefShop, scaleCoefShop);
				component.model = manipulateObject.transform;
				component.ourPosition = positionShop;
				component.SetPos((!this.EnableConfigurePos ? 0f : 1f), 0f);
				component.model.Rotate(rotationShop, Space.World);
				if (category == ShopNGUIController.CategoryNames.SkinsCategory)
				{
					Player_move_c.SetTextureRecursivelyFrom(manipulateObject, (!str5.Equals("CustomSkinID") ? SkinsController.skinsForPers[str5] : Resources.Load("Skin_Start") as Texture), new GameObject[0]);
				}
				if (str5.Equals("cape_Custom") && SkinsController.capeUserTexture != null)
				{
					Player_move_c.SetTextureRecursivelyFrom(manipulateObject, SkinsController.capeUserTexture, new GameObject[0]);
				}
				if (ExpController.Instance != null && ExpController.Instance.OurTier < tier && tier < 100 && (ShopNGUIController.IsWeaponCategory(category) && component.itemID.Equals(WeaponManager.FirstUnboughtOrForOurTier(component.itemID)) && !flag || ShopNGUIController.IsWearCategory(category) && component.itemID.Equals(WeaponManager.FirstUnboughtTag(component.itemID)) && component.itemID != "cape_Custom" && component.itemID != "boots_tabi") && component.locked != null)
				{
					component.locked.SetActive(true);
				}
				if (ShopNGUIController.IsWeaponCategory(category) && !component.itemID.Equals(WeaponManager.FirstUnboughtOrForOurTier(component.itemID)) && tier < 100 || ShopNGUIController.IsWearCategory(category) && !component.itemID.Equals(WeaponManager.FirstUnboughtTag(component.itemID)))
				{
					if (component.arrow != null)
					{
						component.arrow.gameObject.SetActive(true);
					}
					if (category == ShopNGUIController.CategoryNames.HatsCategory)
					{
						component.arrnoInitialPos = new Vector3(85f, component.arrnoInitialPos.y, component.arrnoInitialPos.z);
					}
					if (category == ShopNGUIController.CategoryNames.ArmorCategory)
					{
						component.arrnoInitialPos = new Vector3(105f, component.arrnoInitialPos.y, component.arrnoInitialPos.z);
					}
					if (category == ShopNGUIController.CategoryNames.CapesCategory)
					{
						component.arrnoInitialPos = new Vector3(75f, component.arrnoInitialPos.y, component.arrnoInitialPos.z);
					}
					if (category == ShopNGUIController.CategoryNames.BootsCategory)
					{
						component.arrnoInitialPos = new Vector3(81f, component.arrnoInitialPos.y, component.arrnoInitialPos.z);
					}
					if (category == ShopNGUIController.CategoryNames.MaskCategory)
					{
						component.arrnoInitialPos = new Vector3(75f, component.arrnoInitialPos.y, component.arrnoInitialPos.z);
					}
				}
			}, category, false, (!ShopNGUIController.IsWeaponCategory(category) ? null : item.GetComponent<WeaponSounds>()));
			int num5 = -1;
			if (this.WeaponCategory)
			{
				ItemRecord itemRecord = ItemDb.GetByTag(str5);
				num5 = (itemRecord == null ? -1 : gameObjects.FindIndex((GameObject go) => go.name.Replace("(Clone)", string.Empty) == itemRecord.PrefabName));
			}
			else if (this.currentCategory != ShopNGUIController.CategoryNames.SkinsCategory)
			{
				num5 = gameObjects.FindIndex((GameObject go) => go.nameNoClone() == str5);
			}
			if ((num5 < 0 || num3 < 0 ? false : Mathf.Abs(num3 - num5) <= 2) || this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory || this.EnableConfigurePos)
			{
				Action<GameObject, ShopNGUIController.CategoryNames> action1 = action;
				if (!this.WeaponCategory)
				{
					gameObject = (this.currentCategory == ShopNGUIController.CategoryNames.SkinsCategory ? item : ItemDb.GetWearFromResources(item.nameNoClone(), this.currentCategory));
				}
				else
				{
					gameObject = WeaponManager.InnerPrefabForWeaponSync(item.nameNoClone());
				}
				action1(gameObject, this.currentCategory);
			}
			else
			{
				CoroutineRunner.Instance.StartCoroutine(this.LoadModelAsync(action, item, this.currentCategory));
			}
		}
		this.wrapContent.Reposition();
		this.ChooseCarouselItem(str3, true, false);
	}

	public void ReloadCategoryTempItemsRemoved(List<string> expired)
	{
		if (this.currentCategory != ShopNGUIController.CategoryNames.HatsCategory && expired.Contains("hat_Adamant_3"))
		{
			this.UpdatePersHat(Defs.HatNoneEqupped);
		}
		if (this.currentCategory != ShopNGUIController.CategoryNames.ArmorCategory && expired.Contains("Armor_Adamant_3"))
		{
			this.UpdatePersArmor(Defs.ArmorNewNoneEqupped);
		}
		if (!ShopNGUIController.GuiActive || !TempItemsController.IsCategoryContainsTempItems(this.currentCategory))
		{
			return;
		}
		this.CategoryChosen(this.currentCategory, (expired.Count <= 0 || !TempItemsController.GunsMappingFromTempToConst.ContainsKey(expired[0]) ? this.viewedId : TempItemsController.GunsMappingFromTempToConst[expired[0]]), false);
		this.UpdateIcons();
	}

	private static void SaveSkinAndSendToServer(string id)
	{
		SkinsController.SetCurrentSkin(id);
		byte[] pNG = SkinsController.currentSkinForPers.EncodeToPNG();
		if (pNG != null)
		{
			string base64String = Convert.ToBase64String(pNG);
			if (base64String != null)
			{
				FriendsController.sharedController.skin = base64String;
				FriendsController.sharedController.SendOurData(true);
			}
		}
	}

	public static void SetAsEquippedAndSendToServer(string tg, ShopNGUIController.CategoryNames c)
	{
		Storager.setString(ShopNGUIController.SnForWearCategory(c), tg, false);
		if (FriendsController.sharedController == null)
		{
			UnityEngine.Debug.LogError("FriendsController.sharedController == null");
			return;
		}
		FriendsController.sharedController.SendAccessories();
	}

	private void setBackBlinking()
	{
		try
		{
			base.StopCoroutine("Blink");
			switch (this.trainingState)
			{
				case ShopNGUIController.TrainingState.OnSniperRifle:
				{
					this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
					this.equips[1].normalSprite = "or_btn";
					break;
				}
				case ShopNGUIController.TrainingState.InSniperCategoryNotOnSniperRifle:
				case ShopNGUIController.TrainingState.NotInArmorCategory:
				{
					break;
				}
				case ShopNGUIController.TrainingState.OnArmor:
				{
					this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
					this.equips[1].normalSprite = "or_btn";
					break;
				}
				default:
				{
					goto case ShopNGUIController.TrainingState.NotInArmorCategory;
				}
			}
			this.trainingState = ShopNGUIController.TrainingState.BackBlinking;
			this.toBlink = this.backButton.tweenTarget.GetComponent<UISprite>();
			base.StartCoroutine("Blink", new string[] { "yell_btn", "green_btn" });
			this.trainingColliders.SetActive(false);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setBackBlinking: ", exception));
		}
	}

	private static void SetBankCamerasEnabled()
	{
		foreach (Camera rect in ShopNGUIController.BankRelatedCameras())
		{
			if (!(ExpController.Instance != null) || !ExpController.Instance.IsRenderedWithCamera(rect))
			{
				if (!rect.gameObject.tag.Equals("CamTemp"))
				{
					if (!ShopNGUIController.sharedShop.ourCameras.Contains(rect))
					{
						rect.rect = new Rect(0f, 0f, 1f, 1f);
					}
				}
			}
		}
	}

	public void SetCamera()
	{
		Transform mainMenuPers = this.MainMenu_Pers;
		HOTween.Kill(mainMenuPers);
		Vector3 vector3 = new Vector3(0f, 0f, 0f);
		Vector3 vector31 = new Vector3(0f, 0f, 0f);
		Vector3 vector32 = new Vector3(1f, 1f, 1f);
		ShopNGUIController.CategoryNames categoryName = this.currentCategory;
		switch (categoryName)
		{
			case ShopNGUIController.CategoryNames.HatsCategory:
			{
				vector3 = new Vector3(1.06f, -0.54f, 2.19f);
				vector31 = new Vector3(0f, -9.5f, 0f);
				break;
			}
			case ShopNGUIController.CategoryNames.CapesCategory:
			{
				vector3 = new Vector3(0f, 0f, 0f);
				vector31 = new Vector3(0f, -130.76f, 0f);
				break;
			}
			default:
			{
				if (categoryName == ShopNGUIController.CategoryNames.MaskCategory)
				{
					vector3 = new Vector3(1.06f, -0.54f, 2.19f);
					vector31 = new Vector3(0f, -9.5f, 0f);
					break;
				}
				else
				{
					vector3 = new Vector3(0f, 0f, 0f);
					vector31 = new Vector3(0f, 0f, 0f);
					break;
				}
			}
		}
		float single = 0.5f;
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		HOTween.To(mainMenuPers, single, (new TweenParms()).Prop("localPosition", vector3).Prop("localRotation", new PlugQuaternion(vector31)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear).OnComplete(() => this.idleTimerLastTime = Time.realtimeSinceStartup));
	}

	private static void SetIconChosen(ShopNGUIController.CategoryNames cn)
	{
		for (int i = 0; i < (int)ShopNGUIController.sharedShop.category.buttons.Length; i++)
		{
			ShopNGUIController.sharedShop.category.buttons[i].SetCheckedImage(i == (int)cn);
			if (i == (int)cn)
			{
				ShopNGUIController.sharedShop.category.buttons[i].onButton.GetComponent<BoxCollider>().enabled = false;
			}
		}
	}

	public void SetIconModelsPositions(Transform t, ShopNGUIController.CategoryNames c)
	{
		switch (c)
		{
			case ShopNGUIController.CategoryNames.HatsCategory:
			{
				t.transform.localPosition = new Vector3(-0.62f, -0.04f, 0f);
				t.transform.localRotation = Quaternion.Euler(new Vector3(-75f, -165f, -90f));
				float single = 82.5f;
				t.transform.localScale = new Vector3(single, single, single);
				break;
			}
			case ShopNGUIController.CategoryNames.ArmorCategory:
			{
				t.transform.localPosition = new Vector3(0f, 0f, 0f);
				t.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
				float single1 = 1f;
				t.transform.localScale = new Vector3(single1, single1, single1);
				break;
			}
			case ShopNGUIController.CategoryNames.SkinsCategory:
			{
				SkinsController.SetTransformParamtersForSkinModel(t);
				break;
			}
			case ShopNGUIController.CategoryNames.CapesCategory:
			{
				t.transform.localPosition = new Vector3(-0.720093f, -0.00859833f, 0f);
				t.transform.localRotation = Quaternion.Euler(new Vector3(0f, 30f, -15f));
				float single2 = 50f;
				t.transform.localScale = new Vector3(single2, single2, single2);
				break;
			}
			case ShopNGUIController.CategoryNames.BootsCategory:
			{
				t.transform.localPosition = new Vector3(-0.4f, -0.1f, 0f);
				t.transform.localRotation = Quaternion.Euler(new Vector3(13f, 149f, 0f));
				float single3 = 75f;
				t.transform.localScale = new Vector3(single3, single3, single3);
				break;
			}
			case ShopNGUIController.CategoryNames.GearCategory:
			{
				t.transform.localPosition = new Vector3(4.648193f, 2.444565f, 0f);
				t.transform.localRotation = Quaternion.Euler(new Vector3(0f, 30f, -30f));
				float single4 = 319.3023f;
				t.transform.localScale = new Vector3(single4, single4, single4);
				break;
			}
		}
	}

	private void setInArmorCategoryNotOnArmor()
	{
		try
		{
			base.StopCoroutine("Blink");
			if (this.trainingState == ShopNGUIController.TrainingState.OnArmor)
			{
				this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				this.equips[1].normalSprite = "or_btn";
			}
			this.trainingState = ShopNGUIController.TrainingState.InArmorCategoryNotOnArmor;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setInArmorCategoryNotOnArmor: ", exception));
		}
	}

	private void setInArmorCategoryNotOnArmorRemovedNoviceArmor()
	{
		try
		{
			base.StopCoroutine("Blink");
			if (this.trainingStateRemoveNoviceArmor == ShopNGUIController.TrainingState.OnArmor)
			{
				this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				this.equips[1].normalSprite = "or_btn";
			}
			this.trainingStateRemoveNoviceArmor = ShopNGUIController.TrainingState.InArmorCategoryNotOnArmor;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setInArmorCategoryNotOnArmorRemovedNoviceArmor: ", exception));
		}
	}

	public void SetInGame(bool e)
	{
		this.inGame = e;
	}

	private void setInSniperCategoryNotOnSniperRifle()
	{
		try
		{
			base.StopCoroutine("Blink");
			if (this.trainingState == ShopNGUIController.TrainingState.OnSniperRifle)
			{
				this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				this.equips[1].normalSprite = "or_btn";
			}
			this.trainingState = ShopNGUIController.TrainingState.InSniperCategoryNotOnSniperRifle;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setInSniperCategoryNotOnSniperRifle: ", exception));
		}
	}

	private void setNotInArmorCategory()
	{
		try
		{
			base.StopCoroutine("Blink");
			switch (this.trainingState)
			{
				case ShopNGUIController.TrainingState.OnSniperRifle:
				{
					this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
					this.equips[1].normalSprite = "or_btn";
					break;
				}
				case ShopNGUIController.TrainingState.InSniperCategoryNotOnSniperRifle:
				case ShopNGUIController.TrainingState.NotInArmorCategory:
				{
					break;
				}
				case ShopNGUIController.TrainingState.OnArmor:
				{
					this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
					this.equips[1].normalSprite = "or_btn";
					break;
				}
				default:
				{
					goto case ShopNGUIController.TrainingState.NotInArmorCategory;
				}
			}
			this.trainingState = ShopNGUIController.TrainingState.NotInArmorCategory;
			this.toBlink = this.category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>();
			base.StartCoroutine("Blink", new string[] { "green_btn", "trans_btn" });
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setNotInArmorCategory: ", exception));
		}
	}

	private void setNotInArmorCategoryRemovedNoviceArmor()
	{
		try
		{
			base.StopCoroutine("Blink");
			if (this.trainingStateRemoveNoviceArmor == ShopNGUIController.TrainingState.OnArmor)
			{
				this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				this.equips[1].normalSprite = "or_btn";
			}
			this.trainingStateRemoveNoviceArmor = ShopNGUIController.TrainingState.NotInArmorCategory;
			this.toBlink = this.category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>();
			base.StartCoroutine("Blink", new string[] { "green_btn", "trans_btn" });
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setNotInArmorCategoryRemovedNoviceArmor: ", exception));
		}
	}

	private void setNotInSniperCategory()
	{
		try
		{
			base.StopCoroutine("Blink");
			if (this.trainingState == ShopNGUIController.TrainingState.OnSniperRifle)
			{
				this.equips[1].tweenTarget.GetComponent<UISprite>().spriteName = "or_btn";
				this.equips[1].normalSprite = "or_btn";
			}
			this.trainingState = ShopNGUIController.TrainingState.NotInSniperCategory;
			this.toBlink = this.category.buttons[4].offButton.tweenTarget.GetComponent<UISprite>();
			base.StartCoroutine("Blink", new string[] { "green_btn", "trans_btn" });
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setNotInSniperCategory: ", exception));
		}
	}

	public void SetOfferID(string oid)
	{
		this.offerID = oid;
	}

	private void setOnArmor()
	{
		try
		{
			base.StopCoroutine("Blink");
			if (this.trainingState == ShopNGUIController.TrainingState.NotInArmorCategory)
			{
				this.category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
				this.category.buttons[7].offButton.normalSprite = "trans_btn";
			}
			this.trainingState = ShopNGUIController.TrainingState.OnArmor;
			this.toBlink = this.equips[1].tweenTarget.GetComponent<UISprite>();
			base.StartCoroutine("Blink", new string[] { "or_btn", "green_btn" });
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setOnArmor: ", exception));
		}
	}

	private void setOnArmorRemovedNoviceArmor()
	{
		try
		{
			base.StopCoroutine("Blink");
			if (this.trainingStateRemoveNoviceArmor == ShopNGUIController.TrainingState.NotInArmorCategory)
			{
				this.category.buttons[7].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
				this.category.buttons[7].offButton.normalSprite = "trans_btn";
			}
			this.trainingStateRemoveNoviceArmor = ShopNGUIController.TrainingState.OnArmor;
			this.toBlink = this.equips[1].tweenTarget.GetComponent<UISprite>();
			base.StartCoroutine("Blink", new string[] { "or_btn", "green_btn" });
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setOnArmorRemovedNoviceArmor: ", exception));
		}
	}

	private void setOnSniperRifle()
	{
		try
		{
			base.StopCoroutine("Blink");
			if (this.trainingState == ShopNGUIController.TrainingState.NotInSniperCategory)
			{
				this.category.buttons[4].offButton.tweenTarget.GetComponent<UISprite>().spriteName = "trans_btn";
				this.category.buttons[4].offButton.normalSprite = "trans_btn";
			}
			this.trainingState = ShopNGUIController.TrainingState.OnSniperRifle;
			this.toBlink = this.equips[1].tweenTarget.GetComponent<UISprite>();
			base.StartCoroutine("Blink", new string[] { "or_btn", "green_btn" });
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShopNGUIController.setOnSniperRifle: ", exception));
		}
	}

	public void SetOtherCamerasEnabled(bool e)
	{
		object obj;
		object obj1;
		List<Camera> list = ((IEnumerable<Camera>)(Camera.allCameras ?? new Camera[0])).ToList<Camera>();
		List<Camera> cameras = ProfileController.Instance.GetComponentsInChildren<Camera>(true).ToList<Camera>();
		list.AddRange(cameras);
		list.AddRange(ShopNGUIController.BankRelatedCameras());
		foreach (Camera camera in list)
		{
			if (!(ExpController.Instance != null) || !ExpController.Instance.IsRenderedWithCamera(camera))
			{
				if (!camera.gameObject.tag.Equals("CamTemp"))
				{
					if (!ShopNGUIController.sharedShop.ourCameras.Contains(camera))
					{
						Camera rect = camera;
						if (!e)
						{
							obj = null;
						}
						else
						{
							obj = 1;
						}
						float single = (float)obj;
						if (!e)
						{
							obj1 = null;
						}
						else
						{
							obj1 = 1;
						}
						rect.rect = new Rect(0f, 0f, single, (float)obj1);
					}
				}
			}
		}
	}

	public static void SetPersArmorVisible(Transform armorPoint)
	{
		ShopNGUIController.SetRenderersVisibleFromPoint(armorPoint, ShopNGUIController.ShowArmor);
		if (armorPoint.childCount > 0)
		{
			Transform child = armorPoint.GetChild(0);
			ArmorRefs component = child.GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					ShopNGUIController.SetRenderersVisibleFromPoint(component.leftBone, ShopNGUIController.ShowArmor);
				}
				if (component.rightBone != null)
				{
					ShopNGUIController.SetRenderersVisibleFromPoint(component.rightBone, ShopNGUIController.ShowArmor);
				}
			}
		}
	}

	public static void SetPersHatVisible(Transform hatPoint)
	{
	}

	public static void SetRenderersVisibleFromPoint(Transform pt, bool showArmor)
	{
		Player_move_c.PerformActionRecurs(pt.gameObject, (Transform t) => {
			Renderer component = t.GetComponent<Renderer>();
			if (component != null)
			{
				component.material.shader = Shader.Find((!showArmor ? "Mobile/Transparent-Shop" : "Mobile/Diffuse"));
			}
		});
	}

	public void SetSkinAsCurrent(string id)
	{
		ShopNGUIController.SaveSkinAndSendToServer(id);
		this.FireOnEquipSkin(id);
	}

	public void SetSkinOnPers(Texture skin)
	{
		WeaponSounds component;
		GameObject gameObject;
		if (this.body.transform.childCount <= 0)
		{
			component = null;
		}
		else
		{
			component = this.body.transform.GetChild(0).GetComponent<WeaponSounds>();
		}
		WeaponSounds weaponSound = component;
		if (weaponSound == null)
		{
			gameObject = null;
		}
		else
		{
			gameObject = weaponSound.bonusPrefab;
		}
		GameObject gameObject1 = gameObject;
		GameObject gameObject2 = null;
		GameObject gameObject3 = null;
		if (gameObject1 != null)
		{
			Transform leftArmorHand = weaponSound.LeftArmorHand;
			Transform rightArmorHand = weaponSound.RightArmorHand;
			if (leftArmorHand != null)
			{
				gameObject2 = leftArmorHand.gameObject;
			}
			if (rightArmorHand != null)
			{
				gameObject3 = rightArmorHand.gameObject;
			}
		}
		List<GameObject> gameObjects = new List<GameObject>()
		{
			this.capePoint.gameObject,
			this.hatPoint.gameObject,
			this.bootsPoint.gameObject,
			this.armorPoint.gameObject,
			this.maskPoint.gameObject
		};
		List<GameObject> gameObjects1 = gameObjects;
		if (weaponSound != null && weaponSound.grenatePoint != null)
		{
			gameObjects1.Add(weaponSound.grenatePoint.gameObject);
		}
		if (gameObject1 != null)
		{
			gameObjects1.Add(gameObject1);
		}
		if (gameObject2 != null)
		{
			gameObjects1.Add(gameObject2);
		}
		if (gameObject3 != null)
		{
			gameObjects1.Add(gameObject3);
		}
		if (weaponSound != null)
		{
			List<GameObject> listWeaponAnimEffects = weaponSound.GetListWeaponAnimEffects();
			if (listWeaponAnimEffects != null)
			{
				gameObjects1.AddRange(listWeaponAnimEffects);
			}
		}
		Player_move_c.SetTextureRecursivelyFrom(this.MainMenu_Pers.gameObject, skin, gameObjects1.ToArray());
	}

	private void SetUpUpgradesAndTiers(bool bought, ref bool buyActive, ref bool upgradeActive, ref bool saleActive, ref bool needTierActive, ref bool rentActive, ref bool saleRentActive)
	{
		bool flag;
		bool flag1 = TempItemsController.PriceCoefs.ContainsKey(this.viewedId);
		bool flag2 = false;
		int num = (this.viewedId == null ? -1 : ShopNGUIController._CurrentNumberOfWearUpgrades(this.viewedId, out flag2, this.currentCategory));
		bool flag3 = flag2;
		if ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted) && !this.InTrainingAfterNoviceArmorRemoved)
		{
			buyActive = (this.viewedId == null || flag3 || num != 0 || this.viewedId.Equals("cape_Custom") ? false : !flag1);
			upgradeActive = (this.viewedId == null || flag3 || num == 0 ? false : !flag1);
			rentActive = false;
		}
		else
		{
			buyActive = false;
			upgradeActive = false;
		}
		if (!flag3)
		{
			int num1 = Wear.TierForWear(WeaponManager.FirstUnboughtTag(this.viewedId));
			needTierActive = (!upgradeActive || !(ExpController.Instance != null) || ExpController.Instance.OurTier >= num1 || flag1 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
			if (needTierActive)
			{
				int num2 = (num1 < 0 || num1 >= (int)ExpController.LevelsForTiers.Length ? ExpController.LevelsForTiers[(int)ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[num1]);
				string str = string.Format("{0} {1} {2}", LocalizationStore.Key_0226, num2, LocalizationStore.Get("Key_1022"));
				this.needTierLabel.text = str;
			}
			this.upgrade.isEnabled = (!upgradeActive || !(ExpController.Instance != null) || ExpController.Instance.OurTier >= num1 ? !flag1 : false);
		}
		string str1 = WeaponManager.FirstUnboughtTag(this.viewedId);
		int num3 = ShopNGUIController.DiscountFor(str1, out flag);
		if (flag3 || flag1 || needTierActive || num3 <= 0 || str1 != null && str1.Equals("cape_Custom") && this.inGame || str1 == "cape_Custom" && Defs.isDaterRegim)
		{
			saleActive = false;
		}
		else
		{
			saleActive = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted ? !this.InTrainingAfterNoviceArmorRemoved : false);
			this.salePerc.text = string.Concat(num3, "%");
		}
		saleRentActive = false;
	}

	public void SetWeapon(string tg)
	{
		this.animationCoroutineRunner.StopAllCoroutines();
		if (WeaponManager.sharedManager == null)
		{
			return;
		}
		if (this.armorPoint.childCount > 0)
		{
			ArmorRefs component = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					Vector3 vector3 = component.leftBone.position;
					Quaternion quaternion = component.leftBone.rotation;
					component.leftBone.parent = this.armorPoint.GetChild(0).GetChild(0);
					component.leftBone.position = vector3;
					component.leftBone.rotation = quaternion;
				}
				if (component.rightBone != null)
				{
					Vector3 vector31 = component.rightBone.position;
					Quaternion quaternion1 = component.rightBone.rotation;
					component.rightBone.parent = this.armorPoint.GetChild(0).GetChild(0);
					component.rightBone.position = vector31;
					component.rightBone.rotation = quaternion1;
				}
			}
		}
		List<Transform> transforms = new List<Transform>();
		IEnumerator enumerator = this.body.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				transforms.Add((Transform)enumerator.Current);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		foreach (Transform transform in transforms)
		{
			transform.parent = null;
			transform.position = new Vector3(0f, -10000f, 0f);
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		if (tg == null)
		{
			return;
		}
		if (this.profile != null)
		{
			Resources.UnloadAsset(this.profile);
			this.profile = null;
		}
		ItemRecord byTag = ItemDb.GetByTag(tg);
		if (byTag == null || string.IsNullOrEmpty(byTag.PrefabName))
		{
			UnityEngine.Debug.Log("rec == null || string.IsNullOrEmpty(rec.PrefabName)");
			return;
		}
		GameObject gameObject = Resources.Load<GameObject>(string.Concat("Weapons/", byTag.PrefabName));
		if (gameObject == null)
		{
			UnityEngine.Debug.Log("pref==null");
			return;
		}
		this.profile = Resources.Load<AnimationClip>(string.Concat("ProfileAnimClips/", gameObject.name, "_Profile"));
		GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(gameObject1);
		Player_move_c.SetLayerRecursively(gameObject1, LayerMask.NameToLayer("NGUIShop"));
		gameObject1.transform.parent = this.body.transform;
		this.weapon = gameObject1;
		this.weapon.transform.localScale = new Vector3(1f, 1f, 1f);
		this.weapon.transform.position = this.body.transform.position;
		this.weapon.transform.localPosition = Vector3.zero;
		this.weapon.transform.localRotation = Quaternion.identity;
		WeaponSounds weaponSound = this.weapon.GetComponent<WeaponSounds>();
		if (this.armorPoint.childCount > 0 && weaponSound != null)
		{
			ArmorRefs leftArmorHand = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (leftArmorHand != null)
			{
				if (leftArmorHand.leftBone != null && weaponSound.LeftArmorHand != null)
				{
					leftArmorHand.leftBone.parent = weaponSound.LeftArmorHand;
					leftArmorHand.leftBone.localPosition = Vector3.zero;
					leftArmorHand.leftBone.localRotation = Quaternion.identity;
					leftArmorHand.leftBone.localScale = new Vector3(1f, 1f, 1f);
				}
				if (leftArmorHand.rightBone != null && weaponSound.RightArmorHand != null)
				{
					leftArmorHand.rightBone.parent = weaponSound.RightArmorHand;
					leftArmorHand.rightBone.localPosition = Vector3.zero;
					leftArmorHand.rightBone.localRotation = Quaternion.identity;
					leftArmorHand.rightBone.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
		this.PlayWeaponAnimation();
		this.DisableGunflashes(this.weapon);
		if (SkinsController.currentSkinForPers != null)
		{
			this.SetSkinOnPers(SkinsController.currentSkinForPers);
		}
		this._assignedWeaponTag = tg;
	}

	private void SetWearForCategory(ShopNGUIController.CategoryNames cat, string wear)
	{
		switch (cat)
		{
			case ShopNGUIController.CategoryNames.HatsCategory:
			{
				this._currentHat = wear;
				return;
			}
			case ShopNGUIController.CategoryNames.ArmorCategory:
			{
				this._currentArmor = wear;
				return;
			}
			case ShopNGUIController.CategoryNames.SkinsCategory:
			case ShopNGUIController.CategoryNames.GearCategory:
			{
				return;
			}
			case ShopNGUIController.CategoryNames.CapesCategory:
			{
				this._currentCape = wear;
				return;
			}
			case ShopNGUIController.CategoryNames.BootsCategory:
			{
				this._currentBoots = wear;
				return;
			}
			case ShopNGUIController.CategoryNames.MaskCategory:
			{
				this._currentMask = wear;
				return;
			}
			default:
			{
				return;
			}
		}
	}

	public static void ShowAddTryGun(string gunTag, Transform point, string lr, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitCoinsShopAdditional = null, Action<string> customEquipWearAction = null, bool expiredTryGun = false)
	{
		try
		{
			GameObject gameObject = Resources.Load<GameObject>("TryGunScreen");
			TryGunScreenController component = UnityEngine.Object.Instantiate<GameObject>(gameObject).GetComponent<TryGunScreenController>();
			Player_move_c.SetLayerRecursively(component.gameObject, LayerMask.NameToLayer(lr));
			component.transform.parent = point;
			component.transform.localPosition = new Vector3(0f, 0f, -130f);
			component.transform.localScale = new Vector3(1f, 1f, 1f);
			if (expiredTryGun)
			{
				WeaponManager.sharedManager.AddTryGunPromo(gunTag);
			}
			component.ItemTag = gunTag;
			component.onPurchaseCustomAction = onPurchase;
			component.onEnterCoinsShopAdditionalAction = onEnterCoinsShopAdditional;
			component.onExitCoinsShopAdditionalAction = onExitCoinsShopAdditional;
			component.customEquipWearAction = customEquipWearAction;
			component.ExpiredTryGun = expiredTryGun;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in ShowAddTryGun: ", exception));
		}
	}

	public static bool ShowLockedFacebookSkin()
	{
		bool flag;
		if (!FacebookController.FacebookSupported)
		{
			return false;
		}
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && SkinsController.shopKeyFromNameSkin.ContainsKey("61"))
		{
			string item = SkinsController.shopKeyFromNameSkin["61"];
			if (Array.IndexOf<string>(StoreKitEventListener.skinIDs, item) >= 0)
			{
				Dictionary<int, KeyValuePair<string, string>>.ValueCollection.Enumerator enumerator = InAppData.inAppData.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> current = enumerator.Current;
						if (current.Key == null || !current.Key.Equals(item) || Storager.getInt(current.Value, true) != 0)
						{
							continue;
						}
						flag = false;
						return flag;
					}
					return true;
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				return flag;
			}
		}
		return true;
	}

	public static bool ShowPremimAccountExpiredIfPossible(Transform point, string layer, string header = "", bool showOnlyIfExpired = true)
	{
		if (showOnlyIfExpired && (!PremiumAccountController.AccountHasExpired || !Defs2.CanShowPremiumAccountExpiredWindow))
		{
			return false;
		}
		if (point == null)
		{
			return false;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("PremiumAccount"));
		vector3.transform.parent = point;
		Player_move_c.SetLayerRecursively(vector3, LayerMask.NameToLayer(layer ?? "Default"));
		vector3.transform.localPosition = new Vector3(0f, 0f, -130f);
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		vector3.GetComponent<PremiumAccountScreenController>().Header = header;
		PremiumAccountController.AccountHasExpired = false;
		return true;
	}

	public static void ShowRentScreen(string itemTag, Transform point, string lr, string hdr, string rentText, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitCoinsShopAdditional = null, Action<string> customEquipWearAction = null)
	{
	}

	public static void ShowTempItemExpiredIfPossible(Transform point, string layer, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitoinsShopAdditional = null, Action<string> customEquipWearAction = null)
	{
		List<string> strs = new List<string>();
		foreach (string expiredItem in TempItemsController.sharedController.ExpiredItems)
		{
			if (!TempItemsController.sharedController.CanShowExpiredBannerForTag(expiredItem))
			{
				continue;
			}
			ShopNGUIController.ShowRentScreen(expiredItem, point, layer, LocalizationStore.Get("Key_1156"), LocalizationStore.Get("Key_1157"), onPurchase, onEnterCoinsShopAdditional, onExitoinsShopAdditional, customEquipWearAction);
			strs.Add(expiredItem);
			break;
		}
		foreach (string str in strs)
		{
			TempItemsController.sharedController.ExpiredItems.Remove(str);
		}
	}

	public static void ShowTryGunIfPossible(bool placeForGiveNewTryGun, Transform point, string layer, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitoinsShopAdditional = null, Action<string> customEquipWearAction = null)
	{
		if (!Defs.isHunger && !placeForGiveNewTryGun && WeaponManager.sharedManager != null && WeaponManager.sharedManager.ExpiredTryGuns.Count > 0 && TrainingController.TrainingCompleted)
		{
			foreach (string expiredTryGun in WeaponManager.sharedManager.ExpiredTryGuns)
			{
				try
				{
					if (WeaponManager.sharedManager.weaponsInGame.FirstOrDefault<UnityEngine.Object>((UnityEngine.Object w) => ItemDb.GetByPrefabName(w.name).Tag == expiredTryGun) != null)
					{
						WeaponManager.sharedManager.ExpiredTryGuns.RemoveAll((string t) => t == expiredTryGun);
						if (WeaponManager.LastBoughtTag(expiredTryGun) == null)
						{
							ShopNGUIController.ShowAddTryGun(expiredTryGun, point, layer, onPurchase, onEnterCoinsShopAdditional, onExitoinsShopAdditional, customEquipWearAction, true);
							break;
						}
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(string.Concat("Exception in foreach (var tg in WeaponManager.sharedManager.ExpiredTryGuns): ", exception));
				}
			}
		}
		else if (!Defs.isHunger && !Defs.isDaterRegim && WeaponManager.sharedManager._currentFilterMap == 0)
		{
			if ((!FriendsController.useBuffSystem ? KillRateCheck.instance.giveWeapon : BuffSystem.instance.giveTryGun) && TrainingController.TrainingCompleted)
			{
				try
				{
					int num = ShopNGUIController.UpperCoinsBankBound();
					List<ItemRecord> list = WeaponManager.tryGunsTable.SelectMany<KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>>, string>((KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> kvp) => kvp.Value[ExpController.OurTierForAnyPlace()]).Select<string, ItemRecord>((string prefabName) => ItemDb.GetByPrefabName(prefabName)).Where<ItemRecord>((ItemRecord rec) => (rec.StorageId == null ? false : Storager.getInt(rec.StorageId, true) == 0)).Where<ItemRecord>((ItemRecord rec) => (WeaponManager.sharedManager.IsAvailableTryGun(rec.Tag) ? false : !WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(rec.Tag))).ToList<ItemRecord>();
					List<ItemRecord> itemRecords = (
						from rec in list
						where rec.Price.Currency == "Coins"
						where ShopNGUIController.PriceIfGunWillBeTryGun(rec.Tag) > num
						select rec).Randomize<ItemRecord>().ToList<ItemRecord>();
					string tag = null;
					if (!itemRecords.Any<ItemRecord>())
					{
						int num1 = ShopNGUIController.UpperGemsBankBound();
						List<ItemRecord> list1 = (
							from rec in list
							where rec.Price.Currency == "GemsCurrency"
							where ShopNGUIController.PriceIfGunWillBeTryGun(rec.Tag) > num1
							select rec).Randomize<ItemRecord>().ToList<ItemRecord>();
						tag = (!list1.Any<ItemRecord>() ? ShopNGUIController.TryGunForCategoryWithMaxUnbought() : list1.First<ItemRecord>().Tag);
					}
					else
					{
						tag = itemRecords.First<ItemRecord>().Tag;
					}
					if (tag != null)
					{
						ShopNGUIController.ShowAddTryGun(tag, point, layer, onPurchase, onEnterCoinsShopAdditional, onExitoinsShopAdditional, customEquipWearAction, false);
					}
				}
				catch (Exception exception1)
				{
					UnityEngine.Debug.LogError(string.Concat("Exception in giving: ", exception1));
				}
			}
		}
	}

	public void SimulateCategoryChoose(int num)
	{
		if (num >= 0 && num < (int)this.category.buttons.Length && num != 0)
		{
			this.category.buttons[0].IsChecked = false;
			this.category.buttons[num].IsChecked = true;
		}
	}

	public static string SnForWearCategory(ShopNGUIController.CategoryNames c)
	{
		string capeEquppedSN;
		if (c == ShopNGUIController.CategoryNames.CapesCategory)
		{
			capeEquppedSN = Defs.CapeEquppedSN;
		}
		else if (c == ShopNGUIController.CategoryNames.BootsCategory)
		{
			capeEquppedSN = Defs.BootsEquppedSN;
		}
		else if (c != ShopNGUIController.CategoryNames.ArmorCategory)
		{
			capeEquppedSN = (c != ShopNGUIController.CategoryNames.MaskCategory ? Defs.HatEquppedSN : "MaskEquippedSN");
		}
		else
		{
			capeEquppedSN = Defs.ArmorNewEquppedSN;
		}
		return capeEquppedSN;
	}

	public static void SpendBoughtCurrency(string currency, int count)
	{
		if (currency == null)
		{
			UnityEngine.Debug.LogWarning("SpendBoughtCurrency: currency == null");
			return;
		}
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log(string.Format("<color=#ff00ffff>SpendBoughtCurrency {0} {1}</color>", currency, count));
		}
	}

	private void Start()
	{
		base.StartCoroutine(this.TryToShowExpiredBanner());
	}

	internal static void SynchronizeAndroidPurchases(string comment)
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			UnityEngine.Debug.LogFormat("Trying to synchronize purchases to cloud ({0})", new object[] { comment });
			Action action = () => {
				PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
				if (WeaponManager.sharedManager != null)
				{
					int currentWeaponIndex = WeaponManager.sharedManager.CurrentWeaponIndex;
					WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? 0 : Defs.filterMaps[Application.loadedLevelName]));
					WeaponManager.sharedManager.CurrentWeaponIndex = currentWeaponIndex;
				}
				if (ShopNGUIController.GuiActive)
				{
					ShopNGUIController.sharedShop.UpdateIcons();
				}
			};
			Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
			if (androidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				PurchasesSynchronizer.Instance.SynchronizeAmazonPurchases();
				action();
			}
			else if (androidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				PlayerPrefs.SetInt("PendingGooglePlayGamesSync", 1);
				PurchasesSynchronizer.Instance.AuthenticateAndSynchronize((bool success) => {
					UnityEngine.Debug.LogFormat("[Rilisoft] ShopNguiController.PurchasesSynchronizer.Callback({0}) >: {1:F3}", new object[] { success, Time.realtimeSinceStartup });
					try
					{
						UnityEngine.Debug.LogFormat("Google purchases syncronized ({0}): {1}", new object[] { comment, success });
						if (success)
						{
							action();
						}
					}
					finally
					{
						UnityEngine.Debug.LogFormat("[Rilisoft] ShopNguiController.PurchasesSynchronizer.Callback({0}) <: {1:F3}", new object[] { success, Time.realtimeSinceStartup });
					}
				}, true);
			}
		}
	}

	public static string TempGunOrHighestDPSGun(ShopNGUIController.CategoryNames c, out ShopNGUIController.CategoryNames cn)
	{
		cn = c;
		int num = (int)c;
		string str = null;
		str = ShopNGUIController.TemppOrHighestDPSGunInCategory(num);
		if (str == null && WeaponManager.sharedManager.playerWeapons.Count > 0)
		{
			int component = (WeaponManager.sharedManager.playerWeapons[0] as Weapon).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
			str = ShopNGUIController.TemppOrHighestDPSGunInCategory(component);
			cn = (ShopNGUIController.CategoryNames)component;
		}
		return str;
	}

	private static string TemppOrHighestDPSGunInCategory(int cInt)
	{
		string tag = null;
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.FilteredShopLists != null && WeaponManager.sharedManager.FilteredShopLists.Count > cInt)
		{
			List<GameObject> item = WeaponManager.sharedManager.FilteredShopLists[cInt];
			GameObject gameObject = item.Find((GameObject w) => ItemDb.IsTemporaryGun(ItemDb.GetByPrefabName(w.name.Replace("(Clone)", string.Empty)).Tag));
			if (gameObject != null)
			{
				tag = ItemDb.GetByPrefabName(gameObject.name.Replace("(Clone)", string.Empty)).Tag;
			}
			if (tag == null && item.Count > 0)
			{
				int count = item.Count - 1;
				while (count >= 0)
				{
					string str = ItemDb.GetByPrefabName(item[count].name.Replace("(Clone)", string.Empty)).Tag;
					if (ItemDb.IsTemporaryGun(str) || !(ExpController.Instance != null) || item[count].GetComponent<WeaponSounds>().tier > ExpController.Instance.OurTier)
					{
						count--;
					}
					else
					{
						tag = str;
						break;
					}
				}
			}
		}
		return tag;
	}

	public static Texture TextureForCat(int cc)
	{
		bool flag;
		string str;
		ShopNGUIController.CategoryNames categoryName = (ShopNGUIController.CategoryNames)cc;
		if (!ShopNGUIController.IsWeaponCategory(categoryName))
		{
			str = (!ShopNGUIController.IsWearCategory(categoryName) ? "potion" : ShopNGUIController.sharedShop.WearForCat(categoryName));
		}
		else
		{
			str = ShopNGUIController._CurrentWeaponSetIDs()[(int)categoryName];
		}
		string str1 = str;
		if (str1 == null)
		{
			return null;
		}
		str1 = ShopNGUIController.ItemIDForPrefab(str1, categoryName);
		int num = 1;
		if (ShopNGUIController.IsWeaponCategory(categoryName))
		{
			ItemRecord byTag = ItemDb.GetByTag(str1);
			if ((byTag == null || !byTag.UseImagesFromFirstUpgrade) && (byTag == null || !byTag.TemporaryGun))
			{
				num = ShopNGUIController._CurrentNumberOfUpgrades(str1, out flag, categoryName, true);
			}
		}
		string str2 = string.Concat((!ShopNGUIController.IsWeaponCategory(categoryName) ? str1 : ShopNGUIController._TagForId(str1) ?? string.Empty), "_icon", num);
		string str3 = string.Concat(str2, "_big");
		Texture texture = Resources.Load<Texture>(string.Concat("OfferIcons/", str3));
		if (texture == null)
		{
			texture = Resources.Load<Texture>(string.Concat("ShopIcons/", str2));
		}
		return texture;
	}

	private static string TryGunForCategoryWithMaxUnbought()
	{
		List<ShopNGUIController.CategoryNames> categoryNames = new List<ShopNGUIController.CategoryNames>()
		{
			ShopNGUIController.CategoryNames.PrimaryCategory,
			ShopNGUIController.CategoryNames.BackupCategory,
			ShopNGUIController.CategoryNames.MeleeCategory,
			ShopNGUIController.CategoryNames.SpecilCategory,
			ShopNGUIController.CategoryNames.SniperCategory,
			ShopNGUIController.CategoryNames.PremiumCategory
		};
		List<ShopNGUIController.CategoryNames> list = (
			from cat in categoryNames.Randomize<ShopNGUIController.CategoryNames>()
			orderby (
				from w in (IEnumerable<UnityEngine.Object>)WeaponManager.sharedManager.weaponsInGame
				select ((GameObject)w).GetComponent<WeaponSounds>() into ws
				where (ws.categoryNabor - 1 != (int)cat ? false : ws.tier == ExpController.OurTierForAnyPlace())
				where (!WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(ws.name).Tag) || !WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]) ? false : Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]], true) == 1)
				select ws).ToList<WeaponSounds>().Count
			select cat).ToList<ShopNGUIController.CategoryNames>();
		string tag = null;
		int num = 0;
		while (num < list.Count)
		{
			ShopNGUIController.CategoryNames item = list[num];
			List<WeaponSounds> weaponSounds = (
				from w in (IEnumerable<UnityEngine.Object>)WeaponManager.sharedManager.weaponsInGame
				select ((GameObject)w).GetComponent<WeaponSounds>() into ws
				where (ws.categoryNabor - 1 != (int)item ? false : ws.tier == ExpController.OurTierForAnyPlace())
				select ws).Where<WeaponSounds>((WeaponSounds ws) => {
				List<string> strs = WeaponUpgrades.ChainForTag(ItemDb.GetByPrefabName(ws.name).Tag);
				return (strs == null ? true : (strs.Count <= 0 ? false : strs[0] == ItemDb.GetByPrefabName(ws.name).Tag));
			}).Where<WeaponSounds>((WeaponSounds ws) => (!WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(ws.name).Tag) || !WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]) ? false : Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]], true) == 0)).Where<WeaponSounds>((WeaponSounds ws) => WeaponManager.tryGunsTable[item][ExpController.OurTierForAnyPlace()].Contains(ItemDb.GetByTag(ItemDb.GetByPrefabName(ws.name).Tag).PrefabName)).Where<WeaponSounds>((WeaponSounds ws) => (WeaponManager.sharedManager.IsAvailableTryGun(ItemDb.GetByPrefabName(ws.name).Tag) ? false : !WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(ItemDb.GetByPrefabName(ws.name).Tag))).Randomize<WeaponSounds>().ToList<WeaponSounds>();
			if (weaponSounds.Count<WeaponSounds>() != 0)
			{
				tag = ItemDb.GetByPrefabName(weaponSounds.First<WeaponSounds>().name).Tag;
				break;
			}
			else
			{
				num++;
			}
		}
		return tag;
	}

	public static void TryToBuy(GameObject mainPanel, ItemPrice price, Action onSuccess, Action onFailure = null, Func<bool> successAdditionalCond = null, Action onReturnFromBank = null, Action onEnterCoinsShopAction = null, Action onExitCoinsShopAction = null)
	{
		UnityEngine.Debug.Log("Trying to buy from ShopNGUIController...");
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("BankController.Instance == null");
			return;
		}
		if (price == null)
		{
			UnityEngine.Debug.LogWarning("price == null");
			return;
		}
		EventHandler instance = null;
		instance = (object sender, EventArgs e) => {
			BankController.Instance.BackRequested -= instance;
			BankController.Instance.InterfaceEnabled = false;
			coinsShop.thisScript.notEnoughCurrency = null;
			if (mainPanel != null)
			{
				mainPanel.SetActive(true);
			}
			if (onReturnFromBank != null)
			{
				onReturnFromBank();
			}
			if (onExitCoinsShopAction != null)
			{
				onExitCoinsShopAction();
			}
		};
		EventHandler eventHandler = null;
		eventHandler = (object sender, EventArgs e) => {
			BankController.Instance.BackRequested -= eventHandler;
			mainPanel.SetActive(true);
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int num = Storager.getInt(price.Currency, false);
			num -= price.Price;
			bool flag = num >= 0;
			flag = (successAdditionalCond == null ? flag : (successAdditionalCond() ? true : flag));
			if (!flag)
			{
				if (onFailure != null)
				{
					onFailure();
				}
				coinsShop.thisScript.notEnoughCurrency = price.Currency;
				UnityEngine.Debug.Log("Trying to display bank interface...");
				BankController.Instance.BackRequested += instance;
				BankController.Instance.InterfaceEnabled = true;
				mainPanel.SetActive(false);
				if (onEnterCoinsShopAction != null)
				{
					onEnterCoinsShopAction();
				}
			}
			else
			{
				Storager.setInt(price.Currency, num, false);
				ShopNGUIController.SpendBoughtCurrency(price.Currency, price.Price);
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
				if (FriendsController.useBuffSystem)
				{
					BuffSystem.instance.OnSomethingPurchased();
				}
				if (onSuccess != null)
				{
					onSuccess();
				}
			}
		};
		eventHandler(BankController.Instance, EventArgs.Empty);
	}

	[DebuggerHidden]
	private IEnumerator TryToShowExpiredBanner()
	{
		ShopNGUIController.u003cTryToShowExpiredBanneru003ec__Iterator1AB variable = null;
		return variable;
	}

	public static void UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames cat, bool inGameLocal)
	{
		bool flag = !Defs.isMulti;
		string str = Storager.getString(ShopNGUIController.SnForWearCategory(cat), false);
		Player_move_c component = null;
		if (inGameLocal)
		{
			if (flag)
			{
				if (!SceneLoader.ActiveSceneName.Equals("LevelComplete") && !SceneLoader.ActiveSceneName.Equals("ChooseLevel"))
				{
					component = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
				}
			}
			else if (WeaponManager.sharedManager.myPlayer != null)
			{
				component = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		Storager.setString(ShopNGUIController.SnForWearCategory(cat), ShopNGUIController.NoneEquippedForWearCategory(cat), false);
		FriendsController.sharedController.SendAccessories();
		ShopNGUIController.sharedShop.SetWearForCategory(cat, ShopNGUIController.NoneEquippedForWearCategory(cat));
		if (ShopNGUIController.sharedShop.wearEquipAction != null)
		{
			ShopNGUIController.sharedShop.wearEquipAction(cat, str ?? ShopNGUIController.NoneEquippedForWearCategory(cat), ShopNGUIController.NoneEquippedForWearCategory(cat));
		}
		if (cat == ShopNGUIController.CategoryNames.BootsCategory && inGameLocal && component != null && !str.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.bootsMethods.ContainsKey(str))
		{
			Wear.bootsMethods[str].Value(component, new Dictionary<string, object>());
		}
		if (cat == ShopNGUIController.CategoryNames.CapesCategory && inGameLocal && component != null && !str.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.capesMethods.ContainsKey(str))
		{
			Wear.capesMethods[str].Value(component, new Dictionary<string, object>());
		}
		if (cat == ShopNGUIController.CategoryNames.HatsCategory && inGameLocal && component != null && !str.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.hatsMethods.ContainsKey(str))
		{
			Wear.hatsMethods[str].Value(component, new Dictionary<string, object>());
		}
		if (cat == ShopNGUIController.CategoryNames.ArmorCategory && inGameLocal && component != null && !str.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.armorMethods.ContainsKey(str))
		{
			Wear.armorMethods[str].Value(component, new Dictionary<string, object>());
		}
		if (ShopNGUIController.sharedShop.wearUnequipAction != null)
		{
			ShopNGUIController.sharedShop.wearUnequipAction(cat, str ?? ShopNGUIController.NoneEquippedForWearCategory(cat));
		}
		if (ShopNGUIController.GuiActive)
		{
			ShopNGUIController.sharedShop.UpdateIcon(cat, false);
		}
	}

	private void Update()
	{
		string child;
		float single;
		if (!this.ActiveObject.activeInHierarchy)
		{
			return;
		}
		ExperienceController.sharedController.isShowRanks = (this.rentScreenPoint.childCount != 0 || !(SkinEditorController.sharedController == null) ? false : (BankController.Instance == null ? 0 : (int)BankController.Instance.InterfaceEnabled) == 0);
		if (Time.realtimeSinceStartup - this.timeToUpdateTempGunTime >= 1f)
		{
			this.timeToUpdateTempGunTime = Time.realtimeSinceStartup;
			if (ShopNGUIController.GuiActive && this.viewedId != null && WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(this.viewedId))
			{
				this.UpdateTryGunDiscountTime();
			}
		}
		if (this.hatPoint.transform.childCount <= 1)
		{
			child = (this.hatPoint.transform.childCount <= 0 ? "none" : this.hatPoint.transform.GetChild(0).gameObject.tag);
		}
		else
		{
			child = this.hatPoint.transform.GetChild(1).gameObject.tag;
		}
		string str = child;
		bool flag = (this.currentCategory != ShopNGUIController.CategoryNames.HatsCategory ? false : !Wear.NonArmorHat(str));
		bool flag1 = (this.currentCategory != ShopNGUIController.CategoryNames.ArmorCategory || this.viewedId == null ? false : !TempItemsController.PriceCoefs.ContainsKey(this.viewedId));
		if (this.showArmorButton.gameObject.activeSelf != flag1)
		{
			this.showArmorButton.gameObject.SetActive(flag1);
		}
		bool flag2 = false;
		if (this.showHatButton.gameObject.activeSelf != flag2)
		{
			this.showHatButton.gameObject.SetActive(flag2);
		}
		bool flag3 = (this.currentCategory != ShopNGUIController.CategoryNames.ArmorCategory || this.viewedId == null ? false : TempItemsController.PriceCoefs.ContainsKey(this.viewedId));
		if (this.showArmorButtonTempArmor.gameObject.activeSelf != flag3)
		{
			this.showArmorButtonTempArmor.gameObject.SetActive(flag3);
		}
		bool flag4 = (!flag || this.viewedId == null ? false : TempItemsController.PriceCoefs.ContainsKey(this.viewedId));
		if (this.showHatButtonTempHat.gameObject.activeSelf != flag4)
		{
			this.showHatButtonTempHat.gameObject.SetActive(flag4);
		}
		if (Time.realtimeSinceStartup - this._timePurchaseSuccessfulShown >= 2f)
		{
			this.purchaseSuccessful.SetActive(false);
		}
		if (Time.realtimeSinceStartup - this._timePurchaseRentSuccessfulShown >= 2f)
		{
			this.purchaseSuccessfulRent.SetActive(false);
		}
		if (this.mainPanel.activeInHierarchy && !HOTween.IsTweening(this.MainMenu_Pers))
		{
			float single1 = -120f;
			float single2 = single1;
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				single = (!Application.isEditor ? 0.5f : 100f);
			}
			else
			{
				single = 2f;
			}
			single1 = single2 * single;
			Rect rect = new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height);
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Moved && rect.Contains(touch.position))
				{
					this.idleTimerLastTime = Time.realtimeSinceStartup;
					Transform mainMenuPers = this.MainMenu_Pers;
					Vector3 vector3 = Vector3.up;
					Vector2 vector2 = touch.deltaPosition;
					mainMenuPers.Rotate(vector3, vector2.x * single1 * 0.5f * (Time.realtimeSinceStartup - this.lastTime));
				}
			}
			if (Application.isEditor)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel") * 3f * single1 * (Time.realtimeSinceStartup - this.lastTime);
				this.MainMenu_Pers.Rotate(Vector3.up, axis);
				if (axis != 0f)
				{
					this.idleTimerLastTime = Time.realtimeSinceStartup;
				}
			}
			this.lastTime = Time.realtimeSinceStartup;
		}
		if (this.currentCategory != ShopNGUIController.CategoryNames.CapesCategory && Time.realtimeSinceStartup - this.idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			this.SetCamera();
		}
		ActivityIndicator.IsActiveIndicator = StoreKitEventListener.restoreInProcess;
		this.CheckCenterItemChanging();
	}

	public void UpdateButtons()
	{
		bool flag;
		long num;
		bool flag1;
		bool flag2;
		bool flag3;
		bool flag4;
		bool flag5;
		int num1;
		bool flag6;
		bool flag7;
		bool flag8;
		bool flag9;
		string str;
		GameObject upgrade3;
		bool flag10;
		bool flag11;
		bool flag12;
		bool flag13;
		bool flag14 = false;
		bool flag15 = false;
		bool flag16 = false;
		bool flag17 = false;
		bool flag18 = false;
		bool flag19 = false;
		bool flag20 = false;
		bool flag21 = false;
		bool flag22 = false;
		bool flag23 = false;
		bool flag24 = false;
		bool flag25 = false;
		this.wholePrice2Gear.SetActive(false);
		bool flag26 = false;
		bool[] flagArray = new bool[2];
		bool flag27 = false;
		bool flag28 = false;
		bool flag29 = false;
		bool flag30 = (this.viewedId == null ? false : TempItemsController.PriceCoefs.ContainsKey(this.viewedId));
		this.rentProperties.SetActive((this.viewedId == null || !TempItemsController.IsCategoryContainsTempItems(this.currentCategory) ? false : TempItemsController.PriceCoefs.ContainsKey(this.viewedId)));
		this.prolongateRentText.SetActive((this.viewedId == null || !TempItemsController.PriceCoefs.ContainsKey(this.viewedId) || !(TempItemsController.sharedController != null) ? false : TempItemsController.sharedController.ContainsItem(this.viewedId)));
		bool flag31 = false;
		this.upgrade.isEnabled = true;
		this.upgradeGear.isEnabled = true;
		if (!this.WeaponCategory)
		{
			if (this.weaponProperties.activeSelf)
			{
				this.weaponProperties.SetActive(false);
			}
			if (this.meleeProperties.activeSelf)
			{
				this.meleeProperties.SetActive(false);
			}
			if (this.upgradesAnchor.activeSelf)
			{
				this.upgradesAnchor.SetActive(false);
			}
			if (this.SpecialParams.activeSelf)
			{
				this.SpecialParams.SetActive(false);
			}
			switch (this.currentCategory)
			{
				case ShopNGUIController.CategoryNames.HatsCategory:
				case ShopNGUIController.CategoryNames.ArmorCategory:
				case ShopNGUIController.CategoryNames.CapesCategory:
				case ShopNGUIController.CategoryNames.BootsCategory:
				case ShopNGUIController.CategoryNames.MaskCategory:
				{
					string str1 = WeaponManager.LastBoughtTag(this.viewedId);
					bool flag32 = str1 != null;
					bool flag33 = (!flag32 ? false : this._CurrentEquippedWear.Equals(str1));
					string str2 = WeaponManager.FirstUnboughtTag(this.viewedId);
					this.SetUpUpgradesAndTiers(flag32, ref flag22, ref flag26, ref flag27, ref flag29, ref flag23, ref flag31);
					if (!(this.viewedId != "Armor_Novice") || !flag32 || flag33 || str1 == null || !str1.Equals(this.viewedId) || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted && this.trainingState < ShopNGUIController.TrainingState.OnArmor)
					{
						flag3 = false;
					}
					else
					{
						flag3 = (!this.InTrainingAfterNoviceArmorRemoved ? true : this.viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
					}
					bool flag34 = flag3;
					flag21 = (!(this.viewedId != "Armor_Novice") || !flag32 || !flag33 || str1 == null || !str1.Equals(this.viewedId) || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
					if (this.viewedId == "Armor_Novice")
					{
						flag4 = true;
					}
					else
					{
						flag4 = (str1 == null || !this._CurrentEquippedWear.Equals(str1) || flag26 ? false : !flag30);
					}
					bool flag35 = flag4;
					if (!flag32 && this.viewedId != null && this.viewedId.Equals("cape_Custom"))
					{
						flag16 = (this.inGame || Defs.isDaterRegim || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
					}
					if (!this.inGame && flag32 && this.viewedId != null && this.viewedId.Equals("cape_Custom"))
					{
						flag17 = (Defs.isDaterRegim || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
					}
					if (flag26 || flag30)
					{
						if (!flag34)
						{
							flag5 = false;
						}
						else if (flag30)
						{
							flag5 = true;
						}
						else
						{
							flag5 = (this.viewedId == null || str2 == null ? false : (str1 == null || str1.Equals(str2) ? 0 : (int)str2.Equals(this.viewedId)) == 0);
						}
						flag19 = flag5;
						flag20 = false;
					}
					else
					{
						flag19 = false;
						flag20 = (!flag34 || this.viewedId == null || str2 == null ? false : (str1 == null || str1.Equals(str2) ? 0 : (int)str2.Equals(this.viewedId)) == 0);
					}
					if (!(this.viewedId == "cape_Custom") || !flag32)
					{
						bool[] flagArray1 = flagArray;
						num1 = (!flag26 ? 1 : 0);
						if (!flag35)
						{
							flag6 = false;
						}
						else if (flag30)
						{
							flag6 = true;
						}
						else
						{
							flag6 = (this.viewedId == null || str2 == null ? false : (str1 == null || str1.Equals(str2) ? 0 : (int)str2.Equals(this.viewedId)) == 0);
						}
						flagArray1[num1] = flag6;
						flagArray[(!flag26 ? 0 : 1)] = false;
					}
					else
					{
						flagArray[1] = flag35;
						flagArray[0] = false;
					}
					try
					{
						if (this.currentCategory != ShopNGUIController.CategoryNames.ArmorCategory)
						{
							this.nonArmorWearDEscription.text = LocalizationStore.Get(Wear.descriptionLocalizationKeys[this.viewedId]);
						}
						else
						{
							UILabel uILabel = this.armorCountLabel;
							float item = Wear.armorNum[this.viewedId];
							uILabel.text = item.ToString();
							this.armorCountLabel.gameObject.SetActive(this.viewedId != "Armor_Novice");
							this.armorWearDescription.text = LocalizationStore.Get("Key_0354");
						}
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(string.Concat("Exception in setting desciption for wear: ", exception));
					}
					this.UpdateTempItemTime();
					break;
				}
				case ShopNGUIController.CategoryNames.SkinsCategory:
				{
					flag16 = false;
					bool flag36 = false;
					bool flag37 = false;
					bool num2 = false;
					bool flag38 = this.viewedId == "61";
					bool flag39 = false;
					if (this.viewedId.Equals("CustomSkinID"))
					{
						num2 = Storager.getInt(Defs.SkinsMakerInProfileBought, true) > 0;
						flag16 = (this.inGame || num2 || Defs.isDaterRegim || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
						flag17 = false;
						flag28 = (this.inGame || !num2 || Defs.isDaterRegim || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
						int num3 = ShopNGUIController.DiscountFor(this.viewedId, out flag2);
						if (num2 || flag29 || num3 <= 0 || this.viewedId != null && this.viewedId.Equals("CustomSkinID") && this.inGame || this.inGame || Defs.isDaterRegim)
						{
							flag27 = false;
						}
						else
						{
							flag27 = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted ? !this.InTrainingAfterNoviceArmorRemoved : false);
							this.salePerc.text = string.Concat(num3, "%");
						}
					}
					else
					{
						bool flag40 = false;
						flag39 = SkinsController.IsSkinBought(this.viewedId, out flag40);
						flag22 = (!flag40 || flag39 || flag38 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
						flag26 = false;
						flag37 = ((!flag40 || flag39) && !this.viewedId.Equals(SkinsController.currentSkinNameForPers) && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted) ? !this.InTrainingAfterNoviceArmorRemoved : false);
						flag21 = false;
						flag36 = (!flag40 || flag39 ? this.viewedId.Equals(SkinsController.currentSkinNameForPers) : false);
						flag25 = (!flag38 || flag39 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
						bool flag41 = false;
						flag41 = (!long.TryParse(this.viewedId, out num) ? false : num >= (long)1000000);
						flag17 = (this.inGame || !flag41 || Defs.isDaterRegim || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
						flag18 = (!flag41 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
						int num4 = ShopNGUIController.DiscountFor(this.viewedId, out flag1);
						if (flag39 || flag38 || flag29 || num4 <= 0)
						{
							flag27 = false;
						}
						else
						{
							flag27 = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted ? !this.InTrainingAfterNoviceArmorRemoved : false);
							this.salePerc.text = string.Concat(num4, "%");
						}
					}
					flag19 = false;
					flag20 = (!flag37 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
					flagArray[0] = false;
					flagArray[1] = (!flag36 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
					IEnumerator enumerator = this.skinProperties.transform.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Transform current = (Transform)enumerator.Current;
							if (this.viewedId != null && this.viewedId.Equals("CustomSkinID"))
							{
								if (num2)
								{
									current.gameObject.SetActive(current.gameObject.name.Equals("Custom1_Skin"));
								}
								else
								{
									current.gameObject.SetActive(current.gameObject.name.Equals("Custom_Skin"));
								}
							}
							else if (!flag38 || flag39)
							{
								current.gameObject.SetActive(current.gameObject.name.Equals("Usual_Skin"));
							}
							else
							{
								current.gameObject.SetActive(current.gameObject.name.Equals("Facebook_Skin"));
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable == null)
						{
						}
						disposable.Dispose();
					}
					break;
				}
			}
		}
		else
		{
			WeaponSounds weaponSound = null;
			WeaponSounds weaponSound1 = null;
			string str3 = null;
			string str4 = null;
			if (this.viewedId != null)
			{
				str3 = WeaponManager.FirstUnboughtTag(this.viewedId) ?? this.viewedId;
				string str5 = WeaponManager.FirstTagForOurTier(this.viewedId);
				List<string> strs = WeaponUpgrades.ChainForTag(this.viewedId);
				if (str5 != null && strs != null && strs.IndexOf(str5) > strs.IndexOf(str3))
				{
					str3 = str5;
				}
				str4 = WeaponManager.LastBoughtTag(this.viewedId);
				foreach (WeaponSounds weaponSound2 in WeaponManager.AllWrapperPrefabs())
				{
					if (!ItemDb.GetByPrefabName(weaponSound2.name).Tag.Equals(str4 ?? this.viewedId))
					{
						continue;
					}
					weaponSound = weaponSound2;
					foreach (WeaponSounds weaponSound3 in WeaponManager.AllWrapperPrefabs())
					{
						if (!ItemDb.GetByPrefabName(weaponSound3.name).Tag.Equals(str3))
						{
							continue;
						}
						weaponSound1 = weaponSound3;
						break;
					}
					break;
				}
			}
			bool flag42 = false;
			bool flag43 = false;
			int num5 = (this.viewedId == null ? -1 : ShopNGUIController._CurrentNumberOfUpgrades(this.viewedId, out flag43, this.currentCategory, true));
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId))
			{
				num5 = 0;
			}
			flag42 = (!flag43 ? false : (WeaponManager.sharedManager == null ? 0 : (int)WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId)) == 0);
			flag22 = (this.viewedId == null || flag42 || num5 != 0 || flag30 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
			flag26 = (this.viewedId == null || flag42 || num5 == 0 || weaponSound1.tier >= 100 || flag30 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
			flag23 = false;
			if (WeaponManager.sharedManager != null && this.viewedId != null)
			{
				flag14 = WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId);
				if (flag14)
				{
					try
					{
						UILabel uILabel1 = this.tryGunMatchesCount;
						SaltedInt saltedInt = (SaltedInt)WeaponManager.sharedManager.TryGuns[this.viewedId]["NumberOfMatchesKey"];
						uILabel1.text = saltedInt.Value.ToString();
					}
					catch (Exception exception1)
					{
						UnityEngine.Debug.LogError(string.Concat("Exception in tryGunMatchesCount.text: ", exception1));
					}
				}
				flag15 = WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(this.viewedId);
				if (flag15)
				{
					this.UpdateTryGunDiscountTime();
				}
			}
			flag29 = (!flag26 || !(weaponSound1 != null) || !(ExpController.Instance != null) || ExpController.Instance.OurTier >= weaponSound1.tier || flag30 || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted ? false : !this.InTrainingAfterNoviceArmorRemoved);
			if (weaponSound1 != null && this.viewedId == ItemDb.GetByPrefabName(weaponSound.name).Tag)
			{
				flag29 = false;
			}
			if (flag29)
			{
				int num6 = (weaponSound1.tier < 0 || weaponSound1.tier >= (int)ExpController.LevelsForTiers.Length ? ExpController.LevelsForTiers[(int)ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[weaponSound1.tier]);
				string str6 = string.Format("{0} {1} {2}", LocalizationStore.Key_0226, num6, LocalizationStore.Get("Key_1022"));
				this.needTierLabel.text = str6;
			}
			this.upgrade.isEnabled = (!flag26 || !(weaponSound1 != null) || !(ExpController.Instance != null) ? 0 : (int)(ExpController.Instance.OurTier < weaponSound1.tier)) == 0;
			string str7 = null;
			if (this.viewedId != null)
			{
				str7 = WeaponManager.LastBoughtTag(this.viewedId);
			}
			if (str7 == null && this.viewedId != null && WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId))
			{
				str7 = this.viewedId;
			}
			if (str7 == null || this.viewedId == null)
			{
				flag7 = true;
			}
			else
			{
				flag7 = (this.chosenId == null ? false : this.chosenId.Equals(str7));
			}
			if (flag7 || this.viewedId == null || num5 <= 0 && !WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId) || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted && !(this.viewedId == WeaponTags.HunterRifleTag))
			{
				flag8 = false;
			}
			else
			{
				flag8 = (!this.InTrainingAfterNoviceArmorRemoved ? true : this.viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
			}
			bool flag44 = flag8;
			if (!string.IsNullOrEmpty(this.viewedId))
			{
				if (ShopNGUIController._CurrentWeaponSetIDs()[(int)this.currentCategory] != null)
				{
					if (ShopNGUIController._CurrentWeaponSetIDs()[(int)this.currentCategory].Equals(WeaponManager.LastBoughtTag(this.viewedId) ?? string.Empty))
					{
						goto Label1;
					}
				}
				if (!WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId) || !ShopNGUIController._CurrentWeaponSetIDs()[(int)this.currentCategory].Equals(this.viewedId))
				{
					goto Label2;
				}
			Label1:
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted && !(this.viewedId == WeaponTags.HunterRifleTag))
				{
					goto Label2;
				}
				flag9 = (!this.InTrainingAfterNoviceArmorRemoved ? true : this.viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
				goto Label0;
			}
		Label2:
			flag9 = false;
		Label0:
			bool flag45 = flag9;
			if ((flag26 || flag30 || WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId)) && this.viewedId != null && str3 != null && (flag30 || !str3.Equals(this.viewedId) || WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId)) && flag44)
			{
				if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || this.viewedId == WeaponTags.HunterRifleTag)
				{
					flag13 = (!this.InTrainingAfterNoviceArmorRemoved ? true : this.viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
				}
				else
				{
					flag13 = false;
				}
				flag19 = flag13;
				flag20 = false;
			}
			if ((flag26 || flag30 || WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId)) && this.viewedId != null && str3 != null && (flag30 || !str3.Equals(this.viewedId) || WeaponManager.sharedManager.IsAvailableTryGun(this.viewedId)) && flag45)
			{
				bool[] flagArray2 = flagArray;
				if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || this.viewedId == WeaponTags.HunterRifleTag)
				{
					flag12 = (!this.InTrainingAfterNoviceArmorRemoved ? true : this.viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
				}
				else
				{
					flag12 = false;
				}
				flagArray2[0] = flag12;
				flagArray[1] = false;
			}
			if (!flag26 && !flag30 && !flag22 && flag44)
			{
				flag19 = false;
				if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || this.viewedId == WeaponTags.HunterRifleTag)
				{
					flag11 = (!this.InTrainingAfterNoviceArmorRemoved ? true : this.viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
				}
				else
				{
					flag11 = false;
				}
				flag20 = flag11;
			}
			if (!flag26 && !flag30 && !flag22 && flag45)
			{
				flagArray[0] = false;
				bool[] flagArray3 = flagArray;
				if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted || this.viewedId == WeaponTags.HunterRifleTag)
				{
					flag10 = (!this.InTrainingAfterNoviceArmorRemoved ? true : this.viewedId == WeaponManager.LastBoughtTag("Armor_Army_1"));
				}
				else
				{
					flag10 = false;
				}
				flagArray3[1] = flag10;
			}
			int num7 = ShopNGUIController.DiscountFor(str3, out flag);
			if (flag42 || flag30 || !(weaponSound != null) || weaponSound.tier >= 100 || flag29 || num7 <= 0)
			{
				flag27 = false;
			}
			else
			{
				flag27 = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted ? !this.InTrainingAfterNoviceArmorRemoved : false);
				this.salePerc.text = string.Concat(num7, "%");
			}
			flag31 = false;
			if (str3 != null)
			{
				int count = 1;
				int num8 = 0;
				if (count == 1 && str4 != null && str3.Equals(str4))
				{
					num8 = 1;
				}
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (!upgrade.Contains(str3))
					{
						continue;
					}
					count = upgrade.Count;
					if (str4 == null)
					{
						string str8 = WeaponManager.FirstTagForOurTier(str3);
						if (str8 != null && upgrade.IndexOf(str8) > 0)
						{
							num8 = upgrade.IndexOf(str8);
						}
					}
					else
					{
						num8 = upgrade.IndexOf(str4) + 1;
					}
					break;
				}
				bool flag46 = (!weaponSound.isMelee || weaponSound.isShotMelee ? this.viewedId != null : false);
				if (this.weaponProperties.activeSelf != flag46)
				{
					this.weaponProperties.SetActive(flag46);
				}
				bool flag47 = (!weaponSound.isMelee || weaponSound.isShotMelee ? false : this.viewedId != null);
				if (this.meleeProperties.activeSelf != flag47)
				{
					this.meleeProperties.SetActive(flag47);
				}
				if (this.upgradesAnchor.activeSelf != (this.viewedId == null ? false : !flag30))
				{
					this.upgradesAnchor.SetActive((this.viewedId == null ? false : !flag30));
				}
				bool flag48 = (count != 1 ? false : !flag30);
				if (this.upgrade_1.activeSelf != flag48)
				{
					this.upgrade_1.SetActive(flag48);
				}
				bool flag49 = (count != 2 ? false : !flag30);
				if (this.upgrade_2.activeSelf != flag49)
				{
					this.upgrade_2.SetActive(flag49);
				}
				bool flag50 = (count != 3 ? false : !flag30);
				if (this.upgrade_3.activeSelf != flag50)
				{
					this.upgrade_3.SetActive(flag50);
				}
				if (count > 0)
				{
					if (count != 3)
					{
						upgrade3 = (count != 2 ? this.upgrade_1 : this.upgrade_2);
					}
					else
					{
						upgrade3 = this.upgrade_3;
					}
					GameObject[] color = this.upgradeSprites1;
					if (count == 3)
					{
						color = this.upgradeSprites3;
					}
					if (count == 2)
					{
						color = this.upgradeSprites2;
					}
					if (color == null)
					{
						color = new GameObject[0];
					}
					Array.Sort<GameObject>(color, (GameObject sp1, GameObject sp2) => Defs.CompareAlphaNumerically(sp1.gameObject.name, sp2.gameObject.name));
					for (int i = 0; i < (int)color.Length; i++)
					{
						color[i].SetActive((num8 < i ? false : true));
						if (num8 != i)
						{
							color[i].GetComponent<TweenColor>().enabled = false;
							color[i].GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 1f);
						}
						else
						{
							color[i].GetComponent<TweenColor>().enabled = true;
						}
					}
				}
				this.UpdateTempItemTime();
				if (weaponSound1 == null)
				{
					weaponSound1 = weaponSound;
				}
				int[] numArray = null;
				numArray = (!weaponSound.isMelee || weaponSound.isShotMelee ? new int[] { (!flag30 ? weaponSound.damageShop : (int)weaponSound.DPS), weaponSound.fireRateShop, weaponSound.CapacityShop, weaponSound.mobilityShop } : new int[] { (!flag30 ? weaponSound.damageShop : (int)weaponSound.DPS), weaponSound.fireRateShop, weaponSound.mobilityShop });
				int[] numArray1 = null;
				numArray1 = (!weaponSound.isMelee || weaponSound.isShotMelee ? new int[] { (!flag30 ? weaponSound1.damageShop : (int)weaponSound1.DPS), weaponSound1.fireRateShop, weaponSound1.CapacityShop, weaponSound1.mobilityShop } : new int[] { (!flag30 ? weaponSound1.damageShop : (int)weaponSound1.DPS), weaponSound1.fireRateShop, weaponSound1.mobilityShop });
				bool flag51 = (str4 == null || str3 == null || !(str4 != str3) || this.viewedId == null ? false : str4 == this.viewedId);
				int[] numArray2 = (!flag51 ? numArray1 : numArray);
				if (!weaponSound.isMelee || weaponSound.isShotMelee)
				{
					this.damage.text = ShopNGUIController.GetWeaponStatText(numArray[0], numArray2[0]);
					this.fireRate.text = ShopNGUIController.GetWeaponStatText(numArray[1], numArray2[1]);
					this.capacity.text = ShopNGUIController.GetWeaponStatText(numArray[2], numArray2[2]);
					this.mobility.text = ShopNGUIController.GetWeaponStatText(numArray[3], numArray2[3]);
				}
				else
				{
					this.damageMelee.text = ShopNGUIController.GetWeaponStatText(numArray[0], numArray2[0]);
					this.fireRateMElee.text = ShopNGUIController.GetWeaponStatText(numArray[1], numArray2[1]);
					this.mobilityMelee.text = ShopNGUIController.GetWeaponStatText(numArray[2], numArray2[2]);
				}
				if (!this.SpecialParams.activeSelf)
				{
					this.SpecialParams.SetActive(true);
				}
				if (weaponSound1 == null)
				{
					weaponSound1 = weaponSound;
				}
				WeaponSounds weaponSound4 = (!flag51 ? weaponSound1 : weaponSound);
				if (weaponSound4 != null)
				{
					if (!FriendsController.SandboxEnabled && weaponSound4.InShopEffects.Contains(WeaponSounds.Effects.ForSandbox))
					{
						weaponSound4.InShopEffects.Remove(WeaponSounds.Effects.ForSandbox);
					}
					float single = 90f / (float)weaponSound4.InShopEffects.Count;
					for (int j = 0; j < this.effectsLabels.Count; j++)
					{
						if (this.effectsLabels[j].gameObject.activeSelf != j < weaponSound4.InShopEffects.Count)
						{
							this.effectsLabels[j].gameObject.SetActive(j < weaponSound4.InShopEffects.Count);
						}
						if (j < weaponSound4.InShopEffects.Count)
						{
							Transform vector3 = this.effectsLabels[j].transform;
							float single1 = 0f;
							if (weaponSound4.InShopEffects.Count == 3)
							{
								if (j == 0)
								{
									single1 = 1f;
								}
								if (j == 2)
								{
									single1 = -3f;
								}
							}
							else if (weaponSound4.InShopEffects.Count == 2)
							{
								if (j == 0)
								{
									single1 = -6f;
								}
								if (j == 1)
								{
									single1 = 6f;
								}
							}
							float single2 = vector3.localPosition.x;
							Vector3 vector31 = vector3.localPosition;
							vector3.localPosition = new Vector3(single2, 39f - single * ((float)j + 0.5f) + single1, vector31.z);
							UILabel item1 = this.effectsLabels[j];
							str = (weaponSound4.InShopEffects[j] != WeaponSounds.Effects.Zoom ? string.Empty : string.Concat(weaponSound4.zoomShop.ToString(), "X "));
							KeyValuePair<string, string> keyValuePair = WeaponSounds.keysAndSpritesForEffects[weaponSound4.InShopEffects[j]];
							item1.text = string.Concat(str, LocalizationStore.Get(keyValuePair.Value));
							UISprite key = this.effectsSprites[j];
							KeyValuePair<string, string> keyValuePair1 = WeaponSounds.keysAndSpritesForEffects[weaponSound4.InShopEffects[j]];
							key.spriteName = keyValuePair1.Key;
						}
					}
				}
			}
		}
		if (this.tryGunPanel != null && this.tryGunPanel.activeSelf != flag14)
		{
			this.tryGunPanel.SetActive(flag14);
		}
		if (this.tryGunDiscountPanel != null && this.tryGunDiscountPanel.activeSelf != flag15)
		{
			this.tryGunDiscountPanel.SetActive(flag15);
		}
		if (this.edit != null && this.edit.gameObject.activeSelf != flag17)
		{
			this.edit.gameObject.SetActive(flag17);
		}
		if (this.enable != null && this.enable.gameObject.activeSelf != flag16)
		{
			this.enable.gameObject.SetActive(flag16);
		}
		if (this.delete.gameObject.activeSelf != flag18)
		{
			this.delete.gameObject.SetActive(flag18);
		}
		if (this.buy.gameObject.activeSelf != flag22)
		{
			this.buy.gameObject.SetActive(flag22);
		}
		if (this.rent.gameObject.activeSelf != flag23)
		{
			this.rent.gameObject.SetActive(flag23);
		}
		if (this.equips[0].gameObject.activeSelf != flag19)
		{
			this.equips[0].gameObject.SetActive(flag19);
		}
		if (this.equips[1].gameObject.activeSelf != flag20)
		{
			this.equips[1].gameObject.SetActive(flag20);
		}
		if (this.unequip.gameObject.activeSelf != flag21)
		{
			this.unequip.gameObject.SetActive(flag21);
		}
		if (this.buyGear.gameObject.activeSelf != flag24)
		{
			this.buyGear.gameObject.SetActive(flag24);
		}
		if (this.upgrade.gameObject.activeSelf != flag26)
		{
			this.upgrade.gameObject.SetActive(flag26);
		}
		if (this.facebookLoginLockedSkinButton.gameObject.activeSelf != flag25)
		{
			this.facebookLoginLockedSkinButton.gameObject.SetActive(flag25);
		}
		if (this.equippeds[0].gameObject.activeSelf != flagArray[0])
		{
			this.equippeds[0].gameObject.SetActive(flagArray[0]);
		}
		if (this.equippeds[1].gameObject.activeSelf != flagArray[1])
		{
			this.equippeds[1].gameObject.SetActive(flagArray[1]);
		}
		if (this.sale.gameObject.activeSelf != flag27)
		{
			this.sale.gameObject.SetActive(flag27);
		}
		if (this.saleRent.gameObject.activeSelf != flag31)
		{
			this.saleRent.gameObject.SetActive(flag31);
		}
		if (this.create.gameObject.activeSelf != flag28)
		{
			this.create.gameObject.SetActive(flag28);
		}
		if (this.needTier.gameObject.activeSelf != flag29)
		{
			this.needTier.gameObject.SetActive(flag29);
		}
	}

	public void UpdateIcon(ShopNGUIController.CategoryNames c, bool animateModel = false)
	{
		TweenDelegate.TweenCallback tweenCallback = null;
		TweenDelegate.TweenCallback tweenCallback1 = null;
		ToggleButton toggleButton = this.category.buttons[(int)c];
		List<GameObject> gameObjects = new List<GameObject>();
		Player_move_c.PerformActionRecurs(toggleButton.gameObject, (Transform ch) => {
			if (ch.gameObject == toggleButton.gameObject || ch.gameObject == toggleButton.onButton.gameObject || ch.gameObject == toggleButton.offButton.gameObject || ch.gameObject.name.Equals("Label") || ch.gameObject.name.Equals("ShopIcon"))
			{
				return;
			}
			if (ch.gameObject.name.Equals("Sprite"))
			{
				ch.gameObject.SetActive(false);
				return;
			}
			gameObjects.Add(ch.gameObject);
		});
		foreach (GameObject gameObject in gameObjects)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		if (c == ShopNGUIController.CategoryNames.SkinsCategory || c == ShopNGUIController.CategoryNames.CapesCategory && this._currentCape.Equals("cape_Custom"))
		{
			List<GameObject> gameObjects1 = this.FillModelsList(c);
			GameObject gameObject1 = (c != ShopNGUIController.CategoryNames.SkinsCategory ? ItemDb.GetWearFromResources("cape_Custom", ShopNGUIController.CategoryNames.CapesCategory) : gameObjects1[0]);
			if (gameObject1 == null)
			{
				Player_move_c.PerformActionRecurs(toggleButton.gameObject, (Transform ch) => {
					if (!ch.gameObject.name.Equals("Sprite"))
					{
						return;
					}
					ch.gameObject.SetActive(true);
				});
			}
			else
			{
				ShopNGUIController.AddModel(gameObject1, (GameObject manipulateObject, Vector3 positionShop, Vector3 rotationShop, string readableName, float sc, int _unusedTier, int _unusedLeague) => {
					manipulateObject.transform.parent = toggleButton.transform;
					if (c == ShopNGUIController.CategoryNames.SkinsCategory)
					{
						Player_move_c.SetTextureRecursivelyFrom(manipulateObject, SkinsController.currentSkinForPers, new GameObject[0]);
					}
					float single = 0.5f;
					Transform vector3 = manipulateObject.transform;
					vector3.localPosition = toggleButton.onButton.transform.localPosition + (positionShop * single);
					vector3.localPosition = new Vector3(vector3.localPosition.x, vector3.localPosition.y, 0f);
					vector3.Rotate(rotationShop, Space.World);
					vector3.localScale = new Vector3(sc * single, sc * single, sc * single);
					if (c == ShopNGUIController.CategoryNames.CapesCategory && this._currentCape.Equals("cape_Custom") && SkinsController.capeUserTexture != null)
					{
						Player_move_c.SetTextureRecursivelyFrom(manipulateObject, SkinsController.capeUserTexture, new GameObject[0]);
					}
					this.SetIconModelsPositions(vector3, c);
					if (animateModel)
					{
						Vector3 vector31 = vector3.localScale;
						vector3.localScale = vector3.localScale * 1.25f;
						Transform transforms = vector3;
						TweenParms tweenParm = (new TweenParms()).Prop("localScale", vector31).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear);
						if (tweenCallback == null)
						{
							tweenCallback = () => {
							};
						}
						HOTween.To(transforms, 0.25f, tweenParm.OnComplete(tweenCallback));
					}
				}, c, false, null);
			}
			Player_move_c.PerformActionRecurs(toggleButton.gameObject, (Transform ch) => {
				if (!ch.gameObject.name.Equals("ShopIcon"))
				{
					return;
				}
				ch.GetComponent<UITexture>().mainTexture = null;
			});
		}
		else
		{
			Texture texture = ShopNGUIController.TextureForCat((int)c);
			if (texture == null)
			{
				Player_move_c.PerformActionRecurs(toggleButton.gameObject, (Transform ch) => {
					if (ch.gameObject.name.Equals("Sprite"))
					{
						ch.gameObject.SetActive(true);
						return;
					}
					if (!ch.gameObject.name.Equals("ShopIcon"))
					{
						return;
					}
					ch.GetComponent<UITexture>().mainTexture = null;
				});
			}
			else
			{
				Player_move_c.PerformActionRecurs(toggleButton.gameObject, (Transform ch) => {
					if (ch.gameObject.name.Equals("Sprite"))
					{
						ch.gameObject.SetActive(false);
						return;
					}
					if (!ch.gameObject.name.Equals("ShopIcon"))
					{
						return;
					}
					UITexture component = ch.GetComponent<UITexture>();
					if (component.mainTexture == null || !component.mainTexture.name.Equals(texture.name) || HOTween.IsTweening(ch))
					{
						HOTween.Kill(ch);
						ch.localScale = new Vector3(1.25f, 1.25f, 1.25f);
						Transform transforms = ch;
						TweenParms tweenParm = (new TweenParms()).Prop("localScale", new Vector3(1f, 1f, 1f)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear);
						if (tweenCallback1 == null)
						{
							tweenCallback1 = () => {
							};
						}
						HOTween.To(transforms, 0.25f, tweenParm.OnComplete(tweenCallback1));
					}
					component.mainTexture = texture;
				});
			}
		}
	}

	public void UpdateIcons()
	{
		for (int i = 0; i < (int)this.category.buttons.Length; i++)
		{
			this.UpdateIcon((ShopNGUIController.CategoryNames)i, false);
		}
	}

	public void UpdateItemParameters()
	{
		bool flag;
		this.wholePrice.gameObject.SetActive((this.buy.gameObject.activeInHierarchy || this.upgrade.gameObject.activeInHierarchy ? true : this.enable.gameObject.activeInHierarchy));
		this.wholePriceUpgradeGear.gameObject.SetActive(false);
		this.wholePrice2Gear.gameObject.SetActive(this.buyGear.gameObject.activeInHierarchy);
		bool flag1 = (this.viewedId == null ? false : ShopNGUIController.DiscountFor(WeaponManager.FirstUnboughtOrForOurTier(this.viewedId), out flag) > 0);
		this.wholePriceBG.gameObject.SetActive(!flag1);
		this.wholePriceBG_Discount.gameObject.SetActive(flag1);
		bool flag2 = (this.viewedId == null ? false : ShopNGUIController.DiscountFor(this.viewedId, out flag) > 0);
		this.wholePriceBG2Gear.gameObject.SetActive(!flag2);
		this.wholePriceBG2Gear_Discount.gameObject.SetActive(flag2);
		this.wholePriceUpgradeBG2Gear.gameObject.SetActive(!flag2);
		this.wholePriceUpgradeBG2Gear_Discount.gameObject.SetActive(flag2);
		if (this.viewedId != null)
		{
			ItemPrice itemPrice = ShopNGUIController.currentPrice(this.viewedId, this.currentCategory, this.currentCategory == ShopNGUIController.CategoryNames.GearCategory, true);
			if (this.currentCategory != ShopNGUIController.CategoryNames.GearCategory)
			{
				this.price.text = itemPrice.Price.ToString();
				this.currencyImagePrice.spriteName = (!itemPrice.Currency.Equals("Coins") ? "gem_znachek" : "ingame_coin");
				this.currencyImagePrice.width = (!itemPrice.Currency.Equals("Coins") ? 34 : 30);
				this.currencyImagePrice.height = (!itemPrice.Currency.Equals("Coins") ? 24 : 30);
			}
			else
			{
				this.priceUpgradeGear.text = itemPrice.Price.ToString();
				this.currencyImagePriceUpgradeGear.spriteName = (!itemPrice.Currency.Equals("Coins") ? "gem_znachek" : "ingame_coin");
				this.currencyImagePriceUpgradeGear.width = (!itemPrice.Currency.Equals("Coins") ? 34 : 30);
				this.currencyImagePriceUpgradeGear.height = (!itemPrice.Currency.Equals("Coins") ? 24 : 30);
			}
		}
	}

	public void UpdatePersArmor(string armor)
	{
		if (this.armorPoint.childCount > 0)
		{
			Transform child = this.armorPoint.GetChild(0);
			ArmorRefs component = child.GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					component.leftBone.parent = child.GetChild(0);
				}
				if (component.rightBone != null)
				{
					component.rightBone.parent = child.GetChild(0);
				}
				child.parent = null;
				child.position = new Vector3(0f, -10000f, 0f);
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		if (armor.Equals(Defs.ArmorNewNoneEqupped))
		{
			return;
		}
		string str = Storager.getString(Defs.VisualArmor, false);
		if (!string.IsNullOrEmpty(str) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(str))
		{
			armor = str;
		}
		if (this.weapon != null)
		{
			GameObject gameObject = Resources.Load(string.Concat("Armor_Shop/", armor)) as GameObject;
			if (gameObject == null)
			{
				return;
			}
			GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			ShopNGUIController.DisableLightProbesRecursively(vector3);
			ArmorRefs leftArmorHand = vector3.transform.GetChild(0).GetComponent<ArmorRefs>();
			if (leftArmorHand != null)
			{
				WeaponSounds weaponSound = this.weapon.GetComponent<WeaponSounds>();
				vector3.transform.parent = this.armorPoint.transform;
				vector3.transform.localPosition = Vector3.zero;
				vector3.transform.localRotation = Quaternion.identity;
				vector3.transform.localScale = new Vector3(1f, 1f, 1f);
				Player_move_c.SetLayerRecursively(vector3, LayerMask.NameToLayer("NGUIShop"));
				if (leftArmorHand.leftBone != null && weaponSound.LeftArmorHand != null)
				{
					leftArmorHand.leftBone.parent = weaponSound.LeftArmorHand;
					leftArmorHand.leftBone.localPosition = Vector3.zero;
					leftArmorHand.leftBone.localRotation = Quaternion.identity;
					leftArmorHand.leftBone.localScale = new Vector3(1f, 1f, 1f);
				}
				if (leftArmorHand.rightBone != null && weaponSound.RightArmorHand != null)
				{
					leftArmorHand.rightBone.parent = weaponSound.RightArmorHand;
					leftArmorHand.rightBone.localPosition = Vector3.zero;
					leftArmorHand.rightBone.localRotation = Quaternion.identity;
					leftArmorHand.rightBone.localScale = new Vector3(1f, 1f, 1f);
				}
			}
			ShopNGUIController.SetPersArmorVisible(this.armorPoint);
		}
	}

	public void UpdatePersBoots(string bs)
	{
		IEnumerator enumerator = this.bootsPoint.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (!current.gameObject.name.Equals(bs))
				{
					current.gameObject.SetActive(false);
				}
				else
				{
					current.gameObject.SetActive(true);
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public void UpdatePersCape(string cape)
	{
		for (int i = 0; i < this.capePoint.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.capePoint.transform.GetChild(i).gameObject);
		}
		if (cape.Equals(Defs.CapeNoneEqupped))
		{
			return;
		}
		GameObject gameObject = Resources.Load<GameObject>(string.Concat("Capes/", cape));
		if (gameObject == null)
		{
			return;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(vector3);
		vector3.transform.parent = this.capePoint.transform;
		vector3.transform.localPosition = new Vector3(0f, -0.8f, 0f);
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(vector3, LayerMask.NameToLayer("NGUIShop"));
	}

	public void UpdatePersHat(string hat)
	{
		List<Transform> transforms = new List<Transform>();
		for (int i = 0; i < this.hatPoint.transform.childCount; i++)
		{
			transforms.Add(this.hatPoint.transform.GetChild(i));
		}
		foreach (Transform transform in transforms)
		{
			transform.parent = null;
			transform.position = new Vector3(0f, -10000f, 0f);
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		if (hat.Equals(Defs.HatNoneEqupped))
		{
			return;
		}
		string str = Storager.getString(Defs.VisualHatArmor, false);
		if (!string.IsNullOrEmpty(str) && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(hat) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(hat) < Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(str))
		{
			hat = str;
		}
		GameObject gameObject = Resources.Load(string.Concat("Hats/", hat)) as GameObject;
		if (gameObject == null)
		{
			return;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(vector3);
		vector3.transform.parent = this.hatPoint.transform;
		vector3.transform.localPosition = Vector3.zero;
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(vector3, LayerMask.NameToLayer("NGUIShop"));
		ShopNGUIController.SetPersHatVisible(this.hatPoint);
	}

	public void UpdatePersMask(string mask)
	{
		for (int i = 0; i < this.maskPoint.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.maskPoint.transform.GetChild(i).gameObject);
		}
		if (mask.Equals("MaskNoneEquipped"))
		{
			return;
		}
		GameObject gameObject = Resources.Load(string.Concat("Masks/", mask)) as GameObject;
		if (gameObject == null)
		{
			UnityEngine.Debug.LogWarning(string.Concat("ShopNGUIController UpdatePersMask: maskPrefab == null  mask = ", mask ?? "(null)"));
			return;
		}
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(vector3);
		vector3.transform.parent = this.maskPoint.transform;
		vector3.transform.localPosition = new Vector3(0f, 0f, 0f);
		vector3.transform.localRotation = Quaternion.identity;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(vector3, LayerMask.NameToLayer("NGUIShop"));
	}

	public void UpdatePersSkin(string skinId)
	{
		if (skinId == null)
		{
			UnityEngine.Debug.LogError("Skin id should not be null!");
			return;
		}
		this.SetSkinOnPers(SkinsController.skinsForPers[skinId]);
	}

	public void UpdatePersWithNewItem()
	{
		if (!this.WeaponCategory)
		{
			switch (this.currentCategory)
			{
				case ShopNGUIController.CategoryNames.HatsCategory:
				{
					this.UpdatePersHat(this.viewedId);
					break;
				}
				case ShopNGUIController.CategoryNames.ArmorCategory:
				{
					this.UpdatePersArmor(this.viewedId);
					break;
				}
				case ShopNGUIController.CategoryNames.SkinsCategory:
				{
					if (!this.viewedId.Equals("CustomSkinID"))
					{
						this.UpdatePersSkin(this.viewedId);
					}
					break;
				}
				case ShopNGUIController.CategoryNames.CapesCategory:
				{
					this.UpdatePersCape(this.viewedId);
					break;
				}
				case ShopNGUIController.CategoryNames.BootsCategory:
				{
					this.UpdatePersBoots(this.viewedId);
					break;
				}
				case ShopNGUIController.CategoryNames.GearCategory:
				{
					break;
				}
				case ShopNGUIController.CategoryNames.MaskCategory:
				{
					this.UpdatePersMask(this.viewedId);
					break;
				}
				default:
				{
					goto case ShopNGUIController.CategoryNames.GearCategory;
				}
			}
		}
		else
		{
			string tag = this.viewedId;
			if (tag == null && WeaponManager.sharedManager.playerWeapons.Count > 0)
			{
				tag = ItemDb.GetByPrefabName((WeaponManager.sharedManager.playerWeapons[0] as Weapon).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
			}
			this.SetWeapon(tag);
		}
	}

	private void UpdateTempItemTime()
	{
		if (TempItemsController.sharedController != null)
		{
			bool flag = (!TempItemsController.PriceCoefs.ContainsKey(this.viewedId) ? false : !TempItemsController.sharedController.ContainsItem(this.viewedId));
			if (this.notRented.activeInHierarchy != flag)
			{
				this.notRented.SetActive(flag);
			}
			string str = TempItemsController.sharedController.TimeRemainingForItemString(this.viewedId);
			bool flag1 = (!TempItemsController.sharedController.ContainsItem(this.viewedId) ? false : str.Length < 5);
			if (this.daysLeftLabel.gameObject.activeInHierarchy != flag1)
			{
				this.daysLeftLabel.gameObject.SetActive(flag1);
			}
			if (this.daysLeftValueLabel.gameObject.activeInHierarchy != flag1)
			{
				this.daysLeftValueLabel.gameObject.SetActive(flag1);
			}
			if (flag1)
			{
				this.daysLeftValueLabel.text = str;
			}
			bool flag2 = (!TempItemsController.sharedController.ContainsItem(this.viewedId) ? false : str.Length >= 5);
			if (this.timeLeftLabel.gameObject.activeInHierarchy != flag2)
			{
				this.timeLeftLabel.gameObject.SetActive(flag2);
			}
			if (this.timeLeftValueLabel.gameObject.activeInHierarchy != flag2)
			{
				this.timeLeftValueLabel.gameObject.SetActive(flag2);
			}
			if (flag2)
			{
				this.timeLeftValueLabel.text = str;
			}
			bool flag3 = (!flag2 ? false : TempItemsController.sharedController.TimeRemainingForItems(this.viewedId) <= (long)3600);
			if (this.redBackForTime.activeInHierarchy != flag3)
			{
				this.redBackForTime.SetActive(flag3);
			}
		}
	}

	private void UpdateTryGunDiscountTime()
	{
		try
		{
			this.tryGunDiscountTime.text = TempItemsController.TempItemTimeRemainsStringRepresentation(WeaponManager.sharedManager.StartTimeForTryGunDiscount(this.viewedId) + (long)WeaponManager.TryGunPromoDuration() - PromoActionsManager.CurrentUnixTime);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in tryGunDiscountPanelActive.text: ", exception));
		}
	}

	private static int UpperCoinsBankBound()
	{
		int num = (ExperienceController.sharedController == null ? 1 : ExperienceController.sharedController.currentLevel);
		num = Mathf.Clamp(num, 0, (int)ExperienceController.addCoinsFromLevels.Length - 1);
		return Storager.getInt("Coins", false) + 30 + ExperienceController.addCoinsFromLevels[num];
	}

	private static int UpperGemsBankBound()
	{
		int num = (ExperienceController.sharedController == null ? 1 : ExperienceController.sharedController.currentLevel);
		num = Mathf.Clamp(num, 0, (int)ExperienceController.addGemsFromLevels.Length - 1);
		return Storager.getInt("GemsCurrency", false) + ExperienceController.addGemsFromLevels[num];
	}

	public string WearForCat(ShopNGUIController.CategoryNames c)
	{
		string str;
		if (c == ShopNGUIController.CategoryNames.CapesCategory)
		{
			str = this._currentCape;
		}
		else if (c == ShopNGUIController.CategoryNames.BootsCategory)
		{
			str = this._currentBoots;
		}
		else if (c == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			str = this._currentArmor;
		}
		else if (c != ShopNGUIController.CategoryNames.HatsCategory)
		{
			str = (c != ShopNGUIController.CategoryNames.MaskCategory ? string.Empty : this._currentMask);
		}
		else
		{
			str = this._currentHat;
		}
		return str;
	}

	public static event Action GunBought;

	public static event Action GunOrArmorBought;

	public static event Action ShowArmorChanged;

	public static event Action<string> TryGunBought;

	public delegate void Action7<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

	public enum CategoryNames
	{
		PrimaryCategory,
		BackupCategory,
		MeleeCategory,
		SpecilCategory,
		SniperCategory,
		PremiumCategory,
		HatsCategory,
		ArmorCategory,
		SkinsCategory,
		CapesCategory,
		BootsCategory,
		GearCategory,
		MaskCategory
	}

	public enum TrainingState
	{
		NotInSniperCategory,
		OnSniperRifle,
		InSniperCategoryNotOnSniperRifle,
		NotInArmorCategory,
		OnArmor,
		InArmorCategoryNotOnArmor,
		BackBlinking
	}
}