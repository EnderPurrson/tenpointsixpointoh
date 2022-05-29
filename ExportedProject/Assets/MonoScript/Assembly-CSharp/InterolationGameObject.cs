using Photon;
using System;
using UnityEngine;

public class InterolationGameObject : Photon.MonoBehaviour
{
	public int historyLengh = 5;

	public bool isSynchPosition;

	public bool isSynchRotation;

	public bool isLocalTrasformSynch;

	public bool syncOneAxis;

	public bool sglajEnabled;

	private Quaternion correctPlayerRot;

	private Vector3 correctPlayerPos;

	private double correctPlayerTime;

	private double myTime;

	private Transform myTransform;

	private InterolationGameObject.MovementHistoryEntry[] movementHistory;

	private bool isHitoryClear = true;

	private bool isMine;

	public InterolationGameObject()
	{
	}

	private void AddNewSnapshot(Vector3 playerPos, Quaternion playerRot, double timeStamp)
	{
		for (int i = (int)this.movementHistory.Length - 1; i > 0; i--)
		{
			this.movementHistory[i] = this.movementHistory[i - 1];
		}
		this.movementHistory[0].playerPos = playerPos;
		this.movementHistory[0].playerRot = playerRot;
		this.movementHistory[0].timeStamp = timeStamp;
		if (this.isHitoryClear && this.movementHistory[(int)this.movementHistory.Length - 1].timeStamp > this.myTime)
		{
			this.isHitoryClear = false;
			this.myTime = this.movementHistory[(int)this.movementHistory.Length - 1].timeStamp;
		}
	}

	private void Awake()
	{
		if (!Defs.isMulti)
		{
			base.enabled = false;
		}
		this.myTransform = base.transform;
		this.movementHistory = new InterolationGameObject.MovementHistoryEntry[this.historyLengh];
		for (int i = 0; i < this.historyLengh; i++)
		{
			this.movementHistory[i].timeStamp = 0;
		}
		this.myTime = 1;
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!stream.isWriting)
		{
			if (this.isSynchPosition)
			{
				this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			}
			if (this.isSynchRotation)
			{
				if (!this.syncOneAxis)
				{
					this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
				}
				else
				{
					this.correctPlayerRot = Quaternion.Euler((float)stream.ReceiveNext(), 0f, 0f);
				}
			}
			this.correctPlayerTime = (double)stream.ReceiveNext();
			this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime);
		}
		else
		{
			if (this.isSynchPosition)
			{
				stream.SendNext((!this.isLocalTrasformSynch ? this.myTransform.position : this.myTransform.localPosition));
			}
			if (this.isSynchRotation)
			{
				if (!this.syncOneAxis)
				{
					stream.SendNext((!this.isLocalTrasformSynch ? this.myTransform.rotation : this.myTransform.localRotation));
				}
				else
				{
					stream.SendNext(this.myTransform.localRotation.eulerAngles.x);
				}
			}
			stream.SendNext(PhotonNetwork.time);
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (!stream.isWriting)
		{
			Vector3 vector3 = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			float single = 0f;
			if (this.isSynchPosition)
			{
				stream.Serialize(ref vector3);
			}
			if (this.isSynchRotation)
			{
				stream.Serialize(ref quaternion);
			}
			this.correctPlayerPos = vector3;
			this.correctPlayerRot = quaternion;
			stream.Serialize(ref single);
			this.correctPlayerTime = (double)single;
			this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime);
		}
		else
		{
			Vector3 vector31 = (!this.isLocalTrasformSynch ? this.myTransform.position : this.myTransform.localPosition);
			Quaternion quaternion1 = (!this.isLocalTrasformSynch ? this.myTransform.rotation : this.myTransform.localRotation);
			if (this.isSynchPosition)
			{
				stream.Serialize(ref vector31);
			}
			if (this.isSynchRotation)
			{
				stream.Serialize(ref quaternion1);
			}
			float single1 = (float)Network.time;
			stream.Serialize(ref single1);
		}
	}

	private void Start()
	{
		if (Defs.isInet && base.photonView.isMine || !Defs.isInet && base.GetComponent<NetworkView>().isMine)
		{
			this.isMine = true;
		}
	}

	private void Update()
	{
		double num;
		if (!this.isMine)
		{
			if (this.sglajEnabled && !this.isHitoryClear)
			{
				num = (this.myTime + (double)Time.deltaTime >= this.movementHistory[(int)this.movementHistory.Length - 1].timeStamp ? this.myTime + (double)Time.deltaTime : this.myTime + (double)(Time.deltaTime * 1.5f));
				int num1 = 0;
				int num2 = 0;
				while (num2 < (int)this.movementHistory.Length)
				{
					if (this.movementHistory[num2].timeStamp <= this.myTime)
					{
						break;
					}
					else
					{
						num1 = num2;
						num2++;
					}
				}
				if (num1 == 0)
				{
					this.isHitoryClear = true;
				}
				float single = (float)((num - this.myTime) / (this.movementHistory[num1].timeStamp - this.myTime));
				if (!this.isLocalTrasformSynch)
				{
					if (this.isSynchPosition)
					{
						this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.movementHistory[num1].playerPos, single);
					}
					if (this.isSynchRotation)
					{
						this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, this.movementHistory[num1].playerRot, single);
					}
				}
				else
				{
					if (this.isSynchPosition)
					{
						this.myTransform.localPosition = Vector3.Lerp(this.myTransform.localPosition, this.movementHistory[num1].playerPos, single);
					}
					if (this.isSynchRotation)
					{
						this.myTransform.localRotation = Quaternion.Lerp(this.myTransform.localRotation, this.movementHistory[num1].playerRot, single);
					}
				}
				this.myTime = num;
			}
			else if (!this.isHitoryClear)
			{
				if (!this.isLocalTrasformSynch)
				{
					if (this.isSynchPosition)
					{
						this.myTransform.position = this.movementHistory[(int)this.movementHistory.Length - 1].playerPos;
					}
					if (this.isSynchRotation)
					{
						this.myTransform.rotation = this.movementHistory[(int)this.movementHistory.Length - 1].playerRot;
					}
				}
				else
				{
					if (this.isSynchPosition)
					{
						this.myTransform.localPosition = this.movementHistory[(int)this.movementHistory.Length - 1].playerPos;
					}
					if (this.isSynchRotation)
					{
						this.myTransform.localRotation = this.movementHistory[(int)this.movementHistory.Length - 1].playerRot;
					}
				}
				this.myTime = this.movementHistory[(int)this.movementHistory.Length - 1].timeStamp;
			}
		}
	}

	private struct MovementHistoryEntry
	{
		public Vector3 playerPos;

		public Quaternion playerRot;

		public double timeStamp;
	}
}