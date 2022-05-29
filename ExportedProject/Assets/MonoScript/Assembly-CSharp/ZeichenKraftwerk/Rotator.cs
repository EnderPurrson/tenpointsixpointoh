using System;
using UnityEngine;

namespace ZeichenKraftwerk
{
	public sealed class Rotator : MonoBehaviour
	{
		public Vector3 eulersPerSecond = new Vector3(0f, 0f, 1f);

		private Transform myTransform;

		public Rotator()
		{
		}

		public void Start()
		{
			this.myTransform = base.transform;
		}

		private void Update()
		{
			this.myTransform.Rotate(this.eulersPerSecond * RealTime.deltaTime);
		}
	}
}