using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GooglePlayGames.Native
{
	public class NativeTurnBasedMultiplayerClient : ITurnBasedMultiplayerClient
	{
		private readonly TurnBasedManager mTurnBasedManager;

		private readonly NativeClient mNativeClient;

		private volatile Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> mMatchDelegate;

		internal NativeTurnBasedMultiplayerClient(NativeClient nativeClient, TurnBasedManager manager)
		{
			this.mTurnBasedManager = manager;
			this.mNativeClient = nativeClient;
		}

		public void AcceptFromInbox(Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(action);
			this.mTurnBasedManager.ShowInboxUI((TurnBasedManager.MatchInboxUIResponse callbackResult) => {
				using (NativeTurnBasedMatch nativeTurnBasedMatch = callbackResult.Match())
				{
					if (nativeTurnBasedMatch != null)
					{
						GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch turnBasedMatch = nativeTurnBasedMatch.AsTurnBasedMatch(this.mNativeClient.GetUserId());
						Logger.d(string.Concat("Passing converted match to user callback:", turnBasedMatch));
						action(true, turnBasedMatch);
					}
					else
					{
						action(false, null);
					}
				}
			});
		}

		public void AcceptInvitation(string invitationId, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(action);
			this.FindInvitationWithId(invitationId, (GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation) => {
				if (invitation == null)
				{
					Logger.e(string.Concat("Could not find invitation with id ", invitationId));
					action(false, null);
					return;
				}
				this.mTurnBasedManager.AcceptInvitation(invitation, this.BridgeMatchToUserCallback((UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match) => action(status == UIStatus.Valid, match)));
			});
		}

		public void AcknowledgeFinished(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			Action<bool> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool>(action);
			this.FindEqualVersionMatch(match, action, (NativeTurnBasedMatch foundMatch) => this.mTurnBasedManager.ConfirmPendingCompletion(foundMatch, (TurnBasedManager.TurnBasedMatchResponse response) => action(response.RequestSucceeded())));
		}

		private Action<TurnBasedManager.TurnBasedMatchResponse> BridgeMatchToUserCallback(Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> userCallback)
		{
			// 
			// Current member / type: System.Action`1<GooglePlayGames.Native.PInvoke.TurnBasedManager/TurnBasedMatchResponse> GooglePlayGames.Native.NativeTurnBasedMultiplayerClient::BridgeMatchToUserCallback(System.Action`2<GooglePlayGames.BasicApi.UIStatus,GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>)
			// File path: c:\Users\lbert\Downloads\AF3DWBexsd0viV96e5U9-SkM_V5zvgedtPgl0ckOW0viY3BQRpH0nOQr2srRNskocOff7lYXZtSb-RdgwIBSTEfKABF0f2FHtkSZj0j6yPgtI2YdrQdKtFI\assets\bin\Data\Managed\Assembly-CSharp.dll
			// 
			// Product version: 0.9.2.0
			// Exception in: System.Action<GooglePlayGames.Native.PInvoke.TurnBasedManager/TurnBasedMatchResponse> BridgeMatchToUserCallback(System.Action<GooglePlayGames.BasicApi.UIStatus,GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>)
			// 
			// The given key '' was not present in the dictionary.
			//    at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
			//    at Telerik.JustDecompiler.Decompiler.GotoElimination.GotoCancelation.Preprocess() in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\GotoElimination\GotoCancelation.cs:line 62
			//    at Telerik.JustDecompiler.Decompiler.GotoElimination.GotoCancelation.RemoveGotoStatements() in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\GotoElimination\GotoCancelation.cs:line 43
			//    at Telerik.JustDecompiler.Decompiler.GotoElimination.GotoCancelation.Process(DecompilationContext context, BlockStatement body) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\GotoElimination\GotoCancelation.cs:line 27
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.RunInternal(MethodBody body, BlockStatement block, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 81
			//    at Telerik.JustDecompiler.Decompiler.DecompilationPipeline.Run(MethodBody body, ILanguage language) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\DecompilationPipeline.cs:line 70
			//    at Telerik.JustDecompiler.Decompiler.Extensions.RunPipeline(DecompilationPipeline pipeline, ILanguage language, MethodBody body, DecompilationContext& context) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 95
			//    at Telerik.JustDecompiler.Decompiler.Extensions.Decompile(MethodBody body, ILanguage language, DecompilationContext& context, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\Extensions.cs:line 61
			//    at Telerik.JustDecompiler.Decompiler.WriterContextServices.BaseWriterContextService.DecompileMethod(ILanguage language, MethodDefinition method, TypeSpecificContext typeContext) in D:\a\CodemerxDecompile\CodemerxDecompile\engine\JustDecompiler.Shared\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 118
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}

		public void Cancel(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			Action<bool> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool>(action);
			this.FindEqualVersionMatch(match, action, (NativeTurnBasedMatch foundMatch) => this.mTurnBasedManager.CancelMatch(foundMatch, (CommonErrorStatus.MultiplayerStatus status) => action((int)status > 0)));
		}

		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			this.CreateQuickMatch(minOpponents, maxOpponents, variant, (ulong)0, callback);
		}

		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(action);
			using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder turnBasedMatchConfigBuilder = GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
			{
				turnBasedMatchConfigBuilder.SetVariant(variant).SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetExclusiveBitMask(exclusiveBitmask);
				using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig turnBasedMatchConfig = turnBasedMatchConfigBuilder.Build())
				{
					this.mTurnBasedManager.CreateMatch(turnBasedMatchConfig, this.BridgeMatchToUserCallback((UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match) => action(status == UIStatus.Valid, match)));
				}
			}
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			this.CreateWithInvitationScreen(minOpponents, maxOpponents, variant, (UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match) => callback(status == UIStatus.Valid, match));
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> action = callback;
			action = Callbacks.AsOnGameThreadCallback<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(action);
			this.mTurnBasedManager.ShowPlayerSelectUI(minOpponents, maxOpponents, true, (PlayerSelectUIResponse result) => {
				if (result.Status() != CommonErrorStatus.UIStatus.VALID)
				{
					action(result.Status(), null);
					return;
				}
				using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder turnBasedMatchConfigBuilder = GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
				{
					turnBasedMatchConfigBuilder.PopulateFromUIResponse(result).SetVariant(variant);
					using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig turnBasedMatchConfig = turnBasedMatchConfigBuilder.Build())
					{
						this.mTurnBasedManager.CreateMatch(turnBasedMatchConfig, this.BridgeMatchToUserCallback(action));
					}
				}
			});
		}

		public void DeclineInvitation(string invitationId)
		{
			this.FindInvitationWithId(invitationId, (GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation) => {
				if (invitation == null)
				{
					return;
				}
				this.mTurnBasedManager.DeclineInvitation(invitation);
			});
		}

		private void FindEqualVersionMatch(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> onFailure, Action<NativeTurnBasedMatch> onVersionMatch)
		{
			this.mTurnBasedManager.GetMatch(match.MatchId, (TurnBasedManager.TurnBasedMatchResponse response) => {
				using (NativeTurnBasedMatch nativeTurnBasedMatch = response.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						Logger.e(string.Format("Could not find match {0}", match.MatchId));
						onFailure(false);
					}
					else if (nativeTurnBasedMatch.Version() == match.Version)
					{
						onVersionMatch(nativeTurnBasedMatch);
					}
					else
					{
						Logger.e(string.Format("Attempted to update a stale version of the match. Expected version was {0} but current version is {1}.", match.Version, nativeTurnBasedMatch.Version()));
						onFailure(false);
					}
				}
			});
		}

		private void FindEqualVersionMatchWithParticipant(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string participantId, Action<bool> onFailure, Action<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, NativeTurnBasedMatch> onFoundParticipantAndMatch)
		{
			this.FindEqualVersionMatch(match, onFailure, (NativeTurnBasedMatch foundMatch) => {
				if (participantId != null)
				{
					using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = foundMatch.ParticipantWithId(participantId))
					{
						if (multiplayerParticipant != null)
						{
							onFoundParticipantAndMatch(multiplayerParticipant, foundMatch);
						}
						else
						{
							Logger.e(string.Format("Located match {0} but desired participant with ID {1} could not be found", match.MatchId, participantId));
							onFailure(false);
						}
					}
				}
				else
				{
					using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant1 = GooglePlayGames.Native.PInvoke.MultiplayerParticipant.AutomatchingSentinel())
					{
						onFoundParticipantAndMatch(multiplayerParticipant1, foundMatch);
					}
				}
			});
		}

		private void FindInvitationWithId(string invitationId, Action<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback)
		{
			this.mTurnBasedManager.GetAllTurnbasedMatches((TurnBasedManager.TurnBasedMatchesResponse allMatches) => {
				if ((int)allMatches.Status() <= 0)
				{
					callback(null);
					return;
				}
				IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = allMatches.Invitations().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GooglePlayGames.Native.PInvoke.MultiplayerInvitation current = enumerator.Current;
						using (current)
						{
							if (current.Id().Equals(invitationId))
							{
								callback(current);
								return;
							}
						}
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				callback(null);
			});
		}

		public void Finish(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, MatchOutcome outcome, Action<bool> callback)
		{
			Action<bool> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool>(action);
			this.FindEqualVersionMatch(match, action, (NativeTurnBasedMatch foundMatch) => {
				GooglePlayGames.Native.PInvoke.ParticipantResults participantResult = foundMatch.Results();
				foreach (string participantId in outcome.ParticipantIds)
				{
					Types.MatchResult matchResult = NativeTurnBasedMultiplayerClient.ResultToMatchResult(outcome.GetResultFor(participantId));
					uint placementFor = outcome.GetPlacementFor(participantId);
					if (!participantResult.HasResultsForParticipant(participantId))
					{
						GooglePlayGames.Native.PInvoke.ParticipantResults participantResult1 = participantResult;
						participantResult = participantResult1.WithResult(participantId, placementFor, matchResult);
						participantResult1.Dispose();
					}
					else
					{
						Types.MatchResult matchResult1 = participantResult.ResultsForParticipant(participantId);
						uint num = participantResult.PlacingForParticipant(participantId);
						if (matchResult != matchResult1 || placementFor != num)
						{
							Logger.e(string.Format("Attempted to override existing results for participant {0}: Placing {1}, Result {2}", participantId, num, matchResult1));
							action(false);
							return;
						}
					}
				}
				this.mTurnBasedManager.FinishMatchDuringMyTurn(foundMatch, data, participantResult, (TurnBasedManager.TurnBasedMatchResponse response) => action(response.RequestSucceeded()));
			});
		}

		public void GetAllInvitations(Action<Invitation[]> callback)
		{
			this.mTurnBasedManager.GetAllTurnbasedMatches((TurnBasedManager.TurnBasedMatchesResponse allMatches) => {
				Invitation[] invitationArray = new Invitation[allMatches.InvitationCount()];
				int num = 0;
				IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = allMatches.Invitations().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GooglePlayGames.Native.PInvoke.MultiplayerInvitation current = enumerator.Current;
						int num1 = num;
						num = num1 + 1;
						invitationArray[num1] = current.AsInvitation();
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				callback(invitationArray);
			});
		}

		public void GetAllMatches(Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[]> callback)
		{
			this.mTurnBasedManager.GetAllTurnbasedMatches((TurnBasedManager.TurnBasedMatchesResponse allMatches) => {
				GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[] turnBasedMatchArray = new GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[allMatches.MyTurnMatchesCount() + allMatches.TheirTurnMatchesCount() + allMatches.CompletedMatchesCount()];
				int num = 0;
				IEnumerator<NativeTurnBasedMatch> enumerator = allMatches.MyTurnMatches().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						NativeTurnBasedMatch current = enumerator.Current;
						int num1 = num;
						num = num1 + 1;
						turnBasedMatchArray[num1] = current.AsTurnBasedMatch(this.mNativeClient.GetUserId());
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				IEnumerator<NativeTurnBasedMatch> enumerator1 = allMatches.TheirTurnMatches().GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						NativeTurnBasedMatch nativeTurnBasedMatch = enumerator1.Current;
						int num2 = num;
						num = num2 + 1;
						turnBasedMatchArray[num2] = nativeTurnBasedMatch.AsTurnBasedMatch(this.mNativeClient.GetUserId());
					}
				}
				finally
				{
					if (enumerator1 == null)
					{
					}
					enumerator1.Dispose();
				}
				IEnumerator<NativeTurnBasedMatch> enumerator2 = allMatches.CompletedMatches().GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						NativeTurnBasedMatch current1 = enumerator2.Current;
						int num3 = num;
						num = num3 + 1;
						turnBasedMatchArray[num3] = current1.AsTurnBasedMatch(this.mNativeClient.GetUserId());
					}
				}
				finally
				{
					if (enumerator2 == null)
					{
					}
					enumerator2.Dispose();
				}
				callback(turnBasedMatchArray);
			});
		}

		public int GetMaxMatchDataSize()
		{
			throw new NotImplementedException();
		}

		internal void HandleMatchEvent(Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match)
		{
			Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> action = this.mMatchDelegate;
			if (action == null)
			{
				return;
			}
			if (eventType == Types.MultiplayerEvent.REMOVED)
			{
				Logger.d(string.Concat("Ignoring REMOVE event for match ", matchId));
				return;
			}
			bool flag = eventType == Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
			match.ReferToMe();
			Callbacks.AsCoroutine(this.WaitForLogin(() => {
				action(match.AsTurnBasedMatch(this.mNativeClient.GetUserId()), flag);
				match.ForgetMe();
			}));
		}

		public void Leave(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			Action<bool> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool>(action);
			this.FindEqualVersionMatch(match, action, (NativeTurnBasedMatch foundMatch) => this.mTurnBasedManager.LeaveMatchDuringTheirTurn(foundMatch, (CommonErrorStatus.MultiplayerStatus status) => action((int)status > 0)));
		}

		public void LeaveDuringTurn(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string pendingParticipantId, Action<bool> callback)
		{
			Action<bool> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool>(action);
			this.FindEqualVersionMatchWithParticipant(match, pendingParticipantId, action, (GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch) => this.mTurnBasedManager.LeaveDuringMyTurn(foundMatch, pendingParticipant, (CommonErrorStatus.MultiplayerStatus status) => action((int)status > 0)));
		}

		public void RegisterMatchDelegate(MatchDelegate del)
		{
			if (del != null)
			{
				this.mMatchDelegate = Callbacks.AsOnGameThreadCallback<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool>((GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, bool autoLaunch) => del(match, autoLaunch));
			}
			else
			{
				this.mMatchDelegate = null;
			}
		}

		public void Rematch(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> action = callback;
			action = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(action);
			this.FindEqualVersionMatch(match, (bool failed) => action(false, null), (NativeTurnBasedMatch foundMatch) => this.mTurnBasedManager.Rematch(foundMatch, this.BridgeMatchToUserCallback((UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch m) => action(status == UIStatus.Valid, m))));
		}

		private static Types.MatchResult ResultToMatchResult(MatchOutcome.ParticipantResult result)
		{
			switch (result)
			{
				case MatchOutcome.ParticipantResult.None:
				{
					return Types.MatchResult.NONE;
				}
				case MatchOutcome.ParticipantResult.Win:
				{
					return Types.MatchResult.WIN;
				}
				case MatchOutcome.ParticipantResult.Loss:
				{
					return Types.MatchResult.LOSS;
				}
				case MatchOutcome.ParticipantResult.Tie:
				{
					return Types.MatchResult.TIE;
				}
			}
			Logger.e(string.Concat("Received unknown ParticipantResult ", result));
			return Types.MatchResult.NONE;
		}

		public void TakeTurn(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, string pendingParticipantId, Action<bool> callback)
		{
			Action<bool> action = callback;
			Logger.describe(data);
			action = Callbacks.AsOnGameThreadCallback<bool>(action);
			this.FindEqualVersionMatchWithParticipant(match, pendingParticipantId, action, (GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch) => this.mTurnBasedManager.TakeTurn(foundMatch, data, pendingParticipant, (TurnBasedManager.TurnBasedMatchResponse result) => {
				if (!result.RequestSucceeded())
				{
					Logger.d(string.Concat("Taking turn failed: ", result.ResponseStatus()));
					action(false);
				}
				else
				{
					action(true);
				}
			}));
		}

		[DebuggerHidden]
		private IEnumerator WaitForLogin(Action method)
		{
			NativeTurnBasedMultiplayerClient.u003cWaitForLoginu003ec__Iterator7D variable = null;
			return variable;
		}
	}
}