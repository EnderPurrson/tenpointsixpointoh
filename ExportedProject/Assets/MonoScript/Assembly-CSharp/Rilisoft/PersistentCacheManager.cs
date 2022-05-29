using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class PersistentCacheManager : MonoBehaviour
	{
		private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		private readonly TaskCompletionSource<Dictionary<string, string>> _firstResponsePromise = new TaskCompletionSource<Dictionary<string, string>>();

		private readonly Dictionary<string, string> _signatures = new Dictionary<string, string>();

		private readonly static Lazy<PersistentCacheManager> _instance;

		internal Task FirstResponse
		{
			get
			{
				return this._firstResponsePromise.Task;
			}
		}

		public static PersistentCacheManager Instance
		{
			get
			{
				return PersistentCacheManager._instance.Value;
			}
		}

		internal bool IsEnabled
		{
			get
			{
				return true;
			}
		}

		static PersistentCacheManager()
		{
			PersistentCacheManager._instance = new Lazy<PersistentCacheManager>(new Func<PersistentCacheManager>(PersistentCacheManager.Create));
		}

		public PersistentCacheManager()
		{
		}

		private static PersistentCacheManager Create()
		{
			PersistentCacheManager persistentCacheManager = UnityEngine.Object.FindObjectOfType<PersistentCacheManager>();
			if (persistentCacheManager != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(persistentCacheManager.gameObject);
				return persistentCacheManager;
			}
			GameObject gameObject = new GameObject("Rilisoft.PersistentCacheManager");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<PersistentCacheManager>();
		}

		internal static void DebugReportCacheHit(string url)
		{
			if (!Defs.IsDeveloperBuild)
			{
				return;
			}
			string str = string.Format("Cache hit: {0}", url ?? string.Empty);
			if (!Application.isEditor)
			{
				UnityEngine.Debug.Log(str);
			}
			else
			{
				UnityEngine.Debug.LogFormat("<color=green><b>{0}</b></color>", new object[] { str });
			}
		}

		internal static void DebugReportCacheMiss(string url)
		{
			if (!Defs.IsDeveloperBuild)
			{
				return;
			}
			string str = string.Format("Cache miss: {0}", url ?? string.Empty);
			if (!Application.isEditor)
			{
				UnityEngine.Debug.Log(str);
			}
			else
			{
				UnityEngine.Debug.LogFormat("<color=orange><b>{0}</b></color>", new object[] { str });
			}
		}

		[DebuggerHidden]
		private static IEnumerator DownloadSignaturesCoroutine(string[] urls, CancellationToken cancellationToken, TaskCompletionSource<string> promise)
		{
			PersistentCacheManager.u003cDownloadSignaturesCoroutineu003ec__Iterator176 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator DownloadSignaturesLoopCoroutine(float delaySeconds, CancellationToken cancellationToken, TaskCompletionSource<Dictionary<string, string>> promise)
		{
			PersistentCacheManager.u003cDownloadSignaturesLoopCoroutineu003ec__Iterator173 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator DownloadSignaturesOnceCoroutine(CancellationToken cancellationToken, TaskCompletionSource<Dictionary<string, string>> promise, int context = 0)
		{
			PersistentCacheManager.u003cDownloadSignaturesOnceCoroutineu003ec__Iterator174 variable = null;
			return variable;
		}

		private static string GetSegmentsAsString(string url)
		{
			string empty;
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			try
			{
				string[] segments = (new Uri(url)).Segments;
				if ((int)segments.Length != 0)
				{
					StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
					string[] array = segments.SkipWhile<string>((string s, int i) => (i != 0 ? false : ordinalIgnoreCase.Equals(s, "/"))).SkipWhile<string>((string s, int i) => (i != 0 ? false : ordinalIgnoreCase.Equals(s, "pixelgun3d-config/"))).ToArray<string>();
					empty = string.Concat(array).TrimStart(new char[] { '/' });
				}
				else
				{
					empty = string.Empty;
				}
			}
			catch
			{
				empty = string.Empty;
			}
			return empty;
		}

		private static string GetStorageKey(string segments)
		{
			return string.Format("Cache:{0}", segments);
		}

		public string GetValue(string keyUrl)
		{
			string str;
			string empty;
			if (keyUrl == null)
			{
				throw new ArgumentNullException("keyUrl");
			}
			if (!this.IsEnabled)
			{
				return null;
			}
			if (string.IsNullOrEmpty(keyUrl))
			{
				return string.Empty;
			}
			string segmentsAsString = PersistentCacheManager.GetSegmentsAsString(keyUrl);
			if (string.IsNullOrEmpty(segmentsAsString))
			{
				return string.Empty;
			}
			string storageKey = PersistentCacheManager.GetStorageKey(segmentsAsString);
			string str1 = Storager.getString(storageKey, false);
			if (string.IsNullOrEmpty(str1))
			{
				return string.Empty;
			}
			try
			{
				PersistentCacheManager.CacheEntry cacheEntry = JsonUtility.FromJson<PersistentCacheManager.CacheEntry>(str1);
				if (!this._signatures.TryGetValue(segmentsAsString, out str) || !StringComparer.Ordinal.Equals(str, cacheEntry.signature))
				{
					if (Storager.hasKey(storageKey))
					{
						Storager.setString(storageKey, string.Empty, false);
					}
					empty = string.Empty;
				}
				else
				{
					empty = cacheEntry.payload ?? string.Empty;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				UnityEngine.Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[] { typeof(PersistentCacheManager.CacheEntry).Name, str1 });
				UnityEngine.Debug.LogException(exception);
				empty = null;
			}
			return empty;
		}

		public bool SetValue(string keyUrl, string value)
		{
			string str;
			if (keyUrl == null)
			{
				throw new ArgumentNullException("keyUrl");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!this.IsEnabled)
			{
				return false;
			}
			string segmentsAsString = PersistentCacheManager.GetSegmentsAsString(keyUrl);
			if (string.IsNullOrEmpty(segmentsAsString))
			{
				return false;
			}
			if (!this._signatures.TryGetValue(segmentsAsString, out str))
			{
				return false;
			}
			string storageKey = PersistentCacheManager.GetStorageKey(segmentsAsString);
			PersistentCacheManager.CacheEntry cacheEntry = new PersistentCacheManager.CacheEntry();
			PersistentCacheManager.CacheEntry cacheEntry1 = cacheEntry;
			cacheEntry1.signature = str;
			cacheEntry1.payload = value;
			cacheEntry = cacheEntry1;
			Storager.setString(storageKey, JsonUtility.ToJson(cacheEntry), false);
			return true;
		}

		public Task StartDownloadSignaturesLoop()
		{
			float single;
			TaskCompletionSource<Dictionary<string, string>> taskCompletionSource = this._firstResponsePromise;
			if (!this.IsEnabled)
			{
				taskCompletionSource.TrySetException(new NotSupportedException());
				return taskCompletionSource.Task;
			}
			this._cancellationTokenSource.Cancel();
			this._cancellationTokenSource = new CancellationTokenSource();
			if (!Application.isEditor)
			{
				single = (!Defs.IsDeveloperBuild ? 600f : 60f);
			}
			else
			{
				single = 30f;
			}
			float single1 = single;
			base.StartCoroutine(this.DownloadSignaturesLoopCoroutine(single1, this._cancellationTokenSource.Token, taskCompletionSource));
			return taskCompletionSource.Task;
		}

		[DebuggerHidden]
		private static IEnumerator WaitCompletionAndParseSignaturesCoroutine(Task<string> future, TaskCompletionSource<Dictionary<string, string>> promise)
		{
			PersistentCacheManager.u003cWaitCompletionAndParseSignaturesCoroutineu003ec__Iterator175 variable = null;
			return variable;
		}

		[Serializable]
		private struct CacheEntry
		{
			public string signature;

			public string payload;
		}
	}
}