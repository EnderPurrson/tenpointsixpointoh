using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestLeaderboardsResponse : AGSRequestResponse
{
	public List<AGSLeaderboard> leaderboards;

	public AGSRequestLeaderboardsResponse()
	{
	}

	public static AGSRequestLeaderboardsResponse FromJSON(string json)
	{
		AGSRequestLeaderboardsResponse blankResponseWithError;
		try
		{
			AGSRequestLeaderboardsResponse aGSRequestLeaderboardsResponse = new AGSRequestLeaderboardsResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestLeaderboardsResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestLeaderboardsResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestLeaderboardsResponse.leaderboards = new List<AGSLeaderboard>();
			if (hashtables.ContainsKey("leaderboards"))
			{
				IEnumerator enumerator = (hashtables["leaderboards"] as ArrayList).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Hashtable current = (Hashtable)enumerator.Current;
						aGSRequestLeaderboardsResponse.leaderboards.Add(AGSLeaderboard.fromHashtable(current));
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
			blankResponseWithError = aGSRequestLeaderboardsResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestLeaderboardsResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestLeaderboardsResponse GetBlankResponseWithError(string error, int userData = 0)
	{
		AGSRequestLeaderboardsResponse aGSRequestLeaderboardsResponse = new AGSRequestLeaderboardsResponse()
		{
			error = error,
			userData = userData,
			leaderboards = new List<AGSLeaderboard>()
		};
		return aGSRequestLeaderboardsResponse;
	}

	public static AGSRequestLeaderboardsResponse GetPlatformNotSupportedResponse(int userData)
	{
		return AGSRequestLeaderboardsResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", userData);
	}
}