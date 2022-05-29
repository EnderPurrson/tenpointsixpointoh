using System;
using UnityEngine;

namespace Rilisoft
{
	public class LazyObject<T>
	where T : MonoBehaviour
	{
		private readonly string _resourcePath;

		private GameObject _prefabVal;

		private T _value;

		private readonly GameObject _attachTo;

		private GameObject _prefab
		{
			get
			{
				if (this._prefabVal == null && !this._resourcePath.IsNullOrEmpty())
				{
					this._prefabVal = Resources.Load<GameObject>(this._resourcePath);
				}
				return this._prefabVal;
			}
			set
			{
				this._prefabVal = value;
			}
		}

		public bool ObjectIsActive
		{
			get
			{
				return (!this.ObjectIsLoaded ? false : this._value.gameObject.activeInHierarchy);
			}
		}

		public bool ObjectIsLoaded
		{
			get
			{
				return this._value != null;
			}
		}

		public T Value
		{
			get
			{
				if (this._value == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._prefab);
					this._value = gameObject.GetComponent<T>();
					if (this._attachTo != null)
					{
						gameObject.transform.SetParent(this._attachTo.transform);
						gameObject.transform.localPosition = this._attachTo.transform.localPosition;
						gameObject.transform.localScale = Vector3.one;
					}
				}
				return this._value;
			}
		}

		public LazyObject(GameObject prefab, GameObject attachTo)
		{
			this._prefab = prefab;
			this._attachTo = attachTo;
		}

		public LazyObject(string resourcePath, GameObject attachTo)
		{
			this._resourcePath = resourcePath;
			this._attachTo = attachTo;
		}

		public void DestroyValue()
		{
			if (this._value != null)
			{
				UnityEngine.Object.DestroyImmediate(this._value.gameObject);
			}
		}
	}
}