using System;
using UnityEngine;

public sealed class HideSaleLabelInShop : MonoBehaviour
{
	public GameObject needTier;

	public HideSaleLabelInShop()
	{
	}

	private void Update()
	{
		if (this.needTier.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}
}