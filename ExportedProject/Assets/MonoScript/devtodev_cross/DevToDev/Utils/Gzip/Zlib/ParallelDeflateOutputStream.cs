using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using DevToDev.Utils.Gzip.Crc;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal class ParallelDeflateOutputStream : Stream
	{
		[Flags]
		private enum TraceBits : uint
		{
			None = 0u,
			NotUsed1 = 1u,
			EmitLock = 2u,
			EmitEnter = 4u,
			EmitBegin = 8u,
			EmitDone = 16u,
			EmitSkip = 32u,
			EmitAll = 58u,
			Flush = 64u,
			Lifecycle = 128u,
			Session = 256u,
			Synch = 512u,
			Instance = 1024u,
			Compress = 2048u,
			Write = 4096u,
			WriteEnter = 8192u,
			WriteTake = 16384u,
			All = 4294967295u
		}

		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		private static readonly int BufferPairsPerCore = 4;

		private List<WorkItem> _pool;

		private bool _leaveOpen;

		private bool emitting;

		private Stream _outStream;

		private int _maxBufferPairs;

		private int _bufferSize = IO_BUFFER_SIZE_DEFAULT;

		private AutoResetEvent _newlyCompressedBlob;

		private object _outputLock = new object();

		private bool _isClosed;

		private bool _firstWriteDone;

		private int _currentlyFilling;

		private int _lastFilled;

		private int _lastWritten;

		private int _latestCompressed;

		private int _Crc32;

		private CRC32 _runningCrc;

		private object _latestLock = new object();

		private Queue<int> _toWrite;

		private Queue<int> _toFill;

		private long _totalBytesProcessed;

		private CompressionLevel _compressLevel;

		private volatile global::System.Exception _pendingException;

		private bool _handlingException;

		private object _eLock = new object();

		private TraceBits _DesiredTrace = (TraceBits)26942u;

		public CompressionStrategy Strategy
		{
			[CompilerGenerated]
			get
			{
				return _003CStrategy_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CStrategy_003Ek__BackingField = value;
			}
		}

		public int MaxBufferPairs
		{
			get
			{
				return _maxBufferPairs;
			}
			set
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				_maxBufferPairs = value;
			}
		}

		public int BufferSize
		{
			get
			{
				return _bufferSize;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				_bufferSize = value;
			}
		}

		public int Crc32
		{
			get
			{
				return _Crc32;
			}
		}

		public long BytesProcessed
		{
			get
			{
				return _totalBytesProcessed;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanRead
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
				return _outStream.get_CanWrite();
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
				return _outStream.get_Position();
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		public ParallelDeflateOutputStream(Stream stream)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level)
			: this(stream, level, CompressionStrategy.Default, false)
		{
		}

		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			_outStream = stream;
			_compressLevel = level;
			Strategy = strategy;
			_leaveOpen = leaveOpen;
			MaxBufferPairs = 16;
		}

		private void _InitializePoolOfWorkItems()
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			_toWrite = new Queue<int>();
			_toFill = new Queue<int>();
			_pool = new List<WorkItem>();
			int bufferPairsPerCore = BufferPairsPerCore;
			bufferPairsPerCore = Math.Min(bufferPairsPerCore, _maxBufferPairs);
			for (int i = 0; i < bufferPairsPerCore; i++)
			{
				_pool.Add(new WorkItem(_bufferSize, _compressLevel, Strategy, i));
				_toFill.Enqueue(i);
			}
			_newlyCompressedBlob = new AutoResetEvent(false);
			_runningCrc = new CRC32();
			_currentlyFilling = -1;
			_lastFilled = -1;
			_lastWritten = -1;
			_latestCompressed = -1;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			bool mustWait = false;
			if (_isClosed)
			{
				throw new InvalidOperationException();
			}
			if (_pendingException != null)
			{
				_handlingException = true;
				global::System.Exception pendingException = _pendingException;
				_pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!_firstWriteDone)
			{
				_InitializePoolOfWorkItems();
				_firstWriteDone = true;
			}
			do
			{
				EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num = -1;
				if (_currentlyFilling >= 0)
				{
					num = _currentlyFilling;
				}
				else
				{
					if (_toFill.get_Count() == 0)
					{
						mustWait = true;
						continue;
					}
					num = _toFill.Dequeue();
					_lastFilled++;
				}
				WorkItem workItem = _pool.get_Item(num);
				int num2 = ((workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable));
				workItem.ordinal = _lastFilled;
				Buffer.BlockCopy((global::System.Array)buffer, offset, (global::System.Array)workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(_DeflateOne), (object)workItem))
					{
						throw new global::System.Exception("Cannot enqueue workitem");
					}
					_currentlyFilling = -1;
				}
				else
				{
					_currentlyFilling = num;
				}
				int num3 = 0;
			}
			while (count > 0);
		}

		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(_compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new global::System.Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				_outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			_Crc32 = _runningCrc.Crc32Result;
		}

		private void _Flush(bool lastInput)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (_isClosed)
			{
				throw new InvalidOperationException();
			}
			if (!emitting)
			{
				if (_currentlyFilling >= 0)
				{
					WorkItem wi = _pool.get_Item(_currentlyFilling);
					_DeflateOne(wi);
					_currentlyFilling = -1;
				}
				if (lastInput)
				{
					EmitPendingBuffers(true, false);
					_FlushFinish();
				}
				else
				{
					EmitPendingBuffers(false, false);
				}
			}
		}

		public override void Flush()
		{
			if (_pendingException != null)
			{
				_handlingException = true;
				global::System.Exception pendingException = _pendingException;
				_pendingException = null;
				throw pendingException;
			}
			if (!_handlingException)
			{
				_Flush(false);
			}
		}

		public override void Close()
		{
			if (_pendingException != null)
			{
				_handlingException = true;
				global::System.Exception pendingException = _pendingException;
				_pendingException = null;
				throw pendingException;
			}
			if (!_handlingException && !_isClosed)
			{
				_Flush(true);
				if (!_leaveOpen)
				{
					_outStream.Dispose();
				}
				_isClosed = true;
			}
		}

		public void Dispose()
		{
			((Stream)this).Close();
			_pool = null;
			((Stream)this).Dispose(true);
		}

		protected override void Dispose(bool disposing)
		{
			((Stream)this).Dispose(disposing);
		}

		public void Reset(Stream stream)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (!_firstWriteDone)
			{
				return;
			}
			_toWrite.Clear();
			_toFill.Clear();
			Enumerator<WorkItem> enumerator = _pool.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					WorkItem current = enumerator.get_Current();
					_toFill.Enqueue(current.index);
					current.ordinal = -1;
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			_firstWriteDone = false;
			_totalBytesProcessed = 0L;
			_runningCrc = new CRC32();
			_isClosed = false;
			_currentlyFilling = -1;
			_lastFilled = -1;
			_lastWritten = -1;
			_latestCompressed = -1;
			_outStream = stream;
		}

		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (emitting)
			{
				return;
			}
			emitting = true;
			if (doAll || mustWait)
			{
				((WaitHandle)_newlyCompressedBlob).WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = (doAll ? 200 : (mustWait ? (-1) : 0));
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter((object)_toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (_toWrite.get_Count() > 0)
							{
								num3 = _toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit((object)_toWrite);
						}
						if (num3 < 0)
						{
							continue;
						}
						WorkItem workItem = _pool.get_Item(num3);
						if (workItem.ordinal != _lastWritten + 1)
						{
							lock (_toWrite)
							{
								_toWrite.Enqueue(num3);
							}
							if (num == num3)
							{
								((WaitHandle)_newlyCompressedBlob).WaitOne();
								num = -1;
							}
							else if (num == -1)
							{
								num = num3;
							}
							continue;
						}
						num = -1;
						_outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
						_runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
						_totalBytesProcessed += workItem.inputBytesAvailable;
						workItem.inputBytesAvailable = 0;
						_lastWritten = workItem.ordinal;
						_toFill.Enqueue(workItem.index);
						if (num2 == -1)
						{
							num2 = 0;
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && _lastWritten != _latestCompressed);
			emitting = false;
		}

		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				CRC32 cRC = new CRC32();
				cRC.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				DeflateOneSegment(workItem);
				workItem.crc = cRC.Crc32Result;
				lock (_latestLock)
				{
					if (workItem.ordinal > _latestCompressed)
					{
						_latestCompressed = workItem.ordinal;
					}
				}
				lock (_toWrite)
				{
					_toWrite.Enqueue(workItem.index);
				}
				((EventWaitHandle)_newlyCompressedBlob).Set();
			}
			catch (global::System.Exception pendingException)
			{
				lock (_eLock)
				{
					if (_pendingException != null)
					{
						_pendingException = pendingException;
					}
				}
			}
		}

		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		[Conditional("Trace")]
		private void TraceOutput(TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & _DesiredTrace) != 0)
			{
				lock (_outputLock)
				{
					((object)Thread.get_CurrentThread()).GetHashCode();
				}
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
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
	}
}
