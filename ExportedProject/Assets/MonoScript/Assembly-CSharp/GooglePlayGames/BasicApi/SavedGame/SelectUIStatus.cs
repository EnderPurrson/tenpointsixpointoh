using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	public enum SelectUIStatus
	{
		BadInputError = -4,
		AuthenticationError = -3,
		TimeoutError = -2,
		InternalError = -1,
		SavedGameSelected = 1,
		UserClosedUI = 2
	}
}