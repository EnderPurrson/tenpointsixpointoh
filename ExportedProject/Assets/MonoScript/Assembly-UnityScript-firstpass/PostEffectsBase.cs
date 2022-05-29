using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[Serializable]
public class PostEffectsBase : MonoBehaviour
{
	protected bool supportHDRTextures;

	protected bool supportDX11;

	protected bool isSupported;

	public PostEffectsBase()
	{
		this.supportHDRTextures = true;
		this.isSupported = true;
	}

	public override bool CheckResources()
	{
		Debug.LogWarning(("CheckResources () for " + this.ToString()) + " should be overwritten.");
		return this.isSupported;
	}

	public override bool CheckShader(Shader s)
	{
		bool flag;
		Debug.Log(((("The shader " + s.ToString()) + " on effect ") + this.ToString()) + " is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package.");
		if (s.isSupported)
		{
			flag = false;
		}
		else
		{
			this.NotSupported();
			flag = false;
		}
		return flag;
	}

	public override Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
	{
		Material material;
		if (!s)
		{
			Debug.Log("Missing shader in " + this.ToString());
			this.enabled = false;
			material = null;
		}
		else if (s.isSupported && m2Create && m2Create.shader == s)
		{
			material = m2Create;
		}
		else if (s.isSupported)
		{
			m2Create = new Material(s)
			{
				hideFlags = HideFlags.DontSave
			};
			if (!m2Create)
			{
				material = null;
			}
			else
			{
				material = m2Create;
			}
		}
		else
		{
			this.NotSupported();
			Debug.Log(((("The shader " + s.ToString()) + " on effect ") + this.ToString()) + " is not supported on this platform!");
			material = null;
		}
		return material;
	}

	public override bool CheckSupport()
	{
		return this.CheckSupport(false);
	}

	public override bool CheckSupport(bool needDepth)
	{
		bool flag;
		this.isSupported = true;
		this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		bool flag1 = SystemInfo.graphicsShaderLevel >= 50;
		if (flag1)
		{
			flag1 = SystemInfo.supportsComputeShaders;
		}
		this.supportDX11 = flag1;
		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
		{
			this.NotSupported();
			flag = false;
		}
		else if (!needDepth || SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			if (needDepth)
			{
				this.GetComponent<Camera>().depthTextureMode = this.GetComponent<Camera>().depthTextureMode | DepthTextureMode.Depth;
			}
			flag = true;
		}
		else
		{
			this.NotSupported();
			flag = false;
		}
		return flag;
	}

	public override bool CheckSupport(bool needDepth, bool needHdr)
	{
		bool flag;
		if (!this.CheckSupport(needDepth))
		{
			flag = false;
		}
		else if (!needHdr || this.supportHDRTextures)
		{
			flag = true;
		}
		else
		{
			this.NotSupported();
			flag = false;
		}
		return flag;
	}

	public override Material CreateMaterial(Shader s, Material m2Create)
	{
		Material material;
		if (!s)
		{
			Debug.Log("Missing shader in " + this.ToString());
			material = null;
		}
		else if (m2Create && m2Create.shader == s && s.isSupported)
		{
			material = m2Create;
		}
		else if (s.isSupported)
		{
			m2Create = new Material(s)
			{
				hideFlags = HideFlags.DontSave
			};
			if (!m2Create)
			{
				material = null;
			}
			else
			{
				material = m2Create;
			}
		}
		else
		{
			material = null;
		}
		return material;
	}

	public override void DrawBorder(RenderTexture dest, Material material)
	{
		float single = new float();
		float single1 = new float();
		float single2 = new float();
		float single3 = new float();
		RenderTexture.active = dest;
		bool flag = true;
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);
			float single4 = new float();
			float single5 = new float();
			if (!flag)
			{
				single4 = (float)0;
				single5 = 1f;
			}
			else
			{
				single4 = 1f;
				single5 = (float)0;
			}
			single = (float)0;
			single1 = (float)0 + 1f / ((float)dest.width * 1f);
			single2 = (float)0;
			single3 = 1f;
			GL.Begin(7);
			GL.TexCoord2((float)0, single4);
			GL.Vertex3(single, single2, 0.1f);
			GL.TexCoord2(1f, single4);
			GL.Vertex3(single1, single2, 0.1f);
			GL.TexCoord2(1f, single5);
			GL.Vertex3(single1, single3, 0.1f);
			GL.TexCoord2((float)0, single5);
			GL.Vertex3(single, single3, 0.1f);
			single = 1f - 1f / ((float)dest.width * 1f);
			single1 = 1f;
			single2 = (float)0;
			single3 = 1f;
			GL.TexCoord2((float)0, single4);
			GL.Vertex3(single, single2, 0.1f);
			GL.TexCoord2(1f, single4);
			GL.Vertex3(single1, single2, 0.1f);
			GL.TexCoord2(1f, single5);
			GL.Vertex3(single1, single3, 0.1f);
			GL.TexCoord2((float)0, single5);
			GL.Vertex3(single, single3, 0.1f);
			single = (float)0;
			single1 = 1f;
			single2 = (float)0;
			single3 = (float)0 + 1f / ((float)dest.height * 1f);
			GL.TexCoord2((float)0, single4);
			GL.Vertex3(single, single2, 0.1f);
			GL.TexCoord2(1f, single4);
			GL.Vertex3(single1, single2, 0.1f);
			GL.TexCoord2(1f, single5);
			GL.Vertex3(single1, single3, 0.1f);
			GL.TexCoord2((float)0, single5);
			GL.Vertex3(single, single3, 0.1f);
			single = (float)0;
			single1 = 1f;
			single2 = 1f - 1f / ((float)dest.height * 1f);
			single3 = 1f;
			GL.TexCoord2((float)0, single4);
			GL.Vertex3(single, single2, 0.1f);
			GL.TexCoord2(1f, single4);
			GL.Vertex3(single1, single2, 0.1f);
			GL.TexCoord2(1f, single5);
			GL.Vertex3(single1, single3, 0.1f);
			GL.TexCoord2((float)0, single5);
			GL.Vertex3(single, single3, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
	}

	public override bool Dx11Support()
	{
		return this.supportDX11;
	}

	public override void Main()
	{
	}

	public override void NotSupported()
	{
		this.enabled = false;
		this.isSupported = false;
	}

	public override void OnEnable()
	{
		this.isSupported = true;
	}

	public override void ReportAutoDisable()
	{
		Debug.LogWarning(("The image effect " + this.ToString()) + " has been disabled as it's not supported on the current platform.");
	}

	public override void Start()
	{
		this.CheckResources();
	}
}