using System;
using System.Runtime.CompilerServices;
using UnityEngine.SocialPlatforms;

public class AGSSocialLeaderboardScore : IScore
{
	private readonly AGSScore score;

	public DateTime date
	{
		get
		{
			AGSClient.LogGameCircleError("IScore.date.get is not available for GameCircle");
			return DateTime.MinValue;
		}
	}

	public string formattedValue
	{
		get
		{
			if (this.score == null)
			{
				return null;
			}
			return this.score.scoreString;
		}
	}

	public string leaderboardID
	{
		get;
		set;
	}

	public int rank
	{
		get
		{
			if (this.score == null)
			{
				return 0;
			}
			return this.score.rank;
		}
	}

	public string userID
	{
		get
		{
			if (this.score == null)
			{
				return null;
			}
			return this.score.player.@alias;
		}
	}

	public long @value
	{
		get;
		set;
	}

	public AGSSocialLeaderboardScore(AGSScore score, AGSLeaderboard leaderboard)
	{
		if (score == null)
		{
			AGSClient.LogGameCircleError("AGSSocialLeaderboardScore constructor \"score\" argument should not be null");
			return;
		}
		if (leaderboard == null)
		{
			AGSClient.LogGameCircleError("AGSSocialLeaderboardScore constructor \"leaderboard\" argument should not be null");
			return;
		}
		this.score = score;
		this.leaderboardID = leaderboard.id;
		this.@value = score.scoreValue;
	}

	public AGSSocialLeaderboardScore()
	{
		this.score = null;
		this.leaderboardID = null;
	}

	public void ReportScore(Action<bool> callback)
	{
		GameCircleSocial.Instance.ReportScore(this.@value, this.leaderboardID, callback);
		AGSLeaderboardsClient.SubmitScore(this.leaderboardID, this.@value, 0);
	}
}