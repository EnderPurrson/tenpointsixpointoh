using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExplosionDestroyer : MonoBehaviour
{
	public float Time = 30f;

	public ExplosionDestroyer()
	{
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.Remove());
	}

	[DebuggerHidden]
	private IEnumerator Remove()
	{
		ExplosionDestroyer.u003cRemoveu003ec__Iterator25 variable = null;
		return variable;
	}
}