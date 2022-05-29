using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UIInput))]
public class UIInputFocusAtEnable : MonoBehaviour
{
	private const float FOCUS_DELAY = 0.3f;

	[SerializeField]
	[Tooltip("Применить только один раз")]
	private bool _onlyOnce;

	[ReadOnly]
	[SerializeField]
	private UIInput _input;

	private bool _alreadyTurned;

	public UIInputFocusAtEnable()
	{
	}

	private void Awake()
	{
		this._input = base.GetComponent<UIInput>();
		if (this._input == null)
		{
			UnityEngine.Debug.LogError("input not found");
		}
	}

	private void OnEnable()
	{
		if (this._onlyOnce && this._alreadyTurned)
		{
			return;
		}
		base.StartCoroutine(this.SetSelected());
		this._alreadyTurned = true;
	}

	[DebuggerHidden]
	private IEnumerator SetSelected()
	{
		UIInputFocusAtEnable.u003cSetSelectedu003ec__Iterator199 variable = null;
		return variable;
	}
}