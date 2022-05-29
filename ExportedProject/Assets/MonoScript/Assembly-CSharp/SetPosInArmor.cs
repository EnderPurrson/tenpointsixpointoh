using System;
using UnityEngine;

public class SetPosInArmor : MonoBehaviour
{
	public Transform target;

	private Transform myTransform;

	public SetPosInArmor()
	{
	}

	public void SetPosition()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
			base.transform.rotation = this.target.rotation;
		}
	}

	private void Start()
	{
		this.myTransform = base.transform;
	}

	private void Update()
	{
		if (this.target != null)
		{
			this.SetPosition();
		}
		else if (this.myTransform.root.GetComponent<SkinName>() != null && this.myTransform.root.GetComponent<SkinName>().playerMoveC.transform.childCount > 0 && this.myTransform.root.GetComponent<SkinName>().playerMoveC.transform.GetChild(0).GetComponent<WeaponSounds>() != null)
		{
			if (!base.gameObject.name.Equals("Armor_Arm_Left"))
			{
				this.target = this.myTransform.root.GetComponent<SkinName>().playerMoveC.transform.GetChild(0).GetComponent<WeaponSounds>().RightArmorHand;
			}
			else
			{
				this.target = this.myTransform.root.GetComponent<SkinName>().playerMoveC.transform.GetChild(0).GetComponent<WeaponSounds>().LeftArmorHand;
			}
		}
	}
}