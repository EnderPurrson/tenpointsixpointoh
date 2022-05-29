using System;

namespace Rilisoft
{
	public class Promise<T>
	{
		private readonly Promise<T>.FutureImpl<T> _future;

		public Future<T> Future
		{
			get
			{
				return this._future;
			}
		}

		public Promise()
		{
		}

		public void SetResult(T result)
		{
			this._future.SetResult(result);
		}

		private class FutureImpl<U> : Future<U>
		{
			public FutureImpl()
			{
			}

			internal void SetResult(U result)
			{
				base.SetResult(result);
			}
		}
	}
}