using System;
using UnityEngine;

public class HideInLocal : MonoBehaviour
{
	public HideInLocal()
	{
	}

	private void Start()
	{
		if (!Defs.isInet || Defs.isDaterRegim)
		{
			base.gameObject.SetActive(false);
		}
	}
}