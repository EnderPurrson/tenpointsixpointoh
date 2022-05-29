using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PreloadTexture : MonoBehaviour
{
	public string pathTexture;

	public bool clearMemoryOnUnload = true;

	private UITexture nguiTexture;

	public PreloadTexture()
	{
	}

	[DebuggerHidden]
	private IEnumerator Crt_LoadTexture()
	{
		PreloadTexture.u003cCrt_LoadTextureu003ec__Iterator19A variable = null;
		return variable;
	}

	private void OnDisable()
	{
		if (Device.IsLoweMemoryDevice)
		{
			if (this.nguiTexture != null)
			{
				this.nguiTexture.mainTexture = null;
			}
			ActivityIndicator.ClearMemory();
		}
	}

	private void OnEnable()
	{
		if (!Device.IsLoweMemoryDevice)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			if (this.nguiTexture == null)
			{
				this.nguiTexture = base.GetComponent<UITexture>();
			}
			if (this.nguiTexture != null)
			{
				base.StartCoroutine(this.Crt_LoadTexture());
			}
		}
	}
}