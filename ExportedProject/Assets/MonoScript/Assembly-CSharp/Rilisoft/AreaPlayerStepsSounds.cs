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

		private readonly static Dictionary<int, SkinName> _soundsComponents;

		static AreaPlayerStepsSounds()
		{
			AreaPlayerStepsSounds._soundsComponents = new Dictionary<int, SkinName>();
		}

		public AreaPlayerStepsSounds()
		{
		}

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
			if (AreaPlayerStepsSounds._soundsComponents.ContainsKey(hashCode))
			{
				return AreaPlayerStepsSounds._soundsComponents[hashCode];
			}
			SkinName componentInChildren = go.Ancestors().First<GameObject>((GameObject a) => a.Parent() == null).GetComponentInChildren<SkinName>();
			AreaPlayerStepsSounds._soundsComponents.Add(hashCode, componentInChildren);
			return componentInChildren;
		}
	}
}