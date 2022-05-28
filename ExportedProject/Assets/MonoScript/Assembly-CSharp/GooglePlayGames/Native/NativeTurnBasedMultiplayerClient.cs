using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	public class NativeTurnBasedMultiplayerClient : ITurnBasedMultiplayerClient
	{
		[CompilerGenerated]
		private sealed class _003CCreateQuickMatch_003Ec__AnonStorey253
		{
			internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

			internal void _003C_003Em__FD(UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
			{
				callback(status == UIStatus.Valid, match);
			}
		}

		[CompilerGenerated]
		private sealed class _003CCreateWithInvitationScreen_003Ec__AnonStorey254
		{
			internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

			internal void _003C_003Em__FE(UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
			{
				callback(status == UIStatus.Valid, match);
			}
		}

		[CompilerGenerated]
		private sealed class _003CCreateWithInvitationScreen_003Ec__AnonStorey255
		{
			internal Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

			internal uint variant;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__FF(PlayerSelectUIResponse result)
			{
				if (result.Status() != CommonErrorStatus.UIStatus.VALID)
				{
					callback((UIStatus)result.Status(), null);
					return;
				}
				using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder turnBasedMatchConfigBuilder = GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
				{
					turnBasedMatchConfigBuilder.PopulateFromUIResponse(result).SetVariant(variant);
					using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig config = turnBasedMatchConfigBuilder.Build())
					{
						_003C_003Ef__this.mTurnBasedManager.CreateMatch(config, _003C_003Ef__this.BridgeMatchToUserCallback(callback));
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetAllInvitations_003Ec__AnonStorey256
		{
			internal Action<Invitation[]> callback;

			internal void _003C_003Em__100(TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				Invitation[] array = new Invitation[allMatches.InvitationCount()];
				int num = 0;
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in allMatches.Invitations())
				{
					array[num++] = item.AsInvitation();
				}
				callback(array);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetAllMatches_003Ec__AnonStorey257
		{
			internal Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[]> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__101(TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				int num = allMatches.MyTurnMatchesCount() + allMatches.TheirTurnMatchesCount() + allMatches.CompletedMatchesCount();
				GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[] array = new GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[num];
				int num2 = 0;
				foreach (NativeTurnBasedMatch item in allMatches.MyTurnMatches())
				{
					array[num2++] = item.AsTurnBasedMatch(_003C_003Ef__this.mNativeClient.GetUserId());
				}
				foreach (NativeTurnBasedMatch item2 in allMatches.TheirTurnMatches())
				{
					array[num2++] = item2.AsTurnBasedMatch(_003C_003Ef__this.mNativeClient.GetUserId());
				}
				foreach (NativeTurnBasedMatch item3 in allMatches.CompletedMatches())
				{
					array[num2++] = item3.AsTurnBasedMatch(_003C_003Ef__this.mNativeClient.GetUserId());
				}
				callback(array);
			}
		}

		[CompilerGenerated]
		private sealed class _003CBridgeMatchToUserCallback_003Ec__AnonStorey258
		{
			internal Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> userCallback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__102(TurnBasedManager.TurnBasedMatchResponse callbackResult)
			{
				using (NativeTurnBasedMatch nativeTurnBasedMatch = callbackResult.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						UIStatus arg = UIStatus.InternalError;
						switch (callbackResult.ResponseStatus())
						{
						case CommonErrorStatus.MultiplayerStatus.VALID:
							arg = UIStatus.Valid;
							break;
						case CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE:
							arg = UIStatus.Valid;
							break;
						case CommonErrorStatus.MultiplayerStatus.ERROR_INTERNAL:
							arg = UIStatus.InternalError;
							break;
						case CommonErrorStatus.MultiplayerStatus.ERROR_NOT_AUTHORIZED:
							arg = UIStatus.NotAuthorized;
							break;
						case CommonErrorStatus.MultiplayerStatus.ERROR_VERSION_UPDATE_REQUIRED:
							arg = UIStatus.VersionUpdateRequired;
							break;
						case CommonErrorStatus.MultiplayerStatus.ERROR_TIMEOUT:
							arg = UIStatus.Timeout;
							break;
						}
						userCallback(arg, null);
					}
					else
					{
						GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch turnBasedMatch = nativeTurnBasedMatch.AsTurnBasedMatch(_003C_003Ef__this.mNativeClient.GetUserId());
						Logger.d("Passing converted match to user callback:" + turnBasedMatch);
						userCallback(UIStatus.Valid, turnBasedMatch);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CAcceptFromInbox_003Ec__AnonStorey259
		{
			internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__103(TurnBasedManager.MatchInboxUIResponse callbackResult)
			{
				using (NativeTurnBasedMatch nativeTurnBasedMatch = callbackResult.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						callback(false, null);
						return;
					}
					GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch turnBasedMatch = nativeTurnBasedMatch.AsTurnBasedMatch(_003C_003Ef__this.mNativeClient.GetUserId());
					Logger.d("Passing converted match to user callback:" + turnBasedMatch);
					callback(true, turnBasedMatch);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CAcceptInvitation_003Ec__AnonStorey25A
		{
			internal string invitationId;

			internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__104(GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
			{
				if (invitation == null)
				{
					Logger.e("Could not find invitation with id " + invitationId);
					callback(false, null);
				}
				else
				{
					_003C_003Ef__this.mTurnBasedManager.AcceptInvitation(invitation, _003C_003Ef__this.BridgeMatchToUserCallback(_003C_003Em__113));
				}
			}

			internal void _003C_003Em__113(UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
			{
				callback(status == UIStatus.Valid, match);
			}
		}

		[CompilerGenerated]
		private sealed class _003CFindInvitationWithId_003Ec__AnonStorey25B
		{
			internal Action<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback;

			internal string invitationId;

			internal void _003C_003Em__105(TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				if (allMatches.Status() <= (CommonErrorStatus.MultiplayerStatus)0)
				{
					callback(null);
					return;
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in allMatches.Invitations())
				{
					using (item)
					{
						if (item.Id().Equals(invitationId))
						{
							callback(item);
							return;
						}
					}
				}
				callback(null);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRegisterMatchDelegate_003Ec__AnonStorey25C
		{
			internal MatchDelegate del;

			internal void _003C_003Em__106(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, bool autoLaunch)
			{
				del(match, autoLaunch);
			}
		}

		[CompilerGenerated]
		private sealed class _003CHandleMatchEvent_003Ec__AnonStorey25D
		{
			internal Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> currentDelegate;

			internal NativeTurnBasedMatch match;

			internal bool shouldAutolaunch;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__107()
			{
				currentDelegate(match.AsTurnBasedMatch(_003C_003Ef__this.mNativeClient.GetUserId()), shouldAutolaunch);
				match.ForgetMe();
			}
		}

		[CompilerGenerated]
		private sealed class _003CTakeTurn_003Ec__AnonStorey25E
		{
			internal byte[] data;

			internal Action<bool> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__108(GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch)
			{
				_003C_003Ef__this.mTurnBasedManager.TakeTurn(foundMatch, data, pendingParticipant, _003C_003Em__114);
			}

			internal void _003C_003Em__114(TurnBasedManager.TurnBasedMatchResponse result)
			{
				if (result.RequestSucceeded())
				{
					callback(true);
					return;
				}
				Logger.d("Taking turn failed: " + result.ResponseStatus());
				callback(false);
			}
		}

		[CompilerGenerated]
		private sealed class _003CFindEqualVersionMatch_003Ec__AnonStorey25F
		{
			internal GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match;

			internal Action<bool> onFailure;

			internal Action<NativeTurnBasedMatch> onVersionMatch;

			internal void _003C_003Em__109(TurnBasedManager.TurnBasedMatchResponse response)
			{
				using (NativeTurnBasedMatch nativeTurnBasedMatch = response.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						Logger.e(string.Format("Could not find match {0}", match.MatchId));
						onFailure(false);
					}
					else if (nativeTurnBasedMatch.Version() != match.Version)
					{
						Logger.e(string.Format("Attempted to update a stale version of the match. Expected version was {0} but current version is {1}.", match.Version, nativeTurnBasedMatch.Version()));
						onFailure(false);
					}
					else
					{
						onVersionMatch(nativeTurnBasedMatch);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey260
		{
			internal string participantId;

			internal Action<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, NativeTurnBasedMatch> onFoundParticipantAndMatch;

			internal GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match;

			internal Action<bool> onFailure;

			internal void _003C_003Em__10A(NativeTurnBasedMatch foundMatch)
			{
				//Discarded unreachable code: IL_0023
				if (participantId == null)
				{
					using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant arg = GooglePlayGames.Native.PInvoke.MultiplayerParticipant.AutomatchingSentinel())
					{
						onFoundParticipantAndMatch(arg, foundMatch);
						return;
					}
				}
				using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = foundMatch.ParticipantWithId(participantId))
				{
					if (multiplayerParticipant == null)
					{
						Logger.e(string.Format("Located match {0} but desired participant with ID {1} could not be found", match.MatchId, participantId));
						onFailure(false);
					}
					else
					{
						onFoundParticipantAndMatch(multiplayerParticipant, foundMatch);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CFinish_003Ec__AnonStorey261
		{
			internal MatchOutcome outcome;

			internal Action<bool> callback;

			internal byte[] data;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__10B(NativeTurnBasedMatch foundMatch)
			{
				GooglePlayGames.Native.PInvoke.ParticipantResults participantResults = foundMatch.Results();
				foreach (string participantId in outcome.ParticipantIds)
				{
					Types.MatchResult matchResult = ResultToMatchResult(outcome.GetResultFor(participantId));
					uint placementFor = outcome.GetPlacementFor(participantId);
					if (participantResults.HasResultsForParticipant(participantId))
					{
						Types.MatchResult matchResult2 = participantResults.ResultsForParticipant(participantId);
						uint num = participantResults.PlacingForParticipant(participantId);
						if (matchResult != matchResult2 || placementFor != num)
						{
							Logger.e(string.Format("Attempted to override existing results for participant {0}: Placing {1}, Result {2}", participantId, num, matchResult2));
							callback(false);
							return;
						}
					}
					else
					{
						GooglePlayGames.Native.PInvoke.ParticipantResults participantResults2 = participantResults;
						participantResults = participantResults2.WithResult(participantId, placementFor, matchResult);
						participantResults2.Dispose();
					}
				}
				_003C_003Ef__this.mTurnBasedManager.FinishMatchDuringMyTurn(foundMatch, data, participantResults, _003C_003Em__115);
			}

			internal void _003C_003Em__115(TurnBasedManager.TurnBasedMatchResponse response)
			{
				callback(response.RequestSucceeded());
			}
		}

		[CompilerGenerated]
		private sealed class _003CAcknowledgeFinished_003Ec__AnonStorey262
		{
			internal Action<bool> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__10C(NativeTurnBasedMatch foundMatch)
			{
				_003C_003Ef__this.mTurnBasedManager.ConfirmPendingCompletion(foundMatch, _003C_003Em__116);
			}

			internal void _003C_003Em__116(TurnBasedManager.TurnBasedMatchResponse response)
			{
				callback(response.RequestSucceeded());
			}
		}

		[CompilerGenerated]
		private sealed class _003CLeave_003Ec__AnonStorey263
		{
			internal Action<bool> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__10D(NativeTurnBasedMatch foundMatch)
			{
				_003C_003Ef__this.mTurnBasedManager.LeaveMatchDuringTheirTurn(foundMatch, _003C_003Em__117);
			}

			internal void _003C_003Em__117(CommonErrorStatus.MultiplayerStatus status)
			{
				callback(status > (CommonErrorStatus.MultiplayerStatus)0);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLeaveDuringTurn_003Ec__AnonStorey264
		{
			internal Action<bool> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__10E(GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch)
			{
				_003C_003Ef__this.mTurnBasedManager.LeaveDuringMyTurn(foundMatch, pendingParticipant, _003C_003Em__118);
			}

			internal void _003C_003Em__118(CommonErrorStatus.MultiplayerStatus status)
			{
				callback(status > (CommonErrorStatus.MultiplayerStatus)0);
			}
		}

		[CompilerGenerated]
		private sealed class _003CCancel_003Ec__AnonStorey265
		{
			internal Action<bool> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__10F(NativeTurnBasedMatch foundMatch)
			{
				_003C_003Ef__this.mTurnBasedManager.CancelMatch(foundMatch, _003C_003Em__119);
			}

			internal void _003C_003Em__119(CommonErrorStatus.MultiplayerStatus status)
			{
				callback(status > (CommonErrorStatus.MultiplayerStatus)0);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRematch_003Ec__AnonStorey266
		{
			internal Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback;

			internal NativeTurnBasedMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__110(bool failed)
			{
				callback(false, null);
			}

			internal void _003C_003Em__111(NativeTurnBasedMatch foundMatch)
			{
				_003C_003Ef__this.mTurnBasedManager.Rematch(foundMatch, _003C_003Ef__this.BridgeMatchToUserCallback(_003C_003Em__11A));
			}

			internal void _003C_003Em__11A(UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch m)
			{
				callback(status == UIStatus.Valid, m);
			}
		}

		private readonly TurnBasedManager mTurnBasedManager;

		private readonly NativeClient mNativeClient;

		private volatile Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> mMatchDelegate;

		internal NativeTurnBasedMultiplayerClient(NativeClient nativeClient, TurnBasedManager manager)
		{
			mTurnBasedManager = manager;
			mNativeClient = nativeClient;
		}

		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			CreateQuickMatch(minOpponents, maxOpponents, variant, 0uL, callback);
		}

		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			_003CCreateQuickMatch_003Ec__AnonStorey253 _003CCreateQuickMatch_003Ec__AnonStorey = new _003CCreateQuickMatch_003Ec__AnonStorey253();
			_003CCreateQuickMatch_003Ec__AnonStorey.callback = callback;
			_003CCreateQuickMatch_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CCreateQuickMatch_003Ec__AnonStorey.callback);
			using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder turnBasedMatchConfigBuilder = GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
			{
				turnBasedMatchConfigBuilder.SetVariant(variant).SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents)
					.SetExclusiveBitMask(exclusiveBitmask);
				using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig config = turnBasedMatchConfigBuilder.Build())
				{
					mTurnBasedManager.CreateMatch(config, BridgeMatchToUserCallback(_003CCreateQuickMatch_003Ec__AnonStorey._003C_003Em__FD));
				}
			}
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			_003CCreateWithInvitationScreen_003Ec__AnonStorey254 _003CCreateWithInvitationScreen_003Ec__AnonStorey = new _003CCreateWithInvitationScreen_003Ec__AnonStorey254();
			_003CCreateWithInvitationScreen_003Ec__AnonStorey.callback = callback;
			CreateWithInvitationScreen(minOpponents, maxOpponents, variant, _003CCreateWithInvitationScreen_003Ec__AnonStorey._003C_003Em__FE);
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			_003CCreateWithInvitationScreen_003Ec__AnonStorey255 _003CCreateWithInvitationScreen_003Ec__AnonStorey = new _003CCreateWithInvitationScreen_003Ec__AnonStorey255();
			_003CCreateWithInvitationScreen_003Ec__AnonStorey.callback = callback;
			_003CCreateWithInvitationScreen_003Ec__AnonStorey.variant = variant;
			_003CCreateWithInvitationScreen_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CCreateWithInvitationScreen_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CCreateWithInvitationScreen_003Ec__AnonStorey.callback);
			mTurnBasedManager.ShowPlayerSelectUI(minOpponents, maxOpponents, true, _003CCreateWithInvitationScreen_003Ec__AnonStorey._003C_003Em__FF);
		}

		public void GetAllInvitations(Action<Invitation[]> callback)
		{
			_003CGetAllInvitations_003Ec__AnonStorey256 _003CGetAllInvitations_003Ec__AnonStorey = new _003CGetAllInvitations_003Ec__AnonStorey256();
			_003CGetAllInvitations_003Ec__AnonStorey.callback = callback;
			mTurnBasedManager.GetAllTurnbasedMatches(_003CGetAllInvitations_003Ec__AnonStorey._003C_003Em__100);
		}

		public void GetAllMatches(Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[]> callback)
		{
			_003CGetAllMatches_003Ec__AnonStorey257 _003CGetAllMatches_003Ec__AnonStorey = new _003CGetAllMatches_003Ec__AnonStorey257();
			_003CGetAllMatches_003Ec__AnonStorey.callback = callback;
			_003CGetAllMatches_003Ec__AnonStorey._003C_003Ef__this = this;
			mTurnBasedManager.GetAllTurnbasedMatches(_003CGetAllMatches_003Ec__AnonStorey._003C_003Em__101);
		}

		private Action<TurnBasedManager.TurnBasedMatchResponse> BridgeMatchToUserCallback(Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> userCallback)
		{
			_003CBridgeMatchToUserCallback_003Ec__AnonStorey258 _003CBridgeMatchToUserCallback_003Ec__AnonStorey = new _003CBridgeMatchToUserCallback_003Ec__AnonStorey258();
			_003CBridgeMatchToUserCallback_003Ec__AnonStorey.userCallback = userCallback;
			_003CBridgeMatchToUserCallback_003Ec__AnonStorey._003C_003Ef__this = this;
			return _003CBridgeMatchToUserCallback_003Ec__AnonStorey._003C_003Em__102;
		}

		public void AcceptFromInbox(Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			_003CAcceptFromInbox_003Ec__AnonStorey259 _003CAcceptFromInbox_003Ec__AnonStorey = new _003CAcceptFromInbox_003Ec__AnonStorey259();
			_003CAcceptFromInbox_003Ec__AnonStorey.callback = callback;
			_003CAcceptFromInbox_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CAcceptFromInbox_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CAcceptFromInbox_003Ec__AnonStorey.callback);
			mTurnBasedManager.ShowInboxUI(_003CAcceptFromInbox_003Ec__AnonStorey._003C_003Em__103);
		}

		public void AcceptInvitation(string invitationId, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			_003CAcceptInvitation_003Ec__AnonStorey25A _003CAcceptInvitation_003Ec__AnonStorey25A = new _003CAcceptInvitation_003Ec__AnonStorey25A();
			_003CAcceptInvitation_003Ec__AnonStorey25A.invitationId = invitationId;
			_003CAcceptInvitation_003Ec__AnonStorey25A.callback = callback;
			_003CAcceptInvitation_003Ec__AnonStorey25A._003C_003Ef__this = this;
			_003CAcceptInvitation_003Ec__AnonStorey25A.callback = Callbacks.AsOnGameThreadCallback(_003CAcceptInvitation_003Ec__AnonStorey25A.callback);
			FindInvitationWithId(_003CAcceptInvitation_003Ec__AnonStorey25A.invitationId, _003CAcceptInvitation_003Ec__AnonStorey25A._003C_003Em__104);
		}

		private void FindInvitationWithId(string invitationId, Action<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback)
		{
			_003CFindInvitationWithId_003Ec__AnonStorey25B _003CFindInvitationWithId_003Ec__AnonStorey25B = new _003CFindInvitationWithId_003Ec__AnonStorey25B();
			_003CFindInvitationWithId_003Ec__AnonStorey25B.callback = callback;
			_003CFindInvitationWithId_003Ec__AnonStorey25B.invitationId = invitationId;
			mTurnBasedManager.GetAllTurnbasedMatches(_003CFindInvitationWithId_003Ec__AnonStorey25B._003C_003Em__105);
		}

		public void RegisterMatchDelegate(MatchDelegate del)
		{
			_003CRegisterMatchDelegate_003Ec__AnonStorey25C _003CRegisterMatchDelegate_003Ec__AnonStorey25C = new _003CRegisterMatchDelegate_003Ec__AnonStorey25C();
			_003CRegisterMatchDelegate_003Ec__AnonStorey25C.del = del;
			if (_003CRegisterMatchDelegate_003Ec__AnonStorey25C.del == null)
			{
				mMatchDelegate = null;
			}
			else
			{
				mMatchDelegate = Callbacks.AsOnGameThreadCallback<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool>(_003CRegisterMatchDelegate_003Ec__AnonStorey25C._003C_003Em__106);
			}
		}

		internal void HandleMatchEvent(Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match)
		{
			_003CHandleMatchEvent_003Ec__AnonStorey25D _003CHandleMatchEvent_003Ec__AnonStorey25D = new _003CHandleMatchEvent_003Ec__AnonStorey25D();
			_003CHandleMatchEvent_003Ec__AnonStorey25D.match = match;
			_003CHandleMatchEvent_003Ec__AnonStorey25D._003C_003Ef__this = this;
			_003CHandleMatchEvent_003Ec__AnonStorey25D.currentDelegate = mMatchDelegate;
			if (_003CHandleMatchEvent_003Ec__AnonStorey25D.currentDelegate != null)
			{
				if (eventType == Types.MultiplayerEvent.REMOVED)
				{
					Logger.d("Ignoring REMOVE event for match " + matchId);
					return;
				}
				_003CHandleMatchEvent_003Ec__AnonStorey25D.shouldAutolaunch = eventType == Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
				_003CHandleMatchEvent_003Ec__AnonStorey25D.match.ReferToMe();
				Callbacks.AsCoroutine(WaitForLogin(_003CHandleMatchEvent_003Ec__AnonStorey25D._003C_003Em__107));
			}
		}

		private IEnumerator WaitForLogin(Action method)
		{
			if (string.IsNullOrEmpty(mNativeClient.GetUserId()))
			{
				yield return null;
			}
			method();
		}

		public void TakeTurn(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, string pendingParticipantId, Action<bool> callback)
		{
			_003CTakeTurn_003Ec__AnonStorey25E _003CTakeTurn_003Ec__AnonStorey25E = new _003CTakeTurn_003Ec__AnonStorey25E();
			_003CTakeTurn_003Ec__AnonStorey25E.data = data;
			_003CTakeTurn_003Ec__AnonStorey25E.callback = callback;
			_003CTakeTurn_003Ec__AnonStorey25E._003C_003Ef__this = this;
			Logger.describe(_003CTakeTurn_003Ec__AnonStorey25E.data);
			_003CTakeTurn_003Ec__AnonStorey25E.callback = Callbacks.AsOnGameThreadCallback(_003CTakeTurn_003Ec__AnonStorey25E.callback);
			FindEqualVersionMatchWithParticipant(match, pendingParticipantId, _003CTakeTurn_003Ec__AnonStorey25E.callback, _003CTakeTurn_003Ec__AnonStorey25E._003C_003Em__108);
		}

		private void FindEqualVersionMatch(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> onFailure, Action<NativeTurnBasedMatch> onVersionMatch)
		{
			_003CFindEqualVersionMatch_003Ec__AnonStorey25F _003CFindEqualVersionMatch_003Ec__AnonStorey25F = new _003CFindEqualVersionMatch_003Ec__AnonStorey25F();
			_003CFindEqualVersionMatch_003Ec__AnonStorey25F.match = match;
			_003CFindEqualVersionMatch_003Ec__AnonStorey25F.onFailure = onFailure;
			_003CFindEqualVersionMatch_003Ec__AnonStorey25F.onVersionMatch = onVersionMatch;
			mTurnBasedManager.GetMatch(_003CFindEqualVersionMatch_003Ec__AnonStorey25F.match.MatchId, _003CFindEqualVersionMatch_003Ec__AnonStorey25F._003C_003Em__109);
		}

		private void FindEqualVersionMatchWithParticipant(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string participantId, Action<bool> onFailure, Action<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, NativeTurnBasedMatch> onFoundParticipantAndMatch)
		{
			_003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey260 _003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey = new _003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey260();
			_003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey.participantId = participantId;
			_003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey.onFoundParticipantAndMatch = onFoundParticipantAndMatch;
			_003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey.match = match;
			_003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey.onFailure = onFailure;
			FindEqualVersionMatch(_003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey.match, _003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey.onFailure, _003CFindEqualVersionMatchWithParticipant_003Ec__AnonStorey._003C_003Em__10A);
		}

		public int GetMaxMatchDataSize()
		{
			throw new NotImplementedException();
		}

		public void Finish(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, MatchOutcome outcome, Action<bool> callback)
		{
			_003CFinish_003Ec__AnonStorey261 _003CFinish_003Ec__AnonStorey = new _003CFinish_003Ec__AnonStorey261();
			_003CFinish_003Ec__AnonStorey.outcome = outcome;
			_003CFinish_003Ec__AnonStorey.callback = callback;
			_003CFinish_003Ec__AnonStorey.data = data;
			_003CFinish_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CFinish_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CFinish_003Ec__AnonStorey.callback);
			FindEqualVersionMatch(match, _003CFinish_003Ec__AnonStorey.callback, _003CFinish_003Ec__AnonStorey._003C_003Em__10B);
		}

		private static Types.MatchResult ResultToMatchResult(MatchOutcome.ParticipantResult result)
		{
			switch (result)
			{
			case MatchOutcome.ParticipantResult.Loss:
				return Types.MatchResult.LOSS;
			case MatchOutcome.ParticipantResult.None:
				return Types.MatchResult.NONE;
			case MatchOutcome.ParticipantResult.Tie:
				return Types.MatchResult.TIE;
			case MatchOutcome.ParticipantResult.Win:
				return Types.MatchResult.WIN;
			default:
				Logger.e("Received unknown ParticipantResult " + result);
				return Types.MatchResult.NONE;
			}
		}

		public void AcknowledgeFinished(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			_003CAcknowledgeFinished_003Ec__AnonStorey262 _003CAcknowledgeFinished_003Ec__AnonStorey = new _003CAcknowledgeFinished_003Ec__AnonStorey262();
			_003CAcknowledgeFinished_003Ec__AnonStorey.callback = callback;
			_003CAcknowledgeFinished_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CAcknowledgeFinished_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CAcknowledgeFinished_003Ec__AnonStorey.callback);
			FindEqualVersionMatch(match, _003CAcknowledgeFinished_003Ec__AnonStorey.callback, _003CAcknowledgeFinished_003Ec__AnonStorey._003C_003Em__10C);
		}

		public void Leave(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			_003CLeave_003Ec__AnonStorey263 _003CLeave_003Ec__AnonStorey = new _003CLeave_003Ec__AnonStorey263();
			_003CLeave_003Ec__AnonStorey.callback = callback;
			_003CLeave_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CLeave_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CLeave_003Ec__AnonStorey.callback);
			FindEqualVersionMatch(match, _003CLeave_003Ec__AnonStorey.callback, _003CLeave_003Ec__AnonStorey._003C_003Em__10D);
		}

		public void LeaveDuringTurn(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string pendingParticipantId, Action<bool> callback)
		{
			_003CLeaveDuringTurn_003Ec__AnonStorey264 _003CLeaveDuringTurn_003Ec__AnonStorey = new _003CLeaveDuringTurn_003Ec__AnonStorey264();
			_003CLeaveDuringTurn_003Ec__AnonStorey.callback = callback;
			_003CLeaveDuringTurn_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CLeaveDuringTurn_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CLeaveDuringTurn_003Ec__AnonStorey.callback);
			FindEqualVersionMatchWithParticipant(match, pendingParticipantId, _003CLeaveDuringTurn_003Ec__AnonStorey.callback, _003CLeaveDuringTurn_003Ec__AnonStorey._003C_003Em__10E);
		}

		public void Cancel(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			_003CCancel_003Ec__AnonStorey265 _003CCancel_003Ec__AnonStorey = new _003CCancel_003Ec__AnonStorey265();
			_003CCancel_003Ec__AnonStorey.callback = callback;
			_003CCancel_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CCancel_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CCancel_003Ec__AnonStorey.callback);
			FindEqualVersionMatch(match, _003CCancel_003Ec__AnonStorey.callback, _003CCancel_003Ec__AnonStorey._003C_003Em__10F);
		}

		public void Rematch(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			_003CRematch_003Ec__AnonStorey266 _003CRematch_003Ec__AnonStorey = new _003CRematch_003Ec__AnonStorey266();
			_003CRematch_003Ec__AnonStorey.callback = callback;
			_003CRematch_003Ec__AnonStorey._003C_003Ef__this = this;
			_003CRematch_003Ec__AnonStorey.callback = Callbacks.AsOnGameThreadCallback(_003CRematch_003Ec__AnonStorey.callback);
			FindEqualVersionMatch(match, _003CRematch_003Ec__AnonStorey._003C_003Em__110, _003CRematch_003Ec__AnonStorey._003C_003Em__111);
		}

		public void DeclineInvitation(string invitationId)
		{
			FindInvitationWithId(invitationId, _003CDeclineInvitation_003Em__112);
		}

		[CompilerGenerated]
		private void _003CDeclineInvitation_003Em__112(GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			if (invitation != null)
			{
				mTurnBasedManager.DeclineInvitation(invitation);
			}
		}
	}
}
