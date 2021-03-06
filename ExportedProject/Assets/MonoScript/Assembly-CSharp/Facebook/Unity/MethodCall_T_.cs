using System;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal abstract class MethodCall<T>
	where T : IResult
	{
		public FacebookDelegate<T> Callback
		{
			protected get;
			set;
		}

		protected FacebookBase FacebookImpl
		{
			get;
			set;
		}

		public string MethodName
		{
			get;
			private set;
		}

		protected MethodArguments Parameters
		{
			get;
			set;
		}

		public MethodCall(FacebookBase facebookImpl, string methodName)
		{
			this.Parameters = new MethodArguments();
			this.FacebookImpl = facebookImpl;
			this.MethodName = methodName;
		}

		public abstract void Call(MethodArguments args = null);
	}
}