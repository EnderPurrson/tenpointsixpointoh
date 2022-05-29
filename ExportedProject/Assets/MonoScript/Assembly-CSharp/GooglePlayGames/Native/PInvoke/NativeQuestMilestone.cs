using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeQuestMilestone : BaseReferenceHolder, IQuestMilestone
	{
		public byte[] CompletionRewardData
		{
			get
			{
				return PInvokeUtilities.OutParamsToArray<byte>((byte[] out_bytes, UIntPtr out_size) => QuestMilestone.QuestMilestone_CompletionRewardData(base.SelfPtr(), out_bytes, out_size));
			}
		}

		public ulong CurrentCount
		{
			get
			{
				return QuestMilestone.QuestMilestone_CurrentCount(base.SelfPtr());
			}
		}

		public string EventId
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => QuestMilestone.QuestMilestone_EventId(base.SelfPtr(), out_string, out_size));
			}
		}

		public string Id
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => QuestMilestone.QuestMilestone_Id(base.SelfPtr(), out_string, out_size));
			}
		}

		public string QuestId
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => QuestMilestone.QuestMilestone_QuestId(base.SelfPtr(), out_string, out_size));
			}
		}

		public MilestoneState State
		{
			get
			{
				Types.QuestMilestoneState questMilestoneState = QuestMilestone.QuestMilestone_State(base.SelfPtr());
				switch (questMilestoneState)
				{
					case Types.QuestMilestoneState.NOT_STARTED:
					{
						return MilestoneState.NotStarted;
					}
					case Types.QuestMilestoneState.NOT_COMPLETED:
					{
						return MilestoneState.NotCompleted;
					}
					case Types.QuestMilestoneState.COMPLETED_NOT_CLAIMED:
					{
						return MilestoneState.CompletedNotClaimed;
					}
					case Types.QuestMilestoneState.CLAIMED:
					{
						return MilestoneState.Claimed;
					}
				}
				throw new InvalidOperationException(string.Concat("Unknown state: ", questMilestoneState));
			}
		}

		public ulong TargetCount
		{
			get
			{
				return QuestMilestone.QuestMilestone_TargetCount(base.SelfPtr());
			}
		}

		internal NativeQuestMilestone(IntPtr selfPointer) : base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			QuestMilestone.QuestMilestone_Dispose(selfPointer);
		}

		internal static NativeQuestMilestone FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeQuestMilestone(pointer);
		}

		public override string ToString()
		{
			return string.Format("[NativeQuestMilestone: Id={0}, EventId={1}, QuestId={2}, CurrentCount={3}, TargetCount={4}, State={5}]", new object[] { this.Id, this.EventId, this.QuestId, this.CurrentCount, this.TargetCount, this.State });
		}

		internal bool Valid()
		{
			return QuestMilestone.QuestMilestone_Valid(base.SelfPtr());
		}
	}
}