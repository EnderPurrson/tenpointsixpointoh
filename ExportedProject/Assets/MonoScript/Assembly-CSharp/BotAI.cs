using System;
using UnityEngine;

[RequireComponent(typeof(BotMovement))]
public class BotAI : MonoBehaviour
{
	private bool Agression;

	private bool deaded;

	public Transform Target;

	private Transform myTransform;

	private BotMovement _motor;

	private BotHealth _eh;

	private BotTrigger _botTrigger;

	public Transform homePoint;

	public BotAI()
	{
	}

	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
		}
	}

	public void SetTarget(Transform _tgt, bool agression)
	{
		this.Agression = agression;
		this.Target = _tgt;
		this._motor.SetTarget(this.Target, agression);
	}

	private void Start()
	{
		this.Target = null;
		this._motor = base.GetComponent<BotMovement>();
		this._eh = base.GetComponent<BotHealth>();
		this.myTransform = base.transform;
		this._botTrigger = base.GetComponent<BotTrigger>();
	}

	private void Update()
	{
		if (!this._eh.getIsLife())
		{
			if (!this.deaded)
			{
				base.SendMessage("Death");
				this._botTrigger.shouldDetectPlayer = false;
				this.deaded = true;
			}
		}
	}
}