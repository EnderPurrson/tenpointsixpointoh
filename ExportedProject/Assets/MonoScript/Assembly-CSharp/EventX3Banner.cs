using Rilisoft.NullExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventX3Banner : BannerWindow
{
	public GameObject amazonEventObject;

	public UILabel amazonEventCaptionLabel;

	public UILabel amazonEventTitleLabel;

	public EventX3Banner()
	{
	}

	private void OnAmazonEventUpdated()
	{
		this.amazonEventObject.SetActive(PromoActionsManager.sharedManager.IsAmazonEventX3Active);
		this.RefreshAmazonBonus();
	}

	private void OnDisable()
	{
		PromoActionsManager.EventAmazonX3Updated -= new Action(this.OnAmazonEventUpdated);
	}

	private void OnEnable()
	{
		bool isAmazonEventX3Active = PromoActionsManager.sharedManager.IsAmazonEventX3Active;
		this.amazonEventObject.SetActive(isAmazonEventX3Active);
		PromoActionsManager.EventAmazonX3Updated += new Action(this.OnAmazonEventUpdated);
		this.RefreshAmazonBonus();
	}

	private void RefreshAmazonBonus()
	{
		UILabel[] componentsInChildren = this.amazonEventObject.GetComponentsInChildren<UILabel>();
		UILabel uILabel = this.amazonEventCaptionLabel ?? ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => "CaptionLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase));
		PromoActionsManager.AmazonEventInfo amazonEventInfo = PromoActionsManager.sharedManager.Map<PromoActionsManager, PromoActionsManager.AmazonEventInfo>((PromoActionsManager p) => p.AmazonEvent);
		if (uILabel != null)
		{
			uILabel.text = amazonEventInfo.Map<PromoActionsManager.AmazonEventInfo, string>((PromoActionsManager.AmazonEventInfo e) => e.Caption) ?? string.Empty;
		}
		UILabel[] uILabelArray = (this.amazonEventTitleLabel ?? ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => "TitleLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase))).Map<UILabel, UILabel[]>((UILabel t) => t.GetComponentsInChildren<UILabel>()) ?? new UILabel[0];
		float single = amazonEventInfo.Map<PromoActionsManager.AmazonEventInfo, float>((PromoActionsManager.AmazonEventInfo e) => e.Percentage);
		string str = LocalizationStore.Get("Key_1672");
		UILabel[] uILabelArray1 = uILabelArray;
		for (int i = 0; i < (int)uILabelArray1.Length; i++)
		{
			uILabelArray1[i].text = ("Key_1672".Equals(str, StringComparison.OrdinalIgnoreCase) ? string.Empty : string.Format(str, single));
		}
	}
}