using System;
using UnityEngine;

public class ButtonBannerBase : MonoBehaviour
{
	[HideInInspector]
	public int indexBut;

	public int priorityShow;

	public ButtonBannerBase()
	{
	}

	public virtual bool BannerIsActive()
	{
		return false;
	}

	public virtual void OnChangeLocalize()
	{
	}

	private void OnClick()
	{
		this.OnClickButton();
	}

	public virtual void OnClickButton()
	{
	}

	public virtual void OnHide()
	{
	}

	private void OnPress(bool IsDown)
	{
		if (!IsDown)
		{
			ButtonBannerHUD.instance.ResetTimerNextBanner();
		}
		else
		{
			ButtonBannerHUD.instance.StopTimerNextBanner();
		}
	}

	public virtual void OnShow()
	{
	}

	public virtual void OnUpdateParameter()
	{
	}
}