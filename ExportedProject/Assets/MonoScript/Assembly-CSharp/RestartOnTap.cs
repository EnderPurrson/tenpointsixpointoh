using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnTap : MonoBehaviour
{
	public RestartOnTap()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			Singleton<SceneLoader>.Instance.LoadScene("Level2", LoadSceneMode.Single);
		}
	}
}