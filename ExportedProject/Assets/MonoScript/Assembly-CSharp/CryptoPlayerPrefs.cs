using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class CryptoPlayerPrefs
{
	private static Dictionary<string, string> keyHashs;

	private static Dictionary<string, int> xorOperands;

	private static System.Security.Cryptography.HashAlgorithm _hashAlgorithm;

	private static Dictionary<string, SymmetricAlgorithm> rijndaelDict;

	private static int salt;

	private static string _saltString;

	private static bool _useRijndael;

	private static bool _useXor;

	private static System.Security.Cryptography.HashAlgorithm HashAlgorithm
	{
		get
		{
			if (CryptoPlayerPrefs._hashAlgorithm == null)
			{
				CryptoPlayerPrefs._hashAlgorithm = new MD5CryptoServiceProvider();
			}
			return CryptoPlayerPrefs._hashAlgorithm;
		}
	}

	private static string SaltString
	{
		get
		{
			if (string.IsNullOrEmpty(CryptoPlayerPrefs._saltString))
			{
				CryptoPlayerPrefs._saltString = CryptoPlayerPrefs.salt.ToString();
			}
			return CryptoPlayerPrefs._saltString;
		}
	}

	static CryptoPlayerPrefs()
	{
		CryptoPlayerPrefs.salt = 2147483647;
		CryptoPlayerPrefs._useRijndael = true;
		CryptoPlayerPrefs._useXor = true;
	}

	private static int computePlusOperand(int xor)
	{
		return xor & xor << 1;
	}

	private static int computeXorOperand(string key, string cryptedKey)
	{
		if (CryptoPlayerPrefs.xorOperands == null)
		{
			CryptoPlayerPrefs.xorOperands = new Dictionary<string, int>();
		}
		if (CryptoPlayerPrefs.xorOperands.ContainsKey(key))
		{
			return CryptoPlayerPrefs.xorOperands[key];
		}
		int num = 0;
		string str = cryptedKey;
		for (int i = 0; i < str.Length; i++)
		{
			num += str[i];
		}
		num += CryptoPlayerPrefs.salt;
		CryptoPlayerPrefs.xorOperands.Add(key, num);
		return num;
	}

	private static string decrypt(string cKey)
	{
		return CryptoPlayerPrefs.DecryptString(PlayerPrefs.GetString(cKey), CryptoPlayerPrefs.getEncryptionPassword(cKey));
	}

	private static byte[] DecryptString(byte[] cipherData, SymmetricAlgorithm alg)
	{
		byte[] array;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, alg.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(cipherData, 0, (int)cipherData.Length);
				cryptoStream.Close();
				array = memoryStream.ToArray();
			}
		}
		return array;
	}

	private static string DecryptString(string cipherText, string Password)
	{
		if (CryptoPlayerPrefs.rijndaelDict == null)
		{
			CryptoPlayerPrefs.rijndaelDict = new Dictionary<string, SymmetricAlgorithm>();
		}
		byte[] numArray = Convert.FromBase64String(cipherText);
		byte[] numArray1 = CryptoPlayerPrefs.DecryptString(numArray, CryptoPlayerPrefs.getRijndaelForKey(Password));
		return Encoding.Unicode.GetString(numArray1, 0, (int)numArray1.Length);
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public static void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(CryptoPlayerPrefs.hashedKey(key));
	}

	private static string encrypt(string cKey, string data)
	{
		return CryptoPlayerPrefs.EncryptString(data, CryptoPlayerPrefs.getEncryptionPassword(cKey));
	}

	private static byte[] EncryptString(byte[] clearText, SymmetricAlgorithm alg)
	{
		byte[] array;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, alg.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(clearText, 0, (int)clearText.Length);
				cryptoStream.Close();
				array = memoryStream.ToArray();
			}
		}
		return array;
	}

	private static string EncryptString(string clearText, string Password)
	{
		SymmetricAlgorithm rijndaelForKey = CryptoPlayerPrefs.getRijndaelForKey(Password);
		byte[] bytes = Encoding.Unicode.GetBytes(clearText);
		return Convert.ToBase64String(CryptoPlayerPrefs.EncryptString(bytes, rijndaelForKey));
	}

	private static string getEncryptionPassword(string pw)
	{
		return CryptoPlayerPrefs.Md5Sum(string.Concat(pw, CryptoPlayerPrefs.SaltString));
	}

	public static float GetFloat(string key, float defaultValue = 0)
	{
		return float.Parse(CryptoPlayerPrefs.GetString(key, defaultValue.ToString()));
	}

	public static int GetInt(string key, int defaultValue = 0)
	{
		int num;
		string str = CryptoPlayerPrefs.hashedKey(key);
		if (!PlayerPrefs.HasKey(str))
		{
			return defaultValue;
		}
		num = (!CryptoPlayerPrefs._useRijndael ? PlayerPrefs.GetInt(str) : int.Parse(CryptoPlayerPrefs.decrypt(str)));
		int num1 = num;
		if (CryptoPlayerPrefs._useXor)
		{
			int num2 = CryptoPlayerPrefs.computeXorOperand(key, str);
			num1 = (num2 ^ num) - CryptoPlayerPrefs.computePlusOperand(num2);
		}
		return num1;
	}

	public static long GetLong(string key, long defaultValue = 0)
	{
		return long.Parse(CryptoPlayerPrefs.GetString(key, defaultValue.ToString()));
	}

	private static SymmetricAlgorithm getRijndaelForKey(string key)
	{
		SymmetricAlgorithm bytes;
		if (CryptoPlayerPrefs.rijndaelDict == null)
		{
			CryptoPlayerPrefs.rijndaelDict = new Dictionary<string, SymmetricAlgorithm>();
		}
		if (!CryptoPlayerPrefs.rijndaelDict.TryGetValue(key, out bytes))
		{
			Rfc2898DeriveBytes rfc2898DeriveByte = new Rfc2898DeriveBytes(key, new byte[] { typeof(u003cPrivateImplementationDetailsu003e).GetField("$$field-63").FieldHandle });
			bytes = Rijndael.Create();
			bytes.Key = rfc2898DeriveByte.GetBytes(32);
			bytes.IV = rfc2898DeriveByte.GetBytes(16);
			CryptoPlayerPrefs.rijndaelDict.Add(key, bytes);
		}
		return bytes;
	}

	public static string GetString(string key, string defaultValue = "")
	{
		string str;
		string str1 = CryptoPlayerPrefs.hashedKey(key);
		if (!PlayerPrefs.HasKey(str1))
		{
			return defaultValue;
		}
		str = (!CryptoPlayerPrefs._useRijndael ? PlayerPrefs.GetString(str1) : CryptoPlayerPrefs.decrypt(str1));
		string empty = str;
		if (CryptoPlayerPrefs._useXor)
		{
			int num = CryptoPlayerPrefs.computeXorOperand(key, str1);
			int num1 = CryptoPlayerPrefs.computePlusOperand(num);
			empty = string.Empty;
			string str2 = str;
			for (int i = 0; i < str2.Length; i++)
			{
				char chr = str2[i];
				char chr1 = (char)((num ^ chr) - num1);
				empty = string.Concat(empty, chr1);
			}
		}
		return empty;
	}

	private static string hashedKey(string key)
	{
		string str;
		if (CryptoPlayerPrefs.keyHashs == null)
		{
			CryptoPlayerPrefs.keyHashs = new Dictionary<string, string>();
		}
		if (CryptoPlayerPrefs.keyHashs.TryGetValue(key, out str))
		{
			return str;
		}
		string str1 = CryptoPlayerPrefs.Md5Sum(key);
		CryptoPlayerPrefs.keyHashs.Add(key, str1);
		return str1;
	}

	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(CryptoPlayerPrefs.hashedKey(key));
	}

	public static string Md5Sum(string strToEncrypt)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(strToEncrypt);
		byte[] numArray = CryptoPlayerPrefs.HashAlgorithm.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder(32);
		for (int i = 0; i < (int)numArray.Length; i++)
		{
			stringBuilder.Append(Convert.ToString(numArray[i], 16).PadLeft(2, '0'));
		}
		string str = stringBuilder.ToString();
		if (str.Length >= 32)
		{
			return str;
		}
		return str.PadLeft(32, '0');
	}

	public static void Save()
	{
		PlayerPrefs.Save();
	}

	public static void SetFloat(string key, float val)
	{
		CryptoPlayerPrefs.SetString(key, val.ToString());
	}

	public static void SetInt(string key, int val)
	{
		string str = CryptoPlayerPrefs.hashedKey(key);
		int num = val;
		if (CryptoPlayerPrefs._useXor)
		{
			int num1 = CryptoPlayerPrefs.computeXorOperand(key, str);
			num = val + CryptoPlayerPrefs.computePlusOperand(num1) ^ num1;
		}
		if (!CryptoPlayerPrefs._useRijndael)
		{
			PlayerPrefs.SetInt(str, num);
		}
		else
		{
			PlayerPrefs.SetString(str, CryptoPlayerPrefs.encrypt(str, num.ToString()));
		}
	}

	public static void SetLong(string key, long val)
	{
		CryptoPlayerPrefs.SetString(key, val.ToString());
	}

	public static void setSalt(int s)
	{
		CryptoPlayerPrefs.salt = s;
		CryptoPlayerPrefs._saltString = s.ToString();
	}

	public static void SetString(string key, string val)
	{
		string str = CryptoPlayerPrefs.hashedKey(key);
		string empty = val;
		if (CryptoPlayerPrefs._useXor)
		{
			int num = CryptoPlayerPrefs.computeXorOperand(key, str);
			int num1 = CryptoPlayerPrefs.computePlusOperand(num);
			empty = string.Empty;
			string str1 = val;
			for (int i = 0; i < str1.Length; i++)
			{
				char chr = str1[i];
				char chr1 = (char)(chr + (char)num1 ^ num);
				empty = string.Concat(empty, chr1);
			}
		}
		if (!CryptoPlayerPrefs._useRijndael)
		{
			PlayerPrefs.SetString(str, empty);
		}
		else
		{
			PlayerPrefs.SetString(str, CryptoPlayerPrefs.encrypt(str, empty));
		}
	}

	private static bool test(bool use_Rijndael, bool use_Xor)
	{
		bool flag = true;
		bool flag1 = CryptoPlayerPrefs._useRijndael;
		bool flag2 = CryptoPlayerPrefs._useXor;
		CryptoPlayerPrefs.useRijndael(use_Rijndael);
		CryptoPlayerPrefs.useXor(use_Xor);
		int num = 0;
		string str = "cryptotest_int";
		string str1 = CryptoPlayerPrefs.hashedKey(str);
		CryptoPlayerPrefs.SetInt(str, num);
		int num1 = CryptoPlayerPrefs.GetInt(str, 0);
		bool flag3 = num == num1;
		flag &= flag3;
		Debug.Log(string.Concat("INT Bordertest Zero: ", (!flag3 ? "fail" : "ok")));
		Debug.Log(string.Concat(new object[] { "(Key: ", str, "; Crypted Key: ", str1, "; Input value: ", num, "; Saved value: ", PlayerPrefs.GetString(str1), "; Return value: ", num1, ")" }));
		num = 2147483647;
		str = "cryptotest_intmax";
		str1 = CryptoPlayerPrefs.hashedKey(str);
		CryptoPlayerPrefs.SetInt(str, num);
		num1 = CryptoPlayerPrefs.GetInt(str, 0);
		flag3 = num == num1;
		flag &= flag3;
		Debug.Log(string.Concat("INT Bordertest Max: ", (!flag3 ? "fail" : "ok")));
		Debug.Log(string.Concat(new object[] { "(Key: ", str, "; Crypted Key: ", str1, "; Input value: ", num, "; Saved value: ", PlayerPrefs.GetString(str1), "; Return value: ", num1, ")" }));
		num = -2147483648;
		str = "cryptotest_intmin";
		str1 = CryptoPlayerPrefs.hashedKey(str);
		CryptoPlayerPrefs.SetInt(str, num);
		num1 = CryptoPlayerPrefs.GetInt(str, 0);
		flag3 = num == num1;
		flag &= flag3;
		Debug.Log(string.Concat("INT Bordertest Min: ", (!flag3 ? "fail" : "ok")));
		Debug.Log(string.Concat(new object[] { "(Key: ", str, "; Crypted Key: ", str1, "; Input value: ", num, "; Saved value: ", PlayerPrefs.GetString(str1), "; Return value: ", num1, ")" }));
		str = "cryptotest_intrand";
		str1 = CryptoPlayerPrefs.hashedKey(str);
		bool flag4 = true;
		for (int i = 0; i < 100; i++)
		{
			num = UnityEngine.Random.Range(-2147483648, 2147483647);
			CryptoPlayerPrefs.SetInt(str, num);
			num1 = CryptoPlayerPrefs.GetInt(str, 0);
			flag3 = num == num1;
			flag4 &= flag3;
			flag &= flag3;
		}
		Debug.Log(string.Concat("INT Test Random: ", (!flag4 ? "fail" : "ok")));
		float single = 0f;
		str = "cryptotest_float";
		str1 = CryptoPlayerPrefs.hashedKey(str);
		CryptoPlayerPrefs.SetFloat(str, single);
		float single1 = CryptoPlayerPrefs.GetFloat(str, 0f);
		flag3 = single.ToString().Equals(single1.ToString());
		flag &= flag3;
		Debug.Log(string.Concat("FLOAT Bordertest Zero: ", (!flag3 ? "fail" : "ok")));
		Debug.Log(string.Concat(new object[] { "(Key: ", str, "; Crypted Key: ", str1, "; Input value: ", single, "; Saved value: ", PlayerPrefs.GetString(str1), "; Return value: ", single1, ")" }));
		single = Single.MaxValue;
		str = "cryptotest_floatmax";
		str1 = CryptoPlayerPrefs.hashedKey(str);
		CryptoPlayerPrefs.SetFloat(str, single);
		single1 = CryptoPlayerPrefs.GetFloat(str, 0f);
		flag3 = single.ToString().Equals(single1.ToString());
		flag &= flag3;
		Debug.Log(string.Concat("FLOAT Bordertest Max: ", (!flag3 ? "fail" : "ok")));
		Debug.Log(string.Concat(new object[] { "(Key: ", str, "; Crypted Key: ", str1, "; Input value: ", single, "; Saved value: ", PlayerPrefs.GetString(str1), "; Return value: ", single1, ")" }));
		single = Single.MinValue;
		str = "cryptotest_floatmin";
		str1 = CryptoPlayerPrefs.hashedKey(str);
		CryptoPlayerPrefs.SetFloat(str, single);
		single1 = CryptoPlayerPrefs.GetFloat(str, 0f);
		flag3 = single.ToString().Equals(single1.ToString());
		flag &= flag3;
		Debug.Log(string.Concat("FLOAT Bordertest Min: ", (!flag3 ? "fail" : "ok")));
		Debug.Log(string.Concat(new object[] { "(Key: ", str, "; Crypted Key: ", str1, "; Input value: ", single, "; Saved value: ", PlayerPrefs.GetString(str1), "; Return value: ", single1, ")" }));
		str = "cryptotest_floatrand";
		str1 = CryptoPlayerPrefs.hashedKey(str);
		flag4 = true;
		for (int j = 0; j < 100; j++)
		{
			float single2 = (float)UnityEngine.Random.Range(-2147483648, 2147483647) * UnityEngine.Random.@value;
			single = single2;
			CryptoPlayerPrefs.SetFloat(str, single);
			single1 = CryptoPlayerPrefs.GetFloat(str, 0f);
			flag3 = single.ToString().Equals(single1.ToString());
			flag4 &= flag3;
			flag &= flag3;
		}
		Debug.Log(string.Concat("FLOAT Test Random: ", (!flag4 ? "fail" : "ok")));
		CryptoPlayerPrefs.DeleteKey("cryptotest_int");
		CryptoPlayerPrefs.DeleteKey("cryptotest_intmax");
		CryptoPlayerPrefs.DeleteKey("cryptotest_intmin");
		CryptoPlayerPrefs.DeleteKey("cryptotest_intrandom");
		CryptoPlayerPrefs.DeleteKey("cryptotest_float");
		CryptoPlayerPrefs.DeleteKey("cryptotest_floatmax");
		CryptoPlayerPrefs.DeleteKey("cryptotest_floatmin");
		CryptoPlayerPrefs.DeleteKey("cryptotest_floatrandom");
		CryptoPlayerPrefs.useRijndael(flag1);
		CryptoPlayerPrefs.useXor(flag2);
		return flag;
	}

	public static bool test()
	{
		bool flag = CryptoPlayerPrefs.test(true, true);
		bool flag1 = CryptoPlayerPrefs.test(true, false);
		bool flag2 = CryptoPlayerPrefs.test(false, true);
		bool flag3 = CryptoPlayerPrefs.test(false, false);
		return (!flag || !flag1 || !flag2 ? false : flag3);
	}

	public static void useRijndael(bool use)
	{
		CryptoPlayerPrefs._useRijndael = use;
	}

	public static void useXor(bool use)
	{
		CryptoPlayerPrefs._useXor = use;
	}
}