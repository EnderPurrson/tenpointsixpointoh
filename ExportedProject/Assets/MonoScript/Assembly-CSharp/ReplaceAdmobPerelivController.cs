using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class ReplaceAdmobPerelivController : MonoBehaviour
{
	public static ReplaceAdmobPerelivController sharedController;

	private Texture2D _image;

	private string _adUrl;

	private static int _timesWantToShow;

	private static int _timesShown;

	private long _timeSuspended;

	public string AdUrl
	{
		get
		{
			return this._adUrl;
		}
	}

	public bool DataLoaded
	{
		get
		{
			return (this._image == null ? false : this._adUrl != null);
		}
	}

	public bool DataLoading
	{
		get;
		private set;
	}

	public Texture2D Image
	{
		get
		{
			return this._image;
		}
	}

	private static bool LimitReached
	{
		get
		{
			if (PromoActionsManager.ReplaceAdmobPereliv == null)
			{
				return true;
			}
			if (PromoActionsManager.ReplaceAdmobPereliv.ShowTimesTotal == 0)
			{
				return false;
			}
			return ReplaceAdmobPerelivController._timesShown >= PromoActionsManager.ReplaceAdmobPereliv.ShowTimesTotal;
		}
	}

	public static bool ShouldShowAtThisTime
	{
		get
		{
			return (PromoActionsManager.ReplaceAdmobPereliv == null ? false : ReplaceAdmobPerelivController._timesWantToShow % (PromoActionsManager.ReplaceAdmobPereliv.ShowEveryTimes == 0 ? 1 : PromoActionsManager.ReplaceAdmobPereliv.ShowEveryTimes) == 0);
		}
	}

	public bool ShouldShowInLobby
	{
		get;
		set;
	}

	static ReplaceAdmobPerelivController()
	{
		ReplaceAdmobPerelivController._timesWantToShow = -1;
	}

	public ReplaceAdmobPerelivController()
	{
	}

	private void Awake()
	{
		ReplaceAdmobPerelivController.sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void DestroyImage()
	{
		if (this.Image != null)
		{
			this._image = null;
		}
	}

	private string GetImageURLForOurQuality(string urlString)
	{
		string empty = string.Empty;
		if (Screen.height >= 500)
		{
			empty = "-Medium";
		}
		if (Screen.height >= 900)
		{
			empty = "-Hi";
		}
		urlString = urlString.Insert(urlString.LastIndexOf("."), empty);
		return urlString;
	}

	public static void IncreaseTimesCounter()
	{
		ReplaceAdmobPerelivController._timesWantToShow++;
	}

	[DebuggerHidden]
	private IEnumerator LoadDataCoroutine(int index)
	{
		ReplaceAdmobPerelivController.u003cLoadDataCoroutineu003ec__Iterator1BD variable = null;
		return variable;
	}

	public void LoadPerelivData()
	{
		if (this.DataLoading)
		{
			UnityEngine.Debug.LogWarning("ReplaceAdmobPerelivController: data is already loading. returning...");
			return;
		}
		if (this._image != null)
		{
			UnityEngine.Object.Destroy(this._image);
		}
		this._image = null;
		this._adUrl = null;
		int num = 0;
		if (PromoActionsManager.ReplaceAdmobPereliv.imageUrls.Count <= 0)
		{
			UnityEngine.Debug.LogWarning("ReplaceAdmobPerelivController:PromoActionsManager.ReplaceAdmobPereliv.imageUrls.Count = 0. returning...");
			return;
		}
		num = UnityEngine.Random.Range(0, PromoActionsManager.ReplaceAdmobPereliv.imageUrls.Count);
		base.StartCoroutine(this.LoadDataCoroutine(num));
	}

	private void OnApplicationPause(bool pausing)
	{
		if (pausing)
		{
			this._timeSuspended = PromoActionsManager.CurrentUnixTime;
		}
		else if (PromoActionsManager.CurrentUnixTime - this._timeSuspended > (long)3600)
		{
			ReplaceAdmobPerelivController._timesShown = 0;
		}
	}

	public static bool ReplaceAdmobWithPerelivApplicable()
	{
		bool flag;
		if (PromoActionsManager.ReplaceAdmobPereliv == null)
		{
			return false;
		}
		if (!(ExperienceController.sharedController != null) || PromoActionsManager.ReplaceAdmobPereliv.MinLevel != -1 && PromoActionsManager.ReplaceAdmobPereliv.MinLevel > ExperienceController.sharedController.currentLevel)
		{
			flag = false;
		}
		else
		{
			flag = (PromoActionsManager.ReplaceAdmobPereliv.MaxLevel == -1 ? true : PromoActionsManager.ReplaceAdmobPereliv.MaxLevel >= ExperienceController.sharedController.currentLevel);
		}
		bool flag1 = flag;
		bool showToPaying = PromoActionsManager.ReplaceAdmobPereliv.ShowToPaying;
		bool showToNew = PromoActionsManager.ReplaceAdmobPereliv.ShowToNew;
		bool flag2 = MobileAdManager.UserPredicate(MobileAdManager.Type.Image, Defs.IsDeveloperBuild, showToPaying, showToNew);
		if (UnityEngine.Debug.isDebugBuild)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>(7)
			{
				{ "MinLevel", PromoActionsManager.ReplaceAdmobPereliv.MinLevel },
				{ "MaxLevel", PromoActionsManager.ReplaceAdmobPereliv.MaxLevel },
				{ "levelConstraintIsOk", flag1 },
				{ "LimitReached", ReplaceAdmobPerelivController.LimitReached },
				{ "adIsApplicable", flag2 },
				{ "PromoActionsManager.ReplaceAdmobPereliv.enabled", PromoActionsManager.ReplaceAdmobPereliv.enabled }
			};
			string str = string.Format("ReplaceAdmobWithPerelivApplicable Details: {0}", Json.Serialize(strs));
			UnityEngine.Debug.Log(str);
		}
		return (!flag2 || !PromoActionsManager.ReplaceAdmobPereliv.enabled || ReplaceAdmobPerelivController.LimitReached ? false : flag1);
	}

	public static void TryShowPereliv(string context)
	{
		if (ReplaceAdmobPerelivController.sharedController != null && ReplaceAdmobPerelivController.sharedController.Image != null && ReplaceAdmobPerelivController.sharedController.AdUrl != null)
		{
			AdmobPerelivWindow.admobTexture = ReplaceAdmobPerelivController.sharedController.Image;
			AdmobPerelivWindow.admobUrl = ReplaceAdmobPerelivController.sharedController.AdUrl;
			AdmobPerelivWindow.Context = context;
			PlayerPrefs.SetString(Defs.LastTimeShowBanerKey, DateTime.UtcNow.ToString("s"));
			FlurryPluginWrapper.LogEventAndDublicateToConsole("Replace Admob With Pereliv Show", FlurryPluginWrapper.LevelAndTierParameters, true);
			ReplaceAdmobPerelivController._timesShown++;
		}
	}
}