using System;
using System.Runtime.CompilerServices;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal sealed class InfTree
	{
		private const int MANY = 1440;

		private const int Z_OK = 0;

		private const int Z_STREAM_END = 1;

		private const int Z_NEED_DICT = 2;

		private const int Z_ERRNO = -1;

		private const int Z_STREAM_ERROR = -2;

		private const int Z_DATA_ERROR = -3;

		private const int Z_MEM_ERROR = -4;

		private const int Z_BUF_ERROR = -5;

		private const int Z_VERSION_ERROR = -6;

		internal const int fixed_bl = 9;

		internal const int fixed_bd = 5;

		internal const int BMAX = 15;

		internal static readonly int[] fixed_tl;

		internal static readonly int[] fixed_td;

		internal static readonly int[] cplens;

		internal static readonly int[] cplext;

		internal static readonly int[] cpdist;

		internal static readonly int[] cpdext;

		internal int[] hn;

		internal int[] v;

		internal int[] c;

		internal int[] r;

		internal int[] u;

		internal int[] x;

		private int huft_build(int[] b, int bindex, int n, int s, int[] d, int[] e, int[] t, int[] m, int[] hp, int[] hn, int[] v)
		{
			int num = 0;
			int num2 = n;
			do
			{
				c[b[bindex + num]]++;
				num++;
				num2--;
			}
			while (num2 != 0);
			if (c[0] == n)
			{
				t[0] = -1;
				m[0] = 0;
				return 0;
			}
			int num3 = m[0];
			int i;
			for (i = 1; i <= 15 && c[i] == 0; i++)
			{
			}
			int j = i;
			if (num3 < i)
			{
				num3 = i;
			}
			num2 = 15;
			while (num2 != 0 && c[num2] == 0)
			{
				num2--;
			}
			int num4 = num2;
			if (num3 > num2)
			{
				num3 = num2;
			}
			m[0] = num3;
			int num5 = 1 << i;
			while (i < num2)
			{
				if ((num5 -= c[i]) < 0)
				{
					return -3;
				}
				i++;
				num5 <<= 1;
			}
			if ((num5 -= c[num2]) < 0)
			{
				return -3;
			}
			c[num2] += num5;
			i = (x[1] = 0);
			num = 1;
			int num6 = 2;
			while (--num2 != 0)
			{
				i = (x[num6] = i + c[num]);
				num6++;
				num++;
			}
			num2 = 0;
			num = 0;
			do
			{
				if ((i = b[bindex + num]) != 0)
				{
					v[x[i]++] = num2;
				}
				num++;
			}
			while (++num2 < n);
			n = x[num4];
			num2 = (x[0] = 0);
			num = 0;
			int num7 = -1;
			int num8 = -num3;
			u[0] = 0;
			int num9 = 0;
			int num10 = 0;
			for (; j <= num4; j++)
			{
				int num11 = c[j];
				while (num11-- != 0)
				{
					int num12;
					while (j > num8 + num3)
					{
						num7++;
						num8 += num3;
						num10 = num4 - num8;
						num10 = ((num10 > num3) ? num3 : num10);
						if ((num12 = 1 << (i = j - num8)) > num11 + 1)
						{
							num12 -= num11 + 1;
							num6 = j;
							if (i < num10)
							{
								while (++i < num10 && (num12 <<= 1) > c[++num6])
								{
									num12 -= c[num6];
								}
							}
						}
						num10 = 1 << i;
						if (hn[0] + num10 > 1440)
						{
							return -3;
						}
						num9 = (u[num7] = hn[0]);
						hn[0] += num10;
						if (num7 != 0)
						{
							x[num7] = num2;
							r[0] = (sbyte)i;
							r[1] = (sbyte)num3;
							i = SharedUtils.URShift(num2, num8 - num3);
							r[2] = num9 - u[num7 - 1] - i;
							global::System.Array.Copy((global::System.Array)r, 0, (global::System.Array)hp, (u[num7 - 1] + i) * 3, 3);
						}
						else
						{
							t[0] = num9;
						}
					}
					r[1] = (sbyte)(j - num8);
					if (num >= n)
					{
						r[0] = 192;
					}
					else if (v[num] < s)
					{
						r[0] = (sbyte)((v[num] >= 256) ? 96 : 0);
						r[2] = v[num++];
					}
					else
					{
						r[0] = (sbyte)(e[v[num] - s] + 16 + 64);
						r[2] = d[v[num++] - s];
					}
					num12 = 1 << j - num8;
					for (i = SharedUtils.URShift(num2, num8); i < num10; i += num12)
					{
						global::System.Array.Copy((global::System.Array)r, 0, (global::System.Array)hp, (num9 + i) * 3, 3);
					}
					i = 1 << j - 1;
					while ((num2 & i) != 0)
					{
						num2 ^= i;
						i = SharedUtils.URShift(i, 1);
					}
					num2 ^= i;
					int num13 = (1 << num8) - 1;
					while ((num2 & num13) != x[num7])
					{
						num7--;
						num8 -= num3;
						num13 = (1 << num8) - 1;
					}
				}
			}
			if (num5 == 0 || num4 == 1)
			{
				return 0;
			}
			return -5;
		}

		internal int inflate_trees_bits(int[] c, int[] bb, int[] tb, int[] hp, ZlibCodec z)
		{
			initWorkArea(19);
			hn[0] = 0;
			int num = huft_build(c, 0, 19, 19, null, null, tb, bb, hp, hn, v);
			if (num == -3)
			{
				z.Message = "oversubscribed dynamic bit lengths tree";
			}
			else if (num == -5 || bb[0] == 0)
			{
				z.Message = "incomplete dynamic bit lengths tree";
				num = -3;
			}
			return num;
		}

		internal int inflate_trees_dynamic(int nl, int nd, int[] c, int[] bl, int[] bd, int[] tl, int[] td, int[] hp, ZlibCodec z)
		{
			initWorkArea(288);
			hn[0] = 0;
			int num = huft_build(c, 0, nl, 257, cplens, cplext, tl, bl, hp, hn, v);
			if (num != 0 || bl[0] == 0)
			{
				switch (num)
				{
				case -3:
					z.Message = "oversubscribed literal/length tree";
					break;
				default:
					z.Message = "incomplete literal/length tree";
					num = -3;
					break;
				case -4:
					break;
				}
				return num;
			}
			initWorkArea(288);
			num = huft_build(c, nl, nd, 0, cpdist, cpdext, td, bd, hp, hn, v);
			if (num != 0 || (bd[0] == 0 && nl > 257))
			{
				switch (num)
				{
				case -3:
					z.Message = "oversubscribed distance tree";
					break;
				case -5:
					z.Message = "incomplete distance tree";
					num = -3;
					break;
				default:
					z.Message = "empty distance tree with lengths";
					num = -3;
					break;
				case -4:
					break;
				}
				return num;
			}
			return 0;
		}

		internal static int inflate_trees_fixed(int[] bl, int[] bd, int[][] tl, int[][] td, ZlibCodec z)
		{
			bl[0] = 9;
			bd[0] = 5;
			tl[0] = fixed_tl;
			td[0] = fixed_td;
			return 0;
		}

		private void initWorkArea(int vsize)
		{
			if (hn == null)
			{
				hn = new int[1];
				v = new int[vsize];
				c = new int[16];
				r = new int[3];
				u = new int[15];
				x = new int[16];
				return;
			}
			if (v.Length < vsize)
			{
				v = new int[vsize];
			}
			global::System.Array.Clear((global::System.Array)v, 0, vsize);
			global::System.Array.Clear((global::System.Array)c, 0, 16);
			r[0] = 0;
			r[1] = 0;
			r[2] = 0;
			global::System.Array.Clear((global::System.Array)u, 0, 15);
			global::System.Array.Clear((global::System.Array)x, 0, 16);
		}

		static InfTree()
		{
			int[] array = new int[1536];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			fixed_tl = array;
			int[] array2 = new int[96];
			RuntimeHelpers.InitializeArray((global::System.Array)array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			fixed_td = array2;
			int[] array3 = new int[31];
			RuntimeHelpers.InitializeArray((global::System.Array)array3, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			cplens = array3;
			int[] array4 = new int[31];
			RuntimeHelpers.InitializeArray((global::System.Array)array4, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			cplext = array4;
			int[] array5 = new int[30];
			RuntimeHelpers.InitializeArray((global::System.Array)array5, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			cpdist = array5;
			int[] array6 = new int[30];
			RuntimeHelpers.InitializeArray((global::System.Array)array6, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			cpdext = array6;
		}
	}
}
