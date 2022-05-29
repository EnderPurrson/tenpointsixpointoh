using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class PrefabHandler
	{
		[SerializeField]
		public string FullPath;

		public string ResourcePath
		{
			get
			{
				return PrefabHandler.ToResourcePath(this.FullPath);
			}
		}

		public PrefabHandler()
		{
		}

		public static string ToResourcePath(string fullPath)
		{
			if (fullPath.IsNullOrEmpty())
			{
				return string.Empty;
			}
			List<string> list = fullPath.Split(new char[] { (!fullPath.Contains("/") ? '\\' : '/') }).ToList<string>();
			if (list.Count > 0 && list[0].Contains("Assets"))
			{
				list.RemoveAt(0);
			}
			if (list.Count > 0 && list[0].Contains("Resources"))
			{
				list.RemoveAt(0);
			}
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			fullPath = string.Join(directorySeparatorChar.ToString(), list.ToArray());
			fullPath = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath));
			return fullPath;
		}
	}
}