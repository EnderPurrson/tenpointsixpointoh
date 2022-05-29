using System;
using UnityEngine;

public class ParticleBonuse : MonoBehaviour
{
	public float maxTimer = 2f;

	public float timer = -1f;

	public ParticleBonuse()
	{
	}

	public void ShowParticle()
	{
		base.gameObject.SetActive(true);
		this.timer = this.maxTimer;
	}

	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}