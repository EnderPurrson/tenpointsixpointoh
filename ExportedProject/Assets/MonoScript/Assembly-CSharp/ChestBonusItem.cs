using System;
using UnityEngine;

public class ChestBonusItem : MonoBehaviour
{
	public UILabel timeLifeLabel;

	public UILabel itemNameLabel;

	public UITexture itemImageHolder;

	public ChestBonusItem()
	{
	}

	public void SetData(ChestBonusItemData itemData)
	{
		string empty = string.Empty;
		if (itemData.timeLife != -1)
		{
			empty = itemData.GetTimeLabel(false);
		}
		else
		{
			empty = (itemData.count <= 1 ? LocalizationStore.Get("Key_1059") : string.Format("{0} {1}", itemData.count, LocalizationStore.Get("Key_1230")));
		}
		this.timeLifeLabel.text = empty;
		this.itemImageHolder.mainTexture = ItemDb.GetTextureForShopItem(itemData.tag);
		this.itemNameLabel.text = ItemDb.GetItemNameByTag(itemData.tag);
	}

	public void SetVisible(bool visible)
	{
		base.gameObject.SetActive(visible);
	}
}