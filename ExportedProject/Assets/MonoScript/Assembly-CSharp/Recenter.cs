using System;
using UnityEngine;

public class Recenter : MonoBehaviour
{
	public UICenterOnChild centerOnChildScript;

	[ReadOnly]
	public int nextChild = 1;

	[ReadOnly]
	public int prevChild = -1;

	public Recenter()
	{
	}

	public void CenterOn(int child)
	{
		int siblingIndex = this.centerOnChildScript.centeredObject.transform.GetSiblingIndex() + child;
		if (siblingIndex >= 0 && siblingIndex < this.centerOnChildScript.transform.childCount)
		{
			Transform transforms = this.centerOnChildScript.transform.GetChild(siblingIndex);
			this.centerOnChildScript.CenterOn(transforms);
		}
	}
}