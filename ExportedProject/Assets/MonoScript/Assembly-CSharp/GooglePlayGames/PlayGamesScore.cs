using System;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesScore : IScore
	{
		private string mLbId;

		private long mValue;

		private ulong mRank;

		private string mPlayerId = string.Empty;

		private string mMetadata = string.Empty;

		private DateTime mDate = new DateTime(1970, 1, 1, 0, 0, 0);

		public DateTime date
		{
			get
			{
				return this.mDate;
			}
		}

		public string formattedValue
		{
			get
			{
				return this.mValue.ToString();
			}
		}

		public string leaderboardID
		{
			get
			{
				return this.mLbId;
			}
			set
			{
				this.mLbId = value;
			}
		}

		public int rank
		{
			get
			{
				return (int)this.mRank;
			}
		}

		public string userID
		{
			get
			{
				return this.mPlayerId;
			}
		}

		public long @value
		{
			get
			{
				return this.mValue;
			}
			set
			{
				this.mValue = value;
			}
		}

		internal PlayGamesScore(DateTime date, string leaderboardId, ulong rank, string playerId, ulong value, string metadata)
		{
			this.mDate = date;
			this.mLbId = this.leaderboardID;
			this.mRank = rank;
			this.mPlayerId = playerId;
			this.mValue = (long)value;
			this.mMetadata = metadata;
		}

		public void ReportScore(Action<bool> callback)
		{
			PlayGamesPlatform.Instance.ReportScore(this.mValue, this.mLbId, this.mMetadata, callback);
		}
	}
}