using System;
using UnityEngine;

public class Button_light : MonoBehaviour
{
	public UITexture lightTexture;

	public Button_light()
	{
	}

	private void OnPress(bool isDown)
	{
		this.lightTexture.enabled = isDown;
	}

	private void Start()
	{
		this.lightTexture.enabled = false;
	}
}