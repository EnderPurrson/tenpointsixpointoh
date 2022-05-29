using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class ControlSizeController : MonoBehaviour
{
	public const string ControlsSizeKey = "Controls.Size";

	public ControlSizeView view;

	public UISprite _currentSprite;

	public ControlSizeController()
	{
	}

	private void Awake()
	{
		if (this.view != null && this.view.slider != null)
		{
			ControlSizeSlider component = this.view.slider.GetComponent<ControlSizeSlider>();
			if (component != null)
			{
				component.EnabledChanged += new EventHandler<ControlSizeSlider.EnabledChangedEventArgs>(this.HandleEnabledChanged);
			}
		}
		ButtonJoystickAdjust.PressedDown += new EventHandler<EventArgs>(this.HandleControlPressedDown);
		PressDetector.PressedDown += new EventHandler<EventArgs>(this.HandleControlPressedDown);
	}

	public static void ChangeLeftHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			Debug.Log(string.Concat("[Left Handed] button clicked: ", isChecked));
		}
		if (GlobalGameController.LeftHanded != isChecked)
		{
			GlobalGameController.LeftHanded = isChecked;
			PlayerPrefs.SetInt(Defs.LeftHandedSN, (!isChecked ? 0 : 1));
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
			if (!isChecked)
			{
				FlurryPluginWrapper.LogEvent("Left-handed Layout Enabled");
				if (Debug.isDebugBuild)
				{
					Debug.Log("Left-handed Layout Enabled");
				}
			}
		}
	}

	public void HandleCancelButton()
	{
		Debug.Log("[Cancel] Pressed.");
	}

	public void HandleControlButton(UISprite sprite, UISlider slider)
	{
	}

	private void HandleControlPressedDown(object sender, EventArgs e)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		Debug.Log("Pressed");
		GameObject gameObject = sender as GameObject;
		if (gameObject == null)
		{
			return;
		}
		UISprite component = gameObject.GetComponent<UISprite>();
		if (component == null)
		{
			return;
		}
		this.SetCurrentSprite(component, this.view.slider);
	}

	public void HandleDefaultButton()
	{
		Debug.Log("[Default] Pressed.");
		if (this.view == null || this.view.buttons == null)
		{
			Debug.LogWarning("view == null || view.buttons == null");
			return;
		}
		UISprite[] uISpriteArray = this.view.buttons;
		for (int i = 0; i < (int)uISpriteArray.Length; i++)
		{
			UISprite uISprite = uISpriteArray[i];
			if (uISprite != null)
			{
				ControlSize component = uISprite.GetComponent<ControlSize>();
				if (component != null)
				{
					uISprite.width = component.defaultValue;
				}
			}
		}
	}

	private void HandleEnabledChanged(object sender, ControlSizeSlider.EnabledChangedEventArgs e)
	{
		if (e.Enabled)
		{
			this.LoadControlSize();
			this.SetCurrentSprite(this.view.buttons[0], this.view.slider);
		}
	}

	public void HandleSaveButton()
	{
		Debug.Log("[Save] Pressed.");
		this.SaveControlSize();
	}

	public void HandleSizeSliderChanged(UISlider slider)
	{
		if (this._currentSprite == null)
		{
			Debug.LogWarning("_currentSprite == null");
			return;
		}
		ControlSize component = this._currentSprite.GetComponent<ControlSize>();
		if (component == null)
		{
			Debug.LogWarning("cs == null");
			return;
		}
		this._currentSprite.width = Mathf.RoundToInt(Mathf.Lerp((float)component.minValue, (float)component.maxValue, slider.@value));
	}

	public void LoadControlSize()
	{
		if (this.view == null || this.view.buttons == null)
		{
			Debug.LogWarning("view == null || view.buttons == null");
			return;
		}
		object obj = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]"));
		List<object> objs = obj as List<object>;
		if (objs == null)
		{
			objs = new List<object>((int)this.view.buttons.Length);
			Debug.LogWarning(objs.GetType().FullName);
		}
		int length = (int)this.view.buttons.Length;
		for (int i = 0; i != length; i++)
		{
			if (this.view.buttons[i] != null)
			{
				int num = 0;
				if (i < objs.Count)
				{
					num = Convert.ToInt32(objs[i]);
				}
				this.view.buttons[i].width = (num <= 0 ? this.view.buttons[i].GetComponent<ControlSize>().defaultValue : num);
			}
		}
	}

	private void OnDestroy()
	{
		if (this.view != null && this.view.slider != null)
		{
			ControlSizeSlider component = this.view.slider.GetComponent<ControlSizeSlider>();
			if (component != null)
			{
				component.EnabledChanged -= new EventHandler<ControlSizeSlider.EnabledChangedEventArgs>(this.HandleEnabledChanged);
			}
		}
		ButtonJoystickAdjust.PressedDown -= new EventHandler<EventArgs>(this.HandleControlPressedDown);
		PressDetector.PressedDown -= new EventHandler<EventArgs>(this.HandleControlPressedDown);
	}

	private void RefreshSlider(UISlider slider)
	{
		if (slider == null)
		{
			return;
		}
		if (this._currentSprite != null)
		{
			ControlSize component = this._currentSprite.GetComponent<ControlSize>();
			if (component != null)
			{
				slider.@value = Mathf.InverseLerp((float)component.minValue, (float)component.maxValue, (float)this._currentSprite.width);
			}
			else
			{
				slider.@value = 0f;
			}
		}
		else
		{
			slider.@value = 0f;
		}
	}

	public void SaveControlSize()
	{
		if (this.view == null)
		{
			Debug.LogWarning("view == null");
			return;
		}
		Func<UISprite, int> func = (UISprite s) => (s == null ? 0 : s.width);
		int[] array = this.view.buttons.Select<UISprite, int>(func).ToArray<int>();
		PlayerPrefs.SetString("Controls.Size", Json.Serialize(array));
	}

	private void SetCurrentSprite(UISprite sprite, UISlider slider)
	{
		this._currentSprite = sprite;
		IEnumerator<UISprite> enumerator = (
			from b in (IEnumerable<UISprite>)this.view.buttons
			where b != null
			select b).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UISprite current = enumerator.Current;
				UISprite[] componentsInChildren = current.gameObject.GetComponentsInChildren<UISprite>();
				if ((int)componentsInChildren.Length != 0)
				{
					if (current.gameObject != sprite.gameObject)
					{
						UISprite[] uISpriteArray = componentsInChildren;
						for (int i = 0; i < (int)uISpriteArray.Length; i++)
						{
							uISpriteArray[i].color = Color.white;
						}
					}
					else
					{
						UISprite[] uISpriteArray1 = componentsInChildren;
						for (int j = 0; j < (int)uISpriteArray1.Length; j++)
						{
							uISpriteArray1[j].color = Color.red;
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		this.RefreshSlider(slider);
	}
}