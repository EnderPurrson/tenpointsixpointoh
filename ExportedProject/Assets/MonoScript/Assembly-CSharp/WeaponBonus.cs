using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBonus : Photon.MonoBehaviour
{
	public GameObject weaponPrefab;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private bool runLoading;

	private bool oldIsMaster;

	public WeaponManager _weaponManager;

	private bool isHunger;

	public bool isKilled;

	public WeaponBonus()
	{
	}

	[PunRPC]
	[RPC]
	public void DestroyRPC()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			base.transform.position = new Vector3(0f, -10000f, 0f);
		}
		else
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(false);
		}
	}

	private void OnDestroy()
	{
		if (!Defs.IsSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None) && !this.isHunger)
		{
			return;
		}
	}

	private void Start()
	{
		string str = base.gameObject.name.Substring(0, base.gameObject.name.Length - 13);
		this.weaponPrefab = Resources.Load<GameObject>(string.Concat("Weapons/", str));
		this._weaponManager = WeaponManager.sharedManager;
		this.isHunger = Defs.isHunger;
		if (this.isHunger)
		{
			this._player = this._weaponManager.myPlayer;
			if (this._player != null)
			{
				GameObject component = this._player.GetComponent<SkinName>().playerGameObject;
				if (component == null)
				{
					Debug.LogWarning("WeaponBonus.Start(): playerGo == null");
				}
				else
				{
					this._playerMoveC = component.GetComponent<Player_move_c>();
				}
			}
		}
		else
		{
			this._player = GameObject.FindGameObjectWithTag("Player");
			GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
			if (gameObject == null)
			{
				Debug.LogWarning("WeaponBonus.Start(): playerGun == null");
			}
			else
			{
				this._playerMoveC = gameObject.GetComponent<Player_move_c>();
			}
		}
		if (!Defs.IsSurvival && !this.isHunger)
		{
			GameObject gameObject1 = UnityEngine.Object.Instantiate(Resources.Load("BonusFX"), Vector3.zero, Quaternion.identity) as GameObject;
			gameObject1.transform.parent = base.transform;
			gameObject1.transform.localPosition = Vector3.zero;
			gameObject1.layer = base.gameObject.layer;
			ZombieCreator.SetLayerRecursively(gameObject1, base.gameObject.layer);
		}
	}

	private void Update()
	{
		if (!this.oldIsMaster && PhotonNetwork.isMasterClient && this.isKilled)
		{
			PhotonNetwork.Destroy(base.gameObject);
			return;
		}
		this.oldIsMaster = PhotonNetwork.isMasterClient;
		float single = 120f;
		base.transform.Rotate(base.transform.InverseTransformDirection(Vector3.up), single * Time.deltaTime);
		if (this.runLoading)
		{
			return;
		}
		if (this.isHunger && (this._player == null || this._playerMoveC == null))
		{
			this._player = this._weaponManager.myPlayer;
			if (this._player == null)
			{
				return;
			}
			this._playerMoveC = this._player.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>();
		}
		if (this._playerMoveC == null || this._playerMoveC.isGrenadePress)
		{
			return;
		}
		if (!this.isKilled && !this._playerMoveC.isKilled && Vector3.SqrMagnitude(base.transform.position - this._player.transform.position) < 2.25f)
		{
			this._playerMoveC.AddWeapon(this.weaponPrefab);
			this.isKilled = true;
			if (Defs.IsSurvival || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None || this.isHunger)
			{
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
				{
					TrainingController.isNextStep = TrainingState.GetTheGun;
				}
				if (this.isHunger)
				{
					base.photonView.RPC("DestroyRPC", PhotonTargets.AllBuffered, new object[0]);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				return;
			}
			string[] strArrays = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[] { '#' });
			List<string> strs = new List<string>();
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				strs.Add(strArrays1[i]);
			}
			if (!strs.Contains(LevelBox.weaponsFromBosses[Application.loadedLevelName]))
			{
				strs.Add(LevelBox.weaponsFromBosses[Application.loadedLevelName]);
				Storager.setString(Defs.WeaponsGotInCampaign, string.Join("#", strs.ToArray()), false);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			this.runLoading = true;
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "LevelComplete";
			AutoFade.LoadLevel("LevelToCompleteProm", 2f, 0f, Color.white);
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(true);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("PauseONGuiDrawer") as GameObject);
			gameObject.transform.parent = base.transform;
		}
	}
}