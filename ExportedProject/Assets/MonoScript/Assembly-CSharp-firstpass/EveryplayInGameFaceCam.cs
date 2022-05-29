using System;
using UnityEngine;

public class EveryplayInGameFaceCam : MonoBehaviour
{
	public Material targetMaterial;

	public int textureSideWidth = 128;

	public TextureFormat textureFormat = TextureFormat.RGBA32;

	public TextureWrapMode textureWrapMode;

	private Texture defaultTexture;

	private Texture2D targetTexture;

	public EveryplayInGameFaceCam()
	{
	}

	private void Awake()
	{
		this.targetTexture = new Texture2D(this.textureSideWidth, this.textureSideWidth, this.textureFormat, false)
		{
			wrapMode = this.textureWrapMode
		};
		if (this.targetMaterial && this.targetTexture)
		{
			this.defaultTexture = this.targetMaterial.mainTexture;
			Everyplay.FaceCamSetTargetTexture(this.targetTexture);
			Everyplay.FaceCamSessionStarted += new Everyplay.FaceCamSessionStartedDelegate(this.OnSessionStart);
			Everyplay.FaceCamSessionStopped += new Everyplay.FaceCamSessionStoppedDelegate(this.OnSessionStop);
		}
	}

	private void OnDestroy()
	{
		Everyplay.FaceCamSessionStarted -= new Everyplay.FaceCamSessionStartedDelegate(this.OnSessionStart);
		Everyplay.FaceCamSessionStopped -= new Everyplay.FaceCamSessionStoppedDelegate(this.OnSessionStop);
	}

	private void OnSessionStart()
	{
		if (this.targetMaterial && this.targetTexture)
		{
			this.targetMaterial.mainTexture = this.targetTexture;
		}
	}

	private void OnSessionStop()
	{
		this.targetMaterial.mainTexture = this.defaultTexture;
	}
}