using System;
using System.Security.Cryptography;

internal class MD5Managed : MD5
{
	private byte[] _data;

	private ABCDStruct _abcd;

	private long _totalLength;

	private int _dataSize;

	public MD5Managed()
	{
		((HashAlgorithm)this).HashSizeValue = 128;
		((HashAlgorithm)this).Initialize();
	}

	public override void Initialize()
	{
		_data = new byte[64];
		_dataSize = 0;
		_totalLength = 0L;
		_abcd = default(ABCDStruct);
		_abcd.A = 1732584193u;
		_abcd.B = 4023233417u;
		_abcd.C = 2562383102u;
		_abcd.D = 271733878u;
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		int num = ibStart;
		int num2 = _dataSize + cbSize;
		if (num2 >= 64)
		{
			global::System.Array.Copy((global::System.Array)array, num, (global::System.Array)_data, _dataSize, 64 - _dataSize);
			MD5Core.GetHashBlock(_data, ref _abcd, 0);
			num += 64 - _dataSize;
			num2 -= 64;
			while (num2 >= 64)
			{
				global::System.Array.Copy((global::System.Array)array, num, (global::System.Array)_data, 0, 64);
				MD5Core.GetHashBlock(array, ref _abcd, num);
				num2 -= 64;
				num += 64;
			}
			_dataSize = num2;
			global::System.Array.Copy((global::System.Array)array, num, (global::System.Array)_data, 0, num2);
		}
		else
		{
			global::System.Array.Copy((global::System.Array)array, num, (global::System.Array)_data, _dataSize, cbSize);
			_dataSize = num2;
		}
		_totalLength += cbSize;
	}

	protected override byte[] HashFinal()
	{
		((HashAlgorithm)this).HashValue = MD5Core.GetHashFinalBlock(_data, 0, _dataSize, _abcd, _totalLength * 8);
		return ((HashAlgorithm)this).HashValue;
	}
}
