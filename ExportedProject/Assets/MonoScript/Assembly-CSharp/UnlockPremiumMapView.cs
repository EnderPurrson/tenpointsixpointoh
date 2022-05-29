using Rilisoft;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class UnlockPremiumMapView : MonoBehaviour
{
	public ButtonHandler closeButton;

	public ButtonHandler unlockButton;

	public UISprite priceSprite;

	public UILabel[] priceLabel;

	private int _price = 15;

	private EventHandler ClosePressed;

	private EventHandler UnlockPressed;

	public int Price
	{
		get
		{
			return this._price;
		}
		set
		{
			this._price = value;
			if (this.priceSprite != null)
			{
				this.priceSprite.spriteName = string.Format("premium_baner_{0}", value);
			}
		}
	}

	public UnlockPremiumMapView()
	{
	}

	private void OnDestroy()
	{
		if (this.closeButton != null)
		{
			this.closeButton.Clicked -= new EventHandler(this.RaiseClosePressed);
		}
		if (this.unlockButton != null)
		{
			this.unlockButton.Clicked -= new EventHandler(this.RaiseUnlockPressed);
		}
	}

	private void RaiseClosePressed(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("Close event raised.");
		}
		EventHandler closePressed = this.ClosePressed;
		if (closePressed != null)
		{
			closePressed(sender, e);
		}
	}

	private void RaiseUnlockPressed(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("Unlock event raised.");
		}
		EventHandler unlockPressed = this.UnlockPressed;
		if (unlockPressed != null)
		{
			unlockPressed(sender, e);
		}
	}

	private void SetLabelPrice()
	{
		if (this.priceLabel == null || (int)this.priceLabel.Length == 0)
		{
			return;
		}
		string str = string.Format("{0} {1}", this._price, LocalizationStore.Get("Key_1041"));
		for (int i = 0; i < (int)this.priceLabel.Length; i++)
		{
			this.priceLabel[i].text = str;
		}
	}

	private void Start()
	{
		if (this.closeButton != null)
		{
			this.closeButton.Clicked += new EventHandler(this.RaiseClosePressed);
		}
		if (this.unlockButton != null)
		{
			this.unlockButton.Clicked += new EventHandler(this.RaiseUnlockPressed);
		}
		if (this.priceSprite != null)
		{
			this.priceSprite.spriteName = string.Format("premium_baner_{0}", this._price);
		}
		this.SetLabelPrice();
	}

	public event EventHandler ClosePressed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.ClosePressed += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.ClosePressed -= value;
		}
	}

	public event EventHandler UnlockPressed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.UnlockPressed += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.UnlockPressed -= value;
		}
	}
}