using System.Collections.Generic;

namespace System.Threading
{
	public sealed class CancellationTokenSource
	{
		private object mutex = new object();

		private Action actions;

		private bool isCancellationRequested;

		internal bool IsCancellationRequested
		{
			get
			{
				lock (mutex)
				{
					return isCancellationRequested;
				}
			}
		}

		public CancellationToken Token
		{
			get
			{
				return new CancellationToken(this);
			}
		}

		internal CancellationTokenRegistration Register(Action action)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			lock (mutex)
			{
				actions = (Action)System.Delegate.Combine((System.Delegate)(object)actions, (System.Delegate)(object)action);
				return new CancellationTokenRegistration(this, action);
			}
		}

		internal void Unregister(Action action)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			lock (mutex)
			{
				actions = (Action)System.Delegate.Remove((System.Delegate)(object)actions, (System.Delegate)(object)action);
			}
		}

		public void Cancel()
		{
			Cancel(false);
		}

		public void Cancel(bool throwOnFirstException)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			lock (mutex)
			{
				isCancellationRequested = true;
				if (actions == null)
				{
					return;
				}
				try
				{
					if (throwOnFirstException)
					{
						actions.Invoke();
						return;
					}
					System.Delegate[] invocationList = ((System.Delegate)(object)actions).GetInvocationList();
					foreach (System.Delegate @delegate in invocationList)
					{
						List<System.Exception> val = new List<System.Exception>();
						try
						{
							((Action)@delegate).Invoke();
						}
						catch (System.Exception ex)
						{
							val.Add(ex);
						}
						if (val.get_Count() > 0)
						{
							throw new AggregateException((System.Collections.Generic.IEnumerable<System.Exception>)val);
						}
					}
				}
				finally
				{
					actions = null;
				}
			}
		}
	}
}
