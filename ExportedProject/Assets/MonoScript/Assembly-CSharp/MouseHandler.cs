using System;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
	public MouseHandler()
	{
	}

	private void OnMouseDown()
	{
		Debug.Log("OnMouseDown");
	}

	private void OnMouseOver()
	{
		Debug.Log("OnMouseOver");
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}