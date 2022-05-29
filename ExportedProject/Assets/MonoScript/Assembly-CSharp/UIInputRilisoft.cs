using System;

public class UIInputRilisoft : UIInput
{
	public UIInputRilisoft.OnFocus onFocus;

	public UIInputRilisoft.OnFocusLost onFocusLost;

	public UIInputRilisoft()
	{
	}

	protected override void OnSelect(bool isSelected)
	{
		base.OnSelect(isSelected);
		if (isSelected && this.onFocus != null)
		{
			this.onFocus();
		}
		else if (!isSelected && this.onFocusLost != null)
		{
			this.onFocusLost();
		}
	}

	public delegate void OnFocus();

	public delegate void OnFocusLost();
}