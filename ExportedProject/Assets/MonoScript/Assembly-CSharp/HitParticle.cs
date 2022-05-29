using System;
using UnityEngine;

public class HitParticle : MonoBehaviour
{
	public const float DefaultHeightFlyOutEffect = 1.75f;

	private float liveTime = -1f;

	public float maxliveTime = 0.3f;

	public bool isUseMine;

	private Transform myTransform;

	public ParticleSystem myParticleSystem;

	public HitParticle()
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
		base.gameObject.SetActive(true);
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		this.myParticleSystem.enableEmission = true;
	}

	public void StartShowParticle(Vector3 pos, Quaternion rot, bool _isUseMine, Vector3 flyOutPos)
	{
		this.StartShowParticle(pos, rot, _isUseMine);
		if (this.myTransform.childCount > 0)
		{
			this.myParticleSystem.transform.position = flyOutPos;
		}
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