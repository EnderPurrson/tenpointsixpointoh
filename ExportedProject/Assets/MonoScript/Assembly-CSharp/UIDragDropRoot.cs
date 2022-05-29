using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
	public static Transform root;

	public UIDragDropRoot()
	{
	}

	private void OnDisable()
	{
		if (UIDragDropRoot.root == base.transform)
		{
			UIDragDropRoot.root = null;
		}
	}

	private void OnEnable()
	{
		UIDragDropRoot.root = base.transform;
	}
}