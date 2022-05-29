using I2.Loc;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public sealed class ActivityIndicator : MonoBehaviour
{
	public static ActivityIndicator instance;

	public float rotSpeed = 180f;

	private Vector3 vectRotateSpeed;

	public string text;

	public Camera needCam;

	public GameObject panelWindowLoading;

	public GameObject panelIndicator;

	public GameObject objIndicator;

	public GameObject panelProgress;

	public UILabel lbLoading;

	public UILabel lbPercentLoading;

	public UILabel legendLabel;

	public UITexture[] txFon;

	public UITexture txProgressBar;

	private static float curPers;

	private bool canClearMemory = true;

	public static bool IsActiveIndicator
	{
		get
		{
			if (ActivityIndicator.instance == null || ActivityIndicator.instance.panelIndicator == null)
			{
				return false;
			}
			return ActivityIndicator.instance.panelIndicator.activeSelf;
		}
		set
		{
			if (ActivityIndicator.instance == null)
			{
				return;
			}
			if (ActivityIndicator.instance.panelIndicator != null)
			{
				ActivityIndicator.instance.panelIndicator.SetActive(value);
			}
			if (ActivityIndicator.instance.needCam != null)
			{
				ActivityIndicator.instance.needCam.Render();
			}
			if (!value)
			{
				ActivityIndicator.instance.HandleLocalizationChanged();
			}
		}
	}

	public static bool IsShowWindowLoading
	{
		set
		{
			if (ActivityIndicator.instance != null)
			{
				if (!value && ActivityIndicator.instance.txFon != null)
				{
					ActivityIndicator.instance.txFon[0].mainTexture = null;
				}
				if (ActivityIndicator.instance.panelWindowLoading != null)
				{
					ActivityIndicator.instance.panelWindowLoading.SetActive(value);
				}
			}
		}
	}

	public static float LoadingProgress
	{
		get
		{
			return ActivityIndicator.curPers;
		}
		set
		{
			if (ActivityIndicator.instance != null)
			{
				ActivityIndicator.curPers = value;
				ActivityIndicator.curPers = Mathf.Clamp01(ActivityIndicator.curPers);
				if (ActivityIndicator.curPers < 0f)
				{
					ActivityIndicator.curPers = 0f;
				}
				if (ActivityIndicator.curPers > 1f)
				{
					ActivityIndicator.curPers = 1f;
				}
				if (ActivityIndicator.instance.txProgressBar != null)
				{
					ActivityIndicator.instance.txProgressBar.fillAmount = ActivityIndicator.curPers;
				}
				if (ActivityIndicator.instance.lbPercentLoading)
				{
					ActivityIndicator.instance.lbPercentLoading.text = string.Format("{0}%", Mathf.RoundToInt(ActivityIndicator.curPers * 100f));
				}
			}
		}
	}

	static ActivityIndicator()
	{
	}

	public ActivityIndicator()
	{
	}

	public void Awake()
	{
		ActivityIndicator.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.vectRotateSpeed = new Vector3(0f, this.rotSpeed, 0f);
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			base.gameObject.AddComponent<PurchasesSynchronizerListener>();
		}
	}

	public static void ClearMemory()
	{
		if (ActivityIndicator.instance != null && ActivityIndicator.instance.canClearMemory)
		{
			ActivityIndicator.instance.StartCoroutine(ActivityIndicator.instance.Crt_ClearMemory());
		}
	}

	[DebuggerHidden]
	private IEnumerator Crt_ClearMemory()
	{
		ActivityIndicator.u003cCrt_ClearMemoryu003ec__Iterator1 variable = null;
		return variable;
	}

	private void HandleLocalizationChanged()
	{
		if (this.lbLoading != null)
		{
			this.text = LocalizationStore.Get("Key_0853");
			this.lbLoading.text = this.text;
		}
	}

	private void OnDestroy()
	{
		ActivityIndicator.instance = null;
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	private void OnEnable()
	{
		this.HandleLocalizationChanged();
	}

	[DebuggerHidden]
	public IEnumerable<float> ReplaceLoadingFon(Texture needFon, float duration)
	{
		ActivityIndicator.u003cReplaceLoadingFonu003ec__Iterator0 variable = null;
		return variable;
	}

	public static void SetActiveWithCaption(string caption)
	{
		if (ActivityIndicator.instance != null && ActivityIndicator.instance.lbLoading != null)
		{
			ActivityIndicator.instance.lbLoading.text = caption ?? string.Empty;
		}
		ActivityIndicator.IsActiveIndicator = true;
	}

	public static void SetLoadingFon(Texture needFon)
	{
		if (ActivityIndicator.instance == null)
		{
			return;
		}
		if (ActivityIndicator.instance.txFon[0] == null)
		{
			return;
		}
		ActivityIndicator.instance.txFon[0].mainTexture = needFon;
	}

	private void Start()
	{
		this.OnEnable();
		this.lbLoading.GetComponent<Localize>().enabled = true;
		if (Launcher.UsingNewLauncher)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (this.objIndicator != null)
		{
			this.objIndicator.transform.Rotate(this.vectRotateSpeed * Time.deltaTime);
		}
	}
}