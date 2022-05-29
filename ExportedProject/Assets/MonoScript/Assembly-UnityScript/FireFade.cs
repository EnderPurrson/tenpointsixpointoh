using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class FireFade : MonoBehaviour
{
	public float smokeDestroyTime;

	public float destroySpeed;

	private bool destroyEnabled;

	public FireFade()
	{
		this.smokeDestroyTime = (float)6;
		this.destroySpeed = 0.05f;
	}

	public override void Main()
	{
	}

	public override IEnumerator Start()
	{
		return (new FireFade.u0024Startu002419(this)).GetEnumerator();
	}

	public override void Update()
	{
		if (this.destroyEnabled)
		{
			ParticleRenderer component = (ParticleRenderer)this.GetComponent(typeof(ParticleRenderer));
			Color color = component.materials[1].GetColor("_TintColor");
			color.a = color.a - this.destroySpeed * Time.deltaTime;
			component.materials[1].SetColor("_TintColor", color);
		}
	}
}