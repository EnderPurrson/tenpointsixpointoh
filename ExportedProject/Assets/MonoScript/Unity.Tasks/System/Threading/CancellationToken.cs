namespace System.Threading
{
	public struct CancellationToken
	{
		private CancellationTokenSource source;

		public static CancellationToken None
		{
			get
			{
				return default(CancellationToken);
			}
		}

		public bool IsCancellationRequested
		{
			get
			{
				if (source != null)
				{
					return source.IsCancellationRequested;
				}
				return false;
			}
		}

		internal CancellationToken(CancellationTokenSource source)
		{
			this.source = source;
		}

		public CancellationTokenRegistration Register(Action callback)
		{
			if (source != null)
			{
				return source.Register(callback);
			}
			return default(CancellationTokenRegistration);
		}

		public void ThrowIfCancellationRequested()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (IsCancellationRequested)
			{
				throw new OperationCanceledException();
			}
		}
	}
}
