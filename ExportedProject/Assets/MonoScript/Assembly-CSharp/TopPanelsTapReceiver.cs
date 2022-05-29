using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TopPanelsTapReceiver : MonoBehaviour
{
	static TopPanelsTapReceiver()
	{
		TopPanelsTapReceiver.OnClicked = () => {
		};
	}

	public TopPanelsTapReceiver()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		TopPanelsTapReceiver.OnClicked();
	}

	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti);
	}

	public static event Action OnClicked;
}