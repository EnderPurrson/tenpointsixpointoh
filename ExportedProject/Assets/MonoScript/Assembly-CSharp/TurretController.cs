using Rilisoft;
using RilisoftBot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using ZeichenKraftwerk;

public sealed class TurretController : MonoBehaviour
{
	public AudioClip turretActivSound;

	public AudioClip turretDeadSound;

	public AudioClip musicDater;

	public SkinnedMeshRenderer turretRenderer;

	public SkinnedMeshRenderer turretExplosionRenderer;

	[NonSerialized]
	public int numUpdate;

	public Rotator rotator;

	private float maxSpeedRotator = -1000f;

	private float downSpeedRotator = 500f;

	public Material[] turretRunMaterials;

	public Material musicBoxMaterial;

	public GameObject hitObj;

	public GameObject killedParticle;

	public GameObject explosionAnimObj;

	public GameObject turretObj;

	public float damage = 1f;

	public float[] damageMultyByTier = new float[] { 0.1f, 0.3f, 0.5f, 0.7f, 1f };

	public float[] healthMultyByTier = new float[] { 20f, 40f, 60f, 80f, 100f };

	public Transform tower;

	public Transform gun;

	public AudioClip shotClip;

	public ParticleSystem gunFlash1;

	public ParticleSystem gunFlash2;

	public Transform healthBar;

	public float health = 10000000f;

	public float maxHealth = 10000000f;

	public bool isRunAvailable;

	public bool isRun;

	public GameObject myPlayer;

	public Player_move_c myPlayerMoveC;

	private bool isStartSynh;

	public Transform myTransform;

	public PhotonView photonView;

	private bool isMine;

	public Transform spherePoint;

	public Transform rayGroundPoint;

	public BoxCollider myCollider;

	public Transform target;

	public GameObject isEnemySprite;

	public Transform shotPoint;

	public Transform shotPoint2;

	public bool isKilled;

	public bool isEnemyTurret;

	public int myCommand;

	public NickLabelController myLabel;

	public TextMesh nickLabel;

	private float speedRotateY = 220f;

	private float speedRotateX = 30f;

	private float speedFire = 3f;

	private float radiusZoneMeele = 10f;

	private float radiusZoneMeeleSyrvival = 4f;

	public float maxRadiusScanTarget = 30f;

	private float maxRadiusScanTargetSQR;

	private float idleAlphaY;

	private float idleAlphaX;

	private float idleRotateSpeedX = 20f;

	private float idleRotateSpeedY = 20f;

	private float maxDeltaRotateY = 60f;

	private float maxRotateX = 75f;

	private float minRotateX = -60f;

	private float timerScanTarget = -1f;

	private float maxTimerScanTarget = 1f;

	private float timerScanTargetIdle = -1f;

	private float maxTimerScanTargetIdle = 0.5f;

	private float timerShot;

	private float maxTimerShot = 0.1f;

	private float dissipation = 0.015f;

	public bool dontExecStart;

	private bool inScaning;

	private Rigidbody rigidBody;

	private Vector3 turretMinPos = new Vector3(0f, Single.MaxValue, 0f);

	public GameObject redMark;

	public GameObject blueMark;

	private bool isPlayMusicDater;

	private int _nickColorInd;

	private bool _isSetNickLabelText;

	public NetworkView networkView
	{
		get;
		set;
	}

	public TurretController()
	{
	}

	private void Awake()
	{
		this.photonView = PhotonView.Get(this);
		this.networkView = base.GetComponent<NetworkView>();
		if (this.photonView && this.photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		this.maxRadiusScanTargetSQR = this.maxRadiusScanTarget * this.maxRadiusScanTarget;
		this.rigidBody = base.transform.GetComponent<Rigidbody>();
	}

	[Obfuscation(Exclude=true)]
	private void DestroyTurrel()
	{
		if (!Defs.isMulti)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (!this.isMine)
		{
			base.transform.position = new Vector3(-1000f, -1000f, -1000f);
		}
		else if (Defs.isInet)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			Network.RemoveRPCs(base.GetComponent<NetworkView>().viewID);
			Network.Destroy(base.gameObject);
		}
	}

	[DebuggerHidden]
	private IEnumerator FlashRed()
	{
		TurretController.u003cFlashRedu003ec__Iterator1DB variable = null;
		return variable;
	}

	private List<GameObject> GetAllTargets()
	{
		bool flag = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture ? true : ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
		int count = 0;
		if (!Defs.isMulti || Defs.isCOOP)
		{
			count += Initializer.enemiesObj.Count;
		}
		else
		{
			count += Initializer.players.Count;
			count += Initializer.turretsObj.Count;
		}
		List<GameObject> gameObjects = new List<GameObject>(count);
		if (!Defs.isMulti || Defs.isCOOP)
		{
			for (int i = 0; i < Initializer.enemiesObj.Count; i++)
			{
				gameObjects.Add(Initializer.enemiesObj[i].transform.GetChild(0).gameObject);
			}
		}
		else
		{
			if (!flag)
			{
				for (int j = 0; j < Initializer.players.Count; j++)
				{
					if (!Initializer.players[j].isInvisible && !Initializer.players[j].isKilled)
					{
						gameObjects.Add(Initializer.players[j].mySkinName.gameObject);
					}
				}
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 1)
			{
				for (int k = 0; k < Initializer.bluePlayers.Count; k++)
				{
					if (!Initializer.bluePlayers[k].isInvisible && !Initializer.bluePlayers[k].isKilled)
					{
						gameObjects.Add(Initializer.bluePlayers[k].mySkinName.gameObject);
					}
				}
			}
			else
			{
				for (int l = 0; l < Initializer.redPlayers.Count; l++)
				{
					if (!Initializer.redPlayers[l].isInvisible && !Initializer.redPlayers[l].isKilled)
					{
						gameObjects.Add(Initializer.redPlayers[l].mySkinName.gameObject);
					}
				}
			}
			for (int m = 0; m < Initializer.turretsObj.Count; m++)
			{
				if (Initializer.turretsObj[m].GetComponent<TurretController>().isEnemyTurret)
				{
					gameObjects.Add(Initializer.turretsObj[m]);
				}
			}
		}
		return gameObjects;
	}

	private float GetDeltaAngles(float angle1, float angle2)
	{
		if (angle1 < 0f)
		{
			angle1 += 360f;
		}
		if (angle2 < 0f)
		{
			angle2 += 360f;
		}
		float single = angle1 - angle2;
		if (Mathf.Abs(single) > 180f)
		{
			if (angle1 <= angle2)
			{
				single += 360f;
			}
			else
			{
				single -= 360f;
			}
		}
		return single;
	}

	public Vector3 GetHeadPoint()
	{
		Vector3 vector3 = base.transform.position;
		float single = vector3.y;
		Vector3 vector31 = this.myCollider.size;
		vector3.y = single + (vector31.y - 0.5f);
		return vector3;
	}

	private void HitZombie(GameObject zmb)
	{
		BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(zmb.transform.parent);
		if (!Defs.isMulti)
		{
			botScriptForObject.GetDamage(-this.damage, this.myTransform, true, false);
		}
		else if (Defs.isCOOP && !botScriptForObject.IsDeath)
		{
			botScriptForObject.GetDamageForMultiplayer(-this.damage, null, false);
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().score = GlobalGameController.Score;
			WeaponManager.sharedManager.myNetworkStartTable.SynhScore();
		}
	}

	[PunRPC]
	[RPC]
	public void ImKilledRPC()
	{
		this.ImKilledRPCWithExplosion(false);
	}

	[PunRPC]
	[RPC]
	public void ImKilledRPCWithExplosion(bool isExplosion)
	{
		this.isKilled = true;
		this.nickLabel.gameObject.SetActive(false);
		isExplosion = true;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.turretDeadSound);
		}
		if (!isExplosion)
		{
			this.killedParticle.SetActive(true);
		}
		else
		{
			this.explosionAnimObj.SetActive(true);
			this.turretObj.SetActive(false);
		}
		base.Invoke("DestroyTurrel", 2f);
	}

	public void MeKill(string _nick)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && this.myPlayer != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(_nick, 9, this.myPlayer.GetComponent<SkinName>().NickName, Color.white, null);
		}
	}

	[PunRPC]
	[RPC]
	public void MeKillRPC(int idKillerPhoton)
	{
		string empty = string.Empty;
		foreach (Player_move_c player in Initializer.players)
		{
			if (!(player.mySkinName.photonView != null) || player.mySkinName.photonView.viewID != idKillerPhoton)
			{
				continue;
			}
			empty = player.mySkinName.NickName;
			if (player.Equals(WeaponManager.sharedManager.myPlayerMoveC))
			{
				WeaponManager.sharedManager.myPlayerMoveC.ImKill(idKillerPhoton, 9);
			}
			break;
		}
		this.MeKill(empty);
	}

	[PunRPC]
	[RPC]
	public void MeKillRPCLocal(NetworkViewID idKillerLocal)
	{
		string empty = string.Empty;
		foreach (Player_move_c player in Initializer.players)
		{
			if (!(player.mySkinName.GetComponent<NetworkView>() != null) || !player.mySkinName.GetComponent<NetworkView>().viewID.Equals(idKillerLocal))
			{
				continue;
			}
			empty = player.mySkinName.NickName;
			if (player.Equals(WeaponManager.sharedManager.myPlayerMoveC))
			{
				WeaponManager.sharedManager.myPlayerMoveC.ImKill(idKillerLocal, 9);
			}
			break;
		}
		this.MeKill(empty);
	}

	public void MinusLive(float dm, int idKillerPhoton = 0, NetworkViewID idKillerLocal = default(NetworkViewID))
	{
		this.MinusLive(dm, false, idKillerPhoton, idKillerLocal);
	}

	public void MinusLive(float dm, bool isExplosion, int idKillerPhoton = 0, NetworkViewID idKillerLocal = default(NetworkViewID))
	{
		if (Defs.isDaterRegim || !this.isRun)
		{
			return;
		}
		if (!Defs.isMulti)
		{
			this.MinusLiveReal(dm, isExplosion, 0, new NetworkViewID());
		}
		else
		{
			this.health -= dm;
			if (this.health < 0f)
			{
				this.ImKilledRPCWithExplosion(isExplosion);
				dm = 10000f;
			}
			if (Defs.isInet)
			{
				this.photonView.RPC("MinusLiveRPC", PhotonTargets.All, new object[] { dm, isExplosion, idKillerPhoton });
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("MinusLiveRPCLocal", RPCMode.All, new object[] { dm, isExplosion, idKillerLocal });
			}
		}
	}

	public void MinusLiveReal(float dm, bool isExplosion, int idKillerPhoton = 0, NetworkViewID idKillerLocal = default(NetworkViewID))
	{
		base.StopCoroutine(this.FlashRed());
		base.StartCoroutine(this.FlashRed());
		if (this.isKilled)
		{
			return;
		}
		if (Defs.isMulti && !this.isMine)
		{
			return;
		}
		this.health -= dm;
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("SynchHealth", PhotonTargets.Others, new object[] { this.health });
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SynchHealth", RPCMode.Others, new object[] { this.health });
			}
		}
		if (this.health < 0f)
		{
			this.health = 0f;
			if (!Defs.isMulti)
			{
				this.ImKilledRPCWithExplosion(isExplosion);
			}
			else if (Defs.isInet)
			{
				this.photonView.RPC("ImKilledRPCWithExplosion", PhotonTargets.AllBuffered, new object[] { isExplosion });
				this.photonView.RPC("MeKillRPC", PhotonTargets.All, new object[] { idKillerPhoton });
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("ImKilledRPCWithExplosion", RPCMode.AllBuffered, new object[] { isExplosion });
				base.GetComponent<NetworkView>().RPC("MeKillRPCLocal", RPCMode.All, new object[] { idKillerLocal });
			}
		}
	}

	[PunRPC]
	[RPC]
	public void MinusLiveRPC(float dm, int idKillerPhoton)
	{
		this.MinusLiveReal(dm, false, idKillerPhoton, new NetworkViewID());
	}

	[PunRPC]
	[RPC]
	public void MinusLiveRPC(float dm, bool isExplosion, int idKillerPhoton)
	{
		this.MinusLiveReal(dm, isExplosion, idKillerPhoton, new NetworkViewID());
	}

	[PunRPC]
	[RPC]
	public void MinusLiveRPCLocal(float dm, bool isExplosion, NetworkViewID idKillerLocal)
	{
		this.MinusLiveReal(dm, isExplosion, 0, idKillerLocal);
	}

	private void OnCollisionEnter(Collision col)
	{
		if (this.isMine && col.gameObject.name == "DeadCollider")
		{
			this.MinusLive(1000f, 0, new NetworkViewID());
			base.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	private void OnDestroy()
	{
		if (!Defs.isMulti || this.isMine)
		{
			PotionsController.sharedController.DeActivePotion(GearManager.Turret, null, false);
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
		Initializer.turretsObj.Remove(base.gameObject);
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.isMine)
		{
			this.photonView.RPC("SynchHealth", player, new object[] { this.health });
			if (Defs.isDaterRegim)
			{
				this.photonView.RPC("PlayMusic", player, new object[] { this.isPlayMusicDater });
			}
		}
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (this.isMine)
		{
			this.networkView.RPC("SynchHealth", player, new object[] { this.health });
			if (Defs.isDaterRegim)
			{
				this.networkView.RPC("PlayMusic", player, new object[] { this.isPlayMusicDater });
			}
		}
	}

	[PunRPC]
	[RPC]
	private void PlayMusic(bool isPlay)
	{
		if (this.isPlayMusicDater == isPlay)
		{
			return;
		}
		this.isPlayMusicDater = isPlay;
		if (!isPlay)
		{
			base.GetComponent<AudioSource>().Stop();
		}
		else if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().loop = true;
			base.GetComponent<AudioSource>().clip = this.musicDater;
			base.GetComponent<AudioSource>().Play();
		}
	}

	[DebuggerHidden]
	private IEnumerator ScanTarget()
	{
		TurretController.u003cScanTargetu003ec__Iterator1DA variable = null;
		return variable;
	}

	private Transform ScanTargetObs()
	{
		GameObject[] array;
		RaycastHit raycastHit;
		int num;
		GameObject[] gameObjectArray = null;
		if ((!Defs.isMulti || !Defs.isCOOP) && Defs.isMulti)
		{
			array = GameObject.FindGameObjectsWithTag("HeadCollider");
			gameObjectArray = Initializer.turretsObj.ToArray();
		}
		else
		{
			array = Initializer.enemiesObj.ToArray();
			for (int i = 0; i < (int)array.Length; i++)
			{
				array[i] = array[i].transform.GetChild(0).gameObject;
			}
		}
		float single = 1E+09f;
		Transform transforms = null;
		GameObject[] gameObjectArray1 = new GameObject[0 + (array == null ? 0 : (int)array.Length) + (gameObjectArray == null ? 0 : (int)gameObjectArray.Length)];
		if (array != null)
		{
			array.CopyTo(gameObjectArray1, 0);
		}
		if (gameObjectArray != null)
		{
			GameObject[] gameObjectArray2 = gameObjectArray;
			GameObject[] gameObjectArray3 = gameObjectArray1;
			num = (array == null ? 0 : (int)array.Length);
			((Array)gameObjectArray2).CopyTo((Array)gameObjectArray3, num);
		}
		for (int j = 0; j < (int)gameObjectArray1.Length; j++)
		{
			if (!Defs.isMulti || Defs.isMulti && Defs.isCOOP || gameObjectArray1[j].transform.parent != null && gameObjectArray1[j].transform.parent.gameObject.CompareTag("Player") && (!gameObjectArray1[j].transform.parent.gameObject.Equals(WeaponManager.sharedManager.myPlayer) || Defs.isDaterRegim) && (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints || gameObjectArray1[j].transform.parent.GetComponent<SkinName>().playerMoveC.myCommand != WeaponManager.sharedManager.myPlayerMoveC.myCommand) && !gameObjectArray1[j].transform.parent.GetComponent<SkinName>().playerMoveC.isKilled && gameObjectArray1[j].transform.position.y > -500f && !gameObjectArray1[j].transform.parent.GetComponent<SkinName>().playerMoveC.isInvisible || gameObjectArray1[j].CompareTag("Turret") && gameObjectArray1[j].GetComponent<TurretController>().isEnemyTurret)
			{
				Transform transforms1 = gameObjectArray1[j].transform;
				float single1 = Vector3.SqrMagnitude(transforms1.position - this.myTransform.position);
				if (single1 < this.maxRadiusScanTargetSQR && (Defs.isDaterRegim || Mathf.Acos(Mathf.Sqrt((transforms1.position.x - this.myTransform.position.x) * (transforms1.position.x - this.myTransform.position.x) + (transforms1.position.z - this.myTransform.position.z) * (transforms1.position.z - this.myTransform.position.z)) / Vector3.Distance(transforms1.position, this.myTransform.position)) * 180f / 3.1415927f < this.maxRotateX))
				{
					if (Defs.isDaterRegim)
					{
						return transforms1;
					}
					Vector3 vector3 = Vector3.zero;
					BoxCollider component = transforms1.GetComponent<BoxCollider>();
					if (component != null)
					{
						vector3 = component.center;
					}
					Ray ray = new Ray(this.tower.position, (transforms1.position + vector3) - this.tower.position);
					bool flag = Physics.Raycast(ray, out raycastHit, this.maxRadiusScanTarget, Tools.AllWithoutDamageCollidersMask);
					Transform transforms2 = raycastHit.transform;
					if (flag && (raycastHit.collider.gameObject.transform.Equals(transforms1) || raycastHit.collider.gameObject.transform.parent != null && (raycastHit.collider.gameObject.transform.parent.Equals(transforms1) || raycastHit.collider.gameObject.transform.parent.Equals(transforms1.parent))))
					{
						single = single1;
						transforms = transforms1;
					}
				}
			}
		}
		return transforms;
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
				this.myPlayer = Initializer.players[num].mySkinName.gameObject;
				this.myPlayerMoveC = this.myPlayer.GetComponent<SkinName>().playerMoveC;
				break;
			}
		}
	}

	[Obfuscation(Exclude=true)]
	private void SetNoUseGravityInvoke()
	{
		this.rigidBody.useGravity = false;
		this.rigidBody.isKinematic = true;
		base.transform.GetComponent<BoxCollider>().isTrigger = true;
	}

	private void SetStateIsEnemyTurret()
	{
		bool flag = this.isEnemyTurret;
		this.isEnemyTurret = false;
		if (Defs.isMulti && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints))
		{
			if (this.myPlayer != null && WeaponManager.sharedManager.myPlayerMoveC != null && this.myPlayer.GetComponent<SkinName>().playerMoveC.myCommand != WeaponManager.sharedManager.myPlayerMoveC.myCommand)
			{
				this.isEnemyTurret = true;
			}
		}
		else if (Defs.isMulti && !this.isMine)
		{
			this.isEnemyTurret = true;
		}
	}

	[PunRPC]
	[RPC]
	public void ShotRPC()
	{
		RaycastHit raycastHit;
		if (this.shotPoint2 == null || this.shotPoint == null)
		{
			return;
		}
		if (this.rotator != null)
		{
			this.rotator.eulersPerSecond = new Vector3(0f, 0f, this.maxSpeedRotator);
		}
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.shotClip);
		}
		if (this.gunFlash1 != null)
		{
			this.gunFlash1.enableEmission = true;
			this.gunFlash1.Play();
		}
		base.CancelInvoke("StopGunFlash");
		base.Invoke("StopGunFlash", this.maxTimerShot * 1.1f);
		if (Defs.isMulti && !this.isMine)
		{
			return;
		}
		float single = this.shotPoint2.position.x;
		Vector3 vector3 = this.shotPoint.position;
		float single1 = single - vector3.x + UnityEngine.Random.Range(-this.dissipation, this.dissipation);
		float single2 = this.shotPoint2.position.y;
		Vector3 vector31 = this.shotPoint.position;
		float single3 = single2 - vector31.y + UnityEngine.Random.Range(-this.dissipation, this.dissipation);
		float single4 = this.shotPoint2.position.z;
		Vector3 vector32 = this.shotPoint.position;
		Vector3 vector33 = new Vector3(single1, single3, single4 - vector32.z + UnityEngine.Random.Range(-this.dissipation, this.dissipation));
		Ray ray = new Ray(this.shotPoint.position, vector33);
		if (Physics.Raycast(ray, out raycastHit, 100f, Tools.AllWithoutDamageCollidersMask) && (!Defs.isMulti || WeaponManager.sharedManager.myPlayer != null))
		{
			bool flag = false;
			if ((!Defs.isMulti || !Defs.isCOOP) && Defs.isMulti)
			{
				if (raycastHit.collider.gameObject.transform.parent != null && raycastHit.collider.gameObject.transform.parent.CompareTag("Player") && raycastHit.collider.gameObject.transform.parent.gameObject != WeaponManager.sharedManager.myPlayer)
				{
					Player_move_c component = raycastHit.collider.gameObject.transform.parent.GetComponent<SkinName>().playerMoveC;
					float single5 = this.damageMultyByTier[this.numUpdate];
					if (!Defs.isInet)
					{
						component.MinusLive(WeaponManager.sharedManager.myPlayer.GetComponent<NetworkView>().viewID, single5, Player_move_c.TypeKills.turret, 0, string.Empty, base.GetComponent<NetworkView>().viewID);
					}
					else
					{
						component.MinusLive(WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID, single5, Player_move_c.TypeKills.turret, 0, string.Empty, this.photonView.viewID);
					}
					flag = true;
				}
				if (raycastHit.collider.gameObject != null && raycastHit.collider.gameObject.CompareTag("Turret"))
				{
					TurretController turretController = raycastHit.collider.gameObject.GetComponent<TurretController>();
					float single6 = this.damageMultyByTier[this.numUpdate];
					if (!Defs.isInet)
					{
						turretController.MinusLive(single6, 0, WeaponManager.sharedManager.myPlayer.GetComponent<NetworkView>().viewID);
					}
					else
					{
						turretController.MinusLive(single6, WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID, new NetworkViewID());
					}
					flag = true;
				}
			}
			else
			{
				this.hitObj = raycastHit.collider.gameObject;
				if (raycastHit.collider.gameObject.transform.parent != null && raycastHit.collider.gameObject.transform.parent.CompareTag("Enemy"))
				{
					this.HitZombie(raycastHit.collider.gameObject);
					flag = true;
				}
			}
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					WeaponManager.sharedManager.myPlayerMoveC.photonView.RPC("HoleRPC", PhotonTargets.All, new object[] { flag, raycastHit.point + (raycastHit.normal * 0.001f), Quaternion.FromToRotation(Vector3.up, raycastHit.normal) });
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.GetComponent<NetworkView>().RPC("HoleRPC", RPCMode.All, new object[] { flag, raycastHit.point + (raycastHit.normal * 0.001f), Quaternion.FromToRotation(Vector3.up, raycastHit.normal) });
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		TurretController.u003cStartu003ec__Iterator1D9 variable = null;
		return variable;
	}

	public void StartTurret()
	{
		if (Defs.isMulti && this.isMine)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("StartTurretRPC", PhotonTargets.AllBuffered, new object[0]);
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("StartTurretRPC", RPCMode.AllBuffered, new object[0]);
			}
		}
		else if (!Defs.isMulti)
		{
			this.StartTurretRPC();
		}
		this.myCollider.enabled = true;
		this.rigidBody.isKinematic = false;
		this.rigidBody.useGravity = true;
		base.Invoke("SetNoUseGravityInvoke", 5f);
	}

	[PunRPC]
	[RPC]
	public void StartTurretRPC()
	{
		this.nickLabel.gameObject.SetActive(true);
		this.myCollider.enabled = true;
		base.transform.parent = null;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.turretActivSound);
		}
		Player_move_c.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Default"));
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().stateSynchronization = NetworkStateSynchronization.ReliableDeltaCompressed;
		}
		else
		{
			this.photonView.synchronization = ViewSynchronization.UnreliableOnChange;
		}
		this.isRun = true;
		if (Defs.isDaterRegim)
		{
			this.turretRenderer.material = this.musicBoxMaterial;
		}
		else
		{
			this.turretRenderer.material = this.turretRunMaterials[this.numUpdate];
		}
		if (this.turretExplosionRenderer != null)
		{
			this.turretExplosionRenderer.material = this.turretRunMaterials[this.numUpdate];
		}
	}

	[Obfuscation(Exclude=true)]
	private void StopGunFlash()
	{
		this.gunFlash1.enableEmission = false;
	}

	[PunRPC]
	[RPC]
	public void SynchHealth(float _health)
	{
		if (this.health > _health)
		{
			this.health = _health;
		}
	}

	[PunRPC]
	[RPC]
	public void SynchNumUpdateRPC(int _numUpdate)
	{
		this.numUpdate = _numUpdate;
		if (Defs.isMulti && !Defs.isCOOP)
		{
			this.maxHealth = this.healthMultyByTier[_numUpdate];
			this.health = this.maxHealth;
		}
	}

	private void Update()
	{
		RaycastHit raycastHit;
		if (this.dontExecStart)
		{
			return;
		}
		if (Defs.isMulti && this.myPlayerMoveC != null && !this._isSetNickLabelText)
		{
			this.nickLabel.text = FilterBadWorld.FilterString(this.myPlayerMoveC.mySkinName.NickName);
		}
		this.UpdateNickLabelColor();
		if (this.isRun && this.healthBar != null)
		{
			this.healthBar.localScale = new Vector3((this.health <= 0f ? 0f : this.health / this.maxHealth), 1f, 1f);
		}
		this.SetStateIsEnemyTurret();
		if (this.isEnemySprite != null && this.isEnemySprite.activeSelf != this.isEnemyTurret)
		{
			this.isEnemySprite.SetActive(this.isEnemyTurret);
		}
		if (this.rotator != null && this.rotator.eulersPerSecond.z < -200f)
		{
			this.rotator.eulersPerSecond = new Vector3(0f, 0f, this.rotator.eulersPerSecond.z + this.downSpeedRotator * Time.deltaTime);
		}
		if (Defs.isDaterRegim && this.isPlayMusicDater)
		{
			this.tower.Rotate(new Vector3(0f, 0f, 180f * Time.deltaTime));
			this.gun.Rotate(new Vector3(180f * Time.deltaTime, 0f, 0f));
		}
		if (Defs.isMulti && !this.isMine)
		{
			return;
		}
		if (Defs.isMulti && this.isMine && WeaponManager.sharedManager.myPlayer == null)
		{
			if (Defs.isInet)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
			else
			{
				Network.Destroy(base.gameObject);
			}
			return;
		}
		if (!this.isRun)
		{
			Ray ray = new Ray(this.rayGroundPoint.position, Vector3.down);
			if (!Physics.Raycast(ray, out raycastHit, 1f, Tools.AllWithoutDamageCollidersMask) || raycastHit.distance <= 0.05f || raycastHit.distance >= 0.7f || Physics.CheckSphere(this.spherePoint.position, 1.1f, Tools.AllWithoutMyPlayerMask))
			{
				this.isRunAvailable = false;
				this.turretRenderer.materials[0].SetColor("_TintColor", new Color(1f, 0f, 0f, 0.08f));
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.runTurrelButton.GetComponent<UIButton>().isEnabled = false;
				}
			}
			else
			{
				this.isRunAvailable = true;
				this.turretRenderer.materials[0].SetColor("_TintColor", new Color(1f, 1f, 1f, 0.08f));
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.runTurrelButton.GetComponent<UIButton>().isEnabled = true;
				}
			}
			return;
		}
		if (this.isKilled)
		{
			if (this.gun.rotation.x > this.minRotateX)
			{
				this.gun.Rotate(this.speedRotateX * Time.deltaTime, 0f, 0f);
			}
			return;
		}
		if (this.target != null && (this.target.position.y < -500f || !Defs.isDaterRegim && this.target.CompareTag("Player") && this.target.GetComponent<SkinName>().playerMoveC.isInvisible))
		{
			this.target = null;
		}
		if (this.target == null)
		{
			if (Defs.isDaterRegim && this.isPlayMusicDater)
			{
				this.PlayMusic(false);
				if (Defs.isInet)
				{
					this.photonView.RPC("PlayMusic", PhotonTargets.Others, new object[] { false });
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("PlayMusic", RPCMode.Others, new object[] { false });
				}
			}
			this.timerScanTargetIdle -= Time.deltaTime;
			if (this.timerScanTargetIdle < 0f)
			{
				this.timerScanTargetIdle = this.maxTimerScanTargetIdle;
				if (!this.inScaning)
				{
					base.StartCoroutine(this.ScanTarget());
				}
			}
			if (Defs.isDaterRegim)
			{
				return;
			}
			if (Mathf.Abs(this.idleAlphaY) >= 0.5f)
			{
				float single = Time.deltaTime * this.idleRotateSpeedY * Mathf.Abs(this.idleAlphaY) / this.idleAlphaY;
				this.idleAlphaY -= single;
				Transform transforms = this.tower;
				Vector3 vector3 = this.tower.localRotation.eulerAngles;
				transforms.localRotation = Quaternion.Euler(new Vector3(0f, vector3.y + single, 0f));
			}
			else
			{
				this.idleAlphaY = UnityEngine.Random.Range(-1f * this.maxDeltaRotateY / 2f, this.maxDeltaRotateY / 2f);
			}
			if (Mathf.Abs(this.gun.localRotation.eulerAngles.x) > 1f)
			{
				this.gun.Rotate((float)((this.gun.localRotation.eulerAngles.x >= 180f ? 1 : -1)) * this.speedRotateX * Time.deltaTime, 0f, 0f);
			}
			return;
		}
		if (!Defs.isDaterRegim)
		{
			bool flag = false;
			Vector2 vector2 = new Vector2(this.target.position.x, this.target.position.z);
			float single1 = this.tower.position.x;
			Vector3 vector31 = this.tower.position;
			Vector2 vector21 = vector2 - new Vector2(single1, vector31.z);
			Vector3 vector32 = this.tower.rotation.eulerAngles;
			float deltaAngles = this.GetDeltaAngles(vector32.y, Mathf.Abs(vector21.x) / vector21.x * Vector2.Angle(Vector2.up, vector21));
			float single2 = -this.speedRotateY * Time.deltaTime * Mathf.Abs(deltaAngles) / deltaAngles;
			if (Mathf.Abs(deltaAngles) < 10f)
			{
				flag = true;
			}
			if (Mathf.Abs(single2) > Mathf.Abs(deltaAngles))
			{
				single2 = -deltaAngles;
			}
			if (Mathf.Abs(single2) > 0.001f)
			{
				this.tower.Rotate(0f, single2, 0f);
			}
			Vector3 vector33 = Vector3.zero;
			BoxCollider component = this.target.GetComponent<BoxCollider>();
			if (component != null)
			{
				vector33 = component.center;
			}
			float single3 = this.target.position.y + vector33.y;
			Vector3 vector34 = this.tower.position;
			float single4 = -180f * Mathf.Atan((single3 - vector34.y) / Vector3.Distance(this.target.position + vector33, this.myTransform.position)) / 3.1415927f;
			Vector3 vector35 = this.gun.rotation.eulerAngles;
			float deltaAngles1 = this.GetDeltaAngles(vector35.x, single4);
			single2 = -this.speedRotateX * Time.deltaTime * Mathf.Abs(deltaAngles1) / deltaAngles1;
			if (Mathf.Abs(single2) > Mathf.Abs(deltaAngles1))
			{
				single2 = -deltaAngles1;
			}
			if (single2 > 0f && this.gun.rotation.x > this.maxRotateX)
			{
				single2 = 0f;
			}
			if (single2 < 0f && this.gun.rotation.x < this.minRotateX)
			{
				single2 = 0f;
			}
			if (Mathf.Abs(single2) > 0.001f)
			{
				this.gun.Rotate(single2, 0f, 0f);
			}
			if (flag)
			{
				this.timerShot -= Time.deltaTime;
				if (this.timerShot < 0f)
				{
					if (!Defs.isMulti)
					{
						this.ShotRPC();
					}
					else if (Defs.isInet)
					{
						this.photonView.RPC("ShotRPC", PhotonTargets.All, new object[0]);
					}
					else
					{
						base.GetComponent<NetworkView>().RPC("ShotRPC", RPCMode.All, new object[0]);
					}
					this.timerShot = this.maxTimerShot;
				}
			}
		}
		else if (Defs.isDaterRegim && !this.isPlayMusicDater)
		{
			this.PlayMusic(true);
			if (Defs.isInet)
			{
				this.photonView.RPC("PlayMusic", PhotonTargets.Others, new object[] { true });
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("PlayMusic", RPCMode.Others, new object[] { true });
			}
		}
		this.timerScanTargetIdle -= Time.deltaTime;
		if (this.timerScanTargetIdle < 0f)
		{
			this.timerScanTargetIdle = this.maxTimerScanTargetIdle;
			if (!this.inScaning)
			{
				base.StartCoroutine(this.ScanTarget());
			}
		}
		if (!this.rigidBody.isKinematic)
		{
			if (base.transform.position.y >= this.turretMinPos.y)
			{
				base.transform.position = this.turretMinPos;
			}
			else
			{
				this.turretMinPos = base.transform.position;
			}
		}
	}

	private void UpdateNickLabelColor()
	{
		if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			if (Defs.isDaterRegim)
			{
				if (this._nickColorInd != 0)
				{
					this.nickLabel.color = Color.white;
					this._nickColorInd = 0;
				}
			}
			else if (!Defs.isCOOP && (this.myPlayerMoveC == null || WeaponManager.sharedManager.myPlayerMoveC != this.myPlayerMoveC))
			{
				if (this._nickColorInd != 2)
				{
					this.nickLabel.color = Color.red;
					this._nickColorInd = 2;
				}
			}
			else if (this._nickColorInd != 1)
			{
				this.nickLabel.color = Color.blue;
				this._nickColorInd = 1;
			}
		}
		else if (WeaponManager.sharedManager.myPlayerMoveC == null || this.myPlayerMoveC == null)
		{
			if (this._nickColorInd != 0)
			{
				this.nickLabel.color = Color.white;
				this._nickColorInd = 0;
			}
		}
		else if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == this.myPlayerMoveC.myCommand)
		{
			if (this._nickColorInd != 1)
			{
				this.nickLabel.color = Color.blue;
				this._nickColorInd = 1;
			}
		}
		else if (this._nickColorInd != 2)
		{
			this.nickLabel.color = Color.red;
			this._nickColorInd = 2;
		}
	}
}