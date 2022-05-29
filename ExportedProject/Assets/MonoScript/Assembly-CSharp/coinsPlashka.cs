using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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

	public static Rect symmetricRect
	{
		get
		{
			Rect rect = new Rect(coinsPlashka.thisScript.rectLabelCoins.x, coinsPlashka.thisScript.rectButCoins.y, coinsPlashka.thisScript.rectButCoins.width, coinsPlashka.thisScript.rectButCoins.height)
			{
				x = (float)Screen.width - rect.x - rect.width
			};
			return rect;
		}
	}

	static coinsPlashka()
	{
	}

	public coinsPlashka()
	{
	}

	private void Awake()
	{
		coinsPlashka.thisScript = base.gameObject.GetComponent<coinsPlashka>();
		coinsPlashka.hidePlashka();
		this.tekKolCoins = Storager.getInt("Coins", false);
		this.lastTImeFetchedeychain = Time.realtimeSinceStartup;
		this.isHasKeyAchived500 = PlayerPrefs.HasKey("Achieved500");
		this.isHasKeyAchived1000 = PlayerPrefs.HasKey("Achieved1000");
	}

	public static void hidePlashka()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = false;
		}
	}

	public static void showPlashka()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = true;
		}
	}

	private void Update()
	{
		if (!Social.localUser.authenticated)
		{
			return;
		}
		if (Time.frameCount % 60 != 23)
		{
			return;
		}
		if (this.tekKolCoins >= 500 && !this.isHasKeyAchived500)
		{
			Social.ReportProgress("CgkIr8rGkPIJEAIQBA", 100, (bool success) => Debug.Log(string.Format("Achievement Ekonomist completed: {0}", success)));
			PlayerPrefs.SetInt("Achieved500", 1);
			this.isHasKeyAchived500 = true;
		}
		if (this.tekKolCoins >= 1000 && !this.isHasKeyAchived1000)
		{
			Social.ReportProgress("CgkIr8rGkPIJEAIQBQ", 100, (bool success) => Debug.Log(string.Format("Achievement Rich Man completed: {0}", success)));
			PlayerPrefs.SetInt("Achieved1000", 1);
			this.isHasKeyAchived1000 = true;
		}
	}
}