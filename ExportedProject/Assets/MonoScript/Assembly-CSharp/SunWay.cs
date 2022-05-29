using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SunWay : MonoBehaviour
{
	public float waterLevel;

	public Transform sun;

	private Vector3 directionLoolAt;

	private float distance;

	private float startDistance;

	private float startScale;

	private float startScaleX;

	public float multiplier = 1f;

	private float alpha = 1f;

	public SunWay()
	{
	}

	[DebuggerHidden]
	private IEnumerator GetDistance()
	{
		SunWay.u003cGetDistanceu003ec__Iterator1C8 variable = null;
		return variable;
	}

	private void Start()
	{
		base.StartCoroutine(this.GetDistance());
		Vector3 child = base.transform.GetChild(0).localScale;
		this.startScaleX = child.x;
	}

	private void Update()
	{
		if (NickLabelController.currentCamera && this.startDistance > 0f)
		{
			float single = NickLabelController.currentCamera.transform.position.x;
			Vector3 vector3 = NickLabelController.currentCamera.transform.position;
			Vector2 vector2 = new Vector2(single, vector3.z);
			float single1 = base.transform.position.x;
			Vector3 vector31 = base.transform.position;
			this.distance = Vector2.Distance(vector2, new Vector2(single1, vector31.z));
			Transform transforms = base.transform;
			float single2 = this.sun.position.x;
			float single3 = this.waterLevel;
			Vector3 vector32 = this.sun.position;
			transforms.position = new Vector3(single2, single3, vector32.z);
			Vector3 vector33 = NickLabelController.currentCamera.transform.position + (NickLabelController.currentCamera.transform.forward * -0.5f);
			vector33.y = base.transform.position.y;
			base.transform.LookAt(vector33);
			Transform child = base.transform.GetChild(0);
			float single4 = this.startScaleX;
			Vector3 vector34 = this.sun.transform.position;
			float single5 = single4 * (1f + Mathf.Clamp(vector34.y / 100f, 0f, 0.3f));
			float single6 = this.startScale * Mathf.Pow(this.distance / this.startDistance, this.multiplier);
			Vector3 child1 = base.transform.GetChild(0).localScale;
			child.localScale = new Vector3(single5, single6, child1.z);
			if (this.sun.position.y + 120f <= this.waterLevel)
			{
				this.alpha -= Time.deltaTime;
				base.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, Mathf.Clamp01(this.alpha));
			}
			else
			{
				Material component = base.transform.GetChild(0).GetComponent<Renderer>().material;
				float single7 = this.waterLevel + NickLabelController.currentCamera.transform.position.y;
				Vector3 vector35 = this.sun.transform.position;
				component.color = new Color(1f, 1f, 1f, 1f - Mathf.Clamp01((single7 + vector35.y / 100f) / (20f + this.waterLevel)));
				this.alpha = 1f;
			}
		}
	}
}