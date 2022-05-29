using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Sun Shafts")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[Serializable]
public class SunShafts : PostEffectsBase
{
	public SunShaftsResolution resolution;

	public ShaftsScreenBlendMode screenBlendMode;

	public Transform sunTransform;

	public int radialBlurIterations;

	public Color sunColor;

	public float sunShaftBlurRadius;

	public float sunShaftIntensity;

	public float useSkyBoxAlpha;

	public float maxRadius;

	public bool useDepthTexture;

	public Shader sunShaftsShader;

	private Material sunShaftsMaterial;

	public Shader simpleClearShader;

	private Material simpleClearMaterial;

	public SunShafts()
	{
		this.resolution = SunShaftsResolution.Normal;
		this.screenBlendMode = ShaftsScreenBlendMode.Screen;
		this.radialBlurIterations = 2;
		this.sunColor = Color.white;
		this.sunShaftBlurRadius = 2.5f;
		this.sunShaftIntensity = 1.15f;
		this.useSkyBoxAlpha = 0.75f;
		this.maxRadius = 0.75f;
		this.useDepthTexture = true;
	}

	public override bool CheckResources()
	{
		this.CheckSupport(this.useDepthTexture);
		this.sunShaftsMaterial = this.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
		this.simpleClearMaterial = this.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
		if (!this.isSupported)
		{
			this.ReportAutoDisable();
		}
		return this.isSupported;
	}

	private int ClampBlurIterationsToSomethingThatMakesSense(int its)
	{
		int num;
		if (its >= 1)
		{
			num = (its <= 4 ? its : 4);
		}
		else
		{
			num = 1;
		}
		return num;
	}

	public override void Main()
	{
	}

	public override void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.CheckResources())
		{
			if (this.useDepthTexture)
			{
				this.GetComponent<Camera>().depthTextureMode = this.GetComponent<Camera>().depthTextureMode | DepthTextureMode.Depth;
			}
			float single = 4f;
			if (this.resolution == SunShaftsResolution.Normal)
			{
				single = 2f;
			}
			else if (this.resolution == SunShaftsResolution.High)
			{
				single = 1f;
			}
			Vector3 vector3 = Vector3.one * 0.5f;
			vector3 = (!this.sunTransform ? new Vector3(0.5f, 0.5f, (float)0) : this.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position));
			RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width / single), (int)((float)source.height / single), 0);
			RenderTexture renderTexture = RenderTexture.GetTemporary((int)((float)source.width / single), (int)((float)source.height / single), 0);
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, (float)0, (float)0) * this.sunShaftBlurRadius);
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector3.x, vector3.y, vector3.z, this.maxRadius));
			this.sunShaftsMaterial.SetFloat("_NoSkyBoxMask", 1f - this.useSkyBoxAlpha);
			if (this.useDepthTexture)
			{
				Graphics.Blit(source, renderTexture, this.sunShaftsMaterial, 2);
			}
			else
			{
				RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0);
				RenderTexture.active = temporary1;
				GL.ClearWithSkybox(false, this.GetComponent<Camera>());
				this.sunShaftsMaterial.SetTexture("_Skybox", temporary1);
				Graphics.Blit(source, renderTexture, this.sunShaftsMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary1);
			}
			this.DrawBorder(renderTexture, this.simpleClearMaterial);
			this.radialBlurIterations = this.ClampBlurIterationsToSomethingThatMakesSense(this.radialBlurIterations);
			float single1 = this.sunShaftBlurRadius * 0.0013020834f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(single1, single1, (float)0, (float)0));
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector3.x, vector3.y, vector3.z, this.maxRadius));
			for (int i = 0; i < this.radialBlurIterations; i++)
			{
				Graphics.Blit(renderTexture, temporary, this.sunShaftsMaterial, 1);
				single1 = this.sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(single1, single1, (float)0, (float)0));
				Graphics.Blit(temporary, renderTexture, this.sunShaftsMaterial, 1);
				single1 = this.sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(single1, single1, (float)0, (float)0));
			}
			if (vector3.z < (float)0)
			{
				this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
			}
			else
			{
				this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
			}
			this.sunShaftsMaterial.SetTexture("_ColorBuffer", renderTexture);
			Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode != ShaftsScreenBlendMode.Screen ? 4 : 0));
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture.ReleaseTemporary(temporary);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}
}