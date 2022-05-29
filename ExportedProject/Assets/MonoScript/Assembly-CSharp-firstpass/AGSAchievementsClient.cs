using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AGSAchievementsClient : MonoBehaviour
{
	private static AmazonJavaWrapper JavaObject;

	private readonly static string PROXY_CLASS_NAME;

	static AGSAchievementsClient()
	{
		AGSAchievementsClient.PROXY_CLASS_NAME = "com.amazon.ags.api.unity.AchievementsClientProxyImpl";
		AGSAchievementsClient.JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSAchievementsClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() != IntPtr.Zero)
			{
				AGSAchievementsClient.JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
			else
			{
				AGSClient.LogGameCircleWarning(string.Format("No java class {0} present, can't use AGSAchievementsClient", AGSAchievementsClient.PROXY_CLASS_NAME));
			}
		}
	}

	public AGSAchievementsClient()
	{
	}

	public static void RequestAchievements(int userData = 0)
	{
		AGSAchievementsClient.JavaObject.Call("requestAchievements", new object[] { userData });
	}

	public static void RequestAchievementsFailed(string json)
	{
		AGSRequestAchievementsResponse aGSRequestAchievementsResponse = AGSRequestAchievementsResponse.FromJSON(json);
		if (aGSRequestAchievementsResponse.IsError() && AGSAchievementsClient.RequestAchievementsFailedEvent != null)
		{
			AGSAchievementsClient.RequestAchievementsFailedEvent(aGSRequestAchievementsResponse.error);
		}
		if (AGSAchievementsClient.RequestAchievementsCompleted != null)
		{
			AGSAchievementsClient.RequestAchievementsCompleted(aGSRequestAchievementsResponse);
		}
	}

	public static void RequestAchievementsForPlayer(string playerId, int userData = 0)
	{
		AGSAchievementsClient.JavaObject.Call("requestAchievementsForPlayer", new object[] { playerId, userData });
	}

	public static void RequestAchievementsForPlayerComplete(string json)
	{
		if (AGSAchievementsClient.RequestAchievementsForPlayerCompleted != null)
		{
			AGSAchievementsClient.RequestAchievementsForPlayerCompleted(AGSRequestAchievementsForPlayerResponse.FromJSON(json));
		}
	}

	public static void RequestAchievementsSucceeded(string json)
	{
		AGSRequestAchievementsResponse aGSRequestAchievementsResponse = AGSRequestAchievementsResponse.FromJSON(json);
		if (!aGSRequestAchievementsResponse.IsError() && AGSAchievementsClient.RequestAchievementsSucceededEvent != null)
		{
			AGSAchievementsClient.RequestAchievementsSucceededEvent(aGSRequestAchievementsResponse.achievements);
		}
		if (AGSAchievementsClient.RequestAchievementsCompleted != null)
		{
			AGSAchievementsClient.RequestAchievementsCompleted(aGSRequestAchievementsResponse);
		}
	}

	public static void ShowAchievementsOverlay()
	{
		AGSAchievementsClient.JavaObject.Call("showAchievementsOverlay", new object[0]);
	}

	public static void UpdateAchievementFailed(string json)
	{
		AGSUpdateAchievementResponse aGSUpdateAchievementResponse = AGSUpdateAchievementResponse.FromJSON(json);
		if (aGSUpdateAchievementResponse.IsError() && AGSAchievementsClient.UpdateAchievementFailedEvent != null)
		{
			AGSAchievementsClient.UpdateAchievementFailedEvent(aGSUpdateAchievementResponse.achievementId, aGSUpdateAchievementResponse.error);
		}
		if (AGSAchievementsClient.UpdateAchievementCompleted != null)
		{
			AGSAchievementsClient.UpdateAchievementCompleted(aGSUpdateAchievementResponse);
		}
	}

	public static void UpdateAchievementProgress(string achievementId, float progress, int userData = 0)
	{
		AGSAchievementsClient.JavaObject.Call("updateAchievementProgress", new object[] { achievementId, progress, userData });
	}

	public static void UpdateAchievementSucceeded(string json)
	{
		AGSUpdateAchievementResponse aGSUpdateAchievementResponse = AGSUpdateAchievementResponse.FromJSON(json);
		if (!aGSUpdateAchievementResponse.IsError() && AGSAchievementsClient.UpdateAchievementSucceededEvent != null)
		{
			AGSAchievementsClient.UpdateAchievementSucceededEvent(aGSUpdateAchievementResponse.achievementId);
		}
		if (AGSAchievementsClient.UpdateAchievementCompleted != null)
		{
			AGSAchievementsClient.UpdateAchievementCompleted(aGSUpdateAchievementResponse);
		}
	}

	public static event Action<AGSRequestAchievementsResponse> RequestAchievementsCompleted;

	[Obsolete("RequestAchievementsFailedEvent is deprecated. Use RequestAchievementsCompleted instead.")]
	public static event Action<string> RequestAchievementsFailedEvent;

	public static event Action<AGSRequestAchievementsForPlayerResponse> RequestAchievementsForPlayerCompleted;

	[Obsolete("RequestAchievementsSucceededEvent is deprecated. Use RequestAchievementsCompleted instead.")]
	public static event Action<List<AGSAchievement>> RequestAchievementsSucceededEvent;

	public static event Action<AGSUpdateAchievementResponse> UpdateAchievementCompleted;

	[Obsolete("UpdateAchievementFailedEvent is deprecated. Use UpdateAchievementCompleted instead.")]
	public static event Action<string, string> UpdateAchievementFailedEvent;

	[Obsolete("UpdateAchievementSucceededEvent is deprecated. Use UpdateAchievementCompleted instead.")]
	public static event Action<string> UpdateAchievementSucceededEvent;
}