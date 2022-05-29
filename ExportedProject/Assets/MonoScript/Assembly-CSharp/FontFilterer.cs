using System;
using UnityEngine;

[ExecuteInEditMode]
public class FontFilterer : MonoBehaviour
{
	public FontFilterer()
	{
	}

	private void Start()
	{
		base.GetComponent<TextMesh>().font.material.mainTexture.filterMode = FilterMode.Point;
	}
}