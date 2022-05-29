using GooglePlayGames.Native.PInvoke;
using System;

namespace GooglePlayGames
{
	internal interface IClientImpl
	{
		PlatformConfiguration CreatePlatformConfiguration();

		TokenClient CreateTokenClient(string playerId, bool reset);
	}
}