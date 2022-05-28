using System;
using System.Runtime.CompilerServices;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal static class InternalInflateConstants
	{
		internal static readonly int[] InflateMask;

		static InternalInflateConstants()
		{
			int[] array = new int[17];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			InflateMask = array;
		}
	}
}
