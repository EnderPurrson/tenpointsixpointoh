using System;
using UnityEngine;

public class LabePlaterNameInSpectatorMode : MonoBehaviour
{
	private UILabel label;

	public UILabel clanNameLabel;

	public UITexture clanTexture;

	public LabePlaterNameInSpectatorMode()
	{
	}

	private void Start()
	{
		this.label = base.GetComponent<UILabel>();
	}

	private void Update()
	{
		if (this.label != null && WeaponManager.sharedManager.myTable != null)
		{
			this.label.text = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().playerVidosNick;
			this.clanNameLabel.text = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().playerVidosClanName;
			this.clanTexture.mainTexture = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().playerVidosClanTexture;
		}
	}
}