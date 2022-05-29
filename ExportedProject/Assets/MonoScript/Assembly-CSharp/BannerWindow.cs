using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BannerWindow : MonoBehaviour
{
	public UITexture Background;

	public UIButton ExitButton;

	private bool _isShow;

	public bool IsShow
	{
		get
		{
			return this._isShow;
		}
		set
		{
			this._isShow = value;
		}
	}

	public BannerWindowType type
	{
		get;
		set;
	}

	public BannerWindow()
	{
	}

	public void Hide()
	{
		AdmobPerelivWindow component = base.GetComponent<AdmobPerelivWindow>();
		if (component == null)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			component.Hide();
		}
		this.IsShow = false;
	}

	protected virtual void SetActiveAndShow()
	{
		base.gameObject.SetActive(true);
		this.IsShow = true;
	}

	public void SetBackgroundImage(Texture2D image)
	{
		if (this.Background == null)
		{
			return;
		}
		this.Background.mainTexture = image;
	}

	public void SetEnableExitButton(bool enable)
	{
		if (this.ExitButton == null)
		{
			return;
		}
		this.ExitButton.gameObject.SetActive(enable);
	}

	public virtual void Show()
	{
		this.SetActiveAndShow();
		AdmobPerelivWindow component = base.GetComponent<AdmobPerelivWindow>();
		if (component != null)
		{
			component.Show();
		}
	}

	internal virtual void Submit()
	{
	}
}