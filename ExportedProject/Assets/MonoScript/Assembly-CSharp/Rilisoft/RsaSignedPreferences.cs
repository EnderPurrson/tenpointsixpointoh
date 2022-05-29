using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rilisoft
{
	internal sealed class RsaSignedPreferences : SignedPreferences
	{
		private const string SaltKeyValueFormat = "{0}__{1}__{2}";

		private const string Prefix = "com.Rilisoft";

		private readonly HashAlgorithm _hashAlgorithm = new SHA1CryptoServiceProvider();

		private readonly RSACryptoServiceProvider _signingRsaCsp;

		private readonly RSACryptoServiceProvider _verificationRsaCsp;

		private readonly string _salt;

		private readonly byte[] _prefixBytes = Encoding.UTF8.GetBytes("com.Rilisoft");

		public RsaSignedPreferences(Preferences backPreferences, RSACryptoServiceProvider rsaCsp, string salt) : base(backPreferences)
		{
			this._signingRsaCsp = rsaCsp;
			this._verificationRsaCsp = new RSACryptoServiceProvider();
			this._verificationRsaCsp.ImportParameters(rsaCsp.ExportParameters(false));
			this._salt = salt;
		}

		protected override void AddSignedCore(string key, string value)
		{
			if (key.StartsWith("com.Rilisoft"))
			{
				throw new ArgumentException("Key starts with reserved prefix.", "key");
			}
			base.BackPreferences.Add(key, value);
			base.BackPreferences.Add(this.GetSignatureKey(key), this.Sign(key, value));
		}

		private string GetSignatureKey(string key)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(this._salt);
			byte[] numArray = Encoding.UTF8.GetBytes(key);
			byte[] array = bytes.Concat<byte>(this._prefixBytes).Concat<byte>(numArray).ToArray<byte>();
			byte[] numArray1 = this._hashAlgorithm.ComputeHash(array);
			return string.Format("{0}_{1}", "com.Rilisoft", Convert.ToBase64String(numArray1));
		}

		protected override bool RemoveSignedCore(string key)
		{
			if (key.StartsWith("com.Rilisoft"))
			{
				throw new ArgumentException("Key starts with reserved prefix.", "key");
			}
			base.BackPreferences.Remove(this.GetSignatureKey(key));
			return base.BackPreferences.Remove(key);
		}

		private string Sign(string key, string value)
		{
			string str = string.Format("{0}__{1}__{2}", this._salt, key, value);
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			byte[] numArray = this._signingRsaCsp.SignData(bytes, this._hashAlgorithm);
			return Convert.ToBase64String(numArray);
		}

		protected override bool VerifyCore(string key)
		{
			string str;
			string str1;
			bool flag;
			if (!this.TryGetValueCore(key, out str))
			{
				throw new KeyNotFoundException(string.Format("The given key was not present in the dictionary: {0}", key));
			}
			string str2 = string.Format("{0}__{1}__{2}", this._salt, key, str);
			if (!this.TryGetValueCore(this.GetSignatureKey(key), out str1))
			{
				return false;
			}
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(str2);
				byte[] numArray = Convert.FromBase64String(str1);
				flag = this._verificationRsaCsp.VerifyData(bytes, this._hashAlgorithm, numArray);
			}
			catch (FormatException formatException)
			{
				flag = false;
			}
			return flag;
		}
	}
}