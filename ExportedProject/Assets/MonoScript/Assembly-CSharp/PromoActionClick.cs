using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PromoActionClick : MonoBehaviour
{
	public PromoActionClick()
	{
	}

	private void OnClick()
	{
		if (PromoActionClick.Click != null)
		{
			PromoActionClick.Click(base.transform.parent.GetComponent<PromoActionPreview>().tg);
		}
		Debug.Log("Click");
	}

	private void OnPress(bool down)
	{
		if (down)
		{
			if (base.transform.parent.GetComponent<PromoActionPreview>().pressed != null)
			{
				base.transform.parent.GetComponent<PromoActionPreview>().icon.mainTexture = base.transform.parent.GetComponent<PromoActionPreview>().pressed;
			}
		}
		else if (base.transform.parent.GetComponent<PromoActionPreview>().unpressed != null)
		{
			base.transform.parent.GetComponent<PromoActionPreview>().icon.mainTexture = base.transform.parent.GetComponent<PromoActionPreview>().unpressed;
		}
	}

	public static event Action<string> Click;
}