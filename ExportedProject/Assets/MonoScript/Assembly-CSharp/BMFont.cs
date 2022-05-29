using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BMFont
{
	[HideInInspector]
	[SerializeField]
	private int mSize = 16;

	[HideInInspector]
	[SerializeField]
	private int mBase;

	[HideInInspector]
	[SerializeField]
	private int mWidth;

	[HideInInspector]
	[SerializeField]
	private int mHeight;

	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	[HideInInspector]
	[SerializeField]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();

	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	public int glyphCount
	{
		get
		{
			return (!this.isValid ? 0 : this.mSaved.Count);
		}
	}

	public List<BMGlyph> glyphs
	{
		get
		{
			return this.mSaved;
		}
	}

	public bool isValid
	{
		get
		{
			return this.mSaved.Count > 0;
		}
	}

	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	public BMFont()
	{
	}

	public void Clear()
	{
		this.mDict.Clear();
		this.mSaved.Clear();
	}

	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph bMGlyph = null;
		if (this.mDict.Count == 0)
		{
			int num = 0;
			int count = this.mSaved.Count;
			while (num < count)
			{
				BMGlyph item = this.mSaved[num];
				this.mDict.Add(item.index, item);
				num++;
			}
		}
		if (!this.mDict.TryGetValue(index, out bMGlyph) && createIfMissing)
		{
			bMGlyph = new BMGlyph()
			{
				index = index
			};
			this.mSaved.Add(bMGlyph);
			this.mDict.Add(index, bMGlyph);
		}
		return bMGlyph;
	}

	public BMGlyph GetGlyph(int index)
	{
		return this.GetGlyph(index, false);
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int num = 0;
			int count = this.mSaved.Count;
			while (num < count)
			{
				BMGlyph item = this.mSaved[num];
				if (item != null)
				{
					item.Trim(xMin, yMin, xMax, yMax);
				}
				num++;
			}
		}
	}
}