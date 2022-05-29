using System;
using UnityEngine;

[ExecuteInEditMode]
public class HintedObject : MonoBehaviour
{
	public int fontSize = 20;

	public string term = "hint";

	public float timeToShowHint = 0.2f;

	public MenuHintObject hintObj;

	public Vector3 position;

	public HintedObject.ArrowPos arrowPos;

	public bool showOnPress;

	public bool preview;

	private float timer;

	private bool press;

	private Transform tempTransform;

	private bool isShowing;

	public HintedObject()
	{
	}

	public void CloseHint()
	{
		this.isShowing = false;
		this.hintObj.gameObject.SetActive(false);
		if (Application.isPlaying)
		{
			this.hintObj.tween.ResetToBeginning();
		}
		this.timer = this.timeToShowHint;
		this.hintObj.body.transform.parent = this.hintObj.transform;
	}

	private void OnPress(bool pressed)
	{
		this.timer = this.timeToShowHint;
		this.press = pressed;
		if (!pressed && this.hintObj.isActiveAndEnabled)
		{
			this.CloseHint();
		}
	}

	public void ShowHint()
	{
		this.isShowing = true;
		this.hintObj.gameObject.SetActive(true);
		this.hintObj.body.transform.parent = base.transform;
		this.hintObj.botRightArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.botRight);
		this.hintObj.botCenterArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.botCenter);
		this.hintObj.botLeftArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.botLeft);
		this.hintObj.leftBotArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.leftBot);
		this.hintObj.leftCenterArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.leftCenter);
		this.hintObj.leftTopArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.leftTop);
		this.hintObj.rightTopArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.rightTop);
		this.hintObj.rightCenterArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.rightCenter);
		this.hintObj.rightBotArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.rightBot);
		this.hintObj.topLeftArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.topLeft);
		this.hintObj.topCenterArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.topCenter);
		this.hintObj.topRightArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.topRight);
		this.hintObj.label.text = LocalizationStore.Get(this.term);
		this.hintObj.label.fontSize = this.fontSize;
		this.hintObj.label.transform.localPosition = new Vector3(0f, 0f);
		this.hintObj.body.transform.localPosition = this.position;
		if (Application.isPlaying)
		{
			this.hintObj.tween.PlayForward();
		}
		if (this.arrowPos == HintedObject.ArrowPos.leftTop || this.arrowPos == HintedObject.ArrowPos.rightTop || this.arrowPos == HintedObject.ArrowPos.topCenter || this.arrowPos == HintedObject.ArrowPos.topLeft || this.arrowPos == HintedObject.ArrowPos.topRight)
		{
			this.hintObj.label.pivot = UIWidget.Pivot.TopRight;
		}
		else if (this.arrowPos == HintedObject.ArrowPos.leftCenter || this.arrowPos == HintedObject.ArrowPos.rightCenter)
		{
			this.hintObj.label.pivot = UIWidget.Pivot.Right;
		}
		else
		{
			this.hintObj.label.pivot = UIWidget.Pivot.BottomRight;
		}
	}

	private void Update()
	{
		if (this.press && this.showOnPress)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.ShowHint();
			}
		}
		if (this.isShowing && this.showOnPress && !this.press)
		{
			this.CloseHint();
		}
		if (!Application.isPlaying)
		{
			if (this.preview)
			{
				this.ShowHint();
			}
			if (this.isShowing && !this.preview)
			{
				this.CloseHint();
			}
		}
	}

	public enum ArrowPos
	{
		botRight,
		botCenter,
		botLeft,
		leftBot,
		leftCenter,
		leftTop,
		rightTop,
		rightCenter,
		rightBot,
		topLeft,
		topCenter,
		topRight
	}
}