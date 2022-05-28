using System;
using System.Collections.Generic;

namespace UnityScript.Lang
{
	[Serializable]
	internal class Expando
	{
		protected WeakReference _target;

		protected Dictionary<string, object> _attributes;

		public object Target
		{
			get
			{
				return _target.Target;
			}
		}

		public object this[string key]
		{
			get
			{
				return _attributes[key];
			}
			set
			{
				if (value == null)
				{
					_attributes.Remove(key);
				}
				else
				{
					_attributes[key] = value;
				}
			}
		}

		public Expando(object target)
		{
			_attributes = new Dictionary<string, object>();
			_target = new WeakReference(target);
		}
	}
}
