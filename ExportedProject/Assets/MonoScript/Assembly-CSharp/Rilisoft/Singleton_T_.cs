using System;
using System.Reflection;
using UnityEngine;

namespace Rilisoft
{
	public abstract class Singleton<T> : MonoBehaviour
	where T : MonoBehaviour
	{
		private const string MSG_DUPLICATE = "[Singleton] Something went really wrong - there should never be more than 1 singleton!";

		private const string MSG_PREFAB_NOT_SETTED = "[Singleton] prefab not setted";

		private const string MSG_NOT_FOUND_IN_PREFAB = "[Singleton] can not find singleton class in prefab";

		private static T _instance;

		public static T Instance
		{
			get
			{
				if (Singleton<T>._instance != null)
				{
					return Singleton<T>._instance;
				}
				Singleton<T>._instance = UnityEngine.Object.FindObjectOfType<T>();
				if (Singleton<T>._instance != null)
				{
					Singleton<T>._instance.SendMessage("OnInstanceCreated", SendMessageOptions.DontRequireReceiver);
					return Singleton<T>._instance;
				}
				GameObject gameObject = null;
				ISingletonFromPrefab singletonFromPrefab = (object)Singleton<T>._instance as ISingletonFromPrefab;
				if (singletonFromPrefab == null)
				{
					gameObject = new GameObject(typeof(T).Name);
					Singleton<T>._instance = gameObject.AddComponent<T>();
				}
				else if (singletonFromPrefab.SingletonPrefab == null)
				{
					Debug.LogError("[Singleton] prefab not setted");
				}
				else
				{
					GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(singletonFromPrefab.SingletonPrefab);
					T component = gameObject1.GetComponent<T>();
					if (component == null)
					{
						Debug.LogError("[Singleton] can not find singleton class in prefab");
					}
					else
					{
						Singleton<T>._instance = component;
						gameObject = gameObject1;
					}
				}
				GameObject gameObject2 = gameObject;
				gameObject2.name = string.Concat(gameObject2.name, " [Singleton]");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				Singleton<T>._instance.SendMessage("OnInstanceCreated", SendMessageOptions.DontRequireReceiver);
				if ((int)UnityEngine.Object.FindObjectsOfType<T>().Length > 1)
				{
					Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton!");
				}
				return Singleton<T>._instance;
			}
			protected set
			{
				Singleton<T>._instance = value;
				if ((int)UnityEngine.Object.FindObjectsOfType<T>().Length > 1)
				{
					Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton!");
				}
				UnityEngine.Object.DontDestroyOnLoad(Singleton<T>._instance.gameObject);
				Singleton<T>._instance.SendMessage("OnInstanceCreated", SendMessageOptions.DontRequireReceiver);
			}
		}

		protected static bool IsSetted
		{
			get
			{
				return Singleton<T>._instance != null;
			}
		}

		protected Singleton()
		{
		}

		protected virtual void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}