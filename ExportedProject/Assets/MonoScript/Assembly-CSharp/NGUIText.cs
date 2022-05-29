using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class NGUIText
{
	public static UIFont bitmapFont;

	public static Font dynamicFont;

	public static NGUIText.GlyphInfo glyph;

	public static int fontSize;

	public static float fontScale;

	public static float pixelDensity;

	public static FontStyle fontStyle;

	public static NGUIText.Alignment alignment;

	public static Color tint;

	public static int rectWidth;

	public static int rectHeight;

	public static int regionWidth;

	public static int regionHeight;

	public static int maxLines;

	public static bool gradient;

	public static Color gradientBottom;

	public static Color gradientTop;

	public static bool encoding;

	public static float spacingX;

	public static float spacingY;

	public static bool premultiply;

	public static NGUIText.SymbolStyle symbolStyle;

	public static int finalSize;

	public static float finalSpacingX;

	public static float finalLineHeight;

	public static float baseline;

	public static bool useSymbols;

	private static Color mInvisible;

	private static BetterList<Color> mColors;

	private static float mAlpha;

	private static CharacterInfo mTempChar;

	private static BetterList<float> mSizes;

	private static Color32 s_c0;

	private static Color32 s_c1;

	private static float[] mBoldOffset;

	static NGUIText()
	{
		NGUIText.glyph = new NGUIText.GlyphInfo();
		NGUIText.fontSize = 16;
		NGUIText.fontScale = 1f;
		NGUIText.pixelDensity = 1f;
		NGUIText.fontStyle = FontStyle.Normal;
		NGUIText.alignment = NGUIText.Alignment.Left;
		NGUIText.tint = Color.white;
		NGUIText.rectWidth = 1000000;
		NGUIText.rectHeight = 1000000;
		NGUIText.regionWidth = 1000000;
		NGUIText.regionHeight = 1000000;
		NGUIText.maxLines = 0;
		NGUIText.gradient = false;
		NGUIText.gradientBottom = Color.white;
		NGUIText.gradientTop = Color.white;
		NGUIText.encoding = false;
		NGUIText.spacingX = 0f;
		NGUIText.spacingY = 0f;
		NGUIText.premultiply = false;
		NGUIText.finalSize = 0;
		NGUIText.finalSpacingX = 0f;
		NGUIText.finalLineHeight = 0f;
		NGUIText.baseline = 0f;
		NGUIText.useSymbols = false;
		NGUIText.mInvisible = new Color(0f, 0f, 0f, 0f);
		NGUIText.mColors = new BetterList<Color>();
		NGUIText.mAlpha = 1f;
		NGUIText.mSizes = new BetterList<float>();
		NGUIText.mBoldOffset = new float[] { -0.25f, 0f, 0.25f, 0f, 0f, -0.25f, 0f, 0.25f };
	}

	public static void Align(BetterList<Vector3> verts, int indexOffset, float printedWidth, int elements = 4)
	{
		switch (NGUIText.alignment)
		{
			case NGUIText.Alignment.Center:
			{
				float single = ((float)NGUIText.rectWidth - printedWidth) * 0.5f;
				if (single < 0f)
				{
					return;
				}
				int num = Mathf.RoundToInt((float)NGUIText.rectWidth - printedWidth);
				int num1 = Mathf.RoundToInt((float)NGUIText.rectWidth);
				bool flag = (num & 1) == 1;
				bool flag1 = (num1 & 1) == 1;
				if (flag && !flag1 || !flag && flag1)
				{
					single = single + 0.5f * NGUIText.fontScale;
				}
				for (int i = indexOffset; i < verts.size; i++)
				{
					verts.buffer[i].x += single;
				}
				break;
			}
			case NGUIText.Alignment.Right:
			{
				float single1 = (float)NGUIText.rectWidth - printedWidth;
				if (single1 < 0f)
				{
					return;
				}
				for (int j = indexOffset; j < verts.size; j++)
				{
					verts.buffer[j].x += single1;
				}
				break;
			}
			case NGUIText.Alignment.Justified:
			{
				if (printedWidth < (float)NGUIText.rectWidth * 0.65f)
				{
					return;
				}
				if (((float)NGUIText.rectWidth - printedWidth) * 0.5f < 1f)
				{
					return;
				}
				int num2 = (verts.size - indexOffset) / elements;
				if (num2 < 1)
				{
					return;
				}
				float single2 = 1f / (float)(num2 - 1);
				float single3 = (float)NGUIText.rectWidth / printedWidth;
				int num3 = indexOffset + elements;
				int num4 = 1;
				while (num3 < verts.size)
				{
					float single4 = verts.buffer[num3].x;
					float single5 = verts.buffer[num3 + elements / 2].x;
					float single6 = single5 - single4;
					float single7 = single4 * single3;
					float single8 = single7 + single6;
					float single9 = single5 * single3;
					float single10 = single9 - single6;
					float single11 = (float)num4 * single2;
					single5 = Mathf.Lerp(single8, single9, single11);
					single4 = Mathf.Lerp(single7, single10, single11);
					single4 = Mathf.Round(single4);
					single5 = Mathf.Round(single5);
					if (elements == 4)
					{
						int num5 = num3;
						num3 = num5 + 1;
						verts.buffer[num5].x = single4;
						int num6 = num3;
						num3 = num6 + 1;
						verts.buffer[num6].x = single4;
						int num7 = num3;
						num3 = num7 + 1;
						verts.buffer[num7].x = single5;
						int num8 = num3;
						num3 = num8 + 1;
						verts.buffer[num8].x = single5;
					}
					else if (elements == 2)
					{
						int num9 = num3;
						num3 = num9 + 1;
						verts.buffer[num9].x = single4;
						int num10 = num3;
						num3 = num10 + 1;
						verts.buffer[num10].x = single5;
					}
					else if (elements == 1)
					{
						int num11 = num3;
						num3 = num11 + 1;
						verts.buffer[num11].x = single4;
					}
					num4++;
				}
				break;
			}
		}
	}

	public static int CalculateOffsetToFit(string text)
	{
		BMSymbol symbol;
		if (string.IsNullOrEmpty(text) || NGUIText.regionWidth < 1)
		{
			return 0;
		}
		NGUIText.Prepare(text);
		int length = text.Length;
		int num = 0;
		int num1 = 0;
		int length1 = 0;
		int length2 = text.Length;
		while (length1 < length2)
		{
			if (!NGUIText.useSymbols)
			{
				symbol = null;
			}
			else
			{
				symbol = NGUIText.GetSymbol(text, length1, length);
			}
			BMSymbol bMSymbol = symbol;
			if (bMSymbol != null)
			{
				NGUIText.mSizes.Add(NGUIText.finalSpacingX + (float)bMSymbol.advance * NGUIText.fontScale);
				int num2 = 0;
				int length3 = bMSymbol.sequence.Length - 1;
				while (num2 < length3)
				{
					NGUIText.mSizes.Add(0f);
					num2++;
				}
				length1 = length1 + (bMSymbol.sequence.Length - 1);
				num1 = 0;
			}
			else
			{
				num = text[length1];
				float glyphWidth = NGUIText.GetGlyphWidth(num, num1);
				if (glyphWidth != 0f)
				{
					NGUIText.mSizes.Add(NGUIText.finalSpacingX + glyphWidth);
				}
				num1 = num;
			}
			length1++;
		}
		float item = (float)NGUIText.regionWidth;
		int num3 = NGUIText.mSizes.size;
		while (num3 > 0 && item > 0f)
		{
			int num4 = num3 - 1;
			num3 = num4;
			item -= NGUIText.mSizes[num4];
		}
		NGUIText.mSizes.Clear();
		if (item < 0f)
		{
			num3++;
		}
		return num3;
	}

	public static Vector2 CalculatePrintedSize(string text)
	{
		BMSymbol symbol;
		Vector2 vector2 = Vector2.zero;
		if (!string.IsNullOrEmpty(text))
		{
			if (NGUIText.encoding)
			{
				text = NGUIText.StripSymbols(text);
			}
			NGUIText.Prepare(text);
			float single = 0f;
			float single1 = 0f;
			float single2 = 0f;
			int length = text.Length;
			int num = 0;
			int num1 = 0;
			for (int i = 0; i < length; i++)
			{
				num = text[i];
				if (num == 10)
				{
					if (single > single2)
					{
						single2 = single;
					}
					single = 0f;
					single1 += NGUIText.finalLineHeight;
				}
				else if (num >= 32)
				{
					if (!NGUIText.useSymbols)
					{
						symbol = null;
					}
					else
					{
						symbol = NGUIText.GetSymbol(text, i, length);
					}
					BMSymbol bMSymbol = symbol;
					if (bMSymbol != null)
					{
						float single3 = NGUIText.finalSpacingX + (float)bMSymbol.advance * NGUIText.fontScale;
						if (Mathf.RoundToInt(single + single3) <= NGUIText.regionWidth)
						{
							single += single3;
						}
						else
						{
							if (single > single2)
							{
								single2 = single - NGUIText.finalSpacingX;
							}
							single = single3;
							single1 += NGUIText.finalLineHeight;
						}
						i = i + (bMSymbol.sequence.Length - 1);
						num1 = 0;
					}
					else
					{
						float glyphWidth = NGUIText.GetGlyphWidth(num, num1);
						if (glyphWidth != 0f)
						{
							glyphWidth += NGUIText.finalSpacingX;
							if (Mathf.RoundToInt(single + glyphWidth) <= NGUIText.regionWidth)
							{
								single += glyphWidth;
							}
							else
							{
								if (single > single2)
								{
									single2 = single - NGUIText.finalSpacingX;
								}
								single = glyphWidth;
								single1 += NGUIText.finalLineHeight;
							}
							num1 = num;
						}
					}
				}
			}
			vector2.x = (single <= single2 ? single2 : single - NGUIText.finalSpacingX);
			vector2.y = single1 + NGUIText.finalLineHeight;
		}
		return vector2;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeAlpha(float a)
	{
		int num = Mathf.Clamp(Mathf.RoundToInt(a * 255f), 0, 255);
		return NGUIMath.DecimalToHex8(num);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeColor(string text, Color c)
	{
		return string.Concat(new string[] { "[c][", NGUIText.EncodeColor24(c), "]", text, "[-][/c]" });
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeColor24(Color c)
	{
		int num = 16777215 & NGUIMath.ColorToInt(c) >> 8;
		return NGUIMath.DecimalToHex24(num);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeColor32(Color c)
	{
		return NGUIMath.DecimalToHex32(NGUIMath.ColorToInt(c));
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static void EndLine(ref StringBuilder s)
	{
		int length = s.Length - 1;
		if (length <= 0 || !NGUIText.IsSpace(s[length]))
		{
			s.Append('\n');
		}
		else
		{
			s[length] = '\n';
		}
	}

	public static int GetApproximateCharacterIndex(BetterList<Vector3> verts, BetterList<int> indices, Vector2 pos)
	{
		float single = Single.MaxValue;
		float single1 = Single.MaxValue;
		int num = 0;
		for (int i = 0; i < verts.size; i++)
		{
			float single2 = pos.y;
			Vector3 item = verts[i];
			float single3 = Mathf.Abs(single2 - item.y);
			if (single3 <= single1)
			{
				float single4 = pos.x;
				Vector3 vector3 = verts[i];
				float single5 = Mathf.Abs(single4 - vector3.x);
				if (single3 < single1)
				{
					single1 = single3;
					single = single5;
					num = i;
				}
				else if (single5 < single)
				{
					single = single5;
					num = i;
				}
			}
		}
		return indices[num];
	}

	public static string GetEndOfLineThatFits(string text)
	{
		int length = text.Length;
		int fit = NGUIText.CalculateOffsetToFit(text);
		return text.Substring(fit, length - fit);
	}

	public static int GetExactCharacterIndex(BetterList<Vector3> verts, BetterList<int> indices, Vector2 pos)
	{
		for (int i = 0; i < indices.size; i++)
		{
			int num = i << 1;
			int num1 = num + 1;
			float item = verts[num].x;
			if (pos.x >= item)
			{
				float single = verts[num1].x;
				if (pos.x <= single)
				{
					float item1 = verts[num].y;
					if (pos.y >= item1)
					{
						float single1 = verts[num1].y;
						if (pos.y <= single1)
						{
							return indices[i];
						}
					}
				}
			}
		}
		return 0;
	}

	public static NGUIText.GlyphInfo GetGlyph(int ch, int prev)
	{
		if (NGUIText.bitmapFont != null)
		{
			bool flag = false;
			if (ch == 8201)
			{
				flag = true;
				ch = 32;
			}
			BMGlyph glyph = NGUIText.bitmapFont.bmFont.GetGlyph(ch);
			if (glyph != null)
			{
				int num = (prev == 0 ? 0 : glyph.GetKerning(prev));
				NGUIText.glyph.v0.x = (float)((prev == 0 ? glyph.offsetX : glyph.offsetX + num));
				NGUIText.glyph.v1.y = (float)(-glyph.offsetY);
				NGUIText.glyph.v1.x = NGUIText.glyph.v0.x + (float)glyph.width;
				NGUIText.glyph.v0.y = NGUIText.glyph.v1.y - (float)glyph.height;
				NGUIText.glyph.u0.x = (float)glyph.x;
				NGUIText.glyph.u0.y = (float)(glyph.y + glyph.height);
				NGUIText.glyph.u2.x = (float)(glyph.x + glyph.width);
				NGUIText.glyph.u2.y = (float)glyph.y;
				NGUIText.glyph.u1.x = NGUIText.glyph.u0.x;
				NGUIText.glyph.u1.y = NGUIText.glyph.u2.y;
				NGUIText.glyph.u3.x = NGUIText.glyph.u2.x;
				NGUIText.glyph.u3.y = NGUIText.glyph.u0.y;
				int num1 = glyph.advance;
				if (flag)
				{
					num1 >>= 1;
				}
				NGUIText.glyph.advance = (float)(num1 + num);
				NGUIText.glyph.channel = glyph.channel;
				if (NGUIText.fontScale != 1f)
				{
					NGUIText.glyph.v0 *= NGUIText.fontScale;
					NGUIText.glyph.v1 *= NGUIText.fontScale;
					NGUIText.glyph.advance *= NGUIText.fontScale;
				}
				return NGUIText.glyph;
			}
		}
		else if (NGUIText.dynamicFont != null && NGUIText.dynamicFont.GetCharacterInfo((char)ch, out NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
		{
			NGUIText.glyph.v0.x = (float)NGUIText.mTempChar.minX;
			NGUIText.glyph.v1.x = (float)NGUIText.mTempChar.maxX;
			NGUIText.glyph.v0.y = (float)NGUIText.mTempChar.maxY - NGUIText.baseline;
			NGUIText.glyph.v1.y = (float)NGUIText.mTempChar.minY - NGUIText.baseline;
			NGUIText.glyph.u0 = NGUIText.mTempChar.uvTopLeft;
			NGUIText.glyph.u1 = NGUIText.mTempChar.uvBottomLeft;
			NGUIText.glyph.u2 = NGUIText.mTempChar.uvBottomRight;
			NGUIText.glyph.u3 = NGUIText.mTempChar.uvTopRight;
			NGUIText.glyph.advance = (float)NGUIText.mTempChar.advance;
			NGUIText.glyph.channel = 0;
			NGUIText.glyph.v0.x = Mathf.Round(NGUIText.glyph.v0.x);
			NGUIText.glyph.v0.y = Mathf.Round(NGUIText.glyph.v0.y);
			NGUIText.glyph.v1.x = Mathf.Round(NGUIText.glyph.v1.x);
			NGUIText.glyph.v1.y = Mathf.Round(NGUIText.glyph.v1.y);
			float single = NGUIText.fontScale * NGUIText.pixelDensity;
			if (single != 1f)
			{
				NGUIText.glyph.v0 *= single;
				NGUIText.glyph.v1 *= single;
				NGUIText.glyph.advance *= single;
			}
			return NGUIText.glyph;
		}
		return null;
	}

	public static float GetGlyphWidth(int ch, int prev)
	{
		if (NGUIText.bitmapFont != null)
		{
			bool flag = false;
			if (ch == 8201)
			{
				flag = true;
				ch = 32;
			}
			BMGlyph glyph = NGUIText.bitmapFont.bmFont.GetGlyph(ch);
			if (glyph != null)
			{
				int num = glyph.advance;
				if (flag)
				{
					num >>= 1;
				}
				return NGUIText.fontScale * (float)((prev == 0 ? glyph.advance : num + glyph.GetKerning(prev)));
			}
		}
		else if (NGUIText.dynamicFont != null && NGUIText.dynamicFont.GetCharacterInfo((char)ch, out NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
		{
			return (float)NGUIText.mTempChar.advance * NGUIText.fontScale * NGUIText.pixelDensity;
		}
		return 0f;
	}

	public static BMSymbol GetSymbol(string text, int index, int textLength)
	{
		BMSymbol bMSymbol;
		if (NGUIText.bitmapFont == null)
		{
			bMSymbol = null;
		}
		else
		{
			bMSymbol = NGUIText.bitmapFont.MatchSymbol(text, index, textLength);
		}
		return bMSymbol;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool IsHex(char ch)
	{
		bool flag;
		if ((ch < '0' || ch > '9') && (ch < 'a' || ch > 'f'))
		{
			flag = (ch < 'A' ? false : ch <= 'F');
		}
		else
		{
			flag = true;
		}
		return flag;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	private static bool IsSpace(int ch)
	{
		return (ch == 32 || ch == 8202 || ch == 8203 ? true : ch == 8201);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float ParseAlpha(string text, int index)
	{
		int num = NGUIMath.HexToDecimal(text[index + 1]) << 4 | NGUIMath.HexToDecimal(text[index + 2]);
		return Mathf.Clamp01((float)num / 255f);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color ParseColor24(string text, int offset)
	{
		int num = NGUIMath.HexToDecimal(text[offset]) << 4 | NGUIMath.HexToDecimal(text[offset + 1]);
		int num1 = NGUIMath.HexToDecimal(text[offset + 2]) << 4 | NGUIMath.HexToDecimal(text[offset + 3]);
		int num2 = NGUIMath.HexToDecimal(text[offset + 4]) << 4 | NGUIMath.HexToDecimal(text[offset + 5]);
		float single = 0.003921569f;
		return new Color(single * (float)num, single * (float)num1, single * (float)num2);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color ParseColor32(string text, int offset)
	{
		int num = NGUIMath.HexToDecimal(text[offset]) << 4 | NGUIMath.HexToDecimal(text[offset + 1]);
		int num1 = NGUIMath.HexToDecimal(text[offset + 2]) << 4 | NGUIMath.HexToDecimal(text[offset + 3]);
		int num2 = NGUIMath.HexToDecimal(text[offset + 4]) << 4 | NGUIMath.HexToDecimal(text[offset + 5]);
		int num3 = NGUIMath.HexToDecimal(text[offset + 6]) << 4 | NGUIMath.HexToDecimal(text[offset + 7]);
		float single = 0.003921569f;
		return new Color(single * (float)num, single * (float)num1, single * (float)num2, single * (float)num3);
	}

	public static bool ParseSymbol(string text, ref int index)
	{
		int num = 1;
		bool flag = false;
		bool flag1 = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		return NGUIText.ParseSymbol(text, ref index, null, false, ref num, ref flag, ref flag1, ref flag2, ref flag3, ref flag4);
	}

	public static bool ParseSymbol(string text, ref int index, BetterList<Color> colors, bool premultiply, ref int sub, ref bool bold, ref bool italic, ref bool underline, ref bool strike, ref bool ignoreColor)
	{
		string str;
		Dictionary<string, int> strs;
		int num;
		int length = text.Length;
		if (index + 3 > length || text[index] != '[')
		{
			return false;
		}
		if (text[index + 2] == ']')
		{
			if (text[index + 1] == '-')
			{
				if (colors != null && colors.size > 1)
				{
					colors.RemoveAt(colors.size - 1);
				}
				index += 3;
				return true;
			}
			str = text.Substring(index, 3);
			if (str != null)
			{
				if (NGUIText.u003cu003ef__switchu0024map4 == null)
				{
					strs = new Dictionary<string, int>(5)
					{
						{ "[b]", 0 },
						{ "[i]", 1 },
						{ "[u]", 2 },
						{ "[s]", 3 },
						{ "[c]", 4 }
					};
					NGUIText.u003cu003ef__switchu0024map4 = strs;
				}
				if (NGUIText.u003cu003ef__switchu0024map4.TryGetValue(str, out num))
				{
					switch (num)
					{
						case 0:
						{
							bold = true;
							index += 3;
							return true;
						}
						case 1:
						{
							italic = true;
							index += 3;
							return true;
						}
						case 2:
						{
							underline = true;
							index += 3;
							return true;
						}
						case 3:
						{
							strike = true;
							index += 3;
							return true;
						}
						case 4:
						{
							ignoreColor = true;
							index += 3;
							return true;
						}
					}
				}
			}
		}
		if (index + 4 > length)
		{
			return false;
		}
		if (text[index + 3] == ']')
		{
			str = text.Substring(index, 4);
			if (str != null)
			{
				if (NGUIText.u003cu003ef__switchu0024map5 == null)
				{
					strs = new Dictionary<string, int>(5)
					{
						{ "[/b]", 0 },
						{ "[/i]", 1 },
						{ "[/u]", 2 },
						{ "[/s]", 3 },
						{ "[/c]", 4 }
					};
					NGUIText.u003cu003ef__switchu0024map5 = strs;
				}
				if (NGUIText.u003cu003ef__switchu0024map5.TryGetValue(str, out num))
				{
					switch (num)
					{
						case 0:
						{
							bold = false;
							index += 4;
							return true;
						}
						case 1:
						{
							italic = false;
							index += 4;
							return true;
						}
						case 2:
						{
							underline = false;
							index += 4;
							return true;
						}
						case 3:
						{
							strike = false;
							index += 4;
							return true;
						}
						case 4:
						{
							ignoreColor = false;
							index += 4;
							return true;
						}
					}
				}
			}
			char chr = text[index + 1];
			char chr1 = text[index + 2];
			if (NGUIText.IsHex(chr) && NGUIText.IsHex(chr1))
			{
				int num1 = NGUIMath.HexToDecimal(chr) << 4 | NGUIMath.HexToDecimal(chr1);
				NGUIText.mAlpha = (float)num1 / 255f;
				index += 4;
				return true;
			}
		}
		if (index + 5 > length)
		{
			return false;
		}
		if (text[index + 4] == ']')
		{
			str = text.Substring(index, 5);
			if (str != null)
			{
				if (NGUIText.u003cu003ef__switchu0024map6 == null)
				{
					strs = new Dictionary<string, int>(2)
					{
						{ "[sub]", 0 },
						{ "[sup]", 1 }
					};
					NGUIText.u003cu003ef__switchu0024map6 = strs;
				}
				if (NGUIText.u003cu003ef__switchu0024map6.TryGetValue(str, out num))
				{
					if (num == 0)
					{
						sub = 1;
						index += 5;
						return true;
					}
					if (num == 1)
					{
						sub = 2;
						index += 5;
						return true;
					}
				}
			}
		}
		if (index + 6 > length)
		{
			return false;
		}
		if (text[index + 5] == ']')
		{
			str = text.Substring(index, 6);
			if (str != null)
			{
				if (NGUIText.u003cu003ef__switchu0024map7 == null)
				{
					strs = new Dictionary<string, int>(3)
					{
						{ "[/sub]", 0 },
						{ "[/sup]", 1 },
						{ "[/url]", 2 }
					};
					NGUIText.u003cu003ef__switchu0024map7 = strs;
				}
				if (NGUIText.u003cu003ef__switchu0024map7.TryGetValue(str, out num))
				{
					switch (num)
					{
						case 0:
						{
							sub = 0;
							index += 6;
							return true;
						}
						case 1:
						{
							sub = 0;
							index += 6;
							return true;
						}
						case 2:
						{
							index += 6;
							return true;
						}
					}
				}
			}
		}
		if (text[index + 1] == 'u' && text[index + 2] == 'r' && text[index + 3] == 'l' && text[index + 4] == '=')
		{
			int num2 = text.IndexOf(']', index + 4);
			if (num2 == -1)
			{
				index = text.Length;
				return true;
			}
			index = num2 + 1;
			return true;
		}
		if (index + 8 > length)
		{
			return false;
		}
		if (text[index + 7] != ']')
		{
			if (index + 10 > length)
			{
				return false;
			}
			if (text[index + 9] != ']')
			{
				return false;
			}
			Color color = NGUIText.ParseColor32(text, index + 1);
			if (NGUIText.EncodeColor32(color) != text.Substring(index + 1, 8).ToUpper())
			{
				return false;
			}
			if (colors != null)
			{
				if (premultiply && color.a != 1f)
				{
					color = Color.Lerp(NGUIText.mInvisible, color, color.a);
				}
				colors.Add(color);
			}
			index += 10;
			return true;
		}
		Color color1 = NGUIText.ParseColor24(text, index + 1);
		if (NGUIText.EncodeColor24(color1) != text.Substring(index + 1, 6).ToUpper())
		{
			return false;
		}
		if (colors != null)
		{
			Color item = colors[colors.size - 1];
			color1.a = item.a;
			if (premultiply && color1.a != 1f)
			{
				color1 = Color.Lerp(NGUIText.mInvisible, color1, color1.a);
			}
			colors.Add(color1);
		}
		index += 8;
		return true;
	}

	public static void Prepare(string text)
	{
		if (NGUIText.dynamicFont != null)
		{
			NGUIText.dynamicFont.RequestCharactersInTexture(text, NGUIText.finalSize, NGUIText.fontStyle);
		}
	}

	public static void Print(string text, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		float single;
		float single1;
		float single2;
		float single3;
		Color item;
		BMSymbol symbol;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		int num = verts.size;
		NGUIText.Prepare(text);
		NGUIText.mColors.Add(Color.white);
		NGUIText.mAlpha = 1f;
		int num1 = 0;
		int num2 = 0;
		float single4 = 0f;
		float single5 = 0f;
		float single6 = 0f;
		float single7 = (float)NGUIText.finalSize;
		Color color = NGUIText.tint * NGUIText.gradientBottom;
		Color color1 = NGUIText.tint * NGUIText.gradientTop;
		Color32 color32 = NGUIText.tint;
		int length = text.Length;
		Rect rect = new Rect();
		float single8 = 0f;
		float single9 = 0f;
		float single10 = single7 * NGUIText.pixelDensity;
		bool flag = false;
		int num3 = 0;
		bool flag1 = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		float single11 = 0f;
		if (NGUIText.bitmapFont != null)
		{
			rect = NGUIText.bitmapFont.uvRect;
			single8 = rect.width / (float)NGUIText.bitmapFont.texWidth;
			single9 = rect.height / (float)NGUIText.bitmapFont.texHeight;
		}
		for (int i = 0; i < length; i++)
		{
			num1 = text[i];
			single11 = single4;
			if (num1 == 10)
			{
				if (single4 > single6)
				{
					single6 = single4;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, num, single4 - NGUIText.finalSpacingX, 4);
					num = verts.size;
				}
				single4 = 0f;
				single5 += NGUIText.finalLineHeight;
				num2 = 0;
			}
			else if (num1 < 32)
			{
				num2 = num1;
			}
			else if (!NGUIText.encoding || !NGUIText.ParseSymbol(text, ref i, NGUIText.mColors, NGUIText.premultiply, ref num3, ref flag1, ref flag2, ref flag3, ref flag4, ref flag5))
			{
				if (!NGUIText.useSymbols)
				{
					symbol = null;
				}
				else
				{
					symbol = NGUIText.GetSymbol(text, i, length);
				}
				BMSymbol bMSymbol = symbol;
				if (bMSymbol == null)
				{
					NGUIText.GlyphInfo glyph = NGUIText.GetGlyph(num1, num2);
					if (glyph != null)
					{
						num2 = num1;
						if (num3 != 0)
						{
							glyph.v0.x *= 0.75f;
							glyph.v0.y *= 0.75f;
							glyph.v1.x *= 0.75f;
							glyph.v1.y *= 0.75f;
							if (num3 != 1)
							{
								ref Vector2 vector2Pointer = ref glyph.v0;
								vector2Pointer.y = vector2Pointer.y + NGUIText.fontScale * (float)NGUIText.fontSize * 0.05f;
								ref Vector2 vector2Pointer1 = ref glyph.v1;
								vector2Pointer1.y = vector2Pointer1.y + NGUIText.fontScale * (float)NGUIText.fontSize * 0.05f;
							}
							else
							{
								ref Vector2 vector2Pointer2 = ref glyph.v0;
								vector2Pointer2.y = vector2Pointer2.y - NGUIText.fontScale * (float)NGUIText.fontSize * 0.4f;
								ref Vector2 vector2Pointer3 = ref glyph.v1;
								vector2Pointer3.y = vector2Pointer3.y - NGUIText.fontScale * (float)NGUIText.fontSize * 0.4f;
							}
						}
						single = glyph.v0.x + single4;
						single3 = glyph.v0.y - single5;
						single1 = glyph.v1.x + single4;
						single2 = glyph.v1.y - single5;
						float single12 = glyph.advance;
						if (NGUIText.finalSpacingX < 0f)
						{
							single12 += NGUIText.finalSpacingX;
						}
						if (Mathf.RoundToInt(single4 + single12) > NGUIText.regionWidth)
						{
							if (single4 == 0f)
							{
								return;
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
							{
								NGUIText.Align(verts, num, single4 - NGUIText.finalSpacingX, 4);
								num = verts.size;
							}
							single -= single4;
							single1 -= single4;
							single3 -= NGUIText.finalLineHeight;
							single2 -= NGUIText.finalLineHeight;
							single4 = 0f;
							single5 += NGUIText.finalLineHeight;
							single11 = 0f;
						}
						if (NGUIText.IsSpace(num1))
						{
							if (flag3)
							{
								num1 = 95;
							}
							else if (flag4)
							{
								num1 = 45;
							}
						}
						single4 = single4 + (num3 != 0 ? (NGUIText.finalSpacingX + glyph.advance) * 0.75f : NGUIText.finalSpacingX + glyph.advance);
						if (!NGUIText.IsSpace(num1))
						{
							if (uvs != null)
							{
								if (NGUIText.bitmapFont != null)
								{
									glyph.u0.x = rect.xMin + single8 * glyph.u0.x;
									glyph.u2.x = rect.xMin + single8 * glyph.u2.x;
									glyph.u0.y = rect.yMax - single9 * glyph.u0.y;
									glyph.u2.y = rect.yMax - single9 * glyph.u2.y;
									glyph.u1.x = glyph.u0.x;
									glyph.u1.y = glyph.u2.y;
									glyph.u3.x = glyph.u2.x;
									glyph.u3.y = glyph.u0.y;
								}
								int num4 = 0;
								int num5 = (!flag1 ? 1 : 4);
								while (num4 < num5)
								{
									uvs.Add(glyph.u0);
									uvs.Add(glyph.u1);
									uvs.Add(glyph.u2);
									uvs.Add(glyph.u3);
									num4++;
								}
							}
							if (cols != null)
							{
								if (glyph.channel != 0 && glyph.channel != 15)
								{
									Color color2 = color32;
									color2 *= 0.49f;
									switch (glyph.channel)
									{
										case 1:
										{
											color2.b += 0.51f;
											goto case 7;
										}
										case 2:
										{
											color2.g += 0.51f;
											goto case 7;
										}
										case 3:
										case 5:
										case 6:
										case 7:
										{
											Color32 color321 = color2;
											int num6 = 0;
											int num7 = (!flag1 ? 4 : 16);
											while (num6 < num7)
											{
												cols.Add(color321);
												num6++;
											}
											break;
										}
										case 4:
										{
											color2.r += 0.51f;
											goto case 7;
										}
										case 8:
										{
											color2.a += 0.51f;
											goto case 7;
										}
										default:
										{
											goto case 7;
										}
									}
								}
								else if (!NGUIText.gradient)
								{
									int num8 = 0;
									int num9 = (!flag1 ? 4 : 16);
									while (num8 < num9)
									{
										cols.Add(color32);
										num8++;
									}
								}
								else
								{
									float single13 = single10 + glyph.v0.y / NGUIText.fontScale;
									float single14 = single10 + glyph.v1.y / NGUIText.fontScale;
									single13 /= single10;
									single14 /= single10;
									NGUIText.s_c0 = Color.Lerp(color, color1, single13);
									NGUIText.s_c1 = Color.Lerp(color, color1, single14);
									int num10 = 0;
									int num11 = (!flag1 ? 1 : 4);
									while (num10 < num11)
									{
										cols.Add(NGUIText.s_c0);
										cols.Add(NGUIText.s_c1);
										cols.Add(NGUIText.s_c1);
										cols.Add(NGUIText.s_c0);
										num10++;
									}
								}
							}
							if (flag1)
							{
								for (int j = 0; j < 4; j++)
								{
									float single15 = NGUIText.mBoldOffset[j * 2];
									float single16 = NGUIText.mBoldOffset[j * 2 + 1];
									float single17 = (!flag2 ? 0f : (float)NGUIText.fontSize * 0.1f * ((single2 - single3) / (float)NGUIText.fontSize));
									verts.Add(new Vector3(single + single15 - single17, single3 + single16));
									verts.Add(new Vector3(single + single15 + single17, single2 + single16));
									verts.Add(new Vector3(single1 + single15 + single17, single2 + single16));
									verts.Add(new Vector3(single1 + single15 - single17, single3 + single16));
								}
							}
							else if (flag2)
							{
								float single18 = (float)NGUIText.fontSize * 0.1f * ((single2 - single3) / (float)NGUIText.fontSize);
								verts.Add(new Vector3(single - single18, single3));
								verts.Add(new Vector3(single + single18, single2));
								verts.Add(new Vector3(single1 + single18, single2));
								verts.Add(new Vector3(single1 - single18, single3));
							}
							else
							{
								verts.Add(new Vector3(single, single3));
								verts.Add(new Vector3(single, single2));
								verts.Add(new Vector3(single1, single2));
								verts.Add(new Vector3(single1, single3));
							}
							if (flag3 || flag4)
							{
								NGUIText.GlyphInfo glyphInfo = NGUIText.GetGlyph((!flag4 ? 95 : 45), num2);
								if (glyphInfo != null)
								{
									if (uvs != null)
									{
										if (NGUIText.bitmapFont != null)
										{
											glyphInfo.u0.x = rect.xMin + single8 * glyphInfo.u0.x;
											glyphInfo.u2.x = rect.xMin + single8 * glyphInfo.u2.x;
											glyphInfo.u0.y = rect.yMax - single9 * glyphInfo.u0.y;
											glyphInfo.u2.y = rect.yMax - single9 * glyphInfo.u2.y;
										}
										float single19 = (glyphInfo.u0.x + glyphInfo.u2.x) * 0.5f;
										int num12 = 0;
										int num13 = (!flag1 ? 1 : 4);
										while (num12 < num13)
										{
											uvs.Add(new Vector2(single19, glyphInfo.u0.y));
											uvs.Add(new Vector2(single19, glyphInfo.u2.y));
											uvs.Add(new Vector2(single19, glyphInfo.u2.y));
											uvs.Add(new Vector2(single19, glyphInfo.u0.y));
											num12++;
										}
									}
									if (!flag || !flag4)
									{
										single3 = -single5 + glyphInfo.v0.y;
										single2 = -single5 + glyphInfo.v1.y;
									}
									else
									{
										single3 = (-single5 + glyphInfo.v0.y) * 0.75f;
										single2 = (-single5 + glyphInfo.v1.y) * 0.75f;
									}
									if (!flag1)
									{
										verts.Add(new Vector3(single11, single3));
										verts.Add(new Vector3(single11, single2));
										verts.Add(new Vector3(single4, single2));
										verts.Add(new Vector3(single4, single3));
									}
									else
									{
										for (int k = 0; k < 4; k++)
										{
											float single20 = NGUIText.mBoldOffset[k * 2];
											float single21 = NGUIText.mBoldOffset[k * 2 + 1];
											verts.Add(new Vector3(single11 + single20, single3 + single21));
											verts.Add(new Vector3(single11 + single20, single2 + single21));
											verts.Add(new Vector3(single4 + single20, single2 + single21));
											verts.Add(new Vector3(single4 + single20, single3 + single21));
										}
									}
									if (!NGUIText.gradient)
									{
										int num14 = 0;
										int num15 = (!flag1 ? 4 : 16);
										while (num14 < num15)
										{
											cols.Add(color32);
											num14++;
										}
									}
									else
									{
										float single22 = single10 + glyphInfo.v0.y / NGUIText.fontScale;
										float single23 = single10 + glyphInfo.v1.y / NGUIText.fontScale;
										single22 /= single10;
										single23 /= single10;
										NGUIText.s_c0 = Color.Lerp(color, color1, single22);
										NGUIText.s_c1 = Color.Lerp(color, color1, single23);
										int num16 = 0;
										int num17 = (!flag1 ? 1 : 4);
										while (num16 < num17)
										{
											cols.Add(NGUIText.s_c0);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c0);
											num16++;
										}
									}
								}
							}
						}
					}
				}
				else
				{
					single = single4 + (float)bMSymbol.offsetX * NGUIText.fontScale;
					single1 = single + (float)bMSymbol.width * NGUIText.fontScale;
					single2 = -(single5 + (float)bMSymbol.offsetY * NGUIText.fontScale);
					single3 = single2 - (float)bMSymbol.height * NGUIText.fontScale;
					if (Mathf.RoundToInt(single4 + (float)bMSymbol.advance * NGUIText.fontScale) > NGUIText.regionWidth)
					{
						if (single4 == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
						{
							NGUIText.Align(verts, num, single4 - NGUIText.finalSpacingX, 4);
							num = verts.size;
						}
						single -= single4;
						single1 -= single4;
						single3 -= NGUIText.finalLineHeight;
						single2 -= NGUIText.finalLineHeight;
						single4 = 0f;
						single5 += NGUIText.finalLineHeight;
						single11 = 0f;
					}
					verts.Add(new Vector3(single, single3));
					verts.Add(new Vector3(single, single2));
					verts.Add(new Vector3(single1, single2));
					verts.Add(new Vector3(single1, single3));
					single4 = single4 + (NGUIText.finalSpacingX + (float)bMSymbol.advance * NGUIText.fontScale);
					i = i + (bMSymbol.length - 1);
					num2 = 0;
					if (uvs != null)
					{
						Rect rect1 = bMSymbol.uvRect;
						float single24 = rect1.xMin;
						float single25 = rect1.yMin;
						float single26 = rect1.xMax;
						float single27 = rect1.yMax;
						uvs.Add(new Vector2(single24, single25));
						uvs.Add(new Vector2(single24, single27));
						uvs.Add(new Vector2(single26, single27));
						uvs.Add(new Vector2(single26, single25));
					}
					if (cols != null)
					{
						if (NGUIText.symbolStyle != NGUIText.SymbolStyle.Colored)
						{
							Color32 color322 = Color.white;
							color322.a = color32.a;
							for (int l = 0; l < 4; l++)
							{
								cols.Add(color322);
							}
						}
						else
						{
							for (int m = 0; m < 4; m++)
							{
								cols.Add(color32);
							}
						}
					}
				}
			}
			else
			{
				if (!flag5)
				{
					item = NGUIText.tint * NGUIText.mColors[NGUIText.mColors.size - 1];
					item.a *= NGUIText.mAlpha;
				}
				else
				{
					item = NGUIText.mColors[NGUIText.mColors.size - 1];
					item.a = item.a * (NGUIText.mAlpha * NGUIText.tint.a);
				}
				color32 = item;
				int num18 = 0;
				int num19 = NGUIText.mColors.size - 2;
				while (num18 < num19)
				{
					float single28 = item.a;
					Color item1 = NGUIText.mColors[num18];
					item.a = single28 * item1.a;
					num18++;
				}
				if (NGUIText.gradient)
				{
					color = NGUIText.gradientBottom * item;
					color1 = NGUIText.gradientTop * item;
				}
				i--;
			}
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
		{
			NGUIText.Align(verts, num, single4 - NGUIText.finalSpacingX, 4);
			num = verts.size;
		}
		NGUIText.mColors.Clear();
	}

	public static void PrintApproximateCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
	{
		BMSymbol symbol;
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		float single = 0f;
		float single1 = 0f;
		float single2 = 0f;
		float single3 = (float)NGUIText.fontSize * NGUIText.fontScale * 0.5f;
		int length = text.Length;
		int num = verts.size;
		int num1 = 0;
		int num2 = 0;
		for (int i = 0; i < length; i++)
		{
			num1 = text[i];
			verts.Add(new Vector3(single, -single1 - single3));
			indices.Add(i);
			if (num1 == 10)
			{
				if (single > single2)
				{
					single2 = single;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, num, single - NGUIText.finalSpacingX, 1);
					num = verts.size;
				}
				single = 0f;
				single1 += NGUIText.finalLineHeight;
				num2 = 0;
			}
			else if (num1 < 32)
			{
				num2 = 0;
			}
			else if (!NGUIText.encoding || !NGUIText.ParseSymbol(text, ref i))
			{
				if (!NGUIText.useSymbols)
				{
					symbol = null;
				}
				else
				{
					symbol = NGUIText.GetSymbol(text, i, length);
				}
				BMSymbol bMSymbol = symbol;
				if (bMSymbol != null)
				{
					float single4 = (float)bMSymbol.advance * NGUIText.fontScale + NGUIText.finalSpacingX;
					if (Mathf.RoundToInt(single + single4) <= NGUIText.regionWidth)
					{
						single += single4;
					}
					else
					{
						if (single == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
						{
							NGUIText.Align(verts, num, single - NGUIText.finalSpacingX, 1);
							num = verts.size;
						}
						single = single4;
						single1 += NGUIText.finalLineHeight;
					}
					verts.Add(new Vector3(single, -single1 - single3));
					indices.Add(i + 1);
					i = i + (bMSymbol.sequence.Length - 1);
					num2 = 0;
				}
				else
				{
					float glyphWidth = NGUIText.GetGlyphWidth(num1, num2);
					if (glyphWidth != 0f)
					{
						glyphWidth += NGUIText.finalSpacingX;
						if (Mathf.RoundToInt(single + glyphWidth) <= NGUIText.regionWidth)
						{
							single += glyphWidth;
						}
						else
						{
							if (single == 0f)
							{
								return;
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
							{
								NGUIText.Align(verts, num, single - NGUIText.finalSpacingX, 1);
								num = verts.size;
							}
							single = glyphWidth;
							single1 += NGUIText.finalLineHeight;
						}
						verts.Add(new Vector3(single, -single1 - single3));
						indices.Add(i + 1);
						num2 = num1;
					}
				}
			}
			else
			{
				i--;
			}
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
		{
			NGUIText.Align(verts, num, single - NGUIText.finalSpacingX, 1);
		}
	}

	public static void PrintCaretAndSelection(string text, int start, int end, BetterList<Vector3> caret, BetterList<Vector3> highlight)
	{
		BMSymbol symbol;
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		int num = end;
		if (start > end)
		{
			end = start;
			start = num;
		}
		float single = 0f;
		float single1 = 0f;
		float single2 = 0f;
		float single3 = (float)NGUIText.fontSize * NGUIText.fontScale;
		int num1 = (caret == null ? 0 : caret.size);
		int num2 = (highlight == null ? 0 : highlight.size);
		int length = text.Length;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		bool flag = false;
		bool flag1 = false;
		Vector2 vector2 = Vector2.zero;
		Vector2 vector21 = Vector2.zero;
		while (num3 < length)
		{
			if (caret != null && !flag1 && num <= num3)
			{
				flag1 = true;
				caret.Add(new Vector3(single - 1f, -single1 - single3));
				caret.Add(new Vector3(single - 1f, -single1));
				caret.Add(new Vector3(single + 1f, -single1));
				caret.Add(new Vector3(single + 1f, -single1 - single3));
			}
			num4 = text[num3];
			if (num4 == 10)
			{
				if (single > single2)
				{
					single2 = single;
				}
				if (caret != null && flag1)
				{
					if (NGUIText.alignment != NGUIText.Alignment.Left)
					{
						NGUIText.Align(caret, num1, single - NGUIText.finalSpacingX, 4);
					}
					caret = null;
				}
				if (highlight != null)
				{
					if (flag)
					{
						flag = false;
						highlight.Add(vector21);
						highlight.Add(vector2);
					}
					else if (start <= num3 && end > num3)
					{
						highlight.Add(new Vector3(single, -single1 - single3));
						highlight.Add(new Vector3(single, -single1));
						highlight.Add(new Vector3(single + 2f, -single1));
						highlight.Add(new Vector3(single + 2f, -single1 - single3));
					}
					if (NGUIText.alignment != NGUIText.Alignment.Left && num2 < highlight.size)
					{
						NGUIText.Align(highlight, num2, single - NGUIText.finalSpacingX, 4);
						num2 = highlight.size;
					}
				}
				single = 0f;
				single1 += NGUIText.finalLineHeight;
				num5 = 0;
			}
			else if (num4 < 32)
			{
				num5 = 0;
			}
			else if (!NGUIText.encoding || !NGUIText.ParseSymbol(text, ref num3))
			{
				if (!NGUIText.useSymbols)
				{
					symbol = null;
				}
				else
				{
					symbol = NGUIText.GetSymbol(text, num3, length);
				}
				BMSymbol bMSymbol = symbol;
				float single4 = (bMSymbol == null ? NGUIText.GetGlyphWidth(num4, num5) : (float)bMSymbol.advance * NGUIText.fontScale);
				if (single4 != 0f)
				{
					float single5 = single;
					float single6 = single + single4;
					float single7 = -single1 - single3;
					float single8 = -single1;
					if (Mathf.RoundToInt(single6 + NGUIText.finalSpacingX) > NGUIText.regionWidth)
					{
						if (single == 0f)
						{
							return;
						}
						if (single > single2)
						{
							single2 = single;
						}
						if (caret != null && flag1)
						{
							if (NGUIText.alignment != NGUIText.Alignment.Left)
							{
								NGUIText.Align(caret, num1, single - NGUIText.finalSpacingX, 4);
							}
							caret = null;
						}
						if (highlight != null)
						{
							if (flag)
							{
								flag = false;
								highlight.Add(vector21);
								highlight.Add(vector2);
							}
							else if (start <= num3 && end > num3)
							{
								highlight.Add(new Vector3(single, -single1 - single3));
								highlight.Add(new Vector3(single, -single1));
								highlight.Add(new Vector3(single + 2f, -single1));
								highlight.Add(new Vector3(single + 2f, -single1 - single3));
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && num2 < highlight.size)
							{
								NGUIText.Align(highlight, num2, single - NGUIText.finalSpacingX, 4);
								num2 = highlight.size;
							}
						}
						single5 -= single;
						single6 -= single;
						single7 -= NGUIText.finalLineHeight;
						single8 -= NGUIText.finalLineHeight;
						single = 0f;
						single1 += NGUIText.finalLineHeight;
					}
					single = single + (single4 + NGUIText.finalSpacingX);
					if (highlight != null)
					{
						if (start <= num3 && end > num3)
						{
							if (!flag)
							{
								flag = true;
								highlight.Add(new Vector3(single5, single7));
								highlight.Add(new Vector3(single5, single8));
							}
						}
						else if (flag)
						{
							flag = false;
							highlight.Add(vector21);
							highlight.Add(vector2);
						}
					}
					vector2 = new Vector2(single6, single7);
					vector21 = new Vector2(single6, single8);
					num5 = num4;
				}
			}
			else
			{
				num3--;
			}
			num3++;
		}
		if (caret != null)
		{
			if (!flag1)
			{
				caret.Add(new Vector3(single - 1f, -single1 - single3));
				caret.Add(new Vector3(single - 1f, -single1));
				caret.Add(new Vector3(single + 1f, -single1));
				caret.Add(new Vector3(single + 1f, -single1 - single3));
			}
			if (NGUIText.alignment != NGUIText.Alignment.Left)
			{
				NGUIText.Align(caret, num1, single - NGUIText.finalSpacingX, 4);
			}
		}
		if (highlight != null)
		{
			if (flag)
			{
				highlight.Add(vector21);
				highlight.Add(vector2);
			}
			else if (start < num3 && end == num3)
			{
				highlight.Add(new Vector3(single, -single1 - single3));
				highlight.Add(new Vector3(single, -single1));
				highlight.Add(new Vector3(single + 2f, -single1));
				highlight.Add(new Vector3(single + 2f, -single1 - single3));
			}
			if (NGUIText.alignment != NGUIText.Alignment.Left && num2 < highlight.size)
			{
				NGUIText.Align(highlight, num2, single - NGUIText.finalSpacingX, 4);
			}
		}
	}

	public static void PrintExactCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
	{
		int i;
		BMSymbol symbol;
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		float single = (float)NGUIText.fontSize * NGUIText.fontScale;
		float single1 = 0f;
		float single2 = 0f;
		float single3 = 0f;
		int length = text.Length;
		int num = verts.size;
		int num1 = 0;
		int num2 = 0;
		for (i = 0; i < length; i++)
		{
			num1 = text[i];
			if (num1 == 10)
			{
				if (single1 > single3)
				{
					single3 = single1;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, num, single1 - NGUIText.finalSpacingX, 2);
					num = verts.size;
				}
				single1 = 0f;
				single2 += NGUIText.finalLineHeight;
				num2 = 0;
			}
			else if (num1 < 32)
			{
				num2 = 0;
			}
			else if (!NGUIText.encoding || !NGUIText.ParseSymbol(text, ref i))
			{
				if (!NGUIText.useSymbols)
				{
					symbol = null;
				}
				else
				{
					symbol = NGUIText.GetSymbol(text, i, length);
				}
				BMSymbol bMSymbol = symbol;
				if (bMSymbol != null)
				{
					float single4 = (float)bMSymbol.advance * NGUIText.fontScale + NGUIText.finalSpacingX;
					if (Mathf.RoundToInt(single1 + single4) <= NGUIText.regionWidth)
					{
						indices.Add(i);
						verts.Add(new Vector3(single1, -single2 - single));
						verts.Add(new Vector3(single1 + single4, -single2));
						i = i + (bMSymbol.sequence.Length - 1);
						single1 += single4;
						num2 = 0;
					}
					else
					{
						if (single1 == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
						{
							NGUIText.Align(verts, num, single1 - NGUIText.finalSpacingX, 2);
							num = verts.size;
						}
						single1 = 0f;
						single2 += NGUIText.finalLineHeight;
						num2 = 0;
						i--;
					}
				}
				else
				{
					float glyphWidth = NGUIText.GetGlyphWidth(num1, num2);
					if (glyphWidth != 0f)
					{
						float single5 = glyphWidth + NGUIText.finalSpacingX;
						if (Mathf.RoundToInt(single1 + single5) > NGUIText.regionWidth)
						{
							goto Label1;
						}
						indices.Add(i);
						verts.Add(new Vector3(single1, -single2 - single));
						verts.Add(new Vector3(single1 + single5, -single2));
						num2 = num1;
						single1 += single5;
					}
				}
			}
			else
			{
				i--;
			}
		Label0:
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
		{
			NGUIText.Align(verts, num, single1 - NGUIText.finalSpacingX, 2);
		}
		return;
	Label1:
		if (single1 == 0f)
		{
			return;
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && num < verts.size)
		{
			NGUIText.Align(verts, num, single1 - NGUIText.finalSpacingX, 2);
			num = verts.size;
		}
		single1 = 0f;
		single2 += NGUIText.finalLineHeight;
		num2 = 0;
		i--;
		goto Label0;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void ReplaceSpaceWithNewline(ref StringBuilder s)
	{
		int length = s.Length - 1;
		if (length > 0 && NGUIText.IsSpace(s[length]))
		{
			s[length] = '\n';
		}
	}

	public static string StripSymbols(string text)
	{
		if (text != null)
		{
			int num = 0;
			int length = text.Length;
			while (num < length)
			{
				if (text[num] == '[')
				{
					int num1 = 0;
					bool flag = false;
					bool flag1 = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					int num2 = num;
					if (NGUIText.ParseSymbol(text, ref num2, null, false, ref num1, ref flag, ref flag1, ref flag2, ref flag3, ref flag4))
					{
						text = text.Remove(num, num2 - num);
						length = text.Length;
						continue;
					}
				}
				num++;
			}
		}
		return text;
	}

	public static void Update()
	{
		NGUIText.Update(true);
	}

	public static void Update(bool request)
	{
		NGUIText.finalSize = Mathf.RoundToInt((float)NGUIText.fontSize / NGUIText.pixelDensity);
		NGUIText.finalSpacingX = NGUIText.spacingX * NGUIText.fontScale;
		NGUIText.finalLineHeight = ((float)NGUIText.fontSize + NGUIText.spacingY) * NGUIText.fontScale;
		NGUIText.useSymbols = (!(NGUIText.bitmapFont != null) || !NGUIText.bitmapFont.hasSymbols || !NGUIText.encoding ? false : NGUIText.symbolStyle != NGUIText.SymbolStyle.None);
		Font font = NGUIText.dynamicFont;
		if (font != null && request)
		{
			font.RequestCharactersInTexture(")_-", NGUIText.finalSize, NGUIText.fontStyle);
			if (!font.GetCharacterInfo(')', out NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle) || (float)NGUIText.mTempChar.maxY == 0f)
			{
				font.RequestCharactersInTexture("A", NGUIText.finalSize, NGUIText.fontStyle);
				if (!font.GetCharacterInfo('A', out NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
				{
					NGUIText.baseline = 0f;
					return;
				}
			}
			float single = (float)NGUIText.mTempChar.maxY;
			float single1 = (float)NGUIText.mTempChar.minY;
			NGUIText.baseline = Mathf.Round(single + ((float)NGUIText.finalSize - single + single1) * 0.5f);
		}
	}

	public static bool WrapText(string text, out string finalText, bool wrapLineColors = false)
	{
		return NGUIText.WrapText(text, out finalText, false, wrapLineColors, false);
	}

	public static bool WrapText(string text, out string finalText, bool keepCharCount, bool wrapLineColors, bool useEllipsis = false)
	{
		float single;
		BMSymbol symbol;
		bool flag;
		StringBuilder stringBuilder;
		StringBuilder stringBuilder1;
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 1 || NGUIText.finalLineHeight < 1f)
		{
			finalText = string.Empty;
			return false;
		}
		float single1 = (NGUIText.maxLines <= 0 ? (float)NGUIText.regionHeight : Mathf.Min((float)NGUIText.regionHeight, NGUIText.finalLineHeight * (float)NGUIText.maxLines));
		int num = (NGUIText.maxLines <= 0 ? 1000000 : NGUIText.maxLines);
		num = Mathf.FloorToInt(Mathf.Min((float)num, single1 / NGUIText.finalLineHeight) + 0.01f);
		if (num == 0)
		{
			finalText = string.Empty;
			return false;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		StringBuilder stringBuilder2 = new StringBuilder();
		int length = text.Length;
		float glyphWidth = (float)NGUIText.regionWidth;
		int num1 = 0;
		int num2 = 0;
		int num3 = 1;
		int num4 = 0;
		bool flag1 = true;
		bool flag2 = true;
		bool flag3 = false;
		Color item = NGUIText.tint;
		int num5 = 0;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		if (!NGUIText.useSymbols)
		{
			wrapLineColors = false;
		}
		if (wrapLineColors)
		{
			NGUIText.mColors.Add(item);
			stringBuilder2.Append("[");
			stringBuilder2.Append(NGUIText.EncodeColor(item));
			stringBuilder2.Append("]");
		}
		while (num2 < length)
		{
			char chr = text[num2];
			if (chr > '\u2FFF')
			{
				flag3 = true;
			}
			if (chr != '\n')
			{
				if (NGUIText.encoding)
				{
					if (wrapLineColors)
					{
						if (!NGUIText.ParseSymbol(text, ref num2, NGUIText.mColors, NGUIText.premultiply, ref num5, ref flag4, ref flag5, ref flag6, ref flag7, ref flag8))
						{
							goto Label1;
						}
						if (!flag8)
						{
							item = NGUIText.tint * NGUIText.mColors[NGUIText.mColors.size - 1];
							item.a *= NGUIText.mAlpha;
						}
						else
						{
							item = NGUIText.mColors[NGUIText.mColors.size - 1];
							item.a = item.a * (NGUIText.mAlpha * NGUIText.tint.a);
						}
						int num6 = 0;
						int num7 = NGUIText.mColors.size - 2;
						while (num6 < num7)
						{
							float single2 = item.a;
							Color color = NGUIText.mColors[num6];
							item.a = single2 * color.a;
							num6++;
						}
						num2--;
						goto Label0;
					}
					else if (NGUIText.ParseSymbol(text, ref num2))
					{
						goto Label2;
					}
				}
			Label1:
				if (!NGUIText.useSymbols)
				{
					symbol = null;
				}
				else
				{
					symbol = NGUIText.GetSymbol(text, num2, length);
				}
				BMSymbol bMSymbol = symbol;
				if (bMSymbol != null)
				{
					single = NGUIText.finalSpacingX + (float)bMSymbol.advance * NGUIText.fontScale;
				}
				else
				{
					float glyphWidth1 = NGUIText.GetGlyphWidth(chr, num4);
					if (glyphWidth1 == 0f && !NGUIText.IsSpace(chr))
					{
						goto Label0;
					}
					single = NGUIText.finalSpacingX + glyphWidth1;
				}
				glyphWidth -= single;
				if (NGUIText.IsSpace(chr) && !flag3 && num1 < num2)
				{
					int num8 = num2 - num1 + 1;
					if (num3 == num && glyphWidth <= 0f && num2 < length)
					{
						char chr1 = text[num2];
						if (chr1 < ' ' || NGUIText.IsSpace(chr1))
						{
							num8--;
						}
					}
					stringBuilder2.Append(text.Substring(num1, num8));
					flag1 = false;
					num1 = num2 + 1;
					num4 = chr;
				}
				if (Mathf.RoundToInt(glyphWidth) >= 0)
				{
					num4 = chr;
				}
				else
				{
					if (!flag1 && num3 != num)
					{
						goto Label4;
					}
					if (useEllipsis && num3 == num && num2 > 1)
					{
						float glyphWidth2 = NGUIText.GetGlyphWidth(46, 46) * 3f;
						if (glyphWidth2 < (float)NGUIText.regionWidth)
						{
							glyphWidth += single;
							int num9 = num2;
							int num10 = 0;
							while (num9 > 1 && glyphWidth < glyphWidth2)
							{
								num9--;
								char chr2 = text[num9 - 1];
								char chr3 = text[num9];
								bool flag9 = (glyphWidth != 0f ? false : NGUIText.IsSpace(chr3));
								glyphWidth += NGUIText.GetGlyphWidth(chr3, chr2);
								if (num9 >= num1 || flag9)
								{
									continue;
								}
								num10++;
							}
							if (glyphWidth >= glyphWidth2)
							{
								if (num10 > 0)
								{
									stringBuilder2.Length = Mathf.Max(0, stringBuilder2.Length - num10);
								}
								stringBuilder2.Append(text.Substring(num1, Mathf.Max(0, num9 - num1)));
								while (stringBuilder2.Length > 0 && NGUIText.IsSpace(stringBuilder2[stringBuilder2.Length - 1]))
								{
									StringBuilder length1 = stringBuilder2;
									length1.Length = length1.Length - 1;
								}
								stringBuilder2.Append("...");
								num3++;
								int num11 = num9;
								num2 = num11;
								num1 = num11;
								break;
							}
						}
					}
					stringBuilder2.Append(text.Substring(num1, Mathf.Max(0, num2 - num1)));
					bool flag10 = NGUIText.IsSpace(chr);
					if (!flag10 && !flag3)
					{
						flag2 = false;
					}
					if (wrapLineColors && NGUIText.mColors.size > 0)
					{
						stringBuilder2.Append("[-]");
					}
					int num12 = num3;
					num3 = num12 + 1;
					if (num12 != num)
					{
						if (!keepCharCount)
						{
							NGUIText.EndLine(ref stringBuilder2);
						}
						else
						{
							NGUIText.ReplaceSpaceWithNewline(ref stringBuilder2);
						}
						if (wrapLineColors)
						{
							for (int i = 0; i < NGUIText.mColors.size; i++)
							{
								stringBuilder2.Insert(stringBuilder2.Length - 1, "[-]");
							}
							for (int j = 0; j < NGUIText.mColors.size; j++)
							{
								stringBuilder2.Append("[");
								stringBuilder2.Append(NGUIText.EncodeColor(NGUIText.mColors[j]));
								stringBuilder2.Append("]");
							}
						}
						flag1 = true;
						if (!flag10)
						{
							num1 = num2;
							glyphWidth = (float)NGUIText.regionWidth - single;
						}
						else
						{
							num1 = num2 + 1;
							glyphWidth = (float)NGUIText.regionWidth;
						}
						num4 = 0;
					}
					else
					{
						num1 = num2;
						break;
					}
				}
				if (bMSymbol != null)
				{
					num2 = num2 + (bMSymbol.length - 1);
					num4 = 0;
				}
			}
			else if (num3 != num)
			{
				glyphWidth = (float)NGUIText.regionWidth;
				if (num1 >= num2)
				{
					stringBuilder2.Append(chr);
				}
				else
				{
					stringBuilder2.Append(text.Substring(num1, num2 - num1 + 1));
				}
				if (wrapLineColors)
				{
					for (int k = 0; k < NGUIText.mColors.size; k++)
					{
						stringBuilder2.Insert(stringBuilder2.Length - 1, "[-]");
					}
					for (int l = 0; l < NGUIText.mColors.size; l++)
					{
						stringBuilder2.Append("[");
						stringBuilder2.Append(NGUIText.EncodeColor(NGUIText.mColors[l]));
						stringBuilder2.Append("]");
					}
				}
				flag1 = true;
				num3++;
				num1 = num2 + 1;
				num4 = 0;
			}
			else
			{
				break;
			}
		Label0:
			num2++;
		}
		if (num1 < num2)
		{
			stringBuilder = stringBuilder2.Append(text.Substring(num1, num2 - num1));
		}
		if (wrapLineColors && NGUIText.mColors.size > 0)
		{
			stringBuilder1 = stringBuilder2.Append("[-]");
		}
		finalText = stringBuilder2.ToString();
		NGUIText.mColors.Clear();
		if (!flag2)
		{
			flag = false;
		}
		else
		{
			flag = (num2 == length ? true : num3 <= Mathf.Min(NGUIText.maxLines, num));
		}
		return flag;
	Label2:
		num2--;
		goto Label0;
	Label4:
		flag1 = true;
		glyphWidth = (float)NGUIText.regionWidth;
		num2 = num1 - 1;
		num4 = 0;
		int num13 = num3;
		num3 = num13 + 1;
		if (num13 != num)
		{
			if (!keepCharCount)
			{
				NGUIText.EndLine(ref stringBuilder2);
			}
			else
			{
				NGUIText.ReplaceSpaceWithNewline(ref stringBuilder2);
			}
			if (wrapLineColors)
			{
				for (int m = 0; m < NGUIText.mColors.size; m++)
				{
					stringBuilder2.Insert(stringBuilder2.Length - 1, "[-]");
				}
				for (int n = 0; n < NGUIText.mColors.size; n++)
				{
					stringBuilder2.Append("[");
					stringBuilder2.Append(NGUIText.EncodeColor(NGUIText.mColors[n]));
					stringBuilder2.Append("]");
				}
			}
			goto Label0;
		}
		else
		{
			if (num1 < num2)
			{
				stringBuilder = stringBuilder2.Append(text.Substring(num1, num2 - num1));
			}
			if (wrapLineColors && NGUIText.mColors.size > 0)
			{
				stringBuilder1 = stringBuilder2.Append("[-]");
			}
			finalText = stringBuilder2.ToString();
			NGUIText.mColors.Clear();
			if (!flag2)
			{
				flag = false;
			}
			else
			{
				flag = (num2 == length ? true : num3 <= Mathf.Min(NGUIText.maxLines, num));
			}
			return flag;
		}
	}

	public enum Alignment
	{
		Automatic,
		Left,
		Center,
		Right,
		Justified
	}

	public class GlyphInfo
	{
		public Vector2 v0;

		public Vector2 v1;

		public Vector2 u0;

		public Vector2 u1;

		public Vector2 u2;

		public Vector2 u3;

		public float advance;

		public int channel;

		public GlyphInfo()
		{
		}
	}

	public enum SymbolStyle
	{
		None,
		Normal,
		Colored
	}
}