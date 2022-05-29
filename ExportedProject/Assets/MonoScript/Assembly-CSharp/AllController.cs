using System;
using UnityEngine;

public class AllController : MonoBehaviour
{
	public static AllController instance;

	static AllController()
	{
	}

	public AllController()
	{
	}

	private void Awake()
	{
		Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		AllController.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		AllController.instance = null;
	}
}