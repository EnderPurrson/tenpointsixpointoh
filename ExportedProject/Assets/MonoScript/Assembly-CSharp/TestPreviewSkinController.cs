using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class TestPreviewSkinController : MonoBehaviour
{
	public UIGrid grid;

	public GameObject previewPrefab;

	public TestPreviewSkinController()
	{
	}

	private Color[] GetPixelsByRect(Texture2D texture, Rect rect)
	{
		return texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
	}

	private void Skins()
	{
		for (int i = 0; i < (int)SkinsController.baseSkinsForPersInString.Length; i++)
		{
			Texture2D texture2D = SkinsController.TextureFromString(SkinsController.baseSkinsForPersInString[i], 64, 32);
			Texture2D texture2D1 = new Texture2D(16, 32, TextureFormat.ARGB32, false);
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 32; k++)
				{
					texture2D1.SetPixel(j, k, Color.clear);
				}
			}
			texture2D1.SetPixels(4, 24, 8, 8, this.GetPixelsByRect(texture2D, new Rect(8f, 16f, 8f, 8f)));
			texture2D1.SetPixels(4, 12, 8, 12, this.GetPixelsByRect(texture2D, new Rect(20f, 0f, 8f, 12f)));
			texture2D1.SetPixels(0, 12, 4, 12, this.GetPixelsByRect(texture2D, new Rect(44f, 0f, 4f, 12f)));
			texture2D1.SetPixels(12, 12, 4, 12, this.GetPixelsByRect(texture2D, new Rect(44f, 0f, 4f, 12f)));
			texture2D1.SetPixels(4, 0, 4, 12, this.GetPixelsByRect(texture2D, new Rect(4f, 0f, 4f, 12f)));
			texture2D1.SetPixels(8, 0, 4, 12, this.GetPixelsByRect(texture2D, new Rect(4f, 0f, 4f, 12f)));
			texture2D1.anisoLevel = 1;
			texture2D1.mipMapBias = -0.5f;
			texture2D1.Apply();
			texture2D1.filterMode = FilterMode.Point;
			texture2D1.Apply();
			GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(this.previewPrefab);
			vector3.transform.parent = this.grid.transform;
			vector3.transform.localPosition = new Vector3((float)(160 * i), 0f, 0f);
			vector3.transform.localScale = new Vector3(1f, 1f, 1f);
			vector3.GetComponent<SetTestSkinPreview>().texture.mainTexture = texture2D1;
			vector3.GetComponent<SetTestSkinPreview>().nameLabel.text = i.ToString();
			vector3.GetComponent<SetTestSkinPreview>().keyLabel.text = (!SkinsController.shopKeyFromNameSkin.ContainsKey(i.ToString()) ? string.Empty : SkinsController.shopKeyFromNameSkin[i.ToString()]);
			vector3.name = i.ToString();
		}
		string empty = string.Empty;
		string[] strArrays = new string[] { "player3", "player4" };
		for (int l = 1; l <= 4; l++)
		{
			empty = string.Concat(empty, "\"", SkinsController.StringFromTexture(Resources.Load(string.Concat("MultSkins_6_3/10.6.0_Skin", l)) as Texture2D), "\",\n");
		}
		UnityEngine.Debug.Log(empty);
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		TestPreviewSkinController.u003cStartu003ec__Iterator1D5 variable = null;
		return variable;
	}
}