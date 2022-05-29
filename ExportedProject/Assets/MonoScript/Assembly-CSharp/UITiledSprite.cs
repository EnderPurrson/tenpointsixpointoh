using System;
using UnityEngine;

[ExecuteInEditMode]
public class UITiledSprite : UISlicedSprite
{
	public override UIBasicSprite.Type type
	{
		get
		{
			return UIBasicSprite.Type.Tiled;
		}
	}

	public UITiledSprite()
	{
	}
}