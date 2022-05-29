using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class UpdatesChecker : MonoBehaviour
{
	private const string ActionAddress = "http://pixelgunserver.com/~rilisoft/action.php";

	private UpdatesChecker.Store _currentStore;

	public UpdatesChecker()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this._currentStore = UpdatesChecker.Store.Unknown;
		RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
		switch (buildTargetPlatform)
		{
			case RuntimePlatform.IPhonePlayer:
			{
				this._currentStore = UpdatesChecker.Store.Ios;
				break;
			}
			case RuntimePlatform.Android:
			{
				Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
				if (androidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					this._currentStore = UpdatesChecker.Store.Amazon;
				}
				else if (androidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					this._currentStore = UpdatesChecker.Store.Play;
				}
				break;
			}
			default:
			{
				if (buildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					this._currentStore = UpdatesChecker.Store.Wp8;
					break;
				}
				else
				{
					break;
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator CheckUpdatesCoroutine(UpdatesChecker.Store store)
	{
		UpdatesChecker.u003cCheckUpdatesCoroutineu003ec__Iterator1DC variable = null;
		return variable;
	}

	private void OnApplicationPause(bool pause)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(">>> UpdatesChecker.OnApplicationPause()");
		}
		if (!pause)
		{
			base.StartCoroutine(this.CheckUpdatesCoroutine(this._currentStore));
		}
	}

	private void Start()
	{
		base.StartCoroutine(this.CheckUpdatesCoroutine(this._currentStore));
	}

	private enum Store
	{
		Ios,
		Play,
		Wp8,
		Amazon,
		Unknown
	}
}