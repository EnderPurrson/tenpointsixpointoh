using System;
using System.Runtime.CompilerServices;

namespace ExitGames.Client.Photon
{
	internal static class Version
	{
		internal static readonly byte[] clientVersion;

		static Version()
		{
			byte[] array = new byte[4];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			clientVersion = array;
		}
	}
}
