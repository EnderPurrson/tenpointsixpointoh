using System;
using UnityEngine;

public class HeadShotParticle : MonoBehaviour
{
	private float liveTime = -1f;

	public float maxliveTime = 1.5f;

	public bool isUseMine;

	private Transform myTransform;

	public ParticleEmitter myParticleSystem;

	public HeadShotParticle()
	{
	}

	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
		this.myParticleSystem.emit = false;
	}

	public void StartShowParticle(Vector3 pos, Quaternion rot, bool _isUseMine)
	{
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		this.myParticleSystem.emit = true;
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
			this.myParticleSystem.emit = false;
			this.isUseMine = false;
		}
	}
}