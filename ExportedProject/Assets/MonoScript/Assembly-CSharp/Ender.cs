using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class Ender : MonoBehaviour
{
	public GUIStyle buttonStyle;

	public GameObject enderPers;

	public GameObject cam;

	public GameObject[] clouds;

	public GameObject text;

	private Camera _camera;

	private GUIText _text;

	private readonly float _pauseBeforeClouds = 1f;

	private readonly float _pauseBetweenClouds = 0.1f;

	private readonly float _pauseBetweenTexts = 3f;

	public Ender()
	{
	}

	private void OnGUI()
	{
		if (MainMenuController.sharedController != null && MainMenuController.sharedController.stubLoading.activeSelf)
		{
			return;
		}
		GUI.enabled = true;
		GUI.depth = -10000;
		Rect rect = new Rect(0f, (float)Screen.height - (float)this._camera.targetTexture.height * Defs.Coef, (float)this._camera.targetTexture.width * Defs.Coef, (float)this._camera.targetTexture.height * Defs.Coef);
		GUI.DrawTexture(rect, this._camera.targetTexture);
		rect.width = rect.width / 2f;
		if (GUI.Button(rect, string.Empty, this.buttonStyle))
		{
			MainMenu.BlockInterface = false;
			FlurryPluginWrapper.LogEvent("Ender3D");
			UnityEngine.Object.Destroy(base.gameObject);
			Application.OpenURL(MainMenu.GetEndermanUrl());
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		Ender.u003cStartu003ec__Iterator23 variable = null;
		return variable;
	}
}