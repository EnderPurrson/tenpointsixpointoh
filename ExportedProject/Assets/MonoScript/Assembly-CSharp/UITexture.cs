using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Texture")]
[ExecuteInEditMode]
public class UITexture : UIBasicSprite
{
	[HideInInspector]
	[SerializeField]
	private Rect mRect = new Rect(0f, 0f, 1f, 1f);

	[HideInInspector]
	[SerializeField]
	private Texture mTexture;

	[HideInInspector]
	[SerializeField]
	private Material mMat;

	[HideInInspector]
	[SerializeField]
	private Shader mShader;

	[HideInInspector]
	[SerializeField]
	private Vector4 mBorder = Vector4.zero;

	[HideInInspector]
	[SerializeField]
	private bool mFixedAspect;

	[NonSerialized]
	private int mPMA = -1;

	public override Vector4 border
	{
		get
		{
			return this.mBorder;
		}
		set
		{
			if (this.mBorder != value)
			{
				this.mBorder = value;
				this.MarkAsChanged();
			}
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			float single;
			float single1;
			Vector2 vector2 = base.pivotOffset;
			float single2 = -vector2.x * (float)this.mWidth;
			float single3 = -vector2.y * (float)this.mHeight;
			float single4 = single2 + (float)this.mWidth;
			float single5 = single3 + (float)this.mHeight;
			if (this.mTexture != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int num = this.mTexture.width;
				int num1 = this.mTexture.height;
				int num2 = 0;
				int num3 = 0;
				float single6 = 1f;
				float single7 = 1f;
				if (num > 0 && num1 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num & 1) != 0)
					{
						num2++;
					}
					if ((num1 & 1) != 0)
					{
						num3++;
					}
					single6 = 1f / (float)num * (float)this.mWidth;
					single7 = 1f / (float)num1 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					single2 = single2 + (float)num2 * single6;
				}
				else
				{
					single4 = single4 - (float)num2 * single6;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					single3 = single3 + (float)num3 * single7;
				}
				else
				{
					single5 = single5 - (float)num3 * single7;
				}
			}
			if (!this.mFixedAspect)
			{
				Vector4 vector4 = this.border;
				single = vector4.x + vector4.z;
				single1 = vector4.y + vector4.w;
			}
			else
			{
				single = 0f;
				single1 = 0f;
			}
			float single8 = Mathf.Lerp(single2, single4 - single, this.mDrawRegion.x);
			float single9 = Mathf.Lerp(single3, single5 - single1, this.mDrawRegion.y);
			float single10 = Mathf.Lerp(single2 + single, single4, this.mDrawRegion.z);
			float single11 = Mathf.Lerp(single3 + single1, single5, this.mDrawRegion.w);
			return new Vector4(single8, single9, single10, single11);
		}
	}

	public bool fixedAspect
	{
		get
		{
			return this.mFixedAspect;
		}
		set
		{
			if (this.mFixedAspect != value)
			{
				this.mFixedAspect = value;
				this.mDrawRegion = new Vector4(0f, 0f, 1f, 1f);
				this.MarkAsChanged();
			}
		}
	}

	public override Texture mainTexture
	{
		get
		{
			if (this.mTexture != null)
			{
				return this.mTexture;
			}
			if (this.mMat == null)
			{
				return null;
			}
			return this.mMat.mainTexture;
		}
		set
		{
			if (this.mTexture != value)
			{
				if (!(this.drawCall != null) || this.drawCall.widgetCount != 1 || !(this.mMat == null))
				{
					base.RemoveFromPanel();
					this.mTexture = value;
					this.mPMA = -1;
					this.MarkAsChanged();
				}
				else
				{
					this.mTexture = value;
					this.drawCall.mainTexture = value;
				}
			}
		}
	}

	public override Material material
	{
		get
		{
			return this.mMat;
		}
		set
		{
			if (this.mMat != value)
			{
				base.RemoveFromPanel();
				this.mShader = null;
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	public override bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = (!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied") ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	public override Shader shader
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat.shader;
			}
			if (this.mShader == null)
			{
				this.mShader = Shader.Find("Unlit/Transparent Colored");
			}
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				if (!(this.drawCall != null) || this.drawCall.widgetCount != 1 || !(this.mMat == null))
				{
					base.RemoveFromPanel();
					this.mShader = value;
					this.mPMA = -1;
					this.mMat = null;
					this.MarkAsChanged();
				}
				else
				{
					this.mShader = value;
					this.drawCall.shader = value;
				}
			}
		}
	}

	public Rect uvRect
	{
		get
		{
			return this.mRect;
		}
		set
		{
			if (this.mRect != value)
			{
				this.mRect = value;
				this.MarkAsChanged();
			}
		}
	}

	public UITexture()
	{
	}

	public override void MakePixelPerfect()
	{
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		Texture texture = this.mainTexture;
		if (texture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !base.hasBorder) && texture != null)
		{
			int num = texture.width;
			int num1 = texture.height;
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
		Rect rect = new Rect(this.mRect.x * (float)texture.width, this.mRect.y * (float)texture.height, (float)texture.width * this.mRect.width, (float)texture.height * this.mRect.height);
		Rect rect1 = rect;
		Vector4 vector4 = this.border;
		rect1.xMin = rect1.xMin + vector4.x;
		rect1.yMin = rect1.yMin + vector4.y;
		rect1.xMax = rect1.xMax - vector4.z;
		rect1.yMax = rect1.yMax - vector4.w;
		float single = 1f / (float)texture.width;
		float single1 = 1f / (float)texture.height;
		rect.xMin = rect.xMin * single;
		rect.xMax = rect.xMax * single;
		rect.yMin = rect.yMin * single1;
		rect.yMax = rect.yMax * single1;
		rect1.xMin = rect1.xMin * single;
		rect1.xMax = rect1.xMax * single;
		rect1.yMin = rect1.yMin * single1;
		rect1.yMax = rect1.yMax * single1;
		int num = verts.size;
		base.Fill(verts, uvs, cols, rect, rect1);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, num, verts, uvs, cols);
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mFixedAspect)
		{
			Texture texture = this.mainTexture;
			if (texture != null)
			{
				int num = texture.width;
				int num1 = texture.height;
				if ((num & 1) == 1)
				{
					num++;
				}
				if ((num1 & 1) == 1)
				{
					num1++;
				}
				float single = (float)this.mWidth;
				float single1 = (float)this.mHeight;
				float single2 = single / single1;
				float single3 = (float)num / (float)num1;
				if (single3 >= single2)
				{
					float single4 = (single1 - single / single3) / single1 * 0.5f;
					base.drawRegion = new Vector4(0f, single4, 1f, 1f - single4);
				}
				else
				{
					float single5 = (single - single1 * single3) / single * 0.5f;
					base.drawRegion = new Vector4(single5, 0f, 1f - single5, 1f);
				}
			}
		}
	}
}