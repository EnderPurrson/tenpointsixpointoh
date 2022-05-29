using System;
using System.Collections.Generic;
using UnityEngine;

public class InMemoryKeeper : MonoBehaviour
{
	public List<GameObject> objectsToKeepInMemory = new List<GameObject>();

	public InMemoryKeeper()
	{
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.objectsToKeepInMemory.Add(Resources.Load<GameObject>("Rocket"));
		this.objectsToKeepInMemory.AddRange(Resources.LoadAll<GameObject>("Rays/"));
		this.objectsToKeepInMemory.AddRange(Resources.LoadAll<GameObject>("Explosions/"));
	}
}