using System;
using UnityEngine;

internal sealed class TrainingHelper : MonoBehaviour
{
	private Rect _buttonRect;

	public TrainingHelper()
	{
	}

	private void Start()
	{
		float coef = 211f * Defs.Coef;
		float single = 114f * Defs.Coef;
		float coef1 = 12f * Defs.Coef;
		this._buttonRect = new Rect((float)Screen.width - coef - coef1, single + 64f * Defs.Coef + 3f * coef1, coef, single);
	}
}