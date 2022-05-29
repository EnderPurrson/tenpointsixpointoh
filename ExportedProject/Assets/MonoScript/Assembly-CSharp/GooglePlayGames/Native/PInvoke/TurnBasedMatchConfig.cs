using GooglePlayGames.Native.Cwrapper;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class TurnBasedMatchConfig : BaseReferenceHolder
	{
		internal TurnBasedMatchConfig(IntPtr selfPointer) : base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_Dispose(selfPointer);
		}

		internal long ExclusiveBitMask()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_ExclusiveBitMask(base.SelfPtr());
		}

		internal uint MaximumAutomatchingPlayers()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_MaximumAutomatchingPlayers(base.SelfPtr());
		}

		internal uint MinimumAutomatchingPlayers()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_MinimumAutomatchingPlayers(base.SelfPtr());
		}

		private string PlayerIdAtIndex(UIntPtr index)
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_GetElement(base.SelfPtr(), index, out_string, size));
		}

		internal IEnumerator<string> PlayerIdsToInvite()
		{
			return PInvokeUtilities.ToEnumerator<string>(GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_Length(base.SelfPtr()), new Func<UIntPtr, string>(this.PlayerIdAtIndex));
		}

		internal uint Variant()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfig.TurnBasedMatchConfig_Variant(base.SelfPtr());
		}
	}
}