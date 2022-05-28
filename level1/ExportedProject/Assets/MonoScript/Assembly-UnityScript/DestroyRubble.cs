using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class DestroyRubble : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Start_002415 : GenericGenerator<WaitForSeconds>
	{
		internal DestroyRubble _0024self__002418;

		public _0024Start_002415(DestroyRubble self_)
		{
			_0024self__002418 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return (IEnumerator<WaitForSeconds>)(object)new _0024(_0024self__002418);
		}
	}

	public float maxTime;

	public ParticleEmitter[] particleEmitters;

	public float time;

	public DestroyRubble()
	{
		maxTime = 3f;
	}

	public override IEnumerator Start()
	{
		return new _0024Start_002415(this).GetEnumerator();
	}

	public override void Main()
	{
	}
}
