using Rilisoft;
using System;
using UnityEngine;

public class PlusMinusController : MonoBehaviour
{
	public int stepValue = 1;

	public SaltedInt minValue = new SaltedInt();

	public SaltedInt maxValue = new SaltedInt();

	public SaltedInt @value = new SaltedInt();

	public GameObject plusButton;

	public GameObject minusButton;

	public UILabel countLabel;

	public UILabel headLabel;

	public PlusMinusController()
	{
	}

	private void Awake()
	{
		this.minValue.Value = 4;
		this.maxValue.Value = 8;
		this.@value.Value = 4;
	}

	private void HandleMinusButtonClicked(object sender, EventArgs e)
	{
		ref SaltedInt value = ref this.@value;
		value.Value = value.Value - this.stepValue;
		if (this.@value.Value < this.minValue.Value)
		{
			this.@value.Value = this.minValue.Value;
		}
	}

	private void HandlePlusButtonClicked(object sender, EventArgs e)
	{
		ref SaltedInt value = ref this.@value;
		value.Value = value.Value + this.stepValue;
		if (this.@value.Value > this.maxValue.Value)
		{
			this.@value.Value = this.maxValue.Value;
		}
	}

	private void Start()
	{
		if (this.plusButton != null)
		{
			ButtonHandler component = this.plusButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += new EventHandler(this.HandlePlusButtonClicked);
			}
		}
		if (this.minusButton != null)
		{
			ButtonHandler buttonHandler = this.minusButton.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.HandleMinusButtonClicked);
			}
		}
	}

	private void Update()
	{
		if (this.countLabel != null)
		{
			this.countLabel.text = this.@value.Value.ToString();
		}
	}
}