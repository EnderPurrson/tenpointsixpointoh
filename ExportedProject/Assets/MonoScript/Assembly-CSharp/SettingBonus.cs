using Photon;
using System;
using UnityEngine;

public class SettingBonus : Photon.MonoBehaviour
{
	public int typeOfMass;

	public int numberSpawnZone = -1;

	public SettingBonus()
	{
	}

	public void SetNumberSpawnZone(int _number)
	{
		base.photonView.RPC("SynchNamberSpawnZoneRPC", PhotonTargets.AllBuffered, new object[] { _number });
	}

	private void Start()
	{
	}

	[PunRPC]
	[RPC]
	public void SynchNamberSpawnZoneRPC(int _number)
	{
		this.numberSpawnZone = _number;
	}

	private void Update()
	{
	}
}