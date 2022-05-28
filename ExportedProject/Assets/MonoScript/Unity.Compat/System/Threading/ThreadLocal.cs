using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Threading
{
	public class ThreadLocal<T> : System.IDisposable
	{
		[Serializable]
		[CompilerGenerated]
		private sealed class _003C_003Ec
		{
			public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

			public static Func<T> _003C_003E9__8_0;

			internal T _003C_002Ector_003Eb__8_0()
			{
				return default(T);
			}
		}

		private static long lastId = -1L;

		[ThreadStatic]
		private static IDictionary<long, T> threadLocalData;

		private static System.Collections.Generic.IList<WeakReference> allDataDictionaries = (System.Collections.Generic.IList<WeakReference>)new List<WeakReference>();

		private bool disposed;

		private readonly long id;

		private readonly Func<T> valueFactory;

		private static IDictionary<long, T> ThreadLocalData
		{
			get
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Expected O, but got Unknown
				if (threadLocalData == null)
				{
					threadLocalData = (IDictionary<long, T>)(object)new Dictionary<long, long>();
					lock (allDataDictionaries)
					{
						((System.Collections.Generic.ICollection<WeakReference>)allDataDictionaries).Add(new WeakReference((object)threadLocalData));
					}
				}
				return threadLocalData;
			}
		}

		public unsafe T Value
		{
			get
			{
				//IL_0039: Expected I8, but got O
				CheckDisposed();
				T result = default(T);
				if (((IDictionary<long, long>)(object)ThreadLocalData).TryGetValue(id, ref *(long*)(&result)))
				{
					return result;
				}
				T result2;
				((IDictionary<long, long>)(object)ThreadLocalData).set_Item(id, (long)(result2 = valueFactory.Invoke()));
				return result2;
			}
			set
			{
				//IL_0017: Expected I8, but got O
				CheckDisposed();
				((IDictionary<long, long>)(object)ThreadLocalData).set_Item(id, (long)value);
			}
		}

		public ThreadLocal()
			: this(_003C_003Ec._003C_003E9__8_0 ?? (_003C_003Ec._003C_003E9__8_0 = _003C_003Ec._003C_003E9._003C_002Ector_003Eb__8_0))
		{
		}

		public ThreadLocal(Func<T> valueFactory)
		{
			this.valueFactory = valueFactory;
			id = Interlocked.Increment(ref lastId);
		}

		~ThreadLocal()
		{
			if (!disposed)
			{
				Dispose();
			}
		}

		private void CheckDisposed()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (disposed)
			{
				throw new ObjectDisposedException("ThreadLocal has been disposed.");
			}
		}

		public void Dispose()
		{
			lock (allDataDictionaries)
			{
				for (int i = 0; i < ((System.Collections.Generic.ICollection<WeakReference>)allDataDictionaries).get_Count(); i++)
				{
					IDictionary<object, T> val = allDataDictionaries.get_Item(i).get_Target() as IDictionary<object, T>;
					if (val == null)
					{
						allDataDictionaries.RemoveAt(i);
						i--;
					}
					else
					{
						((IDictionary<object, object>)(object)val).Remove((object)id);
					}
				}
			}
			disposed = true;
		}
	}
}
