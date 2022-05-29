using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class VideoRecordingController : MonoBehaviour, IDisposable
{
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

	public VideoRecordingController()
	{
	}

	private void Awake()
	{
		this._view = base.gameObject.GetComponent<VideoRecordingView>();
		if (this._view != null)
		{
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				this._view.InterfaceEnabled = false;
			}
			if (!Defs.isMulti && !Defs.IsSurvival)
			{
				this._view.InterfaceEnabled = false;
				this.SafeStop();
			}
			if (!GlobalGameController.ShowRec)
			{
				this._view.InterfaceEnabled = false;
			}
		}
	}

	private void ChangeSide()
	{
		Debug.Log(" ChangeSide()");
		this.ChangeSideCoroutine();
	}

	private void ChangeSideCoroutine()
	{
		object obj;
		if (this.isHud)
		{
			Transform vector3 = base.transform;
			obj = (!GlobalGameController.LeftHanded ? 1 : -1);
			float single = base.transform.localPosition.y;
			Vector3 vector31 = base.transform.localPosition;
			vector3.localPosition = new Vector3((float)obj * ((float)Screen.width * 768f / (float)Screen.height / 2f - 30f), single, vector31.z);
		}
	}

	public void Dispose()
	{
		if (this._disposed)
		{
			return;
		}
		IEnumerator<Action> enumerator = (
			from a in this._disposeActions
			where a != null
			select a).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				enumerator.Current();
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		this._disposed = true;
	}

	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= new Action(this.ChangeSide);
		this.Dispose();
	}

	private void OnEnable()
	{
		this.ChangeSideCoroutine();
		this._shouldChangeSideOnEnable = false;
	}

	private void SafeStop()
	{
		if (this._everyplayWrapper.CurrentState == EveryplayWrapper.State.Paused || this._everyplayWrapper.CurrentState == EveryplayWrapper.State.Recording)
		{
			this._everyplayWrapper.Stop();
		}
	}

	private void Start()
	{
		if (this._view != null)
		{
			EventHandler eventHandler = (object sender, EventArgs e) => {
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "Best Score", PlayerPrefs.GetInt(Defs.BestScoreSett, 0) }
				};
				this.u003cu003ef__this._everyplayWrapper.SetMetadata(strs);
			};
			this._view.Started += eventHandler;
			this._disposeActions.Add(new Action(() => this._view.Started -= eventHandler));
		}
		if (this.resumeButton == null)
		{
			Debug.LogError("resumeButton == null");
		}
		else
		{
			this._normalSpriteName = this.resumeButton.normalSprite ?? string.Empty;
			this._pressedSpriteName = this.resumeButton.pressedSprite ?? string.Empty;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
		if (gameObject != null)
		{
			this._pauser = gameObject.GetComponent<Pauser>();
		}
		Everyplay.UploadDidCompleteDelegate num = (int videoId) => {
			PlayerPrefs.GetInt("PostVideo", 0);
			PlayerPrefs.SetInt("PostVideo", PlayerPrefs.GetInt("PostVideo", 0) + 1);
			if (PlayerPrefs.GetInt("Active_loyal_users_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2)
			{
				FacebookController.LogEvent("Active_loyal_users", null);
				PlayerPrefs.SetInt("Active_loyal_users_send", 1);
			}
			if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && StoreKitEventListener.GetDollarsSpent() > 0)
			{
				FacebookController.LogEvent("Active_loyal_users_payed", null);
				PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
			}
		};
		Everyplay.UploadDidComplete += num;
		this._disposeActions.Add(new Action(() => Everyplay.UploadDidComplete -= num));
		PauseNGUIController.PlayerHandUpdated += new Action(this.ChangeSide);
		this.ChangeSideCoroutine();
	}

	private void Update()
	{
		if (this._disposed)
		{
			if (Time.frameCount % 300 == 17)
			{
				Debug.LogWarning(string.Concat(base.GetType().Name, " is disposed."));
			}
			return;
		}
		if (this._view != null)
		{
			if (!GlobalGameController.ShowRec)
			{
				this.SafeStop();
			}
			this._view.InterfaceEnabled = GlobalGameController.ShowRec;
			this._view.InterfaceEnabled = (!this._view.InterfaceEnabled ? false : !ShopNGUIController.GuiActive);
			if (this._pauser != null)
			{
				this._view.InterfaceEnabled = (!this._view.InterfaceEnabled ? false : !this._pauser.paused);
			}
			if (ExperienceController.sharedController != null)
			{
				this._view.InterfaceEnabled = (!this._view.InterfaceEnabled ? false : !ExperienceController.sharedController.isShowNextPlashka);
			}
			if (Defs.isHunger)
			{
				if (!this.isHud)
				{
					NetworkStartTableNGUIController networkStartTableNGUIController = NetworkStartTableNGUIController.sharedController;
					bool flag = (!(networkStartTableNGUIController != null) || !(networkStartTableNGUIController.spectratorModePnl != null) ? false : networkStartTableNGUIController.spectratorModePnl.activeInHierarchy);
					if (flag)
					{
						this.SafeStop();
					}
					this._view.InterfaceEnabled = (!this._view.InterfaceEnabled ? false : !flag);
				}
				else
				{
					WeaponManager weaponManager = WeaponManager.sharedManager;
					if (weaponManager != null && weaponManager.myTable != null)
					{
						NetworkStartTable component = weaponManager.myTable.GetComponent<NetworkStartTable>();
						if (component != null)
						{
							this._view.InterfaceEnabled = (!this._view.InterfaceEnabled ? false : !component.isDeadInHungerGame);
						}
					}
					if (HungerGameController.Instance != null && HungerGameController.Instance.isGo && Initializer.players.Count == 0)
					{
						this._view.InterfaceEnabled = false;
					}
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				this._view.InterfaceEnabled = false;
				this.SafeStop();
			}
			if (!Defs.isMulti && !Defs.IsSurvival)
			{
				this._view.InterfaceEnabled = false;
				this.SafeStop();
			}
		}
		else if (Time.frameCount % 300 == 57)
		{
			Debug.LogWarning("_view == null");
		}
		if (this._everyplayWrapper.Elapsed.TotalMinutes >= 20)
		{
			this.SafeStop();
		}
		if (this.resumeButton != null && this._everyplayWrapper.CurrentState == EveryplayWrapper.State.Paused)
		{
			string str = ((int)Time.time % 2 != 0 ? this._normalSpriteName : this._pressedSpriteName);
			if (this.resumeButton.normalSprite != str)
			{
				this.resumeButton.normalSprite = str;
			}
		}
	}
}