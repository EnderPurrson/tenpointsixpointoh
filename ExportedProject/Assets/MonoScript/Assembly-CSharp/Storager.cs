using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Storager
{
	private const bool useCryptoPlayerPrefs = true;

	private const bool _useSignedPreferences = true;

	private static bool iCloudAvailable;

	private static IDictionary<string, SaltedInt> _keychainCache;

	private static IDictionary<string, string> _keychainStringCache;

	private static Dictionary<string, int> iosCloudSyncBuffer;

	private static bool _weaponDigestIsDirty;

	private readonly static IDictionary<string, SaltedInt> _protectedIntCache;

	private readonly static System.Random _prng;

	private readonly static string[] _expendableKeys;

	public static bool ICloudAvailable
	{
		get
		{
			return Storager.iCloudAvailable;
		}
	}

	public static bool UseSignedPreferences
	{
		get
		{
			return true;
		}
	}

	static Storager()
	{
		Storager.iCloudAvailable = false;
		Storager._keychainCache = new Dictionary<string, SaltedInt>();
		Storager._keychainStringCache = new Dictionary<string, string>();
		Storager.iosCloudSyncBuffer = new Dictionary<string, int>();
		Storager._protectedIntCache = new Dictionary<string, SaltedInt>();
		Storager._prng = new System.Random();
		Storager._expendableKeys = new string[] { GearManager.InvisibilityPotion, GearManager.Jetpack, GearManager.Turret, GearManager.Mech };
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			IEnumerator<string> enumerator = PurchasesSynchronizer.AllItemIds().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					Storager.iosCloudSyncBuffer.Add(current, 0);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}
	}

	public static int getInt(string key, bool useICloud)
	{
		SaltedInt saltedInt;
		string str;
		int num;
		if (Application.isEditor)
		{
			return PlayerPrefs.GetInt(key);
		}
		if (Storager._protectedIntCache.TryGetValue(key, out saltedInt))
		{
			return saltedInt.Value;
		}
		if (CryptoPlayerPrefs.HasKey(key))
		{
			int num1 = CryptoPlayerPrefs.GetInt(key, 0);
			Storager._protectedIntCache.Add(key, new SaltedInt(Storager._prng.Next(), num1));
			return num1;
		}
		if (!key.Equals("Coins") && !key.Equals("GemsCurrency") || !Defs2.SignedPreferences.TryGetValue(key, out str) || !Defs2.SignedPreferences.Verify(key) || !int.TryParse(str, out num))
		{
			return 0;
		}
		return num;
	}

	public static string getString(string key, bool useICloud)
	{
		string str;
		if (Application.isEditor)
		{
			return PlayerPrefs.GetString(key);
		}
		if (Storager._keychainStringCache.TryGetValue(key, out str))
		{
			return str;
		}
		if (!CryptoPlayerPrefs.HasKey(key))
		{
			return string.Empty;
		}
		string str1 = CryptoPlayerPrefs.GetString(key, string.Empty);
		Storager._keychainStringCache.Add(key, str1);
		return str1;
	}

	public static bool hasKey(string key)
	{
		string str;
		int num;
		bool flag = CryptoPlayerPrefs.HasKey(key);
		if (flag || !key.Equals("Coins") && !key.Equals("GemsCurrency") || !Defs2.SignedPreferences.TryGetValue(key, out str) || !Defs2.SignedPreferences.Verify(key) || !int.TryParse(str, out num))
		{
			return flag;
		}
		Storager.setInt(key, Math.Max(0, num), false);
		return true;
	}

	public static void Initialize(bool cloudAvailable)
	{
	}

	public static bool IsInitialized(string flagName)
	{
		if (Application.isEditor)
		{
			return PlayerPrefs.HasKey(flagName);
		}
		return Storager.hasKey(flagName);
	}

	private static void RefreshExpendablesDigest()
	{
		byte[] array = Storager._expendableKeys.SelectMany<string, byte>((string key) => BitConverter.GetBytes(Storager.getInt(key, false))).ToArray<byte>();
		DigestStorager.Instance.Set("ExpendablesCount", array);
	}

	public static void RefreshWeaponDigestIfDirty()
	{
		if (!Storager._weaponDigestIsDirty)
		{
			return;
		}
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("[Rilisoft] > RefreshWeaponsDigest: {0:F3}", new object[] { Time.realtimeSinceStartup });
		}
		Storager.RefreshWeaponsDigest();
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("[Rilisoft] < RefreshWeaponsDigest: {0:F3}", new object[] { Time.realtimeSinceStartup });
		}
	}

	private static void RefreshWeaponsDigest()
	{
		IEnumerable<string> values = 
			from w in WeaponManager.storeIDtoDefsSNMapping.Values
			where Storager.getInt(w, false) == 1
			select w;
		int num = values.Count<string>();
		DigestStorager.Instance.Set("WeaponsCount", num);
		Storager._weaponDigestIsDirty = false;
	}

	public static void SetInitialized(string flagName)
	{
		Storager.setInt(flagName, 0, false);
	}

	public static void setInt(string key, int val, bool useICloud)
	{
		if (!Application.isEditor)
		{
			CryptoPlayerPrefs.SetInt(key, val);
			Storager._protectedIntCache[key] = new SaltedInt(Storager._prng.Next(), val);
			if (key.Equals("Coins") || key.Equals("GemsCurrency"))
			{
				Defs2.SignedPreferences.Add(key, val.ToString());
			}
		}
		else
		{
			PlayerPrefs.SetInt(key, val);
		}
		if (key.Equals("Coins") || key.Equals("GemsCurrency"))
		{
			DigestStorager.Instance.Set(key, val);
		}
		if (Storager._expendableKeys.Contains<string>(key))
		{
			Storager.RefreshExpendablesDigest();
		}
		if (WeaponManager.PurchasableWeaponSetContains(key))
		{
			Storager._weaponDigestIsDirty = true;
		}
	}

	public static void setString(string key, string val, bool useICloud)
	{
		if (Application.isEditor)
		{
			PlayerPrefs.SetString(key, val);
			return;
		}
		CryptoPlayerPrefs.SetString(key, val);
		Storager._keychainStringCache[key] = val;
	}

	public static void SynchronizeIosWithCloud(ref List<string> weaponsForWhichSetRememberedTier, out bool armorArmy1Comes)
	{
		armorArmy1Comes = false;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			!Storager.iCloudAvailable;
		}
	}

	public static void SyncWithCloud(string storageId)
	{
		int num = Storager.getInt(storageId, true);
		if (num > 0)
		{
			Storager.setInt(storageId, num, true);
		}
	}
}