using System;

namespace GooglePlayGames.BasicApi.Quests
{
	public enum QuestClaimMilestoneStatus
	{
		Success,
		BadInput,
		InternalError,
		NotAuthorized,
		Timeout,
		MilestoneAlreadyClaimed,
		MilestoneClaimFailed
	}
}