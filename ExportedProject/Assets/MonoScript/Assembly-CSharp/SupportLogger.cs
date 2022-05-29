using System;
using UnityEngine;

public class SupportLogger : MonoBehaviour
{
	public bool LogTrafficStats = true;

	public SupportLogger()
	{
	}

	public void Start()
	{
		GameObject gameObject = GameObject.Find("PunSupportLogger");
		if (gameObject == null)
		{
			gameObject = new GameObject("PunSupportLogger");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<SupportLogging>().LogTrafficStats = this.LogTrafficStats;
		}
	}
}