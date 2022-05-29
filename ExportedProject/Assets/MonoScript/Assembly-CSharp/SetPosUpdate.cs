using System;
using UnityEngine;

public class SetPosUpdate : MonoBehaviour
{
	public int index;

	public SetPosUpdate()
	{
	}

	private void SetPos()
	{
		int num = this.index;
		if (num != 0)
		{
			if (num == 1)
			{
				base.transform.localPosition = new Vector3(-424f - (768f * (float)Screen.width / (float)Screen.height - 912f) / 2f, 44f, 0f);
			}
		}
		else if (!MainMenu.SkinsMakerSupproted())
		{
			base.transform.localPosition = new Vector3(-124f, 64f, 0f);
		}
		else
		{
			base.transform.localPosition = new Vector3(-385f - (768f * (float)Screen.width / (float)Screen.height - 976f) / 3f, 64f, 1f);
		}
	}

	private void Start()
	{
		this.SetPos();
	}

	private void Update()
	{
		this.SetPos();
	}
}