using System;

namespace GooglePlayGames.BasicApi
{
	public enum UIStatus
	{
		LeftRoom = -18,
		UiBusy = -12,
		UserClosedUI = -6,
		Timeout = -5,
		VersionUpdateRequired = -4,
		NotAuthorized = -3,
		InternalError = -2,
		Valid = 1
	}
}