using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AGSSocialAchievement : IAchievementDescription, IAchievement
{
	public readonly AGSAchievement achievement;

	public string achievedDescription
	{
		get
		{
			return this.achievement.description;
		}
	}

	public bool completed
	{
		get
		{
			return this.achievement.isUnlocked;
		}
	}

	public bool hidden
	{
		get
		{
			return this.achievement.isHidden;
		}
	}

	public string id
	{
		get;
		set;
	}

	public Texture2D image
	{
		get
		{
			AGSClient.LogGameCircleError("IAchievementDescription.image.get is not available for GameCircle");
			return null;
		}
	}

	public DateTime lastReportedDate
	{
		get
		{
			return this.achievement.dateUnlocked;
		}
	}

	public double percentCompleted
	{
		get;
		set;
	}

	public int points
	{
		get
		{
			return this.achievement.pointValue;
		}
	}

	public string title
	{
		get
		{
			return this.achievement.title;
		}
	}

	public string unachievedDescription
	{
		get
		{
			return this.achievement.description;
		}
	}

	public AGSSocialAchievement(AGSAchievement achievement)
	{
		if (achievement != null)
		{
			this.achievement = achievement;
		}
		else
		{
			AGSClient.LogGameCircleError("AGSSocialAchievement constructor \"achievement\" argument should not be null");
			achievement = AGSAchievement.GetBlankAchievement();
		}
		this.id = achievement.id;
		this.percentCompleted = (double)achievement.progress;
	}

	public AGSSocialAchievement()
	{
		this.achievement = AGSAchievement.GetBlankAchievement();
	}

	public void ReportProgress(Action<bool> callback)
	{
		GameCircleSocial.Instance.ReportProgress(this.id, this.percentCompleted, callback);
	}
}