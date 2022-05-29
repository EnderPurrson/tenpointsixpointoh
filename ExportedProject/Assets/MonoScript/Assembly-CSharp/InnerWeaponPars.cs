using System;
using UnityEngine;

public class InnerWeaponPars : MonoBehaviour
{
	public GameObject particlePoint;

	public Transform LeftArmorHand;

	public Transform RightArmorHand;

	public Transform grenatePoint;

	public AudioClip shoot;

	public AudioClip reload;

	public AudioClip empty;

	public AudioClip idle;

	public AudioClip zoomIn;

	public AudioClip zoomOut;

	public AudioClip charge;

	public GameObject bonusPrefab;

	public GameObject animationObject;

	public Texture preview;

	public Texture2D aimTextureV;

	public Texture2D aimTextureH;

	private SkinnedMeshRenderer renderArms;

	public InnerWeaponPars()
	{
	}

	private void Awake()
	{
		this.FindArms();
	}

	private void FindArms()
	{
		if (this.renderArms != null)
		{
			return;
		}
		SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i];
				if (skinnedMeshRenderer != null && skinnedMeshRenderer.gameObject != this.bonusPrefab)
				{
					this.renderArms = skinnedMeshRenderer;
					return;
				}
			}
		}
	}

	public void SetMaterialForArms(Material shMat)
	{
		if (this.renderArms == null)
		{
			this.FindArms();
		}
		if (this.renderArms != null)
		{
			this.renderArms.sharedMaterial = shMat;
		}
	}
}