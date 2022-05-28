using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using DevToDev.Core.Network;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Builders;
using DevToDev.Data.Consts;
using DevToDev.Logic;
using UnityEngine;

namespace DevToDev.Cheat
{
	internal static class CheatClient
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass1
		{
			public string receipt;

			public OnReceiptVerifyCallback callback;

			public void _003CRetrieveCertificate_003Eb__0(Response response, object state)
			{
				if (response.ResponseState == ResponseState.Success)
				{
					OnCertificateGot(receipt, callback, Encoding.get_UTF8().GetBytes(response.ResposeString));
				}
				else
				{
					(state as OnReceiptVerifyCallback)(ReceiptVerificationStatus.ReceiptNotValid);
				}
			}
		}

		private static readonly string STATUS = "status";

		private static void OnCertificateGot(string receipt, OnReceiptVerifyCallback callback, byte[] certificate)
		{
			if (certificate == null || certificate.Length < 0)
			{
				Log.D("Microsoft certificate was not found in Miscrosoft database.");
				callback(ReceiptVerificationStatus.ReceiptNotValid);
			}
			else
			{
				CreateRequestVerifyMetric(receipt, certificate, null, callback);
			}
		}

		private static void RetrieveCertificate(string receipt, OnReceiptVerifyCallback callback, string certificateId)
		{
			_003C_003Ec__DisplayClass1 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass1();
			_003C_003Ec__DisplayClass.receipt = receipt;
			_003C_003Ec__DisplayClass.callback = callback;
			Request request = new RequestBuilder().Url(string.Format("https://go.microsoft.com/fwlink/?LinkId=246509&cid={0}", (object)certificateId)).NeedSigned(false).Build();
			Log.D("Send: " + request.Url);
			NetworkClient networkClient = new NetworkClient(_003C_003Ec__DisplayClass._003CRetrieveCertificate_003Eb__0);
			networkClient.Send(request, _003C_003Ec__DisplayClass.callback);
		}

		public static void VerifyPayment(string receipt, string signature, string publicKey, OnReceiptVerifyCallback callback)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			if (UnityPlayerPlatform.isUnityWSAPlatform())
			{
				if (receipt != null && receipt.get_Length() > 0)
				{
					try
					{
						MemoryStream val = new MemoryStream();
						try
						{
							StreamWriter val2 = new StreamWriter((Stream)(object)val);
							try
							{
								((TextWriter)val2).Write(receipt);
								((TextWriter)val2).Flush();
								((Stream)val).Seek(0L, (SeekOrigin)0);
								string certificateId = CheatClientPlatform.GetCertificateId(val);
								if (certificateId == null)
								{
									Log.D("Receipt xml does not contain Microsoft certificateId.");
									callback(ReceiptVerificationStatus.ReceiptNotValid);
								}
								else
								{
									RetrieveCertificate(receipt, callback, certificateId);
								}
								return;
							}
							finally
							{
								if (val2 != null)
								{
									((global::System.IDisposable)val2).Dispose();
								}
							}
						}
						finally
						{
							if (val != null)
							{
								((global::System.IDisposable)val).Dispose();
							}
						}
					}
					catch (global::System.Exception)
					{
						callback(ReceiptVerificationStatus.ReceiptInternalError);
						return;
					}
				}
				Log.E("\"receipt\" parametr must contain receipt data. \"receipt\" is null or empty.");
				callback(ReceiptVerificationStatus.ReceiptNotValid);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				CreateRequestVerifyMetric(receipt, null, null, callback);
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				CreateRequestVerifyMetric(receipt, Encoding.get_UTF8().GetBytes(signature), publicKey, callback);
			}
		}

		private static void CreateRequestVerifyMetric(string receipt, byte[] signature, string publicKey, OnReceiptVerifyCallback callback)
		{
			VerifyEvent verifyEvent = new VerifyEvent(receipt, signature, publicKey);
			string text = verifyEvent.GetAdditionalDataJson().ToJSON(0);
			byte[] bytes = Encoding.get_UTF8().GetBytes(text);
			Request request = new RequestBuilder().Url(NetworkConsts.MAIN_SERVER + NetworkConsts.WEB).AddParameter(RequestParam.ID, SDKClient.Instance.AppKey).AddParameter(RequestParam.UID, SDKClient.Instance.UsersStorage.Device.DeviceId)
				.AddParameter(RequestParam.FUNCTION, CheatNetworkConst.CHECK_RECEIPT)
				.Secret(SDKClient.Instance.AppSecret)
				.NeedSigned(true)
				.PostData(bytes)
				.Build();
			Log.D("Send: " + request.Url);
			Log.D("Post data: " + text);
			NetworkClient networkClient = new NetworkClient(OnVerifyRequestSend);
			networkClient.Send(request, callback);
		}

		private static void OnVerifyRequestSend(Response response, object state)
		{
			Log.D(string.Concat(new object[4] { "Response state: ", response.ResponseState, " data: ", response.ResposeString }));
			if (response.ResponseState == ResponseState.Success)
			{
				try
				{
					JSONNode jSONNode = JSON.Parse(response.ResposeString);
					if (jSONNode == null)
					{
						(state as OnReceiptVerifyCallback)(ReceiptVerificationStatus.ReceiptServerError);
					}
					else if (jSONNode[STATUS] != null)
					{
						if (jSONNode[STATUS].AsInt == 0)
						{
							(state as OnReceiptVerifyCallback)(ReceiptVerificationStatus.ReceiptValid);
						}
						else
						{
							(state as OnReceiptVerifyCallback)(ReceiptVerificationStatus.ReceiptNotValid);
						}
					}
					else
					{
						(state as OnReceiptVerifyCallback)(ReceiptVerificationStatus.ReceiptServerError);
					}
					return;
				}
				catch (global::System.Exception)
				{
					(state as OnReceiptVerifyCallback)(ReceiptVerificationStatus.ReceiptServerError);
					return;
				}
			}
			(state as OnReceiptVerifyCallback)(ReceiptVerificationStatus.ReceiptServerError);
		}
	}
}
