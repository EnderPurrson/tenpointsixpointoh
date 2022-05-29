using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Move : MonoBehaviour
{
	public Transform target;

	public float speed;

	public float smokeDestroyTime;

	public ParticleRenderer smokeStem;

	public float destroySpeed;

	public float destroySpeedStem;

	private bool destroyEnabled;

	public Move()
	{
	}

	public override void Main()
	{
	}

	public override IEnumerator Start()
	{
		return (new Move.u0024Startu002422(this)).GetEnumerator();
	}

	public override void Update()
	{
		Color color = new Color();
		this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, Time.deltaTime * this.speed);
		if (this.destroyEnabled)
		{
			ParticleRenderer component = (ParticleRenderer)this.GetComponent(typeof(ParticleRenderer));
			color = component.material.GetColor("_TintColor");
			Color color1 = this.smokeStem.material.GetColor("_TintColor");
			if (color.a > (float)0)
			{
				color.a = color.a - this.destroySpeed * Time.deltaTime;
			}
			if (color1.a > (float)0)
			{
				color1.a = color1.a - this.destroySpeedStem * Time.deltaTime;
			}
			this.smokeStem.material.SetColor("_TintColor", color1);
			component.material.SetColor("_TintColor", color);
		}
		if (color.a < (float)0)
		{
			UnityEngine.Object.Destroy(this.transform.root.gameObject);
		}
	}
}