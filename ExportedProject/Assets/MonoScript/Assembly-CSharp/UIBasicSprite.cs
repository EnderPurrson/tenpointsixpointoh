using System;
using UnityEngine;

public abstract class UIBasicSprite : UIWidget
{
	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite.Type mType;

	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite.FillDirection mFillDirection = UIBasicSprite.FillDirection.Radial360;

	[HideInInspector]
	[Range(0f, 1f)]
	[SerializeField]
	protected float mFillAmount = 1f;

	[HideInInspector]
	[SerializeField]
	protected bool mInvert;

	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite.Flip mFlip;

	[NonSerialized]
	private Rect mInnerUV = new Rect();

	[NonSerialized]
	private Rect mOuterUV = new Rect();

	public UIBasicSprite.AdvancedType centerType = UIBasicSprite.AdvancedType.Sliced;

	public UIBasicSprite.AdvancedType leftType = UIBasicSprite.AdvancedType.Sliced;

	public UIBasicSprite.AdvancedType rightType = UIBasicSprite.AdvancedType.Sliced;

	public UIBasicSprite.AdvancedType bottomType = UIBasicSprite.AdvancedType.Sliced;

	public UIBasicSprite.AdvancedType topType = UIBasicSprite.AdvancedType.Sliced;

	protected static Vector2[] mTempPos;

	protected static Vector2[] mTempUVs;

	private Color32 drawingColor
	{
		get
		{
			Color linearSpace = base.color;
			linearSpace.a = this.finalAlpha;
			if (this.premultipliedAlpha)
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
			return linearSpace;
		}
	}

	private Vector4 drawingUVs
	{
		get
		{
			switch (this.mFlip)
			{
				case UIBasicSprite.Flip.Horizontally:
				{
					return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMin, this.mOuterUV.xMin, this.mOuterUV.yMax);
				}
				case UIBasicSprite.Flip.Vertically:
				{
					return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMax, this.mOuterUV.xMax, this.mOuterUV.yMin);
				}
				case UIBasicSprite.Flip.Both:
				{
					return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMax, this.mOuterUV.xMin, this.mOuterUV.yMin);
				}
			}
			return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMin, this.mOuterUV.xMax, this.mOuterUV.yMax);
		}
	}

	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float single = Mathf.Clamp01(value);
			if (this.mFillAmount != single)
			{
				this.mFillAmount = single;
				this.mChanged = true;
			}
		}
	}

	public UIBasicSprite.FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				this.mChanged = true;
			}
		}
	}

	public UIBasicSprite.Flip flip
	{
		get
		{
			return this.mFlip;
		}
		set
		{
			if (this.mFlip != value)
			{
				this.mFlip = value;
				this.MarkAsChanged();
			}
		}
	}

	public bool hasBorder
	{
		get
		{
			Vector4 vector4 = this.border;
			return (vector4.x != 0f || vector4.y != 0f || vector4.z != 0f ? true : vector4.w != 0f);
		}
	}

	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				this.mChanged = true;
			}
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
			Vector4 vector4 = this.border * this.pixelSize;
			int num = Mathf.RoundToInt(vector4.y + vector4.w);
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
			Vector4 vector4 = this.border * this.pixelSize;
			int num = Mathf.RoundToInt(vector4.x + vector4.z);
			return Mathf.Max(base.minWidth, ((num & 1) != 1 ? num : num + 1));
		}
	}

	public virtual float pixelSize
	{
		get
		{
			return 1f;
		}
	}

	public virtual bool premultipliedAlpha
	{
		get
		{
			return false;
		}
	}

	public virtual UIBasicSprite.Type type
	{
		get
		{
			return this.mType;
		}
		set
		{
			if (this.mType != value)
			{
				this.mType = value;
				this.MarkAsChanged();
			}
		}
	}

	static UIBasicSprite()
	{
		UIBasicSprite.mTempPos = new Vector2[4];
		UIBasicSprite.mTempUVs = new Vector2[4];
	}

	protected UIBasicSprite()
	{
	}

	private void AdvancedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture texture = this.mainTexture;
		if (texture == null)
		{
			return;
		}
		Vector4 vector4 = this.border * this.pixelSize;
		if (vector4.x == 0f && vector4.y == 0f && vector4.z == 0f && vector4.w == 0f)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Color32 color32 = this.drawingColor;
		Vector4 vector41 = this.drawingDimensions;
		Vector2 vector2 = new Vector2(this.mInnerUV.width * (float)texture.width, this.mInnerUV.height * (float)texture.height);
		vector2 *= this.pixelSize;
		if (vector2.x < 1f)
		{
			vector2.x = 1f;
		}
		if (vector2.y < 1f)
		{
			vector2.y = 1f;
		}
		UIBasicSprite.mTempPos[0].x = vector41.x;
		UIBasicSprite.mTempPos[0].y = vector41.y;
		UIBasicSprite.mTempPos[3].x = vector41.z;
		UIBasicSprite.mTempPos[3].y = vector41.w;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector4.z;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector4.x;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector4.x;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector4.z;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector4.w;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector4.y;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector4.y;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector4.w;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UIBasicSprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num1 = j + 1;
					if (i == 1 && j == 1)
					{
						if (this.centerType == UIBasicSprite.AdvancedType.Tiled)
						{
							float single = UIBasicSprite.mTempPos[i].x;
							float single1 = UIBasicSprite.mTempPos[num].x;
							float single2 = UIBasicSprite.mTempPos[j].y;
							float single3 = UIBasicSprite.mTempPos[num1].y;
							float single4 = UIBasicSprite.mTempUVs[i].x;
							float single5 = UIBasicSprite.mTempUVs[j].y;
							for (float k = single2; k < single3; k += vector2.y)
							{
								float single6 = single;
								float single7 = UIBasicSprite.mTempUVs[num1].y;
								float single8 = k + vector2.y;
								if (single8 > single3)
								{
									single7 = Mathf.Lerp(single5, single7, (single3 - k) / vector2.y);
									single8 = single3;
								}
								while (single6 < single1)
								{
									float single9 = single6 + vector2.x;
									float single10 = UIBasicSprite.mTempUVs[num].x;
									if (single9 > single1)
									{
										single10 = Mathf.Lerp(single4, single10, (single1 - single6) / vector2.x);
										single9 = single1;
									}
									UIBasicSprite.Fill(verts, uvs, cols, single6, single9, k, single8, single4, single10, single5, single7, color32);
									single6 += vector2.x;
								}
							}
						}
						else if (this.centerType == UIBasicSprite.AdvancedType.Sliced)
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num1].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num1].y, color32);
						}
					}
					else if (i == 1)
					{
						if (j == 0 && this.bottomType == UIBasicSprite.AdvancedType.Tiled || j == 2 && this.topType == UIBasicSprite.AdvancedType.Tiled)
						{
							float single11 = UIBasicSprite.mTempPos[i].x;
							float single12 = UIBasicSprite.mTempPos[num].x;
							float single13 = UIBasicSprite.mTempPos[j].y;
							float single14 = UIBasicSprite.mTempPos[num1].y;
							float single15 = UIBasicSprite.mTempUVs[i].x;
							float single16 = UIBasicSprite.mTempUVs[j].y;
							float single17 = UIBasicSprite.mTempUVs[num1].y;
							for (float l = single11; l < single12; l += vector2.x)
							{
								float single18 = l + vector2.x;
								float single19 = UIBasicSprite.mTempUVs[num].x;
								if (single18 > single12)
								{
									single19 = Mathf.Lerp(single15, single19, (single12 - l) / vector2.x);
									single18 = single12;
								}
								UIBasicSprite.Fill(verts, uvs, cols, l, single18, single13, single14, single15, single19, single16, single17, color32);
							}
						}
						else if (j == 0 && this.bottomType != UIBasicSprite.AdvancedType.Invisible || j == 2 && this.topType != UIBasicSprite.AdvancedType.Invisible)
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num1].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num1].y, color32);
						}
					}
					else if (j != 1)
					{
						if (j == 0 && this.bottomType != UIBasicSprite.AdvancedType.Invisible || j == 2 && this.topType != UIBasicSprite.AdvancedType.Invisible || i == 0 && this.leftType != UIBasicSprite.AdvancedType.Invisible || i == 2 && this.rightType != UIBasicSprite.AdvancedType.Invisible)
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num1].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num1].y, color32);
						}
					}
					else if (i == 0 && this.leftType == UIBasicSprite.AdvancedType.Tiled || i == 2 && this.rightType == UIBasicSprite.AdvancedType.Tiled)
					{
						float single20 = UIBasicSprite.mTempPos[i].x;
						float single21 = UIBasicSprite.mTempPos[num].x;
						float single22 = UIBasicSprite.mTempPos[j].y;
						float single23 = UIBasicSprite.mTempPos[num1].y;
						float single24 = UIBasicSprite.mTempUVs[i].x;
						float single25 = UIBasicSprite.mTempUVs[num].x;
						float single26 = UIBasicSprite.mTempUVs[j].y;
						for (float m = single22; m < single23; m += vector2.y)
						{
							float single27 = UIBasicSprite.mTempUVs[num1].y;
							float single28 = m + vector2.y;
							if (single28 > single23)
							{
								single27 = Mathf.Lerp(single26, single27, (single23 - m) / vector2.y);
								single28 = single23;
							}
							UIBasicSprite.Fill(verts, uvs, cols, single20, single21, m, single28, single24, single25, single26, single27, color32);
						}
					}
					else if (i == 0 && this.leftType != UIBasicSprite.AdvancedType.Invisible || i == 2 && this.rightType != UIBasicSprite.AdvancedType.Invisible)
					{
						UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num1].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num1].y, color32);
					}
				}
			}
		}
	}

	protected void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, Rect outer, Rect inner)
	{
		this.mOuterUV = outer;
		this.mInnerUV = inner;
		switch (this.type)
		{
			case UIBasicSprite.Type.Simple:
			{
				this.SimpleFill(verts, uvs, cols);
				break;
			}
			case UIBasicSprite.Type.Sliced:
			{
				this.SlicedFill(verts, uvs, cols);
				break;
			}
			case UIBasicSprite.Type.Tiled:
			{
				this.TiledFill(verts, uvs, cols);
				break;
			}
			case UIBasicSprite.Type.Filled:
			{
				this.FilledFill(verts, uvs, cols);
				break;
			}
			case UIBasicSprite.Type.Advanced:
			{
				this.AdvancedFill(verts, uvs, cols);
				break;
			}
		}
	}

	private static void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, float v0x, float v1x, float v0y, float v1y, float u0x, float u1x, float u0y, float u1y, Color col)
	{
		verts.Add(new Vector3(v0x, v0y));
		verts.Add(new Vector3(v0x, v1y));
		verts.Add(new Vector3(v1x, v1y));
		verts.Add(new Vector3(v1x, v0y));
		uvs.Add(new Vector2(u0x, u0y));
		uvs.Add(new Vector2(u0x, u1y));
		uvs.Add(new Vector2(u1x, u1y));
		uvs.Add(new Vector2(u1x, u0y));
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
	}

	private void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		float single;
		float single1;
		float single2;
		float single3;
		float single4;
		float single5;
		float single6;
		float single7;
		if (this.mFillAmount < 0.001f)
		{
			return;
		}
		Vector4 vector4 = this.drawingDimensions;
		Vector4 vector41 = this.drawingUVs;
		Color32 color32 = this.drawingColor;
		if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal || this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
		{
			if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal)
			{
				float single8 = (vector41.z - vector41.x) * this.mFillAmount;
				if (!this.mInvert)
				{
					vector4.z = vector4.x + (vector4.z - vector4.x) * this.mFillAmount;
					vector41.z = vector41.x + single8;
				}
				else
				{
					vector4.x = vector4.z - (vector4.z - vector4.x) * this.mFillAmount;
					vector41.x = vector41.z - single8;
				}
			}
			else if (this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
			{
				float single9 = (vector41.w - vector41.y) * this.mFillAmount;
				if (!this.mInvert)
				{
					vector4.w = vector4.y + (vector4.w - vector4.y) * this.mFillAmount;
					vector41.w = vector41.y + single9;
				}
				else
				{
					vector4.y = vector4.w - (vector4.w - vector4.y) * this.mFillAmount;
					vector41.y = vector41.w - single9;
				}
			}
		}
		UIBasicSprite.mTempPos[0] = new Vector2(vector4.x, vector4.y);
		UIBasicSprite.mTempPos[1] = new Vector2(vector4.x, vector4.w);
		UIBasicSprite.mTempPos[2] = new Vector2(vector4.z, vector4.w);
		UIBasicSprite.mTempPos[3] = new Vector2(vector4.z, vector4.y);
		UIBasicSprite.mTempUVs[0] = new Vector2(vector41.x, vector41.y);
		UIBasicSprite.mTempUVs[1] = new Vector2(vector41.x, vector41.w);
		UIBasicSprite.mTempUVs[2] = new Vector2(vector41.z, vector41.w);
		UIBasicSprite.mTempUVs[3] = new Vector2(vector41.z, vector41.y);
		if (this.mFillAmount < 1f)
		{
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial90)
			{
				if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, this.mFillAmount, this.mInvert, 0))
				{
					for (int i = 0; i < 4; i++)
					{
						verts.Add(UIBasicSprite.mTempPos[i]);
						uvs.Add(UIBasicSprite.mTempUVs[i]);
						cols.Add(color32);
					}
				}
				return;
			}
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial180)
			{
				for (int j = 0; j < 2; j++)
				{
					float single10 = 0f;
					float single11 = 1f;
					if (j != 0)
					{
						single = 0.5f;
						single1 = 1f;
					}
					else
					{
						single = 0f;
						single1 = 0.5f;
					}
					UIBasicSprite.mTempPos[0].x = Mathf.Lerp(vector4.x, vector4.z, single);
					UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
					UIBasicSprite.mTempPos[2].x = Mathf.Lerp(vector4.x, vector4.z, single1);
					UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
					UIBasicSprite.mTempPos[0].y = Mathf.Lerp(vector4.y, vector4.w, single10);
					UIBasicSprite.mTempPos[1].y = Mathf.Lerp(vector4.y, vector4.w, single11);
					UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
					UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
					UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(vector41.x, vector41.z, single);
					UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
					UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(vector41.x, vector41.z, single1);
					UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
					UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(vector41.y, vector41.w, single10);
					UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(vector41.y, vector41.w, single11);
					UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
					UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
					single7 = (this.mInvert ? this.mFillAmount * 2f - (float)(1 - j) : this.fillAmount * 2f - (float)j);
					if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(single7), !this.mInvert, NGUIMath.RepeatIndex(j + 3, 4)))
					{
						for (int k = 0; k < 4; k++)
						{
							verts.Add(UIBasicSprite.mTempPos[k]);
							uvs.Add(UIBasicSprite.mTempUVs[k]);
							cols.Add(color32);
						}
					}
				}
				return;
			}
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial360)
			{
				for (int l = 0; l < 4; l++)
				{
					if (l >= 2)
					{
						single2 = 0.5f;
						single3 = 1f;
					}
					else
					{
						single2 = 0f;
						single3 = 0.5f;
					}
					if (l == 0 || l == 3)
					{
						single4 = 0f;
						single5 = 0.5f;
					}
					else
					{
						single4 = 0.5f;
						single5 = 1f;
					}
					UIBasicSprite.mTempPos[0].x = Mathf.Lerp(vector4.x, vector4.z, single2);
					UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
					UIBasicSprite.mTempPos[2].x = Mathf.Lerp(vector4.x, vector4.z, single3);
					UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
					UIBasicSprite.mTempPos[0].y = Mathf.Lerp(vector4.y, vector4.w, single4);
					UIBasicSprite.mTempPos[1].y = Mathf.Lerp(vector4.y, vector4.w, single5);
					UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
					UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
					UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(vector41.x, vector41.z, single2);
					UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
					UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(vector41.x, vector41.z, single3);
					UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
					UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(vector41.y, vector41.w, single4);
					UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(vector41.y, vector41.w, single5);
					UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
					UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
					single6 = (!this.mInvert ? this.mFillAmount * 4f - (float)(3 - NGUIMath.RepeatIndex(l + 2, 4)) : this.mFillAmount * 4f - (float)NGUIMath.RepeatIndex(l + 2, 4));
					if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(single6), this.mInvert, NGUIMath.RepeatIndex(l + 2, 4)))
					{
						for (int m = 0; m < 4; m++)
						{
							verts.Add(UIBasicSprite.mTempPos[m]);
							uvs.Add(UIBasicSprite.mTempUVs[m]);
							cols.Add(color32);
						}
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(UIBasicSprite.mTempPos[n]);
			uvs.Add(UIBasicSprite.mTempUVs[n]);
			cols.Add(color32);
		}
	}

	private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if ((corner & 1) == 1)
		{
			invert = !invert;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float single = Mathf.Clamp01(fill);
		if (invert)
		{
			single = 1f - single;
		}
		single *= 1.5707964f;
		float single1 = Mathf.Cos(single);
		float single2 = Mathf.Sin(single);
		UIBasicSprite.RadialCut(xy, single1, single2, invert, corner);
		UIBasicSprite.RadialCut(uv, single1, single2, invert, corner);
		return true;
	}

	private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
	{
		int num = corner;
		int num1 = NGUIMath.RepeatIndex(corner + 1, 4);
		int num2 = NGUIMath.RepeatIndex(corner + 2, 4);
		int num3 = NGUIMath.RepeatIndex(corner + 3, 4);
		if ((corner & 1) != 1)
		{
			if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num1].y = Mathf.Lerp(xy[num].y, xy[num2].y, sin);
					xy[num2].y = xy[num1].y;
				}
			}
			else if (sin <= cos)
			{
				cos = 1f;
				sin = 1f;
			}
			else
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num2].x = Mathf.Lerp(xy[num].x, xy[num2].x, cos);
					xy[num3].x = xy[num2].x;
				}
			}
			if (!invert)
			{
				xy[num1].x = Mathf.Lerp(xy[num].x, xy[num2].x, cos);
			}
			else
			{
				xy[num3].y = Mathf.Lerp(xy[num].y, xy[num2].y, sin);
			}
		}
		else
		{
			if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num1].x = Mathf.Lerp(xy[num].x, xy[num2].x, cos);
					xy[num2].x = xy[num1].x;
				}
			}
			else if (cos <= sin)
			{
				cos = 1f;
				sin = 1f;
			}
			else
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num2].y = Mathf.Lerp(xy[num].y, xy[num2].y, sin);
					xy[num3].y = xy[num2].y;
				}
			}
			if (invert)
			{
				xy[num1].y = Mathf.Lerp(xy[num].y, xy[num2].y, sin);
			}
			else
			{
				xy[num3].x = Mathf.Lerp(xy[num].x, xy[num2].x, cos);
			}
		}
	}

	private void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 vector4 = this.drawingDimensions;
		Vector4 vector41 = this.drawingUVs;
		Color32 color32 = this.drawingColor;
		verts.Add(new Vector3(vector4.x, vector4.y));
		verts.Add(new Vector3(vector4.x, vector4.w));
		verts.Add(new Vector3(vector4.z, vector4.w));
		verts.Add(new Vector3(vector4.z, vector4.y));
		uvs.Add(new Vector2(vector41.x, vector41.y));
		uvs.Add(new Vector2(vector41.x, vector41.w));
		uvs.Add(new Vector2(vector41.z, vector41.w));
		uvs.Add(new Vector2(vector41.z, vector41.y));
		cols.Add(color32);
		cols.Add(color32);
		cols.Add(color32);
		cols.Add(color32);
	}

	private void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 vector4 = this.border * this.pixelSize;
		if (vector4.x == 0f && vector4.y == 0f && vector4.z == 0f && vector4.w == 0f)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Color32 color32 = this.drawingColor;
		Vector4 vector41 = this.drawingDimensions;
		UIBasicSprite.mTempPos[0].x = vector41.x;
		UIBasicSprite.mTempPos[0].y = vector41.y;
		UIBasicSprite.mTempPos[3].x = vector41.z;
		UIBasicSprite.mTempPos[3].y = vector41.w;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector4.z;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector4.x;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector4.x;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector4.z;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector4.w;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector4.y;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector4.y;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector4.w;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UIBasicSprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num1 = j + 1;
					verts.Add(new Vector3(UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[j].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num1].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[num1].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[j].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num1].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[num1].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y));
					cols.Add(color32);
					cols.Add(color32);
					cols.Add(color32);
					cols.Add(color32);
				}
			}
		}
	}

	private void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 vector4 = new Vector4();
		Texture texture = this.mainTexture;
		if (texture == null)
		{
			return;
		}
		Vector2 vector2 = new Vector2(this.mInnerUV.width * (float)texture.width, this.mInnerUV.height * (float)texture.height);
		vector2 *= this.pixelSize;
		if (texture == null || vector2.x < 2f || vector2.y < 2f)
		{
			return;
		}
		Color32 color32 = this.drawingColor;
		Vector4 vector41 = this.drawingDimensions;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			vector4.x = this.mInnerUV.xMax;
			vector4.z = this.mInnerUV.xMin;
		}
		else
		{
			vector4.x = this.mInnerUV.xMin;
			vector4.z = this.mInnerUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			vector4.y = this.mInnerUV.yMax;
			vector4.w = this.mInnerUV.yMin;
		}
		else
		{
			vector4.y = this.mInnerUV.yMin;
			vector4.w = this.mInnerUV.yMax;
		}
		float single = vector41.x;
		float single1 = vector41.y;
		float single2 = vector4.x;
		float single3 = vector4.y;
		while (single1 < vector41.w)
		{
			single = vector41.x;
			float single4 = single1 + vector2.y;
			float single5 = vector4.w;
			if (single4 > vector41.w)
			{
				single5 = Mathf.Lerp(vector4.y, vector4.w, (vector41.w - single1) / vector2.y);
				single4 = vector41.w;
			}
			while (single < vector41.z)
			{
				float single6 = single + vector2.x;
				float single7 = vector4.z;
				if (single6 > vector41.z)
				{
					single7 = Mathf.Lerp(vector4.x, vector4.z, (vector41.z - single) / vector2.x);
					single6 = vector41.z;
				}
				verts.Add(new Vector3(single, single1));
				verts.Add(new Vector3(single, single4));
				verts.Add(new Vector3(single6, single4));
				verts.Add(new Vector3(single6, single1));
				uvs.Add(new Vector2(single2, single3));
				uvs.Add(new Vector2(single2, single5));
				uvs.Add(new Vector2(single7, single5));
				uvs.Add(new Vector2(single7, single3));
				cols.Add(color32);
				cols.Add(color32);
				cols.Add(color32);
				cols.Add(color32);
				single += vector2.x;
			}
			single1 += vector2.y;
		}
	}

	public enum AdvancedType
	{
		Invisible,
		Sliced,
		Tiled
	}

	public enum FillDirection
	{
		Horizontal,
		Vertical,
		Radial90,
		Radial180,
		Radial360
	}

	public enum Flip
	{
		Nothing,
		Horizontally,
		Vertically,
		Both
	}

	public enum Type
	{
		Simple,
		Sliced,
		Tiled,
		Filled,
		Advanced
	}
}