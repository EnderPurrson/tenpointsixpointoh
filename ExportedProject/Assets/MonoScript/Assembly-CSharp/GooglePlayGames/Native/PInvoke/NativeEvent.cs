using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeEvent : BaseReferenceHolder, IEvent
	{
		public ulong CurrentCount
		{
			get
			{
				return Event.Event_Count(base.SelfPtr());
			}
		}

		public string Description
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Event.Event_Description(base.SelfPtr(), out_string, out_size));
			}
		}

		public string Id
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Event.Event_Id(base.SelfPtr(), out_string, out_size));
			}
		}

		public string ImageUrl
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Event.Event_ImageUrl(base.SelfPtr(), out_string, out_size));
			}
		}

		public string Name
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Event.Event_Name(base.SelfPtr(), out_string, out_size));
			}
		}

		public EventVisibility Visibility
		{
			get
			{
				Types.EventVisibility eventVisibility = Event.Event_Visibility(base.SelfPtr());
				Types.EventVisibility eventVisibility1 = eventVisibility;
				if (eventVisibility1 == Types.EventVisibility.HIDDEN)
				{
					return EventVisibility.Hidden;
				}
				if (eventVisibility1 != Types.EventVisibility.REVEALED)
				{
					throw new InvalidOperationException(string.Concat("Unknown visibility: ", eventVisibility));
				}
				return EventVisibility.Revealed;
			}
		}

		internal NativeEvent(IntPtr selfPointer) : base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			Event.Event_Dispose(selfPointer);
		}

		public override string ToString()
		{
			if (base.IsDisposed())
			{
				return "[NativeEvent: DELETED]";
			}
			return string.Format("[NativeEvent: Id={0}, Name={1}, Description={2}, ImageUrl={3}, CurrentCount={4}, Visibility={5}]", new object[] { this.Id, this.Name, this.Description, this.ImageUrl, this.CurrentCount, this.Visibility });
		}
	}
}