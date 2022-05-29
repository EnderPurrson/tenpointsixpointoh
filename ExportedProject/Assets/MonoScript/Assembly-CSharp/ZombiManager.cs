using Rilisoft;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ZombiManager : MonoBehaviour
{
	public static ZombiManager sharedManager;

	public double timeGame;

	public float nextTimeSynch;

	public float nextAddZombi;

	public List<string> zombiePrefabs = new List<string>();

	private List<string[]> _enemies = new List<string[]>();

	private GameObject[] _enemyCreationZones;

	public bool startGame;

	public double maxTimeGame = 240;

	public PhotonView photonView;

	public bool isPizzaMap;

	public ZombiManager()
	{
	}

	private void addZombi()
	{
		GameObject gameObject = this._enemyCreationZones[UnityEngine.Random.Range(0, (int)this._enemyCreationZones.Length)];
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		float single = component.size.x * gameObject.transform.localScale.x;
		float single1 = component.size.z;
		Vector3 vector3 = gameObject.transform.localScale;
		Vector2 vector2 = new Vector2(single, single1 * vector3.z);
		Vector3 vector31 = gameObject.transform.position;
		float single2 = vector31.x - vector2.x / 2f;
		Vector3 vector32 = gameObject.transform.position;
		Rect rect = new Rect(single2, vector32.z - vector2.y / 2f, vector2.x, vector2.y);
		float single3 = rect.x + UnityEngine.Random.Range(0f, rect.width);
		Vector3 vector33 = gameObject.transform.position;
		Vector3 vector34 = new Vector3(single3, vector33.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		int num = 0;
		double num1 = this.timeGame / this.maxTimeGame * 100;
		if (!this.isPizzaMap)
		{
			if (num1 < 15)
			{
				num = UnityEngine.Random.Range(0, 3);
			}
			if (num1 >= 15 && num1 < 30)
			{
				num = UnityEngine.Random.Range(0, 5);
			}
			if (num1 >= 30 && num1 < 45)
			{
				num = UnityEngine.Random.Range(0, 6);
			}
			if (num1 >= 45 && num1 < 60)
			{
				num = UnityEngine.Random.Range(1, 8);
			}
			if (num1 >= 60 && num1 < 75)
			{
				num = UnityEngine.Random.Range(3, 10);
			}
			if (num1 >= 75)
			{
				num = UnityEngine.Random.Range(3, 11);
			}
		}
		else
		{
			if (num1 < 15)
			{
				num = UnityEngine.Random.Range(0, 4);
			}
			if (num1 >= 15 && num1 < 30)
			{
				num = UnityEngine.Random.Range(0, 5);
			}
			if (num1 >= 30 && num1 < 45)
			{
				num = UnityEngine.Random.Range(0, 6);
			}
			if (num1 >= 45 && num1 < 60)
			{
				num = UnityEngine.Random.Range(1, 7);
			}
			if (num1 >= 60 && num1 < 75)
			{
				num = UnityEngine.Random.Range(1, 9);
			}
			if (num1 >= 75)
			{
				num = UnityEngine.Random.Range(3, 11);
			}
		}
		PhotonNetwork.InstantiateSceneObject(this.zombiePrefabs[num], vector34, Quaternion.identity, 0, null);
	}

	private void addZombies(int count)
	{
		for (int i = 0; i < count; i++)
		{
			this.addZombi();
		}
	}

	private void Awake()
	{
		try
		{
			string[] strArrays = null;
			this.isPizzaMap = (SceneLoader.ActiveSceneName.Equals("Pizza") ? true : SceneLoader.ActiveSceneName.Equals("Paradise_Night"));
			strArrays = (!this.isPizzaMap ? new string[] { "1", "79", "2", "3", "57", "16", "56", "27", "73", "9", "39" } : new string[] { "86", "90", "88", "91", "84", "87", "82", "81", "92", "80", "83" });
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				string str = string.Concat("Enemies/Enemy", strArrays1[i], "_go");
				this.zombiePrefabs.Add(str);
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	public void EndMatch()
	{
		if (this.photonView.isMine)
		{
			if (TimeGameController.sharedController.isEndMatch)
			{
				return;
			}
			TimeGameController.sharedController.isEndMatch = true;
			this.startGame = false;
			this.timeGame = 0;
			GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("NetworkTable");
			float component = -100f;
			string empty = string.Empty;
			int num = 0;
			for (int i = 0; i < (int)gameObjectArray.Length; i++)
			{
				if ((float)gameObjectArray[i].GetComponent<NetworkStartTable>().score > component)
				{
					component = (float)gameObjectArray[i].GetComponent<NetworkStartTable>().score;
					empty = gameObjectArray[i].GetComponent<NetworkStartTable>().NamePlayer;
					num = i;
				}
			}
			this.photonView.RPC("win", PhotonTargets.All, new object[] { empty });
			this.photonView.RPC("WinID", PhotonTargets.All, new object[] { gameObjectArray[num].GetComponent<PhotonView>().ownerId });
		}
	}

	private void OnDestroy()
	{
		ZombiManager.sharedManager = null;
	}

	private void Start()
	{
		if (!Defs.isMulti || !Defs.isCOOP)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		ZombiManager.sharedManager = this;
		try
		{
			this.nextAddZombi = 5f;
			this._enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
			this.photonView = PhotonView.Get(this);
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	[PunRPC]
	[RPC]
	private void synchTime(float _time)
	{
	}

	private void Update()
	{
		try
		{
			int count = Initializer.players.Count;
			if (!this.startGame && count > 0)
			{
				this.startGame = true;
				this.timeGame = 0;
				this.nextTimeSynch = 0f;
				this.nextAddZombi = 0f;
			}
			if (this.startGame && count == 0)
			{
				this.startGame = false;
				this.timeGame = 0;
				this.nextTimeSynch = 0f;
				this.nextAddZombi = 0f;
			}
			if (this.startGame)
			{
				this.timeGame = this.maxTimeGame - TimeGameController.sharedController.timerToEndMatch;
				if (this.timeGame > (double)this.nextAddZombi && this.photonView.isMine && Initializer.enemiesObj.Count < 15)
				{
					float single = 4f;
					if (this.timeGame > this.maxTimeGame * 0.4000000059604645)
					{
						single = 3f;
					}
					if (this.timeGame > this.maxTimeGame * 0.800000011920929)
					{
						single = 2f;
					}
					this.nextAddZombi += single;
					int num = Initializer.players.Count - (Application.loadedLevelName != "Arena" ? 1 : 2);
					num = Mathf.Clamp(num, 1, 15);
					Debug.LogWarning(string.Concat(">>> ZOMBIE COUNT ", num));
					this.addZombies(num);
				}
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	[PunRPC]
	[RPC]
	private void win(string _winer)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.win(_winer, 0, 0, 0);
		}
	}

	[PunRPC]
	[RPC]
	private void WinID(int winID)
	{
		WeaponManager weaponManager = WeaponManager.sharedManager;
		if (weaponManager.myTable != null && weaponManager.myTable.GetComponent<PhotonView>().ownerId == winID)
		{
			int num = Storager.getInt("Rating", false) + 1;
			Storager.setInt("Rating", num, false);
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.TryIncrementWinCountTimestamp();
			}
		}
	}
}