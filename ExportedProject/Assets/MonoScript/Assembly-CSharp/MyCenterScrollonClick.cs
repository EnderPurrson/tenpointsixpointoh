using System;
using UnityEngine;

public class MyCenterScrollonClick : UIDragScrollView
{
	private MyCenterOnChild center;

	public MyCenterScrollonClick()
	{
	}

	private void Awake()
	{
		if (this.center == null)
		{
			this.center = NGUITools.FindInParents<MyCenterOnChild>(base.gameObject);
		}
	}

	private void OnClick()
	{
		this.center.CenterOn(base.transform);
	}
}