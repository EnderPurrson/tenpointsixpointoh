using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class coinsPlashka : MonoBehaviour
{
	public static coinsPlashka thisScript;

	public static bool hideButtonCoins;

	private float kfSize = (float)Screen.height / 768f;

	private bool isHasKeyAchived500;

	private bool isHasKeyAchived1000;

	public Rect rectButCoins;

	public Rect rectLabelCoins;

	private int tekKolCoins;

	private float lastTImeFetchedeychain;

	private Font f;

	[CompilerGenerated]
	private static Action<bool> _003C_003Ef__am_0024cacheA;

	[CompilerGenerated]
	private static Action<bool> _003C_003Ef__am_0024cacheB;

	public static Rect symmetricRect
	{
		get
		{
			Rect result = new Rect(thisScript.rectLabelCoins.x, thisScript.rectButCoins.y, thisScript.rectButCoins.width, thisScript.rectButCoins.height);
			result.x = (float)Screen.width - result.x - result.width;
			return result;
		}
	}

	private void Awake()
	{
		thisScript = base.gameObject.GetComponent<coinsPlashka>();
		hidePlashka();
		tekKolCoins = Storager.getInt("Coins", false);
		lastTImeFetchedeychain = Time.realtimeSinceStartup;
		isHasKeyAchived500 = PlayerPrefs.HasKey("Achieved500");
		isHasKeyAchived1000 = PlayerPrefs.HasKey("Achieved1000");
	}

	public static void showPlashka()
	{
		if (thisScript != null)
		{
			thisScript.enabled = true;
		}
	}

	public static void hidePlashka()
	{
		if (thisScript != null)
		{
			thisScript.enabled = false;
		}
	}

	private void Update()
	{
		if (!Social.localUser.authenticated || Time.frameCount % 60 != 23)
		{
			return;
		}
		if (tekKolCoins >= 500 && !isHasKeyAchived500)
		{
			if (_003C_003Ef__am_0024cacheA == null)
			{
				_003C_003Ef__am_0024cacheA = _003CUpdate_003Em__4BC;
			}
			Social.ReportProgress("CgkIr8rGkPIJEAIQBA", 100.0, _003C_003Ef__am_0024cacheA);
			PlayerPrefs.SetInt("Achieved500", 1);
			isHasKeyAchived500 = true;
		}
		if (tekKolCoins >= 1000 && !isHasKeyAchived1000)
		{
			if (_003C_003Ef__am_0024cacheB == null)
			{
				_003C_003Ef__am_0024cacheB = _003CUpdate_003Em__4BD;
			}
			Social.ReportProgress("CgkIr8rGkPIJEAIQBQ", 100.0, _003C_003Ef__am_0024cacheB);
			PlayerPrefs.SetInt("Achieved1000", 1);
			isHasKeyAchived1000 = true;
		}
	}

	[CompilerGenerated]
	private static void _003CUpdate_003Em__4BC(bool success)
	{
		Debug.Log(string.Format("Achievement Ekonomist completed: {0}", success));
	}

	[CompilerGenerated]
	private static void _003CUpdate_003Em__4BD(bool success)
	{
		Debug.Log(string.Format("Achievement Rich Man completed: {0}", success));
	}
}
