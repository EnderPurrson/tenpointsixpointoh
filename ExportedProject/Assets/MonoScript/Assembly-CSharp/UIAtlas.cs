using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private Material material;

	[HideInInspector]
	[SerializeField]
	private List<UISpriteData> mSprites = new List<UISpriteData>();

	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	[HideInInspector]
	[SerializeField]
	private UIAtlas mReplacement;

	[HideInInspector]
	[SerializeField]
	private UIAtlas.Coordinates mCoordinates;

	[HideInInspector]
	[SerializeField]
	private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

	private int mPMA = -1;

	private Dictionary<string, int> mSpriteIndices = new Dictionary<string, int>();

	public float pixelSize
	{
		get
		{
			return (this.mReplacement == null ? this.mPixelSize : this.mReplacement.pixelSize);
		}
		set
		{
			if (this.mReplacement == null)
			{
				float single = Mathf.Clamp(value, 0.25f, 4f);
				if (this.mPixelSize != single)
				{
					this.mPixelSize = single;
					this.MarkAsChanged();
				}
			}
			else
			{
				this.mReplacement.pixelSize = value;
			}
		}
	}

	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material material = this.spriteMaterial;
				this.mPMA = (!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied") ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uIAtla = value;
			if (uIAtla == this)
			{
				uIAtla = null;
			}
			if (this.mReplacement != uIAtla)
			{
				if (uIAtla != null && uIAtla.replacement == this)
				{
					uIAtla.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uIAtla;
				if (uIAtla != null)
				{
					this.material = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	public List<UISpriteData> spriteList
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.spriteList;
			}
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			return this.mSprites;
		}
		set
		{
			if (this.mReplacement == null)
			{
				this.mSprites = value;
			}
			else
			{
				this.mReplacement.spriteList = value;
			}
		}
	}

	public Material spriteMaterial
	{
		get
		{
			return (this.mReplacement == null ? this.material : this.mReplacement.spriteMaterial);
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
			}
			else if (this.material != null)
			{
				this.MarkAsChanged();
				this.mPMA = -1;
				this.material = value;
				this.MarkAsChanged();
			}
			else
			{
				this.mPMA = 0;
				this.material = value;
			}
		}
	}

	public Texture texture
	{
		get
		{
			Texture texture;
			if (this.mReplacement != null)
			{
				texture = this.mReplacement.texture;
			}
			else if (this.material == null)
			{
				texture = null;
			}
			else
			{
				texture = this.material.mainTexture;
			}
			return texture;
		}
	}

	public UIAtlas()
	{
	}

	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		if (a == null || b == null)
		{
			return false;
		}
		return (a == b || a.References(b) ? true : b.References(a));
	}

	public BetterList<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int num = 0;
		int count = this.mSprites.Count;
		while (num < count)
		{
			UISpriteData item = this.mSprites[num];
			if (item != null && !string.IsNullOrEmpty(item.name))
			{
				betterList.Add(item.name);
			}
			num++;
		}
		return betterList;
	}

	public BetterList<string> GetListOfSprites(string match)
	{
		if (this.mReplacement)
		{
			return this.mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int num = 0;
		int count = this.mSprites.Count;
		while (num < count)
		{
			UISpriteData item = this.mSprites[num];
			if (item != null && !string.IsNullOrEmpty(item.name) && string.Equals(match, item.name, StringComparison.OrdinalIgnoreCase))
			{
				betterList.Add(item.name);
				return betterList;
			}
			num++;
		}
		string[] lower = match.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < (int)lower.Length; i++)
		{
			lower[i] = lower[i].ToLower();
		}
		int num1 = 0;
		int count1 = this.mSprites.Count;
		while (num1 < count1)
		{
			UISpriteData uISpriteDatum = this.mSprites[num1];
			if (uISpriteDatum != null && !string.IsNullOrEmpty(uISpriteDatum.name))
			{
				string str = uISpriteDatum.name.ToLower();
				int num2 = 0;
				for (int j = 0; j < (int)lower.Length; j++)
				{
					if (str.Contains(lower[j]))
					{
						num2++;
					}
				}
				if (num2 == (int)lower.Length)
				{
					betterList.Add(uISpriteDatum.name);
				}
			}
			num1++;
		}
		return betterList;
	}

	public string GetRandomSprite(string startsWith)
	{
		string item;
		if (this.GetSprite(startsWith) != null)
		{
			return startsWith;
		}
		List<UISpriteData> uISpriteDatas = this.spriteList;
		List<string> strs = new List<string>();
		foreach (UISpriteData uISpriteDatum in uISpriteDatas)
		{
			if (!uISpriteDatum.name.StartsWith(startsWith))
			{
				continue;
			}
			strs.Add(uISpriteDatum.name);
		}
		if (strs.Count <= 0)
		{
			item = null;
		}
		else
		{
			item = strs[UnityEngine.Random.Range(0, strs.Count)];
		}
		return item;
	}

	public UISpriteData GetSprite(string name)
	{
		int num;
		UISpriteData item;
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			if (this.mSprites.Count == 0)
			{
				return null;
			}
			if (this.mSpriteIndices.Count != this.mSprites.Count)
			{
				this.MarkSpriteListAsChanged();
			}
			if (this.mSpriteIndices.TryGetValue(name, out num))
			{
				if (num > -1 && num < this.mSprites.Count)
				{
					return this.mSprites[num];
				}
				this.MarkSpriteListAsChanged();
				if (!this.mSpriteIndices.TryGetValue(name, out num))
				{
					item = null;
				}
				else
				{
					item = this.mSprites[num];
				}
				return item;
			}
			int num1 = 0;
			int count = this.mSprites.Count;
			while (num1 < count)
			{
				UISpriteData uISpriteDatum = this.mSprites[num1];
				if (!string.IsNullOrEmpty(uISpriteDatum.name) && name == uISpriteDatum.name)
				{
					this.MarkSpriteListAsChanged();
					return uISpriteDatum;
				}
				num1++;
			}
		}
		return null;
	}

	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		UISprite[] uISpriteArray = NGUITools.FindActive<UISprite>();
		int num = 0;
		int length = (int)uISpriteArray.Length;
		while (num < length)
		{
			UISprite uISprite = uISpriteArray[num];
			if (UIAtlas.CheckIfRelated(this, uISprite.atlas))
			{
				UIAtlas uIAtla = uISprite.atlas;
				uISprite.atlas = null;
				uISprite.atlas = uIAtla;
			}
			num++;
		}
		UIFont[] uIFontArray = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int num1 = 0;
		int length1 = (int)uIFontArray.Length;
		while (num1 < length1)
		{
			UIFont uIFont = uIFontArray[num1];
			if (UIAtlas.CheckIfRelated(this, uIFont.atlas))
			{
				UIAtlas uIAtla1 = uIFont.atlas;
				uIFont.atlas = null;
				uIFont.atlas = uIAtla1;
			}
			num1++;
		}
		UILabel[] uILabelArray = NGUITools.FindActive<UILabel>();
		int num2 = 0;
		int length2 = (int)uILabelArray.Length;
		while (num2 < length2)
		{
			UILabel uILabel = uILabelArray[num2];
			if (uILabel.bitmapFont != null && UIAtlas.CheckIfRelated(this, uILabel.bitmapFont.atlas))
			{
				UIFont uIFont1 = uILabel.bitmapFont;
				uILabel.bitmapFont = null;
				uILabel.bitmapFont = uIFont1;
			}
			num2++;
		}
	}

	public void MarkSpriteListAsChanged()
	{
		this.mSpriteIndices.Clear();
		int num = 0;
		int count = this.mSprites.Count;
		while (num < count)
		{
			this.mSpriteIndices[this.mSprites[num].name] = num;
			num++;
		}
	}

	private bool References(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (atlas == this)
		{
			return true;
		}
		return (this.mReplacement == null ? false : this.mReplacement.References(atlas));
	}

	public void SortAlphabetically()
	{
		this.mSprites.Sort((UISpriteData s1, UISpriteData s2) => s1.name.CompareTo(s2.name));
	}

	private bool Upgrade()
	{
		if (this.mReplacement)
		{
			return this.mReplacement.Upgrade();
		}
		if (this.mSprites.Count != 0 || this.sprites.Count <= 0 || !this.material)
		{
			return false;
		}
		Texture texture = this.material.mainTexture;
		int num = (texture == null ? 512 : texture.width);
		int num1 = (texture == null ? 512 : texture.height);
		for (int i = 0; i < this.sprites.Count; i++)
		{
			UIAtlas.Sprite item = this.sprites[i];
			Rect rect = item.outer;
			Rect rect1 = item.inner;
			if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
			{
				NGUIMath.ConvertToPixels(rect, num, num1, true);
				NGUIMath.ConvertToPixels(rect1, num, num1, true);
			}
			UISpriteData uISpriteDatum = new UISpriteData()
			{
				name = item.name,
				x = Mathf.RoundToInt(rect.xMin),
				y = Mathf.RoundToInt(rect.yMin),
				width = Mathf.RoundToInt(rect.width),
				height = Mathf.RoundToInt(rect.height),
				paddingLeft = Mathf.RoundToInt(item.paddingLeft * rect.width),
				paddingRight = Mathf.RoundToInt(item.paddingRight * rect.width),
				paddingBottom = Mathf.RoundToInt(item.paddingBottom * rect.height),
				paddingTop = Mathf.RoundToInt(item.paddingTop * rect.height),
				borderLeft = Mathf.RoundToInt(rect1.xMin - rect.xMin),
				borderRight = Mathf.RoundToInt(rect.xMax - rect1.xMax),
				borderBottom = Mathf.RoundToInt(rect.yMax - rect1.yMax),
				borderTop = Mathf.RoundToInt(rect1.yMin - rect.yMin)
			};
			this.mSprites.Add(uISpriteDatum);
		}
		this.sprites.Clear();
		return true;
	}

	private enum Coordinates
	{
		Pixels,
		TexCoords
	}

	[Serializable]
	private class Sprite
	{
		public string name;

		public Rect outer;

		public Rect inner;

		public bool rotated;

		public float paddingLeft;

		public float paddingRight;

		public float paddingTop;

		public float paddingBottom;

		public bool hasPadding
		{
			get
			{
				return (this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f ? true : this.paddingBottom != 0f);
			}
		}

		public Sprite()
		{
		}
	}
}