using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EveryplayAnimatedThumbnail : MonoBehaviour
{
	private EveryplayThumbnailPool thumbnailPool;

	private Renderer mainRenderer;

	private Texture defaultTexture;

	private int currentIndex;

	private bool transitionInProgress;

	private float blend;

	public EveryplayAnimatedThumbnail()
	{
	}

	private void Awake()
	{
		this.mainRenderer = base.GetComponent<Renderer>();
	}

	[DebuggerHidden]
	private IEnumerator CrossfadeTransition()
	{
		EveryplayAnimatedThumbnail.u003cCrossfadeTransitionu003ec__Iterator4 variable = null;
		return variable;
	}

	private void OnDestroy()
	{
		this.StopTransitions();
	}

	private void OnDisable()
	{
		this.StopTransitions();
	}

	private void ResetThumbnail()
	{
		this.currentIndex = -1;
		this.StopTransitions();
		this.blend = 0f;
		this.mainRenderer.material.SetFloat("_Blend", this.blend);
		if (this.mainRenderer.material.mainTexture != this.defaultTexture)
		{
			this.mainRenderer.material.mainTextureScale = Vector2.one;
			this.mainRenderer.material.mainTexture = this.defaultTexture;
		}
	}

	private void Start()
	{
		this.thumbnailPool = (EveryplayThumbnailPool)UnityEngine.Object.FindObjectOfType(typeof(EveryplayThumbnailPool));
		if (!this.thumbnailPool)
		{
			UnityEngine.Debug.Log("Everyplay thumbnail pool not found or no material was defined!");
		}
		else
		{
			this.defaultTexture = this.mainRenderer.material.mainTexture;
			this.ResetThumbnail();
		}
	}

	private void StopTransitions()
	{
		this.transitionInProgress = false;
		base.StopAllCoroutines();
	}

	private void Update()
	{
		if (this.thumbnailPool && !this.transitionInProgress)
		{
			if (this.thumbnailPool.availableThumbnailCount <= 0)
			{
				if (this.currentIndex >= 0)
				{
					this.ResetThumbnail();
				}
			}
			else if (this.currentIndex < 0)
			{
				this.currentIndex = 0;
				this.mainRenderer.material.mainTextureScale = this.thumbnailPool.thumbnailScale;
				this.mainRenderer.material.mainTexture = this.thumbnailPool.thumbnailTextures[this.currentIndex];
			}
			else if (this.thumbnailPool.availableThumbnailCount > 1 && Time.frameCount % 50 == 0)
			{
				this.currentIndex++;
				if (this.currentIndex >= this.thumbnailPool.availableThumbnailCount)
				{
					this.currentIndex = 0;
				}
				this.mainRenderer.material.SetTextureScale("_MainTex2", this.thumbnailPool.thumbnailScale);
				this.mainRenderer.material.SetTexture("_MainTex2", this.thumbnailPool.thumbnailTextures[this.currentIndex]);
				this.transitionInProgress = true;
				base.StartCoroutine("CrossfadeTransition");
			}
		}
	}
}