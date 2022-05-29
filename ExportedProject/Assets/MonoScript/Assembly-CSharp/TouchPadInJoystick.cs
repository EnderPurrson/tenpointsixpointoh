using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TouchPadInJoystick : MonoBehaviour
{
	public Transform fireSprite;

	public bool isShooting;

	public bool isJumpPressed;

	public InGameGUI inGameGUI;

	public bool isActiveFireButton;

	private Rect _fireRect = new Rect();

	private bool _shouldRecalcRects;

	private bool _isFirstFrame = true;

	private HungerGameController _hungerGameController;

	private bool _joyActive = true;

	private Player_move_c _playerMoveC;

	private bool pressured;

	public TouchPadInJoystick()
	{
	}

	[DebuggerHidden]
	private IEnumerator _SetIsFirstFrame()
	{
		TouchPadInJoystick.u003c_SetIsFirstFrameu003ec__Iterator1C5 variable = null;
		return variable;
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
		float single2 = 60f;
		if ((int)array.Length > 6)
		{
			single2 = (float)array[6] * 0.5f;
		}
		vector3.center = vector3.center + new Vector3(single1 * 0.5f, single * 0.5f, 0f);
		Vector3 vector31 = vector3.center;
		float coef = (vector31.x - single2) * Defs.Coef;
		Vector3 vector32 = vector3.center;
		this._fireRect = new Rect(coef, (vector32.y - single2) * Defs.Coef, 2f * single2 * Defs.Coef, 2f * single2 * Defs.Coef);
	}

	private bool IsActiveFireButton()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None || Defs.isTurretWeapon)
		{
			return false;
		}
		if (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.On)
		{
			return true;
		}
		if (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.Sniper && this._playerMoveC != null && this._playerMoveC.isZooming)
		{
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= new Action(this.SetSideAndCalcRects);
		ControlsSettingsBase.ControlsChanged -= new Action(this.SetShouldRecalcRects);
	}

	private void OnEnable()
	{
		this.isShooting = false;
		if (this._shouldRecalcRects)
		{
			base.StartCoroutine(this.ReCalcRects());
		}
		this._shouldRecalcRects = false;
		base.StartCoroutine(this._SetIsFirstFrame());
	}

	private void OnPress(bool isDown)
	{
		if (!this._joyActive)
		{
			return;
		}
		if (this.inGameGUI.playerMoveC == null)
		{
			return;
		}
		if (this._fireRect.width.Equals(0f))
		{
			this.CalcRects();
		}
		if (this._isFirstFrame)
		{
			return;
		}
		if (isDown && this._fireRect.Contains(UICamera.lastTouchPosition) && this.fireSprite.gameObject.activeSelf)
		{
			this.isShooting = true;
		}
		if (!isDown)
		{
			this.isShooting = false;
		}
	}

	private void OnPressure(float pressure)
	{
		if (!Defs.touchPressureSupported || !Defs.isUse3DTouch || pressure <= 0.8f)
		{
			this.pressured = false;
			this.isJumpPressed = false;
		}
		else
		{
			if (this.pressured)
			{
				return;
			}
			this.pressured = true;
			this.isJumpPressed = true;
			if (TrainingController.sharedController != null)
			{
				TrainingController.sharedController.Hide3dTouchJump();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator ReCalcRects()
	{
		TouchPadInJoystick.u003cReCalcRectsu003ec__Iterator1C4 variable = null;
		return variable;
	}

	public void SetJoystickActive(bool active)
	{
		this._joyActive = active;
		if (!active)
		{
			this.isShooting = false;
		}
	}

	private void SetShouldRecalcRects()
	{
		this._shouldRecalcRects = true;
	}

	private void SetSideAndCalcRects()
	{
		this.SetShouldRecalcRects();
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		TouchPadInJoystick.u003cStartu003ec__Iterator1C6 variable = null;
		return variable;
	}

	private void Update()
	{
		if (this._playerMoveC == null)
		{
			if (!Defs.isMulti || !(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.myPlayer != null))
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this._playerMoveC = gameObject.GetComponent<SkinName>().playerMoveC;
				}
			}
			else
			{
				this._playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		if (!this._joyActive)
		{
			this.isShooting = false;
			return;
		}
		this.isActiveFireButton = this.IsActiveFireButton();
		if (this.isActiveFireButton != this.fireSprite.gameObject.activeSelf)
		{
			this.fireSprite.gameObject.SetActive(this.isActiveFireButton);
		}
	}
}