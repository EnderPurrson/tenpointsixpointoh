using System;
using UnityEngine;

public sealed class DaysOfValorLabelActive : MonoBehaviour
{
	public UISprite coinsLabel;

	public UISprite expLabel;

	public DaysOfValorLabelActive()
	{
	}

	private void Awake()
	{
		this.UpdateLabels();
	}

	private void Update()
	{
		this.UpdateLabels();
	}

	private void UpdateLabels()
	{
		if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForExp > 1 && !this.expLabel.gameObject.activeSelf)
		{
			this.expLabel.gameObject.SetActive(true);
			UISprite uISprite = this.expLabel;
			int dayOfValorMultiplyerForExp = PromoActionsManager.sharedManager.DayOfValorMultiplyerForExp;
			uISprite.spriteName = string.Concat(dayOfValorMultiplyerForExp.ToString(), "x");
		}
		if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForExp == 1 && this.expLabel.gameObject.activeSelf)
		{
			this.expLabel.gameObject.SetActive(false);
			Transform vector3 = this.coinsLabel.transform;
			float single = this.coinsLabel.transform.localPosition.y;
			Vector3 vector31 = this.coinsLabel.transform.localPosition;
			vector3.localPosition = new Vector3(109f, single, vector31.z);
		}
		if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForMoney > 1 && !this.coinsLabel.gameObject.activeSelf)
		{
			this.coinsLabel.gameObject.SetActive(true);
			UISprite uISprite1 = this.coinsLabel;
			int dayOfValorMultiplyerForMoney = PromoActionsManager.sharedManager.DayOfValorMultiplyerForMoney;
			uISprite1.spriteName = string.Concat(dayOfValorMultiplyerForMoney.ToString(), "x");
			if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForExp > 1)
			{
				Transform transforms = this.coinsLabel.transform;
				float single1 = this.coinsLabel.transform.localPosition.y;
				Vector3 vector32 = this.coinsLabel.transform.localPosition;
				transforms.localPosition = new Vector3(28f, single1, vector32.z);
			}
		}
		if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForMoney == 1 && this.coinsLabel.gameObject.activeSelf)
		{
			this.coinsLabel.gameObject.SetActive(false);
		}
	}
}