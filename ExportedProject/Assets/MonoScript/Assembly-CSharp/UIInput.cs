using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Input Field")]
public class UIInput : MonoBehaviour
{
	public static UIInput current;

	public static UIInput selection;

	public UILabel label;

	public UIInput.InputType inputType;

	public UIInput.OnReturnKey onReturnKey;

	public UIInput.KeyboardType keyboardType;

	public bool hideInput;

	[NonSerialized]
	public bool selectAllTextOnFocus = true;

	public UIInput.Validation validation;

	public int characterLimit;

	public string savedAs;

	[HideInInspector]
	[SerializeField]
	private GameObject selectOnTab;

	public Color activeTextColor = Color.white;

	public Color caretColor = new Color(1f, 1f, 1f, 0.8f);

	public Color selectionColor = new Color(1f, 0.8745098f, 0.5529412f, 0.5f);

	public List<EventDelegate> onSubmit = new List<EventDelegate>();

	public List<EventDelegate> onChange = new List<EventDelegate>();

	public UIInput.OnValidate onValidate;

	[HideInInspector]
	[SerializeField]
	protected string mValue;

	[NonSerialized]
	protected string mDefaultText = string.Empty;

	[NonSerialized]
	protected Color mDefaultColor = Color.white;

	[NonSerialized]
	protected float mPosition;

	[NonSerialized]
	protected bool mDoInit = true;

	[NonSerialized]
	protected NGUIText.Alignment mAlignment = NGUIText.Alignment.Left;

	[NonSerialized]
	protected bool mLoadSavedValue = true;

	protected static int mDrawStart;

	protected static string mLastIME;

	protected static TouchScreenKeyboard mKeyboard;

	private static bool mWaitForKeyboard;

	[NonSerialized]
	protected int mSelectionStart;

	[NonSerialized]
	protected int mSelectionEnd;

	[NonSerialized]
	protected UITexture mHighlight;

	[NonSerialized]
	protected UITexture mCaret;

	[NonSerialized]
	protected Texture2D mBlankTex;

	[NonSerialized]
	protected float mNextBlink;

	[NonSerialized]
	protected float mLastAlpha;

	[NonSerialized]
	protected string mCached = string.Empty;

	[NonSerialized]
	protected int mSelectMe = -1;

	[NonSerialized]
	protected int mSelectTime = -1;

	[NonSerialized]
	private UICamera mCam;

	[NonSerialized]
	private bool mEllipsis;

	private static int mIgnoreKey;

	public UITexture caret
	{
		get
		{
			return this.mCaret;
		}
	}

	public int cursorPosition
	{
		get
		{
			if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
			{
				return this.@value.Length;
			}
			return (!this.isSelected ? this.@value.Length : this.mSelectionEnd);
		}
		set
		{
			if (this.isSelected)
			{
				if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
				{
					return;
				}
				this.mSelectionEnd = value;
				this.UpdateLabel();
			}
		}
	}

	public Color defaultColor
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mDefaultColor;
		}
		set
		{
			this.mDefaultColor = value;
			if (!this.isSelected)
			{
				this.label.color = value;
			}
		}
	}

	public string defaultText
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mDefaultText;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			this.mDefaultText = value;
			this.UpdateLabel();
		}
	}

	public bool inputShouldBeHidden
	{
		get
		{
			return (!this.hideInput || !(this.label != null) || this.label.multiLine ? false : this.inputType != UIInput.InputType.Password);
		}
	}

	public bool isSelected
	{
		get
		{
			return UIInput.selection == this;
		}
		set
		{
			if (value)
			{
				UICamera.selectedObject = base.gameObject;
			}
			else if (this.isSelected)
			{
				UICamera.selectedObject = null;
			}
		}
	}

	[Obsolete("Use UIInput.isSelected instead")]
	public bool selected
	{
		get
		{
			return this.isSelected;
		}
		set
		{
			this.isSelected = value;
		}
	}

	public int selectionEnd
	{
		get
		{
			if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
			{
				return this.@value.Length;
			}
			return (!this.isSelected ? this.@value.Length : this.mSelectionEnd);
		}
		set
		{
			if (this.isSelected)
			{
				if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
				{
					return;
				}
				this.mSelectionEnd = value;
				this.UpdateLabel();
			}
		}
	}

	public int selectionStart
	{
		get
		{
			if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
			{
				return 0;
			}
			return (!this.isSelected ? this.@value.Length : this.mSelectionStart);
		}
		set
		{
			if (this.isSelected)
			{
				if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
				{
					return;
				}
				this.mSelectionStart = value;
				this.UpdateLabel();
			}
		}
	}

	[Obsolete("Use UIInput.value instead")]
	public string text
	{
		get
		{
			return this.@value;
		}
		set
		{
			this.@value = value;
		}
	}

	public string @value
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mValue;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			UIInput.mDrawStart = 0;
			if (Application.platform == RuntimePlatform.BlackBerryPlayer)
			{
				value = value.Replace("\\b", "\b");
			}
			value = this.Validate(value);
			if (this.isSelected && UIInput.mKeyboard != null && this.mCached != value)
			{
				UIInput.mKeyboard.text = value;
				this.mCached = value;
			}
			if (this.mValue != value)
			{
				this.mValue = value;
				this.mLoadSavedValue = false;
				if (!this.isSelected)
				{
					this.SaveToPlayerPrefs(value);
				}
				else if (!string.IsNullOrEmpty(value))
				{
					this.mSelectionStart = value.Length;
					this.mSelectionEnd = this.mSelectionStart;
				}
				else
				{
					this.mSelectionStart = 0;
					this.mSelectionEnd = 0;
				}
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
		}
	}

	static UIInput()
	{
		UIInput.mLastIME = string.Empty;
	}

	public UIInput()
	{
	}

	protected virtual void Cleanup()
	{
		if (this.mHighlight)
		{
			this.mHighlight.enabled = false;
		}
		if (this.mCaret)
		{
			this.mCaret.enabled = false;
		}
		if (this.mBlankTex)
		{
			NGUITools.Destroy(this.mBlankTex);
			this.mBlankTex = null;
		}
	}

	protected void DoBackspace()
	{
		if (!string.IsNullOrEmpty(this.mValue))
		{
			if (this.mSelectionStart == this.mSelectionEnd)
			{
				if (this.mSelectionStart < 1)
				{
					return;
				}
				this.mSelectionEnd--;
			}
			this.Insert(string.Empty);
		}
	}

	protected void ExecuteOnChange()
	{
		if (UIInput.current == null && EventDelegate.IsValid(this.onChange))
		{
			UIInput.current = this;
			EventDelegate.Execute(this.onChange);
			UIInput.current = null;
		}
	}

	protected int GetCharUnderMouse()
	{
		float single;
		Vector3[] vector3Array = this.label.worldCorners;
		Ray ray = UICamera.currentRay;
		Plane plane = new Plane(vector3Array[0], vector3Array[1], vector3Array[2]);
		return (!plane.Raycast(ray, out single) ? 0 : UIInput.mDrawStart + this.label.GetCharacterIndexAtPosition(ray.GetPoint(single), false));
	}

	protected string GetLeftText()
	{
		int num = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
		return (string.IsNullOrEmpty(this.mValue) || num < 0 ? string.Empty : this.mValue.Substring(0, num));
	}

	protected string GetRightText()
	{
		int num = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
		return (string.IsNullOrEmpty(this.mValue) || num >= this.mValue.Length ? string.Empty : this.mValue.Substring(num));
	}

	protected string GetSelection()
	{
		if (string.IsNullOrEmpty(this.mValue) || this.mSelectionStart == this.mSelectionEnd)
		{
			return string.Empty;
		}
		int num = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
		int num1 = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
		return this.mValue.Substring(num, num1 - num);
	}

	protected void Init()
	{
		if (this.mDoInit && this.label != null)
		{
			this.mDoInit = false;
			this.mDefaultText = this.label.text;
			this.mDefaultColor = this.label.color;
			this.label.supportEncoding = false;
			this.mEllipsis = this.label.overflowEllipsis;
			if (this.label.alignment == NGUIText.Alignment.Justified)
			{
				this.label.alignment = NGUIText.Alignment.Left;
				Debug.LogWarning("Input fields using labels with justified alignment are not supported at this time", this);
			}
			this.mAlignment = this.label.alignment;
			this.mPosition = this.label.cachedTransform.localPosition.x;
			this.UpdateLabel();
		}
	}

	protected virtual void Insert(string text)
	{
		string leftText = this.GetLeftText();
		string rightText = this.GetRightText();
		int length = rightText.Length;
		StringBuilder stringBuilder = new StringBuilder(leftText.Length + rightText.Length + text.Length);
		stringBuilder.Append(leftText);
		int num = 0;
		int length1 = text.Length;
		while (num < length1)
		{
			char str = text[num];
			if (str == '\b')
			{
				this.DoBackspace();
			}
			else if (this.characterLimit <= 0 || stringBuilder.Length + length < this.characterLimit)
			{
				if (this.onValidate != null)
				{
					str = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, str);
				}
				else if (this.validation != UIInput.Validation.None)
				{
					str = this.Validate(stringBuilder.ToString(), stringBuilder.Length, str);
				}
				if (str != 0)
				{
					stringBuilder.Append(str);
				}
			}
			else
			{
				break;
			}
			num++;
		}
		this.mSelectionStart = stringBuilder.Length;
		this.mSelectionEnd = this.mSelectionStart;
		int num1 = 0;
		int length2 = rightText.Length;
		while (num1 < length2)
		{
			char chr = rightText[num1];
			if (this.onValidate != null)
			{
				chr = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, chr);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				chr = this.Validate(stringBuilder.ToString(), stringBuilder.Length, chr);
			}
			if (chr != 0)
			{
				stringBuilder.Append(chr);
			}
			num1++;
		}
		this.mValue = stringBuilder.ToString();
		this.UpdateLabel();
		this.ExecuteOnChange();
	}

	public void LoadValue()
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			string str = this.mValue.Replace("\\n", "\n");
			this.mValue = string.Empty;
			this.@value = (!PlayerPrefs.HasKey(this.savedAs) ? str : PlayerPrefs.GetString(this.savedAs));
		}
	}

	protected void OnDeselectEvent()
	{
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null)
		{
			this.label.overflowEllipsis = this.mEllipsis;
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.mValue = this.@value;
			if (UIInput.mKeyboard != null)
			{
				UIInput.mWaitForKeyboard = false;
				UIInput.mKeyboard.active = false;
				UIInput.mKeyboard = null;
			}
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
		UIInput.selection = null;
		this.UpdateLabel();
	}

	private void OnDisable()
	{
		this.Cleanup();
	}

	protected virtual void OnDrag(Vector2 delta)
	{
		if (this.label != null && (UICamera.currentScheme == UICamera.ControlScheme.Mouse || UICamera.currentScheme == UICamera.ControlScheme.Touch))
		{
			this.selectionEnd = this.GetCharUnderMouse();
		}
	}

	private void OnKey(KeyCode key)
	{
		int num = Time.frameCount;
		if (UIInput.mIgnoreKey == num)
		{
			return;
		}
		if (this.mCam != null && (key == this.mCam.cancelKey0 || key == this.mCam.cancelKey1))
		{
			UIInput.mIgnoreKey = num;
			this.isSelected = false;
		}
		else if (key == KeyCode.Tab)
		{
			UIInput.mIgnoreKey = num;
			this.isSelected = false;
			UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
			if (component != null)
			{
				component.OnKey(KeyCode.Tab);
			}
		}
	}

	protected virtual void OnPress(bool isPressed)
	{
		if (isPressed && this.isSelected && this.label != null && (UICamera.currentScheme == UICamera.ControlScheme.Mouse || UICamera.currentScheme == UICamera.ControlScheme.Touch))
		{
			this.selectionEnd = this.GetCharUnderMouse();
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				this.selectionStart = this.mSelectionEnd;
			}
		}
	}

	protected virtual void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			this.OnDeselectEvent();
		}
		else
		{
			this.OnSelectEvent();
		}
	}

	protected void OnSelectEvent()
	{
		this.mSelectTime = Time.frameCount;
		UIInput.selection = this;
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null)
		{
			this.mEllipsis = this.label.overflowEllipsis;
			this.label.overflowEllipsis = false;
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.mSelectMe = Time.frameCount;
		}
	}

	public void RemoveFocus()
	{
		this.isSelected = false;
	}

	protected void SaveToPlayerPrefs(string val)
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			if (!string.IsNullOrEmpty(val))
			{
				PlayerPrefs.SetString(this.savedAs, val);
			}
			else
			{
				PlayerPrefs.DeleteKey(this.savedAs);
			}
		}
	}

	public void SaveValue()
	{
		this.SaveToPlayerPrefs(this.mValue);
	}

	private void Start()
	{
		if (this.selectOnTab != null)
		{
			if (base.GetComponent<UIKeyNavigation>() == null)
			{
				base.gameObject.AddComponent<UIKeyNavigation>().onDown = this.selectOnTab;
			}
			this.selectOnTab = null;
			NGUITools.SetDirty(this);
		}
		if (!this.mLoadSavedValue || string.IsNullOrEmpty(this.savedAs))
		{
			this.@value = this.mValue.Replace("\\n", "\n");
		}
		else
		{
			this.LoadValue();
		}
	}

	public void Submit()
	{
		if (NGUITools.GetActive(this))
		{
			this.mValue = this.@value;
			if (UIInput.current == null)
			{
				UIInput.current = this;
				EventDelegate.Execute(this.onSubmit);
				UIInput.current = null;
			}
			this.SaveToPlayerPrefs(this.mValue);
		}
	}

	protected virtual void Update()
	{
		string str;
		TouchScreenKeyboardType touchScreenKeyboardType;
		if (!this.isSelected || this.mSelectTime == Time.frameCount)
		{
			return;
		}
		if (this.mDoInit)
		{
			this.Init();
		}
		if (UIInput.mWaitForKeyboard)
		{
			if (UIInput.mKeyboard != null && !UIInput.mKeyboard.active)
			{
				return;
			}
			UIInput.mWaitForKeyboard = false;
		}
		if (this.mSelectMe != -1 && this.mSelectMe != Time.frameCount)
		{
			this.mSelectMe = -1;
			this.mSelectionEnd = (!string.IsNullOrEmpty(this.mValue) ? this.mValue.Length : 0);
			UIInput.mDrawStart = 0;
			this.mSelectionStart = (!this.selectAllTextOnFocus ? this.mSelectionEnd : 0);
			this.label.color = this.activeTextColor;
			RuntimePlatform runtimePlatform = Application.platform;
			if (runtimePlatform == RuntimePlatform.IPhonePlayer || runtimePlatform == RuntimePlatform.Android || runtimePlatform == RuntimePlatform.WP8Player || runtimePlatform == RuntimePlatform.BlackBerryPlayer || runtimePlatform == RuntimePlatform.MetroPlayerARM || runtimePlatform == RuntimePlatform.MetroPlayerX64 || runtimePlatform == RuntimePlatform.MetroPlayerX86)
			{
				if (this.inputShouldBeHidden)
				{
					TouchScreenKeyboard.hideInput = true;
					touchScreenKeyboardType = (TouchScreenKeyboardType)this.keyboardType;
					str = "|";
				}
				else if (this.inputType != UIInput.InputType.Password)
				{
					TouchScreenKeyboard.hideInput = false;
					touchScreenKeyboardType = (TouchScreenKeyboardType)this.keyboardType;
					str = this.mValue;
					this.mSelectionStart = this.mSelectionEnd;
				}
				else
				{
					TouchScreenKeyboard.hideInput = false;
					touchScreenKeyboardType = (TouchScreenKeyboardType)this.keyboardType;
					str = this.mValue;
					this.mSelectionStart = this.mSelectionEnd;
				}
				UIInput.mWaitForKeyboard = true;
				UIInput.mKeyboard = (this.inputType != UIInput.InputType.Password ? TouchScreenKeyboard.Open(str, touchScreenKeyboardType, (this.inputShouldBeHidden ? false : this.inputType == UIInput.InputType.AutoCorrect), (!this.label.multiLine ? false : !this.hideInput), false, false, this.defaultText) : TouchScreenKeyboard.Open(str, touchScreenKeyboardType, false, false, true));
			}
			else
			{
				Vector2 vector2 = (!(UICamera.current != null) || !(UICamera.current.cachedCamera != null) ? this.label.worldCorners[0] : UICamera.current.cachedCamera.WorldToScreenPoint(this.label.worldCorners[0]));
				vector2.y = (float)Screen.height - vector2.y;
				Input.imeCompositionMode = IMECompositionMode.On;
				Input.compositionCursorPos = vector2;
			}
			this.UpdateLabel();
			if (string.IsNullOrEmpty(Input.inputString))
			{
				return;
			}
		}
		if (UIInput.mKeyboard == null)
		{
			string str1 = Input.compositionString;
			if (string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(Input.inputString))
			{
				string str2 = Input.inputString;
				for (int i = 0; i < str2.Length; i++)
				{
					char chr = str2[i];
					if (chr >= ' ')
					{
						if (chr != '\uF700')
						{
							if (chr != '\uF701')
							{
								if (chr != '\uF702')
								{
									if (chr != '\uF703')
									{
										this.Insert(chr.ToString());
									}
								}
							}
						}
					}
				}
			}
			if (UIInput.mLastIME != str1)
			{
				this.mSelectionEnd = (!string.IsNullOrEmpty(str1) ? this.mValue.Length + str1.Length : this.mSelectionStart);
				UIInput.mLastIME = str1;
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
		}
		else
		{
			string str3 = (UIInput.mKeyboard.done || !UIInput.mKeyboard.active ? this.mCached : UIInput.mKeyboard.text);
			if (this.inputShouldBeHidden)
			{
				if (str3 != "|")
				{
					if (!string.IsNullOrEmpty(str3))
					{
						this.Insert(str3.Substring(1));
					}
					else if (!UIInput.mKeyboard.done && UIInput.mKeyboard.active)
					{
						this.DoBackspace();
					}
					UIInput.mKeyboard.text = "|";
				}
			}
			else if (this.mCached != str3)
			{
				this.mCached = str3;
				if (!UIInput.mKeyboard.done && UIInput.mKeyboard.active)
				{
					this.@value = str3;
				}
			}
			if (UIInput.mKeyboard.done || !UIInput.mKeyboard.active)
			{
				if (!UIInput.mKeyboard.wasCanceled)
				{
					this.Submit();
				}
				UIInput.mKeyboard = null;
				this.isSelected = false;
				this.mCached = string.Empty;
			}
		}
		if (this.mCaret != null && this.mNextBlink < RealTime.time)
		{
			this.mNextBlink = RealTime.time + 0.5f;
			this.mCaret.enabled = !this.mCaret.enabled;
		}
		if (this.isSelected && this.mLastAlpha != this.label.finalAlpha)
		{
			this.UpdateLabel();
		}
		if (this.mCam == null)
		{
			this.mCam = UICamera.FindCameraForLayer(base.gameObject.layer);
		}
		if (this.mCam != null)
		{
			bool flag = false;
			if (this.label.multiLine)
			{
				bool flag1 = (Input.GetKey(KeyCode.LeftControl) ? true : Input.GetKey(KeyCode.RightControl));
				flag = (this.onReturnKey != UIInput.OnReturnKey.Submit ? !flag1 : flag1);
			}
			if (UICamera.GetKeyDown(this.mCam.submitKey0))
			{
				if (!flag)
				{
					if (UICamera.controller.current != null)
					{
						UICamera.controller.clickNotification = UICamera.ClickNotification.None;
					}
					UICamera.currentKey = this.mCam.submitKey0;
					this.Submit();
				}
				else
				{
					this.Insert("\n");
				}
			}
			if (UICamera.GetKeyDown(this.mCam.submitKey1))
			{
				if (!flag)
				{
					if (UICamera.controller.current != null)
					{
						UICamera.controller.clickNotification = UICamera.ClickNotification.None;
					}
					UICamera.currentKey = this.mCam.submitKey1;
					this.Submit();
				}
				else
				{
					this.Insert("\n");
				}
			}
			if (!this.mCam.useKeyboard && UICamera.GetKeyUp(9))
			{
				this.OnKey(KeyCode.Tab);
			}
		}
	}

	public void UpdateLabel()
	{
		string empty;
		if (this.label != null)
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			bool flag = this.isSelected;
			string str = this.@value;
			bool flag1 = (!string.IsNullOrEmpty(str) ? false : string.IsNullOrEmpty(Input.compositionString));
			this.label.color = (!flag1 || flag ? this.activeTextColor : this.mDefaultColor);
			if (!flag1)
			{
				if (this.inputType != UIInput.InputType.Password)
				{
					empty = str;
				}
				else
				{
					empty = string.Empty;
					string str1 = "*";
					if (this.label.bitmapFont != null && this.label.bitmapFont.bmFont != null && this.label.bitmapFont.bmFont.GetGlyph(42) == null)
					{
						str1 = "x";
					}
					int num = 0;
					int length = str.Length;
					while (num < length)
					{
						empty = string.Concat(empty, str1);
						num++;
					}
				}
				int num1 = (!flag ? 0 : Mathf.Min(empty.Length, this.cursorPosition));
				string str2 = empty.Substring(0, num1);
				if (flag)
				{
					str2 = string.Concat(str2, Input.compositionString);
				}
				empty = string.Concat(str2, empty.Substring(num1, empty.Length - num1));
				if (!flag || this.label.overflowMethod != UILabel.Overflow.ClampContent || this.label.maxLineCount != 1)
				{
					UIInput.mDrawStart = 0;
					this.label.alignment = this.mAlignment;
				}
				else
				{
					int fit = this.label.CalculateOffsetToFit(empty);
					if (fit == 0)
					{
						UIInput.mDrawStart = 0;
						this.label.alignment = this.mAlignment;
					}
					else if (num1 < UIInput.mDrawStart)
					{
						UIInput.mDrawStart = num1;
						this.label.alignment = NGUIText.Alignment.Left;
					}
					else if (fit >= UIInput.mDrawStart)
					{
						fit = this.label.CalculateOffsetToFit(empty.Substring(0, num1));
						if (fit > UIInput.mDrawStart)
						{
							UIInput.mDrawStart = fit;
							this.label.alignment = NGUIText.Alignment.Right;
						}
					}
					else
					{
						UIInput.mDrawStart = fit;
						this.label.alignment = NGUIText.Alignment.Left;
					}
					if (UIInput.mDrawStart != 0)
					{
						empty = empty.Substring(UIInput.mDrawStart, empty.Length - UIInput.mDrawStart);
					}
				}
			}
			else
			{
				empty = (!flag ? this.mDefaultText : string.Empty);
				this.label.alignment = this.mAlignment;
			}
			this.label.text = empty;
			if (!flag || UIInput.mKeyboard != null && !this.inputShouldBeHidden)
			{
				this.Cleanup();
			}
			else
			{
				int num2 = this.mSelectionStart - UIInput.mDrawStart;
				int num3 = this.mSelectionEnd - UIInput.mDrawStart;
				if (this.mBlankTex == null)
				{
					this.mBlankTex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
					for (int i = 0; i < 2; i++)
					{
						for (int j = 0; j < 2; j++)
						{
							this.mBlankTex.SetPixel(j, i, Color.white);
						}
					}
					this.mBlankTex.Apply();
				}
				if (num2 != num3)
				{
					if (this.mHighlight != null)
					{
						this.mHighlight.pivot = this.label.pivot;
						this.mHighlight.mainTexture = this.mBlankTex;
						this.mHighlight.MarkAsChanged();
						this.mHighlight.enabled = true;
					}
					else
					{
						this.mHighlight = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject, 2147483647);
						this.mHighlight.name = "Input Highlight";
						this.mHighlight.mainTexture = this.mBlankTex;
						this.mHighlight.fillGeometry = false;
						this.mHighlight.pivot = this.label.pivot;
						this.mHighlight.SetAnchor(this.label.cachedTransform);
					}
				}
				if (this.mCaret != null)
				{
					this.mCaret.pivot = this.label.pivot;
					this.mCaret.mainTexture = this.mBlankTex;
					this.mCaret.MarkAsChanged();
					this.mCaret.enabled = true;
				}
				else
				{
					this.mCaret = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject, 2147483647);
					this.mCaret.name = "Input Caret";
					this.mCaret.mainTexture = this.mBlankTex;
					this.mCaret.fillGeometry = false;
					this.mCaret.pivot = this.label.pivot;
					this.mCaret.SetAnchor(this.label.cachedTransform);
				}
				if (num2 == num3)
				{
					this.label.PrintOverlay(num2, num3, this.mCaret.geometry, null, this.caretColor, this.selectionColor);
					if (this.mHighlight != null)
					{
						this.mHighlight.enabled = false;
					}
				}
				else
				{
					this.label.PrintOverlay(num2, num3, this.mCaret.geometry, this.mHighlight.geometry, this.caretColor, this.selectionColor);
					this.mHighlight.enabled = this.mHighlight.geometry.hasVertices;
				}
				this.mNextBlink = RealTime.time + 0.5f;
				this.mLastAlpha = this.label.finalAlpha;
			}
		}
	}

	public string Validate(string val)
	{
		if (string.IsNullOrEmpty(val))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder(val.Length);
		for (int i = 0; i < val.Length; i++)
		{
			char str = val[i];
			if (this.onValidate != null)
			{
				str = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, str);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				str = this.Validate(stringBuilder.ToString(), stringBuilder.Length, str);
			}
			if (str != 0)
			{
				stringBuilder.Append(str);
			}
		}
		if (this.characterLimit <= 0 || stringBuilder.Length <= this.characterLimit)
		{
			return stringBuilder.ToString();
		}
		return stringBuilder.ToString(0, this.characterLimit);
	}

	protected char Validate(string text, int pos, char ch)
	{
		if (this.validation == UIInput.Validation.None || !base.enabled)
		{
			return ch;
		}
		if (this.validation == UIInput.Validation.Integer)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Float)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
			if (ch == '.' && !text.Contains("."))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Alphanumeric)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch;
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation != UIInput.Validation.Username)
		{
			if (this.validation == UIInput.Validation.Filename)
			{
				if (ch == ':')
				{
					return '\0';
				}
				if (ch == '/')
				{
					return '\0';
				}
				if (ch == '\\')
				{
					return '\0';
				}
				if (ch == '<')
				{
					return '\0';
				}
				if (ch == '>')
				{
					return '\0';
				}
				if (ch == '|')
				{
					return '\0';
				}
				if (ch == '\u005E')
				{
					return '\0';
				}
				if (ch == '*')
				{
					return '\0';
				}
				if (ch == ';')
				{
					return '\0';
				}
				if (ch == '\"')
				{
					return '\0';
				}
				if (ch == '\u0060')
				{
					return '\0';
				}
				if (ch == '\t')
				{
					return '\0';
				}
				if (ch == '\n')
				{
					return '\0';
				}
				return ch;
			}
			if (this.validation == UIInput.Validation.Name)
			{
				char chr = (text.Length <= 0 ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)]);
				char chr1 = (text.Length <= 0 ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)]);
				if (ch >= 'a' && ch <= 'z')
				{
					if (chr != ' ')
					{
						return ch;
					}
					return (char)(ch - 97 + 65);
				}
				if (ch >= 'A' && ch <= 'Z')
				{
					if (chr == ' ' || chr == '\'')
					{
						return ch;
					}
					return (char)(ch - 65 + 97);
				}
				if (ch == '\'')
				{
					if (chr != ' ' && chr != '\'' && chr1 != '\'' && !text.Contains("'"))
					{
						return ch;
					}
				}
				else if (ch == ' ' && chr != ' ' && chr != '\'' && chr1 != ' ' && chr1 != '\'')
				{
					return ch;
				}
			}
		}
		else
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return (char)(ch - 65 + 97);
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		return '\0';
	}

	public enum InputType
	{
		Standard,
		AutoCorrect,
		Password
	}

	public enum KeyboardType
	{
		Default,
		ASCIICapable,
		NumbersAndPunctuation,
		URL,
		NumberPad,
		PhonePad,
		NamePhonePad,
		EmailAddress
	}

	public enum OnReturnKey
	{
		Default,
		Submit,
		NewLine
	}

	public delegate char OnValidate(string text, int charIndex, char addedChar);

	public enum Validation
	{
		None,
		Integer,
		Float,
		Alphanumeric,
		Username,
		Name,
		Filename
	}
}