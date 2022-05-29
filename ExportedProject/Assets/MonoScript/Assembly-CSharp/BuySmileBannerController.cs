using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class BuySmileBannerController : BannerWindow
{
	public static bool openedFromPromoActions;

	private IDisposable _backSubscription;

	static BuySmileBannerController()
	{
	}

	public BuySmileBannerController()
	{
	}

	public void BuyStickersPack(StickersPackItem curStickPack)
	{
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(curStickPack.KeyForBuy);
		int price = itemPrice.Price;
		string currency = itemPrice.Currency;
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, itemPrice, () => {
			Storager.setInt(curStickPack.KeyForBuy, 1, true);
			try
			{
				FlurryEvents.LogPurchaseStickers(curStickPack.KeyForBuy);
				string str = "Stickers";
				AnalyticsStuff.LogSales(curStickPack.KeyForBuy, str, false);
				AnalyticsFacade.InAppPurchase(curStickPack.KeyForBuy, str, 1, price, currency);
				if (BuySmileBannerController.openedFromPromoActions)
				{
					AnalyticsStuff.LogSpecialOffersPanel("Efficiency", "Buy", "Stickers", curStickPack.KeyForBuy);
				}
				BuySmileBannerController.openedFromPromoActions = false;
			}
			catch (Exception exception)
			{
			}
			if (PrivateChatController.sharedController != null && PrivateChatController.sharedController.gameObject.activeInHierarchy)
			{
				PrivateChatController.sharedController.isBuySmile = true;
				if (!PrivateChatController.sharedController.isShowSmilePanel)
				{
					PrivateChatController.sharedController.showSmileButton.SetActive(true);
				}
				PrivateChatController.sharedController.buySmileButton.SetActive(false);
				this.OnCloseClick();
			}
			if (ChatViewrController.sharedController != null && ChatViewrController.sharedController.gameObject.activeInHierarchy)
			{
				ChatViewrController.sharedController.buySmileButton.SetActive(false);
				if (!ChatViewrController.sharedController.isShowSmilePanel)
				{
					ChatViewrController.sharedController.showSmileButton.SetActive(true);
				}
				this.OnCloseClick();
			}
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "Succeeded", BuySmileBannerController.GetCurrentBuySmileContextName() }
			};
			if (ExperienceController.sharedController != null)
			{
				strs.Add("Level", ExperienceController.sharedController.currentLevel.ToString());
			}
			if (ExpController.Instance != null)
			{
				strs.Add("Tier", ExpController.Instance.OurTier.ToString());
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Smile purchase", strs, true);
			curStickPack.OnBuy();
			ButtonBannerHUD.OnUpdateBanners();
		}, null, null, null, null, null);
	}

	public static string GetCurrentBuySmileContextName()
	{
		string str;
		if (!(FriendsWindowGUI.Instance != null) || !FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			str = (ChatViewrController.sharedController == null ? "Lobby" : "Sandbox");
		}
		else
		{
			str = "Friends";
		}
		return str;
	}

	private void HandleEscape()
	{
		if (FriendsWindowGUI.Instance.Map<FriendsWindowGUI, bool>((FriendsWindowGUI f) => f.InterfaceEnabled) || ChatViewrController.sharedController != null)
		{
			this.OnCloseClick();
		}
		else if (BannerWindowController.SharedController.Map<BannerWindowController, bool>((BannerWindowController b) => b.IsBannerShow(BannerWindowType.buySmiles)))
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	public void OnCloseClick()
	{
		ButtonClickSound.TryPlayClick();
		BuySmileBannerController.openedFromPromoActions = false;
		base.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Buy Smiley Banner");
	}
}