using System.Runtime.CompilerServices;

namespace System.Runtime.ExceptionServices
{
	public class ExceptionDispatchInfo
	{
		public System.Exception SourceException
		{
			[CompilerGenerated]
			get
			{
				return _003CSourceException_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CSourceException_003Ek__BackingField = value;
			}
		}

		public static ExceptionDispatchInfo Capture(System.Exception ex)
		{
			return new ExceptionDispatchInfo(ex);
		}

		private ExceptionDispatchInfo(System.Exception ex)
		{
			SourceException = ex;
		}

		public void Throw()
		{
			throw SourceException;
		}
	}
}
