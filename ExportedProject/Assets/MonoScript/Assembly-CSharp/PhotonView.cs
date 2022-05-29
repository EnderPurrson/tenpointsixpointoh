using Photon;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Photon Networking/Photon View &v")]
public class PhotonView : Photon.MonoBehaviour
{
	public int ownerId;

	public int @group;

	protected internal bool mixedModeIsReliable;

	public int prefixBackup = -1;

	internal object[] instantiationDataField;

	protected internal object[] lastOnSerializeDataSent;

	protected internal object[] lastOnSerializeDataReceived;

	public Component observed;

	public ViewSynchronization synchronization;

	public OnSerializeTransform onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;

	public OnSerializeRigidBody onSerializeRigidBodyOption = OnSerializeRigidBody.All;

	public OwnershipOption ownershipTransfer;

	public List<Component> ObservedComponents;

	private Dictionary<Component, MethodInfo> m_OnSerializeMethodInfos = new Dictionary<Component, MethodInfo>();

	[SerializeField]
	private int viewIdField;

	public int instantiationId;

	protected internal bool didAwake;

	[SerializeField]
	protected internal bool isRuntimeInstantiated;

	protected internal bool removedFromLocalViewList;

	internal UnityEngine.MonoBehaviour[] RpcMonoBehaviours;

	private MethodInfo OnSerializeMethodInfo;

	private bool failedToFindOnSerialize;

	public int CreatorActorNr
	{
		get
		{
			return this.viewIdField / PhotonNetwork.MAX_VIEW_IDS;
		}
	}

	public object[] instantiationData
	{
		get
		{
			if (!this.didAwake)
			{
				this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
			}
			return this.instantiationDataField;
		}
		set
		{
			this.instantiationDataField = value;
		}
	}

	public bool isMine
	{
		get
		{
			bool flag;
			if (this.ownerId == PhotonNetwork.player.ID)
			{
				flag = true;
			}
			else
			{
				flag = (this.isOwnerActive ? false : PhotonNetwork.isMasterClient);
			}
			return flag;
		}
	}

	public bool isOwnerActive
	{
		get
		{
			return (this.ownerId == 0 ? false : PhotonNetwork.networkingPeer.mActors.ContainsKey(this.ownerId));
		}
	}

	public bool isSceneView
	{
		get
		{
			return this.CreatorActorNr == 0;
		}
	}

	public PhotonPlayer owner
	{
		get
		{
			return PhotonPlayer.Find(this.ownerId);
		}
	}

	public int OwnerActorNr
	{
		get
		{
			return this.ownerId;
		}
	}

	public int prefix
	{
		get
		{
			if (this.prefixBackup == -1 && PhotonNetwork.networkingPeer != null)
			{
				this.prefixBackup = PhotonNetwork.networkingPeer.currentLevelPrefix;
			}
			return this.prefixBackup;
		}
		set
		{
			this.prefixBackup = value;
		}
	}

	public int viewID
	{
		get
		{
			return this.viewIdField;
		}
		set
		{
			bool flag = (!this.didAwake ? false : this.viewIdField == 0);
			this.ownerId = value / PhotonNetwork.MAX_VIEW_IDS;
			this.viewIdField = value;
			if (flag)
			{
				PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			}
		}
	}

	public PhotonView()
	{
	}

	protected internal void Awake()
	{
		if (this.viewID != 0)
		{
			PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			this.instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(this.instantiationId);
		}
		this.didAwake = true;
	}

	protected internal void DeserializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component == null)
		{
			return;
		}
		if (component is UnityEngine.MonoBehaviour)
		{
			this.ExecuteComponentOnSerialize(component, stream, info);
		}
		else if (component is Transform)
		{
			Transform transforms = (Transform)component;
			switch (this.onSerializeTransformOption)
			{
				case OnSerializeTransform.OnlyPosition:
				{
					transforms.localPosition = (Vector3)stream.ReceiveNext();
					break;
				}
				case OnSerializeTransform.OnlyRotation:
				{
					transforms.localRotation = (Quaternion)stream.ReceiveNext();
					break;
				}
				case OnSerializeTransform.OnlyScale:
				{
					transforms.localScale = (Vector3)stream.ReceiveNext();
					break;
				}
				case OnSerializeTransform.PositionAndRotation:
				{
					transforms.localPosition = (Vector3)stream.ReceiveNext();
					transforms.localRotation = (Quaternion)stream.ReceiveNext();
					break;
				}
				case OnSerializeTransform.All:
				{
					transforms.localPosition = (Vector3)stream.ReceiveNext();
					transforms.localRotation = (Quaternion)stream.ReceiveNext();
					transforms.localScale = (Vector3)stream.ReceiveNext();
					break;
				}
			}
		}
		else if (component is Rigidbody)
		{
			Rigidbody rigidbody = (Rigidbody)component;
			switch (this.onSerializeRigidBodyOption)
			{
				case OnSerializeRigidBody.OnlyVelocity:
				{
					rigidbody.velocity = (Vector3)stream.ReceiveNext();
					break;
				}
				case OnSerializeRigidBody.OnlyAngularVelocity:
				{
					rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
					break;
				}
				case OnSerializeRigidBody.All:
				{
					rigidbody.velocity = (Vector3)stream.ReceiveNext();
					rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
					break;
				}
			}
		}
		else if (!(component is Rigidbody2D))
		{
			Debug.LogError("Type of observed is unknown when receiving.");
		}
		else
		{
			Rigidbody2D rigidbody2D = (Rigidbody2D)component;
			switch (this.onSerializeRigidBodyOption)
			{
				case OnSerializeRigidBody.OnlyVelocity:
				{
					rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
					break;
				}
				case OnSerializeRigidBody.OnlyAngularVelocity:
				{
					rigidbody2D.angularVelocity = (float)stream.ReceiveNext();
					break;
				}
				case OnSerializeRigidBody.All:
				{
					rigidbody2D.velocity = (Vector2)stream.ReceiveNext();
					rigidbody2D.angularVelocity = (float)stream.ReceiveNext();
					break;
				}
			}
		}
	}

	public void DeserializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		this.DeserializeComponent(this.observed, stream, info);
		if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
		{
			for (int i = 0; i < this.ObservedComponents.Count; i++)
			{
				this.DeserializeComponent(this.ObservedComponents[i], stream, info);
			}
		}
	}

	protected internal void ExecuteComponentOnSerialize(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component != null)
		{
			MethodInfo methodInfo = null;
			if (!this.m_OnSerializeMethodInfos.TryGetValue(component, out methodInfo))
			{
				if (!NetworkingPeer.GetMethod(component as UnityEngine.MonoBehaviour, PhotonNetworkingMessage.OnPhotonSerializeView.ToString(), out methodInfo))
				{
					Debug.LogError(string.Concat("The observed monobehaviour (", component.name, ") of this PhotonView does not implement OnPhotonSerializeView()!"));
					methodInfo = null;
				}
				this.m_OnSerializeMethodInfos.Add(component, methodInfo);
			}
			if (methodInfo != null)
			{
				methodInfo.Invoke(component, new object[] { stream, info });
			}
		}
	}

	public static PhotonView Find(int viewID)
	{
		return PhotonNetwork.networkingPeer.GetPhotonView(viewID);
	}

	public static PhotonView Get(Component component)
	{
		return component.GetComponent<PhotonView>();
	}

	public static PhotonView Get(GameObject gameObj)
	{
		return gameObj.GetComponent<PhotonView>();
	}

	protected internal void OnDestroy()
	{
		if (!this.removedFromLocalViewList)
		{
			bool flag = PhotonNetwork.networkingPeer.LocalCleanPhotonView(this);
			bool flag1 = false;
			if (flag && !flag1 && this.instantiationId > 0 && !PhotonHandler.AppQuits && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(string.Concat("PUN-instantiated '", base.gameObject.name, "' got destroyed by engine. This is OK when loading levels. Otherwise use: PhotonNetwork.Destroy()."));
			}
		}
	}

	public void RefreshRpcMonoBehaviourCache()
	{
		this.RpcMonoBehaviours = base.GetComponents<UnityEngine.MonoBehaviour>();
	}

	public void RequestOwnership()
	{
		PhotonNetwork.networkingPeer.RequestOwnership(this.viewID, this.ownerId);
	}

	public void RPC(string methodName, PhotonTargets target, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, target, false, parameters);
	}

	public void RPC(string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, false, parameters);
	}

	public void RpcSecure(string methodName, PhotonTargets target, bool encrypt, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, target, encrypt, parameters);
	}

	public void RpcSecure(string methodName, PhotonPlayer targetPlayer, bool encrypt, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, encrypt, parameters);
	}

	protected internal void SerializeComponent(Component component, PhotonStream stream, PhotonMessageInfo info)
	{
		if (component == null)
		{
			return;
		}
		if (component is UnityEngine.MonoBehaviour)
		{
			this.ExecuteComponentOnSerialize(component, stream, info);
		}
		else if (component is Transform)
		{
			Transform transforms = (Transform)component;
			switch (this.onSerializeTransformOption)
			{
				case OnSerializeTransform.OnlyPosition:
				{
					stream.SendNext(transforms.localPosition);
					break;
				}
				case OnSerializeTransform.OnlyRotation:
				{
					stream.SendNext(transforms.localRotation);
					break;
				}
				case OnSerializeTransform.OnlyScale:
				{
					stream.SendNext(transforms.localScale);
					break;
				}
				case OnSerializeTransform.PositionAndRotation:
				{
					stream.SendNext(transforms.localPosition);
					stream.SendNext(transforms.localRotation);
					break;
				}
				case OnSerializeTransform.All:
				{
					stream.SendNext(transforms.localPosition);
					stream.SendNext(transforms.localRotation);
					stream.SendNext(transforms.localScale);
					break;
				}
			}
		}
		else if (component is Rigidbody)
		{
			Rigidbody rigidbody = (Rigidbody)component;
			switch (this.onSerializeRigidBodyOption)
			{
				case OnSerializeRigidBody.OnlyVelocity:
				{
					stream.SendNext(rigidbody.velocity);
					break;
				}
				case OnSerializeRigidBody.OnlyAngularVelocity:
				{
					stream.SendNext(rigidbody.angularVelocity);
					break;
				}
				case OnSerializeRigidBody.All:
				{
					stream.SendNext(rigidbody.velocity);
					stream.SendNext(rigidbody.angularVelocity);
					break;
				}
			}
		}
		else if (!(component is Rigidbody2D))
		{
			Debug.LogError(string.Concat("Observed type is not serializable: ", component.GetType()));
		}
		else
		{
			Rigidbody2D rigidbody2D = (Rigidbody2D)component;
			switch (this.onSerializeRigidBodyOption)
			{
				case OnSerializeRigidBody.OnlyVelocity:
				{
					stream.SendNext(rigidbody2D.velocity);
					break;
				}
				case OnSerializeRigidBody.OnlyAngularVelocity:
				{
					stream.SendNext(rigidbody2D.angularVelocity);
					break;
				}
				case OnSerializeRigidBody.All:
				{
					stream.SendNext(rigidbody2D.velocity);
					stream.SendNext(rigidbody2D.angularVelocity);
					break;
				}
			}
		}
	}

	public void SerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		this.SerializeComponent(this.observed, stream, info);
		if (this.ObservedComponents != null && this.ObservedComponents.Count > 0)
		{
			for (int i = 0; i < this.ObservedComponents.Count; i++)
			{
				this.SerializeComponent(this.ObservedComponents[i], stream, info);
			}
		}
	}

	public override string ToString()
	{
		object[] objArray = new object[] { this.viewID, null, null, null };
		objArray[1] = (base.gameObject == null ? "GO==null" : base.gameObject.name);
		objArray[2] = (!this.isSceneView ? string.Empty : "(scene)");
		objArray[3] = this.prefix;
		return string.Format("View ({3}){0} on {1} {2}", objArray);
	}

	public void TransferOwnership(PhotonPlayer newOwner)
	{
		this.TransferOwnership(newOwner.ID);
	}

	public void TransferOwnership(int newOwnerId)
	{
		PhotonNetwork.networkingPeer.TransferOwnership(this.viewID, newOwnerId);
		this.ownerId = newOwnerId;
	}
}