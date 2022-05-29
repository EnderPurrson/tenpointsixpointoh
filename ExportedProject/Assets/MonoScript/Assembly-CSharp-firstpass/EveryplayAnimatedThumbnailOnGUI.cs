using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class EveryplayAnimatedThumbnailOnGUI : MonoBehaviour
{
	public Texture defaultTexture;

	public Rect pixelInset = new Rect(10f, 10f, 256f, 196f);

	private EveryplayThumbnailPool thumbnailPool;

	private int currentIndex;

	private bool transitionInProgress;

	private float blend;

	private Texture bottomTexture;

	private Vector2 bottomTextureScale;

	private Vector2 topTextureScale;

	private Texture topTexture;

	public EveryplayAnimatedThumbnailOnGUI()
	{
	}

	private void Awake()
	{
		this.bottomTexture = this.defaultTexture;
	}

	[DebuggerHidden]
	private IEnumerator CrossfadeTransition()
	{
		EveryplayAnimatedThumbnailOnGUI.u003cCrossfadeTransitionu003ec__Iterator5 variable = null;
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

	private void OnGUI()
	{
		if (Event.current.type.Equals(EventType.Repaint))
		{
			if (this.bottomTexture)
			{
				GUI.DrawTextureWithTexCoords(new Rect(this.pixelInset.x, this.pixelInset.y, this.pixelInset.width, this.pixelInset.height), this.bottomTexture, new Rect(0f, 0f, this.bottomTextureScale.x, this.bottomTextureScale.y));
			}
			if (this.topTexture && this.blend > 0f)
			{
				Color color = GUI.color;
				GUI.color = new Color(color.r, color.g, color.b, this.blend);
				GUI.DrawTextureWithTexCoords(new Rect(this.pixelInset.x, this.pixelInset.y, this.pixelInset.width, this.pixelInset.height), this.topTexture, new Rect(0f, 0f, this.topTextureScale.x, this.topTextureScale.y));
				GUI.color = color;
			}
		}
	}

	private void ResetThumbnail()
	{
		this.currentIndex = -1;
		this.StopTransitions();
		this.blend = 0f;
		this.bottomTextureScale = Vector2.one;
		this.bottomTexture = this.defaultTexture;
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
				this.bottomTextureScale = this.thumbnailPool.thumbnailScale;
				this.bottomTexture = this.thumbnailPool.thumbnailTextures[this.currentIndex];
			}
			else if (this.thumbnailPool.availableThumbnailCount > 1 && Time.frameCount % 50 == 0)
			{
				this.currentIndex++;
				if (this.currentIndex >= this.thumbnailPool.availableThumbnailCount)
				{
					this.currentIndex = 0;
				}
				this.topTextureScale = this.thumbnailPool.thumbnailScale;
				this.topTexture = this.thumbnailPool.thumbnailTextures[this.currentIndex];
				this.transitionInProgress = true;
				base.StartCoroutine("CrossfadeTransition");
			}
		}
	}
}