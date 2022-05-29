using System;
using UnityEngine;

public sealed class HoleScript : MonoBehaviour
{
	public float liveTime = -1f;

	public float maxliveTime = 3f;

	public bool isUseMine;

	private Transform myTransform;

	public HoleScript()
	{
	}

	public void Init()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
		base.gameObject.SetActive(false);
	}

	public void StartShowHole(Vector3 pos, Quaternion rot, bool _isUseMine)
	{
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		base.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (this.liveTime < 0f)
		{
			return;
		}
		this.liveTime -= Time.deltaTime;
		if (this.liveTime < 0f && this.myTransform)
		{
			this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
			this.isUseMine = false;
			base.gameObject.SetActive(false);
		}
	}
}