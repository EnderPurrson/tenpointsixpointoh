using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Tasks.Internal;

namespace System.Threading.Tasks
{
	public abstract class Task
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass20_0<T>
		{
			public TaskCompletionSource<T> tcs;

			public Func<Task, T> continuation;

			public CancellationTokenRegistration cancellation;

			internal void _003CContinueWith_003Eb__0()
			{
				tcs.TrySetCanceled();
			}

			internal void _003CContinueWith_003Eb__1(Task t)
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Expected O, but got Unknown
				_003C_003Ec__DisplayClass20_1<T> _003C_003Ec__DisplayClass20_ = new _003C_003Ec__DisplayClass20_1<T>();
				_003C_003Ec__DisplayClass20_.CS_0024_003C_003E8__locals1 = this;
				_003C_003Ec__DisplayClass20_.t = t;
				immediateExecutor.Invoke(new Action(_003C_003Ec__DisplayClass20_._003CContinueWith_003Eb__2));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass20_1<T>
		{
			public Task t;

			public _003C_003Ec__DisplayClass20_0<T> CS_0024_003C_003E8__locals1;

			internal void _003CContinueWith_003Eb__2()
			{
				try
				{
					CS_0024_003C_003E8__locals1.tcs.TrySetResult(((Func<Task, Task>)(object)CS_0024_003C_003E8__locals1.continuation).Invoke(t));
					CS_0024_003C_003E8__locals1.cancellation.Dispose();
				}
				catch (System.Exception exception)
				{
					CS_0024_003C_003E8__locals1.tcs.TrySetException(exception);
					CS_0024_003C_003E8__locals1.cancellation.Dispose();
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass22_0
		{
			public Action<Task> continuation;

			internal int _003CContinueWith_003Eb__0(Task t)
			{
				continuation.Invoke(t);
				return 0;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass24_0
		{
			public Task[] taskArr;

			public TaskCompletionSource<int> tcs;

			internal void _003CWhenAll_003Eb__0(Task[] _)
			{
				AggregateException[] array = Enumerable.ToArray<AggregateException>(Enumerable.Select<Task, AggregateException>(Enumerable.Where<Task>((System.Collections.Generic.IEnumerable<Task>)taskArr, _003C_003Ec._003C_003E9__24_1 ?? (_003C_003Ec._003C_003E9__24_1 = _003C_003Ec._003C_003E9._003CWhenAll_003Eb__24_1)), _003C_003Ec._003C_003E9__24_2 ?? (_003C_003Ec._003C_003E9__24_2 = _003C_003Ec._003C_003E9._003CWhenAll_003Eb__24_2)));
				if (array.Length != 0)
				{
					tcs.SetException(new AggregateException(array));
				}
				else if (Enumerable.Any<Task>((System.Collections.Generic.IEnumerable<Task>)taskArr, _003C_003Ec._003C_003E9__24_3 ?? (_003C_003Ec._003C_003E9__24_3 = _003C_003Ec._003C_003E9._003CWhenAll_003Eb__24_3)))
				{
					tcs.SetCanceled();
				}
				else
				{
					tcs.SetResult(0);
				}
			}
		}

		[Serializable]
		[CompilerGenerated]
		private sealed class _003C_003Ec
		{
			public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

			public static Func<Task, bool> _003C_003E9__24_1;

			public static Func<Task, AggregateException> _003C_003E9__24_2;

			public static Func<Task, bool> _003C_003E9__24_3;

			internal bool _003CWhenAll_003Eb__24_1(Task t)
			{
				return t.IsFaulted;
			}

			internal AggregateException _003CWhenAll_003Eb__24_2(Task t)
			{
				return t.Exception;
			}

			internal bool _003CWhenAll_003Eb__24_3(Task t)
			{
				return t.IsCanceled;
			}

			internal int _003C_002Ecctor_003Eb__32_0()
			{
				return 0;
			}

			internal void _003C_002Ecctor_003Eb__32_1(Action a)
			{
				bool num = AppDomain.get_CurrentDomain().get_FriendlyName().Equals("IL2CPP Root Domain");
				int num2 = 10;
				if (num)
				{
					num2 = 200;
				}
				executionDepth.Value++;
				try
				{
					if (executionDepth.Value <= num2)
					{
						a.Invoke();
					}
					else
					{
						Factory.Scheduler.Post(a);
					}
				}
				finally
				{
					executionDepth.Value--;
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass26_0
		{
			public TaskCompletionSource<Task> tcs;

			public Func<Task, bool> _003C_003E9__0;

			internal bool _003CWhenAny_003Eb__0(Task t)
			{
				return tcs.TrySetResult(t);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass27_0<T>
		{
			public System.Collections.Generic.IEnumerable<Task<T>> tasks;

			internal T[] _003CWhenAll_003Eb__0(Task _)
			{
				return Enumerable.ToArray<T>(Enumerable.Select<Task<T>, T>(tasks, _003C_003Ec__27<T>._003C_003E9__27_1 ?? (_003C_003Ec__27<T>._003C_003E9__27_1 = (Func<Task<T>, T>)(object)new Func<Task<Task<T>>, Task<T>>(_003C_003Ec__27<T>._003C_003E9._003CWhenAll_003Eb__27_1))));
			}
		}

		[Serializable]
		[CompilerGenerated]
		private sealed class _003C_003Ec__27<T>
		{
			public static readonly _003C_003Ec__27<T> _003C_003E9 = new _003C_003Ec__27<T>();

			public static Func<Task<T>, T> _003C_003E9__27_1;

			internal T _003CWhenAll_003Eb__27_1(Task<T> t)
			{
				return t.Result;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass30_0
		{
			public Action toRun;

			internal int _003CRun_003Eb__0()
			{
				toRun.Invoke();
				return 0;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass31_0
		{
			public TaskCompletionSource<int> tcs;

			internal void _003CDelay_003Eb__0(object _)
			{
				tcs.TrySetResult(0);
			}
		}

		private static readonly ThreadLocal<int> executionDepth = new ThreadLocal<int>(_003C_003Ec._003C_003E9._003C_002Ecctor_003Eb__32_0);

		private static readonly Action<Action> immediateExecutor = _003C_003Ec._003C_003E9._003C_002Ecctor_003Eb__32_1;

		internal readonly object mutex = new object();

		internal System.Collections.Generic.IList<Action<Task>> continuations = (System.Collections.Generic.IList<Action<Task>>)new List<Action<Task>>();

		internal AggregateException exception;

		internal bool isCanceled;

		internal bool isCompleted;

		public static TaskFactory Factory
		{
			get
			{
				return new TaskFactory();
			}
		}

		public AggregateException Exception
		{
			get
			{
				lock (mutex)
				{
					return exception;
				}
			}
		}

		public bool IsCanceled
		{
			get
			{
				lock (mutex)
				{
					return isCanceled;
				}
			}
		}

		public bool IsCompleted
		{
			get
			{
				lock (mutex)
				{
					return isCompleted;
				}
			}
		}

		public bool IsFaulted
		{
			get
			{
				return Exception != null;
			}
		}

		internal Task()
		{
		}

		public void Wait()
		{
			lock (mutex)
			{
				if (!IsCompleted)
				{
					Monitor.Wait(mutex);
				}
				if (IsFaulted)
				{
					throw Exception;
				}
			}
		}

		public Task<T> ContinueWith<T>(Func<Task, T> continuation)
		{
			return ContinueWith<T>(continuation, CancellationToken.None);
		}

		public Task<T> ContinueWith<T>(Func<Task, T> continuation, CancellationToken cancellationToken)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			_003C_003Ec__DisplayClass20_0<T> _003C_003Ec__DisplayClass20_ = new _003C_003Ec__DisplayClass20_0<T>();
			_003C_003Ec__DisplayClass20_.continuation = continuation;
			bool flag = false;
			_003C_003Ec__DisplayClass20_.tcs = new TaskCompletionSource<T>();
			_003C_003Ec__DisplayClass20_.cancellation = cancellationToken.Register(new Action(_003C_003Ec__DisplayClass20_._003CContinueWith_003Eb__0));
			Action<Task> val = _003C_003Ec__DisplayClass20_._003CContinueWith_003Eb__1;
			lock (mutex)
			{
				flag = IsCompleted;
				if (!flag)
				{
					((System.Collections.Generic.ICollection<Action<Task>>)continuations).Add(val);
				}
			}
			if (flag)
			{
				val.Invoke(this);
			}
			return _003C_003Ec__DisplayClass20_.tcs.Task;
		}

		public Task ContinueWith(Action<Task> continuation)
		{
			return ContinueWith(continuation, CancellationToken.None);
		}

		public Task ContinueWith(Action<Task> continuation, CancellationToken cancellationToken)
		{
			_003C_003Ec__DisplayClass22_0 _003C_003Ec__DisplayClass22_ = new _003C_003Ec__DisplayClass22_0();
			_003C_003Ec__DisplayClass22_.continuation = continuation;
			return ContinueWith<int>(_003C_003Ec__DisplayClass22_._003CContinueWith_003Eb__0, cancellationToken);
		}

		public static Task WhenAll(params Task[] tasks)
		{
			return WhenAll((System.Collections.Generic.IEnumerable<Task>)tasks);
		}

		public static Task WhenAll(System.Collections.Generic.IEnumerable<Task> tasks)
		{
			_003C_003Ec__DisplayClass24_0 _003C_003Ec__DisplayClass24_ = new _003C_003Ec__DisplayClass24_0();
			_003C_003Ec__DisplayClass24_.taskArr = Enumerable.ToArray<Task>(tasks);
			if (_003C_003Ec__DisplayClass24_.taskArr.Length == 0)
			{
				return FromResult(0);
			}
			_003C_003Ec__DisplayClass24_.tcs = new TaskCompletionSource<int>();
			Factory.ContinueWhenAll(_003C_003Ec__DisplayClass24_.taskArr, _003C_003Ec__DisplayClass24_._003CWhenAll_003Eb__0);
			return _003C_003Ec__DisplayClass24_.tcs.Task;
		}

		internal static Task<Task> WhenAny(params Task[] tasks)
		{
			return WhenAny((System.Collections.Generic.IEnumerable<Task>)tasks);
		}

		internal static Task<Task> WhenAny(System.Collections.Generic.IEnumerable<Task> tasks)
		{
			_003C_003Ec__DisplayClass26_0 _003C_003Ec__DisplayClass26_ = new _003C_003Ec__DisplayClass26_0();
			_003C_003Ec__DisplayClass26_.tcs = new TaskCompletionSource<Task>();
			System.Collections.Generic.IEnumerator<Task> enumerator = tasks.GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					enumerator.get_Current().ContinueWith<bool>(_003C_003Ec__DisplayClass26_._003C_003E9__0 ?? (_003C_003Ec__DisplayClass26_._003C_003E9__0 = _003C_003Ec__DisplayClass26_._003CWhenAny_003Eb__0));
				}
			}
			finally
			{
				if (enumerator != null)
				{
					((System.IDisposable)enumerator).Dispose();
				}
			}
			return _003C_003Ec__DisplayClass26_.tcs.Task;
		}

		public static Task<T[]> WhenAll<T>(System.Collections.Generic.IEnumerable<Task<T>> tasks)
		{
			_003C_003Ec__DisplayClass27_0<T> _003C_003Ec__DisplayClass27_ = new _003C_003Ec__DisplayClass27_0<T>();
			_003C_003Ec__DisplayClass27_.tasks = tasks;
			return WhenAll(Enumerable.Cast<Task>((System.Collections.IEnumerable)_003C_003Ec__DisplayClass27_.tasks)).OnSuccess<T[]>(_003C_003Ec__DisplayClass27_._003CWhenAll_003Eb__0);
		}

		public static Task<T> FromResult<T>(T result)
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetResult(result);
			return taskCompletionSource.Task;
		}

		public static Task<T> Run<T>(Func<T> toRun)
		{
			return Factory.StartNew<T>(toRun);
		}

		public static Task Run(Action toRun)
		{
			_003C_003Ec__DisplayClass30_0 _003C_003Ec__DisplayClass30_ = new _003C_003Ec__DisplayClass30_0();
			_003C_003Ec__DisplayClass30_.toRun = toRun;
			return Factory.StartNew<int>(_003C_003Ec__DisplayClass30_._003CRun_003Eb__0);
		}

		public static Task Delay(TimeSpan timespan)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			_003C_003Ec__DisplayClass31_0 obj = new _003C_003Ec__DisplayClass31_0
			{
				tcs = new TaskCompletionSource<int>()
			};
			new Timer(new TimerCallback(obj._003CDelay_003Eb__0)).Change(timespan, TimeSpan.FromMilliseconds(-1.0));
			return obj.tcs.Task;
		}
	}
	public sealed class Task<T> : Task
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass4_0
		{
			public Action<Task<T>> continuation;

			internal void _003CContinueWith_003Eb__0(Task t)
			{
				((Action<Task<Task<T>>>)(object)continuation).Invoke((Task<Task<T>>)(object)(Task<T>)t);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass5_0<TResult>
		{
			public Func<Task<T>, TResult> continuation;

			internal TResult _003CContinueWith_003Eb__0(Task t)
			{
				return ((Func<Task<Task<T>>, TResult>)(object)continuation).Invoke((Task<Task<T>>)(object)(Task<T>)t);
			}
		}

		private T result;

		public T Result
		{
			get
			{
				Wait();
				return result;
			}
		}

		internal Task()
		{
		}

		public Task ContinueWith(Action<Task<T>> continuation)
		{
			_003C_003Ec__DisplayClass4_0 _003C_003Ec__DisplayClass4_ = new _003C_003Ec__DisplayClass4_0();
			_003C_003Ec__DisplayClass4_.continuation = continuation;
			return base.ContinueWith((Action<Task>)_003C_003Ec__DisplayClass4_._003CContinueWith_003Eb__0);
		}

		public Task<TResult> ContinueWith<TResult>(Func<Task<T>, TResult> continuation)
		{
			_003C_003Ec__DisplayClass5_0<TResult> _003C_003Ec__DisplayClass5_ = new _003C_003Ec__DisplayClass5_0<TResult>();
			_003C_003Ec__DisplayClass5_.continuation = continuation;
			return base.ContinueWith<TResult>((Func<Task, TResult>)_003C_003Ec__DisplayClass5_._003CContinueWith_003Eb__0);
		}

		private void RunContinuations()
		{
			lock (mutex)
			{
				System.Collections.Generic.IEnumerator<Action<Task>> enumerator = ((System.Collections.Generic.IEnumerable<Action<Task>>)continuations).GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						enumerator.get_Current().Invoke((Task)this);
					}
				}
				finally
				{
					if (enumerator != null)
					{
						((System.IDisposable)enumerator).Dispose();
					}
				}
				continuations = null;
			}
		}

		internal bool TrySetResult(T result)
		{
			lock (mutex)
			{
				if (isCompleted)
				{
					return false;
				}
				isCompleted = true;
				this.result = result;
				Monitor.PulseAll(mutex);
				RunContinuations();
				return true;
			}
		}

		internal bool TrySetCanceled()
		{
			lock (mutex)
			{
				if (isCompleted)
				{
					return false;
				}
				isCompleted = true;
				isCanceled = true;
				Monitor.PulseAll(mutex);
				RunContinuations();
				return true;
			}
		}

		internal bool TrySetException(AggregateException exception)
		{
			lock (mutex)
			{
				if (isCompleted)
				{
					return false;
				}
				isCompleted = true;
				base.exception = exception;
				Monitor.PulseAll(mutex);
				RunContinuations();
				return true;
			}
		}
	}
}
