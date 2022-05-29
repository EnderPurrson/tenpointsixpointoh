using Rilisoft;
using System;
using UnityEngine;

public class PropertyInfoScreenController : MonoBehaviour
{
	public GameObject description;

	public GameObject descriptionMelee;

	private IDisposable _escapeSubscription;

	public PropertyInfoScreenController()
	{
	}

	private void HandleEscape()
	{
		this.Hide();
	}

	public virtual void Hide()
	{
		base.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		if (this._escapeSubscription != null)
		{
			this._escapeSubscription.Dispose();
			this._escapeSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._escapeSubscription != null)
		{
			this._escapeSubscription.Dispose();
		}
		this._escapeSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Property Info");
	}

	public virtual void Show(bool isMelee)
	{
		base.gameObject.SetActive(true);
		((!isMelee ? this.description : this.descriptionMelee)).SetActive(true);
		((!isMelee ? this.descriptionMelee : this.description)).SetActive(false);
	}
}