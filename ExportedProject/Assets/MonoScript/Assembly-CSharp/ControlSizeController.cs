using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.MiniJson;
using UnityEngine;

internal sealed class ControlSizeController : MonoBehaviour
{
	public const string ControlsSizeKey = "Controls.Size";

	public ControlSizeView view;

	public UISprite _currentSprite;

	[CompilerGenerated]
	private static Func<UISprite, int> _003C_003Ef__am_0024cache2;

	[CompilerGenerated]
	private static Func<UISprite, bool> _003C_003Ef__am_0024cache3;

	public void HandleSizeSliderChanged(UISlider slider)
	{
		if (_currentSprite == null)
		{
			Debug.LogWarning("_currentSprite == null");
			return;
		}
		ControlSize component = _currentSprite.GetComponent<ControlSize>();
		if (component == null)
		{
			Debug.LogWarning("cs == null");
		}
		else
		{
			_currentSprite.width = Mathf.RoundToInt(Mathf.Lerp(component.minValue, component.maxValue, slider.value));
		}
	}

	private void HandleControlPressedDown(object sender, EventArgs e)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		Debug.Log("Pressed");
		GameObject gameObject = sender as GameObject;
		if (!(gameObject == null))
		{
			UISprite component = gameObject.GetComponent<UISprite>();
			if (!(component == null))
			{
				SetCurrentSprite(component, view.slider);
			}
		}
	}

	public void HandleControlButton(UISprite sprite, UISlider slider)
	{
	}

	public void HandleSaveButton()
	{
		Debug.Log("[Save] Pressed.");
		SaveControlSize();
	}

	public void HandleDefaultButton()
	{
		Debug.Log("[Default] Pressed.");
		if (view == null || view.buttons == null)
		{
			Debug.LogWarning("view == null || view.buttons == null");
			return;
		}
		UISprite[] buttons = view.buttons;
		foreach (UISprite uISprite in buttons)
		{
			if (!(uISprite == null))
			{
				ControlSize component = uISprite.GetComponent<ControlSize>();
				if (!(component == null))
				{
					uISprite.width = component.defaultValue;
				}
			}
		}
	}

	public void HandleCancelButton()
	{
		Debug.Log("[Cancel] Pressed.");
	}

	public void LoadControlSize()
	{
		if (view == null || view.buttons == null)
		{
			Debug.LogWarning("view == null || view.buttons == null");
			return;
		}
		object obj = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]"));
		List<object> list = obj as List<object>;
		if (list == null)
		{
			list = new List<object>(view.buttons.Length);
			Debug.LogWarning(list.GetType().FullName);
		}
		int num = view.buttons.Length;
		for (int i = 0; i != num; i++)
		{
			if (!(view.buttons[i] == null))
			{
				int num2 = 0;
				if (i < list.Count)
				{
					num2 = Convert.ToInt32(list[i]);
				}
				view.buttons[i].width = ((num2 <= 0) ? view.buttons[i].GetComponent<ControlSize>().defaultValue : num2);
			}
		}
	}

	public void SaveControlSize()
	{
		if (view == null)
		{
			Debug.LogWarning("view == null");
			return;
		}
		if (_003C_003Ef__am_0024cache2 == null)
		{
			_003C_003Ef__am_0024cache2 = _003CSaveControlSize_003Em__287;
		}
		Func<UISprite, int> selector = _003C_003Ef__am_0024cache2;
		int[] obj = view.buttons.Select(selector).ToArray();
		PlayerPrefs.SetString("Controls.Size", Json.Serialize(obj));
	}

	private void Awake()
	{
		if (view != null && view.slider != null)
		{
			ControlSizeSlider component = view.slider.GetComponent<ControlSizeSlider>();
			if (component != null)
			{
				component.EnabledChanged += HandleEnabledChanged;
			}
		}
		ButtonJoystickAdjust.PressedDown = (EventHandler<EventArgs>)Delegate.Combine(ButtonJoystickAdjust.PressedDown, new EventHandler<EventArgs>(HandleControlPressedDown));
		PressDetector.PressedDown = (EventHandler<EventArgs>)Delegate.Combine(PressDetector.PressedDown, new EventHandler<EventArgs>(HandleControlPressedDown));
	}

	private void OnDestroy()
	{
		if (view != null && view.slider != null)
		{
			ControlSizeSlider component = view.slider.GetComponent<ControlSizeSlider>();
			if (component != null)
			{
				component.EnabledChanged -= HandleEnabledChanged;
			}
		}
		ButtonJoystickAdjust.PressedDown = (EventHandler<EventArgs>)Delegate.Remove(ButtonJoystickAdjust.PressedDown, new EventHandler<EventArgs>(HandleControlPressedDown));
		PressDetector.PressedDown = (EventHandler<EventArgs>)Delegate.Remove(PressDetector.PressedDown, new EventHandler<EventArgs>(HandleControlPressedDown));
	}

	private void HandleEnabledChanged(object sender, ControlSizeSlider.EnabledChangedEventArgs e)
	{
		if (e.Enabled)
		{
			LoadControlSize();
			SetCurrentSprite(view.buttons[0], view.slider);
		}
	}

	public static void ChangeLeftHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Left Handed] button clicked: " + isChecked);
		}
		if (GlobalGameController.LeftHanded == isChecked)
		{
			return;
		}
		GlobalGameController.LeftHanded = isChecked;
		PlayerPrefs.SetInt(Defs.LeftHandedSN, isChecked ? 1 : 0);
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

	private void RefreshSlider(UISlider slider)
	{
		if (slider == null)
		{
			return;
		}
		if (_currentSprite == null)
		{
			slider.value = 0f;
			return;
		}
		ControlSize component = _currentSprite.GetComponent<ControlSize>();
		if (component == null)
		{
			slider.value = 0f;
		}
		else
		{
			slider.value = Mathf.InverseLerp(component.minValue, component.maxValue, _currentSprite.width);
		}
	}

	private void SetCurrentSprite(UISprite sprite, UISlider slider)
	{
		_currentSprite = sprite;
		UISprite[] buttons = view.buttons;
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = _003CSetCurrentSprite_003Em__288;
		}
		foreach (UISprite item in buttons.Where(_003C_003Ef__am_0024cache3))
		{
			UISprite[] componentsInChildren = item.gameObject.GetComponentsInChildren<UISprite>();
			if (componentsInChildren.Length == 0)
			{
				continue;
			}
			if (item.gameObject == sprite.gameObject)
			{
				UISprite[] array = componentsInChildren;
				foreach (UISprite uISprite in array)
				{
					uISprite.color = Color.red;
				}
			}
			else
			{
				UISprite[] array2 = componentsInChildren;
				foreach (UISprite uISprite2 in array2)
				{
					uISprite2.color = Color.white;
				}
			}
		}
		RefreshSlider(slider);
	}

	[CompilerGenerated]
	private static int _003CSaveControlSize_003Em__287(UISprite s)
	{
		return (s != null) ? s.width : 0;
	}

	[CompilerGenerated]
	private static bool _003CSetCurrentSprite_003Em__288(UISprite b)
	{
		return b != null;
	}
}
