using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Sprite")]
[ExecuteInEditMode]
public class UISprite : UIBasicSprite
{
	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	[HideInInspector]
	[SerializeField]
	private bool mFillCenter = true;

	[NonSerialized]
	protected UISpriteData mSprite;

	[NonSerialized]
	private bool mSpriteSet;

	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			if (this.mAtlas != value)
			{
				base.RemoveFromPanel();
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.SetAtlasSprite(this.mAtlas.spriteList[0]);
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string str = this.mSpriteName;
					this.mSpriteName = string.Empty;
					this.spriteName = str;
					this.MarkAsChanged();
				}
			}
		}
	}

	public override Vector4 border
	{
		get
		{
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return base.border;
			}
			return new Vector4((float)atlasSprite.borderLeft, (float)atlasSprite.borderBottom, (float)atlasSprite.borderRight, (float)atlasSprite.borderTop);
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 vector2 = base.pivotOffset;
			float single = -vector2.x * (float)this.mWidth;
			float single1 = -vector2.y * (float)this.mHeight;
			float single2 = single + (float)this.mWidth;
			float single3 = single1 + (float)this.mHeight;
			if (this.GetAtlasSprite() != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int num = this.mSprite.paddingLeft;
				int num1 = this.mSprite.paddingBottom;
				int num2 = this.mSprite.paddingRight;
				int num3 = this.mSprite.paddingTop;
				if (this.mType != UIBasicSprite.Type.Simple)
				{
					float single4 = this.pixelSize;
					if (single4 != 1f)
					{
						num = Mathf.RoundToInt(single4 * (float)num);
						num1 = Mathf.RoundToInt(single4 * (float)num1);
						num2 = Mathf.RoundToInt(single4 * (float)num2);
						num3 = Mathf.RoundToInt(single4 * (float)num3);
					}
				}
				int num4 = this.mSprite.width + num + num2;
				int num5 = this.mSprite.height + num1 + num3;
				float single5 = 1f;
				float single6 = 1f;
				if (num4 > 0 && num5 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num4 & 1) != 0)
					{
						num2++;
					}
					if ((num5 & 1) != 0)
					{
						num3++;
					}
					single5 = 1f / (float)num4 * (float)this.mWidth;
					single6 = 1f / (float)num5 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					single = single + (float)num2 * single5;
					single2 = single2 - (float)num * single5;
				}
				else
				{
					single = single + (float)num * single5;
					single2 = single2 - (float)num2 * single5;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					single1 = single1 + (float)num3 * single6;
					single3 = single3 - (float)num1 * single6;
				}
				else
				{
					single1 = single1 + (float)num1 * single6;
					single3 = single3 - (float)num3 * single6;
				}
			}
			Vector4 vector4 = (this.mAtlas == null ? Vector4.zero : this.border * this.pixelSize);
			float single7 = vector4.x + vector4.z;
			float single8 = vector4.y + vector4.w;
			float single9 = Mathf.Lerp(single, single2 - single7, this.mDrawRegion.x);
			float single10 = Mathf.Lerp(single1, single3 - single8, this.mDrawRegion.y);
			float single11 = Mathf.Lerp(single + single7, single2, this.mDrawRegion.z);
			float single12 = Mathf.Lerp(single1 + single8, single3, this.mDrawRegion.w);
			return new Vector4(single9, single10, single11, single12);
		}
	}

	[Obsolete("Use 'centerType' instead")]
	public bool fillCenter
	{
		get
		{
			return this.centerType != UIBasicSprite.AdvancedType.Invisible;
		}
		set
		{
			if (value != this.centerType != UIBasicSprite.AdvancedType.Invisible)
			{
				this.centerType = (!value ? UIBasicSprite.AdvancedType.Invisible : UIBasicSprite.AdvancedType.Sliced);
				this.MarkAsChanged();
			}
		}
	}

	public bool isValid
	{
		get
		{
			return this.GetAtlasSprite() != null;
		}
	}

	public override Material material
	{
		get
		{
			Material material;
			if (this.mAtlas == null)
			{
				material = null;
			}
			else
			{
				material = this.mAtlas.spriteMaterial;
			}
			return material;
		}
	}

	public override int minHeight
	{
		get
		{
			if (this.type != UIBasicSprite.Type.Sliced && this.type != UIBasicSprite.Type.Advanced)
			{
				return base.minHeight;
			}
			float single = this.pixelSize;
			Vector4 vector4 = this.border * this.pixelSize;
			int num = Mathf.RoundToInt(vector4.y + vector4.w);
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite != null)
			{
				num += Mathf.RoundToInt(single * (float)(atlasSprite.paddingTop + atlasSprite.paddingBottom));
			}
			return Mathf.Max(base.minHeight, ((num & 1) != 1 ? num : num + 1));
		}
	}

	public override int minWidth
	{
		get
		{
			if (this.type != UIBasicSprite.Type.Sliced && this.type != UIBasicSprite.Type.Advanced)
			{
				return base.minWidth;
			}
			float single = this.pixelSize;
			Vector4 vector4 = this.border * this.pixelSize;
			int num = Mathf.RoundToInt(vector4.x + vector4.z);
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite != null)
			{
				num += Mathf.RoundToInt(single * (float)(atlasSprite.paddingLeft + atlasSprite.paddingRight));
			}
			return Mathf.Max(base.minWidth, ((num & 1) != 1 ? num : num + 1));
		}
	}

	public override float pixelSize
	{
		get
		{
			return (this.mAtlas == null ? 1f : this.mAtlas.pixelSize);
		}
	}

	public override bool premultipliedAlpha
	{
		get
		{
			return (this.mAtlas == null ? false : this.mAtlas.premultipliedAlpha);
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
			if (string.IsNullOrEmpty(value))
			{
				if (string.IsNullOrEmpty(this.mSpriteName))
				{
					return;
				}
				this.mSpriteName = string.Empty;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
			else if (this.mSpriteName != value)
			{
				this.mSpriteName = value;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
		}
	}

	public UISprite()
	{
	}

	public UISpriteData GetAtlasSprite()
	{
		if (!this.mSpriteSet)
		{
			this.mSprite = null;
		}
		if (this.mSprite == null && this.mAtlas != null)
		{
			if (!string.IsNullOrEmpty(this.mSpriteName))
			{
				UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
				if (sprite == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite);
			}
			if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
			{
				UISpriteData item = this.mAtlas.spriteList[0];
				if (item == null)
				{
					return null;
				}
				this.SetAtlasSprite(item);
				if (this.mSprite == null)
				{
					Debug.LogError(string.Concat(this.mAtlas.name, " seems to have a null sprite!"));
					return null;
				}
				this.mSpriteName = this.mSprite.name;
			}
		}
		return this.mSprite;
	}

	public override void MakePixelPerfect()
	{
		if (!this.isValid)
		{
			return;
		}
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		UISpriteData atlasSprite = this.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return;
		}
		Texture texture = this.mainTexture;
		if (texture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !atlasSprite.hasBorder) && texture != null)
		{
			int num = Mathf.RoundToInt(this.pixelSize * (float)(atlasSprite.width + atlasSprite.paddingLeft + atlasSprite.paddingRight));
			int num1 = Mathf.RoundToInt(this.pixelSize * (float)(atlasSprite.height + atlasSprite.paddingTop + atlasSprite.paddingBottom));
			if ((num & 1) == 1)
			{
				num++;
			}
			if ((num1 & 1) == 1)
			{
				num1++;
			}
			base.width = num;
			base.height = num1;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture texture = this.mainTexture;
		if (texture == null)
		{
			return;
		}
		if (this.mSprite == null)
		{
			this.mSprite = this.atlas.GetSprite(this.spriteName);
		}
		if (this.mSprite == null)
		{
			return;
		}
		Rect rect = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
		Rect texCoords = new Rect((float)(this.mSprite.x + this.mSprite.borderLeft), (float)(this.mSprite.y + this.mSprite.borderTop), (float)(this.mSprite.width - this.mSprite.borderLeft - this.mSprite.borderRight), (float)(this.mSprite.height - this.mSprite.borderBottom - this.mSprite.borderTop));
		rect = NGUIMath.ConvertToTexCoords(rect, texture.width, texture.height);
		texCoords = NGUIMath.ConvertToTexCoords(texCoords, texture.width, texture.height);
		int num = verts.size;
		base.Fill(verts, uvs, cols, rect, texCoords);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, num, verts, uvs, cols);
		}
	}

	protected override void OnInit()
	{
		if (!this.mFillCenter)
		{
			this.mFillCenter = true;
			this.centerType = UIBasicSprite.AdvancedType.Invisible;
		}
		base.OnInit();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mChanged || !this.mSpriteSet)
		{
			this.mSpriteSet = true;
			this.mSprite = null;
			this.mChanged = true;
		}
	}

	protected void SetAtlasSprite(UISpriteData sp)
	{
		this.mChanged = true;
		this.mSpriteSet = true;
		if (sp == null)
		{
			this.mSpriteName = (this.mSprite == null ? string.Empty : this.mSprite.name);
			this.mSprite = sp;
		}
		else
		{
			this.mSprite = sp;
			this.mSpriteName = this.mSprite.name;
		}
	}
}