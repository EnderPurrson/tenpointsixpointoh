using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class DestroyRubble : MonoBehaviour
{
	public float maxTime;

	public ParticleEmitter[] particleEmitters;

	public float time;

	public DestroyRubble()
	{
		this.maxTime = (float)3;
	}

	public override void Main()
	{
	}

	public override IEnumerator Start()
	{
		return (new DestroyRubble.u0024Startu002415(this)).GetEnumerator();
	}
}