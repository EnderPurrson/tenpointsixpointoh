using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class DigestStorager
	{
		private readonly bool _useCryptoPlayerPrefs;

		private readonly HashAlgorithm _hmac;

		private readonly static Lazy<DigestStorager> _instance;

		public static DigestStorager Instance
		{
			get
			{
				return DigestStorager._instance.Value;
			}
		}

		static DigestStorager()
		{
			DigestStorager._instance = new Lazy<DigestStorager>(() => new DigestStorager());
		}

		public DigestStorager()
		{
			this._hmac = new HMACSHA1(new byte[] { 62, 59, 146, 50, 196, 43, 151, 12, 34, 157, 74, 34, 25, 226, 239, 167, 46, 226, 151, 253, 149, 85, 40, 56, 107, 254, 198, 111, 152, 34, 73, 206, 184, 145, 51, 23, 161, 197, 53, 9, 59, 16, 106, 151, 54, 115, 158, 48, 176, 147, 174, 119, 233, 88, 253, 94, 20, 2, 164, 67, 205, 142, 150, 2 }, true);
		}

		public void Clear()
		{
		}

		private byte[] ComputeHash(string key, byte[] valueBytes)
		{
			byte[] bytes = BitConverter.GetBytes(key.GetHashCode());
			byte[] numArray = new byte[(int)bytes.Length + (int)valueBytes.Length];
			bytes.CopyTo(numArray, 0);
			valueBytes.CopyTo(numArray, (int)bytes.Length);
			return this._hmac.ComputeHash(numArray);
		}

		public bool ContainsKey(string key)
		{
			string backingStoreKey = this.GetBackingStoreKey(key);
			return (!this._useCryptoPlayerPrefs ? PlayerPrefs.HasKey(backingStoreKey) : CryptoPlayerPrefs.HasKey(backingStoreKey));
		}

		public string GetBackingStoreKey(string key)
		{
			string str = string.Format("Digest-9.4.1-{0:d}.", BuildSettings.BuildTargetPlatform);
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (Application.isEditor)
			{
				return string.Concat(str, key);
			}
			byte[] bytes = Encoding.UTF8.GetBytes(key);
			return string.Concat(str, Convert.ToBase64String(bytes));
		}

		public void Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			string backingStoreKey = this.GetBackingStoreKey(key);
			if (!this._useCryptoPlayerPrefs)
			{
				PlayerPrefs.DeleteKey(backingStoreKey);
			}
			else
			{
				CryptoPlayerPrefs.DeleteKey(backingStoreKey);
			}
		}

		public void Save()
		{
			if (!this._useCryptoPlayerPrefs)
			{
				PlayerPrefs.Save();
			}
			else
			{
				CryptoPlayerPrefs.Save();
			}
		}

		public void Set(string key, int value)
		{
			this.SetCore(key, BitConverter.GetBytes(value));
		}

		public void Set(string key, string value)
		{
			this.SetCore(key, Encoding.UTF8.GetBytes(value ?? string.Empty));
		}

		public void Set(string key, byte[] value)
		{
			this.SetCore(key, value ?? new byte[0]);
		}

		private void SetCore(string key, byte[] valueBytes)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			string base64String = Convert.ToBase64String(this.ComputeHash(key, valueBytes));
			string backingStoreKey = this.GetBackingStoreKey(key);
			if (!this._useCryptoPlayerPrefs)
			{
				PlayerPrefs.SetString(backingStoreKey, base64String);
			}
			else
			{
				CryptoPlayerPrefs.SetString(backingStoreKey, base64String);
			}
		}

		public bool Verify(string key, int value)
		{
			return this.VerifyCore(key, BitConverter.GetBytes(value));
		}

		public bool Verify(string key, string value)
		{
			return this.VerifyCore(key, Encoding.UTF8.GetBytes(value ?? string.Empty));
		}

		public bool Verify(string key, byte[] value)
		{
			return this.VerifyCore(key, value ?? new byte[0]);
		}

		public bool VerifyCore(string key, byte[] valueBytes)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!this.ContainsKey(key))
			{
				return false;
			}
			byte[] numArray = this.ComputeHash(key, valueBytes);
			string backingStoreKey = this.GetBackingStoreKey(key);
			return Convert.FromBase64String((!this._useCryptoPlayerPrefs ? PlayerPrefs.GetString(backingStoreKey) : CryptoPlayerPrefs.GetString(backingStoreKey, string.Empty))).SequenceEqual<byte>(numArray);
		}
	}
}