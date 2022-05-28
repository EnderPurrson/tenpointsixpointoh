using System.Runtime.CompilerServices;

namespace System
{
	public static class Tuple
	{
		public static Tuple<T1, T2> Create<T1, T2>(T1 t1, T2 t2)
		{
			return new Tuple<T1, T2>(t1, t2);
		}
	}
	public class Tuple<T1, T2>
	{
		public T1 Item1
		{
			[CompilerGenerated]
			get
			{
				return _003CItem1_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CItem1_003Ek__BackingField = value;
			}
		}

		public T2 Item2
		{
			[CompilerGenerated]
			get
			{
				return _003CItem2_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CItem2_003Ek__BackingField = value;
			}
		}

		public Tuple(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public override bool Equals(object obj)
		{
			Tuple<T1, T2> tuple = obj as Tuple<T1, T2>;
			if (tuple == null)
			{
				return false;
			}
			if (object.Equals((object)Item1, (object)tuple.Item1))
			{
				return object.Equals((object)Item2, (object)tuple.Item2);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = ((Item1 != null) ? ((object)Item1).GetHashCode() : 0);
			int num2 = ((Item2 != null) ? ((object)Item2).GetHashCode() : 0);
			return num ^ num2;
		}
	}
}
