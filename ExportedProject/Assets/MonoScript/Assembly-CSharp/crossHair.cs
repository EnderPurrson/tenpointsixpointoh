using System;
using UnityEngine;

internal sealed class crossHair : MonoBehaviour
{
	public Texture2D crossHairTexture;

	private Rect crossHairPosition;

	private Pauser pauser;

	private Player_move_c playerMoveC;

	private PhotonView photonView;

	public crossHair()
	{
	}

	private void OnGUI()
	{
		if ((!Defs.isInet && base.GetComponent<NetworkView>().isMine || Defs.isInet && this.photonView.isMine) && Defs.isMulti || !Defs.isMulti)
		{
			if (this.pauser.paused)
			{
				return;
			}
			GUI.DrawTexture(this.crossHairPosition, this.crossHairTexture);
		}
	}

	private void Start()
	{
		this.photonView = PhotonView.Get(this);
		if ((!Defs.isInet && base.GetComponent<NetworkView>().isMine || Defs.isInet && this.photonView.isMine) && Defs.isMulti || !Defs.isMulti)
		{
			this.crossHairPosition = new Rect((float)((Screen.width - this.crossHairTexture.width * Screen.height / 640) / 2), (float)((Screen.height - this.crossHairTexture.height * Screen.height / 640) / 2), (float)(this.crossHairTexture.width * Screen.height / 640), (float)(this.crossHairTexture.height * Screen.height / 640));
			this.pauser = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>();
			this.playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		}
	}
}