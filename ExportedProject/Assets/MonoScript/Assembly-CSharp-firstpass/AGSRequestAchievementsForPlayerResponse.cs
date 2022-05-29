using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestAchievementsForPlayerResponse : AGSRequestAchievementsResponse
{
	public string playerId;

	public AGSRequestAchievementsForPlayerResponse()
	{
	}

	public static new AGSRequestAchievementsForPlayerResponse FromJSON(string json)
	{
		AGSRequestAchievementsForPlayerResponse blankResponseWithError;
		try
		{
			AGSRequestAchievementsForPlayerResponse aGSRequestAchievementsForPlayerResponse = new AGSRequestAchievementsForPlayerResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestAchievementsForPlayerResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestAchievementsForPlayerResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestAchievementsForPlayerResponse.achievements = new List<AGSAchievement>();
			if (hashtables.ContainsKey("achievements"))
			{
				IEnumerator enumerator = (hashtables["achievements"] as ArrayList).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Hashtable current = (Hashtable)enumerator.Current;
						aGSRequestAchievementsForPlayerResponse.achievements.Add(AGSAchievement.fromHashtable(current));
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
			aGSRequestAchievementsForPlayerResponse.playerId = (!hashtables.ContainsKey("playerId") ? string.Empty : hashtables["playerId"].ToString());
			blankResponseWithError = aGSRequestAchievementsForPlayerResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestAchievementsForPlayerResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", string.Empty, 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestAchievementsForPlayerResponse GetBlankResponseWithError(string error, string playerId = "", int userData = 0)
	{
		AGSRequestAchievementsForPlayerResponse aGSRequestAchievementsForPlayerResponse = new AGSRequestAchievementsForPlayerResponse()
		{
			error = error,
			playerId = playerId,
			userData = userData,
			achievements = new List<AGSAchievement>()
		};
		return aGSRequestAchievementsForPlayerResponse;
	}

	public static AGSRequestAchievementsForPlayerResponse GetPlatformNotSupportedResponse(string playerId, int userData)
	{
		return AGSRequestAchievementsForPlayerResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", playerId, userData);
	}
}