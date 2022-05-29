using System;
using UnityEngine;

public class WallBloodParticle : MonoBehaviour
{
	private float liveTime = -1f;

	private float maxliveTime = 0.1f;

	public bool isUseMine;

	private Transform myTransform;

	public ParticleSystem myParticleSystem;

	public WallBloodParticle()
	{
	}

	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
		this.myParticleSystem.enableEmission = false;
		base.gameObject.SetActive(false);
	}

	public void StartShowParticle(Vector3 pos, Quaternion rot, bool _isUseMine)
	{
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		this.myParticleSystem.enableEmission = true;
		base.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (this.liveTime < 0f)
		{
			return;
		}
		this.liveTime -= Time.deltaTime;
		if (this.liveTime < 0f)
		{
			this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
			this.myParticleSystem.enableEmission = false;
			this.isUseMine = false;
			base.gameObject.SetActive(false);
		}
	}
}