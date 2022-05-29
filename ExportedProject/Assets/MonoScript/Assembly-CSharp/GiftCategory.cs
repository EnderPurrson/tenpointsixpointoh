using Rilisoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class GiftCategory
{
	public GiftCategoryType Type;

	public int ScrollPosition;

	public string KeyTranslateInfoCommon = string.Empty;

	private readonly List<GiftInfo> _rootGifts = new List<GiftInfo>();

	private List<GiftInfo> _ag;

	private List<string> _availableRandomProducts;

	private List<GiftInfo> _allGifts
	{
		get
		{
			List<GiftInfo> giftInfos = this._ag;
			if (giftInfos == null)
			{
				List<GiftInfo> availableGifts = this.GetAvailableGifts();
				List<GiftInfo> giftInfos1 = availableGifts;
				this._ag = availableGifts;
				giftInfos = giftInfos1;
			}
			return giftInfos;
		}
		set
		{
			this._ag = value;
		}
	}

	private List<GiftInfo> _availableGifts
	{
		get
		{
			return (
				from g in this._allGifts
				where this.AvailableGift(g.Id, this.Type)
				select g).ToList<GiftInfo>();
		}
	}

	private float _availableGiftsPercentSum
	{
		get
		{
			return this._availableGifts.Sum<GiftInfo>((GiftInfo g) => g.PercentAddInSlot);
		}
	}

	public bool AnyGifts
	{
		get
		{
			return this._allGifts.Any<GiftInfo>();
		}
	}

	public int AvaliableGiftsCount
	{
		get
		{
			return this._availableGifts.Count;
		}
	}

	public float PercentChance
	{
		get
		{
			if (this.Type == GiftCategoryType.Guns_gray || this.Type == GiftCategoryType.Masks || this.Type == GiftCategoryType.Boots || this.Type == GiftCategoryType.Capes || this.Type == GiftCategoryType.Hats_random || this.Type == GiftCategoryType.ArmorAndHat)
			{
				return this._allGifts[0].PercentAddInSlot;
			}
			return this._allGifts.Sum<GiftInfo>((GiftInfo g) => g.PercentAddInSlot);
		}
	}

	public GiftCategory()
	{
	}

	public void AddGift(GiftInfo info)
	{
		this._rootGifts.Add(info);
	}

	public bool AvailableGift(string idGift, GiftCategoryType curType)
	{
		if (string.IsNullOrEmpty(idGift))
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
			{
				return true;
			}
			case GiftCategoryType.Skins:
			{
				bool flag = false;
				return !SkinsController.IsSkinBought(idGift, out flag);
			}
			case GiftCategoryType.ArmorAndHat:
			{
				return idGift == Wear.ArmorOrArmorHatAvailableForBuy(ShopNGUIController.CategoryNames.ArmorCategory);
			}
			case GiftCategoryType.Wear:
			{
				return !ItemDb.IsItemInInventory(idGift);
			}
			case GiftCategoryType.Editor:
			{
				if (idGift.IsNullOrEmpty() || idGift != "editor_Cape" && idGift != "editor_Skin")
				{
					return false;
				}
				if (idGift == "editor_Skin" && Storager.getInt(Defs.SkinsMakerInProfileBought, false) > 0)
				{
					return false;
				}
				if (idGift == "editor_Cape" && Storager.getInt("cape_Custom", false) > 0)
				{
					return false;
				}
				return true;
			}
			case GiftCategoryType.Masks:
			case GiftCategoryType.Boots:
			case GiftCategoryType.Hats_random:
			{
				return (idGift.IsNullOrEmpty() ? false : Storager.getInt(idGift, true) == 0);
			}
			case GiftCategoryType.Capes:
			{
				if (idGift != "cape_Custom")
				{
					return false;
				}
				return (idGift.IsNullOrEmpty() ? false : Storager.getInt(idGift, true) == 0);
			}
			case GiftCategoryType.Gun1:
			case GiftCategoryType.Gun2:
			case GiftCategoryType.Gun3:
			{
				return Storager.getInt(idGift, true) == 0;
			}
			case GiftCategoryType.Guns_gray:
			{
				if (idGift.IsNullOrEmpty())
				{
					return false;
				}
				ItemRecord itemRecord = GiftController.GrayCategoryWeapons[ExpController.OurTierForAnyPlace()].FirstOrDefault<ItemRecord>((ItemRecord rec) => rec.Tag == idGift);
				return (itemRecord == null ? false : Storager.getInt(itemRecord.StorageId, true) == 0);
			}
			case GiftCategoryType.Stickers:
			{
				TypePackSticker? @enum = idGift.ToEnum<TypePackSticker>(null);
				return (!@enum.HasValue ? false : !StickersController.IsBuyPack(@enum.Value));
			}
		}
		return false;
	}

	public void CheckGifts()
	{
		this._allGifts = this.GetAvailableGifts();
		foreach (GiftInfo _allGift in this._allGifts)
		{
			if (this.Type == GiftCategoryType.ArmorAndHat)
			{
				_allGift.Id = Wear.ArmorOrArmorHatAvailableForBuy(ShopNGUIController.CategoryNames.ArmorCategory);
			}
			if (this.Type != GiftCategoryType.Skins)
			{
				continue;
			}
			_allGift.IsRandomSkin = true;
			_allGift.Id = SkinsController.RandomUnboughtSkinId();
		}
	}

	private List<GiftInfo> GetAvailableGifts()
	{
		List<GiftInfo> giftInfos = new List<GiftInfo>();
		foreach (GiftInfo _rootGift in this._rootGifts)
		{
			if (_rootGift.Id == "guns_gray")
			{
				GiftController.GetAvailableGrayWeaponsTags().ForEach((string w) => giftInfos.Add(GiftInfo.CreateInfo(_rootGift, w, 1)));
			}
			else if (_rootGift.Id == "equip_Mask")
			{
				List<string> availableProducts = GiftCategory.GetAvailableProducts(ShopNGUIController.CategoryNames.MaskCategory, -1, null);
				availableProducts.ForEach((string p) => giftInfos.Add(GiftInfo.CreateInfo(_rootGift, p, 1)));
			}
			else if (_rootGift.Id == "equip_Cape")
			{
				List<string> strs = GiftCategory.GetAvailableProducts(ShopNGUIController.CategoryNames.CapesCategory, -1, new string[] { "cape_Custom" });
				strs.ForEach((string p) => giftInfos.Add(GiftInfo.CreateInfo(_rootGift, p, 1)));
			}
			else if (_rootGift.Id == "equip_Boots")
			{
				List<string> availableProducts1 = GiftCategory.GetAvailableProducts(ShopNGUIController.CategoryNames.BootsCategory, -1, null);
				availableProducts1.ForEach((string p) => giftInfos.Add(GiftInfo.CreateInfo(_rootGift, p, 1)));
			}
			else if (_rootGift.Id != "equip_Hat")
			{
				giftInfos.Add(_rootGift);
			}
			else
			{
				List<string> strs1 = GiftCategory.GetAvailableProducts(ShopNGUIController.CategoryNames.HatsCategory, -1, null);
				strs1.ForEach((string p) => giftInfos.Add(GiftInfo.CreateInfo(_rootGift, p, 1)));
			}
		}
		if (!FriendsController.SandboxEnabled)
		{
			giftInfos.RemoveAll((GiftInfo r) => r.Id == "LikeID");
		}
		return giftInfos;
	}

	private static List<string> GetAvailableProducts(ShopNGUIController.CategoryNames category, int maxTier = -1, string[] withoutIds = null)
	{
		if (maxTier < 0)
		{
			maxTier = ExpController.OurTierForAnyPlace();
		}
		List<string> strs = Wear.AllWears(category, maxTier, true, true);
		List<string> strs1 = PromoActionsGUIController.FilterPurchases(strs, true, false, false, true);
		if (withoutIds != null)
		{
			strs1.AddRange(withoutIds);
		}
		return strs.Except<string>(strs1).ToList<string>();
	}

	public GiftInfo GetRandomGift()
	{
		if (this._availableGifts == null || this._availableGifts.Count == 0)
		{
			return null;
		}
		if (this._availableGiftsPercentSum < 0f)
		{
			return null;
		}
		float single = UnityEngine.Random.Range(0f, this._availableGiftsPercentSum);
		float percentAddInSlot = 0f;
		GiftInfo giftInfo = null;
		int num = 0;
		while (num < this._availableGifts.Count)
		{
			GiftInfo item = this._availableGifts[num];
			percentAddInSlot += item.PercentAddInSlot;
			if (percentAddInSlot <= single)
			{
				num++;
			}
			else
			{
				giftInfo = item;
				break;
			}
		}
		return giftInfo;
	}
}