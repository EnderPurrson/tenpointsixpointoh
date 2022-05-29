using System;
using System.Collections.Generic;

[Serializable]
public class BMGlyph
{
	public int index;

	public int x;

	public int y;

	public int width;

	public int height;

	public int offsetX;

	public int offsetY;

	public int advance;

	public int channel;

	public List<int> kerning;

	public BMGlyph()
	{
	}

	public int GetKerning(int previousChar)
	{
		if (this.kerning != null && previousChar != 0)
		{
			int num = 0;
			int count = this.kerning.Count;
			while (num < count)
			{
				if (this.kerning[num] == previousChar)
				{
					return this.kerning[num + 1];
				}
				num += 2;
			}
		}
		return 0;
	}

	public void SetKerning(int previousChar, int amount)
	{
		if (this.kerning == null)
		{
			this.kerning = new List<int>();
		}
		for (int i = 0; i < this.kerning.Count; i += 2)
		{
			if (this.kerning[i] == previousChar)
			{
				this.kerning[i + 1] = amount;
				return;
			}
		}
		this.kerning.Add(previousChar);
		this.kerning.Add(amount);
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = this.x + this.width;
		int num1 = this.y + this.height;
		if (this.x < xMin)
		{
			int num2 = xMin - this.x;
			this.x += num2;
			this.width -= num2;
			this.offsetX += num2;
		}
		if (this.y < yMin)
		{
			int num3 = yMin - this.y;
			this.y += num3;
			this.height -= num3;
			this.offsetY += num3;
		}
		if (num > xMax)
		{
			BMGlyph bMGlyph = this;
			bMGlyph.width = bMGlyph.width - (num - xMax);
		}
		if (num1 > yMax)
		{
			BMGlyph bMGlyph1 = this;
			bMGlyph1.height = bMGlyph1.height - (num1 - yMax);
		}
	}
}