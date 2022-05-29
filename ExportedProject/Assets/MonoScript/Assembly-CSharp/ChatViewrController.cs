using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class ChatViewrController : MonoBehaviour
{
	public static ChatViewrController sharedController;

	public MyUIInput sendMessageInput;

	public MyUIInput sendMessageInputDater;

	public GameObject fastCommands;

	public Transform chatLabelsHolder;

	private List<ChatLabel> labelChat = new List<ChatLabel>();

	public GameObject buySmileBannerPrefab;

	public Transform smilePanelTransform;

	public GameObject smilesPanel;

	public GameObject showSmileButton;

	public GameObject hideSmileButton;

	public GameObject buySmileButton;

	public GameObject chatLabelPrefab;

	public AudioClip sendChatClip;

	public bool isClanMode;

	public UIButton sendMessageButton;

	public Transform bottomAnchor;

	public UITable labelTable;

	public UIScrollView scrollLabels;

	[NonSerialized]
	public bool isBuySmile;

	private float keyboardSize;

	public bool isShowSmilePanel;

	private bool needReset;

	static ChatViewrController()
	{
	}

	public ChatViewrController()
	{
	}

	private void Awake()
	{
		this.isBuySmile = StickersController.IsBuyAnyPack();
		this.buySmileBannerPrefab.SetActive(false);
		this.hideSmileButton.SetActive(false);
		this.sendMessageInput.gameObject.SetActive(false);
		this.sendMessageInput = this.sendMessageInputDater;
		this.fastCommands.SetActive(false);
		if (!this.isBuySmile)
		{
			this.showSmileButton.SetActive(false);
			this.buySmileButton.SetActive(true);
		}
		else
		{
			this.showSmileButton.SetActive(true);
			this.buySmileButton.SetActive(false);
		}
		if (this.sendMessageInput != null)
		{
			this.sendMessageInput.onKeyboardInter += new Action(this.SendMessageFromInput);
			this.sendMessageInput.onKeyboardCancel += new Action(this.CancelSendPrivateMessage);
			this.sendMessageInput.onKeyboardVisible += new Action(this.OnKeyboardVisible);
			this.sendMessageInput.onKeyboardHide += new Action(this.OnKeyboardHide);
		}
	}

	public void BuySmileOnClick()
	{
		this.buySmileBannerPrefab.SetActive(true);
		this.sendMessageInput.isSelected = false;
		this.sendMessageInput.DeselectInput();
	}

	public void CancelSendPrivateMessage()
	{
		this.sendMessageInput.@value = string.Empty;
	}

	public void CloseChat(bool isHardClose = false)
	{
		if (!isHardClose && this.buySmileBannerPrefab.activeSelf)
		{
			UnityEngine.Debug.LogFormat("Ignoring CloseChat({0}), buySmiley: {1}", new object[] { isHardClose, this.buySmileBannerPrefab.activeSelf });
			return;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.showChat = false;
			WeaponManager.sharedManager.myPlayerMoveC.AddButtonHandlers();
			WeaponManager.sharedManager.myPlayerMoveC.inGameGUI.gameObject.SetActive(true);
			if (!WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
			{
				WeaponManager.sharedManager.currentWeaponSounds.gameObject.SetActive(true);
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.mechPoint.SetActive(true);
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
		ChatViewrController.sharedController = null;
	}

	public void HandleCloseChat()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("[Close Chat] pressed.");
		}
		this.CloseChat(false);
	}

	private void HideSmilePannel()
	{
		this.isShowSmilePanel = false;
		this.showSmileButton.SetActive(true);
		this.hideSmileButton.SetActive(false);
	}

	public void HideSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.HideSmilePannel();
	}

	private void LateUpdate()
	{
		if (this.needReset)
		{
			this.needReset = false;
			this.scrollLabels.ResetPosition();
		}
	}

	public void OnClickSendMessageFromButton()
	{
		if (!string.IsNullOrEmpty(this.sendMessageInput.@value))
		{
			this.PostChat(this.sendMessageInput.@value);
			this.sendMessageInput.@value = string.Empty;
		}
		if (this.isShowSmilePanel)
		{
			this.HideSmilePannel();
		}
		this.needReset = true;
	}

	private void OnDestroy()
	{
		this.sendMessageInput.onKeyboardInter -= new Action(this.SendMessageFromInput);
		this.sendMessageInput.onKeyboardCancel -= new Action(this.CancelSendPrivateMessage);
		this.sendMessageInput.onKeyboardVisible -= new Action(this.OnKeyboardVisible);
		this.sendMessageInput.onKeyboardHide -= new Action(this.OnKeyboardHide);
		ChatViewrController.sharedController = null;
	}

	private void OnDisable()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.messageDelegate -= new Player_move_c.OnMessagesUpdate(this.UpdateMessages);
		}
	}

	private void OnEnable()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.messageDelegate += new Player_move_c.OnMessagesUpdate(this.UpdateMessages);
		}
		this.UpdateMessages();
	}

	public void OnKeyboardHide()
	{
		Transform vector3 = this.bottomAnchor;
		float single = this.bottomAnchor.localPosition.x;
		Vector3 vector31 = this.bottomAnchor.localPosition;
		float coef = vector31.y - this.keyboardSize / Defs.Coef;
		Vector3 vector32 = this.bottomAnchor.localPosition;
		vector3.localPosition = new Vector3(single, coef, vector32.z);
		base.StartCoroutine(this.ResetpositionCoroutine());
		this.smilePanelTransform.gameObject.SetActive(false);
		this.smilePanelTransform.gameObject.SetActive(true);
	}

	public void OnKeyboardVisible()
	{
		this.keyboardSize = this.sendMessageInput.heightKeyboard;
		if (Application.isEditor)
		{
			this.keyboardSize = 200f;
		}
		Transform vector3 = this.bottomAnchor;
		float single = this.bottomAnchor.localPosition.x;
		Vector3 vector31 = this.bottomAnchor.localPosition;
		float coef = vector31.y + this.keyboardSize / Defs.Coef;
		Vector3 vector32 = this.bottomAnchor.localPosition;
		vector3.localPosition = new Vector3(single, coef, vector32.z);
		base.StartCoroutine(this.ResetpositionCoroutine());
	}

	public void PostChat(string _text)
	{
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.sendChatClip);
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SendChat(_text, this.isClanMode, string.Empty);
		}
	}

	[DebuggerHidden]
	private IEnumerator ResetpositionCoroutine()
	{
		ChatViewrController.u003cResetpositionCoroutineu003ec__IteratorF variable = null;
		return variable;
	}

	public void SendMessageFromInput()
	{
		if (!string.IsNullOrEmpty(this.sendMessageInput.@value))
		{
			this.PostChat(this.sendMessageInput.@value);
			this.sendMessageInput.@value = string.Empty;
		}
		if (this.isShowSmilePanel)
		{
			this.HideSmilePannel();
		}
	}

	public void ShowSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.isShowSmilePanel = true;
		this.showSmileButton.SetActive(false);
		this.hideSmileButton.SetActive(true);
		this.scrollLabels.ResetPosition();
	}

	private void Start()
	{
		ChatViewrController.sharedController = this;
		if (this.sendMessageInput != null)
		{
			this.sendMessageInput.isSelected = true;
		}
	}

	private void Update()
	{
		if (this.isShowSmilePanel && this.smilePanelTransform.localPosition.y < -150f)
		{
			this.smilesPanel.SetActive(false);
			this.smilesPanel.SetActive(true);
			Transform vector3 = this.smilePanelTransform;
			float single = this.smilePanelTransform.localPosition.x;
			Vector3 vector31 = this.smilePanelTransform.localPosition;
			Vector3 vector32 = this.smilePanelTransform.localPosition;
			vector3.localPosition = new Vector3(single, vector31.y + Time.deltaTime * 500f, vector32.z);
			this.scrollLabels.MoveRelative((Vector3.up * Time.deltaTime) * 500f);
			if (this.smilePanelTransform.localPosition.y > -150f)
			{
				Transform transforms = this.smilePanelTransform;
				float single1 = this.smilePanelTransform.localPosition.x;
				Vector3 vector33 = this.smilePanelTransform.localPosition;
				transforms.localPosition = new Vector3(single1, -150f, vector33.z);
				this.scrollLabels.ResetPosition();
				this.smilePanelTransform.gameObject.SetActive(false);
				this.smilePanelTransform.gameObject.SetActive(true);
			}
		}
		if (!this.isShowSmilePanel && this.smilePanelTransform.localPosition.y > -314f)
		{
			Transform transforms1 = this.smilePanelTransform;
			float single2 = this.smilePanelTransform.localPosition.x;
			Vector3 vector34 = this.smilePanelTransform.localPosition;
			Vector3 vector35 = this.smilePanelTransform.localPosition;
			transforms1.localPosition = new Vector3(single2, vector34.y - Time.deltaTime * 500f, vector35.z);
			this.scrollLabels.MoveRelative((Vector3.down * Time.deltaTime) * 500f);
			if (this.smilePanelTransform.localPosition.y < -314f)
			{
				Transform transforms2 = this.smilePanelTransform;
				float single3 = this.smilePanelTransform.localPosition.x;
				Vector3 vector36 = this.smilePanelTransform.localPosition;
				transforms2.localPosition = new Vector3(single3, -314f, vector36.z);
				this.scrollLabels.ResetPosition();
				this.smilePanelTransform.gameObject.SetActive(false);
				this.smilePanelTransform.gameObject.SetActive(true);
			}
		}
		if (this.sendMessageButton.isEnabled == string.IsNullOrEmpty(this.sendMessageInput.@value))
		{
			this.sendMessageButton.isEnabled = !string.IsNullOrEmpty(this.sendMessageInput.@value);
		}
	}

	private void UpdateMessages()
	{
		if (WeaponManager.sharedManager.myPlayer == null)
		{
			return;
		}
		Player_move_c playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		while (this.labelChat.Count < playerMoveC.messages.Count)
		{
			GameObject gameObject = NGUITools.AddChild(this.labelTable.gameObject, this.chatLabelPrefab);
			this.labelChat.Add(gameObject.GetComponent<ChatLabel>());
		}
		for (int i = 0; i < this.labelChat.Count; i++)
		{
			string str = "[00FF26]";
			if ((Defs.isInet || !(playerMoveC.messages[i].IDLocal == WeaponManager.sharedManager.myPlayer.GetComponent<NetworkView>().viewID)) && (!Defs.isInet || playerMoveC.messages[i].ID != WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID))
			{
				if (playerMoveC.messages[i].command == 0)
				{
					str = "[FFFF26]";
				}
				if (playerMoveC.messages[i].command == 1)
				{
					str = "[0000FF]";
				}
				if (playerMoveC.messages[i].command == 2)
				{
					str = "[FF0000]";
				}
			}
			else
			{
				str = "[00FF26]";
			}
			ChatLabel item = this.labelChat[this.labelChat.Count - i - 1];
			UILabel uILabel = item.nickLabel;
			Player_move_c.MessageChat messageChat = playerMoveC.messages[i];
			uILabel.text = string.Concat(str, messageChat.text);
			if (!string.IsNullOrEmpty(playerMoveC.messages[i].iconName))
			{
				if (!item.stickerObject.activeInHierarchy)
				{
					item.stickerObject.SetActive(true);
				}
				item.iconSprite.spriteName = playerMoveC.messages[i].iconName;
			}
			else if (item.stickerObject.activeInHierarchy)
			{
				item.stickerObject.SetActive(false);
			}
			Transform vector3 = item.iconSprite.transform;
			float single = (float)(item.nickLabel.width + 20);
			float single1 = vector3.localPosition.y;
			Vector3 vector31 = vector3.localPosition;
			vector3.localPosition = new Vector3(single, single1, vector31.z);
			item.clanTexture.mainTexture = playerMoveC.messages[i].clanLogo;
			this.labelChat[i].gameObject.SetActive(true);
		}
		this.labelTable.Reposition();
	}
}