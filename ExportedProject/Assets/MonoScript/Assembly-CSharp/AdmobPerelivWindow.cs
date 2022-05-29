using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class AdmobPerelivWindow : MonoBehaviour
{
	public AdmobPerelivWindow.WinState state;

	private float timeOn = 0.2f;

	private Transform myTransform;

	public static Texture admobTexture;

	public static string admobUrl;

	public UITexture adTexture;

	public GameObject closeAnchor;

	public UISprite closeSprite;

	public UITexture lightTexture;

	public UISprite closeSpriteAndr;

	public static string Context
	{
		get;
		set;
	}

	static AdmobPerelivWindow()
	{
		AdmobPerelivWindow.admobTexture = null;
		AdmobPerelivWindow.admobUrl = string.Empty;
	}

	public AdmobPerelivWindow()
	{
	}

	private void Awake()
	{
		this.myTransform = base.transform;
		this.closeSprite.gameObject.SetActive(false);
		this.closeSpriteAndr.gameObject.SetActive(true);
	}

	public void Hide()
	{
		if (this.state != AdmobPerelivWindow.WinState.show)
		{
			return;
		}
		this.adTexture.mainTexture = null;
		if (AdmobPerelivWindow.admobTexture != null)
		{
			UnityEngine.Object.Destroy(AdmobPerelivWindow.admobTexture);
		}
		AdmobPerelivWindow.admobTexture = null;
		AdmobPerelivWindow.admobUrl = string.Empty;
		this.state = AdmobPerelivWindow.WinState.none;
		base.gameObject.SetActive(false);
	}

	public void Show()
	{
		if (this.state != AdmobPerelivWindow.WinState.none)
		{
			return;
		}
		if (AdmobPerelivWindow.admobTexture == null)
		{
			Debug.LogWarningFormat("AdmobTexture is null.", new object[0]);
			return;
		}
		float single = (float)AdmobPerelivWindow.admobTexture.width;
		float single1 = (float)AdmobPerelivWindow.admobTexture.height;
		if (single1 / single < (float)Screen.height / (float)Screen.width)
		{
			single1 = single1 * (768f * (float)Screen.width) / ((float)Screen.height * single);
			single = 768f * (float)Screen.width / (float)Screen.height;
		}
		else
		{
			single = single * 768f / single1;
			single1 = 768f;
		}
		if (this.adTexture == null)
		{
			Debug.LogWarning("AdTexture is null.");
		}
		else
		{
			this.adTexture.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			this.adTexture.mainTexture = AdmobPerelivWindow.admobTexture;
			this.adTexture.width = Mathf.RoundToInt(single);
			this.adTexture.height = Mathf.RoundToInt(single1);
		}
		this.myTransform.localPosition = new Vector3(0f, 0f, 0f);
		this.state = AdmobPerelivWindow.WinState.show;
	}

	private void Start()
	{
		if (this.closeAnchor != null)
		{
			this.closeAnchor.transform.localPosition = new Vector3((float)(-Screen.width / 2) * 768f / (float)Screen.height, 384f, 0f);
		}
	}

	private void Update()
	{
		if (this.state == AdmobPerelivWindow.WinState.on && this.myTransform.localPosition.y < 0f)
		{
			float single = this.myTransform.localPosition.y;
			single = single + 770f / this.timeOn * Time.deltaTime;
			if (single > 0f)
			{
				single = 0f;
				this.state = AdmobPerelivWindow.WinState.show;
			}
			this.myTransform.localPosition = new Vector3(0f, single, 0f);
		}
		if (this.state == AdmobPerelivWindow.WinState.off && this.myTransform.localPosition.y > -770f)
		{
			float single1 = this.myTransform.localPosition.y;
			single1 = single1 - 770f / this.timeOn * Time.deltaTime;
			if (single1 < -770f)
			{
				single1 = -770f;
				this.state = AdmobPerelivWindow.WinState.none;
				base.gameObject.SetActive(false);
				this.adTexture.mainTexture = null;
				if (AdmobPerelivWindow.admobTexture != null)
				{
					UnityEngine.Object.Destroy(AdmobPerelivWindow.admobTexture);
				}
				AdmobPerelivWindow.admobTexture = null;
				AdmobPerelivWindow.admobUrl = string.Empty;
			}
			this.myTransform.localPosition = new Vector3(0f, single1, 0f);
		}
	}

	public enum WinState
	{
		none,
		on,
		show,
		off
	}
}