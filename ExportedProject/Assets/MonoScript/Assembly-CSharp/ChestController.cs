using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Photon.MonoBehaviour
{
	public bool isStartChest = true;

	public int currentSpawnZone;

	public float live = 5f;

	public bool isKilled;

	public bool isChestBonus;

	public readonly static int[] weaponForHungerGames;

	public AudioClip brokenAudio;

	private bool oldIsMaster;

	static ChestController()
	{
		ChestController.weaponForHungerGames = new int[] { 1, 2, 3, 8, 53, 5, 52, 51, 66, 67, 162, 333 };
	}

	public ChestController()
	{
	}

	private void DestroyChest()
	{
		base.photonView.RPC("DestroyChestRPC", PhotonTargets.AllBuffered, new object[0]);
	}

	[PunRPC]
	[RPC]
	private void DestroyChestRPC()
	{
		Debug.Log("DestroyChestRPC");
		if (!PhotonNetwork.isMasterClient)
		{
			base.transform.position = new Vector3(0f, -10000f, 0f);
		}
		else
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
	}

	[PunRPC]
	[RPC]
	public void KilledChest()
	{
		if (this.isKilled)
		{
			return;
		}
		this.isKilled = true;
		if (PhotonNetwork.isMasterClient)
		{
			int num = UnityEngine.Random.Range(0, (int)ChestController.weaponForHungerGames.Length);
			if (!this.isChestBonus)
			{
				PhotonNetwork.InstantiateSceneObject(string.Concat("Weapon_Bonuses/Weapon", ChestController.weaponForHungerGames[num], "_Bonus"), base.transform.position, base.transform.rotation, 0, null);
			}
			else if (UnityEngine.Random.Range(0, 11) >= 7)
			{
				PhotonNetwork.InstantiateSceneObject(string.Concat("Weapon_Bonuses/Weapon", ChestController.weaponForHungerGames[num], "_Bonus"), base.transform.position, base.transform.rotation, 0, null);
			}
			else
			{
				BonusController.sharedController.AddBonusForHunger(base.transform.position, this.TypeBonus(), base.transform.GetComponent<SettingBonus>().numberSpawnZone);
			}
		}
		if (Defs.isSoundFX)
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.brokenAudio);
		}
		base.GetComponent<Animation>().Stop();
		base.GetComponent<Animation>().Play("Broken");
		base.Invoke("DestroyChestRPC", 0.5f);
	}

	public void MinusLive(float _minus)
	{
		base.photonView.RPC("KilledChest", PhotonTargets.All, new object[0]);
	}

	[PunRPC]
	[RPC]
	public void MinusLiveRPC(float _minus)
	{
		if (this.isKilled)
		{
			return;
		}
		this.live -= _minus;
		base.photonView.RPC("SynchLiveRPC", PhotonTargets.AllBuffered, new object[] { this.live });
		if (this.live <= 0f)
		{
			base.photonView.RPC("KilledChest", PhotonTargets.AllBuffered, new object[0]);
		}
	}

	private void OnDestroy()
	{
		Initializer.chestsObj.Remove(base.gameObject);
	}

	private void Start()
	{
		Initializer.chestsObj.Add(base.gameObject);
	}

	[PunRPC]
	[RPC]
	public void SynchLiveRPC(float _live)
	{
		this.live = _live;
	}

	private int TypeBonus()
	{
		int num = UnityEngine.Random.Range(0, 100);
		if (num < 40)
		{
			return 0;
		}
		if (num < 62)
		{
			return 1;
		}
		if (num < 85 && !Defs.isHunger)
		{
			return 2;
		}
		return 4;
	}

	private void Update()
	{
		if (!this.oldIsMaster && PhotonNetwork.isMasterClient && this.isKilled)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		this.oldIsMaster = PhotonNetwork.isMasterClient;
	}
}