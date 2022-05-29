using Photon;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class HighlightOwnedGameObj : Photon.MonoBehaviour
{
	public GameObject PointerPrefab;

	public float Offset = 0.5f;

	private Transform markerTransform;

	public HighlightOwnedGameObj()
	{
	}

	private void Update()
	{
		if (base.photonView.isMine)
		{
			if (this.markerTransform == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PointerPrefab);
				gameObject.transform.parent = base.gameObject.transform;
				this.markerTransform = gameObject.transform;
			}
			Vector3 vector3 = base.gameObject.transform.position;
			this.markerTransform.position = new Vector3(vector3.x, vector3.y + this.Offset, vector3.z);
			this.markerTransform.rotation = Quaternion.identity;
		}
		else if (this.markerTransform != null)
		{
			UnityEngine.Object.Destroy(this.markerTransform.gameObject);
			this.markerTransform = null;
		}
	}
}