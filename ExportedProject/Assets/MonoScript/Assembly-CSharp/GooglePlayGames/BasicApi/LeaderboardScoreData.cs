using GooglePlayGames;
using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.BasicApi
{
	public class LeaderboardScoreData
	{
		private string mId;

		private ResponseStatus mStatus;

		private ulong mApproxCount;

		private string mTitle;

		private IScore mPlayerScore;

		private ScorePageToken mPrevPage;

		private ScorePageToken mNextPage;

		private List<PlayGamesScore> mScores = new List<PlayGamesScore>();

		public ulong ApproximateCount
		{
			get
			{
				return this.mApproxCount;
			}
			internal set
			{
				this.mApproxCount = value;
			}
		}

		public string Id
		{
			get
			{
				return this.mId;
			}
			internal set
			{
				this.mId = value;
			}
		}

		public ScorePageToken NextPageToken
		{
			get
			{
				return this.mNextPage;
			}
			internal set
			{
				this.mNextPage = value;
			}
		}

		public IScore PlayerScore
		{
			get
			{
				return this.mPlayerScore;
			}
			internal set
			{
				this.mPlayerScore = value;
			}
		}

		public ScorePageToken PrevPageToken
		{
			get
			{
				return this.mPrevPage;
			}
			internal set
			{
				this.mPrevPage = value;
			}
		}

		public IScore[] Scores
		{
			get
			{
				return this.mScores.ToArray();
			}
		}

		public ResponseStatus Status
		{
			get
			{
				return this.mStatus;
			}
			internal set
			{
				this.mStatus = value;
			}
		}

		public string Title
		{
			get
			{
				return this.mTitle;
			}
			internal set
			{
				this.mTitle = value;
			}
		}

		public bool Valid
		{
			get
			{
				return (this.mStatus == ResponseStatus.Success ? true : this.mStatus == ResponseStatus.SuccessWithStale);
			}
		}

		internal LeaderboardScoreData(string leaderboardId)
		{
			this.mId = leaderboardId;
		}

		internal LeaderboardScoreData(string leaderboardId, ResponseStatus status)
		{
			this.mId = leaderboardId;
			this.mStatus = status;
		}

		internal int AddScore(PlayGamesScore score)
		{
			this.mScores.Add(score);
			return this.mScores.Count;
		}

		public override string ToString()
		{
			return string.Format("[LeaderboardScoreData: mId={0},  mStatus={1}, mApproxCount={2}, mTitle={3}]", new object[] { this.mId, this.mStatus, this.mApproxCount, this.mTitle });
		}
	}
}