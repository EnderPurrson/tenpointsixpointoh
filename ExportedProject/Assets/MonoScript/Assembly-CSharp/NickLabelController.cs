using System;
using UnityEngine;

public sealed class NickLabelController : MonoBehaviour
{
	public static Camera currentCamera;

	public Transform target;

	public NickLabelController.TypeNickLabel currentType;

	[Header("Player label")]
	public UILabel nickLabel;

	public UISprite rankTexture;

	public UILabel clanName;

	public UITexture clanTexture;

	public GameObject placeMarker;

	public UISprite multyKill;

	[Header("Lobby Exp.")]
	public GameObject expFrameLobby;

	public UISprite expProgressSprite;

	public UILabel expLabel;

	public UISprite ranksSpriteForLobby;

	[Header("Point")]
	public UISprite pointSprite;

	public UISprite pointFillSprite;

	[Header("Turret")]
	public GameObject isEnemySprite;

	public GameObject healthObj;

	public UISprite healthSprite;

	[Header("Free avard")]
	public GameObject freeAwardTitle;

	[Header("Free avard")]
	public UILabel lbGetGift;

	[NonSerialized]
	public Player_move_c playerScript;

	private Transform thisTransform;

	private BasePointController pointScript;

	private TurretController turretScript;

	private Vector3 offset = Vector3.up;

	private Vector3 offsetMech = new Vector3(0f, 0.5f, 0f);

	private float timeShow;

	private Vector3 posLabel;

	private int maxHeathWidth = 134;

	private float timerShowMyltyKill;

	private float maxTimerShowMultyKill = 5f;

	private float minScale = 0.5f;

	private float minDist = 10f;

	private float maxDist = 30f;

	private Vector2 coefScreen = new Vector2((float)Screen.width * 768f / (float)Screen.height, 768f);

	private string myLobbyNickname;

	private bool isHideLabel;

	static NickLabelController()
	{
	}

	public NickLabelController()
	{
	}

	private void Awake()
	{
		this.thisTransform = base.transform;
		this.HideLabel();
	}

	private void CheckShow()
	{
		if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
		{
			this.ResetTimeShow(-1f);
			return;
		}
		if (Defs.isDaterRegim)
		{
			this.ResetTimeShow(0.1f);
			return;
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Point)
		{
			if (!(this.pointScript != null) || !this.pointScript.isBaseActive)
			{
				this.ResetTimeShow(-1f);
			}
			else
			{
				this.ResetTimeShow(0.1f);
			}
			return;
		}
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby || this.currentType == NickLabelController.TypeNickLabel.FreeCoins || this.currentType == NickLabelController.TypeNickLabel.GetGift)
		{
			if (AskNameManager.isComplete)
			{
				this.ResetTimeShow(1f);
			}
			return;
		}
		if (Defs.isHunger && HungerGameController.Instance != null && !HungerGameController.Instance.isGo || WeaponManager.sharedManager.myPlayer == null)
		{
			this.ResetTimeShow(0.1f);
			return;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.isKilled)
		{
			this.ResetTimeShow(0.1f);
			return;
		}
		if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints || !(WeaponManager.sharedManager.myPlayer != null) || !(WeaponManager.sharedManager.myPlayerMoveC != null) || !(this.playerScript != null) || WeaponManager.sharedManager.myPlayerMoveC.myCommand != this.playerScript.myCommand)
		{
			return;
		}
		this.ResetTimeShow(0.1f);
	}

	private void HideLabel()
	{
		if (!this.isHideLabel)
		{
			this.isHideLabel = true;
			this.thisTransform.localPosition = new Vector3(0f, -10000f, 0f);
		}
	}

	public void LateUpdate()
	{
		if (this.target == null)
		{
			this.StopShow();
			return;
		}
		this.CheckShow();
		if (this.timeShow > 0f)
		{
			this.timeShow -= Time.deltaTime;
		}
		if (this.timeShow <= 0f || this.target.position.y <= -1000f || !(NickLabelController.currentCamera != null))
		{
			this.HideLabel();
			return;
		}
		this.posLabel = NickLabelController.currentCamera.WorldToViewportPoint((this.target.position + this.offset) + (this.currentType != NickLabelController.TypeNickLabel.Player || !(this.playerScript != null) || !this.playerScript.isMechActive ? Vector3.zero : this.offsetMech));
		if (this.posLabel.z < 0f)
		{
			this.HideLabel();
			return;
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Turret)
		{
			this.UpdateTurrethealthSprite();
		}
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby)
		{
			this.UpdateInfo();
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Player || this.currentType == NickLabelController.TypeNickLabel.Turret)
		{
			if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture)
			{
				this.SetCommandColor((Defs.isDaterRegim || Defs.isCOOP ? 0 : 2));
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				this.SetCommandColor((WeaponManager.sharedManager.myPlayerMoveC.myCommand != this.playerScript.myCommand ? 2 : 1));
			}
			else
			{
				this.SetCommandColor(0);
			}
		}
		this.thisTransform.localPosition = new Vector3((this.posLabel.x - 0.5f) * this.coefScreen.x, (this.posLabel.y - 0.5f) * this.coefScreen.y, 0f);
		this.isHideLabel = false;
	}

	public void ResetTimeShow(float _time = 0.1f)
	{
		this.timeShow = _time;
	}

	public void SetCommandColor(int _command = 0)
	{
		if (_command == 1)
		{
			this.nickLabel.color = Color.blue;
			return;
		}
		if (_command == 2)
		{
			this.nickLabel.color = Color.red;
			return;
		}
		this.nickLabel.color = Color.white;
	}

	public void StartShow(NickLabelController.TypeNickLabel _type, Transform _target)
	{
		Player_move_c component;
		this.thisTransform = base.transform;
		for (int i = 0; i < this.thisTransform.childCount; i++)
		{
			this.thisTransform.GetChild(i).gameObject.SetActive(false);
		}
		this.currentType = _type;
		this.target = _target;
		base.gameObject.SetActive(true);
		this.placeMarker.SetActive(false);
		this.nickLabel.color = Color.white;
		this.healthSprite.enabled = true;
		this.offset = new Vector3(0f, 0.6f, 0f);
		float single = 1f;
		this.SetCommandColor(0);
		if (this.currentType == NickLabelController.TypeNickLabel.Player)
		{
			this.nickLabel.gameObject.SetActive(true);
			this.playerScript = this.target.GetComponent<Player_move_c>();
		}
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby)
		{
			single = 1f;
			this.expFrameLobby.SetActive(true);
			this.nickLabel.gameObject.SetActive(true);
			this.clanName.gameObject.SetActive(true);
			this.clanTexture.gameObject.SetActive(true);
			this.offset = new Vector3(0f, 2.26f, 0f);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Point)
		{
			this.pointScript = this.target.GetComponent<BasePointController>();
			this.pointSprite.spriteName = string.Concat("Point_", this.pointScript.nameBase);
			this.pointFillSprite.spriteName = this.pointSprite.spriteName;
			this.pointSprite.gameObject.SetActive(true);
			this.offset = new Vector3(0f, 2.25f, 0f);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Turret)
		{
			this.turretScript = this.target.GetComponent<TurretController>();
			if (this.turretScript.myPlayer == null)
			{
				component = null;
			}
			else
			{
				component = this.turretScript.myPlayer.GetComponent<SkinName>().playerMoveC;
			}
			this.playerScript = component;
			this.nickLabel.gameObject.SetActive(true);
			if (!Defs.isDaterRegim)
			{
				this.healthObj.SetActive(true);
			}
			this.offset = new Vector3(0f, 1.76f, 0f);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.FreeCoins)
		{
			this.thisTransform.localScale = new Vector3(single, single, single);
			this.freeAwardTitle.gameObject.SetActive(true);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.GetGift)
		{
			this.thisTransform.localScale = new Vector3(single, single, single);
			this.lbGetGift.gameObject.SetActive(true);
		}
		this.thisTransform.localScale = new Vector3(single, single, single);
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby)
		{
			this.UpdateNickInLobby();
		}
		this.UpdateInfo();
		this.HideLabel();
	}

	private void StopShow()
	{
		this.currentType = NickLabelController.TypeNickLabel.None;
		this.HideLabel();
		base.gameObject.SetActive(false);
		this.playerScript = null;
		this.pointScript = null;
		this.turretScript = null;
	}

	private void UpdateExpFrameInLobby()
	{
		int num = ExperienceController.sharedController.currentLevel;
		this.ranksSpriteForLobby.spriteName = string.Concat("Rank_", num.ToString());
		int maxExpLevels = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
		int num1 = Mathf.Clamp(ExperienceController.sharedController.CurrentExperience, 0, maxExpLevels);
		string str = string.Format("{0} {1}/{2}", LocalizationStore.Get("Key_0204"), num1, maxExpLevels);
		if (ExperienceController.sharedController.currentLevel == ExperienceController.maxLevel)
		{
			str = LocalizationStore.Get("Key_0928");
		}
		this.expLabel.text = str;
		this.expProgressSprite.width = Mathf.RoundToInt(146f * (ExperienceController.sharedController.currentLevel != ExperienceController.maxLevel ? (float)num1 / (float)maxExpLevels : 1f));
	}

	public void UpdateInfo()
	{
		int num;
		if (this.currentType == NickLabelController.TypeNickLabel.Player)
		{
			this.nickLabel.text = FilterBadWorld.FilterString(this.playerScript.mySkinName.NickName);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Turret && this.playerScript != null)
		{
			if (!Defs.isMulti)
			{
				this.nickLabel.text = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
			}
			else
			{
				this.nickLabel.text = FilterBadWorld.FilterString(this.playerScript.mySkinName.NickName);
			}
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
			{
				if (WeaponManager.sharedManager.myPlayerMoveC != null)
				{
					num = (WeaponManager.sharedManager.myPlayerMoveC.myCommand != this.playerScript.myCommand ? 2 : 1);
				}
				else
				{
					num = 0;
				}
				this.SetCommandColor(num);
			}
			else
			{
				this.SetCommandColor((this.playerScript.Equals(WeaponManager.sharedManager.myPlayerMoveC) || Defs.isDaterRegim || Defs.isCOOP ? 0 : 2));
			}
		}
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby)
		{
			this.nickLabel.text = this.myLobbyNickname;
			this.clanName.text = FriendsController.sharedController.clanName;
			if (string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
			{
				this.clanTexture.mainTexture = null;
			}
			else if (this.clanTexture.mainTexture == null)
			{
				byte[] numArray = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
				texture2D.LoadImage(numArray);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				this.clanTexture.mainTexture = texture2D;
				Transform vector3 = this.clanTexture.transform;
				float single = vector3.localPosition.y;
				Vector3 vector31 = vector3.localPosition;
				vector3.localPosition = new Vector3((float)(-this.clanName.width) * 0.5f - 16f, single, vector31.z);
			}
			this.UpdateExpFrameInLobby();
		}
	}

	public void UpdateNickInLobby()
	{
		this.myLobbyNickname = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
	}

	private void UpdateTurrethealthSprite()
	{
		float num = (float)Mathf.RoundToInt((float)this.maxHeathWidth * ((this.turretScript.health >= 0f ? this.turretScript.health : 0f) / this.turretScript.maxHealth));
		if (num < 0.1f)
		{
			num = 0f;
			this.healthSprite.enabled = false;
		}
		else if (!this.healthSprite.enabled)
		{
			this.healthSprite.enabled = true;
		}
		this.healthSprite.width = Mathf.RoundToInt(num);
	}

	public enum TypeNickLabel
	{
		None,
		Player,
		Turret,
		Point,
		PlayerLobby,
		FreeCoins,
		GetGift
	}
}