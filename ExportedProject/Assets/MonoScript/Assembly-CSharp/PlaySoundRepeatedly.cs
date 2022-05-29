using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlaySoundRepeatedly : MonoBehaviour
{
	public float Delay;

	public int Repeats = 3;

	public float Between = 1f;

	public float Interval = 60f;

	public PlaySoundRepeatedly()
	{
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.SoundCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator SoundCoroutine()
	{
		PlaySoundRepeatedly.u003cSoundCoroutineu003ec__Iterator177 variable = null;
		return variable;
	}
}