using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Draw Call")]
[ExecuteInEditMode]
public class UIDrawCall : MonoBehaviour
{
	private const int maxIndexBufferCache = 10;

	private static BetterList<UIDrawCall> mActiveList;

	private static BetterList<UIDrawCall> mInactiveList;

	[HideInInspector]
	[NonSerialized]
	public int widgetCount;

	[HideInInspector]
	[NonSerialized]
	public int depthStart = 2147483647;

	[HideInInspector]
	[NonSerialized]
	public int depthEnd = -2147483648;

	[HideInInspector]
	[NonSerialized]
	public UIPanel manager;

	[HideInInspector]
	[NonSerialized]
	public UIPanel panel;

	[HideInInspector]
	[NonSerialized]
	public Texture2D clipTexture;

	[HideInInspector]
	[NonSerialized]
	public bool alwaysOnScreen;

	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector3> norms = new BetterList<Vector3>();

	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector4> tans = new BetterList<Vector4>();

	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	[HideInInspector]
	[NonSerialized]
	public BetterList<Color32> cols = new BetterList<Color32>();

	private Material mMaterial;

	private Texture mTexture;

	private Shader mShader;

	private int mClipCount;

	private Transform mTrans;

	private Mesh mMesh;

	private MeshFilter mFilter;

	private MeshRenderer mRenderer;

	private Material mDynamicMat;

	private int[] mIndices;

	private bool mRebuildMat = true;

	private bool mLegacyShader;

	private int mRenderQueue = 3000;

	private int mTriangles;

	[NonSerialized]
	public bool isDirty;

	[NonSerialized]
	private bool mTextureClip;

	public UIDrawCall.OnRenderCallback onRender;

	private static List<int[]> mCache;

	private static int[] ClipRange;

	private static int[] ClipArgs;

	public static BetterList<UIDrawCall> activeList
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	public Material baseMaterial
	{
		get
		{
			return this.mMaterial;
		}
		set
		{
			if (this.mMaterial != value)
			{
				this.mMaterial = value;
				this.mRebuildMat = true;
			}
		}
	}

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public Material dynamicMaterial
	{
		get
		{
			return this.mDynamicMat;
		}
	}

	public int finalRenderQueue
	{
		get
		{
			return (this.mDynamicMat == null ? this.mRenderQueue : this.mDynamicMat.renderQueue);
		}
	}

	public static BetterList<UIDrawCall> inactiveList
	{
		get
		{
			return UIDrawCall.mInactiveList;
		}
	}

	public bool isClipped
	{
		get
		{
			return this.mClipCount != 0;
		}
	}

	[Obsolete("Use UIDrawCall.activeList")]
	public static BetterList<UIDrawCall> list
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	public Texture mainTexture
	{
		get
		{
			return this.mTexture;
		}
		set
		{
			this.mTexture = value;
			if (this.mDynamicMat != null)
			{
				this.mDynamicMat.mainTexture = value;
			}
		}
	}

	public int renderQueue
	{
		get
		{
			return this.mRenderQueue;
		}
		set
		{
			if (this.mRenderQueue != value)
			{
				this.mRenderQueue = value;
				if (this.mDynamicMat != null)
				{
					this.mDynamicMat.renderQueue = value;
				}
			}
		}
	}

	public Shader shader
	{
		get
		{
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				this.mShader = value;
				this.mRebuildMat = true;
			}
		}
	}

	public int sortingOrder
	{
		get
		{
			return (this.mRenderer == null ? 0 : this.mRenderer.sortingOrder);
		}
		set
		{
			if (this.mRenderer != null && this.mRenderer.sortingOrder != value)
			{
				this.mRenderer.sortingOrder = value;
			}
		}
	}

	public int triangles
	{
		get
		{
			return (this.mMesh == null ? 0 : this.mTriangles);
		}
	}

	static UIDrawCall()
	{
		UIDrawCall.mActiveList = new BetterList<UIDrawCall>();
		UIDrawCall.mInactiveList = new BetterList<UIDrawCall>();
		UIDrawCall.mCache = new List<int[]>(10);
		UIDrawCall.ClipRange = null;
		UIDrawCall.ClipArgs = null;
	}

	public UIDrawCall()
	{
	}

	private void Awake()
	{
		if (UIDrawCall.ClipRange == null)
		{
			UIDrawCall.ClipRange = new int[] { Shader.PropertyToID("_ClipRange0"), Shader.PropertyToID("_ClipRange1"), Shader.PropertyToID("_ClipRange2"), Shader.PropertyToID("_ClipRange4") };
		}
		if (UIDrawCall.ClipArgs == null)
		{
			UIDrawCall.ClipArgs = new int[] { Shader.PropertyToID("_ClipArgs0"), Shader.PropertyToID("_ClipArgs1"), Shader.PropertyToID("_ClipArgs2"), Shader.PropertyToID("_ClipArgs3") };
		}
	}

	public static void ClearAll()
	{
		bool flag = Application.isPlaying;
		int num = UIDrawCall.mActiveList.size;
		while (num > 0)
		{
			int num1 = num - 1;
			num = num1;
			UIDrawCall item = UIDrawCall.mActiveList[num1];
			if (!item)
			{
				continue;
			}
			if (!flag)
			{
				NGUITools.DestroyImmediate(item.gameObject);
			}
			else
			{
				NGUITools.SetActive(item.gameObject, false);
			}
		}
		UIDrawCall.mActiveList.Clear();
	}

	public static int Count(UIPanel panel)
	{
		int num = 0;
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			if (UIDrawCall.mActiveList[i].manager == panel)
			{
				num++;
			}
		}
		return num;
	}

	public static UIDrawCall Create(UIPanel panel, Material mat, Texture tex, Shader shader)
	{
		return UIDrawCall.Create(null, panel, mat, tex, shader);
	}

	private static UIDrawCall Create(string name, UIPanel pan, Material mat, Texture tex, Shader shader)
	{
		UIDrawCall uIDrawCall = UIDrawCall.Create(name);
		uIDrawCall.gameObject.layer = pan.cachedGameObject.layer;
		uIDrawCall.baseMaterial = mat;
		uIDrawCall.mainTexture = tex;
		uIDrawCall.shader = shader;
		uIDrawCall.renderQueue = pan.startingRenderQueue;
		uIDrawCall.sortingOrder = pan.sortingOrder;
		uIDrawCall.manager = pan;
		return uIDrawCall;
	}

	private static UIDrawCall Create(string name)
	{
		if (UIDrawCall.mInactiveList.size <= 0)
		{
			GameObject gameObject = new GameObject(name);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			UIDrawCall uIDrawCall = gameObject.AddComponent<UIDrawCall>();
			UIDrawCall.mActiveList.Add(uIDrawCall);
			return uIDrawCall;
		}
		UIDrawCall uIDrawCall1 = UIDrawCall.mInactiveList.Pop();
		UIDrawCall.mActiveList.Add(uIDrawCall1);
		if (name != null)
		{
			uIDrawCall1.name = name;
		}
		NGUITools.SetActive(uIDrawCall1.gameObject, true);
		return uIDrawCall1;
	}

	private void CreateMaterial()
	{
		string str;
		this.mTextureClip = false;
		this.mLegacyShader = false;
		this.mClipCount = (this.panel == null ? 0 : this.panel.clipCount);
		if (this.mShader == null)
		{
			str = (this.mMaterial == null ? "Unlit/Transparent Colored" : this.mMaterial.shader.name);
		}
		else
		{
			str = this.mShader.name;
		}
		string str1 = str;
		str1 = str1.Replace("GUI/Text Shader", "Unlit/Text");
		if (str1.Length > 2 && str1[str1.Length - 2] == ' ')
		{
			int num = str1[str1.Length - 1];
			if (num > 48 && num <= 57)
			{
				str1 = str1.Substring(0, str1.Length - 2);
			}
		}
		if (str1.StartsWith("Hidden/"))
		{
			str1 = str1.Substring(7);
		}
		str1 = str1.Replace(" (SoftClip)", string.Empty);
		str1 = str1.Replace(" (TextureClip)", string.Empty);
		if (this.panel != null && this.panel.clipping == UIDrawCall.Clipping.TextureMask)
		{
			this.mTextureClip = true;
			this.shader = Shader.Find(string.Concat("Hidden/", str1, " (TextureClip)"));
		}
		else if (this.mClipCount == 0)
		{
			this.shader = Shader.Find(str1);
		}
		else
		{
			this.shader = Shader.Find(string.Concat(new object[] { "Hidden/", str1, " ", this.mClipCount }));
			if (this.shader == null)
			{
				this.shader = Shader.Find(string.Concat(str1, " ", this.mClipCount));
			}
			if (this.shader == null && this.mClipCount == 1)
			{
				this.mLegacyShader = true;
				this.shader = Shader.Find(string.Concat(str1, " (SoftClip)"));
			}
		}
		if (this.shader == null)
		{
			this.shader = Shader.Find("Unlit/Transparent Colored");
		}
		if (this.mMaterial == null)
		{
			this.mDynamicMat = new Material(this.shader)
			{
				name = string.Concat("[NGUI] ", this.shader.name),
				hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset | HideFlags.DontSave
			};
		}
		else
		{
			this.mDynamicMat = new Material(this.mMaterial)
			{
				name = string.Concat("[NGUI] ", this.mMaterial.name),
				hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset | HideFlags.DontSave
			};
			this.mDynamicMat.CopyPropertiesFromMaterial(this.mMaterial);
			string[] strArrays = this.mMaterial.shaderKeywords;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				this.mDynamicMat.EnableKeyword(strArrays[i]);
			}
			if (this.shader != null)
			{
				this.mDynamicMat.shader = this.shader;
			}
			else if (this.mClipCount != 0)
			{
				Debug.LogError(string.Concat(new object[] { str1, " shader doesn't have a clipped shader version for ", this.mClipCount, " clip regions" }));
			}
		}
	}

	public static void Destroy(UIDrawCall dc)
	{
		if (dc)
		{
			dc.onRender = null;
			if (!Application.isPlaying)
			{
				UIDrawCall.mActiveList.Remove(dc);
				NGUITools.DestroyImmediate(dc.gameObject);
			}
			else if (UIDrawCall.mActiveList.Remove(dc))
			{
				NGUITools.SetActive(dc.gameObject, false);
				UIDrawCall.mInactiveList.Add(dc);
			}
		}
	}

	private int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount)
	{
		int num = 0;
		int count = UIDrawCall.mCache.Count;
		while (num < count)
		{
			int[] item = UIDrawCall.mCache[num];
			if (item != null && (int)item.Length == indexCount)
			{
				return item;
			}
			num++;
		}
		int[] numArray = new int[indexCount];
		int num1 = 0;
		for (int i = 0; i < vertexCount; i += 4)
		{
			int num2 = num1;
			num1 = num2 + 1;
			numArray[num2] = i;
			int num3 = num1;
			num1 = num3 + 1;
			numArray[num3] = i + 1;
			int num4 = num1;
			num1 = num4 + 1;
			numArray[num4] = i + 2;
			int num5 = num1;
			num1 = num5 + 1;
			numArray[num5] = i + 2;
			int num6 = num1;
			num1 = num6 + 1;
			numArray[num6] = i + 3;
			int num7 = num1;
			num1 = num7 + 1;
			numArray[num7] = i;
		}
		if (UIDrawCall.mCache.Count > 10)
		{
			UIDrawCall.mCache.RemoveAt(0);
		}
		UIDrawCall.mCache.Add(numArray);
		return numArray;
	}

	private void OnDestroy()
	{
		NGUITools.DestroyImmediate(this.mMesh);
		this.mMesh = null;
	}

	private void OnDisable()
	{
		this.depthStart = 2147483647;
		this.depthEnd = -2147483648;
		this.panel = null;
		this.manager = null;
		this.mMaterial = null;
		this.mTexture = null;
		this.clipTexture = null;
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[0];
		}
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.mDynamicMat = null;
	}

	private void OnEnable()
	{
		this.mRebuildMat = true;
	}

	private void OnWillRenderObject()
	{
		this.UpdateMaterials();
		if (this.onRender != null)
		{
			this.onRender(this.mDynamicMat ?? this.mMaterial);
		}
		if (this.mDynamicMat == null || this.mClipCount == 0)
		{
			return;
		}
		if (this.mTextureClip)
		{
			Vector4 vector4 = this.panel.drawCallClipRange;
			Vector2 vector2 = this.panel.clipSoftness;
			Vector2 vector21 = new Vector2(1000f, 1000f);
			if (vector2.x > 0f)
			{
				vector21.x = vector4.z / vector2.x;
			}
			if (vector2.y > 0f)
			{
				vector21.y = vector4.w / vector2.y;
			}
			this.mDynamicMat.SetVector(UIDrawCall.ClipRange[0], new Vector4(-vector4.x / vector4.z, -vector4.y / vector4.w, 1f / vector4.z, 1f / vector4.w));
			this.mDynamicMat.SetTexture("_ClipTex", this.clipTexture);
		}
		else if (this.mLegacyShader)
		{
			Vector2 vector22 = this.panel.clipSoftness;
			Vector4 vector41 = this.panel.drawCallClipRange;
			Vector2 vector23 = new Vector2(-vector41.x / vector41.z, -vector41.y / vector41.w);
			Vector2 vector24 = new Vector2(1f / vector41.z, 1f / vector41.w);
			Vector2 vector25 = new Vector2(1000f, 1000f);
			if (vector22.x > 0f)
			{
				vector25.x = vector41.z / vector22.x;
			}
			if (vector22.y > 0f)
			{
				vector25.y = vector41.w / vector22.y;
			}
			this.mDynamicMat.mainTextureOffset = vector23;
			this.mDynamicMat.mainTextureScale = vector24;
			this.mDynamicMat.SetVector("_ClipSharpness", vector25);
		}
		else
		{
			UIPanel uIPanel = this.panel;
			int num = 0;
			while (uIPanel != null)
			{
				if (uIPanel.hasClipping)
				{
					float single = 0f;
					Vector4 vector42 = uIPanel.drawCallClipRange;
					if (uIPanel != this.panel)
					{
						Vector3 vector3 = uIPanel.cachedTransform.InverseTransformPoint(this.panel.cachedTransform.position);
						vector42.x -= vector3.x;
						vector42.y -= vector3.y;
						Vector3 vector31 = this.panel.cachedTransform.rotation.eulerAngles;
						Vector3 vector32 = uIPanel.cachedTransform.rotation.eulerAngles - vector31;
						vector32.x = NGUIMath.WrapAngle(vector32.x);
						vector32.y = NGUIMath.WrapAngle(vector32.y);
						vector32.z = NGUIMath.WrapAngle(vector32.z);
						if (Mathf.Abs(vector32.x) > 0.001f || Mathf.Abs(vector32.y) > 0.001f)
						{
							Debug.LogWarning("Panel can only be clipped properly if X and Y rotation is left at 0", this.panel);
						}
						single = vector32.z;
					}
					int num1 = num;
					num = num1 + 1;
					this.SetClipping(num1, vector42, uIPanel.clipSoftness, single);
				}
				uIPanel = uIPanel.parentPanel;
			}
		}
	}

	private Material RebuildMaterial()
	{
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.CreateMaterial();
		this.mDynamicMat.renderQueue = this.mRenderQueue;
		if (this.mTexture != null)
		{
			this.mDynamicMat.mainTexture = this.mTexture;
		}
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[] { this.mDynamicMat };
		}
		return this.mDynamicMat;
	}

	public static void ReleaseAll()
	{
		UIDrawCall.ClearAll();
		UIDrawCall.ReleaseInactive();
	}

	public static void ReleaseInactive()
	{
		int num = UIDrawCall.mInactiveList.size;
		while (num > 0)
		{
			int num1 = num - 1;
			num = num1;
			UIDrawCall item = UIDrawCall.mInactiveList[num1];
			if (!item)
			{
				continue;
			}
			NGUITools.DestroyImmediate(item.gameObject);
		}
		UIDrawCall.mInactiveList.Clear();
	}

	private void SetClipping(int index, Vector4 cr, Vector2 soft, float angle)
	{
		angle *= -0.017453292f;
		Vector2 vector2 = new Vector2(1000f, 1000f);
		if (soft.x > 0f)
		{
			vector2.x = cr.z / soft.x;
		}
		if (soft.y > 0f)
		{
			vector2.y = cr.w / soft.y;
		}
		if (index < (int)UIDrawCall.ClipRange.Length)
		{
			this.mDynamicMat.SetVector(UIDrawCall.ClipRange[index], new Vector4(-cr.x / cr.z, -cr.y / cr.w, 1f / cr.z, 1f / cr.w));
			this.mDynamicMat.SetVector(UIDrawCall.ClipArgs[index], new Vector4(vector2.x, vector2.y, Mathf.Sin(angle), Mathf.Cos(angle)));
		}
	}

	public void UpdateGeometry(int widgetCount)
	{
		bool flag;
		this.widgetCount = widgetCount;
		int num = this.verts.size;
		if (num <= 0 || num != this.uvs.size || num != this.cols.size || num % 4 != 0)
		{
			if (this.mFilter.mesh != null)
			{
				this.mFilter.mesh.Clear();
			}
			Debug.LogError(string.Concat("UIWidgets must fill the buffer with 4 vertices per quad. Found ", num));
		}
		else
		{
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.GetComponent<MeshFilter>();
			}
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (this.verts.size >= 65000)
			{
				this.mTriangles = 0;
				if (this.mFilter.mesh != null)
				{
					this.mFilter.mesh.Clear();
				}
				Debug.LogError(string.Concat("Too many vertices on one panel: ", this.verts.size));
			}
			else
			{
				int num1 = (num >> 1) * 3;
				bool flag1 = (this.mIndices == null ? true : (int)this.mIndices.Length != num1);
				if (this.mMesh == null)
				{
					this.mMesh = new Mesh()
					{
						hideFlags = HideFlags.DontSave,
						name = (this.mMaterial == null ? "[NGUI] Mesh" : string.Concat("[NGUI] ", this.mMaterial.name))
					};
					this.mMesh.MarkDynamic();
					flag1 = true;
				}
				if ((int)this.uvs.buffer.Length != (int)this.verts.buffer.Length || (int)this.cols.buffer.Length != (int)this.verts.buffer.Length || this.norms.buffer != null && (int)this.norms.buffer.Length != (int)this.verts.buffer.Length)
				{
					flag = true;
				}
				else
				{
					flag = (this.tans.buffer == null ? false : (int)this.tans.buffer.Length != (int)this.verts.buffer.Length);
				}
				bool flag2 = flag;
				if (!flag2 && this.panel != null && this.panel.renderQueue != UIPanel.RenderQueue.Automatic)
				{
					flag2 = (this.mMesh == null ? true : this.mMesh.vertexCount != (int)this.verts.buffer.Length);
				}
				this.mTriangles = this.verts.size >> 1;
				if (flag2 || (int)this.verts.buffer.Length > 65000)
				{
					if (flag2 || this.mMesh.vertexCount != this.verts.size)
					{
						this.mMesh.Clear();
						flag1 = true;
					}
					this.mMesh.vertices = this.verts.ToArray();
					this.mMesh.uv = this.uvs.ToArray();
					this.mMesh.colors32 = this.cols.ToArray();
					if (this.norms != null)
					{
						this.mMesh.normals = this.norms.ToArray();
					}
					if (this.tans != null)
					{
						this.mMesh.tangents = this.tans.ToArray();
					}
				}
				else
				{
					if (this.mMesh.vertexCount != (int)this.verts.buffer.Length)
					{
						this.mMesh.Clear();
						flag1 = true;
					}
					this.mMesh.vertices = this.verts.buffer;
					this.mMesh.uv = this.uvs.buffer;
					this.mMesh.colors32 = this.cols.buffer;
					if (this.norms != null)
					{
						this.mMesh.normals = this.norms.buffer;
					}
					if (this.tans != null)
					{
						this.mMesh.tangents = this.tans.buffer;
					}
				}
				if (flag1)
				{
					this.mIndices = this.GenerateCachedIndexBuffer(num, num1);
					this.mMesh.triangles = this.mIndices;
				}
				if (flag2 || !this.alwaysOnScreen)
				{
					this.mMesh.RecalculateBounds();
				}
				this.mFilter.mesh = this.mMesh;
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.GetComponent<MeshRenderer>();
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			this.UpdateMaterials();
		}
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.norms.Clear();
		this.tans.Clear();
	}

	private void UpdateMaterials()
	{
		if (this.panel == null)
		{
			return;
		}
		if (this.mRebuildMat || this.mDynamicMat == null || this.mClipCount != this.panel.clipCount || this.mTextureClip != (this.panel.clipping == UIDrawCall.Clipping.TextureMask))
		{
			this.RebuildMaterial();
			this.mRebuildMat = false;
		}
		else if (this.mRenderer.sharedMaterial != this.mDynamicMat)
		{
			this.mRenderer.sharedMaterials = new Material[] { this.mDynamicMat };
		}
	}

	public enum Clipping
	{
		None = 0,
		TextureMask = 1,
		SoftClip = 3,
		ConstrainButDontClip = 4
	}

	public delegate void OnRenderCallback(Material mat);
}