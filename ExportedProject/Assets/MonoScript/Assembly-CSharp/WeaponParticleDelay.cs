using System;
using UnityEngine;

public class WeaponParticleDelay : MonoBehaviour
{
	private Animation weaponAnimation;

	public string animationName;

	public float delay;

	public ParticleSystem partSystem;

	private bool seqStarted;

	public WeaponParticleDelay()
	{
	}

	private void Awake()
	{
		this.weaponAnimation = base.GetComponent<Animation>();
	}

	private void TurnOnParticleSystem()
	{
		this.partSystem.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (!this.weaponAnimation.IsPlaying(this.animationName))
		{
			this.seqStarted = false;
		}
		else if (!this.seqStarted)
		{
			this.seqStarted = true;
			this.partSystem.gameObject.SetActive(false);
			base.Invoke("TurnOnParticleSystem", this.delay);
		}
	}
}