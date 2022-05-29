using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FacebookFriendsGUIController : MonoBehaviour
{
	public static FacebookFriendsGUIController sharedController;

	public bool _infoRequested;

	public FacebookFriendsGUIController()
	{
	}

	private void GetFacebookFriendsCallback()
	{
		if (FriendsController.sharedController == null || FriendsController.sharedController.facebookFriendsInfo == null)
		{
			return;
		}
		Dictionary<string, Dictionary<string, object>> strs = new Dictionary<string, Dictionary<string, object>>();
		foreach (string key in FriendsController.sharedController.facebookFriendsInfo.Keys)
		{
			bool flag = false;
			if (FriendsController.sharedController.friends != null)
			{
				foreach (string friend in FriendsController.sharedController.friends)
				{
					if (!friend.Equals(key))
					{
						continue;
					}
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			strs.Add(key, FriendsController.sharedController.facebookFriendsInfo[key]);
		}
		UIGrid componentInChildren = base.GetComponentInChildren<UIGrid>();
		if (componentInChildren == null)
		{
			return;
		}
		FriendPreview[] componentsInChildren = base.GetComponentsInChildren<FriendPreview>(true) ?? new FriendPreview[0];
		Dictionary<string, FriendPreview> strs1 = new Dictionary<string, FriendPreview>();
		FriendPreview[] friendPreviewArray = componentsInChildren;
		for (int i = 0; i < (int)friendPreviewArray.Length; i++)
		{
			FriendPreview friendPreview = friendPreviewArray[i];
			if (friendPreview.id == null || !strs.ContainsKey(friendPreview.id))
			{
				friendPreview.transform.parent = null;
				UnityEngine.Object.Destroy(friendPreview.gameObject);
			}
			else
			{
				strs1.Add(friendPreview.id, friendPreview);
			}
		}
		foreach (KeyValuePair<string, Dictionary<string, object>> str in strs)
		{
			Dictionary<string, string> strs2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> value in str.Value)
			{
				strs2.Add(value.Key, value.Value as string);
			}
			if (!strs1.ContainsKey(str.Key))
			{
				GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Friend") as GameObject);
				vector3.transform.parent = componentInChildren.transform;
				vector3.transform.localScale = new Vector3(1f, 1f, 1f);
				vector3.GetComponent<FriendPreview>().facebookFriend = true;
				vector3.GetComponent<FriendPreview>().id = str.Key;
				if (str.Value.ContainsKey("nick"))
				{
					vector3.GetComponent<FriendPreview>().nm.text = str.Value["nick"] as string;
				}
				if (str.Value.ContainsKey("rank"))
				{
					string item = str.Value["rank"] as string;
					if (item.Equals("0"))
					{
						item = "1";
					}
					vector3.GetComponent<FriendPreview>().rank.spriteName = string.Concat("Rank_", item);
				}
				if (str.Value.ContainsKey("skin"))
				{
					vector3.GetComponent<FriendPreview>().SetSkin(str.Value["skin"] as string);
				}
				vector3.GetComponent<FriendPreview>().FillClanAttrs(strs2);
			}
			else
			{
				GameObject gameObject = strs1[str.Key].gameObject;
				gameObject.GetComponent<FriendPreview>().facebookFriend = true;
				gameObject.GetComponent<FriendPreview>().id = str.Key;
				if (str.Value.ContainsKey("nick"))
				{
					gameObject.GetComponent<FriendPreview>().nm.text = str.Value["nick"] as string;
				}
				if (str.Value.ContainsKey("rank"))
				{
					string item1 = str.Value["rank"] as string;
					if (item1.Equals("0"))
					{
						item1 = "1";
					}
					gameObject.GetComponent<FriendPreview>().rank.spriteName = string.Concat("Rank_", item1);
				}
				if (str.Value.ContainsKey("skin"))
				{
					gameObject.GetComponent<FriendPreview>().SetSkin(str.Value["skin"] as string);
				}
				gameObject.GetComponent<FriendPreview>().FillClanAttrs(strs2);
			}
		}
		base.StartCoroutine(this.Repos(componentInChildren));
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			FriendsController.sharedController.facebookFriendsInfo.Clear();
			this._infoRequested = false;
		}
	}

	private void OnDestroy()
	{
		FriendsController.sharedController.GetFacebookFriendsCallback = null;
		FacebookFriendsGUIController.sharedController = null;
	}

	[DebuggerHidden]
	private IEnumerator Repos(UIGrid grid)
	{
		FacebookFriendsGUIController.u003cReposu003ec__Iterator26 variable = null;
		return variable;
	}

	private void Start()
	{
		FacebookFriendsGUIController.sharedController = this;
	}

	private void Update()
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (FacebookController.sharedController.friendsList == null || FacebookController.sharedController.friendsList.Count == 0)
		{
			return;
		}
		if (FriendsController.sharedController.facebookFriendsInfo.Count == 0 && !this._infoRequested && FriendsController.sharedController.GetFacebookFriendsCallback == null)
		{
			FriendsController.sharedController.GetFacebookFriendsInfo(new Action(this.GetFacebookFriendsCallback));
			this._infoRequested = true;
		}
	}
}