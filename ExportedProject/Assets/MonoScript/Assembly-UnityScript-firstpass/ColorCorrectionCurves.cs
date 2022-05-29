using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Color Correction (Curves, Saturation)")]
[ExecuteInEditMode]
[Serializable]
public class ColorCorrectionCurves : PostEffectsBase
{
	public AnimationCurve redChannel;

	public AnimationCurve greenChannel;

	public AnimationCurve blueChannel;

	public bool useDepthCorrection;

	public AnimationCurve zCurve;

	public AnimationCurve depthRedChannel;

	public AnimationCurve depthGreenChannel;

	public AnimationCurve depthBlueChannel;

	private Material ccMaterial;

	private Material ccDepthMaterial;

	private Material selectiveCcMaterial;

	private Texture2D rgbChannelTex;

	private Texture2D rgbDepthChannelTex;

	private Texture2D zCurveTex;

	public float saturation;

	public bool selectiveCc;

	public Color selectiveFromColor;

	public Color selectiveToColor;

	public ColorCorrectionMode mode;

	public bool updateTextures;

	public Shader colorCorrectionCurvesShader;

	public Shader simpleColorCorrectionCurvesShader;

	public Shader colorCorrectionSelectiveShader;

	private bool updateTexturesOnStartup;

	public ColorCorrectionCurves()
	{
		this.saturation = 1f;
		this.selectiveFromColor = Color.white;
		this.selectiveToColor = Color.white;
		this.updateTextures = true;
		this.updateTexturesOnStartup = true;
	}

	public override void Awake()
	{
	}

	public override bool CheckResources()
	{
		this.CheckSupport(this.mode == ColorCorrectionMode.Advanced);
		this.ccMaterial = this.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
		this.ccDepthMaterial = this.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
		this.selectiveCcMaterial = this.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
		if (!this.rgbChannelTex)
		{
			this.rgbChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
		}
		if (!this.rgbDepthChannelTex)
		{
			this.rgbDepthChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
		}
		if (!this.zCurveTex)
		{
			this.zCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
		}
		this.rgbChannelTex.hideFlags = HideFlags.DontSave;
		this.rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
		this.zCurveTex.hideFlags = HideFlags.DontSave;
		this.rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
		this.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
		this.zCurveTex.wrapMode = TextureWrapMode.Clamp;
		if (!this.isSupported)
		{
			this.ReportAutoDisable();
		}
		return this.isSupported;
	}

	public override void Main()
	{
	}

	public override void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.CheckResources())
		{
			if (this.updateTexturesOnStartup)
			{
				this.UpdateParameters();
				this.updateTexturesOnStartup = false;
			}
			if (this.useDepthCorrection)
			{
				this.GetComponent<Camera>().depthTextureMode = this.GetComponent<Camera>().depthTextureMode | DepthTextureMode.Depth;
			}
			RenderTexture temporary = destination;
			if (this.selectiveCc)
			{
				temporary = RenderTexture.GetTemporary(source.width, source.height);
			}
			if (!this.useDepthCorrection)
			{
				this.ccMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, temporary, this.ccMaterial);
			}
			else
			{
				this.ccDepthMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccDepthMaterial.SetTexture("_ZCurve", this.zCurveTex);
				this.ccDepthMaterial.SetTexture("_RgbDepthTex", this.rgbDepthChannelTex);
				this.ccDepthMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, temporary, this.ccDepthMaterial);
			}
			if (this.selectiveCc)
			{
				this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
				this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
				Graphics.Blit(temporary, destination, this.selectiveCcMaterial);
				RenderTexture.ReleaseTemporary(temporary);
			}
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}

	public override void Start()
	{
		base.Start();
		this.updateTexturesOnStartup = true;
	}

	public override void UpdateParameters()
	{
		if (this.redChannel != null && this.greenChannel != null && this.blueChannel != null)
		{
			for (float i = (float)0; i <= 1f; i += 0.003921569f)
			{
				float single = Mathf.Clamp(this.redChannel.Evaluate(i), (float)0, 1f);
				float single1 = Mathf.Clamp(this.greenChannel.Evaluate(i), (float)0, 1f);
				float single2 = Mathf.Clamp(this.blueChannel.Evaluate(i), (float)0, 1f);
				this.rgbChannelTex.SetPixel((int)Mathf.Floor(i * 255f), 0, new Color(single, single, single));
				this.rgbChannelTex.SetPixel((int)Mathf.Floor(i * 255f), 1, new Color(single1, single1, single1));
				this.rgbChannelTex.SetPixel((int)Mathf.Floor(i * 255f), 2, new Color(single2, single2, single2));
				float single3 = Mathf.Clamp(this.zCurve.Evaluate(i), (float)0, 1f);
				this.zCurveTex.SetPixel((int)Mathf.Floor(i * 255f), 0, new Color(single3, single3, single3));
				single = Mathf.Clamp(this.depthRedChannel.Evaluate(i), (float)0, 1f);
				single1 = Mathf.Clamp(this.depthGreenChannel.Evaluate(i), (float)0, 1f);
				single2 = Mathf.Clamp(this.depthBlueChannel.Evaluate(i), (float)0, 1f);
				this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(i * 255f), 0, new Color(single, single, single));
				this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(i * 255f), 1, new Color(single1, single1, single1));
				this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(i * 255f), 2, new Color(single2, single2, single2));
			}
			this.rgbChannelTex.Apply();
			this.rgbDepthChannelTex.Apply();
			this.zCurveTex.Apply();
		}
	}

	public override void UpdateTextures()
	{
		this.UpdateParameters();
	}
}