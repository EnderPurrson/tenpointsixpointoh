using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class SkipPresser : MonoBehaviour
{
	public GameObject windowAnchor;

	public SkipPresser()
	{
	}

	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	public static event Action SkipPressed;
}