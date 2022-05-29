using System;
using UnityEngine;

public class CapePrerenderer : MonoBehaviour
{
	public Camera activeCamera;

	private RenderTexture _rt;

	public bool FinishPrerendering;

	private GameObject _enemiesToRender;

	public CapePrerenderer()
	{
	}

	private void Awake()
	{
		this._rt = new RenderTexture(512, 512, 24);
		this._rt.Create();
		this.activeCamera.targetTexture = this._rt;
		this.activeCamera.useOcclusionCulling = false;
	}

	public Texture Render_()
	{
		this.activeCamera.Render();
		RenderTexture.active = this._rt;
		this.activeCamera.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		return this._rt;
	}
}