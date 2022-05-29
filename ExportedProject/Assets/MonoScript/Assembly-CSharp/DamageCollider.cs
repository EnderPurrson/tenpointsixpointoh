using System;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
	public float damage;

	public float frequency;

	private bool _playerRegistered;

	private float _remainsTimeToHit;

	private Transform cachedTransform;

	public DamageCollider()
	{
	}

	private void CauseDamage()
	{
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.GetDamageFromEnv(this.damage, Vector3.zero);
		}
	}

	public void RegisterPlayer()
	{
		this._playerRegistered = true;
		this._remainsTimeToHit = this.frequency;
		this.CauseDamage();
	}

	private void Start()
	{
		this.cachedTransform = base.transform;
	}

	public void UnregisterPlayer()
	{
		this._playerRegistered = false;
	}

	private void Update()
	{
		if (this._playerRegistered)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC == null)
			{
				this._playerRegistered = false;
				return;
			}
			this._remainsTimeToHit -= Time.deltaTime;
			if (this._remainsTimeToHit <= 0f)
			{
				this._remainsTimeToHit = this.frequency;
				this.CauseDamage();
			}
		}
	}
}