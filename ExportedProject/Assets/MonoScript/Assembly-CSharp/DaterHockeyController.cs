using System;
using UnityEngine;

public class DaterHockeyController : MonoBehaviour
{
	public float coefForce = 200f;

	public int score1;

	public int score2;

	private bool isForceMyPlayer;

	private float timeSendForce = 0.3f;

	private float timerToSendForce = -1f;

	private PhotonView photonView;

	private Rigidbody thisRigidbody;

	private Transform thisTransform;

	private Vector3 resetPositionPoint;

	private bool isFirstSynhPos = true;

	private bool isResetPosition;

	private Vector3 synchPos;

	private Quaternion synchRot;

	private bool isMine;

	public DaterHockeyController()
	{
	}

	private void AddForce(Vector3 _force)
	{
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("AddForceRPC", RPCMode.Server, new object[] { _force });
			this.AddForceRPC(_force);
		}
		else
		{
			this.photonView.RPC("AddForceRPC", PhotonTargets.All, new object[] { _force });
		}
	}

	[PunRPC]
	[RPC]
	private void AddForceRPC(Vector3 _force)
	{
		base.GetComponent<Rigidbody>().AddForce(_force);
	}

	private void Awake()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.thisRigidbody = base.GetComponent<Rigidbody>();
		this.thisTransform = base.transform;
		this.resetPositionPoint = this.thisTransform.position;
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!stream.isWriting)
		{
			this.synchPos = (Vector3)stream.ReceiveNext();
			this.synchRot = (Quaternion)stream.ReceiveNext();
			this.thisRigidbody.velocity = (Vector3)stream.ReceiveNext();
			this.thisRigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
			this.isResetPosition = (bool)stream.ReceiveNext();
			if (this.isFirstSynhPos)
			{
				this.thisTransform.position = this.synchPos;
				this.thisTransform.rotation = this.synchRot;
				this.isFirstSynhPos = false;
				this.isResetPosition = false;
			}
		}
		else
		{
			stream.SendNext(this.thisTransform.position);
			stream.SendNext(this.thisTransform.rotation);
			stream.SendNext(this.thisRigidbody.velocity);
			stream.SendNext(this.thisRigidbody.angularVelocity);
			stream.SendNext(this.isResetPosition);
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (!stream.isWriting)
		{
			Vector3 vector3 = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			bool flag = false;
			stream.Serialize(ref vector3);
			stream.Serialize(ref quaternion);
			stream.Serialize(ref flag);
			this.synchPos = vector3;
			this.synchRot = quaternion;
			this.isResetPosition = flag;
			if (this.isFirstSynhPos || this.isResetPosition)
			{
				this.thisTransform.position = this.synchPos;
				this.thisTransform.rotation = this.synchRot;
				this.isFirstSynhPos = false;
				this.isResetPosition = false;
			}
		}
		else
		{
			Vector3 vector31 = this.thisTransform.position;
			Quaternion quaternion1 = this.thisTransform.rotation;
			stream.Serialize(ref vector31);
			stream.Serialize(ref quaternion1);
			bool flag1 = this.isResetPosition;
			stream.Serialize(ref flag1);
			if (this.isResetPosition)
			{
				this.isResetPosition = false;
			}
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.Equals(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform))
		{
			this.isForceMyPlayer = true;
			return;
		}
		if (this.isMine && collider.gameObject.name.Equals("Gates1"))
		{
			this.ResetPosition();
		}
		if (this.isMine && collider.gameObject.name.Equals("Gates2"))
		{
			this.ResetPosition();
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.Equals(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform))
		{
			this.isForceMyPlayer = false;
		}
		if (this.isMine && collider.gameObject.name.Equals("Stadium"))
		{
			this.ResetPosition();
		}
	}

	private void ResetPosition()
	{
		this.thisTransform.position = this.resetPositionPoint;
		this.thisRigidbody.velocity = Vector3.zero;
		this.thisRigidbody.angularVelocity = Vector3.zero;
		this.isResetPosition = true;
	}

	private void Start()
	{
		bool flag;
		if (!Defs.isMulti || !Defs.isInet && base.GetComponent<NetworkView>().isMine)
		{
			flag = true;
		}
		else
		{
			flag = (!Defs.isInet ? false : this.photonView.isMine);
		}
		this.isMine = flag;
	}

	private void Update()
	{
		if (this.isForceMyPlayer && WeaponManager.sharedManager.myPlayer == null)
		{
			this.isForceMyPlayer = false;
		}
		if (this.isForceMyPlayer)
		{
			this.timerToSendForce -= Time.deltaTime;
			if (this.timerToSendForce < 0f)
			{
				this.timerToSendForce = this.timeSendForce;
				this.AddForce(Vector3.Normalize(this.thisTransform.position - WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform.position) * this.coefForce);
			}
		}
		if (!this.isMine)
		{
			this.thisTransform.position = Vector3.Lerp(this.thisTransform.position, this.synchPos, Time.deltaTime * 5f);
			this.thisTransform.rotation = Quaternion.Lerp(this.thisTransform.rotation, this.synchRot, Time.deltaTime * 5f);
		}
	}
}