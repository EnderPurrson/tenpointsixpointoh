using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class FriendProfileController : IDisposable
	{
		public static string currentFriendId;

		private bool _disposed;

		private FriendProfileView _friendProfileView;

		private GameObject _friendProfileViewGo;

		private IFriendsGUIController _friendsGuiController;

		private string _friendId = string.Empty;

		private ProfileWindowType _windowType;

		private bool _needUpdateFriendList;

		private bool _isPlayerOurFriend;

		private Action<bool> OnCloseEvent;

		public GameObject FriendProfileGo
		{
			get
			{
				return this._friendProfileViewGo;
			}
		}

		static FriendProfileController()
		{
		}

		public FriendProfileController(IFriendsGUIController friendsGuiController, bool oldInterface = true)
		{
			this.Initialize(friendsGuiController, oldInterface);
		}

		public FriendProfileController(Action<bool> onCloseEvent)
		{
			this.Initialize(null, false);
			this.OnCloseEvent = onCloseEvent;
		}

		public void CallbackClanInviteRequest(bool isComplete, bool isRequestExist)
		{
			this._friendProfileView.SetEnableInviteClanButton(true);
			InfoWindowController.CheckShowRequestServerInfoBox(isComplete, isRequestExist);
			if (isComplete)
			{
				this.SetWindowStateByFriendAndClanData(this._friendId, this._windowType);
			}
		}

		public void CallbackFriendAddRequest(bool isComplete, bool isRequestExist)
		{
			this.OnCompleteAddOrDeleteResponse(isComplete, isRequestExist, true);
		}

		public void CallbackRequestDeleteFriend(bool isComplete)
		{
			AnalyticsFacade.SendCustomEvent("Social", new Dictionary<string, object>()
			{
				{ "Deleted Friends", "Profile" }
			});
			this.OnCompleteAddOrDeleteResponse(isComplete, false, false);
		}

		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			FriendPreviewClicker.FriendPreviewClicked -= new Action<string>(this.HandleProfileClicked);
			this._friendProfileView.BackButtonClickEvent -= new Action(this.HandleBackClicked);
			this._friendProfileView.JoinButtonClickEvent -= new Action(this.HandleJoinClicked);
			this._friendProfileView.CopyMyIdButtonClickEvent -= new Action(this.HandleCopyMyIdClicked);
			this._friendProfileView.ChatButtonClickEvent -= new Action(this.HandleChatClicked);
			this._friendProfileView.AddButtonClickEvent -= new Action(this.HandleAddFriendClicked);
			this._friendProfileView.RemoveButtonClickEvent -= new Action(this.HandleRemoveFriendClicked);
			this._friendProfileView.InviteToClanButtonClickEvent -= new Action(this.HandleInviteToClanClicked);
			this._friendProfileView.UpdateRequested -= new Action(this.HandleUpdateRequested);
			FriendsController.FullInfoUpdated -= new Action(this.HandleUpdateRequested);
			this._friendProfileView = null;
			UnityEngine.Object.DestroyObject(this._friendProfileViewGo);
			this._friendProfileViewGo = null;
			this._disposed = true;
		}

		private void HandleAddFriendClicked()
		{
			this._friendProfileView.SetEnableAddButton(false);
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "Added Friends", "Profile" },
				{ "Deleted Friends", "Add" }
			};
			Dictionary<string, object> strs1 = strs;
			FriendsController.SendFriendshipRequest(this._friendId, strs1, new Action<bool, bool>(this.CallbackFriendAddRequest));
		}

		public void HandleBackClicked()
		{
			this._friendProfileView.Reset();
			this._friendProfileViewGo.SetActive(false);
			FriendsController.sharedController.StopRefreshingInfo();
			if (this._friendsGuiController != null)
			{
				this._friendsGuiController.Hide(false);
			}
			else if (this.OnCloseEvent != null)
			{
				this.OnCloseEvent(this._needUpdateFriendList);
			}
		}

		private void HandleChatClicked()
		{
			this.HandleBackClicked();
			if (this._windowType == ProfileWindowType.friend)
			{
				FriendsWindowController instance = FriendsWindowController.Instance;
				if (instance != null)
				{
					instance.SetActiveChatTab(this._friendId);
				}
			}
		}

		private void HandleCopyMyIdClicked()
		{
			FriendsController.CopyPlayerIdToClipboard(this._friendId);
		}

		private void HandleInviteToClanClicked()
		{
			this._friendProfileView.SetEnableInviteClanButton(false);
			FriendsController.SendPlayerInviteToClan(this._friendId, new Action<bool, bool>(this.CallbackClanInviteRequest));
		}

		private void HandleJoinClicked()
		{
			ButtonClickSound.TryPlayClick();
			if (!this._friendProfileView.IsCanConnectToFriend)
			{
				InfoWindowController.ShowInfoBox(this._friendProfileView.NotConnectCondition);
				return;
			}
			if (FriendsController.sharedController.onlineInfo.ContainsKey(this._friendId))
			{
				int num = int.Parse(FriendsController.sharedController.onlineInfo[this._friendId]["game_mode"]);
				string item = FriendsController.sharedController.onlineInfo[this._friendId]["room_name"];
				string str = FriendsController.sharedController.onlineInfo[this._friendId]["map"];
				if (SceneInfoController.instance.GetInfoScene(int.Parse(str)) != null)
				{
					JoinRoomFromFrends.sharedJoinRoomFromFrends.ConnectToRoom(num, item, str);
				}
			}
		}

		internal void HandleProfileClicked(string id)
		{
			this.HandleProfileClickedCore(id, ProfileWindowType.other, null);
		}

		internal void HandleProfileClickedCore(string id, ProfileWindowType type, Action<bool> onCloseEvent)
		{
			if (this._disposed)
			{
				return;
			}
			this.OnCloseEvent = onCloseEvent;
			this._needUpdateFriendList = false;
			this._friendId = id;
			this._friendProfileView.FriendId = id;
			this._windowType = type;
			FriendProfileController.currentFriendId = id;
			this._friendProfileView.Reset();
			this._isPlayerOurFriend = FriendsController.IsPlayerOurFriend(id);
			this.Update();
			if (this._friendsGuiController != null)
			{
				this._friendsGuiController.Hide(true);
			}
			FriendsController.sharedController.StartRefreshingInfo(this._friendId);
			this._friendProfileViewGo.SetActive(true);
			this.SetWindowStateByFriendAndClanData(this._friendId, type);
		}

		private void HandleRemoveFriendClicked()
		{
			this._friendProfileView.SetEnableRemoveButton(false);
			FriendsController.DeleteFriend(this._friendId, new Action<bool>(this.CallbackRequestDeleteFriend));
		}

		private void HandleUpdateRequested()
		{
			this.Update();
		}

		private void Initialize(IFriendsGUIController friendsGuiController, bool oldInterface = true)
		{
			this._friendsGuiController = friendsGuiController;
			string str = (!oldInterface ? "FriendProfileView(UI)" : "FriendProfileView");
			this._friendProfileViewGo = UnityEngine.Object.Instantiate(Resources.Load(str)) as GameObject;
			if (this._friendProfileViewGo == null)
			{
				this._disposed = true;
				return;
			}
			this._friendProfileViewGo.SetActive(false);
			this._friendProfileView = this._friendProfileViewGo.GetComponent<FriendProfileView>();
			if (this._friendProfileView == null)
			{
				UnityEngine.Object.DestroyObject(this._friendProfileViewGo);
				this._friendProfileViewGo = null;
				this._disposed = true;
				return;
			}
			FriendPreviewClicker.FriendPreviewClicked += new Action<string>(this.HandleProfileClicked);
			this._friendProfileView.BackButtonClickEvent += new Action(this.HandleBackClicked);
			this._friendProfileView.JoinButtonClickEvent += new Action(this.HandleJoinClicked);
			this._friendProfileView.CopyMyIdButtonClickEvent += new Action(this.HandleCopyMyIdClicked);
			this._friendProfileView.ChatButtonClickEvent += new Action(this.HandleChatClicked);
			this._friendProfileView.AddButtonClickEvent += new Action(this.HandleAddFriendClicked);
			this._friendProfileView.RemoveButtonClickEvent += new Action(this.HandleRemoveFriendClicked);
			this._friendProfileView.InviteToClanButtonClickEvent += new Action(this.HandleInviteToClanClicked);
			this._friendProfileView.UpdateRequested += new Action(this.HandleUpdateRequested);
			FriendsController.FullInfoUpdated += new Action(this.HandleUpdateRequested);
		}

		private void OnCompleteAddOrDeleteResponse(bool isComplete, bool isRequestExist, bool isAddRequest)
		{
			if (!isAddRequest)
			{
				this._friendProfileView.SetEnableRemoveButton(true);
			}
			else
			{
				this._friendProfileView.SetEnableAddButton(true);
			}
			InfoWindowController.CheckShowRequestServerInfoBox(isComplete, isRequestExist);
			if (isComplete)
			{
				this._needUpdateFriendList = true;
				this._isPlayerOurFriend = FriendsController.IsPlayerOurFriend(this._friendId);
				this.SetWindowStateByFriendAndClanData(this._friendId, this._windowType);
			}
		}

		private void SetDefaultStateProfile()
		{
		}

		private void SetTitle(string playerId, ProfileWindowType type)
		{
			bool flag = FriendsController.IsPlayerOurClanMember(playerId);
			if (this._isPlayerOurFriend && flag)
			{
				if (type != ProfileWindowType.clan)
				{
					this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1526"));
				}
				else
				{
					this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1527"));
				}
			}
			else if (this._isPlayerOurFriend)
			{
				this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1526"));
			}
			else if (!flag)
			{
				this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1525"));
			}
			else
			{
				this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1527"));
			}
		}

		private void SetupStateBottomButtons(string playerId, ProfileWindowType type)
		{
			bool flag = FriendsController.IsPlayerOurClanMember(playerId);
			bool flag1 = FriendsController.IsSelfClanLeader();
			bool flag2 = FriendsController.IsMyPlayerId(playerId);
			bool flag3 = FriendsController.IsAlreadySendInvitePlayer(playerId);
			bool flag4 = FriendsController.IsAlreadySendClanInvitePlayer(playerId);
			bool flag5 = FriendsController.IsFriendsMax();
			bool flag6 = FriendsController.IsMaxClanMembers();
			bool flag7 = (!this._isPlayerOurFriend || type != ProfileWindowType.friend ? false : !flag2);
			bool flag8 = (flag || !flag1 || flag2 ? false : !flag6);
			bool flag9 = (this._isPlayerOurFriend || flag2 ? false : !flag5);
			bool flag10 = (!this._isPlayerOurFriend ? false : !flag2);
			this._friendProfileView.SetActiveAddButton((!flag9 ? false : !flag3));
			this._friendProfileView.SetActiveAddButtonSent((!flag9 ? false : flag3));
			this._friendProfileView.SetActiveInviteButton((!flag8 ? false : !flag4));
			this._friendProfileView.SetActiveAddClanButtonSent((!flag8 ? false : flag4));
			this._friendProfileView.SetActiveChatButton(flag7);
			this._friendProfileView.SetActiveRemoveButton(flag10);
		}

		private void SetWindowStateByFriendAndClanData(string playerId, ProfileWindowType type)
		{
			this.SetTitle(playerId, type);
			this.SetupStateBottomButtons(playerId, type);
		}

		private void Update()
		{
			if (string.IsNullOrEmpty(this._friendId))
			{
				return;
			}
			this.UpdateAllData(this._friendId);
		}

		private void UpdateAccessories(Dictionary<string, object> playerInfo)
		{
			object obj;
			object obj1;
			object obj2;
			int num;
			object obj3;
			if (playerInfo == null || playerInfo.Count == 0)
			{
				return;
			}
			if (playerInfo.TryGetValue("accessories", out obj))
			{
				List<object> objs = obj as List<object>;
				if (objs != null)
				{
					IEnumerator<Dictionary<string, object>> enumerator = objs.OfType<Dictionary<string, object>>().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Dictionary<string, object> current = enumerator.Current;
							string empty = string.Empty;
							if (current.TryGetValue("name", out obj1))
							{
								empty = obj1 as string ?? string.Empty;
							}
							if (!current.TryGetValue("type", out obj2) || !int.TryParse(obj2 as string, out num))
							{
								continue;
							}
							switch (num)
							{
								case 0:
								{
									if (!empty.Equals("cape_Custom", StringComparison.Ordinal))
									{
										this._friendProfileView.SetStockCape(empty);
									}
									else if (current.TryGetValue("skin", out obj3))
									{
										string str = obj3 as string;
										if (!string.IsNullOrEmpty(str))
										{
											byte[] numArray = Convert.FromBase64String(str);
											this._friendProfileView.SetCustomCape(numArray);
										}
									}
									continue;
								}
								case 1:
								{
									this._friendProfileView.SetHat(empty);
									continue;
								}
								case 2:
								{
									this._friendProfileView.SetBoots(empty);
									continue;
								}
								case 3:
								{
									this._friendProfileView.SetArmor(empty);
									continue;
								}
								case 4:
								{
									this._friendProfileView.SetMask(empty);
									continue;
								}
							}
						}
					}
					finally
					{
						if (enumerator == null)
						{
						}
						enumerator.Dispose();
					}
				}
			}
		}

		private void UpdateAllData(string friendId)
		{
			Dictionary<string, object> fullPlayerDataById = FriendsController.GetFullPlayerDataById(friendId);
			if (fullPlayerDataById == null)
			{
				return;
			}
			this.UpdatePlayer(fullPlayerDataById);
			this.UpdateScores(fullPlayerDataById);
			this.UpdateAccessories(fullPlayerDataById);
			FriendsController friendsController = FriendsController.sharedController;
			if (friendsController != null)
			{
				Dictionary<string, Dictionary<string, string>> strs = friendsController.onlineInfo;
				if (strs.ContainsKey(friendId))
				{
					this.UpdateOnline(strs[friendId]);
				}
				else if (!this._isPlayerOurFriend)
				{
					this._friendProfileView.Online = OnlineState.none;
				}
				else
				{
					this._friendProfileView.Online = OnlineState.offline;
				}
			}
		}

		private void UpdateOnline(Dictionary<string, string> onlineInfo)
		{
			FriendsController.ResultParseOnlineData resultParseOnlineDatum = FriendsController.ParseOnlineData(onlineInfo);
			if (resultParseOnlineDatum == null)
			{
				this._friendProfileView.Online = OnlineState.none;
				return;
			}
			this._friendProfileView.Online = resultParseOnlineDatum.GetOnlineStatus();
			this._friendProfileView.FriendGameMode = resultParseOnlineDatum.GetGameModeName();
			this._friendProfileView.FriendLocation = resultParseOnlineDatum.GetMapName();
			this._friendProfileView.IsCanConnectToFriend = resultParseOnlineDatum.IsCanConnect;
			this._friendProfileView.NotConnectCondition = resultParseOnlineDatum.GetNotConnectConditionString();
		}

		private void UpdatePlayer(Dictionary<string, object> playerInfo)
		{
			object obj;
			int num;
			object obj1;
			object obj2;
			int num1;
			object obj3;
			object obj4;
			object obj5;
			object obj6;
			object obj7;
			int num2;
			if (playerInfo == null || playerInfo.Count == 0)
			{
				Debug.LogWarning("playerInfo == null || playerInfo.Count == 0");
				return;
			}
			Dictionary<string, object> item = null;
			item = playerInfo["player"] as Dictionary<string, object>;
			if (item != null)
			{
				if (!item.TryGetValue("friends", out obj) || !int.TryParse(obj as string, out num))
				{
					this._friendProfileView.FriendCount = -1;
				}
				else
				{
					this._friendProfileView.FriendCount = num;
				}
				if (item.TryGetValue("nick", out obj1))
				{
					this._friendProfileView.FriendName = obj1 as string;
				}
				if (item.TryGetValue("rank", out obj2) && int.TryParse(Convert.ToString(obj2), out num1))
				{
					this._friendProfileView.Rank = num1;
				}
				if (item.TryGetValue("skin", out obj3))
				{
					string str = obj3 as string;
					if (!string.IsNullOrEmpty(str))
					{
						byte[] numArray = Convert.FromBase64String(str);
						if (numArray != null && (int)numArray.Length > 0)
						{
							this._friendProfileView.SetSkin(numArray);
						}
					}
				}
				if (item.TryGetValue("clan_name", out obj4))
				{
					this._friendProfileView.clanName.gameObject.SetActive(true);
					string str1 = obj4 as string;
					if (string.IsNullOrEmpty(str1))
					{
						this._friendProfileView.clanName.gameObject.SetActive(false);
					}
					else
					{
						int num3 = 10000;
						if (str1 != null && str1.Length > num3)
						{
							str1 = string.Format("{0}..{1}", str1.Substring(0, (num3 - 2) / 2), str1.Substring(str1.Length - (num3 - 2) / 2, (num3 - 2) / 2));
						}
						this._friendProfileView.clanName.text = str1 ?? string.Empty;
					}
				}
				if (item.TryGetValue("clan_logo", out obj5))
				{
					string str2 = obj5 as string;
					if (string.IsNullOrEmpty(str2))
					{
						this._friendProfileView.clanLogo.gameObject.SetActive(false);
					}
					else
					{
						this._friendProfileView.clanLogo.gameObject.SetActive(true);
						byte[] numArray1 = Convert.FromBase64String(str2);
						if (numArray1 != null && (int)numArray1.Length > 0)
						{
							try
							{
								Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
								texture2D.LoadImage(numArray1);
								texture2D.filterMode = FilterMode.Point;
								texture2D.Apply();
								Texture texture = this._friendProfileView.clanLogo.mainTexture;
								this._friendProfileView.clanLogo.mainTexture = texture2D;
								if (texture != null)
								{
									UnityEngine.Object.DestroyImmediate(texture, true);
								}
							}
							catch (Exception exception)
							{
								Texture texture1 = this._friendProfileView.clanLogo.mainTexture;
								this._friendProfileView.clanLogo.mainTexture = null;
								if (texture1 != null)
								{
									UnityEngine.Object.DestroyImmediate(texture1, true);
								}
							}
						}
					}
				}
				string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
				this._friendProfileView.Username = playerNameOrDefault;
				if (!item.TryGetValue("wins", out obj6))
				{
					this._friendProfileView.WinCount = -1;
				}
				else
				{
					int num4 = Convert.ToInt32(obj6);
					this._friendProfileView.WinCount = num4;
				}
				if (!item.TryGetValue("total_wins", out obj7))
				{
					this._friendProfileView.TotalWinCount = -1;
				}
				else if (!int.TryParse(obj7 as string, out num2))
				{
					Debug.LogWarning(string.Concat("Can not parse “total_wins” field: ", obj7));
				}
				else
				{
					this._friendProfileView.TotalWinCount = num2;
				}
			}
		}

		private void UpdateScores(Dictionary<string, object> playerInfo)
		{
			object obj;
			object obj1;
			int num;
			if (playerInfo == null || playerInfo.Count == 0)
			{
				return;
			}
			if (playerInfo.TryGetValue("scores", out obj))
			{
				List<object> objs = obj as List<object>;
				if (objs != null)
				{
					IEnumerable<Dictionary<string, object>> dictionaries = objs.OfType<Dictionary<string, object>>();
					if (dictionaries.Any<Dictionary<string, object>>())
					{
						Dictionary<string, object> strs = dictionaries.FirstOrDefault<Dictionary<string, object>>((Dictionary<string, object> d) => (!d.ContainsKey("game") ? false : d["game"].Equals("0")));
						if (strs != null)
						{
							if (!strs.TryGetValue("max_score", out obj1) || !int.TryParse(obj1 as string, out num))
							{
								this._friendProfileView.SurvivalScore = -1;
							}
							else
							{
								this._friendProfileView.SurvivalScore = num;
							}
						}
					}
				}
			}
		}

		private enum AccessoriesType
		{
			cape,
			hat,
			boots,
			armor,
			mask
		}
	}
}