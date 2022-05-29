using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

internal sealed class GameServicesController : MonoBehaviour
{
	private static GameServicesController _instance;

	public static GameServicesController Instance
	{
		get
		{
			return GameServicesController._instance;
		}
	}

	public GameServicesController()
	{
	}

	private void Awake()
	{
		if (GameServicesController._instance != null)
		{
			UnityEngine.Debug.LogWarning(string.Concat(base.GetType().Name, " already exists."));
		}
		GameServicesController._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	[DebuggerHidden]
	private static IEnumerator WaitAndIncrementBeginnerAchievementCoroutine()
	{
		return new GameServicesController.u003cWaitAndIncrementBeginnerAchievementCoroutineu003ec__Iterator143();
	}

	public void WaitAuthenticationAndIncrementBeginnerAchievement()
	{
		using (StopwatchLogger stopwatchLogger = new StopwatchLogger("WaitAuthenticationAndIncrementBeginnerAchievement()"))
		{
			base.StartCoroutine(GameServicesController.WaitAndIncrementBeginnerAchievementCoroutine());
		}
	}
}