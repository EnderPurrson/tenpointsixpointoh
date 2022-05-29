using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Screen Space Ambient Occlusion")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class SSAOEffect : MonoBehaviour
{
	public float m_Radius = 0.4f;

	public SSAOEffect.SSAOSamples m_SampleCount = SSAOEffect.SSAOSamples.Medium;

	public float m_OcclusionIntensity = 1.5f;

	public int m_Blur = 2;

	public int m_Downsampling = 2;

	public float m_OcclusionAttenuation = 1f;

	public float m_MinZ = 0.01f;

	public Shader m_SSAOShader;

	private Material m_SSAOMaterial;

	public Texture2D m_RandomTexture;

	private bool m_Supported;

	public SSAOEffect()
	{
	}

	private static Material CreateMaterial(Shader shader)
	{
		if (!shader)
		{
			return null;
		}
		return new Material(shader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
	}

	private void CreateMaterials()
	{
		if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
		{
			this.m_SSAOMaterial = SSAOEffect.CreateMaterial(this.m_SSAOShader);
			this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
		}
	}

	private static void DestroyMaterial(Material mat)
	{
		if (mat)
		{
			UnityEngine.Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	private void OnDisable()
	{
		SSAOEffect.DestroyMaterial(this.m_SSAOMaterial);
	}

	private void OnEnable()
	{
		Camera component = base.GetComponent<Camera>();
		component.depthTextureMode = component.depthTextureMode | DepthTextureMode.DepthNormals;
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		int mRandomTexture;
		int num;
		Texture texture;
		if (!this.m_Supported || !this.m_SSAOShader.isSupported)
		{
			base.enabled = false;
			return;
		}
		this.CreateMaterials();
		this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
		this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
		this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
		this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
		this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
		this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
		RenderTexture temporary = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
		float component = base.GetComponent<Camera>().fieldOfView;
		float single = base.GetComponent<Camera>().farClipPlane;
		float single1 = Mathf.Tan(component * 0.017453292f * 0.5f) * single;
		float component1 = single1 * base.GetComponent<Camera>().aspect;
		this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(component1, single1, single));
		if (!this.m_RandomTexture)
		{
			mRandomTexture = 1;
			num = 1;
		}
		else
		{
			mRandomTexture = this.m_RandomTexture.width;
			num = this.m_RandomTexture.height;
		}
		this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)temporary.width / (float)mRandomTexture, (float)temporary.height / (float)num, 0f));
		this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
		bool mBlur = this.m_Blur > 0;
		if (!mBlur)
		{
			texture = source;
		}
		else
		{
			texture = null;
		}
		Graphics.Blit(texture, temporary, this.m_SSAOMaterial, (int)this.m_SampleCount);
		if (mBlur)
		{
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
			this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)this.m_Blur / (float)source.width, 0f, 0f, 0f));
			this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
			Graphics.Blit(null, renderTexture, this.m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0);
			this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)this.m_Blur / (float)source.height, 0f, 0f));
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(source, temporary1, this.m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(renderTexture);
			temporary = temporary1;
		}
		this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
		Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
		RenderTexture.ReleaseTemporary(temporary);
	}

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			this.m_Supported = false;
			base.enabled = false;
			return;
		}
		this.CreateMaterials();
		if (this.m_SSAOMaterial && this.m_SSAOMaterial.passCount == 5)
		{
			this.m_Supported = true;
			return;
		}
		this.m_Supported = false;
		base.enabled = false;
	}

	public enum SSAOSamples
	{
		Low,
		Medium,
		High
	}
}