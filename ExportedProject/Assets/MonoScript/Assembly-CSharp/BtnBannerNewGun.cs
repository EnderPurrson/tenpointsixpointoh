using Rilisoft;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BtnBannerNewGun : ButtonBannerBase
{
	public string tagForClick = string.Empty;

	public UILabel lbSale;

	public UILabel lbPrice;

	public UITexture txGun;

	public UISprite sprCoinImg;

	public BtnBannerNewGun()
	{
	}

	public override bool BannerIsActive()
	{
		if (PromoActionsManager.sharedManager == null)
		{
			return false;
		}
		if (PromoActionsManager.sharedManager.news.Count > 0)
		{
			return true;
		}
		return false;
	}

	public override void OnChangeLocalize()
	{
	}

	public override void OnClickButton()
	{
		if (!string.IsNullOrEmpty(this.tagForClick))
		{
			MainMenuController.sharedController.HandlePromoActionClicked(this.tagForClick);
		}
	}

	public override void OnHide()
	{
	}

	public override void OnShow()
	{
		if (string.IsNullOrEmpty(this.tagForClick))
		{
			this.OnUpdateParameter();
		}
	}

	public override void OnUpdateParameter()
	{
		if (PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.news.Count > 0)
		{
			int num = UnityEngine.Random.Range(0, PromoActionsManager.sharedManager.news.Count);
			this.tagForClick = PromoActionsManager.sharedManager.news[num];
		}
		if (!string.IsNullOrEmpty(this.tagForClick))
		{
			string empty = string.Empty;
			int num1 = PromoActionsGUIController.CatForTg(this.tagForClick);
			empty = string.Concat("OfferIcons/", PromoActionsGUIController.IconNameForKey(this.tagForClick, num1));
			Texture texture = Resources.Load<Texture>(empty);
			this.txGun.mainTexture = texture;
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId(this.tagForClick);
			UILabel uILabel = this.lbSale;
			string key0419 = LocalizationStore.Key_0419;
			SaltedInt item = PromoActionsManager.sharedManager.discounts[this.tagForClick][0];
			uILabel.text = string.Format("{0}\n{1}%", key0419, item.Value);
			UILabel str = this.lbPrice;
			SaltedInt saltedInt = PromoActionsManager.sharedManager.discounts[this.tagForClick][1];
			str.text = saltedInt.Value.ToString();
			this.lbPrice.color = (!priceByShopId.Currency.Equals("Coins") ? new Color(0.3176f, 0.8117f, 1f) : new Color(1f, 0.8627f, 0f));
			this.sprCoinImg.spriteName = (!priceByShopId.Currency.Equals("Coins") ? "gem_znachek" : "ingame_coin");
		}
	}
}