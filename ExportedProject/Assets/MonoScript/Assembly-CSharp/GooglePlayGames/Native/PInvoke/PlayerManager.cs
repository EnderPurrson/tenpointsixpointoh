using AOT;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class PlayerManager
	{
		private readonly GooglePlayGames.Native.PInvoke.GameServices mGameServices;

		internal PlayerManager(GooglePlayGames.Native.PInvoke.GameServices services)
		{
			this.mGameServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
		}

		internal void FetchFriends(Action<ResponseStatus, List<GooglePlayGames.BasicApi.Multiplayer.Player>> callback)
		{
			GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchConnected(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchConnectedCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse>((GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse rsp) => this.HandleFetchCollected(rsp, callback), new Func<IntPtr, GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse>(GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse.FromPointer)));
		}

		internal void FetchList(string[] userIds, Action<NativePlayer[]> callback)
		{
			GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponseCollector fetchResponseCollector = new GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponseCollector()
			{
				pendingCount = (int)userIds.Length,
				callback = callback
			};
			string[] strArrays = userIds;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_Fetch(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, str, new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse>((GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse rsp) => this.HandleFetchResponse(fetchResponseCollector, rsp), new Func<IntPtr, GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse>(GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse.FromPointer)));
			}
		}

		internal void FetchSelf(Action<GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelf(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchSelfCallback), Callbacks.ToIntPtr<GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse>(callback, new Func<IntPtr, GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse>(GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse.FromPointer)));
		}

		internal void HandleFetchCollected(GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse rsp, Action<ResponseStatus, List<GooglePlayGames.BasicApi.Multiplayer.Player>> callback)
		{
			List<GooglePlayGames.BasicApi.Multiplayer.Player> players = new List<GooglePlayGames.BasicApi.Multiplayer.Player>();
			if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID || rsp.Status() == CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				UIntPtr uIntPtr = rsp.Length();
				Logger.d(string.Concat("Got ", uIntPtr.ToUInt64(), " players"));
				IEnumerator<NativePlayer> enumerator = rsp.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						players.Add(enumerator.Current.AsPlayer());
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
			}
			callback(rsp.Status(), players);
		}

		internal void HandleFetchResponse(GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponseCollector collector, GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse resp)
		{
			if (resp.Status() == CommonErrorStatus.ResponseStatus.VALID || resp.Status() == CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				NativePlayer player = resp.GetPlayer();
				collector.results.Add(player);
			}
			collector.pendingCount--;
			if (collector.pendingCount == 0)
			{
				collector.callback(collector.results.ToArray());
			}
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback))]
		private static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("PlayerManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback))]
		private static void InternalFetchConnectedCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("PlayerManager#InternalFetchConnectedCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback))]
		private static void InternalFetchSelfCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("PlayerManager#InternalFetchSelfCallback", Callbacks.Type.Temporary, response, data);
		}

		internal class FetchListResponse : BaseReferenceHolder, IEnumerable, IEnumerable<NativePlayer>
		{
			internal FetchListResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_Dispose(base.SelfPtr());
			}

			internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse FromPointer(IntPtr selfPointer)
			{
				if (PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse(selfPointer);
			}

			internal NativePlayer GetElement(UIntPtr index)
			{
				if (index.ToUInt64() >= this.Length().ToUInt64())
				{
					throw new ArgumentOutOfRangeException();
				}
				return new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetData_GetElement(base.SelfPtr(), index));
			}

			public IEnumerator<NativePlayer> GetEnumerator()
			{
				return PInvokeUtilities.ToEnumerator<NativePlayer>(this.Length(), (UIntPtr index) => this.GetElement(index));
			}

			internal UIntPtr Length()
			{
				return GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetData_Length(base.SelfPtr());
			}

			internal CommonErrorStatus.ResponseStatus Status()
			{
				return GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetStatus(base.SelfPtr());
			}

			IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}

		internal class FetchResponse : BaseReferenceHolder
		{
			internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_Dispose(base.SelfPtr());
			}

			internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse FromPointer(IntPtr selfPointer)
			{
				if (PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse(selfPointer);
			}

			internal NativePlayer GetPlayer()
			{
				return new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_GetData(base.SelfPtr()));
			}

			internal CommonErrorStatus.ResponseStatus Status()
			{
				return GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_GetStatus(base.SelfPtr());
			}
		}

		internal class FetchResponseCollector
		{
			internal int pendingCount;

			internal List<NativePlayer> results;

			internal Action<NativePlayer[]> callback;

			public FetchResponseCollector()
			{
			}
		}

		internal class FetchSelfResponse : BaseReferenceHolder
		{
			internal FetchSelfResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_Dispose(base.SelfPtr());
			}

			internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse FromPointer(IntPtr selfPointer)
			{
				if (PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse(selfPointer);
			}

			internal NativePlayer Self()
			{
				return new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_GetData(base.SelfPtr()));
			}

			internal CommonErrorStatus.ResponseStatus Status()
			{
				return GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_GetStatus(base.SelfPtr());
			}
		}
	}
}