using System;
using UnityEngine;

internal sealed class AdjustSpriteToLabel : MonoBehaviour
{
	public UILabel label;

	[Range(0f, 100f)]
	public float padding = 30f;

	private UISprite _sprite;

	public AdjustSpriteToLabel()
	{
	}

	private void Start()
	{
		this._sprite = base.GetComponent<UISprite>();
		if (this.label == null)
		{
			this.label = base.transform.parent.GetComponent<UILabel>();
		}
		if (this._sprite == null)
		{
			Debug.LogWarning("sprite == null");
		}
		if (this.label == null)
		{
			Debug.LogWarning("label == null");
		}
	}

	private void Update()
	{
		if (this._sprite == null)
		{
			return;
		}
		if (this.label == null)
		{
			return;
		}
		this._sprite.transform.localPosition = new Vector3(this.padding + 0.5f * (float)this.label.width, 0f, 0f);
	}
}