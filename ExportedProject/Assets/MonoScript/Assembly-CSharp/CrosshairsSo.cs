using System;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairsSo : ScriptableObject
{
	[SerializeField]
	public List<CrosshairData> Crosshairs;

	public CrosshairsSo()
	{
	}
}