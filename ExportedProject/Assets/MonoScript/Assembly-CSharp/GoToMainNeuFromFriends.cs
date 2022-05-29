using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Obsolete]
internal sealed class GoToMainNeuFromFriends : MonoBehaviour
{
	private bool firstFrame = true;

	public GoToMainNeuFromFriends()
	{
	}

	private void HandleClick()
	{
		ButtonClickSound.Instance.PlayClick();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}

	private void OnPress(bool isDown)
	{
		if (!isDown)
		{
			if (this.firstFrame)
			{
				return;
			}
			this.HandleClick();
		}
		else
		{
			this.firstFrame = false;
		}
	}
}