using System.Collections.Generic;

namespace Prime31
{
	public static class StringExtensions
	{
		public static Dictionary<string, string> parseQueryString(this string self)
		{
			Dictionary<string, string> val = new Dictionary<string, string>();
			string[] array = null;
			string[] array2 = self.Split(new char[1] { '?' });
			array = ((array2.Length == 2) ? array2[1].Split(new char[1] { '&' }) : self.Split(new char[1] { '&' }));
			string[] array3 = array;
			foreach (string text in array3)
			{
				string[] array4 = text.Split(new char[1] { '=' });
				val.Add(array4[0], array4[1]);
			}
			return val;
		}
	}
}
