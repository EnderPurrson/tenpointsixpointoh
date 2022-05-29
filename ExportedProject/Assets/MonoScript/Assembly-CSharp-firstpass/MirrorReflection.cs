using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class MirrorReflection : MonoBehaviour
{
	public bool m_DisablePixelLights = true;

	public int m_TextureSize = 256;

	public float m_ClipPlaneOffset = 0.07f;

	public LayerMask m_ReflectLayers = -1;

	private Hashtable m_ReflectionCameras = new Hashtable();

	private RenderTexture m_ReflectionTexture;

	private int m_OldReflectionTextureSize;

	private static bool s_InsideRendering;

	static MirrorReflection()
	{
	}

	public MirrorReflection()
	{
	}

	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 vector4 = projection.inverse * new Vector4(MirrorReflection.sgn(clipPlane.x), MirrorReflection.sgn(clipPlane.y), 1f, 1f);
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
		Vector3 vector3 = pos + (normal * this.m_ClipPlaneOffset);
		Matrix4x4 matrix4x4 = cam.worldToCameraMatrix;
		Vector3 vector31 = matrix4x4.MultiplyPoint(vector3);
		Vector3 vector32 = matrix4x4.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(vector32.x, vector32.y, vector32.z, -Vector3.Dot(vector31, vector32));
	}

	private void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera)
	{
		reflectionCamera = null;
		if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.m_TextureSize)
		{
			if (this.m_ReflectionTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
			}
			this.m_ReflectionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16)
			{
				name = string.Concat("__MirrorReflection", base.GetInstanceID()),
				isPowerOfTwo = true,
				hideFlags = HideFlags.DontSave
			};
			this.m_OldReflectionTextureSize = this.m_TextureSize;
		}
		reflectionCamera = this.m_ReflectionCameras[currentCamera] as Camera;
		if (!reflectionCamera)
		{
			GameObject gameObject = new GameObject(string.Concat(new object[] { "Mirror Refl Camera id", base.GetInstanceID(), " for ", currentCamera.GetInstanceID() }), new Type[] { typeof(Camera), typeof(Skybox) });
			reflectionCamera = gameObject.GetComponent<Camera>();
			reflectionCamera.enabled = false;
			reflectionCamera.transform.position = base.transform.position;
			reflectionCamera.transform.rotation = base.transform.rotation;
			reflectionCamera.gameObject.AddComponent<FlareLayer>();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			this.m_ReflectionCameras[currentCamera] = reflectionCamera;
		}
	}

	private void OnDisable()
	{
		if (this.m_ReflectionTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
			this.m_ReflectionTexture = null;
		}
		IDictionaryEnumerator enumerator = this.m_ReflectionCameras.GetEnumerator();
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
		this.m_ReflectionCameras.Clear();
	}

	public void OnWillRenderObject()
	{
		Camera mReflectLayers;
		if (!base.enabled || !base.GetComponent<Renderer>() || !base.GetComponent<Renderer>().sharedMaterial || !base.GetComponent<Renderer>().enabled)
		{
			return;
		}
		Camera camera = Camera.current;
		if (!camera)
		{
			return;
		}
		if (MirrorReflection.s_InsideRendering)
		{
			return;
		}
		MirrorReflection.s_InsideRendering = true;
		this.CreateMirrorObjects(camera, out mReflectLayers);
		Vector3 vector3 = base.transform.position;
		Vector3 vector31 = base.transform.up;
		int num = QualitySettings.pixelLightCount;
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = 0;
		}
		this.UpdateCameraModes(camera, mReflectLayers);
		float single = -Vector3.Dot(vector31, vector3) - this.m_ClipPlaneOffset;
		Vector4 vector4 = new Vector4(vector31.x, vector31.y, vector31.z, single);
		Matrix4x4 matrix4x4 = Matrix4x4.zero;
		MirrorReflection.CalculateReflectionMatrix(ref matrix4x4, vector4);
		Vector3 vector32 = camera.transform.position;
		Vector3 vector33 = matrix4x4.MultiplyPoint(vector32);
		mReflectLayers.worldToCameraMatrix = camera.worldToCameraMatrix * matrix4x4;
		Vector4 vector41 = this.CameraSpacePlane(mReflectLayers, vector3, vector31, 1f);
		Matrix4x4 matrix4x41 = camera.projectionMatrix;
		MirrorReflection.CalculateObliqueMatrix(ref matrix4x41, vector41);
		mReflectLayers.projectionMatrix = matrix4x41;
		mReflectLayers.cullingMask = -17 & this.m_ReflectLayers.@value;
		mReflectLayers.targetTexture = this.m_ReflectionTexture;
		GL.SetRevertBackfacing(true);
		mReflectLayers.transform.position = vector33;
		Vector3 vector34 = camera.transform.eulerAngles;
		mReflectLayers.transform.eulerAngles = new Vector3(0f, vector34.y, vector34.z);
		mReflectLayers.Render();
		mReflectLayers.transform.position = vector32;
		GL.SetRevertBackfacing(false);
		Material[] component = base.GetComponent<Renderer>().sharedMaterials;
		Material[] materialArray = component;
		for (int i = 0; i < (int)materialArray.Length; i++)
		{
			Material material = materialArray[i];
			if (material.HasProperty("_ReflectionTex"))
			{
				material.SetTexture("_ReflectionTex", this.m_ReflectionTexture);
			}
		}
		Matrix4x4 matrix4x42 = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, new Vector3(0.5f, 0.5f, 0.5f));
		Vector3 vector35 = base.transform.lossyScale;
		Matrix4x4 matrix4x43 = base.transform.localToWorldMatrix * Matrix4x4.Scale(new Vector3(1f / vector35.x, 1f / vector35.y, 1f / vector35.z));
		matrix4x43 = ((matrix4x42 * camera.projectionMatrix) * camera.worldToCameraMatrix) * matrix4x43;
		Material[] materialArray1 = component;
		for (int j = 0; j < (int)materialArray1.Length; j++)
		{
			materialArray1[j].SetMatrix("_ProjMatrix", matrix4x43);
		}
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = num;
		}
		MirrorReflection.s_InsideRendering = false;
	}

	private static float sgn(float a)
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

	private void UpdateCameraModes(Camera src, Camera dest)
	{
		if (dest == null)
		{
			return;
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		if (src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox component = src.GetComponent(typeof(Skybox)) as Skybox;
			Skybox skybox = dest.GetComponent(typeof(Skybox)) as Skybox;
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
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
	}
}