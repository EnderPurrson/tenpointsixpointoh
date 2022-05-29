using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LicenseVerificationController : MonoBehaviour
{
	private readonly static IDictionary<VerificationErrorCode, string> _errorMessages;

	private LicenseVerificationManager _licenseVerificationManager;

	private readonly Action _start;

	private readonly Action _update;

	static LicenseVerificationController()
	{
		Dictionary<VerificationErrorCode, string> verificationErrorCodes = new Dictionary<VerificationErrorCode, string>()
		{
			{ VerificationErrorCode.None, "None" },
			{ VerificationErrorCode.BadResonceOrMessageOrSignature, "Bad responce code, or message, or signature" },
			{ VerificationErrorCode.InvalidSignature, "Invalid signature" },
			{ VerificationErrorCode.InsufficientFieldCount, "Insufficient field count" },
			{ VerificationErrorCode.ResponceMismatch, "Response mismatch" }
		};
		LicenseVerificationController._errorMessages = verificationErrorCodes;
	}

	public LicenseVerificationController()
	{
		LicenseVerificationController licenseVerificationController = this;
		bool flag = false;
		this._start = () => {
			try
			{
				licenseVerificationController._licenseVerificationManager = licenseVerificationController.GetComponent<LicenseVerificationManager>();
				if (licenseVerificationController._licenseVerificationManager != null)
				{
					if (Application.platform == RuntimePlatform.Android)
					{
						if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
						{
							UnityEngine.Object.DontDestroyOnLoad(licenseVerificationController.gameObject);
							licenseVerificationController.StartCoroutine(licenseVerificationController.WaitThenVerifyLicenseCoroutine());
						}
					}
				}
			}
			finally
			{
				flag = true;
			}
		};
		this._update = () => {
			if (!flag)
			{
				SceneManager.LoadScene(LicenseVerificationController.GetTerminalSceneName_f38d05cc(-208861748));
			}
		};
	}

	private static string GetErrorMessage(VerificationErrorCode errorCode)
	{
		string str;
		return (!LicenseVerificationController._errorMessages.TryGetValue(errorCode, out str) ? "Unknown" : str);
	}

	internal static LicenseVerificationController.PackageInfo GetPackageInfo()
	{
		LicenseVerificationController.PackageInfo packageInfo;
		LicenseVerificationController.PackageInfo base64String = new LicenseVerificationController.PackageInfo();
		LicenseVerificationController.PackageInfo empty = base64String;
		empty.PackageName = string.Empty;
		empty.SignatureHash = string.Empty;
		base64String = empty;
		try
		{
			AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
			if (currentActivity != null)
			{
				base64String.PackageName = currentActivity.Call<string>("getPackageName", new object[0]) ?? string.Empty;
				AndroidJavaObject androidJavaObject = currentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
				if (androidJavaObject != null)
				{
					AndroidJavaObject androidJavaObject1 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[] { base64String.PackageName, 64 });
					if (androidJavaObject1 != null)
					{
						AndroidJavaObject[] androidJavaObjectArray = androidJavaObject1.Get<AndroidJavaObject[]>("signatures");
						if (androidJavaObjectArray == null)
						{
							UnityEngine.Debug.LogWarning("signatures == null");
							packageInfo = base64String;
						}
						else if ((int)androidJavaObjectArray.Length == 1)
						{
							AndroidJavaObject androidJavaObject2 = androidJavaObjectArray[0];
							byte[] numArray = androidJavaObject2.Call<byte[]>("toByteArray", new object[0]);
							using (SHA1Managed sHA1Managed = new SHA1Managed())
							{
								byte[] numArray1 = sHA1Managed.ComputeHash(numArray);
								base64String.SignatureHash = Convert.ToBase64String(numArray1);
							}
							return base64String;
						}
						else
						{
							UnityEngine.Debug.LogWarning(string.Concat("signatures.Length == ", (int)androidJavaObjectArray.Length));
							packageInfo = base64String;
						}
					}
					else
					{
						UnityEngine.Debug.LogWarning("packageInfo == null");
						packageInfo = base64String;
					}
				}
				else
				{
					UnityEngine.Debug.LogWarning("manager == null");
					packageInfo = base64String;
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("activity == null");
				packageInfo = base64String;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Format("Error while retrieving Android package info:    {0}", exception));
			return base64String;
		}
		return packageInfo;
	}

	private static string GetTerminalSceneName_f38d05cc(uint gamma)
	{
		return "Clf38d05ccosingScene".Replace(gamma.ToString("x"), string.Empty);
	}

	private void HandleVerificationResponse(VerificationEventArgs e)
	{
		string errorMessage = LicenseVerificationController.GetErrorMessage(e.ErrorCode);
		UnityEngine.Debug.Log(string.Concat("HandleVerificationResponse(): Verification Error: ", errorMessage));
		FlurryPluginWrapper.LogEventWithParameterAndValue("Verification Error", "Message", errorMessage);
		e.ErrorCode != VerificationErrorCode.InvalidSignature;
		string str = string.Format("Verification {0:X3} {1}", (int)e.ReceivedResponseCode, e.ReceivedResponseCode);
		Dictionary<string, string> strs = new Dictionary<string, string>();
		if (e.ReceivedResponseCode != ResponseCode.ErrorContactingServer)
		{
			strs.Add("Received Version Code", e.ReceivedVersionCode.ToString());
			strs.Add("Received Package Name", e.ReceivedPackageName ?? string.Empty);
		}
		FlurryPluginWrapper.LogEvent(str);
	}

	private void Start()
	{
		if (this._start != null)
		{
			this._start();
		}
		else
		{
			SceneManager.LoadScene(LicenseVerificationController.GetTerminalSceneName_f38d05cc(-208861748));
		}
	}

	private void Update()
	{
		if (this._update != null)
		{
			this._update();
		}
		else
		{
			SceneManager.LoadScene(LicenseVerificationController.GetTerminalSceneName_f38d05cc(-208861748));
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitThenVerifyLicenseCoroutine()
	{
		LicenseVerificationController.u003cWaitThenVerifyLicenseCoroutineu003ec__Iterator16C variable = null;
		return variable;
	}

	internal struct PackageInfo
	{
		internal string PackageName
		{
			get;
			set;
		}

		internal string SignatureHash
		{
			get;
			set;
		}
	}

	[Flags]
	private enum PackageInfoFlags
	{
		GetSignatures = 64
	}
}