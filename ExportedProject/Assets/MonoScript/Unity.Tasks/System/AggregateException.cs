using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	public class AggregateException : System.Exception
	{
		public ReadOnlyCollection<System.Exception> InnerExceptions
		{
			[CompilerGenerated]
			get
			{
				return _003CInnerExceptions_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CInnerExceptions_003Ek__BackingField = value;
			}
		}

		public AggregateException(System.Collections.Generic.IEnumerable<System.Exception> innerExceptions)
		{
			InnerExceptions = new ReadOnlyCollection<System.Exception>((System.Collections.Generic.IList<System.Exception>)Enumerable.ToList<System.Exception>(innerExceptions));
		}

		public AggregateException Flatten()
		{
			List<System.Exception> val = new List<System.Exception>();
			System.Collections.Generic.IEnumerator<System.Exception> enumerator = InnerExceptions.GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					System.Exception current = enumerator.get_Current();
					AggregateException ex = current as AggregateException;
					if (ex != null)
					{
						val.AddRange((System.Collections.Generic.IEnumerable<System.Exception>)ex.Flatten().InnerExceptions);
					}
					else
					{
						val.Add(current);
					}
				}
			}
			finally
			{
				if (enumerator != null)
				{
					((System.IDisposable)enumerator).Dispose();
				}
			}
			return new AggregateException((System.Collections.Generic.IEnumerable<System.Exception>)val);
		}

		public override string ToString()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			StringBuilder val = new StringBuilder(base.ToString());
			System.Collections.Generic.IEnumerator<System.Exception> enumerator = InnerExceptions.GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					System.Exception current = enumerator.get_Current();
					val.AppendLine("\n-----------------");
					val.AppendLine(((object)current).ToString());
				}
			}
			finally
			{
				if (enumerator != null)
				{
					((System.IDisposable)enumerator).Dispose();
				}
			}
			return ((object)val).ToString();
		}
	}
}
