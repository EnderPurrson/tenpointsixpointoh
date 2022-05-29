using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TouchPadController : MonoBehaviour
{
	public const string GRENADE_BUY_NORMAL_SPRITE_NAME = "grenade_btn";

	private const string GRENADE_BUY_PRESSED_SPRITE_NAME = "grenade_btn_n";

	public const string LIKE_BUY_NORMAL_SPRITE_NAME = "grenade_like_btn";

	private const string LIKE_BUY_PRESSED_SPRITE_NAME = "grenade_like_btn_n";

	public bool grenadePressed;

	public GrenadeButton grenadeButton;

	public bool jumpPressed;

	public Transform fireSprite;

	public Transform jumpSprite;

	public Transform reloadSpirte;

	public Transform zoomSprite;

	public bool hasAmmo = true;

	public bool _isFirstFrame = true;

	public GameObject jetPackIcon;

	public GameObject jumpIcon;

	private Rect grenadeRect = new Rect();

	private bool isInvokeGrenadePress;

	private UISprite reloadUISprite;

	public bool isShooting;

	public bool isShootingPressure;

	private bool isHunger;

	private Player_move_c move;

	private HungerGameController hungerGameController;

	private Rect fireRect = new Rect();

	private Rect jumpRect = new Rect();

	private Rect reloadRect;

	private Rect zoomRect;

	private Rect moveRect;

	private bool _joyActive = true;

	private bool _isBuyGrenadePressed;

	private CameraTouchControlScheme _touchControlScheme;

	private bool _shouldRecalcRects;

	private int framesCount;

	private int MoveTouchID = -1;

	private Vector2 pastPos = Vector2.zero;

	private Vector2 pastDelta = Vector2.zero;

	private Vector2 compDeltas = Vector2.zero;

	public CameraTouchControlScheme touchControlScheme
	{
		get
		{
			return this._touchControlScheme;
		}
		set
		{
			this._touchControlScheme = value;
			this._touchControlScheme.Reset();
		}
	}

	public TouchPadController()
	{
	}

	[DebuggerHidden]
	private IEnumerator _SetIsFirstFrame()
	{
		TouchPadController.u003c_SetIsFirstFrameu003ec__Iterator1C2 variable = null;
		return variable;
	}

	public void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		if (this._touchControlScheme != null)
		{
			this._touchControlScheme.ApplyDeltaTo(deltaPosition, yawTransform, pitchTransform, sensitivity, invert);
		}
	}

	private void Awake()
	{
		this.reloadUISprite = this.reloadSpirte.GetComponent<UISprite>();
		this.isHunger = Defs.isHunger;
		if (!Device.isPixelGunLow)
		{
			this._touchControlScheme = new CameraTouchControlScheme_CleanNGUI();
		}
		else if (!Defs.isTouchControlSmoothDump)
		{
			this._touchControlScheme = new CameraTouchControlScheme_CleanNGUI();
		}
		else
		{
			this._touchControlScheme = new CameraTouchControlScheme_SmoothDump();
		}
	}

	[DebuggerHidden]
	[Obfuscation(Exclude=true)]
	private IEnumerator BlinkReload()
	{
		TouchPadController.u003cBlinkReloadu003ec__Iterator1C3 variable = null;
		return variable;
	}

	[Obfuscation(Exclude=true)]
	private void BuyGrenadePressInvoke()
	{
		this._isBuyGrenadePressed = true;
		this.grenadeButton.grenadeSprite.spriteName = "grenade_btn_n";
	}

	private void CalcRects()
	{
		Transform root = NGUITools.GetRoot(base.gameObject).transform;
		Camera component = root.GetChild(0).GetChild(0).GetComponent<Camera>();
		Transform transforms = component.transform;
		float single = 768f;
		float single1 = single * ((float)Screen.width / (float)Screen.height);
		List<object> objs = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]")) as List<object>;
		if (objs == null)
		{
			objs = new List<object>();
			UnityEngine.Debug.LogWarning(objs.GetType().FullName);
		}
		int[] array = objs.Select<object, int>(new Func<object, int>(Convert.ToInt32)).ToArray<int>();
		Bounds vector3 = NGUIMath.CalculateRelativeWidgetBounds(transforms, this.fireSprite, true, true);
		float single2 = 62f;
		if ((int)array.Length > 3)
		{
			single2 = (float)array[3] * 0.5f;
		}
		vector3.center = vector3.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		Vector3 vector31 = vector3.center;
		float coef = (vector31.x - single2) * Defs.Coef;
		Vector3 vector32 = vector3.center;
		this.fireRect = new Rect(coef, (vector32.y - single2) * Defs.Coef, 2f * single2 * Defs.Coef, 2f * single2 * Defs.Coef);
		Bounds bound = NGUIMath.CalculateRelativeWidgetBounds(transforms, this.jumpSprite, true, true);
		bound.center = bound.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		float single3 = 62f;
		if ((int)array.Length > 2)
		{
			single3 = (float)array[2] * 0.5f;
		}
		Vector3 vector33 = bound.center;
		float coef1 = (vector33.x - single3 * 0.7f) * Defs.Coef;
		Vector3 vector34 = bound.center;
		this.jumpRect = new Rect(coef1, (vector34.y - single3) * Defs.Coef, 2f * single3 * Defs.Coef, 2f * single3 * Defs.Coef);
		Bounds bound1 = NGUIMath.CalculateRelativeWidgetBounds(transforms, this.reloadSpirte, true, true);
		float single4 = 55f;
		if ((int)array.Length > 1)
		{
			single4 = (float)array[1] * 0.5f;
		}
		bound1.center = bound1.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		Vector3 vector35 = bound1.center;
		float coef2 = (vector35.x - single4) * Defs.Coef;
		Vector3 vector36 = bound1.center;
		this.reloadRect = new Rect(coef2, (vector36.y - single4) * Defs.Coef, 2f * single4 * Defs.Coef, 2f * single4 * Defs.Coef);
		float single5 = 55f;
		if ((int)array.Length > 0)
		{
			single5 = (float)array[0] * 0.5f;
		}
		Bounds bound2 = NGUIMath.CalculateRelativeWidgetBounds(transforms, this.zoomSprite, true, true);
		bound2.center = bound2.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		Vector3 vector37 = bound2.center;
		float coef3 = (vector37.x - single5) * Defs.Coef;
		Vector3 vector38 = bound2.center;
		this.zoomRect = new Rect(coef3, (vector38.y - single5) * Defs.Coef, 2f * single5 * Defs.Coef, 2f * single5 * Defs.Coef);
		float single6 = 55f;
		if ((int)array.Length > 5)
		{
			single6 = (float)array[5] * 0.5f;
		}
		Bounds bound3 = NGUIMath.CalculateRelativeWidgetBounds(transforms, this.grenadeButton.grenadeSprite.transform, true, true);
		bound3.center = bound3.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		Vector3 vector39 = bound3.center;
		float coef4 = (vector39.x - single6) * Defs.Coef;
		Vector3 vector310 = bound3.center;
		this.grenadeRect = new Rect(coef4, (vector310.y - single6) * Defs.Coef, 2f * single6 * Defs.Coef, 2f * single6 * Defs.Coef);
		float single7 = (float)Screen.height * 0.81f;
		if (GlobalGameController.LeftHanded)
		{
			this.moveRect = new Rect((float)Screen.width - single7, 0f, single7, (float)Screen.height * 0.65f);
		}
		else
		{
			this.moveRect = new Rect(0f, 0f, single7, (float)Screen.height * 0.65f);
		}
	}

	public static int GetGrenadeCount()
	{
		if (!(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.myPlayerMoveC != null))
		{
			return 0;
		}
		return WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount;
	}

	public Vector2 GrabDeltaPosition()
	{
		Vector2 deltaPosition = Vector2.zero;
		if (this._touchControlScheme != null)
		{
			deltaPosition = this._touchControlScheme.DeltaPosition;
			this._touchControlScheme.ResetDelta();
		}
		return deltaPosition;
	}

	[Obfuscation(Exclude=true)]
	private void GrenadePressInvoke()
	{
		this.grenadePressed = true;
		this.grenadeButton.grenadeSprite.spriteName = "grenade_btn_n";
		this.move.GrenadePress();
	}

	public void HasAmmo()
	{
		BlinkReloadButton.isBlink = false;
	}

	private static bool IsButtonGrenadeVisible()
	{
		return ((!(InGameGUI.sharedInGameGUI.playerMoveC != null) || InGameGUI.sharedInGameGUI.playerMoveC.isMechActive || Defs.isTurretWeapon) && !(InGameGUI.sharedInGameGUI.playerMoveC == null) ? false : !Defs.isZooming);
	}

	public static bool IsBuyGrenadeActive()
	{
		if (TouchPadController.IsButtonGrenadeVisible() && Defs.isDaterRegim && WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount <= 0)
		{
			return true;
		}
		return false;
	}

	private bool IsDifferendDirections(Vector2 delta1, Vector2 delta2)
	{
		return (JoystickController.leftJoystick.@value == Vector2.zero ? false : Mathf.Sign(delta1.x) != Mathf.Sign(delta2.x));
	}

	private bool IsUseGrenadeActive()
	{
		return (!TouchPadController.IsButtonGrenadeVisible() || !Defs.isGrenateFireEnable || this.isHunger && !this.hungerGameController.isGo || TouchPadController.GetGrenadeCount() <= 0 || WeaponManager.sharedManager._currentFilterMap == 1 ? false : WeaponManager.sharedManager._currentFilterMap != 2);
	}

	public void MakeActive()
	{
		this._joyActive = true;
	}

	public void MakeInactive()
	{
		this.jumpPressed = false;
		this.isShooting = false;
		this.isShootingPressure = false;
		this.Reset();
		this.HasAmmo();
		this._joyActive = false;
	}

	public void NoAmmo()
	{
		BlinkReloadButton.isBlink = true;
	}

	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= new Action(this.SetSideAndCalcRects);
		ControlsSettingsBase.ControlsChanged -= new Action(this.SetShouldRecalcRects);
	}

	private void OnDragTouch(Vector2 delta)
	{
		if (this._joyActive)
		{
			this.framesCount = 0;
			this._touchControlScheme.OnDrag(delta);
			return;
		}
		this.jumpPressed = false;
		this.isShooting = false;
		this._touchControlScheme.ResetDelta();
	}

	private void OnEnable()
	{
		this.isShooting = false;
		this.isShootingPressure = false;
		if (this._shouldRecalcRects)
		{
			base.Invoke("ReCalcRects", 0.1f);
		}
		this._shouldRecalcRects = false;
		base.StartCoroutine(this._SetIsFirstFrame());
	}

	private void OnPress(bool isDown)
	{
		this._touchControlScheme.OnPress(isDown);
		if (!this.move)
		{
			if (Defs.isMulti)
			{
				this.move = WeaponManager.sharedManager.myPlayerMoveC;
			}
			else
			{
				this.move = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
			}
		}
		if (this.fireRect.width.Equals(0f))
		{
			this.CalcRects();
		}
		if (!this._joyActive)
		{
			return;
		}
		if (this._isFirstFrame)
		{
			return;
		}
		if (isDown && this.fireRect.Contains(UICamera.lastTouchPosition) && (Defs.isJumpAndShootButtonOn || !Defs.isUse3DTouch))
		{
			this.isShooting = true;
		}
		if (isDown && this.grenadeRect.Contains(UICamera.lastTouchPosition))
		{
			if (TouchPadController.IsBuyGrenadeActive())
			{
				this.BuyGrenadePressInvoke();
			}
			else if (this.IsUseGrenadeActive() && (!(InGameGUI.sharedInGameGUI != null) || !(InGameGUI.sharedInGameGUI.changeWeaponScroll != null) || !InGameGUI.sharedInGameGUI.changeWeaponScroll.isDragging))
			{
				this.isInvokeGrenadePress = true;
				this.GrenadePressInvoke();
			}
		}
		if (isDown && this.jumpRect.Contains(UICamera.lastTouchPosition) && (Defs.isJumpAndShootButtonOn || !Defs.isUse3DTouch) && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None || TrainingController.stepTraining >= TrainingState.GetTheGun))
		{
			this.jumpPressed = true;
		}
		if (isDown && (InGameGUI.sharedInGameGUI.playerMoveC != null && !InGameGUI.sharedInGameGUI.playerMoveC.isMechActive || InGameGUI.sharedInGameGUI.playerMoveC == null) && this.reloadRect.Contains(UICamera.lastTouchPosition) && this.move && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None || TrainingController.FireButtonEnabled))
		{
			this.move.ReloadPressed();
		}
		bool flag = (this.zoomSprite == null ? false : this.zoomSprite.gameObject.activeInHierarchy);
		if (isDown && (InGameGUI.sharedInGameGUI.playerMoveC != null && !InGameGUI.sharedInGameGUI.playerMoveC.isMechActive || InGameGUI.sharedInGameGUI.playerMoveC == null) && flag && this.zoomRect.Contains(UICamera.lastTouchPosition) && this.move && WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null && WeaponManager.sharedManager.currentWeaponSounds.isZooming)
		{
			this.move.ZoomPress();
		}
		if (!isDown)
		{
			if (this.isInvokeGrenadePress)
			{
				this.isInvokeGrenadePress = false;
				base.CancelInvoke("GrenadePressInvoke");
			}
			if (this._isBuyGrenadePressed)
			{
				this._isBuyGrenadePressed = false;
				this.grenadeButton.grenadeSprite.spriteName = (!Defs.isDaterRegim ? "grenade_btn" : "grenade_like_btn");
				if (this.grenadeRect.Contains(UICamera.lastTouchPosition))
				{
					InGameGUI.sharedInGameGUI.HandleBuyGrenadeClicked(null, EventArgs.Empty);
				}
			}
			this.isShooting = false;
			this.jumpPressed = false;
			if (this.grenadePressed)
			{
				this.grenadePressed = false;
				this.grenadeButton.grenadeSprite.spriteName = (!Defs.isDaterRegim ? "grenade_btn" : "grenade_like_btn");
				this.move.GrenadeFire();
			}
		}
	}

	private void OnPressure(float pressure)
	{
		if (Defs.touchPressureSupported && Defs.isUse3DTouch && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			if (((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None || TrainingController.FireButtonEnabled) && (!this.isHunger || this.hungerGameController.isGo) && this.hasAmmo ? WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon : true) || pressure <= 0.8f)
			{
				this.isShootingPressure = false;
			}
			else
			{
				this.isShootingPressure = true;
			}
		}
	}

	[Obfuscation(Exclude=true)]
	private void ReCalcRects()
	{
		this.CalcRects();
	}

	public void Reset()
	{
		this._touchControlScheme.Reset();
	}

	private void SetActiveChecked(GameObject obj, bool active)
	{
		if (active && !obj.activeSelf)
		{
			obj.SetActive(true);
			return;
		}
		if (active || !obj.activeSelf)
		{
			return;
		}
		obj.SetActive(false);
	}

	private void SetGrenadeUISpriteState()
	{
		string str;
		bool flag = TouchPadController.IsBuyGrenadeActive();
		bool flag1 = this.IsUseGrenadeActive();
		this.SetActiveChecked(this.grenadeButton.gameObject, (flag ? true : flag1));
		if (!this.grenadeButton.gameObject.activeSelf)
		{
			return;
		}
		UISprite uISprite = this.grenadeButton.grenadeSprite;
		if ((this.grenadePressed || this._isBuyGrenadePressed) && this.grenadeRect.Contains(UICamera.lastTouchPosition))
		{
			str = (!Defs.isDaterRegim ? "grenade_btn_n" : "grenade_like_btn_n");
		}
		else
		{
			str = (!Defs.isDaterRegim ? "grenade_btn" : "grenade_like_btn");
		}
		uISprite.spriteName = str;
		if (!flag)
		{
			this.grenadeButton.gameObject.SetActive(true);
			this.SetActiveChecked(this.grenadeButton.priceLabel.gameObject, false);
			int grenadeCount = TouchPadController.GetGrenadeCount();
			this.SetActiveChecked(this.grenadeButton.countLabel.gameObject, true);
			this.grenadeButton.countLabel.text = grenadeCount.ToString();
			this.SetActiveChecked(this.grenadeButton.fullLabel.gameObject, false);
		}
		else if (!Defs.isDaterRegim)
		{
			this.grenadeButton.gameObject.SetActive(false);
		}
		else
		{
			this.SetActiveChecked(this.grenadeButton.priceLabel.gameObject, true);
			this.SetActiveChecked(this.grenadeButton.countLabel.gameObject, false);
			this.SetActiveChecked(this.grenadeButton.fullLabel.gameObject, false);
		}
	}

	private void SetShouldRecalcRects()
	{
		this._shouldRecalcRects = true;
	}

	private void SetSide()
	{
		bool flag;
		if (base.GetComponent<UIAnchor>().side != UIAnchor.Side.BottomRight || !GlobalGameController.LeftHanded)
		{
			flag = (base.GetComponent<UIAnchor>().side != UIAnchor.Side.BottomLeft ? false : !GlobalGameController.LeftHanded);
		}
		else
		{
			flag = true;
		}
		bool flag1 = flag;
		base.GetComponent<UIAnchor>().side = (!GlobalGameController.LeftHanded ? UIAnchor.Side.BottomLeft : UIAnchor.Side.BottomRight);
		Vector3 component = base.GetComponent<BoxCollider>().center;
		component.x = component.x * (!flag1 ? -1f : 1f);
		base.GetComponent<BoxCollider>().center = component;
	}

	private void SetSideAndCalcRects()
	{
		this.SetSide();
		this.SetShouldRecalcRects();
	}

	private void SetSpritesState()
	{
		bool flag;
		bool flag1;
		this.SetGrenadeUISpriteState();
		if (WeaponManager.sharedManager != null && (WeaponManager.sharedManager.currentWeaponSounds.gameObject.name.Equals("WeaponGrenade(Clone)") || WeaponManager.sharedManager.currentWeaponSounds.gameObject.name.Equals("WeaponLike(Clone)")))
		{
			return;
		}
		this.jumpSprite.gameObject.SetActive((Defs.isJumpAndShootButtonOn || !Defs.isUse3DTouch ? (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None ? 0 : (int)(TrainingController.stepTraining < TrainingState.GetTheGun)) == 0 : false));
		bool flag2 = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None ? true : TrainingController.FireButtonEnabled);
		this.fireSprite.gameObject.SetActive(((Defs.isJumpAndShootButtonOn || !Defs.isUse3DTouch) && !Defs.isTurretWeapon && flag2 && (!this.isHunger || this.hungerGameController.isGo) ? !WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon : false));
		GameObject gameObject = this.reloadSpirte.gameObject;
		if ((!(InGameGUI.sharedInGameGUI.playerMoveC != null) || InGameGUI.sharedInGameGUI.playerMoveC.isMechActive) && !(InGameGUI.sharedInGameGUI.playerMoveC == null) || Defs.isTurretWeapon || !flag2)
		{
			flag = false;
		}
		else
		{
			flag = (!(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.currentWeaponSounds != null) || WeaponManager.sharedManager.currentWeaponSounds.isMelee ? false : !WeaponManager.sharedManager.currentWeaponSounds.isShotMelee);
		}
		gameObject.SetActive(flag);
		GameObject gameObject1 = this.zoomSprite.gameObject;
		if ((!(InGameGUI.sharedInGameGUI.playerMoveC != null) || InGameGUI.sharedInGameGUI.playerMoveC.isMechActive) && !(InGameGUI.sharedInGameGUI.playerMoveC == null) || Defs.isTurretWeapon || !flag2)
		{
			flag1 = false;
		}
		else
		{
			flag1 = (!(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.currentWeaponSounds != null) ? false : WeaponManager.sharedManager.currentWeaponSounds.isZooming);
		}
		gameObject1.SetActive(flag1);
		if (this.jumpIcon.activeSelf == Defs.isJetpackEnabled)
		{
			this.jumpIcon.SetActive(!Defs.isJetpackEnabled);
		}
		if (this.jetPackIcon.activeSelf != Defs.isJetpackEnabled)
		{
			this.jetPackIcon.SetActive(Defs.isJetpackEnabled);
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		TouchPadController.u003cStartu003ec__Iterator1C1 variable = null;
		return variable;
	}

	private void Update()
	{
		this.framesCount++;
		if (this.fireRect.width.Equals(0f))
		{
			this.CalcRects();
		}
		this.SetSpritesState();
		this._isFirstFrame = false;
		if (!this._joyActive)
		{
			this.jumpPressed = false;
			this.isShooting = false;
			this.isShootingPressure = false;
			this._touchControlScheme.Reset();
			return;
		}
		if (this.isInvokeGrenadePress && !this.grenadeRect.Contains(UICamera.lastTouchPosition))
		{
			this.isInvokeGrenadePress = false;
			base.CancelInvoke("GrenadePressInvoke");
		}
		this._touchControlScheme.OnUpdate();
		if (Input.touchCount <= 0)
		{
			this.MoveTouchID = -1;
			this.pastPos = Vector2.zero;
			this.pastDelta = Vector2.zero;
		}
		else
		{
			if (this.MoveTouchID == -1)
			{
				for (int i = 0; i < (int)Input.touches.Length; i++)
				{
					if (Input.touches[i].phase == TouchPhase.Began && this.moveRect.Contains(Input.touches[i].position))
					{
						this.MoveTouchID = Input.touches[i].fingerId;
					}
				}
			}
			this.UpdateMoveTouch();
		}
	}

	private void UpdateMoveTouch()
	{
		for (int i = 0; i < (int)Input.touches.Length; i++)
		{
			if (Input.touches[i].fingerId == this.MoveTouchID)
			{
				Touch touch = Input.touches[i];
				if (this.pastPos == Vector2.zero)
				{
					this.pastPos = touch.position;
				}
				Vector2 vector2 = touch.position - this.pastPos;
				if (!(vector2 == Vector2.zero) || !(this.pastDelta != Vector2.zero) || !this.IsDifferendDirections(this.pastDelta, JoystickController.leftJoystick.@value) || this.pastDelta.sqrMagnitude <= 15f)
				{
					vector2 -= this.compDeltas;
					this.compDeltas = Vector2.zero;
				}
				else
				{
					vector2 = this.pastDelta / 2f;
					this.compDeltas += vector2;
				}
				this.OnDragTouch(vector2);
				this.pastPos = touch.position;
				this.pastDelta = vector2;
				if (touch.phase == TouchPhase.Ended)
				{
					this.MoveTouchID = -1;
					this.pastPos = Vector2.zero;
					this.pastDelta = Vector2.zero;
				}
			}
		}
	}
}