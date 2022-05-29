using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Label")]
[ExecuteInEditMode]
public class UILabel : UIWidget
{
	public UILabel.Crispness keepCrispWhenShrunk = UILabel.Crispness.OnDesktop;

	[HideInInspector]
	[SerializeField]
	private Font mTrueTypeFont;

	[HideInInspector]
	[SerializeField]
	private UIFont mFont;

	[HideInInspector]
	[Multiline(6)]
	[SerializeField]
	private string mText = string.Empty;

	[HideInInspector]
	[SerializeField]
	private int mFontSize = 16;

	[HideInInspector]
	[SerializeField]
	private FontStyle mFontStyle;

	[HideInInspector]
	[SerializeField]
	private NGUIText.Alignment mAlignment;

	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineCount;

	[HideInInspector]
	[SerializeField]
	private UILabel.Effect mEffectStyle;

	[HideInInspector]
	[SerializeField]
	private Color mEffectColor = Color.black;

	[HideInInspector]
	[SerializeField]
	private NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Normal;

	[HideInInspector]
	[SerializeField]
	private Vector2 mEffectDistance = Vector2.one;

	[HideInInspector]
	[SerializeField]
	private UILabel.Overflow mOverflow;

	[HideInInspector]
	[SerializeField]
	private Material mMaterial;

	[HideInInspector]
	[SerializeField]
	private bool mApplyGradient;

	[HideInInspector]
	[SerializeField]
	private Color mGradientTop = Color.white;

	[HideInInspector]
	[SerializeField]
	private Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);

	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	[HideInInspector]
	[SerializeField]
	private int mSpacingY;

	[HideInInspector]
	[SerializeField]
	private bool mUseFloatSpacing;

	[HideInInspector]
	[SerializeField]
	private float mFloatSpacingX;

	[HideInInspector]
	[SerializeField]
	private float mFloatSpacingY;

	[HideInInspector]
	[SerializeField]
	private bool mOverflowEllipsis;

	[HideInInspector]
	[SerializeField]
	private bool mShrinkToFit;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineWidth;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineHeight;

	[HideInInspector]
	[SerializeField]
	private float mLineWidth;

	[HideInInspector]
	[SerializeField]
	private bool mMultiline = true;

	[NonSerialized]
	private Font mActiveTTF;

	[NonSerialized]
	private float mDensity = 1f;

	[NonSerialized]
	private bool mShouldBeProcessed = true;

	[NonSerialized]
	private string mProcessedText;

	[NonSerialized]
	private bool mPremultiply;

	[NonSerialized]
	private Vector2 mCalculatedSize = Vector2.zero;

	[NonSerialized]
	private float mScale = 1f;

	[NonSerialized]
	private int mFinalFontSize;

	[NonSerialized]
	private int mLastWidth;

	[NonSerialized]
	private int mLastHeight;

	private static BetterList<UILabel> mList;

	private static Dictionary<Font, int> mFontUsage;

	private static List<UIDrawCall> mTempDrawcalls;

	private static bool mTexRebuildAdded;

	private static BetterList<Vector3> mTempVerts;

	private static BetterList<int> mTempIndices;

	public NGUIText.Alignment alignment
	{
		get
		{
			return this.mAlignment;
		}
		set
		{
			if (this.mAlignment != value)
			{
				this.mAlignment = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	public UnityEngine.Object ambigiousFont
	{
		get
		{
			UnityEngine.Object obj = this.mFont;
			if (obj == null)
			{
				obj = this.mTrueTypeFont;
			}
			return obj;
		}
		set
		{
			UIFont uIFont = value as UIFont;
			if (uIFont == null)
			{
				this.trueTypeFont = value as Font;
			}
			else
			{
				this.bitmapFont = uIFont;
			}
		}
	}

	public bool applyGradient
	{
		get
		{
			return this.mApplyGradient;
		}
		set
		{
			if (this.mApplyGradient != value)
			{
				this.mApplyGradient = value;
				this.MarkAsChanged();
			}
		}
	}

	public UIFont bitmapFont
	{
		get
		{
			return this.mFont;
		}
		set
		{
			if (this.mFont != value)
			{
				base.RemoveFromPanel();
				this.mFont = value;
				this.mTrueTypeFont = null;
				this.MarkAsChanged();
			}
		}
	}

	public int defaultFontSize
	{
		get
		{
			int num;
			if (this.trueTypeFont == null)
			{
				num = (this.mFont == null ? 16 : this.mFont.defaultSize);
			}
			else
			{
				num = this.mFontSize;
			}
			return num;
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.drawingDimensions;
		}
	}

	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (this.mEffectColor != value)
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.shouldBeProcessed = true;
				}
			}
		}
	}

	public Vector2 effectDistance
	{
		get
		{
			return this.mEffectDistance;
		}
		set
		{
			if (this.mEffectDistance != value)
			{
				this.mEffectDistance = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public float effectiveSpacingX
	{
		get
		{
			return (!this.mUseFloatSpacing ? (float)this.mSpacingX : this.mFloatSpacingX);
		}
	}

	public float effectiveSpacingY
	{
		get
		{
			return (!this.mUseFloatSpacing ? (float)this.mSpacingY : this.mFloatSpacingY);
		}
	}

	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public int finalFontSize
	{
		get
		{
			if (this.trueTypeFont)
			{
				return Mathf.RoundToInt(this.mScale * (float)this.mFinalFontSize);
			}
			return Mathf.RoundToInt((float)this.mFinalFontSize * this.mScale);
		}
	}

	public float floatSpacingX
	{
		get
		{
			return this.mFloatSpacingX;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingX, value))
			{
				this.mFloatSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	public float floatSpacingY
	{
		get
		{
			return this.mFloatSpacingY;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingY, value))
			{
				this.mFloatSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	[Obsolete("Use UILabel.bitmapFont instead")]
	public UIFont font
	{
		get
		{
			return this.bitmapFont;
		}
		set
		{
			this.bitmapFont = value;
		}
	}

	public int fontSize
	{
		get
		{
			return this.mFontSize;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 256);
			if (this.mFontSize != value)
			{
				this.mFontSize = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	public FontStyle fontStyle
	{
		get
		{
			return this.mFontStyle;
		}
		set
		{
			if (this.mFontStyle != value)
			{
				this.mFontStyle = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	public Color gradientBottom
	{
		get
		{
			return this.mGradientBottom;
		}
		set
		{
			if (this.mGradientBottom != value)
			{
				this.mGradientBottom = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	public Color gradientTop
	{
		get
		{
			return this.mGradientTop;
		}
		set
		{
			if (this.mGradientTop != value)
			{
				this.mGradientTop = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	public override bool isAnchoredHorizontally
	{
		get
		{
			return (base.isAnchoredHorizontally ? true : this.mOverflow == UILabel.Overflow.ResizeFreely);
		}
	}

	public override bool isAnchoredVertically
	{
		get
		{
			return (base.isAnchoredVertically || this.mOverflow == UILabel.Overflow.ResizeFreely ? true : this.mOverflow == UILabel.Overflow.ResizeHeight);
		}
	}

	private bool isValid
	{
		get
		{
			return (this.mFont != null ? true : this.mTrueTypeFont != null);
		}
	}

	private bool keepCrisp
	{
		get
		{
			if (!(this.trueTypeFont != null) || this.keepCrispWhenShrunk == UILabel.Crispness.Never)
			{
				return false;
			}
			return this.keepCrispWhenShrunk == UILabel.Crispness.Always;
		}
	}

	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	public override Vector3[] localCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localCorners;
		}
	}

	public override Vector2 localSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localSize;
		}
	}

	public override Material material
	{
		get
		{
			if (this.mMaterial != null)
			{
				return this.mMaterial;
			}
			if (this.mFont != null)
			{
				return this.mFont.material;
			}
			if (this.mTrueTypeFont == null)
			{
				return null;
			}
			return this.mTrueTypeFont.material;
		}
		set
		{
			if (this.mMaterial != value)
			{
				base.RemoveFromPanel();
				this.mMaterial = value;
				this.MarkAsChanged();
			}
		}
	}

	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				this.shouldBeProcessed = true;
				if (this.overflowMethod == UILabel.Overflow.ShrinkContent)
				{
					this.MakePixelPerfect();
				}
			}
		}
	}

	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = (!value ? 1 : 0);
				this.shouldBeProcessed = true;
			}
		}
	}

	public bool overflowEllipsis
	{
		get
		{
			return this.mOverflowEllipsis;
		}
		set
		{
			if (this.mOverflowEllipsis != value)
			{
				this.mOverflowEllipsis = value;
				this.MarkAsChanged();
			}
		}
	}

	public UILabel.Overflow overflowMethod
	{
		get
		{
			return this.mOverflow;
		}
		set
		{
			if (this.mOverflow != value)
			{
				this.mOverflow = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public Vector2 printedSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mCalculatedSize;
		}
	}

	public string processedText
	{
		get
		{
			if (this.mLastWidth != this.mWidth || this.mLastHeight != this.mHeight)
			{
				this.mLastWidth = this.mWidth;
				this.mLastHeight = this.mHeight;
				this.mShouldBeProcessed = true;
			}
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mProcessedText;
		}
	}

	private bool shouldBeProcessed
	{
		get
		{
			return this.mShouldBeProcessed;
		}
		set
		{
			if (!value)
			{
				this.mShouldBeProcessed = false;
			}
			else
			{
				this.mChanged = true;
				this.mShouldBeProcessed = true;
			}
		}
	}

	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return this.mOverflow == UILabel.Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				this.overflowMethod = UILabel.Overflow.ShrinkContent;
			}
		}
	}

	public int spacingX
	{
		get
		{
			return this.mSpacingX;
		}
		set
		{
			if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	public int spacingY
	{
		get
		{
			return this.mSpacingY;
		}
		set
		{
			if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public NGUIText.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			if (this.mText == value)
			{
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.mText = string.Empty;
					this.MarkAsChanged();
					this.ProcessAndRequest();
				}
			}
			else if (this.mText != value)
			{
				this.mText = value;
				this.MarkAsChanged();
				this.ProcessAndRequest();
			}
			if (this.autoResizeBoxCollider)
			{
				base.ResizeCollider();
			}
		}
	}

	public Font trueTypeFont
	{
		get
		{
			Font font;
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont;
			}
			if (this.mFont == null)
			{
				font = null;
			}
			else
			{
				font = this.mFont.dynamicFont;
			}
			return font;
		}
		set
		{
			if (this.mTrueTypeFont != value)
			{
				this.SetActiveFont(null);
				base.RemoveFromPanel();
				this.mTrueTypeFont = value;
				this.shouldBeProcessed = true;
				this.mFont = null;
				this.SetActiveFont(value);
				this.ProcessAndRequest();
				if (this.mActiveTTF != null)
				{
					base.MarkAsChanged();
				}
			}
		}
	}

	public bool useFloatSpacing
	{
		get
		{
			return this.mUseFloatSpacing;
		}
		set
		{
			if (this.mUseFloatSpacing != value)
			{
				this.mUseFloatSpacing = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public override Vector3[] worldCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.worldCorners;
		}
	}

	static UILabel()
	{
		UILabel.mList = new BetterList<UILabel>();
		UILabel.mFontUsage = new Dictionary<Font, int>();
		UILabel.mTexRebuildAdded = false;
		UILabel.mTempVerts = new BetterList<Vector3>();
		UILabel.mTempIndices = new BetterList<int>();
	}

	public UILabel()
	{
	}

	public Vector2 ApplyOffset(BetterList<Vector3> verts, int start)
	{
		Vector2 vector2 = base.pivotOffset;
		float single = Mathf.Lerp(0f, (float)(-this.mWidth), vector2.x);
		float single1 = Mathf.Lerp((float)this.mHeight, 0f, vector2.y) + Mathf.Lerp(this.mCalculatedSize.y - (float)this.mHeight, 0f, vector2.y);
		single = Mathf.Round(single);
		single1 = Mathf.Round(single1);
		for (int i = start; i < verts.size; i++)
		{
			verts.buffer[i].x += single;
			verts.buffer[i].y += single1;
		}
		return new Vector2(single, single1);
	}

	public void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color;
		Color color1 = this.mEffectColor;
		color1.a *= this.finalAlpha;
		Color32 color32 = (!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlphaShader ? color1 : NGUITools.ApplyPMA(color1));
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector3 = verts.buffer[i];
			vector3.x += x;
			vector3.y += y;
			verts.buffer[i] = vector3;
			Color32 color321 = cols.buffer[i];
			if (color321.a != 255)
			{
				Color color2 = color1;
				color2.a = (float)color321.a / 255f * color1.a;
				color = (!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlphaShader ? color2 : NGUITools.ApplyPMA(color2));
				cols.buffer[i] = color;
			}
			else
			{
				cols.buffer[i] = color32;
			}
		}
	}

	public void AssumeNaturalSize()
	{
		if (this.ambigiousFont != null)
		{
			this.mWidth = 100000;
			this.mHeight = 100000;
			this.ProcessText(false, true);
			this.mWidth = Mathf.RoundToInt(this.mCalculatedSize.x);
			this.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y);
			if ((this.mWidth & 1) == 1)
			{
				this.mWidth++;
			}
			if ((this.mHeight & 1) == 1)
			{
				this.mHeight++;
			}
			this.MarkAsChanged();
		}
	}

	public int CalculateOffsetToFit(string text)
	{
		this.UpdateNGUIText();
		NGUIText.encoding = false;
		NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
		int fit = NGUIText.CalculateOffsetToFit(text);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return fit;
	}

	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector3 worldPos)
	{
		return this.GetCharacterIndexAtPosition(worldPos, false);
	}

	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector2 localPos)
	{
		return this.GetCharacterIndexAtPosition(localPos, false);
	}

	public int GetCharacterIndex(int currentIndex, KeyCode key)
	{
		if (this.isValid)
		{
			string str = this.processedText;
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			int num = this.defaultFontSize;
			this.UpdateNGUIText();
			NGUIText.PrintApproximateCharacterPositions(str, UILabel.mTempVerts, UILabel.mTempIndices);
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int num1 = 0;
				while (num1 < UILabel.mTempIndices.size)
				{
					if (UILabel.mTempIndices[num1] != currentIndex)
					{
						num1++;
					}
					else
					{
						Vector2 item = UILabel.mTempVerts[num1];
						if (key == KeyCode.UpArrow)
						{
							item.y = item.y + ((float)num + this.effectiveSpacingY);
						}
						else if (key == KeyCode.DownArrow)
						{
							item.y = item.y - ((float)num + this.effectiveSpacingY);
						}
						else if (key == KeyCode.Home)
						{
							item.x -= 1000f;
						}
						else if (key == KeyCode.End)
						{
							item.x += 1000f;
						}
						int approximateCharacterIndex = NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, item);
						if (approximateCharacterIndex != currentIndex)
						{
							UILabel.mTempVerts.Clear();
							UILabel.mTempIndices.Clear();
							return approximateCharacterIndex;
						}
						break;
					}
				}
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
			if (key == KeyCode.UpArrow || key == KeyCode.Home)
			{
				return 0;
			}
			if (key == KeyCode.DownArrow || key == KeyCode.End)
			{
				return str.Length;
			}
		}
		return currentIndex;
	}

	public int GetCharacterIndexAtPosition(Vector3 worldPos, bool precise)
	{
		return this.GetCharacterIndexAtPosition(base.cachedTransform.InverseTransformPoint(worldPos), precise);
	}

	public int GetCharacterIndexAtPosition(Vector2 localPos, bool precise)
	{
		if (this.isValid)
		{
			string str = this.processedText;
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			this.UpdateNGUIText();
			if (!precise)
			{
				NGUIText.PrintApproximateCharacterPositions(str, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			else
			{
				NGUIText.PrintExactCharacterPositions(str, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int num = (!precise ? NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos) : NGUIText.GetExactCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos));
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
				NGUIText.bitmapFont = null;
				NGUIText.dynamicFont = null;
				return num;
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
		return 0;
	}

	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.shouldBeProcessed)
		{
			this.ProcessText();
		}
		return base.GetSides(relativeTo);
	}

	public string GetUrlAtCharacterIndex(int characterIndex)
	{
		int num;
		if (characterIndex != -1 && characterIndex < this.mText.Length - 6)
		{
			num = (this.mText[characterIndex] != '[' || this.mText[characterIndex + 1] != 'u' || this.mText[characterIndex + 2] != 'r' || this.mText[characterIndex + 3] != 'l' || this.mText[characterIndex + 4] != '=' ? this.mText.LastIndexOf("[url=", characterIndex) : characterIndex);
			if (num == -1)
			{
				return null;
			}
			num += 5;
			int num1 = this.mText.IndexOf("]", num);
			if (num1 == -1)
			{
				return null;
			}
			int num2 = this.mText.IndexOf("[/url]", num1);
			if (num2 == -1 || characterIndex <= num2)
			{
				return this.mText.Substring(num, num1 - num);
			}
		}
		return null;
	}

	public string GetUrlAtPosition(Vector3 worldPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos, true));
	}

	public string GetUrlAtPosition(Vector2 localPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos, true));
	}

	public string GetWordAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.Length)
		{
			int num = this.mText.LastIndexOfAny(new char[] { ' ', '\n' }, characterIndex) + 1;
			int length = this.mText.IndexOfAny(new char[] { ' ', '\n', ',', '.' }, characterIndex);
			if (length == -1)
			{
				length = this.mText.Length;
			}
			if (num != length)
			{
				int num1 = length - num;
				if (num1 > 0)
				{
					return NGUIText.StripSymbols(this.mText.Substring(num, num1));
				}
			}
		}
		return null;
	}

	public string GetWordAtPosition(Vector3 worldPos)
	{
		return this.GetWordAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos, true));
	}

	public string GetWordAtPosition(Vector2 localPos)
	{
		return this.GetWordAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos, true));
	}

	public override void MakePixelPerfect()
	{
		if (this.ambigiousFont == null)
		{
			base.MakePixelPerfect();
		}
		else
		{
			Vector3 num = base.cachedTransform.localPosition;
			num.x = (float)Mathf.RoundToInt(num.x);
			num.y = (float)Mathf.RoundToInt(num.y);
			num.z = (float)Mathf.RoundToInt(num.z);
			base.cachedTransform.localPosition = num;
			base.cachedTransform.localScale = Vector3.one;
			if (this.mOverflow != UILabel.Overflow.ResizeFreely)
			{
				int num1 = base.width;
				int num2 = base.height;
				UILabel.Overflow overflow = this.mOverflow;
				if (overflow != UILabel.Overflow.ResizeHeight)
				{
					this.mWidth = 100000;
				}
				this.mHeight = 100000;
				this.mOverflow = UILabel.Overflow.ShrinkContent;
				this.ProcessText(false, true);
				this.mOverflow = overflow;
				int num3 = Mathf.RoundToInt(this.mCalculatedSize.x);
				int num4 = Mathf.RoundToInt(this.mCalculatedSize.y);
				num3 = Mathf.Max(num3, base.minWidth);
				num4 = Mathf.Max(num4, base.minHeight);
				if ((num3 & 1) == 1)
				{
					num3++;
				}
				if ((num4 & 1) == 1)
				{
					num4++;
				}
				this.mWidth = Mathf.Max(num1, num3);
				this.mHeight = Mathf.Max(num2, num4);
				this.MarkAsChanged();
			}
			else
			{
				this.AssumeNaturalSize();
			}
		}
	}

	public override void MarkAsChanged()
	{
		this.shouldBeProcessed = true;
		base.MarkAsChanged();
	}

	protected override void OnAnchor()
	{
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			if (base.isFullyAnchored)
			{
				this.mOverflow = UILabel.Overflow.ShrinkContent;
			}
		}
		else if (this.mOverflow == UILabel.Overflow.ResizeHeight && this.topAnchor.target != null && this.bottomAnchor.target != null)
		{
			this.mOverflow = UILabel.Overflow.ShrinkContent;
		}
		base.OnAnchor();
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused && this.mTrueTypeFont != null)
		{
			this.Invalidate(false);
		}
	}

	protected override void OnDisable()
	{
		this.SetActiveFont(null);
		UILabel.mList.Remove(this);
		base.OnDisable();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (!UILabel.mTexRebuildAdded)
		{
			UILabel.mTexRebuildAdded = true;
			Font.textureRebuilt += new Action<Font>(UILabel.OnFontChanged);
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!this.isValid)
		{
			return;
		}
		int num = verts.size;
		Color linearSpace = base.color;
		linearSpace.a = this.finalAlpha;
		if (this.mFont != null && this.mFont.premultipliedAlphaShader)
		{
			linearSpace = NGUITools.ApplyPMA(linearSpace);
		}
		if (QualitySettings.activeColorSpace == ColorSpace.Linear)
		{
			linearSpace.r = Mathf.GammaToLinearSpace(linearSpace.r);
			linearSpace.g = Mathf.GammaToLinearSpace(linearSpace.g);
			linearSpace.b = Mathf.GammaToLinearSpace(linearSpace.b);
			linearSpace.a = Mathf.GammaToLinearSpace(linearSpace.a);
		}
		string str = this.processedText;
		int num1 = verts.size;
		this.UpdateNGUIText();
		NGUIText.tint = linearSpace;
		NGUIText.Print(str, verts, uvs, cols);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		Vector2 vector2 = this.ApplyOffset(verts, num1);
		if (this.mFont != null && this.mFont.packedFontShader)
		{
			return;
		}
		if (this.effectStyle != UILabel.Effect.None)
		{
			int num2 = verts.size;
			vector2.x = this.mEffectDistance.x;
			vector2.y = this.mEffectDistance.y;
			this.ApplyShadow(verts, uvs, cols, num, num2, vector2.x, -vector2.y);
			if (this.effectStyle == UILabel.Effect.Outline || this.effectStyle == UILabel.Effect.Outline8)
			{
				num = num2;
				num2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, num2, -vector2.x, vector2.y);
				num = num2;
				num2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, num2, vector2.x, vector2.y);
				num = num2;
				num2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, num2, -vector2.x, -vector2.y);
				if (vector2.y >= 2.5f || vector2.x >= 2.5f || vector2.y <= -2.5f || vector2.x <= -2.5f || this.fontSize < 32 || base.height <= 32 || this.effectStyle == UILabel.Effect.Outline8)
				{
					num = num2;
					num2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, num2, -vector2.x, 0f);
					num = num2;
					num2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, num2, vector2.x, 0f);
					num = num2;
					num2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, num2, 0f, vector2.y);
					num = num2;
					num2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, num2, 0f, -vector2.y);
				}
			}
		}
		if (this.onPostFill != null)
		{
			this.onPostFill(this, num, verts, uvs, cols);
		}
	}

	private static void OnFontChanged(Font font)
	{
		for (int i = 0; i < UILabel.mList.size; i++)
		{
			UILabel item = UILabel.mList[i];
			if (item != null)
			{
				Font font1 = item.trueTypeFont;
				if (font1 == font)
				{
					font1.RequestCharactersInTexture(item.mText, item.mFinalFontSize, item.mFontStyle);
					item.MarkAsChanged();
					if (item.panel == null)
					{
						item.CreatePanel();
					}
					if (UILabel.mTempDrawcalls == null)
					{
						UILabel.mTempDrawcalls = new List<UIDrawCall>();
					}
					if (item.drawCall != null && !UILabel.mTempDrawcalls.Contains(item.drawCall))
					{
						UILabel.mTempDrawcalls.Add(item.drawCall);
					}
				}
			}
		}
		if (UILabel.mTempDrawcalls != null)
		{
			int num = 0;
			int count = UILabel.mTempDrawcalls.Count;
			while (num < count)
			{
				UIDrawCall uIDrawCall = UILabel.mTempDrawcalls[num];
				if (uIDrawCall.panel != null)
				{
					uIDrawCall.panel.FillDrawCall(uIDrawCall);
				}
				num++;
			}
			UILabel.mTempDrawcalls.Clear();
		}
	}

	protected override void OnInit()
	{
		base.OnInit();
		UILabel.mList.Add(this);
		this.SetActiveFont(this.trueTypeFont);
	}

	protected override void OnStart()
	{
		base.OnStart();
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
		this.mPremultiply = (!(this.material != null) || !(this.material.shader != null) ? false : this.material.shader.name.Contains("Premultiplied"));
		this.ProcessAndRequest();
	}

	public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
	{
		if (caret != null)
		{
			caret.Clear();
		}
		if (highlight != null)
		{
			highlight.Clear();
		}
		if (!this.isValid)
		{
			return;
		}
		string str = this.processedText;
		this.UpdateNGUIText();
		int num = caret.verts.size;
		Vector2 vector2 = new Vector2(0.5f, 0.5f);
		float single = this.finalAlpha;
		if (highlight == null || start == end)
		{
			NGUIText.PrintCaretAndSelection(str, start, end, caret.verts, null);
		}
		else
		{
			int num1 = highlight.verts.size;
			NGUIText.PrintCaretAndSelection(str, start, end, caret.verts, highlight.verts);
			if (highlight.verts.size > num1)
			{
				this.ApplyOffset(highlight.verts, num1);
				Color32 color = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * single);
				for (int i = num1; i < highlight.verts.size; i++)
				{
					highlight.uvs.Add(vector2);
					highlight.cols.Add(color);
				}
			}
		}
		this.ApplyOffset(caret.verts, num);
		Color32 color32 = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * single);
		for (int j = num; j < caret.verts.size; j++)
		{
			caret.uvs.Add(vector2);
			caret.cols.Add(color32);
		}
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
	}

	private void ProcessAndRequest()
	{
		if (this.ambigiousFont != null)
		{
			this.ProcessText();
		}
	}

	public void ProcessText()
	{
		this.ProcessText(false, true);
	}

	private void ProcessText(bool legacyMode, bool full)
	{
		int num;
		int num1;
		if (!this.isValid)
		{
			return;
		}
		this.mChanged = true;
		this.shouldBeProcessed = false;
		float single = this.mDrawRegion.z - this.mDrawRegion.x;
		float single1 = this.mDrawRegion.w - this.mDrawRegion.y;
		if (!legacyMode)
		{
			num = base.width;
		}
		else
		{
			num = (this.mMaxLineWidth == 0 ? 1000000 : this.mMaxLineWidth);
		}
		NGUIText.rectWidth = num;
		if (!legacyMode)
		{
			num1 = base.height;
		}
		else
		{
			num1 = (this.mMaxLineHeight == 0 ? 1000000 : this.mMaxLineHeight);
		}
		NGUIText.rectHeight = num1;
		NGUIText.regionWidth = (single == 1f ? NGUIText.rectWidth : Mathf.RoundToInt((float)NGUIText.rectWidth * single));
		NGUIText.regionHeight = (single1 == 1f ? NGUIText.rectHeight : Mathf.RoundToInt((float)NGUIText.rectHeight * single1));
		this.mFinalFontSize = Mathf.Abs((!legacyMode ? this.defaultFontSize : Mathf.RoundToInt(base.cachedTransform.localScale.x)));
		this.mScale = 1f;
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 0)
		{
			this.mProcessedText = string.Empty;
			return;
		}
		bool flag = this.trueTypeFont != null;
		if (!flag || !this.keepCrisp)
		{
			this.mDensity = 1f;
		}
		else
		{
			UIRoot uIRoot = base.root;
			if (uIRoot != null)
			{
				this.mDensity = (uIRoot == null ? 1f : uIRoot.pixelSizeAdjustment);
			}
		}
		if (full)
		{
			this.UpdateNGUIText();
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			NGUIText.rectWidth = 1000000;
			NGUIText.regionWidth = 1000000;
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight)
		{
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
		}
		if (this.mFinalFontSize <= 0)
		{
			base.cachedTransform.localScale = Vector3.one;
			this.mProcessedText = string.Empty;
			this.mScale = 1f;
		}
		else
		{
			bool flag1 = this.keepCrisp;
			int num2 = this.mFinalFontSize;
			while (num2 > 0)
			{
				if (!flag1)
				{
					this.mScale = (float)num2 / (float)this.mFinalFontSize;
					NGUIText.fontScale = (!flag ? (float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale : this.mScale);
				}
				else
				{
					this.mFinalFontSize = num2;
					NGUIText.fontSize = this.mFinalFontSize;
				}
				NGUIText.Update(false);
				bool flag2 = NGUIText.WrapText(this.mText, out this.mProcessedText, true, false, (!this.mOverflowEllipsis ? false : this.mOverflow == UILabel.Overflow.ClampContent));
				if (this.mOverflow != UILabel.Overflow.ShrinkContent || flag2)
				{
					if (this.mOverflow == UILabel.Overflow.ResizeFreely)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mWidth = Mathf.Max(this.minWidth, Mathf.RoundToInt(this.mCalculatedSize.x));
						if (single != 1f)
						{
							this.mWidth = Mathf.RoundToInt((float)this.mWidth / single);
						}
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (single1 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / single1);
						}
						if ((this.mWidth & 1) == 1)
						{
							this.mWidth++;
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else if (this.mOverflow != UILabel.Overflow.ResizeHeight)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
					}
					else
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (single1 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / single1);
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					if (legacyMode)
					{
						base.width = Mathf.RoundToInt(this.mCalculatedSize.x);
						base.height = Mathf.RoundToInt(this.mCalculatedSize.y);
						base.cachedTransform.localScale = Vector3.one;
					}
					break;
				}
				else
				{
					int num3 = num2 - 1;
					num2 = num3;
					if (num3 <= 1)
					{
						break;
					}
					else
					{
						num2--;
					}
				}
			}
		}
		if (full)
		{
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
	}

	protected void SetActiveFont(Font fnt)
	{
		int num;
		if (this.mActiveTTF != fnt)
		{
			Font font = this.mActiveTTF;
			if (font != null && UILabel.mFontUsage.TryGetValue(font, out num))
			{
				int num1 = num - 1;
				num = num1;
				num = Mathf.Max(0, num1);
				if (num != 0)
				{
					UILabel.mFontUsage[font] = num;
				}
				else
				{
					UILabel.mFontUsage.Remove(font);
				}
			}
			this.mActiveTTF = fnt;
			font = fnt;
			if (font != null)
			{
				int num2 = 0 + 1;
				UILabel.mFontUsage[font] = num2;
			}
		}
	}

	public void SetCurrentPercent()
	{
		if (UIProgressBar.current != null)
		{
			this.text = string.Concat(Mathf.RoundToInt(UIProgressBar.current.@value * 100f), "%");
		}
	}

	public void SetCurrentProgress()
	{
		if (UIProgressBar.current != null)
		{
			this.text = UIProgressBar.current.@value.ToString("F");
		}
	}

	public void SetCurrentSelection()
	{
		if (UIPopupList.current != null)
		{
			this.text = (!UIPopupList.current.isLocalized ? UIPopupList.current.@value : Localization.Get(UIPopupList.current.@value));
		}
	}

	public void UpdateNGUIText()
	{
		bool flag;
		Font font = this.trueTypeFont;
		bool flag1 = font != null;
		NGUIText.fontSize = this.mFinalFontSize;
		NGUIText.fontStyle = this.mFontStyle;
		NGUIText.rectWidth = this.mWidth;
		NGUIText.rectHeight = this.mHeight;
		NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
		NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		if (!this.mApplyGradient)
		{
			flag = false;
		}
		else
		{
			flag = (this.mFont == null ? true : !this.mFont.packedFontShader);
		}
		NGUIText.gradient = flag;
		NGUIText.gradientTop = this.mGradientTop;
		NGUIText.gradientBottom = this.mGradientBottom;
		NGUIText.encoding = this.mEncoding;
		NGUIText.premultiply = this.mPremultiply;
		NGUIText.symbolStyle = this.mSymbols;
		NGUIText.maxLines = this.mMaxLineCount;
		NGUIText.spacingX = this.effectiveSpacingX;
		NGUIText.spacingY = this.effectiveSpacingY;
		NGUIText.fontScale = (!flag1 ? (float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale : this.mScale);
		if (this.mFont == null)
		{
			NGUIText.dynamicFont = font;
			NGUIText.bitmapFont = null;
		}
		else
		{
			NGUIText.bitmapFont = this.mFont;
			while (true)
			{
				UIFont uIFont = NGUIText.bitmapFont.replacement;
				if (uIFont == null)
				{
					break;
				}
				NGUIText.bitmapFont = uIFont;
			}
			if (!NGUIText.bitmapFont.isDynamic)
			{
				NGUIText.dynamicFont = null;
			}
			else
			{
				NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
				NGUIText.bitmapFont = null;
			}
		}
		if (!flag1 || !this.keepCrisp)
		{
			NGUIText.pixelDensity = 1f;
		}
		else
		{
			UIRoot uIRoot = base.root;
			if (uIRoot != null)
			{
				NGUIText.pixelDensity = (uIRoot == null ? 1f : uIRoot.pixelSizeAdjustment);
			}
		}
		if (this.mDensity != NGUIText.pixelDensity)
		{
			this.ProcessText(false, false);
			NGUIText.rectWidth = this.mWidth;
			NGUIText.rectHeight = this.mHeight;
			NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
			NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		}
		if (this.alignment != NGUIText.Alignment.Automatic)
		{
			NGUIText.alignment = this.alignment;
		}
		else
		{
			UIWidget.Pivot pivot = base.pivot;
			if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
			{
				NGUIText.alignment = NGUIText.Alignment.Left;
			}
			else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
			{
				NGUIText.alignment = NGUIText.Alignment.Right;
			}
			else
			{
				NGUIText.alignment = NGUIText.Alignment.Center;
			}
		}
		NGUIText.Update();
	}

	protected override void UpgradeFrom265()
	{
		this.ProcessText(true, true);
		if (this.mShrinkToFit)
		{
			this.overflowMethod = UILabel.Overflow.ShrinkContent;
			this.mMaxLineCount = 0;
		}
		if (this.mMaxLineWidth == 0)
		{
			this.overflowMethod = UILabel.Overflow.ResizeFreely;
		}
		else
		{
			base.width = this.mMaxLineWidth;
			this.overflowMethod = (this.mMaxLineCount <= 0 ? UILabel.Overflow.ShrinkContent : UILabel.Overflow.ResizeHeight);
		}
		if (this.mMaxLineHeight != 0)
		{
			base.height = this.mMaxLineHeight;
		}
		if (this.mFont != null)
		{
			int num = this.mFont.defaultSize;
			if (base.height < num)
			{
				base.height = num;
			}
			this.fontSize = num;
		}
		this.mMaxLineWidth = 0;
		this.mMaxLineHeight = 0;
		this.mShrinkToFit = false;
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	public bool Wrap(string text, out string final)
	{
		return this.Wrap(text, out final, 1000000);
	}

	public bool Wrap(string text, out string final, int height)
	{
		this.UpdateNGUIText();
		NGUIText.rectHeight = height;
		NGUIText.regionHeight = height;
		bool flag = NGUIText.WrapText(text, out final, false);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return flag;
	}

	public enum Crispness
	{
		Never,
		OnDesktop,
		Always
	}

	public enum Effect
	{
		None,
		Shadow,
		Outline,
		Outline8
	}

	public enum Overflow
	{
		ShrinkContent,
		ClampContent,
		ResizeFreely,
		ResizeHeight
	}
}