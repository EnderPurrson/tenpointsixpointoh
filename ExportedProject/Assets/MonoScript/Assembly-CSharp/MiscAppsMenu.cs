using System;
using UnityEngine;

public class MiscAppsMenu : MonoBehaviour
{
	public static MiscAppsMenu Instance;

	public HiddenSettings misc;

	public MiscAppsMenu()
	{
	}

	private void Awake()
	{
		MiscAppsMenu.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void UnloadMisc()
	{
		this.misc = null;
	}
}