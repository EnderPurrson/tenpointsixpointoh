using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[Serializable]
public class SidescrollControl : MonoBehaviour
{
	public Joystick moveTouchPad;

	public Joystick jumpTouchPad;

	public float forwardSpeed;

	public float backwardSpeed;

	public float jumpSpeed;

	public float inAirMultiplier;

	private Transform thisTransform;

	private CharacterController character;

	private Vector3 velocity;

	private bool canJump;

	public SidescrollControl()
	{
		this.forwardSpeed = (float)4;
		this.backwardSpeed = (float)4;
		this.jumpSpeed = (float)16;
		this.inAirMultiplier = 0.25f;
		this.canJump = true;
	}

	public override void Main()
	{
	}

	public override void OnEndGame()
	{
		this.moveTouchPad.Disable();
		this.jumpTouchPad.Disable();
		this.enabled = false;
	}

	public override void Start()
	{
		this.thisTransform = (Transform)this.GetComponent(typeof(Transform));
		this.character = (CharacterController)this.GetComponent(typeof(CharacterController));
		GameObject gameObject = GameObject.Find("PlayerSpawn");
		if (gameObject)
		{
			this.thisTransform.position = gameObject.transform.position;
		}
	}

	public override void Update()
	{
		Vector3 vector3 = Vector3.zero;
		vector3 = (this.moveTouchPad.position.x <= (float)0 ? (Vector3.right * this.backwardSpeed) * this.moveTouchPad.position.x : (Vector3.right * this.forwardSpeed) * this.moveTouchPad.position.x);
		if (!this.character.isGrounded)
		{
			float single = this.velocity.y;
			Vector3 vector31 = Physics.gravity;
			this.velocity.y = single + vector31.y * Time.deltaTime;
			vector3.x *= this.inAirMultiplier;
		}
		else
		{
			bool flag = false;
			Joystick joystick = this.jumpTouchPad;
			if (!joystick.IsFingerDown())
			{
				this.canJump = true;
			}
			if (this.canJump && joystick.IsFingerDown())
			{
				flag = true;
				this.canJump = false;
			}
			if (flag)
			{
				this.velocity = this.character.velocity;
				this.velocity.y = this.jumpSpeed;
			}
		}
		vector3 += this.velocity;
		vector3 += Physics.gravity;
		vector3 *= Time.deltaTime;
		this.character.Move(vector3);
		if (this.character.isGrounded)
		{
			this.velocity = Vector3.zero;
		}
	}
}