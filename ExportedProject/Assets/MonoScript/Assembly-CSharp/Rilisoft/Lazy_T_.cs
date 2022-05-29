using System;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	internal class Lazy<T>
	{
		private static Func<T> ALREADY_INVOKED_SENTINEL;

		private object m_boxed;

		private Func<T> m_valueFactory;

		private volatile object m_threadSafeObj;

		public T Value
		{
			get
			{
				Lazy<T>.Boxed mBoxed = null;
				if (this.m_boxed == null)
				{
					return this.LazyInitValue();
				}
				mBoxed = this.m_boxed as Lazy<T>.Boxed;
				if (mBoxed == null)
				{
					throw this.m_boxed as Exception;
				}
				return mBoxed.m_value;
			}
		}

		static Lazy()
		{
			Lazy<T>.ALREADY_INVOKED_SENTINEL = () => default(T);
		}

		public Lazy(Func<T> valueFactory)
		{
			this.m_threadSafeObj = new object();
			this.m_valueFactory = valueFactory;
		}

		private Lazy<T>.Boxed CreateValue()
		{
			Lazy<T>.Boxed boxed = null;
			try
			{
				if (this.m_valueFactory == Lazy<T>.ALREADY_INVOKED_SENTINEL)
				{
					throw new InvalidOperationException();
				}
				Func<T> mValueFactory = this.m_valueFactory;
				this.m_valueFactory = Lazy<T>.ALREADY_INVOKED_SENTINEL;
				boxed = new Lazy<T>.Boxed(mValueFactory());
			}
			catch (Exception exception)
			{
				this.m_boxed = exception;
				throw;
			}
			return boxed;
		}

		private T LazyInitValue()
		{
			Lazy<T>.Boxed mBoxed = null;
			object mThreadSafeObj = this.m_threadSafeObj;
			try
			{
				mThreadSafeObj == Lazy<T>.ALREADY_INVOKED_SENTINEL;
				if (this.m_boxed != null)
				{
					mBoxed = this.m_boxed as Lazy<T>.Boxed;
					if (mBoxed == null)
					{
						throw this.m_boxed as Exception;
					}
				}
				else
				{
					mBoxed = this.CreateValue();
					this.m_boxed = mBoxed;
					this.m_threadSafeObj = Lazy<T>.ALREADY_INVOKED_SENTINEL;
				}
			}
			finally
			{
			}
			return mBoxed.m_value;
		}

		private class Boxed
		{
			internal T m_value;

			internal Boxed(T value)
			{
				this.m_value = value;
			}
		}
	}
}