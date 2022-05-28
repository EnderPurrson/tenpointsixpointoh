using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rilisoft;

public sealed class SocialGunBannerView : BannerWindow
{
	public bool freePanelBanner;

	public List<UILabel> rewardLabels;

	private IDisposable _backSubscription;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache4;

	public static event Action<bool> SocialGunBannerViewLoginCompletedWithResult;

	private void SetRewardLabelsText()
	{
		foreach (UILabel rewardLabel in rewardLabels)
		{
			rewardLabel.text = string.Format(LocalizationStore.Get("Key_1531"), 10);
		}
	}

	private void Awake()
	{
		SetRewardLabelsText();
		LocalizationStore.AddEventCallAfterLocalize(HandleLocalizationChanged);
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(HandleEscape, "Social Gun");
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
		HideWindow();
	}

	public override void Show()
	{
		base.Show();
		if (FacebookController.sharedController != null)
		{
			FacebookController.sharedController.UpdateCountShownWindowByShowCondition();
		}
	}

	public void HideWindow()
	{
		ButtonClickSound.TryPlayClick();
		if (!freePanelBanner)
		{
			BannerWindowController sharedController = BannerWindowController.SharedController;
			if (sharedController != null)
			{
				sharedController.HideBannerWindow();
				return;
			}
		}
		Hide();
	}

	public void Continue()
	{
		Action action = _003CContinue_003Em__44D;
		if (_003C_003Ef__am_0024cache4 == null)
		{
			_003C_003Ef__am_0024cache4 = _003CContinue_003Em__44E;
		}
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(action, _003C_003Ef__am_0024cache4);
	}

	private void FireSocialGunBannerViewLoginCompletedEvent(bool val)
	{
		Action<bool> socialGunBannerViewLoginCompletedWithResult = SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult;
		if (socialGunBannerViewLoginCompletedWithResult != null)
		{
			socialGunBannerViewLoginCompletedWithResult(val);
		}
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(HandleLocalizationChanged);
	}

	private void HandleLocalizationChanged()
	{
		SetRewardLabelsText();
	}

	[CompilerGenerated]
	private void _003CContinue_003Em__44D()
	{
		FacebookController.Login(_003CContinue_003Em__44F, _003CContinue_003Em__450, "Social Gun Banner");
	}

	[CompilerGenerated]
	private static void _003CContinue_003Em__44E()
	{
		FacebookController.Login(null, null, "Social Gun Banner");
	}

	[CompilerGenerated]
	private void _003CContinue_003Em__44F()
	{
		FireSocialGunBannerViewLoginCompletedEvent(true);
	}

	[CompilerGenerated]
	private void _003CContinue_003Em__450()
	{
		FireSocialGunBannerViewLoginCompletedEvent(false);
	}
}
