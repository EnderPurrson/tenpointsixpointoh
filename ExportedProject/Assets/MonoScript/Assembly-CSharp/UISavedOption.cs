using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	public string keyName;

	private UIPopupList mList;

	private UIToggle mCheck;

	private UIProgressBar mSlider;

	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(this.keyName) ? this.keyName : string.Concat("NGUI State: ", base.name));
		}
	}

	public UISavedOption()
	{
	}

	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.mCheck = base.GetComponent<UIToggle>();
		this.mSlider = base.GetComponent<UIProgressBar>();
	}

	private void OnDisable()
	{
		if (this.mCheck != null)
		{
			EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
		}
		else if (this.mList != null)
		{
			EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
		}
		else if (this.mSlider == null)
		{
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int num = 0;
			int length = (int)componentsInChildren.Length;
			while (num < length)
			{
				UIToggle uIToggle = componentsInChildren[num];
				if (!uIToggle.@value)
				{
					num++;
				}
				else
				{
					PlayerPrefs.SetString(this.key, uIToggle.name);
					break;
				}
			}
		}
		else
		{
			EventDelegate.Remove(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
		}
	}

	private void OnEnable()
	{
		if (this.mList != null)
		{
			EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
			string str = PlayerPrefs.GetString(this.key);
			if (!string.IsNullOrEmpty(str))
			{
				this.mList.@value = str;
			}
		}
		else if (this.mCheck != null)
		{
			EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
			this.mCheck.@value = PlayerPrefs.GetInt(this.key, (!this.mCheck.startsActive ? 0 : 1)) != 0;
		}
		else if (this.mSlider == null)
		{
			string str1 = PlayerPrefs.GetString(this.key);
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int num = 0;
			int length = (int)componentsInChildren.Length;
			while (num < length)
			{
				UIToggle uIToggle = componentsInChildren[num];
				uIToggle.@value = uIToggle.name == str1;
				num++;
			}
		}
		else
		{
			EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
			this.mSlider.@value = PlayerPrefs.GetFloat(this.key, this.mSlider.@value);
		}
	}

	public void SaveProgress()
	{
		PlayerPrefs.SetFloat(this.key, UIProgressBar.current.@value);
	}

	public void SaveSelection()
	{
		PlayerPrefs.SetString(this.key, UIPopupList.current.@value);
	}

	public void SaveState()
	{
		PlayerPrefs.SetInt(this.key, (!UIToggle.current.@value ? 0 : 1));
	}
}