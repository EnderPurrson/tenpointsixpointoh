using Rilisoft;
using System;
using UnityEngine;

public class SorryWeaponAndArmorBanner : BannerWindow
{
	public bool sorryGearRemoved;

	public bool sorryArmorHatRemoved;

	public UILabel[] labelGoldCompensations;

	public UILabel[] labelGemsCompensations;

	public UIWidget goldContainer;

	public UIWidget gemsContainer;

	private SaltedInt _compensationGoldCount;

	private SaltedInt _compensationGemsCount;

	public SorryWeaponAndArmorBanner()
	{
	}

	private void AligmentCompensationContainer()
	{
		if (this._compensationGoldCount.Value > 0 && this._compensationGemsCount.Value == 0)
		{
			this.goldContainer.gameObject.SetActive(true);
			this.gemsContainer.gameObject.SetActive(false);
		}
		else if (this._compensationGoldCount.Value == 0 && this._compensationGemsCount.Value > 0)
		{
			this.goldContainer.gameObject.SetActive(false);
			this.gemsContainer.gameObject.SetActive(true);
		}
		else if (this._compensationGoldCount.Value > 0 && this._compensationGemsCount.Value > 0)
		{
			Vector3 vector3 = this.goldContainer.transform.localPosition;
			this.goldContainer.transform.localPosition = new Vector3(vector3.x, vector3.y - (float)(this.goldContainer.height / 2), vector3.z);
			vector3 = this.gemsContainer.transform.localPosition;
			this.gemsContainer.transform.localPosition = new Vector3(vector3.x, vector3.y + (float)(this.gemsContainer.height / 2), vector3.z);
		}
	}

	public override void Show()
	{
		if (this.sorryArmorHatRemoved)
		{
			this._compensationGemsCount.Value = 0;
		}
		else if (this.sorryGearRemoved)
		{
			this._compensationGoldCount.Value = 0;
		}
		for (int i = 0; i < (int)this.labelGoldCompensations.Length; i++)
		{
			this.labelGoldCompensations[i].text = this._compensationGoldCount.Value.ToString();
		}
		for (int j = 0; j < (int)this.labelGemsCompensations.Length; j++)
		{
			this.labelGemsCompensations[j].text = this._compensationGemsCount.Value.ToString();
		}
		this.AligmentCompensationContainer();
		base.Show();
	}

	public void SorryWeaponAndArmorExitClick()
	{
		if (this._compensationGoldCount.Value > 0)
		{
			BankController.AddCoins(this._compensationGoldCount.Value, true, AnalyticsConstants.AccrualType.Earned);
		}
		if (this._compensationGemsCount.Value > 0)
		{
			BankController.AddGems(this._compensationGemsCount.Value, true, AnalyticsConstants.AccrualType.Earned);
		}
		if (!this.sorryArmorHatRemoved)
		{
			if (this.sorryGearRemoved)
			{
			}
		}
		Storager.setInt(Defs.ShowSorryWeaponAndArmor, 1, false);
		AudioClip audioClip = Resources.Load("coin_get") as AudioClip;
		if (Defs.isSoundFX && audioClip != null)
		{
			NGUITools.PlaySound(audioClip);
		}
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}
}