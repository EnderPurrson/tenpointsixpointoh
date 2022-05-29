using System;
using UnityEngine;

public class KeybordShow : MonoBehaviour
{
	private TouchScreenKeyboard mKeyboard;

	public bool mKeybordHold = true;

	public int maxChars = 20;

	public GameObject CF;

	public UILabel _Uil;

	private string mText = string.Empty;

	public KeybordShow()
	{
	}

	private void OnClick()
	{
		this.mKeyboard = TouchScreenKeyboard.Open(this.mText, TouchScreenKeyboardType.Default, false);
	}

	private void Start()
	{
		this._Uil = this.CF.GetComponent<UILabel>();
		Vector3 cF = this.CF.transform.position;
		this.CF.transform.position = new Vector3(posNGUI.getPosX(0f), posNGUI.getPosY(0f), cF.z);
		this._Uil.lineWidth = Screen.width;
		this.mKeyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default, false, false);
	}

	private void Update()
	{
		if (this.mKeyboard != null)
		{
			string str = this.mKeyboard.text;
			if (this.mText != str)
			{
				this.mText = string.Empty;
				for (int i = 0; i < str.Length; i++)
				{
					char chr = str[i];
					if (chr != 0)
					{
						KeybordShow keybordShow = this;
						keybordShow.mText = string.Concat(keybordShow.mText, chr);
					}
				}
				if (this.maxChars > 0 && this.mKeyboard.text.Length > this.maxChars)
				{
					this.mKeyboard.text = this.mKeyboard.text.Substring(0, this.maxChars);
				}
				if (this.mText != str)
				{
					this.mKeyboard.text = this.mText;
				}
				base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
			}
			this.mKeyboard.active = true;
			if (this.mKeyboard.done)
			{
				this.mKeyboard.active = true;
				if (string.IsNullOrEmpty(this.mText))
				{
					this._Uil.text = string.Concat(this.mText, '\n', this._Uil.text);
					this.mText = string.Empty;
				}
				if (!this.mKeybordHold)
				{
					this.mKeyboard.active = false;
					this.mKeyboard = null;
				}
			}
			else if (this.mKeyboard.wasCanceled)
			{
				this.mKeyboard.active = false;
				this.mKeyboard = null;
			}
		}
	}
}