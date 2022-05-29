using System;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class NearbyConnectionsStatus
	{
		internal enum InitializationStatus
		{
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			ERROR_INTERNAL = -2,
			VALID = 1
		}
	}
}