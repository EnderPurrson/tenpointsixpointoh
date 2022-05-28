using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
	public class Progress<T> : IProgress<T> where T : EventArgs
	{
		private SynchronizationContext synchronizationContext;

		private SendOrPostCallback synchronizationCallback;

		private Action<T> eventHandler;

		public event EventHandler<T> ProgressChanged
		{
			[CompilerGenerated]
			add
			{
				EventHandler<T> val = this.ProgressChanged;
				EventHandler<T> val2;
				do
				{
					val2 = val;
					EventHandler<T> val3 = (EventHandler<T>)(object)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<T>>(ref this.ProgressChanged, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<T> val = this.ProgressChanged;
				EventHandler<T> val2;
				do
				{
					val2 = val;
					EventHandler<T> val3 = (EventHandler<T>)(object)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<T>>(ref this.ProgressChanged, val3, val2);
				}
				while (val != val2);
			}
		}

		public Progress()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			synchronizationContext = SynchronizationContext.get_Current() ?? ProgressSynchronizationContext.SharedContext;
			synchronizationCallback = new SendOrPostCallback(NotifyDelegates);
		}

		public Progress(Action<T> handler)
			: this()
		{
			eventHandler = handler;
		}

		void IProgress<T>.Report(T value)
		{
			OnReport(value);
		}

		protected virtual void OnReport(T value)
		{
			synchronizationContext.Post(synchronizationCallback, (object)value);
		}

		private void NotifyDelegates(object newValue)
		{
			T val = (T)newValue;
			Action<T> val2 = eventHandler;
			EventHandler<T> progressChanged = this.ProgressChanged;
			if (val2 != null)
			{
				val2.Invoke(val);
			}
			if (progressChanged != null)
			{
				progressChanged.Invoke((object)this, val);
			}
		}
	}
}
