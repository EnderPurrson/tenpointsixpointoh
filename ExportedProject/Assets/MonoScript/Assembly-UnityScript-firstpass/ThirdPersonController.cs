using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[Serializable]
public class ThirdPersonController : MonoBehaviour
{
	public AnimationClip idleAnimation;

	public AnimationClip walkAnimation;

	public AnimationClip runAnimation;

	public AnimationClip jumpPoseAnimation;

	public float walkMaxAnimationSpeed;

	public float trotMaxAnimationSpeed;

	public float runMaxAnimationSpeed;

	public float jumpAnimationSpeed;

	public float landAnimationSpeed;

	private Animation _animation;

	private CharacterState _characterState;

	public float walkSpeed;

	public float trotSpeed;

	public float runSpeed;

	public float inAirControlAcceleration;

	public float jumpHeight;

	public float gravity;

	public float speedSmoothing;

	public float rotateSpeed;

	public float trotAfterSeconds;

	public bool canJump;

	private float jumpRepeatTime;

	private float jumpTimeout;

	private float groundedTimeout;

	private float lockCameraTimer;

	private Vector3 moveDirection;

	private float verticalSpeed;

	private float moveSpeed;

	private CollisionFlags collisionFlags;

	private bool jumping;

	private bool jumpingReachedApex;

	private bool movingBack;

	private bool isMoving;

	private float walkTimeStart;

	private float lastJumpButtonTime;

	private float lastJumpTime;

	private float lastJumpStartHeight;

	private Vector3 inAirVelocity;

	private float lastGroundedTime;

	private bool isControllable;

	public ThirdPersonController()
	{
		this.walkMaxAnimationSpeed = 0.75f;
		this.trotMaxAnimationSpeed = 1f;
		this.runMaxAnimationSpeed = 1f;
		this.jumpAnimationSpeed = 1.15f;
		this.landAnimationSpeed = 1f;
		this.walkSpeed = 2f;
		this.trotSpeed = 4f;
		this.runSpeed = 6f;
		this.inAirControlAcceleration = 3f;
		this.jumpHeight = 0.5f;
		this.gravity = 20f;
		this.speedSmoothing = 10f;
		this.rotateSpeed = 500f;
		this.trotAfterSeconds = 3f;
		this.canJump = true;
		this.jumpRepeatTime = 0.05f;
		this.jumpTimeout = 0.15f;
		this.groundedTimeout = 0.25f;
		this.moveDirection = Vector3.zero;
		this.lastJumpButtonTime = -10f;
		this.lastJumpTime = -1f;
		this.inAirVelocity = Vector3.zero;
		this.isControllable = true;
	}

	public override void ApplyGravity()
	{
		if (this.isControllable)
		{
			Input.GetButton("Jump");
			if (this.jumping && !this.jumpingReachedApex && this.verticalSpeed <= (float)0)
			{
				this.jumpingReachedApex = true;
				this.SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
			if (!this.IsGrounded())
			{
				this.verticalSpeed = this.verticalSpeed - this.gravity * Time.deltaTime;
			}
			else
			{
				this.verticalSpeed = (float)0;
			}
		}
	}

	public override void ApplyJumping()
	{
		if (this.lastJumpTime + this.jumpRepeatTime <= Time.time)
		{
			if (this.IsGrounded() && this.canJump && Time.time < this.lastJumpButtonTime + this.jumpTimeout)
			{
				this.verticalSpeed = this.CalculateJumpVerticalSpeed(this.jumpHeight);
				this.SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public override void Awake()
	{
		this.moveDirection = this.transform.TransformDirection(Vector3.forward);
		this._animation = (Animation)this.GetComponent(typeof(Animation));
		if (!this._animation)
		{
			Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		}
		if (!this.idleAnimation)
		{
			this._animation = null;
			Debug.Log("No idle animation found. Turning off animations.");
		}
		if (!this.walkAnimation)
		{
			this._animation = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if (!this.runAnimation)
		{
			this._animation = null;
			Debug.Log("No run animation found. Turning off animations.");
		}
		if (!this.jumpPoseAnimation && this.canJump)
		{
			this._animation = null;
			Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
		}
	}

	public override float CalculateJumpVerticalSpeed(float targetJumpHeight)
	{
		return Mathf.Sqrt((float)2 * targetJumpHeight * this.gravity);
	}

	public override void DidJump()
	{
		this.jumping = true;
		this.jumpingReachedApex = false;
		this.lastJumpTime = Time.time;
		this.lastJumpStartHeight = this.transform.position.y;
		this.lastJumpButtonTime = (float)-10;
		this._characterState = CharacterState.Jumping;
	}

	public override Vector3 GetDirection()
	{
		return this.moveDirection;
	}

	public override float GetLockCameraTimer()
	{
		return this.lockCameraTimer;
	}

	public override float GetSpeed()
	{
		return this.moveSpeed;
	}

	public override bool HasJumpReachedApex()
	{
		return this.jumpingReachedApex;
	}

	public override bool IsGrounded()
	{
		return (this.collisionFlags & CollisionFlags.Below) != CollisionFlags.None;
	}

	public override bool IsGroundedWithTimeout()
	{
		return this.lastGroundedTime + this.groundedTimeout > Time.time;
	}

	public override bool IsJumping()
	{
		return this.jumping;
	}

	public override bool IsMoving()
	{
		return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}

	public override bool IsMovingBackwards()
	{
		return this.movingBack;
	}

	public override void Main()
	{
	}

	public override void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.moveDirection.y > 0.01f)
		{
		}
	}

	public override void Reset()
	{
		this.gameObject.tag = "Player";
	}

	public override void Update()
	{
		if (!this.isControllable)
		{
			Input.ResetInputAxes();
		}
		if (Input.GetButtonDown("Jump"))
		{
			this.lastJumpButtonTime = Time.time;
		}
		this.UpdateSmoothedMovementDirection();
		this.ApplyGravity();
		this.ApplyJumping();
		Vector3 vector3 = ((this.moveDirection * this.moveSpeed) + new Vector3((float)0, this.verticalSpeed, (float)0)) + this.inAirVelocity;
		vector3 *= Time.deltaTime;
		CharacterController component = (CharacterController)this.GetComponent(typeof(CharacterController));
		this.collisionFlags = component.Move(vector3);
		if (this._animation)
		{
			if (this._characterState == CharacterState.Jumping)
			{
				if (this.jumpingReachedApex)
				{
					this._animation[this.jumpPoseAnimation.name].speed = -this.landAnimationSpeed;
					this._animation[this.jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					this._animation.CrossFade(this.jumpPoseAnimation.name);
				}
				else
				{
					this._animation[this.jumpPoseAnimation.name].speed = this.jumpAnimationSpeed;
					this._animation[this.jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					this._animation.CrossFade(this.jumpPoseAnimation.name);
				}
			}
			else if (component.velocity.sqrMagnitude < 0.1f)
			{
				this._animation.CrossFade(this.idleAnimation.name);
			}
			else if (this._characterState == CharacterState.Running)
			{
				AnimationState item = this._animation[this.runAnimation.name];
				Vector3 vector31 = component.velocity;
				item.speed = Mathf.Clamp(vector31.magnitude, (float)0, this.runMaxAnimationSpeed);
				this._animation.CrossFade(this.runAnimation.name);
			}
			else if (this._characterState == CharacterState.Trotting)
			{
				AnimationState animationState = this._animation[this.walkAnimation.name];
				Vector3 vector32 = component.velocity;
				animationState.speed = Mathf.Clamp(vector32.magnitude, (float)0, this.trotMaxAnimationSpeed);
				this._animation.CrossFade(this.walkAnimation.name);
			}
			else if (this._characterState == CharacterState.Walking)
			{
				AnimationState item1 = this._animation[this.walkAnimation.name];
				Vector3 vector33 = component.velocity;
				item1.speed = Mathf.Clamp(vector33.magnitude, (float)0, this.walkMaxAnimationSpeed);
				this._animation.CrossFade(this.walkAnimation.name);
			}
		}
		if (!this.IsGrounded())
		{
			Vector3 vector34 = vector3;
			vector34.y = (float)0;
			if (vector34.sqrMagnitude > 0.001f)
			{
				this.transform.rotation = Quaternion.LookRotation(vector34);
			}
		}
		else
		{
			this.transform.rotation = Quaternion.LookRotation(this.moveDirection);
		}
		if (this.IsGrounded())
		{
			this.lastGroundedTime = Time.time;
			this.inAirVelocity = Vector3.zero;
			if (this.jumping)
			{
				this.jumping = false;
				this.SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public override void UpdateSmoothedMovementDirection()
	{
		Transform transforms = Camera.main.transform;
		bool flag = this.IsGrounded();
		Vector3 vector3 = transforms.TransformDirection(Vector3.forward);
		vector3.y = (float)0;
		vector3 = vector3.normalized;
		Vector3 vector31 = new Vector3(vector3.z, (float)0, -vector3.x);
		float axisRaw = Input.GetAxisRaw("Vertical");
		float single = Input.GetAxisRaw("Horizontal");
		if (axisRaw >= -0.2f)
		{
			this.movingBack = false;
		}
		else
		{
			this.movingBack = true;
		}
		bool flag1 = this.isMoving;
		bool flag2 = Mathf.Abs(single) > 0.1f;
		if (!flag2)
		{
			flag2 = Mathf.Abs(axisRaw) > 0.1f;
		}
		this.isMoving = flag2;
		Vector3 vector32 = (single * vector31) + (axisRaw * vector3);
		if (!flag)
		{
			if (this.jumping)
			{
				this.lockCameraTimer = (float)0;
			}
			if (this.isMoving)
			{
				this.inAirVelocity = this.inAirVelocity + ((vector32.normalized * Time.deltaTime) * this.inAirControlAcceleration);
			}
		}
		else
		{
			this.lockCameraTimer += Time.deltaTime;
			if (this.isMoving != flag1)
			{
				this.lockCameraTimer = (float)0;
			}
			if (vector32 != Vector3.zero)
			{
				if (this.moveSpeed >= this.walkSpeed * 0.9f || !flag)
				{
					this.moveDirection = Vector3.RotateTowards(this.moveDirection, vector32, this.rotateSpeed * 0.017453292f * Time.deltaTime, (float)1000);
					this.moveDirection = this.moveDirection.normalized;
				}
				else
				{
					this.moveDirection = vector32.normalized;
				}
			}
			float single1 = this.speedSmoothing * Time.deltaTime;
			float single2 = Mathf.Min(vector32.magnitude, 1f);
			this._characterState = CharacterState.Idle;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				single2 *= this.runSpeed;
				this._characterState = CharacterState.Running;
			}
			else if (Time.time - this.trotAfterSeconds <= this.walkTimeStart)
			{
				single2 *= this.walkSpeed;
				this._characterState = CharacterState.Walking;
			}
			else
			{
				single2 *= this.trotSpeed;
				this._characterState = CharacterState.Trotting;
			}
			this.moveSpeed = Mathf.Lerp(this.moveSpeed, single2, single1);
			if (this.moveSpeed < this.walkSpeed * 0.3f)
			{
				this.walkTimeStart = Time.time;
			}
		}
	}
}