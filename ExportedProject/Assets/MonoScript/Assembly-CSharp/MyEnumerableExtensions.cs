using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public static class MyEnumerableExtensions
{
	[CompilerGenerated]
	private sealed class _003CRandomize_003Ec__AnonStorey33C<T>
	{
		internal Random rnd;

		internal int _003C_003Em__530(T item)
		{
			return rnd.Next();
		}
	}

	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
	{
		_003CRandomize_003Ec__AnonStorey33C<T> _003CRandomize_003Ec__AnonStorey33C = new _003CRandomize_003Ec__AnonStorey33C<T>();
		_003CRandomize_003Ec__AnonStorey33C.rnd = new Random();
		return source.OrderBy(_003CRandomize_003Ec__AnonStorey33C._003C_003Em__530);
	}
}
