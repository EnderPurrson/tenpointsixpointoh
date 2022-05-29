using System;
using UnityEngine;

public class ScoreStat : MonoBehaviour
{
	public ScoreStat()
	{
	}

	private void Start()
	{
		base.GetComponent<UILabel>().text = GlobalGameController.Score.ToString();
	}
}