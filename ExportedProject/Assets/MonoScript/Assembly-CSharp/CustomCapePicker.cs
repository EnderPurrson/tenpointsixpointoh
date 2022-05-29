using System;
using UnityEngine;

public sealed class CustomCapePicker : MonoBehaviour
{
	public bool shouldLoadTexture = true;

	public CustomCapePicker()
	{
	}

	private void Start()
	{
		if (this.shouldLoadTexture)
		{
			Texture texture = SkinsController.capeUserTexture;
			texture.filterMode = FilterMode.Point;
			Player_move_c.SetTextureRecursivelyFrom(base.gameObject, texture, new GameObject[0]);
		}
	}
}