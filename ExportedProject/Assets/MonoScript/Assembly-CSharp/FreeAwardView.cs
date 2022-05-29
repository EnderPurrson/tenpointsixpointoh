using I2.Loc;
using Rilisoft;
using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class FreeAwardView : MonoBehaviour
{
	public GameObject backgroundPanel;

	public GameObject waitingPanel;

	public GameObject watchForCoinsPanel;

	public GameObject watchForGemsPanel;

	public GameObject connectionPanel;

	public GameObject awardPanelCoins;

	public GameObject awardPanelGems;

	public GameObject closePanel;

	public UILabel watchHeader;

	public UILabel watchTimer;

	public UIButton nguiWatchButton;

	public UIButton devSkipButton;

	public UILabel prizeMoneyLabel;

	public UISprite currencySprite;

	public UITexture loadingSpinner;

	public UILabel awardOuterLabelCoins;

	public UILabel awardOuterLabelGems;

	private FreeAwardController.State _currentState;

	private readonly Lazy<UILabel[]> _watchTimerLabels;

	internal FreeAwardController.State CurrentState
	{
		private get
		{
			return this._currentState;
		}
		set
		{
			if (value != this._currentState)
			{
				FreeAwardController.WatchState watchState = value as FreeAwardController.WatchState;
				if (watchState == null)
				{
					this.SetWatchButtonEnabled(false);
				}
				else
				{
					TimeSpan estimatedTimeSpan = watchState.GetEstimatedTimeSpan();
					this.SetWatchButtonEnabled(estimatedTimeSpan <= TimeSpan.FromMinutes(0), estimatedTimeSpan);
				}
				this.RefreshAwardLabel(watchState != null);
			}
			if (this.backgroundPanel != null)
			{
				this.backgroundPanel.SetActive(!(value is FreeAwardController.IdleState));
			}
			if (this.waitingPanel != null)
			{
				this.waitingPanel.SetActive(value is FreeAwardController.WaitingState);
			}
			if (this.connectionPanel != null)
			{
				this.connectionPanel.SetActive(value is FreeAwardController.ConnectionState);
			}
			if (this.closePanel != null)
			{
				this.closePanel.SetActive(value is FreeAwardController.CloseState);
			}
			if (!(value is FreeAwardController.WatchState))
			{
				this.watchForGemsPanel.SetActive(false);
				this.watchForCoinsPanel.SetActive(false);
			}
			else if (FreeAwardController.Instance.CurrencyForAward != "GemsCurrency")
			{
				this.watchForGemsPanel.SetActive(false);
				this.watchForCoinsPanel.SetActive(true);
			}
			else
			{
				this.watchForGemsPanel.SetActive(true);
				this.watchForCoinsPanel.SetActive(false);
			}
			if (!(value is FreeAwardController.AwardState))
			{
				this.awardPanelCoins.SetActive(false);
				this.awardPanelGems.SetActive(false);
			}
			else if (FreeAwardController.Instance.CurrencyForAward != "GemsCurrency")
			{
				this.awardPanelCoins.SetActive(true);
			}
			else
			{
				this.awardPanelGems.SetActive(true);
			}
			this._currentState = value;
		}
	}

	public FreeAwardView()
	{
		this._watchTimerLabels = new Lazy<UILabel[]>(new Func<UILabel[]>(this.InitializeWatchTimerLabels));
	}

	private UILabel[] InitializeWatchTimerLabels()
	{
		if (this.watchTimer == null)
		{
			return new UILabel[0];
		}
		List<UILabel> uILabels = new List<UILabel>(3)
		{
			this.watchTimer
		};
		this.watchTimer.GetComponentsInChildren<UILabel>(true, uILabels);
		return uILabels.ToArray();
	}

	private void RefreshAwardLabel(bool visible)
	{
		if (!visible)
		{
			return;
		}
		string str = LocalizationStore.Get(ScriptLocalization.Key_0291);
		if (PromoActionsManager.MobileAdvert != null)
		{
			int num = (!MobileAdManager.IsPayingUser() ? PromoActionsManager.MobileAdvert.AwardCoinsNonpaying : PromoActionsManager.MobileAdvert.AwardCoinsPaying);
			str = string.Concat(str, (FreeAwardController.Instance.CurrencyForAward != "GemsCurrency" ? num.ToString() : string.Format(" [c][50CEFFFF]{0}[-][/c]", num)));
		}
		else
		{
			str = string.Concat(str, " 1");
		}
		List<UILabel> uILabels = new List<UILabel>();
		uILabels.AddRange(this.awardOuterLabelCoins.gameObject.GetComponentsInChildren<UILabel>(true));
		uILabels.AddRange(this.awardOuterLabelGems.gameObject.GetComponentsInChildren<UILabel>(true));
		foreach (UILabel uILabel in uILabels)
		{
			uILabel.text = str;
		}
		this.currencySprite.spriteName = (FreeAwardController.Instance.CurrencyForAward != "GemsCurrency" ? "ingame_coin" : "gem_znachek");
	}

	private void SetWatchButtonEnabled(bool enabled, TimeSpan nextTimeAwailable)
	{
		if (this.nguiWatchButton != null)
		{
			this.nguiWatchButton.isEnabled = enabled;
		}
		if (this.watchHeader != null)
		{
			this.watchHeader.gameObject.SetActive(enabled);
		}
		if (this.watchTimer != null)
		{
			this.watchTimer.transform.parent.gameObject.SetActive(!enabled);
			if (!enabled)
			{
				string str = (nextTimeAwailable.Hours <= 0 ? string.Format("{0}:{1:D2}", nextTimeAwailable.Minutes, nextTimeAwailable.Seconds) : string.Format("{0}:{1:D2}:{2:D2}", nextTimeAwailable.Hours, nextTimeAwailable.Minutes, nextTimeAwailable.Seconds));
				UILabel[] value = this._watchTimerLabels.Value;
				for (int i = 0; i < (int)value.Length; i++)
				{
					value[i].text = str;
				}
			}
		}
	}

	private void SetWatchButtonEnabled(bool enabled)
	{
		this.SetWatchButtonEnabled(enabled, new TimeSpan());
	}

	private void Start()
	{
		bool flag;
		if (this.devSkipButton != null)
		{
			GameObject gameObject = this.devSkipButton.gameObject;
			if (Application.isEditor)
			{
				flag = true;
			}
			else
			{
				flag = (!Defs.IsDeveloperBuild ? false : BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64);
			}
			gameObject.SetActive(flag);
		}
	}

	private void Update()
	{
		FreeAwardController.WaitingState currentState = this.CurrentState as FreeAwardController.WaitingState;
		if (currentState != null && this.loadingSpinner != null)
		{
			float startTime = Time.realtimeSinceStartup - currentState.StartTime;
			int num = Convert.ToInt32(Mathf.Floor(startTime));
			this.loadingSpinner.invert = num % 2 == 0;
			this.loadingSpinner.fillAmount = (!this.loadingSpinner.invert ? 1f - startTime + (float)num : startTime - (float)num);
		}
		FreeAwardController.WatchState watchState = this.CurrentState as FreeAwardController.WatchState;
		if (watchState != null && Time.frameCount % 10 == 0)
		{
			TimeSpan estimatedTimeSpan = watchState.GetEstimatedTimeSpan();
			this.SetWatchButtonEnabled(estimatedTimeSpan <= TimeSpan.FromMinutes(0), estimatedTimeSpan);
		}
	}
}