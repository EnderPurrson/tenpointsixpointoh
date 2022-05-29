using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	public enum SavedGameRequestStatus
	{
		BadInputError = -4,
		AuthenticationError = -3,
		InternalError = -2,
		TimeoutError = -1,
		Success = 1
	}
}