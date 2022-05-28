using System;
using System.IO;
using System.Text;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal class GZipStream : Stream
	{
		public global::System.DateTime? LastModified;

		private int _headerByteCount;

		internal ZlibBaseStream _baseStream;

		private bool _disposed;

		private bool _firstReadDone;

		private string _FileName;

		private string _Comment;

		private int _Crc32;

		internal static readonly global::System.DateTime _unixEpoch = new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1);

		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");

		public string Comment
		{
			get
			{
				return _Comment;
			}
			set
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				_Comment = value;
			}
		}

		public string FileName
		{
			get
			{
				return _FileName;
			}
			set
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				_FileName = value;
				if (_FileName != null)
				{
					if (_FileName.IndexOf("/") != -1)
					{
						_FileName = _FileName.Replace("/", "\\");
					}
					if (_FileName.EndsWith("\\"))
					{
						throw new global::System.Exception("Illegal filename");
					}
					if (_FileName.IndexOf("\\") != -1)
					{
						_FileName = Path2.GetFileName(_FileName);
					}
				}
			}
		}

		public int Crc32
		{
			get
			{
				return _Crc32;
			}
		}

		public virtual FlushType FlushMode
		{
			get
			{
				return _baseStream._flushMode;
			}
			set
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				_baseStream._flushMode = value;
			}
		}

		public int BufferSize
		{
			get
			{
				return _baseStream._bufferSize;
			}
			set
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				if (_baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", (object)value, (object)1024));
				}
				_baseStream._bufferSize = value;
			}
		}

		public virtual long TotalIn
		{
			get
			{
				return _baseStream._z.TotalBytesIn;
			}
		}

		public virtual long TotalOut
		{
			get
			{
				return _baseStream._z.TotalBytesOut;
			}
		}

		public override bool CanRead
		{
			get
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return _baseStream._stream.get_CanRead();
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
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				if (_disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return _baseStream._stream.get_CanWrite();
			}
		}

		public override long Length
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotImplementedException();
			}
		}

		public override long Position
		{
			get
			{
				if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return _baseStream._z.TotalBytesOut + _headerByteCount;
				}
				if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return _baseStream._z.TotalBytesIn + _baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotImplementedException();
			}
		}

		public GZipStream(Stream stream, CompressionMode mode)
			: this(stream, mode, CompressionLevel.Default, false)
		{
		}

		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level)
			: this(stream, mode, level, false)
		{
		}

		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen)
			: this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			_baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!_disposed)
				{
					if (disposing && _baseStream != null)
					{
						((Stream)_baseStream).Close();
						_Crc32 = _baseStream.Crc32;
					}
					_disposed = true;
				}
			}
			finally
			{
				((Stream)this).Dispose(disposing);
			}
		}

		public override void Flush()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			((Stream)_baseStream).Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = ((Stream)_baseStream).Read(buffer, offset, count);
			if (!_firstReadDone)
			{
				_firstReadDone = true;
				FileName = _baseStream._GzipFileName;
				Comment = _baseStream._GzipComment;
			}
			return result;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (_disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!_baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				_headerByteCount = EmitHeader();
			}
			((Stream)_baseStream).Write(buffer, offset, count);
		}

		private int EmitHeader()
		{
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			byte[] array = ((Comment == null) ? null : iso8859dash1.GetBytes(Comment));
			byte[] array2 = ((FileName == null) ? null : iso8859dash1.GetBytes(FileName));
			int num = ((Comment != null) ? (array.Length + 1) : 0);
			int num2 = ((FileName != null) ? (array2.Length + 1) : 0);
			int num3 = 10 + num + num2;
			byte[] array3 = new byte[num3];
			int num4 = 0;
			array3[num4++] = 31;
			array3[num4++] = 139;
			array3[num4++] = 8;
			byte b = 0;
			if (Comment != null)
			{
				b = (byte)(b ^ 0x10u);
			}
			if (FileName != null)
			{
				b = (byte)(b ^ 8u);
			}
			array3[num4++] = b;
			if (!LastModified.get_HasValue())
			{
				LastModified = global::System.DateTime.get_Now();
			}
			TimeSpan val = LastModified.get_Value() - _unixEpoch;
			int num5 = (int)((TimeSpan)(ref val)).get_TotalSeconds();
			global::System.Array.Copy((global::System.Array)BitConverter.GetBytes(num5), 0, (global::System.Array)array3, num4, 4);
			num4 += 4;
			array3[num4++] = 0;
			array3[num4++] = 255;
			if (num2 != 0)
			{
				global::System.Array.Copy((global::System.Array)array2, 0, (global::System.Array)array3, num4, num2 - 1);
				num4 += num2 - 1;
				array3[num4++] = 0;
			}
			if (num != 0)
			{
				global::System.Array.Copy((global::System.Array)array, 0, (global::System.Array)array3, num4, num - 1);
				num4 += num - 1;
				array3[num4++] = 0;
			}
			_baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		public static byte[] CompressString(string s)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			MemoryStream val = new MemoryStream();
			try
			{
				Stream compressor = (Stream)(object)new GZipStream((Stream)(object)val, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				return val.ToArray();
			}
			finally
			{
				if (val != null)
				{
					((global::System.IDisposable)val).Dispose();
				}
			}
		}

		public static byte[] CompressBuffer(byte[] b)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			MemoryStream val = new MemoryStream();
			try
			{
				Stream compressor = (Stream)(object)new GZipStream((Stream)(object)val, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				return val.ToArray();
			}
			finally
			{
				if (val != null)
				{
					((global::System.IDisposable)val).Dispose();
				}
			}
		}

		public static string UncompressString(byte[] compressed)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			MemoryStream val = new MemoryStream(compressed);
			try
			{
				Stream decompressor = (Stream)(object)new GZipStream((Stream)(object)val, CompressionMode.Decompress);
				return ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			finally
			{
				if (val != null)
				{
					((global::System.IDisposable)val).Dispose();
				}
			}
		}

		public static byte[] UncompressBuffer(byte[] compressed)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			MemoryStream val = new MemoryStream(compressed);
			try
			{
				Stream decompressor = (Stream)(object)new GZipStream((Stream)(object)val, CompressionMode.Decompress);
				return ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			finally
			{
				if (val != null)
				{
					((global::System.IDisposable)val).Dispose();
				}
			}
		}
	}
}
