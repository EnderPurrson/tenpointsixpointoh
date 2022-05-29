using System;
using UnityEngine;

public interface IPunPrefabPool
{
	void Destroy(GameObject gameObject);

	GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation);
}