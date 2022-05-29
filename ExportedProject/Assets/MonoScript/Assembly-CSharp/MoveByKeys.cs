using Photon;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class MoveByKeys : Photon.MonoBehaviour
{
	public float Speed = 10f;

	public float JumpForce = 200f;

	public float JumpTimeout = 0.5f;

	private bool isSprite;

	private float jumpingTime;

	private Rigidbody body;

	private Rigidbody2D body2d;

	public MoveByKeys()
	{
	}

	public void FixedUpdate()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		if (Input.GetAxisRaw("Horizontal") < -0.1f || Input.GetAxisRaw("Horizontal") > 0.1f)
		{
			Transform speed = base.transform;
			speed.position = speed.position + ((Vector3.right * (this.Speed * Time.deltaTime)) * Input.GetAxisRaw("Horizontal"));
		}
		if (this.jumpingTime > 0f)
		{
			this.jumpingTime -= Time.deltaTime;
		}
		else if ((this.body != null || this.body2d != null) && Input.GetKey(KeyCode.Space))
		{
			this.jumpingTime = this.JumpTimeout;
			Vector2 jumpForce = Vector2.up * this.JumpForce;
			if (this.body2d != null)
			{
				this.body2d.AddForce(jumpForce);
			}
			else if (this.body != null)
			{
				this.body.AddForce(jumpForce);
			}
		}
		if (!this.isSprite && (Input.GetAxisRaw("Vertical") < -0.1f || Input.GetAxisRaw("Vertical") > 0.1f))
		{
			Transform transforms = base.transform;
			transforms.position = transforms.position + ((Vector3.forward * (this.Speed * Time.deltaTime)) * Input.GetAxisRaw("Vertical"));
		}
	}

	public void Start()
	{
		this.isSprite = base.GetComponent<SpriteRenderer>() != null;
		this.body2d = base.GetComponent<Rigidbody2D>();
		this.body = base.GetComponent<Rigidbody>();
	}
}