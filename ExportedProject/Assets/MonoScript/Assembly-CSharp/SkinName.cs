using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class SkinName : MonoBehaviour
{
	[NonSerialized]
	public string currentHat;

	[NonSerialized]
	public string currentArmor;

	[NonSerialized]
	public string currentCape;

	[NonSerialized]
	public Texture currentCapeTex;

	[NonSerialized]
	public string currentBoots;

	[NonSerialized]
	public string currentMask;

	public Transform onGroundEffectsPoint;

	public GameObject playerGameObject;

	public Player_move_c playerMoveC;

	public string skinName;

	public GameObject hatsPoint;

	public GameObject capesPoint;

	public GameObject bootsPoint;

	public GameObject armorPoint;

	public GameObject maskPoint;

	public string NickName;

	public GameObject camPlayer;

	public GameObject headObj;

	public GameObject bodyLayer;

	public CharacterController character;

	public PhotonView photonView;

	public int typeAnim;

	public WeaponManager _weaponManager;

	public bool isInet;

	public bool isLocal;

	public bool isMine;

	public bool isMulti;

	public AudioClip walkAudio;

	public AudioClip jumpAudio;

	public AudioClip jumpDownAudio;

	public AudioClip walkMech;

	public AudioClip walkMechBear;

	public bool isPlayDownSound;

	public GameObject FPSplayerObject;

	public ThirdPersonNetwork1 interpolateScript;

	private bool _impactedByTramp;

	public bool onRink;

	public bool onConveyor;

	public Vector3 conveyorDirection;

	private ImpactReceiverTrampoline _irt;

	private bool _armorPopularityCacheIsDirty;

	private FirstPersonControlSharp firstPersonControl;

	public int currentAnim;

	private bool _playWalkSound;

	private AudioSource _audio;

	public SkinName()
	{
	}

	[DebuggerHidden]
	public IEnumerator _SetAndResetImpactedByTrampoline()
	{
		SkinName.u003c_SetAndResetImpactedByTrampolineu003ec__Iterator1B2 variable = null;
		return variable;
	}

	private void Awake()
	{
		this.isLocal = !Defs.isInet;
		this.firstPersonControl = base.GetComponent<FirstPersonControlSharp>();
		this._audio = base.GetComponent<AudioSource>();
	}

	public void BlockFirstPersonController()
	{
		this.firstPersonControl.enabled = false;
	}

	private void IncrementArmorPopularity(string currentArmor)
	{
		if (this.isInet && this.isMulti && this.isMine)
		{
			string itemNameNonLocalized = "None";
			if (currentArmor != Defs.ArmorNewNoneEqupped)
			{
				itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(currentArmor, currentArmor, ShopNGUIController.CategoryNames.ArmorCategory, "Unknown");
			}
			Statistics.Instance.IncrementArmorPopularity(itemNameNonLocalized, true);
			this._armorPopularityCacheIsDirty = true;
		}
	}

	public void MoveCamera(Vector2 delta)
	{
		this.firstPersonControl.MoveCamera(delta);
	}

	private void OnControllerColliderHit(ControllerColliderHit col)
	{
		bool flag;
		this.onRink = false;
		if ((!this.isMulti || this.isMine) && this._irt != null && !this._impactedByTramp)
		{
			UnityEngine.Object.Destroy(this._irt);
			this._irt = null;
		}
		if (col.gameObject.tag == "Conveyor" && (!this.isMulti || this.isMine))
		{
			if (!this.onConveyor)
			{
				this.conveyorDirection = Vector3.zero;
			}
			this.onConveyor = true;
			Conveyor component = col.transform.GetComponent<Conveyor>();
			if (!component.accelerateSpeed)
			{
				this.conveyorDirection = col.transform.forward * component.maxspeed;
			}
			else
			{
				this.conveyorDirection = Vector3.Lerp(this.conveyorDirection, col.transform.forward * component.maxspeed, component.acceleration);
			}
			return;
		}
		this.onConveyor = false;
		if (col.gameObject.tag == "Rink" && (!this.isMulti || this.isMine))
		{
			this.onRink = true;
			return;
		}
		if (this._impactedByTramp || !(col.gameObject.tag == "Trampoline") && !(col.gameObject.tag == "ConveyorTrampoline") || this.isMulti && !this.isMine)
		{
			if (!this.isMulti || this.isLocal && base.GetComponent<NetworkView>().isMine)
			{
				flag = true;
			}
			else
			{
				flag = (!this.isInet || !this.photonView ? false : this.photonView.isMine);
			}
			if (flag && col.gameObject.name.Equals("DeadCollider") && !this.playerMoveC.isKilled)
			{
				this.isPlayDownSound = false;
				this.playerMoveC.KillSelf();
			}
			return;
		}
		if (this._irt == null)
		{
			this._irt = base.gameObject.AddComponent<ImpactReceiverTrampoline>();
		}
		if (col.gameObject.tag != "Trampoline")
		{
			this._irt.AddImpact(col.transform.forward, this.conveyorDirection.magnitude * 1.4f);
			this.conveyorDirection = Vector3.zero;
		}
		else
		{
			this._irt.AddImpact(col.transform.up, 45f);
		}
		if (Defs.isSoundFX)
		{
			AudioSource audioSource = col.gameObject.GetComponent<AudioSource>();
			if (audioSource != null)
			{
				audioSource.Play();
			}
		}
		base.StartCoroutine(this._SetAndResetImpactedByTrampoline());
	}

	private void OnDestroy()
	{
		if (this._armorPopularityCacheIsDirty)
		{
			Statistics.Instance.SaveArmorPopularity();
			this._armorPopularityCacheIsDirty = false;
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.photonView && this.photonView.isMine)
		{
			this.SetHat(player);
			this.SetCape(player);
			this.SetBoots(player);
			this.SetArmor(player);
			this.SetMask(player);
		}
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (base.GetComponent<NetworkView>().isMine)
		{
			this.SetHat(null);
			this.SetCape(null);
			this.SetBoots(null);
			this.SetArmor(null);
			this.SetMask(null);
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if ((!this.isMulti || this.isMine) && col.gameObject.name.Equals("DamageCollider"))
		{
			col.gameObject.GetComponent<DamageCollider>().RegisterPlayer();
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if ((!this.isMulti || this.isMine) && col.gameObject.GetComponent<DamageCollider>() != null)
		{
			col.gameObject.GetComponent<DamageCollider>().UnregisterPlayer();
		}
	}

	public void sendAnimJump()
	{
		int num = (!this.character.isGrounded ? 2 : 0);
		if (this.interpolateScript.myAnim != num)
		{
			if (Defs.isSoundFX && num == 2 && !EffectsController.WeAreStealth)
			{
				NGUITools.PlaySound(this.jumpAudio);
			}
			this.interpolateScript.myAnim = num;
			this.interpolateScript.weAreSteals = EffectsController.WeAreStealth;
			if (this.isMulti)
			{
				this.SetAnim(num, EffectsController.WeAreStealth);
			}
		}
	}

	[PunRPC]
	[RPC]
	public void SetAnim(int _typeAnim, bool stealth)
	{
		string str = "Idle";
		this.currentAnim = _typeAnim;
		if (_typeAnim == 0)
		{
			str = "Idle";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
			this._playWalkSound = false;
		}
		else if (_typeAnim == 1)
		{
			str = "Walk";
			if (!stealth && Defs.isSoundFX)
			{
				this._playWalkSound = true;
			}
		}
		else if (_typeAnim == 2)
		{
			str = "Jump";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 4)
		{
			str = "Walk_Back";
			if (!stealth && Defs.isSoundFX)
			{
				this._playWalkSound = true;
			}
		}
		else if (_typeAnim == 5)
		{
			str = "Walk_Left";
			if (!stealth && Defs.isSoundFX)
			{
				this._playWalkSound = true;
			}
		}
		else if (_typeAnim == 6)
		{
			str = "Walk_Right";
			if (!stealth && Defs.isSoundFX)
			{
				this._playWalkSound = true;
			}
		}
		if (_typeAnim == 7)
		{
			str = "Jetpack_Run_Front";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 8)
		{
			str = "Jetpack_Run_Back";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 9)
		{
			str = "Jetpack_Run_Left";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 10)
		{
			str = "Jetpack_Run_Righte";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 11)
		{
			str = "Jetpack_Idle";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (this.isMulti && !this.isMine)
		{
			if (this.playerMoveC.isMechActive || this.playerMoveC.isBearActive)
			{
				this.playerMoveC.mechBodyAnimation.Play(str);
			}
			this.FPSplayerObject.GetComponent<Animation>().Play(str);
			if (this.capesPoint.transform.childCount > 0 && this.capesPoint.transform.GetChild(0).GetComponent<Animation>().GetClip(str) != null)
			{
				this.capesPoint.transform.GetChild(0).GetComponent<Animation>().Play(str);
			}
		}
	}

	[PunRPC]
	[RPC]
	private void SetAnim(int _typeAnim)
	{
		this.SetAnim(_typeAnim, true);
	}

	public void SetArmor(PhotonPlayer player = null)
	{
		if (Defs.isHunger || Defs.isDaterRegim)
		{
			return;
		}
		string str = Storager.getString(Defs.ArmorNewEquppedSN, false);
		this.currentArmor = str;
		if (!Defs.isMulti)
		{
			return;
		}
		bool showArmor = !ShopNGUIController.ShowArmor;
		if (!this.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SetArmorVisInvisibleRPC", RPCMode.Others, new object[] { str, showArmor });
		}
		else if (player != null)
		{
			this.photonView.RPC("SetArmorVisInvisibleRPC", player, new object[] { str, showArmor });
		}
		else
		{
			this.photonView.RPC("SetArmorVisInvisibleRPC", PhotonTargets.Others, new object[] { str, showArmor });
		}
		this.IncrementArmorPopularity(str);
	}

	[DebuggerHidden]
	private IEnumerator SetArmorModel(bool invisible)
	{
		SkinName.u003cSetArmorModelu003ec__Iterator1AF variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	private void SetArmorVisInvisibleRPC(string _currentArmor, bool _isInviseble)
	{
		if (this.armorPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.armorPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.armorPoint.transform.GetChild(i).gameObject);
			}
		}
		this.currentArmor = _currentArmor;
		base.StartCoroutine(this.SetArmorModel(_isInviseble));
	}

	public void SetBoots(PhotonPlayer player = null)
	{
		string str = Storager.getString(Defs.BootsEquppedSN, false);
		this.currentBoots = str;
		if (Defs.isHunger)
		{
			this.currentBoots = string.Empty;
		}
		if (!Defs.isMulti)
		{
			return;
		}
		if (!this.isInet)
		{
			base.GetComponent<NetworkView>().RPC("setBootsRPC", RPCMode.Others, new object[] { str });
		}
		else if (player != null)
		{
			this.photonView.RPC("setBootsRPC", player, new object[] { str });
		}
		else
		{
			this.photonView.RPC("setBootsRPC", PhotonTargets.Others, new object[] { str });
		}
	}

	[PunRPC]
	[RPC]
	private void setBootsRPC(string _currentBoots)
	{
		for (int i = 0; i < this.bootsPoint.transform.childCount; i++)
		{
			Transform child = this.bootsPoint.transform.GetChild(i);
			if (!child.gameObject.name.Equals(_currentBoots) || Device.isPixelGunLow)
			{
				child.gameObject.SetActive(false);
			}
			else
			{
				child.gameObject.SetActive(true);
			}
		}
		this.currentBoots = _currentBoots;
	}

	public void SetCape(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string str = Storager.getString(Defs.CapeEquppedSN, false);
		this.currentCape = str;
		this.UpdateEffectsOnPlayerMoveC();
		if (!Defs.isMulti)
		{
			return;
		}
		if (!str.Equals("cape_Custom"))
		{
			if (!this.isInet)
			{
				base.GetComponent<NetworkView>().RPC("setCapeRPC", RPCMode.Others, new object[] { str });
			}
			else if (player != null)
			{
				this.photonView.RPC("setCapeRPC", player, new object[] { str });
			}
			else
			{
				this.photonView.RPC("setCapeRPC", PhotonTargets.Others, new object[] { str });
			}
			return;
		}
		if (str.Equals("cape_Custom"))
		{
			Texture2D texture2D = SkinsController.capeUserTexture;
			byte[] pNG = texture2D.EncodeToPNG();
			if (texture2D.width != 12 || texture2D.height != 16)
			{
				return;
			}
			if (!this.isInet)
			{
				string base64String = Convert.ToBase64String(pNG);
				base.GetComponent<NetworkView>().RPC("setCapeCustomRPC", RPCMode.Others, new object[] { base64String });
			}
			else if (player != null)
			{
				this.photonView.RPC("setCapeCustomRPC", player, new object[] { pNG });
			}
			else
			{
				this.photonView.RPC("setCapeCustomRPC", PhotonTargets.Others, new object[] { pNG });
			}
		}
	}

	[PunRPC]
	[RPC]
	private void setCapeCustomRPC(byte[] _skinByte)
	{
		if (this.capesPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.capesPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.capesPoint.transform.GetChild(i).gameObject);
			}
		}
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(_skinByte);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		if (texture2D.width != 12 || texture2D.height != 16)
		{
			return;
		}
		UnityEngine.Object obj = Resources.Load("Capes/cape_Custom");
		if (obj == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
		Transform transforms = gameObject.transform;
		gameObject.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
		transforms.parent = this.capesPoint.transform;
		transforms.localPosition = Vector3.zero;
		transforms.localRotation = Quaternion.identity;
		Player_move_c.SetTextureRecursivelyFrom(gameObject, texture2D, new GameObject[0]);
		this.currentCapeTex = texture2D;
		this.currentCape = "cape_Custom";
	}

	[PunRPC]
	[RPC]
	private void setCapeCustomRPC(string str)
	{
		if (this.capesPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.capesPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.capesPoint.transform.GetChild(i).gameObject);
			}
		}
		byte[] numArray = Convert.FromBase64String(str);
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(numArray);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		if (texture2D.width != 12 || texture2D.height != 16)
		{
			return;
		}
		if (!Device.isPixelGunLow)
		{
			UnityEngine.Object obj = Resources.Load("Capes/cape_Custom");
			if (obj == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
			Transform transforms = gameObject.transform;
			gameObject.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
			transforms.parent = this.capesPoint.transform;
			transforms.localPosition = Vector3.zero;
			transforms.localRotation = Quaternion.identity;
			Player_move_c.SetTextureRecursivelyFrom(gameObject, texture2D, new GameObject[0]);
		}
		this.currentCapeTex = texture2D;
		this.currentCape = "cape_Custom";
	}

	[DebuggerHidden]
	private IEnumerator SetCapeModel()
	{
		SkinName.u003cSetCapeModelu003ec__Iterator1AE variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	private void setCapeRPC(string _currentCape)
	{
		if (this.capesPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.capesPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.capesPoint.transform.GetChild(i).gameObject);
			}
		}
		this.currentCapeTex = null;
		this.currentCape = _currentCape;
		this.UpdateEffectsOnPlayerMoveC();
		base.StartCoroutine(this.SetCapeModel());
	}

	public void SetHat(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string str = Storager.getString(Defs.HatEquppedSN, false);
		if (str != null && (Defs.isHunger || Defs.isDaterRegim) && !Wear.NonArmorHat(str))
		{
			str = "hat_NoneEquipped";
		}
		this.currentHat = str;
		if (!Defs.isMulti)
		{
			return;
		}
		bool flag = (ShopNGUIController.ShowHat ? false : !Wear.NonArmorHat(str));
		if (!this.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SetHatWithInvisebleRPC", RPCMode.Others, new object[] { str, flag });
		}
		else if (player != null)
		{
			this.photonView.RPC("SetHatWithInvisebleRPC", player, new object[] { str, flag });
		}
		else
		{
			this.photonView.RPC("SetHatWithInvisebleRPC", PhotonTargets.Others, new object[] { str, flag });
		}
	}

	[DebuggerHidden]
	private IEnumerator SetHatModel(bool invisible)
	{
		SkinName.u003cSetHatModelu003ec__Iterator1B1 variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	private void SetHatWithInvisebleRPC(string _currentHat, bool _isHatInviseble)
	{
		if (this.hatsPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.hatsPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.hatsPoint.transform.GetChild(i).gameObject);
			}
		}
		this.currentHat = _currentHat;
		base.StartCoroutine(this.SetHatModel(_isHatInviseble));
	}

	public void SetMask(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string str = Storager.getString("MaskEquippedSN", false);
		this.currentMask = str;
		this.UpdateEffectsOnPlayerMoveC();
		if (!Defs.isMulti)
		{
			return;
		}
		if (!this.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SetMaskRPC", RPCMode.Others, new object[] { str });
		}
		else if (player != null)
		{
			this.photonView.RPC("SetMaskRPC", player, new object[] { str });
		}
		else
		{
			this.photonView.RPC("SetMaskRPC", PhotonTargets.Others, new object[] { str });
		}
	}

	[DebuggerHidden]
	private IEnumerator SetMaskModel()
	{
		SkinName.u003cSetMaskModelu003ec__Iterator1B0 variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	private void SetMaskRPC(string _currentMask)
	{
		if (this.maskPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.maskPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.maskPoint.transform.GetChild(i).gameObject);
			}
		}
		this.currentMask = _currentMask;
		base.StartCoroutine(this.SetMaskModel());
		this.UpdateEffectsOnPlayerMoveC();
	}

	private void Start()
	{
		this._weaponManager = WeaponManager.sharedManager;
		this.playerMoveC = this.playerGameObject.GetComponent<Player_move_c>();
		this.character = base.transform.GetComponent<CharacterController>();
		this.isMulti = Defs.isMulti;
		this.photonView = PhotonView.Get(this);
		if (this.photonView && this.photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		this.isInet = Defs.isInet;
		if (this.isInet)
		{
			this.isMine = this.photonView.isMine;
		}
		else
		{
			this.isMine = base.GetComponent<NetworkView>().isMine;
		}
		if ((Defs.isInet || base.GetComponent<NetworkView>().isMine) && (!Defs.isInet || this.photonView.isMine) || !Defs.isMulti)
		{
			this.FPSplayerObject.SetActive(false);
		}
		else
		{
			this.camPlayer.active = false;
			this.character.enabled = false;
		}
		if (!Defs.isMulti || !Defs.isInet && base.GetComponent<NetworkView>().isMine || Defs.isInet && this.photonView.isMine)
		{
			base.gameObject.layer = 11;
			this.bodyLayer.layer = 11;
			this.headObj.layer = 11;
		}
		if (this.isMine)
		{
			this.SetCape(null);
			this.SetHat(null);
			this.SetBoots(null);
			this.SetArmor(null);
			this.SetMask(null);
		}
	}

	private void Update()
	{
		if (this.isMulti && this.isMine || !this.isMulti)
		{
			if (this.playerMoveC.isKilled)
			{
				this.isPlayDownSound = false;
			}
			int num = 0;
			if ((this.character.velocity.y > 0.01f || this.character.velocity.y < -0.01f) && !this.character.isGrounded && !Defs.isJetpackEnabled)
			{
				num = 2;
			}
			else if (this.character.velocity.x == 0f && this.character.velocity.z == 0f)
			{
				if (Defs.isJetpackEnabled && !this.character.isGrounded)
				{
					num = 11;
				}
			}
			else if (this.character.isGrounded)
			{
				float single = JoystickController.leftJoystick.@value.x;
				float single1 = JoystickController.leftJoystick.@value.y;
				if (Mathf.Abs(single1) < Mathf.Abs(single))
				{
					num = (single < 0f ? 5 : 6);
				}
				else
				{
					num = (single1 < 0f ? 4 : 1);
				}
			}
			else if (Defs.isJetpackEnabled)
			{
				float single2 = JoystickController.leftJoystick.@value.x;
				float single3 = JoystickController.leftJoystick.@value.y;
				if (Mathf.Abs(single3) < Mathf.Abs(single2))
				{
					num = (single2 < 0f ? 9 : 10);
				}
				else
				{
					num = (single3 < 0f ? 8 : 7);
				}
			}
			if (this.character.velocity.y < -2.5f && !this.character.isGrounded)
			{
				this.isPlayDownSound = true;
			}
			if (this.isPlayDownSound && this.character.isGrounded)
			{
				if (Defs.isSoundFX && !EffectsController.WeAreStealth)
				{
					NGUITools.PlaySound(this.jumpDownAudio);
				}
				this.isPlayDownSound = false;
			}
			if (num != this.typeAnim)
			{
				this.typeAnim = num;
				if ((this.isMulti && this.isMine || !this.isMulti) && this.typeAnim != 2)
				{
					this.interpolateScript.myAnim = this.typeAnim;
					this.interpolateScript.weAreSteals = EffectsController.WeAreStealth;
					this.SetAnim(this.typeAnim, EffectsController.WeAreStealth);
				}
			}
		}
		if (this._playWalkSound)
		{
			AudioClip audioClip = (this.playerMoveC.isMechActive || this.playerMoveC.isBearActive ? this.walkMech : this.walkAudio);
			if (!this._audio.isPlaying || this._audio.clip != audioClip)
			{
				this._audio.loop = false;
				this._audio.clip = audioClip;
				this._audio.Play();
			}
		}
	}

	private void UpdateEffectsOnPlayerMoveC()
	{
		if (this.playerMoveC == null)
		{
			UnityEngine.Debug.LogError("playerMoveC.UpdateEffectsForCurrentWeapon playerMoveC == null");
		}
		else
		{
			this.playerMoveC.UpdateEffectsForCurrentWeapon(this.currentCape, this.currentMask, this.currentHat);
		}
	}
}