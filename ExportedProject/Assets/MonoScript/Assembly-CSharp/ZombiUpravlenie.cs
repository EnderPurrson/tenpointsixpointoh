using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class ZombiUpravlenie : MonoBehaviour
{
	public static bool _deathAudioPlaying;

	public GameObject playerKill;

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

	private ZombiManager _gameController;

	public bool deaded;

	public Transform target;

	public float health;

	private PhotonView photonView;

	public Texture hitTexture;

	private Texture _skin;

	private static SkinsManagerPixlGun _skinsManager;

	private bool _flashing;

	public int typeZombInMas;

	private float timeToUpdateTarget = 5f;

	private float timeToUpdateNavMesh;

	public int tekAnim = -1;

	private float timeToResetPath;

	static ZombiUpravlenie()
	{
	}

	public ZombiUpravlenie()
	{
	}

	[DebuggerHidden]
	private IEnumerator ___DelayedCallback(float time, ZombiUpravlenie.DelayedCallback callback)
	{
		ZombiUpravlenie.u003c___DelayedCallbacku003ec__Iterator125 variable = null;
		return variable;
	}

	private void Awake()
	{
		try
		{
			this._modelChild = base.transform.GetChild(0).gameObject;
			this.health = this._modelChild.GetComponent<Sounds>().health;
			if (Defs.isMulti && !Defs.isCOOP)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (!Defs.isCOOP)
			{
				base.enabled = false;
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			UnityEngine.Debug.LogError("Cooperative mode failure.");
			UnityEngine.Debug.LogException(exception);
			throw;
		}
	}

	public void CodeAfterDelay(float delay, ZombiUpravlenie.DelayedCallback callback)
	{
		base.StartCoroutine(this.___DelayedCallback(delay, callback));
	}

	[PunRPC]
	[RPC]
	private void Death()
	{
		if (!Defs.isCOOP)
		{
			return;
		}
		if (this._nma != null)
		{
			this._nma.enabled = false;
		}
		float single = 0.1f;
		if (Defs.isSoundFX && this._soundClips != null)
		{
			if (this.RequestPlayDeath(this._soundClips.death.length))
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.death);
			}
			single = this._soundClips.death.length;
		}
		this._modelChild.GetComponent<Animation>().Stop();
		if (!this._modelChild.GetComponent<Animation>()[this.deathAnim])
		{
			this.StartFall();
		}
		else
		{
			this._modelChild.GetComponent<Animation>().Play(this.deathAnim);
			single = Mathf.Max(single, this._modelChild.GetComponent<Animation>()[this.deathAnim].length);
			this.CodeAfterDelay(this._modelChild.GetComponent<Animation>()[this.deathAnim].length * 1.25f, new ZombiUpravlenie.DelayedCallback(this.StartFall));
		}
		this.CodeAfterDelay(single, new ZombiUpravlenie.DelayedCallback(this.DestroySelf));
		this._modelChild.GetComponent<BoxCollider>().enabled = false;
		this.deaded = true;
		base.GetComponent<SpawnMonster>().ShouldMove = false;
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[DebuggerHidden]
	private IEnumerator Flash()
	{
		ZombiUpravlenie.u003cFlashu003ec__Iterator124 variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	private void flashRPC()
	{
		base.StartCoroutine(this.Flash());
	}

	public void PlayZombieAttack()
	{
		if (this.tekAnim != 2 && Defs.isCOOP)
		{
			if (this._modelChild.GetComponent<Animation>()[this.attackAnim])
			{
				this._modelChild.GetComponent<Animation>().CrossFade(this.attackAnim);
			}
			else if (this._modelChild.GetComponent<Animation>()[this.shootAnim])
			{
				this._modelChild.GetComponent<Animation>().CrossFade(this.shootAnim);
			}
			this.photonView.RPC("PlayZombieAttackRPC", PhotonTargets.Others, new object[0]);
		}
		this.tekAnim = 2;
	}

	[PunRPC]
	[RPC]
	public void PlayZombieAttackRPC()
	{
		if (this._modelChild.GetComponent<Animation>()[this.attackAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.attackAnim);
		}
		else if (this._modelChild.GetComponent<Animation>()[this.shootAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.shootAnim);
		}
		this.tekAnim = 2;
	}

	public void PlayZombieRun()
	{
		if (this.tekAnim != 1 && Defs.isCOOP)
		{
			if (this._modelChild.GetComponent<Animation>()[this.zombieWalkAnim])
			{
				this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
			}
			this.photonView.RPC("PlayZombieRunRPC", PhotonTargets.Others, new object[0]);
		}
		this.tekAnim = 1;
	}

	[PunRPC]
	[RPC]
	public void PlayZombieRunRPC()
	{
		if (this._modelChild.GetComponent<Animation>()[this.zombieWalkAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
		}
		this.tekAnim = 1;
	}

	public bool RequestPlayDeath(float tm)
	{
		if (ZombiUpravlenie._deathAudioPlaying)
		{
			return false;
		}
		base.StartCoroutine(this.resetDeathAudio(tm));
		return true;
	}

	[DebuggerHidden]
	private IEnumerator resetDeathAudio(float tm)
	{
		ZombiUpravlenie.u003cresetDeathAudiou003ec__Iterator123 variable = null;
		return variable;
	}

	public void setHealth(float _health, bool isFlash)
	{
		this.photonView.RPC("setHealthRPC", PhotonTargets.All, new object[] { _health });
		if (isFlash && !this._flashing)
		{
			base.StartCoroutine(this.Flash());
			this.photonView.RPC("flashRPC", PhotonTargets.Others, new object[0]);
		}
	}

	[PunRPC]
	[RPC]
	private void setHealthRPC(float _health)
	{
		this.health = _health;
	}

	public void setId(int _id)
	{
		this.photonView.RPC("setIdRPC", PhotonTargets.All, new object[] { _id });
	}

	[PunRPC]
	[RPC]
	public void setIdRPC(int _id)
	{
		base.GetComponent<PhotonView>().viewID = _id;
	}

	public static Texture SetSkinForObj(GameObject go)
	{
		if (!ZombiUpravlenie._skinsManager)
		{
			ZombiUpravlenie._skinsManager = GameObject.FindGameObjectWithTag("SkinsManager").GetComponent<SkinsManagerPixlGun>();
		}
		Texture texture = null;
		string str = ZombiUpravlenie.SkinNameForObj(go.name);
		Texture item = ZombiUpravlenie._skinsManager.skins[str] as Texture;
		texture = item;
		if (!item)
		{
			UnityEngine.Debug.Log(string.Concat("No skin: ", str));
		}
		BotHealth.SetTextureRecursivelyFrom(go, texture);
		return texture;
	}

	public static string SkinNameForObj(string objName)
	{
		return objName;
	}

	[PunRPC]
	[RPC]
	public void SlowdownRPC(float coef)
	{
	}

	private void Start()
	{
		try
		{
			this._skin = ZombiUpravlenie.SetSkinForObj(this._modelChild);
			this._nma = base.GetComponent<NavMeshAgent>();
			this._modelChildCollider = this._modelChild.GetComponent<BoxCollider>();
			this.shootAnim = this.offAnim;
			this.player = GameObject.FindGameObjectWithTag("Player");
			this._gameController = GameObject.FindGameObjectWithTag("ZombiCreator").GetComponent<ZombiManager>();
			this._soundClips = this._modelChild.GetComponent<Sounds>();
			this.CurLifeTime = this._soundClips.timeToHit;
			this.target = null;
			this._modelChild.GetComponent<Animation>().Stop();
			this.Walk();
			this._soundClips.attackingSpeed += UnityEngine.Random.Range(-this._soundClips.attackingSpeedRandomRange[0], this._soundClips.attackingSpeedRandomRange[1]);
			this.photonView = PhotonView.Get(this);
			this._skin = ZombiUpravlenie.SetSkinForObj(this._modelChild);
			if (this.photonView.isMine)
			{
				this.photonView.RPC("setHealthRPC", PhotonTargets.All, new object[] { this._soundClips.health });
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			UnityEngine.Debug.LogError("Cooperative mode failure.");
			UnityEngine.Debug.LogException(exception);
			throw;
		}
	}

	private void StartFall()
	{
		this.falling = true;
	}

	private void Update()
	{
		try
		{
			if (!ZombiManager.sharedManager.startGame)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (this.photonView.isMine)
			{
				if (!this.deaded)
				{
					if (this.target != null && this.target.CompareTag("Player") && this.target.GetComponent<SkinName>().playerMoveC.isInvisible)
					{
						this.target = null;
					}
					if (!(this.target != null) || this.timeToUpdateTarget <= 0f)
					{
						this.timeToResetPath -= Time.deltaTime;
						if (this.timeToResetPath <= 0f)
						{
							this.timeToResetPath = 5f;
							this._nma.ResetPath();
							float single = (float)(-20 + UnityEngine.Random.Range(0, 40));
							Vector3 vector3 = base.transform.position;
							Vector3 vector31 = new Vector3(single, vector3.y, (float)(-20 + UnityEngine.Random.Range(0, 40)));
							base.transform.LookAt(vector31);
							this._nma.SetDestination(vector31);
							this._nma.speed = this._soundClips.notAttackingSpeed;
						}
						GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Turret");
						if (Initializer.players.Count > 0)
						{
							this.timeToUpdateTarget = 5f;
							float single1 = Vector3.SqrMagnitude(base.transform.position - Initializer.players[0].myPlayerTransform.position);
							this.target = Initializer.players[0].myPlayerTransform;
							foreach (Player_move_c player in Initializer.players)
							{
								if (!player.isInvisible)
								{
									float single2 = Vector3.SqrMagnitude(base.transform.position - player.myPlayerTransform.position);
									if (single2 >= single1)
									{
										continue;
									}
									single1 = single2;
									this.target = player.myPlayerTransform;
								}
							}
							GameObject[] gameObjectArray1 = gameObjectArray;
							for (int i = 0; i < (int)gameObjectArray1.Length; i++)
							{
								GameObject gameObject = gameObjectArray1[i];
								if (gameObject.GetComponent<TurretController>().isRun)
								{
									float single3 = Vector3.SqrMagnitude(base.transform.position - gameObject.transform.position);
									if (single3 < single1)
									{
										single1 = single3;
										this.target = gameObject.transform;
									}
								}
							}
						}
					}
					else
					{
						this.timeToUpdateTarget -= Time.deltaTime;
						float single4 = Vector3.SqrMagnitude(this.target.position - base.transform.position);
						float single5 = this.target.position.x;
						float single6 = base.transform.position.y;
						Vector3 vector32 = this.target.position;
						Vector3 vector33 = new Vector3(single5, single6, vector32.z);
						if (single4 < this._soundClips.attackDistance * this._soundClips.attackDistance)
						{
							if (this._nma.path != null)
							{
								this._nma.ResetPath();
							}
							this.CurLifeTime -= Time.deltaTime;
							base.transform.LookAt(vector33);
							if (this.CurLifeTime <= 0f)
							{
								this.CurLifeTime = this._soundClips.timeToHit;
								if (Defs.isSoundFX)
								{
									base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.bite);
								}
								if (this.target.CompareTag("Player"))
								{
									this.target.GetComponent<SkinName>().playerMoveC.minusLiveFromZombi((float)this._soundClips.damagePerHit, base.transform.position);
								}
								if (this.target.CompareTag("Turret"))
								{
									this.target.GetComponent<TurretController>().MinusLive((float)this._soundClips.damagePerHit, 0, new NetworkViewID());
								}
							}
							this.PlayZombieAttack();
						}
						else
						{
							this.timeToUpdateNavMesh -= Time.deltaTime;
							if (this.timeToUpdateNavMesh < 0f)
							{
								this._nma.SetDestination(vector33);
								this._nma.speed = this._soundClips.attackingSpeed * Mathf.Pow(1.05f, (float)GlobalGameController.AllLevelsCompleted);
								this.timeToUpdateNavMesh = 0.5f;
							}
							this.CurLifeTime = this._soundClips.timeToHit;
							this.PlayZombieRun();
						}
					}
					if (this.health <= 0f)
					{
						this.photonView.RPC("Death", PhotonTargets.All, new object[0]);
					}
				}
				else if (this.falling)
				{
					float single7 = 7f;
					Transform transforms = base.transform;
					float single8 = base.transform.position.x;
					Vector3 vector34 = base.transform.position;
					float single9 = vector34.y - single7 * Time.deltaTime;
					Vector3 vector35 = base.transform.position;
					transforms.position = new Vector3(single8, single9, vector35.z);
				}
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			UnityEngine.Debug.LogError("Cooperative mode failure.");
			UnityEngine.Debug.LogException(exception);
			throw;
		}
	}

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