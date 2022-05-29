using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RilisoftBot
{
	public class BaseBot : MonoBehaviour
	{
		public const string BaseNameBotGuard = "BossGuard";

		private const int ScoreForDamage = 5;

		private const float HeadShotDamageModif = 2f;

		private const int MultiplySmoothMove = 5;

		public string nameBot;

		[Header("Sound settings")]
		public AudioClip damageSound;

		public AudioClip voiceMobSoud;

		public AudioClip takeDamageSound;

		public AudioClip deathSound;

		public AudioClip stepSound;

		public AudioClip runStepSound;

		[Header("Common damage settings")]
		public float notAttackingSpeed = 1f;

		public float attackingSpeed = 1f;

		public float health = 100f;

		public float attackDistance = 3f;

		public float detectRadius = 17f;

		public float damagePerHit = 1f;

		public int scorePerKill = 50;

		public float[] attackingSpeedRandomRange = new float[] { -0.5f, 0.5f };

		[Header("Effects settings")]
		public Texture flashDeadthTexture;

		public float heightFlyOutHitEffect = 1.75f;

		[NonSerialized]
		public int indexMobPrefabForCoop;

		protected BotAiController botAiController;

		protected Transform mobModel;

		protected Animation animations;

		protected bool isMobChampion;

		protected BoxCollider modelCollider;

		protected SphereCollider headCollider;

		protected BaseBot.BotAnimationName animationsName;

		protected AudioSource audioSource;

		private bool _isFlashing;

		private PhotonView _photonView;

		private bool _isMultiplayerMode;

		private BotChangeDamageMaterial[] _botMaterials;

		private IEnemyEffectsManager _effectsManager;

		private bool _isPlayingDamageSound;

		private bool _isDeathAudioPlaying;

		[Header("Automatic animation speed settings")]
		public bool isAutomaticAnimationEnable;

		[Range(0.1f, 2f)]
		public float speedAnimationWalk = 1f;

		[Range(0.1f, 2f)]
		public float speedAnimationRun = 1f;

		[Range(0.1f, 2f)]
		public float speedAnimationAttack = 1f;

		[Header("Flying settings")]
		public bool isFlyingSpeedLimit;

		public float maxFlyingSpeed;

		[Header("Guard settings")]
		public GameObject[] guards;

		private bool _isWeaponCreated;

		private bool _killed;

		private float _modMoveSpeedByDebuff = 1f;

		private List<BotDebuff> _botDebufs = new List<BotDebuff>();

		private Vector3 _botPosition;

		private Quaternion _botRotation;

		private BaseBot.RunNetworkAnimationType _currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.None;

		public float baseHealth
		{
			get;
			private set;
		}

		public bool IsDeath
		{
			get;
			private set;
		}

		public bool IsFalling
		{
			get;
			private set;
		}

		public bool needDestroyByMasterClient
		{
			get;
			private set;
		}

		public BaseBot()
		{
		}

		private void AntiHackForCreateMobInInvalidGameMode()
		{
			if (Defs.isMulti && !Defs.isCOOP)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		public void ApplyDebuff(BotDebuffType type, float timeLife, object parametrs)
		{
			BotDebuff debuffByType = this.GetDebuffByType(type);
			if (debuffByType != null)
			{
				this.ReplaceDebuff(debuffByType, timeLife, parametrs);
			}
			else
			{
				BotDebuff botDebuff = new BotDebuff(type, timeLife, parametrs);
				botDebuff.OnRun += new BotDebuff.OnRunDelegate(this.RunDebuff);
				botDebuff.OnStop += new BotDebuff.OnStopDelegate(this.StopDebuff);
				this._botDebufs.Add(botDebuff);
			}
		}

		public void ApplyDebuffByMode(BotDebuffType type, float timeLife, object parametrs)
		{
			if (!this._isMultiplayerMode)
			{
				this.ApplyDebuff(type, timeLife, parametrs);
				return;
			}
			this.ApplyDebufForMultiplayer(type, timeLife, parametrs);
		}

		public void ApplyDebufForMultiplayer(BotDebuffType type, float timeLife, object parametrs)
		{
			this.ApplyDebuff(type, timeLife, parametrs);
			if (type == BotDebuffType.DecreaserSpeed)
			{
				this._photonView.RPC("ApplyDebuffRPC", PhotonTargets.Others, new object[] { (int)type, timeLife, (float)parametrs });
			}
		}

		[PunRPC]
		[RPC]
		public void ApplyDebuffRPC(int typeDebuff, float timeLife, float parametr)
		{
			this.ApplyDebuff((BotDebuffType)typeDebuff, timeLife, parametr);
		}

		private void Awake()
		{
			this._effectsManager = base.GetComponent<IEnemyEffectsManager>();
			this.audioSource = base.GetComponent<AudioSource>();
			this.isMobChampion = false;
			this.Initialize();
		}

		private float CheckAnimationSpeedRunMoveForBot(float modSpeed)
		{
			float single = this.speedAnimationRun * modSpeed;
			this.animations[this.animationsName.Run].speed = single;
			return single * this.notAttackingSpeed;
		}

		private float CheckAnimationSpeedWalkMoveForBot(float modSpeed)
		{
			float single = this.speedAnimationWalk * modSpeed;
			this.animations[this.animationsName.Walk].speed = single;
			return single * this.attackingSpeed;
		}

		[DebuggerHidden]
		private IEnumerator CheckCanPlayDamageAudio(float timeOut)
		{
			BaseBot.u003cCheckCanPlayDamageAudiou003ec__Iterator110 variable = null;
			return variable;
		}

		public virtual bool CheckEnemyInAttackZone(float distanceToEnemy)
		{
			return false;
		}

		private void CheckForceKillGuards()
		{
			if ((int)this.guards.Length == 0)
			{
				return;
			}
			ZombieCreator zombieCreator = ZombieCreator.sharedCreator;
			if (zombieCreator == null)
			{
				return;
			}
			for (int i = 0; i < (int)zombieCreator.bossGuads.Length; i++)
			{
				GameObject gameObject = zombieCreator.bossGuads[i];
				if (gameObject.gameObject != null)
				{
					BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(gameObject.transform);
					if (!botScriptForObject.IsDeath)
					{
						botScriptForObject.GetDamage(-2.1474836E+09f, null, false, false);
					}
				}
			}
		}

		[DebuggerHidden]
		private IEnumerator DelayedDestroySelf(float delay)
		{
			BaseBot.u003cDelayedDestroySelfu003ec__Iterator113 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator DelayedSetFallState(float delay)
		{
			BaseBot.u003cDelayedSetFallStateu003ec__Iterator112 variable = null;
			return variable;
		}

		public virtual void DelayShootAfterEvent(float seconds)
		{
		}

		public void DestroyByNetworkType()
		{
			if (!this._isMultiplayerMode)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (!PhotonNetwork.isMasterClient)
			{
				this.needDestroyByMasterClient = true;
				this.DisableMobForDeleteMasterClient();
			}
			else
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
		}

		private void DisableMobForDeleteMasterClient()
		{
			this.modelCollider.gameObject.SetActive(false);
			if (this.headCollider != null)
			{
				this.headCollider.gameObject.SetActive(false);
			}
			MonoBehaviour[] components = base.GetComponents<MonoBehaviour>();
			for (int i = 0; i < (int)components.Length; i++)
			{
				bool flag = components[i] is PhotonView;
				if (!flag && !(components[i] is BaseBot))
				{
					components[i].enabled = false;
				}
			}
		}

		public void FireByRPC(Vector3 pointFire, Vector3 positionToFire)
		{
			this._photonView.RPC("FireBulletRPC", PhotonTargets.Others, new object[] { pointFire, positionToFire });
		}

		public float GetAttackSpeedByCompleteLevel()
		{
			if (this.isAutomaticAnimationEnable)
			{
				return this.CheckAnimationSpeedRunMoveForBot(this._modMoveSpeedByDebuff);
			}
			return this.attackingSpeed * this._modMoveSpeedByDebuff;
		}

		public static BaseBot GetBotScriptForObject(Transform obj)
		{
			return obj.GetComponent<BaseBot>();
		}

		private Texture GetBotSkin()
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
			if ((int)componentsInChildren.Length == 0)
			{
				return null;
			}
			return componentsInChildren[(int)componentsInChildren.Length - 1].material.mainTexture;
		}

		public void GetDamage(float damage, Transform instigator, string weaponName, bool isOwnerDamage = true, bool isHeadShot = false)
		{
			this.GetDamage(damage, instigator, isOwnerDamage, isHeadShot);
			if (this._killed)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("Bot is receiving damage after death.");
				}
				return;
			}
			if (this.health != 0f)
			{
				return;
			}
			if (!isOwnerDamage)
			{
				return;
			}
			if (!TrainingController.TrainingCompleted)
			{
				return;
			}
			if (Defs.isMulti && NetworkStartTable.LocalOrPasswordRoom())
			{
				return;
			}
			ShopNGUIController.CategoryNames categoryName = ShopNGUIController.CategoryNames.BackupCategory | ShopNGUIController.CategoryNames.MeleeCategory | ShopNGUIController.CategoryNames.SpecilCategory | ShopNGUIController.CategoryNames.SniperCategory | ShopNGUIController.CategoryNames.PremiumCategory | ShopNGUIController.CategoryNames.HatsCategory | ShopNGUIController.CategoryNames.ArmorCategory | ShopNGUIController.CategoryNames.SkinsCategory | ShopNGUIController.CategoryNames.CapesCategory | ShopNGUIController.CategoryNames.BootsCategory | ShopNGUIController.CategoryNames.GearCategory | ShopNGUIController.CategoryNames.MaskCategory;
			string str = weaponName.Replace("(Clone)", string.Empty);
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(str);
			if (byPrefabName != null)
			{
				categoryName = (ShopNGUIController.CategoryNames)PromoActionsGUIController.CatForTg(byPrefabName.Tag);
			}
			QuestMediator.NotifyKillMonster(categoryName, (Defs.isMulti ? false : !Defs.IsSurvival));
			this._killed = true;
		}

		public void GetDamage(float damage, Transform instigator, bool isOwnerDamage = true, bool isHeadShot = false)
		{
			if (this.IsDeath)
			{
				return;
			}
			if (damage < 0f && !this._isFlashing)
			{
				base.StartCoroutine(this.ShowDamageEffect());
			}
			if (!isHeadShot)
			{
				this.ShowHitEffect();
			}
			else
			{
				this.ShowHeadShotEffect();
				damage *= 2f;
			}
			this.health += damage;
			if (this.health < 0f)
			{
				this.health = 0f;
			}
			if (this.health == 0f)
			{
				this.PrepareDeath(isOwnerDamage);
				if (isOwnerDamage)
				{
					this.TakeBonusForKill();
				}
			}
			else if (isOwnerDamage)
			{
				GlobalGameController.Score = GlobalGameController.Score + 5;
			}
			this.TryPlayDamageSound(this.damageSound.length);
			if (instigator != null && this.health > 0f)
			{
				this.botAiController.SetTargetForced(instigator);
			}
		}

		public void GetDamageForMultiplayer(float damage, Transform instigator, string weaponName, bool isHeadShot = false)
		{
			this.GetDamage(damage, instigator, weaponName, true, isHeadShot);
			this._photonView.RPC("GetDamageRPC", PhotonTargets.Others, new object[] { damage, instigator, false, isHeadShot });
		}

		public void GetDamageForMultiplayer(float damage, Transform instigator, bool isHeadShot = false)
		{
			this.GetDamage(damage, instigator, true, isHeadShot);
			this._photonView.RPC("GetDamageRPC", PhotonTargets.Others, new object[] { damage, instigator, false, isHeadShot });
		}

		[PunRPC]
		[RPC]
		public void GetDamageRPC(float damage, Transform instigator, bool isOwnerDamage, bool isHeadShot)
		{
			this.GetDamage(damage, instigator, isOwnerDamage, isHeadShot);
		}

		private BotDebuff GetDebuffByType(BotDebuffType type)
		{
			for (int i = 0; i < this._botDebufs.Count; i++)
			{
				if (this._botDebufs[i].type == type)
				{
					return this._botDebufs[i];
				}
			}
			return null;
		}

		public virtual Vector3 GetHeadPoint()
		{
			Vector3 vector3 = base.transform.position;
			vector3.y = vector3.y + (this.headCollider == null ? this.modelCollider.size.y * 0.75f : this.headCollider.center.y);
			return vector3;
		}

		public virtual float GetMaxAttackDistance()
		{
			return this.GetMaxAttackDistance();
		}

		public static Vector3 GetPositionSpawnGuard(Vector3 bossPosition)
		{
			float single = UnityEngine.Random.Range(0.5f, 1f);
			return bossPosition + new Vector3(single, single, single);
		}

		public float GetSquareAttackDistance()
		{
			return this.attackDistance * this.attackDistance;
		}

		public float GetSquareDetectRadius()
		{
			return this.detectRadius * this.detectRadius;
		}

		public float GetWalkSpeed()
		{
			if (this.isAutomaticAnimationEnable)
			{
				return this.CheckAnimationSpeedWalkMoveForBot(this._modMoveSpeedByDebuff);
			}
			return this.notAttackingSpeed * this._modMoveSpeedByDebuff;
		}

		private void IncreaseRange()
		{
			if (!this.isAutomaticAnimationEnable)
			{
				this.attackingSpeed = Mathf.Max(this.attackingSpeed, 3f);
			}
			else
			{
				this.speedAnimationRun = Mathf.Max(this.speedAnimationRun, 1.5f);
			}
			this.detectRadius = 150f;
		}

		protected virtual void Initialize()
		{
			this.animationsName = new BaseBot.BotAnimationName();
			this.AntiHackForCreateMobInInvalidGameMode();
			this._photonView = base.GetComponent<PhotonView>();
			this._isMultiplayerMode = (this._photonView == null ? false : Defs.isCOOP);
			this.animations = base.GetComponentInChildren<Animation>();
			this.animations.Stop();
			this.botAiController = base.GetComponent<BotAiController>();
			this.modelCollider = base.GetComponentInChildren<BoxCollider>();
			this.headCollider = base.GetComponentInChildren<SphereCollider>();
			UnityEngine.Random.seed = (int)DateTime.Now.Ticks & 65535;
			this.InitializeRandomAttackSpeed();
			this.ModifyParametrsForLocalMode();
			this.needDestroyByMasterClient = false;
			this.baseHealth = this.health;
		}

		private void InitializeRandomAttackSpeed()
		{
			if (!this.isAutomaticAnimationEnable)
			{
				this.attackingSpeed += UnityEngine.Random.Range(-this.attackingSpeedRandomRange[0], this.attackingSpeedRandomRange[1]);
			}
			else
			{
				float single = (this.attackingSpeed - this.attackingSpeedRandomRange[0]) / this.attackingSpeed;
				float single1 = (this.attackingSpeed + this.attackingSpeedRandomRange[1]) / this.attackingSpeed;
				this.speedAnimationRun = UnityEngine.Random.Range(single, single1);
			}
		}

		private void InitNetworkStateData()
		{
			this._botPosition = base.transform.position;
			this._botRotation = base.transform.rotation;
		}

		private bool IsBotGuard()
		{
			return base.gameObject.name.Contains("BossGuard");
		}

		private bool IsCanPlayDeathSound(float timeOut)
		{
			if (this._isDeathAudioPlaying)
			{
				return false;
			}
			base.StartCoroutine(this.ResetDeathAudio(timeOut));
			return true;
		}

		public static void LogDebugData(string message)
		{
		}

		public void MakeDamage(Transform target, float damageValue)
		{
			bool flag = false;
			if (target.CompareTag("Player"))
			{
				flag = true;
				Player_move_c component = target.GetComponent<SkinName>().playerMoveC;
				if (!this._isMultiplayerMode)
				{
					component.hit(damageValue, base.transform.position, false);
				}
				else
				{
					component.minusLiveFromZombi(damageValue, base.transform.position);
				}
			}
			else if (target.CompareTag("Turret"))
			{
				flag = true;
				target.GetComponent<TurretController>().MinusLive(damageValue, 0, new NetworkViewID());
			}
			else if (target.CompareTag("Enemy"))
			{
				flag = true;
				BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(target);
				if (!this._isMultiplayerMode)
				{
					botScriptForObject.GetDamage(damageValue, null, false, false);
				}
				else
				{
					botScriptForObject.GetDamageForMultiplayer(damageValue, null, false);
				}
			}
			if (flag)
			{
				this.TryPlayAudioClip(this.takeDamageSound);
			}
		}

		public void MakeDamage(Transform target)
		{
			this.MakeDamage(target, this.damagePerHit);
		}

		private void ModifyParametrsForLocalMode()
		{
			float diffModif = 0f;
			float single = 0f;
			if (!this.isAutomaticAnimationEnable)
			{
				diffModif = this.notAttackingSpeed;
				single = this.attackingSpeed;
			}
			else
			{
				diffModif = this.speedAnimationWalk;
				single = this.speedAnimationRun;
			}
			if (!this._isMultiplayerMode)
			{
				if (!Defs.IsSurvival)
				{
					single *= Defs.DiffModif;
					this.health *= Defs.DiffModif;
					diffModif *= Defs.DiffModif;
				}
				else if (Defs.IsSurvival && TrainingController.TrainingCompleted)
				{
					int num = ZombieCreator.sharedCreator.currentWave;
					if (num == 0)
					{
						diffModif *= 0.75f;
						single *= 0.75f;
						this.health *= 0.7f;
					}
					else if (num == 1)
					{
						diffModif *= 0.85f;
						single *= 0.85f;
						this.health *= 0.8f;
					}
					else if (num == 2)
					{
						diffModif *= 0.9f;
						single *= 0.9f;
						this.health *= 0.9f;
					}
					else if (num >= 7)
					{
						diffModif *= 1.25f;
						single *= 1.25f;
					}
					else if (num >= 9)
					{
						this.health *= 1.25f;
					}
				}
			}
			if (this._isMultiplayerMode || Defs.IsSurvival)
			{
				diffModif = diffModif * (0.9f + (float)ExpController.OurTierForAnyPlace() * 0.1f);
				BaseBot baseBot = this;
				baseBot.health = baseBot.health * (0.9f + (float)ExpController.OurTierForAnyPlace() * 0.1f);
			}
			if (!this.isAutomaticAnimationEnable)
			{
				this.notAttackingSpeed = diffModif;
				this.attackingSpeed = single;
			}
			else
			{
				this.speedAnimationWalk = diffModif;
				this.speedAnimationRun = single;
			}
			if (!Defs.IsSurvival && !this._isMultiplayerMode)
			{
				this.SetRangeParametrs();
			}
		}

		protected virtual void OnBotDestroyEvent()
		{
		}

		private void OnDestroy()
		{
			this.OnBotDestroyEvent();
			if (!this._isMultiplayerMode)
			{
				ZombieCreator.LastEnemy -= new Action(this.IncreaseRange);
			}
			Initializer.enemiesObj.Remove(base.gameObject);
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (!this._isMultiplayerMode)
			{
				return;
			}
			if (!stream.isWriting)
			{
				this._botPosition = (Vector3)stream.ReceiveNext();
				this._botRotation = (Quaternion)stream.ReceiveNext();
			}
			else
			{
				stream.SendNext(base.transform.position);
				stream.SendNext(base.transform.rotation);
			}
		}

		public void OrientToTarget(Vector3 targetPos)
		{
			base.transform.LookAt(targetPos);
		}

		[Obfuscation(Exclude=true)]
		public void PlayAnimationIdle()
		{
			this.animations.Stop();
			if (this.animations[this.animationsName.Idle])
			{
				this.animations.CrossFade(this.animationsName.Idle);
			}
			this.StopSteps();
		}

		public void PlayAnimationWalk()
		{
			this.animations.Stop();
			if (!this.animations[this.animationsName.Walk])
			{
				this.animations.CrossFade(this.animationsName.Run);
			}
			else
			{
				this.animations.CrossFade(this.animationsName.Walk);
			}
			this.PlayWalkStepSound();
		}

		protected virtual void PlayAnimationZombieAttackOrStop()
		{
			if (this.animations[this.animationsName.Attack])
			{
				this.animations.CrossFade(this.animationsName.Attack);
			}
			else if (this.animations[this.animationsName.Stop])
			{
				this.animations.CrossFade(this.animationsName.Stop);
			}
			this.StopSteps();
		}

		private void PlayAnimationZombieWalk()
		{
			if (this.animations[this.animationsName.Run])
			{
				this.animations.CrossFade(this.animationsName.Run);
			}
			this.PlayRunStepSound();
		}

		public void PlayAnimZombieAttackOrStopByMode()
		{
			if (!this._isMultiplayerMode)
			{
				this.PlayAnimationZombieAttackOrStop();
				return;
			}
			if (this._currentRunNetworkAnimation != BaseBot.RunNetworkAnimationType.ZombieAttackOrStop)
			{
				this.PlayAnimationZombieAttackOrStop();
				this._photonView.RPC("PlayZombieAttackRPC", PhotonTargets.Others, new object[0]);
			}
			this._currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.ZombieAttackOrStop;
		}

		public void PlayAnimZombieWalkByMode()
		{
			if (!this._isMultiplayerMode)
			{
				this.PlayAnimationZombieWalk();
				return;
			}
			if (this._currentRunNetworkAnimation != BaseBot.RunNetworkAnimationType.ZombieWalk)
			{
				this.PlayAnimationZombieWalk();
				this._photonView.RPC("PlayZombieRunRPC", PhotonTargets.Others, new object[0]);
			}
			this._currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.ZombieWalk;
		}

		public void PlayRunStepSound()
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.runStepSound == null)
			{
				return;
			}
			if (this.audioSource.clip != this.runStepSound)
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.runStepSound;
				this.audioSource.Play();
			}
		}

		public void PlayVoiceSound()
		{
			this.TryPlayAudioClip(this.voiceMobSoud);
		}

		public void PlayWalkStepSound()
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.stepSound == null)
			{
				return;
			}
			if (this.audioSource.clip != this.stepSound)
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.stepSound;
				this.audioSource.Play();
			}
		}

		[PunRPC]
		[RPC]
		public void PlayZombieAttackRPC()
		{
			this.PlayAnimationZombieAttackOrStop();
			this._currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.ZombieAttackOrStop;
		}

		[PunRPC]
		[RPC]
		public void PlayZombieRunRPC()
		{
			this.PlayAnimationZombieWalk();
			this._currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.ZombieWalk;
		}

		[Obfuscation(Exclude=true)]
		public void PrepareDeath(bool isOwnerDamage = true)
		{
			if (!this._isMultiplayerMode)
			{
				ZombieCreator.LastEnemy -= new Action(this.IncreaseRange);
			}
			this.botAiController.isDetectPlayer = false;
			this.botAiController.IsCanMove = false;
			this.IsDeath = true;
			float single = this.deathSound.length;
			this.TryPlayDeathSound(single);
			this.animations.Stop();
			if (!this.animations[this.animationsName.Death])
			{
				this.IsFalling = true;
			}
			else
			{
				this.animations.Play(this.animationsName.Death);
				single = Mathf.Max(single, this.animations[this.animationsName.Death].length);
				base.StartCoroutine(this.DelayedSetFallState(this.animations[this.animationsName.Death].length * 1.25f));
			}
			base.StartCoroutine(this.DelayedDestroySelf(single));
			this.modelCollider.enabled = false;
			if (this.headCollider != null)
			{
				this.headCollider.enabled = false;
			}
			if (isOwnerDamage)
			{
				GlobalGameController.Score = GlobalGameController.Score + this.scorePerKill;
			}
			this.CheckForceKillGuards();
		}

		private void ReplaceDebuff(BotDebuff oldDebuff, float newTimeLife, object newParametrs)
		{
			if (oldDebuff.type == BotDebuffType.DecreaserSpeed)
			{
				oldDebuff.ReplaceValues(newTimeLife, newParametrs);
				this.RunDebuff(oldDebuff);
			}
		}

		[DebuggerHidden]
		private IEnumerator ResetDeathAudio(float timeOut)
		{
			BaseBot.u003cResetDeathAudiou003ec__Iterator111 variable = null;
			return variable;
		}

		private void RunDebuff(BotDebuff debuff)
		{
			if (debuff.type == BotDebuffType.DecreaserSpeed)
			{
				this._modMoveSpeedByDebuff = debuff.GetFloatParametr();
			}
		}

		[PunRPC]
		[RPC]
		public void SetBotHealthRPC(float botHealth)
		{
			this.health = botHealth;
		}

		public void SetPositionForFallState()
		{
			Transform vector3 = base.transform;
			float single = base.transform.position.x;
			Vector3 vector31 = base.transform.position;
			float single1 = vector31.y - 7f * Time.deltaTime;
			Vector3 vector32 = base.transform.position;
			vector3.position = new Vector3(single, single1, vector32.z);
		}

		private void SetRangeParametrs()
		{
			if (this.isMobChampion || this.IsBotGuard())
			{
				return;
			}
			ZombieCreator.LastEnemy += new Action(this.IncreaseRange);
			if (ZombieCreator.sharedCreator.IsLasTMonsRemains)
			{
				this.IncreaseRange();
			}
		}

		[DebuggerHidden]
		private IEnumerator ShowDamageEffect()
		{
			BaseBot.u003cShowDamageEffectu003ec__Iterator114 variable = null;
			return variable;
		}

		private void ShowDamageTexture(bool isEnable)
		{
			if (this._botMaterials == null || (int)this._botMaterials.Length == 0)
			{
				return;
			}
			for (int i = 0; i < (int)this._botMaterials.Length; i++)
			{
				if (!isEnable)
				{
					this._botMaterials[i].ResetMainMaterial();
				}
				else
				{
					this._botMaterials[i].ShowDamageEffect();
				}
			}
		}

		private void ShowHeadShotEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = HeadShotStackController.sharedController.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.headCollider.transform.TransformPoint(this.headCollider.center));
			}
		}

		private void ShowHitEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = HitStackController.sharedController.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				if (this.headCollider == null)
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, base.transform.position + (Vector3.up * this.heightFlyOutHitEffect));
				}
				else
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.headCollider.transform.TransformPoint(this.headCollider.center));
				}
			}
		}

		private void Start()
		{
			if (this._isMultiplayerMode && this._photonView.isMine)
			{
				this._photonView.RPC("SetBotHealthRPC", PhotonTargets.All, new object[] { this.health });
			}
			if (!this._isMultiplayerMode)
			{
				ZombieCreator numOfLiveZombies = ZombieCreator.sharedCreator;
				numOfLiveZombies.NumOfLiveZombies = numOfLiveZombies.NumOfLiveZombies + 1;
			}
			this.mobModel = this.modelCollider.transform.GetChild(0);
			this._botMaterials = base.GetComponentsInChildren<BotChangeDamageMaterial>();
			this.InitNetworkStateData();
			Initializer.enemiesObj.Add(base.gameObject);
			if (this._effectsManager == null)
			{
				this._effectsManager = base.gameObject.AddComponent<PortalEnemyEffectsManager>();
			}
			this._effectsManager.ShowSpawnEffect();
		}

		private void StopDebuff(BotDebuff debuff)
		{
			if (debuff.type == BotDebuffType.DecreaserSpeed)
			{
				this._modMoveSpeedByDebuff = 1f;
			}
		}

		public void StopSteps()
		{
			if (this.stepSound == null)
			{
				return;
			}
			if (this.audioSource.clip == this.stepSound || this.audioSource.clip == this.runStepSound)
			{
				this.audioSource.Pause();
				this.audioSource.clip = null;
			}
		}

		private void TakeBonusForKill()
		{
			if (!this.isMobChampion)
			{
				return;
			}
			if (this._isWeaponCreated)
			{
				return;
			}
			if (!LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
			{
				return;
			}
			string item = LevelBox.weaponsFromBosses[Application.loadedLevelName];
			Vector3 vector3 = base.gameObject.transform.position + new Vector3(0f, 0.25f, 0f);
			if (Application.loadedLevelName == "Sky_islands")
			{
				vector3 -= new Vector3(0f, 1.5f, 0f);
			}
			GameObject gameObject = ZombieCreator.sharedCreator.weaponBonus;
			((gameObject == null ? BonusCreator._CreateBonus(item, vector3) : BonusCreator._CreateBonusFromPrefab(gameObject, vector3))).AddComponent<GotToNextLevel>();
			ZombieCreator.sharedCreator.weaponBonus = null;
			this._isWeaponCreated = true;
		}

		public void TryPlayAudioClip(AudioClip audioClip)
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.audioSource == null || audioClip == null)
			{
				return;
			}
			this.audioSource.PlayOneShot(audioClip);
		}

		public void TryPlayDamageSound(float delay)
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.audioSource == null || this._isPlayingDamageSound)
			{
				return;
			}
			base.StartCoroutine(this.CheckCanPlayDamageAudio(delay));
			this.audioSource.PlayOneShot(this.damageSound);
		}

		public void TryPlayDeathSound(float delay)
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.audioSource == null || !this.IsCanPlayDeathSound(delay))
			{
				return;
			}
			this.audioSource.PlayOneShot(this.deathSound);
		}

		private void Update()
		{
			this.UpdateDebuffState();
			if (base.GetComponent<AudioSource>().enabled == Time.timeScale == 0f)
			{
				base.GetComponent<AudioSource>().enabled = !base.GetComponent<AudioSource>().enabled;
				if (base.GetComponent<AudioSource>().enabled)
				{
					base.GetComponent<AudioSource>().Play();
				}
			}
			if (!this._isMultiplayerMode)
			{
				return;
			}
			if (PhotonNetwork.isMasterClient && this.needDestroyByMasterClient)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
			if (!this._photonView.isMine && !this.needDestroyByMasterClient)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this._botPosition, Time.deltaTime * 5f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this._botRotation, Time.deltaTime * 5f);
			}
		}

		public void UpdateDebuffState()
		{
			if (this._botDebufs.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._botDebufs.Count; i++)
			{
				if (this._botDebufs[i].isRun)
				{
					BotDebuff item = this._botDebufs[i];
					item.timeLife = item.timeLife - Time.deltaTime;
					if (this._botDebufs[i].timeLife <= 0f)
					{
						this._botDebufs[i].Stop();
						this._botDebufs.Remove(this._botDebufs[i]);
					}
				}
				else
				{
					this._botDebufs[i].Run();
				}
			}
		}

		protected class BotAnimationName
		{
			public string Walk;

			public string Run;

			public string Stop;

			public string Death;

			public string Attack;

			public string Idle;

			public BotAnimationName()
			{
			}
		}

		private enum RunNetworkAnimationType
		{
			ZombieWalk,
			ZombieAttackOrStop,
			None
		}
	}
}