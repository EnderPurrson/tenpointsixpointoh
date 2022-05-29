using I2.Loc;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

internal sealed class VideoRecordingView : MonoBehaviour, IDisposable
{
	public UIAnchor interfaceContainer;

	public UILabel caption;

	public UIButton pauseButton;

	public UIButton recordButton;

	public UIButton resumeButton;

	public UIButton shareButton;

	public UIButton stopButton;

	public GameObject frame1;

	public GameObject frame2;

	private string startRecLocalize;

	private string recLocalize;

	private string doneLocalize;

	private readonly TaskCompletionSource<bool> _isRecordingSupportedPromise = new TaskCompletionSource<bool>();

	private readonly List<Action> _disposeActions = new List<Action>();

	private bool _disposed;

	private readonly EveryplayWrapper _everyplayWrapper = EveryplayWrapper.Instance;

	private bool _interfaceEnabled = true;

	private EventHandler Starting;

	private EventHandler Started;

	private EveryplayWrapper.State CurrentState
	{
		get
		{
			return this._everyplayWrapper.CurrentState;
		}
	}

	public bool InterfaceEnabled
	{
		get
		{
			return this._interfaceEnabled;
		}
		set
		{
			this._interfaceEnabled = value;
		}
	}

	private Task IsRecordingSupportedFuture
	{
		get
		{
			return this._isRecordingSupportedPromise.Task;
		}
	}

	public static bool IsWeakDevice
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return true;
			}
			return (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android ? false : Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon);
		}
	}

	public VideoRecordingView()
	{
	}

	private void BindHandler(UIButton button, EventHandler handler)
	{
		if (this._disposed)
		{
			return;
		}
		if (button == null)
		{
			return;
		}
		ButtonHandler component = button.GetComponent<ButtonHandler>();
		if (component != null)
		{
			component.Clicked += handler;
			this._disposeActions.Add(new Action(() => component.Clicked -= handler));
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

	private void HandleLocalizationChanged()
	{
		this.SetButtonsText();
	}

	private void HandlePauseButton(object sender, EventArgs e)
	{
		if (this._disposed)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		this._everyplayWrapper.Pause();
	}

	private void HandleRecordButton(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (LoadingInAfterGame.isShowLoading)
		{
			return;
		}
		if (this._disposed)
		{
			return;
		}
		NoodlePermissionGranter.GrantPermission(NoodlePermissionGranter.NoodleAndroidPermission.RECORD_AUDIO);
		NoodlePermissionGranter.GrantPermission(NoodlePermissionGranter.NoodleAndroidPermission.CAMERA);
		EventHandler starting = this.Starting;
		if (starting != null)
		{
			starting(this, EventArgs.Empty);
		}
		if (this.frame1 != null)
		{
			this.frame1.SetActive(false);
		}
		if (this.frame2 != null)
		{
			this.frame2.SetActive(true);
		}
		this._everyplayWrapper.Record();
		EventHandler started = this.Started;
		if (started != null)
		{
			started(this, EventArgs.Empty);
		}
	}

	private void HandleResumeButton(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (this._disposed)
		{
			return;
		}
		this._everyplayWrapper.Resume();
	}

	private void HandleShareButton(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (this._disposed)
		{
			return;
		}
		this._everyplayWrapper.Share();
	}

	private void HandleStopButton(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (this._disposed)
		{
			return;
		}
		this._everyplayWrapper.Stop();
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		this.Dispose();
	}

	private void SetButtonsText()
	{
		this.startRecLocalize = LocalizationStore.Key_0207;
		this.recLocalize = LocalizationStore.Key_0558;
		this.doneLocalize = LocalizationStore.Key_0559;
	}

	private void Start()
	{
		CoroutineRunner.Instance.StartCoroutine(this.WaitUntilEveryplayRecordingIsSupported());
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		this.SetButtonsText();
		bool flag = false;
		try
		{
			flag = (Application.isEditor ? true : this._everyplayWrapper.IsSupported());
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
		if (!flag)
		{
			UnityEngine.Debug.Log("Everyplay is not supported.");
			base.gameObject.SetActive(false);
			return;
		}
		if (((IEnumerable<UnityEngine.Object>)(new UnityEngine.Object[] { this.interfaceContainer, this.caption, this.pauseButton, this.recordButton, this.resumeButton, this.shareButton, this.stopButton })).Any<UnityEngine.Object>((UnityEngine.Object ri) => ri == null))
		{
			this._disposed = true;
			return;
		}
		this.BindHandler(this.pauseButton, new EventHandler(this.HandlePauseButton));
		this.BindHandler(this.recordButton, new EventHandler(this.HandleRecordButton));
		this.BindHandler(this.resumeButton, new EventHandler(this.HandleResumeButton));
		this.BindHandler(this.stopButton, new EventHandler(this.HandleStopButton));
		this.BindHandler(this.shareButton, new EventHandler(this.HandleShareButton));
		this.pauseButton.gameObject.SetActive(false);
		this.recordButton.gameObject.SetActive(true);
		this.resumeButton.gameObject.SetActive(false);
		this.shareButton.gameObject.SetActive(false);
		this.stopButton.gameObject.SetActive(false);
		this.interfaceContainer.gameObject.SetActive((!this._interfaceEnabled ? false : flag));
	}

	private void Update()
	{
		bool flag;
		if (this._disposed)
		{
			return;
		}
		bool flag1 = Application.isEditor;
		bool isWeakDevice = VideoRecordingView.IsWeakDevice;
		bool flag2 = (!this.IsRecordingSupportedFuture.IsCompleted ? false : !this.IsRecordingSupportedFuture.IsFaulted);
		GameObject gameObject = this.interfaceContainer.gameObject;
		if (!this._interfaceEnabled || isWeakDevice)
		{
			flag = false;
		}
		else
		{
			flag = (!flag1 ? flag2 : true);
		}
		gameObject.SetActive(flag);
		if (this.interfaceContainer.gameObject.activeInHierarchy)
		{
			if (!Application.isEditor)
			{
				bool flag3 = this._everyplayWrapper.IsPaused();
				bool flag4 = this._everyplayWrapper.IsRecording();
				this.pauseButton.isEnabled = (!flag2 ? false : !flag3);
				this.recordButton.isEnabled = (!flag2 ? false : !flag4);
				this.resumeButton.isEnabled = (!flag2 ? false : flag3);
				this.shareButton.isEnabled = (!flag2 ? false : !flag4);
				this.stopButton.isEnabled = (flag4 ? true : flag3);
			}
			this.pauseButton.gameObject.SetActive(this.CurrentState == EveryplayWrapper.State.Recording);
			this.recordButton.gameObject.SetActive((this.CurrentState == EveryplayWrapper.State.Initial ? true : this.CurrentState == EveryplayWrapper.State.Idle));
			this.resumeButton.gameObject.SetActive(this.CurrentState == EveryplayWrapper.State.Paused);
			bool currentState = this.CurrentState == EveryplayWrapper.State.Idle;
			bool flag5 = (this.CurrentState == EveryplayWrapper.State.Recording ? true : this.CurrentState == EveryplayWrapper.State.Paused);
			this.shareButton.gameObject.SetActive(currentState);
			this.stopButton.gameObject.SetActive((this.CurrentState == EveryplayWrapper.State.Recording ? true : this.CurrentState == EveryplayWrapper.State.Paused));
			if ((currentState || flag5) && this.frame2 != null && !this.frame2.activeSelf)
			{
				if (this.frame1 != null)
				{
					this.frame1.SetActive(false);
				}
				if (this.frame2 != null)
				{
					this.frame2.SetActive(true);
				}
			}
			TimeSpan elapsed = this._everyplayWrapper.Elapsed;
			string str = string.Format("{0}:{1:00}", (int)elapsed.TotalMinutes, elapsed.Seconds);
			switch (this.CurrentState)
			{
				case EveryplayWrapper.State.Recording:
				{
					this.caption.text = string.Format("{0}\n{1}", this.recLocalize, str);
					break;
				}
				case EveryplayWrapper.State.Paused:
				{
					this.caption.text = string.Format("{0}\n{1}", this.recLocalize, str);
					break;
				}
				case EveryplayWrapper.State.Idle:
				{
					this.caption.text = string.Format("{0}\n{1}", this.doneLocalize, str);
					break;
				}
				default:
				{
					this.caption.text = this.startRecLocalize;
					break;
				}
			}
		}
		if (Time.frameCount % 300 == 0 && this._everyplayWrapper.IsSupported())
		{
			this._everyplayWrapper.CheckState();
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitUntilEveryplayRecordingIsSupported()
	{
		VideoRecordingView.u003cWaitUntilEveryplayRecordingIsSupportedu003ec__Iterator19C variable = null;
		return variable;
	}

	public event EventHandler Started
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.Started += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.Started -= value;
		}
	}

	public event EventHandler Starting
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.Starting += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.Starting -= value;
		}
	}
}