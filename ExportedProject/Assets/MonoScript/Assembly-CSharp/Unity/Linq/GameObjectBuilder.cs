using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Linq
{
	public class GameObjectBuilder
	{
		private readonly GameObject original;

		private readonly IEnumerable<GameObjectBuilder> children;

		public GameObjectBuilder(GameObject original, params GameObjectBuilder[] children) : this(original, (IEnumerable<GameObjectBuilder>)children)
		{
		}

		public GameObjectBuilder(GameObject original, IEnumerable<GameObjectBuilder> children)
		{
			this.original = original;
			this.children = children;
		}

		public GameObject Instantiate()
		{
			return this.Instantiate(TransformCloneType.KeepOriginal);
		}

		public GameObject Instantiate(bool setActive)
		{
			return this.Instantiate(TransformCloneType.KeepOriginal);
		}

		public GameObject Instantiate(TransformCloneType cloneType)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.original);
			this.InstantiateChildren(gameObject, cloneType, null);
			return gameObject;
		}

		public GameObject Instantiate(TransformCloneType cloneType, bool setActive)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.original);
			this.InstantiateChildren(gameObject, cloneType, new bool?(setActive));
			return gameObject;
		}

		private void InstantiateChildren(GameObject root, TransformCloneType cloneType, bool? setActive)
		{
			IEnumerator<GameObjectBuilder> enumerator = this.children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GameObjectBuilder current = enumerator.Current;
					GameObject gameObject = root.Add(current.original, cloneType, setActive, null);
					current.InstantiateChildren(gameObject, cloneType, setActive);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}
	}
}