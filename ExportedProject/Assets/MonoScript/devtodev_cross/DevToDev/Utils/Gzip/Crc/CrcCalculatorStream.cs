using System;
using System.IO;

namespace DevToDev.Utils.Gzip.Crc
{
	internal class CrcCalculatorStream : Stream, global::System.IDisposable
	{
		private static readonly long UnsetLengthLimit = -99L;

		internal Stream _innerStream;

		private CRC32 _Crc32;

		private long _lengthLimit = -99L;

		private bool _leaveOpen;

		public long TotalBytesSlurped
		{
			get
			{
				return _Crc32.TotalBytesRead;
			}
		}

		public int Crc
		{
			get
			{
				return _Crc32.Crc32Result;
			}
		}

		public bool LeaveOpen
		{
			get
			{
				return _leaveOpen;
			}
			set
			{
				_leaveOpen = value;
			}
		}

		public override bool CanRead
		{
			get
			{
				return _innerStream.get_CanRead();
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return _innerStream.get_CanWrite();
			}
		}

		public override long Length
		{
			get
			{
				if (_lengthLimit == UnsetLengthLimit)
				{
					return _innerStream.get_Length();
				}
				return _lengthLimit;
			}
		}

		public override long Position
		{
			get
			{
				return _Crc32.TotalBytesRead;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		public CrcCalculatorStream(Stream stream)
			: this(true, UnsetLengthLimit, stream, null)
		{
		}

		public CrcCalculatorStream(Stream stream, bool leaveOpen)
			: this(leaveOpen, UnsetLengthLimit, stream, null)
		{
		}

		public CrcCalculatorStream(Stream stream, long length)
			: this(true, length, stream, null)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (length < 0)
			{
				throw new ArgumentException("length");
			}
		}

		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen)
			: this(leaveOpen, length, stream, null)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (length < 0)
			{
				throw new ArgumentException("length");
			}
		}

		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32)
			: this(leaveOpen, length, stream, crc32)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (length < 0)
			{
				throw new ArgumentException("length");
			}
		}

		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			_innerStream = stream;
			_Crc32 = crc32 ?? new CRC32();
			_lengthLimit = length;
			_leaveOpen = leaveOpen;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = count;
			if (_lengthLimit != UnsetLengthLimit)
			{
				if (_Crc32.TotalBytesRead >= _lengthLimit)
				{
					return 0;
				}
				long num2 = _lengthLimit - _Crc32.TotalBytesRead;
				if (num2 < count)
				{
					num = (int)num2;
				}
			}
			int num3 = _innerStream.Read(buffer, offset, num);
			if (num3 > 0)
			{
				_Crc32.SlurpBlock(buffer, offset, num3);
			}
			return num3;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				_Crc32.SlurpBlock(buffer, offset, count);
			}
			_innerStream.Write(buffer, offset, count);
		}

		public override void Flush()
		{
			_innerStream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}

		void global::System.IDisposable.Dispose()
		{
			((Stream)this).Dispose();
			if (!_leaveOpen)
			{
				_innerStream.Dispose();
			}
		}
	}
}
