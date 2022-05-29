using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ButOpenGift : MonoBehaviour
{
	public static ButOpenGift instance;

	public string nameShaderColor = "_Color";

	public float speedAnim = 1f;

	public Color normalColor = Color.white;

	public Color pressColor = Color.white;

	public Color animColor = Color.white;

	public Material[] allHighlightMaterial;

	public Collider colActive;

	public Collider colNormal;

	public bool _activeHighLight;

	public bool _isPressBut;

	public bool _isCanGetGift;

	public Color curColorHS;

	private NickLabelController labelOverGift;

	private bool _canShowLabel;

	public bool ActiveHighLight
	{
		get
		{
			return this._activeHighLight;
		}
		set
		{
			if (this._activeHighLight != value)
			{
				this._activeHighLight = value;
				if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
				{
					this._activeHighLight = false;
				}
				this.UpdateHUDStateGift();
			}
		}
	}

	public bool CanShowLabel
	{
		get
		{
			return this._canShowLabel;
		}
		set
		{
			this._canShowLabel = value;
			this.UpdateHUDStateGift();
		}
	}

	private bool CanTap
	{
		get
		{
			return (MainMenuController.SavedShwonLobbyLevelIsLessThanActual() || !TrainingController.TrainingCompleted || !GiftController.Instance.ActiveGift ? false : AnimationGift.instance.objGift.activeSelf);
		}
	}

	public bool IsCanGetGift
	{
		get
		{
			return this._isCanGetGift;
		}
		set
		{
			this._isCanGetGift = value;
			this.SetStateColliderActive(this._isCanGetGift);
			this.UpdateHUDStateGift();
		}
	}

	public bool IsPressBut
	{
		get
		{
			return this._isPressBut;
		}
		set
		{
			this._isPressBut = value;
			this.UpdateHUDStateGift();
		}
	}

	static ButOpenGift()
	{
	}

	public ButOpenGift()
	{
	}

	private void AnimHS1()
	{
		HOTween.Kill(this);
		HOTween.To(this, this.speedAnim, (new TweenParms()).Prop("curColorHS", this.normalColor).OnUpdate(() => this.SetColor(this.curColorHS)).OnComplete(new TweenDelegate.TweenCallback(this.AnimHS2)));
	}

	private void AnimHS2()
	{
		HOTween.Kill(this);
		HOTween.To(this, this.speedAnim, (new TweenParms()).Prop("curColorHS", this.animColor).OnUpdate(() => this.SetColor(this.curColorHS)).OnComplete(new TweenDelegate.TweenCallback(this.AnimHS1)));
	}

	private void Awake()
	{
		ButOpenGift.instance = this;
		HOTween.Init();
		this._activeHighLight = false;
		this._isPressBut = false;
		this._isCanGetGift = false;
	}

	public void CloseGift()
	{
		GiftBannerWindow.instance.CloseBannerEndAnimtion();
		this.ActiveHighLight = true;
		MainMenuController.canRotationLobbyPlayer = true;
	}

	public void HideLabelTap()
	{
		if (this.labelOverGift == null || !this.labelOverGift.gameObject.activeSelf)
		{
			return;
		}
		if (this.labelOverGift != null && this.labelOverGift.gameObject.activeSelf)
		{
			this.labelOverGift.gameObject.SetActive(false);
		}
	}

	private void OnClick()
	{
		if (this.CanTap)
		{
			GiftScroll.canReCreateSlots = true;
			ButtonClickSound.Instance.PlayClick();
			MainMenuController.canRotationLobbyPlayer = false;
			GiftBannerWindow.isForceClose = false;
			GiftBannerWindow.isActiveBanner = true;
			if (ButOpenGift.onOpen != null)
			{
				ButOpenGift.onOpen();
			}
			MainMenuController.sharedController.SaveShowPanelAndClose();
			MainMenuController.sharedController.OnShowBannerGift();
			GiftBannerWindow.instance.SetVisibleBanner(false);
			this.ActiveHighLight = false;
			this.UpdateHUDStateGift();
		}
	}

	private void OnDestroy()
	{
		ButOpenGift.instance = null;
	}

	private void OnDragOut()
	{
		this.OnPress(false);
	}

	private void OnPress(bool isDown)
	{
		if (this.CanTap)
		{
			this.IsPressBut = isDown;
			this.UpdateHUDStateGift();
		}
	}

	public void OpenGift()
	{
		GiftBannerWindow.instance.SetVisibleBanner(true);
	}

	private void SetActiveHighLight(bool val)
	{
		if (!val)
		{
			this.StopAnim();
			this.SetColor(new Color(0f, 0f, 0f, 0f));
		}
	}

	private void SetColor(Color needColor)
	{
		this.curColorHS = needColor;
		if (this.allHighlightMaterial != null)
		{
			for (int i = 0; i < (int)this.allHighlightMaterial.Length; i++)
			{
				this.allHighlightMaterial[i].SetColor(this.nameShaderColor, needColor);
			}
		}
	}

	private void SetStateAnim()
	{
		this.AnimHS2();
	}

	private void SetStateClick(bool val)
	{
		this.StopAnim();
		if (!val)
		{
			this.SetColor(this.normalColor);
		}
		else
		{
			this.SetColor(this.pressColor);
		}
	}

	private void SetStateColliderActive(bool val)
	{
		if (this.colActive)
		{
			this.colActive.enabled = val;
		}
		if (this.colNormal)
		{
			this.colNormal.enabled = !val;
		}
	}

	private void ShowLabelTap()
	{
		if (this.labelOverGift != null && this.labelOverGift.gameObject.activeSelf)
		{
			return;
		}
		if (this.labelOverGift == null)
		{
			this.labelOverGift = NickLabelStack.sharedStack.GetNextCurrentLabel();
			this.labelOverGift.StartShow(NickLabelController.TypeNickLabel.GetGift, AnimationGift.instance.transform);
		}
		if (!this.labelOverGift.gameObject.activeSelf)
		{
			this.labelOverGift.gameObject.SetActive(true);
		}
	}

	private void Start()
	{
		GiftController.Instance.TryGetData();
		this.UpdateHUDStateGift();
	}

	private void StopAnim()
	{
		HOTween.Kill(this);
	}

	public void UpdateHUDStateGift()
	{
		if (!TrainingController.TrainingCompleted || !GiftController.Instance.ActiveGift)
		{
			this.SetActiveHighLight(false);
			this.HideLabelTap();
			this._isPressBut = false;
			this._isCanGetGift = false;
			this._activeHighLight = false;
		}
		else if (!this.ActiveHighLight)
		{
			this.SetActiveHighLight(false);
			this.HideLabelTap();
		}
		else
		{
			if (this.IsPressBut)
			{
				this.SetStateClick(true);
			}
			else if (!this.IsCanGetGift)
			{
				this.SetStateClick(false);
			}
			else
			{
				this.SetStateAnim();
			}
			if (!this.IsCanGetGift || !this.CanShowLabel)
			{
				this.HideLabelTap();
			}
			else
			{
				this.ShowLabelTap();
			}
		}
	}

	public static event Action onOpen;
}