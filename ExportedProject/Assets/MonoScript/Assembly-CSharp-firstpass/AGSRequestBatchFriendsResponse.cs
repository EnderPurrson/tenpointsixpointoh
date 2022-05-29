using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestBatchFriendsResponse : AGSRequestResponse
{
	public List<AGSPlayer> friends;

	public AGSRequestBatchFriendsResponse()
	{
	}

	public static AGSRequestBatchFriendsResponse FromJSON(string json)
	{
		AGSRequestBatchFriendsResponse blankResponseWithError;
		try
		{
			AGSRequestBatchFriendsResponse aGSRequestBatchFriendsResponse = new AGSRequestBatchFriendsResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestBatchFriendsResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestBatchFriendsResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestBatchFriendsResponse.friends = new List<AGSPlayer>();
			if (hashtables.ContainsKey("friends"))
			{
				IEnumerator enumerator = (hashtables["friends"] as ArrayList).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Hashtable current = (Hashtable)enumerator.Current;
						aGSRequestBatchFriendsResponse.friends.Add(AGSPlayer.fromHashtable(current));
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
			blankResponseWithError = aGSRequestBatchFriendsResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestBatchFriendsResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", null, 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestBatchFriendsResponse GetBlankResponseWithError(string error, List<string> friendIdsRequested = null, int userData = 0)
	{
		AGSRequestBatchFriendsResponse aGSRequestBatchFriendsResponse = new AGSRequestBatchFriendsResponse()
		{
			error = error,
			friends = new List<AGSPlayer>()
		};
		if (friendIdsRequested != null)
		{
			foreach (string str in friendIdsRequested)
			{
				aGSRequestBatchFriendsResponse.friends.Add(AGSPlayer.BlankPlayerWithID(str));
			}
		}
		aGSRequestBatchFriendsResponse.userData = userData;
		return aGSRequestBatchFriendsResponse;
	}

	public static AGSRequestBatchFriendsResponse GetPlatformNotSupportedResponse(List<string> friendIdsRequested, int userData)
	{
		return AGSRequestBatchFriendsResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", friendIdsRequested, userData);
	}
}