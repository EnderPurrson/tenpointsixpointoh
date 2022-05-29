using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Popup List")]
[ExecuteInEditMode]
public class UIPopupList : UIWidgetContainer
{
	private const float animSpeed = 0.15f;

	public static UIPopupList current;

	private static GameObject mChild;

	private static float mFadeOutComplete;

	public UIAtlas atlas;

	public UIFont bitmapFont;

	public Font trueTypeFont;

	public int fontSize = 16;

	public FontStyle fontStyle;

	public string backgroundSprite;

	public string highlightSprite;

	public UIPopupList.Position position;

	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	public List<string> items = new List<string>();

	public List<object> itemData = new List<object>();

	public Vector2 padding = new Vector3(4f, 4f);

	public Color textColor = Color.white;

	public Color backgroundColor = Color.white;

	public Color highlightColor = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	public bool isAnimated = true;

	public bool isLocalized;

	public bool separatePanel = true;

	public UIPopupList.OpenOn openOn;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	[HideInInspector]
	[SerializeField]
	protected string mSelectedItem;

	[HideInInspector]
	[SerializeField]
	protected UIPanel mPanel;

	[HideInInspector]
	[SerializeField]
	protected UISprite mBackground;

	[HideInInspector]
	[SerializeField]
	protected UISprite mHighlight;

	[HideInInspector]
	[SerializeField]
	protected UILabel mHighlightedLabel;

	[HideInInspector]
	[SerializeField]
	protected List<UILabel> mLabelList = new List<UILabel>();

	[HideInInspector]
	[SerializeField]
	protected float mBgBorder;

	[NonSerialized]
	protected GameObject mSelection;

	[NonSerialized]
	protected int mOpenFrame;

	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	[HideInInspector]
	[SerializeField]
	private string functionName = "OnSelectionChange";

	[HideInInspector]
	[SerializeField]
	private float textScale;

	[HideInInspector]
	[SerializeField]
	private UIFont font;

	[HideInInspector]
	[SerializeField]
	private UILabel textLabel;

	private UIPopupList.LegacyEvent mLegacyEvent;

	[NonSerialized]
	protected bool mExecuting;

	protected bool mUseDynamicFont;

	protected bool mTweening;

	public GameObject source;

	private float activeFontScale
	{
		get
		{
			return (this.trueTypeFont != null || this.bitmapFont == null ? 1f : (float)this.fontSize / (float)this.bitmapFont.defaultSize);
		}
	}

	private int activeFontSize
	{
		get
		{
			return (this.trueTypeFont != null || this.bitmapFont == null ? this.fontSize : this.bitmapFont.defaultSize);
		}
	}

	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = value as Font;
				this.bitmapFont = null;
				this.font = null;
			}
			else if (value is UIFont)
			{
				this.bitmapFont = value as UIFont;
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	public virtual object data
	{
		get
		{
			int num = this.items.IndexOf(this.mSelectedItem);
			return (num <= -1 || num >= this.itemData.Count ? null : this.itemData[num]);
		}
	}

	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D collider2D = base.GetComponent<Collider2D>();
			return (collider2D == null ? false : collider2D.enabled);
		}
	}

	public static bool isOpen
	{
		get
		{
			bool flag;
			if (UIPopupList.current == null)
			{
				flag = false;
			}
			else
			{
				flag = (UIPopupList.mChild != null ? true : UIPopupList.mFadeOutComplete > Time.unscaledTime);
			}
			return flag;
		}
	}

	private bool isValid
	{
		get
		{
			return (this.bitmapFont != null ? true : this.trueTypeFont != null);
		}
	}

	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public UIPopupList.LegacyEvent onSelectionChange
	{
		get
		{
			return this.mLegacyEvent;
		}
		set
		{
			this.mLegacyEvent = value;
		}
	}

	[Obsolete("Use 'value' instead")]
	public string selection
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

	public virtual string @value
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			this.mSelectedItem = value;
			if (this.mSelectedItem == null)
			{
				return;
			}
			if (this.mSelectedItem != null)
			{
				this.TriggerCallbacks();
			}
		}
	}

	static UIPopupList()
	{
	}

	public UIPopupList()
	{
	}

	public virtual void AddItem(string text)
	{
		this.items.Add(text);
		this.itemData.Add(null);
	}

	public virtual void AddItem(string text, object data)
	{
		this.items.Add(text);
		this.itemData.Add(data);
	}

	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	protected virtual void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	protected virtual void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 vector3;
		Vector3 vector31 = widget.cachedTransform.localPosition;
		vector3 = (!placeAbove ? new Vector3(vector31.x, 0f, vector31.z) : new Vector3(vector31.x, bottom, vector31.z));
		widget.cachedTransform.localPosition = vector3;
		GameObject gameObject = widget.gameObject;
		TweenPosition.Begin(gameObject, 0.15f, vector31).method = UITweener.Method.EaseOut;
	}

	protected virtual void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.gameObject;
		Transform vector3 = widget.cachedTransform;
		float single = (float)this.activeFontSize * this.activeFontScale + this.mBgBorder * 2f;
		vector3.localScale = new Vector3(1f, single / (float)widget.height, 1f);
		TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 vector31 = vector3.localPosition;
			vector3.localPosition = new Vector3(vector31.x, vector31.y - (float)widget.height + single, vector31.z);
			TweenPosition.Begin(gameObject, 0.15f, vector31).method = UITweener.Method.EaseOut;
		}
	}

	public virtual void Clear()
	{
		this.items.Clear();
		this.itemData.Clear();
	}

	public static void Close()
	{
		if (UIPopupList.current != null)
		{
			UIPopupList.current.CloseSelf();
			UIPopupList.current = null;
		}
	}

	[DebuggerHidden]
	private IEnumerator CloseIfUnselected()
	{
		UIPopupList.u003cCloseIfUnselectedu003ec__IteratorC1 variable = null;
		return variable;
	}

	public virtual void CloseSelf()
	{
		if (UIPopupList.mChild != null && UIPopupList.current == this)
		{
			base.StopCoroutine("CloseIfUnselected");
			this.mSelection = null;
			this.mLabelList.Clear();
			if (!this.isAnimated)
			{
				UnityEngine.Object.Destroy(UIPopupList.mChild);
				UIPopupList.mFadeOutComplete = Time.unscaledTime + 0.1f;
			}
			else
			{
				UIWidget[] componentsInChildren = UIPopupList.mChild.GetComponentsInChildren<UIWidget>();
				int num = 0;
				int length = (int)componentsInChildren.Length;
				while (num < length)
				{
					UIWidget uIWidget = componentsInChildren[num];
					Color color = uIWidget.color;
					color.a = 0f;
					TweenColor.Begin(uIWidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
					num++;
				}
				Collider[] colliderArray = UIPopupList.mChild.GetComponentsInChildren<Collider>();
				int num1 = 0;
				int length1 = (int)colliderArray.Length;
				while (num1 < length1)
				{
					colliderArray[num1].enabled = false;
					num1++;
				}
				UnityEngine.Object.Destroy(UIPopupList.mChild, 0.15f);
				UIPopupList.mFadeOutComplete = Time.unscaledTime + Mathf.Max(0.1f, 0.15f);
			}
			this.mBackground = null;
			this.mHighlight = null;
			UIPopupList.mChild = null;
			UIPopupList.current = null;
		}
	}

	protected virtual Vector3 GetHighlightPosition()
	{
		if (this.mHighlightedLabel == null || this.mHighlight == null)
		{
			return Vector3.zero;
		}
		UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return Vector3.zero;
		}
		float single = this.atlas.pixelSize;
		float single1 = (float)atlasSprite.borderLeft * single;
		float single2 = (float)atlasSprite.borderTop * single;
		return this.mHighlightedLabel.cachedTransform.localPosition + new Vector3(-single1, single2, 1f);
	}

	protected virtual void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			this.mHighlightedLabel = lbl;
			if (this.mHighlight.GetAtlasSprite() == null)
			{
				return;
			}
			Vector3 highlightPosition = this.GetHighlightPosition();
			if (instant || !this.isAnimated)
			{
				this.mHighlight.cachedTransform.localPosition = highlightPosition;
			}
			else
			{
				TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
				if (!this.mTweening)
				{
					this.mTweening = true;
					base.StartCoroutine("UpdateTweenPosition");
				}
			}
		}
	}

	protected virtual void OnClick()
	{
		if (this.mOpenFrame == Time.frameCount)
		{
			return;
		}
		if (UIPopupList.mChild == null)
		{
			if (this.openOn == UIPopupList.OpenOn.DoubleClick || this.openOn == UIPopupList.OpenOn.Manual)
			{
				return;
			}
			if (this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
			{
				return;
			}
			this.Show();
		}
		else if (this.mHighlightedLabel != null)
		{
			this.OnItemPress(this.mHighlightedLabel.gameObject, true);
		}
	}

	protected virtual void OnDisable()
	{
		this.CloseSelf();
	}

	protected virtual void OnDoubleClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick)
		{
			this.Show();
		}
	}

	protected virtual void OnEnable()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			this.eventReceiver = null;
			this.functionName = null;
		}
		if (this.font != null)
		{
			if (this.font.isDynamic)
			{
				this.trueTypeFont = this.font.dynamicFont;
				this.fontStyle = this.font.dynamicFontStyle;
				this.mUseDynamicFont = true;
			}
			else if (this.bitmapFont == null)
			{
				this.bitmapFont = this.font;
				this.mUseDynamicFont = false;
			}
			this.font = null;
		}
		if (this.textScale != 0f)
		{
			this.fontSize = (this.bitmapFont == null ? 16 : Mathf.RoundToInt((float)this.bitmapFont.defaultSize * this.textScale));
			this.textScale = 0f;
		}
		if (this.trueTypeFont == null && this.bitmapFont != null && this.bitmapFont.isDynamic)
		{
			this.trueTypeFont = this.bitmapFont.dynamicFont;
			this.bitmapFont = null;
		}
	}

	protected virtual void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			this.Highlight(go.GetComponent<UILabel>(), false);
		}
	}

	protected virtual void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
			this.@value = go.GetComponent<UIEventListener>().parameter as string;
			UIPlaySound[] components = base.GetComponents<UIPlaySound>();
			int num = 0;
			int length = (int)components.Length;
			while (num < length)
			{
				UIPlaySound uIPlaySound = components[num];
				if (uIPlaySound.trigger == UIPlaySound.Trigger.OnClick)
				{
					NGUITools.PlaySound(uIPlaySound.audioClip, uIPlaySound.volume, 1f);
				}
				num++;
			}
			this.CloseSelf();
		}
	}

	protected virtual void OnKey(KeyCode key)
	{
		if (base.enabled && UIPopupList.current == this && (key == UICamera.current.cancelKey0 || key == UICamera.current.cancelKey1))
		{
			this.OnSelect(false);
		}
	}

	protected virtual void OnLocalize()
	{
		if (this.isLocalized)
		{
			this.TriggerCallbacks();
		}
	}

	protected virtual void OnNavigate(KeyCode key)
	{
		if (base.enabled && UIPopupList.current == this)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (num == -1)
			{
				num = 0;
			}
			if (key == KeyCode.UpArrow)
			{
				if (num > 0)
				{
					int num1 = num - 1;
					num = num1;
					this.Select(this.mLabelList[num1], false);
				}
			}
			else if (key == KeyCode.DownArrow && num + 1 < this.mLabelList.Count)
			{
				int num2 = num + 1;
				num = num2;
				this.Select(this.mLabelList[num2], false);
			}
		}
	}

	protected virtual void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			this.CloseSelf();
		}
	}

	protected virtual void OnValidate()
	{
		Font font = this.trueTypeFont;
		UIFont uIFont = this.bitmapFont;
		this.bitmapFont = null;
		this.trueTypeFont = null;
		if (font != null && (uIFont == null || !this.mUseDynamicFont))
		{
			this.bitmapFont = null;
			this.trueTypeFont = font;
			this.mUseDynamicFont = true;
		}
		else if (uIFont == null)
		{
			this.trueTypeFont = font;
			this.mUseDynamicFont = true;
		}
		else if (!uIFont.isDynamic)
		{
			this.bitmapFont = uIFont;
			this.mUseDynamicFont = false;
		}
		else
		{
			this.trueTypeFont = uIFont.dynamicFont;
			this.fontStyle = uIFont.dynamicFontStyle;
			this.fontSize = uIFont.defaultSize;
			this.mUseDynamicFont = true;
		}
	}

	public virtual void RemoveItem(string text)
	{
		int num = this.items.IndexOf(text);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	public virtual void RemoveItemByData(object data)
	{
		int num = this.itemData.IndexOf(data);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
	}

	public virtual void Show()
	{
		Vector3 vector3;
		Vector3 vector31;
		Vector3 vector32;
		Vector3 vector33;
		if (!base.enabled || !NGUITools.GetActive(base.gameObject) || !(UIPopupList.mChild == null) || !(this.atlas != null) || !this.isValid || this.items.Count <= 0)
		{
			this.OnSelect(false);
		}
		else
		{
			this.mLabelList.Clear();
			base.StopCoroutine("CloseIfUnselected");
			UICamera.selectedObject = UICamera.hoveredObject ?? base.gameObject;
			this.mSelection = UICamera.selectedObject;
			this.source = UICamera.selectedObject;
			if (this.source == null)
			{
				UnityEngine.Debug.LogError("Popup list needs a source object...");
				return;
			}
			this.mOpenFrame = Time.frameCount;
			if (this.mPanel == null)
			{
				this.mPanel = UIPanel.Find(base.transform);
				if (this.mPanel == null)
				{
					return;
				}
			}
			UIPopupList.mChild = new GameObject("Drop-down List")
			{
				layer = base.gameObject.layer
			};
			if (this.separatePanel)
			{
				if (base.GetComponent<Collider>() != null)
				{
					UIPopupList.mChild.AddComponent<Rigidbody>().isKinematic = true;
				}
				else if (base.GetComponent<Collider2D>() != null)
				{
					UIPopupList.mChild.AddComponent<Rigidbody2D>().isKinematic = true;
				}
				UIPopupList.mChild.AddComponent<UIPanel>().depth = 1000000;
			}
			UIPopupList.current = this;
			Transform transforms = UIPopupList.mChild.transform;
			transforms.parent = this.mPanel.cachedTransform;
			if (this.openOn != UIPopupList.OpenOn.Manual || !(this.mSelection != base.gameObject))
			{
				Bounds bound = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, base.transform, false, false);
				vector3 = bound.min;
				vector31 = bound.max;
				transforms.localPosition = vector3;
				vector32 = transforms.position;
			}
			else
			{
				vector32 = UICamera.lastEventPosition;
				vector3 = this.mPanel.cachedTransform.InverseTransformPoint(this.mPanel.anchorCamera.ScreenToWorldPoint(vector32));
				vector31 = vector3;
				transforms.localPosition = vector3;
				vector32 = transforms.position;
			}
			base.StartCoroutine("CloseIfUnselected");
			transforms.localRotation = Quaternion.identity;
			transforms.localScale = Vector3.one;
			this.mBackground = NGUITools.AddSprite(UIPopupList.mChild, this.atlas, this.backgroundSprite, (!this.separatePanel ? NGUITools.CalculateNextDepth(this.mPanel.gameObject) : 0));
			this.mBackground.pivot = UIWidget.Pivot.TopLeft;
			this.mBackground.color = this.backgroundColor;
			Vector4 vector4 = this.mBackground.border;
			this.mBgBorder = vector4.y;
			this.mBackground.cachedTransform.localPosition = new Vector3(0f, vector4.y, 0f);
			this.mHighlight = NGUITools.AddSprite(UIPopupList.mChild, this.atlas, this.highlightSprite, this.mBackground.depth + 1);
			this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
			this.mHighlight.color = this.highlightColor;
			UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return;
			}
			float single = (float)atlasSprite.borderTop;
			float single1 = (float)this.activeFontSize * this.activeFontScale;
			float single2 = 0f;
			float single3 = -this.padding.y;
			List<UILabel> uILabels = new List<UILabel>();
			if (!this.items.Contains(this.mSelectedItem))
			{
				this.mSelectedItem = null;
			}
			int num = 0;
			int count = this.items.Count;
			while (num < count)
			{
				string item = this.items[num];
				UILabel str = NGUITools.AddWidget<UILabel>(UIPopupList.mChild, this.mBackground.depth + 2);
				str.name = num.ToString();
				str.pivot = UIWidget.Pivot.TopLeft;
				str.bitmapFont = this.bitmapFont;
				str.trueTypeFont = this.trueTypeFont;
				str.fontSize = this.fontSize;
				str.fontStyle = this.fontStyle;
				str.text = (!this.isLocalized ? item : Localization.Get(item));
				str.color = this.textColor;
				Transform transforms1 = str.cachedTransform;
				float single4 = vector4.x + this.padding.x;
				Vector2 vector2 = str.pivotOffset;
				transforms1.localPosition = new Vector3(single4 - vector2.x, single3, -1f);
				str.overflowMethod = UILabel.Overflow.ResizeFreely;
				str.alignment = this.alignment;
				uILabels.Add(str);
				single3 -= single1;
				single3 -= this.padding.y;
				single2 = Mathf.Max(single2, str.printedSize.x);
				UIEventListener boolDelegate = UIEventListener.Get(str.gameObject);
				UIPopupList uIPopupList = this;
				boolDelegate.onHover = new UIEventListener.BoolDelegate(uIPopupList.OnItemHover);
				UIPopupList uIPopupList1 = this;
				boolDelegate.onPress = new UIEventListener.BoolDelegate(uIPopupList1.OnItemPress);
				boolDelegate.parameter = item;
				if (this.mSelectedItem == item || num == 0 && string.IsNullOrEmpty(this.mSelectedItem))
				{
					this.Highlight(str, true);
				}
				this.mLabelList.Add(str);
				num++;
			}
			single2 = Mathf.Max(single2, vector31.x - vector3.x - (vector4.x + this.padding.x) * 2f);
			float single5 = single2;
			Vector3 vector34 = new Vector3(single5 * 0.5f, -single1 * 0.5f, 0f);
			Vector3 vector35 = new Vector3(single5, single1 + this.padding.y, 1f);
			int num1 = 0;
			int count1 = uILabels.Count;
			while (num1 < count1)
			{
				UILabel uILabel = uILabels[num1];
				NGUITools.AddWidgetCollider(uILabel.gameObject);
				uILabel.autoResizeBoxCollider = false;
				BoxCollider component = uILabel.GetComponent<BoxCollider>();
				if (component == null)
				{
					BoxCollider2D boxCollider2D = uILabel.GetComponent<BoxCollider2D>();
					boxCollider2D.offset = vector34;
					boxCollider2D.size = vector35;
				}
				else
				{
					vector34.z = component.center.z;
					component.center = vector34;
					component.size = vector35;
				}
				num1++;
			}
			int num2 = Mathf.RoundToInt(single2);
			single2 = single2 + (vector4.x + this.padding.x) * 2f;
			single3 -= vector4.y;
			this.mBackground.width = Mathf.RoundToInt(single2);
			this.mBackground.height = Mathf.RoundToInt(-single3 + vector4.y);
			int num3 = 0;
			int count2 = uILabels.Count;
			while (num3 < count2)
			{
				UILabel item1 = uILabels[num3];
				item1.overflowMethod = UILabel.Overflow.ShrinkContent;
				item1.width = num2;
				num3++;
			}
			float single6 = 2f * this.atlas.pixelSize;
			float single7 = single2 - (vector4.x + this.padding.x) * 2f + (float)atlasSprite.borderLeft * single6;
			float single8 = single1 + single * single6;
			this.mHighlight.width = Mathf.RoundToInt(single7);
			this.mHighlight.height = Mathf.RoundToInt(single8);
			bool flag = this.position == UIPopupList.Position.Above;
			if (this.position == UIPopupList.Position.Auto)
			{
				UICamera uICamera = UICamera.FindCameraForLayer(this.mSelection.layer);
				if (uICamera != null)
				{
					Vector3 viewportPoint = uICamera.cachedCamera.WorldToViewportPoint(vector32);
					flag = viewportPoint.y < 0.5f;
				}
			}
			if (this.isAnimated)
			{
				this.AnimateColor(this.mBackground);
				if (Time.timeScale == 0f || Time.timeScale >= 0.1f)
				{
					float single9 = single3 + single1;
					this.Animate(this.mHighlight, flag, single9);
					int num4 = 0;
					int count3 = uILabels.Count;
					while (num4 < count3)
					{
						this.Animate(uILabels[num4], flag, single9);
						num4++;
					}
					this.AnimateScale(this.mBackground, flag, single9);
				}
			}
			if (!flag)
			{
				vector31.y = vector3.y + vector4.y;
				vector3.y = vector31.y - (float)this.mBackground.height;
				vector31.x = vector3.x + (float)this.mBackground.width;
			}
			else
			{
				vector3.y = vector31.y - vector4.y;
				vector31.y = vector3.y + (float)this.mBackground.height;
				vector31.x = vector3.x + (float)this.mBackground.width;
				transforms.localPosition = new Vector3(vector3.x, vector31.y - vector4.y, vector3.z);
			}
			Transform transforms2 = this.mPanel.cachedTransform.parent;
			if (transforms2 != null)
			{
				vector3 = this.mPanel.cachedTransform.TransformPoint(vector3);
				vector31 = this.mPanel.cachedTransform.TransformPoint(vector31);
				vector3 = transforms2.InverseTransformPoint(vector3);
				vector31 = transforms2.InverseTransformPoint(vector31);
			}
			vector33 = (!this.mPanel.hasClipping ? this.mPanel.CalculateConstrainOffset(vector3, vector31) : Vector3.zero);
			vector32 = transforms.localPosition + vector33;
			vector32.x = Mathf.Round(vector32.x);
			vector32.y = Mathf.Round(vector32.y);
			transforms.localPosition = vector32;
		}
	}

	protected virtual void Start()
	{
		if (this.textLabel != null)
		{
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
			this.textLabel = null;
		}
		if (Application.isPlaying)
		{
			if (string.IsNullOrEmpty(this.mSelectedItem) && this.items.Count > 0)
			{
				this.mSelectedItem = this.items[0];
			}
			if (!string.IsNullOrEmpty(this.mSelectedItem))
			{
				this.TriggerCallbacks();
			}
		}
	}

	protected void TriggerCallbacks()
	{
		if (!this.mExecuting)
		{
			this.mExecuting = true;
			UIPopupList uIPopupList = UIPopupList.current;
			UIPopupList.current = this;
			if (this.mLegacyEvent != null)
			{
				this.mLegacyEvent(this.mSelectedItem);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
			}
			UIPopupList.current = uIPopupList;
			this.mExecuting = false;
		}
	}

	[DebuggerHidden]
	protected virtual IEnumerator UpdateTweenPosition()
	{
		UIPopupList.u003cUpdateTweenPositionu003ec__IteratorC0 variable = null;
		return variable;
	}

	public delegate void LegacyEvent(string val);

	public enum OpenOn
	{
		ClickOrTap,
		RightClick,
		DoubleClick,
		Manual
	}

	public enum Position
	{
		Auto,
		Above,
		Below
	}
}