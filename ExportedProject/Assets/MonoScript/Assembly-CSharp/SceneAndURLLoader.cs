using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAndURLLoader : MonoBehaviour
{
	private PauseMenu m_PauseMenu;

	public SceneAndURLLoader()
	{
	}

	private void Awake()
	{
		this.m_PauseMenu = base.GetComponentInChildren<PauseMenu>();
	}

	public void LoadURL(string url)
	{
		Application.OpenURL(url);
	}

	public void SceneLoad(string sceneName)
	{
		this.m_PauseMenu.MenuOff();
		Singleton<SceneLoader>.Instance.LoadScene(sceneName, LoadSceneMode.Single);
	}
}