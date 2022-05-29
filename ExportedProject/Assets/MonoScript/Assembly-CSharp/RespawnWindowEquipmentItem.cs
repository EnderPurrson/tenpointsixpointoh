using System;
using UnityEngine;

public class RespawnWindowEquipmentItem : MonoBehaviour
{
	public UITexture itemImage;

	public UISprite emptyImage;

	[NonSerialized]
	public string itemTag;

	[NonSerialized]
	public int itemCategory;

	public RespawnWindowEquipmentItem()
	{
	}

	private static bool IsNoneEquipment(string itemTag)
	{
		return (string.IsNullOrEmpty(itemTag) || itemTag == Defs.HatNoneEqupped || itemTag == Defs.ArmorNewNoneEqupped || itemTag == Defs.CapeNoneEqupped || itemTag == Defs.BootsNoneEqupped ? true : itemTag == "MaskNoneEquipped");
	}

	public void SetItemTag(string itemTag, int itemCategory)
	{
		if (RespawnWindowEquipmentItem.IsNoneEquipment(itemTag))
		{
			this.itemImage.gameObject.SetActive(false);
			this.emptyImage.gameObject.SetActive(true);
			this.itemTag = null;
			this.itemCategory = -1;
			return;
		}
		this.itemImage.gameObject.SetActive(true);
		this.emptyImage.gameObject.SetActive(false);
		this.itemTag = itemTag;
		this.itemCategory = itemCategory;
		int? nullable = null;
		this.itemImage.mainTexture = ItemDb.GetItemIcon(itemTag, (ShopNGUIController.CategoryNames)itemCategory, nullable);
	}
}