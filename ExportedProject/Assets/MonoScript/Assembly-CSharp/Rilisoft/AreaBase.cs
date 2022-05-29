using System;
using UnityEngine;

namespace Rilisoft
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Collider))]
	public abstract class AreaBase : MonoBehaviour
	{
		public const string AREA_OBJECT_TAG = "Area";

		public const string AREA_OBJECT_LAYER = "Ignore Raycast";

		[ReadOnly]
		[SerializeField]
		private bool _isActive;

		[SerializeField]
		private string _description;

		protected AreaBase()
		{
		}

		protected virtual void Awake()
		{
			Collider component = base.GetComponent<Collider>();
			if (component == null)
			{
				throw new Exception("Collider not found");
			}
			if (!component.isTrigger)
			{
				Debug.LogWarningFormat("[AREA SYSTEM] collider now is trigger, go:'{0}'", new object[] { base.gameObject.name });
				component.isTrigger = true;
			}
			int layer = LayerMask.NameToLayer("Ignore Raycast");
			if (base.gameObject.layer != layer)
			{
				base.gameObject.layer = layer;
			}
			if (!base.gameObject.CompareTag("Area"))
			{
				base.gameObject.tag = "Area";
			}
		}

		public virtual void CheckIn(GameObject to)
		{
			this._isActive = true;
		}

		public virtual void CheckOut(GameObject from)
		{
			this._isActive = false;
		}
	}
}