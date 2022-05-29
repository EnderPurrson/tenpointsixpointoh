using System;
using System.Collections;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
	public bool shouldMove = true;

	public bool isCycle;

	public GameObject[] _targetCyclePoints;

	private int _targetIndex;

	private float _minDist = 5f;

	private Vector2 _spawnPoint;

	private float _lastTimeGo = -1f;

	private float _timeForIdle = 2f;

	private Vector3 _targetPoint;

	private Rect _moveBounds;

	private float halbLength = 17f;

	private float _dst;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private NavMeshAgent _nma;

	public bool ShouldMove
	{
		get
		{
			return this.shouldMove;
		}
		set
		{
			if (this.shouldMove == value)
			{
				return;
			}
			this.shouldMove = value;
			if (this.shouldMove)
			{
				this.ResetPars();
				base.SendMessage("Walk");
			}
		}
	}

	public SpawnMonster()
	{
	}

	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
		}
	}

	private void ResetPars()
	{
		this._targetIndex = 0;
		this._lastTimeGo = -1f;
	}

	private void Start()
	{
		this._nma = base.GetComponent<NavMeshAgent>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				this._modelChild = ((Transform)enumerator.Current).gameObject;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		this._soundClips = this._modelChild.GetComponent<Sounds>();
		if (!this.isCycle)
		{
			float single = base.transform.position.x;
			Vector3 vector3 = base.transform.position;
			this._spawnPoint = new Vector2(single, vector3.z);
		}
		this.ShouldMove = false;
		this.ShouldMove = true;
		this._moveBounds = new Rect(-this.halbLength, -this.halbLength, this.halbLength * 2f, this.halbLength * 2f);
	}

	private void Update()
	{
		if (!this.shouldMove)
		{
			return;
		}
		if (this._lastTimeGo <= Time.time)
		{
			this._nma.ResetPath();
			float single = -this.halbLength + UnityEngine.Random.Range(0f, this.halbLength * 2f);
			Vector3 vector3 = base.transform.position;
			this._targetPoint = new Vector3(single, vector3.y, -this.halbLength + UnityEngine.Random.Range(0f, this.halbLength * 2f));
			this._lastTimeGo = Time.time + Vector3.Distance(base.transform.position, this._targetPoint) / this._soundClips.notAttackingSpeed + this._timeForIdle;
			base.transform.LookAt(this._targetPoint);
			this._nma.SetDestination(this._targetPoint);
			this._nma.speed = this._soundClips.notAttackingSpeed;
		}
	}
}