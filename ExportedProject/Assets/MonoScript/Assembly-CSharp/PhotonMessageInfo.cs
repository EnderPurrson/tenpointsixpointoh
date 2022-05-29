using System;

public class PhotonMessageInfo
{
	private int timeInt;

	public PhotonPlayer sender;

	public PhotonView photonView;

	public double timestamp
	{
		get
		{
			return (double)((float)this.timeInt) / 1000;
		}
	}

	public PhotonMessageInfo()
	{
		this.sender = PhotonNetwork.player;
		this.timeInt = (int)(PhotonNetwork.time * 1000);
		this.photonView = null;
	}

	public PhotonMessageInfo(PhotonPlayer player, int timestamp, PhotonView view)
	{
		this.sender = player;
		this.timeInt = timestamp;
		this.photonView = view;
	}

	public override string ToString()
	{
		return string.Format("[PhotonMessageInfo: Sender='{1}' Senttime={0}]", this.timestamp, this.sender);
	}
}