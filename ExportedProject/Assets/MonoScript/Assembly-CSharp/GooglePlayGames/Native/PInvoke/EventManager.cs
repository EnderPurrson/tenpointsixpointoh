using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class EventManager
	{
		private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal EventManager(GooglePlayGames.Native.PInvoke.GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
		}

		internal void Fetch(Types.DataSource source, string eventId, Action<GooglePlayGames.Native.PInvoke.EventManager.FetchResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.EventManager.EventManager_Fetch(this.mServices.AsHandle(), source, eventId, new GooglePlayGames.Native.Cwrapper.EventManager.FetchCallback(GooglePlayGames.Native.PInvoke.EventManager.InternalFetchCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.EventManager.FetchResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.EventManager.FetchResponse>(GooglePlayGames.Native.PInvoke.EventManager.FetchResponse.FromPointer)));
		}

		internal void FetchAll(Types.DataSource source, Action<GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAll(this.mServices.AsHandle(), source, new GooglePlayGames.Native.Cwrapper.EventManager.FetchAllCallback(GooglePlayGames.Native.PInvoke.EventManager.InternalFetchAllCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse>(GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse.FromPointer)));
		}

		internal void Increment(string eventId, uint steps)
		{
			GooglePlayGames.Native.Cwrapper.EventManager.EventManager_Increment(this.mServices.AsHandle(), eventId, steps);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.EventManager.FetchAllCallback))]
		internal static void InternalFetchAllCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("EventManager#FetchAllCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.EventManager.FetchCallback))]
		internal static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("EventManager#FetchCallback", Callbacks.Type.Temporary, response, data);
		}

		internal class FetchAllResponse : BaseReferenceHolder
		{
			internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_Dispose(selfPointer);
			}

			internal List<NativeEvent> Data()
			{
				return (
					from ptr in (IEnumerable<IntPtr>)PInvokeUtilities.OutParamsToArray<IntPtr>((IntPtr[] out_arg, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_GetData(base.SelfPtr(), out_arg, out_size))
					select new NativeEvent(ptr)).ToList<NativeEvent>();
			}

			internal static GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse(pointer);
			}

			internal bool RequestSucceeded()
			{
				return (int)this.ResponseStatus() > 0;
			}

			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_GetStatus(base.SelfPtr());
			}
		}

		internal class FetchResponse : BaseReferenceHolder
		{
			internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_Dispose(selfPointer);
			}

			internal NativeEvent Data()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeEvent(GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_GetData(base.SelfPtr()));
			}

			internal static GooglePlayGames.Native.PInvoke.EventManager.FetchResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.EventManager.FetchResponse(pointer);
			}

			internal bool RequestSucceeded()
			{
				return (int)this.ResponseStatus() > 0;
			}

			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_GetStatus(base.SelfPtr());
			}
		}
	}
}