using Photon;
using System;
using UnityEngine;

public class ThirdPersonNetwork1 : Photon.MonoBehaviour
{
	private ThirdPersonCamera cameraScript;

	private ThirdPersonController controllerScript;

	private bool iskilled;

	private bool oldIsKilled;

	public bool sglajEnabled;

	public bool sglajEnabledVidos;

	private Vector3 correctPlayerPos;

	private double correctPlayerTime;

	private Quaternion correctPlayerRot = Quaternion.identity;

	public Player_move_c playerMovec;

	public bool isStartAngel;

	private Transform myTransform;

	private double myTime;

	private ThirdPersonNetwork1.MovementHistoryEntry[] movementHistory;

	private int historyLengh = 5;

	private bool isHitoryClear = true;

	public int myAnim;

	private int myAnimOld;

	public SkinName skinName;

	public bool weAreSteals;

	public bool isTeleported;

	private bool isFirstSnapshot = true;

	private bool isMine;

	private bool isFirstHistoryFull;

	public ThirdPersonNetwork1()
	{
	}

	private void AddNewSnapshot(Vector3 playerPos, Quaternion playerRot, double timeStamp, int _anim, bool _weAreSteals)
	{
		for (int i = (int)this.movementHistory.Length - 1; i > 0; i--)
		{
			this.movementHistory[i] = this.movementHistory[i - 1];
		}
		this.movementHistory[0].playerPos = playerPos;
		this.movementHistory[0].playerRot = playerRot;
		this.movementHistory[0].timeStamp = timeStamp;
		this.movementHistory[0].anim = _anim;
		this.movementHistory[0].weAreSteals = _weAreSteals;
		if (this.isHitoryClear && this.movementHistory[(int)this.movementHistory.Length - 1].timeStamp > this.myTime)
		{
			this.isHitoryClear = false;
			this.myTime = this.movementHistory[(int)this.movementHistory.Length - 1].timeStamp;
			if (!this.isFirstHistoryFull)
			{
				this.myTransform.position = this.movementHistory[(int)this.movementHistory.Length - 1].playerPos;
				this.myTransform.rotation = this.movementHistory[(int)this.movementHistory.Length - 1].playerRot;
				this.isFirstHistoryFull = true;
			}
		}
	}

	private void Awake()
	{
		if (!Defs.isMulti)
		{
			base.enabled = false;
		}
		this.myTransform = base.transform;
		this.correctPlayerPos = new Vector3(0f, -10000f, 0f);
		this.movementHistory = new ThirdPersonNetwork1.MovementHistoryEntry[this.historyLengh];
		for (int i = 0; i < this.historyLengh; i++)
		{
			this.movementHistory[i].timeStamp = 0;
		}
		this.myTime = 1;
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			this.iskilled = this.playerMovec.isKilled;
			if (this.playerMovec.CurHealth <= 0f)
			{
				this.iskilled = true;
			}
			stream.SendNext(this.myTransform.position);
			stream.SendNext(this.myTransform.rotation.eulerAngles.y);
			stream.SendNext(this.iskilled);
			stream.SendNext(PhotonNetwork.time);
			stream.SendNext(this.myAnim);
			stream.SendNext(EffectsController.WeAreStealth);
			stream.SendNext(this.playerMovec.isImmortality);
			stream.SendNext(this.isTeleported);
			this.isTeleported = false;
		}
		else if (this.isFirstSnapshot)
		{
			this.isFirstSnapshot = false;
		}
		else
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = Quaternion.Euler(0f, (float)stream.ReceiveNext(), 0f);
			this.oldIsKilled = this.iskilled;
			this.iskilled = (bool)stream.ReceiveNext();
			this.playerMovec.isKilled = this.iskilled;
			int num = 0;
			bool flag = false;
			this.correctPlayerTime = (double)stream.ReceiveNext();
			if (this.iskilled || Mathf.Abs((float)this.myTime - (float)this.correctPlayerTime) > 1000f)
			{
				this.isHitoryClear = true;
				this.myTime = this.correctPlayerTime;
			}
			num = (int)stream.ReceiveNext();
			flag = (bool)stream.ReceiveNext();
			this.playerMovec.isImmortality = (bool)stream.ReceiveNext();
			this.isTeleported = (bool)stream.ReceiveNext();
			if (this.isTeleported)
			{
				this.oldIsKilled = true;
			}
			this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime, num, flag);
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (!stream.isWriting)
		{
			Vector3 vector3 = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			float single = 0f;
			stream.Serialize(ref vector3);
			stream.Serialize(ref quaternion);
			this.correctPlayerPos = vector3;
			this.correctPlayerRot = quaternion;
			this.oldIsKilled = this.iskilled;
			stream.Serialize(ref this.iskilled);
			this.playerMovec.isKilled = this.iskilled;
			stream.Serialize(ref single);
			this.correctPlayerTime = (double)single;
			if (this.iskilled)
			{
				this.isHitoryClear = true;
				this.myTime = this.correctPlayerTime;
			}
			int num = 0;
			stream.Serialize(ref num);
			bool flag = false;
			stream.Serialize(ref flag);
			bool flag1 = false;
			stream.Serialize(ref flag1);
			this.playerMovec.isImmortality = flag1;
			stream.Serialize(ref this.isTeleported);
			if (this.isTeleported)
			{
				this.oldIsKilled = true;
			}
			this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime, num, flag);
		}
		else
		{
			Vector3 vector31 = this.myTransform.position;
			Quaternion quaternion1 = this.myTransform.rotation;
			stream.Serialize(ref vector31);
			stream.Serialize(ref quaternion1);
			this.iskilled = this.playerMovec.isKilled;
			stream.Serialize(ref this.iskilled);
			float single1 = (float)Network.time;
			stream.Serialize(ref single1);
			int num1 = this.myAnim;
			stream.Serialize(ref num1);
			bool weAreStealth = EffectsController.WeAreStealth;
			stream.Serialize(ref weAreStealth);
			bool flag2 = this.playerMovec.isImmortality;
			stream.Serialize(ref flag2);
			stream.Serialize(ref this.isTeleported);
			this.isTeleported = false;
		}
	}

	private void Start()
	{
		if (Defs.isInet && base.photonView.isMine || !Defs.isInet && base.GetComponent<NetworkView>().isMine)
		{
			this.isMine = true;
		}
	}

	public void StartAngel()
	{
		this.isStartAngel = true;
	}

	private void Update()
	{
		double num;
		if (!this.isMine)
		{
			if (!this.playerMovec.isWeaponSet && this.myTransform.position.y > -8000f)
			{
				this.myTransform.position = new Vector3(0f, -10000f, 0f);
				return;
			}
			if (this.iskilled)
			{
				if (!this.oldIsKilled)
				{
					this.oldIsKilled = this.iskilled;
					this.isStartAngel = false;
				}
				if (this.myTransform.position.y > -8000f)
				{
					this.myTransform.position = new Vector3(0f, -10000f, 0f);
				}
			}
			else if (!this.oldIsKilled && !this.isHitoryClear && (this.sglajEnabled || this.sglajEnabledVidos || this.playerMovec.isInvisible))
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
				if (this.movementHistory[num1].timeStamp - this.myTime <= 4 || num1 <= 0)
				{
					float single = (float)((num - this.myTime) / (this.movementHistory[num1].timeStamp - this.myTime));
					this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.movementHistory[num1].playerPos, single);
					if (Device.isPixelGunLow)
					{
						this.myTransform.rotation = this.movementHistory[num1].playerRot;
					}
					else
					{
						this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, this.movementHistory[num1].playerRot, single);
					}
					this.myTime = num;
					if (this.myAnim != this.movementHistory[num1].anim)
					{
						this.skinName.SetAnim(this.movementHistory[num1].anim, this.movementHistory[num1].weAreSteals);
						this.myAnim = this.movementHistory[num1].anim;
					}
				}
				else
				{
					num1--;
					this.myTransform.position = this.movementHistory[num1].playerPos;
					this.myTransform.rotation = this.movementHistory[num1].playerRot;
					this.myTime = this.movementHistory[num1].timeStamp;
				}
			}
			else if (!this.isHitoryClear)
			{
				this.myTransform.position = this.movementHistory[(int)this.movementHistory.Length - 1].playerPos;
				this.myTransform.rotation = this.movementHistory[(int)this.movementHistory.Length - 1].playerRot;
				this.myTime = this.movementHistory[(int)this.movementHistory.Length - 1].timeStamp;
			}
			if (this.isStartAngel && this.myTransform.position.y > -8000f)
			{
				this.myTransform.position = new Vector3(0f, -10000f, 0f);
			}
		}
	}

	private struct MovementHistoryEntry
	{
		public Vector3 playerPos;

		public Quaternion playerRot;

		public int anim;

		public bool weAreSteals;

		public double timeStamp;
	}
}