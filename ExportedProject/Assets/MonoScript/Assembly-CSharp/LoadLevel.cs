using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
	public Texture fon;

	public LoadLevel()
	{
	}

	private void OnGUI()
	{
		Rect rect = new Rect(((float)Screen.width - 1366f * Defs.Coef) / 2f, 0f, 1366f * Defs.Coef, (float)Screen.height);
		GUI.DrawTexture(rect, this.fon, ScaleMode.StretchToFill);
	}

	private void Start()
	{
		Singleton<SceneLoader>.Instance.LoadScene("Level3", LoadSceneMode.Single);
	}
}