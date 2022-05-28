using System;
using System.Collections;
using System.Collections.Generic;
using CompilerGenerated;

namespace UnityScript.Lang
{
	public static class ExpandoServices
	{
		[Serializable]
		internal class _0024GetExpandoFor_0024locals_002415
		{
			internal object _0024o;
		}

		[Serializable]
		internal class _0024GetExpandoFor_0024closure_00244
		{
			internal _0024GetExpandoFor_0024locals_002415 _0024_0024locals_002416;

			public _0024GetExpandoFor_0024closure_00244(_0024GetExpandoFor_0024locals_002415 _0024_0024locals_002416)
			{
				this._0024_0024locals_002416 = _0024_0024locals_002416;
			}

			public bool Invoke(Expando e)
			{
				return e.Target == _0024_0024locals_002416._0024o;
			}
		}

		[NonSerialized]
		private static List<Expando> _expandos = new List<Expando>();

		public static int ExpandoObjectCount
		{
			get
			{
				Purge();
				return ((ICollection)_expandos).Count;
			}
		}

		public static object GetExpandoProperty(object target, string name)
		{
			Expando expandoFor = GetExpandoFor(target);
			return (expandoFor != null) ? expandoFor[name] : null;
		}

		public static object SetExpandoProperty(object target, string name, object value)
		{
			Expando orCreateExpandoFor = GetOrCreateExpandoFor(target);
			orCreateExpandoFor[name] = value;
			return value;
		}

		public static Expando GetExpandoFor(object o)
		{
			//Discarded unreachable code: IL_0044
			_0024GetExpandoFor_0024locals_002415 _0024GetExpandoFor_0024locals_0024 = new _0024GetExpandoFor_0024locals_002415();
			_0024GetExpandoFor_0024locals_0024._0024o = o;
			lock (_expandos)
			{
				Purge();
				return _expandos.Find(_0024adaptor_0024__ExpandoServices_0024callable0_002460_29___0024Predicate_00240.Adapt(new _0024GetExpandoFor_0024closure_00244(_0024GetExpandoFor_0024locals_0024).Invoke));
			}
		}

		public static Expando GetOrCreateExpandoFor(object o)
		{
			//Discarded unreachable code: IL_0032
			lock (_expandos)
			{
				Expando expando = GetExpandoFor(o);
				if (expando == null)
				{
					expando = new Expando(o);
					_expandos.Add(expando);
				}
				return expando;
			}
		}

		public static void Purge()
		{
			lock (_expandos)
			{
				_expandos.RemoveAll(_0024adaptor_0024__ExpandoServices_0024callable0_002460_29___0024Predicate_00240.Adapt(_0024Purge_0024closure_00245));
			}
		}

		internal static bool _0024Purge_0024closure_00245(Expando e)
		{
			return e.Target == null;
		}
	}
}
