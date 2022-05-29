using Rilisoft;
using System;
using UnityEngine;

[Serializable]
public class GiftInfo
{
	public string Id;

	public SaltedInt Count = new SaltedInt(12499947, 0);

	public float PercentAddInSlot;

	public string KeyTranslateInfo = string.Empty;

	[HideInInspector]
	public bool IsRandomSkin;

	[ReadOnly]
	public GiftInfo RootInfo;

	public ShopNGUIController.CategoryNames? TypeShopCat
	{
		get
		{
			ItemDb.GetByTag(this.Id);
			if (this.Id.IsNullOrEmpty())
			{
				return null;
			}
			int itemCategory = ItemDb.GetItemCategory(this.Id);
			if (itemCategory >= 0)
			{
				return new ShopNGUIController.CategoryNames?((ShopNGUIController.CategoryNames)itemCategory);
			}
			return null;
		}
	}

	public GiftInfo()
	{
	}

	public static GiftInfo CreateInfo(GiftInfo rootGift, string giftId, int count = 1)
	{
		GiftInfo giftInfo = new GiftInfo()
		{
			Count = count,
			Id = giftId,
			KeyTranslateInfo = rootGift.KeyTranslateInfo,
			PercentAddInSlot = rootGift.PercentAddInSlot,
			RootInfo = rootGift
		};
		return giftInfo;
	}
}