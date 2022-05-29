using System;
using UnityEngine;

public class QuitOnEscapeOrBack : MonoBehaviour
{
	public QuitOnEscapeOrBack()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}