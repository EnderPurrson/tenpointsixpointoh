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
				return ToResourcePath(FullPath);
			}
		}

		public static string ToResourcePath(string fullPath)
		{
			if (fullPath.IsNullOrEmpty())
			{
				return string.Empty;
			}
			List<string> list = fullPath.Split((!fullPath.Contains("/")) ? '\\' : '/').ToList();
			if (list.Count > 0 && list[0].Contains("Assets"))
			{
				list.RemoveAt(0);
			}
			if (list.Count > 0 && list[0].Contains("Resources"))
			{
				list.RemoveAt(0);
			}
			fullPath = string.Join(Path.DirectorySeparatorChar.ToString(), list.ToArray());
			fullPath = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath));
			return fullPath;
		}
	}
}
