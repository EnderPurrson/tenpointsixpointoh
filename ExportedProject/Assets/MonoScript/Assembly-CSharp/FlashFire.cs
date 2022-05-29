using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlashFire : MonoBehaviour
{
	public GameObject gunFlashObj;

	public float timeFireAction = 0.2f;

	private float activeTime;

	private WeaponSounds ws;

	public FlashFire()
	{
	}

	private void Awake()
	{
		this.ws = base.GetComponent<WeaponSounds>();
	}

	public void fire(Player_move_c moveC)
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.fireCourotine(moveC));
		}
	}

	[DebuggerHidden]
	public IEnumerator fireCourotine(Player_move_c moveC)
	{
		FlashFire.u003cfireCourotineu003ec__Iterator29 variable = null;
		return variable;
	}

	private void Start()
	{
		if (this.gunFlashObj == null)
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform current = (Transform)enumerator.Current;
					bool flag = false;
					if (current.gameObject.name.Equals("BulletSpawnPoint"))
					{
						IEnumerator enumerator1 = current.GetEnumerator();
						try
						{
							while (enumerator1.MoveNext())
							{
								Transform transforms = (Transform)enumerator1.Current;
								if (!transforms.gameObject.name.Equals("GunFlash"))
								{
									continue;
								}
								flag = true;
								this.gunFlashObj = transforms.gameObject;
								break;
							}
						}
						finally
						{
							IDisposable disposable = enumerator1 as IDisposable;
							if (disposable == null)
							{
							}
							disposable.Dispose();
						}
					}
					if (!flag)
					{
						continue;
					}
					break;
				}
			}
			finally
			{
				IDisposable disposable1 = enumerator as IDisposable;
				if (disposable1 == null)
				{
				}
				disposable1.Dispose();
			}
		}
		WeaponManager.SetGunFlashActive(this.gunFlashObj, false);
	}

	private void Update()
	{
		if (this.activeTime > 0f)
		{
			this.activeTime -= Time.deltaTime;
			if (this.activeTime <= 0f)
			{
				WeaponManager.SetGunFlashActive(this.gunFlashObj, false);
			}
		}
	}
}