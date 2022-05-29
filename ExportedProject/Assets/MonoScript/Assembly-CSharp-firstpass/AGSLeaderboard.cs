using System;
using System.Collections;

public class AGSLeaderboard
{
	public string name;

	public string id;

	public string displayText;

	public string scoreFormat;

	public string imageUrl;

	public AGSLeaderboard()
	{
	}

	public static AGSLeaderboard fromHashtable(Hashtable hashtable)
	{
		AGSLeaderboard aGSLeaderboard = new AGSLeaderboard()
		{
			name = hashtable["leaderboardName"].ToString(),
			id = hashtable["leaderboardId"].ToString(),
			displayText = hashtable["leaderboardDisplayText"].ToString(),
			scoreFormat = hashtable["leaderboardScoreFormat"].ToString(),
			imageUrl = hashtable["leaderboardImageUrl"].ToString()
		};
		return aGSLeaderboard;
	}

	public static AGSLeaderboard GetBlankLeaderboard()
	{
		AGSLeaderboard aGSLeaderboard = new AGSLeaderboard()
		{
			name = string.Empty,
			id = string.Empty,
			displayText = string.Empty,
			scoreFormat = string.Empty,
			imageUrl = string.Empty
		};
		return aGSLeaderboard;
	}

	public override string ToString()
	{
		return string.Format("name: {0}, id: {1}, displayText: {2}, scoreFormat: {3}, imageUrl: {4}", new object[] { this.name, this.id, this.displayText, this.scoreFormat, this.imageUrl });
	}
}