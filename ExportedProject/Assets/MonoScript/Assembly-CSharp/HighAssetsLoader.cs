using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class HighAssetsLoader : MonoBehaviour
{
	public readonly static string LightmapsFolder;

	public readonly static string HighFolder;

	public readonly static string AtlasFolder;

	static HighAssetsLoader()
	{
		HighAssetsLoader.LightmapsFolder = "Lightmap";
		HighAssetsLoader.HighFolder = "High";
		HighAssetsLoader.AtlasFolder = "Atlas";
	}

	public HighAssetsLoader()
	{
	}

	private void OnLevelWasLoaded(int lev)
	{
		if (Device.isWeakDevice || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 && !Application.isEditor)
		{
			return;
		}
		string str = ResPath.Combine(ResPath.Combine(HighAssetsLoader.LightmapsFolder, HighAssetsLoader.HighFolder), Application.loadedLevelName);
		string str1 = ResPath.Combine(ResPath.Combine(HighAssetsLoader.AtlasFolder, HighAssetsLoader.HighFolder), Application.loadedLevelName);
		Texture2D[] texture2DArray = Resources.LoadAll<Texture2D>(str);
		if (texture2DArray != null && (int)texture2DArray.Length > 0)
		{
			List<Texture2D> texture2Ds = new List<Texture2D>();
			Texture2D[] texture2DArray1 = texture2DArray;
			for (int i = 0; i < (int)texture2DArray1.Length; i++)
			{
				texture2Ds.Add(texture2DArray1[i]);
			}
			texture2Ds.Sort((Texture2D lightmap1, Texture2D lightmap2) => lightmap1.name.CompareTo(lightmap2.name));
			LightmapData lightmapDatum = new LightmapData()
			{
				lightmapFar = texture2Ds[0]
			};
			LightmapSettings.lightmaps = (new List<LightmapData>()
			{
				lightmapDatum
			}).ToArray();
		}
		Texture2D[] texture2DArray2 = Resources.LoadAll<Texture2D>(str1);
		string str2 = string.Concat(Application.loadedLevelName, "_Atlas");
		if (texture2DArray2 != null && (int)texture2DArray2.Length > 0)
		{
			UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(Renderer));
			List<Material> materials = new List<Material>();
			UnityEngine.Object[] objArray1 = objArray;
			for (int j = 0; j < (int)objArray1.Length; j++)
			{
				Renderer renderer = (Renderer)objArray1[j];
				if (renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.name != null && renderer.sharedMaterial.name.Contains(str2) && !materials.Contains(renderer.sharedMaterial))
				{
					materials.Add(renderer.sharedMaterial);
				}
			}
			List<Texture2D> texture2Ds1 = new List<Texture2D>();
			Texture2D[] texture2DArray3 = texture2DArray2;
			for (int k = 0; k < (int)texture2DArray3.Length; k++)
			{
				texture2Ds1.Add(texture2DArray3[k]);
			}
			materials.Sort((Material m1, Material m2) => m1.name.CompareTo(m2.name));
			texture2Ds1.Sort((Texture2D a1, Texture2D a2) => a1.name.CompareTo(a2.name));
			for (int l = 0; l < Mathf.Min(materials.Count, texture2Ds1.Count); l++)
			{
				materials[l].mainTexture = texture2Ds1[l];
			}
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}