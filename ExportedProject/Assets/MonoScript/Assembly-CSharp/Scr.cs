using System;
using UnityEngine;

public class Scr : MonoBehaviour
{
	public Scr()
	{
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
	}
}