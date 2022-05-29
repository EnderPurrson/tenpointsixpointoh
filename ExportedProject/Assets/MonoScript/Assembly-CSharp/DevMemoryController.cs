using System;
using UnityEngine;

public class DevMemoryController : MonoBehaviour
{
	public static string keyActiveMemoryInfo;

	public static DevMemoryController instance;

	static DevMemoryController()
	{
		DevMemoryController.keyActiveMemoryInfo = "keyActiveMemoryInfo";
	}

	public DevMemoryController()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.Destroy(this);
	}
}