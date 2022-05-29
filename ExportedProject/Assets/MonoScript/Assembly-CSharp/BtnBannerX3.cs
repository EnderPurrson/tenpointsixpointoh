using System;

public class BtnBannerX3 : ButtonBannerBase
{
	public BtnBannerX3()
	{
	}

	public override bool BannerIsActive()
	{
		return PromoActionsManager.sharedManager.IsEventX3Active;
	}

	public override void OnChangeLocalize()
	{
	}

	public override void OnClickButton()
	{
		MainMenuController.sharedController.ShowBankWindow();
	}

	public override void OnHide()
	{
	}

	public override void OnShow()
	{
	}
}