using System;
using UnityEngine;

public class ChatScroll : MonoBehaviour
{
	public UITextListEdit _Textlist;

	private bool isTouch;

	public ChatScroll()
	{
	}

	private void LateUpdate()
	{
		if (this.isTouch)
		{
			this._Textlist.OnScroll((float)Math.Round((double)(-Input.GetAxis("Mouse Y") / 5f), 1));
		}
	}

	private void Start()
	{
		base.transform.position = new Vector3(posNGUI.getPosX((float)Screen.width * 0.1f), posNGUI.getPosY((float)Screen.height * 0.03f), 1f);
	}

	private void Update()
	{
		Touch[] touchArray = Input.touches;
		for (int i = 0; i < (int)touchArray.Length; i++)
		{
			Touch touch = touchArray[i];
			if (touch.phase == TouchPhase.Began)
			{
				this.isTouch = true;
				this._Textlist.OnSelect(true);
			}
			if (touch.phase == TouchPhase.Ended)
			{
				this.isTouch = false;
				this._Textlist.OnSelect(false);
			}
		}
	}
}