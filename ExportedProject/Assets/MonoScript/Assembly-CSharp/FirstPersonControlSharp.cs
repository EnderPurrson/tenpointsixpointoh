using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

internal sealed class FirstPersonControlSharp : MonoBehaviour
{
	private const string newbieJumperAchievement = "NewbieJumperAchievement";

	private const int maxJumpCount = 10;

	private const string keyNinja = "NinjaJumpsCount";

	public Transform cameraPivot;

	public float forwardSpeed = 4f;

	public float backwardSpeed = 1f;

	public float sidestepSpeed = 1f;

	public float jumpSpeed = 4.5f;

	public float inAirMultiplier = 0.25f;

	public Vector2 rotationSpeed = new Vector2(2f, 1f);

	public float tiltPositiveYAxis = 0.6f;

	public float tiltNegativeYAxis = 0.4f;

	public float tiltXAxisMinimum = 0.1f;

	public string myIp;

	public GameObject playerGameObject;

	public int typeAnim;

	private Transform thisTransform;

	public GameObject camPlayer;

	private CharacterController character;

	private Vector3 cameraVelocity;

	private Vector3 velocity;

	private bool canJump = true;

	public bool isMine;

	private Rect fireZone;

	private Rect jumpZone;

	private bool jump;

	private float timeUpdateAnim;

	public AudioClip jumpClip;

	private Player_move_c _moveC;

	public float gravityMultiplier = 1f;

	private Vector3 mousePosOld = Vector3.zero;

	private bool _invert;

	public bool ninjaJumpUsed = true;

	private HungerGameController hungerGameController;

	private bool isHunger;

	private bool isInet;

	private bool isMulti;

	private SkinName mySkinName;

	private int oldJumpCount;

	private int oldNinjaJumpsCount;

	private Vector3 _movement;

	private Vector2 _cameraMouseDelta;

	private bool jumpBy3dTouch;

	private Vector3 rinkMovement;

	private bool steptRink;

	private bool secondJumpEnabled = true;

	public FirstPersonControlSharp()
	{
	}

	private void Awake()
	{
		this.isHunger = Defs.isHunger;
		this.isInet = Defs.isInet;
		this.isMulti = Defs.isMulti;
	}

	[DebuggerHidden]
	private IEnumerator EnableSecondJump()
	{
		FirstPersonControlSharp.u003cEnableSecondJumpu003ec__Iterator1C7 variable = null;
		return variable;
	}

	private Vector2 GrabCameraInputDelta()
	{
		Vector2 vector2 = Vector2.zero;
		TouchPadController touchPadController = JoystickController.rightJoystick;
		if (touchPadController != null)
		{
			vector2 = touchPadController.GrabDeltaPosition();
		}
		return vector2;
	}

	private void HandleInvertCamUpdated()
	{
		this._invert = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
	}

	private void Jump()
	{
		if (!TrainingController.TrainingCompleted)
		{
			TrainingController.timeShowJump = 1000f;
			HintController.instance.HideHintByName("press_jump");
		}
		this.jump = true;
		this.canJump = false;
		if (!Defs.isJetpackEnabled)
		{
			this.mySkinName.sendAnimJump();
		}
		if (TrainingController.TrainingCompleted && (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android || BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer) && Social.localUser.authenticated)
		{
			int num = this.oldJumpCount + 1;
			if (this.oldJumpCount < 10)
			{
				this.oldJumpCount = num;
				if (num == 10)
				{
					PlayerPrefs.SetInt("NewbieJumperAchievement", num);
					float single = 100f;
					string str = (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer || Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon ? "Jumper_id" : "CgkIr8rGkPIJEAIQAQ");
					if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
					{
						Social.ReportProgress(str, (double)single, (bool success) => string.Format("Newbie Jumper achievement progress {0:0.0}%: {1}", single, success));
					}
					else
					{
						AGSAchievementsClient.UpdateAchievementProgress(str, single, 0);
					}
				}
			}
		}
	}

	public void MoveCamera(Vector2 delta)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.SwipeToRotate && delta != Vector2.zero)
		{
			TrainingController.isNextStep = TrainingState.SwipeToRotate;
		}
		float sensitivity = Defs.Sensitivity;
		float single = 1f;
		if (this._moveC != null)
		{
			single = single * (!this._moveC.isZooming ? 1f : 0.2f);
		}
		if (JoystickController.rightJoystick != null)
		{
			JoystickController.rightJoystick.ApplyDeltaTo(delta, this.thisTransform, this.cameraPivot.transform, sensitivity * single, this._invert);
		}
	}

	private void OnDestroy()
	{
		if (!this.isMulti || this.isMine)
		{
			PauseNGUIController.InvertCamUpdated -= new Action(this.HandleInvertCamUpdated);
		}
	}

	private void OnEndGame()
	{
		if (!this.isMulti || this.isMine)
		{
			if (JoystickController.leftJoystick)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(false);
			}
			if (JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.gameObject.SetActive(false);
			}
		}
		base.enabled = false;
	}

	private void RegisterNinjAchievment()
	{
		if (Social.localUser.authenticated)
		{
			int num = this.oldNinjaJumpsCount + 1;
			if (this.oldNinjaJumpsCount < 50)
			{
				Storager.setInt("NinjaJumpsCount", num, false);
			}
			this.oldNinjaJumpsCount = num;
			if (!Storager.hasKey("ParkourNinjaAchievementCompleted") && num >= 50)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					GpgFacade instance = GpgFacade.Instance;
					instance.IncrementAchievement("CgkIr8rGkPIJEAIQAw", 1, (bool success) => UnityEngine.Debug.Log(string.Concat("Achievement Parkour Ninja incremented: ", success)));
				}
				Storager.setInt("ParkourNinjaAchievementCompleted", 1, false);
			}
		}
	}

	[PunRPC]
	[RPC]
	private void setIp(string _ip)
	{
		this.myIp = _ip;
	}

	private void Start()
	{
		this.mySkinName = base.GetComponent<SkinName>();
		if (this.isInet)
		{
			this.isMine = PhotonView.Get(this).isMine;
		}
		else
		{
			this.isMine = base.GetComponent<NetworkView>().isMine;
		}
		if (this.isHunger)
		{
			this.hungerGameController = HungerGameController.Instance;
		}
		if (!this.isMulti || this.isMine)
		{
			this.HandleInvertCamUpdated();
			PauseNGUIController.InvertCamUpdated += new Action(this.HandleInvertCamUpdated);
			this.oldJumpCount = PlayerPrefs.GetInt("NewbieJumperAchievement", 0);
			this.oldNinjaJumpsCount = (!Storager.hasKey("NinjaJumpsCount") ? 0 : Storager.getInt("NinjaJumpsCount", false));
		}
		this.thisTransform = base.GetComponent<Transform>();
		this.character = base.GetComponent<CharacterController>();
		this._moveC = this.playerGameObject.GetComponent<Player_move_c>();
	}

	private void Update()
	{
		if (this.isMulti && !this.isMine)
		{
			return;
		}
		if (this.mySkinName.playerMoveC.isKilled)
		{
			return;
		}
		if (JoystickController.leftJoystick == null || JoystickController.rightJoystick == null)
		{
			return;
		}
		if (this.mySkinName.playerMoveC.isRocketJump && this.character.isGrounded)
		{
			this.mySkinName.playerMoveC.isRocketJump = false;
		}
		this._movement = this.thisTransform.TransformDirection(new Vector3(JoystickController.leftJoystick.@value.x, 0f, JoystickController.leftJoystick.@value.y));
		if ((!this.isHunger || !this.hungerGameController.isGo) && this.isHunger)
		{
			this._movement = Vector3.zero;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining < TrainingState.TapToMove)
		{
			this._movement = Vector3.zero;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToMove && this._movement != Vector3.zero)
		{
			TrainingController.isNextStep = TrainingState.TapToMove;
		}
		this._movement.y = 0f;
		this._movement.Normalize();
		Vector2 vector2 = new Vector2(Mathf.Abs(JoystickController.leftJoystick.@value.x), Mathf.Abs(JoystickController.leftJoystick.@value.y));
		if (JoystickController.leftTouchPad.isShooting && JoystickController.leftTouchPad.isActiveFireButton)
		{
			vector2 = new Vector2(0f, 0f);
		}
		if (vector2.y <= vector2.x)
		{
			FirstPersonControlSharp firstPersonControlSharp = this;
			firstPersonControlSharp._movement = firstPersonControlSharp._movement * (this.sidestepSpeed * EffectsController.SpeedModifier(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1) * vector2.x * (float)((!this.character.isGrounded ? 1 : 1)));
		}
		else if (JoystickController.leftJoystick.@value.y <= 0f)
		{
			FirstPersonControlSharp firstPersonControlSharp1 = this;
			firstPersonControlSharp1._movement = firstPersonControlSharp1._movement * (this.backwardSpeed * EffectsController.SpeedModifier(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1) * vector2.y);
		}
		else
		{
			FirstPersonControlSharp firstPersonControlSharp2 = this;
			firstPersonControlSharp2._movement = firstPersonControlSharp2._movement * (this.forwardSpeed * EffectsController.SpeedModifier(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1) * vector2.y);
		}
		if (!this.character.isGrounded)
		{
			if (!JoystickController.leftTouchPad.isJumpPressed && this.jumpBy3dTouch)
			{
				this.secondJumpEnabled = true;
				this.jumpBy3dTouch = false;
			}
			if (this.jump && this.mySkinName.interpolateScript.myAnim == 0 && !Defs.isJetpackEnabled)
			{
				this.mySkinName.sendAnimJump();
			}
			TouchPadController touchPadController = JoystickController.rightJoystick;
			TouchPadInJoystick touchPadInJoystick = JoystickController.leftTouchPad;
			if ((touchPadController.jumpPressed || touchPadInJoystick.isJumpPressed) && (EffectsController.NinjaJumpEnabled && !this.ninjaJumpUsed && this.secondJumpEnabled || Defs.isJetpackEnabled))
			{
				if (!Defs.isJetpackEnabled)
				{
					this.RegisterNinjAchievment();
				}
				this.ninjaJumpUsed = true;
				this.canJump = false;
				if (!Defs.isJetpackEnabled)
				{
					this.mySkinName.sendAnimJump();
				}
				this.velocity.y = 1.1f * (this.jumpSpeed * EffectsController.JumpModifier);
			}
			if (!Defs.isJetpackEnabled)
			{
				touchPadController.jumpPressed = false;
			}
			ref Vector3 vector3Pointer = ref this.velocity;
			float single = vector3Pointer.y;
			Vector3 vector3 = Physics.gravity;
			vector3Pointer.y = single + vector3.y * this.gravityMultiplier * Time.deltaTime;
		}
		else
		{
			if (EffectsController.NinjaJumpEnabled)
			{
				this.ninjaJumpUsed = false;
			}
			this.canJump = true;
			this.jump = false;
			TouchPadController touchPadController1 = JoystickController.rightJoystick;
			if (this.canJump && (touchPadController1.jumpPressed || JoystickController.leftTouchPad.isJumpPressed))
			{
				if (!Defs.isJetpackEnabled)
				{
					touchPadController1.jumpPressed = false;
				}
				this.Jump();
			}
			if (this.jump)
			{
				this.secondJumpEnabled = false;
				if (JoystickController.leftTouchPad.isJumpPressed)
				{
					this.jumpBy3dTouch = true;
				}
				else
				{
					base.StartCoroutine(this.EnableSecondJump());
				}
				this.velocity = Vector3.zero;
				this.velocity.y = this.jumpSpeed * EffectsController.JumpModifier;
			}
		}
		this._movement += this.velocity;
		FirstPersonControlSharp firstPersonControlSharp3 = this;
		firstPersonControlSharp3._movement = firstPersonControlSharp3._movement + (Physics.gravity * this.gravityMultiplier);
		this._movement *= Time.deltaTime;
		this.timeUpdateAnim -= Time.deltaTime;
		if (this.timeUpdateAnim < 0f)
		{
			if (!this.character.isGrounded)
			{
				this._moveC.WalkAnimation();
			}
			else
			{
				this.timeUpdateAnim = 0.5f;
				Vector2 vector21 = new Vector2(this._movement.x, this._movement.z);
				if (vector21.sqrMagnitude <= 0f)
				{
					this._moveC.IdleAnimation();
				}
				else
				{
					this._moveC.WalkAnimation();
				}
			}
		}
		this.Update2();
	}

	private void Update2()
	{
		if (!this.character.enabled)
		{
			return;
		}
		if (this.mySkinName.onRink)
		{
			if (!this.steptRink)
			{
				this.rinkMovement = this._movement;
				this.steptRink = true;
			}
			this.rinkMovement = Vector3.MoveTowards(this.rinkMovement, this._movement, 0.068f * Time.deltaTime);
			this.rinkMovement.y = this._movement.y;
			this.character.Move(this.rinkMovement);
		}
		else
		{
			if (this.mySkinName.onConveyor)
			{
				FirstPersonControlSharp firstPersonControlSharp = this;
				firstPersonControlSharp._movement = firstPersonControlSharp._movement + (this.mySkinName.conveyorDirection * Time.deltaTime);
			}
			this.character.Move(this._movement);
			this._movement = Vector3.zero;
			this.steptRink = false;
		}
		if (!this.character.isGrounded)
		{
			if (this.mySkinName.onRink)
			{
				this.rinkMovement = this._movement;
			}
			this.mySkinName.onConveyor = false;
		}
		else
		{
			this.velocity = Vector3.zero;
		}
		Vector2 vector2 = this.GrabCameraInputDelta();
		if (Device.isPixelGunLow && Defs.isTouchControlSmoothDump)
		{
			this.MoveCamera(vector2);
		}
		if (Defs.isMulti && CameraSceneController.sharedController.killCamController.enabled)
		{
			CameraSceneController.sharedController.killCamController.UpdateMouseX();
		}
	}

	private Vector2 updateKeyboardControls()
	{
		int num = 0;
		int num1 = 0;
		if (Input.GetKey("w"))
		{
			num = 1;
		}
		if (Input.GetKey("s"))
		{
			num = -1;
		}
		if (Input.GetKey("a"))
		{
			num1 = -1;
		}
		if (Input.GetKey("d"))
		{
			num1 = 1;
		}
		return new Vector2((float)num1, (float)num);
	}
}