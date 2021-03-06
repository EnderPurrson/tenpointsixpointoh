using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ManualPhotonViewAllocator : MonoBehaviour
{
	public GameObject Prefab;

	public ManualPhotonViewAllocator()
	{
	}

	public void AllocateManualPhotonView()
	{
		PhotonView photonView = base.gameObject.GetPhotonView();
		if (photonView == null)
		{
			Debug.LogError("Can't do manual instantiation without PhotonView component.");
			return;
		}
		int num = PhotonNetwork.AllocateViewID();
		photonView.RPC("InstantiateRpc", PhotonTargets.AllBuffered, new object[] { num });
	}

	[PunRPC]
	public void InstantiateRpc(int viewID)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.Prefab, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity) as GameObject;
		gameObject.GetPhotonView().viewID = viewID;
		gameObject.GetComponent<OnClickDestroy>().DestroyByRpc = true;
	}
}