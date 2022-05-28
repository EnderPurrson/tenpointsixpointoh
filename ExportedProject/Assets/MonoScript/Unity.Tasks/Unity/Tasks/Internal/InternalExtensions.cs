using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Unity.Tasks.Internal
{
	internal static class InternalExtensions
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass0_0<TIn, TResult>
		{
			public Func<Task<TIn>, TResult> continuation;

			internal TResult _003COnSuccess_003Eb__0(Task t)
			{
				return ((Func<Task<Task<TIn>>, TResult>)(object)continuation).Invoke((Task<Task<TIn>>)(object)(Task<TIn>)t);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass1_0<TIn>
		{
			public Action<Task<TIn>> continuation;

			internal object _003COnSuccess_003Eb__0(Task<TIn> t)
			{
				((Action<Task<Task<TIn>>>)(object)continuation).Invoke((Task<Task<TIn>>)(object)t);
				return null;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2_0<TResult>
		{
			public Func<Task, TResult> continuation;

			internal Task<TResult> _003COnSuccess_003Eb__0(Task t)
			{
				if (t.IsFaulted)
				{
					AggregateException ex = t.Exception.Flatten();
					if (ex.InnerExceptions.get_Count() == 1)
					{
						ExceptionDispatchInfo.Capture(ex.InnerExceptions.get_Item(0)).Throw();
					}
					else
					{
						ExceptionDispatchInfo.Capture(ex).Throw();
					}
					return Task.FromResult(default(TResult));
				}
				if (t.IsCanceled)
				{
					TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
					taskCompletionSource.SetCanceled();
					return taskCompletionSource.Task;
				}
				return Task.FromResult(((Func<Task, Task>)(object)continuation).Invoke(t));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass3_0
		{
			public Action<Task> continuation;

			internal object _003COnSuccess_003Eb__0(Task t)
			{
				continuation.Invoke(t);
				return null;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass4_0
		{
			public Func<Task<bool>> predicate;

			public Func<Task> body;

			public Func<Task> iterate;

			public Func<Task, Task> _003C_003E9__2;

			public Func<Task<bool>, Task> _003C_003E9__1;

			internal Task _003CWhileAsync_003Eb__0()
			{
				return predicate.Invoke().OnSuccess<bool, Task>(_003C_003E9__1 ?? (_003C_003E9__1 = _003CWhileAsync_003Eb__1)).Unwrap();
			}

			internal Task _003CWhileAsync_003Eb__1(Task<bool> t)
			{
				if (!t.Result)
				{
					return Task.FromResult(0);
				}
				return body.Invoke().OnSuccess<Task>(_003C_003E9__2 ?? (_003C_003E9__2 = _003CWhileAsync_003Eb__2)).Unwrap();
			}

			internal Task _003CWhileAsync_003Eb__2(Task _)
			{
				return iterate.Invoke();
			}
		}

		internal static Task<TResult> OnSuccess<TIn, TResult>(this Task<TIn> task, Func<Task<TIn>, TResult> continuation)
		{
			_003C_003Ec__DisplayClass0_0<TIn, TResult> _003C_003Ec__DisplayClass0_ = new _003C_003Ec__DisplayClass0_0<TIn, TResult>();
			_003C_003Ec__DisplayClass0_.continuation = continuation;
			return task.OnSuccess<TResult>(_003C_003Ec__DisplayClass0_._003COnSuccess_003Eb__0);
		}

		internal static Task OnSuccess<TIn>(this Task<TIn> task, Action<Task<TIn>> continuation)
		{
			_003C_003Ec__DisplayClass1_0<TIn> _003C_003Ec__DisplayClass1_ = new _003C_003Ec__DisplayClass1_0<TIn>();
			_003C_003Ec__DisplayClass1_.continuation = continuation;
			return task.OnSuccess<TIn, object>(_003C_003Ec__DisplayClass1_._003COnSuccess_003Eb__0);
		}

		internal static Task<TResult> OnSuccess<TResult>(this Task task, Func<Task, TResult> continuation)
		{
			_003C_003Ec__DisplayClass2_0<TResult> _003C_003Ec__DisplayClass2_ = new _003C_003Ec__DisplayClass2_0<TResult>();
			_003C_003Ec__DisplayClass2_.continuation = continuation;
			return task.ContinueWith<Task<TResult>>(_003C_003Ec__DisplayClass2_._003COnSuccess_003Eb__0).Unwrap();
		}

		internal static Task OnSuccess(this Task task, Action<Task> continuation)
		{
			_003C_003Ec__DisplayClass3_0 _003C_003Ec__DisplayClass3_ = new _003C_003Ec__DisplayClass3_0();
			_003C_003Ec__DisplayClass3_.continuation = continuation;
			return task.OnSuccess<object>(_003C_003Ec__DisplayClass3_._003COnSuccess_003Eb__0);
		}

		internal static Task WhileAsync(Func<Task<bool>> predicate, Func<Task> body)
		{
			_003C_003Ec__DisplayClass4_0 obj = new _003C_003Ec__DisplayClass4_0
			{
				predicate = predicate,
				body = body,
				iterate = null
			};
			obj.iterate = obj._003CWhileAsync_003Eb__0;
			return obj.iterate.Invoke();
		}
	}
}
