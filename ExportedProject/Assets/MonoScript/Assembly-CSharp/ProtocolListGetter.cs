using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class ProtocolListGetter : MonoBehaviour
{
	public static bool currentVersionIsSupported;

	private string CurrentVersionSupportedKey = string.Concat("CurrentVersionSupportedKey", GlobalGameController.AppVersion);

	public static int CurrentPlatform
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return 0;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return 1;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return 2;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return 2;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return 3;
			}
			return 101;
		}
	}

	static ProtocolListGetter()
	{
		ProtocolListGetter.currentVersionIsSupported = true;
	}

	public ProtocolListGetter()
	{
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		ProtocolListGetter.u003cStartu003ec__IteratorFE variable = null;
		return variable;
	}
}