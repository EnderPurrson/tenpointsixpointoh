using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RilisoftBot
{
	public class BotAiController : MonoBehaviour
	{
		private const int MaxAttempTeleportation = 5;

		private const float TimeDelayedTeleport = 0.2f;

		private const float TimeOutUpdateMultiplayerTargets = 3f;

		private const float TimeOutUpdateLocalTargets = 1f;

		private BaseBot _botController;

		private BotAiController.TypeBot _typeBot;

		private BotAiController.AiState _currentState;

		private bool _isMultiplayerCoopMode;

		private PhotonView _photonView;

		private bool _isDeaded;

		[Header("Patrol module settings")]
		public float minLenghtMove = 9f;

		private bool _isCanMove;

		private float _lastTimeMoving;

		private Vector3 _targetPoint;

		private NavMeshAgent _naveMeshAgent;

		[Header("Movement module settings")]
		public bool isStationary;

		public bool isTeleportationMove;

		public static bool deathAudioPlaying;

		private float _timeToTakeDamage;

		private bool _isFalling;

		private BoxCollider _modelCollider;

		private float _timeToCheckAvailabelShot;

		private bool _isTargetAvalabelShot;

		[Header("Teleport movement setting")]
		public float timeToNextTeleport = 2f;

		public float[] DeltaTeleportAttackDistance = new float[] { 1f, 2f };

		public GameObject effectTeleport;

		public float angleByPlayerLook = 30f;

		public AudioClip teleportStart;

		public AudioClip teleportEnd;

		private float _timeLastTeleport;

		private GameObject _effectObject;

		private bool _isWaiting;

		[NonSerialized]
		public bool isDetectPlayer = true;

		private bool _isEntered;

		private float _timeToUpdateMultiplayerTargets = 3f;

		private float _timeToUpdateLocalTargets = 1f;

		private bool _isTargetCaptureForce;

		public Transform currentTarget
		{
			get;
			private set;
		}

		public bool IsCanMove
		{
			get
			{
				return this._isCanMove;
			}
			set
			{
				if (this._isCanMove == value)
				{
					return;
				}
				this._isCanMove = value;
				if (this._isCanMove)
				{
					this._lastTimeMoving = -1f;
					this._botController.PlayAnimZombieWalkByMode();
				}
			}
		}

		private bool IsWaitingState
		{
			get
			{
				return this._isWaiting;
			}
			set
			{
				if (this._isWaiting == value)
				{
					return;
				}
				this._isWaiting = value;
				if (this._isWaiting)
				{
					this._botController.PlayAnimationIdle();
				}
			}
		}

		public BotAiController()
		{
		}

		private BotAiController.AiState CheckActiveAttackState()
		{
			if (this._botController.IsDeath || this.currentTarget == null)
			{
				return BotAiController.AiState.None;
			}
			if (this.CheckMoveFromTeleport())
			{
				return BotAiController.AiState.Teleportation;
			}
			float single = Vector3.SqrMagnitude(this.currentTarget.position - (base.transform.position + Vector3.up));
			if (!this._botController.CheckEnemyInAttackZone(single))
			{
				return (!this.isStationary ? BotAiController.AiState.MoveToTarget : BotAiController.AiState.Waiting);
			}
			if (this._typeBot != BotAiController.TypeBot.Shooting && this._typeBot != BotAiController.TypeBot.ShootAndMelee)
			{
				return BotAiController.AiState.Damage;
			}
			this.CheckTargetAvailabelForShot();
			BotAiController.AiState aiState = (!this.isStationary ? BotAiController.AiState.MoveToTarget : BotAiController.AiState.Waiting);
			if (this._isTargetAvalabelShot)
			{
				return BotAiController.AiState.Damage;
			}
			return aiState;
		}

		private bool CheckApplyMultiplayerLogic()
		{
			if (!this._isMultiplayerCoopMode)
			{
				return false;
			}
			if (ZombiManager.sharedManager == null)
			{
				return true;
			}
			if (!ZombiManager.sharedManager.startGame)
			{
				if (PhotonNetwork.isMasterClient)
				{
					this._botController.DestroyByNetworkType();
				}
				return true;
			}
			if (!this._photonView.isMine)
			{
				return true;
			}
			return false;
		}

		private bool CheckForcedTarget()
		{
			if (!this._isTargetCaptureForce)
			{
				return false;
			}
			if (this.IsCurrentTargetLost())
			{
				this.SetTargetToMove(null);
				this._isTargetCaptureForce = false;
			}
			return true;
		}

		private bool CheckMoveFromTeleport()
		{
			if (!this.isTeleportationMove)
			{
				return false;
			}
			if (this._timeLastTeleport <= 0f)
			{
				this._timeLastTeleport = this.timeToNextTeleport;
				return true;
			}
			this._timeLastTeleport -= Time.deltaTime;
			return false;
		}

		private void CheckTargetAvailabelForShot()
		{
			this._timeToCheckAvailabelShot -= Time.deltaTime;
			if (this._timeToCheckAvailabelShot > 0f)
			{
				return;
			}
			this._timeToCheckAvailabelShot = 1f;
			this._isTargetAvalabelShot = this.IsTargetAvailabelForShot();
		}

		private void CheckTargetForLocalMode()
		{
			if (this.CheckForcedTarget())
			{
				return;
			}
			if (!this._isEntered)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Turret");
				float distanceToTurret = this.GetDistanceToTurret(gameObject);
				bool flag = (distanceToTurret != -1f ? this._botController.detectRadius >= distanceToTurret : false);
				GameObject gameObject1 = GameObject.FindGameObjectWithTag("Player");
				float distanceToPlayer = this.GetDistanceToPlayer(gameObject1);
				bool flag1 = (distanceToPlayer != -1f ? this._botController.detectRadius >= distanceToPlayer : false);
				Transform transforms = null;
				if (flag1 && flag)
				{
					transforms = (distanceToPlayer >= distanceToTurret ? gameObject.transform : gameObject1.transform);
				}
				else if (flag1)
				{
					transforms = gameObject1.transform;
				}
				else if (flag)
				{
					transforms = gameObject.transform;
				}
				if (transforms != null)
				{
					this.SetTargetToMove(transforms);
					this._isEntered = true;
				}
			}
			else if (this.IsCurrentTargetLost())
			{
				this.SetTargetToMove(null);
				this._isEntered = false;
			}
		}

		private void CheckTargetForMultiplayerMode()
		{
			bool flag;
			if (this.CheckForcedTarget())
			{
				return;
			}
			float single = -1f;
			GameObject nearestTargetForMultiplayer = this.GetNearestTargetForMultiplayer(out flag);
			if (nearestTargetForMultiplayer == null)
			{
				this.SetTargetToMove(null);
				return;
			}
			single = (!flag ? this.GetDistanceToTurret(nearestTargetForMultiplayer) : this.GetDistanceToPlayer(nearestTargetForMultiplayer));
			if (single == -1f || this._botController.detectRadius < single || this.IsTargetLost(nearestTargetForMultiplayer.transform))
			{
				this.SetTargetToMove(null);
			}
			else
			{
				this.SetTargetToMove(nearestTargetForMultiplayer.transform);
			}
		}

		private BotAiController.TypeBot GetCurrentTypeBot()
		{
			if (this._botController == null)
			{
				return BotAiController.TypeBot.None;
			}
			if (this._botController is MeleeBot || this._botController is MeleeBossBot)
			{
				return BotAiController.TypeBot.Melee;
			}
			if (this._botController is MeleeShootBot)
			{
				return BotAiController.TypeBot.ShootAndMelee;
			}
			return BotAiController.TypeBot.Shooting;
		}

		private float GetDistanceToPlayer(GameObject playerObj)
		{
			if (playerObj == null)
			{
				return -1f;
			}
			if (this.IsTargetNotAvailabel(playerObj.transform, BotAiController.TargetType.Player))
			{
				return -1f;
			}
			return Vector3.Distance(base.transform.position, playerObj.transform.position);
		}

		private float GetDistanceToTurret(GameObject turretObj)
		{
			if (turretObj == null)
			{
				return -1f;
			}
			if (this.IsTargetNotAvailabel(turretObj.transform, BotAiController.TargetType.Turret))
			{
				return -1f;
			}
			return Vector3.Distance(base.transform.position, turretObj.transform.position);
		}

		private GameObject GetNearestTargetForMultiplayer(out bool isTargetPlayer)
		{
			isTargetPlayer = false;
			GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Turret");
			GameObject item = null;
			if (Initializer.players.Count > 0)
			{
				float single = Single.MaxValue;
				for (int i = 0; i < Initializer.players.Count; i++)
				{
					if (!this.IsTargetNotAvailabel(Initializer.players[i]))
					{
						float single1 = Vector3.SqrMagnitude(base.transform.position - Initializer.players[i].myPlayerTransform.position);
						if (single1 < single)
						{
							single = single1;
							item = Initializer.players[i].mySkinName.gameObject;
							isTargetPlayer = true;
						}
					}
				}
				for (int j = 0; j < (int)gameObjectArray.Length; j++)
				{
					if (!this.IsTargetNotAvailabel(gameObjectArray[j].transform, BotAiController.TargetType.Turret))
					{
						float single2 = Vector3.SqrMagnitude(base.transform.position - gameObjectArray[j].transform.position);
						if (single2 < single)
						{
							single = single2;
							item = gameObjectArray[j];
							isTargetPlayer = false;
						}
					}
				}
			}
			return item;
		}

		private Vector3 GetPositionFromTeleport()
		{
			Vector3 vector3 = Vector3.zero;
			float deltaTeleportAttackDistance = this._botController.attackDistance + this.DeltaTeleportAttackDistance[0];
			float single = this._botController.attackDistance + this.DeltaTeleportAttackDistance[1];
			float single1 = UnityEngine.Random.Range(deltaTeleportAttackDistance, single);
			float single2 = UnityEngine.Random.@value;
			if (single2 >= 0f && single2 < 0.4f)
			{
				Quaternion quaternion = Quaternion.Euler(0f, this.angleByPlayerLook, 0f);
				vector3 = this.currentTarget.position + (quaternion * this.currentTarget.forward * single1);
			}
			else if (single2 >= 0.4f && single2 < 0.5f)
			{
				Quaternion quaternion1 = Quaternion.Euler(0f, this.angleByPlayerLook, 0f);
				Vector3 vector31 = this.currentTarget.forward;
				vector31.z = -vector31.z;
				vector3 = this.currentTarget.position + (quaternion1 * vector31 * single1);
			}
			else if (single2 < 0.5f || single2 >= 0.6f)
			{
				Quaternion quaternion2 = Quaternion.Euler(0f, -this.angleByPlayerLook, 0f);
				vector3 = this.currentTarget.position + (quaternion2 * this.currentTarget.forward * single1);
			}
			else
			{
				Vector3 vector32 = this.currentTarget.forward;
				vector32.z = -vector32.z;
				Quaternion quaternion3 = Quaternion.Euler(0f, -this.angleByPlayerLook, 0f);
				vector3 = this.currentTarget.position + (quaternion3 * vector32 * single1);
			}
			return vector3;
		}

		private string GetTargetTagAndPointToShot(out Vector3 pointToShot)
		{
			BotAiController.TargetType targetType = this.GetTargetType(this.currentTarget);
			if (targetType != BotAiController.TargetType.Player)
			{
				if (targetType == BotAiController.TargetType.Turret)
				{
					pointToShot = this.currentTarget.GetComponent<TurretController>().GetHeadPoint();
					return "Turret";
				}
				if (targetType != BotAiController.TargetType.Bot)
				{
					pointToShot = Vector3.zero;
					return null;
				}
				pointToShot = this.currentTarget.GetComponent<BaseBot>().GetHeadPoint();
				return "Enemy";
			}
			SkinName component = this.currentTarget.GetComponent<SkinName>();
			if (component == null)
			{
				pointToShot = Vector3.zero;
				return null;
			}
			pointToShot = component.headObj.transform.position;
			return "Player";
		}

		private BotAiController.TargetType GetTargetType(Transform target)
		{
			if (target.CompareTag("Player"))
			{
				return BotAiController.TargetType.Player;
			}
			if (target.CompareTag("Turret"))
			{
				return BotAiController.TargetType.Turret;
			}
			if (this.currentTarget.CompareTag("Enemy"))
			{
				return BotAiController.TargetType.Bot;
			}
			return BotAiController.TargetType.None;
		}

		private float GetTimeToTakeDamageMeleeBot()
		{
			if (this._typeBot != BotAiController.TypeBot.Melee)
			{
				return 0f;
			}
			return (this._botController as MeleeBot).CheckTimeToTakeDamage();
		}

		private void InitializePatrolModule()
		{
			this._lastTimeMoving = -1f;
			if (!this._isMultiplayerCoopMode || this._photonView.isMine)
			{
				this.IsCanMove = !this.isStationary;
			}
			else
			{
				this._botController.PlayAnimZombieWalkByMode();
				this.IsCanMove = false;
			}
		}

		private void InitTeleportData()
		{
			if (!this.isTeleportationMove)
			{
				return;
			}
			this._effectObject = UnityEngine.Object.Instantiate<GameObject>(this.effectTeleport);
			this._effectObject.transform.parent = base.transform;
			this._effectObject.transform.localPosition = Vector3.zero;
			this._effectObject.transform.rotation = Quaternion.identity;
			this._effectObject.SetActive(false);
		}

		private bool IsCurrentTargetLost()
		{
			return this.IsTargetLost(this.currentTarget);
		}

		private bool IsTargetAvailabelForShot()
		{
			Vector3 vector3;
			RaycastHit raycastHit;
			Vector3 headPoint = this._botController.GetHeadPoint();
			string targetTagAndPointToShot = this.GetTargetTagAndPointToShot(out vector3);
			float maxAttackDistance = this._botController.GetMaxAttackDistance();
			if (!Physics.Raycast(headPoint, vector3 - headPoint, out raycastHit, maxAttackDistance, Tools.AllAvailabelBotRaycastMask))
			{
				return false;
			}
			return raycastHit.collider.transform.root.CompareTag(targetTagAndPointToShot);
		}

		private bool IsTargetLost(Transform target)
		{
			if (target == null)
			{
				return true;
			}
			if (this._isTargetCaptureForce && target.gameObject == null)
			{
				return true;
			}
			BotAiController.TargetType targetType = this.GetTargetType(target);
			if (targetType == BotAiController.TargetType.Player)
			{
				if (this.IsTargetNotAvailabel(target, BotAiController.TargetType.Player))
				{
					return true;
				}
				if (!this._isTargetCaptureForce && Vector3.SqrMagnitude(base.transform.position - target.transform.position) > this._botController.GetSquareDetectRadius())
				{
					return true;
				}
			}
			if (targetType == BotAiController.TargetType.Turret && this.IsTargetNotAvailabel(target, BotAiController.TargetType.Turret))
			{
				return true;
			}
			return false;
		}

		private bool IsTargetNotAvailabel(Transform target, BotAiController.TargetType targetType)
		{
			if (targetType == BotAiController.TargetType.Player)
			{
				SkinName component = target.GetComponent<SkinName>();
				if (component != null && component.playerMoveC.isInvisible)
				{
					return true;
				}
			}
			if (targetType == BotAiController.TargetType.Turret)
			{
				TurretController turretController = target.GetComponent<TurretController>();
				if (turretController != null && (turretController.isKilled || !turretController.isRun))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsTargetNotAvailabel(Player_move_c target)
		{
			if (target != null && target.isInvisible)
			{
				return true;
			}
			return false;
		}

		private void ResetNavigationPathIfNeed()
		{
			if (this._naveMeshAgent.path == null)
			{
				return;
			}
			if (this._naveMeshAgent.isOnOffMeshLink)
			{
				return;
			}
			this._naveMeshAgent.ResetPath();
		}

		public void SetTargetForced(Transform target)
		{
			this.SetTargetToMove(target);
			this._isTargetCaptureForce = true;
		}

		public void SetTargetToMove(Transform target)
		{
			if (target != null && this.currentTarget != target)
			{
				this.ResetNavigationPathIfNeed();
				this._botController.PlayVoiceSound();
				this._botController.PlayAnimZombieWalkByMode();
			}
			else if (target == null && this.currentTarget != target)
			{
				this.ResetNavigationPathIfNeed();
				this._botController.PlayAnimationWalk();
			}
			this.currentTarget = target;
			this.IsCanMove = (target != null ? false : !this.isStationary);
		}

		[DebuggerHidden]
		private IEnumerator ShowEffectTeleport(float seconds)
		{
			BotAiController.u003cShowEffectTeleportu003ec__Iterator115 variable = null;
			return variable;
		}

		private void Start()
		{
			this._photonView = base.GetComponent<PhotonView>();
			this._isMultiplayerCoopMode = (!Defs.isCOOP ? false : this._photonView != null);
			this._currentState = BotAiController.AiState.None;
			this._botController = base.GetComponent<BaseBot>();
			this._typeBot = this.GetCurrentTypeBot();
			this._naveMeshAgent = base.GetComponent<NavMeshAgent>();
			this._modelCollider = base.GetComponentInChildren<BoxCollider>();
			this.InitializePatrolModule();
			if (this._typeBot == BotAiController.TypeBot.Melee)
			{
				this._timeToTakeDamage = this.GetTimeToTakeDamageMeleeBot();
			}
			this._timeLastTeleport = this.timeToNextTeleport;
			this.InitTeleportData();
			this._naveMeshAgent.Warp(base.transform.position + (this._naveMeshAgent.baseOffset * Vector3.up));
		}

		[DebuggerHidden]
		private IEnumerator TeleportFromRandomPoint()
		{
			BotAiController.u003cTeleportFromRandomPointu003ec__Iterator116 variable = null;
			return variable;
		}

		private void Update()
		{
			if (Defs.isMulti && this._naveMeshAgent.enabled != PhotonNetwork.isMasterClient)
			{
				this._naveMeshAgent.enabled = PhotonNetwork.isMasterClient;
			}
			if (this.CheckApplyMultiplayerLogic())
			{
				return;
			}
			this.UpdateTargetsForBot();
			this.UpdateCurrentAiState();
			if (this._currentState == BotAiController.AiState.Patrol)
			{
				this.UpdatePatrolState();
			}
			else if (this._currentState == BotAiController.AiState.MoveToTarget)
			{
				this.UpdateMoveToTargetState();
			}
			else if (this._currentState == BotAiController.AiState.Damage)
			{
				this.UpdateDamagedTargetState();
			}
			else if (this._currentState == BotAiController.AiState.Waiting)
			{
				this.IsWaitingState = true;
			}
			else if (this._currentState == BotAiController.AiState.Teleportation)
			{
				base.StartCoroutine(this.TeleportFromRandomPoint());
			}
			if (this._botController.IsDeath)
			{
				if (this._botController.IsFalling)
				{
					this._botController.SetPositionForFallState();
				}
				if (!this._isDeaded)
				{
					this._naveMeshAgent.enabled = false;
					this._isDeaded = true;
				}
			}
		}

		private void UpdateCurrentAiState()
		{
			if (this.IsCanMove)
			{
				this._currentState = BotAiController.AiState.Patrol;
			}
			else if (this._botController.IsDeath || !(this.currentTarget != null))
			{
				this._currentState = BotAiController.AiState.None;
			}
			else
			{
				this._currentState = this.CheckActiveAttackState();
			}
		}

		private void UpdateDamagedTargetState()
		{
			this.IsWaitingState = false;
			this.ResetNavigationPathIfNeed();
			Vector3 vector3 = this.currentTarget.position;
			vector3.y = base.transform.position.y;
			this._botController.OrientToTarget(vector3);
			this._botController.PlayAnimZombieAttackOrStopByMode();
			if (this._typeBot == BotAiController.TypeBot.Melee)
			{
				this._timeToTakeDamage -= Time.deltaTime;
				if (this._timeToTakeDamage <= 0f)
				{
					this._botController.MakeDamage(this.currentTarget);
					this._timeToTakeDamage = this.GetTimeToTakeDamageMeleeBot();
				}
			}
		}

		private void UpdateMoveToTargetState()
		{
			this.IsWaitingState = false;
			if (!this._botController.isFlyingSpeedLimit || !this._naveMeshAgent.isOnOffMeshLink)
			{
				this._naveMeshAgent.speed = this._botController.GetAttackSpeedByCompleteLevel();
			}
			else
			{
				this._naveMeshAgent.speed = this._botController.maxFlyingSpeed;
			}
			this._naveMeshAgent.SetDestination(this.currentTarget.position);
			if (this._typeBot == BotAiController.TypeBot.Melee)
			{
				this._timeToTakeDamage = this.GetTimeToTakeDamageMeleeBot();
			}
			this._botController.PlayAnimZombieWalkByMode();
		}

		private void UpdatePatrolState()
		{
			if (!this.IsCanMove)
			{
				return;
			}
			if (this._lastTimeMoving <= Time.time)
			{
				this.ResetNavigationPathIfNeed();
				Vector3 vector3 = base.transform.position;
				this._targetPoint = new Vector3(vector3.x + UnityEngine.Random.Range(-this.minLenghtMove, this.minLenghtMove), vector3.y, vector3.z + UnityEngine.Random.Range(-this.minLenghtMove, this.minLenghtMove));
				this._lastTimeMoving = Time.time + Vector3.Distance(base.transform.position, this._targetPoint) / this._botController.GetWalkSpeed();
				if (!this._naveMeshAgent.SetDestination(this._targetPoint))
				{
					this._lastTimeMoving = 0f;
					return;
				}
				this._botController.OrientToTarget(this._targetPoint);
				this._naveMeshAgent.speed = this._botController.GetWalkSpeed();
			}
		}

		private void UpdateTargetsForBot()
		{
			if (!this.isDetectPlayer)
			{
				return;
			}
			if (!this._isMultiplayerCoopMode)
			{
				this._timeToUpdateLocalTargets -= Time.deltaTime;
				if (this._timeToUpdateLocalTargets <= 0f)
				{
					this.CheckTargetForLocalMode();
				}
			}
			else
			{
				this._timeToUpdateMultiplayerTargets -= Time.deltaTime;
				if (this._timeToUpdateMultiplayerTargets <= 0f)
				{
					this._timeToUpdateMultiplayerTargets = 3f;
					this.CheckTargetForMultiplayerMode();
				}
			}
		}

		private enum AiState
		{
			Patrol,
			MoveToTarget,
			Damage,
			Waiting,
			Teleportation,
			None
		}

		private enum TargetType
		{
			Player,
			Turret,
			Bot,
			None
		}

		private enum TypeBot
		{
			Melee,
			Shooting,
			ShootAndMelee,
			None
		}
	}
}