using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MarafonBonusWindow : BannerWindow
{
	public GameObject premiumInterface;

	public UIScrollView bonusScrollView;

	public UIGrid bonusScroll;

	public UILabel title;

	public GameObject bonusEverydayItem;

	public UIScrollView scrollView;

	public BonusEverydayItem[] superPrizes;

	public MarafonBonusWindow()
	{
	}

	private void CentralizeScrollByCurrentBonus()
	{
		if (this.bonusScroll == null)
		{
			return;
		}
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		Transform child = this.bonusScroll.GetChild(currentBonusIndex);
		if (child != null)
		{
			if (currentBonusIndex > 2 && currentBonusIndex < 27)
			{
				this.bonusScroll.GetComponent<UICenterOnChild>().springStrength = 8f;
				this.bonusScroll.GetComponent<UICenterOnChild>().CenterOn(child);
			}
			else if (currentBonusIndex >= 27)
			{
				this.bonusScroll.GetComponent<UICenterOnChild>().springStrength = 8f;
				Transform transforms = this.bonusScroll.GetChild(27);
				if (transforms != null)
				{
					this.bonusScroll.GetComponent<UICenterOnChild>().CenterOn(transforms);
				}
			}
			child.localScale = Vector3.one;
		}
		this.bonusScroll.GetComponent<UICenterOnChild>().onCenter = new UICenterOnChild.OnCenterCallback(this.ResetScrollPosition);
	}

	private void FillBonusesForEveryday()
	{
		List<BonusMarafonItem> bonusItems = MarafonBonusController.Get.BonusItems;
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		BonusEverydayItem[] componentsInChildren = this.bonusScroll.GetComponentsInChildren<BonusEverydayItem>(true);
		bool length = (int)componentsInChildren.Length != 0;
		BonusEverydayItem vector3 = null;
		GameObject gameObject = null;
		for (int i = 0; i < bonusItems.Count; i++)
		{
			if (!length)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bonusEverydayItem);
				gameObject.name = string.Format("{0:00}", i);
			}
			vector3 = (!length ? gameObject.GetComponent<BonusEverydayItem>() : componentsInChildren[i]);
			if (vector3 != null)
			{
				vector3.FillData(i, currentBonusIndex, ((i + 1) % 7 == 0 ? true : i == bonusItems.Count - 1));
			}
			if (!length)
			{
				this.bonusScroll.AddChild(gameObject.transform);
				gameObject.gameObject.SetActive(true);
			}
			vector3.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		}
		vector3.SetBackgroundForBonusWeek();
		this.bonusScroll.Reposition();
	}

	private void FillPrizesForEveryweek()
	{
		List<BonusMarafonItem> bonusItems = MarafonBonusController.Get.BonusItems;
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		int num = 0;
		for (int i = 6; i < bonusItems.Count; i += 7)
		{
			BonusEverydayItem bonusEverydayItem = this.superPrizes[num];
			num++;
			if (bonusEverydayItem != null)
			{
				bonusEverydayItem.FillData(i, currentBonusIndex, false);
			}
		}
		int length = (int)this.superPrizes.Length - 1;
		int count = bonusItems.Count - 1;
		this.superPrizes[length].FillData(count, currentBonusIndex, false);
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.StartCentralizeBonusItem());
	}

	public void OnGetRewardClick()
	{
		ButtonClickSound.TryPlayClick();
		this.scrollView.ResetPosition();
		MarafonBonusController.Get.TakeMarafonBonus();
		BannerWindowController.SharedController.HideBannerWindow();
	}

	private void ResetScrollPosition(GameObject centerElement)
	{
		this.bonusScroll.GetComponent<UICenterOnChild>().enabled = false;
		this.bonusScroll.Reposition();
	}

	public override void Show()
	{
		MarafonBonusController.Get.InitializeBonusItems();
		this.FillBonusesForEveryday();
		this.FillPrizesForEveryweek();
		base.Show();
	}

	[DebuggerHidden]
	public IEnumerator StartCentralizeBonusItem()
	{
		MarafonBonusWindow.u003cStartCentralizeBonusItemu003ec__Iterator16F variable = null;
		return variable;
	}

	internal sealed override void Submit()
	{
		this.OnGetRewardClick();
	}

	private void Update()
	{
		if (this.premiumInterface.activeSelf != (PremiumAccountController.Instance == null ? false : PremiumAccountController.Instance.isAccountActive))
		{
			this.premiumInterface.SetActive((PremiumAccountController.Instance == null ? false : PremiumAccountController.Instance.isAccountActive));
		}
	}
}