using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class BackSystem : MonoBehaviour
	{
		private readonly static Lazy<BackSystem> _instance;

		private readonly LinkedList<BackSystem.Subscription> _subscriptions = new LinkedList<BackSystem.Subscription>();

		public static bool Active
		{
			get
			{
				return true;
			}
		}

		public static BackSystem Instance
		{
			get
			{
				return BackSystem._instance.Value;
			}
		}

		static BackSystem()
		{
			BackSystem._instance = new Lazy<BackSystem>(new Func<BackSystem>(BackSystem.InitializeInstance));
		}

		public BackSystem()
		{
		}

		private void CollectGarbage()
		{
			for (LinkedListNode<BackSystem.Subscription> i = this._subscriptions.Last; i != null; i = this._subscriptions.Last)
			{
				if (!i.Value.Disposed)
				{
					return;
				}
				this._subscriptions.RemoveLast();
			}
		}

		private static bool EscapePressed()
		{
			if (!BackSystem.Active)
			{
				return false;
			}
			return Input.GetKeyUp(KeyCode.Escape);
		}

		private static BackSystem InitializeInstance()
		{
			BackSystem backSystem = UnityEngine.Object.FindObjectOfType<BackSystem>();
			if (backSystem != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(backSystem.gameObject);
				return backSystem;
			}
			GameObject gameObject = new GameObject("Rilisoft.BackSystem");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<BackSystem>();
		}

		public IDisposable Register(Action callback, string context = null)
		{
			BackSystem.Subscription subscription = new BackSystem.Subscription(callback, context, this._subscriptions);
			if (Application.isEditor)
			{
				Debug.Log(string.Format("<color=lightblue>Back stack after registration: {0}</color>", this));
			}
			return subscription;
		}

		private object ToJson()
		{
			List<string> strs = new List<string>(this._subscriptions.Count);
			foreach (BackSystem.Subscription _subscription in this._subscriptions)
			{
				if (!_subscription.Disposed)
				{
					strs.Add(_subscription.Context);
				}
				else
				{
					strs.Add(string.Concat(_subscription.Context, " (Disposed)"));
				}
			}
			return strs;
		}

		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}

		private void Update()
		{
			if (BackSystem.EscapePressed())
			{
				Input.ResetInputAxes();
				string str = (!Application.isEditor ? string.Empty : this.ToString());
				this.CollectGarbage();
				LinkedListNode<BackSystem.Subscription> last = this._subscriptions.Last;
				if (last == null)
				{
					return;
				}
				last.Value.Invoke();
				if (Application.isEditor)
				{
					Debug.Log(string.Format("<color=#db7093ff>Back stack on invoke: {0} -> {1}</color>", str, this));
				}
			}
			else if (Input.GetKeyUp(KeyCode.Backspace) && Application.isEditor)
			{
				Debug.Log(string.Format("<color=#db7093ff>Current back stack: {0}</color>", this));
			}
		}

		private sealed class Subscription : IDisposable
		{
			private Action _callback;

			private readonly string _context;

			private LinkedListNode<BackSystem.Subscription> _node;

			public string Context
			{
				get
				{
					return this._context;
				}
			}

			public bool Disposed
			{
				get
				{
					return this._node == null;
				}
			}

			internal Subscription(Action callback, string context, LinkedList<BackSystem.Subscription> list)
			{
				this._callback = callback;
				this._context = context ?? string.Empty;
				if (list != null)
				{
					this._node = list.AddLast(this);
				}
			}

			public void Dispose()
			{
				if (this.Disposed)
				{
					return;
				}
				this._callback = null;
				LinkedList<BackSystem.Subscription> list = this._node.List;
				if (list != null)
				{
					list.Remove(this._node);
				}
				this._node = null;
			}

			public void Invoke()
			{
				if (this.Disposed)
				{
					Debug.LogWarning("Attempt to invoke disposed handler.");
					return;
				}
				if (this._callback != null)
				{
					this._callback();
				}
			}
		}
	}
}