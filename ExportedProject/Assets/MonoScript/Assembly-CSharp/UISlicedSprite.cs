using System;
using UnityEngine;

[ExecuteInEditMode]
public class UISlicedSprite : UISprite
{
	public override UIBasicSprite.Type type
	{
		get
		{
			return UIBasicSprite.Type.Sliced;
		}
	}

	public UISlicedSprite()
	{
	}
}