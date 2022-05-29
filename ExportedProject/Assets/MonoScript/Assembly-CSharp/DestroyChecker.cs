using System;
using UnityEngine;

public class DestroyChecker : MonoBehaviour
{
	public DestroyChecker()
	{
	}

	private void OnDestroy()
	{
		Debug.Log("Destroy");
	}
}