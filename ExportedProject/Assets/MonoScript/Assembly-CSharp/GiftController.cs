using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GiftController : MonoBehaviour
{
	private const string FREE_SPINS_STORAGER_KEY = "freeSpinsCount";

	public const string KEY_COUNT_GIFT_FOR_NEW_PLAYER = "keyCountGiftNewPlayer";

	public const string KEY_EDITOR_SKIN = "editor_Skin";

	public const string KEY_EDITOR_CAPE = "editor_Cape";

	public const string KEY_COLLECTION_GUNS_GRAY = "guns_gray";

	public const string KEY_COLLECTION_MASK = "equip_Mask";

	public const string KEY_COLLECTION_CAPE = "equip_Cape";

	public const string KEY_COLLECTION_BOOTS = "equip_Boots";

	public const string KEY_COLLECTION_HAT = "equip_Hat";

	private const string KEY_FOR_SAVE_SERVER_TIME = "SaveServerTime";

	private const string KEY_NEWPLAYER_ARMOR_GETTED = "keyIsGetArmorNewPlayer";

	private const string KEY_NEWPLAYER_SKIN_GETTED = "keyIsGetSkinNewPlayer";

	private const float UPDATE_DATA_FROM_SERVER_INTERVAL = 870f;

	private const int TIME_TO_NEXT_GIFT = 14400;

	public static GiftController Instance;

	public SaltedInt CostBuyCanGetGift = new SaltedInt(15461355, 0);

	private bool _canGetTimerGift;

	private float _localTimer = -1f;

	private int _oldTime = -1;

	[ReadOnly]
	[SerializeField]
	private readonly List<GiftCategory> _categories = new List<GiftCategory>();

	[ReadOnly]
	[SerializeField]
	private readonly List<SlotInfo> _slots = new List<SlotInfo>();

	[ReadOnly]
	[SerializeField]
	private readonly List<GiftNewPlayerInfo> _forNewPlayer = new List<GiftNewPlayerInfo>();

	private bool _cfgGachaIsActive;

	private static Dictionary<int, List<ItemRecord>> _grayCategoryWeapons;

	private SaltedInt _freeSpins = new SaltedInt(15461355, 0);

	private GiftCategoryType? _prevDroppedCategoryType;

	private bool _kAlreadyGenerateSlot;

	private bool _kDataLoading;

	public bool ActiveGift
	{
		get
		{
			return (!this._cfgGachaIsActive || !this.DataIsLoaded ? false : FriendsController.ServerTime >= (long)0);
		}
	}

	public bool CanGetFreeSpinGift
	{
		get
		{
			return (!this.ActiveGift ? false : this.FreeSpins > 0);
		}
	}

	public bool CanGetGift
	{
		get
		{
			return (this.CanGetTimerGift ? true : this.CanGetFreeSpinGift);
		}
	}

	public bool CanGetTimerGift
	{
		get
		{
			return (!this.ActiveGift ? false : this._canGetTimerGift);
		}
	}

	public static int CountGetGiftForNewPlayer
	{
		get
		{
			return Storager.getInt("keyCountGiftNewPlayer", false);
		}
		set
		{
			if (value >= 0 && value < GiftController.CountGetGiftForNewPlayer)
			{
				Storager.setInt("keyCountGiftNewPlayer", value, false);
			}
		}
	}

	public bool DataIsLoaded
	{
		get
		{
			if (this._slots == null)
			{
				return false;
			}
			if (this._slots.Count == 0)
			{
				return false;
			}
			return true;
		}
	}

	internal TimeSpan FreeGachaAvailableIn
	{
		get
		{
			if (!Storager.hasKey("SaveServerTime"))
			{
				this.LastTimeGetGift = FriendsController.ServerTime - (long)14400 + (long)1;
			}
			long serverTime = FriendsController.ServerTime - this.LastTimeGetGift;
			return TimeSpan.FromSeconds((double)((long)14400 - serverTime));
		}
	}

	public int FreeSpins
	{
		get
		{
			return this._freeSpins.Value;
		}
	}

	public static Dictionary<int, List<ItemRecord>> GrayCategoryWeapons
	{
		get
		{
			if (GiftController._grayCategoryWeapons == null)
			{
				GiftController._grayCategoryWeapons = new Dictionary<int, List<ItemRecord>>();
				Dictionary<int, List<ItemRecord>> nums = GiftController._grayCategoryWeapons;
				List<ItemRecord> itemRecords = new List<ItemRecord>()
				{
					ItemDb.GetByPrefabName("Weapon10"),
					ItemDb.GetByPrefabName("Weapon44"),
					ItemDb.GetByPrefabName("Weapon79")
				};
				nums.Add(0, itemRecords);
				Dictionary<int, List<ItemRecord>> nums1 = GiftController._grayCategoryWeapons;
				itemRecords = new List<ItemRecord>()
				{
					ItemDb.GetByPrefabName("Weapon278"),
					ItemDb.GetByPrefabName("Weapon336"),
					ItemDb.GetByPrefabName("Weapon65"),
					ItemDb.GetByPrefabName("Weapon286")
				};
				nums1.Add(1, itemRecords);
				Dictionary<int, List<ItemRecord>> nums2 = GiftController._grayCategoryWeapons;
				itemRecords = new List<ItemRecord>()
				{
					ItemDb.GetByPrefabName("Weapon252"),
					ItemDb.GetByPrefabName("Weapon258"),
					ItemDb.GetByPrefabName("Weapon48"),
					ItemDb.GetByPrefabName("Weapon253")
				};
				nums2.Add(2, itemRecords);
				Dictionary<int, List<ItemRecord>> nums3 = GiftController._grayCategoryWeapons;
				itemRecords = new List<ItemRecord>()
				{
					ItemDb.GetByPrefabName("Weapon257"),
					ItemDb.GetByPrefabName("Weapon262"),
					ItemDb.GetByPrefabName("Weapon251")
				};
				nums3.Add(3, itemRecords);
				Dictionary<int, List<ItemRecord>> nums4 = GiftController._grayCategoryWeapons;
				itemRecords = new List<ItemRecord>()
				{
					ItemDb.GetByPrefabName("Weapon330"),
					ItemDb.GetByPrefabName("Weapon308")
				};
				nums4.Add(4, itemRecords);
				Dictionary<int, List<ItemRecord>> nums5 = GiftController._grayCategoryWeapons;
				itemRecords = new List<ItemRecord>()
				{
					ItemDb.GetByPrefabName("Weapon222")
				};
				nums5.Add(5, itemRecords);
			}
			return GiftController._grayCategoryWeapons;
		}
	}

	private long LastTimeGetGift
	{
		get
		{
			return (long)Storager.getInt("SaveServerTime", false);
		}
		set
		{
			Storager.setInt("SaveServerTime", (int)value, false);
		}
	}

	public List<SlotInfo> Slots
	{
		get
		{
			return this._slots;
		}
	}

	public float TimeLeft
	{
		get
		{
			return this._localTimer;
		}
	}

	public static string UrlForLoadData
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_wp8.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_android.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_amazon.json";
			}
			return string.Empty;
		}
	}

	static GiftController()
	{
		GiftController.OnChangeSlots = () => {
		};
		GiftController.OnTimerEnded = () => {
		};
		GiftController.OnUpdateTimer = (string s) => {
		};
	}

	public GiftController()
	{
	}

	private void Awake()
	{
		GiftController.Instance = this;
		this._freeSpins.Value = Storager.getInt("freeSpinsCount", true);
		this._localTimer = -1f;
		this._categories.Clear();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Storager.hasKey("SaveServerTime"))
		{
			Storager.setInt("keyCountGiftNewPlayer", 2, false);
		}
		if (!Storager.hasKey("keyCountGiftNewPlayer"))
		{
			Storager.setInt("keyCountGiftNewPlayer", 0, false);
		}
		if (!Storager.hasKey("keyIsGetArmorNewPlayer"))
		{
			Storager.setInt("keyIsGetArmorNewPlayer", 0, false);
		}
		if (!Storager.hasKey("keyIsGetSkinNewPlayer"))
		{
			Storager.setInt("keyIsGetSkinNewPlayer", 0, false);
		}
		Storager.getInt("keyCountGiftNewPlayer", false);
		base.StartCoroutine(this.GetDataFromServerLoop());
		FriendsController.ServerTimeUpdated += new Action(this.OnUpdateTimeFromServer);
	}

	[DebuggerHidden]
	private IEnumerator CheckAvailableGifts()
	{
		GiftController.u003cCheckAvailableGiftsu003ec__Iterator14A variable = null;
		return variable;
	}

	public void CheckAvaliableSlots()
	{
		bool flag = false;
		for (int i = 0; i < this._slots.Count; i++)
		{
			SlotInfo item = this._slots[i];
			if (item.CheckAvaliableGift())
			{
				flag = true;
			}
			if (item.gift == null)
			{
				this._slots.RemoveAt(i);
				i--;
			}
		}
		if (flag && GiftController.OnChangeSlots != null)
		{
			GiftController.OnChangeSlots();
		}
	}

	[DebuggerHidden]
	private IEnumerator DownloadDataFormServer()
	{
		GiftController.u003cDownloadDataFormServeru003ec__Iterator149 variable = null;
		return variable;
	}

	public static List<string> GetAvailableGrayWeaponsTags()
	{
		int num = ExpController.OurTierForAnyPlace();
		return (
			from w in GiftController.GrayCategoryWeapons[num]
			where Storager.getInt(w.StorageId, true) == 0
			select w.Tag).ToList<string>();
	}

	[DebuggerHidden]
	private IEnumerator GetDataFromServerLoop()
	{
		GiftController.u003cGetDataFromServerLoopu003ec__Iterator148 variable = null;
		return variable;
	}

	public SlotInfo GetGift(bool ignoreAvailabilityCheck = false)
	{
		if (!ignoreAvailabilityCheck)
		{
			if (!this.CanGetTimerGift)
			{
				if (this.FreeSpins <= 0)
				{
					return null;
				}
				ref SaltedInt value = ref this._freeSpins;
				value.Value = value.Value - 1;
				Storager.setInt("freeSpinsCount", this._freeSpins.Value, true);
			}
			else
			{
				this._canGetTimerGift = false;
				this._localTimer = -1f;
				this.ReSaveLastTimeSever();
			}
		}
		List<SlotInfo> list = (
			from s in this._slots
			where !s.NoDropped
			select s).ToList<SlotInfo>();
		float single = list.Sum<SlotInfo>((SlotInfo s) => s.percentGetSlot);
		float single1 = UnityEngine.Random.Range(0f, single);
		float single2 = 0f;
		SlotInfo slotInfo = null;
		int num = 0;
		while (num < list.Count)
		{
			SlotInfo item = list[num];
			single2 += item.percentGetSlot;
			if (single1 > single2)
			{
				num++;
			}
			else
			{
				slotInfo = item;
				slotInfo.numInScroll = this._slots.IndexOf(item);
				break;
			}
		}
		if (slotInfo != null)
		{
			GiftController.CountGetGiftForNewPlayer = GiftController.CountGetGiftForNewPlayer - 1;
			this.GiveProductForSlot(slotInfo);
		}
		slotInfo.NoDropped = true;
		this._prevDroppedCategoryType = new GiftCategoryType?(slotInfo.category.Type);
		return slotInfo;
	}

	public GiftNewPlayerInfo GetInfoNewPlayer(GiftCategoryType needCat)
	{
		return this._forNewPlayer.Find((GiftNewPlayerInfo val) => val.TypeCategory == needCat);
	}

	private string GetRandomGrayWeapon()
	{
		List<string> availableGrayWeaponsTags = GiftController.GetAvailableGrayWeaponsTags();
		if (!availableGrayWeaponsTags.Any<string>())
		{
			return string.Empty;
		}
		return availableGrayWeaponsTags[UnityEngine.Random.Range(0, availableGrayWeaponsTags.Count)];
	}

	public SlotInfo GetRandomSlot()
	{
		return null;
	}

	public string GetStringTimer()
	{
		string str;
		string str1;
		string str2;
		int num = (int)(this._localTimer / 3600f);
		int num1 = (int)(this._localTimer / 60f) - num * 60;
		int num2 = (int)this._localTimer - num * 3600 - num1 * 60;
		str = (num >= 10 ? num.ToString() : string.Concat("0", num));
		str1 = (num1 >= 10 ? num1.ToString() : string.Concat("0", num1));
		str2 = (num2 >= 10 ? num2.ToString() : string.Concat("0", num2));
		return string.Concat(new string[] { str, ":", str1, ":", str2 });
	}

	private void GiveProduct(ShopNGUIController.CategoryNames category, string tag)
	{
		ShopNGUIController.ProvideShopItemOnStarterPackBoguht(category, tag, 1, false, 0, null, null, true, true, false);
		if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
		{
			ShopNGUIController.sharedShop.wearEquipAction(category, string.Empty, string.Empty);
		}
	}

	public void GiveProductForSlot(SlotInfo curSlot)
	{
		if (curSlot != null)
		{
			switch (curSlot.category.Type)
			{
				case GiftCategoryType.Coins:
				{
					BankController.AddCoins(curSlot.CountGift, false, AnalyticsConstants.AccrualType.Earned);
					base.StartCoroutine(BankController.WaitForIndicationGems(false));
					break;
				}
				case GiftCategoryType.Gems:
				{
					BankController.AddGems(curSlot.CountGift, false, AnalyticsConstants.AccrualType.Earned);
					base.StartCoroutine(BankController.WaitForIndicationGems(true));
					break;
				}
				case GiftCategoryType.Grenades:
				{
					int num = Storager.getInt(curSlot.gift.Id, false);
					Storager.setInt(curSlot.gift.Id, num + curSlot.gift.Count.Value, false);
					break;
				}
				case GiftCategoryType.Gear:
				{
					int num1 = Storager.getInt(curSlot.gift.Id, false);
					Storager.setInt(curSlot.gift.Id, num1 + curSlot.gift.Count.Value, false);
					break;
				}
				case GiftCategoryType.Skins:
				{
					Storager.setInt("keyIsGetSkinNewPlayer", 1, false);
					ShopNGUIController.ProvideShopItemOnStarterPackBoguht(ShopNGUIController.CategoryNames.SkinsCategory, curSlot.gift.Id, 1, false, 0, null, null, false, true, false);
					break;
				}
				case GiftCategoryType.ArmorAndHat:
				{
					Storager.setInt("keyIsGetArmorNewPlayer", 1, false);
					ShopNGUIController.CategoryNames? typeShopCat = curSlot.gift.TypeShopCat;
					if ((typeShopCat.GetValueOrDefault() != ShopNGUIController.CategoryNames.ArmorCategory ? false : typeShopCat.HasValue))
					{
						ShopNGUIController.ProvideShopItemOnStarterPackBoguht(ShopNGUIController.CategoryNames.ArmorCategory, curSlot.gift.Id, 1, false, 0, null, null, true, true, false);
						if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
						{
							ShopNGUIController.sharedShop.wearEquipAction(7, string.Empty, string.Empty);
						}
					}
					break;
				}
				case GiftCategoryType.Wear:
				{
					ShopNGUIController.CategoryNames? nullable = curSlot.gift.TypeShopCat;
					ShopNGUIController.ProvideShopItemOnStarterPackBoguht(nullable.Value, curSlot.gift.Id, 1, false, 0, null, null, true, true, false);
					if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
					{
						Action<ShopNGUIController.CategoryNames, string, string> action = ShopNGUIController.sharedShop.wearEquipAction;
						ShopNGUIController.CategoryNames? typeShopCat1 = curSlot.gift.TypeShopCat;
						action(typeShopCat1.Value, string.Empty, string.Empty);
					}
					break;
				}
				case GiftCategoryType.Editor:
				{
					if (curSlot.gift.Id == "editor_Cape")
					{
						this.GiveProduct(ShopNGUIController.CategoryNames.CapesCategory, "cape_Custom");
					}
					else if (curSlot.gift.Id != "editor_Skin")
					{
						UnityEngine.Debug.LogError(string.Format("[GIFT] unknown editor id: '{0}'", curSlot.gift.Id));
					}
					else
					{
						Storager.setInt(Defs.SkinsMakerInProfileBought, 1, true);
					}
					break;
				}
				case GiftCategoryType.Masks:
				{
					this.GiveProduct(ShopNGUIController.CategoryNames.MaskCategory, curSlot.gift.Id);
					break;
				}
				case GiftCategoryType.Capes:
				{
					this.GiveProduct(ShopNGUIController.CategoryNames.CapesCategory, curSlot.gift.Id);
					break;
				}
				case GiftCategoryType.Boots:
				{
					this.GiveProduct(ShopNGUIController.CategoryNames.BootsCategory, curSlot.gift.Id);
					break;
				}
				case GiftCategoryType.Hats_random:
				{
					this.GiveProduct(ShopNGUIController.CategoryNames.HatsCategory, curSlot.gift.Id);
					break;
				}
				case GiftCategoryType.Gun1:
				case GiftCategoryType.Gun2:
				case GiftCategoryType.Gun3:
				case GiftCategoryType.Guns_gray:
				{
					if (!WeaponManager.IsExclusiveWeapon(curSlot.gift.Id))
					{
						ShopNGUIController.CategoryNames? nullable1 = curSlot.gift.TypeShopCat;
						this.GiveProduct(nullable1.Value, curSlot.gift.Id);
					}
					else
					{
						WeaponManager.ProvideExclusiveWeaponByTag(curSlot.gift.Id);
					}
					break;
				}
				case GiftCategoryType.Stickers:
				{
					TypePackSticker? @enum = curSlot.gift.Id.ToEnum<TypePackSticker>(null);
					if (!@enum.HasValue)
					{
						throw new Exception("sticker id type parse error");
					}
					StickersController.BuyStickersPack(@enum.Value);
					break;
				}
				case GiftCategoryType.Freespins:
				{
					ref SaltedInt value = ref this._freeSpins;
					value.Value = value.Value + curSlot.gift.Count.Value;
					Storager.setInt("freeSpinsCount", this._freeSpins.Value, true);
					break;
				}
			}
		}
	}

	private void OnDataLoaded()
	{
		this.SetGifts();
	}

	private void OnDestroy()
	{
		FriendsController.ServerTimeUpdated -= new Action(this.OnUpdateTimeFromServer);
		GiftController.Instance = null;
	}

	private void OnUpdateTimeFromServer()
	{
		if (this._slots.Count == 0)
		{
			base.StartCoroutine(this.DownloadDataFormServer());
			return;
		}
		if (FriendsController.ServerTime < (long)0)
		{
			return;
		}
		this._localTimer = -1f;
		this._canGetTimerGift = false;
		if (!Storager.hasKey("SaveServerTime"))
		{
			this.LastTimeGetGift = FriendsController.ServerTime - (long)14400 + (long)1;
		}
		int serverTime = (int)(FriendsController.ServerTime - this.LastTimeGetGift);
		if (serverTime < 14400)
		{
			this._canGetTimerGift = false;
			this._localTimer = (float)(14400 - serverTime);
		}
		else
		{
			this._canGetTimerGift = true;
			if (GiftController.OnTimerEnded != null)
			{
				GiftController.OnTimerEnded();
			}
		}
	}

	private GiftCategoryType ParseToEnum(string typeCat)
	{
		GiftCategoryType? @enum = typeCat.ToEnum<GiftCategoryType>(null);
		return (!@enum.HasValue ? GiftCategoryType.none : @enum.Value);
	}

	public void RecreateSlots()
	{
		IEnumerable<GiftCategory> giftCategories;
		if (!this._kAlreadyGenerateSlot && this._cfgGachaIsActive)
		{
			this._kAlreadyGenerateSlot = true;
			this._slots.Clear();
			if (!FriendsController.SandboxEnabled)
			{
				giftCategories = 
					from c in this._categories
					where c.Type != GiftCategoryType.Gear
					select c;
			}
			else
			{
				giftCategories = this._categories;
			}
			IEnumerator<GiftCategory> enumerator = giftCategories.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GiftCategory current = enumerator.Current;
					current.CheckGifts();
					if (current.AvaliableGiftsCount >= 1)
					{
						SlotInfo slotInfo = new SlotInfo()
						{
							category = current,
							gift = current.GetRandomGift()
						};
						if (slotInfo.gift != null && !string.IsNullOrEmpty(slotInfo.gift.Id))
						{
							slotInfo.percentGetSlot = current.PercentChance;
							slotInfo.positionInScroll = current.ScrollPosition;
							slotInfo.isActiveEvent = false;
							if (GiftController.CountGetGiftForNewPlayer > 0)
							{
								this.SetPerGetGiftForNewPlayer(slotInfo);
							}
							this._slots.Add(slotInfo);
						}
					}
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			if (this._prevDroppedCategoryType.HasValue)
			{
				SlotInfo slotInfo1 = this._slots.FirstOrDefault<SlotInfo>((SlotInfo s) => {
					GiftCategoryType type = s.category.Type;
					GiftCategoryType? nullable = this._prevDroppedCategoryType;
					return (type != nullable.GetValueOrDefault() ? false : nullable.HasValue);
				});
				if (slotInfo1 != null)
				{
					slotInfo1.NoDropped = true;
				}
			}
			this._slots.Sort((SlotInfo left, SlotInfo right) => {
				if (left == null && right == null)
				{
					return 0;
				}
				if (left == null)
				{
					return -1;
				}
				if (right == null)
				{
					return 1;
				}
				return left.positionInScroll.CompareTo(right.positionInScroll);
			});
			if (GiftController.OnChangeSlots != null)
			{
				GiftController.OnChangeSlots();
			}
			this.OnUpdateTimeFromServer();
		}
	}

	public void ReCreateSlots()
	{
		this._kAlreadyGenerateSlot = false;
		this.SetGifts();
	}

	public void ReSaveLastTimeSever()
	{
		this.LastTimeGetGift = FriendsController.ServerTime;
		this.OnUpdateTimeFromServer();
	}

	public void SetGifts()
	{
		if (!this._cfgGachaIsActive)
		{
			this._categories.Clear();
			this._slots.Clear();
			if (GiftController.OnChangeSlots != null)
			{
				GiftController.OnChangeSlots();
			}
		}
		else if (this._categories != null && this._categories.Count > 0)
		{
			base.StartCoroutine(this.CheckAvailableGifts());
		}
	}

	private void SetPerGetGiftForNewPlayer(SlotInfo curSlot)
	{
		float percent = 0f;
		int value = curSlot.gift.Count.Value;
		curSlot.isActiveEvent = true;
		GiftNewPlayerInfo infoNewPlayer = this.GetInfoNewPlayer(curSlot.category.Type);
		if (infoNewPlayer != null)
		{
			value = infoNewPlayer.Count.Value;
			if (curSlot.category.Type == GiftCategoryType.ArmorAndHat && Storager.getInt("keyIsGetArmorNewPlayer", false) == 0)
			{
				percent = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Skins && Storager.getInt("keyIsGetSkinNewPlayer", false) == 0)
			{
				percent = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Coins)
			{
				percent = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Gems)
			{
				percent = infoNewPlayer.Percent;
			}
		}
		curSlot.percentGetSlot = percent;
		curSlot.CountGift = value;
	}

	public void SetTimer(int val)
	{
		if (val > 14400)
		{
			val = 14400;
		}
		if (val != 0)
		{
			long serverTime = FriendsController.ServerTime - (long)(14400 - val);
			this.LastTimeGetGift = serverTime;
		}
		else
		{
			this.LastTimeGetGift = FriendsController.ServerTime - (long)14400 + (long)1;
		}
		this.OnUpdateTimeFromServer();
	}

	public void TryGetData()
	{
		if (!this.DataIsLoaded)
		{
			base.StartCoroutine(this.DownloadDataFormServer());
		}
	}

	private void Update()
	{
		if (this._localTimer > 0f)
		{
			this._localTimer -= Time.deltaTime;
			if (this._localTimer < 0f)
			{
				this._localTimer = 0f;
			}
			this._canGetTimerGift = false;
			if (this._oldTime != (int)this._localTimer)
			{
				this._oldTime = (int)this._localTimer;
				if (GiftController.OnUpdateTimer != null)
				{
					GiftController.OnUpdateTimer(this.GetStringTimer());
				}
			}
		}
		else if (!this._canGetTimerGift && (int)this._localTimer == 0)
		{
			this._localTimer = -1f;
			this._canGetTimerGift = true;
			if (GiftController.OnUpdateTimer != null)
			{
				GiftController.OnUpdateTimer(this.GetStringTimer());
			}
			if (GiftController.OnTimerEnded != null)
			{
				GiftController.OnTimerEnded();
			}
		}
	}

	public void UpdateSlot(SlotInfo curSlot)
	{
		curSlot.category.CheckGifts();
		curSlot.gift = curSlot.category.GetRandomGift();
		if (curSlot.gift != null)
		{
			curSlot.percentGetSlot = curSlot.category.PercentChance;
			curSlot.positionInScroll = curSlot.category.ScrollPosition;
		}
		else
		{
			this._slots.Remove(curSlot);
		}
		foreach (SlotInfo _slot in this._slots)
		{
			this._categories.FirstOrDefault<GiftCategory>((GiftCategory c) => c == _slot.category);
			if (GiftController.CountGetGiftForNewPlayer <= 0)
			{
				_slot.percentGetSlot = _slot.category.PercentChance;
			}
			else
			{
				this.SetPerGetGiftForNewPlayer(_slot);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitDrop(GiftCategory cat, string id, bool isContains = false)
	{
		GiftController.u003cWaitDropu003ec__Iterator147 variable = null;
		return variable;
	}

	public static event Action OnChangeSlots;

	public static event Action OnTimerEnded;

	public static event Action<string> OnUpdateTimer;
}