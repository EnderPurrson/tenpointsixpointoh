using GooglePlayGames.BasicApi;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesLeaderboard : ILeaderboard
	{
		private string mId;

		private UserScope mUserScope;

		private Range mRange;

		private TimeScope mTimeScope;

		private string[] mFilteredUserIds;

		private bool mLoading;

		private IScore mLocalUserScore;

		private uint mMaxRange;

		private List<PlayGamesScore> mScoreList = new List<PlayGamesScore>();

		private string mTitle;

		public string id
		{
			get
			{
				return this.mId;
			}
			set
			{
				this.mId = value;
			}
		}

		public bool loading
		{
			get
			{
				return JustDecompileGenerated_get_loading();
			}
			set
			{
				JustDecompileGenerated_set_loading(value);
			}
		}

		public bool JustDecompileGenerated_get_loading()
		{
			return this.mLoading;
		}

		internal void JustDecompileGenerated_set_loading(bool value)
		{
			this.mLoading = value;
		}

		public IScore localUserScore
		{
			get
			{
				return this.mLocalUserScore;
			}
		}

		public uint maxRange
		{
			get
			{
				return this.mMaxRange;
			}
		}

		public Range range
		{
			get
			{
				return this.mRange;
			}
			set
			{
				this.mRange = value;
			}
		}

		public int ScoreCount
		{
			get
			{
				return this.mScoreList.Count;
			}
		}

		public IScore[] scores
		{
			get
			{
				PlayGamesScore[] playGamesScoreArray = new PlayGamesScore[this.mScoreList.Count];
				this.mScoreList.CopyTo(playGamesScoreArray);
				return playGamesScoreArray;
			}
		}

		public TimeScope timeScope
		{
			get
			{
				return this.mTimeScope;
			}
			set
			{
				this.mTimeScope = value;
			}
		}

		public string title
		{
			get
			{
				return this.mTitle;
			}
		}

		public UserScope userScope
		{
			get
			{
				return this.mUserScope;
			}
			set
			{
				this.mUserScope = value;
			}
		}

		public PlayGamesLeaderboard(string id)
		{
			this.mId = id;
		}

		internal int AddScore(PlayGamesScore score)
		{
			if (this.mFilteredUserIds == null || (int)this.mFilteredUserIds.Length == 0)
			{
				this.mScoreList.Add(score);
			}
			else
			{
				string[] strArrays = this.mFilteredUserIds;
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					if (strArrays[i].Equals(score.userID))
					{
						return this.mScoreList.Count;
					}
				}
				this.mScoreList.Add(score);
			}
			return this.mScoreList.Count;
		}

		internal bool HasAllScores()
		{
			return (this.mScoreList.Count >= this.mRange.count ? true : (long)this.mScoreList.Count >= (ulong)this.maxRange);
		}

		public void LoadScores(Action<bool> callback)
		{
			PlayGamesPlatform.Instance.LoadScores(this, callback);
		}

		internal bool SetFromData(LeaderboardScoreData data)
		{
			if (data.Valid)
			{
				Debug.Log(string.Concat("Setting leaderboard from: ", data));
				this.SetMaxRange(data.ApproximateCount);
				this.SetTitle(data.Title);
				this.SetLocalUserScore((PlayGamesScore)data.PlayerScore);
				IScore[] scores = data.Scores;
				for (int i = 0; i < (int)scores.Length; i++)
				{
					this.AddScore((PlayGamesScore)scores[i]);
				}
				this.mLoading = ((int)data.Scores.Length == 0 ? true : this.HasAllScores());
			}
			return data.Valid;
		}

		internal void SetLocalUserScore(PlayGamesScore score)
		{
			this.mLocalUserScore = score;
		}

		internal void SetMaxRange(ulong val)
		{
			this.mMaxRange = (uint)val;
		}

		internal void SetTitle(string value)
		{
			this.mTitle = value;
		}

		public void SetUserFilter(string[] userIDs)
		{
			this.mFilteredUserIds = userIDs;
		}
	}
}