using System;
using System.Collections.Generic;

public class StarterPackItemData
{
	public List<string> variantTags = new List<string>();

	public int count;

	public string validTag
	{
		get
		{
			for (int i = 0; i < this.variantTags.Count; i++)
			{
				if (!ItemDb.IsItemInInventory(this.variantTags[i]) && !this.IsInvalidArmorTag(this.variantTags[i]))
				{
					return this.variantTags[i];
				}
			}
			return string.Empty;
		}
	}

	public StarterPackItemData()
	{
	}

	private bool IsInvalidArmorTag(string tag)
	{
		List<string> strs = null;
		if (Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Contains(tag))
		{
			strs = new List<string>();
			strs.AddRange(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0]);
		}
		else if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(tag))
		{
			strs = new List<string>();
			strs.AddRange(Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0]);
		}
		if (strs == null)
		{
			return false;
		}
		foreach (string str in PromoActionsGUIController.FilterPurchases(strs, true, true, false, true))
		{
			strs.Remove(str);
		}
		if (strs.Count == 0)
		{
			return true;
		}
		return strs[0] != tag;
	}
}