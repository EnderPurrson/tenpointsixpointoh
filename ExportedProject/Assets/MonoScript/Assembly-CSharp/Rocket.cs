using RilisoftBot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class Rocket : MonoBehaviour
{
	private int _rocketNum;

	public string weaponName = string.Empty;

	public GameObject[] rockets;

	public float damage;

	public float radiusDamage;

	public float radiusDamageSelf;

	public float radiusImpulse;

	public float multiplayerDamage;

	public Vector2 damageRange;

	public PhotonView photonView;

	private bool isMulti;

	private bool isInet;

	private bool isCompany;

	private bool isCOOP;

	public bool isMine;

	private WeaponManager _weaponManager;

	private bool isKilled;

	public Transform target;

	public bool isRun;

	private Player_move_c myPlayerMoveC;

	private bool isStartSynh;

	private Vector3 correctPos = Vector3.zero;

	public Transform myTransform;

	private bool isFirstPos = true;

	public bool dontExecStart;

	public RocketSettings currentRocketSettings;

	public float impulseForce;

	public float impulseForceSelf;

	private int counterJumpLightning;

	private Transform targetLightning;

	private float timerFromJumpLightning = 1f;

	private float maxTimerFromJumpLightning = 1f;

	private Transform _targetDamageLightning;

	private Transform _targetDamage;

	private float progressCaptureTargetLightning;

	private List<Transform> targetsDamageLightningList = new List<Transform>();

	private bool isDetectFirstTargetLightning;

	private Rigidbody myRigidbody;

	private Transform stickedObject;

	private Vector3 stickedObjectPos;

	public bool isSlowdown
	{
		get;
		set;
	}

	public int rocketNum
	{
		get
		{
			return this._rocketNum;
		}
		set
		{
			this._rocketNum = value;
			this.currentRocketSettings = this.rockets[value].GetComponent<RocketSettings>();
		}
	}

	public float slowdownCoeff
	{
		get;
		set;
	}

	public float slowdownTime
	{
		get;
		set;
	}

	private Transform targetDamageLightning
	{
		get
		{
			return this._targetDamageLightning;
		}
		set
		{
			this._targetDamageLightning = value;
		}
	}

	public string weaponPrefabName
	{
		get;
		set;
	}

	public Rocket()
	{
	}

	private void Awake()
	{
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.isCompany = Defs.isCompany;
		this.isCOOP = Defs.isCOOP;
		this.photonView = base.GetComponent<PhotonView>();
		this.myRigidbody = base.GetComponent<Rigidbody>();
		this.myTransform = base.transform;
		if (!this.isMulti)
		{
			this.isMine = true;
		}
		else if (this.isInet)
		{
			this.isMine = this.photonView.isMine;
		}
		else
		{
			this.isMine = base.GetComponent<NetworkView>().isMine;
		}
		this._weaponManager = WeaponManager.sharedManager;
	}

	public void BazookaExplosion(string explosionName)
	{
		this.ShowExplosion(explosionName);
		GameObject gameObject = WeaponManager.sharedManager.myPlayer;
		if (gameObject == null)
		{
			return;
		}
		Vector3 vector3 = base.transform.position;
		Vector3 vector31 = gameObject.transform.position - vector3;
		float single = vector31.sqrMagnitude;
		float single1 = this.radiusImpulse * this.radiusImpulse;
		if (single < single1)
		{
			ImpactReceiver component = gameObject.GetComponent<ImpactReceiver>();
			if (component == null)
			{
				component = gameObject.AddComponent<ImpactReceiver>();
			}
			float single2 = 100f;
			if (this.radiusImpulse != 0f)
			{
				single2 = Mathf.Sqrt(single / single1);
			}
			float single3 = Mathf.Max(0f, 1f - single2);
			component.AddImpact(vector31, (!this.isMulti || this.isMine ? this.impulseForceSelf : this.impulseForce) * single3);
			if ((!this.isMulti || this.isMine) && single3 > 0.01f)
			{
				WeaponManager.sharedManager.myPlayerMoveC.isRocketJump = true;
			}
		}
	}

	[PunRPC]
	[RPC]
	private void Collide(string _weaponName, Vector3 _pos)
	{
		this.myTransform.position = _pos;
		if (Defs.inComingMessagesCounter <= 5 && this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.Lightning)
		{
			if (!string.IsNullOrEmpty(this.weaponName))
			{
				_weaponName = this.weaponName;
			}
			this.BazookaExplosion(_weaponName);
		}
		this.DestroyRocket();
	}

	[Obfuscation(Exclude=true)]
	private void DestroyRocket()
	{
		if (!this.isMulti || this.isMine)
		{
			base.CancelInvoke("KillRocket");
			RocketStack.sharedController.ReturnRocket(base.gameObject);
		}
		this.SetRocketDeactive();
	}

	private Transform FindLightningTarget()
	{
		RaycastHit raycastHit;
		Transform transforms = null;
		float single = Single.MaxValue;
		List<Transform> allTargets = this.GetAllTargets();
		for (int i = 0; i < allTargets.Count; i++)
		{
			Transform item = allTargets[i];
			if (!item.Equals(this.targetDamageLightning))
			{
				float single1 = Vector3.SqrMagnitude(this.myTransform.position - item.position);
				if (single1 < this.currentRocketSettings.raduisDetectTargetLightning * this.currentRocketSettings.raduisDetectTargetLightning && single1 < single && Physics.Raycast(this.myTransform.position, item.position - this.myTransform.position, out raycastHit, this.currentRocketSettings.raduisDetectTargetLightning, Player_move_c._ShootRaycastLayerMask) && raycastHit.collider.gameObject != null && raycastHit.collider.gameObject.transform.root.Equals(item))
				{
					transforms = item;
					single = single1;
				}
			}
		}
		return transforms;
	}

	private Transform FindNearestTarget(float searchAngle)
	{
		RaycastHit raycastHit;
		Transform transforms = null;
		float single = Single.MaxValue;
		List<Transform> allTargets = this.GetAllTargets();
		for (int i = 0; i < allTargets.Count; i++)
		{
			Transform item = allTargets[i];
			if (!item.Equals(this._targetDamage))
			{
				Vector3 vector3 = item.position - this.myTransform.position;
				if (Vector3.Angle(this.myTransform.forward, vector3.normalized) <= searchAngle)
				{
					float single1 = Vector3.SqrMagnitude(vector3);
					if (single1 < this.currentRocketSettings.raduisDetectTarget * this.currentRocketSettings.raduisDetectTarget && single1 < single && Physics.Raycast(this.myTransform.position, item.position - this.myTransform.position, out raycastHit, this.currentRocketSettings.raduisDetectTarget, Player_move_c._ShootRaycastLayerMask) && raycastHit.collider.gameObject != null && raycastHit.collider.gameObject.transform.root.Equals(item))
					{
						transforms = item;
						single = single1;
					}
				}
			}
		}
		return transforms;
	}

	private List<Transform> GetAllTargets()
	{
		List<Transform> transforms = new List<Transform>();
		if (!Defs.isMulti || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			for (int i = 0; i < Initializer.enemiesObj.Count; i++)
			{
				if (!Initializer.enemiesObj[i].GetComponent<BaseBot>().IsDeath)
				{
					transforms.Add(Initializer.enemiesObj[i].transform);
				}
			}
		}
		else
		{
			for (int j = 0; j < Initializer.players.Count; j++)
			{
				if (!Initializer.players[j].Equals(this._weaponManager.myPlayerMoveC))
				{
					if (this.IsEnemyTarget(Initializer.players[j].myPlayerTransform))
					{
						if (!Initializer.players[j].isKilled)
						{
							transforms.Add(Initializer.players[j].myPlayerTransform);
						}
					}
				}
			}
			for (int k = 0; k < Initializer.turretsObj.Count; k++)
			{
				TurretController component = Initializer.turretsObj[k].GetComponent<TurretController>();
				if (this.IsEnemyTarget(Initializer.turretsObj[k].transform))
				{
					if (!component.isKilled)
					{
						transforms.Add(component.transform);
					}
				}
			}
		}
		return transforms;
	}

	public void Hit(Collider hitCollider)
	{
		PlayerEventScoreController.ScoreEvent scoreEvent;
		string str;
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
		{
			if (this.isMulti && this.isMine)
			{
				if (this.isInet)
				{
					this.photonView.RPC("ShowExplosion", PhotonTargets.All, new object[] { this.weaponName });
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("ShowExplosion", RPCMode.All, new object[] { this.weaponName });
				}
			}
			else if (!this.isMulti)
			{
				this.ShowExplosion(this.weaponName);
			}
		}
		GameObject gameObject = null;
		if (hitCollider != null)
		{
			gameObject = hitCollider.gameObject;
		}
		Vector3 vector3 = base.transform.position;
		if (!this.isMulti || this.isMine)
		{
			if (this.isMulti && this._weaponManager.myPlayer == null)
			{
				return;
			}
			if (this.currentRocketSettings.typeDead == WeaponSounds.TypeDead.like)
			{
				Player_move_c component = null;
				if (!(hitCollider != null) || !(hitCollider.gameObject.transform.parent != null) || !hitCollider.gameObject.transform.parent.gameObject.CompareTag("Player"))
				{
					float single = 1E+09f;
					for (int i = 0; i < Initializer.players.Count; i++)
					{
						Player_move_c item = Initializer.players[i];
						if (!item.Equals(WeaponManager.sharedManager.myPlayerMoveC))
						{
							float single1 = Vector3.SqrMagnitude(Initializer.players[i].myPlayerTransform.position - vector3);
							if (single1 < this.radiusDamage * this.radiusDamage && single1 < single)
							{
								component = item;
							}
						}
					}
				}
				else
				{
					component = hitCollider.gameObject.transform.parent.gameObject.GetComponent<SkinName>().playerMoveC;
				}
				if (component != null)
				{
					WeaponManager.sharedManager.myPlayerMoveC.SendLike(component);
				}
				return;
			}
			for (int j = 0; j < Initializer.enemiesObj.Count; j++)
			{
				bool flag = false;
				bool flag1 = false;
				if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
				{
					flag = Initializer.enemiesObj[j].transform.Equals(this.targetDamageLightning);
				}
				else if (this.IsDamageByRadius())
				{
					if (this.IsHitInDamageRadius(Initializer.enemiesObj[j].transform.position, vector3, this.radiusDamage))
					{
						flag = true;
					}
				}
				else if (gameObject != null && gameObject.transform.parent && gameObject.transform.parent.gameObject.Equals(Initializer.enemiesObj[j]))
				{
					flag = true;
					flag1 = (hitCollider.GetType() != typeof(SphereCollider) ? false : true);
				}
				if (flag)
				{
					BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(Initializer.enemiesObj[j].transform);
					float single2 = 1f;
					if (flag1)
					{
						single2 = 2f + EffectsController.AddingForHeadshot(this._weaponManager.currentWeaponSounds.categoryNabor - 1);
					}
					float single3 = this.damage + UnityEngine.Random.Range(this.damageRange.x, this.damageRange.y);
					single3 *= single2;
					if (!this.isMulti)
					{
						if (this.isSlowdown)
						{
							botScriptForObject.ApplyDebuffByMode(BotDebuffType.DecreaserSpeed, this.slowdownTime, this.slowdownCoeff);
						}
						botScriptForObject.GetDamage(-single3, WeaponManager.sharedManager.myPlayer.transform, this.weaponPrefabName, true, flag1);
					}
					else if (!botScriptForObject.IsDeath)
					{
						if (this.isSlowdown)
						{
							botScriptForObject.ApplyDebuffByMode(BotDebuffType.DecreaserSpeed, this.slowdownTime, this.slowdownCoeff);
						}
						botScriptForObject.GetDamageForMultiplayer(-single3, null, this.weaponPrefabName, flag1);
						this._weaponManager.myNetworkStartTable.score = GlobalGameController.Score;
						this._weaponManager.myNetworkStartTable.SynhScore();
					}
				}
			}
			if (!Defs.isCOOP)
			{
				for (int k = 0; k < Initializer.turretsObj.Count; k++)
				{
					TurretController turretController = Initializer.turretsObj[k].GetComponent<TurretController>();
					bool flag2 = false;
					if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
					{
						flag2 = Initializer.turretsObj[k].transform.Equals(this.targetDamageLightning);
					}
					else if (this.IsDamageByRadius())
					{
						if (turretController.isEnemyTurret && this.IsHitInDamageRadius(Initializer.turretsObj[k].transform.position, vector3, this.radiusDamage))
						{
							flag2 = true;
						}
					}
					else if (gameObject != null && turretController.isEnemyTurret && gameObject.Equals(Initializer.turretsObj[k]))
					{
						flag2 = true;
					}
					if (flag2)
					{
						bool flag3 = (this.currentRocketSettings.typeDead != WeaponSounds.TypeDead.explosion ? false : true);
						float single4 = this.multiplayerDamage;
						this._weaponManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.damageTurret, single4);
						if (!Defs.isInet)
						{
							turretController.MinusLive(single4, flag3, 0, WeaponManager.sharedManager.myPlayer.GetComponent<NetworkView>().viewID);
						}
						else
						{
							turretController.MinusLive(single4, flag3, WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID, new NetworkViewID());
						}
					}
				}
			}
			for (int l = 0; l < Initializer.damagedObj.Count; l++)
			{
				bool flag4 = false;
				if (this.IsDamageByRadius())
				{
					if (this.IsHitInDamageRadius(Initializer.damagedObj[l].transform.position, vector3, this.radiusDamage))
					{
						flag4 = true;
					}
				}
				else if (gameObject != null && gameObject.transform.gameObject.Equals(Initializer.damagedObj[l]))
				{
					flag4 = true;
				}
				if (flag4)
				{
					float single5 = this.damage + UnityEngine.Random.Range(this.damageRange.x, this.damageRange.y);
					DamagedExplosionObject.TryApplyDamageToObject(Initializer.damagedObj[l], -single5);
				}
			}
			if (this.isMulti)
			{
				foreach (Player_move_c player in Initializer.players)
				{
					bool flag5 = false;
					flag5 = (this.isInet ? player.mySkinName.photonView.isMine : player.mySkinName.GetComponent<NetworkView>().isMine);
					if ((!this.isCOOP || !flag5) && (this.isCOOP || !flag5 && (this.isCompany || Defs.isFlag || Defs.isCapturePoints) && (!this.isCompany && !Defs.isFlag && !Defs.isCapturePoints || player.myCommand == this._weaponManager.myTable.GetComponent<NetworkStartTable>().myCommand)))
					{
						continue;
					}
					bool flag6 = false;
					bool flag7 = false;
					if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
					{
						flag6 = player.myPlayerTransform.Equals(this.targetDamageLightning);
					}
					else if (this.IsDamageByRadius())
					{
						if (this.IsHitInDamageRadius(player.myPlayerTransform.position, vector3, (!flag5 ? this.radiusDamage : this.radiusDamageSelf)))
						{
							flag6 = true;
						}
					}
					else if (gameObject != null && gameObject.transform.parent != null && gameObject.transform.parent.gameObject.Equals(player.mySkinName.gameObject))
					{
						flag6 = true;
						flag7 = gameObject.CompareTag("HeadCollider");
					}
					if (!flag6)
					{
						continue;
					}
					if (!Defs.isDaterRegim)
					{
						float single6 = 1f;
						if (flag7)
						{
							single6 = 2f + EffectsController.AddingForHeadshot(this._weaponManager.currentWeaponSounds.categoryNabor - 1);
						}
						if (!flag5)
						{
							int num = (int)this.currentRocketSettings.typeDead;
							Player_move_c.TypeKills typeKill = (!flag7 ? this.currentRocketSettings.typeKilsIconChat : Player_move_c.TypeKills.headshot);
							float single7 = this.multiplayerDamage * single6;
							PlayerScoreController playerScoreController = this._weaponManager.myPlayerMoveC.myScoreController;
							if (flag7)
							{
								scoreEvent = (!player.isMechActive ? PlayerEventScoreController.ScoreEvent.damageHead : PlayerEventScoreController.ScoreEvent.damageMechHead);
							}
							else if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Grenade)
							{
								scoreEvent = PlayerEventScoreController.ScoreEvent.damageGrenade;
							}
							else if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Rocket || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.GravityRocket || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Autoaim || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.AutoTarget)
							{
								scoreEvent = PlayerEventScoreController.ScoreEvent.damageExplosion;
							}
							else
							{
								scoreEvent = (!player.isMechActive ? PlayerEventScoreController.ScoreEvent.damageBody : PlayerEventScoreController.ScoreEvent.damageMechBody);
							}
							playerScoreController.AddScoreOnEvent(scoreEvent, single7);
							if (this.isSlowdown)
							{
								if (!this.isInet)
								{
									player.GetComponent<NetworkView>().RPC("SlowdownRPC", RPCMode.All, new object[] { this.slowdownCoeff, this.slowdownTime });
								}
								else
								{
									player.photonView.RPC("SlowdownRPC", PhotonTargets.All, new object[] { this.slowdownCoeff, this.slowdownTime });
								}
							}
							if (this.isInet)
							{
								player.MinusLive(this._weaponManager.myPlayer.GetComponent<PhotonView>().viewID, single7, typeKill, num, (this.rocketNum != 10 ? this.weaponPrefabName : string.Empty), 0);
							}
							else
							{
								Player_move_c playerMoveC = player;
								NetworkViewID networkViewID = this._weaponManager.myPlayer.GetComponent<NetworkView>().viewID;
								float single8 = single7;
								Player_move_c.TypeKills typeKill1 = typeKill;
								int num1 = num;
								str = (this.rocketNum != 10 ? this.weaponPrefabName : string.Empty);
								NetworkViewID networkViewID1 = new NetworkViewID();
								playerMoveC.MinusLive(networkViewID, single8, typeKill1, num1, str, networkViewID1);
							}
						}
						else
						{
							float selfExplosionDamageDecreaseCoef = this.multiplayerDamage * EffectsController.SelfExplosionDamageDecreaseCoef * single6;
							float single9 = selfExplosionDamageDecreaseCoef - player.curArmor;
							player.SendStartFlashMine();
							if (single9 >= 0f)
							{
								player.curArmor = 0f;
							}
							else
							{
								Player_move_c playerMoveC1 = player;
								playerMoveC1.curArmor = playerMoveC1.curArmor - selfExplosionDamageDecreaseCoef;
								single9 = 0f;
							}
							if (player.CurHealth > 0f)
							{
								Player_move_c curHealth = player;
								curHealth.CurHealth = curHealth.CurHealth - single9;
								if (player.CurHealth > 0f)
								{
									player.IndicateDamage();
								}
								else
								{
									player.isSuicided = true;
									player.sendImDeath(player.mySkinName.NickName);
									player.SendImKilled();
								}
							}
						}
					}
					else if (!flag5 && this._weaponManager.currentWeaponSounds.isDaterWeapon && !player.isMechActive)
					{
						player.SendDaterChat(WeaponManager.sharedManager.myPlayerMoveC.mySkinName.NickName, WeaponManager.sharedManager.currentWeaponSounds.daterMessage, player.mySkinName.NickName);
					}
				}
			}
		}
	}

	private bool IsDamageByRadius()
	{
		return (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Grenade || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Bomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Rocket || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.GravityRocket || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Autoaim || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Ball || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.AutoTarget ? true : this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyBomb);
	}

	private bool IsEnemyTarget(Transform _target)
	{
		if (_target == null)
		{
			return false;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC == null)
		{
			return false;
		}
		if (_target.GetComponent<SkinName>() != null)
		{
			if (_target.Equals(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform))
			{
				return false;
			}
			Player_move_c component = _target.GetComponent<SkinName>().playerMoveC;
			if (ConnectSceneNGUIController.isTeamRegim && component.myCommand == WeaponManager.sharedManager.myPlayerMoveC.myCommand)
			{
				return false;
			}
			return true;
		}
		if (_target.GetComponent<TurretController>() == null)
		{
			return true;
		}
		TurretController turretController = _target.GetComponent<TurretController>();
		if (!turretController.isEnemyTurret)
		{
			return false;
		}
		if (ConnectSceneNGUIController.isTeamRegim && turretController.myCommand == WeaponManager.sharedManager.myPlayerMoveC.myCommand)
		{
			return false;
		}
		return true;
	}

	private bool IsGrenadeRocketNum(int _rocketNum)
	{
		return (_rocketNum == 10 ? true : _rocketNum == 40);
	}

	private bool IsHitInDamageRadius(Vector3 targetPos, Vector3 selfPos, float radius)
	{
		return (targetPos - selfPos).sqrMagnitude < radius * radius;
	}

	private bool IsKilledTarget(Transform _target)
	{
		if (_target == null)
		{
			return true;
		}
		if (_target.GetComponent<SkinName>() != null)
		{
			return _target.GetComponent<SkinName>().playerMoveC.isKilled;
		}
		if (_target.GetComponent<TurretController>() != null)
		{
			return _target.GetComponent<TurretController>().isKilled;
		}
		if (_target.GetComponent<BaseBot>() == null)
		{
			return true;
		}
		return _target.GetComponent<BaseBot>().IsDeath;
	}

	[Obfuscation(Exclude=true)]
	private void KillRocket()
	{
		this.KillRocket(null);
	}

	public void KillRocket(Collider _hitCollision)
	{
		if (this.isKilled)
		{
			return;
		}
		this.Hit(_hitCollision);
		this.isKilled = true;
		if (!this.isMulti)
		{
			this.Collide(this.weaponName, this.myTransform.position);
		}
		else if (!this.isInet)
		{
			base.GetComponent<NetworkView>().RPC("Collide", RPCMode.All, new object[] { this.weaponName, this.myTransform.position });
		}
		else if (this.photonView == null)
		{
			UnityEngine.Debug.Log("Rocket.KillRocket():    photonView == null");
		}
		else
		{
			this.photonView.RPC("Collide", PhotonTargets.All, new object[] { this.weaponName, this.myTransform.position });
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!this.isRun || this.rocketNum == 10)
		{
			return;
		}
		if (this.isMulti && !this.isMine)
		{
			return;
		}
		if (other.gameObject.CompareTag("CapturePoint"))
		{
			return;
		}
		if (!this.isMulti && (other.gameObject.tag.Equals("Player") || other.transform.parent != null && other.transform.parent.gameObject.tag.Equals("Player")))
		{
			return;
		}
		if (this.isMulti && (other.gameObject.tag.Equals("Player") && other.gameObject == this._weaponManager.myPlayer || other.transform.parent != null && other.transform.parent.gameObject.tag.Equals("Player") && other.transform.parent.gameObject == this._weaponManager.myPlayer))
		{
			return;
		}
		if (other.gameObject.name.Equals("DamageCollider"))
		{
			return;
		}
		if ((this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Bomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Ball) && (other.gameObject.transform.parent == null && !other.gameObject.transform.gameObject.CompareTag("Turret") || other.gameObject.transform.parent != null && !other.gameObject.transform.parent.gameObject.CompareTag("Player") && !other.gameObject.transform.parent.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("DamagedExplosion")))
		{
			return;
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyBomb)
		{
			if (!this.myRigidbody.isKinematic)
			{
				base.transform.position = other.contacts[0].point + (other.contacts[0].normal * 0.035f);
				this.myRigidbody.isKinematic = true;
				this.stickedObject = other.transform;
				this.stickedObjectPos = this.stickedObject.position;
			}
			return;
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Ghost || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.MegaBullet && (!(other.gameObject.transform.parent != null) || !other.gameObject.transform.parent.gameObject.CompareTag("Untagged")) && (!(other.gameObject.transform.parent == null) || !other.gameObject.CompareTag("Untagged")))
		{
			this.Hit(other.collider);
		}
		else
		{
			this.KillRocket(other.collider);
		}
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		this.isStartSynh = true;
		if (!stream.isWriting)
		{
			this.correctPos = (Vector3)stream.ReceiveNext();
			this.myTransform.rotation = (Quaternion)stream.ReceiveNext();
		}
		else
		{
			stream.SendNext(this.myTransform.position);
			stream.SendNext(this.myTransform.rotation);
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		this.isStartSynh = true;
		if (!stream.isWriting)
		{
			Vector3 vector3 = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			stream.Serialize(ref vector3);
			stream.Serialize(ref quaternion);
			this.correctPos = vector3;
			this.myTransform.rotation = quaternion;
			if (this.isFirstPos)
			{
				this.isFirstPos = false;
				this.myTransform.position = vector3;
				this.myTransform.rotation = quaternion;
			}
		}
		else
		{
			Vector3 vector31 = this.myTransform.position;
			Quaternion quaternion1 = this.myTransform.rotation;
			stream.Serialize(ref vector31);
			stream.Serialize(ref quaternion1);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!this.isRun || this.rocketNum == 10)
		{
			return;
		}
		if (this.isMulti && !this.isMine)
		{
			return;
		}
		if (other.gameObject.name.Equals("DamageCollider"))
		{
			return;
		}
		if (other.gameObject.CompareTag("CapturePoint"))
		{
			return;
		}
		if (other.gameObject.CompareTag("Area"))
		{
			return;
		}
		if (!this.isMulti && (other.gameObject.tag.Equals("Player") || other.transform.parent != null && other.transform.parent.gameObject.tag.Equals("Player")))
		{
			return;
		}
		if (this.isMulti && (other.gameObject.tag.Equals("Player") && other.gameObject == this._weaponManager.myPlayer || other.transform.parent != null && other.transform.parent.gameObject.tag.Equals("Player") && other.transform.parent.gameObject == this._weaponManager.myPlayer))
		{
			return;
		}
		if ((this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Bomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Ball) && (other.gameObject.transform.parent == null && !other.gameObject.transform.gameObject.CompareTag("Turret") || other.gameObject.transform.parent != null && !other.gameObject.transform.parent.gameObject.CompareTag("Player") && !other.gameObject.transform.parent.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("DamagedExplosion")))
		{
			return;
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyBomb)
		{
			return;
		}
		if (this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.Ghost && (this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.MegaBullet && this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.Lightning || other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject.CompareTag("Untagged") || other.gameObject.transform.parent == null && other.gameObject.CompareTag("Untagged")))
		{
			this.KillRocket(other);
		}
		else if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
		{
			if (this.IsEnemyTarget(other.transform.root))
			{
				this.targetDamageLightning = other.transform.root;
				this.timerFromJumpLightning = this.maxTimerFromJumpLightning;
				this.counterJumpLightning++;
				if (this.counterJumpLightning <= this.currentRocketSettings.countJumpLightning)
				{
					this.Hit(null);
				}
				else
				{
					this.KillRocket();
				}
			}
		}
		else if (this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.MegaBullet)
		{
			this.Hit(other);
		}
	}

	public void SendNetworkViewMyPlayer(NetworkViewID myId)
	{
		base.GetComponent<NetworkView>().RPC("SendNetworkViewMyPlayerRPC", RPCMode.AllBuffered, new object[] { myId });
	}

	[PunRPC]
	[RPC]
	public void SendNetworkViewMyPlayerRPC(NetworkViewID myId)
	{
		int num = 0;
		while (num < Initializer.players.Count)
		{
			if (!myId.Equals(Initializer.players[num].mySkinName.GetComponent<NetworkView>().viewID))
			{
				num++;
			}
			else
			{
				this.myPlayerMoveC = Initializer.players[num];
				break;
			}
		}
	}

	public void SendSetRocketActiveRPC()
	{
		if (this.isMulti && this.isMine)
		{
			if (this.isInet)
			{
				this.photonView.RPC("SetRocketActive", PhotonTargets.All, new object[] { this.weaponPrefabName, this.radiusImpulse, base.transform.position });
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SetRocketActive", RPCMode.All, new object[] { this.weaponPrefabName, this.radiusImpulse, base.transform.position });
			}
		}
		else if (!this.isMulti)
		{
			this.SetRocketActive(this.weaponPrefabName, this.radiusImpulse, base.transform.position);
		}
	}

	[PunRPC]
	[RPC]
	public void SetRocketActive(string weapon, float _radiusImpulse, Vector3 pos)
	{
		if (weapon == "WeaponGrenade")
		{
			this.weaponName = "WeaponGrenade";
			this.impulseForce = 50f;
			this.impulseForceSelf = 133.4f;
			this.SetRocketActive(10, _radiusImpulse, pos);
			return;
		}
		if (weapon == "WeaponLike")
		{
			this.impulseForce = 50f;
			this.impulseForceSelf = 133.4f;
			this.weaponName = "WeaponLike";
			this.SetRocketActive(40, _radiusImpulse, pos);
			return;
		}
		WeaponSounds component = (Resources.Load(string.Concat("Weapons/", weapon)) as GameObject).GetComponent<WeaponSounds>();
		this.weaponName = component.bazookaExplosionName;
		this.impulseForce = component.impulseForce;
		this.impulseForceSelf = component.impulseForceSelf;
		this.SetRocketActive(component.rocketNum, _radiusImpulse, pos);
	}

	[PunRPC]
	[RPC]
	public void SetRocketActive(int rn, float _radiusImpulse, Vector3 pos)
	{
		if (!this.isMine)
		{
			base.transform.position = pos;
		}
		this.rocketNum = rn;
		this.radiusImpulse = _radiusImpulse;
		if ((int)this.rockets.Length == 0)
		{
			return;
		}
		if (rn >= (int)this.rockets.Length)
		{
			rn = 0;
		}
		this.rockets[rn].SetActive(true);
		this.timerFromJumpLightning = this.maxTimerFromJumpLightning;
		this.counterJumpLightning = 0;
		this.isDetectFirstTargetLightning = false;
		this.targetDamageLightning = null;
		this._targetDamage = null;
		this.targetLightning = null;
		this.targetsDamageLightningList.Clear();
		base.GetComponent<BoxCollider>().size = this.rockets[rn].GetComponent<RocketSettings>().sizeBoxCollider;
		base.GetComponent<BoxCollider>().center = this.rockets[rn].transform.localPosition + new Vector3();
		base.StartCoroutine(this.StartRocketCoroutine());
	}

	[PunRPC]
	[RPC]
	public void SetRocketDeactive()
	{
		this.isRun = false;
		this.isStartSynh = false;
		this.dontExecStart = false;
		int num = this.rocketNum;
		if (num >= (int)this.rockets.Length)
		{
			num = 0;
		}
		this.rockets[num].SetActive(false);
		if (this.isMulti && this.isInet && this.photonView != null)
		{
			this.photonView.synchronization = ViewSynchronization.Off;
		}
		if (this.isMulti)
		{
			this.isInet;
		}
		base.transform.position = Vector3.down * 10000f;
		if (this.currentRocketSettings != null && this.currentRocketSettings.trail != null)
		{
			this.currentRocketSettings.trail.enabled = false;
		}
	}

	[PunRPC]
	[RPC]
	private void ShowExplosion(string explosionName)
	{
		Vector3 vector3 = base.transform.position;
		string str = ResPath.Combine("Explosions", explosionName);
		GameObject objectFromName = RayAndExplosionsStackController.sharedController.GetObjectFromName(str);
		if (objectFromName != null)
		{
			objectFromName.transform.position = vector3;
		}
	}

	private void Start()
	{
		if (!this.isMulti || this.isMine)
		{
			this.myPlayerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		else
		{
			this.myRigidbody.isKinematic = true;
		}
	}

	public void StartRocket()
	{
		if (this.isMulti && this.isMine)
		{
			if (this.isInet)
			{
				this.photonView.RPC("StartRocketRPC", PhotonTargets.All, new object[0]);
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("StartRocketRPC", RPCMode.All, new object[0]);
			}
		}
		else if (!this.isMulti)
		{
			this.StartRocketRPC();
		}
		this.myRigidbody.isKinematic = false;
	}

	[DebuggerHidden]
	private IEnumerator StartRocketCoroutine()
	{
		Rocket.u003cStartRocketCoroutineu003ec__Iterator103 variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	public void StartRocketRPC()
	{
		if (this.isMulti && this.isInet && this.photonView != null)
		{
			this.photonView.synchronization = ViewSynchronization.UnreliableOnChange;
		}
		if (this.isMulti && !this.isInet)
		{
			base.GetComponent<NetworkView>().stateSynchronization = NetworkStateSynchronization.Unreliable;
		}
		if (this.rocketNum == 10 && this.currentRocketSettings != null && this.currentRocketSettings.trail != null)
		{
			this.currentRocketSettings.trail.enabled = true;
		}
		base.transform.parent = null;
		Player_move_c.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Rocket"));
		this.isRun = true;
		if (!this.isMulti || this.isMine)
		{
			base.Invoke("KillRocket", (!Defs.isDaterRegim || this.rocketNum != 36 && this.rocketNum != 18 ? this.currentRocketSettings.lifeTime : 1f));
		}
	}

	private void Update()
	{
		if (!this.isMulti || this.isMine)
		{
			switch (this.currentRocketSettings.typeFly)
			{
				case RocketSettings.TypeFlyRocket.Autoaim:
				case RocketSettings.TypeFlyRocket.AutoaimBullet:
				{
					if (this.isRun && WeaponManager.sharedManager.myPlayerMoveC != null && !WeaponManager.sharedManager.myPlayerMoveC.isKilled)
					{
						Vector3 pointAutoAim = WeaponManager.sharedManager.myPlayerMoveC.GetPointAutoAim(this.myTransform.position);
						Vector3 vector3 = (pointAutoAim - this.myTransform.position).normalized;
						this.myRigidbody.AddForce(vector3 * 27f);
						Rigidbody rigidbody = this.myRigidbody;
						Vector3 vector31 = this.myRigidbody.velocity;
						rigidbody.velocity = vector31.normalized * this.currentRocketSettings.autoRocketForce;
						this.myTransform.rotation = Quaternion.LookRotation(this.myRigidbody.velocity);
					}
					break;
				}
				case RocketSettings.TypeFlyRocket.Lightning:
				{
					if (this.targetDamageLightning != null)
					{
						this.myRigidbody.isKinematic = true;
						this.targetLightning = this.FindLightningTarget();
						if (this.targetLightning != null)
						{
							this.targetDamageLightning = null;
							this.progressCaptureTargetLightning = 0f;
						}
						else
						{
							this.myTransform.position = this.targetDamageLightning.position;
							this.timerFromJumpLightning -= Time.deltaTime;
							if (this.timerFromJumpLightning <= 0f)
							{
								this.counterJumpLightning++;
								if (this.counterJumpLightning <= this.currentRocketSettings.countJumpLightning)
								{
									this.Hit(null);
								}
								else
								{
									this.KillRocket();
								}
								this.timerFromJumpLightning = this.maxTimerFromJumpLightning;
							}
						}
					}
					if (this.targetLightning != null)
					{
						if (this.IsKilledTarget(this.targetLightning))
						{
							this.KillRocket();
						}
						else
						{
							this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.targetLightning.position, this.progressCaptureTargetLightning + 5f * Time.deltaTime);
						}
					}
					else if (this.isDetectFirstTargetLightning && (this.targetDamageLightning == null || this.IsKilledTarget(this.targetDamageLightning)))
					{
						this.KillRocket();
					}
					break;
				}
				case RocketSettings.TypeFlyRocket.AutoTarget:
				{
					if (this.isRun)
					{
						if (this._targetDamage == null || this.IsKilledTarget(this._targetDamage) || (this._targetDamage.position - this.myTransform.position).sqrMagnitude > (this.currentRocketSettings.raduisDetectTarget + 1f) * (this.currentRocketSettings.raduisDetectTarget + 1f))
						{
							this._targetDamage = this.FindNearestTarget(45f);
						}
						if (this._targetDamage != null)
						{
							Vector3 component = Vector3.zero;
							if (this._targetDamage.childCount > 0 && this._targetDamage.GetChild(0).GetComponent<BoxCollider>() != null)
							{
								component = this._targetDamage.GetChild(0).GetComponent<BoxCollider>().center;
							}
							Vector3 vector32 = (this._targetDamage.position + component) - this.myTransform.position;
							Vector3 vector33 = vector32.normalized;
							this.myRigidbody.AddForce(vector33 * 9f);
							Rigidbody rigidbody1 = this.myRigidbody;
							Vector3 vector34 = this.myRigidbody.velocity;
							rigidbody1.velocity = vector34.normalized * this.currentRocketSettings.autoRocketForce;
							this.myTransform.rotation = Quaternion.LookRotation(this.myRigidbody.velocity);
						}
					}
					break;
				}
				case RocketSettings.TypeFlyRocket.StickyBomb:
				{
					if (!this.isRun || !this.myRigidbody.isKinematic)
					{
						return;
					}
					if (this.stickedObject != null && this.stickedObjectPos != this.stickedObject.position)
					{
						this.KillRocket();
						return;
					}
					foreach (Transform allTarget in this.GetAllTargets())
					{
						if ((allTarget.position - this.myTransform.position).sqrMagnitude >= this.currentRocketSettings.raduisDetectTarget * this.currentRocketSettings.raduisDetectTarget)
						{
							continue;
						}
						this.KillRocket();
					}
					break;
				}
			}
		}
		if (!Defs.isMulti || !this.isStartSynh)
		{
			return;
		}
		if (Defs.isInet && this.photonView != null && !this.photonView.isMine || !Defs.isInet && !base.GetComponent<NetworkView>().isMine)
		{
			if (Defs.isInet || Vector3.SqrMagnitude(this.myTransform.position - this.correctPos) <= 300f)
			{
				this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.correctPos, Time.deltaTime * 5f);
			}
			else
			{
				this.myTransform.position = this.correctPos;
			}
		}
	}
}