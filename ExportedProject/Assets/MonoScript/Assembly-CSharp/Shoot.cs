using System;
using UnityEngine;

internal sealed class Shoot : MonoBehaviour
{
	public float Range = 1000f;

	public Transform _transform;

	public GameObject bullet;

	private GameObject _bulletSpawnPoint;

	public int lives = 100;

	public Shoot()
	{
	}

	[PunRPC]
	[RPC]
	private void Popal(NetworkViewID Popal, NetworkMessageInfo info)
	{
		Debug.Log(string.Concat(new object[] { Popal, " ", base.gameObject.transform.GetComponent<NetworkView>().viewID, " ", info.sender }));
	}

	public void shootS()
	{
		RaycastHit raycastHit;
		Debug.Log(string.Concat("Shot!!", base.transform.position));
		if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f)), out raycastHit, 100f, Player_move_c._ShootRaycastLayerMask))
		{
			Debug.Log("Hit!");
			if (raycastHit.collider.gameObject.transform.CompareTag("Enemy") && Defs.isMulti)
			{
				base.GetComponent<NetworkView>().RPC("Popal", RPCMode.All, new object[] { raycastHit.collider.gameObject.transform.GetComponent<NetworkView>().viewID });
			}
		}
	}

	private void Start()
	{
		this._bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
	}
}