using System;
using UnityEngine;

public class OnJoinedInstantiate : MonoBehaviour
{
	public Transform SpawnPosition;

	public float PositionOffset = 2f;

	public GameObject[] PrefabsToInstantiate;

	public OnJoinedInstantiate()
	{
	}

	public void OnJoinedRoom()
	{
		if (this.PrefabsToInstantiate != null)
		{
			GameObject[] prefabsToInstantiate = this.PrefabsToInstantiate;
			for (int i = 0; i < (int)prefabsToInstantiate.Length; i++)
			{
				GameObject gameObject = prefabsToInstantiate[i];
				Debug.Log(string.Concat("Instantiating: ", gameObject.name));
				Vector3 spawnPosition = Vector3.up;
				if (this.SpawnPosition != null)
				{
					spawnPosition = this.SpawnPosition.position;
				}
				Vector3 vector3 = UnityEngine.Random.insideUnitSphere;
				vector3.y = 0f;
				vector3 = vector3.normalized;
				Vector3 positionOffset = spawnPosition + (this.PositionOffset * vector3);
				PhotonNetwork.Instantiate(gameObject.name, positionOffset, Quaternion.identity, 0);
			}
		}
	}
}