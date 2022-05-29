using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class BotMovement : MonoBehaviour
{
	public static bool _deathAudioPlaying;

	private Transform target;

	private float timeToRemoveLive;

	public ZombieCreator _gameController;

	private bool Agression;

	private bool deaded;

	private Player_move_c healthDown;

	private GameObject player;

	private float CurLifeTime;

	private string idleAnim = "Idle";

	private string normWalkAnim = "Norm_Walk";

	private string zombieWalkAnim = "Zombie_Walk";

	private string offAnim = "Zombie_Off";

	private string deathAnim = "Zombie_Dead";

	private string onAnim = "Zombie_On";

	private string attackAnim = "Zombie_Attack";

	private string shootAnim;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private bool falling;

	private NavMeshAgent _nma;

	private BoxCollider _modelChildCollider;

	private Transform myTransform;

	static BotMovement()
	{
	}

	public BotMovement()
	{
	}

	[DebuggerHidden]
	private IEnumerator ___DelayedCallback(float time, BotMovement.DelayedCallback callback)
	{
		BotMovement.u003c___DelayedCallbacku003ec__Iterator10F variable = null;
		return variable;
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	private void Attack()
	{
	}

	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
			return;
		}
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				this._modelChild = ((Transform)enumerator.Current).gameObject;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
	}

	public void CodeAfterDelay(float delay, BotMovement.DelayedCallback callback)
	{
		base.StartCoroutine(this.___DelayedCallback(delay, callback));
	}

	[Obfuscation(Exclude=true)]
	private void Death()
	{
		ZombieCreator.LastEnemy -= new Action(this.IncreaseRange);
		this._nma.enabled = false;
		if (this.RequestPlayDeath(this._soundClips.death.length) && Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.death);
		}
		float single = this._soundClips.death.length;
		this._modelChild.GetComponent<Animation>().Stop();
		if (!this._modelChild.GetComponent<Animation>()[this.deathAnim])
		{
			this.StartFall();
		}
		else
		{
			this._modelChild.GetComponent<Animation>().Play(this.deathAnim);
			single = Mathf.Max(single, this._modelChild.GetComponent<Animation>()[this.deathAnim].length);
			this.CodeAfterDelay(this._modelChild.GetComponent<Animation>()[this.deathAnim].length * 1.25f, new BotMovement.DelayedCallback(this.StartFall));
		}
		this.CodeAfterDelay(single, new BotMovement.DelayedCallback(this.DestroySelf));
		this._modelChild.GetComponent<BoxCollider>().enabled = false;
		this.deaded = true;
		base.GetComponent<SpawnMonster>().ShouldMove = false;
		ZombieCreator numOfDeadZombies = this._gameController;
		numOfDeadZombies.NumOfDeadZombies = numOfDeadZombies.NumOfDeadZombies + 1;
		GlobalGameController.Score = GlobalGameController.Score + this._soundClips.scorePerKill;
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void FixedUpdate()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}

	private void IncreaseRange()
	{
		this._modelChild.GetComponent<Sounds>().attackingSpeed = Mathf.Max(this._modelChild.GetComponent<Sounds>().attackingSpeed, 3f);
		this._modelChild.GetComponent<Sounds>().detectRadius = 150f;
	}

	private void OnDestroy()
	{
		ZombieCreator.LastEnemy -= new Action(this.IncreaseRange);
	}

	public void PlayZombieRun()
	{
		if (this._modelChild.GetComponent<Animation>()[this.zombieWalkAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
		}
	}

	public bool RequestPlayDeath(float tm)
	{
		if (BotMovement._deathAudioPlaying)
		{
			return false;
		}
		base.StartCoroutine(this.resetDeathAudio(tm));
		return true;
	}

	[DebuggerHidden]
	private IEnumerator resetDeathAudio(float tm)
	{
		BotMovement.u003cresetDeathAudiou003ec__Iterator10E variable = null;
		return variable;
	}

	private void Run()
	{
	}

	public void SetTarget(Transform _target, bool agression)
	{
		this.Agression = agression;
		if (_target && this.target != _target)
		{
			this._nma.ResetPath();
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.voice);
			}
			this.PlayZombieRun();
		}
		else if (!_target && this.target != _target)
		{
			this._nma.ResetPath();
			this.Walk();
		}
		this.target = _target;
		base.GetComponent<SpawnMonster>().ShouldMove = _target == null;
	}

	public void Slowdown(float coef)
	{
	}

	private void Start()
	{
		this.myTransform = base.transform;
		this._nma = base.GetComponent<NavMeshAgent>();
		this._modelChildCollider = this._modelChild.GetComponent<BoxCollider>();
		this.shootAnim = this.offAnim;
		this.healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		this.player = GameObject.FindGameObjectWithTag("Player");
		this._gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
		ZombieCreator numOfLiveZombies = this._gameController;
		numOfLiveZombies.NumOfLiveZombies = numOfLiveZombies.NumOfLiveZombies + 1;
		this._soundClips = this._modelChild.GetComponent<Sounds>();
		this.timeToRemoveLive = this._soundClips.timeToHit * Mathf.Pow(0.95f, (float)GlobalGameController.AllLevelsCompleted);
		this.CurLifeTime = this.timeToRemoveLive;
		this.target = null;
		this._modelChild.GetComponent<Animation>().Stop();
		this.Walk();
		this._soundClips.attackingSpeed += UnityEngine.Random.Range(-this._soundClips.attackingSpeedRandomRange[0], this._soundClips.attackingSpeedRandomRange[1]);
		if (!Defs.IsSurvival)
		{
			this._soundClips.attackingSpeed *= Defs.DiffModif;
			this._soundClips.health *= Defs.DiffModif;
			this._soundClips.notAttackingSpeed *= Defs.DiffModif;
		}
		if (!Defs.IsSurvival && !base.gameObject.name.Contains("Boss"))
		{
			ZombieCreator.LastEnemy += new Action(this.IncreaseRange);
			if (this._gameController.IsLasTMonsRemains)
			{
				this.IncreaseRange();
			}
		}
	}

	private void StartFall()
	{
		this.falling = true;
	}

	private void Stop()
	{
	}

	private void Update()
	{
		if (!this.deaded)
		{
			if (this.target != null)
			{
				float single = Vector3.SqrMagnitude(this.target.position - this.myTransform.position);
				float single1 = this.target.position.x;
				float single2 = this.myTransform.position.y;
				Vector3 vector3 = this.target.position;
				Vector3 vector31 = new Vector3(single1, single2, vector3.z);
				if (single < this._soundClips.attackDistance * this._soundClips.attackDistance)
				{
					if (this._nma.path != null)
					{
						this._nma.ResetPath();
					}
					this.CurLifeTime -= Time.deltaTime;
					this.myTransform.LookAt(vector31);
					if (this.CurLifeTime <= 0f)
					{
						if (this.target.CompareTag("Player"))
						{
							this.healthDown.hit((float)this._soundClips.damagePerHit, this.myTransform.position, false);
						}
						if (this.target.CompareTag("Turret"))
						{
							this.target.GetComponent<TurretController>().MinusLive((float)this._soundClips.damagePerHit, 0, new NetworkViewID());
						}
						this.CurLifeTime = this.timeToRemoveLive;
						if (Defs.isSoundFX)
						{
							base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.bite);
						}
					}
					if (this._modelChild.GetComponent<Animation>()[this.attackAnim])
					{
						this._modelChild.GetComponent<Animation>().CrossFade(this.attackAnim);
					}
					else if (this._modelChild.GetComponent<Animation>()[this.shootAnim])
					{
						this._modelChild.GetComponent<Animation>().CrossFade(this.shootAnim);
					}
				}
				else
				{
					this._nma.SetDestination(vector31);
					this._nma.speed = this._soundClips.attackingSpeed * Mathf.Pow(1.05f, (float)GlobalGameController.AllLevelsCompleted);
					this.CurLifeTime = this.timeToRemoveLive;
					this.PlayZombieRun();
				}
			}
		}
		else if (this.falling)
		{
			float single3 = 7f;
			Transform transforms = this.myTransform;
			float single4 = this.myTransform.position.x;
			Vector3 vector32 = this.myTransform.position;
			float single5 = vector32.y - single3 * Time.deltaTime;
			Vector3 vector33 = this.myTransform.position;
			transforms.position = new Vector3(single4, single5, vector33.z);
		}
	}

	[Obfuscation(Exclude=true)]
	private void Walk()
	{
		this._modelChild.GetComponent<Animation>().Stop();
		if (!this._modelChild.GetComponent<Animation>()[this.normWalkAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
		}
		else
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.normWalkAnim);
		}
	}

	public delegate void DelayedCallback();
}