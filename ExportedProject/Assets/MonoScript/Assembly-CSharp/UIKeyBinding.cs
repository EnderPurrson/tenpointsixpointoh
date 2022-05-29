using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	private static List<UIKeyBinding> mList;

	public KeyCode keyCode;

	public UIKeyBinding.Modifier modifier;

	public UIKeyBinding.Action action;

	[NonSerialized]
	private bool mIgnoreUp;

	[NonSerialized]
	private bool mIsInput;

	[NonSerialized]
	private bool mPress;

	static UIKeyBinding()
	{
		UIKeyBinding.mList = new List<UIKeyBinding>();
	}

	public UIKeyBinding()
	{
	}

	public static bool IsBound(KeyCode key)
	{
		int num = 0;
		int count = UIKeyBinding.mList.Count;
		while (num < count)
		{
			UIKeyBinding item = UIKeyBinding.mList[num];
			if (item != null && item.keyCode == key)
			{
				return true;
			}
			num++;
		}
		return false;
	}

	protected virtual bool IsModifierActive()
	{
		if (this.modifier == UIKeyBinding.Modifier.Any)
		{
			return true;
		}
		if (this.modifier == UIKeyBinding.Modifier.Alt)
		{
			if (UICamera.GetKey(308) || UICamera.GetKey(307))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Control)
		{
			if (UICamera.GetKey(306) || UICamera.GetKey(305))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Shift)
		{
			if (UICamera.GetKey(304) || UICamera.GetKey(303))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.None)
		{
			return (UICamera.GetKey(308) || UICamera.GetKey(307) || UICamera.GetKey(306) || UICamera.GetKey(305) || UICamera.GetKey(304) ? false : !UICamera.GetKey(303));
		}
		return false;
	}

	protected virtual void OnBindingClick()
	{
		UICamera.Notify(base.gameObject, "OnClick", null);
	}

	protected virtual void OnBindingPress(bool pressed)
	{
		UICamera.Notify(base.gameObject, "OnPress", pressed);
	}

	protected virtual void OnDisable()
	{
		UIKeyBinding.mList.Remove(this);
	}

	protected virtual void OnEnable()
	{
		UIKeyBinding.mList.Add(this);
	}

	protected virtual void OnSubmit()
	{
		if (UICamera.currentKey == this.keyCode && this.IsModifierActive())
		{
			this.mIgnoreUp = true;
		}
	}

	protected virtual void Start()
	{
		UIInput component = base.GetComponent<UIInput>();
		this.mIsInput = component != null;
		if (component != null)
		{
			UIKeyBinding uIKeyBinding = this;
			EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(uIKeyBinding.OnSubmit));
		}
	}

	protected virtual void Update()
	{
		if (UICamera.inputHasFocus)
		{
			return;
		}
		if (this.keyCode == KeyCode.None || !this.IsModifierActive())
		{
			return;
		}
		bool getKeyDown = UICamera.GetKeyDown(this.keyCode);
		bool getKeyUp = UICamera.GetKeyUp(this.keyCode);
		if (getKeyDown)
		{
			this.mPress = true;
		}
		if (this.action == UIKeyBinding.Action.PressAndClick || this.action == UIKeyBinding.Action.All)
		{
			if (getKeyDown)
			{
				UICamera.currentKey = this.keyCode;
				this.OnBindingPress(true);
			}
			if (this.mPress && getKeyUp)
			{
				UICamera.currentKey = this.keyCode;
				this.OnBindingPress(false);
				this.OnBindingClick();
			}
		}
		if ((this.action == UIKeyBinding.Action.Select || this.action == UIKeyBinding.Action.All) && getKeyUp)
		{
			if (this.mIsInput)
			{
				if (!this.mIgnoreUp && !UICamera.inputHasFocus && this.mPress)
				{
					UICamera.selectedObject = base.gameObject;
				}
				this.mIgnoreUp = false;
			}
			else if (this.mPress)
			{
				UICamera.hoveredObject = base.gameObject;
			}
		}
		if (getKeyUp)
		{
			this.mPress = false;
		}
	}

	public enum Action
	{
		PressAndClick,
		Select,
		All
	}

	public enum Modifier
	{
		Any,
		Shift,
		Control,
		Alt,
		None
	}
}