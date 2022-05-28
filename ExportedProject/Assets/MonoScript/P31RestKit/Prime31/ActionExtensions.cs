using System;
using System.Reflection;
using UnityEngine;

namespace Prime31
{
	public static class ActionExtensions
	{
		private static void invoke(global::System.Delegate listener, object[] args)
		{
			if (!((MethodBase)listener.get_Method()).get_IsStatic() && (listener.get_Target() == null || listener.get_Target().Equals((object)default(object))))
			{
				Debug.LogError("an event listener is still subscribed to an event with the method " + ((MemberInfo)listener.get_Method()).get_Name() + " even though it is null. Be sure to balance your event subscriptions.");
			}
			else
			{
				((MethodBase)listener.get_Method()).Invoke(listener.get_Target(), args);
			}
		}

		public static void fire(this Action handler)
		{
			if (handler != null)
			{
				object[] args = new object[0];
				global::System.Delegate[] invocationList = ((global::System.Delegate)(object)handler).GetInvocationList();
				foreach (global::System.Delegate listener in invocationList)
				{
					invoke(listener, args);
				}
			}
		}

		public static void fire<T>(this Action<T> handler, T param)
		{
			if (handler != null)
			{
				object[] args = new object[1] { param };
				global::System.Delegate[] invocationList = ((global::System.Delegate)(object)handler).GetInvocationList();
				foreach (global::System.Delegate listener in invocationList)
				{
					invoke(listener, args);
				}
			}
		}

		public static void fire<T, U>(this Action<T, U> handler, T param1, U param2)
		{
			if (handler != null)
			{
				object[] args = new object[2] { param1, param2 };
				global::System.Delegate[] invocationList = ((global::System.Delegate)(object)handler).GetInvocationList();
				foreach (global::System.Delegate listener in invocationList)
				{
					invoke(listener, args);
				}
			}
		}

		public static void fire<T, U, V>(this Action<T, U, V> handler, T param1, U param2, V param3)
		{
			if (handler != null)
			{
				object[] args = new object[3] { param1, param2, param3 };
				global::System.Delegate[] invocationList = ((global::System.Delegate)(object)handler).GetInvocationList();
				foreach (global::System.Delegate listener in invocationList)
				{
					invoke(listener, args);
				}
			}
		}
	}
}
