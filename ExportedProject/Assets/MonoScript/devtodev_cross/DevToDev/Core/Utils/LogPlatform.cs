using System.IO;
using UnityEngine;

namespace DevToDev.Core.Utils
{
	public class LogPlatform
	{
		public static string GetDirectoryPath()
		{
			return string.Concat((object)Application.persistentDataPath, (object)Path.DirectorySeparatorChar, (object)"devtodev");
		}

		public static string GetFullPath()
		{
			return string.Concat((object)GetDirectoryPath(), (object)Path.DirectorySeparatorChar);
		}

		public static void CloseStream(Stream stream)
		{
			stream.Close();
		}

		public static bool WriteAndroidLog(string level, string tag, string pstring)
		{
			if (level == null || tag == null || pstring == null)
			{
				return true;
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log");
			androidJavaClass.CallStatic<int>(level, new object[2] { tag, pstring });
			return true;
		}
	}
}
