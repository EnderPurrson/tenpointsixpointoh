using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	public GameObject child;

	public GameObject sound;

	public Lightning()
	{
	}

	[DebuggerHidden]
	private IEnumerator lightning()
	{
		Lightning.u003clightningu003ec__Iterator1B9 variable = null;
		return variable;
	}

	private void Start()
	{
		if (this.child == null)
		{
			return;
		}
		if (this.sound != null)
		{
			this.sound.SetActive(false);
		}
		base.StartCoroutine(this.lightning());
	}
}