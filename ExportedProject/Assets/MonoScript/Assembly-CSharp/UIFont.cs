using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Font")]
[ExecuteInEditMode]
public class UIFont : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private Material mMat;

	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	[HideInInspector]
	[SerializeField]
	private BMFont mFont = new BMFont();

	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	[HideInInspector]
	[SerializeField]
	private UIFont mReplacement;

	[HideInInspector]
	[SerializeField]
	private List<BMSymbol> mSymbols = new List<BMSymbol>();

	[HideInInspector]
	[SerializeField]
	private Font mDynamicFont;

	[HideInInspector]
	[SerializeField]
	private int mDynamicFontSize = 16;

	[HideInInspector]
	[SerializeField]
	private FontStyle mDynamicFontStyle;

	[NonSerialized]
	private UISpriteData mSprite;

	private int mPMA = -1;

	private int mPacked = -1;

	public UIAtlas atlas
	{
		get
		{
			return (this.mReplacement == null ? this.mAtlas : this.mReplacement.atlas);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.atlas = value;
			}
			else if (this.mAtlas != value)
			{
				this.mPMA = -1;
				this.mAtlas = value;
				if (this.mAtlas != null)
				{
					this.mMat = this.mAtlas.spriteMaterial;
					if (this.sprite != null)
					{
						this.mUVRect = this.uvRect;
					}
				}
				this.MarkAsChanged();
			}
		}
	}

	public BMFont bmFont
	{
		get
		{
			return (this.mReplacement == null ? this.mFont : this.mReplacement.bmFont);
		}
		set
		{
			if (this.mReplacement == null)
			{
				this.mFont = value;
			}
			else
			{
				this.mReplacement.bmFont = value;
			}
		}
	}

	public int defaultSize
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.defaultSize;
			}
			if (this.isDynamic || this.mFont == null)
			{
				return this.mDynamicFontSize;
			}
			return this.mFont.charSize;
		}
		set
		{
			if (this.mReplacement == null)
			{
				this.mDynamicFontSize = value;
			}
			else
			{
				this.mReplacement.defaultSize = value;
			}
		}
	}

	public Font dynamicFont
	{
		get
		{
			return (this.mReplacement == null ? this.mDynamicFont : this.mReplacement.dynamicFont);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFont = value;
			}
			else if (this.mDynamicFont != value)
			{
				if (this.mDynamicFont != null)
				{
					this.material = null;
				}
				this.mDynamicFont = value;
				this.MarkAsChanged();
			}
		}
	}

	public FontStyle dynamicFontStyle
	{
		get
		{
			return (this.mReplacement == null ? this.mDynamicFontStyle : this.mReplacement.dynamicFontStyle);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFontStyle = value;
			}
			else if (this.mDynamicFontStyle != value)
			{
				this.mDynamicFontStyle = value;
				this.MarkAsChanged();
			}
		}
	}

	private Texture dynamicTexture
	{
		get
		{
			if (this.mReplacement)
			{
				return this.mReplacement.dynamicTexture;
			}
			if (!this.isDynamic)
			{
				return null;
			}
			return this.mDynamicFont.material.mainTexture;
		}
	}

	public bool hasSymbols
	{
		get
		{
			bool flag;
			if (this.mReplacement == null)
			{
				flag = (this.mSymbols == null ? false : this.mSymbols.Count != 0);
			}
			else
			{
				flag = this.mReplacement.hasSymbols;
			}
			return flag;
		}
	}

	public bool isDynamic
	{
		get
		{
			return (this.mReplacement == null ? this.mDynamicFont != null : this.mReplacement.isDynamic);
		}
	}

	public bool isValid
	{
		get
		{
			return (this.mDynamicFont != null ? true : this.mFont.isValid);
		}
	}

	public Material material
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.material;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.spriteMaterial;
			}
			if (this.mMat == null)
			{
				if (this.mDynamicFont == null)
				{
					return null;
				}
				return this.mDynamicFont.material;
			}
			if (this.mDynamicFont != null && this.mMat != this.mDynamicFont.material)
			{
				this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
			}
			return this.mMat;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.material = value;
			}
			else if (this.mMat != value)
			{
				this.mPMA = -1;
				this.mMat = value;
				this.MarkAsChanged();
			}
		}
	}

	public bool packedFontShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.packedFontShader;
			}
			if (this.mAtlas != null)
			{
				return false;
			}
			if (this.mPacked == -1)
			{
				Material material = this.material;
				this.mPacked = (!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Packed") ? 0 : 1);
			}
			return this.mPacked == 1;
		}
	}

	[Obsolete("Use UIFont.premultipliedAlphaShader instead")]
	public bool premultipliedAlpha
	{
		get
		{
			return this.premultipliedAlphaShader;
		}
	}

	public bool premultipliedAlphaShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlphaShader;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = (!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied") ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	public UIFont replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIFont uIFont = value;
			if (uIFont == this)
			{
				uIFont = null;
			}
			if (this.mReplacement != uIFont)
			{
				if (uIFont != null && uIFont.replacement == this)
				{
					uIFont.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uIFont;
				if (uIFont != null)
				{
					this.mPMA = -1;
					this.mMat = null;
					this.mFont = null;
					this.mDynamicFont = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	[Obsolete("Use UIFont.defaultSize instead")]
	public int size
	{
		get
		{
			return this.defaultSize;
		}
		set
		{
			this.defaultSize = value;
		}
	}

	public UISpriteData sprite
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.sprite;
			}
			if (this.mSprite == null && this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
			{
				this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
				if (this.mSprite == null)
				{
					this.mSprite = this.mAtlas.GetSprite(base.name);
				}
				if (this.mSprite != null)
				{
					this.UpdateUVRect();
				}
				else
				{
					this.mFont.spriteName = null;
				}
				int num = 0;
				int count = this.mSymbols.Count;
				while (num < count)
				{
					this.symbols[num].MarkAsChanged();
					num++;
				}
			}
			return this.mSprite;
		}
	}

	public string spriteName
	{
		get
		{
			return (this.mReplacement == null ? this.mFont.spriteName : this.mReplacement.spriteName);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteName = value;
			}
			else if (this.mFont.spriteName != value)
			{
				this.mFont.spriteName = value;
				this.MarkAsChanged();
			}
		}
	}

	public List<BMSymbol> symbols
	{
		get
		{
			return (this.mReplacement == null ? this.mSymbols : this.mReplacement.symbols);
		}
	}

	public int texHeight
	{
		get
		{
			int num;
			if (this.mReplacement == null)
			{
				num = (this.mFont == null ? 1 : this.mFont.texHeight);
			}
			else
			{
				num = this.mReplacement.texHeight;
			}
			return num;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texHeight = value;
			}
			else if (this.mFont != null)
			{
				this.mFont.texHeight = value;
			}
		}
	}

	public Texture2D texture
	{
		get
		{
			Texture2D texture2D;
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			Material material = this.material;
			if (material == null)
			{
				texture2D = null;
			}
			else
			{
				texture2D = material.mainTexture as Texture2D;
			}
			return texture2D;
		}
	}

	public int texWidth
	{
		get
		{
			int num;
			if (this.mReplacement == null)
			{
				num = (this.mFont == null ? 1 : this.mFont.texWidth);
			}
			else
			{
				num = this.mReplacement.texWidth;
			}
			return num;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texWidth = value;
			}
			else if (this.mFont != null)
			{
				this.mFont.texWidth = value;
			}
		}
	}

	public Rect uvRect
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.uvRect;
			}
			return (!(this.mAtlas != null) || this.sprite == null ? new Rect(0f, 0f, 1f, 1f) : this.mUVRect);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.uvRect = value;
			}
			else if (this.sprite == null && this.mUVRect != value)
			{
				this.mUVRect = value;
				this.MarkAsChanged();
			}
		}
	}

	public UIFont()
	{
	}

	public void AddSymbol(string sequence, string spriteName)
	{
		this.GetSymbol(sequence, true).spriteName = spriteName;
		this.MarkAsChanged();
	}

	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		if (a == null || b == null)
		{
			return false;
		}
		if (a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0])
		{
			return true;
		}
		return (a == b || a.References(b) ? true : b.References(a));
	}

	private BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int num = 0;
		int count = this.mSymbols.Count;
		while (num < count)
		{
			BMSymbol item = this.mSymbols[num];
			if (item.sequence == sequence)
			{
				return item;
			}
			num++;
		}
		if (!createIfMissing)
		{
			return null;
		}
		BMSymbol bMSymbol = new BMSymbol()
		{
			sequence = sequence
		};
		this.mSymbols.Add(bMSymbol);
		return bMSymbol;
	}

	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		this.mSprite = null;
		UILabel[] uILabelArray = NGUITools.FindActive<UILabel>();
		int num = 0;
		int length = (int)uILabelArray.Length;
		while (num < length)
		{
			UILabel uILabel = uILabelArray[num];
			if (uILabel.enabled && NGUITools.GetActive(uILabel.gameObject) && UIFont.CheckIfRelated(this, uILabel.bitmapFont))
			{
				UIFont uIFont = uILabel.bitmapFont;
				uILabel.bitmapFont = null;
				uILabel.bitmapFont = uIFont;
			}
			num++;
		}
		int num1 = 0;
		int count = this.symbols.Count;
		while (num1 < count)
		{
			this.symbols[num1].MarkAsChanged();
			num1++;
		}
	}

	public BMSymbol MatchSymbol(string text, int offset, int textLength)
	{
		int count = this.mSymbols.Count;
		if (count == 0)
		{
			return null;
		}
		textLength -= offset;
		for (int i = 0; i < count; i++)
		{
			BMSymbol item = this.mSymbols[i];
			int num = item.length;
			if (num != 0 && textLength >= num)
			{
				bool flag = true;
				int num1 = 0;
				while (num1 < num)
				{
					if (text[offset + num1] == item.sequence[num1])
					{
						num1++;
					}
					else
					{
						flag = false;
						break;
					}
				}
				if (flag && item.Validate(this.atlas))
				{
					return item;
				}
			}
		}
		return null;
	}

	private bool References(UIFont font)
	{
		if (font == null)
		{
			return false;
		}
		if (font == this)
		{
			return true;
		}
		return (this.mReplacement == null ? false : this.mReplacement.References(font));
	}

	public void RemoveSymbol(string sequence)
	{
		BMSymbol symbol = this.GetSymbol(sequence, false);
		if (symbol != null)
		{
			this.symbols.Remove(symbol);
		}
		this.MarkAsChanged();
	}

	public void RenameSymbol(string before, string after)
	{
		BMSymbol symbol = this.GetSymbol(before, false);
		if (symbol != null)
		{
			symbol.sequence = after;
		}
		this.MarkAsChanged();
	}

	private void Trim()
	{
		if (this.mAtlas.texture != null && this.mSprite != null)
		{
			Rect pixels = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
			Rect rect = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			int num = Mathf.RoundToInt(rect.xMin - pixels.xMin);
			int num1 = Mathf.RoundToInt(rect.yMin - pixels.yMin);
			int num2 = Mathf.RoundToInt(rect.xMax - pixels.xMin);
			int num3 = Mathf.RoundToInt(rect.yMax - pixels.yMin);
			this.mFont.Trim(num, num1, num2, num3);
		}
	}

	public void UpdateUVRect()
	{
		if (this.mAtlas == null)
		{
			return;
		}
		Texture texture = this.mAtlas.texture;
		if (texture != null)
		{
			this.mUVRect = new Rect((float)(this.mSprite.x - this.mSprite.paddingLeft), (float)(this.mSprite.y - this.mSprite.paddingTop), (float)(this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight), (float)(this.mSprite.height + this.mSprite.paddingTop + this.mSprite.paddingBottom));
			this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
			if (this.mSprite.hasPadding)
			{
				this.Trim();
			}
		}
	}

	public bool UsesSprite(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Equals(this.spriteName))
			{
				return true;
			}
			int num = 0;
			int count = this.symbols.Count;
			while (num < count)
			{
				if (s.Equals(this.symbols[num].spriteName))
				{
					return true;
				}
				num++;
			}
		}
		return false;
	}
}