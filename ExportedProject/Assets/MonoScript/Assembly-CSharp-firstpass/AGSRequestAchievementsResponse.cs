using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestAchievementsResponse : AGSRequestResponse
{
	public List<AGSAchievement> achievements;

	public AGSRequestAchievementsResponse()
	{
	}

	public static AGSRequestAchievementsResponse FromJSON(string json)
	{
		AGSRequestAchievementsResponse blankResponseWithError;
		try
		{
			AGSRequestAchievementsResponse aGSRequestAchievementsResponse = new AGSRequestAchievementsResponse();
			Hashtable hashtables = json.hashtableFromJson();
			aGSRequestAchievementsResponse.error = (!hashtables.ContainsKey("error") ? string.Empty : hashtables["error"].ToString());
			aGSRequestAchievementsResponse.userData = (!hashtables.ContainsKey("userData") ? 0 : int.Parse(hashtables["userData"].ToString()));
			aGSRequestAchievementsResponse.achievements = new List<AGSAchievement>();
			if (hashtables.ContainsKey("achievements"))
			{
				IEnumerator enumerator = (hashtables["achievements"] as ArrayList).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Hashtable current = (Hashtable)enumerator.Current;
						aGSRequestAchievementsResponse.achievements.Add(AGSAchievement.fromHashtable(current));
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
			blankResponseWithError = aGSRequestAchievementsResponse;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(exception.ToString());
			blankResponseWithError = AGSRequestAchievementsResponse.GetBlankResponseWithError("ERROR_PARSING_JSON", 0);
		}
		return blankResponseWithError;
	}

	public static AGSRequestAchievementsResponse GetBlankResponseWithError(string error, int userData = 0)
	{
		AGSRequestAchievementsResponse aGSRequestAchievementsResponse = new AGSRequestAchievementsResponse()
		{
			error = error,
			userData = userData,
			achievements = new List<AGSAchievement>()
		};
		return aGSRequestAchievementsResponse;
	}

	public static AGSRequestAchievementsResponse GetPlatformNotSupportedResponse(int userData)
	{
		return AGSRequestAchievementsResponse.GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", userData);
	}
}