using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestBonusView : MonoBehaviour
{
	public UILabel[] title;

	public UILabel description;

	public ChestBonusItem[] bonusItems;

	public float cellWidth;

	public float startXPos;

	public ChestBonusView()
	{
	}

	private void CenterItems(int countHideElements)
	{
		float single = (float)countHideElements / 2f;
		float single1 = this.cellWidth * single;
		int length = (int)this.bonusItems.Length - countHideElements;
		for (int i = 0; i < length; i++)
		{
			Vector3 vector3 = this.bonusItems[i].transform.localPosition;
			float single2 = this.startXPos + single1 + this.cellWidth * (float)i;
			this.bonusItems[i].transform.localPosition = new Vector3(single2, vector3.y, vector3.z);
		}
	}

	private void CreateBonusesItemsAndAlign(ChestBonusData bonus)
	{
		int num = 0;
		for (int i = 0; i < (int)this.bonusItems.Length; i++)
		{
			if (i < bonus.items.Count)
			{
				this.bonusItems[i].SetVisible(true);
				this.bonusItems[i].SetData(bonus.items[i]);
			}
			else
			{
				this.bonusItems[i].SetVisible(false);
				num++;
			}
		}
		this.CenterItems(num);
	}

	public void OnButtonOkClick()
	{
		base.gameObject.SetActive(false);
	}

	private void SetTitleText(string text)
	{
		for (int i = 0; i < (int)this.title.Length; i++)
		{
			this.title[i].text = text;
		}
	}

	public void Show(ChestBonusData bonus)
	{
		if (bonus.items == null || bonus.items.Count == 0)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.SetTitleText(LocalizationStore.Get("Key_1057"));
		this.description.text = LocalizationStore.Get("Key_1058");
		this.CreateBonusesItemsAndAlign(bonus);
	}
}