using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DinamicDayCicle : MonoBehaviour
{
	public MaterialToChange[] matToChange;

	public float lerpFactor;

	private int nextCicle;

	private float cicleTime;

	public int currentCicle;

	private float matchTime;

	private float timeDelta;

	public DinamicDayCicle()
	{
	}

	[DebuggerHidden]
	private IEnumerator MatColorChange()
	{
		DinamicDayCicle.u003cMatColorChangeu003ec__Iterator22 variable = null;
		return variable;
	}

	private void ResetColors()
	{
		MaterialToChange[] materialToChangeArray = this.matToChange;
		for (int i = 0; i < (int)materialToChangeArray.Length; i++)
		{
			MaterialToChange materialToChange = materialToChangeArray[i];
			if (materialToChange.changecolor)
			{
				materialToChange.currentColor = materialToChange.cicleColors[0];
			}
			if (materialToChange.cicleLerp != null && (int)materialToChange.cicleLerp.Length == (int)materialToChange.cicleColors.Length)
			{
				materialToChange.currentLerp = materialToChange.cicleLerp[0];
			}
		}
	}

	private void Start()
	{
		this.ResetColors();
		base.StartCoroutine(this.MatColorChange());
	}

	private void Update()
	{
		if (!(TimeGameController.sharedController != null) || PhotonNetwork.room == null || string.IsNullOrEmpty(ConnectSceneNGUIController.maxKillProperty))
		{
			this.ResetColors();
		}
		else if (PhotonNetwork.room.customProperties.ContainsKey(ConnectSceneNGUIController.maxKillProperty))
		{
			int num = -1;
			int.TryParse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString(), out num);
			if (num < 0)
			{
				this.ResetColors();
				return;
			}
			this.matchTime = (float)num * 60f;
			if ((float)TimeGameController.sharedController.timerToEndMatch < this.matchTime)
			{
				this.timeDelta = this.matchTime - (float)TimeGameController.sharedController.timerToEndMatch;
				if (this.matchTime != this.timeDelta)
				{
					MaterialToChange[] materialToChangeArray = this.matToChange;
					for (int i = 0; i < (int)materialToChangeArray.Length; i++)
					{
						MaterialToChange materialToChange = materialToChangeArray[i];
						this.cicleTime = this.matchTime / (float)((int)materialToChange.cicleColors.Length);
						this.currentCicle = Mathf.FloorToInt(this.timeDelta / this.matchTime * (float)((int)materialToChange.cicleColors.Length));
						this.nextCicle = Mathf.Min(this.currentCicle + 1, (int)materialToChange.cicleColors.Length - 1);
						this.lerpFactor = (this.timeDelta - this.cicleTime * (float)this.currentCicle) / this.cicleTime;
						if (materialToChange.changecolor && this.currentCicle < (int)materialToChange.cicleColors.Length)
						{
							materialToChange.currentColor = Color.Lerp(materialToChange.cicleColors[this.currentCicle], materialToChange.cicleColors[this.nextCicle], this.lerpFactor);
						}
						if (materialToChange.cicleLerp != null && (int)materialToChange.cicleLerp.Length == (int)materialToChange.cicleColors.Length && this.currentCicle < (int)materialToChange.cicleColors.Length)
						{
							materialToChange.currentLerp = Mathf.Lerp(materialToChange.cicleLerp[this.currentCicle], materialToChange.cicleLerp[this.nextCicle], this.lerpFactor);
						}
					}
				}
			}
		}
	}
}