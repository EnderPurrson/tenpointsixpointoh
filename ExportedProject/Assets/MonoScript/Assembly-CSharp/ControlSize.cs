using System;
using UnityEngine;

internal sealed class ControlSize : MonoBehaviour
{
	public int minValue;

	public int maxValue;

	public int defaultValue;

	public ControlSize()
	{
	}

	private void Update()
	{
		if (this.maxValue < this.minValue)
		{
			this.maxValue = this.minValue;
		}
		if (this.defaultValue < this.minValue)
		{
			this.defaultValue = this.minValue;
		}
		if (this.defaultValue > this.maxValue)
		{
			this.defaultValue = this.maxValue;
		}
	}
}