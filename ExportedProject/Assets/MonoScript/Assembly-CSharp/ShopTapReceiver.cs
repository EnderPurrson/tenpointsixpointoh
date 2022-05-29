using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopTapReceiver : MonoBehaviour
{
	private bool blinkShop;

	private float lastTimeBlink;

	private UISprite sp;

	public ShopTapReceiver()
	{
	}

	public static void AddClickHndIfNotExist(Action handler)
	{
		if (ShopTapReceiver.ShopClicked == null || Array.IndexOf<Delegate>(ShopTapReceiver.ShopClicked.GetInvocationList(), handler) < 0)
		{
			ShopTapReceiver.ShopClicked = (Action)Delegate.Combine(ShopTapReceiver.ShopClicked, handler);
		}
	}

	private void HandleStartBlinkShop()
	{
		this.blinkShop = true;
		this.lastTimeBlink = Time.realtimeSinceStartup;
	}

	private void HandleStopBlinkShop()
	{
		base.GetComponentInChildren<UISprite>().spriteName = "green_btn";
		this.blinkShop = false;
	}

	private void OnClick()
	{
		if (LoadingInAfterGame.isShowLoading)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.Reset();
		}
		if (ShopTapReceiver.ShopClicked == null)
		{
			Debug.Log("ShopClicked == null");
		}
		else
		{
			ShopTapReceiver.ShopClicked();
		}
	}

	private void OnDestroy()
	{
	}

	private void Start()
	{
		this.sp = base.GetComponentInChildren<UISprite>();
	}

	private void Update()
	{
		if (this.blinkShop && Time.realtimeSinceStartup - this.lastTimeBlink > 0.16f)
		{
			this.lastTimeBlink = Time.realtimeSinceStartup;
			this.sp.spriteName = (!this.sp.spriteName.Equals("green_btn") ? "green_btn" : "green_btn_n");
		}
	}

	public static event Action ShopClicked;
}