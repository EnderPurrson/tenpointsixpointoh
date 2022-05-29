using System;
using UnityEngine;

public class CustomButtonSound : MonoBehaviour
{
	public AudioClip clickSound;

	public CustomButtonSound()
	{
	}

	private void OnClick()
	{
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.clickSound, 1f, 1f);
		}
	}
}