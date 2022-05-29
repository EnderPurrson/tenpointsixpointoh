using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class ProfileView : MonoBehaviour
{
	public UILabel pixelgunFriendsID;

	public GameObject interfaceHolder;

	public UIRoot interfaceHolder2d;

	public GameObject worldHolder3d;

	public UILabel totalWeeklyWinsCount;

	public UILabel deathmatchWinCount;

	public UILabel teamBattleWinCount;

	public UILabel capturePointCount;

	public UILabel deadlyGamesWinCount;

	public UILabel flagCaptureWinCount;

	public UILabel totalWinCount;

	public UILabel lbGameTotalKills;

	public UILabel lbGameKillrate;

	public UILabel lbGameAccuracy;

	public UILabel lbGameLikes;

	public UILabel coopTimeSurvivalPointCount;

	public UILabel waveCountLabel;

	public UILabel killedCountLabel;

	public UILabel survivalScoreLabel;

	public UILabel box1StarsLabel;

	public UILabel box2StarsLabel;

	public UILabel box3StarsLabel;

	public UILabel secretCoinsLabel;

	public UILabel secretGemsLabel;

	public UIInputRilisoft nicknameInput;

	public UITexture clanLogo;

	public ButtonHandler backButton;

	public UIButton achievementsButton;

	public UIButton leaderboardsButton;

	public UIButton copyIdButton;

	public CharacterView characterView;

	private EventHandler<ProfileView.InputEventArgs> NicknameInput;

	public string Box1StarsLabel
	{
		get
		{
			return ProfileView.GetText(this.box1StarsLabel);
		}
		set
		{
			ProfileView.SetText(this.box1StarsLabel, value);
			if (this.box1StarsLabel != null)
			{
				UISprite componentInChildren = this.box1StarsLabel.GetComponentInChildren<UISprite>();
				if (componentInChildren != null)
				{
					Vector3 vector3 = this.box1StarsLabel.transform.localPosition;
					componentInChildren.transform.localPosition = new Vector3((float)(-this.box1StarsLabel.width), vector3.y, vector3.z);
				}
			}
		}
	}

	public string Box2StarsLabel
	{
		get
		{
			return ProfileView.GetText(this.box2StarsLabel);
		}
		set
		{
			ProfileView.SetText(this.box2StarsLabel, value);
			if (this.box2StarsLabel != null)
			{
				UISprite componentInChildren = this.box2StarsLabel.GetComponentInChildren<UISprite>();
				if (componentInChildren != null)
				{
					Vector3 vector3 = this.box2StarsLabel.transform.localPosition;
					componentInChildren.transform.localPosition = new Vector3((float)(-this.box2StarsLabel.width), vector3.y, vector3.z);
				}
			}
		}
	}

	public string Box3StarsLabel
	{
		get
		{
			return ProfileView.GetText(this.box3StarsLabel);
		}
		set
		{
			ProfileView.SetText(this.box3StarsLabel, value);
			if (this.box3StarsLabel != null)
			{
				UISprite componentInChildren = this.box3StarsLabel.GetComponentInChildren<UISprite>();
				if (componentInChildren != null)
				{
					Vector3 vector3 = this.box3StarsLabel.transform.localPosition;
					componentInChildren.transform.localPosition = new Vector3((float)(-this.box3StarsLabel.width), vector3.y, vector3.z);
				}
			}
		}
	}

	public string CapturePointWinCount
	{
		get
		{
			return ProfileView.GetText(this.capturePointCount);
		}
		set
		{
			ProfileView.SetText(this.capturePointCount, value);
		}
	}

	public string CoopTimeSurvivalPointCount
	{
		get
		{
			return ProfileView.GetText(this.coopTimeSurvivalPointCount);
		}
		set
		{
			ProfileView.SetText(this.coopTimeSurvivalPointCount, value);
		}
	}

	public string DeadlyGamesWinCount
	{
		get
		{
			return ProfileView.GetText(this.deadlyGamesWinCount);
		}
		set
		{
			ProfileView.SetText(this.deadlyGamesWinCount, value);
		}
	}

	public string DeathmatchWinCount
	{
		get
		{
			return ProfileView.GetText(this.deathmatchWinCount);
		}
		set
		{
			ProfileView.SetText(this.deathmatchWinCount, value);
		}
	}

	public string FlagCaptureWinCount
	{
		get
		{
			return ProfileView.GetText(this.flagCaptureWinCount);
		}
		set
		{
			ProfileView.SetText(this.flagCaptureWinCount, value);
		}
	}

	public string GameAccuracy
	{
		get
		{
			return ProfileView.GetText(this.lbGameAccuracy);
		}
		set
		{
			ProfileView.SetText(this.lbGameAccuracy, value);
		}
	}

	public string GameKillrate
	{
		get
		{
			return ProfileView.GetText(this.lbGameKillrate);
		}
		set
		{
			ProfileView.SetText(this.lbGameKillrate, value);
		}
	}

	public string GameLikes
	{
		get
		{
			return ProfileView.GetText(this.lbGameLikes);
		}
		set
		{
			ProfileView.SetText(this.lbGameLikes, value);
		}
	}

	public string GameTotalKills
	{
		get
		{
			return ProfileView.GetText(this.lbGameTotalKills);
		}
		set
		{
			ProfileView.SetText(this.lbGameTotalKills, value);
		}
	}

	private bool IdPlayerExist
	{
		get
		{
			return (FriendsController.sharedController == null ? false : !string.IsNullOrEmpty(FriendsController.sharedController.id));
		}
	}

	public string KilledCountLabel
	{
		get
		{
			return ProfileView.GetText(this.killedCountLabel);
		}
		set
		{
			ProfileView.SetText(this.killedCountLabel, value);
		}
	}

	public string Nickname
	{
		get
		{
			return (this.nicknameInput == null ? string.Empty : this.nicknameInput.@value ?? string.Empty);
		}
		set
		{
			if (this.nicknameInput != null)
			{
				this.nicknameInput.@value = value ?? string.Empty;
			}
		}
	}

	public string PixelgunFriendsID
	{
		get
		{
			return ProfileView.GetText(this.pixelgunFriendsID);
		}
		set
		{
			ProfileView.SetText(this.pixelgunFriendsID, string.Concat("ID: ", value));
		}
	}

	public string SecretCoinsLabel
	{
		get
		{
			return ProfileView.GetText(this.secretCoinsLabel);
		}
		set
		{
			if (this.secretCoinsLabel == null)
			{
				return;
			}
			ProfileView.SetText(this.secretCoinsLabel, value);
			UISprite componentInChildren = this.secretCoinsLabel.GetComponentInChildren<UISprite>();
			if (componentInChildren == null)
			{
				return;
			}
			Vector3 vector3 = this.secretCoinsLabel.transform.localPosition;
			componentInChildren.transform.localPosition = new Vector3((float)(-this.secretCoinsLabel.width), vector3.y, vector3.z);
		}
	}

	public string SecretGemsLabel
	{
		get
		{
			return ProfileView.GetText(this.secretGemsLabel);
		}
		set
		{
			if (this.secretGemsLabel == null)
			{
				return;
			}
			ProfileView.SetText(this.secretGemsLabel, value);
			UISprite componentInChildren = this.secretGemsLabel.GetComponentInChildren<UISprite>();
			if (componentInChildren == null)
			{
				return;
			}
			Vector3 vector3 = this.secretGemsLabel.transform.localPosition;
			componentInChildren.transform.localPosition = new Vector3((float)(-this.secretGemsLabel.width), vector3.y, vector3.z);
		}
	}

	public string SurvivalScoreLabel
	{
		get
		{
			return ProfileView.GetText(this.survivalScoreLabel);
		}
		set
		{
			ProfileView.SetText(this.survivalScoreLabel, value);
		}
	}

	public string TeamBattleWinCount
	{
		get
		{
			return ProfileView.GetText(this.teamBattleWinCount);
		}
		set
		{
			ProfileView.SetText(this.teamBattleWinCount, value);
		}
	}

	public string TotalWeeklyWinCount
	{
		get
		{
			return ProfileView.GetText(this.totalWeeklyWinsCount);
		}
		set
		{
			ProfileView.SetText(this.totalWeeklyWinsCount, value);
		}
	}

	public string TotalWinCount
	{
		get
		{
			return ProfileView.GetText(this.totalWinCount);
		}
		set
		{
			ProfileView.SetText(this.totalWinCount, value);
		}
	}

	public string WaveCountLabel
	{
		get
		{
			return ProfileView.GetText(this.waveCountLabel);
		}
		set
		{
			ProfileView.SetText(this.waveCountLabel, value);
		}
	}

	public ProfileView()
	{
	}

	private void Awake()
	{
		if (this.copyIdButton != null && BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			this.copyIdButton.gameObject.SetActive(false);
		}
	}

	public void CheckBtnCopy()
	{
		base.StartCoroutine(this.Crt_CheckBtnCopy());
	}

	[DebuggerHidden]
	private IEnumerator Crt_CheckBtnCopy()
	{
		ProfileView.u003cCrt_CheckBtnCopyu003ec__Iterator17D variable = null;
		return variable;
	}

	private static string GetText(UILabel label)
	{
		if (label == null)
		{
			return string.Empty;
		}
		return label.text ?? string.Empty;
	}

	public void OnCopyIdClick()
	{
		FriendsController.CopyMyIdToClipboard();
	}

	public void OnSubmit()
	{
		if (this.nicknameInput == null)
		{
			return;
		}
		EventHandler<ProfileView.InputEventArgs> nicknameInput = this.NicknameInput;
		if (nicknameInput != null)
		{
			nicknameInput(this, new ProfileView.InputEventArgs(this.nicknameInput.@value));
		}
	}

	public void RemoveArmor()
	{
		this.characterView.RemoveArmor();
	}

	public void RemoveBoots()
	{
		this.characterView.RemoveBoots();
	}

	public void RemoveCape()
	{
		this.characterView.RemoveCape();
	}

	public void RemoveHat()
	{
		this.characterView.RemoveHat();
	}

	public void RemoveMask()
	{
		this.characterView.RemoveMask();
	}

	public void SetClanLogo(string logoBase64)
	{
		if (this.clanLogo == null)
		{
			UnityEngine.Debug.LogWarning("clanLogo == null");
			return;
		}
		Texture2D clanLogo = CharacterView.GetClanLogo(logoBase64);
		if (clanLogo != null)
		{
			this.clanLogo.mainTexture = clanLogo;
		}
		else
		{
			this.clanLogo.transform.parent.gameObject.SetActive(false);
		}
	}

	private static void SetText(UILabel label, string value)
	{
		if (label != null)
		{
			label.text = value ?? string.Empty;
		}
	}

	public void SetWeaponAndSkin(string tg, bool replaceRemovedWeapons)
	{
		this.characterView.SetWeaponAndSkin(tg, SkinsController.currentSkinForPers, replaceRemovedWeapons);
	}

	public void UpdateArmor(string armor)
	{
		this.characterView.UpdateArmor(armor);
		ShopNGUIController.SetPersArmorVisible(this.characterView.armorPoint);
	}

	public void UpdateBoots(string bs)
	{
		this.characterView.UpdateBoots(bs);
	}

	public void UpdateCape(string cape)
	{
		this.characterView.UpdateCape(cape, null);
	}

	public void UpdateHat(string hat)
	{
		this.characterView.UpdateHat(hat);
		ShopNGUIController.SetPersHatVisible(this.characterView.hatPoint);
	}

	public void UpdateMask(string mask)
	{
		this.characterView.UpdateMask(mask);
	}

	public event EventHandler BackButtonPressed
	{
		add
		{
			if (this.backButton != null)
			{
				this.backButton.Clicked += value;
			}
		}
		remove
		{
			if (this.backButton != null)
			{
				this.backButton.Clicked -= value;
			}
		}
	}

	public event EventHandler<ProfileView.InputEventArgs> NicknameInput
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.NicknameInput += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.NicknameInput -= value;
		}
	}

	public class InputEventArgs : EventArgs
	{
		public string Input
		{
			get;
			private set;
		}

		public InputEventArgs(string input)
		{
			this.Input = input ?? string.Empty;
		}
	}
}