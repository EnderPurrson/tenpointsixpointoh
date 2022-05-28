using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ItemDb
{
	[CompilerGenerated]
	private sealed class _003CGetItemName_003Ec__AnonStorey34B
	{
		internal ItemRecord firstRec;

		internal bool _003C_003Em__55D(WeaponSounds weapon)
		{
			return weapon.name == firstRec.PrefabName;
		}
	}

	public const int CrystalCrossbowCoinsPrice = 150;

	private static List<ItemRecord> _records;

	private static Dictionary<int, ItemRecord> _recordsById;

	private static Dictionary<string, ItemRecord> _recordsByTag;

	private static Dictionary<string, ItemRecord> _recordsByStorageId;

	private static Dictionary<string, ItemRecord> _recordsByShopId;

	private static Dictionary<string, ItemRecord> _recordsByPrefabName;

	[CompilerGenerated]
	private static Func<ItemRecord, bool> _003C_003Ef__am_0024cache6;

	[CompilerGenerated]
	private static Func<ItemRecord, bool> _003C_003Ef__am_0024cache7;

	[CompilerGenerated]
	private static Func<ItemRecord, string> _003C_003Ef__am_0024cache8;

	[CompilerGenerated]
	private static Func<ItemRecord, bool> _003C_003Ef__am_0024cache9;

	[CompilerGenerated]
	private static Func<ItemRecord, string> _003C_003Ef__am_0024cacheA;

	public static List<ItemRecord> allRecords
	{
		get
		{
			return _records;
		}
	}

	static ItemDb()
	{
		_records = ItemDbRecords.GetRecords();
		_recordsById = new Dictionary<int, ItemRecord>(_records.Count);
		_recordsByTag = new Dictionary<string, ItemRecord>(_records.Count);
		_recordsByStorageId = new Dictionary<string, ItemRecord>(_records.Count);
		_recordsByShopId = new Dictionary<string, ItemRecord>(_records.Count);
		_recordsByPrefabName = new Dictionary<string, ItemRecord>(_records.Count);
		for (int i = 0; i < _records.Count; i++)
		{
			ItemRecord itemRecord = _records[i];
			_recordsById[itemRecord.Id] = itemRecord;
			if (!string.IsNullOrEmpty(itemRecord.Tag))
			{
				_recordsByTag[itemRecord.Tag] = itemRecord;
			}
			if (!string.IsNullOrEmpty(itemRecord.StorageId))
			{
				_recordsByStorageId[itemRecord.StorageId] = itemRecord;
			}
			if (!string.IsNullOrEmpty(itemRecord.ShopId))
			{
				_recordsByShopId[itemRecord.ShopId] = itemRecord;
			}
			if (!string.IsNullOrEmpty(itemRecord.PrefabName))
			{
				_recordsByPrefabName[itemRecord.PrefabName] = itemRecord;
			}
		}
	}

	public static ItemRecord GetById(int id)
	{
		if (_recordsById.ContainsKey(id))
		{
			return _recordsById[id];
		}
		return null;
	}

	public static ItemRecord GetByTag(string tag)
	{
		if (tag == null)
		{
			return null;
		}
		ItemRecord value;
		if (_recordsByTag.TryGetValue(tag, out value))
		{
			return value;
		}
		return null;
	}

	public static ItemRecord GetByPrefabName(string prefabName)
	{
		if (prefabName == null)
		{
			return null;
		}
		ItemRecord value;
		if (_recordsByPrefabName.TryGetValue(prefabName, out value))
		{
			return value;
		}
		return null;
	}

	public static ItemRecord GetByShopId(string shopId)
	{
		if (shopId == null)
		{
			return null;
		}
		ItemRecord value;
		if (_recordsByShopId.TryGetValue(shopId, out value))
		{
			return value;
		}
		return null;
	}

	public static ItemPrice GetPriceByShopId(string shopId)
	{
		if (shopId == null)
		{
			return null;
		}
		ItemRecord value;
		if (_recordsByShopId.TryGetValue(shopId, out value))
		{
			return value.Price;
		}
		return VirtualCurrencyHelper.Price(shopId);
	}

	public static bool IsCanBuy(string tag)
	{
		ItemRecord byTag = GetByTag(tag);
		if (byTag != null)
		{
			return byTag.CanBuy;
		}
		return false;
	}

	public static string GetShopIdByTag(string tag)
	{
		ItemRecord byTag = GetByTag(tag);
		return (byTag == null) ? null : byTag.ShopId;
	}

	public static string GetTagByShopId(string shopId)
	{
		ItemRecord byShopId = GetByShopId(shopId);
		return (byShopId == null) ? null : byShopId.Tag;
	}

	public static string GetStorageIdByTag(string tag)
	{
		ItemRecord byTag = GetByTag(tag);
		return (byTag == null) ? null : byTag.StorageId;
	}

	public static string GetStorageIdByShopId(string shopId)
	{
		ItemRecord byShopId = GetByShopId(shopId);
		return (byShopId == null) ? null : byShopId.StorageId;
	}

	public static IEnumerable<ItemRecord> GetCanBuyWeapon(bool includeDeactivated = false)
	{
		if (includeDeactivated)
		{
			List<ItemRecord> records = _records;
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003CGetCanBuyWeapon_003Em__558;
			}
			return records.Where(_003C_003Ef__am_0024cache6);
		}
		List<ItemRecord> records2 = _records;
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = _003CGetCanBuyWeapon_003Em__559;
		}
		return records2.Where(_003C_003Ef__am_0024cache7);
	}

	public static string[] GetCanBuyWeaponTags(bool includeDeactivated = false)
	{
		IEnumerable<ItemRecord> canBuyWeapon = GetCanBuyWeapon(includeDeactivated);
		if (_003C_003Ef__am_0024cache8 == null)
		{
			_003C_003Ef__am_0024cache8 = _003CGetCanBuyWeaponTags_003Em__55A;
		}
		return canBuyWeapon.Select(_003C_003Ef__am_0024cache8).ToArray();
	}

	public static List<string> GetCanBuyWeaponStorageIds(bool includeDeactivated = false)
	{
		IEnumerable<ItemRecord> canBuyWeapon = GetCanBuyWeapon(includeDeactivated);
		if (_003C_003Ef__am_0024cache9 == null)
		{
			_003C_003Ef__am_0024cache9 = _003CGetCanBuyWeaponStorageIds_003Em__55B;
		}
		IEnumerable<ItemRecord> source = canBuyWeapon.Where(_003C_003Ef__am_0024cache9);
		if (_003C_003Ef__am_0024cacheA == null)
		{
			_003C_003Ef__am_0024cacheA = _003CGetCanBuyWeaponStorageIds_003Em__55C;
		}
		return source.Select(_003C_003Ef__am_0024cacheA).ToList();
	}

	public static void Fill_tagToStoreIDMapping(Dictionary<string, string> tagToStoreIDMapping)
	{
		tagToStoreIDMapping.Clear();
		foreach (KeyValuePair<string, ItemRecord> item in _recordsByTag)
		{
			if (!string.IsNullOrEmpty(item.Value.ShopId))
			{
				tagToStoreIDMapping[item.Key] = item.Value.ShopId;
			}
		}
	}

	public static void Fill_storeIDtoDefsSNMapping(Dictionary<string, string> storeIDtoDefsSNMapping)
	{
		storeIDtoDefsSNMapping.Clear();
		foreach (KeyValuePair<string, ItemRecord> item in _recordsByShopId)
		{
			if (!string.IsNullOrEmpty(item.Value.StorageId))
			{
				storeIDtoDefsSNMapping[item.Key] = item.Value.StorageId;
			}
		}
	}

	public static bool IsTemporaryGun(string tg)
	{
		if (tg == null)
		{
			return false;
		}
		ItemRecord byTag = GetByTag(tg);
		return byTag != null && byTag.TemporaryGun;
	}

	public static bool IsWeaponCanDrop(string tag)
	{
		if (tag == "Knife")
		{
			return false;
		}
		ItemRecord byTag = GetByTag(tag);
		if (byTag == null)
		{
			return false;
		}
		return !byTag.CanBuy;
	}

	public static void InitStorageForTag(string tag)
	{
		ItemRecord byTag = GetByTag(tag);
		if (byTag == null)
		{
			Debug.LogWarning("item didn't found for tag: " + tag);
		}
		else if (string.IsNullOrEmpty(byTag.StorageId))
		{
			Debug.LogWarning("StoragId is null or empty for tag: " + tag);
		}
		else
		{
			Storager.setInt(byTag.StorageId, 0, false);
		}
	}

	public static int GetItemCategory(string tag)
	{
		int num = -1;
		ItemRecord byTag = GetByTag(tag);
		if (byTag != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + byTag.PrefabName);
			return (!(weaponSounds != null)) ? (-1) : (weaponSounds.categoryNabor - 1);
		}
		if (num == -1)
		{
			bool flag = false;
			foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> item in Wear.wear)
			{
				foreach (List<string> item2 in item.Value)
				{
					if (item2.Contains(tag))
					{
						flag = true;
						num = (int)item.Key;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		if (num == -1 && (SkinsController.skinsNamesForPers.ContainsKey(tag) || tag.Equals("CustomSkinID")))
		{
			num = 8;
		}
		if (num == -1 && GearManager.IsItemGear(tag))
		{
			num = 11;
		}
		return num;
	}

	public static int[] GetItemFilterMap(string tag)
	{
		ItemRecord byTag = GetByTag(tag);
		if (byTag != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + byTag.PrefabName);
			return (!(weaponSounds != null)) ? new int[0] : ((weaponSounds.filterMap == null) ? new int[0] : weaponSounds.filterMap);
		}
		return new int[0];
	}

	public static ShopPositionParams GetInfoForNonWeaponItem(string name, ShopNGUIController.CategoryNames category)
	{
		ShopPositionParams result = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			result = Resources.Load<ShopPositionParams>("Armor_Info/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			result = Resources.Load<ShopPositionParams>("Hats_Info/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			result = Resources.Load<ShopPositionParams>("Capes_Info/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			result = Resources.Load<ShopPositionParams>("Shop_Boots_Info/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			result = Resources.Load<ShopPositionParams>("Shop_Gear/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			result = Resources.Load<ShopPositionParams>("Masks_Info/" + name);
		}
		return result;
	}

	public static GameObject GetWearFromResources(string name, ShopNGUIController.CategoryNames category)
	{
		GameObject result = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			result = Resources.Load<GameObject>("Armor/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			result = Resources.Load<GameObject>("Hats/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			result = Resources.Load<GameObject>("Capes/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			result = Resources.Load<GameObject>("Shop_Boots/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			result = Resources.Load<GameObject>("Shop_Gear/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			result = Resources.Load<GameObject>("Masks/" + name);
		}
		return result;
	}

	public static ResourceRequest GetWearFromResourcesAsync(string name, ShopNGUIController.CategoryNames category)
	{
		ResourceRequest result = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			result = Resources.LoadAsync<GameObject>("Armor/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			result = Resources.LoadAsync<GameObject>("Hats/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			result = Resources.LoadAsync<GameObject>("Capes/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			result = Resources.LoadAsync<GameObject>("Shop_Boots/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			result = Resources.LoadAsync<GameObject>("Shop_Gear/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			result = Resources.LoadAsync<GameObject>("Masks/" + name);
		}
		return result;
	}

	public static string GetItemName(string tag, ShopNGUIController.CategoryNames category)
	{
		if (string.IsNullOrEmpty(tag))
		{
			return string.Empty;
		}
		ShopPositionParams shopPositionParams = null;
		ItemRecord byTag = GetByTag(tag);
		if (byTag != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + byTag.PrefabName);
			string empty = string.Empty;
			if (weaponSounds != null)
			{
				try
				{
					_003CGetItemName_003Ec__AnonStorey34B _003CGetItemName_003Ec__AnonStorey34B = new _003CGetItemName_003Ec__AnonStorey34B();
					ItemRecord byPrefabName = GetByPrefabName(weaponSounds.name.Replace("(Clone)", string.Empty));
					string tag2 = WeaponUpgrades.TagOfFirstUpgrade(byPrefabName.Tag);
					_003CGetItemName_003Ec__AnonStorey34B.firstRec = GetByTag(tag2);
					return WeaponManager.AllWrapperPrefabs().First(_003CGetItemName_003Ec__AnonStorey34B._003C_003Em__55D).shopName;
				}
				catch (Exception ex)
				{
					Debug.LogError("Error in getting shop name of first upgrade: " + ex);
					return weaponSounds.shopName;
				}
			}
			return empty;
		}
		shopPositionParams = GetInfoForNonWeaponItem(tag, category);
		if (shopPositionParams != null)
		{
			return shopPositionParams.shopName;
		}
		return string.Empty;
	}

	public static string GetItemNameNonLocalized(string tag, string shopId, ShopNGUIController.CategoryNames category, string defaultDesc = null)
	{
		ItemRecord byTag = GetByTag(tag);
		if (byTag != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + byTag.PrefabName);
			return (!(weaponSounds != null)) ? string.Empty : weaponSounds.shopNameNonLocalized;
		}
		switch (category)
		{
		case ShopNGUIController.CategoryNames.ArmorCategory:
			return Resources.Load<ShopPositionParams>("Armor_Info/" + tag).shopNameNonLocalized;
		case ShopNGUIController.CategoryNames.HatsCategory:
		{
			ShopPositionParams shopPositionParams = Resources.Load<ShopPositionParams>("Hats_Info/" + tag);
			return (!(shopPositionParams != null)) ? string.Empty : shopPositionParams.shopNameNonLocalized;
		}
		default:
			if (InAppData.inappReadableNames.ContainsKey(shopId))
			{
				return InAppData.inappReadableNames[shopId];
			}
			return defaultDesc ?? shopId;
		}
	}

	public static Texture GetItemIcon(string tag, ShopNGUIController.CategoryNames category, bool withUpdates)
	{
		if (category == (ShopNGUIController.CategoryNames)(-1))
		{
			return null;
		}
		string text = null;
		string text2 = tag;
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			foreach (List<string> upgrade in WeaponUpgrades.upgrades)
			{
				if (upgrade.Contains(tag))
				{
					text2 = upgrade[0];
					break;
				}
			}
		}
		if (text2 != null)
		{
			if (withUpdates)
			{
				int num = 1;
				if (ShopNGUIController.IsWeaponCategory(category))
				{
					ItemRecord byTag = GetByTag(tag);
					if ((byTag == null || !byTag.UseImagesFromFirstUpgrade) && (byTag == null || !byTag.TemporaryGun))
					{
						bool maxUpgrade;
						num = 1 + ShopNGUIController._CurrentNumberOfUpgrades(text2, out maxUpgrade, category);
					}
				}
				else if (category == ShopNGUIController.CategoryNames.GearCategory)
				{
					num = ((!Defs.isDaterRegim) ? ((!(tag != GearManager.Grenade)) ? 1 : (1 + GearManager.CurrentNumberOfUphradesForGear(tag))) : 0);
				}
				text = text2 + "_icon" + num + "_big";
			}
			else
			{
				text = text2 + "_icon1_big";
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			return Resources.Load<Texture>("OfferIcons/" + text);
		}
		return null;
	}

	public static Texture GetItemIcon(string tag, ShopNGUIController.CategoryNames category, int? upgradeNum = null)
	{
		if (category == (ShopNGUIController.CategoryNames)(-1))
		{
			return null;
		}
		string text = null;
		string text2 = tag;
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			foreach (List<string> upgrade in WeaponUpgrades.upgrades)
			{
				int num = upgrade.IndexOf(tag);
				if (num != -1)
				{
					text2 = upgrade[0];
					if (!upgradeNum.HasValue)
					{
						upgradeNum = 1 + num;
					}
					break;
				}
			}
		}
		if (upgradeNum.HasValue)
		{
			int num2 = 1;
			if (ShopNGUIController.IsWeaponCategory(category))
			{
				ItemRecord byTag = GetByTag(tag);
				if ((byTag == null || !byTag.UseImagesFromFirstUpgrade) && (byTag == null || !byTag.TemporaryGun))
				{
					num2 = ((!upgradeNum.HasValue) ? 1 : upgradeNum.Value);
				}
			}
			text = text2 + "_icon" + num2 + "_big";
		}
		else
		{
			text = text2 + "_icon1_big";
		}
		if (!string.IsNullOrEmpty(text))
		{
			return Resources.Load<Texture>("OfferIcons/" + text);
		}
		return null;
	}

	public static Texture GetTextureItemByTag(string tag, int? upgradeNum = null)
	{
		int itemCategory = GetItemCategory(tag);
		return GetItemIcon(tag, (ShopNGUIController.CategoryNames)itemCategory, upgradeNum);
	}

	public static bool IsItemInInventory(string tag)
	{
		string key = tag;
		string storageIdByTag = GetStorageIdByTag(tag);
		if (!string.IsNullOrEmpty(storageIdByTag))
		{
			key = storageIdByTag;
		}
		if (!Storager.hasKey(key))
		{
			return false;
		}
		return Storager.getInt(key, true) == 1;
	}

	public static bool HasWeaponNeedUpgradesForBuyNext(string tag)
	{
		foreach (List<string> upgrade in WeaponUpgrades.upgrades)
		{
			int num = upgrade.IndexOf(tag);
			if (num != -1)
			{
				bool flag = true;
				for (int i = 0; i < num; i++)
				{
					flag = flag && IsItemInInventory(upgrade[i]);
				}
				return flag;
			}
		}
		return true;
	}

	public static string GetItemNameByTag(string tag)
	{
		int itemCategory = GetItemCategory(tag);
		return GetItemName(tag, (ShopNGUIController.CategoryNames)itemCategory);
	}

	public static WeaponSounds GetWeaponInfoByPrefabName(string prefabName)
	{
		if (prefabName != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + prefabName);
			return (!(weaponSounds != null)) ? null : weaponSounds;
		}
		return null;
	}

	public static WeaponSounds GetWeaponInfo(string weaponTag)
	{
		ItemRecord byTag = GetByTag(weaponTag);
		if (byTag == null)
		{
			return null;
		}
		return GetWeaponInfoByPrefabName(byTag.PrefabName);
	}

	public static Texture GetTextureForShopItem(string itemTag)
	{
		int value = 0;
		string text = itemTag;
		bool flag = GearManager.IsItemGear(itemTag);
		if (flag)
		{
			text = GearManager.HolderQuantityForID(itemTag);
			value = GearManager.CurrentNumberOfUphradesForGear(text) + 1;
		}
		if (flag && (text == GearManager.Turret || text == GearManager.Mech))
		{
			int? upgradeNum = value;
			if (upgradeNum.HasValue && upgradeNum.Value > 0)
			{
				return GetTextureItemByTag(text, upgradeNum);
			}
		}
		return GetTextureItemByTag(text);
	}

	[CompilerGenerated]
	private static bool _003CGetCanBuyWeapon_003Em__558(ItemRecord item)
	{
		return item.CanBuy;
	}

	[CompilerGenerated]
	private static bool _003CGetCanBuyWeapon_003Em__559(ItemRecord item)
	{
		return item.CanBuy && !item.Deactivated;
	}

	[CompilerGenerated]
	private static string _003CGetCanBuyWeaponTags_003Em__55A(ItemRecord item)
	{
		return item.Tag;
	}

	[CompilerGenerated]
	private static bool _003CGetCanBuyWeaponStorageIds_003Em__55B(ItemRecord item)
	{
		return item.StorageId != null;
	}

	[CompilerGenerated]
	private static string _003CGetCanBuyWeaponStorageIds_003Em__55C(ItemRecord item)
	{
		return item.StorageId;
	}
}
