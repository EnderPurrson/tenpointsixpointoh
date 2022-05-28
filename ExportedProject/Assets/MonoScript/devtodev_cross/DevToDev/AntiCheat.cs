using System;
using System.Runtime.CompilerServices;
using DevToDev.Cheat;
using DevToDev.Core.Utils;
using DevToDev.Logic;
using UnityEngine;

namespace DevToDev
{
	public static class AntiCheat
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2
		{
			public string receipt;

			public string signature;

			public string publicKey;

			public OnReceiptVerifyCallback callback;

			public void _003CVerifyReceipt_003Eb__0()
			{
				CheatClient.VerifyPayment(receipt, signature, publicKey, callback);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass5
		{
			public OnTimeVerifyCallback callback;

			public void _003CVerifyTime_003Eb__4()
			{
				TimeManager.Instance.CheckTime(callback);
			}
		}

		public static void VerifyReceipt(string receipt, string signature, string publicKey, OnReceiptVerifyCallback callback)
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			Action val = null;
			_003C_003Ec__DisplayClass2 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass2();
			_003C_003Ec__DisplayClass.receipt = receipt;
			_003C_003Ec__DisplayClass.signature = signature;
			_003C_003Ec__DisplayClass.publicKey = publicKey;
			_003C_003Ec__DisplayClass.callback = callback;
			if (SDKClient.Instance.IsInitialized && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || UnityPlayerPlatform.isUnityWSAPlatform()) && _003C_003Ec__DisplayClass.receipt != null && _003C_003Ec__DisplayClass.callback != null)
			{
				AsyncOperationDispatcher asyncOperationDispatcher = SDKClient.Instance.AsyncOperationDispatcher;
				if (val == null)
				{
					val = new Action(_003C_003Ec__DisplayClass._003CVerifyReceipt_003Eb__0);
				}
				asyncOperationDispatcher.DispatchOnMainThread(val);
			}
		}

		public static void VerifyTime(OnTimeVerifyCallback callback)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			_003C_003Ec__DisplayClass5 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass5();
			_003C_003Ec__DisplayClass.callback = callback;
			if (SDKClient.Instance.IsInitialized && _003C_003Ec__DisplayClass.callback != null)
			{
				SDKClient.Instance.AsyncOperationDispatcher.DispatchOnMainThread(new Action(_003C_003Ec__DisplayClass._003CVerifyTime_003Eb__4));
			}
		}
	}
}
