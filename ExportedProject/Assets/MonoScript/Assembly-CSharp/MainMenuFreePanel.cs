using Facebook.Unity;
using Rilisoft;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainMenuFreePanel : MonoBehaviour
{
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

	public MainMenuFreePanel()
	{
	}

	private void HandleEnderClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Ender", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		if (!Application.isEditor)
		{
			Application.OpenURL(MainMenu.GetEndermanUrl());
		}
		else
		{
			Debug.Log(MainMenu.GetEndermanUrl());
		}
	}

	private void HandleEveryPlayClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Everyplay", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL("https://everyplay.com/pixel-gun-3d--");
	}

	private void HandleFacebookSubscribeClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Subscribe Facebook", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL("http://pixelgun3d.com/facebook.html");
	}

	private void HandlePostFacebookClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
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

	private void HandlePostTwittwerClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
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
				TwitterController.Instance.PostStatusUpdate("Come and play with me in epic multiplayer shooter - Pixel Gun 3D! http://goo.gl/dQMf4n", null);
			}
		}
	}

	private void HandleRateAsClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Rate", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		FlurryPluginWrapper.LogFreeCoinsRateUs();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL(MainMenuController.RateUsURL);
	}

	private void HandleSocialGunEventStateChanged(bool enable)
	{
		this._socialGunPanel.gameObject.SetActive(enable);
		base.GetComponentsInChildren<RewardedLikeButton>(true).ForEach<RewardedLikeButton>((RewardedLikeButton b) => b.gameObject.SetActive(!enable));
		if (FacebookController.sharedController != null)
		{
			this._socialGunEventTimerLabel.text = string.Empty;
		}
	}

	private void HandleTwitterSubscribeClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Subscribe Twitter", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL("https://twitter.com/PixelGun3D");
	}

	private void HandleYoutubeClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("YouTube", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL("http://www.youtube.com/channel/UCsClw1gnMrmF6ssIB_166_Q");
	}

	private void OnDestroy()
	{
		FacebookController.SocialGunEventStateChanged -= new Action<bool>(this.HandleSocialGunEventStateChanged);
	}

	public void OnSocialGunButtonClicked()
	{
		MainMenuController.sharedController.OnSocialGunEventButtonClick();
	}

	public void SetVisible(bool visible)
	{
		if (base.gameObject.activeSelf != visible)
		{
			base.gameObject.SetActive(visible);
		}
	}

	private void Start()
	{
		this._postNewsLabel.SetActive(false);
		if (this._youtubeButton != null)
		{
			this._youtubeButton.Clicked += new EventHandler(this.HandleYoutubeClicked);
		}
		if (this._everyplayButton != null)
		{
			this._everyplayButton.Clicked += new EventHandler(this.HandleEveryPlayClicked);
		}
		if (this._enderManButton != null)
		{
			this._enderManButton.Clicked += new EventHandler(this.HandleEnderClicked);
		}
		if (this._postFacebookButton != null)
		{
			this._postFacebookButton.Clicked += new EventHandler(this.HandlePostFacebookClicked);
		}
		if (this._postTwitterButton != null)
		{
			this._postTwitterButton.Clicked += new EventHandler(this.HandlePostTwittwerClicked);
		}
		if (this._rateUsButton != null)
		{
			this._rateUsButton.Clicked += new EventHandler(this.HandleRateAsClicked);
		}
		if (this._twitterSubcribeButton != null)
		{
			this._twitterSubcribeButton.Clicked += new EventHandler(this.HandleTwitterSubscribeClicked);
		}
		if (this._facebookSubcribeButton != null)
		{
			this._facebookSubcribeButton.Clicked += new EventHandler(this.HandleFacebookSubscribeClicked);
		}
		if (this._backButton != null)
		{
			this._backButton.Clicked += new EventHandler((object sender, EventArgs e) => MainMenuController.sharedController._isCancellationRequested = true);
		}
		FacebookController.SocialGunEventStateChanged += new Action<bool>(this.HandleSocialGunEventStateChanged);
		if (FacebookController.sharedController != null)
		{
			this.HandleSocialGunEventStateChanged(FacebookController.sharedController.SocialGunEventActive);
		}
	}

	private void Update()
	{
		bool flag = (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled ? !ShopNGUIController.GuiActive : false);
		if (this._starParticleSocialGunButton != null && this._starParticleSocialGunButton.activeInHierarchy != flag)
		{
			this._starParticleSocialGunButton.SetActive(flag);
		}
		if (this._postFacebookButton.gameObject.activeSelf != (!FacebookController.FacebookSupported || !FacebookController.FacebookPost_Old_Supported ? false : FB.IsLoggedIn))
		{
			this._postFacebookButton.gameObject.SetActive((!FacebookController.FacebookSupported || !FacebookController.FacebookPost_Old_Supported ? false : FB.IsLoggedIn));
		}
		if (this._postTwitterButton.gameObject.activeSelf != (!TwitterController.TwitterSupported || !TwitterController.TwitterSupported_OldPosts ? false : TwitterController.IsLoggedIn))
		{
			this._postTwitterButton.gameObject.SetActive((!TwitterController.TwitterSupported || !TwitterController.TwitterSupported_OldPosts ? false : TwitterController.IsLoggedIn));
		}
		if (FacebookController.sharedController != null && FacebookController.sharedController.SocialGunEventActive)
		{
			this._socialGunEventTimerLabel.text = string.Empty;
		}
	}
}