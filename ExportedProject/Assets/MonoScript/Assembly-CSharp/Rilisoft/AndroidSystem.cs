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

		private static readonly Lazy<AndroidSystem> _instance;

		private readonly Lazy<long> _firstInstallTime = new Lazy<long>(GetFirstInstallTime);

		private readonly Lazy<string> _packageName = new Lazy<string>(GetPackageName);

		[CompilerGenerated]
		private static Func<AndroidSystem> _003C_003Ef__am_0024cache4;

		[CompilerGenerated]
		private static Func<byte[]> _003C_003Ef__am_0024cache5;

		[CompilerGenerated]
		private static Func<AndroidJavaObject, byte[]> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<byte[], bool> _003C_003Ef__am_0024cache7;

		public static AndroidSystem Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public AndroidJavaObject CurrentActivity
		{
			get
			{
				//Discarded unreachable code: IL_0070, IL_008d
				try
				{
					if (_currentActivity != null && _currentActivity.IsAlive)
					{
						return _currentActivity.Target as AndroidJavaObject;
					}
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					if (@static == null)
					{
						_currentActivity = null;
						return null;
					}
					_currentActivity = new WeakReference(@static, false);
					return @static;
				}
				catch (Exception exception)
				{
					Debug.LogWarning("Exception occured while getting Android current activity. See next log entry for details.");
					Debug.LogException(exception);
					return null;
				}
			}
		}

		public long FirstInstallTime
		{
			get
			{
				return _firstInstallTime.Value;
			}
		}

		public string PackageName
		{
			get
			{
				return _packageName.Value;
			}
		}

		private AndroidSystem()
		{
		}

		static AndroidSystem()
		{
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003C_instance_003Em__241;
			}
			_instance = new Lazy<AndroidSystem>(_003C_003Ef__am_0024cache4);
		}

		public byte[] GetSignatureHash()
		{
			//Discarded unreachable code: IL_0133
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003CGetSignatureHash_003Em__242;
			}
			Lazy<byte[]> lazy = new Lazy<byte[]>(_003C_003Ef__am_0024cache5);
			if (Application.platform != RuntimePlatform.Android)
			{
				return lazy.Value;
			}
			AndroidJavaObject androidJavaObject = CurrentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				throw new InvalidOperationException("getPackageManager() == null");
			}
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[2] { PackageName, 64 });
			if (androidJavaObject2 == null)
			{
				throw new InvalidOperationException("getPackageInfo() == null");
			}
			AndroidJavaObject[] array = androidJavaObject2.Get<AndroidJavaObject[]>("signatures");
			if (array == null)
			{
				throw new InvalidOperationException("signatures() == null");
			}
			using (SHA1Managed @object = new SHA1Managed())
			{
				if (_003C_003Ef__am_0024cache6 == null)
				{
					_003C_003Ef__am_0024cache6 = _003CGetSignatureHash_003Em__243;
				}
				IEnumerable<byte[]> source = array.Select(_003C_003Ef__am_0024cache6);
				if (_003C_003Ef__am_0024cache7 == null)
				{
					_003C_003Ef__am_0024cache7 = _003CGetSignatureHash_003Em__244;
				}
				IEnumerable<byte[]> source2 = source.Where(_003C_003Ef__am_0024cache7).Select(@object.ComputeHash);
				return source2.FirstOrDefault() ?? lazy.Value;
			}
		}

		public string GetAdvertisingId()
		{
			//Discarded unreachable code: IL_00ae, IL_00d2
			if (Application.platform != RuntimePlatform.Android)
			{
				return string.Empty;
			}
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
				AndroidJavaObject currentActivity = CurrentActivity;
				if (currentActivity == null)
				{
					Debug.LogWarning(string.Format("Failed to get Android advertising id: {0} == null", "currentActivity"));
					return string.Empty;
				}
				AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", new object[1] { currentActivity });
				if (androidJavaObject == null)
				{
					Debug.LogWarning(string.Format("Failed to get Android advertising id: {0} == null", "adInfo"));
					return string.Empty;
				}
				string text = androidJavaObject.Call<string>("getId", new object[0]);
				return text ?? string.Empty;
			}
			catch (Exception exception)
			{
				Debug.LogWarning("Exception occured while getting Android advertising id. See next log entry for details.");
				Debug.LogException(exception);
				return string.Empty;
			}
		}

		private static long GetFirstInstallTime()
		{
			if (Application.isEditor)
			{
				return 0L;
			}
			AndroidJavaObject androidJavaObject = Instance.CurrentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				throw new InvalidOperationException("getPackageManager() == null");
			}
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[2] { Instance.PackageName, 0 });
			if (androidJavaObject2 == null)
			{
				throw new InvalidOperationException("getPackageInfo() == null");
			}
			return androidJavaObject2.Get<long>("firstInstallTime");
		}

		internal static int GetSdkVersion()
		{
			if (Application.isEditor)
			{
				return 0;
			}
			IntPtr clazz = AndroidJNI.FindClass("android.os.Build$VERSION");
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
			return AndroidJNI.GetStaticIntField(clazz, staticFieldID);
		}

		private static string GetPackageName()
		{
			if (Application.isEditor)
			{
				return string.Empty;
			}
			AndroidJavaObject currentActivity = Instance.CurrentActivity;
			return currentActivity.Call<string>("getPackageName", new object[0]) ?? string.Empty;
		}

		[CompilerGenerated]
		private static AndroidSystem _003C_instance_003Em__241()
		{
			return new AndroidSystem();
		}

		[CompilerGenerated]
		private static byte[] _003CGetSignatureHash_003Em__242()
		{
			return new byte[20];
		}

		[CompilerGenerated]
		private static byte[] _003CGetSignatureHash_003Em__243(AndroidJavaObject s)
		{
			return s.Call<byte[]>("toByteArray", new object[0]);
		}

		[CompilerGenerated]
		private static bool _003CGetSignatureHash_003Em__244(byte[] s)
		{
			return s != null;
		}
	}
}
