using System;
using System.Collections.Generic;
using UnityEngine;

public class RayAndExplosionsStackController : MonoBehaviour
{
	public static RayAndExplosionsStackController sharedController;

	private Dictionary<string, List<GameObject>> gameObjects = new Dictionary<string, List<GameObject>>();

	private Dictionary<string, float> timeUseGameObjects = new Dictionary<string, float>();

	public Transform mytranform;

	public RayAndExplosionsStackController()
	{
	}

	private void Awake()
	{
		RayAndExplosionsStackController.sharedController = this;
		this.mytranform = base.GetComponent<Transform>();
	}

	public GameObject GetObjectFromName(string _name)
	{
		GameObject item = null;
		bool flag = this.gameObjects.ContainsKey(_name);
		if (flag)
		{
			while (this.gameObjects[_name].Count > 0 && this.gameObjects[_name][0] == null)
			{
				this.gameObjects[_name].RemoveAt(0);
			}
		}
		if (!flag || this.gameObjects[_name].Count <= 0)
		{
			GameObject gameObject = Resources.Load(_name) as GameObject;
			if (gameObject != null)
			{
				item = UnityEngine.Object.Instantiate(gameObject, Vector3.down * 10000f, Quaternion.identity) as GameObject;
				item.GetComponent<Transform>().parent = this.mytranform;
				item.GetComponent<RayAndExplosionsStackItem>().myName = _name;
			}
		}
		else
		{
			item = this.gameObjects[_name][0];
			this.gameObjects[_name].RemoveAt(0);
			item.SetActive(true);
		}
		if (item == null && Application.isEditor)
		{
			Debug.LogError(string.Concat("GameOblect ", _name, " in RayAndExplosionsStackController not create!!!"));
		}
		return item;
	}

	private void OnDestroy()
	{
		RayAndExplosionsStackController.sharedController = null;
	}

	public void ReturnObjectFromName(GameObject returnObject, string _name)
	{
		returnObject.GetComponent<Transform>().position = Vector3.down * 10000f;
		returnObject.SetActive(false);
		returnObject.transform.parent = this.mytranform;
		if (!this.gameObjects.ContainsKey(_name))
		{
			this.gameObjects.Add(_name, new List<GameObject>());
		}
		if (this.timeUseGameObjects.ContainsKey(_name))
		{
			this.timeUseGameObjects[_name] = Time.realtimeSinceStartup;
		}
		else
		{
			this.timeUseGameObjects.Add(_name, Time.realtimeSinceStartup);
		}
		this.gameObjects[_name].Add(returnObject);
	}
}