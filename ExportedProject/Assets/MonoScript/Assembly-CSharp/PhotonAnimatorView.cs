using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Photon Networking/Photon Animator View")]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Animator))]
public class PhotonAnimatorView : MonoBehaviour
{
	private Animator m_Animator;

	private PhotonStreamQueue m_StreamQueue;

	[HideInInspector]
	[SerializeField]
	private bool ShowLayerWeightsInspector = true;

	[HideInInspector]
	[SerializeField]
	private bool ShowParameterInspector = true;

	[HideInInspector]
	[SerializeField]
	private List<PhotonAnimatorView.SynchronizedParameter> m_SynchronizeParameters = new List<PhotonAnimatorView.SynchronizedParameter>();

	[HideInInspector]
	[SerializeField]
	private List<PhotonAnimatorView.SynchronizedLayer> m_SynchronizeLayers = new List<PhotonAnimatorView.SynchronizedLayer>();

	private Vector3 m_ReceiverPosition;

	private float m_LastDeserializeTime;

	private bool m_WasSynchronizeTypeChanged = true;

	private PhotonView m_PhotonView;

	private List<string> m_raisedDiscreteTriggersCache = new List<string>();

	public PhotonAnimatorView()
	{
	}

	private void Awake()
	{
		this.m_PhotonView = base.GetComponent<PhotonView>();
		this.m_StreamQueue = new PhotonStreamQueue(120);
		this.m_Animator = base.GetComponent<Animator>();
	}

	public void CacheDiscreteTriggers()
	{
		int num = 0;
		while (num < this.m_SynchronizeParameters.Count)
		{
			PhotonAnimatorView.SynchronizedParameter item = this.m_SynchronizeParameters[num];
			if (item.SynchronizeType != PhotonAnimatorView.SynchronizeType.Discrete || item.Type != PhotonAnimatorView.ParameterType.Trigger || !this.m_Animator.GetBool(item.Name) || item.Type != PhotonAnimatorView.ParameterType.Trigger)
			{
				num++;
			}
			else
			{
				this.m_raisedDiscreteTriggersCache.Add(item.Name);
				break;
			}
		}
	}

	private void DeserializeDataContinuously()
	{
		if (!this.m_StreamQueue.HasQueuedObjects())
		{
			return;
		}
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				this.m_Animator.SetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex, (float)this.m_StreamQueue.ReceiveNext());
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter item = this.m_SynchronizeParameters[j];
			if (item.SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				PhotonAnimatorView.ParameterType type = item.Type;
				switch (type)
				{
					case PhotonAnimatorView.ParameterType.Float:
					{
						this.m_Animator.SetFloat(item.Name, (float)this.m_StreamQueue.ReceiveNext());
						break;
					}
					case PhotonAnimatorView.ParameterType.Int:
					{
						this.m_Animator.SetInteger(item.Name, (int)this.m_StreamQueue.ReceiveNext());
						break;
					}
					case PhotonAnimatorView.ParameterType.Bool:
					{
						this.m_Animator.SetBool(item.Name, (bool)this.m_StreamQueue.ReceiveNext());
						break;
					}
					default:
					{
						if (type == PhotonAnimatorView.ParameterType.Trigger)
						{
							this.m_Animator.SetBool(item.Name, (bool)this.m_StreamQueue.ReceiveNext());
							break;
						}
						else
						{
							break;
						}
					}
				}
			}
		}
	}

	private void DeserializeDataDiscretly(PhotonStream stream)
	{
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				this.m_Animator.SetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex, (float)stream.ReceiveNext());
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter item = this.m_SynchronizeParameters[j];
			if (item.SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				PhotonAnimatorView.ParameterType type = item.Type;
				switch (type)
				{
					case PhotonAnimatorView.ParameterType.Float:
					{
						if (!(stream.PeekNext() is float))
						{
							return;
						}
						this.m_Animator.SetFloat(item.Name, (float)stream.ReceiveNext());
						break;
					}
					case PhotonAnimatorView.ParameterType.Int:
					{
						if (!(stream.PeekNext() is int))
						{
							return;
						}
						this.m_Animator.SetInteger(item.Name, (int)stream.ReceiveNext());
						break;
					}
					case PhotonAnimatorView.ParameterType.Bool:
					{
						if (!(stream.PeekNext() is bool))
						{
							return;
						}
						this.m_Animator.SetBool(item.Name, (bool)stream.ReceiveNext());
						break;
					}
					default:
					{
						if (type == PhotonAnimatorView.ParameterType.Trigger)
						{
							if (!(stream.PeekNext() is bool))
							{
								return;
							}
							if ((bool)stream.ReceiveNext())
							{
								this.m_Animator.SetTrigger(item.Name);
							}
							break;
						}
						else
						{
							break;
						}
					}
				}
			}
		}
	}

	private void DeserializeSynchronizationTypeState(PhotonStream stream)
	{
		byte[] numArray = (byte[])stream.ReceiveNext();
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			this.m_SynchronizeLayers[i].SynchronizeType = (PhotonAnimatorView.SynchronizeType)numArray[i];
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			this.m_SynchronizeParameters[j].SynchronizeType = (PhotonAnimatorView.SynchronizeType)numArray[this.m_SynchronizeLayers.Count + j];
		}
	}

	public bool DoesLayerSynchronizeTypeExist(int layerIndex)
	{
		return this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex) != -1;
	}

	public bool DoesParameterSynchronizeTypeExist(string name)
	{
		return this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name) != -1;
	}

	public PhotonAnimatorView.SynchronizeType GetLayerSynchronizeType(int layerIndex)
	{
		int num = this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex);
		if (num == -1)
		{
			return PhotonAnimatorView.SynchronizeType.Disabled;
		}
		return this.m_SynchronizeLayers[num].SynchronizeType;
	}

	public PhotonAnimatorView.SynchronizeType GetParameterSynchronizeType(string name)
	{
		int num = this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name);
		if (num == -1)
		{
			return PhotonAnimatorView.SynchronizeType.Disabled;
		}
		return this.m_SynchronizeParameters[num].SynchronizeType;
	}

	public List<PhotonAnimatorView.SynchronizedLayer> GetSynchronizedLayers()
	{
		return this.m_SynchronizeLayers;
	}

	public List<PhotonAnimatorView.SynchronizedParameter> GetSynchronizedParameters()
	{
		return this.m_SynchronizeParameters;
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (this.m_Animator == null)
		{
			return;
		}
		if (!stream.isWriting)
		{
			if (stream.PeekNext() is byte[])
			{
				this.DeserializeSynchronizationTypeState(stream);
			}
			this.m_StreamQueue.Deserialize(stream);
			this.DeserializeDataDiscretly(stream);
		}
		else
		{
			if (this.m_WasSynchronizeTypeChanged)
			{
				this.m_StreamQueue.Reset();
				this.SerializeSynchronizationTypeState(stream);
				this.m_WasSynchronizeTypeChanged = false;
			}
			this.m_StreamQueue.Serialize(stream);
			this.SerializeDataDiscretly(stream);
		}
	}

	private void SerializeDataContinuously()
	{
		if (this.m_Animator == null)
		{
			return;
		}
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				this.m_StreamQueue.SendNext(this.m_Animator.GetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex));
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter item = this.m_SynchronizeParameters[j];
			if (item.SynchronizeType == PhotonAnimatorView.SynchronizeType.Continuous)
			{
				PhotonAnimatorView.ParameterType type = item.Type;
				switch (type)
				{
					case PhotonAnimatorView.ParameterType.Float:
					{
						this.m_StreamQueue.SendNext(this.m_Animator.GetFloat(item.Name));
						break;
					}
					case PhotonAnimatorView.ParameterType.Int:
					{
						this.m_StreamQueue.SendNext(this.m_Animator.GetInteger(item.Name));
						break;
					}
					case PhotonAnimatorView.ParameterType.Bool:
					{
						this.m_StreamQueue.SendNext(this.m_Animator.GetBool(item.Name));
						break;
					}
					default:
					{
						if (type == PhotonAnimatorView.ParameterType.Trigger)
						{
							this.m_StreamQueue.SendNext(this.m_Animator.GetBool(item.Name));
							break;
						}
						else
						{
							break;
						}
					}
				}
			}
		}
	}

	private void SerializeDataDiscretly(PhotonStream stream)
	{
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			if (this.m_SynchronizeLayers[i].SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				stream.SendNext(this.m_Animator.GetLayerWeight(this.m_SynchronizeLayers[i].LayerIndex));
			}
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			PhotonAnimatorView.SynchronizedParameter item = this.m_SynchronizeParameters[j];
			if (item.SynchronizeType == PhotonAnimatorView.SynchronizeType.Discrete)
			{
				PhotonAnimatorView.ParameterType type = item.Type;
				switch (type)
				{
					case PhotonAnimatorView.ParameterType.Float:
					{
						stream.SendNext(this.m_Animator.GetFloat(item.Name));
						break;
					}
					case PhotonAnimatorView.ParameterType.Int:
					{
						stream.SendNext(this.m_Animator.GetInteger(item.Name));
						break;
					}
					case PhotonAnimatorView.ParameterType.Bool:
					{
						stream.SendNext(this.m_Animator.GetBool(item.Name));
						break;
					}
					default:
					{
						if (type == PhotonAnimatorView.ParameterType.Trigger)
						{
							stream.SendNext(this.m_raisedDiscreteTriggersCache.Contains(item.Name));
							break;
						}
						else
						{
							break;
						}
					}
				}
			}
		}
		this.m_raisedDiscreteTriggersCache.Clear();
	}

	private void SerializeSynchronizationTypeState(PhotonStream stream)
	{
		byte[] synchronizeType = new byte[this.m_SynchronizeLayers.Count + this.m_SynchronizeParameters.Count];
		for (int i = 0; i < this.m_SynchronizeLayers.Count; i++)
		{
			synchronizeType[i] = (byte)this.m_SynchronizeLayers[i].SynchronizeType;
		}
		for (int j = 0; j < this.m_SynchronizeParameters.Count; j++)
		{
			synchronizeType[this.m_SynchronizeLayers.Count + j] = (byte)this.m_SynchronizeParameters[j].SynchronizeType;
		}
		stream.SendNext(synchronizeType);
	}

	public void SetLayerSynchronized(int layerIndex, PhotonAnimatorView.SynchronizeType synchronizeType)
	{
		if (Application.isPlaying)
		{
			this.m_WasSynchronizeTypeChanged = true;
		}
		int num = this.m_SynchronizeLayers.FindIndex((PhotonAnimatorView.SynchronizedLayer item) => item.LayerIndex == layerIndex);
		if (num != -1)
		{
			this.m_SynchronizeLayers[num].SynchronizeType = synchronizeType;
		}
		else
		{
			List<PhotonAnimatorView.SynchronizedLayer> mSynchronizeLayers = this.m_SynchronizeLayers;
			PhotonAnimatorView.SynchronizedLayer synchronizedLayer = new PhotonAnimatorView.SynchronizedLayer()
			{
				LayerIndex = layerIndex,
				SynchronizeType = synchronizeType
			};
			mSynchronizeLayers.Add(synchronizedLayer);
		}
	}

	public void SetParameterSynchronized(string name, PhotonAnimatorView.ParameterType type, PhotonAnimatorView.SynchronizeType synchronizeType)
	{
		if (Application.isPlaying)
		{
			this.m_WasSynchronizeTypeChanged = true;
		}
		int num = this.m_SynchronizeParameters.FindIndex((PhotonAnimatorView.SynchronizedParameter item) => item.Name == name);
		if (num != -1)
		{
			this.m_SynchronizeParameters[num].SynchronizeType = synchronizeType;
		}
		else
		{
			List<PhotonAnimatorView.SynchronizedParameter> mSynchronizeParameters = this.m_SynchronizeParameters;
			PhotonAnimatorView.SynchronizedParameter synchronizedParameter = new PhotonAnimatorView.SynchronizedParameter()
			{
				Name = name,
				Type = type,
				SynchronizeType = synchronizeType
			};
			mSynchronizeParameters.Add(synchronizedParameter);
		}
	}

	private void Update()
	{
		if (this.m_Animator.applyRootMotion && !this.m_PhotonView.isMine && PhotonNetwork.connected)
		{
			this.m_Animator.applyRootMotion = false;
		}
		if (!PhotonNetwork.inRoom || PhotonNetwork.room.playerCount <= 1)
		{
			this.m_StreamQueue.Reset();
			return;
		}
		if (!this.m_PhotonView.isMine)
		{
			this.DeserializeDataContinuously();
		}
		else
		{
			this.SerializeDataContinuously();
			this.CacheDiscreteTriggers();
		}
	}

	public enum ParameterType
	{
		Float = 1,
		Int = 3,
		Bool = 4,
		Trigger = 9
	}

	[Serializable]
	public class SynchronizedLayer
	{
		public PhotonAnimatorView.SynchronizeType SynchronizeType;

		public int LayerIndex;

		public SynchronizedLayer()
		{
		}
	}

	[Serializable]
	public class SynchronizedParameter
	{
		public PhotonAnimatorView.ParameterType Type;

		public PhotonAnimatorView.SynchronizeType SynchronizeType;

		public string Name;

		public SynchronizedParameter()
		{
		}
	}

	public enum SynchronizeType
	{
		Disabled,
		Discrete,
		Continuous
	}
}