using System;
using System.Runtime.CompilerServices;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal sealed class Tree
	{
		internal const int Buf_size = 16;

		private static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

		internal static readonly int[] ExtraLengthBits;

		internal static readonly int[] ExtraDistanceBits;

		internal static readonly int[] extra_blbits;

		internal static readonly sbyte[] bl_order;

		private static readonly sbyte[] _dist_code;

		internal static readonly sbyte[] LengthCode;

		internal static readonly int[] LengthBase;

		internal static readonly int[] DistanceBase;

		internal short[] dyn_tree;

		internal int max_code;

		internal StaticTree staticTree;

		internal static int DistanceCode(int dist)
		{
			if (dist >= 256)
			{
				return _dist_code[256 + SharedUtils.URShift(dist, 7)];
			}
			return _dist_code[dist];
		}

		internal void gen_bitlen(DeflateManager s)
		{
			short[] array = dyn_tree;
			short[] treeCodes = staticTree.treeCodes;
			int[] extraBits = staticTree.extraBits;
			int extraBase = staticTree.extraBase;
			int maxLength = staticTree.maxLength;
			int num = 0;
			for (int i = 0; i <= InternalConstants.MAX_BITS; i++)
			{
				s.bl_count[i] = 0;
			}
			array[s.heap[s.heap_max] * 2 + 1] = 0;
			int j;
			for (j = s.heap_max + 1; j < HEAP_SIZE; j++)
			{
				int num2 = s.heap[j];
				int i = array[array[num2 * 2 + 1] * 2 + 1] + 1;
				if (i > maxLength)
				{
					i = maxLength;
					num++;
				}
				array[num2 * 2 + 1] = (short)i;
				if (num2 <= max_code)
				{
					s.bl_count[i]++;
					int num3 = 0;
					if (num2 >= extraBase)
					{
						num3 = extraBits[num2 - extraBase];
					}
					short num4 = array[num2 * 2];
					s.opt_len += num4 * (i + num3);
					if (treeCodes != null)
					{
						s.static_len += num4 * (treeCodes[num2 * 2 + 1] + num3);
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			do
			{
				int i = maxLength - 1;
				while (s.bl_count[i] == 0)
				{
					i--;
				}
				s.bl_count[i]--;
				s.bl_count[i + 1] = (short)(s.bl_count[i + 1] + 2);
				s.bl_count[maxLength]--;
				num -= 2;
			}
			while (num > 0);
			for (int i = maxLength; i != 0; i--)
			{
				int num2 = s.bl_count[i];
				while (num2 != 0)
				{
					int num5 = s.heap[--j];
					if (num5 <= max_code)
					{
						if (array[num5 * 2 + 1] != i)
						{
							s.opt_len = (int)(s.opt_len + ((long)i - (long)array[num5 * 2 + 1]) * array[num5 * 2]);
							array[num5 * 2 + 1] = (short)i;
						}
						num2--;
					}
				}
			}
		}

		internal void build_tree(DeflateManager s)
		{
			short[] array = dyn_tree;
			short[] treeCodes = staticTree.treeCodes;
			int elems = staticTree.elems;
			int num = -1;
			s.heap_len = 0;
			s.heap_max = HEAP_SIZE;
			for (int i = 0; i < elems; i++)
			{
				if (array[i * 2] != 0)
				{
					num = (s.heap[++s.heap_len] = i);
					s.depth[i] = 0;
				}
				else
				{
					array[i * 2 + 1] = 0;
				}
			}
			int num2;
			while (s.heap_len < 2)
			{
				num2 = (s.heap[++s.heap_len] = ((num < 2) ? (++num) : 0));
				array[num2 * 2] = 1;
				s.depth[num2] = 0;
				s.opt_len--;
				if (treeCodes != null)
				{
					s.static_len -= treeCodes[num2 * 2 + 1];
				}
			}
			max_code = num;
			for (int i = s.heap_len / 2; i >= 1; i--)
			{
				s.pqdownheap(array, i);
			}
			num2 = elems;
			do
			{
				int i = s.heap[1];
				s.heap[1] = s.heap[s.heap_len--];
				s.pqdownheap(array, 1);
				int num3 = s.heap[1];
				s.heap[--s.heap_max] = i;
				s.heap[--s.heap_max] = num3;
				array[num2 * 2] = (short)(array[i * 2] + array[num3 * 2]);
				s.depth[num2] = (sbyte)(Math.Max((byte)s.depth[i], (byte)s.depth[num3]) + 1);
				array[i * 2 + 1] = (array[num3 * 2 + 1] = (short)num2);
				s.heap[1] = num2++;
				s.pqdownheap(array, 1);
			}
			while (s.heap_len >= 2);
			s.heap[--s.heap_max] = s.heap[1];
			gen_bitlen(s);
			gen_codes(array, num, s.bl_count);
		}

		internal static void gen_codes(short[] tree, int max_code, short[] bl_count)
		{
			short[] array = new short[InternalConstants.MAX_BITS + 1];
			short num = 0;
			for (int i = 1; i <= InternalConstants.MAX_BITS; i++)
			{
				num = (array[i] = (short)(num + bl_count[i - 1] << 1));
			}
			for (int j = 0; j <= max_code; j++)
			{
				int num2 = tree[j * 2 + 1];
				if (num2 != 0)
				{
					tree[j * 2] = (short)bi_reverse(array[num2]++, num2);
				}
			}
		}

		internal static int bi_reverse(int code, int len)
		{
			int num = 0;
			do
			{
				num |= code & 1;
				code >>= 1;
				num <<= 1;
			}
			while (--len > 0);
			return num >> 1;
		}

		static Tree()
		{
			int[] array = new int[29];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			ExtraLengthBits = array;
			int[] array2 = new int[30];
			RuntimeHelpers.InitializeArray((global::System.Array)array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			ExtraDistanceBits = array2;
			int[] array3 = new int[19];
			RuntimeHelpers.InitializeArray((global::System.Array)array3, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			extra_blbits = array3;
			sbyte[] array4 = new sbyte[19];
			RuntimeHelpers.InitializeArray((global::System.Array)array4, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			bl_order = array4;
			sbyte[] array5 = new sbyte[512];
			RuntimeHelpers.InitializeArray((global::System.Array)array5, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			_dist_code = array5;
			sbyte[] array6 = new sbyte[256];
			RuntimeHelpers.InitializeArray((global::System.Array)array6, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			LengthCode = array6;
			int[] array7 = new int[29];
			RuntimeHelpers.InitializeArray((global::System.Array)array7, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			LengthBase = array7;
			int[] array8 = new int[30];
			RuntimeHelpers.InitializeArray((global::System.Array)array8, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			DistanceBase = array8;
		}
	}
}
