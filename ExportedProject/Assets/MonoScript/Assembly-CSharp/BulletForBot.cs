using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class BulletForBot : MonoBehaviour
{
	[NonSerialized]
	public float lifeTime;

	private float _bulletSpeed;

	private Vector3 _startPos;

	private Vector3 _endPos;

	private bool _isFrienlyFire;

	private float _startBulletTime;

	private bool doDamage = true;

	private bool _isMoveByPhysics;

	private BulletForBot.OnBulletDamageDelegate OnBulletDamage;

	public bool IsUse
	{
		get;
		private set;
	}

	public bool needDestroyByStop
	{
		get;
		set;
	}

	public BulletForBot()
	{
	}

	[DebuggerHidden]
	private IEnumerator ApplyForce(Vector3 force)
	{
		BulletForBot.u003cApplyForceu003ec__Iterator11A variable = null;
		return variable;
	}

	public void ApplyForceFroBullet(Vector3 startPos, Vector3 endPos, bool isFriendlyFire, float forceValue, Vector3 forceVector, bool doDamage)
	{
		this._isMoveByPhysics = true;
		this._isFrienlyFire = isFriendlyFire;
		this._startBulletTime = Time.realtimeSinceStartup;
		base.transform.position = startPos;
		base.transform.rotation = Quaternion.LookRotation(endPos - startPos);
		base.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		this.SetVisible(true);
		this.EnablePhysicsGravityControll(true);
		this.IsUse = true;
		this.doDamage = doDamage;
		base.StartCoroutine(this.ApplyForce(forceVector * forceValue));
	}

	private void CheckRunDamageEvent(GameObject target)
	{
		if (this.OnBulletDamage == null)
		{
			return;
		}
		if (this.doDamage)
		{
			this.OnBulletDamage(target, base.transform.position);
		}
		this.StopBullet();
	}

	private void CollisionEvent(GameObject collisionObj)
	{
		if (!this.IsUse)
		{
			return;
		}
		Transform transforms = collisionObj.transform.root;
		if (base.transform.root == transforms.transform.root)
		{
			return;
		}
		if (!this._isFrienlyFire && transforms.tag.Equals("Enemy"))
		{
			return;
		}
		if (transforms.tag.Equals("Player") || transforms.tag.Equals("Turret"))
		{
			this.CheckRunDamageEvent(transforms.gameObject);
			return;
		}
		if (!this._isFrienlyFire || !transforms.tag.Equals("Enemy"))
		{
			this.CheckRunDamageEvent(null);
			return;
		}
		this.CheckRunDamageEvent(transforms.gameObject);
	}

	private void EnablePhysicsGravityControll(bool enable)
	{
		base.GetComponent<Rigidbody>().useGravity = enable;
		base.GetComponent<Rigidbody>().isKinematic = !enable;
	}

	private void OnCollisionEnter(Collision collisionObj)
	{
		this.CollisionEvent(collisionObj.gameObject);
	}

	private void OnTriggerEnter(Collider collisionObj)
	{
		this.CollisionEvent(collisionObj.gameObject);
	}

	private void SetVisible(bool enable)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().enabled = enable;
		}
		if (base.GetComponent<ParticleSystem>() != null)
		{
			base.GetComponent<ParticleSystem>().enableEmission = enable;
		}
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = enable;
		}
	}

	public void StartBullet(Vector3 startPos, Vector3 endPos, float bulletSpeed, bool isFriendlyFire, bool doDamage)
	{
		this._isMoveByPhysics = false;
		this._startPos = startPos;
		this._endPos = endPos;
		this._isFrienlyFire = isFriendlyFire;
		this._bulletSpeed = bulletSpeed;
		base.transform.position = this._startPos;
		this.IsUse = true;
		base.transform.gameObject.SetActive(true);
		this._startBulletTime = Time.realtimeSinceStartup;
		base.transform.rotation = Quaternion.LookRotation(endPos - startPos);
		this.doDamage = doDamage;
	}

	private void StopBullet()
	{
		if (this.needDestroyByStop)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!this._isMoveByPhysics)
		{
			base.transform.gameObject.SetActive(false);
		}
		else
		{
			this.SetVisible(false);
		}
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		this.IsUse = false;
		if (this._isMoveByPhysics)
		{
			this.EnablePhysicsGravityControll(false);
		}
	}

	private void Update()
	{
		if (!this.IsUse)
		{
			return;
		}
		if (!this._isMoveByPhysics)
		{
			Vector3 vector3 = this._endPos - this._startPos;
			Transform transforms = base.transform;
			transforms.position = transforms.position + ((vector3.normalized * this._bulletSpeed) * Time.deltaTime);
		}
		if (Time.realtimeSinceStartup - this._startBulletTime >= this.lifeTime)
		{
			this.StopBullet();
		}
	}

	public event BulletForBot.OnBulletDamageDelegate OnBulletDamage
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.OnBulletDamage += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.OnBulletDamage -= value;
		}
	}

	public delegate void OnBulletDamageDelegate(GameObject targetDamage, Vector3 positionDamage);
}