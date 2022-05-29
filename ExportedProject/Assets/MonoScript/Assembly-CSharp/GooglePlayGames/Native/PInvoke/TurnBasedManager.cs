using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class TurnBasedManager
	{
		private readonly GooglePlayGames.Native.PInvoke.GameServices mGameServices;

		internal TurnBasedManager(GooglePlayGames.Native.PInvoke.GameServices services)
		{
			this.mGameServices = services;
		}

		internal void AcceptInvitation(GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			IntPtr intPtr = invitation.AsPointer();
			Logger.d(string.Concat("Accepting invitation: ", intPtr.ToInt64()));
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_AcceptInvitation(this.mGameServices.AsHandle(), invitation.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		internal void CancelMatch(NativeTurnBasedMatch match, Action<CommonErrorStatus.MultiplayerStatus> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_CancelMatch(this.mGameServices.AsHandle(), match.AsPointer(), new TurnBasedMultiplayerManager.MultiplayerStatusCallback(TurnBasedManager.InternalMultiplayerStatusCallback), Callbacks.ToIntPtr(callback));
		}

		internal void ConfirmPendingCompletion(NativeTurnBasedMatch match, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ConfirmPendingCompletion(this.mGameServices.AsHandle(), match.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		internal void CreateMatch(GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig config, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_CreateTurnBasedMatch(this.mGameServices.AsHandle(), config.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		internal void DeclineInvitation(GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_DeclineInvitation(this.mGameServices.AsHandle(), invitation.AsPointer());
		}

		internal void FinishMatchDuringMyTurn(NativeTurnBasedMatch match, byte[] data, GooglePlayGames.Native.PInvoke.ParticipantResults results, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FinishMatchDuringMyTurn(this.mGameServices.AsHandle(), match.AsPointer(), data, new UIntPtr((uint)data.Length), results.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		internal void GetAllTurnbasedMatches(Action<TurnBasedManager.TurnBasedMatchesResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FetchMatches(this.mGameServices.AsHandle(), new TurnBasedMultiplayerManager.TurnBasedMatchesCallback(TurnBasedManager.InternalTurnBasedMatchesCallback), Callbacks.ToIntPtr<TurnBasedManager.TurnBasedMatchesResponse>(callback, new Func<IntPtr, TurnBasedManager.TurnBasedMatchesResponse>(TurnBasedManager.TurnBasedMatchesResponse.FromPointer)));
		}

		internal void GetMatch(string matchId, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_FetchMatch(this.mGameServices.AsHandle(), matchId, new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.MatchInboxUICallback))]
		internal static void InternalMatchInboxUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("TurnBasedManager#MatchInboxUICallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.MultiplayerStatusCallback))]
		internal static void InternalMultiplayerStatusCallback(CommonErrorStatus.MultiplayerStatus status, IntPtr data)
		{
			Logger.d(string.Concat("InternalMultiplayerStatusCallback: ", status));
			Action<CommonErrorStatus.MultiplayerStatus> tempCallback = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.MultiplayerStatus>>(data);
			try
			{
				tempCallback(status);
			}
			catch (Exception exception)
			{
				Logger.e(string.Concat("Error encountered executing InternalMultiplayerStatusCallback. Smothering to avoid passing exception into Native: ", exception));
			}
		}

		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.PlayerSelectUICallback))]
		internal static void InternalPlayerSelectUIcallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("TurnBasedManager#PlayerSelectUICallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.TurnBasedMatchCallback))]
		internal static void InternalTurnBasedMatchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("TurnBasedManager#InternalTurnBasedMatchCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(TurnBasedMultiplayerManager.TurnBasedMatchesCallback))]
		internal static void InternalTurnBasedMatchesCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("TurnBasedManager#TurnBasedMatchesCallback", Callbacks.Type.Temporary, response, data);
		}

		internal void LeaveDuringMyTurn(NativeTurnBasedMatch match, GooglePlayGames.Native.PInvoke.MultiplayerParticipant nextParticipant, Action<CommonErrorStatus.MultiplayerStatus> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_LeaveMatchDuringMyTurn(this.mGameServices.AsHandle(), match.AsPointer(), nextParticipant.AsPointer(), new TurnBasedMultiplayerManager.MultiplayerStatusCallback(TurnBasedManager.InternalMultiplayerStatusCallback), Callbacks.ToIntPtr(callback));
		}

		internal void LeaveMatchDuringTheirTurn(NativeTurnBasedMatch match, Action<CommonErrorStatus.MultiplayerStatus> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_LeaveMatchDuringTheirTurn(this.mGameServices.AsHandle(), match.AsPointer(), new TurnBasedMultiplayerManager.MultiplayerStatusCallback(TurnBasedManager.InternalMultiplayerStatusCallback), Callbacks.ToIntPtr(callback));
		}

		internal void Rematch(NativeTurnBasedMatch match, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_Rematch(this.mGameServices.AsHandle(), match.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		internal void ShowInboxUI(Action<TurnBasedManager.MatchInboxUIResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ShowMatchInboxUI(this.mGameServices.AsHandle(), new TurnBasedMultiplayerManager.MatchInboxUICallback(TurnBasedManager.InternalMatchInboxUICallback), Callbacks.ToIntPtr<TurnBasedManager.MatchInboxUIResponse>(callback, new Func<IntPtr, TurnBasedManager.MatchInboxUIResponse>(TurnBasedManager.MatchInboxUIResponse.FromPointer)));
		}

		internal void ShowPlayerSelectUI(uint minimumPlayers, uint maxiumPlayers, bool allowAutomatching, Action<PlayerSelectUIResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_ShowPlayerSelectUI(this.mGameServices.AsHandle(), minimumPlayers, maxiumPlayers, allowAutomatching, new TurnBasedMultiplayerManager.PlayerSelectUICallback(TurnBasedManager.InternalPlayerSelectUIcallback), Callbacks.ToIntPtr<PlayerSelectUIResponse>(callback, new Func<IntPtr, PlayerSelectUIResponse>(PlayerSelectUIResponse.FromPointer)));
		}

		internal void TakeTurn(NativeTurnBasedMatch match, byte[] data, GooglePlayGames.Native.PInvoke.MultiplayerParticipant nextParticipant, Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TakeMyTurn(this.mGameServices.AsHandle(), match.AsPointer(), data, new UIntPtr((uint)data.Length), match.Results().AsPointer(), nextParticipant.AsPointer(), new TurnBasedMultiplayerManager.TurnBasedMatchCallback(TurnBasedManager.InternalTurnBasedMatchCallback), TurnBasedManager.ToCallbackPointer(callback));
		}

		private static IntPtr ToCallbackPointer(Action<TurnBasedManager.TurnBasedMatchResponse> callback)
		{
			return Callbacks.ToIntPtr<TurnBasedManager.TurnBasedMatchResponse>(callback, new Func<IntPtr, TurnBasedManager.TurnBasedMatchResponse>(TurnBasedManager.TurnBasedMatchResponse.FromPointer));
		}

		internal class MatchInboxUIResponse : BaseReferenceHolder
		{
			internal MatchInboxUIResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_Dispose(selfPointer);
			}

			internal static TurnBasedManager.MatchInboxUIResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new TurnBasedManager.MatchInboxUIResponse(pointer);
			}

			internal NativeTurnBasedMatch Match()
			{
				if (this.UiStatus() != CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_GetMatch(base.SelfPtr()));
			}

			internal CommonErrorStatus.UIStatus UiStatus()
			{
				return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_MatchInboxUIResponse_GetStatus(base.SelfPtr());
			}
		}

		internal delegate void TurnBasedMatchCallback(TurnBasedManager.TurnBasedMatchResponse response);

		internal class TurnBasedMatchesResponse : BaseReferenceHolder
		{
			internal TurnBasedMatchesResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_Dispose(base.SelfPtr());
			}

			internal IEnumerable<NativeTurnBasedMatch> CompletedMatches()
			{
				return PInvokeUtilities.ToEnumerable<NativeTurnBasedMatch>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_Length(base.SelfPtr()), (UIntPtr index) => new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_GetElement(base.SelfPtr(), index)));
			}

			internal int CompletedMatchesCount()
			{
				return (int)TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_Length(base.SelfPtr()).ToUInt32();
			}

			internal static TurnBasedManager.TurnBasedMatchesResponse FromPointer(IntPtr pointer)
			{
				if (PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new TurnBasedManager.TurnBasedMatchesResponse(pointer);
			}

			internal int InvitationCount()
			{
				return (int)TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_Length(base.SelfPtr()).ToUInt32();
			}

			internal IEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> Invitations()
			{
				return PInvokeUtilities.ToEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerInvitation>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_Length(base.SelfPtr()), (UIntPtr index) => new GooglePlayGames.Native.PInvoke.MultiplayerInvitation(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_GetElement(base.SelfPtr(), index)));
			}

			internal IEnumerable<NativeTurnBasedMatch> MyTurnMatches()
			{
				return PInvokeUtilities.ToEnumerable<NativeTurnBasedMatch>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_Length(base.SelfPtr()), (UIntPtr index) => new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_GetElement(base.SelfPtr(), index)));
			}

			internal int MyTurnMatchesCount()
			{
				return (int)TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_Length(base.SelfPtr()).ToUInt32();
			}

			internal CommonErrorStatus.MultiplayerStatus Status()
			{
				return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetStatus(base.SelfPtr());
			}

			internal IEnumerable<NativeTurnBasedMatch> TheirTurnMatches()
			{
				return PInvokeUtilities.ToEnumerable<NativeTurnBasedMatch>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_Length(base.SelfPtr()), (UIntPtr index) => new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_GetElement(base.SelfPtr(), index)));
			}

			internal int TheirTurnMatchesCount()
			{
				return (int)TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_Length(base.SelfPtr()).ToUInt32();
			}
		}

		internal class TurnBasedMatchResponse : BaseReferenceHolder
		{
			internal TurnBasedMatchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_Dispose(selfPointer);
			}

			internal static TurnBasedManager.TurnBasedMatchResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new TurnBasedManager.TurnBasedMatchResponse(pointer);
			}

			internal NativeTurnBasedMatch Match()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeTurnBasedMatch(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetMatch(base.SelfPtr()));
			}

			internal bool RequestSucceeded()
			{
				return (int)this.ResponseStatus() > 0;
			}

			internal CommonErrorStatus.MultiplayerStatus ResponseStatus()
			{
				return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetStatus(base.SelfPtr());
			}
		}
	}
}