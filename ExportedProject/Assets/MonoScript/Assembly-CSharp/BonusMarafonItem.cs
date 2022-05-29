using Rilisoft;
using System;
using UnityEngine;

public sealed class BonusMarafonItem
{
	private const string PathToBonusesIcons = "OfferIcons/Marathon/";

	public BonusItemType type;

	public SaltedInt count;

	public string iconPreviewFileName;

	public string tag;

	public BonusMarafonItem(BonusItemType elementType, int countElements, string iconPreviewName, string tagWeapon = null)
	{
		this.type = elementType;
		this.count = countElements;
		this.iconPreviewFileName = iconPreviewName;
		this.tag = tagWeapon;
	}

	public Texture2D GetIcon()
	{
		if (this.type != BonusItemType.TemporaryWeapon)
		{
			return Resources.Load<Texture2D>(string.Concat("OfferIcons/Marathon/", this.iconPreviewFileName));
		}
		ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(this.tag);
		return ItemDb.GetItemIcon(this.tag, itemCategory, false) as Texture2D;
	}

	public string GetLongDescription()
	{
		string empty = string.Empty;
		switch (this.type)
		{
			case BonusItemType.PotionInvisible:
			{
				empty = LocalizationStore.Get("Key_0851");
				return empty;
			}
			case BonusItemType.JetPack:
			{
				empty = LocalizationStore.Get("Key_0850");
				return empty;
			}
			case BonusItemType.Granade:
			{
				return empty;
			}
			case BonusItemType.Turret:
			{
				empty = LocalizationStore.Get("Key_0852");
				return empty;
			}
			case BonusItemType.Mech:
			{
				empty = LocalizationStore.Get("Key_0849");
				return empty;
			}
			case BonusItemType.TemporaryWeapon:
			{
				empty = LocalizationStore.Get("Key_1200");
				return empty;
			}
			default:
			{
				return empty;
			}
		}
	}

	public string GetShortDescription()
	{
		string empty = string.Empty;
		switch (this.type)
		{
			case BonusItemType.Gold:
			{
				empty = LocalizationStore.Get("Key_0936");
				break;
			}
			case BonusItemType.Real:
			{
				empty = LocalizationStore.Get("Key_0771");
				break;
			}
			case BonusItemType.PotionInvisible:
			{
				empty = LocalizationStore.Get("Key_0775");
				break;
			}
			case BonusItemType.JetPack:
			{
				empty = LocalizationStore.Get("Key_0772");
				break;
			}
			case BonusItemType.Granade:
			{
				empty = LocalizationStore.Get("Key_0776");
				break;
			}
			case BonusItemType.Turret:
			{
				empty = LocalizationStore.Get("Key_0773");
				break;
			}
			case BonusItemType.Mech:
			{
				empty = LocalizationStore.Get("Key_0774");
				break;
			}
			case BonusItemType.TemporaryWeapon:
			{
				empty = ItemDb.GetItemNameByTag(this.tag);
				break;
			}
		}
		if (string.IsNullOrEmpty(empty) || this.count.Value == 0)
		{
			return string.Empty;
		}
		if (this.count.Value <= 1)
		{
			return empty;
		}
		return string.Format("{0} {1}", this.count.Value, empty);
	}
}