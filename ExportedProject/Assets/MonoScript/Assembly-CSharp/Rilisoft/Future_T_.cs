using System;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public class Future<T>
	{
		private bool _isCompleted;

		private T _result;

		private EventHandler Completed;

		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
		}

		public T Result
		{
			get
			{
				if (!this._isCompleted)
				{
					throw new InvalidOperationException("Future is not completed.");
				}
				return this._result;
			}
		}

		public Future()
		{
		}

		protected void SetResult(T result)
		{
			this._result = result;
			this._isCompleted = true;
			EventHandler completed = this.Completed;
			if (completed != null)
			{
				completed(this, EventArgs.Empty);
			}
		}

		public event EventHandler Completed
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Completed += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Completed -= value;
			}
		}
	}
}