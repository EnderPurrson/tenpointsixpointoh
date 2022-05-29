using System;
using UnityEngine;

public class RPG_Controller : MonoBehaviour
{
	public static RPG_Controller instance;

	public CharacterController characterController;

	public float walkSpeed = 10f;

	public float turnSpeed = 2.5f;

	public float jumpHeight = 10f;

	public float gravity = 20f;

	public float fallingThreshold = -6f;

	private Vector3 playerDir;

	private Vector3 playerDirWorld;

	private Vector3 rotation = Vector3.zero;

	public RPG_Controller()
	{
	}

	private void Awake()
	{
		RPG_Controller.instance = this;
		this.characterController = base.GetComponent("CharacterController") as CharacterController;
		RPG_Camera.CameraSetup();
	}

	private void GetInput()
	{
		float single;
		float single1;
		float single2 = 0f;
		float single3 = 0f;
		if (Input.GetButton("Horizontal Strafe"))
		{
			if (Input.GetAxis("Horizontal Strafe") >= 0f)
			{
				single1 = (Input.GetAxis("Horizontal Strafe") <= 0f ? 0f : 1f);
			}
			else
			{
				single1 = -1f;
			}
			single2 = single1;
		}
		if (Input.GetButton("Vertical"))
		{
			if (Input.GetAxis("Vertical") >= 0f)
			{
				single = (Input.GetAxis("Vertical") <= 0f ? 0f : 1f);
			}
			else
			{
				single = -1f;
			}
			single3 = single;
		}
		if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
		{
			single3 = 1f;
		}
		this.playerDir = (single2 * Vector3.right) + (single3 * Vector3.forward);
		if (RPG_Animation.instance != null)
		{
			RPG_Animation.instance.SetCurrentMoveDir(this.playerDir);
		}
		if (this.characterController.isGrounded)
		{
			this.playerDirWorld = base.transform.TransformDirection(this.playerDir);
			if (Mathf.Abs(this.playerDir.x) + Mathf.Abs(this.playerDir.z) > 1f)
			{
				this.playerDirWorld.Normalize();
			}
			this.playerDirWorld *= this.walkSpeed;
			this.playerDirWorld.y = this.fallingThreshold;
			if (Input.GetButtonDown("Jump"))
			{
				this.playerDirWorld.y = this.jumpHeight;
				if (RPG_Animation.instance != null)
				{
					RPG_Animation.instance.Jump();
				}
			}
		}
		this.rotation.y = Input.GetAxis("Horizontal") * this.turnSpeed;
	}

	private void StartMotor()
	{
		ref Vector3 vector3Pointer = ref this.playerDirWorld;
		vector3Pointer.y = vector3Pointer.y - this.gravity * Time.deltaTime;
		this.characterController.Move(this.playerDirWorld * Time.deltaTime);
		base.transform.Rotate(this.rotation);
		if (!Input.GetMouseButton(0))
		{
			RPG_Camera.instance.RotateWithCharacter();
		}
	}

	private void Update()
	{
		if (Camera.main == null)
		{
			return;
		}
		if (this.characterController == null)
		{
			Debug.Log("Error: No Character Controller component found! Please add one to the GameObject which has this script attached.");
			return;
		}
		this.GetInput();
		this.StartMotor();
	}
}