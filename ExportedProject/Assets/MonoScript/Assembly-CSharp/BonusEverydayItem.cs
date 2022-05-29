using I2.Loc;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BonusEverydayItem : MonoBehaviour
{
	public UISprite checkTakedReward;

	public UITexture imageReward;

	public UILabel descriptionReward;

	public UILabel descriptionReward1;

	public UILabel descriptionReward2;

	public UILabel titleDayTakeReward;

	public UITexture background;

	public UITexture backgroundWeekly;

	public UIWidget hightlightWeeklyBonus;

	public BonusItemDetailInfo windowDetail;

	public UIWidget hightlightBonus;

	private BonusMarafonItem _bonusData;

	protected int BonusIndex
	{
		get;
		set;
	}

	public BonusEverydayItem()
	{
	}

	public void FillData(int bonusIndex, int currentBonusIndex, bool isBonusWeekly)
	{
		this.BonusIndex = bonusIndex;
		this.SetTitleItem();
		if (bonusIndex < currentBonusIndex)
		{
			this.SetCheckForTakedReward();
		}
		bool flag = bonusIndex == currentBonusIndex;
		if (this.hightlightBonus != null && !isBonusWeekly)
		{
			this.hightlightBonus.alpha = (!flag ? 0f : 1f);
		}
		if (this._bonusData != null)
		{
			this.SetDescriptionItem(this._bonusData.GetShortDescription());
			this.SetImageForReward(this._bonusData.GetIcon());
		}
		if (isBonusWeekly && this.hightlightWeeklyBonus != null)
		{
			this.SetBackgroundForBonusWeek();
			this.hightlightWeeklyBonus.alpha = (!flag ? 0f : 1f);
		}
	}

	private void HandleLocalizationChanged()
	{
		this.SetTitleItem();
	}

	private void OnClick()
	{
		this.OnClickDetailInfo();
	}

	private void OnClickDetailInfo()
	{
		if (this._bonusData == null)
		{
			return;
		}
		string shortDescription = this._bonusData.GetShortDescription();
		string longDescription = this._bonusData.GetLongDescription();
		Texture2D icon = this._bonusData.GetIcon();
		this.windowDetail.SetTitle(shortDescription);
		this.windowDetail.SetDescription(longDescription);
		this.windowDetail.SetImage(icon);
		this.windowDetail.Show();
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	public void SetBackgroundForBonusWeek()
	{
		this.background.gameObject.SetActive(false);
		if (this.backgroundWeekly != null)
		{
			this.backgroundWeekly.gameObject.SetActive(true);
		}
	}

	public void SetCheckForTakedReward()
	{
		this.checkTakedReward.gameObject.SetActive(true);
	}

	public void SetDescriptionItem(string text)
	{
		this.descriptionReward.text = text;
		if (this.descriptionReward1 != null)
		{
			this.descriptionReward1.text = text;
		}
		if (this.descriptionReward2 != null)
		{
			this.descriptionReward2.text = text;
		}
	}

	public void SetImageForReward(Texture2D image)
	{
		this.imageReward.mainTexture = image;
	}

	private void SetTitleItem(string text)
	{
		this.titleDayTakeReward.text = text;
	}

	private void SetTitleItem()
	{
		this.SetTitleItem(string.Format("{0} {1}", LocalizationStore.Get("Key_0469"), this.BonusIndex + 1));
	}

	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}
}