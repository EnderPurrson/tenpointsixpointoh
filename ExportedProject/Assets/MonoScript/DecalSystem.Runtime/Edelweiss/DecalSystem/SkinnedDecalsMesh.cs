using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public class SkinnedDecalsMesh : GenericDecalsMesh<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh>
	{
		private AddSkinnedMeshDelegate m_AddSkinnedMeshDelegate;

		private List<Vector3> m_OriginalVertices = new List<Vector3>();

		private List<BoneWeight> m_BoneWeights = new List<BoneWeight>();

		private List<Transform> l_Bones = new List<Transform>();

		private List<Matrix4x4> m_BindPoses = new List<Matrix4x4>();

		public List<Vector3> OriginalVertices
		{
			get
			{
				return m_OriginalVertices;
			}
		}

		public List<BoneWeight> BoneWeights
		{
			get
			{
				return m_BoneWeights;
			}
		}

		public List<Transform> Bones
		{
			get
			{
				return l_Bones;
			}
		}

		public List<Matrix4x4> BindPoses
		{
			get
			{
				return m_BindPoses;
			}
		}

		public SkinnedDecalsMesh()
		{
		}

		public SkinnedDecalsMesh(SkinnedDecals a_Decals)
		{
			m_Decals = a_Decals;
		}

		internal override void InitializeDelegates()
		{
			bool flag = m_Decals.CurrentTangentsMode == TangentsMode.Target;
			bool flag2 = m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2;
			bool flag3 = m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2;
			if (!flag && !flag2 && !flag3)
			{
				m_RemoveRangeDelegate = RemoveUnoptimized;
				m_AddSkinnedMeshDelegate = AddUnoptimized;
			}
			else if (!flag && !flag2 && flag3)
			{
				m_RemoveRangeDelegate = RemoveUnoptimized;
				m_AddSkinnedMeshDelegate = AddUnoptimized;
			}
			else if (!flag && flag2 && !flag3)
			{
				m_RemoveRangeDelegate = RemoveUnoptimized;
				m_AddSkinnedMeshDelegate = AddUnoptimized;
			}
			else if (!flag && flag2 && flag3)
			{
				m_RemoveRangeDelegate = RemoveUnoptimized;
				m_AddSkinnedMeshDelegate = AddUnoptimized;
			}
			else if (flag && !flag2 && !flag3)
			{
				m_RemoveRangeDelegate = RemoveUnoptimized;
				m_AddSkinnedMeshDelegate = AddUnoptimized;
			}
			else if (flag && !flag2 && flag3)
			{
				m_RemoveRangeDelegate = RemoveUnoptimized;
				m_AddSkinnedMeshDelegate = AddUnoptimized;
			}
			else if (flag && flag2 && !flag3)
			{
				m_RemoveRangeDelegate = RemoveUnoptimized;
				m_AddSkinnedMeshDelegate = AddUnoptimized;
			}
			else if (flag && flag2 && flag3)
			{
				m_RemoveRangeDelegate = RemoveUnoptimized;
				m_AddSkinnedMeshDelegate = AddUnoptimized;
			}
		}

		internal override void SetRangesForAddedProjector(SkinnedDecalProjectorBase a_Projector)
		{
			base.SetRangesForAddedProjector(a_Projector);
			a_Projector.DecalsMeshLowerBonesIndex = l_Bones.get_Count();
			a_Projector.DecalsMeshUpperBonesIndex = l_Bones.get_Count() - 1;
		}

		public void Add(Mesh a_Mesh, Transform[] a_Bones, SkinQuality a_SkinQuality, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			SkinnedDecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
			if (activeDecalProjector == null)
			{
				throw new NullReferenceException("The active decal projector is not allowed to be null as mesh data should be added!");
			}
			if (m_Decals.CurrentUVMode == UVMode.Project && (0 > activeDecalProjector.UV1RectangleIndex || activeDecalProjector.UV1RectangleIndex >= m_Decals.uvRectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv rectangle index of the active projector is not a valid index within the decals uv rectangles array!");
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.Project && (0 > activeDecalProjector.UV2RectangleIndex || activeDecalProjector.UV2RectangleIndex >= m_Decals.uv2Rectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
			}
			if (a_Mesh == null)
			{
				throw new ArgumentNullException("The mesh parameter is not allowed to be null!");
			}
			Vector3[] vertices = a_Mesh.vertices;
			Vector3[] normals = a_Mesh.normals;
			Vector4[] array = null;
			Vector2[] array2 = null;
			Vector2[] array3 = null;
			BoneWeight[] boneWeights = a_Mesh.boneWeights;
			int[] triangles = a_Mesh.triangles;
			Matrix4x4[] bindposes = a_Mesh.bindposes;
			if (triangles == null)
			{
				throw new NullReferenceException("The triangles in the mesh are null!");
			}
			if (a_Bones == null)
			{
				throw new NullReferenceException("The bones are null!");
			}
			if (bindposes == null)
			{
				throw new NullReferenceException("The bind poses in the mesh are null!");
			}
			if (a_Bones.Length != bindposes.Length)
			{
				throw new FormatException("The number of bind poses in the mesh does not match the number of bones!");
			}
			if (vertices == null)
			{
				return;
			}
			bool flag = false;
			if (normals == null || normals.Length == 0)
			{
				flag = true;
				a_Mesh.RecalculateNormals();
				normals = a_Mesh.normals;
			}
			else if (vertices.Length != normals.Length)
			{
				throw new FormatException("The number of vertices in the mesh does not match the number of normals!");
			}
			if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
			{
				array = a_Mesh.tangents;
				if (array == null)
				{
					throw new NullReferenceException("The tangents in the mesh are null!");
				}
				if (vertices.Length != array.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of tangents!");
				}
			}
			if (m_Decals.CurrentUVMode == UVMode.TargetUV)
			{
				array2 = a_Mesh.uv;
				if (array2 == null)
				{
					throw new NullReferenceException("The uv's in the mesh are null!");
				}
				if (vertices.Length != array2.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of uv's!");
				}
			}
			else if (m_Decals.CurrentUVMode == UVMode.TargetUV2)
			{
				array2 = a_Mesh.uv2;
				if (array2 == null)
				{
					throw new NullReferenceException("The uv2's in the mesh are null!");
				}
				if (vertices.Length != array2.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of uv2's!");
				}
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV)
			{
				array3 = a_Mesh.uv;
				if (array3 == null)
				{
					throw new NullReferenceException("The uv's in the mesh are null!");
				}
				if (vertices.Length != array3.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of uv's!");
				}
			}
			else if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
			{
				array3 = a_Mesh.uv2;
				if (array3 == null)
				{
					throw new NullReferenceException("The uv2's in the mesh are null!");
				}
				if (vertices.Length != array3.Length)
				{
					throw new FormatException("The number of vertices in the mesh does not match the number of uv2's!");
				}
			}
			Add(vertices, normals, array, array2, array3, boneWeights, triangles, a_Bones, bindposes, a_SkinQuality, a_WorldToMeshMatrix, a_MeshToWorldMatrix);
			if (flag)
			{
				a_Mesh.normals = null;
			}
		}

		public void Add(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, Transform[] a_Bones, Matrix4x4[] a_BindPoses, SkinQuality a_SkinQuality, Matrix4x4 a_WorldToMeshMatrix, Matrix4x4 a_MeshToWorldMatrix)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			SkinnedDecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
			if (activeDecalProjector == null)
			{
				throw new NullReferenceException("The active decal projector is not allowed to be null as mesh data should be added!");
			}
			if (m_Decals.CurrentUVMode == UVMode.Project && (0 > activeDecalProjector.UV1RectangleIndex || activeDecalProjector.UV1RectangleIndex >= m_Decals.uvRectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv rectangle index of the active projector is not a valid index within the decals uv rectangles array!");
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.Project && (0 > activeDecalProjector.UV2RectangleIndex || activeDecalProjector.UV2RectangleIndex >= m_Decals.uv2Rectangles.Length))
			{
				throw new IndexOutOfRangeException("The uv2 rectangle index of the active projector is not a valid index within the decals uv2 rectangles array!");
			}
			if (a_Vertices == null)
			{
				throw new ArgumentNullException("The vertices parameter is not allowed to be null!");
			}
			if (a_Normals == null)
			{
				throw new ArgumentNullException("The normals parameter is not allowed to be null!");
			}
			if (a_Triangles == null)
			{
				throw new ArgumentNullException("The triangles parameter is not allowed to be null!");
			}
			if (a_Vertices.Length != a_Normals.Length)
			{
				throw new FormatException("The number of vertices does not match the number of normals!");
			}
			if (a_Bones == null)
			{
				throw new NullReferenceException("The bones are null!");
			}
			if (a_BindPoses == null)
			{
				throw new NullReferenceException("The bind poses are null!");
			}
			if (a_Bones.Length != a_BindPoses.Length)
			{
				throw new FormatException("The number of bind poses does not match the number of bones!");
			}
			if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
			{
				if (a_Tangents == null)
				{
					throw new ArgumentNullException("The tangents parameter is not allowed to be null!");
				}
				if (a_Vertices.Length != a_Tangents.Length)
				{
					throw new FormatException("The number of vertices does not match the number of tangents!");
				}
			}
			if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
			{
				if (a_UVs == null)
				{
					throw new ArgumentNullException("The uv parameter is not allowed to be null!");
				}
				if (a_Vertices.Length != a_UVs.Length)
				{
					throw new FormatException("The number of vertices does not match the number of uv's!");
				}
			}
			else if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
			{
				if (a_UV2s == null)
				{
					throw new NullReferenceException("The uv2 parameter is not allowed to be null!");
				}
				if (a_Vertices.Length != a_UV2s.Length)
				{
					throw new FormatException("The number of vertices does not match the number of uv2's!");
				}
			}
			Vector3 v = new Vector3(0f, 1f, 0f);
			Matrix4x4 worldToLocalMatrix = m_Decals.CachedTransform.worldToLocalMatrix;
			v = (activeDecalProjector.ProjectorToWorldMatrix * worldToLocalMatrix).inverse.transpose.MultiplyVector(v).normalized;
			Matrix4x4 a_MeshToDecalsMatrix = worldToLocalMatrix * a_MeshToWorldMatrix;
			Matrix4x4 transpose = a_MeshToDecalsMatrix.inverse.transpose;
			float a_CullingDotProduct = Mathf.Cos(activeDecalProjector.CullingAngle * ((float)Math.PI / 180f));
			int count = m_Vertices.get_Count();
			int count2 = l_Bones.get_Count();
			for (int i = 0; i < a_Bones.Length; i++)
			{
				l_Bones.Add(a_Bones[i]);
				m_BindPoses.Add(a_BindPoses[i] * a_MeshToDecalsMatrix.inverse);
			}
			m_AddSkinnedMeshDelegate(a_Vertices, a_Normals, a_Tangents, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, count2, a_BindPoses, a_SkinQuality, v, a_CullingDotProduct, worldToLocalMatrix, a_MeshToDecalsMatrix, transpose);
			float num = a_WorldToMeshMatrix[0, 0] * a_WorldToMeshMatrix[1, 1] * a_WorldToMeshMatrix[2, 2];
			if (!(num < 0f))
			{
				for (int j = 0; j < a_Triangles.Length; j += 3)
				{
					int a_Index = a_Triangles[j];
					int a_Index2 = a_Triangles[j + 1];
					int a_Index3 = a_Triangles[j + 2];
					if (!m_RemovedIndices.IsRemovedIndex(a_Index) && !m_RemovedIndices.IsRemovedIndex(a_Index2) && !m_RemovedIndices.IsRemovedIndex(a_Index3))
					{
						a_Index = count + m_RemovedIndices.AdjustedIndex(a_Index);
						a_Index2 = count + m_RemovedIndices.AdjustedIndex(a_Index2);
						a_Index3 = count + m_RemovedIndices.AdjustedIndex(a_Index3);
						m_Triangles.Add(a_Index);
						m_Triangles.Add(a_Index2);
						m_Triangles.Add(a_Index3);
					}
				}
			}
			else
			{
				for (int k = 0; k < a_Triangles.Length; k += 3)
				{
					int a_Index4 = a_Triangles[k];
					int a_Index5 = a_Triangles[k + 1];
					int a_Index6 = a_Triangles[k + 2];
					if (!m_RemovedIndices.IsRemovedIndex(a_Index4) && !m_RemovedIndices.IsRemovedIndex(a_Index5) && !m_RemovedIndices.IsRemovedIndex(a_Index6))
					{
						a_Index4 = count + m_RemovedIndices.AdjustedIndex(a_Index4);
						a_Index5 = count + m_RemovedIndices.AdjustedIndex(a_Index5);
						a_Index6 = count + m_RemovedIndices.AdjustedIndex(a_Index6);
						m_Triangles.Add(a_Index5);
						m_Triangles.Add(a_Index4);
						m_Triangles.Add(a_Index6);
					}
				}
			}
			m_RemovedIndices.Clear();
			activeDecalProjector.DecalsMeshUpperVertexIndex = m_Vertices.get_Count() - 1;
			activeDecalProjector.DecalsMeshUpperTriangleIndex = m_Triangles.get_Count() - 1;
			activeDecalProjector.DecalsMeshUpperBonesIndex = l_Bones.get_Count() - 1;
			activeDecalProjector.IsTangentProjectionCalculated = false;
			activeDecalProjector.IsUV1ProjectionCalculated = false;
			activeDecalProjector.IsUV2ProjectionCalculated = false;
		}

		internal override void RemoveRangesOfVertexAlignedLists(int a_LowerIndex, int a_Count)
		{
			base.RemoveRangesOfVertexAlignedLists(a_LowerIndex, a_Count);
			m_OriginalVertices.RemoveRange(a_LowerIndex, a_Count);
			m_BoneWeights.RemoveRange(a_LowerIndex, a_Count);
		}

		internal override int BoneIndexOffset(SkinnedDecalProjectorBase a_Projector)
		{
			return a_Projector.DecalsMeshBonesCount;
		}

		internal override void RemoveBonesAndAdjustBoneWeightIndices(SkinnedDecalProjectorBase a_Projector)
		{
			int decalsMeshLowerBonesIndex = a_Projector.DecalsMeshLowerBonesIndex;
			int decalsMeshUpperBonesIndex = a_Projector.DecalsMeshUpperBonesIndex;
			int decalsMeshBonesCount = a_Projector.DecalsMeshBonesCount;
			if (decalsMeshBonesCount > 0)
			{
				l_Bones.RemoveRange(decalsMeshLowerBonesIndex, decalsMeshBonesCount);
				m_BindPoses.RemoveRange(decalsMeshLowerBonesIndex, decalsMeshBonesCount);
			}
			for (int i = a_Projector.DecalsMeshLowerVertexIndex; i < m_BoneWeights.get_Count(); i++)
			{
				BoneWeight boneWeight = m_BoneWeights.get_Item(i);
				boneWeight.boneIndex0 = AdjustedIndex(boneWeight.boneIndex0, decalsMeshUpperBonesIndex, decalsMeshBonesCount);
				boneWeight.boneIndex1 = AdjustedIndex(boneWeight.boneIndex1, decalsMeshUpperBonesIndex, decalsMeshBonesCount);
				boneWeight.boneIndex2 = AdjustedIndex(boneWeight.boneIndex2, decalsMeshUpperBonesIndex, decalsMeshBonesCount);
				boneWeight.boneIndex3 = AdjustedIndex(boneWeight.boneIndex3, decalsMeshUpperBonesIndex, decalsMeshBonesCount);
				m_BoneWeights.set_Item(i, boneWeight);
			}
		}

		private int AdjustedIndex(int a_Index, int a_UpperIndex, int a_Offset)
		{
			int num = a_Index;
			if (num > a_UpperIndex)
			{
				num -= a_Offset;
			}
			return num;
		}

		internal override void AdjustProjectorIndices(SkinnedDecalProjectorBase a_Projector, int a_VertexIndexOffset, int a_TriangleIndexOffset, int a_BoneIndexOffset)
		{
			base.AdjustProjectorIndices(a_Projector, a_VertexIndexOffset, a_TriangleIndexOffset, a_BoneIndexOffset);
			a_Projector.DecalsMeshLowerBonesIndex -= a_BoneIndexOffset;
			a_Projector.DecalsMeshUpperBonesIndex -= a_BoneIndexOffset;
		}

		public override void ClearAll()
		{
			base.ClearAll();
			OriginalVertices.Clear();
			BoneWeights.Clear();
			Bones.Clear();
			BindPoses.Clear();
		}

		private void AddUnoptimized(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, SkinQuality a_SkinQuality, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			if (a_SkinQuality == SkinQuality.Bone1 || (a_SkinQuality == SkinQuality.Auto && QualitySettings.blendWeights == BlendWeights.OneBone))
			{
				AddUnoptimizedSkinQuality1(a_Vertices, a_Normals, a_Tangents, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, a_BoneIndexOffset, a_BindPoses, a_ReversedProjectionNormal, a_CullingDotProduct, a_WorldToDecalsMatrix, a_MeshToDecalsMatrix, a_MeshToDecalsMatrixInvertedTransposed);
				return;
			}
			if (a_SkinQuality == SkinQuality.Bone2 || (a_SkinQuality == SkinQuality.Auto && QualitySettings.blendWeights == BlendWeights.TwoBones))
			{
				AddUnoptimizedSkinQuality2(a_Vertices, a_Normals, a_Tangents, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, a_BoneIndexOffset, a_BindPoses, a_ReversedProjectionNormal, a_CullingDotProduct, a_WorldToDecalsMatrix, a_MeshToDecalsMatrix, a_MeshToDecalsMatrixInvertedTransposed);
				return;
			}
			switch (a_SkinQuality)
			{
			case SkinQuality.Auto:
				if (QualitySettings.blendWeights != BlendWeights.FourBones)
				{
					break;
				}
				goto case SkinQuality.Bone4;
			case SkinQuality.Bone4:
				AddUnoptimizedSkinQuality4(a_Vertices, a_Normals, a_Tangents, a_UVs, a_UV2s, a_BoneWeights, a_Triangles, a_BoneIndexOffset, a_BindPoses, a_ReversedProjectionNormal, a_CullingDotProduct, a_WorldToDecalsMatrix, a_MeshToDecalsMatrix, a_MeshToDecalsMatrixInvertedTransposed);
				break;
			}
		}

		private void AddUnoptimizedSkinQuality1(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				BoneWeight boneWeight = a_BoneWeights[i];
				boneWeight.boneIndex0 += a_BoneIndexOffset;
				boneWeight.weight0 = 1f;
				boneWeight.weight1 = 0f;
				boneWeight.weight2 = 0f;
				boneWeight.weight3 = 0f;
				Matrix4x4 matrix4x = l_Bones.get_Item(boneWeight.boneIndex0).localToWorldMatrix * m_BindPoses.get_Item(boneWeight.boneIndex0);
				Matrix4x4 matrix4x2 = default(Matrix4x4);
				for (int j = 0; j < 16; j++)
				{
					matrix4x2[j] = matrix4x[j] * boneWeight.weight0;
				}
				matrix4x2 = a_WorldToDecalsMatrix * matrix4x2 * a_MeshToDecalsMatrix;
				Matrix4x4 transpose = matrix4x2.inverse.transpose;
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				Vector3 normalized2 = transpose.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized2))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 vector = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				Vector3 vector2 = matrix4x2.MultiplyPoint3x4(a_Vertices[i]);
				m_Vertices.Add(vector2);
				m_OriginalVertices.Add(vector);
				m_Normals.Add(normalized);
				if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
				{
					Vector4 vector3 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Tangents[i]).normalized;
					m_Tangents.Add(vector3);
				}
				if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
				{
					m_UVs.Add(a_UVs[i]);
				}
				if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
				{
					m_UV2s.Add(a_UV2s[i]);
				}
				m_BoneWeights.Add(boneWeight);
			}
		}

		private void AddUnoptimizedSkinQuality2(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				BoneWeight boneWeight = a_BoneWeights[i];
				boneWeight.boneIndex0 += a_BoneIndexOffset;
				boneWeight.boneIndex1 += a_BoneIndexOffset;
				float num = 1f / (boneWeight.weight0 + boneWeight.weight1);
				boneWeight.weight0 *= num;
				boneWeight.weight1 *= num;
				boneWeight.weight2 = 0f;
				boneWeight.weight3 = 0f;
				Matrix4x4 matrix4x = l_Bones.get_Item(boneWeight.boneIndex0).localToWorldMatrix * m_BindPoses.get_Item(boneWeight.boneIndex0);
				Matrix4x4 matrix4x2 = l_Bones.get_Item(boneWeight.boneIndex1).localToWorldMatrix * m_BindPoses.get_Item(boneWeight.boneIndex1);
				Matrix4x4 matrix4x3 = default(Matrix4x4);
				for (int j = 0; j < 16; j++)
				{
					matrix4x3[j] = matrix4x[j] * boneWeight.weight0 + matrix4x2[j] * boneWeight.weight1;
				}
				matrix4x3 = a_WorldToDecalsMatrix * matrix4x3 * a_MeshToDecalsMatrix;
				Matrix4x4 transpose = matrix4x3.inverse.transpose;
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				Vector3 normalized2 = transpose.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized2))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 vector = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				Vector3 vector2 = matrix4x3.MultiplyPoint3x4(a_Vertices[i]);
				m_Vertices.Add(vector2);
				m_OriginalVertices.Add(vector);
				m_Normals.Add(normalized);
				if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
				{
					Vector4 vector3 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Tangents[i]).normalized;
					m_Tangents.Add(vector3);
				}
				if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
				{
					m_UVs.Add(a_UVs[i]);
				}
				if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
				{
					m_UV2s.Add(a_UV2s[i]);
				}
				m_BoneWeights.Add(boneWeight);
			}
		}

		private void AddUnoptimizedSkinQuality4(Vector3[] a_Vertices, Vector3[] a_Normals, Vector4[] a_Tangents, Vector2[] a_UVs, Vector2[] a_UV2s, BoneWeight[] a_BoneWeights, int[] a_Triangles, int a_BoneIndexOffset, Matrix4x4[] a_BindPoses, Vector3 a_ReversedProjectionNormal, float a_CullingDotProduct, Matrix4x4 a_WorldToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrix, Matrix4x4 a_MeshToDecalsMatrixInvertedTransposed)
		{
			for (int i = 0; i < a_Vertices.Length; i++)
			{
				BoneWeight boneWeight = a_BoneWeights[i];
				boneWeight.boneIndex0 += a_BoneIndexOffset;
				boneWeight.boneIndex1 += a_BoneIndexOffset;
				boneWeight.boneIndex2 += a_BoneIndexOffset;
				boneWeight.boneIndex3 += a_BoneIndexOffset;
				Matrix4x4 matrix4x = l_Bones.get_Item(boneWeight.boneIndex0).localToWorldMatrix * m_BindPoses.get_Item(boneWeight.boneIndex0);
				Matrix4x4 matrix4x2 = l_Bones.get_Item(boneWeight.boneIndex1).localToWorldMatrix * m_BindPoses.get_Item(boneWeight.boneIndex1);
				Matrix4x4 matrix4x3 = l_Bones.get_Item(boneWeight.boneIndex2).localToWorldMatrix * m_BindPoses.get_Item(boneWeight.boneIndex2);
				Matrix4x4 matrix4x4 = l_Bones.get_Item(boneWeight.boneIndex3).localToWorldMatrix * m_BindPoses.get_Item(boneWeight.boneIndex3);
				Matrix4x4 matrix4x5 = default(Matrix4x4);
				for (int j = 0; j < 16; j++)
				{
					matrix4x5[j] = matrix4x[j] * boneWeight.weight0 + matrix4x2[j] * boneWeight.weight1 + matrix4x3[j] * boneWeight.weight2 + matrix4x4[j] * boneWeight.weight3;
				}
				matrix4x5 = a_WorldToDecalsMatrix * matrix4x5 * a_MeshToDecalsMatrix;
				Matrix4x4 transpose = matrix4x5.inverse.transpose;
				Vector3 normalized = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Normals[i]).normalized;
				Vector3 normalized2 = transpose.MultiplyVector(a_Normals[i]).normalized;
				if (a_CullingDotProduct > Vector3.Dot(a_ReversedProjectionNormal, normalized2))
				{
					m_RemovedIndices.AddRemovedIndex(i);
					continue;
				}
				Vector3 vector = a_MeshToDecalsMatrix.MultiplyPoint3x4(a_Vertices[i]);
				Vector3 vector2 = matrix4x5.MultiplyPoint3x4(a_Vertices[i]);
				m_Vertices.Add(vector2);
				m_OriginalVertices.Add(vector);
				m_Normals.Add(normalized);
				if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
				{
					Vector4 vector3 = a_MeshToDecalsMatrixInvertedTransposed.MultiplyVector(a_Tangents[i]).normalized;
					m_Tangents.Add(vector3);
				}
				if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
				{
					m_UVs.Add(a_UVs[i]);
				}
				if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
				{
					m_UV2s.Add(a_UV2s[i]);
				}
				m_BoneWeights.Add(boneWeight);
			}
		}

		private void RemoveUnoptimized(int a_StartIndex, int a_Count)
		{
			m_Vertices.RemoveRange(a_StartIndex, a_Count);
			m_Normals.RemoveRange(a_StartIndex, a_Count);
			m_OriginalVertices.RemoveRange(a_StartIndex, a_Count);
			m_BoneWeights.RemoveRange(a_StartIndex, a_Count);
			if (m_Decals.CurrentUVMode == UVMode.TargetUV || m_Decals.CurrentUVMode == UVMode.TargetUV2)
			{
				m_UVs.RemoveRange(a_StartIndex, a_Count);
			}
			if (m_Decals.CurrentUV2Mode == UV2Mode.TargetUV || m_Decals.CurrentUV2Mode == UV2Mode.TargetUV2)
			{
				m_UV2s.RemoveRange(a_StartIndex, a_Count);
			}
			if (m_Decals.CurrentTangentsMode == TangentsMode.Target)
			{
				m_Tangents.RemoveRange(a_StartIndex, a_Count);
			}
		}

		public override void OffsetActiveProjectorVertices()
		{
			SkinnedDecalProjectorBase activeDecalProjector = base.ActiveDecalProjector;
			float meshOffset = activeDecalProjector.MeshOffset;
			if (!Mathf.Approximately(meshOffset, 0f))
			{
				Matrix4x4 worldToLocalMatrix = m_Decals.CachedTransform.worldToLocalMatrix;
				Matrix4x4 transpose = worldToLocalMatrix.transpose;
				int decalsMeshLowerVertexIndex = activeDecalProjector.DecalsMeshLowerVertexIndex;
				int decalsMeshUpperVertexIndex = activeDecalProjector.DecalsMeshUpperVertexIndex;
				for (int i = decalsMeshLowerVertexIndex; i <= decalsMeshUpperVertexIndex; i++)
				{
					Vector3 v = transpose.MultiplyVector(m_Normals.get_Item(i)).normalized * meshOffset;
					v = worldToLocalMatrix.MultiplyVector(v);
					m_OriginalVertices.set_Item(i, m_OriginalVertices.get_Item(i) + v);
				}
			}
		}
	}
}
