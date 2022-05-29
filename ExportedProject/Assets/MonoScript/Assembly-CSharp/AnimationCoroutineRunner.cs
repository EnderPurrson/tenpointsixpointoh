using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimationCoroutineRunner : MonoBehaviour
{
	public AnimationCoroutineRunner()
	{
	}

	[DebuggerHidden]
	public IEnumerator Play(Animation animation, string clipName, bool useTimeScale, Action onComplete)
	{
		AnimationCoroutineRunner.u003cPlayu003ec__Iterator2 variable = null;
		return variable;
	}

	public void StartPlay(Animation animation, string clipName, bool useTimeScale, Action onComplete)
	{
		base.StartCoroutine(this.Play(animation, clipName, useTimeScale, onComplete));
	}
}