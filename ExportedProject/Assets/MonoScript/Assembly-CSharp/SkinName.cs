using System;
using System.Collections;
using Rilisoft;
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

	public void MoveCamera(Vector2 delta)
	{
		firstPersonControl.MoveCamera(delta);
	}

	public void BlockFirstPersonController()
	{
		firstPersonControl.enabled = false;
	}

	public void sendAnimJump()
	{
		int num = ((!character.isGrounded) ? 2 : 0);
		if (interpolateScript.myAnim != num)
		{
			if (Defs.isSoundFX && num == 2 && !EffectsController.WeAreStealth)
			{
				NGUITools.PlaySound(jumpAudio);
			}
			interpolateScript.myAnim = num;
			interpolateScript.weAreSteals = EffectsController.WeAreStealth;
			if (isMulti)
			{
				SetAnim(num, EffectsController.WeAreStealth);
			}
		}
	}

	[PunRPC]
	[RPC]
	public void SetAnim(int _typeAnim, bool stealth)
	{
		string animation = "Idle";
		currentAnim = _typeAnim;
		switch (_typeAnim)
		{
		case 0:
			animation = "Idle";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
			_playWalkSound = false;
			break;
		case 1:
			animation = "Walk";
			if (!stealth && Defs.isSoundFX)
			{
				_playWalkSound = true;
			}
			break;
		case 2:
			animation = "Jump";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
			break;
		}
		switch (_typeAnim)
		{
		case 4:
			animation = "Walk_Back";
			if (!stealth && Defs.isSoundFX)
			{
				_playWalkSound = true;
			}
			break;
		case 5:
			animation = "Walk_Left";
			if (!stealth && Defs.isSoundFX)
			{
				_playWalkSound = true;
			}
			break;
		case 6:
			animation = "Walk_Right";
			if (!stealth && Defs.isSoundFX)
			{
				_playWalkSound = true;
			}
			break;
		}
		if (_typeAnim == 7)
		{
			animation = "Jetpack_Run_Front";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (_typeAnim == 8)
		{
			animation = "Jetpack_Run_Back";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (_typeAnim == 9)
		{
			animation = "Jetpack_Run_Left";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (_typeAnim == 10)
		{
			animation = "Jetpack_Run_Righte";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (_typeAnim == 11)
		{
			animation = "Jetpack_Idle";
			if (Defs.isSoundFX)
			{
				_audio.Stop();
			}
		}
		if (isMulti && !isMine)
		{
			if (playerMoveC.isMechActive || playerMoveC.isBearActive)
			{
				playerMoveC.mechBodyAnimation.Play(animation);
			}
			FPSplayerObject.GetComponent<Animation>().Play(animation);
			if (capesPoint.transform.childCount > 0 && capesPoint.transform.GetChild(0).GetComponent<Animation>().GetClip(animation) != null)
			{
				capesPoint.transform.GetChild(0).GetComponent<Animation>().Play(animation);
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SetAnim(int _typeAnim)
	{
		SetAnim(_typeAnim, true);
	}

	[PunRPC]
	[RPC]
	private void setCapeCustomRPC(byte[] _skinByte)
	{
		if (capesPoint.transform.childCount > 0)
		{
			for (int i = 0; i < capesPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(capesPoint.transform.GetChild(i).gameObject);
			}
		}
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(_skinByte);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		if (texture2D.width == 12 && texture2D.height == 16)
		{
			UnityEngine.Object @object = Resources.Load("Capes/cape_Custom");
			if (!(@object == null))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(@object) as GameObject;
				Transform transform = gameObject.transform;
				gameObject.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
				transform.parent = capesPoint.transform;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				Player_move_c.SetTextureRecursivelyFrom(gameObject, texture2D, new GameObject[0]);
				currentCapeTex = texture2D;
				currentCape = "cape_Custom";
			}
		}
	}

	[RPC]
	[PunRPC]
	private void setCapeCustomRPC(string str)
	{
		if (capesPoint.transform.childCount > 0)
		{
			for (int i = 0; i < capesPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(capesPoint.transform.GetChild(i).gameObject);
			}
		}
		byte[] data = Convert.FromBase64String(str);
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		if (texture2D.width != 12 || texture2D.height != 16)
		{
			return;
		}
		if (!Device.isPixelGunLow)
		{
			UnityEngine.Object @object = Resources.Load("Capes/cape_Custom");
			if (@object == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(@object) as GameObject;
			Transform transform = gameObject.transform;
			gameObject.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
			transform.parent = capesPoint.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			Player_move_c.SetTextureRecursivelyFrom(gameObject, texture2D, new GameObject[0]);
		}
		currentCapeTex = texture2D;
		currentCape = "cape_Custom";
	}

	private void UpdateEffectsOnPlayerMoveC()
	{
		if (playerMoveC != null)
		{
			playerMoveC.UpdateEffectsForCurrentWeapon(currentCape, currentMask, currentHat);
		}
		else
		{
			Debug.LogError("playerMoveC.UpdateEffectsForCurrentWeapon playerMoveC == null");
		}
	}

	[RPC]
	[PunRPC]
	private void setCapeRPC(string _currentCape)
	{
		if (capesPoint.transform.childCount > 0)
		{
			for (int i = 0; i < capesPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(capesPoint.transform.GetChild(i).gameObject);
			}
		}
		currentCapeTex = null;
		currentCape = _currentCape;
		UpdateEffectsOnPlayerMoveC();
		StartCoroutine(SetCapeModel());
	}

	private IEnumerator SetCapeModel()
	{
		if (!Device.isPixelGunLow)
		{
			LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Capes/" + currentCape);
			while (!request.isDone)
			{
				yield return null;
			}
			GameObject _capPrefab = request.asset as GameObject;
			if (!(_capPrefab == null))
			{
				GameObject _cap = UnityEngine.Object.Instantiate(_capPrefab);
				Transform _capTransform = _cap.transform;
				_capTransform.parent = capesPoint.transform;
				_capTransform.localPosition = Vector3.zero;
				_capTransform.localRotation = Quaternion.identity;
			}
		}
	}

	[RPC]
	[PunRPC]
	private void SetArmorVisInvisibleRPC(string _currentArmor, bool _isInviseble)
	{
		if (armorPoint.transform.childCount > 0)
		{
			for (int i = 0; i < armorPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(armorPoint.transform.GetChild(i).gameObject);
			}
		}
		currentArmor = _currentArmor;
		StartCoroutine(SetArmorModel(_isInviseble));
	}

	private IEnumerator SetArmorModel(bool invisible)
	{
		GameObject _armPrefab = null;
		if (Device.isPixelGunLow && !string.IsNullOrEmpty(currentArmor) && currentArmor != Defs.ArmorNewNoneEqupped)
		{
			_armPrefab = Resources.Load<GameObject>("Armor_Low");
		}
		else
		{
			LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Armor/" + currentArmor);
			while (!request.isDone)
			{
				yield return null;
			}
			_armPrefab = request.asset as GameObject;
		}
		if (_armPrefab == null)
		{
			yield break;
		}
		GameObject _armor = UnityEngine.Object.Instantiate(_armPrefab);
		Transform _armorTranform = _armor.transform;
		if (Device.isPixelGunLow)
		{
			try
			{
				SkinnedMeshRenderer armorRendered = _armorTranform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>();
				armorRendered.material = Resources.Load<Material>("LowPolyArmorMaterials/" + currentArmor + "_low");
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Debug.LogError("Exception setting material for low armor: " + currentArmor + "   exception: " + e);
			}
		}
		if (invisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(_armorTranform, false);
		}
		ArmorRefs ar = _armorTranform.GetChild(0).GetComponent<ArmorRefs>();
		if (ar != null)
		{
			if (playerMoveC != null && playerMoveC.transform.childCount > 0)
			{
				WeaponSounds ws = playerMoveC.myCurrentWeaponSounds;
				ar.leftBone.GetComponent<SetPosInArmor>().target = ws.LeftArmorHand;
				ar.rightBone.GetComponent<SetPosInArmor>().target = ws.RightArmorHand;
			}
			_armorTranform.parent = armorPoint.transform;
			_armorTranform.localPosition = Vector3.zero;
			_armorTranform.localRotation = Quaternion.identity;
			_armorTranform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	[PunRPC]
	[RPC]
	private void setBootsRPC(string _currentBoots)
	{
		for (int i = 0; i < bootsPoint.transform.childCount; i++)
		{
			Transform child = bootsPoint.transform.GetChild(i);
			if (child.gameObject.name.Equals(_currentBoots) && !Device.isPixelGunLow)
			{
				child.gameObject.SetActive(true);
			}
			else
			{
				child.gameObject.SetActive(false);
			}
		}
		currentBoots = _currentBoots;
	}

	[PunRPC]
	[RPC]
	private void SetMaskRPC(string _currentMask)
	{
		if (maskPoint.transform.childCount > 0)
		{
			for (int i = 0; i < maskPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(maskPoint.transform.GetChild(i).gameObject);
			}
		}
		currentMask = _currentMask;
		StartCoroutine(SetMaskModel());
		UpdateEffectsOnPlayerMoveC();
	}

	private IEnumerator SetMaskModel()
	{
		if (!Device.isPixelGunLow)
		{
			LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Masks/" + currentMask);
			while (!request.isDone)
			{
				yield return null;
			}
			GameObject maskPrefab = request.asset as GameObject;
			if (maskPrefab != null)
			{
				GameObject maskInstance = UnityEngine.Object.Instantiate(maskPrefab);
				Transform maskTransform = maskInstance.transform;
				maskTransform.parent = maskPoint.transform;
				maskTransform.localPosition = Vector3.zero;
				maskTransform.localRotation = Quaternion.identity;
				maskTransform.localScale = Vector3.one;
			}
		}
	}

	[PunRPC]
	[RPC]
	private void SetHatWithInvisebleRPC(string _currentHat, bool _isHatInviseble)
	{
		if (hatsPoint.transform.childCount > 0)
		{
			for (int i = 0; i < hatsPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(hatsPoint.transform.GetChild(i).gameObject);
			}
		}
		currentHat = _currentHat;
		StartCoroutine(SetHatModel(_isHatInviseble));
	}

	private IEnumerator SetHatModel(bool invisible)
	{
		if (Device.isPixelGunLow)
		{
			yield break;
		}
		LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Hats/" + currentHat);
		while (!request.isDone)
		{
			yield return null;
		}
		GameObject _hatPrefab = request.asset as GameObject;
		if (!(_hatPrefab == null))
		{
			GameObject _hat = UnityEngine.Object.Instantiate(_hatPrefab);
			Transform _hatTransform = _hat.transform;
			if (invisible)
			{
				ShopNGUIController.SetRenderersVisibleFromPoint(_hatTransform, false);
			}
			_hatTransform.parent = hatsPoint.transform;
			_hatTransform.localPosition = Vector3.zero;
			_hatTransform.localRotation = Quaternion.identity;
			_hatTransform.localScale = Vector3.one;
		}
	}

	private void Awake()
	{
		isLocal = !Defs.isInet;
		firstPersonControl = GetComponent<FirstPersonControlSharp>();
		_audio = GetComponent<AudioSource>();
	}

	private void Start()
	{
		_weaponManager = WeaponManager.sharedManager;
		playerMoveC = playerGameObject.GetComponent<Player_move_c>();
		character = base.transform.GetComponent<CharacterController>();
		isMulti = Defs.isMulti;
		photonView = PhotonView.Get(this);
		if ((bool)photonView && photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		isInet = Defs.isInet;
		if (!isInet)
		{
			isMine = GetComponent<NetworkView>().isMine;
		}
		else
		{
			isMine = photonView.isMine;
		}
		if (((!Defs.isInet && !GetComponent<NetworkView>().isMine) || (Defs.isInet && !photonView.isMine)) && Defs.isMulti)
		{
			camPlayer.active = false;
			character.enabled = false;
		}
		else
		{
			FPSplayerObject.SetActive(false);
		}
		if (!Defs.isMulti || (!Defs.isInet && GetComponent<NetworkView>().isMine) || (Defs.isInet && photonView.isMine))
		{
			base.gameObject.layer = 11;
			bodyLayer.layer = 11;
			headObj.layer = 11;
		}
		if (isMine)
		{
			SetCape();
			SetHat();
			SetBoots();
			SetArmor();
			SetMask();
		}
	}

	private void OnDestroy()
	{
		if (_armorPopularityCacheIsDirty)
		{
			Statistics.Instance.SaveArmorPopularity();
			_armorPopularityCacheIsDirty = false;
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	public void SetMask(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string text = (currentMask = Storager.getString("MaskEquippedSN", false));
		UpdateEffectsOnPlayerMoveC();
		if (!Defs.isMulti)
		{
			return;
		}
		if (isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetMaskRPC", PhotonTargets.Others, text);
			}
			else
			{
				photonView.RPC("SetMaskRPC", player, text);
			}
		}
		else
		{
			GetComponent<NetworkView>().RPC("SetMaskRPC", RPCMode.Others, text);
		}
	}

	public void SetCape(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string text = (currentCape = Storager.getString(Defs.CapeEquppedSN, false));
		UpdateEffectsOnPlayerMoveC();
		if (!Defs.isMulti)
		{
			return;
		}
		if (!text.Equals("cape_Custom"))
		{
			if (isInet)
			{
				if (player == null)
				{
					photonView.RPC("setCapeRPC", PhotonTargets.Others, text);
				}
				else
				{
					photonView.RPC("setCapeRPC", player, text);
				}
			}
			else
			{
				GetComponent<NetworkView>().RPC("setCapeRPC", RPCMode.Others, text);
			}
		}
		else
		{
			if (!text.Equals("cape_Custom"))
			{
				return;
			}
			Texture2D capeUserTexture = SkinsController.capeUserTexture;
			byte[] array = capeUserTexture.EncodeToPNG();
			if (capeUserTexture.width != 12 || capeUserTexture.height != 16)
			{
				return;
			}
			if (isInet)
			{
				if (player == null)
				{
					photonView.RPC("setCapeCustomRPC", PhotonTargets.Others, array);
				}
				else
				{
					photonView.RPC("setCapeCustomRPC", player, array);
				}
			}
			else
			{
				string text2 = Convert.ToBase64String(array);
				GetComponent<NetworkView>().RPC("setCapeCustomRPC", RPCMode.Others, text2);
			}
		}
	}

	public void SetArmor(PhotonPlayer player = null)
	{
		if (Defs.isHunger || Defs.isDaterRegim)
		{
			return;
		}
		string text = (currentArmor = Storager.getString(Defs.ArmorNewEquppedSN, false));
		if (!Defs.isMulti)
		{
			return;
		}
		bool flag = !ShopNGUIController.ShowArmor;
		if (isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetArmorVisInvisibleRPC", PhotonTargets.Others, text, flag);
			}
			else
			{
				photonView.RPC("SetArmorVisInvisibleRPC", player, text, flag);
			}
		}
		else
		{
			GetComponent<NetworkView>().RPC("SetArmorVisInvisibleRPC", RPCMode.Others, text, flag);
		}
		IncrementArmorPopularity(text);
	}

	public void SetBoots(PhotonPlayer player = null)
	{
		string text = (currentBoots = Storager.getString(Defs.BootsEquppedSN, false));
		if (Defs.isHunger)
		{
			currentBoots = string.Empty;
		}
		if (!Defs.isMulti)
		{
			return;
		}
		if (isInet)
		{
			if (player == null)
			{
				photonView.RPC("setBootsRPC", PhotonTargets.Others, text);
			}
			else
			{
				photonView.RPC("setBootsRPC", player, text);
			}
		}
		else
		{
			GetComponent<NetworkView>().RPC("setBootsRPC", RPCMode.Others, text);
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if ((bool)photonView && photonView.isMine)
		{
			SetHat(player);
			SetCape(player);
			SetBoots(player);
			SetArmor(player);
			SetMask(player);
		}
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (GetComponent<NetworkView>().isMine)
		{
			SetHat();
			SetCape();
			SetBoots();
			SetArmor();
			SetMask();
		}
	}

	public void SetHat(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string text = Storager.getString(Defs.HatEquppedSN, false);
		if (text != null && (Defs.isHunger || Defs.isDaterRegim) && !Wear.NonArmorHat(text))
		{
			text = "hat_NoneEquipped";
		}
		currentHat = text;
		if (!Defs.isMulti)
		{
			return;
		}
		bool flag = !ShopNGUIController.ShowHat && !Wear.NonArmorHat(text);
		if (isInet)
		{
			if (player == null)
			{
				photonView.RPC("SetHatWithInvisebleRPC", PhotonTargets.Others, text, flag);
			}
			else
			{
				photonView.RPC("SetHatWithInvisebleRPC", player, text, flag);
			}
		}
		else
		{
			GetComponent<NetworkView>().RPC("SetHatWithInvisebleRPC", RPCMode.Others, text, flag);
		}
	}

	private void Update()
	{
		if ((isMulti && isMine) || !isMulti)
		{
			if (playerMoveC.isKilled)
			{
				isPlayDownSound = false;
			}
			int num = 0;
			if ((character.velocity.y > 0.01f || character.velocity.y < -0.01f) && !character.isGrounded && !Defs.isJetpackEnabled)
			{
				num = 2;
			}
			else if (character.velocity.x != 0f || character.velocity.z != 0f)
			{
				if (character.isGrounded)
				{
					float x = JoystickController.leftJoystick.value.x;
					float y = JoystickController.leftJoystick.value.y;
					num = ((Mathf.Abs(y) >= Mathf.Abs(x)) ? ((y >= 0f) ? 1 : 4) : ((!(x >= 0f)) ? 5 : 6));
				}
				else if (Defs.isJetpackEnabled)
				{
					float x2 = JoystickController.leftJoystick.value.x;
					float y2 = JoystickController.leftJoystick.value.y;
					num = ((Mathf.Abs(y2) >= Mathf.Abs(x2)) ? ((!(y2 >= 0f)) ? 8 : 7) : ((!(x2 >= 0f)) ? 9 : 10));
				}
			}
			else if (Defs.isJetpackEnabled && !character.isGrounded)
			{
				num = 11;
			}
			if (character.velocity.y < -2.5f && !character.isGrounded)
			{
				isPlayDownSound = true;
			}
			if (isPlayDownSound && character.isGrounded)
			{
				if (Defs.isSoundFX && !EffectsController.WeAreStealth)
				{
					NGUITools.PlaySound(jumpDownAudio);
				}
				isPlayDownSound = false;
			}
			if (num != typeAnim)
			{
				typeAnim = num;
				if (((isMulti && isMine) || !isMulti) && typeAnim != 2)
				{
					interpolateScript.myAnim = typeAnim;
					interpolateScript.weAreSteals = EffectsController.WeAreStealth;
					SetAnim(typeAnim, EffectsController.WeAreStealth);
				}
			}
		}
		if (_playWalkSound)
		{
			AudioClip audioClip = ((!playerMoveC.isMechActive && !playerMoveC.isBearActive) ? walkAudio : walkMech);
			if (!_audio.isPlaying || _audio.clip != audioClip)
			{
				_audio.loop = false;
				_audio.clip = audioClip;
				_audio.Play();
			}
		}
	}

	public IEnumerator _SetAndResetImpactedByTrampoline()
	{
		_impactedByTramp = true;
		yield return new WaitForSeconds(0.1f);
		_impactedByTramp = false;
	}

	private void OnControllerColliderHit(ControllerColliderHit col)
	{
		onRink = false;
		if ((!isMulti || isMine) && _irt != null && !_impactedByTramp)
		{
			UnityEngine.Object.Destroy(_irt);
			_irt = null;
		}
		if (col.gameObject.tag == "Conveyor" && (!isMulti || isMine))
		{
			if (!onConveyor)
			{
				conveyorDirection = Vector3.zero;
			}
			onConveyor = true;
			Conveyor component = col.transform.GetComponent<Conveyor>();
			if (component.accelerateSpeed)
			{
				conveyorDirection = Vector3.Lerp(conveyorDirection, col.transform.forward * component.maxspeed, component.acceleration);
			}
			else
			{
				conveyorDirection = col.transform.forward * component.maxspeed;
			}
			return;
		}
		onConveyor = false;
		if (col.gameObject.tag == "Rink" && (!isMulti || isMine))
		{
			onRink = true;
		}
		else if (!_impactedByTramp && (col.gameObject.tag == "Trampoline" || col.gameObject.tag == "ConveyorTrampoline") && (!isMulti || isMine))
		{
			if (_irt == null)
			{
				_irt = base.gameObject.AddComponent<ImpactReceiverTrampoline>();
			}
			if (col.gameObject.tag == "Trampoline")
			{
				_irt.AddImpact(col.transform.up, 45f);
			}
			else
			{
				_irt.AddImpact(col.transform.forward, conveyorDirection.magnitude * 1.4f);
				conveyorDirection = Vector3.zero;
			}
			if (Defs.isSoundFX)
			{
				AudioSource component2 = col.gameObject.GetComponent<AudioSource>();
				if (component2 != null)
				{
					component2.Play();
				}
			}
			StartCoroutine(_SetAndResetImpactedByTrampoline());
		}
		else if ((!isMulti || (isLocal && GetComponent<NetworkView>().isMine) || (isInet && (bool)photonView && photonView.isMine)) && col.gameObject.name.Equals("DeadCollider") && !playerMoveC.isKilled)
		{
			isPlayDownSound = false;
			playerMoveC.KillSelf();
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if ((!isMulti || isMine) && col.gameObject.name.Equals("DamageCollider"))
		{
			col.gameObject.GetComponent<DamageCollider>().RegisterPlayer();
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if ((!isMulti || isMine) && col.gameObject.GetComponent<DamageCollider>() != null)
		{
			col.gameObject.GetComponent<DamageCollider>().UnregisterPlayer();
		}
	}

	private void IncrementArmorPopularity(string currentArmor)
	{
		if (isInet && isMulti && isMine)
		{
			string key = "None";
			if (currentArmor != Defs.ArmorNewNoneEqupped)
			{
				key = ItemDb.GetItemNameNonLocalized(currentArmor, currentArmor, ShopNGUIController.CategoryNames.ArmorCategory, "Unknown");
			}
			Statistics.Instance.IncrementArmorPopularity(key);
			_armorPopularityCacheIsDirty = true;
		}
	}
}
