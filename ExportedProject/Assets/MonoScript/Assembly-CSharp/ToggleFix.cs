using System;
using UnityEngine;

public class ToggleFix : MonoBehaviour
{
	public UIToggle toggle;

	public UIButton button;

	public UISprite background;

	public UISprite checkmark;

	public bool oldState;

	public bool firstUpdate = true;

	public ToggleFix()
	{
	}

	private void OnPress()
	{
		UISprite color = this.checkmark;
		float single = this.checkmark.color.r;
		float single1 = this.checkmark.color.g;
		Color color1 = this.checkmark.color;
		color.color = new Color(single, single1, color1.b, 0f);
		UISprite uISprite = this.background;
		float single2 = this.background.color.r;
		float single3 = this.background.color.g;
		Color color2 = this.background.color;
		uISprite.color = new Color(single2, single3, color2.b, 1f);
	}

	private void Start()
	{
		this.button = base.GetComponent<UIButton>();
	}

	private void Update()
	{
		if (this.button.state != UIButtonColor.State.Pressed)
		{
			this.checkmark.color = new Color(this.checkmark.color.r, this.checkmark.color.g, this.checkmark.color.b, (!this.toggle.@value ? 0f : 1f));
			this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, (!this.toggle.@value ? 1f : 0f));
		}
	}
}