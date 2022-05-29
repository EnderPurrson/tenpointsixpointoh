using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Rilisoft
{
	public class LicenseVerificationManager : MonoBehaviour, IDisposable
	{
		public TextAsset serviceBinder;

		public string publicKeyModulusBase64;

		public string publicKeyExponentBase64;

		private readonly static SHA1 _dummy;

		private AndroidJavaObject _activity;

		private AndroidJavaObject _lvlCheckType;

		private bool _disposed = true;

		private string _packageName = string.Empty;

		private readonly System.Random _prng = new System.Random();

		private RSAParameters _publicKey = new RSAParameters();

		static LicenseVerificationManager()
		{
			LicenseVerificationManager._dummy = new SHA1CryptoServiceProvider();
		}

		public LicenseVerificationManager()
		{
		}

		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			Resources.UnloadAsset(this.serviceBinder);
			this.serviceBinder = null;
			if (this._activity != null)
			{
				this._activity.Dispose();
				this._activity = null;
			}
			if (this._lvlCheckType != null)
			{
				this._lvlCheckType.Dispose();
				this._lvlCheckType = null;
			}
			this._disposed = true;
		}

		private void LoadServiceBinder()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return;
			}
			this._activity = AndroidSystem.Instance.CurrentActivity;
			this._packageName = this._activity.Call<string>("getPackageName", new object[0]);
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogFormat("{0}.LoadServiceBinder(): Skipping when target platform is Amazon.", new object[] { base.GetType().Name });
				return;
			}
			string str = Path.Combine(this._activity.Call<AndroidJavaObject>("getCacheDir", new object[0]).Call<string>("getPath", new object[0]), this._packageName);
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Cache directory: {0}", new object[] { str });
			}
			byte[] numArray = this.serviceBinder.bytes;
			Directory.CreateDirectory(str);
			File.WriteAllBytes(string.Concat(str, "/classes.jar"), numArray);
			Directory.CreateDirectory(string.Concat(str, "/odex"));
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("dalvik.system.DexClassLoader", new object[] { string.Concat(str, "/classes.jar"), string.Concat(str, "/odex"), null, this._activity.Call<AndroidJavaObject>("getClassLoader", new object[0]) }))
			{
				this._lvlCheckType = androidJavaObject.Call<AndroidJavaObject>("findClass", new object[] { "com.unity3d.plugin.lvl.ServiceBinder" });
			}
			if (this._lvlCheckType == null)
			{
				Debug.Log("Could not instantiate ServiceBinder.");
				this.Dispose();
			}
			Directory.Delete(str, true);
		}

		private void OnDestroy()
		{
			this.Dispose();
		}

		private void Process(AndroidJavaObject lvlCheck, int nonce, Action<VerificationEventArgs> completionHandler)
		{
			Debug.LogFormat("> {0}.Process()", new object[] { base.GetType().Name });
			try
			{
				int num = lvlCheck.Get<int>("_arg0");
				string str = lvlCheck.Get<string>("_arg1");
				string str1 = lvlCheck.Get<string>("_arg2");
				VerificationEventArgs verificationEventArg = new VerificationEventArgs()
				{
					ReceivedResponseCode = (ResponseCode)num,
					SentNonce = nonce,
					SentPackageName = this._packageName
				};
				VerificationEventArgs num1 = verificationEventArg;
				if (num < 0 || string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str1))
				{
					num1.ErrorCode = VerificationErrorCode.BadResonceOrMessageOrSignature;
					completionHandler(num1);
				}
				else
				{
					try
					{
						byte[] bytes = Encoding.UTF8.GetBytes(str);
						byte[] numArray = Convert.FromBase64String(str1);
						RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
						rSACryptoServiceProvider.ImportParameters(this._publicKey);
						if (!rSACryptoServiceProvider.VerifyHash((new SHA1Managed()).ComputeHash(bytes), CryptoConfig.MapNameToOID("SHA1"), numArray))
						{
							num1.ErrorCode = VerificationErrorCode.InvalidSignature;
							completionHandler(num1);
							goto Label1;
						}
					}
					catch (FormatException formatException)
					{
						num1.ErrorCode = VerificationErrorCode.FormatError;
						completionHandler(num1);
						goto Label1;
					}
					int num2 = str.IndexOf(':');
					string[] strArrays = ((num2 != -1 ? str.Substring(0, num2) : str)).Split(new char[] { '|' });
					if ((int)strArrays.Length < 6)
					{
						num1.ErrorCode = VerificationErrorCode.InsufficientFieldCount;
						completionHandler(num1);
					}
					else if (strArrays[0].CompareTo(num.ToString()) == 0)
					{
						num1.ReceivedNonce = Convert.ToInt32(strArrays[1]);
						num1.ReceivedPackageName = strArrays[2];
						num1.ReceivedVersionCode = Convert.ToInt32(strArrays[3]);
						num1.ReceivedUserId = strArrays[4];
						num1.ReceivedTimestamp = Convert.ToInt64(strArrays[5]);
						lvlCheck.Dispose();
						completionHandler(num1);
						return;
					}
					else
					{
						num1.ErrorCode = VerificationErrorCode.ResponceMismatch;
						completionHandler(num1);
					}
				}
			Label1:
				object[] objArray = new object[] { num, str, str1 };
				Debug.LogWarningFormat("Response code: {0}    Message: '{1}'    Signature: '{2}'", objArray);
			}
			finally
			{
				Debug.LogFormat("< {0}.Process()", new object[] { base.GetType().Name });
			}
		}

		private void Start()
		{
			Debug.LogFormat("> {0}.Start()", new object[] { base.GetType().Name });
			try
			{
				if (this.serviceBinder == null || string.IsNullOrEmpty(this.publicKeyModulusBase64) || string.IsNullOrEmpty(this.publicKeyExponentBase64))
				{
					Debug.LogWarning("Object not properly initialized.");
				}
				else
				{
					this._publicKey.Modulus = Convert.FromBase64String(this.publicKeyModulusBase64);
					this._publicKey.Exponent = Convert.FromBase64String(this.publicKeyExponentBase64);
					bool rawClass = false;
					try
					{
						if (Application.platform == RuntimePlatform.Android)
						{
							rawClass = (new AndroidJavaClass("android.os.Build")).GetRawClass() != IntPtr.Zero;
						}
					}
					catch (Exception exception)
					{
						Debug.LogWarning(exception);
					}
					if (rawClass)
					{
						Debug.LogFormat("{0}.Start() > LoadServiceBinder()", new object[] { base.GetType().Name });
						try
						{
							this.LoadServiceBinder();
						}
						finally
						{
							Debug.LogFormat("{0}.Start() < LoadServiceBinder()", new object[] { base.GetType().Name });
						}
						this._disposed = false;
					}
					else
					{
						Debug.LogWarning("Not running on Android.");
					}
				}
			}
			finally
			{
				Debug.LogFormat("< {0}.Start()", new object[] { base.GetType().Name });
			}
		}

		public void Verify(Action<VerificationEventArgs> completionHandler)
		{
			if (this._disposed)
			{
				Debug.LogWarningFormat("Object disposed: {0}", new object[] { base.GetType().Name });
				return;
			}
			if (completionHandler == null)
			{
				Debug.LogWarning("Completion handler should not be null.");
				return;
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return;
			}
			int num = this._prng.Next();
			object[] objArray = new object[] { new AndroidJavaObject[] { this._activity } };
			if (this._lvlCheckType == null)
			{
				Debug.LogWarning("LvlCheck is null.");
				return;
			}
			AndroidJavaObject[] androidJavaObjectArray = this._lvlCheckType.Call<AndroidJavaObject[]>("getConstructors", new object[0]);
			AndroidJavaObject androidJavaObject = androidJavaObjectArray[0].Call<AndroidJavaObject>("newInstance", objArray);
			androidJavaObject.Call("create", new object[] { num, new AndroidJavaRunnable(() => this.Process(androidJavaObject, num, completionHandler)) });
		}
	}
}