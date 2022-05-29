using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToFriendsFromClans : MonoBehaviour
{
	public BackToFriendsFromClans()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "Friends";
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}
}