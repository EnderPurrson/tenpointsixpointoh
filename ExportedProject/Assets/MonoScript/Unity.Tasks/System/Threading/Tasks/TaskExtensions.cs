using System.IO;
using System.Runtime.CompilerServices;
using Unity.Tasks.Internal;

namespace System.Threading.Tasks
{
	public static class TaskExtensions
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass0_0
		{
			public TaskCompletionSource<int> tcs;

			public Task<Task> task;

			public Action<Task> _003C_003E9__1;

			internal void _003CUnwrap_003Eb__0(Task<Task> t)
			{
				if (t.IsFaulted)
				{
					tcs.TrySetException(t.Exception);
				}
				else if (t.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					task.Result.ContinueWith(_003C_003E9__1 ?? (_003C_003E9__1 = _003CUnwrap_003Eb__1));
				}
			}

			internal void _003CUnwrap_003Eb__1(Task inner)
			{
				if (inner.IsFaulted)
				{
					tcs.TrySetException(inner.Exception);
				}
				else if (inner.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					tcs.TrySetResult(0);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass1_0<T>
		{
			public TaskCompletionSource<T> tcs;

			public Action<Task<T>> _003C_003E9__1;

			internal void _003CUnwrap_003Eb__0(Task<Task<T>> t)
			{
				if (t.IsFaulted)
				{
					tcs.TrySetException(t.Exception);
				}
				else if (t.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					t.Result.ContinueWith(_003C_003E9__1 ?? (_003C_003E9__1 = (Action<Task<T>>)(object)new Action<Task<Task<T>>>(_003CUnwrap_003Eb__1)));
				}
			}

			internal void _003CUnwrap_003Eb__1(Task<T> inner)
			{
				if (inner.IsFaulted)
				{
					tcs.TrySetException(inner.Exception);
				}
				else if (inner.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					tcs.TrySetResult(inner.Result);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2_0
		{
			public StreamReader reader;

			internal string _003CReadToEndAsync_003Eb__0()
			{
				return ((TextReader)reader).ReadToEnd();
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass4_0
		{
			public Stream stream;

			public byte[] buffer;

			public int bufferSize;

			public CancellationToken cancellationToken;

			public int bytesRead;

			public Stream destination;

			public Func<Task<int>, bool> _003C_003E9__1;

			public Action<Task> _003C_003E9__3;

			internal Task<bool> _003CCopyToAsync_003Eb__0()
			{
				return stream.ReadAsync(buffer, 0, bufferSize, cancellationToken).OnSuccess<int, bool>(_003C_003E9__1 ?? (_003C_003E9__1 = _003CCopyToAsync_003Eb__1));
			}

			internal bool _003CCopyToAsync_003Eb__1(Task<int> readTask)
			{
				bytesRead = readTask.Result;
				return bytesRead > 0;
			}

			internal Task _003CCopyToAsync_003Eb__2()
			{
				cancellationToken.ThrowIfCancellationRequested();
				return destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).OnSuccess(_003C_003E9__3 ?? (_003C_003E9__3 = _003CCopyToAsync_003Eb__3));
			}

			internal void _003CCopyToAsync_003Eb__3(Task _)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
		}

		public static Task Unwrap(this Task<Task> task)
		{
			_003C_003Ec__DisplayClass0_0 _003C_003Ec__DisplayClass0_ = new _003C_003Ec__DisplayClass0_0();
			_003C_003Ec__DisplayClass0_.task = task;
			_003C_003Ec__DisplayClass0_.tcs = new TaskCompletionSource<int>();
			_003C_003Ec__DisplayClass0_.task.ContinueWith(_003C_003Ec__DisplayClass0_._003CUnwrap_003Eb__0);
			return _003C_003Ec__DisplayClass0_.tcs.Task;
		}

		public static Task<T> Unwrap<T>(this Task<Task<T>> task)
		{
			_003C_003Ec__DisplayClass1_0<T> _003C_003Ec__DisplayClass1_ = new _003C_003Ec__DisplayClass1_0<T>();
			_003C_003Ec__DisplayClass1_.tcs = new TaskCompletionSource<T>();
			task.ContinueWith(_003C_003Ec__DisplayClass1_._003CUnwrap_003Eb__0);
			return _003C_003Ec__DisplayClass1_.tcs.Task;
		}

		public static Task<string> ReadToEndAsync(this StreamReader reader)
		{
			return Task.Run<string>(new _003C_003Ec__DisplayClass2_0
			{
				reader = reader
			}._003CReadToEndAsync_003Eb__0);
		}

		public static Task CopyToAsync(this Stream stream, Stream destination)
		{
			return stream.CopyToAsync(destination, 2048, CancellationToken.None);
		}

		public static Task CopyToAsync(this Stream stream, Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			_003C_003Ec__DisplayClass4_0 _003C_003Ec__DisplayClass4_ = new _003C_003Ec__DisplayClass4_0();
			_003C_003Ec__DisplayClass4_.stream = stream;
			_003C_003Ec__DisplayClass4_.bufferSize = bufferSize;
			_003C_003Ec__DisplayClass4_.cancellationToken = cancellationToken;
			_003C_003Ec__DisplayClass4_.destination = destination;
			_003C_003Ec__DisplayClass4_.buffer = new byte[_003C_003Ec__DisplayClass4_.bufferSize];
			_003C_003Ec__DisplayClass4_.bytesRead = 0;
			return InternalExtensions.WhileAsync(_003C_003Ec__DisplayClass4_._003CCopyToAsync_003Eb__0, _003C_003Ec__DisplayClass4_._003CCopyToAsync_003Eb__2);
		}

		public static Task<int> ReadAsync(this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
				taskCompletionSource.SetCanceled();
				return taskCompletionSource.Task;
			}
			return Task.Factory.FromAsync<byte[], int, int, int>((Func<byte[], int, int, AsyncCallback, object, IAsyncResult>)stream.BeginRead, (Func<IAsyncResult, int>)stream.EndRead, buffer, offset, count, (object)default(object));
		}

		public static Task WriteAsync(this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				taskCompletionSource.SetCanceled();
				return taskCompletionSource.Task;
			}
			return Task.Factory.FromAsync((Func<byte[], int, int, AsyncCallback, object, IAsyncResult>)stream.BeginWrite, (Action<IAsyncResult>)stream.EndWrite, buffer, offset, count, (object)default(object));
		}
	}
}
