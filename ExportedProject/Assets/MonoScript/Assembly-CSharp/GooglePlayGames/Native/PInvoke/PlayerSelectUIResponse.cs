using GooglePlayGames.Native.Cwrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class PlayerSelectUIResponse : BaseReferenceHolder, IEnumerable, IEnumerable<string>
	{
		internal PlayerSelectUIResponse(IntPtr selfPointer) : base(selfPointer)
		{
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

		public IEnumerator<string> GetEnumerator()
		{
			return PInvokeUtilities.ToEnumerator<string>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_Length(base.SelfPtr()), new Func<UIntPtr, string>(this.PlayerIdAtIndex));
		}

		internal uint MaximumAutomatchingPlayers()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMaximumAutomatchingPlayers(base.SelfPtr());
		}

		internal uint MinimumAutomatchingPlayers()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMinimumAutomatchingPlayers(base.SelfPtr());
		}

		private string PlayerIdAtIndex(UIntPtr index)
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_GetElement(base.SelfPtr(), index, out_string, size));
		}

		internal CommonErrorStatus.UIStatus Status()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetStatus(base.SelfPtr());
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}