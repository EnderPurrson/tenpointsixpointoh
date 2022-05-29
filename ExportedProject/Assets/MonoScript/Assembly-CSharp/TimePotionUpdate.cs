using System;
using UnityEngine;

public class TimePotionUpdate : MonoBehaviour
{
	public UILabel myLabel;

	public GameObject mySpriteObj;

	public string myPotionName;

	[NonSerialized]
	public float timerUpdate = -1f;

	public TimePotionUpdate()
	{
	}

	private void SetTimeForLabel()
	{
		if (!PotionsController.sharedController.PotionIsActive(this.myPotionName))
		{
			if (this.mySpriteObj != null && this.mySpriteObj.activeSelf)
			{
				this.mySpriteObj.SetActive(false);
				this.myLabel.text = string.Empty;
			}
			return;
		}
		if (this.mySpriteObj != null && !this.mySpriteObj.activeSelf)
		{
			this.mySpriteObj.SetActive(true);
		}
		float single = PotionsController.sharedController.RemainDuratioForPotion(this.myPotionName);
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)single);
		this.myLabel.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		if (single > 5f)
		{
			this.myLabel.color = new Color(1f, 1f, 1f);
		}
		else
		{
			this.myLabel.color = new Color(1f, 0f, 0f);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.myLabel.enabled)
		{
			this.timerUpdate -= Time.deltaTime;
			if (this.timerUpdate < 0f)
			{
				this.timerUpdate = 0.25f;
				this.SetTimeForLabel();
			}
		}
	}

	public void UpdateTime()
	{
		float single = PotionsController.sharedController.RemainDuratioForPotion(this.myPotionName);
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)single);
		this.myLabel.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		if (single > 5f)
		{
			this.myLabel.color = new Color(1f, 1f, 1f);
		}
		else
		{
			this.myLabel.color = new Color(1f, 0f, 0f);
		}
	}
}