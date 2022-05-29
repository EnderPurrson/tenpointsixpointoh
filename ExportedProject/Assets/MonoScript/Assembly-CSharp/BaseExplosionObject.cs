using Rilisoft;
using RilisoftBot;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseExplosionObject : MonoBehaviour
{
	private const string ExplosionAnimationName = "Broken";

	[Header("Common Settings")]
	public GameObject explosionObject;

	[Header("Common Damage settings")]
	public float radiusExplosion;

	[Header("Common Damage settings")]
	public float radiusMaxExplosion;

	public float damageZombie = 2f;

	public float[] damageByTier = new float[(int)ExpController.LevelsForTiers.Length];

	[Header("Common Effect settings")]
	public GameObject explosionEffect;

	protected bool isMultiplayerMode;

	protected PhotonView photonView;

	private ExplosionObjectRespawnController _respawnController;

	public BaseExplosionObject()
	{
	}

	private void ApplyDamage(Transform target, float distanceToTarget, float diameterExplosion, float diameterMaxExplosion)
	{
		int num;
		float single = 0f;
		if (target.CompareTag("Player"))
		{
			Player_move_c component = target.GetComponent<SkinName>().playerMoveC;
			if (!this.isMultiplayerMode)
			{
				num = ExpController.OurTierForAnyPlace();
			}
			else
			{
				num = (component.myTable == null ? 0 : ExpController.TierForLevel(component.myTable.GetComponent<NetworkStartTable>().myRanks));
			}
			int num1 = num;
			single = (distanceToTarget <= diameterMaxExplosion ? this.damageByTier[num1] : this.damageByTier[num1] * ((diameterExplosion - (distanceToTarget - diameterMaxExplosion)) / diameterExplosion));
			if (!this.isMultiplayerMode)
			{
				component.GetDamageFromEnv(single, base.transform.position);
			}
			else
			{
				component.SendDamageFromEnv(single, base.transform.position);
			}
		}
		else if (target.CompareTag("Turret"))
		{
			TurretController turretController = target.GetComponent<TurretController>();
			single = (distanceToTarget <= diameterMaxExplosion ? this.damageByTier[turretController.numUpdate] : this.damageByTier[turretController.numUpdate] * ((diameterExplosion - (distanceToTarget - diameterMaxExplosion)) / diameterExplosion));
			turretController.MinusLive(single, 0, new NetworkViewID());
		}
		else if (target.CompareTag("Enemy"))
		{
			BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(target);
			single = (distanceToTarget <= diameterMaxExplosion ? this.damageZombie : this.damageZombie * ((diameterExplosion - (distanceToTarget - diameterMaxExplosion)) / diameterExplosion));
			if (!this.isMultiplayerMode)
			{
				botScriptForObject.GetDamage(-single, null, false, false);
			}
			else
			{
				botScriptForObject.GetDamageForMultiplayer(-single, null, false);
			}
		}
		else if (target.childCount > 0 && target.GetChild(0).CompareTag("DamagedExplosion"))
		{
			DamagedExplosionObject damagedExplosionObject = target.GetChild(0).GetComponent<DamagedExplosionObject>();
			if (damagedExplosionObject != null && damagedExplosionObject.healthPoints > 0f)
			{
				damagedExplosionObject.healthPoints = 0f;
				damagedExplosionObject.Invoke("RunExplosion", 0.1f);
			}
		}
	}

	private void CheckTakeDamage()
	{
		Collider[] colliderArray = Physics.OverlapSphere(base.transform.position, this.radiusExplosion, Tools.AllWithoutDamageCollidersMask);
		if ((int)colliderArray.Length == 0)
		{
			return;
		}
		List<Transform> transforms = new List<Transform>();
		float single = this.radiusExplosion * this.radiusExplosion;
		float single1 = this.radiusMaxExplosion * this.radiusMaxExplosion;
		for (int i = 0; i < (int)colliderArray.Length; i++)
		{
			if (colliderArray[i].gameObject != null)
			{
				Transform transforms1 = colliderArray[i].transform.root;
				if (!(transforms1.gameObject == null) && !(base.transform.gameObject == null))
				{
					if (!transforms.Contains(transforms1))
					{
						if (this.IsTargetAvailable(transforms1))
						{
							float single2 = (transforms1.position - base.transform.position).sqrMagnitude;
							if (single2 <= single)
							{
								this.ApplyDamage(transforms1, single2, single, single1);
								transforms.Add(transforms1);
							}
						}
					}
				}
			}
		}
	}

	[PunRPC]
	[RPC]
	public void DestroyObjectByNetworkRpc()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			this.explosionObject.SetActive(false);
		}
		else
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
	}

	protected virtual void InitializeData()
	{
		this.isMultiplayerMode = Defs.isMulti;
		this.photonView = PhotonView.Get(this);
		this.InitializeRespawnPoint();
	}

	private void InitializeRespawnPoint()
	{
		if (!this.isMultiplayerMode || PhotonNetwork.isMasterClient)
		{
			this._respawnController = base.transform.parent.GetComponent<ExplosionObjectRespawnController>();
			return;
		}
		GameObject item = null;
		float single = Single.MaxValue;
		for (int i = 0; i < ExplosionObjectRespawnController.respawnList.Count; i++)
		{
			if (ExplosionObjectRespawnController.respawnList[i] != null)
			{
				Vector3 vector3 = ExplosionObjectRespawnController.respawnList[i].transform.position - base.transform.position;
				float single1 = vector3.sqrMagnitude;
				if (single1 < single)
				{
					single = single1;
					item = ExplosionObjectRespawnController.respawnList[i];
				}
			}
		}
		if (item == null)
		{
			this._respawnController = null;
		}
		else
		{
			base.transform.parent = item.transform;
			this._respawnController = item.GetComponent<ExplosionObjectRespawnController>();
		}
	}

	protected bool IsTargetAvailable(Transform targetTransform)
	{
		bool flag;
		if (targetTransform.Equals(base.transform))
		{
			return false;
		}
		if (targetTransform.CompareTag("Player") || targetTransform.CompareTag("Enemy") || targetTransform.CompareTag("Turret"))
		{
			flag = true;
		}
		else
		{
			flag = (targetTransform.childCount <= 0 ? false : targetTransform.GetChild(0).CompareTag("DamagedExplosion"));
		}
		return flag;
	}

	private void OnDestroy()
	{
		Initializer.damagedObj.Remove(base.gameObject);
	}

	private void PlayDestroyEffect()
	{
		UnityEngine.Object.Instantiate(this.explosionEffect, base.transform.position, Quaternion.identity);
		base.GetComponent<Animation>().Play("Broken");
	}

	[PunRPC]
	[RPC]
	public void PlayDestroyEffectRpc()
	{
		this.PlayDestroyEffect();
	}

	private void RecreateObject()
	{
		if (!this.isMultiplayerMode)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.DestroyObjectByNetworkRpc();
			this.photonView.RPC("DestroyObjectByNetworkRpc", PhotonTargets.Others, new object[0]);
		}
		if (this.isMultiplayerMode)
		{
			this.StartNewRespanObjectRpc();
			this.photonView.RPC("StartNewRespanObjectRpc", PhotonTargets.Others, new object[0]);
		}
		else if (this._respawnController != null)
		{
			this._respawnController.StartProcessNewRespawn();
		}
	}

	public void RunExplosion()
	{
		if (!this.isMultiplayerMode)
		{
			this.PlayDestroyEffect();
		}
		else
		{
			this.PlayDestroyEffect();
			this.photonView.RPC("PlayDestroyEffectRpc", PhotonTargets.Others, new object[0]);
		}
		this.CheckTakeDamage();
		this.RecreateObject();
	}

	private void Start()
	{
		this.InitializeData();
		Initializer.damagedObj.Add(base.gameObject);
	}

	[PunRPC]
	[RPC]
	public void StartNewRespanObjectRpc()
	{
		if (this._respawnController != null)
		{
			this._respawnController.StartProcessNewRespawn();
		}
	}
}