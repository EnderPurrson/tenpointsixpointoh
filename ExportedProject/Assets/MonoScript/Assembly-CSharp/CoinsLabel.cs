using System;
using UnityEngine;

public class CoinsLabel : MonoBehaviour
{
	public UILabel mylabel;

	public CoinsLabel()
	{
	}

	private void SetCountCoins()
	{
		this.mylabel.text = "1234";
	}

	private void Start()
	{
		this.SetCountCoins();
	}

	private void Update()
	{
		this.SetCountCoins();
	}
}