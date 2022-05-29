using System;
using UnityEngine;

[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
	public PointedAtGameObjectInfo()
	{
	}

	private void OnGUI()
	{
		if (InputToEvent.goPointedAt != null)
		{
			PhotonView photonView = InputToEvent.goPointedAt.GetPhotonView();
			if (photonView != null)
			{
				Vector3 vector3 = Input.mousePosition;
				float single = (float)Screen.height;
				Vector3 vector31 = Input.mousePosition;
				GUI.Label(new Rect(vector3.x + 5f, single - vector31.y - 15f, 300f, 30f), string.Format("ViewID {0} {1}{2}", photonView.viewID, (!photonView.isSceneView ? string.Empty : "scene "), (!photonView.isMine ? string.Concat("owner: ", photonView.ownerId) : "mine")));
			}
		}
	}
}