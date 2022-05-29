using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[RequireComponent(typeof(Collider))]
	public class AreaVisitMonitor : MonoBehaviour
	{
		[ReadOnly]
		[SerializeField]
		private List<AreaBase> _activeAreas = new List<AreaBase>();

		public AreaVisitMonitor()
		{
		}

		private void Awake()
		{
			Collider component = base.GetComponent<Collider>();
			if (component == null)
			{
				throw new Exception("Collider not found");
			}
			if (!component.isTrigger)
			{
				Debug.LogWarningFormat("[AREA SYSTEM] collider now is trigger go:'{0}'", new object[] { base.gameObject.name });
				component.isTrigger = true;
			}
		}

		private void OnDisable()
		{
			this._activeAreas.ForEach((AreaBase a) => a.CheckOut(base.gameObject));
			this._activeAreas.Clear();
		}

		private void OnTriggerEnter(Collider other)
		{
			AreaBase component = other.GetComponent<AreaBase>();
			if (component == null)
			{
				return;
			}
			this._activeAreas.Add(component);
			component.CheckIn(base.gameObject);
		}

		private void OnTriggerExit(Collider other)
		{
			AreaBase component = other.GetComponent<AreaBase>();
			if (component == null)
			{
				return;
			}
			if (this._activeAreas.Contains(component))
			{
				this._activeAreas.Remove(component);
			}
			component.CheckOut(base.gameObject);
		}

		private void Update()
		{
		}
	}
}