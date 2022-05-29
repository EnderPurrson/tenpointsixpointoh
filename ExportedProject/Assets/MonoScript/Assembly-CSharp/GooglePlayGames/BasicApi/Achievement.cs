using System;

namespace GooglePlayGames.BasicApi
{
	public class Achievement
	{
		private readonly static DateTime UnixEpoch;

		private string mId = string.Empty;

		private bool mIsIncremental;

		private bool mIsRevealed;

		private bool mIsUnlocked;

		private int mCurrentSteps;

		private int mTotalSteps;

		private string mDescription = string.Empty;

		private string mName = string.Empty;

		private long mLastModifiedTime;

		private ulong mPoints;

		private string mRevealedImageUrl;

		private string mUnlockedImageUrl;

		public int CurrentSteps
		{
			get
			{
				return this.mCurrentSteps;
			}
			set
			{
				this.mCurrentSteps = value;
			}
		}

		public string Description
		{
			get
			{
				return this.mDescription;
			}
			set
			{
				this.mDescription = value;
			}
		}

		public string Id
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

		public bool IsIncremental
		{
			get
			{
				return this.mIsIncremental;
			}
			set
			{
				this.mIsIncremental = value;
			}
		}

		public bool IsRevealed
		{
			get
			{
				return this.mIsRevealed;
			}
			set
			{
				this.mIsRevealed = value;
			}
		}

		public bool IsUnlocked
		{
			get
			{
				return this.mIsUnlocked;
			}
			set
			{
				this.mIsUnlocked = value;
			}
		}

		public DateTime LastModifiedTime
		{
			get
			{
				return Achievement.UnixEpoch.AddMilliseconds((double)this.mLastModifiedTime);
			}
			set
			{
				this.mLastModifiedTime = (long)(value - Achievement.UnixEpoch).TotalMilliseconds;
			}
		}

		public string Name
		{
			get
			{
				return this.mName;
			}
			set
			{
				this.mName = value;
			}
		}

		public ulong Points
		{
			get
			{
				return this.mPoints;
			}
			set
			{
				this.mPoints = value;
			}
		}

		public string RevealedImageUrl
		{
			get
			{
				return this.mRevealedImageUrl;
			}
			set
			{
				this.mRevealedImageUrl = value;
			}
		}

		public int TotalSteps
		{
			get
			{
				return this.mTotalSteps;
			}
			set
			{
				this.mTotalSteps = value;
			}
		}

		public string UnlockedImageUrl
		{
			get
			{
				return this.mUnlockedImageUrl;
			}
			set
			{
				this.mUnlockedImageUrl = value;
			}
		}

		static Achievement()
		{
			Achievement.UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		}

		public Achievement()
		{
		}

		public override string ToString()
		{
			object[] objArray = new object[] { this.mId, this.mName, this.mDescription, null, null, null, null, null };
			objArray[3] = (!this.mIsIncremental ? "STANDARD" : "INCREMENTAL");
			objArray[4] = this.mIsRevealed;
			objArray[5] = this.mIsUnlocked;
			objArray[6] = this.mCurrentSteps;
			objArray[7] = this.mTotalSteps;
			return string.Format("[Achievement] id={0}, name={1}, desc={2}, type={3}, revealed={4}, unlocked={5}, steps={6}/{7}", objArray);
		}
	}
}