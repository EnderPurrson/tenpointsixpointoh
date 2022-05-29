using System;
using UnityEngine;

public class FlagPedestalController : MonoBehaviour
{
	public GameObject BluePedestal;

	public GameObject RedPedestal;

	public FlagPedestalController()
	{
	}

	public void SetColor(int _color)
	{
		if (_color == 1)
		{
			this.BluePedestal.SetActive(true);
			this.RedPedestal.SetActive(false);
		}
		else if (_color != 2)
		{
			this.BluePedestal.SetActive(false);
			this.RedPedestal.SetActive(false);
		}
		else
		{
			this.BluePedestal.SetActive(false);
			this.RedPedestal.SetActive(true);
		}
	}
}