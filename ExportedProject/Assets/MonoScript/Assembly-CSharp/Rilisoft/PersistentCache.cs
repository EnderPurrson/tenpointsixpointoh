using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class PersistentCache
	{
		private readonly string _persistentDataPath = string.Empty;

		private static readonly Lazy<PersistentCache> _instance;

		[CompilerGenerated]
		private static Func<PersistentCache> _003C_003Ef__am_0024cache2;

		public string PersistentDataPath
		{
			get
			{
				return _persistentDataPath;
			}
		}

		public static PersistentCache Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public PersistentCache()
		{
			try
			{
				string text = ((!string.IsNullOrEmpty(Application.persistentDataPath)) ? Application.persistentDataPath : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
				if (!string.IsNullOrEmpty(text))
				{
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					_persistentDataPath = text;
				}
			}
			catch (Exception exception)
			{
				Debug.LogWarning("Caught exception while persistent data path initialization. See next error message for details.");
				Debug.LogException(exception);
			}
		}

		static PersistentCache()
		{
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003C_instance_003Em__390;
			}
			_instance = new Lazy<PersistentCache>(_003C_003Ef__am_0024cache2);
		}

		public string GetCachePathByUri(string url)
		{
			//Discarded unreachable code: IL_0072, IL_0084
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(_persistentDataPath))
			{
				return string.Empty;
			}
			try
			{
				Uri uri = new Uri(url);
				string[] segments = uri.Segments;
				string path = string.Concat(segments).TrimStart('/');
				return Path.Combine(_persistentDataPath, path);
			}
			catch
			{
				return string.Empty;
			}
		}

		[CompilerGenerated]
		private static PersistentCache _003C_instance_003Em__390()
		{
			return new PersistentCache();
		}
	}
}
