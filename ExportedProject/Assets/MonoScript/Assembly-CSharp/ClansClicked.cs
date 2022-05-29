using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClansClicked : MonoBehaviour
{
	public ClansClicked()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = Resources.Load<Texture>("Friends_Loading");
		LoadConnectScene.sceneToLoad = "Clans";
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}
}