using System;
using System.Collections.Generic;
using UnityEngine;

public class TableGearController : MonoBehaviour
{
	public static TableGearController sharedController;

	public TimePotionUpdate[] potionLables;

	public UITable table;

	public UILabel activatePotionLabel;

	private string[] keysForLabel = new string[] { "Key_1813", "Key_1810", "Key_1812", "Key_1811" };

	private string[] keysForLabelDater = new string[] { "Key_1853", "Key_1851", "Key_1854", "Key_1852" };

	private float timerShowLabel = -1f;

	static TableGearController()
	{
	}

	public TableGearController()
	{
	}

	private void OnDestroy()
	{
		TableGearController.sharedController = null;
	}

	public void ReactivatePotion(string _potion)
	{
		int num = (int)Enum.Parse(typeof(TableGearController.TypeGear), _potion);
		this.potionLables[num].transform.GetChild(0).GetComponent<TweenScale>().enabled = true;
		this.ReNameLabelObjects();
		this.table.Reposition();
		this.activatePotionLabel.text = LocalizationStore.Get((!Defs.isDaterRegim ? this.keysForLabel[num] : this.keysForLabelDater[num]));
		this.activatePotionLabel.gameObject.SetActive(true);
		this.timerShowLabel = 2f;
	}

	private void ReNameLabelObjects()
	{
		for (int i = 0; i < PotionsController.sharedController.activePotionsList.Count; i++)
		{
			string item = PotionsController.sharedController.activePotionsList[i];
			TableGearController.TypeGear typeGear = (TableGearController.TypeGear)((int)Enum.Parse(typeof(TableGearController.TypeGear), item));
			this.potionLables[(int)typeGear].name = i.ToString();
		}
	}

	private void Start()
	{
		TableGearController.sharedController = this;
	}

	private void Update()
	{
		if (this.timerShowLabel > 0f)
		{
			this.timerShowLabel -= Time.deltaTime;
			if (this.timerShowLabel < 0f)
			{
				this.activatePotionLabel.gameObject.SetActive(false);
			}
		}
		for (int i = 0; i < (int)this.potionLables.Length; i++)
		{
			if (PotionsController.sharedController.PotionIsActive(this.potionLables[i].myPotionName))
			{
				if (!this.potionLables[i].gameObject.activeSelf)
				{
					this.potionLables[i].transform.GetChild(0).GetComponent<TweenScale>().enabled = true;
					this.potionLables[i].gameObject.SetActive(true);
					this.ReNameLabelObjects();
					this.table.Reposition();
					string str = this.potionLables[i].myPotionName;
					int num = (int)Enum.Parse(typeof(TableGearController.TypeGear), str);
					this.activatePotionLabel.text = LocalizationStore.Get((!Defs.isDaterRegim ? this.keysForLabel[num] : this.keysForLabelDater[num]));
					this.activatePotionLabel.gameObject.SetActive(true);
					this.timerShowLabel = 2f;
				}
				this.potionLables[i].timerUpdate -= Time.deltaTime;
				if (this.potionLables[i].timerUpdate < 0f)
				{
					this.potionLables[i].timerUpdate = 0.25f;
					this.potionLables[i].UpdateTime();
				}
			}
			else if (this.potionLables[i].gameObject.activeSelf)
			{
				this.potionLables[i].gameObject.SetActive(false);
				this.potionLables[i].myLabel.text = string.Empty;
				this.table.Reposition();
			}
		}
	}

	private enum TypeGear
	{
		Turret,
		Mech,
		Jetpack,
		InvisibilityPotion
	}
}