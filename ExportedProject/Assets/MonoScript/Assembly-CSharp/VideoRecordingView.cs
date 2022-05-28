using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using UnityEngine;

internal sealed class VideoRecordingView : MonoBehaviour, IDisposable
{
	[CompilerGenerated]
	private sealed class _003CBindHandler_003Ec__AnonStorey302
	{
		internal EventHandler handler;

		internal ButtonHandler buttonHandler;

		internal void _003C_003Em__473()
		{
			buttonHandler.Clicked -= handler;
		}
	}

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

	[CompilerGenerated]
	private static Func<Action, bool> _003C_003Ef__am_0024cache13;

	[CompilerGenerated]
	private static Func<UnityEngine.Object, bool> _003C_003Ef__am_0024cache14;

	public bool InterfaceEnabled
	{
		get
		{
			return _interfaceEnabled;
		}
		set
		{
			_interfaceEnabled = value;
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
			return BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon;
		}
	}

	private Task IsRecordingSupportedFuture
	{
		get
		{
			return _isRecordingSupportedPromise.Task;
		}
	}

	private EveryplayWrapper.State CurrentState
	{
		get
		{
			return _everyplayWrapper.CurrentState;
		}
	}

	public event EventHandler Starting;

	public event EventHandler Started;

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		List<Action> disposeActions = _disposeActions;
		if (_003C_003Ef__am_0024cache13 == null)
		{
			_003C_003Ef__am_0024cache13 = _003CDispose_003Em__471;
		}
		foreach (Action item in disposeActions.Where(_003C_003Ef__am_0024cache13))
		{
			item();
		}
		_disposed = true;
	}

	private void SetButtonsText()
	{
		startRecLocalize = LocalizationStore.Key_0207;
		recLocalize = LocalizationStore.Key_0558;
		doneLocalize = LocalizationStore.Key_0559;
	}

	private void Start()
	{
		CoroutineRunner.Instance.StartCoroutine(WaitUntilEveryplayRecordingIsSupported());
		LocalizationStore.AddEventCallAfterLocalize(HandleLocalizationChanged);
		SetButtonsText();
		bool flag = false;
		try
		{
			flag = Application.isEditor || _everyplayWrapper.IsSupported();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		if (!flag)
		{
			Debug.Log("Everyplay is not supported.");
			base.gameObject.SetActive(false);
			return;
		}
		UnityEngine.Object[] source = new UnityEngine.Object[7] { interfaceContainer, caption, pauseButton, recordButton, resumeButton, shareButton, stopButton };
		if (_003C_003Ef__am_0024cache14 == null)
		{
			_003C_003Ef__am_0024cache14 = _003CStart_003Em__472;
		}
		if (source.Any(_003C_003Ef__am_0024cache14))
		{
			_disposed = true;
			return;
		}
		BindHandler(pauseButton, HandlePauseButton);
		BindHandler(recordButton, HandleRecordButton);
		BindHandler(resumeButton, HandleResumeButton);
		BindHandler(stopButton, HandleStopButton);
		BindHandler(shareButton, HandleShareButton);
		pauseButton.gameObject.SetActive(false);
		recordButton.gameObject.SetActive(true);
		resumeButton.gameObject.SetActive(false);
		shareButton.gameObject.SetActive(false);
		stopButton.gameObject.SetActive(false);
		interfaceContainer.gameObject.SetActive(_interfaceEnabled && flag);
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(HandleLocalizationChanged);
		Dispose();
	}

	private void HandleLocalizationChanged()
	{
		SetButtonsText();
	}

	private IEnumerator WaitUntilEveryplayRecordingIsSupported()
	{
		WaitForSeconds delay = new WaitForSeconds(5f);
		Task isRecordingSupportedFuture = IsRecordingSupportedFuture;
		while (!isRecordingSupportedFuture.IsCompleted)
		{
			try
			{
				if (_everyplayWrapper.IsRecordingSupported())
				{
					_isRecordingSupportedPromise.TrySetResult(true);
					break;
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				_isRecordingSupportedPromise.TrySetException(ex);
				break;
			}
			yield return delay;
		}
		Debug.LogFormat("isRecordingSupportedFuture.IsFaulted: {0}", isRecordingSupportedFuture.IsFaulted);
	}

	private void Update()
	{
		if (_disposed)
		{
			return;
		}
		bool isEditor = Application.isEditor;
		bool isWeakDevice = IsWeakDevice;
		bool flag = IsRecordingSupportedFuture.IsCompleted && !IsRecordingSupportedFuture.IsFaulted;
		interfaceContainer.gameObject.SetActive(_interfaceEnabled && !isWeakDevice && (isEditor || flag));
		if (interfaceContainer.gameObject.activeInHierarchy)
		{
			if (!Application.isEditor)
			{
				bool flag2 = _everyplayWrapper.IsPaused();
				bool flag3 = _everyplayWrapper.IsRecording();
				pauseButton.isEnabled = flag && !flag2;
				recordButton.isEnabled = flag && !flag3;
				resumeButton.isEnabled = flag && flag2;
				shareButton.isEnabled = flag && !flag3;
				stopButton.isEnabled = flag3 || flag2;
			}
			pauseButton.gameObject.SetActive(CurrentState == EveryplayWrapper.State.Recording);
			recordButton.gameObject.SetActive(CurrentState == EveryplayWrapper.State.Initial || CurrentState == EveryplayWrapper.State.Idle);
			resumeButton.gameObject.SetActive(CurrentState == EveryplayWrapper.State.Paused);
			bool flag4 = CurrentState == EveryplayWrapper.State.Idle;
			bool flag5 = CurrentState == EveryplayWrapper.State.Recording || CurrentState == EveryplayWrapper.State.Paused;
			shareButton.gameObject.SetActive(flag4);
			stopButton.gameObject.SetActive(CurrentState == EveryplayWrapper.State.Recording || CurrentState == EveryplayWrapper.State.Paused);
			if ((flag4 || flag5) && frame2 != null && !frame2.activeSelf)
			{
				if (frame1 != null)
				{
					frame1.SetActive(false);
				}
				if (frame2 != null)
				{
					frame2.SetActive(true);
				}
			}
			TimeSpan elapsed = _everyplayWrapper.Elapsed;
			string arg = string.Format("{0}:{1:00}", (int)elapsed.TotalMinutes, elapsed.Seconds);
			switch (CurrentState)
			{
			case EveryplayWrapper.State.Recording:
				caption.text = string.Format("{0}\n{1}", recLocalize, arg);
				break;
			case EveryplayWrapper.State.Paused:
				caption.text = string.Format("{0}\n{1}", recLocalize, arg);
				break;
			case EveryplayWrapper.State.Idle:
				caption.text = string.Format("{0}\n{1}", doneLocalize, arg);
				break;
			default:
				caption.text = startRecLocalize;
				break;
			}
		}
		if (Time.frameCount % 300 == 0 && _everyplayWrapper.IsSupported())
		{
			_everyplayWrapper.CheckState();
		}
	}

	private void BindHandler(UIButton button, EventHandler handler)
	{
		_003CBindHandler_003Ec__AnonStorey302 _003CBindHandler_003Ec__AnonStorey = new _003CBindHandler_003Ec__AnonStorey302();
		_003CBindHandler_003Ec__AnonStorey.handler = handler;
		if (!_disposed && !(button == null))
		{
			_003CBindHandler_003Ec__AnonStorey.buttonHandler = button.GetComponent<ButtonHandler>();
			if (_003CBindHandler_003Ec__AnonStorey.buttonHandler != null)
			{
				_003CBindHandler_003Ec__AnonStorey.buttonHandler.Clicked += _003CBindHandler_003Ec__AnonStorey.handler;
				_disposeActions.Add(_003CBindHandler_003Ec__AnonStorey._003C_003Em__473);
			}
		}
	}

	private void HandlePauseButton(object sender, EventArgs e)
	{
		if (!_disposed && (!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown))
		{
			_everyplayWrapper.Pause();
		}
	}

	private void HandleRecordButton(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !LoadingInAfterGame.isShowLoading && !_disposed)
		{
			NoodlePermissionGranter.GrantPermission(NoodlePermissionGranter.NoodleAndroidPermission.RECORD_AUDIO);
			NoodlePermissionGranter.GrantPermission(NoodlePermissionGranter.NoodleAndroidPermission.CAMERA);
			EventHandler starting = this.Starting;
			if (starting != null)
			{
				starting(this, EventArgs.Empty);
			}
			if (frame1 != null)
			{
				frame1.SetActive(false);
			}
			if (frame2 != null)
			{
				frame2.SetActive(true);
			}
			_everyplayWrapper.Record();
			EventHandler started = this.Started;
			if (started != null)
			{
				started(this, EventArgs.Empty);
			}
		}
	}

	private void HandleResumeButton(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !_disposed)
		{
			_everyplayWrapper.Resume();
		}
	}

	private void HandleShareButton(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !_disposed)
		{
			_everyplayWrapper.Share();
		}
	}

	private void HandleStopButton(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !_disposed)
		{
			_everyplayWrapper.Stop();
		}
	}

	[CompilerGenerated]
	private static bool _003CDispose_003Em__471(Action a)
	{
		return a != null;
	}

	[CompilerGenerated]
	private static bool _003CStart_003Em__472(UnityEngine.Object ri)
	{
		return ri == null;
	}
}
