using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScorePage : BaseReferenceHolder
	{
		internal NativeScorePage(IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			ScorePage.ScorePage_Dispose(selfPointer);
		}

		internal Types.LeaderboardCollection GetCollection()
		{
			return ScorePage.ScorePage_Collection(SelfPtr());
		}

		private UIntPtr Length()
		{
			return ScorePage.ScorePage_Entries_Length(SelfPtr());
		}

		private NativeScoreEntry GetElement(UIntPtr index)
		{
			if (index.ToUInt64() >= Length().ToUInt64())
			{
				throw new ArgumentOutOfRangeException();
			}
			return new NativeScoreEntry(ScorePage.ScorePage_Entries_GetElement(SelfPtr(), index));
		}

		public IEnumerator<NativeScoreEntry> GetEnumerator()
		{
			return PInvokeUtilities.ToEnumerator(ScorePage.ScorePage_Entries_Length(SelfPtr()), _003CGetEnumerator_003Em__151);
		}

		internal bool HasNextScorePage()
		{
			return ScorePage.ScorePage_HasNextScorePage(SelfPtr());
		}

		internal bool HasPrevScorePage()
		{
			return ScorePage.ScorePage_HasPreviousScorePage(SelfPtr());
		}

		internal NativeScorePageToken GetNextScorePageToken()
		{
			return new NativeScorePageToken(ScorePage.ScorePage_NextScorePageToken(SelfPtr()));
		}

		internal NativeScorePageToken GetPreviousScorePageToken()
		{
			return new NativeScorePageToken(ScorePage.ScorePage_PreviousScorePageToken(SelfPtr()));
		}

		internal bool Valid()
		{
			return ScorePage.ScorePage_Valid(SelfPtr());
		}

		internal Types.LeaderboardTimeSpan GetTimeSpan()
		{
			return ScorePage.ScorePage_TimeSpan(SelfPtr());
		}

		internal Types.LeaderboardStart GetStart()
		{
			return ScorePage.ScorePage_Start(SelfPtr());
		}

		internal string GetLeaderboardId()
		{
			return PInvokeUtilities.OutParamsToString(_003CGetLeaderboardId_003Em__152);
		}

		internal static NativeScorePage FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeScorePage(pointer);
		}

		[CompilerGenerated]
		private NativeScoreEntry _003CGetEnumerator_003Em__151(UIntPtr index)
		{
			return GetElement(index);
		}

		[CompilerGenerated]
		private UIntPtr _003CGetLeaderboardId_003Em__152(StringBuilder out_string, UIntPtr out_size)
		{
			return ScorePage.ScorePage_LeaderboardId(SelfPtr(), out_string, out_size);
		}
	}
}
