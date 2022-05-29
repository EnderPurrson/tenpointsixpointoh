using System;
using UnityEngine;

public class StarReview : MonoBehaviour
{
	[HideInInspector]
	public int numOrderStar;

	public UILabel lbNumStar;

	public GameObject objFonStar;

	public GameObject objActiveStar;

	public StarReview()
	{
	}

	private void OnClick()
	{
		ReviewHUDWindow.Instance.OnClickStarRating();
	}

	private void OnPress(bool isDown)
	{
		if (!isDown)
		{
			ReviewHUDWindow.Instance.SelectStar(null);
		}
		else
		{
			ReviewHUDWindow.Instance.SelectStar(this);
		}
	}

	public void SetActiveStar(bool val)
	{
		if (this.objActiveStar)
		{
			this.objActiveStar.SetActive(val);
		}
	}
}