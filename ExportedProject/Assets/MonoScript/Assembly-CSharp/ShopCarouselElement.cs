using System;
using UnityEngine;

public class ShopCarouselElement : MonoBehaviour
{
	public GameObject locked;

	public Transform arrow;

	public UILabel topSeller;

	public UILabel quantity;

	public UILabel newnew;

	public bool showTS;

	public bool showNew;

	public bool showQuantity;

	public string prefabPath;

	public Vector3 baseScale;

	public Vector3 ourPosition;

	public string itemID;

	public string readableName;

	public Transform model;

	private float lastTimeUpdated;

	public Vector3 arrnoInitialPos;

	public ShopCarouselElement()
	{
	}

	private void Awake()
	{
		this.arrnoInitialPos = new Vector3(70.05f, -0.00016f, -120f);
	}

	private void HandlePotionActivated(string obj)
	{
		if (this.itemID != null && obj != null && this.itemID.Equals(obj))
		{
			UILabel uILabel = this.quantity;
			int num = Storager.getInt(GearManager.HolderQuantityForID(this.itemID), false);
			uILabel.text = string.Concat(num.ToString(), (this.itemID == null || !GearManager.HolderQuantityForID(this.itemID).Equals(GearManager.Grenade) ? string.Empty : string.Concat("/", GearManager.MaxCountForGear(GearManager.HolderQuantityForID(this.itemID)))));
		}
	}

	private void OnDestroy()
	{
		PotionsController.PotionActivated -= new Action<string>(this.HandlePotionActivated);
	}

	public void SetPos(float scaleCoef, float offset)
	{
		if (this.model != null)
		{
			this.model.localScale = this.baseScale * scaleCoef;
			this.model.localPosition = (this.ourPosition * scaleCoef) + new Vector3(offset, 0f, 0f);
		}
		if (this.arrow != null)
		{
			this.arrow.localScale = new Vector3(1f, 1f, 1f) * scaleCoef;
			this.arrow.localPosition = new Vector3(this.arrnoInitialPos.x * scaleCoef, this.arrnoInitialPos.y * scaleCoef, this.arrnoInitialPos.z) + new Vector3(offset, 0f, 0f);
		}
		if (this.locked != null)
		{
			this.locked.transform.localScale = new Vector3(1f, 1f, 1f) * scaleCoef;
			this.locked.transform.localPosition = new Vector3(0f, 0f, this.arrnoInitialPos.z) + new Vector3(offset, 0f, 0f);
		}
	}

	public void SetQuantity()
	{
		UILabel uILabel = this.quantity;
		int num = Storager.getInt(GearManager.HolderQuantityForID(this.itemID), false);
		uILabel.text = string.Concat(num.ToString(), (this.itemID == null || !GearManager.HolderQuantityForID(this.itemID).Equals(GearManager.Grenade) ? string.Empty : string.Concat("/", GearManager.MaxCountForGear(GearManager.HolderQuantityForID(this.itemID)))));
	}

	private void Start()
	{
		if (Array.IndexOf<string>(PotionsController.potions, this.itemID) >= 0)
		{
			this.quantity.gameObject.SetActive(true);
			this.HandlePotionActivated(this.itemID);
		}
		PotionsController.PotionActivated += new Action<string>(this.HandlePotionActivated);
	}
}