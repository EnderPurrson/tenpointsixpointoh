using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestBonusData
{
	public string linkKey;

	public bool isVisible;

	public List<ChestBonusItemData> items;

	public ChestBonusData()
	{
	}

	public Texture GetImage()
	{
		if (this.items == null || this.items.Count == 0)
		{
			return null;
		}
		string empty = string.Empty;
		if (this.items.Count != 1)
		{
			return Resources.Load<Texture>("Textures/Bank/StarterPack_Weapon");
		}
		return ItemDb.GetTextureForShopItem(this.items[0].tag);
	}

	public string GetItemCountOrTime()
	{
		if (this.items == null || this.items.Count == 0)
		{
			return string.Empty;
		}
		if (this.items.Count != 1)
		{
			return string.Empty;
		}
		ChestBonusItemData item = this.items[0];
		int num = item.timeLife / 24;
		return (item.timeLife != -1 ? item.GetTimeLabel(true) : item.count.ToString());
	}
}