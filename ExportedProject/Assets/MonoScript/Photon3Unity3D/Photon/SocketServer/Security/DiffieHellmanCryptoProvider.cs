using System;
using System.Security.Cryptography;
using Photon.SocketServer.Numeric;

namespace Photon.SocketServer.Security
{
	internal class DiffieHellmanCryptoProvider : ICryptoProvider, global::System.IDisposable
	{
		private static readonly BigInteger primeRoot = new BigInteger(OakleyGroups.Generator);

		private readonly BigInteger prime;

		private readonly BigInteger secret;

		private readonly BigInteger publicKey;

		private Rijndael crypto;

		private byte[] sharedKey;

		public bool IsInitialized
		{
			get
			{
				return crypto != null;
			}
		}

		public byte[] PublicKey
		{
			get
			{
				return publicKey.GetBytes();
			}
		}

		public DiffieHellmanCryptoProvider()
		{
			prime = new BigInteger(OakleyGroups.OakleyPrime768);
			secret = GenerateRandomSecret(160);
			publicKey = CalculatePublicKey();
		}

		public void DeriveSharedKey(byte[] otherPartyPublicKey)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			BigInteger otherPartyPublicKey2 = new BigInteger(otherPartyPublicKey);
			BigInteger bigInteger = CalculateSharedKey(otherPartyPublicKey2);
			sharedKey = bigInteger.GetBytes();
			SHA256 val = (SHA256)new SHA256Managed();
			byte[] key;
			try
			{
				key = ((HashAlgorithm)val).ComputeHash(sharedKey);
			}
			finally
			{
				if (val != null)
				{
					((global::System.IDisposable)val).Dispose();
				}
			}
			crypto = (Rijndael)new RijndaelManaged();
			((SymmetricAlgorithm)crypto).set_Key(key);
			((SymmetricAlgorithm)crypto).set_IV(new byte[16]);
			((SymmetricAlgorithm)crypto).set_Padding((PaddingMode)2);
		}

		public byte[] Encrypt(byte[] data)
		{
			return Encrypt(data, 0, data.Length);
		}

		public byte[] Encrypt(byte[] data, int offset, int count)
		{
			ICryptoTransform val = ((SymmetricAlgorithm)crypto).CreateEncryptor();
			try
			{
				return val.TransformFinalBlock(data, offset, count);
			}
			finally
			{
				if (val != null)
				{
					((global::System.IDisposable)val).Dispose();
				}
			}
		}

		public byte[] Decrypt(byte[] data)
		{
			return Decrypt(data, 0, data.Length);
		}

		public byte[] Decrypt(byte[] data, int offset, int count)
		{
			ICryptoTransform val = ((SymmetricAlgorithm)crypto).CreateDecryptor();
			try
			{
				return val.TransformFinalBlock(data, offset, count);
			}
			finally
			{
				if (val != null)
				{
					((global::System.IDisposable)val).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize((object)this);
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
		}

		private BigInteger CalculatePublicKey()
		{
			return primeRoot.ModPow(secret, prime);
		}

		private BigInteger CalculateSharedKey(BigInteger otherPartyPublicKey)
		{
			return otherPartyPublicKey.ModPow(secret, prime);
		}

		private BigInteger GenerateRandomSecret(int secretLength)
		{
			BigInteger bigInteger;
			do
			{
				bigInteger = BigInteger.GenerateRandom(secretLength);
			}
			while (bigInteger >= prime - 1 || bigInteger == 0);
			return bigInteger;
		}
	}
}
