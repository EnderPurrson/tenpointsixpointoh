using System;
using UnityEngine;

public class DamagedExplosionObject : BaseExplosionObject
{
	public const string NameTag = "DamagedExplosion";

	[Header("Damaged Health settings")]
	public float healthPoints = 100f;

	[Range(1f, 100f)]
	public float percentHealthForFireEffect = 95f;

	[Header("Damaged Effect settings")]
	public GameObject fireEffect;

	public float timeToDestroyByFire = 5f;

	private float _maxHealth;

	public DamagedExplosionObject()
	{
	}

	public void GetDamage(float damage)
	{
		if (this.healthPoints <= 0f)
		{
			return;
		}
		if (this.healthPoints / 100f * this.healthPoints <= this.percentHealthForFireEffect && !this.fireEffect.activeSelf)
		{
			this.SetVisibleFireEffect(true);
			base.Invoke("RunExplosion", this.timeToDestroyByFire);
		}
		this.healthPoints += damage;
		if (this.healthPoints <= 0f)
		{
			this.healthPoints = 0f;
			base.RunExplosion();
		}
	}

	protected override void InitializeData()
	{
		base.InitializeData();
		this._maxHealth = this.healthPoints;
		this.SetVisibleFireEffect(false);
	}

	private void SetVisibleFireEffect(bool visible)
	{
		if (!this.isMultiplayerMode)
		{
			this.fireEffect.SetActive(visible);
		}
		else
		{
			this.SetVisibleFireEffectRpc(visible);
			this.photonView.RPC("SetVisibleFireEffectRpc", PhotonTargets.Others, new object[] { visible });
		}
	}

	[PunRPC]
	[RPC]
	private void SetVisibleFireEffectRpc(bool visible)
	{
		this.fireEffect.SetActive(visible);
	}

	public static void TryApplyDamageToObject(GameObject explosionObject, float damage)
	{
		DamagedExplosionObject component = explosionObject.GetComponent<DamagedExplosionObject>();
		if (component != null)
		{
			component.GetDamage(damage);
		}
	}
}