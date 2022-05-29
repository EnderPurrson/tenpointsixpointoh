using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestFriendIdsResponse : AGSRequestResponse
{
	public List<string> friendIds;

	public AGSRequestFriendIdsResponse()
	{
	}

	public static AGSRequestFriendIdsResponse FromJSON(string json)
	{
		AGSRequestFriendIdsResponse blankResponseWithError;
		try
		{
			AGSRequestFriendIdsResponse aGSRequestFriendIdsResponse = new AGSRequestFriendIdsResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestFriendIdsResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestFriendIdsResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestFriendIdsResponse.friendIds = new List<string>();
			if (hashtables.ContainsKey("friendIds"))
			{
				IEnumerator enumerator = (hashtables["friendIds"] as ArrayList).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current = (string)enumerator.Current;
						aGSRequestFriendIdsResponse.friendIds.Add(current);
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
			blankResponseWithError = aGSRequestFriendIdsResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestFriendIdsResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestFriendIdsResponse GetBlankResponseWithError(string error, int userData = 0)
	{
		AGSRequestFriendIdsResponse aGSRequestFriendIdsResponse = new AGSRequestFriendIdsResponse()
		{
			error = error,
			userData = userData,
			friendIds = new List<string>()
		};
		return aGSRequestFriendIdsResponse;
	}

	public static AGSRequestFriendIdsResponse GetPlatformNotSupportedResponse(int userData)
	{
		return AGSRequestFriendIdsResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", userData);
	}
}