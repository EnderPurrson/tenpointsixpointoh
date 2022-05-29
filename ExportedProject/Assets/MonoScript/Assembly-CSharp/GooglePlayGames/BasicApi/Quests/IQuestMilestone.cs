using System;

namespace GooglePlayGames.BasicApi.Quests
{
	public interface IQuestMilestone
	{
		byte[] CompletionRewardData
		{
			get;
		}

		ulong CurrentCount
		{
			get;
		}

		string EventId
		{
			get;
		}

		string Id
		{
			get;
		}

		string QuestId
		{
			get;
		}

		MilestoneState State
		{
			get;
		}

		ulong TargetCount
		{
			get;
		}
	}
}