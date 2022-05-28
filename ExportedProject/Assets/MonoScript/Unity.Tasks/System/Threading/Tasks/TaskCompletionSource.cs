using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	public class TaskCompletionSource<T>
	{
		public Task<T> Task
		{
			[CompilerGenerated]
			get
			{
				return _003CTask_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CTask_003Ek__BackingField = value;
			}
		}

		public TaskCompletionSource()
		{
			Task = new Task<T>();
		}

		public bool TrySetResult(T result)
		{
			return Task.TrySetResult(result);
		}

		public bool TrySetException(AggregateException exception)
		{
			return Task.TrySetException(exception);
		}

		public bool TrySetException(System.Exception exception)
		{
			AggregateException ex = exception as AggregateException;
			if (ex != null)
			{
				return Task.TrySetException(ex);
			}
			return Task.TrySetException(new AggregateException(new System.Exception[1] { exception }).Flatten());
		}

		public bool TrySetCanceled()
		{
			return Task.TrySetCanceled();
		}

		public void SetResult(T result)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (!TrySetResult(result))
			{
				throw new InvalidOperationException("Cannot set the result of a completed task.");
			}
		}

		public void SetException(AggregateException exception)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (!TrySetException(exception))
			{
				throw new InvalidOperationException("Cannot set the exception of a completed task.");
			}
		}

		public void SetException(System.Exception exception)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (!TrySetException(exception))
			{
				throw new InvalidOperationException("Cannot set the exception of a completed task.");
			}
		}

		public void SetCanceled()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (!TrySetCanceled())
			{
				throw new InvalidOperationException("Cannot cancel a completed task.");
			}
		}
	}
}
