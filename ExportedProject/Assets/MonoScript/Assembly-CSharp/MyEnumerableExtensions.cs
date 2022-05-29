using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class MyEnumerableExtensions
{
	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
	{
		Random random = new Random();
		return 
			from  in source
			orderby random.Next()
			select ;
	}
}