using ExitGames.Client.Photon;
using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BonusController : MonoBehaviour
{
	public static BonusController sharedController;

	public List<int> lowLevelPlayers = new List<int>();

	public GameObject bonusPrefab;

	public BonusItem[] bonusStack;

	private float creationInterval = 7f;

	public float timerToAddBonus;

	private bool isMulti;

	private bool isInet;

	private bool isStopCreateBonus;

	public bool isBeginCreateBonuses;

	private WeaponManager _weaponManager;

	private GameObject[] bonusCreationZones;

	private ZombieCreator zombieCreator;

	private PhotonView photonView;

	public int maxCountBonus = 5;

	private int activeBonusesCount;

	private int sumProbabilitys;

	private Dictionary<int, int> probabilityBonusDict = new Dictionary<int, int>();

	private Dictionary<int, Dictionary<string, int>> probabilityBonus = new Dictionary<int, Dictionary<string, int>>();

	private NetworkView _networkView
	{
		get;
		set;
	}

	public BonusController()
	{
	}

	private void AddBonus(Vector3 pos, int _type)
	{
		GameObject gameObject;
		if (Defs.isMulti && Defs.isInet && this.lowLevelPlayers.Count > 0 && (_type == 5 || _type == 8 || _type == 7 || _type == 6))
		{
			return;
		}
		if (!this.isMulti)
		{
			int enemiesToKill = GlobalGameController.EnemiesToKill - this.zombieCreator.NumOfDeadZombies;
			if (!Defs.IsSurvival && enemiesToKill <= 0 && !this.zombieCreator.bossShowm || Defs.IsSurvival && this.zombieCreator.stopGeneratingBonuses)
			{
				if (!Defs.IsSurvival)
				{
					this.isStopCreateBonus = true;
				}
				return;
			}
		}
		if (_type == 9)
		{
			if (!this.CanSpawnGemBonus())
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable["SpecialBonus"] = PhotonNetwork.time + 480;
			PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
		}
		int num = -1;
		if (pos.Equals(Vector3.zero))
		{
			GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Chest");
			if (this.activeBonusesCount + (int)gameObjectArray.Length >= this.maxCountBonus)
			{
				return;
			}
			num = UnityEngine.Random.Range(0, (int)this.bonusCreationZones.Length);
			bool[] flagArray = new bool[(int)this.bonusCreationZones.Length];
			for (int i = 0; i < (int)flagArray.Length; i++)
			{
				flagArray[i] = false;
			}
			for (int j = 0; j < (int)this.bonusStack.Length; j++)
			{
				if (this.bonusStack[j].isActive && this.bonusStack[j].mySpawnNumber != -1)
				{
					flagArray[this.bonusStack[j].mySpawnNumber] = true;
				}
			}
			for (int k = 0; k < (int)gameObjectArray.Length; k++)
			{
				if (gameObjectArray[k].GetComponent<SettingBonus>().numberSpawnZone != -1)
				{
					flagArray[gameObjectArray[k].GetComponent<SettingBonus>().numberSpawnZone] = true;
				}
			}
			while (flagArray[num])
			{
				num++;
				if (num != (int)flagArray.Length)
				{
					continue;
				}
				num = 0;
			}
			GameObject gameObject1 = this.bonusCreationZones[num];
			BoxCollider component = gameObject1.GetComponent<BoxCollider>();
			float single = component.size.x * gameObject1.transform.localScale.x;
			float single1 = component.size.z;
			Vector3 vector3 = gameObject1.transform.localScale;
			Vector2 vector2 = new Vector2(single, single1 * vector3.z);
			Vector3 vector31 = gameObject1.transform.position;
			float single2 = vector31.x - vector2.x / 2f;
			Vector3 vector32 = gameObject1.transform.position;
			Rect rect = new Rect(single2, vector32.z - vector2.y / 2f, vector2.x, vector2.y);
			float single3 = rect.x + UnityEngine.Random.Range(0f, rect.width);
			Vector3 vector33 = gameObject1.transform.position;
			pos = new Vector3(single3, vector33.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		}
		if (_type != 3)
		{
			int num1 = 0;
			while (num1 < (int)this.bonusStack.Length)
			{
				if (this.bonusStack[num1].isActive)
				{
					num1++;
				}
				else
				{
					this.MakeBonusRPC(num1, _type, pos, (num != -1 ? -1f : (float)this.GetTimeForBonus()), num);
					if (this.isMulti)
					{
						if (!this.isInet)
						{
							NetworkView networkView = this._networkView;
							object[] objArray = new object[] { num1, _type, pos, null, null };
							objArray[3] = (num != -1 ? -1f : (float)this.GetTimeForBonus());
							objArray[4] = num;
							networkView.RPC("MakeBonusRPC", RPCMode.Others, objArray);
						}
						else
						{
							PhotonView photonView = this.photonView;
							object[] objArray1 = new object[] { num1, _type, pos, null, null };
							objArray1[3] = (num != -1 ? -1f : (float)this.GetTimeForBonus());
							objArray1[4] = num;
							photonView.RPC("MakeBonusRPC", PhotonTargets.Others, objArray1);
						}
					}
					break;
				}
			}
		}
		else if (!this.isMulti || !this.isInet)
		{
			GameObject gameObject2 = Resources.Load(string.Concat("Bonuses/Bonus_", _type)) as GameObject;
			gameObject = (GameObject)UnityEngine.Object.Instantiate(gameObject2, pos, Quaternion.identity);
			gameObject.GetComponent<SettingBonus>().numberSpawnZone = num;
		}
		else
		{
			gameObject = PhotonNetwork.InstantiateSceneObject(string.Concat("Bonuses/Bonus_", (_type == -1 ? this.IndexBonus() : _type)), pos, Quaternion.identity, 0, null);
			gameObject.GetComponent<SettingBonus>().SetNumberSpawnZone(num);
		}
	}

	public void AddBonusAfterKillPlayer(Vector3 _pos)
	{
		if (!Defs.isInet)
		{
			this._networkView.RPC("AddBonusAfterKillPlayerRPC", RPCMode.Server, new object[] { _pos });
		}
		else
		{
			this.photonView.RPC("AddBonusAfterKillPlayerRPC", PhotonTargets.MasterClient, new object[] { _pos });
		}
	}

	[PunRPC]
	[RPC]
	private void AddBonusAfterKillPlayerRPC(Vector3 _pos)
	{
		this.AddBonusAfterKillPlayerRPC(this.IndexBonusOnKill(), _pos);
	}

	[PunRPC]
	[RPC]
	private void AddBonusAfterKillPlayerRPC(int _type, Vector3 _pos)
	{
		if (!Defs.isMulti)
		{
			this.AddBonus(_pos, _type);
			return;
		}
		if (Defs.isInet && PhotonNetwork.isMasterClient && !Defs.isHunger)
		{
			this.AddBonus(_pos, _type);
		}
		if (!Defs.isInet && Network.isServer)
		{
			this.AddBonus(_pos, _type);
		}
	}

	public void AddBonusForHunger(Vector3 pos, int _type, int spawnZoneIndex)
	{
		if (!Defs.isHunger)
		{
			return;
		}
		int num = 0;
		while (num < (int)this.bonusStack.Length)
		{
			if (this.bonusStack[num].isActive)
			{
				num++;
			}
			else
			{
				this.MakeBonusRPC(num, _type, pos, -1f, spawnZoneIndex);
				if (this.isMulti)
				{
					if (!this.isInet)
					{
						this._networkView.RPC("MakeBonusRPC", RPCMode.Others, new object[] { num, _type, pos, -1f, spawnZoneIndex });
					}
					else
					{
						this.photonView.RPC("MakeBonusRPC", PhotonTargets.Others, new object[] { num, _type, pos, -1f, spawnZoneIndex });
					}
				}
				break;
			}
		}
	}

	public void AddWeaponAfterKillPlayer(string _weaponName, Vector3 _pos)
	{
		this.photonView.RPC("AddWeaponAfterKillPlayerRPC", PhotonTargets.MasterClient, new object[] { _weaponName, _pos });
	}

	[PunRPC]
	[RPC]
	private void AddWeaponAfterKillPlayerRPC(string _weaponName, Vector3 _pos)
	{
		PhotonNetwork.InstantiateSceneObject(string.Concat("Weapon_Bonuses/", _weaponName, "_Bonus"), new Vector3(_pos.x, _pos.y - 0.5f, _pos.z), Quaternion.identity, 0, null);
	}

	private void Awake()
	{
		if (BonusController.sharedController != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			BonusController.sharedController = this;
		}
		if (Defs.IsSurvival)
		{
			this.creationInterval = 9f;
		}
		this.timerToAddBonus = this.creationInterval;
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.maxCountBonus = (!Defs.IsSurvival ? 5 : 3);
	}

	private bool CanSpawnGemBonus()
	{
		if (Defs.isHunger || !Defs.isMulti || !Defs.isInet || NetworkStartTable.LocalOrPasswordRoom())
		{
			return false;
		}
		if (PhotonNetwork.room != null && PhotonNetwork.room.customProperties["SpecialBonus"] != null && Convert.ToDouble(PhotonNetwork.room.customProperties["SpecialBonus"]) <= PhotonNetwork.time)
		{
			return true;
		}
		return false;
	}

	public void ClearBonuses()
	{
		for (int i = 0; i < (int)this.bonusStack.Length; i++)
		{
			if (this.bonusStack[i].isActive)
			{
				this.RemoveBonusRPC(i);
			}
		}
	}

	public void GetAndRemoveBonus(int index)
	{
		if (this.isMulti && this.isInet && !NetworkStartTable.LocalOrPasswordRoom())
		{
			this.RemoveBonusWithRewardRPC(PhotonNetwork.player, index);
			this.photonView.RPC("RemoveBonusWithRewardRPC", PhotonTargets.Others, new object[] { PhotonNetwork.player, index });
		}
	}

	[PunRPC]
	[RPC]
	private void GetBonusRewardRPC(int index)
	{
		if (index < (int)this.bonusStack.Length && this.bonusStack[index].isActive && this.bonusStack[index].isPickedUp)
		{
			if (this.bonusStack[index].playerPicked.Equals(PhotonNetwork.player))
			{
				if (this.bonusStack[index].type == BonusController.TypeBonus.Gem)
				{
					BankController.AddGems(1, true, AnalyticsConstants.AccrualType.Earned);
				}
			}
			this.RemoveBonusRPC(index);
		}
	}

	private double GetTimeForBonus()
	{
		double num = -1;
		num = (!Defs.isInet ? Network.time + 15 : PhotonNetwork.time + 15);
		return num;
	}

	private int IndexBonus()
	{
		int key;
		int num = UnityEngine.Random.Range(0, this.sumProbabilitys);
		Dictionary<int, Dictionary<string, int>>.Enumerator enumerator = this.probabilityBonus.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, Dictionary<string, int>> current = enumerator.Current;
				if (num < current.Value["min"] || num >= current.Value["max"])
				{
					continue;
				}
				key = current.Key;
				return key;
			}
			return 0;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return key;
	}

	private int IndexBonusOnKill()
	{
		int key;
		if (this.CanSpawnGemBonus() && UnityEngine.Random.Range(0, 100) < 5)
		{
			return 9;
		}
		int num = UnityEngine.Random.Range(0, this.sumProbabilitys);
		Dictionary<int, Dictionary<string, int>>.Enumerator enumerator = this.probabilityBonus.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, Dictionary<string, int>> current = enumerator.Current;
				if (num < current.Value["min"] || num >= current.Value["max"])
				{
					continue;
				}
				key = current.Key;
				return key;
			}
			return 0;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return key;
	}

	private void InitStack()
	{
		this.bonusStack = new BonusItem[this.maxCountBonus + 6];
		for (int i = 0; i < (int)this.bonusStack.Length; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.bonusPrefab, Vector3.down * 100f, Quaternion.identity);
			gameObject.transform.parent = base.transform;
			this.bonusStack[i] = gameObject.GetComponent<BonusItem>();
		}
	}

	[PunRPC]
	[RPC]
	private void MakeBonusRPC(int index, int type, Vector3 position, float expireTime, int zoneNumber)
	{
		if (index < (int)this.bonusStack.Length && !this.bonusStack[index].isActive)
		{
			this.bonusStack[index].ActivateBonus((BonusController.TypeBonus)type, position, (double)expireTime, zoneNumber, index);
			if (!this.bonusStack[index].isTimeBonus)
			{
				this.activeBonusesCount++;
			}
		}
	}

	private void OnDestroy()
	{
		if (this.photonView)
		{
			PhotonObjectCacher.RemoveObject(base.gameObject);
		}
	}

	private void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (PhotonNetwork.isMasterClient)
		{
			for (int i = 0; i < (int)this.bonusStack.Length; i++)
			{
				if (this.bonusStack[i].isActive)
				{
					this.photonView.RPC("MakeBonusRPC", player, new object[] { i, (int)this.bonusStack[i].type, this.bonusStack[i].transform.position, (float)this.bonusStack[i].expireTime, this.bonusStack[i].mySpawnNumber });
				}
			}
		}
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			for (int i = 0; i < (int)this.bonusStack.Length; i++)
			{
				if (this.bonusStack[i].isActive)
				{
					this._networkView.RPC("MakeBonusRPC", player, new object[] { i, (int)this.bonusStack[i].type, this.bonusStack[i].transform.position, (float)this.bonusStack[i].expireTime, this.bonusStack[i].mySpawnNumber });
				}
			}
		}
	}

	private void PickupBonus(int index, PhotonPlayer player)
	{
		if (index < (int)this.bonusStack.Length && this.bonusStack[index].isActive && !this.bonusStack[index].isPickedUp)
		{
			this.bonusStack[index].PickupBonus(player);
		}
	}

	public void RemoveBonus(int index)
	{
		this.RemoveBonusRPC(index);
		if (this.isMulti)
		{
			if (!this.isInet)
			{
				this._networkView.RPC("RemoveBonusRPC", RPCMode.Others, new object[] { index });
			}
			else
			{
				this.photonView.RPC("RemoveBonusRPC", PhotonTargets.Others, new object[] { index });
			}
		}
	}

	[PunRPC]
	[RPC]
	private void RemoveBonusRPC(int index)
	{
		if (index < (int)this.bonusStack.Length && this.bonusStack[index].isActive)
		{
			if (!this.bonusStack[index].isTimeBonus)
			{
				this.activeBonusesCount--;
			}
			this.bonusStack[index].DeactivateBonus();
		}
	}

	[PunRPC]
	[RPC]
	private void RemoveBonusWithRewardRPC(PhotonPlayer sender, int index)
	{
		if (!this.isMulti || !this.isInet || NetworkStartTable.LocalOrPasswordRoom())
		{
			return;
		}
		if (index < (int)this.bonusStack.Length && this.bonusStack[index].isActive)
		{
			this.PickupBonus(index, sender);
		}
	}

	private void SetProbability()
	{
		this.probabilityBonusDict.Clear();
		this.probabilityBonus.Clear();
		this.sumProbabilitys = 0;
		if (!Defs.isMulti)
		{
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None)
			{
				this.probabilityBonusDict.Add(0, 55);
				this.probabilityBonusDict.Add(1, 14);
				this.probabilityBonusDict.Add(2, 12);
				this.probabilityBonusDict.Add(4, 15);
			}
			else
			{
				this.probabilityBonusDict.Add(0, 100);
			}
		}
		else if (Defs.isHunger)
		{
			this.probabilityBonusDict.Add(3, 100);
		}
		else if (SceneLoader.ActiveSceneName.Equals("Knife"))
		{
			this.probabilityBonusDict.Add(1, 75);
			this.probabilityBonusDict.Add(2, 25);
		}
		else if (Defs.isDaterRegim)
		{
			this.probabilityBonusDict.Add(0, 100);
		}
		else if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.probabilityBonusDict.Add(0, 100);
		}
		else if (Defs.isCOOP)
		{
			this.probabilityBonusDict.Add(0, 55);
			this.probabilityBonusDict.Add(1, 14);
			this.probabilityBonusDict.Add(2, 12);
			this.probabilityBonusDict.Add(4, 15);
		}
		else if (!SceneLoader.ActiveSceneName.Equals("WalkingFortress"))
		{
			this.probabilityBonusDict.Add(0, 50);
			this.probabilityBonusDict.Add(1, 10);
			this.probabilityBonusDict.Add(2, 10);
			if (WeaponManager.sharedManager._currentFilterMap == 0)
			{
				this.probabilityBonusDict.Add(4, 15);
			}
			this.probabilityBonusDict.Add(5, 2);
			this.probabilityBonusDict.Add(8, 5);
			this.probabilityBonusDict.Add(7, 3);
			this.probabilityBonusDict.Add(6, 5);
		}
		else
		{
			this.probabilityBonusDict.Add(0, 50);
			this.probabilityBonusDict.Add(1, 10);
			this.probabilityBonusDict.Add(2, 5);
			this.probabilityBonusDict.Add(4, 10);
			this.probabilityBonusDict.Add(5, 2);
			this.probabilityBonusDict.Add(8, 5);
			this.probabilityBonusDict.Add(7, 3);
			this.probabilityBonusDict.Add(6, 15);
		}
		foreach (KeyValuePair<int, int> keyValuePair in this.probabilityBonusDict)
		{
			Dictionary<string, int> strs = new Dictionary<string, int>()
			{
				{ "min", this.sumProbabilitys }
			};
			this.sumProbabilitys += keyValuePair.Value;
			strs.Add("max", this.sumProbabilitys);
			this.probabilityBonus.Add(keyValuePair.Key, strs);
		}
	}

	private void Start()
	{
		this.photonView = PhotonView.Get(this);
		this._networkView = base.GetComponent<NetworkView>();
		if (this.photonView)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		this.bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		if (this.maxCountBonus > (int)this.bonusCreationZones.Length)
		{
			this.maxCountBonus = (int)this.bonusCreationZones.Length;
		}
		this.zombieCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
		this._weaponManager = WeaponManager.sharedManager;
		this.SetProbability();
		this.InitStack();
	}

	private void Update()
	{
		bool flag = false;
		if (!this.isMulti)
		{
			flag = true;
		}
		else
		{
			flag = (!this.isInet ? Network.isServer : PhotonNetwork.isMasterClient);
		}
		if (flag)
		{
			for (int i = 0; i < (int)this.bonusStack.Length; i++)
			{
				if (this.bonusStack[i].isActive && this.bonusStack[i].isPickedUp)
				{
					this.photonView.RPC("GetBonusRewardRPC", PhotonTargets.All, new object[] { i });
				}
			}
		}
		if (!this.isStopCreateBonus && flag)
		{
			this.timerToAddBonus -= Time.deltaTime;
		}
		if (this.timerToAddBonus < 0f)
		{
			this.timerToAddBonus = this.creationInterval;
			this.AddBonus(Vector3.zero, this.IndexBonus());
		}
	}

	public enum TypeBonus
	{
		Ammo,
		Health,
		Armor,
		Chest,
		Grenade,
		Mech,
		JetPack,
		Invisible,
		Turret,
		Gem
	}
}