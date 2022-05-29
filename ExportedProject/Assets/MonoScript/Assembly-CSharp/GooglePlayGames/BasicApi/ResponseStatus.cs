using System;

namespace GooglePlayGames.BasicApi
{
	public enum ResponseStatus
	{
		Timeout = -5,
		VersionUpdateRequired = -4,
		NotAuthorized = -3,
		InternalError = -2,
		LicenseCheckFailed = -1,
		Success = 1,
		SuccessWithStale = 2
	}
}