using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class HideAtChildOf : MonoBehaviour
	{
		[SerializeField]
		private string _rootObjectName;

		public HideAtChildOf()
		{
		}

		private void Start()
		{
			if (this._rootObjectName.IsNullOrEmpty())
			{
				return;
			}
			this._rootObjectName = this._rootObjectName.ToLower();
			IEnumerator<GameObject> enumerator = base.gameObject.Ancestors().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name.ToLower() != this._rootObjectName)
					{
						continue;
					}
					base.gameObject.SetActive(false);
					break;
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