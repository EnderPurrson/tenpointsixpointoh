using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

internal sealed class HighAssetsLoader : MonoBehaviour
{
	public static readonly string LightmapsFolder = "Lightmap";

	public static readonly string HighFolder = "High";

	public static readonly string AtlasFolder = "Atlas";

	[CompilerGenerated]
	private static Comparison<Texture2D> _003C_003Ef__am_0024cache3;

	[CompilerGenerated]
	private static Comparison<Material> _003C_003Ef__am_0024cache4;

	[CompilerGenerated]
	private static Comparison<Texture2D> _003C_003Ef__am_0024cache5;

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnLevelWasLoaded(int lev)
	{
		if (Device.isWeakDevice || (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 && !Application.isEditor))
		{
			return;
		}
		string path = ResPath.Combine(ResPath.Combine(LightmapsFolder, HighFolder), Application.loadedLevelName);
		string path2 = ResPath.Combine(ResPath.Combine(AtlasFolder, HighFolder), Application.loadedLevelName);
		Texture2D[] array = Resources.LoadAll<Texture2D>(path);
		if (array != null && array.Length > 0)
		{
			List<Texture2D> list = new List<Texture2D>();
			Texture2D[] array2 = array;
			foreach (Texture2D item in array2)
			{
				list.Add(item);
			}
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003COnLevelWasLoaded_003Em__179;
			}
			list.Sort(_003C_003Ef__am_0024cache3);
			LightmapData lightmapData = new LightmapData();
			lightmapData.lightmapFar = list[0];
			List<LightmapData> list2 = new List<LightmapData>();
			list2.Add(lightmapData);
			LightmapSettings.lightmaps = list2.ToArray();
		}
		Texture2D[] array3 = Resources.LoadAll<Texture2D>(path2);
		string value = Application.loadedLevelName + "_Atlas";
		if (array3 == null || array3.Length <= 0)
		{
			return;
		}
		UnityEngine.Object[] array4 = UnityEngine.Object.FindObjectsOfType(typeof(Renderer));
		List<Material> list3 = new List<Material>();
		UnityEngine.Object[] array5 = array4;
		for (int j = 0; j < array5.Length; j++)
		{
			Renderer renderer = (Renderer)array5[j];
			if (renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.name != null && renderer.sharedMaterial.name.Contains(value) && !list3.Contains(renderer.sharedMaterial))
			{
				list3.Add(renderer.sharedMaterial);
			}
		}
		List<Texture2D> list4 = new List<Texture2D>();
		Texture2D[] array6 = array3;
		foreach (Texture2D item2 in array6)
		{
			list4.Add(item2);
		}
		if (_003C_003Ef__am_0024cache4 == null)
		{
			_003C_003Ef__am_0024cache4 = _003COnLevelWasLoaded_003Em__17A;
		}
		list3.Sort(_003C_003Ef__am_0024cache4);
		if (_003C_003Ef__am_0024cache5 == null)
		{
			_003C_003Ef__am_0024cache5 = _003COnLevelWasLoaded_003Em__17B;
		}
		list4.Sort(_003C_003Ef__am_0024cache5);
		for (int l = 0; l < Mathf.Min(list3.Count, list4.Count); l++)
		{
			list3[l].mainTexture = list4[l];
		}
	}

	[CompilerGenerated]
	private static int _003COnLevelWasLoaded_003Em__179(Texture2D lightmap1, Texture2D lightmap2)
	{
		return lightmap1.name.CompareTo(lightmap2.name);
	}

	[CompilerGenerated]
	private static int _003COnLevelWasLoaded_003Em__17A(Material m1, Material m2)
	{
		return m1.name.CompareTo(m2.name);
	}

	[CompilerGenerated]
	private static int _003COnLevelWasLoaded_003Em__17B(Texture2D a1, Texture2D a2)
	{
		return a1.name.CompareTo(a2.name);
	}
}
