using System;
using UnityEngine;

public sealed class WavesSurvivedStat : MonoBehaviour
{
	public WavesSurvivedStat()
	{
	}

	private void Start()
	{
		UILabel component = base.GetComponent<UILabel>();
		int num = PlayerPrefs.GetInt(Defs.WavesSurvivedS, 0);
		component.text = num.ToString();
	}
}