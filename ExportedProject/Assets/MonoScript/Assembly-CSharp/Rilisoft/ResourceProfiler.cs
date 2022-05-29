using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class ResourceProfiler
	{
		private readonly IDictionary<Material, MaterialInfo> _materials = new Dictionary<Material, MaterialInfo>();

		private readonly IDictionary<Mesh, MeshInfo> _meshes = new Dictionary<Mesh, MeshInfo>();

		public ResourceProfiler()
		{
		}

		public void Update()
		{
			Renderer[] rendererArray = UnityEngine.Object.FindObjectsOfType<Renderer>();
			for (int i = 0; i < (int)rendererArray.Length; i++)
			{
				Renderer renderer = rendererArray[i];
				Material[] materialArray = renderer.sharedMaterials;
				for (int j = 0; j < (int)materialArray.Length; j++)
				{
					Material material = materialArray[j];
					MaterialInfo materialInfo = null;
					if (!this._materials.TryGetValue(material, out materialInfo))
					{
						materialInfo = new MaterialInfo();
						this._materials.Add(material, materialInfo);
					}
					materialInfo.AddRenderer(renderer);
				}
			}
			MeshFilter[] meshFilterArray = UnityEngine.Object.FindObjectsOfType<MeshFilter>();
			for (int k = 0; k < (int)meshFilterArray.Length; k++)
			{
				MeshFilter meshFilter = meshFilterArray[k];
				Mesh mesh = meshFilter.sharedMesh;
				if (mesh != null)
				{
					MeshInfo meshInfo = null;
					if (!this._meshes.TryGetValue(mesh, out meshInfo))
					{
						meshInfo = new MeshInfo();
						this._meshes.Add(mesh, meshInfo);
					}
					meshInfo.AddMeshFilter(meshFilter);
				}
			}
		}
	}
}