using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AGSPlayerClient : MonoBehaviour
{
	private static AmazonJavaWrapper JavaObject;

	private readonly static string PROXY_CLASS_NAME;

	static AGSPlayerClient()
	{
		AGSPlayerClient.PROXY_CLASS_NAME = "com.amazon.ags.api.unity.PlayerClientProxyImpl";
		AGSPlayerClient.JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSPlayerClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() != IntPtr.Zero)
			{
				AGSPlayerClient.JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
			else
			{
				AGSClient.LogGameCircleWarning(string.Concat("No java class ", AGSPlayerClient.PROXY_CLASS_NAME, " present, can't use AGSPlayerClient"));
			}
		}
	}

	public AGSPlayerClient()
	{
	}

	public static void BatchFriendsRequestComplete(string json)
	{
		if (AGSPlayerClient.RequestBatchFriendsCompleted != null)
		{
			AGSPlayerClient.RequestBatchFriendsCompleted(AGSRequestBatchFriendsResponse.FromJSON(json));
		}
	}

	public static bool IsSignedIn()
	{
		return AGSPlayerClient.JavaObject.Call<bool>("isSignedIn", new object[0]);
	}

	public static void LocalPlayerFriendsComplete(string json)
	{
		if (AGSPlayerClient.RequestFriendIdsCompleted != null)
		{
			AGSPlayerClient.RequestFriendIdsCompleted(AGSRequestFriendIdsResponse.FromJSON(json));
		}
	}

	public static void OnSignedInStateChanged(bool isSignedIn)
	{
		if (AGSPlayerClient.OnSignedInStateChangedEvent != null)
		{
			AGSPlayerClient.OnSignedInStateChangedEvent(isSignedIn);
		}
	}

	public static void PlayerFailed(string json)
	{
		AGSRequestPlayerResponse aGSRequestPlayerResponse = AGSRequestPlayerResponse.FromJSON(json);
		if (aGSRequestPlayerResponse.IsError() && AGSPlayerClient.PlayerFailedEvent != null)
		{
			AGSPlayerClient.PlayerFailedEvent(aGSRequestPlayerResponse.error);
		}
		if (AGSPlayerClient.RequestLocalPlayerCompleted != null)
		{
			AGSPlayerClient.RequestLocalPlayerCompleted(aGSRequestPlayerResponse);
		}
	}

	public static void PlayerReceived(string json)
	{
		AGSRequestPlayerResponse aGSRequestPlayerResponse = AGSRequestPlayerResponse.FromJSON(json);
		if (!aGSRequestPlayerResponse.IsError() && AGSPlayerClient.PlayerReceivedEvent != null)
		{
			AGSPlayerClient.PlayerReceivedEvent(aGSRequestPlayerResponse.player);
		}
		if (AGSPlayerClient.RequestLocalPlayerCompleted != null)
		{
			AGSPlayerClient.RequestLocalPlayerCompleted(aGSRequestPlayerResponse);
		}
	}

	public static void RequestBatchFriends(List<string> friendIds, int userData = 0)
	{
		string str = MiniJSON.jsonEncode(friendIds.ToArray());
		AGSPlayerClient.JavaObject.Call("requestBatchFriends", new object[] { str, userData });
	}

	public static void RequestFriendIds(int userData = 0)
	{
		AGSPlayerClient.JavaObject.Call("requestLocalPlayerFriends", new object[] { userData });
	}

	public static void RequestLocalPlayer(int userData = 0)
	{
		AGSPlayerClient.JavaObject.Call("requestLocalPlayer", new object[] { userData });
	}

	public static event Action<bool> OnSignedInStateChangedEvent;

	[Obsolete("PlayerFailedEvent is deprecated. Use RequestLocalPlayerCompleted instead.")]
	public static event Action<string> PlayerFailedEvent;

	[Obsolete("PlayerReceivedEvent is deprecated. Use RequestLocalPlayerCompleted instead.")]
	public static event Action<AGSPlayer> PlayerReceivedEvent;

	public static event Action<AGSRequestBatchFriendsResponse> RequestBatchFriendsCompleted;

	public static event Action<AGSRequestFriendIdsResponse> RequestFriendIdsCompleted;

	public static event Action<AGSRequestPlayerResponse> RequestLocalPlayerCompleted;
}