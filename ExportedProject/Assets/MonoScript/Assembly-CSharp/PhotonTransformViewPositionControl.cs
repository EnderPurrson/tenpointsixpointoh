using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTransformViewPositionControl
{
	private PhotonTransformViewPositionModel m_Model;

	private float m_CurrentSpeed;

	private double m_LastSerializeTime;

	private Vector3 m_SynchronizedSpeed = Vector3.zero;

	private float m_SynchronizedTurnSpeed;

	private Vector3 m_NetworkPosition;

	private Queue<Vector3> m_OldNetworkPositions = new Queue<Vector3>();

	private bool m_UpdatedPositionAfterOnSerialize = true;

	public PhotonTransformViewPositionControl(PhotonTransformViewPositionModel model)
	{
		this.m_Model = model;
	}

	private void DeserializeData(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 vector3 = (Vector3)stream.ReceiveNext();
		if (this.m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || this.m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			this.m_SynchronizedSpeed = (Vector3)stream.ReceiveNext();
			this.m_SynchronizedTurnSpeed = (float)stream.ReceiveNext();
		}
		if (this.m_OldNetworkPositions.Count == 0)
		{
			this.m_NetworkPosition = vector3;
		}
		this.m_OldNetworkPositions.Enqueue(this.m_NetworkPosition);
		this.m_NetworkPosition = vector3;
		while (this.m_OldNetworkPositions.Count > this.m_Model.ExtrapolateNumberOfStoredPositions)
		{
			this.m_OldNetworkPositions.Dequeue();
		}
	}

	public Vector3 GetExtrapolatedPositionOffset()
	{
		float mLastSerializeTime = (float)(PhotonNetwork.time - this.m_LastSerializeTime);
		if (this.m_Model.ExtrapolateIncludingRoundTripTime)
		{
			mLastSerializeTime = mLastSerializeTime + (float)PhotonNetwork.GetPing() / 1000f;
		}
		Vector3 mSynchronizedSpeed = Vector3.zero;
		switch (this.m_Model.ExtrapolateOption)
		{
			case PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues:
			{
				Quaternion quaternion = Quaternion.Euler(0f, this.m_SynchronizedTurnSpeed * mLastSerializeTime, 0f);
				mSynchronizedSpeed = quaternion * this.m_SynchronizedSpeed * mLastSerializeTime;
				break;
			}
			case PhotonTransformViewPositionModel.ExtrapolateOptions.EstimateSpeedAndTurn:
			{
				Vector3 mNetworkPosition = (this.m_NetworkPosition - this.GetOldestStoredNetworkPosition()) * (float)PhotonNetwork.sendRateOnSerialize;
				mSynchronizedSpeed = mNetworkPosition * mLastSerializeTime;
				break;
			}
			case PhotonTransformViewPositionModel.ExtrapolateOptions.FixedSpeed:
			{
				Vector3 vector3 = (this.m_NetworkPosition - this.GetOldestStoredNetworkPosition()).normalized;
				mSynchronizedSpeed = (vector3 * this.m_Model.ExtrapolateSpeed) * mLastSerializeTime;
				break;
			}
		}
		return mSynchronizedSpeed;
	}

	public Vector3 GetNetworkPosition()
	{
		return this.m_NetworkPosition;
	}

	private Vector3 GetOldestStoredNetworkPosition()
	{
		Vector3 mNetworkPosition = this.m_NetworkPosition;
		if (this.m_OldNetworkPositions.Count > 0)
		{
			mNetworkPosition = this.m_OldNetworkPositions.Peek();
		}
		return mNetworkPosition;
	}

	public void OnPhotonSerializeView(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (!stream.isWriting)
		{
			this.DeserializeData(stream, info);
		}
		else
		{
			this.SerializeData(currentPosition, stream, info);
		}
		this.m_LastSerializeTime = PhotonNetwork.time;
		this.m_UpdatedPositionAfterOnSerialize = false;
	}

	private void SerializeData(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(currentPosition);
		this.m_NetworkPosition = currentPosition;
		if (this.m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || this.m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			stream.SendNext(this.m_SynchronizedSpeed);
			stream.SendNext(this.m_SynchronizedTurnSpeed);
		}
	}

	public void SetSynchronizedValues(Vector3 speed, float turnSpeed)
	{
		this.m_SynchronizedSpeed = speed;
		this.m_SynchronizedTurnSpeed = turnSpeed;
	}

	public Vector3 UpdatePosition(Vector3 currentPosition)
	{
		Vector3 networkPosition = this.GetNetworkPosition() + this.GetExtrapolatedPositionOffset();
		switch (this.m_Model.InterpolateOption)
		{
			case PhotonTransformViewPositionModel.InterpolateOptions.Disabled:
			{
				if (!this.m_UpdatedPositionAfterOnSerialize)
				{
					currentPosition = networkPosition;
					this.m_UpdatedPositionAfterOnSerialize = true;
				}
				break;
			}
			case PhotonTransformViewPositionModel.InterpolateOptions.FixedSpeed:
			{
				currentPosition = Vector3.MoveTowards(currentPosition, networkPosition, Time.deltaTime * this.m_Model.InterpolateMoveTowardsSpeed);
				break;
			}
			case PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed:
			{
				if (this.m_OldNetworkPositions.Count != 0)
				{
					float single = Vector3.Distance(this.m_NetworkPosition, this.GetOldestStoredNetworkPosition()) / (float)this.m_OldNetworkPositions.Count * (float)PhotonNetwork.sendRateOnSerialize;
					currentPosition = Vector3.MoveTowards(currentPosition, networkPosition, Time.deltaTime * single);
					break;
				}
				else
				{
					break;
				}
			}
			case PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues:
			{
				if (this.m_SynchronizedSpeed.magnitude != 0f)
				{
					currentPosition = Vector3.MoveTowards(currentPosition, networkPosition, Time.deltaTime * this.m_SynchronizedSpeed.magnitude);
				}
				else
				{
					currentPosition = networkPosition;
				}
				break;
			}
			case PhotonTransformViewPositionModel.InterpolateOptions.Lerp:
			{
				currentPosition = Vector3.Lerp(currentPosition, networkPosition, Time.deltaTime * this.m_Model.InterpolateLerpSpeed);
				break;
			}
		}
		if (this.m_Model.TeleportEnabled && Vector3.Distance(currentPosition, this.GetNetworkPosition()) > this.m_Model.TeleportIfDistanceGreaterThan)
		{
			currentPosition = this.GetNetworkPosition();
		}
		return currentPosition;
	}
}