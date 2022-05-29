using System;

namespace GooglePlayGames.BasicApi
{
	public class ScorePageToken
	{
		private string mId;

		private object mInternalObject;

		private LeaderboardCollection mCollection;

		private LeaderboardTimeSpan mTimespan;

		public LeaderboardCollection Collection
		{
			get
			{
				return this.mCollection;
			}
		}

		internal object InternalObject
		{
			get
			{
				return this.mInternalObject;
			}
		}

		public string LeaderboardId
		{
			get
			{
				return this.mId;
			}
		}

		public LeaderboardTimeSpan TimeSpan
		{
			get
			{
				return this.mTimespan;
			}
		}

		internal ScorePageToken(object internalObject, string id, LeaderboardCollection collection, LeaderboardTimeSpan timespan)
		{
			this.mInternalObject = internalObject;
			this.mId = id;
			this.mCollection = collection;
			this.mTimespan = timespan;
		}
	}
}