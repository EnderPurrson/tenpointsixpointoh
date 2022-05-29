using System;
using UnityEngine;

public class AnchorMover : MonoBehaviour
{
	public Transform flag1;

	public Transform flag2;

	public AnchorMover()
	{
	}

	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= new Action(this.SetSide);
	}

	private void OnEnable()
	{
		this.SetSideCoroutine();
	}

	private void SetSide()
	{
		this.SetSideCoroutine();
	}

	private void SetSideCoroutine()
	{
		object obj;
		object obj1;
		Transform vector3 = this.flag1;
		obj = (!GlobalGameController.LeftHanded ? 1 : -1);
		float single = this.flag1.localPosition.y;
		Vector3 vector31 = this.flag1.localPosition;
		vector3.localPosition = new Vector3((float)obj * ((float)Screen.width * 768f / (float)Screen.height / 2f - 30f), single, vector31.z);
		Transform transforms = this.flag2;
		obj1 = (!GlobalGameController.LeftHanded ? 1 : -1);
		float single1 = this.flag2.localPosition.y;
		Vector3 vector32 = this.flag2.localPosition;
		transforms.localPosition = new Vector3((float)obj1 * ((float)Screen.width * 768f / (float)Screen.height / 2f - 30f), single1, vector32.z);
	}

	private void Start()
	{
		this.SetSide();
		PauseNGUIController.PlayerHandUpdated += new Action(this.SetSide);
	}
}