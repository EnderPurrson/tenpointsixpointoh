using I2.Loc;
using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

internal sealed class SettingsController : MonoBehaviour
{
	public const int SensitivityLowerBound = 6;

	public const int SensitivityUpperBound = 19;

	public MainMenuHeroCamera rotateCamera;

	public UIButton backButton;

	public UIButton controlsButton;

	public UIButton syncButton;

	public UIButton signOutButton;

	public GameObject controlsSettings;

	public GameObject tapPanel;

	public GameObject swipePanel;

	public GameObject mainPanel;

	public UISlider sensitivitySlider;

	public SettingsToggleButtons chatToggleButtons;

	public SettingsToggleButtons musicToggleButtons;

	public SettingsToggleButtons soundToggleButtons;

	public SettingsToggleButtons invertCameraToggleButtons;

	public SettingsToggleButtons recToggleButtons;

	public SettingsToggleButtons pressureToucheToggleButtons;

	public SettingsToggleButtons hideJumpAndShootButtons;

	public SettingsToggleButtons leftHandedToggleButtons;

	public SettingsToggleButtons switchingWeaponsToggleButtons;

	public SettingsToggleButtons fps60ToggleButtons;

	public Texture googlePlayServicesTexture;

	private IDisposable _backSubscription;

	private bool _backRequested;

	private float _cachedSensitivity;

	public SettingsController()
	{
	}

	public static void ChangeLeftHandedRightHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(string.Concat("[Left Handed] button clicked: ", isChecked));
		}
		if (GlobalGameController.LeftHanded != isChecked)
		{
			GlobalGameController.LeftHanded = isChecked;
			PlayerPrefs.SetInt(Defs.LeftHandedSN, (!isChecked ? 0 : 1));
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
			if (SettingsController.ControlsClicked != null)
			{
				SettingsController.ControlsClicked();
			}
			if (!isChecked)
			{
				FlurryPluginWrapper.LogEvent("Left-handed Layout Enabled");
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log("Left-handed Layout Enabled");
				}
			}
		}
	}

	public static void ChangeSwitchingWeaponHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(string.Concat("[Switching Weapon button clicked: ", isChecked));
		}
		if (GlobalGameController.switchingWeaponSwipe == isChecked)
		{
			GlobalGameController.switchingWeaponSwipe = !isChecked;
			PlayerPrefs.SetInt(Defs.SwitchingWeaponsSwipeRegimSN, (!GlobalGameController.switchingWeaponSwipe ? 0 : 1));
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
		}
	}

	private void HandleBackFromSettings(object sender, EventArgs e)
	{
		this._backRequested = true;
	}

	private void HandleControlsClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Controls] button clicked.");
		}
		this.controlsSettings.SetActive(true);
		this.tapPanel.SetActive(!GlobalGameController.switchingWeaponSwipe);
		this.swipePanel.SetActive(false);
		this.swipePanel.transform.parent.gameObject.SetActive(!GlobalGameController.switchingWeaponSwipe);
		base.gameObject.SetActive(false);
		if (SettingsController.ControlsClicked != null)
		{
			SettingsController.ControlsClicked();
		}
	}

	private void HandleLocalizationChanged()
	{
		this.SetSyncLabelText();
	}

	private void HandleRestoreClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Restore] button clicked.");
		}
		WeaponManager.RefreshExpControllers();
		ProgressSynchronizer.Instance.SynchronizeIosProgress();
		CoroutineRunner.Instance.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine(0));
		CampaignProgressSynchronizer.Instance.Sync();
		SkinsSynchronizer.Instance.Sync();
	}

	private void HandleSyncClicked(object sender, EventArgs e)
	{
		Action<UIButton> action1 = null;
		Action action2 = null;
		Action action3 = null;
		Action action4 = null;
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Sync] button clicked.");
			this.RefreshSignOutButton();
		}
		TrophiesSynchronizer.Instance.Sync();
		SkinsSynchronizer.Instance.Sync();
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
		else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			UIButton uIButton1 = (sender as MonoBehaviour).Map<MonoBehaviour, UIButton>((MonoBehaviour o) => o.GetComponent<UIButton>());
			CoroutineRunner.Instance.StartCoroutine(this.SynchronizeAmazonCoroutine(uIButton1));
		}
		else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			UIButton uIButton2 = (sender as MonoBehaviour).Map<MonoBehaviour, UIButton>((MonoBehaviour o) => o.GetComponent<UIButton>());
			if (uIButton2 != null)
			{
				uIButton2.isEnabled = false;
			}
			Action action5 = () => {
				Action<bool> action = (bool succeeded) => {
					try
					{
						UnityEngine.Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.Callback({0}) >: {1:F3}", new object[] { succeeded, Time.realtimeSinceStartup });
						if (succeeded && WeaponManager.sharedManager != null)
						{
							WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
						}
						StoreKitEventListener.purchaseInProcess = false;
						UnityEngine.Debug.LogFormat("[Rilisoft] PurchasesSynchronizer.HasItemsToBeSaved: {0}", new object[] { PurchasesSynchronizer.Instance.HasItemsToBeSaved });
						if (PurchasesSynchronizer.Instance.HasItemsToBeSaved)
						{
							int num = MainMenuController.FindMaxLevel(PurchasesSynchronizer.Instance.ItemsToBeSaved);
							if (Defs.IsDeveloperBuild)
							{
								UnityEngine.Debug.LogFormat("[Rilisoft] Incoming level: {0}", new object[] { num });
							}
							if (num > 0)
							{
								if (ShopNGUIController.GuiActive)
								{
									UnityEngine.Debug.LogWarning("Skipping saving to storager while in Shop.");
									return;
								}
								else if (StringComparer.Ordinal.Equals(SceneManager.GetActiveScene().name, Defs.MainMenuScene))
								{
									TrainingController.OnGetProgress();
									if (HintController.instance != null)
									{
										HintController.instance.ShowNext();
									}
									string str = LocalizationStore.Get("Key_1977");
									UnityEngine.Debug.LogFormat("[Rilisoft] > StartCoroutine(SaveItemsToStorager): {1} {0:F3}", new object[] { Time.realtimeSinceStartup, str });
									if (action3 == null)
									{
										action3 = () => {
											CoroutineRunner instance = CoroutineRunner.Instance;
											if (action4 == null)
											{
												action4 = () => {
													UnityEngine.Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.InnerCallback >: {0:F3}", new object[] { Time.realtimeSinceStartup });
													PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
													if (WeaponManager.sharedManager != null)
													{
														WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
													}
													UnityEngine.Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.InnerCallback <: {0:F3}", new object[] { Time.realtimeSinceStartup });
												};
											}
											instance.StartCoroutine(MainMenuController.SaveItemsToStorager(action4));
										};
									}
									InfoWindowController.ShowRestorePanel(action3);
									UnityEngine.Debug.LogFormat("[Rilisoft] < StartCoroutine(SaveItemsToStorager): {1} {0:F3}", new object[] { Time.realtimeSinceStartup, str });
								}
								else
								{
									UnityEngine.Debug.LogWarning("Skipping saving to storager while not Main Menu.");
									return;
								}
							}
						}
						PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
						UnityEngine.Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.Callback({0}) <: {1:F3}", new object[] { succeeded, Time.realtimeSinceStartup });
					}
					finally
					{
						if (uIButton2 != null)
						{
							uIButton2.isEnabled = true;
						}
					}
				};
				if (!Application.isEditor)
				{
					if (!PurchasesSynchronizer.Instance.SynchronizeIfAuthenticated(action))
					{
						UIButton uIButton = uIButton2;
						if (action1 == null)
						{
							action1 = (UIButton s) => s.isEnabled = true;
						}
						uIButton.Do<UIButton>(action1);
					}
					ProgressSynchronizer progressSynchronizer = ProgressSynchronizer.Instance;
					if (action2 == null)
					{
						action2 = () => {
							if (WeaponManager.sharedManager != null)
							{
								WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
							}
						};
					}
					progressSynchronizer.SynchronizeIfAuthenticated(action2);
					GoogleIAB.queryInventory(StoreKitEventListener.starterPackIds);
				}
				else
				{
					UnityEngine.Debug.Log("Simulating sync...");
					IEnumerator enumerator = PurchasesSynchronizer.Instance.SimulateSynchronization(action);
					CoroutineRunner.Instance.StartCoroutine(enumerator);
				}
				CampaignProgressSynchronizer.Instance.Sync();
				this.RefreshSignOutButton();
				this.SetSyncLabelText();
			};
			StoreKitEventListener.purchaseInProcess = true;
			CoroutineRunner.Instance.StartCoroutine(this.RestoreProgressIndicator(5f));
			if (!GpgFacade.Instance.IsAuthenticated())
			{
				if (Application.isEditor)
				{
					action5();
					return;
				}
				try
				{
					GpgFacade gpgFacade = GpgFacade.Instance;
					gpgFacade.Authenticate((bool succeeded) => {
						PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(!succeeded));
						if (!succeeded)
						{
							UnityEngine.Debug.LogWarning("Authentication failed.");
							StoreKitEventListener.purchaseInProcess = false;
							if (uIButton2 != null)
							{
								uIButton2.isEnabled = true;
							}
						}
						else
						{
							UnityEngine.Debug.LogFormat("Authentication succeeded: {0}, {1}, {2}", new object[] { Social.localUser.id, Social.localUser.userName, Social.localUser.state });
							action5();
						}
					}, false);
				}
				catch (InvalidOperationException invalidOperationException1)
				{
					InvalidOperationException invalidOperationException = invalidOperationException1;
					UnityEngine.Debug.LogWarning("SettingsController: Exception occured while authenticating with Google Play Games. See next exception message for details.");
					UnityEngine.Debug.LogException(invalidOperationException);
					if (uIButton2 != null)
					{
						uIButton2.isEnabled = true;
					}
				}
			}
			else
			{
				string str1 = string.Format("Already authenticated: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
				UnityEngine.Debug.Log(str1);
				action5();
			}
		}
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
		this._backSubscription = BackSystem.Instance.Register(() => this.HandleBackFromSettings(this, EventArgs.Empty), "Settings");
		this.RefreshSignOutButton();
	}

	internal void RefreshSignOutButton()
	{
		if (this.signOutButton != null)
		{
			if (Application.isEditor)
			{
				this.signOutButton.gameObject.SetActive(true);
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				this.signOutButton.gameObject.SetActive(GpgFacade.Instance.IsAuthenticated());
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator RestoreProgressIndicator(float delayTime)
	{
		SettingsController.u003cRestoreProgressIndicatoru003ec__Iterator18F variable = null;
		return variable;
	}

	public static void Set60FPSEnable(bool isChecked, Action handler = null)
	{
		GlobalGameController.is60FPSEnable = !isChecked;
	}

	private void SetSyncLabelText()
	{
		UILabel component = null;
		Transform transforms = this.syncButton.transform.FindChild("Label");
		if (transforms != null)
		{
			component = transforms.gameObject.GetComponent<UILabel>();
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (component != null)
			{
				component.text = LocalizationStore.Get("Key_0080");
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && component != null)
		{
			component.text = LocalizationStore.Get("Key_0935");
		}
	}

	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		if (this.backButton != null)
		{
			ButtonHandler component = this.backButton.GetComponent<ButtonHandler>();
			component.Clicked += new EventHandler(this.HandleBackFromSettings);
		}
		if (this.controlsButton != null)
		{
			ButtonHandler buttonHandler = this.controlsButton.GetComponent<ButtonHandler>();
			buttonHandler.Clicked += new EventHandler(this.HandleControlsClicked);
		}
		if (this.syncButton != null)
		{
			ButtonHandler component1 = this.syncButton.GetComponent<ButtonHandler>();
			this.SetSyncLabelText();
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				this.syncButton.gameObject.SetActive(true);
				component1.Clicked += new EventHandler(this.HandleRestoreClicked);
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				this.syncButton.gameObject.SetActive(true);
				component1.Clicked += new EventHandler(this.HandleSyncClicked);
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				this.syncButton.gameObject.SetActive(false);
				component1.Clicked += new EventHandler(this.HandleSyncClicked);
			}
		}
		if (this.sensitivitySlider == null)
		{
			UnityEngine.Debug.LogWarning("sensitivitySlider == null");
		}
		else
		{
			float single = Mathf.Clamp(Defs.Sensitivity, 6f, 19f);
			this.sensitivitySlider.@value = (single - 6f) / 13f;
			this._cachedSensitivity = single;
		}
		this.musicToggleButtons.IsChecked = Defs.isSoundMusic;
		this.musicToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log(string.Concat("[Music] button clicked: ", e.IsChecked));
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("MenuBackgroundMusic");
			MenuBackgroundMusic menuBackgroundMusic = (gameObject == null ? null : gameObject.GetComponent<MenuBackgroundMusic>());
			if (Defs.isSoundMusic != e.IsChecked)
			{
				Defs.isSoundMusic = e.IsChecked;
				PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
				PlayerPrefs.Save();
				if (menuBackgroundMusic == null)
				{
					UnityEngine.Debug.LogWarning("menuBackgroundMusic == null");
				}
				else if (!e.IsChecked)
				{
					menuBackgroundMusic.Stop();
				}
				else
				{
					menuBackgroundMusic.Play();
				}
			}
		});
		this.soundToggleButtons.IsChecked = Defs.isSoundFX;
		this.soundToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log(string.Concat("[Sound] button clicked: ", e.IsChecked));
			}
			if (Defs.isSoundFX != e.IsChecked)
			{
				Defs.isSoundFX = e.IsChecked;
				PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
				PlayerPrefs.Save();
			}
		});
		this.chatToggleButtons.IsChecked = Defs.IsChatOn;
		this.chatToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => SettingsController.SwitchChatSetting(e.IsChecked, null));
		this.invertCameraToggleButtons.IsChecked = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
		this.invertCameraToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log(string.Concat("[Invert Camera] button clicked: ", e.IsChecked));
			}
			if (PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1 != e.IsChecked)
			{
				PlayerPrefs.SetInt(Defs.InvertCamSN, Convert.ToInt32(e.IsChecked));
				PlayerPrefs.Save();
			}
		});
		if (this.leftHandedToggleButtons != null)
		{
			this.leftHandedToggleButtons.IsChecked = GlobalGameController.LeftHanded;
			this.leftHandedToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => SettingsController.ChangeLeftHandedRightHanded(e.IsChecked, null));
		}
		if (this.fps60ToggleButtons != null)
		{
			this.fps60ToggleButtons.IsChecked = !GlobalGameController.is60FPSEnable;
			this.fps60ToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => SettingsController.Set60FPSEnable(e.IsChecked, null));
		}
		if (this.switchingWeaponsToggleButtons != null)
		{
			this.switchingWeaponsToggleButtons.IsChecked = !GlobalGameController.switchingWeaponSwipe;
			this.switchingWeaponsToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => SettingsController.ChangeSwitchingWeaponHanded(e.IsChecked, null));
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			this.pressureToucheToggleButtons.gameObject.SetActive(true);
			this.recToggleButtons.gameObject.SetActive(false);
			this.pressureToucheToggleButtons.IsChecked = Defs.isUse3DTouch;
			this.pressureToucheToggleButtons.Clicked += new EventHandler<ToggleButtonEventArgs>((object sender, ToggleButtonEventArgs e) => {
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log(string.Concat("3D touche button clicked: ", e.IsChecked));
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
					UnityEngine.Debug.Log(string.Concat("[Rec. Buttons] button clicked: ", e.IsChecked));
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
					UnityEngine.Debug.Log(string.Concat("3D touche button clicked: ", e.IsChecked));
				}
				Defs.isJumpAndShootButtonOn = e.IsChecked;
			});
		}
		else
		{
			this.hideJumpAndShootButtons.gameObject.SetActive(false);
		}
	}

	public static void SwitchChatSetting(bool on, Action additional = null)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(string.Concat("[Chat] button clicked: ", on));
		}
		if (Defs.IsChatOn != on)
		{
			Defs.IsChatOn = on;
			if (additional != null)
			{
				additional();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator SynchronizeAmazonCoroutine(UIButton syncButton)
	{
		SettingsController.u003cSynchronizeAmazonCoroutineu003ec__Iterator18E variable = null;
		return variable;
	}

	private void Update()
	{
		if (this._backRequested)
		{
			this._backRequested = false;
			this.mainPanel.SetActive(true);
			base.gameObject.SetActive(false);
			this.rotateCamera.OnMainMenuCloseOptions();
			return;
		}
		float single = this.sensitivitySlider.@value * 13f;
		float single1 = Mathf.Clamp(single + 6f, 6f, 19f);
		if (this._cachedSensitivity != single1)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log(string.Concat("New sensitivity: ", single1));
			}
			Defs.Sensitivity = single1;
			this._cachedSensitivity = single1;
		}
	}

	public static event Action ControlsClicked;
}