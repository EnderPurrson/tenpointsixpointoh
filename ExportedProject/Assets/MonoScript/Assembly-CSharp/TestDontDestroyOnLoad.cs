using System;
using UnityEngine;

public class TestDontDestroyOnLoad : MonoBehaviour
{
	public TestDontDestroyOnLoad()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}