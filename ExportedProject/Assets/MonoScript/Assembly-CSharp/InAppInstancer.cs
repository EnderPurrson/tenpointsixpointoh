using com.amazon.mas.cpt.ads;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

[Obfuscation(Exclude=true)]
internal sealed class InAppInstancer : MonoBehaviour
{
	public GameObject inAppGameObjectPrefab;

	private bool _amazonGamecircleManagerInitialized;

	private bool _amazonIapManagerInitialized;

	private string _leaderboardId = string.Empty;

	public InAppInstancer()
	{
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
		AGSLeaderboardsClient.SubmitScoreSucceededEvent += new Action<string>(this.HandleSubmitScoreSucceeded);
		AGSLeaderboardsClient.SubmitScoreFailedEvent += new Action<string, string>(this.HandleSubmitScoreFailed);
		AGSLeaderboardsClient.SubmitScore(this._leaderboardId, (long)PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0), 0);
	}

	private void HandleAmazonPotentialProgressConflicts()
	{
		UnityEngine.Debug.Log("HandleAmazonPotentialProgressConflicts()");
	}

	private void HandleAmazonSyncFailed()
	{
		UnityEngine.Debug.LogWarning(string.Concat("HandleAmazonSyncFailed(): ", AGSWhispersyncClient.failReason));
	}

	private void HandleAmazonThrottled()
	{
		UnityEngine.Debug.LogWarning("HandleAmazonThrottled().");
	}

	private void HandleSubmitScoreFailed(string leaderbordId, string error)
	{
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= new Action<string>(this.HandleSubmitScoreSucceeded);
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= new Action<string, string>(this.HandleSubmitScoreFailed);
		UnityEngine.Debug.LogError(string.Format("Submit score failed for leaderboard {0}:\n{1}", leaderbordId, error));
	}

	private void HandleSubmitScoreSucceeded(string leaderbordId)
	{
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= new Action<string>(this.HandleSubmitScoreSucceeded);
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= new Action<string, string>(this.HandleSubmitScoreFailed);
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log(string.Concat("Submit score succeeded for leaderboard ", leaderbordId));
		}
	}

	private void HandleUpdateAchievementCompleted(AGSUpdateAchievementResponse response)
	{
		UnityEngine.Debug.Log((!string.IsNullOrEmpty(response.error) ? string.Format("Achievement {0} failed. {1}", response.achievementId, response.error) : string.Format("Achievement {0} succeeded.", response.achievementId)));
	}

	[DebuggerHidden]
	private IEnumerator InitializeAmazonGamecircleManager()
	{
		InAppInstancer.u003cInitializeAmazonGamecircleManageru003ec__Iterator89 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		InAppInstancer.u003cStartu003ec__Iterator88 variable = null;
		return variable;
	}
}