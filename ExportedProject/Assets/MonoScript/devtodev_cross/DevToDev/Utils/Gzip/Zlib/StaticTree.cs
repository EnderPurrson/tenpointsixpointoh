using System;
using System.Runtime.CompilerServices;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal sealed class StaticTree
	{
		internal static readonly short[] lengthAndLiteralsTreeCodes;

		internal static readonly short[] distTreeCodes;

		internal static readonly StaticTree Literals;

		internal static readonly StaticTree Distances;

		internal static readonly StaticTree BitLengths;

		internal short[] treeCodes;

		internal int[] extraBits;

		internal int extraBase;

		internal int elems;

		internal int maxLength;

		private StaticTree(short[] treeCodes, int[] extraBits, int extraBase, int elems, int maxLength)
		{
			this.treeCodes = treeCodes;
			this.extraBits = extraBits;
			this.extraBase = extraBase;
			this.elems = elems;
			this.maxLength = maxLength;
		}

		static StaticTree()
		{
			short[] array = new short[576];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			lengthAndLiteralsTreeCodes = array;
			short[] array2 = new short[60];
			RuntimeHelpers.InitializeArray((global::System.Array)array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			distTreeCodes = array2;
			Literals = new StaticTree(lengthAndLiteralsTreeCodes, Tree.ExtraLengthBits, InternalConstants.LITERALS + 1, InternalConstants.L_CODES, InternalConstants.MAX_BITS);
			Distances = new StaticTree(distTreeCodes, Tree.ExtraDistanceBits, 0, InternalConstants.D_CODES, InternalConstants.MAX_BITS);
			BitLengths = new StaticTree(null, Tree.extra_blbits, 0, InternalConstants.BL_CODES, InternalConstants.MAX_BL_BITS);
		}
	}
}
