using Rilisoft;
using System;
using UnityEngine;

public class TeamNumberOfPlayer : MonoBehaviour
{
	public int @value;

	public GameObject button2x2;

	public GameObject button3x3;

	public GameObject button4x4;

	public GameObject button5x5;

	private int oldValue = 8;

	public TeamNumberOfPlayer()
	{
	}

	private void HandleButton2x2Clicked(object sender, EventArgs e)
	{
		this.SetValue(4);
	}

	private void HandleButton3x3Clicked(object sender, EventArgs e)
	{
		this.SetValue(6);
	}

	private void HandleButton4x4Clicked(object sender, EventArgs e)
	{
		this.SetValue(8);
	}

	private void HandleButton5x5Clicked(object sender, EventArgs e)
	{
		this.SetValue(10);
	}

	public void SetValue(int _value)
	{
		this.@value = _value;
		this.button2x2.GetComponent<UIButton>().isEnabled = this.@value != 4;
		this.button3x3.GetComponent<UIButton>().isEnabled = this.@value != 6;
		this.button4x4.GetComponent<UIButton>().isEnabled = this.@value != 8;
		this.button5x5.GetComponent<UIButton>().isEnabled = this.@value != 10;
	}

	private void Start()
	{
		if (this.button2x2 != null)
		{
			ButtonHandler component = this.button2x2.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += new EventHandler(this.HandleButton2x2Clicked);
			}
		}
		if (this.button3x3 != null)
		{
			ButtonHandler buttonHandler = this.button3x3.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.HandleButton3x3Clicked);
			}
		}
		if (this.button4x4 != null)
		{
			ButtonHandler component1 = this.button4x4.GetComponent<ButtonHandler>();
			if (component1 != null)
			{
				component1.Clicked += new EventHandler(this.HandleButton4x4Clicked);
			}
		}
		if (this.button5x5 != null)
		{
			ButtonHandler buttonHandler1 = this.button5x5.GetComponent<ButtonHandler>();
			if (buttonHandler1 != null)
			{
				buttonHandler1.Clicked += new EventHandler(this.HandleButton5x5Clicked);
			}
		}
		this.@value = 10;
		this.button2x2.GetComponent<UIButton>().isEnabled = true;
		this.button3x3.GetComponent<UIButton>().isEnabled = true;
		this.button4x4.GetComponent<UIButton>().isEnabled = true;
		this.button5x5.GetComponent<UIButton>().isEnabled = false;
	}
}