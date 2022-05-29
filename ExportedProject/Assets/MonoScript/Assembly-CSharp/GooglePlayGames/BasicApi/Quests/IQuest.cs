using System;

namespace GooglePlayGames.BasicApi.Quests
{
	public interface IQuest
	{
		DateTime? AcceptedTime
		{
			get;
		}

		string BannerUrl
		{
			get;
		}

		string Description
		{
			get;
		}

		DateTime ExpirationTime
		{
			get;
		}

		string IconUrl
		{
			get;
		}

		string Id
		{
			get;
		}

		IQuestMilestone Milestone
		{
			get;
		}

		string Name
		{
			get;
		}

		DateTime StartTime
		{
			get;
		}

		QuestState State
		{
			get;
		}
	}
}