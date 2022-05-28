using System;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class PauseNGUIController : ControlsSettingsBase
{
	public static PauseNGUIController sharedController;

	public SettingsToggleButtons switchingWeaponsToggleButtons;

	public SettingsToggleButtons chatToggleButtons;

	public SettingsToggleButtons musicToggleButtons;

	public SettingsToggleButtons soundToggleButtons;

	public SettingsToggleButtons invertCameraToggleButtons;

	public SettingsToggleButtons recToggleButtons;

	public SettingsToggleButtons pressureToucheToggleButtons;

	public SettingsToggleButtons hideJumpAndShootButtons;

	public SettingsToggleButtons leftHandedToggleButtons;

	public SettingsToggleButtons fps60ToggleButtons;

	public UIButton controlsButton;

	public GameObject tapPanelInSettings;

	public GameObject swipePanelInSettings;

	public UISlider sensitivitySlider;

	public UIButton resumeButton;

	private IDisposable _backSubscription;

	private float _cachedSensitivity;

	private bool _shopOpened;

	private float _lastBackFromShopTime;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache18;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache19;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache1A;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache1B;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache1C;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache1D;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache1E;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache1F;

	[CompilerGenerated]
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache20;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache21;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache22;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache23;

	public static event Action InvertCamUpdated;

	public static event Action ChatSettUpdated;

	public static event Action PlayerHandUpdated;

	public static event Action SwitchingWeaponsUpdated;

	public static bool RecButtonsAvailable()
	{
		if (Application.isEditor)
		{
			return true;
		}
		if (VideoRecordingView.IsWeakDevice)
		{
			return false;
		}
		return EveryplayWrapper.Instance.IsRecordingSupported();
	}

	public static void Set60FPSEnable(bool isChecked, Action handler = null)
	{
		GlobalGameController.is60FPSEnable = !isChecked;
	}

	private new void Start()
	{
		base.Start();
		sharedController = this;
		resumeButton.GetComponent<ButtonHandler>().Clicked += HandleResumeButton;
		musicToggleButtons.IsChecked = Defs.isSoundMusic;
		SettingsToggleButtons settingsToggleButtons = musicToggleButtons;
		if (_003C_003Ef__am_0024cache18 == null)
		{
			_003C_003Ef__am_0024cache18 = _003CStart_003Em__1DE;
		}
		settingsToggleButtons.Clicked += _003C_003Ef__am_0024cache18;
		soundToggleButtons.IsChecked = Defs.isSoundFX;
		SettingsToggleButtons settingsToggleButtons2 = soundToggleButtons;
		if (_003C_003Ef__am_0024cache19 == null)
		{
			_003C_003Ef__am_0024cache19 = _003CStart_003Em__1DF;
		}
		settingsToggleButtons2.Clicked += _003C_003Ef__am_0024cache19;
		chatToggleButtons.IsChecked = Defs.IsChatOn;
		SettingsToggleButtons settingsToggleButtons3 = chatToggleButtons;
		if (_003C_003Ef__am_0024cache1A == null)
		{
			_003C_003Ef__am_0024cache1A = _003CStart_003Em__1E0;
		}
		settingsToggleButtons3.Clicked += _003C_003Ef__am_0024cache1A;
		invertCameraToggleButtons.IsChecked = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
		SettingsToggleButtons settingsToggleButtons4 = invertCameraToggleButtons;
		if (_003C_003Ef__am_0024cache1B == null)
		{
			_003C_003Ef__am_0024cache1B = _003CStart_003Em__1E1;
		}
		settingsToggleButtons4.Clicked += _003C_003Ef__am_0024cache1B;
		if (fps60ToggleButtons != null)
		{
			fps60ToggleButtons.IsChecked = !GlobalGameController.is60FPSEnable;
			SettingsToggleButtons settingsToggleButtons5 = fps60ToggleButtons;
			if (_003C_003Ef__am_0024cache1C == null)
			{
				_003C_003Ef__am_0024cache1C = _003CStart_003Em__1E2;
			}
			settingsToggleButtons5.Clicked += _003C_003Ef__am_0024cache1C;
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			pressureToucheToggleButtons.gameObject.SetActive(true);
			recToggleButtons.gameObject.SetActive(false);
			pressureToucheToggleButtons.IsChecked = Defs.isUse3DTouch;
			pressureToucheToggleButtons.Clicked += _003CStart_003Em__1E3;
		}
		else
		{
			pressureToucheToggleButtons.gameObject.SetActive(false);
			recToggleButtons.gameObject.SetActive(RecButtonsAvailable());
			recToggleButtons.IsChecked = GlobalGameController.ShowRec;
			SettingsToggleButtons settingsToggleButtons6 = recToggleButtons;
			if (_003C_003Ef__am_0024cache1D == null)
			{
				_003C_003Ef__am_0024cache1D = _003CStart_003Em__1E4;
			}
			settingsToggleButtons6.Clicked += _003C_003Ef__am_0024cache1D;
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			hideJumpAndShootButtons.gameObject.SetActive(Defs.isUse3DTouch);
			hideJumpAndShootButtons.IsChecked = Defs.isJumpAndShootButtonOn;
			SettingsToggleButtons settingsToggleButtons7 = hideJumpAndShootButtons;
			if (_003C_003Ef__am_0024cache1E == null)
			{
				_003C_003Ef__am_0024cache1E = _003CStart_003Em__1E5;
			}
			settingsToggleButtons7.Clicked += _003C_003Ef__am_0024cache1E;
		}
		else
		{
			hideJumpAndShootButtons.gameObject.SetActive(false);
		}
		if (sensitivitySlider != null)
		{
			float sensitivity = Defs.Sensitivity;
			float num = Mathf.Clamp(sensitivity, 6f, 19f);
			float num2 = num - 6f;
			sensitivitySlider.value = num2 / 13f;
			_cachedSensitivity = num;
		}
		else
		{
			Debug.LogWarning("sensitivitySlider == null");
		}
		if (leftHandedToggleButtons != null)
		{
			leftHandedToggleButtons.IsChecked = GlobalGameController.LeftHanded;
			SettingsToggleButtons settingsToggleButtons8 = leftHandedToggleButtons;
			if (_003C_003Ef__am_0024cache1F == null)
			{
				_003C_003Ef__am_0024cache1F = _003CStart_003Em__1E6;
			}
			settingsToggleButtons8.Clicked += _003C_003Ef__am_0024cache1F;
		}
		switchingWeaponsToggleButtons.IsChecked = !GlobalGameController.switchingWeaponSwipe;
		SettingsToggleButtons settingsToggleButtons9 = switchingWeaponsToggleButtons;
		if (_003C_003Ef__am_0024cache20 == null)
		{
			_003C_003Ef__am_0024cache20 = _003CStart_003Em__1E7;
		}
		settingsToggleButtons9.Clicked += _003C_003Ef__am_0024cache20;
		if (controlsButton != null)
		{
			controlsButton.GetComponent<ButtonHandler>().Clicked += _003CStart_003Em__1E8;
		}
	}

	private void HandleResumeButton(object sender, EventArgs e)
	{
		if (!InPauseShop())
		{
			base.gameObject.SetActive(false);
		}
	}

	private bool InPauseShop()
	{
		return InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null && InGameGUI.sharedInGameGUI.playerMoveC.isInappWinOpen;
	}

	private void Update()
	{
		if (_isCancellationRequested)
		{
			if (SettingsJoysticksPanel.activeInHierarchy)
			{
				HandleCancelPosJoystikClicked(this, null);
			}
			else if (!InPauseShop())
			{
				HandleResumeButton(this, null);
				_isCancellationRequested = false;
				return;
			}
			_isCancellationRequested = false;
		}
		float num = sensitivitySlider.value * 13f;
		float num2 = Mathf.Clamp(num + 6f, 6f, 19f);
		if (_cachedSensitivity != num2)
		{
			if (Application.isEditor)
			{
				Debug.Log("New sensitivity: " + num2);
			}
			Defs.Sensitivity = num2;
			_cachedSensitivity = num2;
		}
		if (!InPauseShop())
		{
			if (_shopOpened)
			{
				_lastBackFromShopTime = Time.realtimeSinceStartup;
			}
			_shopOpened = false;
			if (!Defs.isMulti)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
		}
		else
		{
			_shopOpened = true;
			_lastBackFromShopTime = float.PositiveInfinity;
		}
	}

	private new void OnEnable()
	{
		base.OnEnable();
		if (!InPauseShop())
		{
			ExperienceController.sharedController.isShowRanks = true;
			ExperienceController.sharedController.posRanks = NetworkStartTable.ExperiencePosRanks;
		}
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(HandleEscape, "Pause");
	}

	private void OnDestroy()
	{
		sharedController = null;
	}

	private void OnDisable()
	{
		if (!InPauseShop())
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void HandleEscape()
	{
		if (!InPauseShop())
		{
			_isCancellationRequested = true;
		}
	}

	protected override void HandleSavePosJoystikClicked(object sender, EventArgs e)
	{
		base.HandleSavePosJoystikClicked(sender, e);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	protected override void HandleCancelPosJoystikClicked(object sender, EventArgs e)
	{
		SettingsJoysticksPanel.SetActive(false);
		settingsPanel.SetActive(true);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1DE(object sender, ToggleButtonEventArgs e)
	{
		bool isSoundMusic = Defs.isSoundMusic;
		Defs.isSoundMusic = e.IsChecked;
		PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
		PlayerPrefs.Save();
		if (isSoundMusic == Defs.isSoundMusic || isSoundMusic == Defs.isSoundMusic)
		{
			return;
		}
		if (Defs.isSoundMusic)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
			if (MenuBackgroundMusic.sharedMusic != null && gameObject != null)
			{
				AudioSource component = gameObject.GetComponent<AudioSource>();
				if (component != null)
				{
					MenuBackgroundMusic.sharedMusic.PlayMusic(component);
				}
			}
			return;
		}
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("BackgroundMusic");
		if (MenuBackgroundMusic.sharedMusic != null && gameObject2 != null)
		{
			AudioSource component2 = gameObject2.GetComponent<AudioSource>();
			if (component2 != null)
			{
				MenuBackgroundMusic.sharedMusic.StopMusic(component2);
			}
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1DF(object sender, ToggleButtonEventArgs e)
	{
		Defs.isSoundFX = e.IsChecked;
		PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
		PlayerPrefs.Save();
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1E0(object sender, ToggleButtonEventArgs e)
	{
		bool isChecked = e.IsChecked;
		if (_003C_003Ef__am_0024cache21 == null)
		{
			_003C_003Ef__am_0024cache21 = _003CStart_003Em__1E9;
		}
		SettingsController.SwitchChatSetting(isChecked, _003C_003Ef__am_0024cache21);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1E1(object sender, ToggleButtonEventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Invert Camera] button clicked: " + e.IsChecked);
		}
		bool flag = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
		if (flag != e.IsChecked)
		{
			PlayerPrefs.SetInt(Defs.InvertCamSN, Convert.ToInt32(e.IsChecked));
			PlayerPrefs.Save();
			Action invertCamUpdated = PauseNGUIController.InvertCamUpdated;
			if (invertCamUpdated != null)
			{
				invertCamUpdated();
			}
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1E2(object sender, ToggleButtonEventArgs e)
	{
		Set60FPSEnable(e.IsChecked);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1E3(object sender, ToggleButtonEventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("3D touche button clicked: " + e.IsChecked);
		}
		Defs.isUse3DTouch = e.IsChecked;
		hideJumpAndShootButtons.gameObject.SetActive(Defs.isUse3DTouch);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1E4(object sender, ToggleButtonEventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Rec. Buttons] button clicked: " + e.IsChecked);
		}
		if (GlobalGameController.ShowRec != e.IsChecked)
		{
			GlobalGameController.ShowRec = e.IsChecked;
			PlayerPrefs.SetInt(Defs.ShowRecSN, e.IsChecked ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1E5(object sender, ToggleButtonEventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("3D touche button clicked: " + e.IsChecked);
		}
		Defs.isJumpAndShootButtonOn = e.IsChecked;
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1E6(object sender, ToggleButtonEventArgs e)
	{
		bool isChecked = e.IsChecked;
		if (_003C_003Ef__am_0024cache22 == null)
		{
			_003C_003Ef__am_0024cache22 = _003CStart_003Em__1EA;
		}
		SettingsController.ChangeLeftHandedRightHanded(isChecked, _003C_003Ef__am_0024cache22);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1E7(object sender, ToggleButtonEventArgs e)
	{
		bool isChecked = e.IsChecked;
		if (_003C_003Ef__am_0024cache23 == null)
		{
			_003C_003Ef__am_0024cache23 = _003CStart_003Em__1EB;
		}
		SettingsController.ChangeSwitchingWeaponHanded(isChecked, _003C_003Ef__am_0024cache23);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1E8(object sender, EventArgs e)
	{
		if (!InPauseShop())
		{
			ButtonClickSound.Instance.PlayClick();
			settingsPanel.SetActive(false);
			SettingsJoysticksPanel.SetActive(true);
			swipePanelInSettings.transform.parent.gameObject.SetActive(!Defs.isDaterRegim || !GlobalGameController.switchingWeaponSwipe);
			swipePanelInSettings.SetActive(Defs.isDaterRegim && GlobalGameController.switchingWeaponSwipe);
			tapPanelInSettings.SetActive(!GlobalGameController.switchingWeaponSwipe);
			ExperienceController.sharedController.isShowRanks = false;
			ExpController.Instance.InterfaceEnabled = false;
			HandleControlsClicked();
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1E9()
	{
		Action chatSettUpdated = PauseNGUIController.ChatSettUpdated;
		if (chatSettUpdated != null)
		{
			chatSettUpdated();
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1EA()
	{
		Action playerHandUpdated = PauseNGUIController.PlayerHandUpdated;
		if (playerHandUpdated != null)
		{
			playerHandUpdated();
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__1EB()
	{
		Action switchingWeaponsUpdated = PauseNGUIController.SwitchingWeaponsUpdated;
		if (switchingWeaponsUpdated != null)
		{
			switchingWeaponsUpdated();
		}
	}
}
