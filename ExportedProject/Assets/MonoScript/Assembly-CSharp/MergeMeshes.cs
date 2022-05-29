using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Tasharen/Merge Meshes")]
public class MergeMeshes : MonoBehaviour
{
	public Material material;

	public MergeMeshes.PostMerge afterMerging;

	private string mName;

	private Transform mTrans;

	private Mesh mMesh;

	private MeshFilter mFilter;

	private MeshRenderer mRen;

	private List<GameObject> mDisabledGO = new List<GameObject>();

	private List<Renderer> mDisabledRen = new List<Renderer>();

	private bool mMerge = true;

	public MergeMeshes()
	{
	}

	public void Clear()
	{
		int num = 0;
		int count = this.mDisabledGO.Count;
		while (num < count)
		{
			GameObject item = this.mDisabledGO[num];
			if (item)
			{
				TWTools.SetActive(item, true);
			}
			num++;
		}
		int num1 = 0;
		int count1 = this.mDisabledRen.Count;
		while (num1 < count1)
		{
			Renderer renderer = this.mDisabledRen[num1];
			if (renderer)
			{
				renderer.enabled = true;
			}
			num1++;
		}
		this.mDisabledGO.Clear();
		this.mDisabledRen.Clear();
		if (this.mRen != null)
		{
			this.mRen.enabled = false;
		}
	}

	public void Merge(bool immediate)
	{
		Vector2[] vector2Array;
		Vector2[] vector2Array1;
		Vector3[] vector3Array;
		Vector4[] vector4Array;
		Color[] colorArray;
		Vector3[] vector3Array1;
		Vector4[] vector4Array1;
		Vector2[] vector2Array2;
		Vector2[] vector2Array3;
		Color[] colorArray1;
		if (!immediate)
		{
			this.mMerge = true;
			base.enabled = true;
			return;
		}
		this.mMerge = false;
		this.mName = base.name;
		this.mFilter = base.GetComponent<MeshFilter>();
		this.mTrans = base.transform;
		this.Clear();
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
		if ((int)componentsInChildren.Length == 0 || this.mFilter != null && (int)componentsInChildren.Length == 1)
		{
			return;
		}
		GameObject gameObject = base.gameObject;
		Matrix4x4 matrix4x4 = gameObject.transform.worldToLocalMatrix;
		int num = 0;
		int length = 0;
		int length1 = 0;
		int num1 = 0;
		int length2 = 0;
		int num2 = 0;
		int length3 = 0;
		MeshFilter[] meshFilterArray = componentsInChildren;
		for (int i = 0; i < (int)meshFilterArray.Length; i++)
		{
			MeshFilter meshFilter = meshFilterArray[i];
			if (meshFilter != this.mFilter)
			{
				if (!meshFilter.gameObject.isStatic)
				{
					Mesh mesh = meshFilter.sharedMesh;
					if (this.material == null)
					{
						this.material = meshFilter.GetComponent<Renderer>().sharedMaterial;
					}
					num += mesh.vertexCount;
					length += (int)mesh.triangles.Length;
					if (mesh.normals != null)
					{
						length1 += (int)mesh.normals.Length;
					}
					if (mesh.tangents != null)
					{
						num1 += (int)mesh.tangents.Length;
					}
					if (mesh.colors != null)
					{
						length2 += (int)mesh.colors.Length;
					}
					if (mesh.uv != null)
					{
						num2 += (int)mesh.uv.Length;
					}
					if (mesh.uv2 != null)
					{
						length3 += (int)mesh.uv2.Length;
					}
				}
				else
				{
					Debug.LogError("MergeMeshes can't merge objects marked as static", meshFilter.gameObject);
				}
			}
		}
		if (num == 0)
		{
			Debug.LogWarning("Unable to find any non-static objects to merge", this);
			return;
		}
		Vector3[] vector3Array2 = new Vector3[num];
		int[] numArray = new int[length];
		if (num2 != num)
		{
			vector2Array = null;
		}
		else
		{
			vector2Array = new Vector2[num];
		}
		Vector2[] vector2Array4 = vector2Array;
		if (length3 != num)
		{
			vector2Array1 = null;
		}
		else
		{
			vector2Array1 = new Vector2[num];
		}
		Vector2[] vector2Array5 = vector2Array1;
		if (length1 != num)
		{
			vector3Array = null;
		}
		else
		{
			vector3Array = new Vector3[num];
		}
		Vector3[] vector3Array3 = vector3Array;
		if (num1 != num)
		{
			vector4Array = null;
		}
		else
		{
			vector4Array = new Vector4[num];
		}
		Vector4[] vector4Array2 = vector4Array;
		if (length2 != num)
		{
			colorArray = null;
		}
		else
		{
			colorArray = new Color[num];
		}
		Color[] colorArray2 = colorArray;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		MeshFilter[] meshFilterArray1 = componentsInChildren;
		for (int j = 0; j < (int)meshFilterArray1.Length; j++)
		{
			MeshFilter meshFilter1 = meshFilterArray1[j];
			if (!(meshFilter1 == this.mFilter) && !meshFilter1.gameObject.isStatic)
			{
				Mesh mesh1 = meshFilter1.sharedMesh;
				if (mesh1.vertexCount != 0)
				{
					Matrix4x4 matrix4x41 = meshFilter1.transform.localToWorldMatrix;
					Renderer component = meshFilter1.GetComponent<Renderer>();
					if (this.afterMerging != MergeMeshes.PostMerge.DestroyRenderers)
					{
						component.enabled = false;
						this.mDisabledRen.Add(component);
					}
					if (this.afterMerging == MergeMeshes.PostMerge.DisableGameObjects)
					{
						GameObject gameObject1 = meshFilter1.gameObject;
						Transform transforms = gameObject1.transform;
						while (transforms != this.mTrans)
						{
							if (transforms.GetComponent<Rigidbody>() == null)
							{
								transforms = transforms.parent;
							}
							else
							{
								gameObject1 = transforms.gameObject;
								break;
							}
						}
						this.mDisabledGO.Add(gameObject1);
						TWTools.SetActive(gameObject1, false);
					}
					Vector3[] vector3Array4 = mesh1.vertices;
					if (vector3Array3 == null)
					{
						vector3Array1 = null;
					}
					else
					{
						vector3Array1 = mesh1.normals;
					}
					Vector3[] vector3Array5 = vector3Array1;
					if (vector4Array2 == null)
					{
						vector4Array1 = null;
					}
					else
					{
						vector4Array1 = mesh1.tangents;
					}
					Vector4[] vector4Array3 = vector4Array1;
					if (vector2Array4 == null)
					{
						vector2Array2 = null;
					}
					else
					{
						vector2Array2 = mesh1.uv;
					}
					Vector2[] vector2Array6 = vector2Array2;
					if (vector2Array5 == null)
					{
						vector2Array3 = null;
					}
					else
					{
						vector2Array3 = mesh1.uv2;
					}
					Vector2[] vector2Array7 = vector2Array3;
					if (colorArray2 == null)
					{
						colorArray1 = null;
					}
					else
					{
						colorArray1 = mesh1.colors;
					}
					Color[] colorArray3 = colorArray1;
					int[] numArray1 = mesh1.triangles;
					int num6 = 0;
					int length4 = (int)vector3Array4.Length;
					while (num6 < length4)
					{
						vector3Array2[num5] = matrix4x4.MultiplyPoint3x4(matrix4x41.MultiplyPoint3x4(vector3Array4[num6]));
						if (vector3Array3 != null)
						{
							vector3Array3[num5] = matrix4x4.MultiplyVector(matrix4x41.MultiplyVector(vector3Array5[num6]));
						}
						if (colorArray2 != null)
						{
							colorArray2[num5] = colorArray3[num6];
						}
						if (vector2Array4 != null)
						{
							vector2Array4[num5] = vector2Array6[num6];
						}
						if (vector2Array5 != null)
						{
							vector2Array5[num5] = vector2Array7[num6];
						}
						if (vector4Array2 != null)
						{
							Vector4 vector4 = vector4Array3[num6];
							Vector3 vector3 = new Vector3(vector4.x, vector4.y, vector4.z);
							vector3 = matrix4x4.MultiplyVector(matrix4x41.MultiplyVector(vector3));
							vector4.x = vector3.x;
							vector4.y = vector3.y;
							vector4.z = vector3.z;
							vector4Array2[num5] = vector4;
						}
						num5++;
						num6++;
					}
					int num7 = 0;
					int length5 = (int)numArray1.Length;
					while (num7 < length5)
					{
						int num8 = num4;
						num4 = num8 + 1;
						numArray[num8] = num3 + numArray1[num7];
						num7++;
					}
					num3 = num5;
					if (this.afterMerging == MergeMeshes.PostMerge.DestroyGameObjects)
					{
						UnityEngine.Object.Destroy(meshFilter1.gameObject);
					}
					else if (this.afterMerging == MergeMeshes.PostMerge.DestroyRenderers)
					{
						UnityEngine.Object.Destroy(component);
					}
				}
			}
		}
		if (this.afterMerging == MergeMeshes.PostMerge.DestroyGameObjects)
		{
			componentsInChildren = null;
			this.mDisabledGO.Clear();
		}
		if ((int)vector3Array2.Length <= 0)
		{
			this.Release();
		}
		else
		{
			if (this.mMesh != null)
			{
				this.mMesh.Clear();
			}
			else
			{
				this.mMesh = new Mesh()
				{
					hideFlags = HideFlags.DontSave
				};
			}
			this.mMesh.name = this.mName;
			this.mMesh.vertices = vector3Array2;
			this.mMesh.normals = vector3Array3;
			this.mMesh.tangents = vector4Array2;
			this.mMesh.colors = colorArray2;
			this.mMesh.uv = vector2Array4;
			this.mMesh.uv2 = vector2Array5;
			this.mMesh.triangles = numArray;
			this.mMesh.RecalculateBounds();
			if (this.mFilter == null)
			{
				this.mFilter = gameObject.AddComponent<MeshFilter>();
				this.mFilter.mesh = this.mMesh;
			}
			if (this.mRen == null)
			{
				this.mRen = gameObject.AddComponent<MeshRenderer>();
			}
			this.mRen.sharedMaterial = this.material;
			this.mRen.enabled = true;
			gameObject.name = string.Concat(new object[] { this.mName, " (", (int)numArray.Length / 3, " tri)" });
		}
		base.enabled = false;
	}

	public void Release()
	{
		this.Clear();
		TWTools.Destroy(this.mRen);
		TWTools.Destroy(this.mFilter);
		TWTools.Destroy(this.mMesh);
		this.mFilter = null;
		this.mMesh = null;
		this.mRen = null;
	}

	private void Start()
	{
		if (this.mMerge)
		{
			this.Merge(true);
		}
		base.enabled = false;
	}

	private void Update()
	{
		if (this.mMerge)
		{
			this.Merge(true);
		}
		base.enabled = false;
	}

	public enum PostMerge
	{
		DisableRenderers,
		DestroyRenderers,
		DisableGameObjects,
		DestroyGameObjects
	}
}