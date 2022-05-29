using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoresLabel : MonoBehaviour
{
	private UILabel _label;

	private bool isHunger;

	private string scoreLocalize;

	public ScoresLabel()
	{
	}

	private void Start()
	{
		this.isHunger = Defs.isHunger;
		base.gameObject.SetActive((Defs.IsSurvival || Defs.isCOOP ? true : this.isHunger));
		this._label = base.GetComponent<UILabel>();
		this.scoreLocalize = (!this.isHunger ? LocalizationStore.Key_0190 : LocalizationStore.Key_0351);
	}

	private void Update()
	{
		if (!this.isHunger)
		{
			this._label.text = string.Format("{0}", GlobalGameController.Score);
		}
		else
		{
			this._label.text = string.Format("{0}", (Initializer.players == null ? 0 : Initializer.players.Count - 1));
		}
	}
}