using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class AndroidSystem
	{
		private WeakReference _currentActivity;

		private readonly static Lazy<AndroidSystem> _instance;

		private readonly Lazy<long> _firstInstallTime = new Lazy<long>(new Func<long>(AndroidSystem.GetFirstInstallTime));

		private readonly Lazy<string> _packageName = new Lazy<string>(new Func<string>(AndroidSystem.GetPackageName));

		public AndroidJavaObject CurrentActivity
		{
			get
			{
				AndroidJavaObject target;
				try
				{
					if (this._currentActivity == null || !this._currentActivity.IsAlive)
					{
						AndroidJavaObject @static = (new AndroidJavaClass("com.unity3d.player.UnityPlayer")).GetStatic<AndroidJavaObject>("currentActivity");
						if (@static != null)
						{
							this._currentActivity = new WeakReference(@static, false);
							target = @static;
						}
						else
						{
							this._currentActivity = null;
							target = null;
						}
					}
					else
					{
						target = this._currentActivity.Target as AndroidJavaObject;
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Debug.LogWarning("Exception occured while getting Android current activity. See next log entry for details.");
					Debug.LogException(exception);
					target = null;
				}
				return target;
			}
		}

		public long FirstInstallTime
		{
			get
			{
				return this._firstInstallTime.Value;
			}
		}

		public static AndroidSystem Instance
		{
			get
			{
				return AndroidSystem._instance.Value;
			}
		}

		public string PackageName
		{
			get
			{
				return this._packageName.Value;
			}
		}

		static AndroidSystem()
		{
			AndroidSystem._instance = new Lazy<AndroidSystem>(() => new AndroidSystem());
		}

		private AndroidSystem()
		{
		}

		public string GetAdvertisingId()
		{
			string empty;
			if (Application.platform != RuntimePlatform.Android)
			{
				return string.Empty;
			}
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
				AndroidJavaObject currentActivity = this.CurrentActivity;
				if (currentActivity != null)
				{
					AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", new object[] { currentActivity });
					if (androidJavaObject != null)
					{
						empty = androidJavaObject.Call<string>("getId", new object[0]) ?? string.Empty;
					}
					else
					{
						Debug.LogWarning(string.Format("Failed to get Android advertising id: {0} == null", "adInfo"));
						empty = string.Empty;
					}
				}
				else
				{
					Debug.LogWarning(string.Format("Failed to get Android advertising id: {0} == null", "currentActivity"));
					empty = string.Empty;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Debug.LogWarning("Exception occured while getting Android advertising id. See next log entry for details.");
				Debug.LogException(exception);
				empty = string.Empty;
			}
			return empty;
		}

		private static long GetFirstInstallTime()
		{
			if (Application.isEditor)
			{
				return (long)0;
			}
			AndroidJavaObject androidJavaObject = AndroidSystem.Instance.CurrentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				throw new InvalidOperationException("getPackageManager() == null");
			}
			AndroidJavaObject androidJavaObject1 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[] { AndroidSystem.Instance.PackageName, 0 });
			if (androidJavaObject1 == null)
			{
				throw new InvalidOperationException("getPackageInfo() == null");
			}
			return androidJavaObject1.Get<long>("firstInstallTime");
		}

		private static string GetPackageName()
		{
			if (Application.isEditor)
			{
				return string.Empty;
			}
			AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
			return currentActivity.Call<string>("getPackageName", new object[0]) ?? string.Empty;
		}

		internal static int GetSdkVersion()
		{
			if (Application.isEditor)
			{
				return 0;
			}
			IntPtr intPtr = AndroidJNI.FindClass("android.os.Build$VERSION");
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(intPtr, "SDK_INT", "I");
			return AndroidJNI.GetStaticIntField(intPtr, staticFieldID);
		}

		public byte[] GetSignatureHash()
		{
			byte[] numArray;
			Lazy<byte[]> lazy = new Lazy<byte[]>(() => new byte[20]);
			if (Application.platform != RuntimePlatform.Android)
			{
				return lazy.Value;
			}
			AndroidJavaObject androidJavaObject = this.CurrentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				throw new InvalidOperationException("getPackageManager() == null");
			}
			AndroidJavaObject androidJavaObject1 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[] { this.PackageName, 64 });
			if (androidJavaObject1 == null)
			{
				throw new InvalidOperationException("getPackageInfo() == null");
			}
			AndroidJavaObject[] androidJavaObjectArray = androidJavaObject1.Get<AndroidJavaObject[]>("signatures");
			if (androidJavaObjectArray == null)
			{
				throw new InvalidOperationException("signatures() == null");
			}
			using (SHA1Managed sHA1Managed = new SHA1Managed())
			{
				IEnumerable<byte[]> numArrays = (
					from s in (IEnumerable<AndroidJavaObject>)androidJavaObjectArray
					select s.Call<byte[]>("toByteArray", new object[0]) into s
					where s != null
					select s).Select<byte[], byte[]>(new Func<byte[], byte[]>(sHA1Managed.ComputeHash));
				numArray = numArrays.FirstOrDefault<byte[]>() ?? lazy.Value;
			}
			return numArray;
		}
	}
}