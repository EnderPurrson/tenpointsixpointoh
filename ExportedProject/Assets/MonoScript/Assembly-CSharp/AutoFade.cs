using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoFade : MonoBehaviour
{
	private static AutoFade m_Instance;

	private Material m_Material;

	private string m_LevelName = string.Empty;

	private bool m_Fading;

	private bool isLoadScene = true;

	private float killedTime;

	public static bool Fading
	{
		get
		{
			return AutoFade.Instance.m_Fading;
		}
	}

	private static AutoFade Instance
	{
		get
		{
			if (AutoFade.m_Instance == null)
			{
				AutoFade.m_Instance = (new GameObject("AutoFade")).AddComponent<AutoFade>();
			}
			return AutoFade.m_Instance;
		}
	}

	static AutoFade()
	{
	}

	public AutoFade()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		AutoFade.m_Instance = this;
		this.m_Material = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
	}

	private void DrawQuad(Color aColor, float aAlpha)
	{
		aColor.a = aAlpha;
		if (!this.m_Material.SetPass(0))
		{
			UnityEngine.Debug.LogWarning("Couldnot set pass for material.");
		}
		else
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(7);
			GL.Color(aColor);
			GL.Vertex3(0f, 0f, -1f);
			GL.Vertex3(0f, 1f, -1f);
			GL.Vertex3(1f, 1f, -1f);
			GL.Vertex3(1f, 0f, -1f);
			GL.End();
			GL.PopMatrix();
		}
	}

	[DebuggerHidden]
	private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor, bool collectGrabage)
	{
		AutoFade.u003cFadeu003ec__Iterator106 variable = null;
		return variable;
	}

	public static void fadeKilled(float aFadeOutTime, float aFadeKilledTime, float aFadeInTime, Color aColor)
	{
		if (AutoFade.Fading)
		{
			return;
		}
		AutoFade.Instance.isLoadScene = false;
		AutoFade.Instance.killedTime = aFadeKilledTime;
		AutoFade.Instance.StartFade(aFadeOutTime, aFadeInTime, aColor, true);
	}

	public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (AutoFade.Fading)
		{
			return;
		}
		AutoFade.Instance.isLoadScene = true;
		AutoFade.Instance.m_LevelName = aLevelName;
		AutoFade.Instance.StartFade(aFadeOutTime, aFadeInTime, aColor, false);
	}

	private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor, bool collectGarbage = false)
	{
		this.m_Fading = true;
		base.StartCoroutine(this.Fade(aFadeOutTime, aFadeInTime, aColor, collectGarbage));
	}
}