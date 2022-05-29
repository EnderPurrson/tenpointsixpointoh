using System;
using System.Collections.Generic;
using UnityEngine;

public class RocketStack : MonoBehaviour
{
	public static RocketStack sharedController;

	private List<GameObject> gameObjects = new List<GameObject>();

	private float timeUseGameObjects;

	public Transform mytranform;

	public RocketStack()
	{
	}

	private void Awake()
	{
		RocketStack.sharedController = this;
		this.mytranform = base.transform;
	}

	public GameObject GetRocket()
	{
		while (this.gameObjects.Count > 0 && this.gameObjects[0] == null)
		{
			this.gameObjects.RemoveAt(0);
		}
		GameObject item = null;
		if (this.gameObjects.Count <= 0)
		{
			if (!Defs.isMulti)
			{
				item = UnityEngine.Object.Instantiate(Resources.Load("Rocket") as GameObject, Vector3.down * 10000f, Quaternion.identity) as GameObject;
			}
			else
			{
				item = (Defs.isInet ? PhotonNetwork.Instantiate("Rocket", Vector3.down * 10000f, Quaternion.identity, 0) : (GameObject)Network.Instantiate(Resources.Load("Rocket") as GameObject, Vector3.down * 10000f, Quaternion.identity, 0));
			}
			item.transform.parent = this.mytranform;
		}
		else
		{
			item = this.gameObjects[0];
			this.gameObjects.RemoveAt(0);
			item.SetActive(true);
		}
		return item;
	}

	private void OnDestroy()
	{
		RocketStack.sharedController = null;
	}

	public void ReturnRocket(GameObject returnObject)
	{
		Rigidbody component = returnObject.GetComponent<Rigidbody>();
		component.velocity = Vector3.zero;
		component.isKinematic = false;
		component.useGravity = false;
		component.angularVelocity = Vector3.zero;
		returnObject.transform.position = Vector3.down * 10000f;
		returnObject.SetActive(false);
		this.timeUseGameObjects = Time.realtimeSinceStartup;
		this.gameObjects.Add(returnObject);
	}
}