using System;

namespace Rilisoft
{
	internal sealed class ActionDisposable : IDisposable
	{
		private readonly Action _action;

		private bool _disposed;

		public ActionDisposable(Action action)
		{
			this._action = action;
		}

		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			if (this._action != null)
			{
				this._action();
			}
			this._disposed = true;
		}
	}
}