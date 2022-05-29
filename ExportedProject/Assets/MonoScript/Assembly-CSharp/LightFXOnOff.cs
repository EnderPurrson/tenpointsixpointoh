using Rilisoft;
using System;
using UnityEngine;

public class LightFXOnOff : MonoBehaviour
{
	public LightFXOnOff()
	{
	}

	private void Start()
	{
		if (Device.isWeakDevice || Application.platform == RuntimePlatform.Android || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}
}