using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class Decals : GenericDecals<Decals, DecalProjectorBase, DecalsMesh>
	{
		private List<DecalsMeshRenderer> m_DecalsMeshRenderers = new List<DecalsMeshRenderer>();

		public override bool CastShadows
		{
			get
			{
				return DecalsMeshRenderers[0].MeshRenderer.castShadows;
			}
			set
			{
				DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;
				foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers)
				{
					decalsMeshRenderer.MeshRenderer.castShadows = value;
				}
			}
		}

		public override bool ReceiveShadows
		{
			get
			{
				return DecalsMeshRenderers[0].MeshRenderer.receiveShadows;
			}
			set
			{
				DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;
				foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers)
				{
					decalsMeshRenderer.MeshRenderer.receiveShadows = value;
				}
			}
		}

		public override bool UseLightProbes
		{
			get
			{
				return DecalsMeshRenderers[0].MeshRenderer.useLightProbes;
			}
			set
			{
				DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;
				foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers)
				{
					decalsMeshRenderer.MeshRenderer.useLightProbes = value;
				}
			}
		}

		public override Transform LightProbeAnchor
		{
			get
			{
				return DecalsMeshRenderers[0].MeshRenderer.probeAnchor;
			}
			set
			{
				DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;
				foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers)
				{
					decalsMeshRenderer.MeshRenderer.probeAnchor = value;
				}
			}
		}

		public DecalsMeshRenderer[] DecalsMeshRenderers
		{
			get
			{
				return m_DecalsMeshRenderers.ToArray();
			}
		}

		public bool IsDecalsMeshRenderer(MeshRenderer a_MeshRenderer)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			bool result = false;
			Enumerator<DecalsMeshRenderer> enumerator = m_DecalsMeshRenderers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DecalsMeshRenderer current = enumerator.get_Current();
					if (a_MeshRenderer == current.MeshRenderer)
					{
						return true;
					}
				}
				return result;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		protected override void ApplyMaterialToMeshRenderers()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<DecalsMeshRenderer> enumerator = m_DecalsMeshRenderers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DecalsMeshRenderer current = enumerator.get_Current();
					current.MeshRenderer.material = base.CurrentMaterial;
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public override void OnEnable()
		{
			InitializeDecalsMeshRenderers();
			if (m_DecalsMeshRenderers.get_Count() == 0)
			{
				PushDecalsMeshRenderer();
			}
		}

		public override void InitializeDecalsMeshRenderers()
		{
			m_DecalsMeshRenderers.Clear();
			Transform cachedTransform = base.CachedTransform;
			for (int i = 0; i < cachedTransform.childCount; i++)
			{
				Transform child = cachedTransform.GetChild(i);
				DecalsMeshRenderer component = child.GetComponent<DecalsMeshRenderer>();
				if (component != null)
				{
					m_DecalsMeshRenderers.Add(component);
				}
			}
		}

		public void UpdateDecalsMeshes(DecalsMesh a_DecalsMesh)
		{
			if (a_DecalsMesh.Vertices.get_Count() <= 65535)
			{
				if (m_DecalsMeshRenderers.get_Count() == 0)
				{
					PushDecalsMeshRenderer();
				}
				else if (m_DecalsMeshRenderers.get_Count() > 1)
				{
					while (m_DecalsMeshRenderers.get_Count() > 1)
					{
						PopDecalsMeshRenderer();
					}
				}
				DecalsMeshRenderer a_DecalsMeshRenderer = m_DecalsMeshRenderers.get_Item(0);
				ApplyToDecalsMeshRenderer(a_DecalsMeshRenderer, a_DecalsMesh);
			}
			else
			{
				int num = 0;
				for (int i = 0; i < a_DecalsMesh.Projectors.get_Count(); i++)
				{
					GenericDecalProjectorBase a_FirstProjector = a_DecalsMesh.Projectors.get_Item(i);
					GenericDecalProjectorBase a_LastProjector = a_DecalsMesh.Projectors.get_Item(i);
					if (num >= m_DecalsMeshRenderers.get_Count())
					{
						PushDecalsMeshRenderer();
					}
					DecalsMeshRenderer a_DecalsMeshRenderer2 = m_DecalsMeshRenderers.get_Item(num);
					int num2 = 0;
					int num3 = i;
					GenericDecalProjectorBase genericDecalProjectorBase = a_DecalsMesh.Projectors.get_Item(i);
					while (i < a_DecalsMesh.Projectors.get_Count() && num2 + genericDecalProjectorBase.DecalsMeshVertexCount <= 65535)
					{
						a_LastProjector = genericDecalProjectorBase;
						num2 += genericDecalProjectorBase.DecalsMeshVertexCount;
						i++;
						if (i < a_DecalsMesh.Projectors.get_Count())
						{
							genericDecalProjectorBase = a_DecalsMesh.Projectors.get_Item(i);
						}
					}
					if (num3 != i)
					{
						ApplyToDecalsMeshRenderer(a_DecalsMeshRenderer2, a_DecalsMesh, a_FirstProjector, a_LastProjector);
						num++;
					}
				}
				while (num + 1 < m_DecalsMeshRenderers.get_Count())
				{
					PopDecalsMeshRenderer();
				}
			}
			SetDecalsMeshesAreNotOptimized();
		}

		private void PushDecalsMeshRenderer()
		{
			GameObject gameObject = new GameObject("Decals Mesh Renderer");
			Transform transform = gameObject.transform;
			transform.parent = base.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			DecalsMeshRenderer decalsMeshRenderer = AddDecalsMeshRendererComponentToGameObject(gameObject);
			decalsMeshRenderer.MeshRenderer.material = base.CurrentMaterial;
			m_DecalsMeshRenderers.Add(decalsMeshRenderer);
		}

		private void PopDecalsMeshRenderer()
		{
			DecalsMeshRenderer decalsMeshRenderer = m_DecalsMeshRenderers.get_Item(m_DecalsMeshRenderers.get_Count() - 1);
			if (Application.isPlaying)
			{
				Object.Destroy(decalsMeshRenderer.MeshFilter.mesh);
				Object.Destroy(decalsMeshRenderer.gameObject);
			}
			m_DecalsMeshRenderers.RemoveAt(m_DecalsMeshRenderers.get_Count() - 1);
		}

		private void ApplyToDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer, DecalsMesh a_DecalsMesh)
		{
			Mesh mesh = MeshOfDecalsMeshRenderer(a_DecalsMeshRenderer);
			mesh.Clear();
			mesh.MarkDynamic();
			mesh.vertices = a_DecalsMesh.Vertices.ToArray();
			if (base.CurrentNormalsMode != 0)
			{
				mesh.normals = a_DecalsMesh.Normals.ToArray();
			}
			if (base.CurrentTangentsMode != 0)
			{
				mesh.tangents = a_DecalsMesh.Tangents.ToArray();
			}
			mesh.uv = a_DecalsMesh.UVs.ToArray();
			if (base.CurrentUV2Mode != 0)
			{
				mesh.uv2 = a_DecalsMesh.UV2s.ToArray();
			}
			mesh.triangles = a_DecalsMesh.Triangles.ToArray();
		}

		private void ApplyToDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer, DecalsMesh a_DecalsMesh, GenericDecalProjectorBase a_FirstProjector, GenericDecalProjectorBase a_LastProjector)
		{
			int decalsMeshLowerVertexIndex = a_FirstProjector.DecalsMeshLowerVertexIndex;
			int decalsMeshUpperVertexIndex = a_LastProjector.DecalsMeshUpperVertexIndex;
			int decalsMeshLowerTriangleIndex = a_FirstProjector.DecalsMeshLowerTriangleIndex;
			int decalsMeshUpperTriangleIndex = a_LastProjector.DecalsMeshUpperTriangleIndex;
			Mesh mesh = MeshOfDecalsMeshRenderer(a_DecalsMeshRenderer);
			mesh.Clear();
			mesh.MarkDynamic();
			Vector3[] a_Array = new Vector3[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
			CopyListRangeToArray(ref a_Array, a_DecalsMesh.Vertices, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			mesh.vertices = a_Array;
			int[] a_Array2 = new int[decalsMeshUpperTriangleIndex - decalsMeshLowerTriangleIndex + 1];
			CopyListRangeToArray(ref a_Array2, a_DecalsMesh.Triangles, decalsMeshLowerTriangleIndex, decalsMeshUpperTriangleIndex);
			for (int i = 0; i < a_Array2.Length; i++)
			{
				a_Array2[i] -= decalsMeshLowerVertexIndex;
			}
			mesh.triangles = a_Array2;
			Vector2[] a_Array3 = new Vector2[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
			CopyListRangeToArray(ref a_Array3, a_DecalsMesh.UVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
			mesh.uv = a_Array3;
			if (base.CurrentUV2Mode != 0 && base.CurrentUV2Mode != UV2Mode.Lightmapping)
			{
				Vector2[] a_Array4 = new Vector2[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array4, a_DecalsMesh.UV2s, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.uv2 = a_Array4;
			}
			if (base.CurrentNormalsMode != 0)
			{
				Vector3[] a_Array5 = new Vector3[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array5, a_DecalsMesh.Normals, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.normals = a_Array5;
			}
			if (base.CurrentTangentsMode != 0)
			{
				Vector4[] a_Array6 = new Vector4[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
				CopyListRangeToArray(ref a_Array6, a_DecalsMesh.Tangents, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
				mesh.tangents = a_Array6;
			}
		}

		private static void CopyListRangeToArray<T>(ref T[] a_Array, List<T> a_List, int a_LowerListIndex, int a_UpperListIndex)
		{
			int num = 0;
			for (int i = a_LowerListIndex; i <= a_UpperListIndex; i++)
			{
				a_Array[num] = a_List.get_Item(i);
				num++;
			}
		}

		private Mesh MeshOfDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer)
		{
			Mesh mesh;
			if (Application.isPlaying)
			{
				if (a_DecalsMeshRenderer.MeshFilter.mesh == null)
				{
					mesh = new Mesh();
					mesh.name = "Decal Mesh";
					a_DecalsMeshRenderer.MeshFilter.mesh = mesh;
				}
				else
				{
					mesh = a_DecalsMeshRenderer.MeshFilter.mesh;
					mesh.Clear();
				}
			}
			else if (a_DecalsMeshRenderer.MeshFilter.sharedMesh == null)
			{
				mesh = new Mesh();
				mesh.name = "Decal Mesh";
				a_DecalsMeshRenderer.MeshFilter.sharedMesh = mesh;
			}
			else
			{
				mesh = a_DecalsMeshRenderer.MeshFilter.sharedMesh;
				mesh.Clear();
			}
			return mesh;
		}

		public override void OptimizeDecalsMeshes()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			base.OptimizeDecalsMeshes();
			Enumerator<DecalsMeshRenderer> enumerator = m_DecalsMeshRenderers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DecalsMeshRenderer current = enumerator.get_Current();
					if (Application.isPlaying)
					{
						if (current.MeshFilter != null && current.MeshFilter.mesh != null)
						{
							current.MeshFilter.mesh.Optimize();
						}
					}
					else if (current.MeshFilter != null && current.MeshFilter.sharedMesh != null)
					{
						current.MeshFilter.sharedMesh.Optimize();
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		protected abstract DecalsMeshRenderer AddDecalsMeshRendererComponentToGameObject(GameObject a_GameObject);
	}
}
