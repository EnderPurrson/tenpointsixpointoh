using System;
using UnityEngine;

public class ElixirSprite : MonoBehaviour
{
	private UISprite spr;

	public ElixirSprite()
	{
	}

	private void Start()
	{
		bool flag = !Defs.isMulti;
		base.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		this.spr = base.GetComponent<UISprite>();
		this.spr.enabled = false;
	}

	private void Update()
	{
	}
}