using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DevToDev.Utils.Gzip.Crc;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal class ZlibBaseStream : Stream
	{
		internal enum StreamMode
		{
			Writer = 0,
			Reader = 1,
			Undefined = 2
		}

		protected internal ZlibCodec _z;

		protected internal StreamMode _streamMode = StreamMode.Undefined;

		protected internal FlushType _flushMode;

		protected internal ZlibStreamFlavor _flavor;

		protected internal CompressionMode _compressionMode;

		protected internal CompressionLevel _level;

		protected internal bool _leaveOpen;

		protected internal byte[] _workingBuffer;

		protected internal int _bufferSize = 16384;

		protected internal byte[] _buf1 = new byte[1];

		protected internal Stream _stream;

		protected internal CompressionStrategy Strategy;

		private CRC32 crc;

		protected internal string _GzipFileName;

		protected internal string _GzipComment;

		protected internal global::System.DateTime _GzipMtime;

		protected internal int _gzipHeaderByteCount;

		private bool nomoreinput;

		internal int Crc32
		{
			get
			{
				if (crc == null)
				{
					return 0;
				}
				return crc.Crc32Result;
			}
		}

		protected internal bool _wantCompress
		{
			get
			{
				return _compressionMode == CompressionMode.Compress;
			}
		}

		private ZlibCodec z
		{
			get
			{
				if (_z == null)
				{
					bool flag = _flavor == ZlibStreamFlavor.ZLIB;
					_z = new ZlibCodec();
					if (_compressionMode == CompressionMode.Decompress)
					{
						_z.InitializeInflate(flag);
					}
					else
					{
						_z.Strategy = Strategy;
						_z.InitializeDeflate(_level, flag);
					}
				}
				return _z;
			}
		}

		private byte[] workingBuffer
		{
			get
			{
				if (_workingBuffer == null)
				{
					_workingBuffer = new byte[_bufferSize];
				}
				return _workingBuffer;
			}
		}

		public override bool CanRead
		{
			get
			{
				return _stream.get_CanRead();
			}
		}

		public override bool CanSeek
		{
			get
			{
				return _stream.get_CanSeek();
			}
		}

		public override bool CanWrite
		{
			get
			{
				return _stream.get_CanWrite();
			}
		}

		public override long Length
		{
			get
			{
				return _stream.get_Length();
			}
		}

		public override long Position
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotImplementedException();
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotImplementedException();
			}
		}

		public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen)
		{
			_flushMode = FlushType.None;
			_stream = stream;
			_leaveOpen = leaveOpen;
			_compressionMode = compressionMode;
			_flavor = flavor;
			_level = level;
			if (flavor == ZlibStreamFlavor.GZIP)
			{
				crc = new CRC32();
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (crc != null)
			{
				crc.SlurpBlock(buffer, offset, count);
			}
			if (_streamMode == StreamMode.Undefined)
			{
				_streamMode = StreamMode.Writer;
			}
			else if (_streamMode != 0)
			{
				throw new ZlibException("Cannot Write after Reading.");
			}
			if (count == 0)
			{
				return;
			}
			z.InputBuffer = buffer;
			_z.NextIn = offset;
			_z.AvailableBytesIn = count;
			bool flag = false;
			do
			{
				_z.OutputBuffer = workingBuffer;
				_z.NextOut = 0;
				_z.AvailableBytesOut = _workingBuffer.Length;
				int num = (_wantCompress ? _z.Deflate(_flushMode) : _z.Inflate(_flushMode));
				if (num != 0 && num != 1)
				{
					throw new ZlibException((_wantCompress ? "de" : "in") + "flating: " + _z.Message);
				}
				_stream.Write(_workingBuffer, 0, _workingBuffer.Length - _z.AvailableBytesOut);
				flag = _z.AvailableBytesIn == 0 && _z.AvailableBytesOut != 0;
				if (_flavor == ZlibStreamFlavor.GZIP && !_wantCompress)
				{
					flag = _z.AvailableBytesIn == 8 && _z.AvailableBytesOut != 0;
				}
			}
			while (!flag);
		}

		private void finish()
		{
			if (_z == null)
			{
				return;
			}
			if (_streamMode == StreamMode.Writer)
			{
				bool flag = false;
				do
				{
					_z.OutputBuffer = workingBuffer;
					_z.NextOut = 0;
					_z.AvailableBytesOut = _workingBuffer.Length;
					int num = (_wantCompress ? _z.Deflate(FlushType.Finish) : _z.Inflate(FlushType.Finish));
					if (num != 1 && num != 0)
					{
						string text = (_wantCompress ? "de" : "in") + "flating";
						if (_z.Message == null)
						{
							throw new ZlibException(string.Format("{0}: (rc = {1})", (object)text, (object)num));
						}
						throw new ZlibException(text + ": " + _z.Message);
					}
					if (_workingBuffer.Length - _z.AvailableBytesOut > 0)
					{
						_stream.Write(_workingBuffer, 0, _workingBuffer.Length - _z.AvailableBytesOut);
					}
					flag = _z.AvailableBytesIn == 0 && _z.AvailableBytesOut != 0;
					if (_flavor == ZlibStreamFlavor.GZIP && !_wantCompress)
					{
						flag = _z.AvailableBytesIn == 8 && _z.AvailableBytesOut != 0;
					}
				}
				while (!flag);
				((Stream)this).Flush();
				if (_flavor == ZlibStreamFlavor.GZIP)
				{
					if (!_wantCompress)
					{
						throw new ZlibException("Writing with decompression is not supported.");
					}
					int crc32Result = crc.Crc32Result;
					_stream.Write(BitConverter.GetBytes(crc32Result), 0, 4);
					int num2 = (int)(crc.TotalBytesRead & 0xFFFFFFFFu);
					_stream.Write(BitConverter.GetBytes(num2), 0, 4);
				}
			}
			else
			{
				if (_streamMode != StreamMode.Reader || _flavor != ZlibStreamFlavor.GZIP)
				{
					return;
				}
				if (_wantCompress)
				{
					throw new ZlibException("Reading with compression is not supported.");
				}
				if (_z.TotalBytesOut == 0)
				{
					return;
				}
				byte[] array = new byte[8];
				if (_z.AvailableBytesIn < 8)
				{
					global::System.Array.Copy((global::System.Array)_z.InputBuffer, _z.NextIn, (global::System.Array)array, 0, _z.AvailableBytesIn);
					int num3 = 8 - _z.AvailableBytesIn;
					int num4 = _stream.Read(array, _z.AvailableBytesIn, num3);
					if (num3 != num4)
					{
						throw new ZlibException(string.Format("Missing or incomplete GZIP trailer. Expected 8 bytes, got {0}.", (object)(_z.AvailableBytesIn + num4)));
					}
				}
				else
				{
					global::System.Array.Copy((global::System.Array)_z.InputBuffer, _z.NextIn, (global::System.Array)array, 0, array.Length);
				}
				int num5 = BitConverter.ToInt32(array, 0);
				int crc32Result2 = crc.Crc32Result;
				int num6 = BitConverter.ToInt32(array, 4);
				int num7 = (int)(_z.TotalBytesOut & 0xFFFFFFFFu);
				if (crc32Result2 != num5)
				{
					throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", (object)crc32Result2, (object)num5));
				}
				if (num7 != num6)
				{
					throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", (object)num7, (object)num6));
				}
			}
		}

		private void end()
		{
			if (z != null)
			{
				if (_wantCompress)
				{
					_z.EndDeflate();
				}
				else
				{
					_z.EndInflate();
				}
				_z = null;
			}
		}

		public override void Close()
		{
			if (_stream == null)
			{
				return;
			}
			try
			{
				finish();
			}
			finally
			{
				end();
				if (!_leaveOpen)
				{
					_stream.Dispose();
				}
				_stream = null;
			}
		}

		public override void Flush()
		{
			_stream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			_stream.SetLength(value);
		}

		private string ReadZeroTerminatedString()
		{
			List<byte> val = new List<byte>();
			bool flag = false;
			do
			{
				int num = _stream.Read(_buf1, 0, 1);
				if (num != 1)
				{
					throw new ZlibException("Unexpected EOF reading GZIP header.");
				}
				if (_buf1[0] == 0)
				{
					flag = true;
				}
				else
				{
					val.Add(_buf1[0]);
				}
			}
			while (!flag);
			byte[] array = val.ToArray();
			return GZipStream.iso8859dash1.GetString(array, 0, array.Length);
		}

		private int _ReadAndValidateGzipHeader()
		{
			int num = 0;
			byte[] array = new byte[10];
			int num2 = _stream.Read(array, 0, array.Length);
			switch (num2)
			{
			case 0:
				return 0;
			default:
				throw new ZlibException("Not a valid GZIP stream.");
			case 10:
			{
				if (array[0] != 31 || array[1] != 139 || array[2] != 8)
				{
					throw new ZlibException("Bad GZIP header.");
				}
				int num3 = BitConverter.ToInt32(array, 4);
				_GzipMtime = GZipStream._unixEpoch.AddSeconds((double)num3);
				num += num2;
				if ((array[3] & 4) == 4)
				{
					num2 = _stream.Read(array, 0, 2);
					num += num2;
					short num4 = (short)(array[0] + array[1] * 256);
					byte[] array2 = new byte[num4];
					num2 = _stream.Read(array2, 0, array2.Length);
					if (num2 != num4)
					{
						throw new ZlibException("Unexpected end-of-file reading GZIP header.");
					}
					num += num2;
				}
				if ((array[3] & 8) == 8)
				{
					_GzipFileName = ReadZeroTerminatedString();
				}
				if ((array[3] & 0x10) == 16)
				{
					_GzipComment = ReadZeroTerminatedString();
				}
				if ((array[3] & 2) == 2)
				{
					((Stream)this).Read(_buf1, 0, 1);
				}
				return num;
			}
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			if (_streamMode == StreamMode.Undefined)
			{
				if (!_stream.get_CanRead())
				{
					throw new ZlibException("The stream is not readable.");
				}
				_streamMode = StreamMode.Reader;
				z.AvailableBytesIn = 0;
				if (_flavor == ZlibStreamFlavor.GZIP)
				{
					_gzipHeaderByteCount = _ReadAndValidateGzipHeader();
					if (_gzipHeaderByteCount == 0)
					{
						return 0;
					}
				}
			}
			if (_streamMode != StreamMode.Reader)
			{
				throw new ZlibException("Cannot Read after Writing.");
			}
			if (count == 0)
			{
				return 0;
			}
			if (nomoreinput && _wantCompress)
			{
				return 0;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (offset < ((global::System.Array)buffer).GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > ((global::System.Array)buffer).GetLength(0))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num = 0;
			_z.OutputBuffer = buffer;
			_z.NextOut = offset;
			_z.AvailableBytesOut = count;
			_z.InputBuffer = workingBuffer;
			do
			{
				if (_z.AvailableBytesIn == 0 && !nomoreinput)
				{
					_z.NextIn = 0;
					_z.AvailableBytesIn = _stream.Read(_workingBuffer, 0, _workingBuffer.Length);
					if (_z.AvailableBytesIn == 0)
					{
						nomoreinput = true;
					}
				}
				num = (_wantCompress ? _z.Deflate(_flushMode) : _z.Inflate(_flushMode));
				if (nomoreinput && num == -5)
				{
					return 0;
				}
				if (num != 0 && num != 1)
				{
					throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", (object)(_wantCompress ? "de" : "in"), (object)num, (object)_z.Message));
				}
			}
			while (((!nomoreinput && num != 1) || _z.AvailableBytesOut != count) && _z.AvailableBytesOut > 0 && !nomoreinput && num == 0);
			if (_z.AvailableBytesOut > 0)
			{
				if (num == 0)
				{
					int availableBytesIn = _z.AvailableBytesIn;
				}
				if (nomoreinput && _wantCompress)
				{
					num = _z.Deflate(FlushType.Finish);
					if (num != 0 && num != 1)
					{
						throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", (object)num, (object)_z.Message));
					}
				}
			}
			num = count - _z.AvailableBytesOut;
			if (crc != null)
			{
				crc.SlurpBlock(buffer, offset, num);
			}
			return num;
		}

		public static void CompressString(string s, Stream compressor)
		{
			byte[] bytes = Encoding.get_UTF8().GetBytes(s);
			try
			{
				compressor.Write(bytes, 0, bytes.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((global::System.IDisposable)compressor).Dispose();
				}
			}
		}

		public static void CompressBuffer(byte[] b, Stream compressor)
		{
			try
			{
				compressor.Write(b, 0, b.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((global::System.IDisposable)compressor).Dispose();
				}
			}
		}

		public static string UncompressString(byte[] compressed, Stream decompressor)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			byte[] array = new byte[1024];
			Encoding uTF = Encoding.get_UTF8();
			MemoryStream val = new MemoryStream();
			try
			{
				try
				{
					int num;
					while ((num = decompressor.Read(array, 0, array.Length)) != 0)
					{
						((Stream)val).Write(array, 0, num);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((global::System.IDisposable)decompressor).Dispose();
					}
				}
				((Stream)val).Seek(0L, (SeekOrigin)0);
				StreamReader val2 = new StreamReader((Stream)(object)val, uTF);
				return ((TextReader)val2).ReadToEnd();
			}
			finally
			{
				if (val != null)
				{
					((global::System.IDisposable)val).Dispose();
				}
			}
		}

		public static byte[] UncompressBuffer(byte[] compressed, Stream decompressor)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			byte[] array = new byte[1024];
			MemoryStream val = new MemoryStream();
			try
			{
				try
				{
					int num;
					while ((num = decompressor.Read(array, 0, array.Length)) != 0)
					{
						((Stream)val).Write(array, 0, num);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((global::System.IDisposable)decompressor).Dispose();
					}
				}
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
	}
}
