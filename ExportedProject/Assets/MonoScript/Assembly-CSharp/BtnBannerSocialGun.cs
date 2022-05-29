using System;

public class BtnBannerSocialGun : ButtonBannerBase
{
	public BtnBannerSocialGun()
	{
	}

	public override bool BannerIsActive()
	{
		return FacebookController.sharedController.SocialGunEventActive;
	}

	public override void OnChangeLocalize()
	{
	}

	public override void OnClickButton()
	{
		MainMenuController.sharedController.OnSocialGunEventButtonClick();
	}

	public override void OnHide()
	{
	}

	public override void OnShow()
	{
	}
}