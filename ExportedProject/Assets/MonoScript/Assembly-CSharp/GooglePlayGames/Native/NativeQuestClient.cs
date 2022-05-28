using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	internal class NativeQuestClient : IQuestsClient
	{
		[CompilerGenerated]
		private sealed class _003CFetch_003Ec__AnonStorey22B
		{
			internal Action<ResponseStatus, IQuest> callback;

			internal void _003C_003Em__C3(GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse response)
			{
				ResponseStatus arg = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, null);
				}
				else
				{
					callback(arg, response.Data());
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CFetchMatchingState_003Ec__AnonStorey22C
		{
			internal Action<ResponseStatus, List<IQuest>> callback;

			internal void _003C_003Em__C4(GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse response)
			{
				ResponseStatus arg = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, null);
				}
				else
				{
					callback(arg, response.Data().Cast<IQuest>().ToList());
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CFromQuestUICallback_003Ec__AnonStorey22D
		{
			internal Action<QuestUiResult, IQuest, IQuestMilestone> callback;

			internal void _003C_003Em__C5(GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse response)
			{
				if (!response.RequestSucceeded())
				{
					callback(UiErrorToQuestUiResult(response.RequestStatus()), null, null);
					return;
				}
				NativeQuest nativeQuest = response.AcceptedQuest();
				NativeQuestMilestone nativeQuestMilestone = response.MilestoneToClaim();
				if (nativeQuest != null)
				{
					callback(QuestUiResult.UserRequestsQuestAcceptance, nativeQuest, null);
					nativeQuestMilestone.Dispose();
					return;
				}
				if (nativeQuestMilestone != null)
				{
					callback(QuestUiResult.UserRequestsMilestoneClaiming, null, response.MilestoneToClaim());
					nativeQuest.Dispose();
					return;
				}
				Logger.e("Quest UI succeeded without a quest acceptance or milestone claim.");
				nativeQuest.Dispose();
				nativeQuestMilestone.Dispose();
				callback(QuestUiResult.InternalError, null, null);
			}
		}

		[CompilerGenerated]
		private sealed class _003CAccept_003Ec__AnonStorey22E
		{
			internal Action<QuestAcceptStatus, IQuest> callback;

			internal void _003C_003Em__C6(GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse response)
			{
				if (response.RequestSucceeded())
				{
					callback(QuestAcceptStatus.Success, response.AcceptedQuest());
				}
				else
				{
					callback(FromAcceptStatus(response.ResponseStatus()), null);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CClaimMilestone_003Ec__AnonStorey22F
		{
			internal Action<QuestClaimMilestoneStatus, IQuest, IQuestMilestone> callback;

			internal void _003C_003Em__C7(GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse response)
			{
				if (response.RequestSucceeded())
				{
					callback(QuestClaimMilestoneStatus.Success, response.Quest(), response.ClaimedMilestone());
				}
				else
				{
					callback(FromClaimStatus(response.ResponseStatus()), null, null);
				}
			}
		}

		private readonly GooglePlayGames.Native.PInvoke.QuestManager mManager;

		internal NativeQuestClient(GooglePlayGames.Native.PInvoke.QuestManager manager)
		{
			mManager = Misc.CheckNotNull(manager);
		}

		public void Fetch(DataSource source, string questId, Action<ResponseStatus, IQuest> callback)
		{
			_003CFetch_003Ec__AnonStorey22B _003CFetch_003Ec__AnonStorey22B = new _003CFetch_003Ec__AnonStorey22B();
			_003CFetch_003Ec__AnonStorey22B.callback = callback;
			Misc.CheckNotNull(questId);
			Misc.CheckNotNull(_003CFetch_003Ec__AnonStorey22B.callback);
			_003CFetch_003Ec__AnonStorey22B.callback = CallbackUtils.ToOnGameThread(_003CFetch_003Ec__AnonStorey22B.callback);
			mManager.Fetch(ConversionUtils.AsDataSource(source), questId, _003CFetch_003Ec__AnonStorey22B._003C_003Em__C3);
		}

		public void FetchMatchingState(DataSource source, QuestFetchFlags flags, Action<ResponseStatus, List<IQuest>> callback)
		{
			_003CFetchMatchingState_003Ec__AnonStorey22C _003CFetchMatchingState_003Ec__AnonStorey22C = new _003CFetchMatchingState_003Ec__AnonStorey22C();
			_003CFetchMatchingState_003Ec__AnonStorey22C.callback = callback;
			Misc.CheckNotNull(_003CFetchMatchingState_003Ec__AnonStorey22C.callback);
			_003CFetchMatchingState_003Ec__AnonStorey22C.callback = CallbackUtils.ToOnGameThread(_003CFetchMatchingState_003Ec__AnonStorey22C.callback);
			mManager.FetchList(ConversionUtils.AsDataSource(source), (int)flags, _003CFetchMatchingState_003Ec__AnonStorey22C._003C_003Em__C4);
		}

		public void ShowAllQuestsUI(Action<QuestUiResult, IQuest, IQuestMilestone> callback)
		{
			Misc.CheckNotNull(callback);
			callback = CallbackUtils.ToOnGameThread(callback);
			mManager.ShowAllQuestUI(FromQuestUICallback(callback));
		}

		public void ShowSpecificQuestUI(IQuest quest, Action<QuestUiResult, IQuest, IQuestMilestone> callback)
		{
			Misc.CheckNotNull(quest);
			Misc.CheckNotNull(callback);
			callback = CallbackUtils.ToOnGameThread(callback);
			NativeQuest nativeQuest = quest as NativeQuest;
			if (nativeQuest == null)
			{
				Logger.e("Encountered quest that was not generated by this IQuestClient");
				callback(QuestUiResult.BadInput, null, null);
			}
			else
			{
				mManager.ShowQuestUI(nativeQuest, FromQuestUICallback(callback));
			}
		}

		private static QuestUiResult UiErrorToQuestUiResult(CommonErrorStatus.UIStatus status)
		{
			switch (status)
			{
			case CommonErrorStatus.UIStatus.ERROR_INTERNAL:
				return QuestUiResult.InternalError;
			case CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED:
				return QuestUiResult.NotAuthorized;
			case CommonErrorStatus.UIStatus.ERROR_CANCELED:
				return QuestUiResult.UserCanceled;
			case CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED:
				return QuestUiResult.VersionUpdateRequired;
			case CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
				return QuestUiResult.Timeout;
			case CommonErrorStatus.UIStatus.ERROR_UI_BUSY:
				return QuestUiResult.UiBusy;
			default:
				Logger.e("Unknown error status: " + status);
				return QuestUiResult.InternalError;
			}
		}

		private static Action<GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse> FromQuestUICallback(Action<QuestUiResult, IQuest, IQuestMilestone> callback)
		{
			_003CFromQuestUICallback_003Ec__AnonStorey22D _003CFromQuestUICallback_003Ec__AnonStorey22D = new _003CFromQuestUICallback_003Ec__AnonStorey22D();
			_003CFromQuestUICallback_003Ec__AnonStorey22D.callback = callback;
			return _003CFromQuestUICallback_003Ec__AnonStorey22D._003C_003Em__C5;
		}

		public void Accept(IQuest quest, Action<QuestAcceptStatus, IQuest> callback)
		{
			_003CAccept_003Ec__AnonStorey22E _003CAccept_003Ec__AnonStorey22E = new _003CAccept_003Ec__AnonStorey22E();
			_003CAccept_003Ec__AnonStorey22E.callback = callback;
			Misc.CheckNotNull(quest);
			Misc.CheckNotNull(_003CAccept_003Ec__AnonStorey22E.callback);
			_003CAccept_003Ec__AnonStorey22E.callback = CallbackUtils.ToOnGameThread(_003CAccept_003Ec__AnonStorey22E.callback);
			NativeQuest nativeQuest = quest as NativeQuest;
			if (nativeQuest == null)
			{
				Logger.e("Encountered quest that was not generated by this IQuestClient");
				_003CAccept_003Ec__AnonStorey22E.callback(QuestAcceptStatus.BadInput, null);
			}
			else
			{
				mManager.Accept(nativeQuest, _003CAccept_003Ec__AnonStorey22E._003C_003Em__C6);
			}
		}

		private static QuestAcceptStatus FromAcceptStatus(CommonErrorStatus.QuestAcceptStatus status)
		{
			switch (status)
			{
			case CommonErrorStatus.QuestAcceptStatus.ERROR_INTERNAL:
				return QuestAcceptStatus.InternalError;
			case CommonErrorStatus.QuestAcceptStatus.ERROR_NOT_AUTHORIZED:
				return QuestAcceptStatus.NotAuthorized;
			case CommonErrorStatus.QuestAcceptStatus.ERROR_QUEST_NOT_STARTED:
				return QuestAcceptStatus.QuestNotStarted;
			case CommonErrorStatus.QuestAcceptStatus.ERROR_QUEST_NO_LONGER_AVAILABLE:
				return QuestAcceptStatus.QuestNoLongerAvailable;
			case CommonErrorStatus.QuestAcceptStatus.ERROR_TIMEOUT:
				return QuestAcceptStatus.Timeout;
			case CommonErrorStatus.QuestAcceptStatus.VALID:
				return QuestAcceptStatus.Success;
			default:
				Logger.e("Encountered unknown status: " + status);
				return QuestAcceptStatus.InternalError;
			}
		}

		public void ClaimMilestone(IQuestMilestone milestone, Action<QuestClaimMilestoneStatus, IQuest, IQuestMilestone> callback)
		{
			_003CClaimMilestone_003Ec__AnonStorey22F _003CClaimMilestone_003Ec__AnonStorey22F = new _003CClaimMilestone_003Ec__AnonStorey22F();
			_003CClaimMilestone_003Ec__AnonStorey22F.callback = callback;
			Misc.CheckNotNull(milestone);
			Misc.CheckNotNull(_003CClaimMilestone_003Ec__AnonStorey22F.callback);
			_003CClaimMilestone_003Ec__AnonStorey22F.callback = CallbackUtils.ToOnGameThread(_003CClaimMilestone_003Ec__AnonStorey22F.callback);
			NativeQuestMilestone nativeQuestMilestone = milestone as NativeQuestMilestone;
			if (nativeQuestMilestone == null)
			{
				Logger.e("Encountered milestone that was not generated by this IQuestClient");
				_003CClaimMilestone_003Ec__AnonStorey22F.callback(QuestClaimMilestoneStatus.BadInput, null, null);
			}
			else
			{
				mManager.ClaimMilestone(nativeQuestMilestone, _003CClaimMilestone_003Ec__AnonStorey22F._003C_003Em__C7);
			}
		}

		private static QuestClaimMilestoneStatus FromClaimStatus(CommonErrorStatus.QuestClaimMilestoneStatus status)
		{
			switch (status)
			{
			case CommonErrorStatus.QuestClaimMilestoneStatus.VALID:
				return QuestClaimMilestoneStatus.Success;
			case CommonErrorStatus.QuestClaimMilestoneStatus.ERROR_INTERNAL:
				return QuestClaimMilestoneStatus.InternalError;
			case CommonErrorStatus.QuestClaimMilestoneStatus.ERROR_MILESTONE_ALREADY_CLAIMED:
				return QuestClaimMilestoneStatus.MilestoneAlreadyClaimed;
			case CommonErrorStatus.QuestClaimMilestoneStatus.ERROR_MILESTONE_CLAIM_FAILED:
				return QuestClaimMilestoneStatus.MilestoneClaimFailed;
			case CommonErrorStatus.QuestClaimMilestoneStatus.ERROR_NOT_AUTHORIZED:
				return QuestClaimMilestoneStatus.NotAuthorized;
			case CommonErrorStatus.QuestClaimMilestoneStatus.ERROR_TIMEOUT:
				return QuestClaimMilestoneStatus.Timeout;
			default:
				Logger.e("Encountered unknown status: " + status);
				return QuestClaimMilestoneStatus.InternalError;
			}
		}
	}
}
