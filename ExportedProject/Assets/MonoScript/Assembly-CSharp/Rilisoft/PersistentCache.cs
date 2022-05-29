using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class PersistentCache
	{
		private readonly string _persistentDataPath = string.Empty;

		private readonly static Lazy<PersistentCache> _instance;

		public static PersistentCache Instance
		{
			get
			{
				return PersistentCache._instance.Value;
			}
		}

		public string PersistentDataPath
		{
			get
			{
				return this._persistentDataPath;
			}
		}

		static PersistentCache()
		{
			PersistentCache._instance = new Lazy<PersistentCache>(() => new PersistentCache());
		}

		public PersistentCache()
		{
			try
			{
				string str = (!string.IsNullOrEmpty(Application.persistentDataPath) ? Application.persistentDataPath : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
				if (!string.IsNullOrEmpty(str))
				{
					if (!Directory.Exists(str))
					{
						Directory.CreateDirectory(str);
					}
					this._persistentDataPath = str;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Debug.LogWarning("Caught exception while persistent data path initialization. See next error message for details.");
				Debug.LogException(exception);
			}
		}

		public string GetCachePathByUri(string url)
		{
			string empty;
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(this._persistentDataPath))
			{
				return string.Empty;
			}
			try
			{
				string str = string.Concat((new Uri(url)).Segments).TrimStart(new char[] { '/' });
				empty = Path.Combine(this._persistentDataPath, str);
			}
			catch
			{
				empty = string.Empty;
			}
			return empty;
		}
	}
}