using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class RealTimeEventListenerHelper : BaseReferenceHolder
	{
		internal RealTimeEventListenerHelper(IntPtr selfPointer) : base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_Dispose(selfPointer);
		}

		internal static GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper Create()
		{
			return new GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper(GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_Construct());
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnDataReceivedCallback))]
		internal static void InternalOnDataReceived(IntPtr room, IntPtr participant, IntPtr data, UIntPtr dataLength, bool isReliable, IntPtr userData)
		{
			unsafe
			{
				Logger.d(string.Concat("Entering InternalOnDataReceived: ", userData.ToInt64()));
				Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant, byte[], bool> permanentCallback = Callbacks.IntPtrToPermanentCallback<Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant, byte[], bool>>(userData);
				using (NativeRealTimeRoom nativeRealTimeRoom = NativeRealTimeRoom.FromPointer(room))
				{
					using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = GooglePlayGames.Native.PInvoke.MultiplayerParticipant.FromPointer(participant))
					{
						if (permanentCallback != null)
						{
							byte[] numArray = null;
							if (dataLength.ToUInt64() != 0)
							{
								numArray = new byte[dataLength.ToUInt32()];
								Marshal.Copy(data, numArray, 0, (int)dataLength.ToUInt32());
							}
							try
							{
								permanentCallback(nativeRealTimeRoom, multiplayerParticipant, numArray, isReliable);
							}
							catch (Exception exception)
							{
								Logger.e(string.Concat("Error encountered executing InternalOnDataReceived. Smothering to avoid passing exception into Native: ", exception));
							}
						}
						else
						{
							return;
						}
					}
				}
			}
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnP2PConnectedCallback))]
		internal static void InternalOnP2PConnectedCallback(IntPtr room, IntPtr participant, IntPtr data)
		{
			GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.PerformRoomAndParticipantCallback("InternalOnP2PConnectedCallback", room, participant, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnP2PDisconnectedCallback))]
		internal static void InternalOnP2PDisconnectedCallback(IntPtr room, IntPtr participant, IntPtr data)
		{
			GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.PerformRoomAndParticipantCallback("InternalOnP2PDisconnectedCallback", room, participant, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnParticipantStatusChangedCallback))]
		internal static void InternalOnParticipantStatusChangedCallback(IntPtr room, IntPtr participant, IntPtr data)
		{
			GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.PerformRoomAndParticipantCallback("InternalOnParticipantStatusChangedCallback", room, participant, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback))]
		internal static void InternalOnRoomConnectedSetChangedCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealTimeEventListenerHelper#InternalOnRoomConnectedSetChangedCallback", Callbacks.Type.Permanent, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnRoomStatusChangedCallback))]
		internal static void InternalOnRoomStatusChangedCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealTimeEventListenerHelper#InternalOnRoomStatusChangedCallback", Callbacks.Type.Permanent, response, data);
		}

		internal static void PerformRoomAndParticipantCallback(string callbackName, IntPtr room, IntPtr participant, IntPtr data)
		{
			Logger.d(string.Concat("Entering ", callbackName));
			try
			{
				NativeRealTimeRoom nativeRealTimeRoom = NativeRealTimeRoom.FromPointer(room);
				using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = GooglePlayGames.Native.PInvoke.MultiplayerParticipant.FromPointer(participant))
				{
					Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> permanentCallback = Callbacks.IntPtrToPermanentCallback<Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant>>(data);
					if (permanentCallback != null)
					{
						permanentCallback(nativeRealTimeRoom, multiplayerParticipant);
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Logger.e(string.Concat(new object[] { "Error encountered executing ", callbackName, ". Smothering to avoid passing exception into Native: ", exception }));
			}
		}

		internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnDataReceivedCallback(Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant, byte[], bool> callback)
		{
			IntPtr intPtr = Callbacks.ToIntPtr(callback);
			Logger.d(string.Concat("OnData Callback has addr: ", intPtr.ToInt64()));
			GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnDataReceivedCallback(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnDataReceivedCallback(GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.InternalOnDataReceived), intPtr);
			return this;
		}

		internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnP2PConnectedCallback(Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> callback)
		{
			GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnP2PConnectedCallback(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnP2PConnectedCallback(GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.InternalOnP2PConnectedCallback), Callbacks.ToIntPtr(callback));
			return this;
		}

		internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnP2PDisconnectedCallback(Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> callback)
		{
			GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnP2PDisconnectedCallback(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnP2PDisconnectedCallback(GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.InternalOnP2PDisconnectedCallback), Callbacks.ToIntPtr(callback));
			return this;
		}

		internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnParticipantStatusChangedCallback(Action<NativeRealTimeRoom, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> callback)
		{
			GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnParticipantStatusChangedCallback(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnParticipantStatusChangedCallback(GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.InternalOnParticipantStatusChangedCallback), Callbacks.ToIntPtr(callback));
			return this;
		}

		internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnRoomConnectedSetChangedCallback(Action<NativeRealTimeRoom> callback)
		{
			GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnRoomConnectedSetChangedCallback(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback(GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.InternalOnRoomConnectedSetChangedCallback), GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.ToCallbackPointer(callback));
			return this;
		}

		internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnRoomStatusChangedCallback(Action<NativeRealTimeRoom> callback)
		{
			GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnRoomStatusChangedCallback(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnRoomStatusChangedCallback(GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.InternalOnRoomStatusChangedCallback), GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.ToCallbackPointer(callback));
			return this;
		}

		private static IntPtr ToCallbackPointer(Action<NativeRealTimeRoom> callback)
		{
			return Callbacks.ToIntPtr(new Action<IntPtr>((IntPtr result) => {
				NativeRealTimeRoom nativeRealTimeRoom = NativeRealTimeRoom.FromPointer(result);
				if (callback != null)
				{
					callback(nativeRealTimeRoom);
				}
				else if (nativeRealTimeRoom != null)
				{
					nativeRealTimeRoom.Dispose();
				}
			}));
		}
	}
}