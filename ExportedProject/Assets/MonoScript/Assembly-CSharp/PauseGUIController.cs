using System;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class PauseGUIController : MonoBehaviour
{
	[SerializeField]
	private UIButton _btnResume;

	[SerializeField]
	private UIButton _btnExit;

	[SerializeField]
	private UIButton _btnSettings;

	[SerializeField]
	private UIButton _btnBank;

	[SerializeField]
	private PrefabHandler _settingsPrefab;

	private IDisposable _backSubscription;

	private LazyObject<PauseNGUIController> _pauseNguiLazy;

	private bool _shopOpened;

	private float _lastBackFromShopTime;

	public static PauseGUIController Instance { get; private set; }

	public PauseNGUIController SettingsPanel
	{
		get
		{
			if (_pauseNguiLazy == null)
			{
				_pauseNguiLazy = new LazyObject<PauseNGUIController>(_settingsPrefab.ResourcePath, InGameGUI.sharedInGameGUI.SubpanelsContainer);
			}
			return _pauseNguiLazy.Value;
		}
	}

	public bool IsPaused
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	private bool InPauseShop
	{
		get
		{
			return InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null && InGameGUI.sharedInGameGUI.playerMoveC.isInappWinOpen;
		}
	}

	private void Awake()
	{
		Instance = this;
		_btnResume.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Em__38C;
		_btnExit.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Em__38D;
		_btnBank.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Em__38E;
		_btnSettings.GetComponent<ButtonHandler>().Clicked += _003CAwake_003Em__38F;
	}

	private void OnEnable()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			_btnBank.gameObject.SetActive(false);
		}
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(Close, "Pause window");
	}

	private void Update()
	{
		if (!InPauseShop)
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

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void Close()
	{
		if (!InPauseShop)
		{
			ButtonClickSound.Instance.PlayClick();
			Debug.Log((InGameGUI.sharedInGameGUI != null) + " " + (InGameGUI.sharedInGameGUI.playerMoveC != null));
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.SetPause();
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			ExperienceController.sharedController.isShowRanks = false;
			ExpController.Instance.InterfaceEnabled = false;
		}
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__38C(object sender, EventArgs e)
	{
		Close();
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__38D(object sender, EventArgs e)
	{
		if (!InPauseShop && !(Time.realtimeSinceStartup < _lastBackFromShopTime + 0.5f))
		{
			ButtonClickSound.Instance.PlayClick();
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.QuitGame();
			}
			else if (PhotonNetwork.room != null)
			{
				coinsShop.hideCoinsShop();
				coinsPlashka.hidePlashka();
				Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
				PhotonNetwork.LeaveRoom();
			}
		}
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__38E(object sender, EventArgs e)
	{
		if (!InPauseShop)
		{
			ButtonClickSound.Instance.PlayClick();
			ExperienceController.sharedController.isShowRanks = false;
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.GoToShopFromPause();
			}
		}
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__38F(object sender, EventArgs e)
	{
		SettingsPanel.gameObject.SetActive(true);
	}
}
