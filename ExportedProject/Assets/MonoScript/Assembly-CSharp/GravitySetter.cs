using Rilisoft;
using System;
using UnityEngine;

public class GravitySetter : MonoBehaviour
{
	public readonly static float normalGravity;

	public readonly static float spaceBaseGravity;

	public readonly static float matrixGravity;

	static GravitySetter()
	{
		GravitySetter.normalGravity = -9.81f;
		GravitySetter.spaceBaseGravity = -6.54f;
		GravitySetter.matrixGravity = -4.9049997f;
	}

	public GravitySetter()
	{
	}

	private void OnLevelWasLoaded(int lev)
	{
		if (SceneLoader.ActiveSceneName.Equals("Space"))
		{
			Physics.gravity = new Vector3(0f, GravitySetter.spaceBaseGravity, 0f);
		}
		else if (!SceneLoader.ActiveSceneName.Equals("Matrix"))
		{
			Physics.gravity = new Vector3(0f, GravitySetter.normalGravity, 0f);
		}
		else
		{
			Physics.gravity = new Vector3(0f, GravitySetter.matrixGravity, 0f);
		}
	}
}