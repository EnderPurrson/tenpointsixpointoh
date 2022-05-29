using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("Tasharen/Water")]
[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class TasharenWater : MonoBehaviour
{
	public TasharenWater.Quality quality = TasharenWater.Quality.High;

	public LayerMask highReflectionMask = -1;

	public LayerMask mediumReflectionMask = -1;

	public bool keepUnderCamera = true;

	private Transform mTrans;

	private Hashtable mCameras = new Hashtable();

	private RenderTexture mTex;

	private int mTexSize;

	private Renderer mRen;

	private static bool mIsRendering;

	public LayerMask reflectionMask
	{
		get
		{
			switch (this.quality)
			{
				case TasharenWater.Quality.Medium:
				{
					return this.mediumReflectionMask;
				}
				case TasharenWater.Quality.High:
				case TasharenWater.Quality.Uber:
				{
					return this.highReflectionMask;
				}
			}
			return 0;
		}
	}

	public int reflectionTextureSize
	{
		get
		{
			switch (this.quality)
			{
				case TasharenWater.Quality.Medium:
				case TasharenWater.Quality.High:
				{
					return 512;
				}
				case TasharenWater.Quality.Uber:
				{
					return 1024;
				}
			}
			return 0;
		}
	}

	public bool useRefraction
	{
		get
		{
			return this.quality > TasharenWater.Quality.Fastest;
		}
	}

	static TasharenWater()
	{
	}

	public TasharenWater()
	{
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		this.mRen = base.GetComponent<Renderer>();
		this.quality = TasharenWater.GetQuality();
	}

	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 vector4 = projection.inverse * new Vector4(TasharenWater.SignExt(clipPlane.x), TasharenWater.SignExt(clipPlane.y), 1f, 1f);
		Vector4 vector41 = clipPlane * (2f / Vector4.Dot(clipPlane, vector4));
		projection[2] = vector41.x - projection[3];
		projection[6] = vector41.y - projection[7];
		projection[10] = vector41.z - projection[11];
		projection[14] = vector41.w - projection[15];
	}

	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
	}

	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Matrix4x4 matrix4x4 = cam.worldToCameraMatrix;
		Vector3 vector3 = matrix4x4.MultiplyPoint(pos);
		Vector3 vector31 = matrix4x4.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(vector31.x, vector31.y, vector31.z, -Vector3.Dot(vector3, vector31));
	}

	private void Clear()
	{
		if (this.mTex)
		{
			UnityEngine.Object.DestroyImmediate(this.mTex);
			this.mTex = null;
		}
	}

	private void CopyCamera(Camera src, Camera dest)
	{
		if (src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox component = src.GetComponent<Skybox>();
			Skybox skybox = dest.GetComponent<Skybox>();
			if (!component || !component.material)
			{
				skybox.enabled = false;
			}
			else
			{
				skybox.enabled = true;
				skybox.material = component.material;
			}
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
		dest.depthTextureMode = DepthTextureMode.None;
		dest.renderingPath = RenderingPath.Forward;
	}

	public static TasharenWater.Quality GetQuality()
	{
		return (TasharenWater.Quality)PlayerPrefs.GetInt("Water", 3);
	}

	private Camera GetReflectionCamera(Camera current, Material mat, int textureSize)
	{
		if (!this.mTex || this.mTexSize != textureSize)
		{
			if (this.mTex)
			{
				UnityEngine.Object.DestroyImmediate(this.mTex);
			}
			this.mTex = new RenderTexture(textureSize, textureSize, 16)
			{
				name = string.Concat("__MirrorReflection", base.GetInstanceID()),
				isPowerOfTwo = true,
				hideFlags = HideFlags.DontSave
			};
			this.mTexSize = textureSize;
		}
		Camera item = this.mCameras[current] as Camera;
		if (!item)
		{
			GameObject gameObject = new GameObject(string.Concat(new object[] { "Mirror Refl Camera id", base.GetInstanceID(), " for ", current.GetInstanceID() }), new Type[] { typeof(Camera), typeof(Skybox) })
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			item = gameObject.GetComponent<Camera>();
			item.enabled = false;
			Transform transforms = item.transform;
			transforms.position = this.mTrans.position;
			transforms.rotation = this.mTrans.rotation;
			item.gameObject.AddComponent<FlareLayer>();
			this.mCameras[current] = item;
		}
		if (mat.HasProperty("_ReflectionTex"))
		{
			mat.SetTexture("_ReflectionTex", this.mTex);
		}
		return item;
	}

	private void LateUpdate()
	{
		if (this.keepUnderCamera)
		{
			Vector3 vector3 = Camera.main.transform.position;
			vector3.y = this.mTrans.position.y;
			if (this.mTrans.position != vector3)
			{
				this.mTrans.position = vector3;
			}
		}
	}

	private void OnDisable()
	{
		this.Clear();
		IDictionaryEnumerator enumerator = this.mCameras.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UnityEngine.Object.DestroyImmediate(((Camera)((DictionaryEntry)enumerator.Current).Value).gameObject);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		this.mCameras.Clear();
	}

	private void OnWillRenderObject()
	{
		if (TasharenWater.mIsRendering)
		{
			return;
		}
		if (!base.enabled || !this.mRen || !this.mRen.enabled)
		{
			this.Clear();
			return;
		}
		Material material = this.mRen.sharedMaterial;
		if (!material)
		{
			return;
		}
		this.quality = TasharenWater.Quality.Fastest;
		material.shader.maximumLOD = 100;
	}

	public static void SetQuality(TasharenWater.Quality q)
	{
		TasharenWater[] tasharenWaterArray = UnityEngine.Object.FindObjectsOfType(typeof(TasharenWater)) as TasharenWater[];
		if ((int)tasharenWaterArray.Length <= 0)
		{
			PlayerPrefs.SetInt("Water", (int)q);
		}
		else
		{
			TasharenWater[] tasharenWaterArray1 = tasharenWaterArray;
			for (int i = 0; i < (int)tasharenWaterArray1.Length; i++)
			{
				tasharenWaterArray1[i].quality = q;
			}
		}
	}

	private static float SignExt(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	public enum Quality
	{
		Fastest,
		Low,
		Medium,
		High,
		Uber
	}
}