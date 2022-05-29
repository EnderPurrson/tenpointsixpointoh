using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
	public List<GameObject> activate;

	public List<GameObject> deactivate;

	[HideInInspector]
	[SerializeField]
	private GameObject target;

	[HideInInspector]
	[SerializeField]
	private bool inverse;

	public UIToggledObjects()
	{
	}

	private void Awake()
	{
		if (this.target != null)
		{
			if (this.activate.Count != 0 || this.deactivate.Count != 0)
			{
				this.target = null;
			}
			else if (!this.inverse)
			{
				this.activate.Add(this.target);
			}
			else
			{
				this.deactivate.Add(this.target);
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.Toggle));
	}

	private void Set(GameObject go, bool state)
	{
		if (go != null)
		{
			NGUITools.SetActive(go, state);
		}
	}

	public void Toggle()
	{
		bool flag = UIToggle.current.@value;
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.Set(this.activate[i], flag);
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				this.Set(this.deactivate[j], !flag);
			}
		}
	}
}