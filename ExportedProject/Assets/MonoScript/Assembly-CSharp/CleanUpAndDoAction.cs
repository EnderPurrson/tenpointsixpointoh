using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CleanUpAndDoAction : MonoBehaviour
{
	public Texture riliFon;

	public Texture blackPixel;

	public static Action action;

	public CleanUpAndDoAction()
	{
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackPixel, ScaleMode.StretchToFill);
		GUI.DrawTexture(AppsMenu.RiliFonRect(), this.riliFon, ScaleMode.StretchToFill);
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		return new CleanUpAndDoAction.u003cStartu003ec__Iterator16();
	}
}