using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class LevelArt : MonoBehaviour
{
	private const int ComicsOnScreen = 4;

	public readonly static bool ShouldShowArts;

	public GUIStyle startButton;

	public static bool endOfBox;

	public GUIStyle labelsStyle;

	public float widthBackLabel = 770f;

	public float heightBackLabel = 100f;

	private float _alphaForComics;

	private int _currentComicsImageIndex;

	private bool _isFirstLaunch = true;

	public float _delayShowComics = 3f;

	private bool _isSkipComics;

	private int _countOfComics = 4;

	private Texture _backgroundComics;

	private List<Texture> _comicsTextures = new List<Texture>();

	private bool _isShowButton;

	private string _currentSubtitle;

	private bool _needShowSubtitle;

	static LevelArt()
	{
		LevelArt.ShouldShowArts = true;
	}

	public LevelArt()
	{
	}

	private string _NameForNumber(int num)
	{
		return ResPath.Combine("Arts", ResPath.Combine((!LevelArt.endOfBox ? CurrentCampaignGame.levelSceneName : CurrentCampaignGame.boXName), num.ToString()));
	}

	private void GoToLevel()
	{
		if (!LevelArt.endOfBox)
		{
			string[] strArrays = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			if (!LevelArt.endOfBox && Array.IndexOf<string>(strArrays, CurrentCampaignGame.levelSceneName) == -1)
			{
				List<string> strs = new List<string>();
				string[] strArrays1 = strArrays;
				for (int i = 0; i < (int)strArrays1.Length; i++)
				{
					strs.Add(strArrays1[i]);
				}
				strs.Add(CurrentCampaignGame.levelSceneName);
				Save.SaveStringArray(Defs.ArtLevsS, strs.ToArray());
			}
		}
		else
		{
			string[] strArrays2 = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			if (Array.IndexOf<string>(strArrays2, CurrentCampaignGame.boXName) == -1)
			{
				List<string> strs1 = new List<string>();
				string[] strArrays3 = strArrays2;
				for (int j = 0; j < (int)strArrays3.Length; j++)
				{
					strs1.Add(strArrays3[j]);
				}
				strs1.Add(CurrentCampaignGame.boXName);
				Save.SaveStringArray(Defs.ArtBoxS, strs1.ToArray());
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.endOfBox ? "CampaignLoading" : "ChooseLevel"), LoadSceneMode.Single);
	}

	[Obsolete("Use ComicsCampaign via uGUI instead of this class.")]
	private void OnGUI()
	{
	}

	[DebuggerHidden]
	[Obfuscation(Exclude=true)]
	private IEnumerator ShowArts()
	{
		LevelArt.u003cShowArtsu003ec__IteratorA4 variable = null;
		return variable;
	}

	[Obsolete("Use ComicsCampaign via uGUI instead of this class.")]
	private void Start()
	{
		this._needShowSubtitle = LocalizationStore.CurrentLanguage != "English";
		this.labelsStyle.font = LocalizationStore.GetFontByLocalize("Key_04B_03");
		this.labelsStyle.fontSize = Mathf.RoundToInt(20f * Defs.Coef);
		if (Resources.Load<Texture>(this._NameForNumber(5)) != null)
		{
			this._countOfComics *= 2;
		}
		base.StartCoroutine("ShowArts");
		this._backgroundComics = Resources.Load<Texture>(string.Concat("Arts_background_", CurrentCampaignGame.boXName));
		if (!LevelArt.endOfBox)
		{
			string[] strArrays = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			int num = 0;
			while (num < (int)strArrays.Length)
			{
				if (!strArrays[num].Equals(CurrentCampaignGame.levelSceneName))
				{
					num++;
				}
				else
				{
					this._isFirstLaunch = false;
					break;
				}
			}
		}
		else
		{
			string[] strArrays1 = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			int num1 = 0;
			while (num1 < (int)strArrays1.Length)
			{
				if (!strArrays1[num1].Equals(CurrentCampaignGame.boXName))
				{
					num1++;
				}
				else
				{
					this._isFirstLaunch = false;
					break;
				}
			}
		}
		this._isShowButton = !this._isFirstLaunch;
	}

	public static string WrappedText(string text)
	{
		int num = 30;
		StringBuilder stringBuilder = new StringBuilder();
		int num1 = 0;
		int num2 = 0;
		while (num1 < text.Length)
		{
			stringBuilder.Append(text[num1]);
			if (text[num1] == '\n')
			{
				stringBuilder.Append('\n');
			}
			if (num2 >= num && text[num1] == ' ')
			{
				stringBuilder.Append("\n\n");
				num2 = 0;
			}
			num1++;
			num2++;
		}
		return stringBuilder.ToString();
	}
}