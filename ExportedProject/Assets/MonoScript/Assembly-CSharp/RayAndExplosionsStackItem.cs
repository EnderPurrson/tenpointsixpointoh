using System;
using System.Reflection;
using UnityEngine;

public class RayAndExplosionsStackItem : MonoBehaviour
{
	public string myName;

	public float timeDeactivate = 1f;

	private bool isNotAutoEnd;

	public RayAndExplosionsStackItem()
	{
	}

	[Obfuscation(Exclude=true)]
	public void Deactivate()
	{
		base.CancelInvoke("Deactivate");
		if (RayAndExplosionsStackController.sharedController != null)
		{
			if (base.GetComponent<AudioSource>())
			{
				base.GetComponent<AudioSource>().Stop();
			}
			RayAndExplosionsStackController.sharedController.ReturnObjectFromName(base.gameObject, this.myName);
		}
	}

	private void OnEnable()
	{
		this.isNotAutoEnd = base.GetComponent<FreezerRay>() == null;
		if (base.GetComponent<AudioSource>() && Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().Play();
		}
		base.Invoke("Deactivate", this.timeDeactivate);
	}

	private void Start()
	{
		this.isNotAutoEnd = base.GetComponent<FreezerRay>() == null;
		if (this.isNotAutoEnd)
		{
			base.Invoke("Deactivate", this.timeDeactivate);
		}
	}
}