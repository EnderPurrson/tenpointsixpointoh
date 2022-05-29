using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class MenuBackgroundMusic : MonoBehaviour
{
	private List<AudioSource> _customMusicStack = new List<AudioSource>();

	private AudioSource currentAudioSource;

	public static bool keepPlaying;

	public static MenuBackgroundMusic sharedMusic;

	private static string[] scenetsToPlayMusicOn;

	static MenuBackgroundMusic()
	{
		MenuBackgroundMusic.keepPlaying = false;
		MenuBackgroundMusic.scenetsToPlayMusicOn = new string[] { Defs.MainMenuScene, "ConnectScene", "ConnectSceneSandbox", "SettingScene", "SkinEditor", "ChooseLevel", "CampaignChooseBox", "ProfileShop", "Friends", "Clans" };
	}

	public MenuBackgroundMusic()
	{
	}

	private void HandleFreeAwardControllerStateChanged(object sender, FreeAwardController.StateEventArgs e)
	{
		string str = string.Format(CultureInfo.InvariantCulture, "HandleFreeAwardControllerStateChanged({0} -> {1})", new object[] { e.OldState, e.State });
		ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
		try
		{
			if (e.State is FreeAwardController.WatchingState)
			{
				this.Stop();
			}
			else if (e.OldState is FreeAwardController.WatchingState)
			{
				this.Play();
			}
		}
		finally
		{
			scopeLogger.Dispose();
		}
	}

	private void OnApplicationPause(bool pausing)
	{
		if (pausing)
		{
			this.PauseCurrentMusic();
		}
		else
		{
			this.PlayCurrentMusic();
		}
	}

	private void OnLevelWasLoaded(int idx)
	{
		base.StopAllCoroutines();
		CoroutineRunner.Instance.StartCoroutine(this.WaitFreeAwardControllerAndSubscribeCoroutine());
		foreach (AudioSource audioSource in this._customMusicStack)
		{
			audioSource.Stop();
		}
		this._customMusicStack.Clear();
		if (Array.IndexOf<string>(MenuBackgroundMusic.scenetsToPlayMusicOn, Application.loadedLevelName) < 0 && !MenuBackgroundMusic.keepPlaying)
		{
			this.StopMusic(base.GetComponent<AudioSource>());
		}
		else if (!base.GetComponent<AudioSource>().isPlaying && PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true))
		{
			this.PlayMusic(base.GetComponent<AudioSource>());
		}
		MenuBackgroundMusic.keepPlaying = false;
	}

	private void PauseCurrentMusic()
	{
		if (this.currentAudioSource != null)
		{
			this.currentAudioSource.Pause();
		}
	}

	internal void Play()
	{
		if (Defs.isSoundMusic)
		{
			this.PlayMusic(base.GetComponent<AudioSource>());
		}
	}

	private void PlayCurrentMusic()
	{
		if (this.currentAudioSource != null)
		{
			this.PlayMusic(this.currentAudioSource);
		}
	}

	public void PlayCustomMusicFrom(GameObject audioSourceObj)
	{
		this.RemoveNullsFromCustomMusicStack();
		if (audioSourceObj != null && Defs.isSoundMusic)
		{
			AudioSource component = audioSourceObj.GetComponent<AudioSource>();
			this.PlayMusic(component);
			if (!this._customMusicStack.Contains(component))
			{
				if (this._customMusicStack.Count > 0)
				{
					this.StopMusic(this._customMusicStack[this._customMusicStack.Count - 1]);
				}
				this._customMusicStack.Add(audioSourceObj.GetComponent<AudioSource>());
			}
		}
		string activeScene = SceneManager.GetActiveScene().name;
		if (Array.IndexOf<string>(MenuBackgroundMusic.scenetsToPlayMusicOn, activeScene) < 0)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
			if (gameObject != null)
			{
				AudioSource audioSource = gameObject.GetComponent<AudioSource>();
				if (audioSource != null)
				{
					this.StopMusic(audioSource);
				}
			}
		}
		else
		{
			this.Stop();
		}
	}

	public void PlayMusic(AudioSource audioSource)
	{
		if (audioSource == null)
		{
			return;
		}
		if (!Defs.isSoundMusic)
		{
			return;
		}
		if (Switcher.comicsSound != null && audioSource != Switcher.comicsSound.GetComponent<AudioSource>())
		{
			UnityEngine.Object.Destroy(Switcher.comicsSound);
			Switcher.comicsSound = null;
		}
		if (PhotonNetwork.connected)
		{
			float single = 0f;
			single = Convert.ToSingle(PhotonNetwork.time) - audioSource.clip.length * (float)Mathf.FloorToInt(Convert.ToSingle(PhotonNetwork.time) / audioSource.clip.length);
			audioSource.time = single;
		}
		audioSource.Play();
	}

	[DebuggerHidden]
	private IEnumerator PlayMusicInternal(AudioSource audioSource)
	{
		MenuBackgroundMusic.u003cPlayMusicInternalu003ec__IteratorBC variable = null;
		return variable;
	}

	private void RemoveNullsFromCustomMusicStack()
	{
		List<AudioSource> audioSources = this._customMusicStack;
		this._customMusicStack = new List<AudioSource>();
		foreach (AudioSource audioSource in audioSources)
		{
			if (audioSource == null)
			{
				continue;
			}
			this._customMusicStack.Add(audioSource);
		}
	}

	internal void Start()
	{
		MenuBackgroundMusic.sharedMusic = this;
		Defs.isSoundMusic = PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true);
		Defs.isSoundFX = PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Stop()
	{
		this.StopMusic(base.GetComponent<AudioSource>());
	}

	public void StopCustomMusicFrom(GameObject audioSourceObj)
	{
		this.RemoveNullsFromCustomMusicStack();
		AudioSource component = audioSourceObj.GetComponent<AudioSource>();
		if (audioSourceObj != null && component != null)
		{
			this.StopMusic(component);
			this._customMusicStack.Remove(component);
		}
		if (this._customMusicStack.Count > 0)
		{
			this.PlayMusic(this._customMusicStack[this._customMusicStack.Count - 1]);
		}
		else if (Array.IndexOf<string>(MenuBackgroundMusic.scenetsToPlayMusicOn, Application.loadedLevelName) < 0)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
			if (gameObject != null)
			{
				AudioSource audioSource = gameObject.GetComponent<AudioSource>();
				if (audioSource != null)
				{
					this.PlayMusic(audioSource);
				}
			}
		}
		else
		{
			this.Play();
		}
	}

	public void StopMusic(AudioSource audioSource)
	{
		if (audioSource == null)
		{
			return;
		}
		audioSource.Stop();
	}

	[DebuggerHidden]
	private IEnumerator StopMusicInternal(AudioSource audioSource)
	{
		MenuBackgroundMusic.u003cStopMusicInternalu003ec__IteratorBD variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator WaitFreeAwardControllerAndSubscribeCoroutine()
	{
		MenuBackgroundMusic.u003cWaitFreeAwardControllerAndSubscribeCoroutineu003ec__IteratorBB variable = null;
		return variable;
	}
}