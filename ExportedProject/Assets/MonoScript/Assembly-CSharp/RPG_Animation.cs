using System;
using UnityEngine;

public class RPG_Animation : MonoBehaviour
{
	public static RPG_Animation instance;

	public RPG_Animation.CharacterMoveDirection currentMoveDir;

	public RPG_Animation.CharacterState currentState;

	public RPG_Animation()
	{
	}

	private void Awake()
	{
		RPG_Animation.instance = this;
	}

	private void Idle()
	{
		base.GetComponent<Animation>().CrossFade("idle");
	}

	public void Jump()
	{
		this.currentState = RPG_Animation.CharacterState.Jump;
		if (base.GetComponent<Animation>().IsPlaying("jump"))
		{
			base.GetComponent<Animation>().Stop("jump");
		}
		base.GetComponent<Animation>().CrossFade("jump");
	}

	public void SetCurrentMoveDir(Vector3 playerDir)
	{
		bool flag = false;
		bool flag1 = false;
		bool flag2 = false;
		bool flag3 = false;
		if (playerDir.z > 0f)
		{
			flag = true;
		}
		if (playerDir.z < 0f)
		{
			flag1 = true;
		}
		if (playerDir.x < 0f)
		{
			flag2 = true;
		}
		if (playerDir.x > 0f)
		{
			flag3 = true;
		}
		if (flag)
		{
			if (flag2)
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeForwardLeft;
			}
			else if (!flag3)
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.Forward;
			}
			else
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeForwardRight;
			}
		}
		else if (flag1)
		{
			if (flag2)
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeBackLeft;
			}
			else if (!flag3)
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.Backward;
			}
			else
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeBackRight;
			}
		}
		else if (flag2)
		{
			this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeLeft;
		}
		else if (!flag3)
		{
			this.currentMoveDir = RPG_Animation.CharacterMoveDirection.None;
		}
		else
		{
			this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeRight;
		}
	}

	public void SetCurrentState()
	{
		if (RPG_Controller.instance.characterController.isGrounded)
		{
			switch (this.currentMoveDir)
			{
				case RPG_Animation.CharacterMoveDirection.None:
				{
					this.currentState = RPG_Animation.CharacterState.Idle;
					break;
				}
				case RPG_Animation.CharacterMoveDirection.Forward:
				{
					this.currentState = RPG_Animation.CharacterState.Walk;
					break;
				}
				case RPG_Animation.CharacterMoveDirection.Backward:
				{
					this.currentState = RPG_Animation.CharacterState.WalkBack;
					break;
				}
				case RPG_Animation.CharacterMoveDirection.StrafeLeft:
				{
					this.currentState = RPG_Animation.CharacterState.StrafeLeft;
					break;
				}
				case RPG_Animation.CharacterMoveDirection.StrafeRight:
				{
					this.currentState = RPG_Animation.CharacterState.StrafeRight;
					break;
				}
				case RPG_Animation.CharacterMoveDirection.StrafeForwardLeft:
				{
					this.currentState = RPG_Animation.CharacterState.Walk;
					break;
				}
				case RPG_Animation.CharacterMoveDirection.StrafeForwardRight:
				{
					this.currentState = RPG_Animation.CharacterState.Walk;
					break;
				}
				case RPG_Animation.CharacterMoveDirection.StrafeBackLeft:
				{
					this.currentState = RPG_Animation.CharacterState.WalkBack;
					break;
				}
				case RPG_Animation.CharacterMoveDirection.StrafeBackRight:
				{
					this.currentState = RPG_Animation.CharacterState.WalkBack;
					break;
				}
			}
		}
	}

	public void StartAnimation()
	{
		switch (this.currentState)
		{
			case RPG_Animation.CharacterState.Idle:
			{
				this.Idle();
				break;
			}
			case RPG_Animation.CharacterState.Walk:
			{
				if (this.currentMoveDir == RPG_Animation.CharacterMoveDirection.StrafeForwardLeft)
				{
					this.StrafeForwardLeft();
				}
				else if (this.currentMoveDir != RPG_Animation.CharacterMoveDirection.StrafeForwardRight)
				{
					this.Walk();
				}
				else
				{
					this.StrafeForwardRight();
				}
				break;
			}
			case RPG_Animation.CharacterState.WalkBack:
			{
				if (this.currentMoveDir == RPG_Animation.CharacterMoveDirection.StrafeBackLeft)
				{
					this.StrafeBackLeft();
				}
				else if (this.currentMoveDir != RPG_Animation.CharacterMoveDirection.StrafeBackRight)
				{
					this.WalkBack();
				}
				else
				{
					this.StrafeBackRight();
				}
				break;
			}
			case RPG_Animation.CharacterState.StrafeLeft:
			{
				this.StrafeLeft();
				break;
			}
			case RPG_Animation.CharacterState.StrafeRight:
			{
				this.StrafeRight();
				break;
			}
		}
	}

	private void StrafeBackLeft()
	{
		base.GetComponent<Animation>().CrossFade("strafebackleft");
	}

	private void StrafeBackRight()
	{
		base.GetComponent<Animation>().CrossFade("strafebackright");
	}

	private void StrafeForwardLeft()
	{
		base.GetComponent<Animation>().CrossFade("strafeforwardleft");
	}

	private void StrafeForwardRight()
	{
		base.GetComponent<Animation>().CrossFade("strafeforwardright");
	}

	private void StrafeLeft()
	{
		base.GetComponent<Animation>().CrossFade("strafeleft");
	}

	private void StrafeRight()
	{
		base.GetComponent<Animation>().CrossFade("straferight");
	}

	private void Update()
	{
		this.SetCurrentState();
		this.StartAnimation();
	}

	private void Walk()
	{
		base.GetComponent<Animation>().CrossFade("walk");
	}

	private void WalkBack()
	{
		base.GetComponent<Animation>().CrossFade("walkback");
	}

	public enum CharacterMoveDirection
	{
		None,
		Forward,
		Backward,
		StrafeLeft,
		StrafeRight,
		StrafeForwardLeft,
		StrafeForwardRight,
		StrafeBackLeft,
		StrafeBackRight
	}

	public enum CharacterState
	{
		Idle,
		Walk,
		WalkBack,
		StrafeLeft,
		StrafeRight,
		Jump
	}
}