using System;
using UnityEngine;

public class AchieveBox : MonoBehaviour
{
	private Vector3 posToMove;

	private Vector3 hidePos;

	private UISprite mySprite;

	[HideInInspector]
	public bool isOpened;

	private bool toggled;

	public float speed = 300f;

	public AchieveBox()
	{
	}

	private void Awake()
	{
		this.mySprite = base.GetComponent<UISprite>();
		this.hidePos = base.transform.localPosition;
	}

	public void HideBox()
	{
		this.toggled = true;
		this.posToMove = this.hidePos;
	}

	public void ShowBox()
	{
		base.gameObject.SetActive(true);
		this.toggled = true;
		this.posToMove = this.hidePos + (Vector3.down * (float)this.mySprite.height);
	}

	private void Update()
	{
		if (!this.toggled)
		{
			return;
		}
		if (base.transform.localPosition == this.posToMove)
		{
			this.toggled = false;
			this.isOpened = !this.isOpened;
			if (!this.isOpened)
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.posToMove, this.speed * RealTime.deltaTime);
		}
	}
}