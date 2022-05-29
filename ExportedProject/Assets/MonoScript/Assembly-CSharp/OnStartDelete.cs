using System;
using UnityEngine;

public class OnStartDelete : MonoBehaviour
{
	public OnStartDelete()
	{
	}

	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}