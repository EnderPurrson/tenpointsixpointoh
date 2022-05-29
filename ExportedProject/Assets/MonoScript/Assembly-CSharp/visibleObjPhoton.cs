using System;
using UnityEngine;

public class visibleObjPhoton : MonoBehaviour
{
	public ThirdPersonNetwork1 lerpScript;

	public bool isVisible;

	public visibleObjPhoton()
	{
	}

	private void Awake()
	{
		if (!Defs.isMulti || !Defs.isInet)
		{
			base.enabled = false;
		}
	}

	private void OnBecameInvisible()
	{
		this.isVisible = false;
		if (this.lerpScript != null)
		{
			this.lerpScript.sglajEnabled = false;
		}
	}

	private void OnBecameVisible()
	{
		this.isVisible = true;
		if (this.lerpScript != null)
		{
			this.lerpScript.sglajEnabled = true;
		}
	}

	private void Start()
	{
	}
}