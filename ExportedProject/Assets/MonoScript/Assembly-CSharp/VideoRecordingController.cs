using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class VideoRecordingController : MonoBehaviour, IDisposable
{
	[CompilerGenerated]
	private sealed class _003CStart_003Ec__AnonStorey301
	{
		internal Everyplay.UploadDidCompleteDelegate uploadHadler;

		internal VideoRecordingController _003C_003Ef__this;

		internal void _003C_003Em__470()
		{
			Everyplay.UploadDidComplete -= uploadHadler;
		}
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ec__AnonStorey300
	{
		internal EventHandler startedHandler;

		internal VideoRecordingController _003C_003Ef__this;

		internal void _003C_003Em__46D(object sender, EventArgs e)
		{
			Dictionary<string, object> metadata = new Dictionary<string, object> { 
			{
				"Best Score",
				PlayerPrefs.GetInt(Defs.BestScoreSett, 0)
			} };
			_003C_003Ef__this._everyplayWrapper.SetMetadata(metadata);
		}

		internal void _003C_003Em__46E()
		{
			_003C_003Ef__this._view.Started -= startedHandler;
		}
	}

	public bool isHud;

	public UIButton resumeButton;

	private List<Action> _disposeActions = new List<Action>();

	private bool _disposed;

	private readonly EveryplayWrapper _everyplayWrapper = EveryplayWrapper.Instance;

	private string _normalSpriteName = string.Empty;

	private Pauser _pauser;

	private string _pressedSpriteName = string.Empty;

	private VideoRecordingView _view;

	private bool _shouldChangeSideOnEnable;

	[CompilerGenerated]
	private static Func<Action, bool> _003C_003Ef__am_0024cacheA;

	[CompilerGenerated]
	private static Everyplay.UploadDidCompleteDelegate _003C_003Ef__am_0024cacheB;

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		List<Action> disposeActions = _disposeActions;
		if (_003C_003Ef__am_0024cacheA == null)
		{
			_003C_003Ef__am_0024cacheA = _003CDispose_003Em__46C;
		}
		foreach (Action item in disposeActions.Where(_003C_003Ef__am_0024cacheA))
		{
			item();
		}
		_disposed = true;
	}

	private void Awake()
	{
		_view = base.gameObject.GetComponent<VideoRecordingView>();
		if (_view != null)
		{
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				_view.InterfaceEnabled = false;
			}
			if (!Defs.isMulti && !Defs.IsSurvival)
			{
				_view.InterfaceEnabled = false;
				SafeStop();
			}
			if (!GlobalGameController.ShowRec)
			{
				_view.InterfaceEnabled = false;
			}
		}
	}

	private void ChangeSide()
	{
		Debug.Log(" ChangeSide()");
		ChangeSideCoroutine();
	}

	private void ChangeSideCoroutine()
	{
		if (isHud)
		{
			base.transform.localPosition = new Vector3((float)((!GlobalGameController.LeftHanded) ? 1 : (-1)) * ((float)Screen.width * 768f / (float)Screen.height / 2f - 30f), base.transform.localPosition.y, base.transform.localPosition.z);
		}
	}

	private void Start()
	{
		_003CStart_003Ec__AnonStorey301 _003CStart_003Ec__AnonStorey = new _003CStart_003Ec__AnonStorey301();
		_003CStart_003Ec__AnonStorey._003C_003Ef__this = this;
		if (_view != null)
		{
			_003CStart_003Ec__AnonStorey300 _003CStart_003Ec__AnonStorey2 = new _003CStart_003Ec__AnonStorey300();
			_003CStart_003Ec__AnonStorey2._003C_003Ef__this = this;
			_003CStart_003Ec__AnonStorey2.startedHandler = _003CStart_003Ec__AnonStorey2._003C_003Em__46D;
			_view.Started += _003CStart_003Ec__AnonStorey2.startedHandler;
			_disposeActions.Add(_003CStart_003Ec__AnonStorey2._003C_003Em__46E);
		}
		if (resumeButton != null)
		{
			_normalSpriteName = resumeButton.normalSprite ?? string.Empty;
			_pressedSpriteName = resumeButton.pressedSprite ?? string.Empty;
		}
		else
		{
			Debug.LogError("resumeButton == null");
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
		if (gameObject != null)
		{
			_pauser = gameObject.GetComponent<Pauser>();
		}
		if (_003C_003Ef__am_0024cacheB == null)
		{
			_003C_003Ef__am_0024cacheB = _003CStart_003Em__46F;
		}
		_003CStart_003Ec__AnonStorey.uploadHadler = _003C_003Ef__am_0024cacheB;
		Everyplay.UploadDidComplete += _003CStart_003Ec__AnonStorey.uploadHadler;
		_disposeActions.Add(_003CStart_003Ec__AnonStorey._003C_003Em__470);
		PauseNGUIController.PlayerHandUpdated += ChangeSide;
		ChangeSideCoroutine();
	}

	private void Update()
	{
		if (_disposed)
		{
			if (Time.frameCount % 300 == 17)
			{
				Debug.LogWarning(GetType().Name + " is disposed.");
			}
			return;
		}
		if (_view != null)
		{
			if (!GlobalGameController.ShowRec)
			{
				SafeStop();
			}
			_view.InterfaceEnabled = GlobalGameController.ShowRec;
			_view.InterfaceEnabled = _view.InterfaceEnabled && !ShopNGUIController.GuiActive;
			if (_pauser != null)
			{
				_view.InterfaceEnabled = _view.InterfaceEnabled && !_pauser.paused;
			}
			if (ExperienceController.sharedController != null)
			{
				_view.InterfaceEnabled = _view.InterfaceEnabled && !ExperienceController.sharedController.isShowNextPlashka;
			}
			if (Defs.isHunger)
			{
				if (isHud)
				{
					WeaponManager sharedManager = WeaponManager.sharedManager;
					if (sharedManager != null && sharedManager.myTable != null)
					{
						NetworkStartTable component = sharedManager.myTable.GetComponent<NetworkStartTable>();
						if (component != null)
						{
							_view.InterfaceEnabled = _view.InterfaceEnabled && !component.isDeadInHungerGame;
						}
					}
					if (HungerGameController.Instance != null && HungerGameController.Instance.isGo && Initializer.players.Count == 0)
					{
						_view.InterfaceEnabled = false;
					}
				}
				else
				{
					NetworkStartTableNGUIController sharedController = NetworkStartTableNGUIController.sharedController;
					bool flag = sharedController != null && sharedController.spectratorModePnl != null && sharedController.spectratorModePnl.activeInHierarchy;
					if (flag)
					{
						SafeStop();
					}
					_view.InterfaceEnabled = _view.InterfaceEnabled && !flag;
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				_view.InterfaceEnabled = false;
				SafeStop();
			}
			if (!Defs.isMulti && !Defs.IsSurvival)
			{
				_view.InterfaceEnabled = false;
				SafeStop();
			}
		}
		else if (Time.frameCount % 300 == 57)
		{
			Debug.LogWarning("_view == null");
		}
		if (_everyplayWrapper.Elapsed.TotalMinutes >= 20.0)
		{
			SafeStop();
		}
		if (resumeButton != null && _everyplayWrapper.CurrentState == EveryplayWrapper.State.Paused)
		{
			string text = (((int)Time.time % 2 != 0) ? _normalSpriteName : _pressedSpriteName);
			if (resumeButton.normalSprite != text)
			{
				resumeButton.normalSprite = text;
			}
		}
	}

	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= ChangeSide;
		Dispose();
	}

	private void OnEnable()
	{
		ChangeSideCoroutine();
		_shouldChangeSideOnEnable = false;
	}

	private void SafeStop()
	{
		if (_everyplayWrapper.CurrentState == EveryplayWrapper.State.Paused || _everyplayWrapper.CurrentState == EveryplayWrapper.State.Recording)
		{
			_everyplayWrapper.Stop();
		}
	}

	[CompilerGenerated]
	private static bool _003CDispose_003Em__46C(Action a)
	{
		return a != null;
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__46F(int videoId)
	{
		int @int = PlayerPrefs.GetInt("PostVideo", 0);
		PlayerPrefs.SetInt("PostVideo", PlayerPrefs.GetInt("PostVideo", 0) + 1);
		if (PlayerPrefs.GetInt("Active_loyal_users_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2)
		{
			FacebookController.LogEvent("Active_loyal_users");
			PlayerPrefs.SetInt("Active_loyal_users_send", 1);
		}
		if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && StoreKitEventListener.GetDollarsSpent() > 0)
		{
			FacebookController.LogEvent("Active_loyal_users_payed");
			PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
		}
	}
}
