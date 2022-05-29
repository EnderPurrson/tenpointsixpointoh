using System;
using UnityEngine;

[Serializable]
public class BMSymbol
{
	public string sequence;

	public string spriteName;

	private UISpriteData mSprite;

	private bool mIsValid;

	private int mLength;

	private int mOffsetX;

	private int mOffsetY;

	private int mWidth;

	private int mHeight;

	private int mAdvance;

	private Rect mUV;

	public int advance
	{
		get
		{
			return this.mAdvance;
		}
	}

	public int height
	{
		get
		{
			return this.mHeight;
		}
	}

	public int length
	{
		get
		{
			if (this.mLength == 0)
			{
				this.mLength = this.sequence.Length;
			}
			return this.mLength;
		}
	}

	public int offsetX
	{
		get
		{
			return this.mOffsetX;
		}
	}

	public int offsetY
	{
		get
		{
			return this.mOffsetY;
		}
	}

	public Rect uvRect
	{
		get
		{
			return this.mUV;
		}
	}

	public int width
	{
		get
		{
			return this.mWidth;
		}
	}

	public BMSymbol()
	{
	}

	public void MarkAsChanged()
	{
		this.mIsValid = false;
	}

	public bool Validate(UIAtlas atlas)
	{
		UISpriteData sprite;
		if (atlas == null)
		{
			return false;
		}
		if (!this.mIsValid)
		{
			if (string.IsNullOrEmpty(this.spriteName))
			{
				return false;
			}
			if (atlas == null)
			{
				sprite = null;
			}
			else
			{
				sprite = atlas.GetSprite(this.spriteName);
			}
			this.mSprite = sprite;
			if (this.mSprite != null)
			{
				Texture texture = atlas.texture;
				if (texture != null)
				{
					this.mUV = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
					this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
					this.mOffsetX = this.mSprite.paddingLeft;
					this.mOffsetY = this.mSprite.paddingTop;
					this.mWidth = this.mSprite.width;
					this.mHeight = this.mSprite.height;
					this.mAdvance = this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight;
					this.mIsValid = true;
				}
				else
				{
					this.mSprite = null;
				}
			}
		}
		return this.mSprite != null;
	}
}