using System;
using UnityEngine;

public sealed class PlayerDeadController : MonoBehaviour
{
	private float liveTime = -1f;

	private float maxliveTime = 4.8f;

	public bool isUseMine;

	private Transform myTransform;

	public Animation myAnimation;

	public GameObject[] playerDeads;

	public DeadEnergyController deadEnergyController;

	public DeadExplosionController deadExplosionController;

	public PlayerDeadController()
	{
	}

	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
	}

	public void StartShow(Vector3 pos, Quaternion rot, int _typeDead, bool _isUseMine, Texture _skin)
	{
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		if (_typeDead == 1)
		{
			this.playerDeads[1].SetActive(true);
			this.TryPlayAudioClip(this.playerDeads[1]);
			this.deadExplosionController.StartAnim();
		}
		else if (_typeDead < 2 || _typeDead > 9)
		{
			this.playerDeads[0].SetActive(true);
		}
		else
		{
			this.playerDeads[2].SetActive(true);
			this.TryPlayAudioClip(this.playerDeads[2]);
			Color color = new Color(0f, 0.5f, 1f);
			if (_typeDead == 3)
			{
				color = new Color(1f, 0f, 0f);
			}
			if (_typeDead == 4)
			{
				color = new Color(1f, 0f, 0f);
			}
			if (_typeDead == 4)
			{
				color = new Color(1f, 0f, 1f);
			}
			if (_typeDead == 5)
			{
				color = new Color(0f, 0.5f, 1f);
			}
			if (_typeDead == 6)
			{
				color = new Color(1f, 0.91f, 0f);
			}
			if (_typeDead == 7)
			{
				color = new Color(0f, 0.85f, 0f);
			}
			if (_typeDead == 8)
			{
				color = new Color(1f, 0.55f, 0f);
			}
			if (_typeDead == 9)
			{
				color = new Color(1f, 1f, 1f);
			}
			this.deadEnergyController.StartAnim(color, _skin);
		}
	}

	private void TryPlayAudioClip(GameObject obj)
	{
		if (!Defs.isSoundFX)
		{
			return;
		}
		AudioSource component = obj.GetComponent<AudioSource>();
		if (component == null)
		{
			return;
		}
		component.Play();
	}

	private void Update()
	{
		if (this.liveTime < 0f)
		{
			return;
		}
		this.liveTime -= Time.deltaTime;
		if (this.liveTime < 0f)
		{
			this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
			this.playerDeads[0].SetActive(false);
			if (!Device.isPixelGunLow)
			{
				this.playerDeads[1].SetActive(false);
				this.playerDeads[2].SetActive(false);
			}
			this.isUseMine = false;
		}
	}
}