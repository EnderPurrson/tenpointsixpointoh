using System;
using UnityEngine;

public sealed class NetworkInterpolatedTransform : MonoBehaviour
{
	private bool iskilled;

	private bool oldIsKilled;

	public Player_move_c playerMovec;

	public bool isStartAngel;

	public Vector3 correctPlayerPos;

	public Quaternion correctPlayerRot = Quaternion.identity;

	private Transform myTransform;

	public NetworkInterpolatedTransform()
	{
	}

	private void Awake()
	{
		if (!Defs.isMulti || Defs.isInet)
		{
			base.enabled = false;
		}
		this.correctPlayerPos = new Vector3(0f, -10000f, 0f);
		this.myTransform = base.transform;
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (!stream.isWriting)
		{
			Vector3 vector3 = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			stream.Serialize(ref vector3);
			stream.Serialize(ref quaternion);
			this.correctPlayerPos = vector3;
			this.correctPlayerRot = quaternion;
			this.oldIsKilled = this.iskilled;
			stream.Serialize(ref this.iskilled);
			this.playerMovec.isKilled = this.iskilled;
		}
		else
		{
			Vector3 vector31 = base.transform.localPosition;
			Quaternion quaternion1 = base.transform.localRotation;
			stream.Serialize(ref vector31);
			stream.Serialize(ref quaternion1);
			this.iskilled = this.playerMovec.isKilled;
			stream.Serialize(ref this.iskilled);
		}
	}

	public void StartAngel()
	{
		this.isStartAngel = true;
	}

	private void Update()
	{
		if (!Defs.isInet && !base.GetComponent<NetworkView>().isMine)
		{
			if (this.iskilled)
			{
				if (!this.oldIsKilled)
				{
					this.oldIsKilled = this.iskilled;
					if (!this.isStartAngel)
					{
						this.StartAngel();
					}
					this.isStartAngel = false;
				}
				this.myTransform.position = new Vector3(0f, -1000f, 0f);
			}
			else if (this.oldIsKilled)
			{
				this.myTransform.position = this.correctPlayerPos;
				this.myTransform.rotation = this.correctPlayerRot;
			}
			else
			{
				if (Vector3.SqrMagnitude(this.myTransform.position - this.correctPlayerPos) > 0.04f)
				{
					this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.correctPlayerPos, Time.deltaTime * 5f);
				}
				this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, this.correctPlayerRot, Time.deltaTime * 5f);
			}
			if (this.isStartAngel)
			{
				this.myTransform.position = new Vector3(0f, -1000f, 0f);
			}
		}
	}
}