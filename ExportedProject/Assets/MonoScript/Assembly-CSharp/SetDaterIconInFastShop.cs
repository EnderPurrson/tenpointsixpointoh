using System;
using UnityEngine;

public class SetDaterIconInFastShop : MonoBehaviour
{
	public string daterIconName;

	public SetDaterIconInFastShop()
	{
	}

	private void Awake()
	{
		if (Defs.isDaterRegim)
		{
			base.GetComponent<UISprite>().spriteName = this.daterIconName;
		}
	}
}