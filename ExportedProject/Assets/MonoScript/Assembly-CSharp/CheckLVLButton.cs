using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class CheckLVLButton : MonoBehaviour
{
	public TextAsset ServiceBinder;

	private string m_PublicKey_Modulus_Base64 = "AKE8zE2qrBYWssLwhSmsBcC+SjPnYyyXzgk/62xkEXx5h2JjmNrATIA1rAP+u0s8ehFU2T1ZRcxzIJAKtU8HS/wOWmBqRs2gl+6PenMqfEesEhfsQro/viwJE2g5y1tL3iNm5HtxFR7twLfcYDOOnvlQ7nEVKkC73+kkEW9eZ7n5WOYtPMSp3sIF+yOFNh9EbwGQ8qIqfqmT5iOGb2rnwH3NI8GW8O1xoleNc4m2Ny1NDee5mCpOdVsfrTmie05HPUZdalQk42/m8F7IU6oVV1T+q+JGmy1sP/DiVIdEpuvZW6bOmpj+7z8ue9V47HAkzC310Gp9fefax2zYJG9piy0=";

	private string m_PublicKey_Exponent_Base64 = "AQAB";

	private RSAParameters m_PublicKey = new RSAParameters();

	private bool m_RunningOnAndroid;

	private AndroidJavaObject m_Activity;

	private AndroidJavaObject m_LVLCheckType;

	private AndroidJavaObject m_LVLCheck;

	private string m_ButtonMessage = "Invalid LVL key!\nCheck the source...";

	private bool m_ButtonEnabled = true;

	private string m_PackageName;

	private int m_Nonce;

	private bool m_LVL_Received;

	private string m_ResponseCode_Received;

	private string m_PackageName_Received;

	private int m_Nonce_Received;

	private int m_VersionCode_Received;

	private string m_UserID_Received;

	private string m_Timestamp_Received;

	private int m_MaxRetry_Received;

	private string m_LicenceValidityTimestamp_Received;

	private string m_GracePeriodTimestamp_Received;

	private string m_UpdateTimestamp_Received;

	private string m_FileURL1_Received = string.Empty;

	private string m_FileURL2_Received = string.Empty;

	private string m_FileName1_Received;

	private string m_FileName2_Received;

	private int m_FileSize1_Received;

	private int m_FileSize2_Received;

	public CheckLVLButton()
	{
	}

	private long ConvertEpochSecondsToTicks(long secs)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		long num = (long)10000;
		DateTime maxValue = DateTime.MaxValue;
		long ticks = (maxValue.Ticks - dateTime.Ticks) / num;
		if (secs < (long)0)
		{
			secs = (long)0;
		}
		if (secs > ticks)
		{
			secs = ticks;
		}
		return dateTime.Ticks + secs * num;
	}

	internal static Dictionary<string, string> DecodeExtras(string query)
	{
		string str;
		Dictionary<string, string> strs = new Dictionary<string, string>();
		if (query.Length == 0)
		{
			return strs;
		}
		string str1 = query;
		int length = str1.Length;
		int num = 0;
		bool flag = true;
		while (num <= length)
		{
			int num1 = -1;
			int length1 = -1;
			for (int i = num; i < length; i++)
			{
				if (num1 == -1 && str1[i] == '=')
				{
					num1 = i + 1;
				}
				else if (str1[i] == '&')
				{
					length1 = i;
					break;
				}
			}
			if (flag)
			{
				flag = false;
				if (str1[num] == '?')
				{
					num++;
				}
			}
			if (num1 != -1)
			{
				str = WWW.UnEscapeURL(str1.Substring(num, num1 - num - 1));
			}
			else
			{
				str = null;
				num1 = num;
			}
			if (length1 >= 0)
			{
				num = length1 + 1;
			}
			else
			{
				num = -1;
				length1 = str1.Length;
			}
			string str2 = WWW.UnEscapeURL(str1.Substring(num1, length1 - num1));
			strs.Add(str, str2);
			if (num != -1)
			{
				continue;
			}
			break;
		}
		return strs;
	}

	private void LoadServiceBinder()
	{
		byte[] serviceBinder = this.ServiceBinder.bytes;
		this.m_Activity = (new AndroidJavaClass("com.unity3d.player.UnityPlayer")).GetStatic<AndroidJavaObject>("currentActivity");
		this.m_PackageName = this.m_Activity.Call<string>("getPackageName", new object[0]);
		string str = Path.Combine(this.m_Activity.Call<AndroidJavaObject>("getCacheDir", new object[0]).Call<string>("getPath", new object[0]), this.m_PackageName);
		Directory.CreateDirectory(str);
		File.WriteAllBytes(string.Concat(str, "/classes.jar"), serviceBinder);
		Directory.CreateDirectory(string.Concat(str, "/odex"));
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("dalvik.system.DexClassLoader", new object[] { string.Concat(str, "/classes.jar"), string.Concat(str, "/odex"), null, this.m_Activity.Call<AndroidJavaObject>("getClassLoader", new object[0]) });
		this.m_LVLCheckType = androidJavaObject.Call<AndroidJavaObject>("findClass", new object[] { "com.unity3d.plugin.lvl.ServiceBinder" });
		Directory.Delete(str, true);
	}

	private void OnGUI()
	{
		if (!this.m_RunningOnAndroid)
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Use LVL checks only on the Android device!");
			return;
		}
		GUI.enabled = this.m_ButtonEnabled;
		if (GUI.Button(new Rect(10f, 10f, 450f, 300f), this.m_ButtonMessage))
		{
			this.m_Nonce = (new System.Random()).Next();
			object[] mActivity = new object[] { new AndroidJavaObject[] { this.m_Activity } };
			AndroidJavaObject[] androidJavaObjectArray = this.m_LVLCheckType.Call<AndroidJavaObject[]>("getConstructors", new object[0]);
			this.m_LVLCheck = androidJavaObjectArray[0].Call<AndroidJavaObject>("newInstance", mActivity);
			this.m_LVLCheck.Call("create", new object[] { this.m_Nonce, new AndroidJavaRunnable(this.Process) });
			this.m_ButtonMessage = "Checking...";
			this.m_ButtonEnabled = false;
		}
		GUI.enabled = true;
		if (this.m_LVLCheck != null || this.m_LVL_Received)
		{
			GUI.Label(new Rect(10f, 320f, 450f, 20f), "Requesting LVL response:");
			GUI.Label(new Rect(20f, 340f, 450f, 20f), string.Concat("Package name  = ", this.m_PackageName));
			GUI.Label(new Rect(20f, 360f, 450f, 20f), string.Concat("Request nonce = 0x", this.m_Nonce.ToString("X")));
		}
		if (this.m_LVLCheck == null && this.m_LVL_Received)
		{
			GUI.Label(new Rect(10f, 420f, 450f, 20f), "Received LVL response:");
			GUI.Label(new Rect(20f, 440f, 450f, 20f), string.Concat("Response code  = ", this.m_ResponseCode_Received));
			GUI.Label(new Rect(20f, 460f, 450f, 20f), string.Concat("Package name   = ", this.m_PackageName_Received));
			GUI.Label(new Rect(20f, 480f, 450f, 20f), string.Concat("Received nonce = 0x", this.m_Nonce_Received.ToString("X")));
			GUI.Label(new Rect(20f, 500f, 450f, 20f), string.Concat("Version code = ", this.m_VersionCode_Received));
			GUI.Label(new Rect(20f, 520f, 450f, 20f), string.Concat("User ID   = ", this.m_UserID_Received));
			GUI.Label(new Rect(20f, 540f, 450f, 20f), string.Concat("Timestamp = ", this.m_Timestamp_Received));
			GUI.Label(new Rect(20f, 560f, 450f, 20f), string.Concat("Max Retry = ", this.m_MaxRetry_Received));
			GUI.Label(new Rect(20f, 580f, 450f, 20f), string.Concat("License Validity = ", this.m_LicenceValidityTimestamp_Received));
			GUI.Label(new Rect(20f, 600f, 450f, 20f), string.Concat("Grace Period = ", this.m_GracePeriodTimestamp_Received));
			GUI.Label(new Rect(20f, 620f, 450f, 20f), string.Concat("Update Since = ", this.m_UpdateTimestamp_Received));
			GUI.Label(new Rect(20f, 640f, 450f, 20f), string.Concat("Main OBB URL = ", this.m_FileURL1_Received.Substring(0, Mathf.Min(this.m_FileURL1_Received.Length, 50)), "..."));
			GUI.Label(new Rect(20f, 660f, 450f, 20f), string.Concat("Main OBB Name = ", this.m_FileName1_Received));
			GUI.Label(new Rect(20f, 680f, 450f, 20f), string.Concat("Main OBB Size = ", this.m_FileSize1_Received));
			GUI.Label(new Rect(20f, 700f, 450f, 20f), string.Concat("Patch OBB URL = ", this.m_FileURL2_Received.Substring(0, Mathf.Min(this.m_FileURL2_Received.Length, 50)), "..."));
			GUI.Label(new Rect(20f, 720f, 450f, 20f), string.Concat("Patch OBB Name = ", this.m_FileName2_Received));
			GUI.Label(new Rect(20f, 740f, 450f, 20f), string.Concat("Patch OBB Size = ", this.m_FileSize2_Received));
		}
	}

	private void Process()
	{
		string str;
		string empty;
		this.m_LVL_Received = true;
		this.m_ButtonMessage = "Check LVL";
		this.m_ButtonEnabled = true;
		if (this.m_LVLCheck == null)
		{
			return;
		}
		int num = this.m_LVLCheck.Get<int>("_arg0");
		string str1 = this.m_LVLCheck.Get<string>("_arg1");
		string str2 = this.m_LVLCheck.Get<string>("_arg2");
		this.m_LVLCheck = null;
		this.m_ResponseCode_Received = num.ToString();
		if (num < 0 || string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
		{
			this.m_PackageName_Received = "<Failed>";
			return;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(str1);
		byte[] numArray = Convert.FromBase64String(str2);
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.ImportParameters(this.m_PublicKey);
		if (!rSACryptoServiceProvider.VerifyHash((new SHA1Managed()).ComputeHash(bytes), CryptoConfig.MapNameToOID("SHA1"), numArray))
		{
			this.m_ResponseCode_Received = "<Failed>";
			this.m_PackageName_Received = "<Invalid Signature>";
			return;
		}
		int num1 = str1.IndexOf(':');
		if (num1 != -1)
		{
			str = str1.Substring(0, num1);
			empty = (num1 < str1.Length ? str1.Substring(num1 + 1) : string.Empty);
		}
		else
		{
			str = str1;
			empty = string.Empty;
		}
		string[] strArrays = str.Split(new char[] { '|' });
		if (strArrays[0].CompareTo(num.ToString()) != 0)
		{
			this.m_ResponseCode_Received = "<Failed>";
			this.m_PackageName_Received = "<Response Mismatch>";
			return;
		}
		this.m_ResponseCode_Received = strArrays[0];
		this.m_Nonce_Received = Convert.ToInt32(strArrays[1]);
		this.m_PackageName_Received = strArrays[2];
		this.m_VersionCode_Received = Convert.ToInt32(strArrays[3]);
		this.m_UserID_Received = strArrays[4];
		long ticks = this.ConvertEpochSecondsToTicks(Convert.ToInt64(strArrays[5]));
		DateTime dateTime = new DateTime(ticks);
		this.m_Timestamp_Received = dateTime.ToLocalTime().ToString();
		if (!string.IsNullOrEmpty(empty))
		{
			Dictionary<string, string> strs = CheckLVLButton.DecodeExtras(empty);
			if (!strs.ContainsKey("GR"))
			{
				this.m_MaxRetry_Received = 0;
			}
			else
			{
				this.m_MaxRetry_Received = Convert.ToInt32(strs["GR"]);
			}
			if (!strs.ContainsKey("VT"))
			{
				this.m_LicenceValidityTimestamp_Received = null;
			}
			else
			{
				ticks = this.ConvertEpochSecondsToTicks(Convert.ToInt64(strs["VT"]));
				DateTime dateTime1 = new DateTime(ticks);
				this.m_LicenceValidityTimestamp_Received = dateTime1.ToLocalTime().ToString();
			}
			if (!strs.ContainsKey("GT"))
			{
				this.m_GracePeriodTimestamp_Received = null;
			}
			else
			{
				ticks = this.ConvertEpochSecondsToTicks(Convert.ToInt64(strs["GT"]));
				DateTime dateTime2 = new DateTime(ticks);
				this.m_GracePeriodTimestamp_Received = dateTime2.ToLocalTime().ToString();
			}
			if (!strs.ContainsKey("UT"))
			{
				this.m_UpdateTimestamp_Received = null;
			}
			else
			{
				ticks = this.ConvertEpochSecondsToTicks(Convert.ToInt64(strs["UT"]));
				DateTime dateTime3 = new DateTime(ticks);
				this.m_UpdateTimestamp_Received = dateTime3.ToLocalTime().ToString();
			}
			if (!strs.ContainsKey("FILE_URL1"))
			{
				this.m_FileURL1_Received = string.Empty;
			}
			else
			{
				this.m_FileURL1_Received = strs["FILE_URL1"];
			}
			if (!strs.ContainsKey("FILE_URL2"))
			{
				this.m_FileURL2_Received = string.Empty;
			}
			else
			{
				this.m_FileURL2_Received = strs["FILE_URL2"];
			}
			if (!strs.ContainsKey("FILE_NAME1"))
			{
				this.m_FileName1_Received = null;
			}
			else
			{
				this.m_FileName1_Received = strs["FILE_NAME1"];
			}
			if (!strs.ContainsKey("FILE_NAME2"))
			{
				this.m_FileName2_Received = null;
			}
			else
			{
				this.m_FileName2_Received = strs["FILE_NAME2"];
			}
			if (!strs.ContainsKey("FILE_SIZE1"))
			{
				this.m_FileSize1_Received = 0;
			}
			else
			{
				this.m_FileSize1_Received = Convert.ToInt32(strs["FILE_SIZE1"]);
			}
			if (!strs.ContainsKey("FILE_SIZE2"))
			{
				this.m_FileSize2_Received = 0;
			}
			else
			{
				this.m_FileSize2_Received = Convert.ToInt32(strs["FILE_SIZE2"]);
			}
		}
	}

	private void Start()
	{
		Debug.Log(string.Concat("private string m_PublicKey_Modulus_Base64 = \"", this.m_PublicKey_Modulus_Base64, "\";"));
		Debug.Log(string.Concat("private string m_PublicKey_Exponent_Base64 = \"", this.m_PublicKey_Exponent_Base64, "\";"));
		this.m_PublicKey.Modulus = Convert.FromBase64String(this.m_PublicKey_Modulus_Base64);
		this.m_PublicKey.Exponent = Convert.FromBase64String(this.m_PublicKey_Exponent_Base64);
		this.m_RunningOnAndroid = (new AndroidJavaClass("android.os.Build")).GetRawClass() != IntPtr.Zero;
		if (!this.m_RunningOnAndroid)
		{
			return;
		}
		this.LoadServiceBinder();
		SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
		this.m_ButtonMessage = "Check LVL";
	}
}