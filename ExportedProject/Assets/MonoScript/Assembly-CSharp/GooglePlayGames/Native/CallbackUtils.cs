using System;
using System.Runtime.CompilerServices;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	internal static class CallbackUtils
	{
		[CompilerGenerated]
		private sealed class _003CToOnGameThread_003Ec__AnonStorey202<T>
		{
			private sealed class _003CToOnGameThread_003Ec__AnonStorey203
			{
				internal T val;

				internal _003CToOnGameThread_003Ec__AnonStorey202<T> _003C_003Ef__ref_0024514;

				internal void _003C_003Em__8F()
				{
					_003C_003Ef__ref_0024514.toConvert(val);
				}
			}

			internal Action<T> toConvert;

			internal void _003C_003Em__8A(T val)
			{
				_003CToOnGameThread_003Ec__AnonStorey203 _003CToOnGameThread_003Ec__AnonStorey = new _003CToOnGameThread_003Ec__AnonStorey203();
				_003CToOnGameThread_003Ec__AnonStorey._003C_003Ef__ref_0024514 = this;
				_003CToOnGameThread_003Ec__AnonStorey.val = val;
				PlayGamesHelperObject.RunOnGameThread(_003CToOnGameThread_003Ec__AnonStorey._003C_003Em__8F);
			}
		}

		[CompilerGenerated]
		private sealed class _003CToOnGameThread_003Ec__AnonStorey204<T1, T2>
		{
			private sealed class _003CToOnGameThread_003Ec__AnonStorey205
			{
				internal T1 val1;

				internal T2 val2;

				internal _003CToOnGameThread_003Ec__AnonStorey204<T1, T2> _003C_003Ef__ref_0024516;

				internal void _003C_003Em__90()
				{
					_003C_003Ef__ref_0024516.toConvert(val1, val2);
				}
			}

			internal Action<T1, T2> toConvert;

			internal void _003C_003Em__8C(T1 val1, T2 val2)
			{
				_003CToOnGameThread_003Ec__AnonStorey205 _003CToOnGameThread_003Ec__AnonStorey = new _003CToOnGameThread_003Ec__AnonStorey205();
				_003CToOnGameThread_003Ec__AnonStorey._003C_003Ef__ref_0024516 = this;
				_003CToOnGameThread_003Ec__AnonStorey.val1 = val1;
				_003CToOnGameThread_003Ec__AnonStorey.val2 = val2;
				PlayGamesHelperObject.RunOnGameThread(_003CToOnGameThread_003Ec__AnonStorey._003C_003Em__90);
			}
		}

		[CompilerGenerated]
		private sealed class _003CToOnGameThread_003Ec__AnonStorey206<T1, T2, T3>
		{
			private sealed class _003CToOnGameThread_003Ec__AnonStorey207
			{
				internal T1 val1;

				internal T2 val2;

				internal T3 val3;

				internal _003CToOnGameThread_003Ec__AnonStorey206<T1, T2, T3> _003C_003Ef__ref_0024518;

				internal void _003C_003Em__91()
				{
					_003C_003Ef__ref_0024518.toConvert(val1, val2, val3);
				}
			}

			internal Action<T1, T2, T3> toConvert;

			internal void _003C_003Em__8E(T1 val1, T2 val2, T3 val3)
			{
				_003CToOnGameThread_003Ec__AnonStorey207 _003CToOnGameThread_003Ec__AnonStorey = new _003CToOnGameThread_003Ec__AnonStorey207();
				_003CToOnGameThread_003Ec__AnonStorey._003C_003Ef__ref_0024518 = this;
				_003CToOnGameThread_003Ec__AnonStorey.val1 = val1;
				_003CToOnGameThread_003Ec__AnonStorey.val2 = val2;
				_003CToOnGameThread_003Ec__AnonStorey.val3 = val3;
				PlayGamesHelperObject.RunOnGameThread(_003CToOnGameThread_003Ec__AnonStorey._003C_003Em__91);
			}
		}

		internal static Action<T> ToOnGameThread<T>(Action<T> toConvert)
		{
			_003CToOnGameThread_003Ec__AnonStorey202<T> _003CToOnGameThread_003Ec__AnonStorey = new _003CToOnGameThread_003Ec__AnonStorey202<T>();
			_003CToOnGameThread_003Ec__AnonStorey.toConvert = toConvert;
			if (_003CToOnGameThread_003Ec__AnonStorey.toConvert == null)
			{
				return _003CToOnGameThread_00601_003Em__89;
			}
			return _003CToOnGameThread_003Ec__AnonStorey._003C_003Em__8A;
		}

		internal static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
		{
			_003CToOnGameThread_003Ec__AnonStorey204<T1, T2> _003CToOnGameThread_003Ec__AnonStorey = new _003CToOnGameThread_003Ec__AnonStorey204<T1, T2>();
			_003CToOnGameThread_003Ec__AnonStorey.toConvert = toConvert;
			if (_003CToOnGameThread_003Ec__AnonStorey.toConvert == null)
			{
				return _003CToOnGameThread_00602_003Em__8B;
			}
			return _003CToOnGameThread_003Ec__AnonStorey._003C_003Em__8C;
		}

		internal static Action<T1, T2, T3> ToOnGameThread<T1, T2, T3>(Action<T1, T2, T3> toConvert)
		{
			_003CToOnGameThread_003Ec__AnonStorey206<T1, T2, T3> _003CToOnGameThread_003Ec__AnonStorey = new _003CToOnGameThread_003Ec__AnonStorey206<T1, T2, T3>();
			_003CToOnGameThread_003Ec__AnonStorey.toConvert = toConvert;
			if (_003CToOnGameThread_003Ec__AnonStorey.toConvert == null)
			{
				return _003CToOnGameThread_00603_003Em__8D;
			}
			return _003CToOnGameThread_003Ec__AnonStorey._003C_003Em__8E;
		}

		[CompilerGenerated]
		private static void _003CToOnGameThread_00601_003Em__89<T>(T P_0)
		{
		}

		[CompilerGenerated]
		private static void _003CToOnGameThread_00602_003Em__8B<T1, T2>(T1 P_0, T2 P_1)
		{
		}

		[CompilerGenerated]
		private static void _003CToOnGameThread_00603_003Em__8D<T1, T2, T3>(T1 P_0, T2 P_1, T3 P_2)
		{
		}
	}
}
