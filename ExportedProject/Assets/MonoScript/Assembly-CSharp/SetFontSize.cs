using System;
using System.Reflection;
using UnityEngine;

public class SetFontSize : MonoBehaviour
{
	private UILabel myLabel;

	public SetFontSize()
	{
	}

	private void OnEnable()
	{
		base.Invoke("UpdateFontSize", 0.05f);
	}

	private void Start()
	{
		this.myLabel = base.GetComponent<UILabel>();
		this.UpdateFontSize();
	}

	private void Update()
	{
		if (this.myLabel.fontSize != this.myLabel.height)
		{
			this.myLabel.fontSize = this.myLabel.height;
		}
	}

	[Obfuscation(Exclude=true)]
	private void UpdateFontSize()
	{
		if (this.myLabel != null)
		{
			this.myLabel.fontSize = this.myLabel.height;
		}
	}
}