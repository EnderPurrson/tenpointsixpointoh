using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	public class TaskScheduler
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass3_0
		{
			public Action action;

			internal void _003CPost_003Eb__0(object o)
			{
				action.Invoke();
			}
		}

		private static SynchronizationContext defaultContext = new SynchronizationContext();

		private SynchronizationContext context;

		public TaskScheduler(SynchronizationContext context)
		{
			this.context = context ?? defaultContext;
		}

		public void Post(Action action)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			_003C_003Ec__DisplayClass3_0 _003C_003Ec__DisplayClass3_ = new _003C_003Ec__DisplayClass3_0();
			_003C_003Ec__DisplayClass3_.action = action;
			context.Post(new SendOrPostCallback(_003C_003Ec__DisplayClass3_._003CPost_003Eb__0), (object)default(object));
		}

		public static TaskScheduler FromCurrentSynchronizationContext()
		{
			return new TaskScheduler(SynchronizationContext.get_Current());
		}
	}
}
