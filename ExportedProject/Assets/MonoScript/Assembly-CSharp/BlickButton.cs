using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BlickButton : MonoBehaviour
{
	public float firstSdvig;

	public float blickPeriod = 3f;

	public float blickSpeed = 0.3f;

	public UISprite blickSprite;

	public UIButton baseButton;

	public string baseNameSprite;

	public int countFrame;

	public BlickButton()
	{
	}

	[DebuggerHidden]
	private IEnumerator Blink()
	{
		BlickButton.u003cBlinku003ec__IteratorA variable = null;
		return variable;
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		BlickButton.u003cMyWaitForSecondsu003ec__IteratorB variable = null;
		return variable;
	}

	private void Start()
	{
		this.blickSprite.gameObject.SetActive(false);
		base.StartCoroutine(this.Blink());
	}

	private void Update()
	{
	}
}