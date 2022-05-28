using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class AreaPlayerStepsSounds : AreaBase
	{
		[SerializeField]
		private PlayerStepsSoundsData _sounds;

		[ReadOnly]
		[SerializeField]
		private PlayerStepsSoundsData _soundsOriginal;

		private static readonly Dictionary<int, SkinName> _soundsComponents = new Dictionary<int, SkinName>();

		[CompilerGenerated]
		private static Func<GameObject, bool> _003C_003Ef__am_0024cache3;

		public override void CheckIn(GameObject to)
		{
			base.CheckIn(to);
		}

		public override void CheckOut(GameObject from)
		{
			base.CheckOut(from);
		}

		private SkinName GetSoundsComponent(GameObject go)
		{
			int hashCode = go.GetHashCode();
			if (_soundsComponents.ContainsKey(hashCode))
			{
				return _soundsComponents[hashCode];
			}
			IEnumerable<GameObject> source = go.Ancestors();
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003CGetSoundsComponent_003Em__246;
			}
			SkinName componentInChildren = source.First(_003C_003Ef__am_0024cache3).GetComponentInChildren<SkinName>();
			_soundsComponents.Add(hashCode, componentInChildren);
			return componentInChildren;
		}

		[CompilerGenerated]
		private static bool _003CGetSoundsComponent_003Em__246(GameObject a)
		{
			return a.Parent() == null;
		}
	}
}
