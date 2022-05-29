using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
	private static CoroutineRunner _instance;

	public static CoroutineRunner Instance
	{
		get
		{
			if (CoroutineRunner._instance == null)
			{
				try
				{
					GameObject gameObject = new GameObject("CoroutineRunner");
					CoroutineRunner._instance = gameObject.AddComponent<CoroutineRunner>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(string.Concat("[Rilisoft] CoroutineRunner: Instance exception: ", exception));
				}
			}
			return CoroutineRunner._instance;
		}
	}

	static CoroutineRunner()
	{
	}

	public CoroutineRunner()
	{
	}

	[DebuggerHidden]
	public static IEnumerator WaitForSeconds(float tm)
	{
		CoroutineRunner.u003cWaitForSecondsu003ec__Iterator130 variable = null;
		return variable;
	}
}