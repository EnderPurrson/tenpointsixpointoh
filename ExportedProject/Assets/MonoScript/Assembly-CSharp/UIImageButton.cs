using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	public UISprite target;

	public string normalSprite;

	public string hoverSprite;

	public string pressedSprite;

	public string disabledSprite;

	public bool pixelSnap = true;

	public bool isEnabled
	{
		get
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			return (!component ? false : component.enabled);
		}
		set
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			if (!component)
			{
				return;
			}
			if (component.enabled != value)
			{
				component.enabled = value;
				this.UpdateImage();
			}
		}
	}

	public UIImageButton()
	{
	}

	private void OnEnable()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
		this.UpdateImage();
	}

	private void OnHover(bool isOver)
	{
		if (this.isEnabled && this.target != null)
		{
			this.SetSprite((!isOver ? this.normalSprite : this.hoverSprite));
		}
	}

	private void OnPress(bool pressed)
	{
		if (!pressed)
		{
			this.UpdateImage();
		}
		else
		{
			this.SetSprite(this.pressedSprite);
		}
	}

	private void OnValidate()
	{
		if (this.target != null)
		{
			if (string.IsNullOrEmpty(this.normalSprite))
			{
				this.normalSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.hoverSprite))
			{
				this.hoverSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.pressedSprite))
			{
				this.pressedSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.disabledSprite))
			{
				this.disabledSprite = this.target.spriteName;
			}
		}
	}

	private void SetSprite(string sprite)
	{
		if (this.target.atlas == null || this.target.atlas.GetSprite(sprite) == null)
		{
			return;
		}
		this.target.spriteName = sprite;
		if (this.pixelSnap)
		{
			this.target.MakePixelPerfect();
		}
	}

	private void UpdateImage()
	{
		if (this.target != null)
		{
			if (!this.isEnabled)
			{
				this.SetSprite(this.disabledSprite);
			}
			else
			{
				this.SetSprite((!UICamera.IsHighlighted(base.gameObject) ? this.normalSprite : this.hoverSprite));
			}
		}
	}
}