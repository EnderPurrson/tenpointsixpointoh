using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class FriendProfileView : MonoBehaviour
{
	private const string DefaultStatisticString = "-";

	public Transform pers;

	public GameObject[] bootsPoint;

	public GameObject capePoint;

	public GameObject armorPoint;

	public GameObject hatPoint;

	public GameObject maskPoint;

	public GameObject characterModel;

	public GameObject armorLeftPl;

	public GameObject armorRightPl;

	public UISprite rankSprite;

	public UILabel friendCountLabel;

	public UILabel friendLocationLabel;

	public UILabel friendGameModeLabel;

	public UILabel friendNameLabel;

	public UILabel survivalScoreLabel;

	public UILabel winCountLabel;

	public UILabel totalWinCountLabel;

	public UILabel clanName;

	public UILabel friendIdLabel;

	public UILabel[] titlesLabel;

	public UITexture clanLogo;

	[Header("Online state settings")]
	public UILabel inFriendStateLabel;

	[Header("Online state settings")]
	public UILabel offlineStateLabel;

	[Header("Online state settings")]
	public UILabel playingStateLabel;

	public UISprite inFriendState;

	public UISprite offlineState;

	public UISprite playingState;

	public GameObject playingStateInfoContainer;

	[Header("Buttons settings")]
	public UIButton backButton;

	public UIButton joinButton;

	public UIButton sendMyIdButton;

	public UIButton chatButton;

	public UIButton inviteToClanButton;

	public UIButton addFriendButton;

	public UIButton removeFriendButton;

	public UITable buttonAlignContainer;

	public UILabel addOrRemoveButtonLabel;

	public UISprite notConnectJoinButtonSprite;

	public UISprite addFrienButtonSentState;

	public UISprite addClanButtonSentState;

	private IDisposable _backSubscription;

	private bool _escapePressed;

	private float lastTime;

	private float idleTimerLastTime;

	private readonly Lazy<Rect> _touchZone = new Lazy<Rect>(new Func<Rect>(() => new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height)));

	private Action BackButtonClickEvent;

	private Action JoinButtonClickEvent;

	private Action CopyMyIdButtonClickEvent;

	private Action ChatButtonClickEvent;

	private Action AddButtonClickEvent;

	private Action RemoveButtonClickEvent;

	private Action InviteToClanButtonClickEvent;

	private Action UpdateRequested;

	public int FriendCount
	{
		get;
		set;
	}

	public string FriendGameMode
	{
		get;
		set;
	}

	public string FriendId
	{
		get;
		set;
	}

	public string FriendLocation
	{
		get;
		set;
	}

	public string FriendName
	{
		get;
		set;
	}

	public bool IsCanConnectToFriend
	{
		get;
		set;
	}

	public string NotConnectCondition
	{
		get;
		set;
	}

	public OnlineState Online
	{
		get;
		set;
	}

	public int Rank
	{
		get;
		set;
	}

	public int SurvivalScore
	{
		get;
		set;
	}

	public int TotalWinCount
	{
		get;
		set;
	}

	public string Username
	{
		get;
		set;
	}

	public int WinCount
	{
		get;
		set;
	}

	public FriendProfileView()
	{
	}

	private void Awake()
	{
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		this.Reset();
		if (this.backButton != null)
		{
			EventDelegate.Add(this.backButton.onClick, new EventDelegate.Callback(this.OnBackButtonClick));
		}
		if (this.joinButton != null)
		{
			EventDelegate.Add(this.joinButton.onClick, new EventDelegate.Callback(this.OnJoinButtonClick));
		}
		if (this.sendMyIdButton != null)
		{
			EventDelegate.Add(this.sendMyIdButton.onClick, new EventDelegate.Callback(this.OnSendMyIdButtonClick));
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				this.sendMyIdButton.gameObject.SetActive(false);
			}
		}
		if (this.chatButton != null)
		{
			EventDelegate.Add(this.chatButton.onClick, new EventDelegate.Callback(this.OnChatButtonClick));
		}
		if (this.addFriendButton != null)
		{
			EventDelegate.Add(this.addFriendButton.onClick, new EventDelegate.Callback(this.OnAddButtonClick));
		}
		if (this.removeFriendButton != null)
		{
			EventDelegate.Add(this.removeFriendButton.onClick, new EventDelegate.Callback(this.OnRemoveButtonClick));
		}
		if (this.inviteToClanButton != null)
		{
			EventDelegate.Add(this.inviteToClanButton.onClick, new EventDelegate.Callback(this.OnInviteToClanButtonClick));
		}
	}

	private void HandleEscape()
	{
		if (!InfoWindowController.IsActive)
		{
			this._escapePressed = true;
		}
	}

	private void OnAddButtonClick()
	{
		if (this.AddButtonClickEvent != null)
		{
			this.AddButtonClickEvent();
		}
	}

	private void OnBackButtonClick()
	{
		if (this.BackButtonClickEvent != null)
		{
			this.BackButtonClickEvent();
		}
	}

	private void OnChatButtonClick()
	{
		if (this.ChatButtonClickEvent != null)
		{
			this.ChatButtonClickEvent();
		}
	}

	private void OnDisable()
	{
		base.StopCoroutine("RequestUpdate");
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		base.StartCoroutine("RequestUpdate");
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Friend Profile");
	}

	private void OnInviteToClanButtonClick()
	{
		if (this.InviteToClanButtonClickEvent != null)
		{
			this.InviteToClanButtonClickEvent();
		}
	}

	private void OnJoinButtonClick()
	{
		if (this.JoinButtonClickEvent != null)
		{
			this.JoinButtonClickEvent();
		}
	}

	private void OnRemoveButtonClick()
	{
		if (this.RemoveButtonClickEvent != null)
		{
			this.RemoveButtonClickEvent();
		}
	}

	private void OnSendMyIdButtonClick()
	{
		if (this.CopyMyIdButtonClickEvent != null)
		{
			this.CopyMyIdButtonClickEvent();
		}
	}

	[DebuggerHidden]
	[Obfuscation(Exclude=true)]
	private IEnumerator RequestUpdate()
	{
		FriendProfileView.u003cRequestUpdateu003ec__Iterator13D variable = null;
		return variable;
	}

	public void Reset()
	{
		GameObject gameObject;
		this.IsCanConnectToFriend = false;
		this.FriendLocation = string.Empty;
		this.FriendCount = 0;
		this.FriendName = string.Empty;
		this.Online = (!FriendsController.IsPlayerOurFriend(this.FriendId) ? OnlineState.none : OnlineState.offline);
		this.Rank = 0;
		this.SurvivalScore = 0;
		this.Username = string.Empty;
		this.WinCount = 0;
		if (this.characterModel != null)
		{
			Texture texture = Resources.Load<Texture>(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1"));
			if (texture != null)
			{
				GameObject[] gameObjectArray = new GameObject[7];
				if (this.bootsPoint == null || (int)this.bootsPoint.Length <= 0)
				{
					gameObject = null;
				}
				else
				{
					gameObject = this.bootsPoint[0].transform.parent.gameObject;
				}
				gameObjectArray[0] = gameObject;
				gameObjectArray[1] = this.hatPoint;
				gameObjectArray[2] = this.capePoint;
				gameObjectArray[3] = this.armorPoint;
				gameObjectArray[4] = this.armorLeftPl;
				gameObjectArray[5] = this.armorRightPl;
				gameObjectArray[6] = this.maskPoint;
				Player_move_c.SetTextureRecursivelyFrom(this.characterModel, texture, gameObjectArray);
			}
		}
		this.SetOnlineState(this.Online);
		if (this.bootsPoint != null && (int)this.bootsPoint.Length > 0)
		{
			GameObject[] gameObjectArray1 = this.bootsPoint;
			for (int i = 0; i < (int)gameObjectArray1.Length; i++)
			{
				gameObjectArray1[i].SetActive(false);
			}
		}
		if (this.hatPoint != null)
		{
			Transform transforms = this.hatPoint.transform;
			for (int j = 0; j != transforms.childCount; j++)
			{
				UnityEngine.Object.Destroy(transforms.GetChild(j).gameObject);
			}
		}
		if (this.maskPoint != null)
		{
			Transform transforms1 = this.maskPoint.transform;
			for (int k = 0; k != transforms1.childCount; k++)
			{
				UnityEngine.Object.Destroy(transforms1.GetChild(k).gameObject);
			}
		}
		if (this.capePoint != null)
		{
			Transform transforms2 = this.capePoint.transform;
			for (int l = 0; l != transforms2.childCount; l++)
			{
				UnityEngine.Object.Destroy(transforms2.GetChild(l).gameObject);
			}
		}
		if (this.armorPoint != null)
		{
			Transform transforms3 = this.armorPoint.transform;
			for (int m = 0; m != transforms3.childCount; m++)
			{
				UnityEngine.Object.Destroy(transforms3.GetChild(m).gameObject);
			}
		}
		this.SetEnableAddButton(true);
		this.SetEnableInviteClanButton(true);
	}

	private void ReturnPersTonNormState()
	{
		HOTween.Kill(this.pers);
		Vector3 vector3 = new Vector3(0f, -180f, 0f);
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		HOTween.To(this.pers, 0.5f, (new TweenParms()).Prop("localRotation", new PlugQuaternion(vector3)).Ease(EaseType.Linear).OnComplete(() => this.idleTimerLastTime = Time.realtimeSinceStartup));
	}

	public void SetActiveAddButton(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.addFriendButton.gameObject, isActive);
	}

	public void SetActiveAddButtonSent(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.addFrienButtonSentState.gameObject, isActive);
	}

	public void SetActiveAddClanButtonSent(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.addClanButtonSentState.gameObject, isActive);
	}

	private void SetActiveAndRepositionButtons(GameObject button, bool isActive)
	{
		bool flag = button.activeSelf;
		button.SetActive(isActive);
		if (flag != isActive)
		{
			this.buttonAlignContainer.Reposition();
			this.buttonAlignContainer.repositionNow = true;
		}
	}

	public void SetActiveChatButton(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.chatButton.gameObject, isActive);
	}

	public void SetActiveInviteButton(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.inviteToClanButton.gameObject, isActive);
	}

	public void SetActiveRemoveButton(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.removeFriendButton.gameObject, isActive);
	}

	public void SetArmor(string armorName)
	{
		if (string.IsNullOrEmpty(armorName))
		{
			UnityEngine.Debug.LogWarning("Name of armor should not be empty.");
			return;
		}
		List<Transform> transforms = new List<Transform>();
		for (int i = 0; i < this.armorPoint.transform.childCount; i++)
		{
			transforms.Add(this.armorPoint.transform.GetChild(i));
		}
		foreach (Transform transform in transforms)
		{
			ArmorRefs component = transform.GetChild(0).GetComponent<ArmorRefs>();
			if (component == null)
			{
				continue;
			}
			if (component.leftBone != null)
			{
				component.leftBone.parent = transform.GetChild(0);
			}
			if (component.rightBone != null)
			{
				component.rightBone.parent = transform.GetChild(0);
			}
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		if (!armorName.Equals(Defs.ArmorNewNoneEqupped))
		{
			UnityEngine.Object obj = Resources.Load(string.Concat("Armor/", armorName));
			if (obj == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
			ArmorRefs armorRef = gameObject.transform.GetChild(0).GetComponent<ArmorRefs>();
			if (armorRef != null)
			{
				armorRef.leftBone.parent = this.armorLeftPl.transform;
				armorRef.leftBone.localPosition = Vector3.zero;
				armorRef.leftBone.localRotation = Quaternion.identity;
				armorRef.leftBone.localScale = Vector3.one;
				armorRef.rightBone.parent = this.armorRightPl.transform;
				armorRef.rightBone.localPosition = Vector3.zero;
				armorRef.rightBone.localRotation = Quaternion.identity;
				armorRef.rightBone.localScale = Vector3.one;
				gameObject.transform.parent = this.armorPoint.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				Player_move_c.SetLayerRecursively(gameObject, this.armorPoint.layer);
			}
		}
	}

	public void SetBoots(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			UnityEngine.Debug.LogWarning("Name of boots should not be empty.");
			return;
		}
		if (this.bootsPoint != null && (int)this.bootsPoint.Length > 0)
		{
			for (int i = 0; i != (int)this.bootsPoint.Length; i++)
			{
				this.bootsPoint[i].SetActive(this.bootsPoint[i].name.Equals(name));
			}
		}
	}

	public void SetCustomCape(byte[] capeBytes)
	{
		if (this.capePoint != null)
		{
			Transform transforms = this.capePoint.transform;
			for (int i = 0; i != transforms.childCount; i++)
			{
				UnityEngine.Object.Destroy(transforms.GetChild(i).gameObject);
			}
			UnityEngine.Object obj = Resources.Load("Capes/cape_Custom");
			if (obj != null)
			{
				capeBytes = capeBytes ?? new byte[0];
				Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
				texture2D.LoadImage(capeBytes);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(obj);
				gameObject.transform.parent = transforms;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				Player_move_c.SetLayerRecursively(gameObject, this.capePoint.layer);
				gameObject.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
				Player_move_c.SetTextureRecursivelyFrom(gameObject, texture2D, new GameObject[0]);
			}
		}
	}

	public void SetEnableAddButton(bool enable)
	{
		if (this.addFriendButton != null)
		{
			this.addFriendButton.isEnabled = enable;
		}
	}

	public void SetEnableInviteClanButton(bool enable)
	{
		if (this.inviteToClanButton != null)
		{
			this.inviteToClanButton.isEnabled = enable;
		}
	}

	public void SetEnableRemoveButton(bool enable)
	{
		if (this.removeFriendButton != null)
		{
			this.removeFriendButton.isEnabled = enable;
		}
	}

	public void SetHat(string hatName)
	{
		if (string.IsNullOrEmpty(hatName))
		{
			UnityEngine.Debug.LogWarning("Name of hat should not be empty.");
			return;
		}
		if (this.hatPoint != null)
		{
			Transform transforms = this.hatPoint.transform;
			for (int i = 0; i != transforms.childCount; i++)
			{
				UnityEngine.Object.Destroy(transforms.GetChild(i).gameObject);
			}
			UnityEngine.Object obj = Resources.Load(string.Concat("Hats/", hatName));
			if (obj != null)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(obj);
				gameObject.transform.parent = transforms;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				Player_move_c.SetLayerRecursively(gameObject, this.hatPoint.layer);
			}
		}
	}

	public void SetMask(string maskName)
	{
		if (string.IsNullOrEmpty(maskName))
		{
			UnityEngine.Debug.LogWarning("Name of mask should not be empty.");
			return;
		}
		if (this.maskPoint != null)
		{
			Transform transforms = this.maskPoint.transform;
			for (int i = 0; i != transforms.childCount; i++)
			{
				UnityEngine.Object.Destroy(transforms.GetChild(i).gameObject);
			}
			UnityEngine.Object obj = Resources.Load(string.Concat("Masks/", maskName));
			if (obj != null)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(obj);
				gameObject.transform.parent = transforms;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				Player_move_c.SetLayerRecursively(gameObject, this.maskPoint.layer);
			}
		}
	}

	private void SetOnlineState(OnlineState onlineState)
	{
		bool flag = onlineState == OnlineState.offline;
		bool flag1 = onlineState == OnlineState.inFriends;
		bool flag2 = onlineState == OnlineState.playing;
		this.offlineStateLabel.Do<UILabel>((UILabel l) => l.gameObject.SetActive(flag));
		this.inFriendStateLabel.Do<UILabel>((UILabel l) => l.gameObject.SetActive(flag1));
		this.playingStateLabel.Do<UILabel>((UILabel l) => l.gameObject.SetActive(flag2));
		this.offlineState.Do<UISprite>((UISprite l) => l.gameObject.SetActive(flag));
		this.inFriendState.Do<UISprite>((UISprite l) => l.gameObject.SetActive(flag1));
		this.playingState.Do<UISprite>((UISprite l) => l.gameObject.SetActive(flag2));
		if (this.playingStateInfoContainer != null)
		{
			this.playingStateInfoContainer.SetActive(flag2);
		}
	}

	public void SetSkin(byte[] skinBytes)
	{
		GameObject gameObject;
		skinBytes = skinBytes ?? new byte[0];
		if (this.characterModel != null)
		{
			Func<byte[], Texture2D> func = (byte[] bytes) => {
				Texture2D texture2D = new Texture2D(64, 32)
				{
					filterMode = FilterMode.Point
				};
				texture2D.LoadImage(bytes);
				texture2D.Apply();
				return texture2D;
			};
			Texture2D texture2D1 = ((int)skinBytes.Length <= 0 ? Resources.Load<Texture2D>(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1")) : func(skinBytes));
			GameObject[] gameObjectArray = new GameObject[7];
			if (this.bootsPoint == null || (int)this.bootsPoint.Length <= 0)
			{
				gameObject = null;
			}
			else
			{
				gameObject = this.bootsPoint[0].transform.parent.gameObject;
			}
			gameObjectArray[0] = gameObject;
			gameObjectArray[1] = this.hatPoint;
			gameObjectArray[2] = this.capePoint;
			gameObjectArray[3] = this.armorPoint;
			gameObjectArray[4] = this.armorLeftPl;
			gameObjectArray[5] = this.armorRightPl;
			gameObjectArray[6] = this.maskPoint;
			Player_move_c.SetTextureRecursivelyFrom(this.characterModel, texture2D1, gameObjectArray);
		}
	}

	public void SetStockCape(string capeName)
	{
		if (string.IsNullOrEmpty(capeName))
		{
			UnityEngine.Debug.LogWarning("Name of cape should not be empty.");
			return;
		}
		if (this.capePoint != null)
		{
			Transform transforms = this.capePoint.transform;
			for (int i = 0; i != transforms.childCount; i++)
			{
				UnityEngine.Object.Destroy(transforms.GetChild(i).gameObject);
			}
			UnityEngine.Object obj = Resources.Load(string.Concat("Capes/", capeName));
			if (obj != null)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(obj);
				gameObject.transform.parent = transforms;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				Player_move_c.SetLayerRecursively(gameObject, this.capePoint.layer);
			}
		}
	}

	public void SetTitle(string titleText)
	{
		for (int i = 0; i < (int)this.titlesLabel.Length; i++)
		{
			this.titlesLabel[i].text = titleText;
		}
	}

	private void Update()
	{
		if (this._escapePressed)
		{
			this._escapePressed = false;
			this.OnBackButtonClick();
			return;
		}
		this.UpdateLightweight();
		float single = -120f;
		single = single * (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android ? 0.5f : 2f);
		Rect value = this._touchZone.Value;
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Moved && value.Contains(touch.position))
			{
				this.idleTimerLastTime = Time.realtimeSinceStartup;
				Transform transforms = this.pers;
				Vector3 vector3 = Vector3.up;
				Vector2 vector2 = touch.deltaPosition;
				transforms.Rotate(vector3, vector2.x * single * 0.5f * (Time.realtimeSinceStartup - this.lastTime));
			}
		}
		if (Application.isEditor)
		{
			float axis = Input.GetAxis("Mouse ScrollWheel") * 3f * single * (Time.realtimeSinceStartup - this.lastTime);
			this.pers.Rotate(Vector3.up, axis);
			if (axis != 0f)
			{
				this.idleTimerLastTime = Time.realtimeSinceStartup;
			}
		}
		if (Time.realtimeSinceStartup - this.idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			this.ReturnPersTonNormState();
		}
		this.lastTime = Time.realtimeSinceStartup;
	}

	private void UpdateLightweight()
	{
		if (this.friendLocationLabel != null)
		{
			this.friendLocationLabel.text = this.FriendLocation ?? string.Empty;
		}
		if (this.friendCountLabel != null)
		{
			this.friendCountLabel.text = (this.FriendCount >= 0 ? this.FriendCount.ToString() : "-");
		}
		if (this.friendNameLabel != null)
		{
			this.friendNameLabel.text = this.FriendName ?? string.Empty;
		}
		this.SetOnlineState(this.Online);
		this.notConnectJoinButtonSprite.alpha = (!this.IsCanConnectToFriend ? 1f : 0f);
		if (this.rankSprite != null)
		{
			string str = string.Concat("Rank_", this.Rank);
			if (!this.rankSprite.spriteName.Equals(str))
			{
				this.rankSprite.spriteName = str;
			}
		}
		if (this.survivalScoreLabel != null)
		{
			this.survivalScoreLabel.text = (this.SurvivalScore >= 0 ? this.SurvivalScore.ToString() : "-");
		}
		if (this.winCountLabel != null)
		{
			this.winCountLabel.text = (this.WinCount >= 0 ? this.WinCount.ToString() : "-");
		}
		if (this.totalWinCountLabel != null)
		{
			this.totalWinCountLabel.text = (this.TotalWinCount >= 0 ? this.TotalWinCount.ToString() : "-");
		}
		if (this.friendGameModeLabel != null)
		{
			this.friendGameModeLabel.text = this.FriendGameMode;
		}
		if (this.friendIdLabel != null)
		{
			this.friendIdLabel.text = this.FriendId;
		}
	}

	public event Action AddButtonClickEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.AddButtonClickEvent += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.AddButtonClickEvent -= value;
		}
	}

	public event Action BackButtonClickEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.BackButtonClickEvent += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.BackButtonClickEvent -= value;
		}
	}

	public event Action ChatButtonClickEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.ChatButtonClickEvent += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.ChatButtonClickEvent -= value;
		}
	}

	public event Action CopyMyIdButtonClickEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.CopyMyIdButtonClickEvent += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.CopyMyIdButtonClickEvent -= value;
		}
	}

	public event Action InviteToClanButtonClickEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.InviteToClanButtonClickEvent += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.InviteToClanButtonClickEvent -= value;
		}
	}

	public event Action JoinButtonClickEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.JoinButtonClickEvent += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.JoinButtonClickEvent -= value;
		}
	}

	public event Action RemoveButtonClickEvent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.RemoveButtonClickEvent += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.RemoveButtonClickEvent -= value;
		}
	}

	public event Action UpdateRequested
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.UpdateRequested += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.UpdateRequested -= value;
		}
	}
}