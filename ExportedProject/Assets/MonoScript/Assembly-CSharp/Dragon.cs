using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Dragon : MonoBehaviour
{
	public GameObject child;

	private AudioSource childSound;

	public GameObject wingsFirst;

	public GameObject wingsSecond;

	public Dragon()
	{
	}

	[DebuggerHidden]
	private IEnumerator dragonfly()
	{
		Dragon.u003cdragonflyu003ec__Iterator1B3 variable = null;
		return variable;
	}

	private void Start()
	{
		if (this.child == null || this.wingsFirst == null || this.wingsSecond == null)
		{
			return;
		}
		this.childSound = this.child.GetComponent<AudioSource>();
		base.StartCoroutine(this.dragonfly());
	}
}