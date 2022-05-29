using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelReset : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public LevelReset()
	{
	}

	public void OnPointerClick(PointerEventData data)
	{
		Singleton<SceneLoader>.Instance.LoadSceneAsync(Application.loadedLevelName, LoadSceneMode.Single);
	}

	private void Update()
	{
	}
}