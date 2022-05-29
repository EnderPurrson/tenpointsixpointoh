using System;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{
	public Transform t_target;

	public LookAtScript()
	{
	}

	private void Update()
	{
		base.transform.LookAt(this.t_target);
	}
}