using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
	private bool m_Levelloaded;

	public ReturnToMainMenu()
	{
	}

	public void GoBackToMainMenu()
	{
		Debug.Log("going back to main menu");
		Singleton<SceneLoader>.Instance.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	private void OnLevelWasLoaded(int level)
	{
		this.m_Levelloaded = true;
	}

	public void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (this.m_Levelloaded)
		{
			Canvas component = base.gameObject.GetComponent<Canvas>();
			component.enabled = false;
			component.enabled = true;
			this.m_Levelloaded = false;
		}
	}
}