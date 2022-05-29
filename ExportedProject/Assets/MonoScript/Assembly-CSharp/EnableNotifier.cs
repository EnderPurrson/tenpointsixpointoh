using System;
using System.Collections.Generic;
using UnityEngine;

public class EnableNotifier : MonoBehaviour
{
	public List<EventDelegate> onEnable = new List<EventDelegate>();

	public bool isSoundFX;

	public EnableNotifier()
	{
	}

	private void OnEnable()
	{
		if (!this.isSoundFX)
		{
			EventDelegate.Execute(this.onEnable);
		}
		else if (Defs.isSoundFX)
		{
			EventDelegate.Execute(this.onEnable);
		}
	}
}