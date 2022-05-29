using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggled Components")]
[ExecuteInEditMode]
[RequireComponent(typeof(UIToggle))]
public class UIToggledComponents : MonoBehaviour
{
	public List<MonoBehaviour> activate;

	public List<MonoBehaviour> deactivate;

	[HideInInspector]
	[SerializeField]
	private MonoBehaviour target;

	[HideInInspector]
	[SerializeField]
	private bool inverse;

	public UIToggledComponents()
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

	public void Toggle()
	{
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.activate[i].enabled = UIToggle.current.@value;
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				MonoBehaviour item = this.deactivate[j];
				item.enabled = !UIToggle.current.@value;
			}
		}
	}
}