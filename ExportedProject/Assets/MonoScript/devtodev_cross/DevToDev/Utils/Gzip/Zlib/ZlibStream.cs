using System;
using System.IO;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal class ZlibStream : Stream
	{
		internal ZlibBaseStream _baseStream;

		private bool _disposed;

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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
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
					throw new ObjectDisposedException("ZlibStream");
				}
				return _baseStream._stream.get_CanWrite();
			}
		}

		public override long Length
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return _baseStream._z.TotalBytesOut;
				}
				if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return _baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		public ZlibStream(Stream stream, CompressionMode mode)
			: this(stream, mode, CompressionLevel.Default, false)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level)
			: this(stream, mode, level, false)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen)
			: this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			_baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
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
				throw new ObjectDisposedException("ZlibStream");
			}
			((Stream)_baseStream).Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (_disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return ((Stream)_baseStream).Read(buffer, offset, count);
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

		public override void Write(byte[] buffer, int offset, int count)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (_disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			((Stream)_baseStream).Write(buffer, offset, count);
		}

		public static byte[] CompressString(string s)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			MemoryStream val = new MemoryStream();
			try
			{
				Stream compressor = (Stream)(object)new ZlibStream((Stream)(object)val, CompressionMode.Compress, CompressionLevel.BestCompression);
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
				Stream compressor = (Stream)(object)new ZlibStream((Stream)(object)val, CompressionMode.Compress, CompressionLevel.BestCompression);
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
				Stream decompressor = (Stream)(object)new ZlibStream((Stream)(object)val, CompressionMode.Decompress);
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
				Stream decompressor = (Stream)(object)new ZlibStream((Stream)(object)val, CompressionMode.Decompress);
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
