using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal sealed class ComicsCampaign : MonoBehaviour
{
	public RawImage background;

	public RawImage[] comicFrames = new RawImage[4];

	public Button skipButton;

	public Button backButton;

	public Text subtitlesText;

	private string[] _subtitles = new string[] { string.Empty, string.Empty, string.Empty, string.Empty };

	private int _frameCount;

	private bool _hasSecondPage;

	private bool _isFirstLaunch = true;

	private Coroutine _coroutine;

	private Action _skipHandler;

	public ComicsCampaign()
	{
	}

	private void Awake()
	{
		if (this.subtitlesText != null)
		{
			this.subtitlesText.transform.parent.gameObject.SetActive(LocalizationStore.CurrentLanguage != "English");
		}
		this._frameCount = Math.Min(4, (int)this.comicFrames.Length);
		this._isFirstLaunch = this.DetermineIfFirstLaunch();
	}

	private bool DetermineIfFirstLaunch()
	{
		if (LevelArt.endOfBox)
		{
			string[] strArrays = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			return !strArrays.Any<string>(new Func<string, bool>(CurrentCampaignGame.boXName.Equals));
		}
		string[] strArrays1 = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
		return !strArrays1.Any<string>(new Func<string, bool>(CurrentCampaignGame.levelSceneName.Equals));
	}

	[DebuggerHidden]
	private IEnumerator FadeInCoroutine(Action skipHandler = null)
	{
		ComicsCampaign.u003cFadeInCoroutineu003ec__Iterator12F variable = null;
		return variable;
	}

	private static string GetNameForIndex(int num, bool endOfBox)
	{
		return ResPath.Combine("Arts", ResPath.Combine((!endOfBox ? CurrentCampaignGame.levelSceneName : CurrentCampaignGame.boXName), num.ToString()));
	}

	private void GotoLevelOrBoxmap()
	{
		if (this._coroutine != null)
		{
			base.StopCoroutine(this._coroutine);
		}
		if (!LevelArt.endOfBox)
		{
			string[] strArrays = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			if (Array.IndexOf<string>(strArrays, CurrentCampaignGame.levelSceneName) == -1)
			{
				List<string> strs = new List<string>(strArrays)
				{
					CurrentCampaignGame.levelSceneName
				};
				Save.SaveStringArray(Defs.ArtLevsS, strs.ToArray());
			}
		}
		else
		{
			string[] strArrays1 = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			if (Array.IndexOf<string>(strArrays1, CurrentCampaignGame.boXName) == -1)
			{
				List<string> strs1 = new List<string>(strArrays1)
				{
					CurrentCampaignGame.boXName
				};
				Save.SaveStringArray(Defs.ArtBoxS, strs1.ToArray());
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.endOfBox ? "CampaignLoading" : "ChooseLevel"), LoadSceneMode.Single);
	}

	private void GotoNextPage()
	{
		if (!this._isFirstLaunch)
		{
			this.SetSkipHandler(new Action(this.GotoLevelOrBoxmap));
		}
		else
		{
			this.SetSkipHandler(null);
		}
		if (this._coroutine != null)
		{
			base.StopCoroutine(this._coroutine);
		}
		for (int i = 0; i != (int)this.comicFrames.Length; i++)
		{
			if (this.comicFrames[i] != null)
			{
				this.comicFrames[i].texture = null;
				this.comicFrames[i].color = new Color(1f, 1f, 1f, 0f);
				this._subtitles[i] = string.Empty;
			}
		}
		Resources.UnloadUnusedAssets();
		int num = 0;
		while (num != this._frameCount)
		{
			string nameForIndex = ComicsCampaign.GetNameForIndex(this._frameCount + num + 1, LevelArt.endOfBox);
			Texture texture = Resources.Load<Texture>(nameForIndex);
			if (texture != null)
			{
				this.comicFrames[num].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)texture.width);
				this.comicFrames[num].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)texture.height);
				this.comicFrames[num].texture = texture;
				string str = (!LevelArt.endOfBox ? string.Format("{0}_{1}", CurrentCampaignGame.levelSceneName, this._frameCount + num) : string.Format("{0}_{1}", CurrentCampaignGame.boXName, this._frameCount + num));
				this._subtitles[num] = LocalizationStore.Get(str) ?? string.Empty;
				num++;
			}
			else
			{
				break;
			}
		}
		this._coroutine = base.StartCoroutine(this.FadeInCoroutine(new Action(this.GotoLevelOrBoxmap)));
	}

	public void HandleBackPressed()
	{
		ButtonClickSound.TryPlayClick();
		Singleton<SceneLoader>.Instance.LoadScene("ChooseLevel", LoadSceneMode.Single);
	}

	public void HandleSkipPressed()
	{
		ButtonClickSound.TryPlayClick();
		UnityEngine.Debug.Log("[Skip] pressed.");
		if (this._skipHandler != null)
		{
			this._skipHandler();
		}
	}

	private void SetSkipHandler(Action skipHandler)
	{
		this._skipHandler = skipHandler;
		if (this.skipButton != null)
		{
			this.skipButton.gameObject.SetActive(skipHandler != null);
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		ComicsCampaign.u003cStartu003ec__Iterator12E variable = null;
		return variable;
	}
}