using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite")]
[ExecuteInEditMode]
public class UI2DSprite : UIBasicSprite
{
	[HideInInspector]
	[SerializeField]
	private Sprite mSprite;

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

	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	public Sprite nextSprite;

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
			if (this.mSprite != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int num = Mathf.RoundToInt(this.mSprite.rect.width);
				int num1 = Mathf.RoundToInt(this.mSprite.rect.height);
				int num2 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
				int num3 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
				float single6 = this.mSprite.rect.width - this.mSprite.textureRect.width;
				Vector2 vector21 = this.mSprite.textureRectOffset;
				int num4 = Mathf.RoundToInt(single6 - vector21.x);
				float single7 = this.mSprite.rect.height - this.mSprite.textureRect.height;
				Vector2 vector22 = this.mSprite.textureRectOffset;
				int num5 = Mathf.RoundToInt(single7 - vector22.y);
				float single8 = 1f;
				float single9 = 1f;
				if (num > 0 && num1 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num & 1) != 0)
					{
						num4++;
					}
					if ((num1 & 1) != 0)
					{
						num5++;
					}
					single8 = 1f / (float)num * (float)this.mWidth;
					single9 = 1f / (float)num1 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					single2 = single2 + (float)num4 * single8;
					single4 = single4 - (float)num2 * single8;
				}
				else
				{
					single2 = single2 + (float)num2 * single8;
					single4 = single4 - (float)num4 * single8;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					single3 = single3 + (float)num5 * single9;
					single5 = single5 - (float)num3 * single9;
				}
				else
				{
					single3 = single3 + (float)num3 * single9;
					single5 = single5 - (float)num5 * single9;
				}
			}
			if (!this.mFixedAspect)
			{
				Vector4 vector4 = this.border * this.pixelSize;
				single = vector4.x + vector4.z;
				single1 = vector4.y + vector4.w;
			}
			else
			{
				single = 0f;
				single1 = 0f;
			}
			float single10 = Mathf.Lerp(single2, single4 - single, this.mDrawRegion.x);
			float single11 = Mathf.Lerp(single3, single5 - single1, this.mDrawRegion.y);
			float single12 = Mathf.Lerp(single2 + single, single4, this.mDrawRegion.z);
			float single13 = Mathf.Lerp(single3 + single1, single5, this.mDrawRegion.w);
			return new Vector4(single10, single11, single12, single13);
		}
	}

	public override Texture mainTexture
	{
		get
		{
			if (this.mSprite != null)
			{
				return this.mSprite.texture;
			}
			if (this.mMat == null)
			{
				return null;
			}
			return this.mMat.mainTexture;
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
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	public override float pixelSize
	{
		get
		{
			return this.mPixelSize;
		}
	}

	public override bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Shader shader = this.shader;
				this.mPMA = (!(shader != null) || !shader.name.Contains("Premultiplied") ? 0 : 1);
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
				base.RemoveFromPanel();
				this.mShader = value;
				if (this.mMat == null)
				{
					this.mPMA = -1;
					this.MarkAsChanged();
				}
			}
		}
	}

	public Sprite sprite2D
	{
		get
		{
			return this.mSprite;
		}
		set
		{
			if (this.mSprite != value)
			{
				base.RemoveFromPanel();
				this.mSprite = value;
				this.nextSprite = null;
				base.CreatePanel();
			}
		}
	}

	public UI2DSprite()
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
			Rect rect = this.mSprite.rect;
			int num = Mathf.RoundToInt(rect.width);
			int num1 = Mathf.RoundToInt(rect.height);
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
		Rect rect = (this.mSprite == null ? new Rect(0f, 0f, (float)texture.width, (float)texture.height) : this.mSprite.textureRect);
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
		if (this.nextSprite != null)
		{
			if (this.nextSprite != this.mSprite)
			{
				this.sprite2D = this.nextSprite;
			}
			this.nextSprite = null;
		}
		base.OnUpdate();
		if (this.mFixedAspect && this.mainTexture != null)
		{
			int num = Mathf.RoundToInt(this.mSprite.rect.width);
			int num1 = Mathf.RoundToInt(this.mSprite.rect.height);
			int num2 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
			int num3 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
			float single = this.mSprite.rect.width - this.mSprite.textureRect.width;
			Vector2 vector2 = this.mSprite.textureRectOffset;
			int num4 = Mathf.RoundToInt(single - vector2.x);
			float single1 = this.mSprite.rect.height - this.mSprite.textureRect.height;
			Vector2 vector21 = this.mSprite.textureRectOffset;
			int num5 = Mathf.RoundToInt(single1 - vector21.y);
			num = num + num2 + num4;
			num1 = num1 + num5 + num3;
			float single2 = (float)this.mWidth;
			float single3 = (float)this.mHeight;
			float single4 = single2 / single3;
			float single5 = (float)num / (float)num1;
			if (single5 >= single4)
			{
				float single6 = (single3 - single2 / single5) / single3 * 0.5f;
				base.drawRegion = new Vector4(0f, single6, 1f, 1f - single6);
			}
			else
			{
				float single7 = (single2 - single3 * single5) / single2 * 0.5f;
				base.drawRegion = new Vector4(single7, 0f, 1f - single7, 1f);
			}
		}
	}
}