using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class LevelCompleteLoader : MonoBehaviour
{
	public static Action action;

	public static string sceneName;

	private Texture fon;

	public UICamera myUICam;

	private Texture loadingNote;

	static LevelCompleteLoader()
	{
		LevelCompleteLoader.sceneName = string.Empty;
	}

	public LevelCompleteLoader()
	{
	}

	[DebuggerHidden]
	private IEnumerator loadNext()
	{
		return new LevelCompleteLoader.u003cloadNextu003ec__IteratorA5();
	}

	private void Start()
	{
		ActivityIndicator.IsActiveIndicator = true;
		if (LevelCompleteLoader.sceneName.Equals("LevelComplete"))
		{
			string str = string.Concat("LevelLoadings", (!Device.isRetinaAndStrong ? string.Empty : "/Hi"), "/LevelComplete_back");
			if (Defs.IsSurvival)
			{
				str = "GameOver_Coliseum";
			}
			this.fon = Resources.Load<Texture>(str);
		}
		else
		{
			this.fon = Resources.Load<Texture>(ConnectSceneNGUIController.MainLoadingTexture());
		}
		UITexture uITexture = (new GameObject()).AddComponent<UITexture>();
		uITexture.mainTexture = this.fon;
		uITexture.SetRect(0f, 0f, 1366f, 768f);
		uITexture.transform.SetParent(this.myUICam.transform, false);
		uITexture.transform.localScale = Vector3.one;
		uITexture.transform.localPosition = Vector3.zero;
		base.StartCoroutine(this.loadNext());
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}
}