using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

internal sealed class SetParentWeapon : MonoBehaviour
{
	private bool isMine;

	private bool isInet;

	private bool isMulti;

	private PhotonView photonView;

	public SetParentWeapon()
	{
	}

	[Obfuscation(Exclude=true)]
	private void SetParent()
	{
		int d = -1;
		NetworkPlayer component = base.GetComponent<NetworkView>().owner;
		if (this.isInet && this.photonView && this.photonView.owner != null)
		{
			d = this.photonView.owner.ID;
		}
		foreach (Player_move_c player in Initializer.players)
		{
			if ((!this.isInet || !(player.mySkinName.photonView != null) || player.mySkinName.photonView.owner == null || player.mySkinName.photonView.owner.ID != d) && (this.isInet || !player.mySkinName.GetComponent<NetworkView>().owner.Equals(component)))
			{
				continue;
			}
			GameObject gameObject = player.mySkinName.playerGameObject;
			GameObject child = null;
			base.transform.position = Vector3.zero;
			if (!base.transform.GetComponent<WeaponSounds>().isMelee)
			{
				IEnumerator enumerator = base.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Transform current = (Transform)enumerator.Current;
						if (!current.gameObject.name.Equals("BulletSpawnPoint") || current.childCount < 0)
						{
							continue;
						}
						child = current.GetChild(0).gameObject;
						if (!this.isMine)
						{
							WeaponManager.SetGunFlashActive(child, false);
						}
						break;
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
			IEnumerator enumerator1 = gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator1.MoveNext())
				{
					Transform transforms = (Transform)enumerator1.Current;
					transforms.parent = null;
					Transform transforms1 = transforms;
					transforms1.position = transforms1.position + (-Vector3.up * 1000f);
				}
			}
			finally
			{
				IDisposable disposable1 = enumerator1 as IDisposable;
				if (disposable1 == null)
				{
				}
				disposable1.Dispose();
			}
			base.transform.parent = gameObject.transform;
			if (base.transform.FindChild("BulletSpawnPoint") != null)
			{
				player._bulletSpawnPoint = base.transform.FindChild("BulletSpawnPoint").gameObject;
			}
			base.transform.localPosition = new Vector3(0f, -1.7f, 0f);
			base.transform.rotation = gameObject.transform.rotation;
			if (gameObject.transform.parent.gameObject != null)
			{
				player.SetTextureForBodyPlayer(player._skin);
			}
			return;
		}
		base.Invoke("SetParent", 0.1f);
	}

	private void Start()
	{
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
		{
			return;
		}
		this.isMulti = Defs.isMulti;
		if (!this.isMulti)
		{
			return;
		}
		this.isInet = Defs.isInet;
		this.photonView = PhotonView.Get(this);
		if (this.isInet)
		{
			this.isMine = this.photonView.isMine;
		}
		else
		{
			this.isMine = base.GetComponent<NetworkView>().isMine;
		}
		this.SetParent();
	}
}