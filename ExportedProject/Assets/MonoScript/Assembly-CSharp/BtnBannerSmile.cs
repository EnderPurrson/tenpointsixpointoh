using System;

public class BtnBannerSmile : ButtonBannerBase
{
	public string tagForClick = string.Empty;

	public BtnBannerSmile()
	{
	}

	public override bool BannerIsActive()
	{
		return !StickersController.IsBuyAllPack();
	}

	public override void OnChangeLocalize()
	{
	}

	public override void OnClickButton()
	{
		MainMenuController.sharedController.HandlePromoActionClicked(this.tagForClick);
	}

	public override void OnHide()
	{
	}

	public override void OnShow()
	{
	}
}