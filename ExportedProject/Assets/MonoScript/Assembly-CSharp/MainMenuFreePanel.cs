using System;
using System.Runtime.CompilerServices;
using Facebook.Unity;
using Rilisoft;
using UnityEngine;

public class MainMenuFreePanel : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHandleSocialGunEventStateChanged_003Ec__AnonStorey2CB
	{
		internal bool enable;

		internal void _003C_003Em__389(RewardedLikeButton b)
		{
			b.gameObject.SetActive(!enable);
		}
	}

	[SerializeField]
	private GameObject _postNewsLabel;

	[SerializeField]
	private GameObject _starParticleSocialGunButton;

	[SerializeField]
	private GameObject _socialGunPanel;

	[SerializeField]
	private ButtonHandler _youtubeButton;

	[SerializeField]
	private ButtonHandler _everyplayButton;

	[SerializeField]
	private ButtonHandler _enderManButton;

	[SerializeField]
	private ButtonHandler _postFacebookButton;

	[SerializeField]
	private ButtonHandler _postTwitterButton;

	[SerializeField]
	private ButtonHandler _rateUsButton;

	[SerializeField]
	private ButtonHandler _backButton;

	[SerializeField]
	private ButtonHandler _twitterSubcribeButton;

	[SerializeField]
	private ButtonHandler _facebookSubcribeButton;

	[SerializeField]
	private UILabel _socialGunEventTimerLabel;

	[CompilerGenerated]
	private static EventHandler _003C_003Ef__am_0024cacheD;

	private void Start()
	{
		_postNewsLabel.SetActive(false);
		if (_youtubeButton != null)
		{
			_youtubeButton.Clicked += HandleYoutubeClicked;
		}
		if (_everyplayButton != null)
		{
			_everyplayButton.Clicked += HandleEveryPlayClicked;
		}
		if (_enderManButton != null)
		{
			_enderManButton.Clicked += HandleEnderClicked;
		}
		if (_postFacebookButton != null)
		{
			_postFacebookButton.Clicked += HandlePostFacebookClicked;
		}
		if (_postTwitterButton != null)
		{
			_postTwitterButton.Clicked += HandlePostTwittwerClicked;
		}
		if (_rateUsButton != null)
		{
			_rateUsButton.Clicked += HandleRateAsClicked;
		}
		if (_twitterSubcribeButton != null)
		{
			_twitterSubcribeButton.Clicked += HandleTwitterSubscribeClicked;
		}
		if (_facebookSubcribeButton != null)
		{
			_facebookSubcribeButton.Clicked += HandleFacebookSubscribeClicked;
		}
		if (_backButton != null)
		{
			ButtonHandler backButton = _backButton;
			if (_003C_003Ef__am_0024cacheD == null)
			{
				_003C_003Ef__am_0024cacheD = _003CStart_003Em__388;
			}
			backButton.Clicked += _003C_003Ef__am_0024cacheD;
		}
		FacebookController.SocialGunEventStateChanged += HandleSocialGunEventStateChanged;
		if (FacebookController.sharedController != null)
		{
			HandleSocialGunEventStateChanged(FacebookController.sharedController.SocialGunEventActive);
		}
	}

	private void Update()
	{
		bool flag = (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !ShopNGUIController.GuiActive;
		if (_starParticleSocialGunButton != null && _starParticleSocialGunButton.activeInHierarchy != flag)
		{
			_starParticleSocialGunButton.SetActive(flag);
		}
		if (_postFacebookButton.gameObject.activeSelf != (FacebookController.FacebookSupported && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn))
		{
			_postFacebookButton.gameObject.SetActive(FacebookController.FacebookSupported && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn);
		}
		if (_postTwitterButton.gameObject.activeSelf != (TwitterController.TwitterSupported && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn))
		{
			_postTwitterButton.gameObject.SetActive(TwitterController.TwitterSupported && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn);
		}
		if (FacebookController.sharedController != null && FacebookController.sharedController.SocialGunEventActive)
		{
			_socialGunEventTimerLabel.text = string.Empty;
		}
	}

	private void OnDestroy()
	{
		FacebookController.SocialGunEventStateChanged -= HandleSocialGunEventStateChanged;
	}

	public void SetVisible(bool visible)
	{
		if (base.gameObject.activeSelf != visible)
		{
			base.gameObject.SetActive(visible);
		}
	}

	public void OnSocialGunButtonClicked()
	{
		MainMenuController.sharedController.OnSocialGunEventButtonClick();
	}

	private void HandleSocialGunEventStateChanged(bool enable)
	{
		_003CHandleSocialGunEventStateChanged_003Ec__AnonStorey2CB _003CHandleSocialGunEventStateChanged_003Ec__AnonStorey2CB = new _003CHandleSocialGunEventStateChanged_003Ec__AnonStorey2CB();
		_003CHandleSocialGunEventStateChanged_003Ec__AnonStorey2CB.enable = enable;
		_socialGunPanel.gameObject.SetActive(_003CHandleSocialGunEventStateChanged_003Ec__AnonStorey2CB.enable);
		GetComponentsInChildren<RewardedLikeButton>(true).ForEach(_003CHandleSocialGunEventStateChanged_003Ec__AnonStorey2CB._003C_003Em__389);
		if (FacebookController.sharedController != null)
		{
			_socialGunEventTimerLabel.text = string.Empty;
		}
	}

	private void HandleYoutubeClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened)
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				FlurryEvents.LogAfterTraining("YouTube", TrainingController.TrainingCompletedFlagForLogging.Value);
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL("http://www.youtube.com/channel/UCsClw1gnMrmF6ssIB_166_Q");
		}
	}

	private void HandleEveryPlayClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened)
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				FlurryEvents.LogAfterTraining("Everyplay", TrainingController.TrainingCompletedFlagForLogging.Value);
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL("https://everyplay.com/pixel-gun-3d--");
		}
	}

	private void HandleEnderClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Ender", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		if (Application.isEditor)
		{
			Debug.Log(MainMenu.GetEndermanUrl());
		}
		else
		{
			Application.OpenURL(MainMenu.GetEndermanUrl());
		}
	}

	private void HandlePostFacebookClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened && !MainMenuController.ShowBannerOrLevelup())
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				FlurryEvents.LogAfterTraining("Post Facebook", TrainingController.TrainingCompletedFlagForLogging.Value);
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			FacebookController.ShowPostDialog();
			FlurryPluginWrapper.LogFacebook();
			FlurryPluginWrapper.LogFreeCoinsFacebook();
		}
	}

	private void HandlePostTwittwerClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened || MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Post Twitter", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		FlurryPluginWrapper.LogTwitter();
		FlurryPluginWrapper.LogFreeCoinsTwitter();
		if (!Application.isEditor)
		{
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.PostStatusUpdate("Come and play with me in epic multiplayer shooter - Pixel Gun 3D! http://goo.gl/dQMf4n");
			}
		}
	}

	private void HandleRateAsClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened && !MainMenuController.ShowBannerOrLevelup())
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				FlurryEvents.LogAfterTraining("Rate", TrainingController.TrainingCompletedFlagForLogging.Value);
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			FlurryPluginWrapper.LogFreeCoinsRateUs();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL(MainMenuController.RateUsURL);
		}
	}

	private void HandleTwitterSubscribeClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened)
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				FlurryEvents.LogAfterTraining("Subscribe Twitter", TrainingController.TrainingCompletedFlagForLogging.Value);
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL("https://twitter.com/PixelGun3D");
		}
	}

	private void HandleFacebookSubscribeClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened)
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				FlurryEvents.LogAfterTraining("Subscribe Facebook", TrainingController.TrainingCompletedFlagForLogging.Value);
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL("http://pixelgun3d.com/facebook.html");
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__388(object sender, EventArgs e)
	{
		MainMenuController.sharedController._isCancellationRequested = true;
	}
}
