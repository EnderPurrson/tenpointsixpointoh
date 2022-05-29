using System;
using UnityEngine;

namespace Facebook.Unity
{
	internal class ComponentFactory
	{
		public const string GameObjectName = "UnityFacebookSDKPlugin";

		private static GameObject facebookGameObject;

		private static GameObject FacebookGameObject
		{
			get
			{
				if (ComponentFactory.facebookGameObject == null)
				{
					ComponentFactory.facebookGameObject = new GameObject("UnityFacebookSDKPlugin");
				}
				return ComponentFactory.facebookGameObject;
			}
		}

		public ComponentFactory()
		{
		}

		public static T AddComponent<T>()
		where T : MonoBehaviour
		{
			return ComponentFactory.FacebookGameObject.AddComponent<T>();
		}

		public static T GetComponent<T>(ComponentFactory.IfNotExist ifNotExist = 0)
		where T : MonoBehaviour
		{
			GameObject facebookGameObject = ComponentFactory.FacebookGameObject;
			T component = facebookGameObject.GetComponent<T>();
			if (component == null && ifNotExist == ComponentFactory.IfNotExist.AddNew)
			{
				component = facebookGameObject.AddComponent<T>();
			}
			return component;
		}

		internal enum IfNotExist
		{
			AddNew,
			ReturnNull
		}
	}
}