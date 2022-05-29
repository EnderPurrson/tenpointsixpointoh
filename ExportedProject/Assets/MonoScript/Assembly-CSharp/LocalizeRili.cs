using I2.Loc;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class LocalizeRili : MonoBehaviour
{
	public GameObject[] labels;

	public string term;

	public bool execute;

	public LocalizeRili()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.execute)
		{
			return;
		}
		if (this.labels == null)
		{
			return;
		}
		Debug.Log("Localized");
		GameObject[] gameObjectArray = this.labels;
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			GameObject gameObject = gameObjectArray[i];
			gameObject.gameObject.AddComponent<Localize>().SetTerm("Key_04B_03", "Key_04B_03");
			if (this.term != string.Empty)
			{
				gameObject.gameObject.AddComponent<Localize>().SetTerm(this.term, this.term);
			}
		}
		this.execute = false;
	}
}