using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ECPNManager : MonoBehaviour
{
	public string GoogleCloudMessageProjectID = "339873998127";

	public string phpFilesLocation = "https://secure.pixelgunserver.com/ecpn";

	public string packageName = "com.pixel.gun3d";

	private string devToken;

	private AndroidJavaObject playerActivityContext;

	public ECPNManager()
	{
	}

	[DebuggerHidden]
	private IEnumerator DeleteDeviceFromServer(string rID)
	{
		ECPNManager.u003cDeleteDeviceFromServeru003ec__Iterator2 variable = null;
		return variable;
	}

	public string GetDevToken()
	{
		return this.devToken;
	}

	public void RegisterAndroidDevice(string rID)
	{
		UnityEngine.Debug.Log(string.Concat("DeviceToken: ", rID));
		base.StartCoroutine(this.StoreDeviceID(rID, "android"));
	}

	public void RequestDeviceToken()
	{
		if (!Application.isEditor)
		{
			if (this.playerActivityContext == null)
			{
				this.playerActivityContext = (new AndroidJavaClass("com.unity3d.player.UnityPlayer")).GetStatic<AndroidJavaObject>("currentActivity");
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass(string.Concat(this.packageName, ".GCMRegistration"));
			androidJavaClass.CallStatic("RegisterDevice", new object[] { this.playerActivityContext, this.GoogleCloudMessageProjectID });
		}
	}

	public void RequestUnregisterDevice()
	{
		if (this.playerActivityContext == null)
		{
			this.playerActivityContext = (new AndroidJavaClass("com.unity3d.player.UnityPlayer")).GetStatic<AndroidJavaObject>("currentActivity");
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(string.Concat(this.packageName, ".GCMRegistration"));
		androidJavaClass.CallStatic("UnregisterDevice", new object[] { this.playerActivityContext });
	}

	[DebuggerHidden]
	private IEnumerator SendECPNmessage()
	{
		ECPNManager.u003cSendECPNmessageu003ec__Iterator1 variable = null;
		return variable;
	}

	public void SendMessageToAll()
	{
		base.StartCoroutine(this.SendECPNmessage());
	}

	[DebuggerHidden]
	private IEnumerator StoreDeviceID(string rID, string os)
	{
		ECPNManager.u003cStoreDeviceIDu003ec__Iterator0 variable = null;
		return variable;
	}

	public void UnregisterDevice(string rID)
	{
		UnityEngine.Debug.Log(string.Concat("Unregister DeviceToken: ", rID));
		base.StartCoroutine(this.DeleteDeviceFromServer(rID));
	}
}