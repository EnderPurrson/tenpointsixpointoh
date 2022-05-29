using System;
using UnityEngine;

public class NetworkStartTableOnGUI : MonoBehaviour
{
	public NetworkStartTable myTable;

	public NetworkStartTableOnGUI()
	{
	}

	private void OnGUI()
	{
		this.myTable.MyOnGUI();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}