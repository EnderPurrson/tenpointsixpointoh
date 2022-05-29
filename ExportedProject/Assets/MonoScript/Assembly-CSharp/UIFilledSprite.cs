using System;
using UnityEngine;

[ExecuteInEditMode]
public class UIFilledSprite : UISprite
{
	public override UIBasicSprite.Type type
	{
		get
		{
			return UIBasicSprite.Type.Filled;
		}
	}

	public UIFilledSprite()
	{
	}
}