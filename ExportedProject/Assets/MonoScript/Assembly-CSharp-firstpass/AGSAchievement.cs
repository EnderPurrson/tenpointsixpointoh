using System;
using System.Collections;

public class AGSAchievement
{
	public string title;

	public string id;

	public string description;

	public float progress;

	public int pointValue;

	public bool isHidden;

	public bool isUnlocked;

	public int position;

	public DateTime dateUnlocked;

	public AGSAchievement()
	{
	}

	public static AGSAchievement fromHashtable(Hashtable hashtable)
	{
		AGSAchievement blankAchievement;
		try
		{
			AGSAchievement aGSAchievement = new AGSAchievement()
			{
				title = hashtable["achievementTitle"].ToString(),
				id = hashtable["achievementId"].ToString(),
				description = hashtable["achievementDescription"].ToString(),
				progress = float.Parse(hashtable["achievementProgress"].ToString()),
				pointValue = int.Parse(hashtable["achievementPointValue"].ToString()),
				position = int.Parse(hashtable["achievementPosition"].ToString()),
				isUnlocked = bool.Parse(hashtable["achievementUnlocked"].ToString()),
				isHidden = bool.Parse(hashtable["achievementHidden"].ToString()),
				dateUnlocked = AGSAchievement.getTimefromEpochTime(long.Parse(hashtable["achievementDateUnlocked"].ToString()))
			};
			blankAchievement = aGSAchievement;
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(string.Concat("Returning blank achievement due to exception getting achievement from hashtable: ", exception.ToString()));
			blankAchievement = AGSAchievement.GetBlankAchievement();
		}
		return blankAchievement;
	}

	public static AGSAchievement GetBlankAchievement()
	{
		AGSAchievement aGSAchievement = new AGSAchievement()
		{
			title = string.Empty,
			id = string.Empty,
			description = string.Empty,
			pointValue = 0,
			isHidden = false,
			isUnlocked = false,
			progress = 0f,
			position = 0,
			dateUnlocked = DateTime.MinValue
		};
		return aGSAchievement;
	}

	private static DateTime getTimefromEpochTime(long javaTimeStamp)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return dateTime.AddMilliseconds((double)javaTimeStamp).ToLocalTime();
	}

	public override string ToString()
	{
		return string.Format("title: {0}, id: {1}, pointValue: {2}, hidden: {3}, unlocked: {4}, progress: {5}, position: {6}, date: {7} ", new object[] { this.title, this.id, this.pointValue, this.isHidden, this.isUnlocked, this.progress, this.position, this.dateUnlocked });
	}
}