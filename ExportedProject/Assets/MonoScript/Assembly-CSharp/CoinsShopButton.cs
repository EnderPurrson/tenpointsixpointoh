using System;
using UnityEngine;

public class CoinsShopButton : MonoBehaviour
{
	public GameObject eventX3;

	public CoinsShopButton()
	{
	}

	private void OnDestroy()
	{
		PromoActionsManager.EventX3Updated -= new Action(this.OnEventX3Updated);
	}

	private void OnEnable()
	{
		this.OnEventX3Updated();
	}

	private void OnEventX3Updated()
	{
		bool flag = (PromoActionsManager.sharedManager == null ? false : PromoActionsManager.sharedManager.IsEventX3Active);
		if (this.eventX3 != null && this.eventX3.activeSelf != flag)
		{
			this.eventX3.SetActive(flag);
		}
	}

	private void Start()
	{
		PromoActionsManager.EventX3Updated += new Action(this.OnEventX3Updated);
		this.OnEventX3Updated();
	}
}