using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class FireFade : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Start_002419 : GenericGenerator<WaitForSeconds>
	{
		internal FireFade _0024self__002421;

		public _0024Start_002419(FireFade self_)
		{
			_0024self__002421 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return (IEnumerator<WaitForSeconds>)(object)new _0024(_0024self__002421);
		}
	}

	public float smokeDestroyTime;

	public float destroySpeed;

	private bool destroyEnabled;

	public FireFade()
	{
		smokeDestroyTime = 6f;
		destroySpeed = 0.05f;
	}

	public override IEnumerator Start()
	{
		return new _0024Start_002419(this).GetEnumerator();
	}

	public override void Update()
	{
		if (destroyEnabled)
		{
			ParticleRenderer particleRenderer = (ParticleRenderer)GetComponent(typeof(ParticleRenderer));
			Color color = particleRenderer.materials[1].GetColor("_TintColor");
			color.a -= destroySpeed * Time.deltaTime;
			particleRenderer.materials[1].SetColor("_TintColor", color);
		}
	}

	public override void Main()
	{
	}
}
