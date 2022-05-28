using System;
using System.Runtime.CompilerServices;

namespace Photon.SocketServer.Security
{
	internal static class OakleyGroups
	{
		public static readonly int Generator = 22;

		public static readonly byte[] OakleyPrime768;

		public static readonly byte[] OakleyPrime1024;

		public static readonly byte[] OakleyPrime1536;

		static OakleyGroups()
		{
			byte[] array = new byte[96];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			OakleyPrime768 = array;
			byte[] array2 = new byte[128];
			RuntimeHelpers.InitializeArray((global::System.Array)array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			OakleyPrime1024 = array2;
			byte[] array3 = new byte[192];
			RuntimeHelpers.InitializeArray((global::System.Array)array3, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			OakleyPrime1536 = array3;
		}
	}
}
