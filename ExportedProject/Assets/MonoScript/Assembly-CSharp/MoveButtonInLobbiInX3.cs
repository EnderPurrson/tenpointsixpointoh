using System;
using UnityEngine;

public class MoveButtonInLobbiInX3 : MonoBehaviour
{
	public float yX3;

	private Transform myTransform;

	private float yNotX3;

	private bool oldStateX3;

	public MoveButtonInLobbiInX3()
	{
	}

	private void Move()
	{
		float single;
		if (this.oldStateX3 != PromoActionsManager.sharedManager.IsEventX3Active)
		{
			this.oldStateX3 = PromoActionsManager.sharedManager.IsEventX3Active;
			Transform vector3 = this.myTransform;
			float single1 = this.myTransform.localPosition.x;
			single = (!this.oldStateX3 ? this.yNotX3 : this.yX3);
			Vector3 vector31 = this.myTransform.localPosition;
			vector3.localPosition = new Vector3(single1, single, vector31.z);
		}
	}

	private void Start()
	{
		this.myTransform = base.transform;
		this.yNotX3 = this.myTransform.localPosition.y;
		this.Move();
	}

	private void Update()
	{
		this.Move();
	}
}