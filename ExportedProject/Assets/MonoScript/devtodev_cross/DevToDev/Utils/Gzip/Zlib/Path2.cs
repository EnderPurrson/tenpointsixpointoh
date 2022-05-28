using System;
using System.Runtime.CompilerServices;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal static class Path2
	{
		internal const int MAX_PATH = 260;

		internal const int MAX_DIRECTORY_PATH = 248;

		public static readonly char DirectorySeparatorChar;

		public static readonly char AltDirectorySeparatorChar;

		public static readonly char VolumeSeparatorChar;

		public static readonly char[] InvalidPathChars;

		internal static readonly char[] TrimEndChars;

		private static readonly char[] RealInvalidPathChars;

		private static readonly char[] InvalidFileNameChars;

		public static readonly char PathSeparator;

		internal static readonly int MaxPath;

		private static readonly int MaxDirectoryLength;

		internal static readonly int MaxLongPath;

		private static readonly string Prefix;

		private static readonly char[] s_Base32Char;

		static Path2()
		{
			DirectorySeparatorChar = '\\';
			AltDirectorySeparatorChar = '/';
			VolumeSeparatorChar = ':';
			char[] array = new char[36];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			char[] array2 = (InvalidPathChars = array);
			char[] array3 = new char[8];
			RuntimeHelpers.InitializeArray((global::System.Array)array3, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			char[] array4 = (TrimEndChars = array3);
			char[] array5 = new char[36];
			RuntimeHelpers.InitializeArray((global::System.Array)array5, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			char[] array6 = (RealInvalidPathChars = array5);
			char[] array7 = new char[41];
			RuntimeHelpers.InitializeArray((global::System.Array)array7, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			char[] array8 = (InvalidFileNameChars = array7);
			PathSeparator = ';';
			MaxPath = 260;
			MaxDirectoryLength = 255;
			MaxLongPath = 32000;
			Prefix = "\\\\?\\";
			char[] array9 = new char[32];
			RuntimeHelpers.InitializeArray((global::System.Array)array9, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			char[] array10 = (s_Base32Char = array9);
		}

		internal static void CheckInvalidPathChars(string path, bool checkAdditional = false)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (path != null)
			{
				if (!HasIllegalCharacters(path, checkAdditional))
				{
					return;
				}
				throw new ArgumentException("The path has invalid characters.", "path");
			}
			throw new ArgumentNullException("path");
		}

		internal static bool HasIllegalCharacters(string path, bool checkAdditional)
		{
			for (int i = 0; i < path.get_Length(); i++)
			{
				int num = path.get_Chars(i);
				if (num == 34 || num == 60 || num == 62 || num == 124 || num < 32)
				{
					return true;
				}
				if (checkAdditional && (num == 63 || num == 42))
				{
					return true;
				}
			}
			return false;
		}

		public static string GetFileName(string path)
		{
			if (path != null)
			{
				CheckInvalidPathChars(path);
				int length = path.get_Length();
				int num = length;
				char c;
				do
				{
					int num2 = num - 1;
					num = num2;
					if (num2 < 0)
					{
						return path;
					}
					c = path.get_Chars(num);
				}
				while (c != DirectorySeparatorChar && c != AltDirectorySeparatorChar && c != VolumeSeparatorChar);
				return path.Substring(num + 1, length - num - 1);
			}
			return path;
		}
	}
}
