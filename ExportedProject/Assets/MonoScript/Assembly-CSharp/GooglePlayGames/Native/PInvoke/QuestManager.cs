using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class QuestManager
	{
		private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal QuestManager(GooglePlayGames.Native.PInvoke.GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
		}

		internal void Accept(NativeQuest quest, Action<GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_Accept(this.mServices.AsHandle(), quest.AsPointer(), new GooglePlayGames.Native.Cwrapper.QuestManager.AcceptCallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalAcceptCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse>(GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse.FromPointer)));
		}

		internal void ClaimMilestone(NativeQuestMilestone milestone, Action<GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestone(this.mServices.AsHandle(), milestone.AsPointer(), new GooglePlayGames.Native.Cwrapper.QuestManager.ClaimMilestoneCallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalClaimMilestoneCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse>(GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse.FromPointer)));
		}

		internal void Fetch(Types.DataSource source, string questId, Action<GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_Fetch(this.mServices.AsHandle(), source, questId, new GooglePlayGames.Native.Cwrapper.QuestManager.FetchCallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalFetchCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse>(GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse.FromPointer)));
		}

		internal void FetchList(Types.DataSource source, int fetchFlags, Action<GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchList(this.mServices.AsHandle(), source, fetchFlags, new GooglePlayGames.Native.Cwrapper.QuestManager.FetchListCallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalFetchListCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse>(GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse.FromPointer)));
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.AcceptCallback))]
		internal static void InternalAcceptCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#AcceptCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.ClaimMilestoneCallback))]
		internal static void InternalClaimMilestoneCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#ClaimMilestoneCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.FetchCallback))]
		internal static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#FetchCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.FetchListCallback))]
		internal static void InternalFetchListCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#FetchListCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback))]
		internal static void InternalQuestUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#QuestUICallback", Callbacks.Type.Temporary, response, data);
		}

		internal void ShowAllQuestUI(Action<GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ShowAllUI(this.mServices.AsHandle(), new GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalQuestUICallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse>(GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse.FromPointer)));
		}

		internal void ShowQuestUI(NativeQuest quest, Action<GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ShowUI(this.mServices.AsHandle(), quest.AsPointer(), new GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalQuestUICallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse>(GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse.FromPointer)));
		}

		internal class AcceptResponse : BaseReferenceHolder
		{
			internal AcceptResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			internal NativeQuest AcceptedQuest()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_GetAcceptedQuest(base.SelfPtr()));
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_Dispose(selfPointer);
			}

			internal static GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse(pointer);
			}

			internal bool RequestSucceeded()
			{
				return (int)this.ResponseStatus() > 0;
			}

			internal CommonErrorStatus.QuestAcceptStatus ResponseStatus()
			{
				return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_GetStatus(base.SelfPtr());
			}
		}

		internal class ClaimMilestoneResponse : BaseReferenceHolder
		{
			internal ClaimMilestoneResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_Dispose(selfPointer);
			}

			internal NativeQuestMilestone ClaimedMilestone()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				NativeQuestMilestone nativeQuestMilestone = new NativeQuestMilestone(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetClaimedMilestone(base.SelfPtr()));
				if (nativeQuestMilestone.Valid())
				{
					return nativeQuestMilestone;
				}
				nativeQuestMilestone.Dispose();
				return null;
			}

			internal static GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse(pointer);
			}

			internal NativeQuest Quest()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				NativeQuest nativeQuest = new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetQuest(base.SelfPtr()));
				if (nativeQuest.Valid())
				{
					return nativeQuest;
				}
				nativeQuest.Dispose();
				return null;
			}

			internal bool RequestSucceeded()
			{
				return (int)this.ResponseStatus() > 0;
			}

			internal CommonErrorStatus.QuestClaimMilestoneStatus ResponseStatus()
			{
				return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetStatus(base.SelfPtr());
			}
		}

		internal class FetchListResponse : BaseReferenceHolder
		{
			internal FetchListResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_Dispose(selfPointer);
			}

			internal IEnumerable<NativeQuest> Data()
			{
				return PInvokeUtilities.ToEnumerable<NativeQuest>(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetData_Length(base.SelfPtr()), (UIntPtr index) => new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetData_GetElement(base.SelfPtr(), index)));
			}

			internal static GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse(pointer);
			}

			internal bool RequestSucceeded()
			{
				return (int)this.ResponseStatus() > 0;
			}

			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetStatus(base.SelfPtr());
			}
		}

		internal class FetchResponse : BaseReferenceHolder
		{
			internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_Dispose(selfPointer);
			}

			internal NativeQuest Data()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_GetData(base.SelfPtr()));
			}

			internal static GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse(pointer);
			}

			internal bool RequestSucceeded()
			{
				return (int)this.ResponseStatus() > 0;
			}

			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_GetStatus(base.SelfPtr());
			}
		}

		internal class QuestUIResponse : BaseReferenceHolder
		{
			internal QuestUIResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			internal NativeQuest AcceptedQuest()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				NativeQuest nativeQuest = new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetAcceptedQuest(base.SelfPtr()));
				if (nativeQuest.Valid())
				{
					return nativeQuest;
				}
				nativeQuest.Dispose();
				return null;
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_Dispose(selfPointer);
			}

			internal static GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse(pointer);
			}

			internal NativeQuestMilestone MilestoneToClaim()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				NativeQuestMilestone nativeQuestMilestone = new NativeQuestMilestone(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetMilestoneToClaim(base.SelfPtr()));
				if (nativeQuestMilestone.Valid())
				{
					return nativeQuestMilestone;
				}
				nativeQuestMilestone.Dispose();
				return null;
			}

			internal CommonErrorStatus.UIStatus RequestStatus()
			{
				return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetStatus(base.SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return (int)this.RequestStatus() > 0;
			}
		}
	}
}