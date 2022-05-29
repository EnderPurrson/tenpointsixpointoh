using System;
using UnityEngine;

[AddComponentMenu("Common/Full Screen Option")]
public class FullScreenOption : MonoBehaviour
{
	public FullScreenOption()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			if (!Screen.fullScreen)
			{
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			}
			else
			{
				Screen.SetResolution(1280, 720, false);
			}
		}
	}
}