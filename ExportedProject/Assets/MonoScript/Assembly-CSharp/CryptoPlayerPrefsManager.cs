using System;
using UnityEngine;

public class CryptoPlayerPrefsManager : MonoBehaviour
{
	public int salt = 2147483647;

	public bool useRijndael = true;

	public bool useXor = true;

	public CryptoPlayerPrefsManager()
	{
	}

	private void Awake()
	{
		CryptoPlayerPrefs.setSalt(this.salt);
		CryptoPlayerPrefs.useRijndael(this.useRijndael);
		CryptoPlayerPrefs.useXor(this.useXor);
		UnityEngine.Object.Destroy(this);
	}
}