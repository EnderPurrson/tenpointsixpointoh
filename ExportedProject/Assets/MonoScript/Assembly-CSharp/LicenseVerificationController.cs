using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LicenseVerificationController : MonoBehaviour
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct PackageInfo
	{
		internal string PackageName { get; set; }

		internal string SignatureHash { get; set; }
	}

	[Flags]
	private enum PackageInfoFlags
	{
		GetSignatures = 0x40
	}

	[CompilerGenerated]
	private sealed class _003CLicenseVerificationController_003Ec__AnonStorey2C9
	{
		internal bool startExecuted;

		internal LicenseVerificationController _003C_003Ef__this;

		internal void _003C_003Em__384()
		{
			try
			{
				_003C_003Ef__this._licenseVerificationManager = _003C_003Ef__this.GetComponent<LicenseVerificationManager>();
				if (!(_003C_003Ef__this._licenseVerificationManager == null) && Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					UnityEngine.Object.DontDestroyOnLoad(_003C_003Ef__this.gameObject);
					_003C_003Ef__this.StartCoroutine(_003C_003Ef__this.WaitThenVerifyLicenseCoroutine());
				}
			}
			finally
			{
				startExecuted = true;
			}
		}

		internal void _003C_003Em__385()
		{
			if (!startExecuted)
			{
				SceneManager.LoadScene(GetTerminalSceneName_f38d05cc(4086105548u));
			}
		}
	}

	private static readonly IDictionary<VerificationErrorCode, string> _errorMessages = new Dictionary<VerificationErrorCode, string>
	{
		{
			VerificationErrorCode.None,
			"None"
		},
		{
			VerificationErrorCode.BadResonceOrMessageOrSignature,
			"Bad responce code, or message, or signature"
		},
		{
			VerificationErrorCode.InvalidSignature,
			"Invalid signature"
		},
		{
			VerificationErrorCode.InsufficientFieldCount,
			"Insufficient field count"
		},
		{
			VerificationErrorCode.ResponceMismatch,
			"Response mismatch"
		}
	};

	private LicenseVerificationManager _licenseVerificationManager;

	private readonly Action _start;

	private readonly Action _update;

	public LicenseVerificationController()
	{
		_003CLicenseVerificationController_003Ec__AnonStorey2C9 _003CLicenseVerificationController_003Ec__AnonStorey2C = new _003CLicenseVerificationController_003Ec__AnonStorey2C9();
		base._002Ector();
		_003CLicenseVerificationController_003Ec__AnonStorey2C._003C_003Ef__this = this;
		_003CLicenseVerificationController_003Ec__AnonStorey2C.startExecuted = false;
		_start = _003CLicenseVerificationController_003Ec__AnonStorey2C._003C_003Em__384;
		_update = _003CLicenseVerificationController_003Ec__AnonStorey2C._003C_003Em__385;
	}

	private static string GetTerminalSceneName_f38d05cc(uint gamma)
	{
		return "Clf38d05ccosingScene".Replace(gamma.ToString("x"), string.Empty);
	}

	private void Start()
	{
		if (_start == null)
		{
			SceneManager.LoadScene(GetTerminalSceneName_f38d05cc(4086105548u));
		}
		else
		{
			_start();
		}
	}

	private void Update()
	{
		if (_update == null)
		{
			SceneManager.LoadScene(GetTerminalSceneName_f38d05cc(4086105548u));
		}
		else
		{
			_update();
		}
	}

	internal static PackageInfo GetPackageInfo()
	{
		PackageInfo packageInfo = default(PackageInfo);
		packageInfo.PackageName = string.Empty;
		packageInfo.SignatureHash = string.Empty;
		PackageInfo result = packageInfo;
		try
		{
			AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
			if (currentActivity == null)
			{
				Debug.LogWarning("activity == null");
				return result;
			}
			result.PackageName = currentActivity.Call<string>("getPackageName", new object[0]) ?? string.Empty;
			AndroidJavaObject androidJavaObject = currentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				Debug.LogWarning("manager == null");
				return result;
			}
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[2] { result.PackageName, 64 });
			if (androidJavaObject2 == null)
			{
				Debug.LogWarning("packageInfo == null");
				return result;
			}
			AndroidJavaObject[] array = androidJavaObject2.Get<AndroidJavaObject[]>("signatures");
			if (array == null)
			{
				Debug.LogWarning("signatures == null");
				return result;
			}
			if (array.Length != 1)
			{
				Debug.LogWarning("signatures.Length == " + array.Length);
				return result;
			}
			AndroidJavaObject androidJavaObject3 = array[0];
			byte[] buffer = androidJavaObject3.Call<byte[]>("toByteArray", new object[0]);
			using (SHA1Managed sHA1Managed = new SHA1Managed())
			{
				byte[] inArray = sHA1Managed.ComputeHash(buffer);
				result.SignatureHash = Convert.ToBase64String(inArray);
				return result;
			}
		}
		catch (Exception arg)
		{
			string message = string.Format("Error while retrieving Android package info:    {0}", arg);
			Debug.LogError(message);
			return result;
		}
	}

	private static string GetErrorMessage(VerificationErrorCode errorCode)
	{
		string value;
		return (!_errorMessages.TryGetValue(errorCode, out value)) ? "Unknown" : value;
	}

	private void HandleVerificationResponse(VerificationEventArgs e)
	{
		string errorMessage = GetErrorMessage(e.ErrorCode);
		Debug.Log("HandleVerificationResponse(): Verification Error: " + errorMessage);
		FlurryPluginWrapper.LogEventWithParameterAndValue("Verification Error", "Message", errorMessage);
		if (e.ErrorCode == VerificationErrorCode.InvalidSignature)
		{
		}
		string eventName = string.Format("Verification {0:X3} {1}", (int)e.ReceivedResponseCode, e.ReceivedResponseCode);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (e.ReceivedResponseCode != ResponseCode.ErrorContactingServer)
		{
			dictionary.Add("Received Version Code", e.ReceivedVersionCode.ToString());
			dictionary.Add("Received Package Name", e.ReceivedPackageName ?? string.Empty);
		}
		FlurryPluginWrapper.LogEvent(eventName);
	}

	private IEnumerator WaitThenVerifyLicenseCoroutine()
	{
		yield return new WaitForSeconds(20f);
		_licenseVerificationManager.Verify(HandleVerificationResponse);
	}
}
