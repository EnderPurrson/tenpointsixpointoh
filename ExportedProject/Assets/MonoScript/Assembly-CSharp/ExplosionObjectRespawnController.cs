using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObjectRespawnController : MonoBehaviour
{
	[Header("Time settings")]
	public float timeToNextRespawn;

	[Header("Object settings")]
	public GameObject explosionObjectPrefab;

	private GameObject _currentExplosionObject;

	private bool _isMultiplayerMode;

	public static List<GameObject> respawnList;

	static ExplosionObjectRespawnController()
	{
		ExplosionObjectRespawnController.respawnList = new List<GameObject>();
	}

	public ExplosionObjectRespawnController()
	{
	}

	private void CreateExplosionObject()
	{
		if (!this._isMultiplayerMode)
		{
			this._currentExplosionObject = UnityEngine.Object.Instantiate<GameObject>(this.explosionObjectPrefab);
		}
		else if (!PhotonNetwork.isMasterClient)
		{
			this._currentExplosionObject = null;
		}
		else
		{
			string str = string.Format("ExplosionObjects/{0}", this.explosionObjectPrefab.name);
			this._currentExplosionObject = PhotonNetwork.InstantiateSceneObject(str, base.transform.position, base.transform.rotation, 0, null);
		}
		if (this._currentExplosionObject != null)
		{
			this._currentExplosionObject.transform.parent = base.transform;
			this._currentExplosionObject.transform.localPosition = Vector3.zero;
			this._currentExplosionObject.transform.localRotation = Quaternion.identity;
		}
	}

	private void OnDestroy()
	{
		ExplosionObjectRespawnController.respawnList.Remove(base.gameObject);
	}

	private void Start()
	{
		this._isMultiplayerMode = Defs.isMulti;
		this.CreateExplosionObject();
		ExplosionObjectRespawnController.respawnList.Add(base.gameObject);
	}

	public void StartProcessNewRespawn()
	{
		this._currentExplosionObject = null;
		base.Invoke("CreateExplosionObject", this.timeToNextRespawn);
	}
}