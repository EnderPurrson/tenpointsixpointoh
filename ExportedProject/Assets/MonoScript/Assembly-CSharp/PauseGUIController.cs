using Rilisoft;
using System;
using System.Runtime.CompilerServices;
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

	private bool InPauseShop
	{
		get
		{
			return (!(InGameGUI.sharedInGameGUI != null) || !(InGameGUI.sharedInGameGUI.playerMoveC != null) ? false : InGameGUI.sharedInGameGUI.playerMoveC.isInappWinOpen);
		}
	}

	public static PauseGUIController Instance
	{
		get;
		private set;
	}

	public bool IsPaused
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	public PauseNGUIController SettingsPanel
	{
		get
		{
			if (this._pauseNguiLazy == null)
			{
				this._pauseNguiLazy = new LazyObject<PauseNGUIController>(this._settingsPrefab.ResourcePath, InGameGUI.sharedInGameGUI.SubpanelsContainer);
			}
			return this._pauseNguiLazy.Value;
		}
	}

	public PauseGUIController()
	{
	}

	private void Awake()
	{
		PauseGUIController.Instance = this;
		this._btnResume.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.Close());
		this._btnExit.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
			if (this.InPauseShop)
			{
				return;
			}
			if (Time.realtimeSinceStartup < this._lastBackFromShopTime + 0.5f)
			{
				return;
			}
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
		});
		this._btnBank.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => {
			if (this.InPauseShop)
			{
				return;
			}
			ButtonClickSound.Instance.PlayClick();
			ExperienceController.sharedController.isShowRanks = false;
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.GoToShopFromPause();
			}
		});
		this._btnSettings.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.SettingsPanel.gameObject.SetActive(true));
	}

	private void Close()
	{
		if (this.InPauseShop)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		Debug.Log(string.Concat(InGameGUI.sharedInGameGUI != null, " ", InGameGUI.sharedInGameGUI.playerMoveC != null));
		if (!(InGameGUI.sharedInGameGUI != null) || !(InGameGUI.sharedInGameGUI.playerMoveC != null))
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			InGameGUI.sharedInGameGUI.playerMoveC.SetPause(true);
		}
		ExperienceController.sharedController.isShowRanks = false;
		ExpController.Instance.InterfaceEnabled = false;
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
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this._btnBank.gameObject.SetActive(false);
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.Close), "Pause window");
	}

	private void Update()
	{
		if (this.InPauseShop)
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
}