using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NewLobbyLevelAnimator : MonoBehaviour
{
	private const int numOFStepsWhenAppearingButton = 25;

	public float buttonAlphaTime = 1f;

	public float timeBetweenShineAndTip = 0.3f;

	public float timeTipShown = 5f;

	public List<GameObject> buttons;

	public List<GameObject> tips;

	public List<GameObject> shines;

	public List<NewLobbyLevelAnimator.CondtionsForShow> conditions;

	private bool _tapped;

	public NewLobbyLevelAnimator()
	{
	}

	private void Awake()
	{
		foreach (GameObject button in this.buttons)
		{
			button.GetComponent<UISprite>().alpha = 0f;
		}
		foreach (GameObject shine in this.shines)
		{
			shine.SetActive(false);
		}
		foreach (GameObject tip in this.tips)
		{
			tip.SetActive(false);
		}
	}

	public void OnMouseDown()
	{
		this._tapped = true;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		NewLobbyLevelAnimator.u003cStartu003ec__IteratorCD variable = null;
		return variable;
	}

	public enum CondtionsForShow
	{
		None,
		PromoOffers,
		Premium
	}
}