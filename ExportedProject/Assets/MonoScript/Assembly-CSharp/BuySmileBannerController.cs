using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;

public sealed class BuySmileBannerController : BannerWindow
{
	[CompilerGenerated]
	private sealed class _003CBuyStickersPack_003Ec__AnonStorey1DE
	{
		internal StickersPackItem curStickPack;

		internal int priceAmount;

		internal string priceCurrency;

		internal BuySmileBannerController _003C_003Ef__this;

		internal void _003C_003Em__9()
		{
			Storager.setInt(curStickPack.KeyForBuy, 1, true);
			try
			{
				FlurryEvents.LogPurchaseStickers(curStickPack.KeyForBuy);
				string text = "Stickers";
				AnalyticsStuff.LogSales(curStickPack.KeyForBuy, text);
				AnalyticsFacade.InAppPurchase(curStickPack.KeyForBuy, text, 1, priceAmount, priceCurrency);
				if (openedFromPromoActions)
				{
					AnalyticsStuff.LogSpecialOffersPanel("Efficiency", "Buy", "Stickers", curStickPack.KeyForBuy);
				}
				openedFromPromoActions = false;
			}
			catch (Exception)
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
				_003C_003Ef__this.OnCloseClick();
			}
			if (ChatViewrController.sharedController != null && ChatViewrController.sharedController.gameObject.activeInHierarchy)
			{
				ChatViewrController.sharedController.buySmileButton.SetActive(false);
				if (!ChatViewrController.sharedController.isShowSmilePanel)
				{
					ChatViewrController.sharedController.showSmileButton.SetActive(true);
				}
				_003C_003Ef__this.OnCloseClick();
			}
			string currentBuySmileContextName = GetCurrentBuySmileContextName();
			Dictionary<string, string> dictionary = new Dictionary<string, string> { { "Succeeded", currentBuySmileContextName } };
			if (ExperienceController.sharedController != null)
			{
				dictionary.Add("Level", ExperienceController.sharedController.currentLevel.ToString());
			}
			if (ExpController.Instance != null)
			{
				dictionary.Add("Tier", ExpController.Instance.OurTier.ToString());
			}
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Smile purchase", dictionary);
			curStickPack.OnBuy();
			ButtonBannerHUD.OnUpdateBanners();
		}
	}

	public static bool openedFromPromoActions;

	private IDisposable _backSubscription;

	[CompilerGenerated]
	private static Func<FriendsWindowGUI, bool> _003C_003Ef__am_0024cache2;

	[CompilerGenerated]
	private static Func<BannerWindowController, bool> _003C_003Ef__am_0024cache3;

	public static string GetCurrentBuySmileContextName()
	{
		return (FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled) ? "Friends" : ((!(ChatViewrController.sharedController != null)) ? "Lobby" : "Sandbox");
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(HandleEscape, "Buy Smiley Banner");
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void HandleEscape()
	{
		FriendsWindowGUI instance = FriendsWindowGUI.Instance;
		if (_003C_003Ef__am_0024cache2 == null)
		{
			_003C_003Ef__am_0024cache2 = _003CHandleEscape_003Em__7;
		}
		if (instance.Map(_003C_003Ef__am_0024cache2) || ChatViewrController.sharedController != null)
		{
			OnCloseClick();
			return;
		}
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = _003CHandleEscape_003Em__8;
		}
		if (sharedController.Map(_003C_003Ef__am_0024cache3))
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	public void OnCloseClick()
	{
		ButtonClickSound.TryPlayClick();
		openedFromPromoActions = false;
		base.gameObject.SetActive(false);
	}

	public void BuyStickersPack(StickersPackItem curStickPack)
	{
		_003CBuyStickersPack_003Ec__AnonStorey1DE _003CBuyStickersPack_003Ec__AnonStorey1DE = new _003CBuyStickersPack_003Ec__AnonStorey1DE();
		_003CBuyStickersPack_003Ec__AnonStorey1DE.curStickPack = curStickPack;
		_003CBuyStickersPack_003Ec__AnonStorey1DE._003C_003Ef__this = this;
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(_003CBuyStickersPack_003Ec__AnonStorey1DE.curStickPack.KeyForBuy);
		_003CBuyStickersPack_003Ec__AnonStorey1DE.priceAmount = itemPrice.Price;
		_003CBuyStickersPack_003Ec__AnonStorey1DE.priceCurrency = itemPrice.Currency;
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, itemPrice, _003CBuyStickersPack_003Ec__AnonStorey1DE._003C_003Em__9);
	}

	[CompilerGenerated]
	private static bool _003CHandleEscape_003Em__7(FriendsWindowGUI f)
	{
		return f.InterfaceEnabled;
	}

	[CompilerGenerated]
	private static bool _003CHandleEscape_003Em__8(BannerWindowController b)
	{
		return b.IsBannerShow(BannerWindowType.buySmiles);
	}
}
