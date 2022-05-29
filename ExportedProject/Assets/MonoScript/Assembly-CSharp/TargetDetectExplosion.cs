using System;
using UnityEngine;

public class TargetDetectExplosion : MonoBehaviour
{
	[Header("Detect settings")]
	public float durationBeforeExplosion;

	public DamagedExplosionObject explosionScript;

	private bool _isEnter;

	public TargetDetectExplosion()
	{
	}

	private void Awake()
	{
		if (this.explosionScript == null)
		{
			this.explosionScript = base.transform.parent.GetComponent<DamagedExplosionObject>();
		}
	}

	private bool IsTargetAvailable(Transform targetTransform)
	{
		if (targetTransform.Equals(base.transform))
		{
			return false;
		}
		return (targetTransform.CompareTag("Player") || targetTransform.CompareTag("Enemy") ? true : targetTransform.CompareTag("Turret"));
	}

	private void OnTargetEnter()
	{
		this.explosionScript.GetDamage(-this.explosionScript.healthPoints);
	}

	private void OnTriggerEnter(Collider collisionObj)
	{
		if (!this.IsTargetAvailable(collisionObj.transform.root))
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
			this.OnTargetEnter();
		}
		else
		{
			base.Invoke("OnTargetEnter", this.durationBeforeExplosion);
		}
	}
}