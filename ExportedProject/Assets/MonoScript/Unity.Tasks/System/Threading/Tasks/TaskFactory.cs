using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	public class TaskFactory
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass9_0<T>
		{
			public TaskCompletionSource<T> tcs;

			public Func<T> func;

			internal void _003CStartNew_003Eb__0()
			{
				try
				{
					tcs.SetResult(func.Invoke());
				}
				catch (System.Exception exception)
				{
					tcs.SetException(exception);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass10_0<TArg1, TArg2, TArg3>
		{
			public Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod;

			public TArg1 arg1;

			public TArg2 arg2;

			public TArg3 arg3;

			public object state;

			internal IAsyncResult _003CFromAsync_003Eb__0(AsyncCallback callback, object _)
			{
				return beginMethod(arg1, arg2, arg3, callback, state);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass11_0<TArg1, TArg2, TArg3, TResult>
		{
			public Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod;

			public TArg1 arg1;

			public TArg2 arg2;

			public TArg3 arg3;

			public object state;

			internal IAsyncResult _003CFromAsync_003Eb__0(AsyncCallback callback, object _)
			{
				return beginMethod(arg1, arg2, arg3, callback, state);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass12_0<TArg1, TArg2>
		{
			public Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod;

			public TArg1 arg1;

			public TArg2 arg2;

			public object state;

			internal IAsyncResult _003CFromAsync_003Eb__0(AsyncCallback callback, object _)
			{
				return beginMethod.Invoke(arg1, arg2, callback, state);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass13_0<TArg1, TArg2, TResult>
		{
			public Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod;

			public TArg1 arg1;

			public TArg2 arg2;

			public object state;

			internal IAsyncResult _003CFromAsync_003Eb__0(AsyncCallback callback, object _)
			{
				return beginMethod.Invoke(arg1, arg2, callback, state);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass14_0<TArg1>
		{
			public Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod;

			public TArg1 arg1;

			public object state;

			internal IAsyncResult _003CFromAsync_003Eb__0(AsyncCallback callback, object _)
			{
				return beginMethod.Invoke(arg1, callback, state);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass15_0<TArg1, TResult>
		{
			public Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod;

			public TArg1 arg1;

			public object state;

			internal IAsyncResult _003CFromAsync_003Eb__0(AsyncCallback callback, object _)
			{
				return beginMethod.Invoke(arg1, callback, state);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass16_0
		{
			public Action<IAsyncResult> endMethod;

			internal int _003CFromAsync_003Eb__0(IAsyncResult result)
			{
				endMethod.Invoke(result);
				return 0;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass17_0<TResult>
		{
			public TaskCompletionSource<TResult> tcs;

			public Func<IAsyncResult, TResult> endMethod;

			public CancellationTokenRegistration cancellation;

			internal void _003CFromAsync_003Eb__0()
			{
				tcs.TrySetCanceled();
			}

			internal void _003CFromAsync_003Eb__1(IAsyncResult result)
			{
				try
				{
					TResult result2 = ((Func<IAsyncResult, IAsyncResult>)(object)endMethod).Invoke(result);
					tcs.TrySetResult(result2);
					cancellation.Dispose();
				}
				catch (System.Exception exception)
				{
					tcs.TrySetException(exception);
					cancellation.Dispose();
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass18_0
		{
			public int remaining;

			public TaskCompletionSource<Task[]> tcs;

			public Task[] tasks;

			public Action<Task[]> continuationAction;

			public Action<Task> _003C_003E9__0;

			internal void _003CContinueWhenAll_003Eb__0(Task _)
			{
				if (Interlocked.Decrement(ref remaining) == 0)
				{
					tcs.TrySetResult(tasks);
				}
			}

			internal void _003CContinueWhenAll_003Eb__1(Task<Task[]> t)
			{
				continuationAction.Invoke(t.Result);
			}
		}

		private readonly TaskScheduler scheduler;

		private readonly CancellationToken cancellationToken;

		public TaskScheduler Scheduler
		{
			get
			{
				return scheduler;
			}
		}

		internal TaskFactory(TaskScheduler scheduler, CancellationToken cancellationToken)
		{
			this.scheduler = scheduler;
			this.cancellationToken = cancellationToken;
		}

		public TaskFactory(TaskScheduler scheduler)
			: this(scheduler, CancellationToken.None)
		{
		}

		public TaskFactory(CancellationToken cancellationToken)
			: this(TaskScheduler.FromCurrentSynchronizationContext(), cancellationToken)
		{
		}

		public TaskFactory()
			: this(TaskScheduler.FromCurrentSynchronizationContext(), CancellationToken.None)
		{
		}

		public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
			: this(scheduler, cancellationToken)
		{
		}

		public Task<T> StartNew<T>(Func<T> func)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			_003C_003Ec__DisplayClass9_0<T> _003C_003Ec__DisplayClass9_ = new _003C_003Ec__DisplayClass9_0<T>();
			_003C_003Ec__DisplayClass9_.func = func;
			_003C_003Ec__DisplayClass9_.tcs = new TaskCompletionSource<T>();
			scheduler.Post(new Action(_003C_003Ec__DisplayClass9_._003CStartNew_003Eb__0));
			return _003C_003Ec__DisplayClass9_.tcs.Task;
		}

		public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			_003C_003Ec__DisplayClass10_0<TArg1, TArg2, TArg3> _003C_003Ec__DisplayClass10_ = new _003C_003Ec__DisplayClass10_0<TArg1, TArg2, TArg3>();
			_003C_003Ec__DisplayClass10_.beginMethod = beginMethod;
			_003C_003Ec__DisplayClass10_.arg1 = arg1;
			_003C_003Ec__DisplayClass10_.arg2 = arg2;
			_003C_003Ec__DisplayClass10_.arg3 = arg3;
			_003C_003Ec__DisplayClass10_.state = state;
			return FromAsync(_003C_003Ec__DisplayClass10_._003CFromAsync_003Eb__0, endMethod, _003C_003Ec__DisplayClass10_.state);
		}

		public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			_003C_003Ec__DisplayClass11_0<TArg1, TArg2, TArg3, TResult> _003C_003Ec__DisplayClass11_ = new _003C_003Ec__DisplayClass11_0<TArg1, TArg2, TArg3, TResult>();
			_003C_003Ec__DisplayClass11_.beginMethod = beginMethod;
			_003C_003Ec__DisplayClass11_.arg1 = arg1;
			_003C_003Ec__DisplayClass11_.arg2 = arg2;
			_003C_003Ec__DisplayClass11_.arg3 = arg3;
			_003C_003Ec__DisplayClass11_.state = state;
			return FromAsync<TResult>(_003C_003Ec__DisplayClass11_._003CFromAsync_003Eb__0, endMethod, _003C_003Ec__DisplayClass11_.state);
		}

		public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			_003C_003Ec__DisplayClass12_0<TArg1, TArg2> _003C_003Ec__DisplayClass12_ = new _003C_003Ec__DisplayClass12_0<TArg1, TArg2>();
			_003C_003Ec__DisplayClass12_.beginMethod = beginMethod;
			_003C_003Ec__DisplayClass12_.arg1 = arg1;
			_003C_003Ec__DisplayClass12_.arg2 = arg2;
			_003C_003Ec__DisplayClass12_.state = state;
			return FromAsync(_003C_003Ec__DisplayClass12_._003CFromAsync_003Eb__0, endMethod, _003C_003Ec__DisplayClass12_.state);
		}

		public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			_003C_003Ec__DisplayClass13_0<TArg1, TArg2, TResult> _003C_003Ec__DisplayClass13_ = new _003C_003Ec__DisplayClass13_0<TArg1, TArg2, TResult>();
			_003C_003Ec__DisplayClass13_.beginMethod = beginMethod;
			_003C_003Ec__DisplayClass13_.arg1 = arg1;
			_003C_003Ec__DisplayClass13_.arg2 = arg2;
			_003C_003Ec__DisplayClass13_.state = state;
			return FromAsync<TResult>(_003C_003Ec__DisplayClass13_._003CFromAsync_003Eb__0, endMethod, _003C_003Ec__DisplayClass13_.state);
		}

		public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state)
		{
			_003C_003Ec__DisplayClass14_0<TArg1> _003C_003Ec__DisplayClass14_ = new _003C_003Ec__DisplayClass14_0<TArg1>();
			_003C_003Ec__DisplayClass14_.beginMethod = beginMethod;
			_003C_003Ec__DisplayClass14_.arg1 = arg1;
			_003C_003Ec__DisplayClass14_.state = state;
			return FromAsync(_003C_003Ec__DisplayClass14_._003CFromAsync_003Eb__0, endMethod, _003C_003Ec__DisplayClass14_.state);
		}

		public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
		{
			_003C_003Ec__DisplayClass15_0<TArg1, TResult> _003C_003Ec__DisplayClass15_ = new _003C_003Ec__DisplayClass15_0<TArg1, TResult>();
			_003C_003Ec__DisplayClass15_.beginMethod = beginMethod;
			_003C_003Ec__DisplayClass15_.arg1 = arg1;
			_003C_003Ec__DisplayClass15_.state = state;
			return FromAsync<TResult>(_003C_003Ec__DisplayClass15_._003CFromAsync_003Eb__0, endMethod, _003C_003Ec__DisplayClass15_.state);
		}

		public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state)
		{
			_003C_003Ec__DisplayClass16_0 _003C_003Ec__DisplayClass16_ = new _003C_003Ec__DisplayClass16_0();
			_003C_003Ec__DisplayClass16_.endMethod = endMethod;
			return FromAsync<int>(beginMethod, _003C_003Ec__DisplayClass16_._003CFromAsync_003Eb__0, state);
		}

		public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			_003C_003Ec__DisplayClass17_0<TResult> _003C_003Ec__DisplayClass17_ = new _003C_003Ec__DisplayClass17_0<TResult>();
			_003C_003Ec__DisplayClass17_.endMethod = endMethod;
			_003C_003Ec__DisplayClass17_.tcs = new TaskCompletionSource<TResult>();
			_003C_003Ec__DisplayClass17_.cancellation = cancellationToken.Register(new Action(_003C_003Ec__DisplayClass17_._003CFromAsync_003Eb__0));
			if (cancellationToken.IsCancellationRequested)
			{
				_003C_003Ec__DisplayClass17_.tcs.TrySetCanceled();
				_003C_003Ec__DisplayClass17_.cancellation.Dispose();
				return _003C_003Ec__DisplayClass17_.tcs.Task;
			}
			try
			{
				beginMethod.Invoke(new AsyncCallback(_003C_003Ec__DisplayClass17_._003CFromAsync_003Eb__1), state);
			}
			catch (System.Exception exception)
			{
				_003C_003Ec__DisplayClass17_.tcs.TrySetException(exception);
				_003C_003Ec__DisplayClass17_.cancellation.Dispose();
			}
			return _003C_003Ec__DisplayClass17_.tcs.Task;
		}

		public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
		{
			_003C_003Ec__DisplayClass18_0 _003C_003Ec__DisplayClass18_ = new _003C_003Ec__DisplayClass18_0();
			_003C_003Ec__DisplayClass18_.tasks = tasks;
			_003C_003Ec__DisplayClass18_.continuationAction = continuationAction;
			_003C_003Ec__DisplayClass18_.remaining = _003C_003Ec__DisplayClass18_.tasks.Length;
			_003C_003Ec__DisplayClass18_.tcs = new TaskCompletionSource<Task[]>();
			if (_003C_003Ec__DisplayClass18_.remaining == 0)
			{
				_003C_003Ec__DisplayClass18_.tcs.TrySetResult(_003C_003Ec__DisplayClass18_.tasks);
			}
			Task[] tasks2 = _003C_003Ec__DisplayClass18_.tasks;
			for (int i = 0; i < tasks2.Length; i++)
			{
				tasks2[i].ContinueWith(_003C_003Ec__DisplayClass18_._003C_003E9__0 ?? (_003C_003Ec__DisplayClass18_._003C_003E9__0 = _003C_003Ec__DisplayClass18_._003CContinueWhenAll_003Eb__0));
			}
			return _003C_003Ec__DisplayClass18_.tcs.Task.ContinueWith(_003C_003Ec__DisplayClass18_._003CContinueWhenAll_003Eb__1);
		}
	}
	internal class TaskFactory<T>
	{
		private readonly TaskFactory factory;

		public TaskScheduler Scheduler
		{
			get
			{
				return factory.Scheduler;
			}
		}

		internal TaskFactory(TaskScheduler scheduler, CancellationToken cancellationToken)
		{
			factory = new TaskFactory(scheduler, cancellationToken);
		}

		public TaskFactory(TaskScheduler scheduler)
			: this(scheduler, CancellationToken.None)
		{
		}

		public TaskFactory(CancellationToken cancellationToken)
			: this(TaskScheduler.FromCurrentSynchronizationContext(), cancellationToken)
		{
		}

		public TaskFactory()
			: this(TaskScheduler.FromCurrentSynchronizationContext(), CancellationToken.None)
		{
		}

		public Task<T> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			return factory.FromAsync<TArg1, TArg2, TArg3, T>(beginMethod, endMethod, arg1, arg2, arg3, state);
		}

		public Task<T> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			return factory.FromAsync<TArg1, TArg2, T>(beginMethod, endMethod, arg1, arg2, state);
		}

		public Task<T> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, TArg1 arg1, object state)
		{
			return factory.FromAsync<TArg1, T>(beginMethod, endMethod, arg1, state);
		}

		public Task<T> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, object state)
		{
			return factory.FromAsync<T>(beginMethod, endMethod, state);
		}
	}
}
