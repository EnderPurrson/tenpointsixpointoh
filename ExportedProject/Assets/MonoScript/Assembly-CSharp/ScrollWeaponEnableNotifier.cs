using System;
using UnityEngine;

public class ScrollWeaponEnableNotifier : MonoBehaviour
{
	public InGameGUI inGameGui;

	public ScrollWeaponEnableNotifier()
	{
	}

	private void OnEnable()
	{
		this.inGameGui.StartCoroutine(this.inGameGui._DisableSwiping(0.5f));
	}
}