using System;
using UnityEngine;

public class KeybordHolder : MonoBehaviour
{
	public KeybordShow _tk;

	public KeybordHolder()
	{
	}

	private void OnClick()
	{
		if (!this._tk.mKeybordHold)
		{
			this._tk.mKeybordHold = true;
		}
		else
		{
			this._tk.mKeybordHold = false;
		}
	}
}