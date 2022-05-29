using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeMessageListenerHelper : BaseReferenceHolder
	{
		internal NativeMessageListenerHelper() : base(MessageListenerHelper.MessageListenerHelper_Construct())
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			MessageListenerHelper.MessageListenerHelper_Dispose(selfPointer);
		}

		[MonoPInvokeCallback(typeof(MessageListenerHelper.OnDisconnectedCallback))]
		private static void InternalOnDisconnectedCallback(long id, string lostEndpointId, IntPtr userData)
		{
			Action<long, string> permanentCallback = Callbacks.IntPtrToPermanentCallback<Action<long, string>>(userData);
			if (permanentCallback == null)
			{
				return;
			}
			try
			{
				permanentCallback(id, lostEndpointId);
			}
			catch (Exception exception)
			{
				Logger.e(string.Concat("Error encountered executing NativeMessageListenerHelper#InternalOnDisconnectedCallback. Smothering to avoid passing exception into Native: ", exception));
			}
		}

		[MonoPInvokeCallback(typeof(MessageListenerHelper.OnMessageReceivedCallback))]
		private static void InternalOnMessageReceivedCallback(long id, string name, IntPtr data, UIntPtr dataLength, bool isReliable, IntPtr userData)
		{
			NativeMessageListenerHelper.OnMessageReceived permanentCallback = Callbacks.IntPtrToPermanentCallback<NativeMessageListenerHelper.OnMessageReceived>(userData);
			if (permanentCallback == null)
			{
				return;
			}
			try
			{
				permanentCallback(id, name, Callbacks.IntPtrAndSizeToByteArray(data, dataLength), isReliable);
			}
			catch (Exception exception)
			{
				Logger.e(string.Concat("Error encountered executing NativeMessageListenerHelper#InternalOnMessageReceivedCallback. Smothering to avoid passing exception into Native: ", exception));
			}
		}

		internal void SetOnDisconnectedCallback(Action<long, string> callback)
		{
			MessageListenerHelper.MessageListenerHelper_SetOnDisconnectedCallback(base.SelfPtr(), new MessageListenerHelper.OnDisconnectedCallback(NativeMessageListenerHelper.InternalOnDisconnectedCallback), Callbacks.ToIntPtr(callback));
		}

		internal void SetOnMessageReceivedCallback(NativeMessageListenerHelper.OnMessageReceived callback)
		{
			MessageListenerHelper.MessageListenerHelper_SetOnMessageReceivedCallback(base.SelfPtr(), new MessageListenerHelper.OnMessageReceivedCallback(NativeMessageListenerHelper.InternalOnMessageReceivedCallback), Callbacks.ToIntPtr(callback));
		}

		internal delegate void OnMessageReceived(long localClientId, string remoteEndpointId, byte[] data, bool isReliable);
	}
}