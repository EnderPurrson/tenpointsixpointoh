using System;
using UnityEngine;

public class PhotonTransformViewScaleControl
{
	private PhotonTransformViewScaleModel m_Model;

	private Vector3 m_NetworkScale = Vector3.one;

	public PhotonTransformViewScaleControl(PhotonTransformViewScaleModel model)
	{
		this.m_Model = model;
	}

	public Vector3 GetScale(Vector3 currentScale)
	{
		switch (this.m_Model.InterpolateOption)
		{
			case PhotonTransformViewScaleModel.InterpolateOptions.Disabled:
			{
				return this.m_NetworkScale;
			}
			case PhotonTransformViewScaleModel.InterpolateOptions.MoveTowards:
			{
				return Vector3.MoveTowards(currentScale, this.m_NetworkScale, this.m_Model.InterpolateMoveTowardsSpeed * Time.deltaTime);
			}
			case PhotonTransformViewScaleModel.InterpolateOptions.Lerp:
			{
				return Vector3.Lerp(currentScale, this.m_NetworkScale, this.m_Model.InterpolateLerpSpeed * Time.deltaTime);
			}
			default:
			{
				return this.m_NetworkScale;
			}
		}
	}

	public void OnPhotonSerializeView(Vector3 currentScale, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (!stream.isWriting)
		{
			this.m_NetworkScale = (Vector3)stream.ReceiveNext();
		}
		else
		{
			stream.SendNext(currentScale);
			this.m_NetworkScale = currentScale;
		}
	}
}