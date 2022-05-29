using System;
using System.Reflection;
using UnityEngine;

internal sealed class RemoveExplosion : MonoBehaviour
{
	public RemoveExplosion()
	{
	}

	[Obfuscation(Exclude=true)]
	private void Remove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Start()
	{
		float single = (base.GetComponent<ParticleSystem>() == null ? 0.1f : base.GetComponent<ParticleSystem>().duration);
		if (base.GetComponent<AudioSource>() && base.GetComponent<AudioSource>().enabled && Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().Play();
		}
		base.Invoke("Remove", 7f);
	}
}