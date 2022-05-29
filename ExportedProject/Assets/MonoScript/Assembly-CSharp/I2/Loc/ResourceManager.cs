using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class ResourceManager : MonoBehaviour
	{
		private static ResourceManager mInstance;

		public UnityEngine.Object[] Assets;

		private Dictionary<string, UnityEngine.Object> mResourcesCache = new Dictionary<string, UnityEngine.Object>();

		private bool mCleaningScheduled;

		public static ResourceManager pInstance
		{
			get
			{
				if (ResourceManager.mInstance == null)
				{
					ResourceManager.mInstance = (ResourceManager)UnityEngine.Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (ResourceManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", new Type[] { typeof(ResourceManager) })
					{
						hideFlags = gameObject.hideFlags | HideFlags.HideAndDontSave
					};
					ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
				}
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(ResourceManager.mInstance.gameObject);
				}
				return ResourceManager.mInstance;
			}
		}

		public ResourceManager()
		{
		}

		public void CleanResourceCache()
		{
			this.mResourcesCache.Clear();
			base.CancelInvoke();
			this.mCleaningScheduled = false;
		}

		private UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int num = 0;
				int length = (int)this.Assets.Length;
				while (num < length)
				{
					if (this.Assets[num] != null && this.Assets[num].name == Name)
					{
						return this.Assets[num];
					}
					num++;
				}
			}
			return null;
		}

		public T GetAsset<T>(string Name)
		where T : UnityEngine.Object
		{
			T t = (T)(this.FindAsset(Name) as T);
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		public bool HasAsset(UnityEngine.Object Obj)
		{
			if (this.Assets == null)
			{
				return false;
			}
			return Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		public T LoadFromResources<T>(string Path)
		where T : UnityEngine.Object
		{
			UnityEngine.Object obj;
			if (this.mResourcesCache.TryGetValue(Path, out obj) && obj != null)
			{
				return (T)(obj as T);
			}
			T t = Resources.Load<T>(Path);
			this.mResourcesCache[Path] = t;
			if (!this.mCleaningScheduled)
			{
				base.Invoke("CleanResourceCache", 0.1f);
				this.mCleaningScheduled = true;
			}
			return t;
		}
	}
}