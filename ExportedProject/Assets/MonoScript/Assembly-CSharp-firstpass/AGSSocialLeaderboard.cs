using System;
using System.Runtime.CompilerServices;
using UnityEngine.SocialPlatforms;

public class AGSSocialLeaderboard : ILeaderboard
{
	private readonly AGSLeaderboard leaderboard;

	public long localPlayerScore;

	public int localPlayerRank;

	private TimeScope _timeScope;

	public string id
	{
		get;
		set;
	}

	public bool loading
	{
		get
		{
			return !this.ScoresAvailable();
		}
	}

	public IScore localUserScore
	{
		get
		{
			AGSScore aGSScore = new AGSScore()
			{
				player = AGSSocialLocalUser.player,
				scoreValue = this.localPlayerScore,
				scoreString = this.localPlayerScore.ToString(),
				rank = this.localPlayerRank
			};
			return new AGSSocialLeaderboardScore(aGSScore, this.leaderboard);
		}
	}

	public uint maxRange
	{
		get
		{
			AGSClient.LogGameCircleError("ILeaderboard.maxRange.get is not available for GameCircle");
			return (uint)0;
		}
	}

	public Range range
	{
		get;
		set;
	}

	public IScore[] scores
	{
		get
		{
			return JustDecompileGenerated_get_scores();
		}
		set
		{
			JustDecompileGenerated_set_scores(value);
		}
	}

	private IScore[] JustDecompileGenerated_scores_k__BackingField;

	public IScore[] JustDecompileGenerated_get_scores()
	{
		return this.JustDecompileGenerated_scores_k__BackingField;
	}

	public void JustDecompileGenerated_set_scores(IScore[] value)
	{
		this.JustDecompileGenerated_scores_k__BackingField = value;
	}

	public TimeScope timeScope
	{
		get
		{
			return this._timeScope;
		}
		set
		{
			this.localPlayerScore = (long)-1;
			this.localPlayerRank = -1;
			this.scores = new AGSSocialLeaderboardScore[0];
			this._timeScope = value;
			this.LoadScores(null);
			GameCircleSocial.Instance.RequestLocalUserScore(this);
		}
	}

	public string title
	{
		get
		{
			if (this.leaderboard == null)
			{
				return null;
			}
			return this.leaderboard.name;
		}
	}

	public UserScope userScope
	{
		get;
		set;
	}

	public AGSSocialLeaderboard(AGSLeaderboard leaderboard)
	{
		if (leaderboard != null)
		{
			this.leaderboard = leaderboard;
		}
		else
		{
			AGSClient.LogGameCircleError("AGSSocialLeaderboard constructor \"leaderboard\" argument should not be null");
			this.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
		}
		this.id = leaderboard.id;
		this.scores = new AGSSocialLeaderboardScore[0];
		this.localPlayerScore = (long)-1;
		this.localPlayerRank = -1;
		this._timeScope = TimeScope.AllTime;
	}

	public AGSSocialLeaderboard()
	{
		this.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
		this.localPlayerScore = (long)-1;
		this.localPlayerRank = -1;
		this._timeScope = TimeScope.AllTime;
	}

	public void LoadScores(Action<bool> callback)
	{
		if (this.leaderboard == null)
		{
			callback(false);
			return;
		}
		GameCircleSocial.Instance.RequestScores(this, callback);
	}

	public bool ScoresAvailable()
	{
		return (this.leaderboard == null || this.scores == null || (int)this.scores.Length <= 0 || this.localPlayerScore <= (long)-1 ? false : this.localPlayerRank > -1);
	}

	public void SetUserFilter(string[] userIDs)
	{
		AGSClient.LogGameCircleError("ILeaderboard.SetUserFilter is not available for GameCircle");
	}
}