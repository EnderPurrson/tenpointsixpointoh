using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ItemDb
{
	public const int CrystalCrossbowCoinsPrice = 150;

	private static List<ItemRecord> _records;

	private static Dictionary<int, ItemRecord> _recordsById;

	private static Dictionary<string, ItemRecord> _recordsByTag;

	private static Dictionary<string, ItemRecord> _recordsByStorageId;

	private static Dictionary<string, ItemRecord> _recordsByShopId;

	private static Dictionary<string, ItemRecord> _recordsByPrefabName;

	public static List<ItemRecord> allRecords
	{
		get
		{
			return ItemDb._records;
		}
	}

	static ItemDb()
	{
		ItemDb._records = ItemDbRecords.GetRecords();
		ItemDb._recordsById = new Dictionary<int, ItemRecord>(ItemDb._records.Count);
		ItemDb._recordsByTag = new Dictionary<string, ItemRecord>(ItemDb._records.Count);
		ItemDb._recordsByStorageId = new Dictionary<string, ItemRecord>(ItemDb._records.Count);
		ItemDb._recordsByShopId = new Dictionary<string, ItemRecord>(ItemDb._records.Count);
		ItemDb._recordsByPrefabName = new Dictionary<string, ItemRecord>(ItemDb._records.Count);
		for (int i = 0; i < ItemDb._records.Count; i++)
		{
			ItemRecord item = ItemDb._records[i];
			ItemDb._recordsById[item.Id] = item;
			if (!string.IsNullOrEmpty(item.Tag))
			{
				ItemDb._recordsByTag[item.Tag] = item;
			}
			if (!string.IsNullOrEmpty(item.StorageId))
			{
				ItemDb._recordsByStorageId[item.StorageId] = item;
			}
			if (!string.IsNullOrEmpty(item.ShopId))
			{
				ItemDb._recordsByShopId[item.ShopId] = item;
			}
			if (!string.IsNullOrEmpty(item.PrefabName))
			{
				ItemDb._recordsByPrefabName[item.PrefabName] = item;
			}
		}
	}

	public static void Fill_storeIDtoDefsSNMapping(Dictionary<string, string> storeIDtoDefsSNMapping)
	{
		storeIDtoDefsSNMapping.Clear();
		foreach (KeyValuePair<string, ItemRecord> storageId in ItemDb._recordsByShopId)
		{
			if (string.IsNullOrEmpty(storageId.Value.StorageId))
			{
				continue;
			}
			storeIDtoDefsSNMapping[storageId.Key] = storageId.Value.StorageId;
		}
	}

	public static void Fill_tagToStoreIDMapping(Dictionary<string, string> tagToStoreIDMapping)
	{
		tagToStoreIDMapping.Clear();
		foreach (KeyValuePair<string, ItemRecord> shopId in ItemDb._recordsByTag)
		{
			if (string.IsNullOrEmpty(shopId.Value.ShopId))
			{
				continue;
			}
			tagToStoreIDMapping[shopId.Key] = shopId.Value.ShopId;
		}
	}

	public static ItemRecord GetById(int id)
	{
		if (!ItemDb._recordsById.ContainsKey(id))
		{
			return null;
		}
		return ItemDb._recordsById[id];
	}

	public static ItemRecord GetByPrefabName(string prefabName)
	{
		ItemRecord itemRecord;
		if (prefabName == null)
		{
			return null;
		}
		if (ItemDb._recordsByPrefabName.TryGetValue(prefabName, out itemRecord))
		{
			return itemRecord;
		}
		return null;
	}

	public static ItemRecord GetByShopId(string shopId)
	{
		ItemRecord itemRecord;
		if (shopId == null)
		{
			return null;
		}
		if (ItemDb._recordsByShopId.TryGetValue(shopId, out itemRecord))
		{
			return itemRecord;
		}
		return null;
	}

	public static ItemRecord GetByTag(string tag)
	{
		ItemRecord itemRecord;
		if (tag == null)
		{
			return null;
		}
		if (ItemDb._recordsByTag.TryGetValue(tag, out itemRecord))
		{
			return itemRecord;
		}
		return null;
	}

	public static IEnumerable<ItemRecord> GetCanBuyWeapon(bool includeDeactivated = false)
	{
		if (includeDeactivated)
		{
			return 
				from item in ItemDb._records
				where item.CanBuy
				select item;
		}
		return 
			from item in ItemDb._records
			where (!item.CanBuy ? false : !item.Deactivated)
			select item;
	}

	public static List<string> GetCanBuyWeaponStorageIds(bool includeDeactivated = false)
	{
		return (
			from item in ItemDb.GetCanBuyWeapon(includeDeactivated)
			where item.StorageId != null
			select item.StorageId).ToList<string>();
	}

	public static string[] GetCanBuyWeaponTags(bool includeDeactivated = false)
	{
		return (
			from item in ItemDb.GetCanBuyWeapon(includeDeactivated)
			select item.Tag).ToArray<string>();
	}

	public static ShopPositionParams GetInfoForNonWeaponItem(string name, ShopNGUIController.CategoryNames category)
	{
		ShopPositionParams shopPositionParam = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			shopPositionParam = Resources.Load<ShopPositionParams>(string.Concat("Armor_Info/", name));
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			shopPositionParam = Resources.Load<ShopPositionParams>(string.Concat("Hats_Info/", name));
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			shopPositionParam = Resources.Load<ShopPositionParams>(string.Concat("Capes_Info/", name));
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			shopPositionParam = Resources.Load<ShopPositionParams>(string.Concat("Shop_Boots_Info/", name));
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			shopPositionParam = Resources.Load<ShopPositionParams>(string.Concat("Shop_Gear/", name));
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			shopPositionParam = Resources.Load<ShopPositionParams>(string.Concat("Masks_Info/", name));
		}
		return shopPositionParam;
	}

	public static int GetItemCategory(string tag)
	{
		int key = -1;
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag != null)
		{
			WeaponSounds weaponSound = Resources.Load<WeaponSounds>(string.Concat("Weapons/", byTag.PrefabName));
			return (weaponSound == null ? -1 : weaponSound.categoryNabor - 1);
		}
		if (key == -1)
		{
			string str = tag;
			bool flag = false;
			foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
			{
				foreach (List<string> value in keyValuePair.Value)
				{
					if (!value.Contains(str))
					{
						continue;
					}
					flag = true;
					key = (int)keyValuePair.Key;
					break;
				}
				if (!flag)
				{
					continue;
				}
				break;
			}
		}
		if (key == -1 && (SkinsController.skinsNamesForPers.ContainsKey(tag) || tag.Equals("CustomSkinID")))
		{
			key = 8;
		}
		if (key == -1 && GearManager.IsItemGear(tag))
		{
			key = 11;
		}
		return key;
	}

	public static int[] GetItemFilterMap(string tag)
	{
		int[] numArray;
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag == null)
		{
			return new int[0];
		}
		WeaponSounds weaponSound = Resources.Load<WeaponSounds>(string.Concat("Weapons/", byTag.PrefabName));
		if (weaponSound == null)
		{
			numArray = new int[0];
		}
		else
		{
			numArray = (weaponSound.filterMap == null ? new int[0] : weaponSound.filterMap);
		}
		return numArray;
	}

	public static Texture GetItemIcon(string tag, ShopNGUIController.CategoryNames category, bool withUpdates)
	{
		bool flag;
		int num;
		if (category == (ShopNGUIController.CategoryNames.BackupCategory | ShopNGUIController.CategoryNames.MeleeCategory | ShopNGUIController.CategoryNames.SpecilCategory | ShopNGUIController.CategoryNames.SniperCategory | ShopNGUIController.CategoryNames.PremiumCategory | ShopNGUIController.CategoryNames.HatsCategory | ShopNGUIController.CategoryNames.ArmorCategory | ShopNGUIController.CategoryNames.SkinsCategory | ShopNGUIController.CategoryNames.CapesCategory | ShopNGUIController.CategoryNames.BootsCategory | ShopNGUIController.CategoryNames.GearCategory | ShopNGUIController.CategoryNames.MaskCategory))
		{
			return null;
		}
		string str = null;
		string item = tag;
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			foreach (List<string> upgrade in WeaponUpgrades.upgrades)
			{
				if (!upgrade.Contains(tag))
				{
					continue;
				}
				item = upgrade[0];
				break;
			}
		}
		if (item != null)
		{
			if (!withUpdates)
			{
				str = string.Concat(item, "_icon1_big");
			}
			else
			{
				int num1 = 1;
				if (ShopNGUIController.IsWeaponCategory(category))
				{
					ItemRecord byTag = ItemDb.GetByTag(tag);
					if ((byTag == null || !byTag.UseImagesFromFirstUpgrade) && (byTag == null || !byTag.TemporaryGun))
					{
						num1 = 1 + ShopNGUIController._CurrentNumberOfUpgrades(item, out flag, category, true);
					}
				}
				else if (category == ShopNGUIController.CategoryNames.GearCategory)
				{
					if (!Defs.isDaterRegim)
					{
						num = (tag == GearManager.Grenade ? 1 : 1 + GearManager.CurrentNumberOfUphradesForGear(tag));
					}
					else
					{
						num = 0;
					}
					num1 = num;
				}
				str = string.Concat(item, "_icon", num1.ToString(), "_big");
			}
		}
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		return Resources.Load<Texture>(string.Concat("OfferIcons/", str));
	}

	public static Texture GetItemIcon(string tag, ShopNGUIController.CategoryNames category, int? upgradeNum = null)
	{
		if (category == (ShopNGUIController.CategoryNames.BackupCategory | ShopNGUIController.CategoryNames.MeleeCategory | ShopNGUIController.CategoryNames.SpecilCategory | ShopNGUIController.CategoryNames.SniperCategory | ShopNGUIController.CategoryNames.PremiumCategory | ShopNGUIController.CategoryNames.HatsCategory | ShopNGUIController.CategoryNames.ArmorCategory | ShopNGUIController.CategoryNames.SkinsCategory | ShopNGUIController.CategoryNames.CapesCategory | ShopNGUIController.CategoryNames.BootsCategory | ShopNGUIController.CategoryNames.GearCategory | ShopNGUIController.CategoryNames.MaskCategory))
		{
			return null;
		}
		string str = null;
		string item = tag;
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			foreach (List<string> upgrade in WeaponUpgrades.upgrades)
			{
				int num = upgrade.IndexOf(tag);
				if (num == -1)
				{
					continue;
				}
				item = upgrade[0];
				if (!upgradeNum.HasValue)
				{
					upgradeNum = new int?(1 + num);
				}
				break;
			}
		}
		if (!upgradeNum.HasValue)
		{
			str = string.Concat(item, "_icon1_big");
		}
		else
		{
			int num1 = 1;
			if (ShopNGUIController.IsWeaponCategory(category))
			{
				ItemRecord byTag = ItemDb.GetByTag(tag);
				if ((byTag == null || !byTag.UseImagesFromFirstUpgrade) && (byTag == null || !byTag.TemporaryGun))
				{
					num1 = (!upgradeNum.HasValue ? 1 : upgradeNum.Value);
				}
			}
			str = string.Concat(item, "_icon", num1.ToString(), "_big");
		}
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		return Resources.Load<Texture>(string.Concat("OfferIcons/", str));
	}

	public static string GetItemName(string tag, ShopNGUIController.CategoryNames category)
	{
		if (string.IsNullOrEmpty(tag))
		{
			return string.Empty;
		}
		ShopPositionParams infoForNonWeaponItem = null;
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag == null)
		{
			infoForNonWeaponItem = ItemDb.GetInfoForNonWeaponItem(tag, category);
			if (infoForNonWeaponItem == null)
			{
				return string.Empty;
			}
			return infoForNonWeaponItem.shopName;
		}
		WeaponSounds weaponSound = Resources.Load<WeaponSounds>(string.Concat("Weapons/", byTag.PrefabName));
		string empty = string.Empty;
		if (weaponSound != null)
		{
			try
			{
				ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponSound.name.Replace("(Clone)", string.Empty));
				string str = WeaponUpgrades.TagOfFirstUpgrade(byPrefabName.Tag);
				ItemRecord itemRecord = ItemDb.GetByTag(str);
				empty = WeaponManager.AllWrapperPrefabs().First<WeaponSounds>((WeaponSounds weapon) => weapon.name == itemRecord.PrefabName).shopName;
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Error in getting shop name of first upgrade: ", exception));
				empty = weaponSound.shopName;
			}
		}
		return empty;
	}

	public static string GetItemNameByTag(string tag)
	{
		return ItemDb.GetItemName(tag, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(tag));
	}

	public static string GetItemNameNonLocalized(string tag, string shopId, ShopNGUIController.CategoryNames category, string defaultDesc = null)
	{
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag != null)
		{
			WeaponSounds weaponSound = Resources.Load<WeaponSounds>(string.Concat("Weapons/", byTag.PrefabName));
			return (weaponSound == null ? string.Empty : weaponSound.shopNameNonLocalized);
		}
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			return Resources.Load<ShopPositionParams>(string.Concat("Armor_Info/", tag)).shopNameNonLocalized;
		}
		if (category != ShopNGUIController.CategoryNames.HatsCategory)
		{
			if (InAppData.inappReadableNames.ContainsKey(shopId))
			{
				return InAppData.inappReadableNames[shopId];
			}
			return defaultDesc ?? shopId;
		}
		ShopPositionParams shopPositionParam = Resources.Load<ShopPositionParams>(string.Concat("Hats_Info/", tag));
		return (shopPositionParam == null ? string.Empty : shopPositionParam.shopNameNonLocalized);
	}

	public static ItemPrice GetPriceByShopId(string shopId)
	{
		ItemRecord itemRecord;
		if (shopId == null)
		{
			return null;
		}
		if (ItemDb._recordsByShopId.TryGetValue(shopId, out itemRecord))
		{
			return itemRecord.Price;
		}
		return VirtualCurrencyHelper.Price(shopId);
	}

	public static string GetShopIdByTag(string tag)
	{
		string shopId;
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag == null)
		{
			shopId = null;
		}
		else
		{
			shopId = byTag.ShopId;
		}
		return shopId;
	}

	public static string GetStorageIdByShopId(string shopId)
	{
		string storageId;
		ItemRecord byShopId = ItemDb.GetByShopId(shopId);
		if (byShopId == null)
		{
			storageId = null;
		}
		else
		{
			storageId = byShopId.StorageId;
		}
		return storageId;
	}

	public static string GetStorageIdByTag(string tag)
	{
		string storageId;
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag == null)
		{
			storageId = null;
		}
		else
		{
			storageId = byTag.StorageId;
		}
		return storageId;
	}

	public static string GetTagByShopId(string shopId)
	{
		string tag;
		ItemRecord byShopId = ItemDb.GetByShopId(shopId);
		if (byShopId == null)
		{
			tag = null;
		}
		else
		{
			tag = byShopId.Tag;
		}
		return tag;
	}

	public static Texture GetTextureForShopItem(string itemTag)
	{
		int num = 0;
		string str = itemTag;
		bool flag = GearManager.IsItemGear(itemTag);
		if (flag)
		{
			str = GearManager.HolderQuantityForID(itemTag);
			num = GearManager.CurrentNumberOfUphradesForGear(str) + 1;
		}
		if (flag && (str == GearManager.Turret || str == GearManager.Mech))
		{
			int? nullable = new int?(num);
			if ((!nullable.HasValue ? false : nullable.Value > 0))
			{
				return ItemDb.GetTextureItemByTag(str, nullable);
			}
		}
		return ItemDb.GetTextureItemByTag(str, null);
	}

	public static Texture GetTextureItemByTag(string tag, int? upgradeNum = null)
	{
		return ItemDb.GetItemIcon(tag, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(tag), upgradeNum);
	}

	public static WeaponSounds GetWeaponInfo(string weaponTag)
	{
		ItemRecord byTag = ItemDb.GetByTag(weaponTag);
		if (byTag == null)
		{
			return null;
		}
		return ItemDb.GetWeaponInfoByPrefabName(byTag.PrefabName);
	}

	public static WeaponSounds GetWeaponInfoByPrefabName(string prefabName)
	{
		WeaponSounds weaponSound;
		if (prefabName == null)
		{
			return null;
		}
		WeaponSounds weaponSound1 = Resources.Load<WeaponSounds>(string.Concat("Weapons/", prefabName));
		if (weaponSound1 == null)
		{
			weaponSound = null;
		}
		else
		{
			weaponSound = weaponSound1;
		}
		return weaponSound;
	}

	public static GameObject GetWearFromResources(string name, ShopNGUIController.CategoryNames category)
	{
		GameObject gameObject = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			gameObject = Resources.Load<GameObject>(string.Concat("Armor/", name));
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			gameObject = Resources.Load<GameObject>(string.Concat("Hats/", name));
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			gameObject = Resources.Load<GameObject>(string.Concat("Capes/", name));
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			gameObject = Resources.Load<GameObject>(string.Concat("Shop_Boots/", name));
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			gameObject = Resources.Load<GameObject>(string.Concat("Shop_Gear/", name));
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			gameObject = Resources.Load<GameObject>(string.Concat("Masks/", name));
		}
		return gameObject;
	}

	public static ResourceRequest GetWearFromResourcesAsync(string name, ShopNGUIController.CategoryNames category)
	{
		ResourceRequest resourceRequest = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			resourceRequest = Resources.LoadAsync<GameObject>(string.Concat("Armor/", name));
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			resourceRequest = Resources.LoadAsync<GameObject>(string.Concat("Hats/", name));
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			resourceRequest = Resources.LoadAsync<GameObject>(string.Concat("Capes/", name));
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			resourceRequest = Resources.LoadAsync<GameObject>(string.Concat("Shop_Boots/", name));
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			resourceRequest = Resources.LoadAsync<GameObject>(string.Concat("Shop_Gear/", name));
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			resourceRequest = Resources.LoadAsync<GameObject>(string.Concat("Masks/", name));
		}
		return resourceRequest;
	}

	public static bool HasWeaponNeedUpgradesForBuyNext(string tag)
	{
		bool flag;
		List<List<string>>.Enumerator enumerator = WeaponUpgrades.upgrades.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				List<string> current = enumerator.Current;
				int num = current.IndexOf(tag);
				if (num == -1)
				{
					continue;
				}
				bool flag1 = true;
				for (int i = 0; i < num; i++)
				{
					flag1 = (!flag1 ? false : ItemDb.IsItemInInventory(current[i]));
				}
				flag = flag1;
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

	public static void InitStorageForTag(string tag)
	{
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag == null)
		{
			Debug.LogWarning(string.Concat("item didn't found for tag: ", tag));
			return;
		}
		if (string.IsNullOrEmpty(byTag.StorageId))
		{
			Debug.LogWarning(string.Concat("StoragId is null or empty for tag: ", tag));
			return;
		}
		Storager.setInt(byTag.StorageId, 0, false);
	}

	public static bool IsCanBuy(string tag)
	{
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag == null)
		{
			return false;
		}
		return byTag.CanBuy;
	}

	public static bool IsItemInInventory(string tag)
	{
		string str = tag;
		string storageIdByTag = ItemDb.GetStorageIdByTag(tag);
		if (!string.IsNullOrEmpty(storageIdByTag))
		{
			str = storageIdByTag;
		}
		if (!Storager.hasKey(str))
		{
			return false;
		}
		return Storager.getInt(str, true) == 1;
	}

	public static bool IsTemporaryGun(string tg)
	{
		if (tg == null)
		{
			return false;
		}
		ItemRecord byTag = ItemDb.GetByTag(tg);
		return (byTag == null ? false : byTag.TemporaryGun);
	}

	public static bool IsWeaponCanDrop(string tag)
	{
		if (tag == "Knife")
		{
			return false;
		}
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag == null)
		{
			return false;
		}
		return !byTag.CanBuy;
	}
}