using System;
using UnityEngine;

public class ObjectPrerenderer : MonoBehaviour
{
	public Camera activeCamera;

	private RenderTexture _rt;

	public bool FinishPrerendering;

	private GameObject _enemiesToRender;

	public ObjectPrerenderer()
	{
	}

	private void Awake()
	{
		this._rt = new RenderTexture(32, 32, 24);
		this._rt.Create();
		this.activeCamera.targetTexture = this._rt;
		this.activeCamera.useOcclusionCulling = false;
	}

	public void Render_()
	{
		this.activeCamera.Render();
		RenderTexture.active = this._rt;
		this.activeCamera.targetTexture = null;
		RenderTexture.active = null;
	}
}