using System;
using UnityEngine;

[Serializable]
public class CrosshairData
{
	public int ID;

	public string Name;

	public Texture2D PreviewTexture;

	public CrosshairData.aimSprite center = new CrosshairData.aimSprite("aim_1", new Vector2(6f, 6f), new Vector2(0f, 0f));

	public CrosshairData.aimSprite up = new CrosshairData.aimSprite("aim_2", new Vector2(6f, 10f), new Vector2(0f, 5f));

	public CrosshairData.aimSprite leftUp = new CrosshairData.aimSprite(string.Empty, new Vector2(12f, 12f), new Vector2(8f, 8f));

	public CrosshairData.aimSprite left = new CrosshairData.aimSprite("aim_2", new Vector2(10f, 6f), new Vector2(5f, 0f));

	public CrosshairData.aimSprite leftDown = new CrosshairData.aimSprite(string.Empty, new Vector2(0f, 0f), new Vector2(0f, 0f));

	public CrosshairData.aimSprite down = new CrosshairData.aimSprite("aim_3", new Vector2(6f, 10f), new Vector2(0f, 5f));

	public CrosshairData()
	{
	}

	[Serializable]
	public class aimSprite
	{
		public string spriteName;

		public Vector2 spriteSize;

		public Vector2 offset;

		public aimSprite(string name, Vector2 size, Vector2 pos)
		{
			this.spriteName = name;
			this.spriteSize = size;
			this.offset = pos;
		}
	}
}