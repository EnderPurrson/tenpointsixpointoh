using System;
using System.Security.Cryptography;
using System.Text;

namespace Rilisoft
{
	public sealed class PlayerPrefsHelper : IDisposable
	{
		private bool _disposed;

		private readonly HMAC _hmac;

		private readonly string _hmacPrefsKey;

		internal PlayerPrefsHelper()
		{
			using (HashAlgorithm sHA256Managed = new SHA256Managed())
			{
				byte[] bytes = Encoding.UTF8.GetBytes("PrefsKey");
				byte[] numArray = sHA256Managed.ComputeHash(bytes);
				this._hmacPrefsKey = BitConverter.ToString(numArray).Replace("-", string.Empty);
				this._hmacPrefsKey = this._hmacPrefsKey.Substring(0, Math.Min(32, this._hmacPrefsKey.Length)).ToLower();
				byte[] bytes1 = Encoding.UTF8.GetBytes("HmacKey");
				this._hmac = new HMACSHA256(sHA256Managed.ComputeHash(bytes1));
			}
		}

		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._hmac.Clear();
			this._disposed = true;
		}

		public bool Verify()
		{
			return true;
		}
	}
}