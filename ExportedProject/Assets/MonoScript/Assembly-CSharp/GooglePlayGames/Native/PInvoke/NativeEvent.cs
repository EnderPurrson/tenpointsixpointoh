using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeEvent : BaseReferenceHolder, IEvent
	{
		public string Id
		{
			get
			{
				return PInvokeUtilities.OutParamsToString(_003Cget_Id_003Em__13C);
			}
		}

		public string Name
		{
			get
			{
				return PInvokeUtilities.OutParamsToString(_003Cget_Name_003Em__13D);
			}
		}

		public string Description
		{
			get
			{
				return PInvokeUtilities.OutParamsToString(_003Cget_Description_003Em__13E);
			}
		}

		public string ImageUrl
		{
			get
			{
				return PInvokeUtilities.OutParamsToString(_003Cget_ImageUrl_003Em__13F);
			}
		}

		public ulong CurrentCount
		{
			get
			{
				return Event.Event_Count(SelfPtr());
			}
		}

		public EventVisibility Visibility
		{
			get
			{
				Types.EventVisibility eventVisibility = Event.Event_Visibility(SelfPtr());
				switch (eventVisibility)
				{
				case Types.EventVisibility.HIDDEN:
					return EventVisibility.Hidden;
				case Types.EventVisibility.REVEALED:
					return EventVisibility.Revealed;
				default:
					throw new InvalidOperationException("Unknown visibility: " + eventVisibility);
				}
			}
		}

		internal NativeEvent(IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			Event.Event_Dispose(selfPointer);
		}

		public override string ToString()
		{
			if (IsDisposed())
			{
				return "[NativeEvent: DELETED]";
			}
			return string.Format("[NativeEvent: Id={0}, Name={1}, Description={2}, ImageUrl={3}, CurrentCount={4}, Visibility={5}]", Id, Name, Description, ImageUrl, CurrentCount, Visibility);
		}

		[CompilerGenerated]
		private UIntPtr _003Cget_Id_003Em__13C(StringBuilder out_string, UIntPtr out_size)
		{
			return Event.Event_Id(SelfPtr(), out_string, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003Cget_Name_003Em__13D(StringBuilder out_string, UIntPtr out_size)
		{
			return Event.Event_Name(SelfPtr(), out_string, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003Cget_Description_003Em__13E(StringBuilder out_string, UIntPtr out_size)
		{
			return Event.Event_Description(SelfPtr(), out_string, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003Cget_ImageUrl_003Em__13F(StringBuilder out_string, UIntPtr out_size)
		{
			return Event.Event_ImageUrl(SelfPtr(), out_string, out_size);
		}
	}
}
