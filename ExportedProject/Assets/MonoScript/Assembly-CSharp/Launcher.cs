using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

internal sealed class Launcher : MonoBehaviour
{
	public string intendedSignatureHash;

	public GameObject inAppGameObjectPrefab;

	public UnityEngine.Canvas Canvas;

	public Slider ProgressSlider;

	public Text ProgressLabel;

	public RawImage SplashScreen;

	public GameObject amazonIapManagerPrefab;

	private GameObject amazonGameCircleManager;

	private static float? _progress;

	private bool _amazonGamecircleManagerInitialized;

	private bool _amazonIapManagerInitialized;

	private bool _crossfadeFinished;

	private static bool? _usingNewLauncher;

	private string _leaderboardId = string.Empty;

	private int _targetFramerate = 30;

	internal static LicenseVerificationController.PackageInfo? PackageInfo
	{
		get;
		set;
	}

	internal static bool UsingNewLauncher
	{
		get
		{
			return (!Launcher._usingNewLauncher.HasValue ? false : Launcher._usingNewLauncher.Value);
		}
	}

	public Launcher()
	{
	}

	[DebuggerHidden]
	private IEnumerable<float> AppsMenuStartCoroutine()
	{
		Launcher.u003cAppsMenuStartCoroutineu003ec__Iterator155 variable = null;
		return variable;
	}

	private void Awake()
	{
		if (Application.platform == RuntimePlatform.Android || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
		{
			Application.targetFrameRate = 30;
		}
		this._targetFramerate = (Application.targetFrameRate != -1 ? Mathf.Clamp(Application.targetFrameRate, 30, 60) : 300);
		if (!Launcher._usingNewLauncher.HasValue)
		{
			Launcher._usingNewLauncher = new bool?(SceneLoader.ActiveSceneName.Equals("Launcher"));
		}
		if (this.ProgressLabel != null)
		{
			this.ProgressLabel.text = 0f.ToString("P0");
		}
	}

	private static string GetTerminalSceneName_3afcc97c(uint gamma)
	{
		return "ClosingScene";
	}

	private void HandleAmazonGamecircleServiceNotReady(string message)
	{
		UnityEngine.Debug.LogError(string.Concat("Amazon GameCircle service is not ready:\n", message));
	}

	private void HandleAmazonGamecircleServiceReady()
	{
		AGSClient.ServiceReadyEvent -= new Action(this.HandleAmazonGamecircleServiceReady);
		AGSClient.ServiceNotReadyEvent -= new Action<string>(this.HandleAmazonGamecircleServiceNotReady);
		UnityEngine.Debug.Log("Amazon GameCircle service is initialized.");
		AGSAchievementsClient.UpdateAchievementCompleted += new Action<AGSUpdateAchievementResponse>(this.HandleUpdateAchievementCompleted);
		AGSLeaderboardsClient.SubmitScoreSucceededEvent += new Action<string>(this.HandleAmazonSubmitScoreSucceeded);
		AGSLeaderboardsClient.SubmitScoreFailedEvent += new Action<string, string>(this.HandleAmazonSubmitScoreFailed);
		AGSLeaderboardsClient.SubmitScore(this._leaderboardId, (long)PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0), 0);
	}

	private void HandleAmazonPotentialProgressConflicts()
	{
		UnityEngine.Debug.Log("HandleAmazonPotentialProgressConflicts()");
	}

	private void HandleAmazonSubmitScoreFailed(string leaderbordId, string error)
	{
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= new Action<string>(this.HandleAmazonSubmitScoreSucceeded);
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= new Action<string, string>(this.HandleAmazonSubmitScoreFailed);
		UnityEngine.Debug.LogError(string.Format("Submit score failed for leaderboard {0}:\n{1}", leaderbordId, error));
	}

	private void HandleAmazonSubmitScoreSucceeded(string leaderbordId)
	{
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= new Action<string>(this.HandleAmazonSubmitScoreSucceeded);
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= new Action<string, string>(this.HandleAmazonSubmitScoreFailed);
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log(string.Concat("Submit score succeeded for leaderboard ", leaderbordId));
		}
	}

	private void HandleAmazonSyncFailed()
	{
		UnityEngine.Debug.LogWarning(string.Concat("HandleAmazonSyncFailed(): ", AGSWhispersyncClient.failReason));
	}

	private void HandleAmazonThrottled()
	{
		UnityEngine.Debug.LogWarning("HandleAmazonThrottled().");
	}

	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive)
	{
		UnityEngine.Debug.Log(string.Format("GameThrive HandleNotification(“{0}”, ..., {1})", message, isActive));
	}

	private void HandleUpdateAchievementCompleted(AGSUpdateAchievementResponse response)
	{
		UnityEngine.Debug.Log((!string.IsNullOrEmpty(response.error) ? string.Format("Achievement {0} failed. {1}", response.achievementId, response.error) : string.Format("Achievement {0} succeeded.", response.achievementId)));
	}

	[DebuggerHidden]
	private IEnumerable<float> InAppInstancerStartCoroutine()
	{
		Launcher.u003cInAppInstancerStartCoroutineu003ec__Iterator156 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerable<float> InitRootCoroutine()
	{
		Launcher.u003cInitRootCoroutineu003ec__Iterator154 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerable<float> LoadingProgressFadeIn()
	{
		Launcher.u003cLoadingProgressFadeInu003ec__Iterator151 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator LoadingProgressFadeInCoroutine()
	{
		Launcher.u003cLoadingProgressFadeInCoroutineu003ec__Iterator152 variable = null;
		return variable;
	}

	private static void LogMessageWithBounds(string prefix, Launcher.Bounds bounds)
	{
		UnityEngine.Debug.Log(string.Format("{0}: [{1:P0}, {2:P0}]\t\t{3}", new object[] { prefix, bounds.Lower, bounds.Upper, Time.frameCount }));
	}

	[DebuggerHidden]
	private IEnumerable<float> SplashScreenFadeOut()
	{
		Launcher.u003cSplashScreenFadeOutu003ec__Iterator150 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		Launcher.u003cStartu003ec__Iterator153 variable = null;
		return variable;
	}

	private struct Bounds
	{
		private readonly float _lower;

		private readonly float _upper;

		public float Lower
		{
			get
			{
				return this._lower;
			}
		}

		public float Upper
		{
			get
			{
				return this._upper;
			}
		}

		public Bounds(float lower, float upper)
		{
			this._lower = Mathf.Min(lower, upper);
			this._upper = Mathf.Max(lower, upper);
		}

		private float Clamp(float value)
		{
			return Mathf.Clamp(value, this._lower, this._upper);
		}

		public float Lerp(float value, float t)
		{
			return Mathf.Lerp(this.Clamp(value), this._upper, t);
		}

		public float Lerp(float t)
		{
			return this.Lerp(this._lower, t);
		}
	}
}