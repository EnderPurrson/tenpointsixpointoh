using Rilisoft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class EveryplayWrapper
{
	private EveryplayWrapper.State _currenState;

	private static EveryplayWrapper _instance;

	private Stopwatch _stopwatch = new Stopwatch();

	public EveryplayWrapper.State CurrentState
	{
		get
		{
			return this._currenState;
		}
	}

	public TimeSpan Elapsed
	{
		get
		{
			return this._stopwatch.Elapsed;
		}
	}

	public long ElapsedMilliseconds
	{
		get
		{
			return this._stopwatch.ElapsedMilliseconds;
		}
	}

	public static EveryplayWrapper Instance
	{
		get
		{
			if (EveryplayWrapper._instance == null)
			{
				EveryplayWrapper._instance = new EveryplayWrapper();
			}
			return EveryplayWrapper._instance;
		}
	}

	public EveryplayWrapper()
	{
	}

	private void CheckCommand(string command, params EveryplayWrapper.State[] expectedStates)
	{
		if (Array.FindIndex<EveryplayWrapper.State>(expectedStates, (EveryplayWrapper.State s) => s == this.CurrentState) != -1)
		{
			string str = string.Format("{0} command in valid state {1}.", command, this.CurrentState);
			UnityEngine.Debug.Log(str);
		}
		else
		{
			string str1 = string.Format("{0} command in invalid state {1}.", command, this.CurrentState);
			UnityEngine.Debug.LogError(str1);
		}
	}

	public bool CheckState()
	{
		bool flag = true;
		switch (this.CurrentState)
		{
			case EveryplayWrapper.State.Initial:
			{
				if (this.IsRecording())
				{
					string str = string.Format("Everyplay.IsRecording() in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str);
					flag = false;
				}
				if (this._stopwatch.IsRunning)
				{
					string str1 = string.Format("Stopwatch.IsRunning in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str1);
					flag = false;
				}
				break;
			}
			case EveryplayWrapper.State.Recording:
			{
				if (!this.IsRecording())
				{
					string str2 = string.Format("!Everyplay.IsRecording() in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str2);
					flag = false;
				}
				if (this.IsPaused())
				{
					string str3 = string.Format("Everyplay.IsPaused() in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str3);
					flag = false;
				}
				if (!this._stopwatch.IsRunning)
				{
					string str4 = string.Format("!Stopwatch.IsRunning in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str4);
					flag = false;
				}
				break;
			}
			case EveryplayWrapper.State.Paused:
			{
				if (!this.IsPaused())
				{
					string str5 = string.Format("!Everyplay.IsPaused() in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str5);
					flag = false;
				}
				if (this._stopwatch.IsRunning)
				{
					string str6 = string.Format("Stopwatch.IsRunning in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str6);
					flag = false;
				}
				break;
			}
			case EveryplayWrapper.State.Idle:
			{
				if (this.IsRecording())
				{
					string str7 = string.Format("Everyplay.IsRecording() in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str7);
					flag = false;
				}
				if (this._stopwatch.IsRunning)
				{
					string str8 = string.Format("Stopwatch.IsRunning in {0} state.", this.CurrentState);
					UnityEngine.Debug.LogError(str8);
					flag = false;
				}
				break;
			}
		}
		return flag;
	}

	public bool IsPaused()
	{
		if (!this.IsSupported())
		{
			return false;
		}
		return Everyplay.IsPaused();
	}

	public bool IsRecording()
	{
		if (!this.IsSupported())
		{
			return false;
		}
		return Everyplay.IsRecording();
	}

	public bool IsRecordingSupported()
	{
		if (!this.IsSupported())
		{
			return false;
		}
		return Everyplay.IsRecordingSupported();
	}

	public bool IsSupported()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			return false;
		}
		return Everyplay.IsSupported();
	}

	public void Pause()
	{
		if (!this.IsSupported())
		{
			return;
		}
		this.CheckCommand("Pause", new EveryplayWrapper.State[] { EveryplayWrapper.State.Recording });
		Everyplay.PauseRecording();
		this._currenState = EveryplayWrapper.State.Paused;
		this._stopwatch.Stop();
	}

	public void Record()
	{
		if (!this.IsSupported())
		{
			return;
		}
		this.CheckCommand("Record", new EveryplayWrapper.State[] { EveryplayWrapper.State.Initial, EveryplayWrapper.State.Idle });
		Everyplay.StartRecording();
		this._currenState = EveryplayWrapper.State.Recording;
		this._stopwatch.Reset();
		this._stopwatch.Start();
	}

	public void Resume()
	{
		if (!this.IsSupported())
		{
			return;
		}
		this.CheckCommand("Resume", new EveryplayWrapper.State[] { EveryplayWrapper.State.Paused });
		Everyplay.ResumeRecording();
		this._currenState = EveryplayWrapper.State.Recording;
		this._stopwatch.Start();
	}

	public void SetMetadata(Dictionary<string, object> dict)
	{
		if (!this.IsSupported())
		{
			return;
		}
		Everyplay.SetMetadata(dict);
	}

	public void Share()
	{
		if (!this.IsSupported())
		{
			return;
		}
		this.CheckCommand("Share", new EveryplayWrapper.State[] { EveryplayWrapper.State.Idle });
		Everyplay.ShowSharingModal();
	}

	public void Stop()
	{
		if (!this.IsSupported())
		{
			return;
		}
		this.CheckCommand("Stop", new EveryplayWrapper.State[] { EveryplayWrapper.State.Recording, EveryplayWrapper.State.Paused });
		string activeScene = SceneManager.GetActiveScene().name ?? string.Empty;
		UnityEngine.Debug.LogFormat("Trying to add metadata to shared video.    Map: '{0}'", new object[] { activeScene });
		Everyplay.SetMetadata("map", activeScene);
		Everyplay.StopRecording();
		this._currenState = EveryplayWrapper.State.Idle;
		this._stopwatch.Stop();
	}

	public enum State
	{
		Initial,
		Recording,
		Paused,
		Idle
	}
}