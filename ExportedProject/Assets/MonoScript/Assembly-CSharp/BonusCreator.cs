using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class BonusCreator : MonoBehaviour
{
	public GameObject[] bonusPrefabs;

	public float creationInterval = 15f;

	public float weaponCreationInterval = 30f;

	private UnityEngine.Object[] weaponPrefabs;

	private int _lastWeapon = -1;

	private bool _isMultiplayer;

	private ArrayList bonuses = new ArrayList();

	private ArrayList _weapons = new ArrayList();

	public WeaponManager _weaponManager;

	private GameObject[] _bonusCreationZones;

	private ZombieCreator _zombieCreator;

	private ArrayList _weaponsProbDistr = new ArrayList();

	public BonusCreator()
	{
	}

	public static GameObject _CreateBonus(string _weaponName, Vector3 pos)
	{
		GameObject gameObject = BonusCreator._CreateBonusPrefab(_weaponName);
		if (gameObject == null)
		{
			UnityEngine.Debug.Log("null");
			return null;
		}
		return BonusCreator._CreateBonusFromPrefab(gameObject, pos);
	}

	public static GameObject _CreateBonusFromPrefab(UnityEngine.Object bonus, Vector3 pos)
	{
		GameObject vector3 = (GameObject)UnityEngine.Object.Instantiate(bonus, pos, Quaternion.identity);
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		return vector3;
	}

	public static GameObject _CreateBonusPrefab(string _weaponName)
	{
		GameObject gameObject = Resources.Load(string.Concat("Weapon_Bonuses/", _weaponName, "_Bonus")) as GameObject;
		if (gameObject != null)
		{
			return gameObject;
		}
		UnityEngine.Debug.Log("null");
		return null;
	}

	private int _curLevel()
	{
		return (Defs.isMulti ? GlobalGameController.currentLevel : CurrentCampaignGame.currentLevel);
	}

	private float _DistrSum()
	{
		float current = 0f;
		IEnumerator enumerator = this._weaponsProbDistr.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				current += (float)((int)enumerator.Current);
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
		return current;
	}

	private int _indexForType(int type)
	{
		int num = 0;
		if (type == 9 || type == 10)
		{
			num = 1;
		}
		else if (type == 8)
		{
			num = 2;
		}
		return num;
	}

	public void addBonusFromPhotonRPC(int _id, int _type, Vector3 _pos, Quaternion rot)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.bonusPrefabs[this._indexForType(_type)], _pos, rot);
		gameObject.GetComponent<PhotonView>().viewID = _id;
		gameObject.GetComponent<SettingBonus>().typeOfMass = _type;
	}

	[DebuggerHidden]
	private IEnumerator AddWeapon()
	{
		BonusCreator.u003cAddWeaponu003ec__IteratorD variable = null;
		return variable;
	}

	private void Awake()
	{
		if (Defs.IsSurvival)
		{
			this.creationInterval = 9f;
			this.weaponCreationInterval = 15f;
		}
		if (!Defs.isMulti)
		{
			this._isMultiplayer = false;
		}
		else
		{
			this._isMultiplayer = true;
		}
		if (!this._isMultiplayer)
		{
			this.weaponPrefabs = WeaponManager.sharedManager.weaponsInGame;
			UnityEngine.Object[] objArray = this.weaponPrefabs;
			for (int i = 0; i < (int)objArray.Length; i++)
			{
				WeaponSounds component = ((GameObject)objArray[i]).GetComponent<WeaponSounds>();
				this._weaponsProbDistr.Add(component.Probability);
			}
		}
	}

	public void BeginCreateBonuses()
	{
		if (Application.isEditor && Defs.IsSurvival && !SceneLoader.ActiveSceneName.Equals(Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % (int)Defs.SurvivalMaps.Length]))
		{
			return;
		}
		if (Defs.IsSurvival)
		{
			base.StartCoroutine(this.AddWeapon());
		}
	}

	[PunRPC]
	[RPC]
	private void delBonus(NetworkViewID id)
	{
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Bonus");
		int num = 0;
		while (num < (int)gameObjectArray.Length)
		{
			GameObject gameObject = gameObjectArray[num];
			if (!id.Equals(gameObject.GetComponent<NetworkView>().viewID))
			{
				num++;
			}
			else
			{
				UnityEngine.Object.Destroy(gameObject);
				break;
			}
		}
	}

	private void Start()
	{
		this._bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		this._zombieCreator = base.gameObject.GetComponent<ZombieCreator>();
		this._weaponManager = WeaponManager.sharedManager;
	}
}