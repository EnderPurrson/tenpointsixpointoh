using System;
using System.Collections.Generic;
using UnityEngine;

public class BlueRedButtonController : MonoBehaviour
{
	public UIButton blueButton;

	public UIButton redButton;

	public bool isBlueAvalible = true;

	public bool isRedAvalible = true;

	public int countBlue;

	public int countRed;

	public BlueRedButtonController()
	{
	}

	private void Start()
	{
		if (!Defs.isFlag && !Defs.isCompany && !Defs.isCapturePoints)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		this.countBlue = 0;
		this.countRed = 0;
		for (int i = 0; i < Initializer.networkTables.Count; i++)
		{
			if (Initializer.networkTables[i].myCommand == 1)
			{
				this.countBlue++;
			}
			if (Initializer.networkTables[i].myCommand == 2)
			{
				this.countRed++;
			}
		}
		this.isBlueAvalible = true;
		this.isRedAvalible = true;
		if (PhotonNetwork.room != null && (this.countBlue >= PhotonNetwork.room.maxPlayers / 2 || this.countBlue - this.countRed > 1))
		{
			this.isBlueAvalible = false;
		}
		if (PhotonNetwork.room != null && (this.countRed >= PhotonNetwork.room.maxPlayers / 2 || this.countRed - this.countBlue > 1))
		{
			this.isRedAvalible = false;
		}
		if (this.isBlueAvalible != this.blueButton.isEnabled)
		{
			this.blueButton.isEnabled = this.isBlueAvalible;
		}
		if (this.isRedAvalible != this.redButton.isEnabled)
		{
			this.redButton.isEnabled = this.isRedAvalible;
		}
	}
}