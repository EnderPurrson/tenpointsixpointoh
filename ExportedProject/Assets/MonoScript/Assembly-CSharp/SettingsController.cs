using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class SettingsController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHandleSyncClicked_003Ec__AnonStorey2F8
	{
		internal UIButton syncButton;

		internal Action afterAuth;

		internal SettingsController _003C_003Ef__this;

		private static Action<UIButton> _003C_003Ef__am_0024cache3;

		private static Action _003C_003Ef__am_0024cache4;

		private static Action _003C_003Ef__am_0024cache5;

		private static Action _003C_003Ef__am_0024cache6;

		internal void _003C_003Em__442()
		{
			Action<bool> callback = _003C_003Em__446;
			if (Application.isEditor)
			{
				Debug.Log("Simulating sync...");
				IEnumerator routine = PurchasesSynchronizer.Instance.SimulateSynchronization(callback);
				CoroutineRunner.Instance.StartCoroutine(routine);
			}
			else
			{
				if (!PurchasesSynchronizer.Instance.SynchronizeIfAuthenticated(callback))
				{
					UIButton o = syncButton;
					if (_003C_003Ef__am_0024cache3 == null)
					{
						_003C_003Ef__am_0024cache3 = _003C_003Em__447;
					}
					o.Do(_003C_003Ef__am_0024cache3);
				}
				ProgressSynchronizer instance = ProgressSynchronizer.Instance;
				if (_003C_003Ef__am_0024cache4 == null)
				{
					_003C_003Ef__am_0024cache4 = _003C_003Em__448;
				}
				instance.SynchronizeIfAuthenticated(_003C_003Ef__am_0024cache4);
				GoogleIAB.queryInventory(StoreKitEventListener.starterPackIds);
			}
			CampaignProgressSynchronizer.Instance.Sync();
			_003C_003Ef__this.RefreshSignOutButton();
			_003C_003Ef__this.SetSyncLabelText();
		}

		internal void _003C_003Em__443(bool succeeded)
		{
			PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(!succeeded));
			if (succeeded)
			{
				Debug.LogFormat("Authentication succeeded: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
				afterAuth();
			}
			else
			{
				Debug.LogWarning("Authentication failed.");
				StoreKitEventListener.purchaseInProcess = false;
				if (syncButton != null)
				{
					syncButton.isEnabled = true;
				}
			}
		}

		internal void _003C_003Em__446(bool succeeded)
		{
			try
			{
				Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.Callback({0}) >: {1:F3}", succeeded, Time.realtimeSinceStartup);
				if (succeeded && WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
				}
				StoreKitEventListener.purchaseInProcess = false;
				Debug.LogFormat("[Rilisoft] PurchasesSynchronizer.HasItemsToBeSaved: {0}", PurchasesSynchronizer.Instance.HasItemsToBeSaved);
				if (PurchasesSynchronizer.Instance.HasItemsToBeSaved)
				{
					int num = MainMenuController.FindMaxLevel(PurchasesSynchronizer.Instance.ItemsToBeSaved);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Rilisoft] Incoming level: {0}", num);
					}
					if (num > 0)
					{
						if (ShopNGUIController.GuiActive)
						{
							Debug.LogWarning("Skipping saving to storager while in Shop.");
							return;
						}
						if (!StringComparer.Ordinal.Equals(SceneManager.GetActiveScene().name, Defs.MainMenuScene))
						{
							Debug.LogWarning("Skipping saving to storager while not Main Menu.");
							return;
						}
						TrainingController.OnGetProgress();
						if (HintController.instance != null)
						{
							HintController.instance.ShowNext();
						}
						string text = LocalizationStore.Get("Key_1977");
						Debug.LogFormat("[Rilisoft] > StartCoroutine(SaveItemsToStorager): {1} {0:F3}", Time.realtimeSinceStartup, text);
						if (_003C_003Ef__am_0024cache5 == null)
						{
							_003C_003Ef__am_0024cache5 = _003C_003Em__449;
						}
						InfoWindowController.ShowRestorePanel(_003C_003Ef__am_0024cache5);
						Debug.LogFormat("[Rilisoft] < StartCoroutine(SaveItemsToStorager): {1} {0:F3}", Time.realtimeSinceStartup, text);
					}
				}
				PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
				Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.Callback({0}) <: {1:F3}", succeeded, Time.realtimeSinceStartup);
			}
			finally
			{
				if (syncButton != null)
				{
					syncButton.isEnabled = true;
				}
			}
		}

		private static void _003C_003Em__447(UIButton s)
		{
			s.isEnabled = true;
		}

		private static void _003C_003Em__448()
		{
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
			}
		}

		private static void _003C_003Em__449()
		{
			CoroutineRunner instance = CoroutineRunner.Instance;
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003C_003Em__44A;
			}
			instance.StartCoroutine(MainMenuController.SaveItemsToStorager(_003C_003Ef__am_0024cache6));
		}

		private static void _003C_003Em__44A()
		{
			Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.InnerCallback >: {0:F3}", Time.realtimeSinceStartup);
			PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
			}
			Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.InnerCallback <: {0:F3}", Time.realtimeSinceStartup);
		}
	}

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
	private static EventHandler<ToggleButtonEventArgs> _003C_003Ef__am_0024cache21;

	[CompilerGenerated]
	private static Func<MonoBehaviour, UIButton> _003C_003Ef__am_0024cache22;

	[CompilerGenerated]
	private static Func<MonoBehaviour, UIButton> _003C_003Ef__am_0024cache23;

	public static event Action ControlsClicked;

	private IEnumerator SynchronizeAmazonCoroutine(UIButton syncButton)
	{
		if (syncButton != null)
		{
			syncButton.isEnabled = false;
		}
		try
		{
			if (!GameCircleSocial.Instance.localUser.authenticated)
			{
				Debug.LogFormat("[Rilisoft] Sign in to GameCircle ({0})", GetType().Name);
				AGSClient.ShowSignInPage();
			}
			Scene activeScene = SceneManager.GetActiveScene();
			float endTime = Time.realtimeSinceStartup + 60f;
			while (!GameCircleSocial.Instance.localUser.authenticated && Time.realtimeSinceStartup < endTime)
			{
				yield return null;
			}
			if (!GameCircleSocial.Instance.localUser.authenticated || !activeScene.IsValid() || !activeScene.isLoaded)
			{
				Debug.LogWarningFormat("Stop syncing attempt. Scene {0} valid: {1}, loaded: {2}. User authenticated: {3}", activeScene.name, activeScene.IsValid(), activeScene.isLoaded, GameCircleSocial.Instance.localUser.authenticated);
				yield break;
			}
			PurchasesSynchronizer.Instance.SynchronizeAmazonPurchases();
			if (PurchasesSynchronizer.Instance.HasItemsToBeSaved)
			{
				int maxLevel = MainMenuController.FindMaxLevel(PurchasesSynchronizer.Instance.ItemsToBeSaved);
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("[Rilisoft] Incoming level: {0}", maxLevel);
				}
				if (maxLevel > 0)
				{
					if (ShopNGUIController.GuiActive)
					{
						Debug.LogWarning("Skipping saving to storager while in Shop.");
						yield break;
					}
					if (!StringComparer.Ordinal.Equals(SceneManager.GetActiveScene().name, Defs.MainMenuScene))
					{
						Debug.LogWarning("Skipping saving to storager while not Main Menu.");
						yield break;
					}
					TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
					InfoWindowController.ShowRestorePanel(((_003CSynchronizeAmazonCoroutine_003Ec__Iterator18E)(object)this)._003C_003Em__444);
					Task<bool> future = promise.Task;
					while (!future.IsCompleted)
					{
						yield return null;
					}
					ProgressSynchronizer.Instance.SynchronizeAmazonProgress();
					if (WeaponManager.sharedManager != null)
					{
						WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
					}
				}
			}
			StarterPackController.Get.RestoreStarterPackForAmazon();
			SetSyncLabelText();
		}
		finally
		{
			if (syncButton != null)
			{
				syncButton.isEnabled = true;
			}
		}
	}

	public static void SwitchChatSetting(bool on, Action additional = null)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Chat] button clicked: " + on);
		}
		bool isChatOn = Defs.IsChatOn;
		if (isChatOn != on)
		{
			Defs.IsChatOn = on;
			if (additional != null)
			{
				additional();
			}
		}
	}

	public static void Set60FPSEnable(bool isChecked, Action handler = null)
	{
		GlobalGameController.is60FPSEnable = !isChecked;
	}

	public static void ChangeLeftHandedRightHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Left Handed] button clicked: " + isChecked);
		}
		if (GlobalGameController.LeftHanded == isChecked)
		{
			return;
		}
		GlobalGameController.LeftHanded = isChecked;
		PlayerPrefs.SetInt(Defs.LeftHandedSN, isChecked ? 1 : 0);
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
			if (Debug.isDebugBuild)
			{
				Debug.Log("Left-handed Layout Enabled");
			}
		}
	}

	public static void ChangeSwitchingWeaponHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Switching Weapon button clicked: " + isChecked);
		}
		if (GlobalGameController.switchingWeaponSwipe == isChecked)
		{
			GlobalGameController.switchingWeaponSwipe = !isChecked;
			PlayerPrefs.SetInt(Defs.SwitchingWeaponsSwipeRegimSN, GlobalGameController.switchingWeaponSwipe ? 1 : 0);
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
		}
	}

	private void SetSyncLabelText()
	{
		UILabel uILabel = null;
		Transform transform = syncButton.transform.FindChild("Label");
		if (transform != null)
		{
			uILabel = transform.gameObject.GetComponent<UILabel>();
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (uILabel != null)
			{
				uILabel.text = LocalizationStore.Get("Key_0080");
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && uILabel != null)
		{
			uILabel.text = LocalizationStore.Get("Key_0935");
		}
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(_003COnEnable_003Em__435, "Settings");
		RefreshSignOutButton();
	}

	internal void RefreshSignOutButton()
	{
		if (signOutButton != null)
		{
			if (Application.isEditor)
			{
				signOutButton.gameObject.SetActive(true);
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				signOutButton.gameObject.SetActive(GpgFacade.Instance.IsAuthenticated());
			}
		}
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(HandleLocalizationChanged);
		if (backButton != null)
		{
			ButtonHandler component = backButton.GetComponent<ButtonHandler>();
			component.Clicked += HandleBackFromSettings;
		}
		if (controlsButton != null)
		{
			ButtonHandler component2 = controlsButton.GetComponent<ButtonHandler>();
			component2.Clicked += HandleControlsClicked;
		}
		if (syncButton != null)
		{
			ButtonHandler component3 = syncButton.GetComponent<ButtonHandler>();
			SetSyncLabelText();
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				syncButton.gameObject.SetActive(true);
				component3.Clicked += HandleRestoreClicked;
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				syncButton.gameObject.SetActive(true);
				component3.Clicked += HandleSyncClicked;
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				bool active = false;
				syncButton.gameObject.SetActive(active);
				component3.Clicked += HandleSyncClicked;
			}
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
		musicToggleButtons.IsChecked = Defs.isSoundMusic;
		SettingsToggleButtons settingsToggleButtons = musicToggleButtons;
		if (_003C_003Ef__am_0024cache19 == null)
		{
			_003C_003Ef__am_0024cache19 = _003CStart_003Em__436;
		}
		settingsToggleButtons.Clicked += _003C_003Ef__am_0024cache19;
		soundToggleButtons.IsChecked = Defs.isSoundFX;
		SettingsToggleButtons settingsToggleButtons2 = soundToggleButtons;
		if (_003C_003Ef__am_0024cache1A == null)
		{
			_003C_003Ef__am_0024cache1A = _003CStart_003Em__437;
		}
		settingsToggleButtons2.Clicked += _003C_003Ef__am_0024cache1A;
		chatToggleButtons.IsChecked = Defs.IsChatOn;
		SettingsToggleButtons settingsToggleButtons3 = chatToggleButtons;
		if (_003C_003Ef__am_0024cache1B == null)
		{
			_003C_003Ef__am_0024cache1B = _003CStart_003Em__438;
		}
		settingsToggleButtons3.Clicked += _003C_003Ef__am_0024cache1B;
		invertCameraToggleButtons.IsChecked = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
		SettingsToggleButtons settingsToggleButtons4 = invertCameraToggleButtons;
		if (_003C_003Ef__am_0024cache1C == null)
		{
			_003C_003Ef__am_0024cache1C = _003CStart_003Em__439;
		}
		settingsToggleButtons4.Clicked += _003C_003Ef__am_0024cache1C;
		if (leftHandedToggleButtons != null)
		{
			leftHandedToggleButtons.IsChecked = GlobalGameController.LeftHanded;
			SettingsToggleButtons settingsToggleButtons5 = leftHandedToggleButtons;
			if (_003C_003Ef__am_0024cache1D == null)
			{
				_003C_003Ef__am_0024cache1D = _003CStart_003Em__43A;
			}
			settingsToggleButtons5.Clicked += _003C_003Ef__am_0024cache1D;
		}
		if (fps60ToggleButtons != null)
		{
			fps60ToggleButtons.IsChecked = !GlobalGameController.is60FPSEnable;
			SettingsToggleButtons settingsToggleButtons6 = fps60ToggleButtons;
			if (_003C_003Ef__am_0024cache1E == null)
			{
				_003C_003Ef__am_0024cache1E = _003CStart_003Em__43B;
			}
			settingsToggleButtons6.Clicked += _003C_003Ef__am_0024cache1E;
		}
		if (switchingWeaponsToggleButtons != null)
		{
			switchingWeaponsToggleButtons.IsChecked = !GlobalGameController.switchingWeaponSwipe;
			SettingsToggleButtons settingsToggleButtons7 = switchingWeaponsToggleButtons;
			if (_003C_003Ef__am_0024cache1F == null)
			{
				_003C_003Ef__am_0024cache1F = _003CStart_003Em__43C;
			}
			settingsToggleButtons7.Clicked += _003C_003Ef__am_0024cache1F;
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			pressureToucheToggleButtons.gameObject.SetActive(true);
			recToggleButtons.gameObject.SetActive(false);
			pressureToucheToggleButtons.IsChecked = Defs.isUse3DTouch;
			pressureToucheToggleButtons.Clicked += _003CStart_003Em__43D;
		}
		else
		{
			pressureToucheToggleButtons.gameObject.SetActive(false);
			recToggleButtons.gameObject.SetActive(PauseNGUIController.RecButtonsAvailable());
			recToggleButtons.IsChecked = GlobalGameController.ShowRec;
			SettingsToggleButtons settingsToggleButtons8 = recToggleButtons;
			if (_003C_003Ef__am_0024cache20 == null)
			{
				_003C_003Ef__am_0024cache20 = _003CStart_003Em__43E;
			}
			settingsToggleButtons8.Clicked += _003C_003Ef__am_0024cache20;
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			hideJumpAndShootButtons.gameObject.SetActive(Defs.isUse3DTouch);
			hideJumpAndShootButtons.IsChecked = Defs.isJumpAndShootButtonOn;
			SettingsToggleButtons settingsToggleButtons9 = hideJumpAndShootButtons;
			if (_003C_003Ef__am_0024cache21 == null)
			{
				_003C_003Ef__am_0024cache21 = _003CStart_003Em__43F;
			}
			settingsToggleButtons9.Clicked += _003C_003Ef__am_0024cache21;
		}
		else
		{
			hideJumpAndShootButtons.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (_backRequested)
		{
			_backRequested = false;
			mainPanel.SetActive(true);
			base.gameObject.SetActive(false);
			rotateCamera.OnMainMenuCloseOptions();
			return;
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
	}

	private void HandleBackFromSettings(object sender, EventArgs e)
	{
		_backRequested = true;
	}

	private void HandleControlsClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Controls] button clicked.");
		}
		controlsSettings.SetActive(true);
		tapPanel.SetActive(!GlobalGameController.switchingWeaponSwipe);
		swipePanel.SetActive(false);
		swipePanel.transform.parent.gameObject.SetActive(!GlobalGameController.switchingWeaponSwipe);
		base.gameObject.SetActive(false);
		if (SettingsController.ControlsClicked != null)
		{
			SettingsController.ControlsClicked();
		}
	}

	private void HandleRestoreClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Restore] button clicked.");
		}
		WeaponManager.RefreshExpControllers();
		ProgressSynchronizer.Instance.SynchronizeIosProgress();
		CoroutineRunner.Instance.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine());
		CampaignProgressSynchronizer.Instance.Sync();
		SkinsSynchronizer.Instance.Sync();
	}

	private void HandleSyncClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Sync] button clicked.");
			RefreshSignOutButton();
		}
		TrophiesSynchronizer.Instance.Sync();
		SkinsSynchronizer.Instance.Sync();
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				MonoBehaviour o = sender as MonoBehaviour;
				if (_003C_003Ef__am_0024cache22 == null)
				{
					_003C_003Ef__am_0024cache22 = _003CHandleSyncClicked_003Em__440;
				}
				UIButton uIButton = o.Map(_003C_003Ef__am_0024cache22);
				CoroutineRunner.Instance.StartCoroutine(SynchronizeAmazonCoroutine(uIButton));
			}
			else
			{
				if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return;
				}
				_003CHandleSyncClicked_003Ec__AnonStorey2F8 _003CHandleSyncClicked_003Ec__AnonStorey2F = new _003CHandleSyncClicked_003Ec__AnonStorey2F8();
				_003CHandleSyncClicked_003Ec__AnonStorey2F._003C_003Ef__this = this;
				MonoBehaviour o2 = sender as MonoBehaviour;
				if (_003C_003Ef__am_0024cache23 == null)
				{
					_003C_003Ef__am_0024cache23 = _003CHandleSyncClicked_003Em__441;
				}
				_003CHandleSyncClicked_003Ec__AnonStorey2F.syncButton = o2.Map(_003C_003Ef__am_0024cache23);
				if (_003CHandleSyncClicked_003Ec__AnonStorey2F.syncButton != null)
				{
					_003CHandleSyncClicked_003Ec__AnonStorey2F.syncButton.isEnabled = false;
				}
				_003CHandleSyncClicked_003Ec__AnonStorey2F.afterAuth = _003CHandleSyncClicked_003Ec__AnonStorey2F._003C_003Em__442;
				StoreKitEventListener.purchaseInProcess = true;
				CoroutineRunner.Instance.StartCoroutine(RestoreProgressIndicator(5f));
				if (GpgFacade.Instance.IsAuthenticated())
				{
					string message = string.Format("Already authenticated: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
					Debug.Log(message);
					_003CHandleSyncClicked_003Ec__AnonStorey2F.afterAuth();
					return;
				}
				if (!Application.isEditor)
				{
					try
					{
						GpgFacade.Instance.Authenticate(_003CHandleSyncClicked_003Ec__AnonStorey2F._003C_003Em__443, false);
						return;
					}
					catch (InvalidOperationException exception)
					{
						Debug.LogWarning("SettingsController: Exception occured while authenticating with Google Play Games. See next exception message for details.");
						Debug.LogException(exception);
						if (_003CHandleSyncClicked_003Ec__AnonStorey2F.syncButton != null)
						{
							_003CHandleSyncClicked_003Ec__AnonStorey2F.syncButton.isEnabled = true;
						}
						return;
					}
				}
				_003CHandleSyncClicked_003Ec__AnonStorey2F.afterAuth();
			}
		}
		else if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
		{
		}
	}

	private IEnumerator RestoreProgressIndicator(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		StoreKitEventListener.purchaseInProcess = false;
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(HandleLocalizationChanged);
	}

	private void HandleLocalizationChanged()
	{
		SetSyncLabelText();
	}

	[CompilerGenerated]
	private void _003COnEnable_003Em__435()
	{
		HandleBackFromSettings(this, EventArgs.Empty);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__436(object sender, ToggleButtonEventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Music] button clicked: " + e.IsChecked);
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("MenuBackgroundMusic");
		MenuBackgroundMusic menuBackgroundMusic = ((!(gameObject != null)) ? null : gameObject.GetComponent<MenuBackgroundMusic>());
		if (Defs.isSoundMusic == e.IsChecked)
		{
			return;
		}
		Defs.isSoundMusic = e.IsChecked;
		PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
		PlayerPrefs.Save();
		if (menuBackgroundMusic != null)
		{
			if (e.IsChecked)
			{
				menuBackgroundMusic.Play();
			}
			else
			{
				menuBackgroundMusic.Stop();
			}
		}
		else
		{
			Debug.LogWarning("menuBackgroundMusic == null");
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__437(object sender, ToggleButtonEventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Sound] button clicked: " + e.IsChecked);
		}
		if (Defs.isSoundFX != e.IsChecked)
		{
			Defs.isSoundFX = e.IsChecked;
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
			PlayerPrefs.Save();
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__438(object sender, ToggleButtonEventArgs e)
	{
		SwitchChatSetting(e.IsChecked);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__439(object sender, ToggleButtonEventArgs e)
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
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__43A(object sender, ToggleButtonEventArgs e)
	{
		ChangeLeftHandedRightHanded(e.IsChecked);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__43B(object sender, ToggleButtonEventArgs e)
	{
		Set60FPSEnable(e.IsChecked);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__43C(object sender, ToggleButtonEventArgs e)
	{
		ChangeSwitchingWeaponHanded(e.IsChecked);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__43D(object sender, ToggleButtonEventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("3D touche button clicked: " + e.IsChecked);
		}
		Defs.isUse3DTouch = e.IsChecked;
		hideJumpAndShootButtons.gameObject.SetActive(Defs.isUse3DTouch);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__43E(object sender, ToggleButtonEventArgs e)
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
	private static void _003CStart_003Em__43F(object sender, ToggleButtonEventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("3D touche button clicked: " + e.IsChecked);
		}
		Defs.isJumpAndShootButtonOn = e.IsChecked;
	}

	[CompilerGenerated]
	private static UIButton _003CHandleSyncClicked_003Em__440(MonoBehaviour o)
	{
		return o.GetComponent<UIButton>();
	}

	[CompilerGenerated]
	private static UIButton _003CHandleSyncClicked_003Em__441(MonoBehaviour o)
	{
		return o.GetComponent<UIButton>();
	}
}
