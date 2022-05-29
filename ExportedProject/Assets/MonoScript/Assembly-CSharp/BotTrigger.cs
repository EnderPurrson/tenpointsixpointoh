using System;
using System.Collections;
using UnityEngine;

public class BotTrigger : MonoBehaviour
{
	public bool shouldDetectPlayer = true;

	private bool _entered;

	private BotAI _eai;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private Transform myTransform;

	public BotTrigger()
	{
	}

	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
		}
	}

	private void Start()
	{
		this.myTransform = base.transform;
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
		this._eai = base.GetComponent<BotAI>();
		this._player = GameObject.FindGameObjectWithTag("Player");
		if (this._player != null)
		{
			this._playerMoveC = this._player.GetComponent<SkinName>().playerMoveC;
		}
	}

	private void Update()
	{
		if (!this.shouldDetectPlayer)
		{
			return;
		}
		if (!this._entered)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Turret");
			if (gameObject != null && gameObject.GetComponent<TurretController>() != null && (gameObject.GetComponent<TurretController>().isKilled || !gameObject.GetComponent<TurretController>().isRun))
			{
				gameObject = null;
			}
			float single = (gameObject == null ? 1E+09f : Vector3.Distance(this.myTransform.position, gameObject.transform.position));
			bool flag = (gameObject == null ? false : single <= this._soundClips.detectRadius);
			float single1 = Vector3.Distance(this.myTransform.position, this._player.transform.position);
			bool flag1 = (this._playerMoveC.isInvisible ? false : single1 <= this._soundClips.detectRadius);
			Transform transforms = null;
			if (!flag1 || !flag)
			{
				if (flag1)
				{
					transforms = this._player.transform;
				}
				if (flag)
				{
					transforms = gameObject.transform;
				}
			}
			else
			{
				transforms = (single1 >= single ? gameObject.transform : this._player.transform);
			}
			if (transforms != null)
			{
				this._eai.SetTarget(transforms, true);
				this._entered = true;
			}
		}
		else if (this._eai.Target == null || this._eai.Target.CompareTag("Player") && (this._playerMoveC.isInvisible || this._entered && Vector3.SqrMagnitude(base.transform.position - this._player.transform.position) > this._soundClips.detectRadius * this._soundClips.detectRadius) || this._eai.Target.CompareTag("Turret") && this._eai.Target.GetComponent<TurretController>().isKilled && this._eai.Target.GetComponent<TurretController>().isRun)
		{
			this._eai.SetTarget(null, false);
			this._entered = false;
		}
	}
}