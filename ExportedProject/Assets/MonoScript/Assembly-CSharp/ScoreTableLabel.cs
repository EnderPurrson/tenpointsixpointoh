using System;
using UnityEngine;

public class ScoreTableLabel : MonoBehaviour
{
	public ScoreTableLabel()
	{
	}

	private void Start()
	{
		if (Defs.isCOOP)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Get("Key_0190");
		}
		else if (!Defs.isFlag)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Get("Key_0191");
		}
		else
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Get("Key_1006");
		}
	}

	private void Update()
	{
	}
}