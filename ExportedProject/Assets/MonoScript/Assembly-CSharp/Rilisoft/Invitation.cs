using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class Invitation : MonoBehaviour
	{
		public UILabel nm;

		public GameObject accept;

		public GameObject reject;

		public GameObject JoinClan;

		public GameObject RejectClan;

		public GameObject youAlready;

		public UISprite rank;

		public string id;

		public string recordId;

		public bool outgoing;

		public bool IsClanInv;

		public UITexture ClanLogo;

		public string clanLogoString;

		private float timeLastCheck;

		private float _timeToUpdateInactivityState;

		private float inactivityStartTm;

		public Invitation()
		{
		}

		public void DisableButtons()
		{
			if (this.accept != null)
			{
				this.accept.SetActive(false);
			}
			this.reject.SetActive(false);
			this.inactivityStartTm = Time.realtimeSinceStartup;
			if (this.JoinClan != null)
			{
				this.JoinClan.SetActive(false);
			}
			if (this.RejectClan != null)
			{
				this.RejectClan.SetActive(false);
			}
		}

		public void KeepClanData()
		{
			FriendsController.sharedController.tempClanID = this.id;
			FriendsController.sharedController.tempClanLogo = this.clanLogoString ?? string.Empty;
			FriendsController.sharedController.tempClanName = this.nm.text ?? string.Empty;
		}

		private void Start()
		{
			this._timeToUpdateInactivityState = 25f;
			this.inactivityStartTm = Single.PositiveInfinity;
			this.UpdateInfo();
			if (this.JoinClan != null)
			{
				this.JoinClan.SetActive((!this.IsClanInv || !string.IsNullOrEmpty(FriendsController.sharedController.ClanID) ? false : string.IsNullOrEmpty(FriendsController.sharedController.JoinClanSent)));
			}
			if (this.RejectClan != null)
			{
				this.RejectClan.SetActive(this.IsClanInv);
			}
			if (this.youAlready != null)
			{
				this.youAlready.SetActive((!this.IsClanInv ? false : !string.IsNullOrEmpty(FriendsController.sharedController.ClanID)));
			}
			if (this.ClanLogo != null)
			{
				this.ClanLogo.gameObject.SetActive(this.IsClanInv);
			}
			if (this.accept != null)
			{
				this.accept.SetActive(!this.IsClanInv);
			}
			this.reject.SetActive(!this.IsClanInv);
			this.rank.gameObject.SetActive(!this.IsClanInv);
		}

		private void Update()
		{
			if (Time.realtimeSinceStartup - this.inactivityStartTm > this._timeToUpdateInactivityState)
			{
				this.inactivityStartTm = Single.PositiveInfinity;
				if (this.accept != null)
				{
					this.accept.SetActive(true);
				}
				this.reject.SetActive(!this.IsClanInv);
				if (this.JoinClan != null)
				{
					this.JoinClan.SetActive((!this.IsClanInv || !string.IsNullOrEmpty(FriendsController.sharedController.ClanID) ? false : string.IsNullOrEmpty(FriendsController.sharedController.JoinClanSent)));
				}
				if (this.RejectClan != null)
				{
					this.RejectClan.SetActive(this.IsClanInv);
				}
				if (this.youAlready != null)
				{
					this.youAlready.SetActive((!this.IsClanInv ? false : !string.IsNullOrEmpty(FriendsController.sharedController.ClanID)));
				}
			}
			if (Time.realtimeSinceStartup - this.timeLastCheck > 1f)
			{
				this.timeLastCheck = Time.realtimeSinceStartup;
				this.UpdateInfo();
			}
		}

		private void UpdateInfo()
		{
			string str;
			string str1;
			string str2;
			Dictionary<string, object> strs;
			object obj;
			if (!this.IsClanInv)
			{
				if (this.id != null && FriendsController.sharedController.playersInfo.TryGetValue(this.id, out strs) && strs.TryGetValue("player", out obj))
				{
					Dictionary<string, object> strs1 = obj as Dictionary<string, object>;
					this.nm.text = strs1["nick"] as string;
					string str3 = Convert.ToString(strs1["rank"]);
					this.rank.spriteName = string.Concat("Rank_", (!str3.Equals("0") ? str3 : "1"));
				}
				return;
			}
			foreach (Dictionary<string, string> clanInvite in FriendsController.sharedController.ClanInvites)
			{
				if (!clanInvite.TryGetValue("id", out str) || !str.Equals(this.id))
				{
					continue;
				}
				if (clanInvite.TryGetValue("logo", out str1))
				{
					try
					{
						byte[] numArray = Convert.FromBase64String(str1);
						Texture2D texture2D = new Texture2D(8, 8, TextureFormat.ARGB32, false);
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
				if (clanInvite.TryGetValue("name", out str2))
				{
					this.nm.text = str2;
				}
				break;
			}
		}
	}
}