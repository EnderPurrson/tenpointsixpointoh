using System;
using UnityEngine;

public class UIJoystick : MonoBehaviour
{
	public const int NO_TOUCH_ID = -100;

	public Transform target;

	public float radius;

	public Vector2 @value;

	private HungerGameController _hungerGameController;

	private bool _isHunger;

	private float? _actualRadius;

	private TouchPadInJoystick touchPadInJoystick;

	private UIWidget _joystickWidget;

	private bool _grabTouches;

	private Vector2 _touchWorldPos;

	private Vector2 _touchPrevWorldPos;

	private int _touchId;

	public float ActualRadius
	{
		get
		{
			return (!this._actualRadius.HasValue ? this.radius : this._actualRadius.Value);
		}
		set
		{
			this._actualRadius = new float?(value);
		}
	}

	public float ActualRadiusSq
	{
		get
		{
			float actualRadius = this.ActualRadius;
			return actualRadius * actualRadius;
		}
	}

	public UIJoystick()
	{
	}

	private void Awake()
	{
		this._joystickWidget = base.GetComponent<UIWidget>();
		this.touchPadInJoystick = base.GetComponent<TouchPadInJoystick>();
		this._isHunger = Defs.isHunger;
	}

	private void ChangeSide()
	{
		base.transform.parent.GetComponent<UIAnchor>().side = (!GlobalGameController.LeftHanded ? UIAnchor.Side.BottomRight : UIAnchor.Side.BottomLeft);
		this.Reset();
	}

	public static Touch? GetTouchById(int touchId)
	{
		Touch[] touchArray = Input.touches;
		for (int i = 0; i < (int)touchArray.Length; i++)
		{
			if (touchArray[i].fingerId == touchId && (touchArray[i].phase == TouchPhase.Began || touchArray[i].phase == TouchPhase.Moved || touchArray[i].phase == TouchPhase.Stationary))
			{
				return new Touch?(touchArray[i]);
			}
		}
		return null;
	}

	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= new Action(this.ChangeSide);
	}

	private void OnDrag2(Vector2 delta)
	{
		this.target.position = UICamera.currentCamera.ScreenToWorldPoint(this._touchWorldPos);
		Transform vector3 = this.target;
		float single = this.target.localPosition.x;
		Vector3 vector31 = this.target.localPosition;
		vector3.localPosition = new Vector3(single, vector31.y, 0f);
		if (this.target.localPosition.magnitude > this.ActualRadius)
		{
			this.target.localPosition = Vector3.ClampMagnitude(this.target.localPosition, this.ActualRadius);
		}
		this.@value = this.target.localPosition;
		this.@value = (this.@value / this.ActualRadius) * Mathf.InverseLerp(this.ActualRadius, 2f, 1f);
	}

	private void OnPress(bool isDown)
	{
		if (isDown && this._touchId == -100 || !isDown && this._touchId != -100)
		{
			this._grabTouches = isDown;
			this._touchId = (!isDown ? -100 : UICamera.currentTouchID);
			this._touchWorldPos = (!isDown ? Vector2.zero : UICamera.currentTouch.pos);
			this._touchPrevWorldPos = this._touchWorldPos;
		}
	}

	private void ProcessInput()
	{
		if (this._grabTouches)
		{
			this.ProcessTouches();
		}
		else if (!Defs.isMouseControl)
		{
			this.Reset();
		}
	}

	private void ProcessTouches()
	{
		if (this._touchId != -100)
		{
			Touch? touchById = UIJoystick.GetTouchById(this._touchId);
			if (touchById.HasValue)
			{
				this._touchWorldPos = touchById.Value.position;
			}
		}
		if (this._touchId == -100)
		{
			this.Reset();
		}
		else
		{
			Vector2 vector2 = this._touchPrevWorldPos - this._touchWorldPos;
			this._touchPrevWorldPos = this._touchWorldPos;
			this.OnDrag2(vector2);
			if (this.touchPadInJoystick.isShooting)
			{
				this.@value = Vector2.zero;
				this.target.localPosition = Vector3.zero;
			}
		}
	}

	public void Reset()
	{
		this.@value = Vector2.zero;
		this.target.localPosition = Vector3.zero;
		this._grabTouches = false;
		this._touchId = -100;
		this._touchWorldPos = Vector2.zero;
		this._touchPrevWorldPos = this._touchWorldPos;
	}

	public void SetJoystickActive(bool joyActive)
	{
		base.enabled = joyActive;
		if (!joyActive)
		{
			this.Reset();
		}
	}

	private void Start()
	{
		this.ChangeSide();
		PauseNGUIController.PlayerHandUpdated += new Action(this.ChangeSide);
		if (this._isHunger)
		{
			this._hungerGameController = GameObject.FindGameObjectWithTag("HungerGameController").GetComponent<HungerGameController>();
		}
		this.UpdateVisibility();
		this.Reset();
	}

	private void Update()
	{
		this.ProcessInput();
		this.UpdateVisibility();
	}

	private void UpdateVisibility()
	{
		this._joystickWidget.alpha = ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None || TrainingController.stepTraining >= TrainingState.TapToMove ? (!this._isHunger ? 0 : (int)(!this._hungerGameController.isGo)) != 0 : true) ? 0f : 1f);
	}
}