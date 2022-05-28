using System.IO;
using System.Reflection;
using UnityEngine;

namespace DevToDev.Core.Serialization
{
	public class DataSaverPlatform
	{
		public static string GetDirectoryPath()
		{
			return "devtodev";
		}

		public static void CloseStream(Stream stream)
		{
			stream.Close();
		}

		public static string GetFullPath(string appKey)
		{
			string persistentDataPath = Application.persistentDataPath;
			object obj = persistentDataPath;
			return string.Concat(new object[6]
			{
				obj,
				Path.DirectorySeparatorChar,
				GetDirectoryPath(),
				Path.DirectorySeparatorChar,
				appKey,
				Path.DirectorySeparatorChar
			});
		}

		public static string GetFileName<T>(string fieldName)
		{
			PropertyInfo property = typeof(T).GetProperty(fieldName, (BindingFlags)24);
			return (string)property.GetValue((object)default(object), (object[])default(object[]));
		}
	}
}
