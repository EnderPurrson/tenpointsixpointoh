using Rilisoft;
using System;
using System.Runtime.CompilerServices;
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

	static PauseNGUIController()
	{
	}

	public PauseNGUIController()
	{
	}

	protected override void HandleCancelPosJoystikClicked(object sender, EventArgs e)
	{
		this.SettingsJoysticksPanel.SetActive(false);
		this.settingsPanel.SetActive(true);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	private void HandleEscape()
	{
		if (!this.InPauseShop())
		{
			this._isCancellationRequested = true;
		}
	}

	private void HandleResumeButton(object sender, EventArgs e)
	{
		if (this.InPauseShop())
		{
			return;
		}
		base.gameObject.SetActive(false);
	}

	protected override void HandleSavePosJoystikClicked(object sender, EventArgs e)
	{
		base.HandleSavePosJoystikClicked(sender, e);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	private bool InPauseShop()
	{
		return (!(InGameGUI.sharedInGameGUI != null) || !(InGameGUI.sharedInGameGUI.playerMoveC != null) ? false : InGameGUI.sharedInGameGUI.playerMoveC.isInappWinOpen);
	}

	private void OnDestroy()
	{
		PauseNGUIController.sharedController = null;
	}

	private void OnDisable()
	{
		if (!this.InPauseShop())
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private new void OnEnable()
	{
		base.OnEnable();
		if (!this.InPauseShop())
		{
			ExperienceController.sharedController.isShowRanks = true;
			ExperienceController.sharedController.posRanks = NetworkStartTable.ExperiencePosRanks;
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Pause");
	}

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
		PauseNGUIController.sharedController = this;
		this.resumeButton.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleResumeButton);
		this.musicToggleButtons.IsChecked = Defs.isSoundMusic;
		this.musicToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			bool flag = Defs.isSoundMusic;
			Defs.isSoundMusic = e.IsChecked;
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
			PlayerPrefs.Save();
			if (flag != Defs.isSoundMusic && flag != Defs.isSoundMusic)
			{
				if (!Defs.isSoundMusic)
				{
					GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
					if (MenuBackgroundMusic.sharedMusic != null && gameObject != null)
					{
						AudioSource component = gameObject.GetComponent<AudioSource>();
						if (component != null)
						{
							MenuBackgroundMusic.sharedMusic.StopMusic(component);
						}
					}
				}
				else
				{
					GameObject gameObject1 = GameObject.FindGameObjectWithTag("BackgroundMusic");
					if (MenuBackgroundMusic.sharedMusic != null && gameObject1 != null)
					{
						AudioSource audioSource = gameObject1.GetComponent<AudioSource>();
						if (audioSource != null)
						{
							MenuBackgroundMusic.sharedMusic.PlayMusic(audioSource);
						}
					}
				}
			}
		});
		this.soundToggleButtons.IsChecked = Defs.isSoundFX;
		this.soundToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			Defs.isSoundFX = e.IsChecked;
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
			PlayerPrefs.Save();
		});
		this.chatToggleButtons.IsChecked = Defs.IsChatOn;
		this.chatToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => SettingsController.SwitchChatSetting(e.IsChecked, () => {
			Action action = PauseNGUIController.ChatSettUpdated;
			if (action != null)
			{
				action();
			}
		}));
		this.invertCameraToggleButtons.IsChecked = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
		this.invertCameraToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			if (Application.isEditor)
			{
				Debug.Log(string.Concat("[Invert Camera] button clicked: ", e.IsChecked));
			}
			if (PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1 != e.IsChecked)
			{
				PlayerPrefs.SetInt(Defs.InvertCamSN, Convert.ToInt32(e.IsChecked));
				PlayerPrefs.Save();
				Action action = PauseNGUIController.InvertCamUpdated;
				if (action != null)
				{
					action();
				}
			}
		});
		if (this.fps60ToggleButtons != null)
		{
			this.fps60ToggleButtons.IsChecked = !GlobalGameController.is60FPSEnable;
			this.fps60ToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => PauseNGUIController.Set60FPSEnable(e.IsChecked, null));
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			this.pressureToucheToggleButtons.gameObject.SetActive(true);
			this.recToggleButtons.gameObject.SetActive(false);
			this.pressureToucheToggleButtons.IsChecked = Defs.isUse3DTouch;
			this.pressureToucheToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
				if (Application.isEditor)
				{
					Debug.Log(string.Concat("3D touche button clicked: ", e.IsChecked));
				}
				Defs.isUse3DTouch = e.IsChecked;
				this.hideJumpAndShootButtons.gameObject.SetActive(Defs.isUse3DTouch);
			});
		}
		else
		{
			this.pressureToucheToggleButtons.gameObject.SetActive(false);
			this.recToggleButtons.gameObject.SetActive(PauseNGUIController.RecButtonsAvailable());
			this.recToggleButtons.IsChecked = GlobalGameController.ShowRec;
			this.recToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
				if (Application.isEditor)
				{
					Debug.Log(string.Concat("[Rec. Buttons] button clicked: ", e.IsChecked));
				}
				if (GlobalGameController.ShowRec != e.IsChecked)
				{
					GlobalGameController.ShowRec = e.IsChecked;
					PlayerPrefs.SetInt(Defs.ShowRecSN, (!e.IsChecked ? 0 : 1));
					PlayerPrefs.Save();
				}
			});
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			this.hideJumpAndShootButtons.gameObject.SetActive(Defs.isUse3DTouch);
			this.hideJumpAndShootButtons.IsChecked = Defs.isJumpAndShootButtonOn;
			this.hideJumpAndShootButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
				if (Application.isEditor)
				{
					Debug.Log(string.Concat("3D touche button clicked: ", e.IsChecked));
				}
				Defs.isJumpAndShootButtonOn = e.IsChecked;
			});
		}
		else
		{
			this.hideJumpAndShootButtons.gameObject.SetActive(false);
		}
		if (this.sensitivitySlider == null)
		{
			Debug.LogWarning("sensitivitySlider == null");
		}
		else
		{
			float single = Mathf.Clamp(Defs.Sensitivity, 6f, 19f);
			this.sensitivitySlider.@value = (float)(single - 6f) / 13f;
			this._cachedSensitivity = (float)single;
		}
		if (this.leftHandedToggleButtons != null)
		{
			this.leftHandedToggleButtons.IsChecked = GlobalGameController.LeftHanded;
			this.leftHandedToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => SettingsController.ChangeLeftHandedRightHanded(e.IsChecked, () => {
				Action action = PauseNGUIController.PlayerHandUpdated;
				if (action != null)
				{
					action();
				}
			}));
		}
		this.switchingWeaponsToggleButtons.IsChecked = !GlobalGameController.switchingWeaponSwipe;
		this.switchingWeaponsToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => SettingsController.ChangeSwitchingWeaponHanded(e.IsChecked, () => {
			Action action = PauseNGUIController.SwitchingWeaponsUpdated;
			if (action != null)
			{
				action();
			}
		}));
		if (this.controlsButton != null)
		{
			this.controlsButton.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
				if (this.InPauseShop())
				{
					return;
				}
				ButtonClickSound.Instance.PlayClick();
				this.settingsPanel.SetActive(false);
				this.SettingsJoysticksPanel.SetActive(true);
				this.swipePanelInSettings.transform.parent.gameObject.SetActive((!Defs.isDaterRegim ? 0 : (int)GlobalGameController.switchingWeaponSwipe) == 0);
				this.swipePanelInSettings.SetActive((!Defs.isDaterRegim ? false : GlobalGameController.switchingWeaponSwipe));
				this.tapPanelInSettings.SetActive(!GlobalGameController.switchingWeaponSwipe);
				ExperienceController.sharedController.isShowRanks = false;
				ExpController.Instance.InterfaceEnabled = false;
				base.HandleControlsClicked();
			});
		}
	}

	private void Update()
	{
		if (this._isCancellationRequested)
		{
			if (this.SettingsJoysticksPanel.activeInHierarchy)
			{
				this.HandleCancelPosJoystikClicked(this, null);
			}
			else if (!this.InPauseShop())
			{
				this.HandleResumeButton(this, null);
				this._isCancellationRequested = false;
				return;
			}
			this._isCancellationRequested = false;
		}
		float single = this.sensitivitySlider.@value * 13f;
		float single1 = Mathf.Clamp(single + 6f, 6f, 19f);
		if (this._cachedSensitivity != single1)
		{
			if (Application.isEditor)
			{
				Debug.Log(string.Concat("New sensitivity: ", single1));
			}
			Defs.Sensitivity = single1;
			this._cachedSensitivity = single1;
		}
		if (this.InPauseShop())
		{
			this._shopOpened = true;
			this._lastBackFromShopTime = Single.PositiveInfinity;
		}
		else
		{
			if (this._shopOpened)
			{
				this._lastBackFromShopTime = Time.realtimeSinceStartup;
			}
			this._shopOpened = false;
			if (!Defs.isMulti)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
		}
	}

	public static event Action ChatSettUpdated;

	public static event Action InvertCamUpdated;

	public static event Action PlayerHandUpdated;

	public static event Action SwitchingWeaponsUpdated;
}