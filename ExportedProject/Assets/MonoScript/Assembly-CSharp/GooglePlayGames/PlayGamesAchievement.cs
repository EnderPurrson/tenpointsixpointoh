using GooglePlayGames.BasicApi;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	internal class PlayGamesAchievement : IAchievementDescription, IAchievement
	{
		private readonly ReportProgress mProgressCallback;

		private string mId = string.Empty;

		private bool mIsIncremental;

		private int mCurrentSteps;

		private int mTotalSteps;

		private double mPercentComplete;

		private bool mCompleted;

		private bool mHidden;

		private DateTime mLastModifiedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		private string mTitle = string.Empty;

		private string mRevealedImageUrl = string.Empty;

		private string mUnlockedImageUrl = string.Empty;

		private WWW mImageFetcher;

		private Texture2D mImage;

		private string mDescription = string.Empty;

		private ulong mPoints;

		public string achievedDescription
		{
			get
			{
				return this.mDescription;
			}
		}

		public bool completed
		{
			get
			{
				return this.mCompleted;
			}
		}

		public int currentSteps
		{
			get
			{
				return this.mCurrentSteps;
			}
		}

		public bool hidden
		{
			get
			{
				return this.mHidden;
			}
		}

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

		public Texture2D image
		{
			get
			{
				return this.LoadImage();
			}
		}

		public bool isIncremental
		{
			get
			{
				return this.mIsIncremental;
			}
		}

		public DateTime lastReportedDate
		{
			get
			{
				return this.mLastModifiedTime;
			}
		}

		public double percentCompleted
		{
			get
			{
				return this.mPercentComplete;
			}
			set
			{
				this.mPercentComplete = value;
			}
		}

		public int points
		{
			get
			{
				return (int)this.mPoints;
			}
		}

		public string title
		{
			get
			{
				return this.mTitle;
			}
		}

		public int totalSteps
		{
			get
			{
				return this.mTotalSteps;
			}
		}

		public string unachievedDescription
		{
			get
			{
				return this.mDescription;
			}
		}

		internal PlayGamesAchievement()
		{
			PlayGamesPlatform instance = PlayGamesPlatform.Instance;
			this(new ReportProgress(instance.ReportProgress));
		}

		internal PlayGamesAchievement(ReportProgress progressCallback)
		{
			this.mProgressCallback = progressCallback;
		}

		internal PlayGamesAchievement(Achievement ach) : this()
		{
			this.mId = ach.Id;
			this.mIsIncremental = ach.IsIncremental;
			this.mCurrentSteps = ach.CurrentSteps;
			this.mTotalSteps = ach.TotalSteps;
			if (!ach.IsIncremental)
			{
				this.mPercentComplete = (!ach.IsUnlocked ? 0 : 100);
			}
			else if (ach.TotalSteps <= 0)
			{
				this.mPercentComplete = 0;
			}
			else
			{
				this.mPercentComplete = (double)ach.CurrentSteps / (double)ach.TotalSteps * 100;
			}
			this.mCompleted = ach.IsUnlocked;
			this.mHidden = !ach.IsRevealed;
			this.mLastModifiedTime = ach.LastModifiedTime;
			this.mTitle = ach.Name;
			this.mDescription = ach.Description;
			this.mPoints = ach.Points;
			this.mRevealedImageUrl = ach.RevealedImageUrl;
			this.mUnlockedImageUrl = ach.UnlockedImageUrl;
		}

		private Texture2D LoadImage()
		{
			if (this.hidden)
			{
				return null;
			}
			string str = (!this.completed ? this.mRevealedImageUrl : this.mUnlockedImageUrl);
			if (!string.IsNullOrEmpty(str))
			{
				if (this.mImageFetcher == null || this.mImageFetcher.url != str)
				{
					this.mImageFetcher = new WWW(str);
					this.mImage = null;
				}
				if (this.mImage != null)
				{
					return this.mImage;
				}
				if (this.mImageFetcher.isDone)
				{
					this.mImage = this.mImageFetcher.texture;
					return this.mImage;
				}
			}
			return null;
		}

		public void ReportProgress(Action<bool> callback)
		{
			this.mProgressCallback(this.mId, this.mPercentComplete, callback);
		}
	}
}