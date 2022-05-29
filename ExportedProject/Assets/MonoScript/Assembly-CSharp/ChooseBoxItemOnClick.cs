using System;
using UnityEngine;

public class ChooseBoxItemOnClick : MonoBehaviour
{
	public ChooseBoxItemOnClick()
	{
	}

	private void OnClick()
	{
		if (!base.gameObject.name.Contains((ChooseBox.instance.nguiController.selectIndexMap + 1).ToString()))
		{
			ButtonClickSound.TryPlayClick();
			MyCenterOnChild component = base.transform.parent.GetComponent<MyCenterOnChild>();
			if (component != null)
			{
				component.CenterOn(base.transform);
			}
		}
		else
		{
			ChooseBox.instance.StartNameBox(base.gameObject.name);
		}
	}
}