using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeQuest : BaseReferenceHolder, IQuest
	{
		private volatile NativeQuestMilestone mCachedMilestone;

		public DateTime? AcceptedTime
		{
			get
			{
				long num = Quest.Quest_AcceptedTime(base.SelfPtr());
				if (num == 0)
				{
					return null;
				}
				return new DateTime?(PInvokeUtilities.FromMillisSinceUnixEpoch(num));
			}
		}

		public string BannerUrl
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_BannerUrl(base.SelfPtr(), out_string, out_size));
			}
		}

		public string Description
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_Description(base.SelfPtr(), out_string, out_size));
			}
		}

		public DateTime ExpirationTime
		{
			get
			{
				return PInvokeUtilities.FromMillisSinceUnixEpoch(Quest.Quest_ExpirationTime(base.SelfPtr()));
			}
		}

		public string IconUrl
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_IconUrl(base.SelfPtr(), out_string, out_size));
			}
		}

		public string Id
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_Id(base.SelfPtr(), out_string, out_size));
			}
		}

		public IQuestMilestone Milestone
		{
			get
			{
				if (this.mCachedMilestone == null)
				{
					this.mCachedMilestone = NativeQuestMilestone.FromPointer(Quest.Quest_CurrentMilestone(base.SelfPtr()));
				}
				return this.mCachedMilestone;
			}
		}

		public string Name
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_Name(base.SelfPtr(), out_string, out_size));
			}
		}

		public DateTime StartTime
		{
			get
			{
				return PInvokeUtilities.FromMillisSinceUnixEpoch(Quest.Quest_StartTime(base.SelfPtr()));
			}
		}

		public QuestState State
		{
			get
			{
				Types.QuestState questState = Quest.Quest_State(base.SelfPtr());
				switch (questState)
				{
					case Types.QuestState.UPCOMING:
					{
						return QuestState.Upcoming;
					}
					case Types.QuestState.OPEN:
					{
						return QuestState.Open;
					}
					case Types.QuestState.ACCEPTED:
					{
						return QuestState.Accepted;
					}
					case Types.QuestState.COMPLETED:
					{
						return QuestState.Completed;
					}
					case Types.QuestState.EXPIRED:
					{
						return QuestState.Expired;
					}
					case Types.QuestState.FAILED:
					{
						return QuestState.Failed;
					}
				}
				throw new InvalidOperationException(string.Concat("Unknown state: ", questState));
			}
		}

		internal NativeQuest(IntPtr selfPointer) : base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			Quest.Quest_Dispose(selfPointer);
		}

		internal static NativeQuest FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeQuest(pointer);
		}

		public override string ToString()
		{
			if (base.IsDisposed())
			{
				return "[NativeQuest: DELETED]";
			}
			return string.Format("[NativeQuest: Id={0}, Name={1}, Description={2}, BannerUrl={3}, IconUrl={4}, State={5}, StartTime={6}, ExpirationTime={7}, AcceptedTime={8}]", new object[] { this.Id, this.Name, this.Description, this.BannerUrl, this.IconUrl, this.State, this.StartTime, this.ExpirationTime, this.AcceptedTime });
		}

		internal bool Valid()
		{
			return Quest.Quest_Valid(base.SelfPtr());
		}
	}
}