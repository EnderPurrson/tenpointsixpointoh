using System;
using UnityEngine;

[RequireComponent(typeof(UISprite))]
[RequireComponent(typeof(UIToggle))]
public class RilisoftAlphaToggle : MonoBehaviour
{
	[Range(0f, 1f)]
	public float alphaOnState;

	[Range(0f, 1f)]
	public float alphaOffState;

	private UIToggle _toggle;

	private UISprite _toggledSprite;

	public RilisoftAlphaToggle()
	{
	}

	public void OnAlphaChange()
	{
		if (!this._toggle.@value)
		{
			this._toggledSprite.alpha = this.alphaOffState;
		}
		else
		{
			this._toggledSprite.alpha = this.alphaOnState;
		}
	}

	private void OnDestroy()
	{
		if (this._toggle != null)
		{
			EventDelegate.Remove(this._toggle.onChange, new EventDelegate.Callback(this.OnAlphaChange));
		}
	}

	private void Start()
	{
		this._toggle = base.GetComponent<UIToggle>();
		this._toggledSprite = this._toggle.GetComponent<UISprite>();
		if (this._toggle != null && this._toggledSprite != null)
		{
			this.OnAlphaChange();
			EventDelegate.Add(this._toggle.onChange, new EventDelegate.Callback(this.OnAlphaChange));
		}
	}
}