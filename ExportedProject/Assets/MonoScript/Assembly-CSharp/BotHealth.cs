using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class BotHealth : MonoBehaviour
{
	public static bool _hurtAudioIsPlaying;

	private static SkinsManagerPixlGun _skinsManager;

	public string myName = "Bot";

	private bool IsLife = true;

	public Texture hitTexture;

	private BotAI ai;

	private Player_move_c healthDown;

	private bool _flashing;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private Texture _skin;

	private bool _weaponCreated;

	static BotHealth()
	{
	}

	public BotHealth()
	{
	}

	private void _CreateBonusWeapon()
	{
		if (LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName) && base.gameObject.name.Contains("Boss") && !this._weaponCreated)
		{
			string item = LevelBox.weaponsFromBosses[Application.loadedLevelName];
			Vector3 vector3 = base.gameObject.transform.position + new Vector3(0f, 0.25f, 0f);
			((base.GetComponent<BotMovement>()._gameController.weaponBonus == null ? BonusCreator._CreateBonus(item, vector3) : BonusCreator._CreateBonusFromPrefab(base.GetComponent<BotMovement>()._gameController.weaponBonus, vector3))).AddComponent<GotToNextLevel>();
			base.GetComponent<BotMovement>()._gameController.weaponBonus = null;
			this._weaponCreated = true;
		}
	}

	public void adjustHealth(float _health, Transform target)
	{
		if (_health < 0f && !this._flashing)
		{
			base.StartCoroutine(this.Flash());
		}
		this._soundClips.health += _health;
		if (this._soundClips.health < 0f)
		{
			this._soundClips.health = 0f;
		}
		if (UnityEngine.Debug.isDebugBuild)
		{
			this._CreateBonusWeapon();
			this.IsLife = false;
		}
		else if (this._soundClips.health != 0f)
		{
			GlobalGameController.Score = GlobalGameController.Score + 5;
		}
		else
		{
			this._CreateBonusWeapon();
			this.IsLife = false;
		}
		if (this.RequestPlayHurt(this._soundClips.hurt.length) && Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.hurt);
		}
		if (target.CompareTag("Player") && !target.GetComponent<SkinName>().playerMoveC.isInvisible || target.CompareTag("Turret"))
		{
			this.ai.SetTarget(target, true);
		}
	}

	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
		}
	}

	[DebuggerHidden]
	private IEnumerator Flash()
	{
		BotHealth.u003cFlashu003ec__Iterator10D variable = null;
		return variable;
	}

	public bool getIsLife()
	{
		return this.IsLife;
	}

	public bool RequestPlayHurt(float tm)
	{
		if (BotHealth._hurtAudioIsPlaying)
		{
			return false;
		}
		base.StartCoroutine(this.resetHurtAudio(tm));
		return true;
	}

	[DebuggerHidden]
	private IEnumerator resetHurtAudio(float tm)
	{
		BotHealth.u003cresetHurtAudiou003ec__Iterator10C variable = null;
		return variable;
	}

	public static Texture SetSkinForObj(GameObject go)
	{
		if (!BotHealth._skinsManager)
		{
			BotHealth._skinsManager = GameObject.FindGameObjectWithTag("SkinsManager").GetComponent<SkinsManagerPixlGun>();
		}
		Texture texture = null;
		string str = BotHealth.SkinNameForObj(go.name);
		Texture item = BotHealth._skinsManager.skins[str] as Texture;
		texture = item;
		if (!item)
		{
			UnityEngine.Debug.Log(string.Concat("No skin: ", str));
		}
		BotHealth.SetTextureRecursivelyFrom(go, texture);
		return texture;
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt)
	{
		IEnumerator enumerator = obj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (!current.name.Equals("Ears"))
				{
					if (current.gameObject.GetComponent<Renderer>() && current.gameObject.GetComponent<Renderer>().material)
					{
						current.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
					}
					BotHealth.SetTextureRecursivelyFrom(current.gameObject, txt);
				}
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

	public static string SkinNameForObj(string objName)
	{
		string str;
		if (!Defs.IsSurvival)
		{
			str = (TrainingController.TrainingCompleted ? string.Concat(objName, "_Level", CurrentCampaignGame.currentLevel) : string.Concat(objName, "_Level3"));
		}
		else
		{
			str = objName;
		}
		return str;
	}

	private void Start()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				this._modelChild = ((Transform)enumerator.Current).gameObject;
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
		this._soundClips = this._modelChild.GetComponent<Sounds>();
		if (Defs.IsSurvival && TrainingController.TrainingCompleted)
		{
			if (ZombieCreator.sharedCreator.currentWave == 0)
			{
				this._soundClips.notAttackingSpeed *= 0.75f;
				this._soundClips.attackingSpeed *= 0.75f;
				this._soundClips.health *= 0.7f;
			}
			if (ZombieCreator.sharedCreator.currentWave == 1)
			{
				this._soundClips.notAttackingSpeed *= 0.85f;
				this._soundClips.attackingSpeed *= 0.85f;
				this._soundClips.health *= 0.8f;
			}
			if (ZombieCreator.sharedCreator.currentWave == 2)
			{
				this._soundClips.notAttackingSpeed *= 0.9f;
				this._soundClips.attackingSpeed *= 0.9f;
				this._soundClips.health *= 0.9f;
			}
			if (ZombieCreator.sharedCreator.currentWave >= 7)
			{
				this._soundClips.notAttackingSpeed *= 1.25f;
				this._soundClips.attackingSpeed *= 1.25f;
			}
			if (ZombieCreator.sharedCreator.currentWave >= 9)
			{
				this._soundClips.health *= 1.25f;
			}
		}
		this.ai = base.GetComponent<BotAI>();
		this.healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		if (base.gameObject.name.IndexOf("Boss") != -1)
		{
			this._skin = this._modelChild.GetComponentInChildren<Renderer>().material.mainTexture;
		}
		else
		{
			this._skin = BotHealth.SetSkinForObj(this._modelChild);
		}
	}
}