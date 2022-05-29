using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class FriendPreview : MonoBehaviour
{
	public UILabel nm;

	public UITexture preview;

	public Texture2D mySkin;

	public UISprite rank;

	public bool facebookFriend;

	public bool ClanMember;

	public bool ClanInvite;

	public GameObject avatarButton;

	public GameObject @join;

	public GameObject delete;

	public GameObject addFacebookFriend;

	public GameObject cancel;

	public GameObject leader;

	public string id;

	public bool isInviteFromUs;

	public bool IsClanLeader;

	public UITexture ClanLogo;

	public UILabel clanName;

	public GameObject onlineStateContainer;

	public UILabel offline;

	public UILabel onlineLab;

	public UILabel playing;

	private float timeLastCheck;

	private readonly Lazy<ClansGUIController> _clansGuiController;

	private float inactivityStartTm;

	private bool _disableButtons;

	public FriendPreview()
	{
		this._clansGuiController = new Lazy<ClansGUIController>(new Func<ClansGUIController>(this.GetComponentInParent<ClansGUIController>));
	}

	public void DisableButtons()
	{
		this._disableButtons = true;
		this.delete.SetActive(false);
		if (this.facebookFriend)
		{
			this.addFacebookFriend.SetActive(false);
		}
		this.inactivityStartTm = Time.realtimeSinceStartup;
		this.UpdateOnline();
	}

	public void FillClanAttrs(Dictionary<string, string> plDict)
	{
		if (plDict == null)
		{
			return;
		}
		if (this.ClanMember)
		{
			string item = null;
			if (!string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
			{
				item = FriendsController.sharedController.clanLogo;
			}
			else if (plDict.ContainsKey("clan_logo") && plDict["clan_logo"] != null && plDict["clan_logo"] != null && !plDict["clan_logo"].Equals("null"))
			{
				item = plDict["clan_logo"];
			}
			if (item != null)
			{
				this.ClanLogo.gameObject.SetActive(true);
				try
				{
					byte[] numArray = Convert.FromBase64String(item);
					Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
					texture2D.LoadImage(numArray);
					texture2D.filterMode = FilterMode.Point;
					texture2D.Apply();
					Texture clanLogo = this.ClanLogo.mainTexture;
					this.ClanLogo.mainTexture = texture2D;
					if (clanLogo != null)
					{
						UnityEngine.Object.Destroy(clanLogo);
					}
				}
				catch (Exception exception)
				{
					Texture texture = this.ClanLogo.mainTexture;
					this.ClanLogo.mainTexture = null;
					if (texture != null)
					{
						UnityEngine.Object.Destroy(texture);
					}
				}
			}
		}
		else if (!plDict.ContainsKey("clan_logo") || string.IsNullOrEmpty(plDict["clan_logo"]) || plDict["clan_logo"].Equals("null"))
		{
			this.ClanLogo.gameObject.SetActive(false);
		}
		else
		{
			this.ClanLogo.gameObject.SetActive(true);
			try
			{
				byte[] numArray1 = Convert.FromBase64String(plDict["clan_logo"]);
				Texture2D texture2D1 = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
				texture2D1.LoadImage(numArray1);
				texture2D1.filterMode = FilterMode.Point;
				texture2D1.Apply();
				Texture clanLogo1 = this.ClanLogo.mainTexture;
				this.ClanLogo.mainTexture = texture2D1;
				if (clanLogo1 != null)
				{
					UnityEngine.Object.Destroy(clanLogo1);
				}
			}
			catch (Exception exception1)
			{
				Texture texture1 = this.ClanLogo.mainTexture;
				this.ClanLogo.mainTexture = null;
				if (texture1 != null)
				{
					UnityEngine.Object.Destroy(texture1);
				}
			}
		}
		if (!plDict.ContainsKey("clan_name") || plDict["clan_name"] == null || plDict["clan_name"].Equals("null"))
		{
			this.clanName.gameObject.SetActive(false);
		}
		else
		{
			this.clanName.gameObject.SetActive(true);
			string str = plDict["clan_name"];
			int num = 12;
			if (str != null && str.Length > num)
			{
				str = string.Format("{0}..{1}", str.Substring(0, (num - 2) / 2), str.Substring(str.Length - (num - 2) / 2, (num - 2) / 2));
			}
			if (str != null)
			{
				this.clanName.text = str;
			}
		}
		if (plDict.ContainsKey("clan_creator_id") && plDict["clan_creator_id"] != null && this.id != null)
		{
			bool flag = plDict["clan_creator_id"].Equals(this.id);
			this.leader.SetActive(flag);
			this.avatarButton.GetComponent<UIButton>().normalSprite = (!flag ? "avatar_frame" : "avatar_leader_frame");
		}
	}

	private Color[] GetPixelsByRect(Texture2D texture, Rect rect)
	{
		return texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
	}

	private Texture2D getTexFromTexByRect(Texture2D texForCut, Rect rectForCut)
	{
		Color[] pixels = texForCut.GetPixels((int)rectForCut.x, (int)rectForCut.y, (int)rectForCut.width, (int)rectForCut.height);
		Texture2D texture2D = new Texture2D((int)rectForCut.width, (int)rectForCut.height)
		{
			filterMode = FilterMode.Point
		};
		texture2D.SetPixels(pixels);
		texture2D.Apply();
		return texture2D;
	}

	public void HandleAvatarClick()
	{
		Action<bool> action = (bool needUpdate) => {
		};
		FriendsController.ShowProfile(this.id, ProfileWindowType.other, action);
	}

	private bool IsFriendInClan()
	{
		Dictionary<string, Dictionary<string, object>> strs = FriendsController.sharedController.playersInfo;
		if (strs == null)
		{
			return false;
		}
		if (!strs.ContainsKey(this.id))
		{
			return false;
		}
		if (!strs[this.id].ContainsKey("player"))
		{
			return false;
		}
		Dictionary<string, object> item = strs[this.id]["player"] as Dictionary<string, object>;
		if (item == null)
		{
			return false;
		}
		if (!item.ContainsKey("clan_creator_id"))
		{
			return false;
		}
		string str = Convert.ToString(item["clan_creator_id"]);
		return !string.IsNullOrEmpty(str);
	}

	private void OnDestroy()
	{
		FriendsGUIController.UpdaeOnlineEvent -= new Action(this.UpdateOnline);
	}

	public void SetSkin(string skinStr)
	{
		bool flag = true;
		if (string.IsNullOrEmpty(skinStr) || skinStr.Equals("empty"))
		{
			this.mySkin = Resources.Load(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1")) as Texture2D;
			flag = false;
		}
		else
		{
			byte[] numArray = Convert.FromBase64String(skinStr);
			Texture2D texture2D = new Texture2D(64, 32);
			texture2D.LoadImage(numArray);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			this.mySkin = texture2D;
		}
		Texture2D texture2D1 = new Texture2D(20, 20, TextureFormat.ARGB32, false);
		for (int i = 0; i < 20; i++)
		{
			for (int j = 0; j < 20; j++)
			{
				texture2D1.SetPixel(i, j, Color.clear);
			}
		}
		texture2D1.SetPixels(6, 6, 8, 8, this.GetPixelsByRect(this.mySkin, new Rect(8f, 16f, 8f, 8f)));
		texture2D1.SetPixels(6, 0, 8, 6, this.GetPixelsByRect(this.mySkin, new Rect(20f, 6f, 8f, 6f)));
		texture2D1.SetPixels(2, 0, 4, 6, this.GetPixelsByRect(this.mySkin, new Rect(44f, 6f, 4f, 6f)));
		texture2D1.SetPixels(14, 0, 4, 6, this.GetPixelsByRect(this.mySkin, new Rect(44f, 6f, 4f, 6f)));
		texture2D1.anisoLevel = 1;
		texture2D1.mipMapBias = -0.5f;
		texture2D1.Apply();
		texture2D1.filterMode = FilterMode.Point;
		if (flag)
		{
			UnityEngine.Object.Destroy(this.mySkin);
		}
		Texture texture = this.preview.mainTexture;
		this.preview.mainTexture = texture2D1;
		if (texture != null && !texture.name.Equals("dude") && !texture.name.Equals("multi_skin_1"))
		{
			UnityEngine.Object.Destroy(texture);
		}
	}

	private void Start()
	{
		if (!this.facebookFriend && !this.ClanMember && !this.ClanInvite)
		{
			FriendsGUIController.UpdaeOnlineEvent += new Action(this.UpdateOnline);
		}
		if (this.isInviteFromUs && this.preview != null)
		{
			this.preview.alpha = 0.4f;
		}
		this.@join.SetActive((!this.@join.activeSelf || this.facebookFriend && !this.ClanMember ? false : !this.ClanInvite));
		this.delete.SetActive((!this.delete.activeSelf || this.facebookFriend ? false : !this.ClanInvite));
		this.@join.GetComponent<UIButton>().isEnabled = false;
		if (this.onlineStateContainer != null)
		{
			this.onlineStateContainer.SetActive(!this.facebookFriend);
		}
		this.addFacebookFriend.SetActive((this.facebookFriend ? true : this.ClanInvite));
		if (this.ClanInvite)
		{
			this.addFacebookFriend.GetComponent<UIButton>().isEnabled = !FriendsController.sharedController.ClanLimitReached;
			UIButton component = this.avatarButton.GetComponent<UIButton>();
			if (component != null)
			{
				EventDelegate eventDelegate = new EventDelegate(() => {
					if (this._clansGuiController.Value != null)
					{
						ClansGUIController.State currentState = this._clansGuiController.Value.CurrentState;
						Action<bool> value = (bool needUpdateFriendList) => {
							if (this._clansGuiController.Value != null)
							{
								this._clansGuiController.Value.CurrentState = currentState;
								this._clansGuiController.Value.ShowAddMembersScreen();
							}
						};
						FriendsController.ShowProfile(this.id, ProfileWindowType.other, value);
						this._clansGuiController.Value.CurrentState = ClansGUIController.State.ProfileDetails;
					}
				});
				component.onClick.Add(eventDelegate);
			}
		}
		bool flag = true;
		this.cancel.SetActive((!this.ClanInvite ? false : flag));
		this.avatarButton.GetComponent<UIButton>().enabled = !this.facebookFriend;
		if (this.facebookFriend || this.ClanInvite)
		{
			UnityEngine.Object.Destroy(this.avatarButton.GetComponent<FriendPreviewClicker>());
		}
		this.UpdateInfo();
	}

	private void Update()
	{
		if (this.ClanInvite)
		{
			this.addFacebookFriend.SetActive((FriendsController.sharedController.ClanSentInvites.Contains(this.id) || FriendsController.sharedController.clanSentInvitesLocal.Contains(this.id) ? false : !FriendsController.sharedController.friendsDeletedLocal.Contains(this.id)));
			this.addFacebookFriend.GetComponent<UIButton>().isEnabled = !FriendsController.sharedController.ClanLimitReached;
			this.cancel.SetActive((!FriendsController.sharedController.ClanSentInvites.Contains(this.id) || FriendsController.sharedController.clanCancelledInvitesLocal.Contains(this.id) ? false : !FriendsController.sharedController.friendsDeletedLocal.Contains(this.id)));
		}
		if (this.ClanMember)
		{
			bool flag = false;
			foreach (Dictionary<string, string> clanMember in FriendsController.sharedController.clanMembers)
			{
				if (!clanMember.ContainsKey("id") || !clanMember["id"].Equals(this.id))
				{
					continue;
				}
				flag = true;
				break;
			}
			this.delete.SetActive((!flag || FriendsController.sharedController.clanDeletedLocal.Contains(this.id) || FriendsController.sharedController.id == null ? false : FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID)));
		}
		if (Time.realtimeSinceStartup - this.inactivityStartTm > 25f)
		{
			this.inactivityStartTm = Single.PositiveInfinity;
			if (!this.isInviteFromUs)
			{
				this._disableButtons = false;
				this.UpdateOnline();
			}
			this.delete.SetActive((this.facebookFriend ? false : !this.ClanInvite));
		}
		if (Time.realtimeSinceStartup - this.timeLastCheck > 1f)
		{
			this.timeLastCheck = Time.realtimeSinceStartup;
			this.UpdateOnline();
			this.UpdateInfo();
		}
		if (this.facebookFriend && !this._disableButtons)
		{
			bool flag1 = false;
			if (FriendsController.sharedController.friends != null)
			{
				foreach (string friend in FriendsController.sharedController.friends)
				{
					if (!friend.Equals(this.id))
					{
						continue;
					}
					flag1 = true;
					break;
				}
			}
			this.addFacebookFriend.SetActive(!flag1);
		}
	}

	private void UpdateInfo()
	{
		Dictionary<string, object> strs;
		object obj;
		string str;
		object obj1;
		if (this.facebookFriend)
		{
			return;
		}
		if (this.id != null)
		{
			if (this.ClanMember)
			{
				foreach (Dictionary<string, string> clanMember in FriendsController.sharedController.clanMembers)
				{
					if (!clanMember.TryGetValue("id", out str) || !this.id.Equals(str))
					{
						continue;
					}
					if (clanMember.ContainsKey("nick"))
					{
						this.nm.text = clanMember["nick"];
					}
					if (clanMember.ContainsKey("rank"))
					{
						this.rank.spriteName = string.Concat("Rank_", clanMember["rank"]);
					}
					if (clanMember.ContainsKey("skin"))
					{
						this.SetSkin(clanMember["skin"]);
					}
					Dictionary<string, string> strs1 = new Dictionary<string, string>();
					if (!FriendsController.sharedController.playersInfo.ContainsKey(str) || !FriendsController.sharedController.playersInfo[str].TryGetValue("player", out obj1))
					{
						if (!string.IsNullOrEmpty(FriendsController.sharedController.clanName))
						{
							strs1.Add("clan_name", FriendsController.sharedController.clanName);
						}
						if (!string.IsNullOrEmpty(FriendsController.sharedController.clanLeaderID))
						{
							strs1.Add("clan_creator_id", FriendsController.sharedController.clanLeaderID);
						}
					}
					else
					{
						foreach (KeyValuePair<string, object> keyValuePair in obj1 as Dictionary<string, object>)
						{
							strs1.Add(keyValuePair.Key, Convert.ToString(keyValuePair.Value));
						}
					}
					this.FillClanAttrs(strs1);
					break;
				}
			}
			else if (FriendsController.sharedController.playersInfo.TryGetValue(this.id, out strs) && strs.TryGetValue("player", out obj))
			{
				Dictionary<string, object> strs2 = obj as Dictionary<string, object>;
				this.nm.text = strs2["nick"] as string;
				this.rank.spriteName = string.Concat("Rank_", Convert.ToString(strs2["rank"]));
				this.SetSkin(strs2["skin"] as string);
				Dictionary<string, string> strs3 = new Dictionary<string, string>();
				foreach (KeyValuePair<string, object> keyValuePair1 in strs2)
				{
					strs3.Add(keyValuePair1.Key, Convert.ToString(keyValuePair1.Value));
				}
				this.FillClanAttrs(strs3);
			}
		}
	}

	private void UpdateOnline()
	{
		int num;
		int num1;
		int num2;
		string str;
		if (this.facebookFriend)
		{
			return;
		}
		if (!FriendsController.sharedController.onlineInfo.ContainsKey(this.id))
		{
			this.offline.gameObject.SetActive(true);
			this.onlineLab.gameObject.SetActive(false);
			this.playing.gameObject.SetActive(false);
			this.@join.GetComponent<UIButton>().isEnabled = false;
		}
		else
		{
			string item = FriendsController.sharedController.onlineInfo[this.id]["game_mode"];
			string item1 = FriendsController.sharedController.onlineInfo[this.id]["delta"];
			string str1 = FriendsController.sharedController.onlineInfo[this.id]["protocol"];
			int num3 = int.Parse(item);
			if (num3 <= 99)
			{
				num3 = -1;
			}
			else
			{
				num3 /= 100;
			}
			if (int.TryParse(item, out num1))
			{
				if (num1 > 99)
				{
					num1 = num1 - num3 * 100;
				}
				num1 /= 10;
			}
			else
			{
				num1 = -1;
			}
			if (int.TryParse(item1, out num))
			{
				if ((float)num > FriendsController.onlineDelta || num3 != 3 && (num3 != (int)ConnectSceneNGUIController.myPlatformConnect && num3 != -1 || ExpController.GetOurTier() != num1))
				{
					this.offline.gameObject.SetActive(true);
					this.onlineLab.gameObject.SetActive(false);
					this.playing.gameObject.SetActive(false);
					this.@join.GetComponent<UIButton>().isEnabled = false;
				}
				else
				{
					string str2 = str1;
					string multiplayerProtocolVersion = GlobalGameController.MultiplayerProtocolVersion;
					if (item != null && int.TryParse(item, out num2))
					{
						if (num2 != -1)
						{
							this.offline.gameObject.SetActive(false);
							this.onlineLab.gameObject.SetActive(false);
							this.playing.gameObject.SetActive(true);
							this.@join.GetComponent<UIButton>().isEnabled = (this._disableButtons ? false : multiplayerProtocolVersion == str2);
							if (FriendsController.sharedController.onlineInfo[this.id].TryGetValue("map", out str) && SceneInfoController.instance.GetInfoScene(int.Parse(str)) == null)
							{
								this.@join.GetComponent<UIButton>().isEnabled = false;
							}
						}
						else
						{
							this.offline.gameObject.SetActive(false);
							this.onlineLab.gameObject.SetActive(true);
							this.playing.gameObject.SetActive(false);
							this.@join.GetComponent<UIButton>().isEnabled = false;
						}
					}
				}
			}
		}
	}
}