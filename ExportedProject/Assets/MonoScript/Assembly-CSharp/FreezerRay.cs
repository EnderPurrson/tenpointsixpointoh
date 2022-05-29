using System;
using UnityEngine;

public class FreezerRay : MonoBehaviour
{
	private Player_move_c mc;

	public float lifetime = 0.1f;

	public float timeLeft;

	public float Length
	{
		set
		{
			base.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, value));
		}
	}

	public FreezerRay()
	{
	}

	private void HandleFreezerFired(float length)
	{
		this.timeLeft += this.lifetime;
		this.Length = length;
	}

	private void OnDisable()
	{
		if (this.mc != null)
		{
			this.mc.FreezerFired -= new Action<float>(this.HandleFreezerFired);
		}
	}

	private void OnEnable()
	{
		this.timeLeft += this.lifetime;
	}

	public void SetParentMoveC(Player_move_c move_c)
	{
		this.mc = move_c;
		if (this.mc != null)
		{
			this.mc.FreezerFired += new Action<float>(this.HandleFreezerFired);
		}
	}

	private void Update()
	{
		this.timeLeft -= Time.deltaTime;
		if (this.timeLeft <= 0f)
		{
			base.GetComponent<RayAndExplosionsStackItem>().Deactivate();
			return;
		}
		Transform transforms = null;
		if (this.mc != null && this.mc.transform.childCount > 0)
		{
			Transform child = this.mc.transform.GetChild(0);
			FlashFire component = child.GetComponent<FlashFire>();
			if (component != null && component.gunFlashObj != null)
			{
				transforms = component.gunFlashObj.transform;
			}
		}
		if (this.mc != null && transforms != null && transforms.parent != null && transforms.parent.parent != null)
		{
			base.transform.position = transforms.parent.position;
			base.transform.forward = transforms.parent.parent.forward;
		}
	}
}