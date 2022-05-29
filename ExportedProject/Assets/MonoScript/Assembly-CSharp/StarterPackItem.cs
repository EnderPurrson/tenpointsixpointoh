using System;
using UnityEngine;

public class StarterPackItem : MonoBehaviour
{
	public UITexture imageItem;

	public UILabel nameItem;

	public UILabel countItems;

	public UILabel realPriceItem;

	public StarterPackItem()
	{
	}

	public void SetData(StarterPackItemData itemData)
	{
		bool flag = itemData.count > 1;
		this.countItems.gameObject.SetActive(flag);
		if (flag)
		{
			this.countItems.text = itemData.count.ToString();
		}
		string str = itemData.validTag;
		int num = 0;
		string str1 = str;
		bool flag1 = GearManager.IsItemGear(str);
		if (flag1)
		{
			str1 = GearManager.HolderQuantityForID(str);
			num = GearManager.CurrentNumberOfUphradesForGear(str1);
		}
		if (!flag1 || !(str1 == GearManager.Turret) && !(str1 == GearManager.Mech))
		{
			this.imageItem.mainTexture = ItemDb.GetTextureItemByTag(str1, null);
		}
		else
		{
			int? nullable = new int?(num);
			this.imageItem.mainTexture = ItemDb.GetTextureItemByTag(str1, nullable);
		}
		this.nameItem.text = ItemDb.GetItemNameByTag(str);
		string shopIdByTag = ItemDb.GetShopIdByTag(str);
		if (string.IsNullOrEmpty(shopIdByTag))
		{
			shopIdByTag = (!flag1 ? str : GearManager.OneItemIDForGear(str1, num));
		}
		ItemPrice priceByShopId = ItemDb.GetPriceByShopId(shopIdByTag);
		if (priceByShopId != null)
		{
			int price = priceByShopId.Price * itemData.count;
			string str2 = (priceByShopId.Currency != "Coins" ? LocalizationStore.Get("Key_0771") : LocalizationStore.Get("Key_0936"));
			this.realPriceItem.text = string.Format("{0} {1}", price, str2);
		}
	}
}