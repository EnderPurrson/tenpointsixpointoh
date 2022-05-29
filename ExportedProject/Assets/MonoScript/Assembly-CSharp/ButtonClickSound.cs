using System;
using UnityEngine;

public sealed class ButtonClickSound : MonoBehaviour
{
	public static ButtonClickSound Instance;

	public AudioClip Click;

	public ButtonClickSound()
	{
	}

	public void PlayClick()
	{
		if (this.Click != null && Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.Click);
		}
	}

	private void Start()
	{
		ButtonClickSound.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void TryPlayClick()
	{
		if (ButtonClickSound.Instance == null)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
	}
}