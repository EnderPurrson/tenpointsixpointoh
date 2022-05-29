using Rilisoft;
using System;
using UnityEngine;

public class ShadowOnOff : MonoBehaviour
{
	public ShadowOnOff()
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