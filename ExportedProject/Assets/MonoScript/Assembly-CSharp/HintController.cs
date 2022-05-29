using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal sealed class HintController : MonoBehaviour
{
	public HintObject hintObject;

	public HintController.HintItem[] hints;

	private readonly List<HintController.HintItem> hintToShow = new List<HintController.HintItem>();

	private EventDelegate showNextEvent;

	private readonly static List<HintController> _instances;

	private HintController.HintItem inShow;

	public static HintController instance
	{
		get
		{
			if (!HintController._instances.Any<HintController>())
			{
				return null;
			}
			return HintController._instances.Last<HintController>();
		}
	}

	static HintController()
	{
		HintController._instances = new List<HintController>();
	}

	public HintController()
	{
	}

	private void Awake()
	{
		HintController._instances.Add(this);
	}

	private bool CheckShowReason(HintController.HintItem hint)
	{
		HintController.ShowReason showReason = hint.showReason;
		if (showReason == HintController.ShowReason.New)
		{
			return true;
		}
		if (showReason != HintController.ShowReason.TrainingStage)
		{
			return false;
		}
		return (TrainingController.TrainingCompleted ? false : TrainingController.CompletedTrainingStage == hint.trainingStage);
	}

	public void HideCurrentHintObjectLabel()
	{
		if (this.hintToShow.Count > 0)
		{
			this.hintObject.gameObject.SetActive(false);
		}
	}

	public void HideHintByName(string name)
	{
		if (this.inShow == null || !(this.inShow.name == name))
		{
			int num = 0;
			while (num < this.hintToShow.Count)
			{
				if (this.hintToShow[num].name != name)
				{
					num++;
				}
				else
				{
					this.hintToShow.RemoveAt(num);
					break;
				}
			}
		}
		else
		{
			this.ShowNext();
		}
	}

	private void OnDestroy()
	{
		HintController._instances.Remove(this);
	}

	private void OnEnable()
	{
		base.Invoke("StartShow", 0.5f);
	}

	public void ShowCurrentHintObjectLabel()
	{
		if (this.hintToShow.Count > 0)
		{
			this.hintObject.gameObject.SetActive(true);
		}
	}

	private void ShowHint()
	{
		if (this.hintToShow.Count > 0)
		{
			HintController.HintItem item = this.hintToShow[0];
			if (this.inShow == item)
			{
				return;
			}
			if (item.hideReason == HintController.HideReason.ButtonClick)
			{
				item.targetButton = item.target.GetComponent<UIButton>();
				item.targetButton.onClick.Add(this.showNextEvent);
			}
			if (item.timeout > 0f)
			{
				base.Invoke("ShowNext", item.timeout);
			}
			if (item.buttonsToBlock != null && (int)item.buttonsToBlock.Length > 0)
			{
				item.buttonsState = new bool[(int)item.buttonsToBlock.Length];
				for (int i = 0; i < (int)item.buttonsToBlock.Length; i++)
				{
					item.buttonsState[i] = item.buttonsToBlock[i].isEnabled;
					if (item.buttonsToBlock[i] != item.target.GetComponent<UIButton>())
					{
						item.buttonsToBlock[i].isEnabled = false;
					}
				}
			}
			if (item.objectsToHide != null && (int)item.objectsToHide.Length > 0)
			{
				item.objActiveState = new bool[(int)item.objectsToHide.Length];
				for (int j = 0; j < (int)item.objectsToHide.Length; j++)
				{
					item.objActiveState[j] = item.objectsToHide[j].activeSelf;
					item.objectsToHide[j].SetActive(false);
				}
			}
			if (item.enableColliders)
			{
				item.collidersObj.SetActive(true);
			}
			if (item.indicateTarget)
			{
				if (item.targetButton == null)
				{
					item.targetButton = item.target.GetComponent<UIButton>();
				}
				if (string.IsNullOrEmpty(item.indicatedSpriteName) || !(item.targetButton != null))
				{
					item.targetSprites = item.target.GetComponentsInChildren<UISprite>();
				}
				else
				{
					item.targetSprite = item.targetButton.tweenTarget.GetComponent<UISprite>();
					item.defaultSpriteName = item.targetSprite.spriteName;
				}
			}
			this.hintObject.Show(item);
			this.inShow = item;
		}
	}

	public void ShowHintByName(string name, float time = 0)
	{
		if (this.hints == null)
		{
			return;
		}
		for (int i = 0; i < (int)this.hints.Length; i++)
		{
			if (this.hints[i].name == name && !this.hintToShow.Contains(this.hints[i]))
			{
				this.hintToShow.Add(this.hints[i]);
				if (time != 0f)
				{
					base.Invoke("ShowHint", time);
				}
				else
				{
					this.ShowHint();
				}
			}
		}
	}

	public void ShowNext()
	{
		if (this.hintToShow.Count == 0)
		{
			return;
		}
		if (this.hintToShow[0].hideReason == HintController.HideReason.ButtonClick)
		{
			this.hintToShow[0].target.GetComponent<UIButton>().onClick.Remove(this.showNextEvent);
		}
		if (this.hintToShow[0].buttonsToBlock != null && (int)this.hintToShow[0].buttonsToBlock.Length > 0)
		{
			for (int i = 0; i < (int)this.hintToShow[0].buttonsToBlock.Length; i++)
			{
				this.hintToShow[0].buttonsToBlock[i].isEnabled = this.hintToShow[0].buttonsState[i];
			}
		}
		if (this.hintToShow[0].objectsToHide != null && (int)this.hintToShow[0].objectsToHide.Length > 0)
		{
			for (int j = 0; j < (int)this.hintToShow[0].objectsToHide.Length; j++)
			{
				this.hintToShow[0].objectsToHide[j].SetActive(this.hintToShow[0].objActiveState[j]);
			}
		}
		if (this.hintToShow[0].enableColliders)
		{
			this.hintToShow[0].collidersObj.SetActive(false);
		}
		this.hintToShow.RemoveAt(0);
		this.hintObject.Hide();
		this.inShow = null;
		this.ShowHint();
	}

	private void Start()
	{
		this.showNextEvent = new EventDelegate(this, "ShowNext");
		base.Invoke("StartShow", 0.5f);
	}

	public void StartShow()
	{
		if (this.hints != null && this.hintToShow.Count == 0)
		{
			for (int i = 0; i < (int)this.hints.Length; i++)
			{
				if (this.CheckShowReason(this.hints[i]))
				{
					this.hintToShow.Add(this.hints[i]);
				}
			}
		}
		this.ShowHint();
	}

	public enum HideReason
	{
		None,
		ButtonClick
	}

	[Serializable]
	public class HintItem
	{
		public string name;

		public GameObject target;

		public string hintText;

		public Vector3 relativeHintPosition;

		public Vector3 relativeLabelPosition;

		public HintController.ShowReason showReason;

		public HintController.HideReason hideReason;

		public UIButton[] buttonsToBlock;

		public GameObject[] objectsToHide;

		[HideInInspector]
		public bool[] buttonsState;

		[HideInInspector]
		public bool[] objActiveState;

		[HideInInspector]
		public UIButton targetButton;

		[HideInInspector]
		public UISprite targetSprite;

		[HideInInspector]
		public UISprite[] targetSprites;

		public float timeout;

		public bool indicateTarget;

		public bool manualRotateArrow;

		public bool scaleTween;

		public bool showLabelByCode;

		public Vector3 manualArrowRotation;

		public string indicatedSpriteName;

		[HideInInspector]
		public string defaultSpriteName;

		public bool enableColliders;

		public GameObject collidersObj;

		public TrainingController.NewTrainingCompletedStage trainingStage;

		public string logInFlurryEvent;

		public HintItem()
		{
		}
	}

	public enum ShowReason
	{
		New,
		PlayerDay,
		PlayerSession,
		level,
		TrainingStage,
		OpenByScript
	}
}