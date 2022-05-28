using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class PlayerSelectUIResponse : BaseReferenceHolder, IEnumerable, IEnumerable<string>
	{
		[CompilerGenerated]
		private sealed class _003CPlayerIdAtIndex_003Ec__AnonStorey272
		{
			internal UIntPtr index;

			internal PlayerSelectUIResponse _003C_003Ef__this;

			internal UIntPtr _003C_003Em__162(StringBuilder out_string, UIntPtr size)
			{
				return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_GetElement(_003C_003Ef__this.SelfPtr(), index, out_string, size);
			}
		}

		internal PlayerSelectUIResponse(IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal CommonErrorStatus.UIStatus Status()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetStatus(SelfPtr());
		}

		private string PlayerIdAtIndex(UIntPtr index)
		{
			_003CPlayerIdAtIndex_003Ec__AnonStorey272 _003CPlayerIdAtIndex_003Ec__AnonStorey = new _003CPlayerIdAtIndex_003Ec__AnonStorey272();
			_003CPlayerIdAtIndex_003Ec__AnonStorey.index = index;
			_003CPlayerIdAtIndex_003Ec__AnonStorey._003C_003Ef__this = this;
			return PInvokeUtilities.OutParamsToString(_003CPlayerIdAtIndex_003Ec__AnonStorey._003C_003Em__162);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return PInvokeUtilities.ToEnumerator(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_Length(SelfPtr()), PlayerIdAtIndex);
		}

		internal uint MinimumAutomatchingPlayers()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMinimumAutomatchingPlayers(SelfPtr());
		}

		internal uint MaximumAutomatchingPlayers()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMaximumAutomatchingPlayers(SelfPtr());
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_Dispose(selfPointer);
		}

		internal static PlayerSelectUIResponse FromPointer(IntPtr pointer)
		{
			if (PInvokeUtilities.IsNull(pointer))
			{
				return null;
			}
			return new PlayerSelectUIResponse(pointer);
		}
	}
}
