using System;
using UnityEngine;

internal sealed class PauseONGuiDrawer : MonoBehaviour
{
	public Action act;

	public PauseONGuiDrawer()
	{
	}

	private void OnGUI()
	{
		if (this.act != null)
		{
			this.act();
		}
	}
}