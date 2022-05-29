using System;
using System.Reflection;
using UnityEngine;

internal sealed class Bullet : MonoBehaviour
{
	private float LifeTime = 0.5f;

	private float RespawnTime;

	public float bulletSpeed = 200f;

	public float lifeS = 100f;

	public Vector3 startPos;

	public Vector3 endPos;

	public bool isUse;

	public TrailRenderer myRender;

	public Bullet()
	{
	}

	[Obfuscation(Exclude=true)]
	private void RemoveSelf()
	{
		base.transform.position = new Vector3(-10000f, -10000f, -10000f);
		this.myRender.enabled = false;
		this.isUse = false;
		base.gameObject.SetActive(false);
	}

	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	public void StartBullet()
	{
		base.gameObject.SetActive(true);
		base.CancelInvoke("RemoveSelf");
		base.Invoke("RemoveSelf", this.LifeTime);
		base.transform.position = this.startPos;
		this.isUse = true;
		this.myRender.enabled = true;
	}

	private void Update()
	{
		if (!this.isUse)
		{
			return;
		}
		Transform transforms = base.transform;
		Vector3 vector3 = transforms.position;
		Vector3 vector31 = this.endPos - this.startPos;
		transforms.position = vector3 + ((vector31.normalized * this.bulletSpeed) * Time.deltaTime);
		if (Vector3.SqrMagnitude(this.startPos - base.transform.position) >= this.lifeS * this.lifeS)
		{
			this.RemoveSelf();
		}
	}
}