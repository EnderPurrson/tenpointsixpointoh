using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MyUIInput : UIInput
{
	[NonSerialized]
	public float heightKeyboard;

	public Action onKeyboardInter;

	public Action onKeyboardCancel;

	public Action onKeyboardVisible;

	public Action onKeyboardHide;

	private bool isKeyboardVisible;

	private float timerlog = 0.3f;

	private bool _selectAfterPause;

	public MyUIInput()
	{
	}

	private void Awake()
	{
		this.hideInput = false;
	}

	public void DeselectInput()
	{
		this.OnDeselectEventCustom();
	}

	public float GetKeyboardHeight()
	{
		return this.heightKeyboard;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			if (this._selectAfterPause)
			{
				base.isSelected = true;
			}
			this._selectAfterPause = false;
		}
		else
		{
			this._selectAfterPause = base.isSelected;
			if (base.isSelected)
			{
				base.isSelected = false;
			}
		}
	}

	protected void OnDeselectEventCustom()
	{
		if (this.mDoInit)
		{
			base.Init();
		}
		if (UIInput.mKeyboard != null)
		{
			UIInput.mKeyboard.active = false;
			UIInput.mKeyboard = null;
		}
		if (this.label != null)
		{
			this.mValue = base.@value;
			if (!string.IsNullOrEmpty(this.mValue))
			{
				this.label.text = this.mValue;
			}
			else
			{
				this.label.text = this.mDefaultText;
				this.label.color = this.mDefaultColor;
			}
			Input.imeCompositionMode = IMECompositionMode.Auto;
			this.label.alignment = this.mAlignment;
		}
		base.isSelected = false;
		UIInput.selection = null;
		base.UpdateLabel();
	}

	private void OnDestroy()
	{
		base.OnSelect(false);
		this.DeselectInput();
	}

	private void OnDeviceOrientationChanged(DeviceOrientation ori)
	{
		if (base.isSelected)
		{
			base.StartCoroutine(this.ReSelect());
		}
	}

	private void OnDisable()
	{
		DeviceOrientationMonitor.OnOrientationChange -= new Action<DeviceOrientation>(this.OnDeviceOrientationChanged);
		base.OnSelect(false);
		this.DeselectInput();
		base.Cleanup();
	}

	private void OnEnable()
	{
		DeviceOrientationMonitor.OnOrientationChange += new Action<DeviceOrientation>(this.OnDeviceOrientationChanged);
	}

	protected override void OnSelect(bool isSelected)
	{
		if (isSelected)
		{
			base.OnSelectEvent();
		}
		else if (!this.hideInput)
		{
			base.OnDeselectEvent();
		}
	}

	[DebuggerHidden]
	private IEnumerator ReSelect()
	{
		MyUIInput.u003cReSelectu003ec__IteratorBE variable = null;
		return variable;
	}

	private void SetKeyboardHeight()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView", new object[0]);
			using (AndroidJavaObject androidJavaObject1 = new AndroidJavaObject("android.graphics.Rect", new object[0]))
			{
				androidJavaObject.Call("getWindowVisibleDisplayFrame", new object[] { androidJavaObject1 });
				this.heightKeyboard = (float)(Screen.height - androidJavaObject1.Call<int>("height", new object[0]));
			}
		}
	}

	private new void Update()
	{
		if (Application.isEditor)
		{
			if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown("enter")) && this.onKeyboardInter != null)
			{
				this.onKeyboardInter();
			}
			if (Input.GetKeyDown(KeyCode.KeypadPlus) && this.onKeyboardVisible != null)
			{
				this.onKeyboardVisible();
			}
			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				if (this.onKeyboardHide != null)
				{
					this.onKeyboardHide();
				}
				this.DeselectInput();
			}
		}
		base.Update();
	}
}