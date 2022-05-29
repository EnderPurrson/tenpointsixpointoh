using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class DeveloperConsoleController : MonoBehaviour
{
	public static DeveloperConsoleController instance;

	public DeveloperConsoleView view;

	public static bool isDebugGuiVisible;

	public bool isMiniConsole;

	private int sliderLevel;

	private IDisposable _escapeSubscription;

	public UIToggle buffToogle;

	public UIToggle ratingToogle;

	private bool? _enemiesInCampaignDirty;

	private bool _backRequested;

	private bool _initialized;

	private bool _needsRestart;

	static DeveloperConsoleController()
	{
	}

	public DeveloperConsoleController()
	{
	}

	private void Awake()
	{
		DeveloperConsoleController.instance = this;
	}

	public void ChangePremiumAccountLiveTime(UIInput input)
	{
	}

	public void ClearAllPremiumAccounts()
	{
	}

	public void ClearCurrentPremiumAccont()
	{
	}

	public void ClearStarterPackData()
	{
	}

	public void FillAll()
	{
	}

	public void HandleAddCoinsButton()
	{
	}

	public void HandleAddGemsButton()
	{
	}

	public void HandleAdIdCanged(UIToggle toggle)
	{
	}

	public void HandleBackButton()
	{
		this._backRequested = true;
	}

	public void HandleClearCloud()
	{
	}

	public void HandleClearKeychainAndPlayerPrefs()
	{
	}

	public void HandleClearProgressButton()
	{
	}

	public void HandleClearPurchasesButton()
	{
	}

	public void HandleClearX3()
	{
	}

	public void HandleCoinsInputSubmit(UIInput input)
	{
		if (!input.isActiveAndEnabled)
		{
			return;
		}
	}

	public void HandleEnemiesInCampaignChange()
	{
	}

	public void HandleEnemiesInCampaignInput(UIInput input)
	{
	}

	public void HandleEnemyCountInSurvivalWaveInput(UIInput input)
	{
	}

	private void HandleEscape()
	{
		this._backRequested = true;
	}

	public void HandleExperienceSliderChanged()
	{
	}

	public void HandleFacebookLoginReward(UIToggle toggle)
	{
	}

	public void HandleFillGunsButton()
	{
	}

	public void HandleFillProgressButton()
	{
	}

	public void HandleForcedEventX3Changed(UIToggle toggle)
	{
	}

	private void HandleGemsInputSubmit(UIInput input)
	{
		if (!input.isActiveAndEnabled)
		{
			return;
		}
	}

	public void HandleInvalidateQuestConfig(UILabel label)
	{
	}

	public void HandleIpadMiniRetinaChanged(UIToggle toggle)
	{
	}

	public void HandleIsDebugGuiVisibleChanged(UIToggle toggle)
	{
	}

	public void HandleIsPayingChanged(UIToggle toggle)
	{
	}

	public void HandleIsPixelGunLowChanged(UIToggle toggle)
	{
	}

	public void HandleLevelChanged()
	{
	}

	public void HandleLevelMinusButton()
	{
	}

	public void HandleLevelPlusButton()
	{
	}

	public void HandleLevelSliderChanged()
	{
	}

	public void HandleMouseControlChanged(UIToggle toggle)
	{
	}

	public void HandleRatingSliderChanged()
	{
	}

	public void HandleSet60FpsChanged(UIToggle toggle)
	{
	}

	public void HandleSignInOuButton(UILabel socialUsernameLabel)
	{
	}

	public void HandleSpectatorMode(UIToggle toggle)
	{
	}

	public void HandleStrongDeviceChanged(UIToggle toggle)
	{
	}

	public void HandleTempGunChanged(UIToggle toggle)
	{
	}

	public void HandleTipsShownButton()
	{
	}

	public void HandleTrainingCompleteChanged(UIToggle toggle)
	{
	}

	public void HandleUnbanUs(UIButton butt)
	{
	}

	public void OnChangeReviewActive()
	{
	}

	public void OnChangeStarterPackCooldown(UIInput inputField)
	{
	}

	public void OnChangeStarterPackLive(UIInput inputField)
	{
	}

	public void OnChangeStateMemoryInfo()
	{
	}

	public void OnClickRating()
	{
	}

	public void OnClickSystemBuff()
	{
	}

	private void OnDestroy()
	{
		DeveloperConsoleController.instance = null;
	}

	private void OnDisable()
	{
		if (this._escapeSubscription != null)
		{
			this._escapeSubscription.Dispose();
			this._escapeSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._escapeSubscription != null)
		{
			this._escapeSubscription.Dispose();
		}
		this._escapeSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "DevConsole");
	}

	private void Refresh()
	{
	}

	private void RefreshExperience()
	{
	}

	private void RefreshLevel()
	{
	}

	private void RefreshLevelSlider()
	{
	}

	private void RefreshRating(bool current)
	{
	}

	private static void SetItemsBought(bool bought, bool onlyGuns = true)
	{
	}

	public void SetMarathonCurrentDay(UIInput input)
	{
	}

	public void SetMarathonTestMode(UIToggle toggle)
	{
	}

	public void SetOffGameGUIMode(UIToggle toggle)
	{
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		return new DeveloperConsoleController.u003cStartu003ec__Iterator1B5();
	}

	private void Update()
	{
	}

	public void UpdateStateActiveMemoryInfo()
	{
	}
}