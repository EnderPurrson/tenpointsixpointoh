using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Mobile Bloom V2")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[Serializable]
public class MobileBloom : MonoBehaviour
{
	public float intensity;

	public float threshhold;

	public float blurWidth;

	public bool extraBlurry;

	public Material bloomMaterial;

	private bool supported;

	private RenderTexture tempRtA;

	private RenderTexture tempRtB;

	public MobileBloom()
	{
		this.intensity = 0.7f;
		this.threshhold = 0.75f;
		this.blurWidth = 1f;
	}

	public override void CreateBuffers()
	{
		if (!this.tempRtA)
		{
			this.tempRtA = new RenderTexture(Screen.width / 4, Screen.height / 4, 0)
			{
				hideFlags = HideFlags.DontSave
			};
		}
		if (!this.tempRtB)
		{
			this.tempRtB = new RenderTexture(Screen.width / 4, Screen.height / 4, 0)
			{
				hideFlags = HideFlags.DontSave
			};
		}
	}

	public override bool EarlyOutIfNotSupported(RenderTexture source, RenderTexture destination)
	{
		bool flag;
		if (this.Supported())
		{
			flag = false;
		}
		else
		{
			this.enabled = false;
			Graphics.Blit(source, destination);
			flag = true;
		}
		return flag;
	}

	public override void Main()
	{
	}

	public override void OnDisable()
	{
		if (this.tempRtA)
		{
			UnityEngine.Object.DestroyImmediate(this.tempRtA);
			this.tempRtA = null;
		}
		if (this.tempRtB)
		{
			UnityEngine.Object.DestroyImmediate(this.tempRtB);
			this.tempRtB = null;
		}
	}

	public override void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.CreateBuffers();
		if (!this.EarlyOutIfNotSupported(source, destination))
		{
			this.bloomMaterial.SetVector("_Parameter", new Vector4((float)0, (float)0, this.threshhold, this.intensity / (1f - this.threshhold)));
			float single = 1f / ((float)source.width * 1f);
			float single1 = 1f / ((float)source.height * 1f);
			this.bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * single, 1.5f * single1, -1.5f * single, 1.5f * single1));
			this.bloomMaterial.SetVector("_OffsetsB", new Vector4(-1.5f * single, -1.5f * single1, 1.5f * single, -1.5f * single1));
			Graphics.Blit(source, this.tempRtB, this.bloomMaterial, 1);
			single = single * (4f * this.blurWidth);
			single1 = single1 * (4f * this.blurWidth);
			this.bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * single, (float)0, -1.5f * single, (float)0));
			this.bloomMaterial.SetVector("_OffsetsB", new Vector4(0.5f * single, (float)0, -0.5f * single, (float)0));
			Graphics.Blit(this.tempRtB, this.tempRtA, this.bloomMaterial, 2);
			this.bloomMaterial.SetVector("_OffsetsA", new Vector4((float)0, 1.5f * single1, (float)0, -1.5f * single1));
			this.bloomMaterial.SetVector("_OffsetsB", new Vector4((float)0, 0.5f * single1, (float)0, -0.5f * single1));
			Graphics.Blit(this.tempRtA, this.tempRtB, this.bloomMaterial, 2);
			if (this.extraBlurry)
			{
				this.bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * single, (float)0, -1.5f * single, (float)0));
				this.bloomMaterial.SetVector("_OffsetsB", new Vector4(0.5f * single, (float)0, -0.5f * single, (float)0));
				Graphics.Blit(this.tempRtB, this.tempRtA, this.bloomMaterial, 2);
				this.bloomMaterial.SetVector("_OffsetsA", new Vector4((float)0, 1.5f * single1, (float)0, -1.5f * single1));
				this.bloomMaterial.SetVector("_OffsetsB", new Vector4((float)0, 0.5f * single1, (float)0, -0.5f * single1));
				Graphics.Blit(this.tempRtA, this.tempRtB, this.bloomMaterial, 2);
			}
			this.bloomMaterial.SetTexture("_Bloom", this.tempRtB);
			Graphics.Blit(source, destination, this.bloomMaterial, 0);
		}
	}

	public override bool Supported()
	{
		bool flag;
		if (!this.supported)
		{
			bool flag1 = SystemInfo.supportsImageEffects;
			if (flag1)
			{
				flag1 = SystemInfo.supportsRenderTextures;
			}
			if (flag1)
			{
				flag1 = this.bloomMaterial.shader.isSupported;
			}
			this.supported = flag1;
			flag = this.supported;
		}
		else
		{
			flag = true;
		}
		return flag;
	}
}