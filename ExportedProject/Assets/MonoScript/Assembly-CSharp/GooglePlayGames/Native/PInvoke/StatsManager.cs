using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class StatsManager
	{
		private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal StatsManager(GooglePlayGames.Native.PInvoke.GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
		}

		internal void FetchForPlayer(Action<GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse> callback)
		{
			Misc.CheckNotNull<Action<GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse>>(callback);
			GooglePlayGames.Native.Cwrapper.StatsManager.StatsManager_FetchForPlayer(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new GooglePlayGames.Native.Cwrapper.StatsManager.FetchForPlayerCallback(GooglePlayGames.Native.PInvoke.StatsManager.InternalFetchForPlayerCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse>(GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse.FromPointer)));
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.StatsManager.FetchForPlayerCallback))]
		private static void InternalFetchForPlayerCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("StatsManager#InternalFetchForPlayerCallback", Callbacks.Type.Temporary, response, data);
		}

		internal class FetchForPlayerResponse : BaseReferenceHolder
		{
			internal FetchForPlayerResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.StatsManager.StatsManager_FetchForPlayerResponse_Dispose(selfPointer);
			}

			internal static GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse(pointer);
			}

			internal NativePlayerStats PlayerStats()
			{
				return new NativePlayerStats(GooglePlayGames.Native.Cwrapper.StatsManager.StatsManager_FetchForPlayerResponse_GetData(base.SelfPtr()));
			}

			internal CommonErrorStatus.ResponseStatus Status()
			{
				return GooglePlayGames.Native.Cwrapper.StatsManager.StatsManager_FetchForPlayerResponse_GetStatus(base.SelfPtr());
			}
		}
	}
}