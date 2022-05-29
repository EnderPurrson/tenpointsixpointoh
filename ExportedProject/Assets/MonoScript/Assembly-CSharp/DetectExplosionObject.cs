using System;
using UnityEngine;

public class DetectExplosionObject : BaseExplosionObject
{
	[Header("Detect settings")]
	public float durationBeforeExplosion;

	private bool _isEnter;

	public DetectExplosionObject()
	{
	}

	private void Awake()
	{
		this.SetEnableDetectCollider(false);
	}

	private void CollisionEvent(GameObject collisionObj)
	{
		if (!base.IsTargetAvailable(collisionObj.transform.root))
		{
			return;
		}
		if (this._isEnter)
		{
			return;
		}
		this._isEnter = true;
		if (this.durationBeforeExplosion == 0f)
		{
			base.RunExplosion();
		}
		else
		{
			base.Invoke("RunExplosion", this.durationBeforeExplosion);
		}
	}

	protected override void InitializeData()
	{
		base.InitializeData();
		this.SetEnableDetectCollider(true);
	}

	private void OnCollisionEnter(Collision collisionObj)
	{
		this.CollisionEvent(collisionObj.gameObject);
	}

	private void OnTriggerEnter(Collider collisionObj)
	{
		this.CollisionEvent(collisionObj.gameObject);
	}

	private void SetEnableDetectCollider(bool enable)
	{
		if (base.GetComponent<Collider>() == null)
		{
			return;
		}
		base.GetComponent<Collider>().enabled = enable;
	}
}