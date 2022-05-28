using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

[Serializable]
public class GiftCategory
{
	[CompilerGenerated]
	private sealed class _003CAvailableGift_003Ec__AnonStorey2B1
	{
		internal string idGift;

		internal bool _003C_003Em__2C6(ItemRecord rec)
		{
			return rec.Tag == idGift;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetAvailableGifts_003Ec__AnonStorey2B2
	{
		internal List<GiftInfo> res;
	}

	[CompilerGenerated]
	private sealed class _003CGetAvailableGifts_003Ec__AnonStorey2B3
	{
		internal GiftInfo root;

		internal _003CGetAvailableGifts_003Ec__AnonStorey2B2 _003C_003Ef__ref_0024690;

		internal void _003C_003Em__2C7(string w)
		{
			_003C_003Ef__ref_0024690.res.Add(GiftInfo.CreateInfo(root, w));
		}

		internal void _003C_003Em__2C8(string p)
		{
			_003C_003Ef__ref_0024690.res.Add(GiftInfo.CreateInfo(root, p));
		}

		internal void _003C_003Em__2C9(string p)
		{
			_003C_003Ef__ref_0024690.res.Add(GiftInfo.CreateInfo(root, p));
		}

		internal void _003C_003Em__2CA(string p)
		{
			_003C_003Ef__ref_0024690.res.Add(GiftInfo.CreateInfo(root, p));
		}

		internal void _003C_003Em__2CB(string p)
		{
			_003C_003Ef__ref_0024690.res.Add(GiftInfo.CreateInfo(root, p));
		}
	}

	public GiftCategoryType Type;

	public int ScrollPosition;

	public string KeyTranslateInfoCommon = string.Empty;

	private readonly List<GiftInfo> _rootGifts = new List<GiftInfo>();

	private List<GiftInfo> _ag;

	private List<string> _availableRandomProducts;

	[CompilerGenerated]
	private static Func<GiftInfo, float> _003C_003Ef__am_0024cache6;

	[CompilerGenerated]
	private static Func<GiftInfo, float> _003C_003Ef__am_0024cache7;

	[CompilerGenerated]
	private static Predicate<GiftInfo> _003C_003Ef__am_0024cache8;

	private List<GiftInfo> _allGifts
	{
		get
		{
			return _ag ?? (_ag = GetAvailableGifts());
		}
		set
		{
			_ag = value;
		}
	}

	public bool AnyGifts
	{
		get
		{
			return _allGifts.Any();
		}
	}

	public float PercentChance
	{
		get
		{
			if (Type == GiftCategoryType.Guns_gray || Type == GiftCategoryType.Masks || Type == GiftCategoryType.Boots || Type == GiftCategoryType.Capes || Type == GiftCategoryType.Hats_random || Type == GiftCategoryType.ArmorAndHat)
			{
				return _allGifts[0].PercentAddInSlot;
			}
			List<GiftInfo> allGifts = _allGifts;
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003Cget_PercentChance_003Em__2C3;
			}
			return allGifts.Sum(_003C_003Ef__am_0024cache6);
		}
	}

	private List<GiftInfo> _availableGifts
	{
		get
		{
			return _allGifts.Where(_003Cget__availableGifts_003Em__2C4).ToList();
		}
	}

	public int AvaliableGiftsCount
	{
		get
		{
			return _availableGifts.Count;
		}
	}

	private float _availableGiftsPercentSum
	{
		get
		{
			List<GiftInfo> availableGifts = _availableGifts;
			if (_003C_003Ef__am_0024cache7 == null)
			{
				_003C_003Ef__am_0024cache7 = _003Cget__availableGiftsPercentSum_003Em__2C5;
			}
			return availableGifts.Sum(_003C_003Ef__am_0024cache7);
		}
	}

	public void AddGift(GiftInfo info)
	{
		_rootGifts.Add(info);
	}

	public void CheckGifts()
	{
		_allGifts = GetAvailableGifts();
		foreach (GiftInfo allGift in _allGifts)
		{
			if (Type == GiftCategoryType.ArmorAndHat)
			{
				allGift.Id = Wear.ArmorOrArmorHatAvailableForBuy(ShopNGUIController.CategoryNames.ArmorCategory);
			}
			if (Type == GiftCategoryType.Skins)
			{
				allGift.IsRandomSkin = true;
				allGift.Id = SkinsController.RandomUnboughtSkinId();
			}
		}
	}

	public bool AvailableGift(string idGift, GiftCategoryType curType)
	{
		_003CAvailableGift_003Ec__AnonStorey2B1 _003CAvailableGift_003Ec__AnonStorey2B = new _003CAvailableGift_003Ec__AnonStorey2B1();
		_003CAvailableGift_003Ec__AnonStorey2B.idGift = idGift;
		if (string.IsNullOrEmpty(_003CAvailableGift_003Ec__AnonStorey2B.idGift))
		{
			return false;
		}
		switch (curType)
		{
		case GiftCategoryType.Coins:
		case GiftCategoryType.Gems:
		case GiftCategoryType.Grenades:
		case GiftCategoryType.Gear:
		case GiftCategoryType.Event_content:
		case GiftCategoryType.Freespins:
			return true;
		case GiftCategoryType.Guns_gray:
		{
			if (_003CAvailableGift_003Ec__AnonStorey2B.idGift.IsNullOrEmpty())
			{
				return false;
			}
			ItemRecord itemRecord = GiftController.GrayCategoryWeapons[ExpController.OurTierForAnyPlace()].FirstOrDefault(_003CAvailableGift_003Ec__AnonStorey2B._003C_003Em__2C6);
			return itemRecord != null && Storager.getInt(itemRecord.StorageId, true) == 0;
		}
		case GiftCategoryType.Gun1:
		case GiftCategoryType.Gun2:
		case GiftCategoryType.Gun3:
			return Storager.getInt(_003CAvailableGift_003Ec__AnonStorey2B.idGift, true) == 0;
		case GiftCategoryType.Wear:
			return !ItemDb.IsItemInInventory(_003CAvailableGift_003Ec__AnonStorey2B.idGift);
		case GiftCategoryType.ArmorAndHat:
		{
			string text = Wear.ArmorOrArmorHatAvailableForBuy(ShopNGUIController.CategoryNames.ArmorCategory);
			return _003CAvailableGift_003Ec__AnonStorey2B.idGift == text;
		}
		case GiftCategoryType.Skins:
		{
			bool isForMoneySkin = false;
			return !SkinsController.IsSkinBought(_003CAvailableGift_003Ec__AnonStorey2B.idGift, out isForMoneySkin);
		}
		case GiftCategoryType.Editor:
			if (_003CAvailableGift_003Ec__AnonStorey2B.idGift.IsNullOrEmpty() || (_003CAvailableGift_003Ec__AnonStorey2B.idGift != "editor_Cape" && _003CAvailableGift_003Ec__AnonStorey2B.idGift != "editor_Skin"))
			{
				return false;
			}
			if (_003CAvailableGift_003Ec__AnonStorey2B.idGift == "editor_Skin" && Storager.getInt(Defs.SkinsMakerInProfileBought, false) > 0)
			{
				return false;
			}
			if (_003CAvailableGift_003Ec__AnonStorey2B.idGift == "editor_Cape" && Storager.getInt("cape_Custom", false) > 0)
			{
				return false;
			}
			return true;
		case GiftCategoryType.Masks:
		case GiftCategoryType.Boots:
		case GiftCategoryType.Hats_random:
			return !_003CAvailableGift_003Ec__AnonStorey2B.idGift.IsNullOrEmpty() && Storager.getInt(_003CAvailableGift_003Ec__AnonStorey2B.idGift, true) == 0;
		case GiftCategoryType.Capes:
			if (_003CAvailableGift_003Ec__AnonStorey2B.idGift != "cape_Custom")
			{
				return false;
			}
			return !_003CAvailableGift_003Ec__AnonStorey2B.idGift.IsNullOrEmpty() && Storager.getInt(_003CAvailableGift_003Ec__AnonStorey2B.idGift, true) == 0;
		case GiftCategoryType.Stickers:
		{
			TypePackSticker? typePackSticker = _003CAvailableGift_003Ec__AnonStorey2B.idGift.ToEnum<TypePackSticker>();
			return typePackSticker.HasValue && !StickersController.IsBuyPack(typePackSticker.Value);
		}
		default:
			return false;
		}
	}

	private static List<string> GetAvailableProducts(ShopNGUIController.CategoryNames category, int maxTier = -1, string[] withoutIds = null)
	{
		if (maxTier < 0)
		{
			maxTier = ExpController.OurTierForAnyPlace();
		}
		List<string> list = Wear.AllWears(category, maxTier, true, true);
		List<string> list2 = PromoActionsGUIController.FilterPurchases(list, true, false);
		if (withoutIds != null)
		{
			list2.AddRange(withoutIds);
		}
		return list.Except(list2).ToList();
	}

	public GiftInfo GetRandomGift()
	{
		if (_availableGifts == null || _availableGifts.Count == 0)
		{
			return null;
		}
		if (_availableGiftsPercentSum < 0f)
		{
			return null;
		}
		float num = UnityEngine.Random.Range(0f, _availableGiftsPercentSum);
		float num2 = 0f;
		GiftInfo result = null;
		for (int i = 0; i < _availableGifts.Count; i++)
		{
			GiftInfo giftInfo = _availableGifts[i];
			num2 += giftInfo.PercentAddInSlot;
			if (num2 > num)
			{
				result = giftInfo;
				break;
			}
		}
		return result;
	}

	private List<GiftInfo> GetAvailableGifts()
	{
		_003CGetAvailableGifts_003Ec__AnonStorey2B2 _003CGetAvailableGifts_003Ec__AnonStorey2B = new _003CGetAvailableGifts_003Ec__AnonStorey2B2();
		_003CGetAvailableGifts_003Ec__AnonStorey2B.res = new List<GiftInfo>();
		_003CGetAvailableGifts_003Ec__AnonStorey2B3 _003CGetAvailableGifts_003Ec__AnonStorey2B2 = new _003CGetAvailableGifts_003Ec__AnonStorey2B3();
		_003CGetAvailableGifts_003Ec__AnonStorey2B2._003C_003Ef__ref_0024690 = _003CGetAvailableGifts_003Ec__AnonStorey2B;
		foreach (GiftInfo rootGift in _rootGifts)
		{
			_003CGetAvailableGifts_003Ec__AnonStorey2B2.root = rootGift;
			if (_003CGetAvailableGifts_003Ec__AnonStorey2B2.root.Id == "guns_gray")
			{
				List<string> availableGrayWeaponsTags = GiftController.GetAvailableGrayWeaponsTags();
				availableGrayWeaponsTags.ForEach(_003CGetAvailableGifts_003Ec__AnonStorey2B2._003C_003Em__2C7);
			}
			else if (_003CGetAvailableGifts_003Ec__AnonStorey2B2.root.Id == "equip_Mask")
			{
				List<string> availableProducts = GetAvailableProducts(ShopNGUIController.CategoryNames.MaskCategory);
				availableProducts.ForEach(_003CGetAvailableGifts_003Ec__AnonStorey2B2._003C_003Em__2C8);
			}
			else if (_003CGetAvailableGifts_003Ec__AnonStorey2B2.root.Id == "equip_Cape")
			{
				List<string> availableProducts2 = GetAvailableProducts(ShopNGUIController.CategoryNames.CapesCategory, -1, new string[1] { "cape_Custom" });
				availableProducts2.ForEach(_003CGetAvailableGifts_003Ec__AnonStorey2B2._003C_003Em__2C9);
			}
			else if (_003CGetAvailableGifts_003Ec__AnonStorey2B2.root.Id == "equip_Boots")
			{
				List<string> availableProducts3 = GetAvailableProducts(ShopNGUIController.CategoryNames.BootsCategory);
				availableProducts3.ForEach(_003CGetAvailableGifts_003Ec__AnonStorey2B2._003C_003Em__2CA);
			}
			else if (_003CGetAvailableGifts_003Ec__AnonStorey2B2.root.Id == "equip_Hat")
			{
				List<string> availableProducts4 = GetAvailableProducts(ShopNGUIController.CategoryNames.HatsCategory);
				availableProducts4.ForEach(_003CGetAvailableGifts_003Ec__AnonStorey2B2._003C_003Em__2CB);
			}
			else
			{
				_003CGetAvailableGifts_003Ec__AnonStorey2B.res.Add(_003CGetAvailableGifts_003Ec__AnonStorey2B2.root);
			}
		}
		if (!FriendsController.SandboxEnabled)
		{
			List<GiftInfo> res = _003CGetAvailableGifts_003Ec__AnonStorey2B.res;
			if (_003C_003Ef__am_0024cache8 == null)
			{
				_003C_003Ef__am_0024cache8 = _003CGetAvailableGifts_003Em__2CC;
			}
			res.RemoveAll(_003C_003Ef__am_0024cache8);
		}
		return _003CGetAvailableGifts_003Ec__AnonStorey2B.res;
	}

	[CompilerGenerated]
	private static float _003Cget_PercentChance_003Em__2C3(GiftInfo g)
	{
		return g.PercentAddInSlot;
	}

	[CompilerGenerated]
	private bool _003Cget__availableGifts_003Em__2C4(GiftInfo g)
	{
		return AvailableGift(g.Id, Type);
	}

	[CompilerGenerated]
	private static float _003Cget__availableGiftsPercentSum_003Em__2C5(GiftInfo g)
	{
		return g.PercentAddInSlot;
	}

	[CompilerGenerated]
	private static bool _003CGetAvailableGifts_003Em__2CC(GiftInfo r)
	{
		return r.Id == "LikeID";
	}
}
