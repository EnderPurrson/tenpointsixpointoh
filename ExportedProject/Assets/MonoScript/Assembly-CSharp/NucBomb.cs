using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NucBomb : MonoBehaviour
{
	public float BeforeActivate = 12f;

	public float BeforeDestroy = 90f;

	public NucBomb()
	{
	}

	private void FixedUpdate()
	{
		base.GetComponent<AudioSource>().mute = !Defs.isSoundFX;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		NucBomb.u003cStartu003ec__IteratorD0 variable = null;
		return variable;
	}
}