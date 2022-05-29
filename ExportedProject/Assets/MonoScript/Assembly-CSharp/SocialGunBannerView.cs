using I2.Loc;
using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public sealed class SocialGunBannerView : BannerWindow
{
	public bool freePanelBanner;

	public List<UILabel> rewardLabels;

	private IDisposable _backSubscription;

	public SocialGunBannerView()
	{
	}

	private void Awake()
	{
		this.SetRewardLabelsText();
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	public void Continue()
	{
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(() => FacebookController.Login(() => this.FireSocialGunBannerViewLoginCompletedEvent(true), () => this.FireSocialGunBannerViewLoginCompletedEvent(false), "Social Gun Banner", null), () => FacebookController.Login(null, null, "Social Gun Banner", null));
	}

	private void FireSocialGunBannerViewLoginCompletedEvent(bool val)
	{
		Action<bool> action = SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult;
		if (action != null)
		{
			action(val);
		}
	}

	private void HandleEscape()
	{
		this.HideWindow();
	}

	private void HandleLocalizationChanged()
	{
		this.SetRewardLabelsText();
	}

	public void HideWindow()
	{
		ButtonClickSound.TryPlayClick();
		if (!this.freePanelBanner)
		{
			BannerWindowController sharedController = BannerWindowController.SharedController;
			if (sharedController != null)
			{
				sharedController.HideBannerWindow();
				return;
			}
		}
		base.Hide();
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
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
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Social Gun");
	}

	private void SetRewardLabelsText()
	{
		foreach (UILabel rewardLabel in this.rewardLabels)
		{
			rewardLabel.text = string.Format(LocalizationStore.Get("Key_1531"), 10);
		}
	}

	public override void Show()
	{
		base.Show();
		if (FacebookController.sharedController != null)
		{
			FacebookController.sharedController.UpdateCountShownWindowByShowCondition();
		}
	}

	public static event Action<bool> SocialGunBannerViewLoginCompletedWithResult;
}