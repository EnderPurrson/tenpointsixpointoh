using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimationGift : MonoBehaviour
{
	public static AnimationGift instance;

	public GameObject objGift;

	public AudioClip soundOpen;

	public AudioClip soundClose;

	public AudioClip soundGetGift;

	private Animator _animator;

	private float timeoutShowGift = 2f;

	public AnimationGift()
	{
	}

	private void Awake()
	{
		AnimationGift.instance = this;
		this._animator = this.objGift.GetComponent<Animator>();
		this.SetVisibleObjGift(false);
	}

	private void ChangeActiveMainMenu(bool val)
	{
		if (ButOpenGift.instance != null)
		{
			if (!val)
			{
				ButOpenGift.instance.HideLabelTap();
			}
			else
			{
				ButOpenGift.instance.UpdateHUDStateGift();
			}
		}
	}

	private void ChangeLayer(string nameLayer)
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = LayerMask.NameToLayer(nameLayer);
		}
	}

	public void CheckStateGift()
	{
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.IsCanGetGift = GiftController.Instance.CanGetTimerGift;
		}
		if (this._animator != null)
		{
			this._animator.SetBool("IsEnabled", GiftController.Instance.CanGetTimerGift);
		}
	}

	public void CheckVisibleGift()
	{
		if (!TrainingController.TrainingCompleted || !GiftController.Instance.ActiveGift)
		{
			this.SetVisibleObjGift(false);
		}
		else
		{
			this.SetVisibleObjGift(true);
			this.CheckStateGift();
		}
	}

	public void CloseGift()
	{
		if (this._animator != null)
		{
			this._animator.SetBool("IsOpen", false);
		}
		if (Defs.isSoundFX && base.GetComponent<AudioSource>() && this.soundClose)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.soundClose);
		}
	}

	private void EventCloseEndCam()
	{
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.CloseGift();
		}
	}

	private void EventOpenEndCam()
	{
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.OpenGift();
		}
	}

	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			base.Invoke("CheckVisibleGift", 0.1f);
		}
	}

	private void OnDetstroy()
	{
		AnimationGift.instance = null;
	}

	private void OnDisable()
	{
		GiftBannerWindow.onHideInfoGift -= new Action(this.CloseGift);
		GiftBannerWindow.onGetGift -= new Action(this.OpenGift);
		GiftBannerWindow.onHideInfoGift -= new Action(this.CheckStateGift);
		GiftBannerWindow.onOpenInfoGift -= new Action(this.OnOpenInfoGift);
		GiftController.OnTimerEnded -= new Action(this.CheckStateGift);
		FriendsController.ServerTimeUpdated -= new Action(this.CheckVisibleGift);
		GiftController.OnChangeSlots -= new Action(this.CheckVisibleGift);
		MainMenuController.onLoadMenu -= new Action(this.OnLoadMenu);
		TrainingController.onChangeTraining -= new Action(this.CheckVisibleGift);
		MainMenuHeroCamera.onEndOpenGift -= new Action(this.EventOpenEndCam);
		MainMenuHeroCamera.onEndCloseGift -= new Action(this.EventCloseEndCam);
		MainMenuController.onActiveMainMenu -= new Action<bool>(this.ChangeActiveMainMenu);
	}

	private void OnEnable()
	{
		GiftBannerWindow.onGetGift += new Action(this.OpenGift);
		GiftBannerWindow.onHideInfoGift += new Action(this.CloseGift);
		GiftBannerWindow.onHideInfoGift += new Action(this.CheckStateGift);
		GiftBannerWindow.onOpenInfoGift += new Action(this.OnOpenInfoGift);
		GiftController.OnTimerEnded += new Action(this.CheckStateGift);
		GiftController.OnChangeSlots += new Action(this.CheckVisibleGift);
		MainMenuController.onLoadMenu += new Action(this.OnLoadMenu);
		TrainingController.onChangeTraining += new Action(this.CheckVisibleGift);
		FriendsController.ServerTimeUpdated += new Action(this.CheckVisibleGift);
		MainMenuHeroCamera.onEndOpenGift += new Action(this.EventOpenEndCam);
		MainMenuHeroCamera.onEndCloseGift += new Action(this.EventCloseEndCam);
		MainMenuController.onActiveMainMenu += new Action<bool>(this.ChangeActiveMainMenu);
	}

	private void OnLoadMenu()
	{
		this.CheckVisibleGift();
	}

	public void OnOpenInfoGift()
	{
		if (this._animator != null)
		{
			this._animator.SetBool("IsOpen", true);
		}
		if (Defs.isSoundFX && base.GetComponent<AudioSource>() && this.soundGetGift)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.soundGetGift);
		}
	}

	public void OpenGift()
	{
		base.StartCoroutine(this.WaitOpenGift());
		if (Defs.isSoundFX && base.GetComponent<AudioSource>() && this.soundOpen)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.soundOpen);
		}
		TutorialQuestManager.Instance.AddFulfilledQuest("getGotcha");
		QuestMediator.NotifyGetGotcha();
	}

	public void ResetAnimation()
	{
		if (this._animator != null)
		{
			this._animator.SetBool("IsOpen", false);
		}
		base.StopCoroutine("WaitOpenGift");
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.CanShowLabel = false;
		}
	}

	private void SetVisibleObjGift(bool val)
	{
		if (this.objGift.activeSelf == val)
		{
			return;
		}
		if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow && GiftBannerWindow.instance.curStateAnimAward != GiftBannerWindow.StepAnimation.none)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted || GiftController.Instance == null || !GiftController.Instance.ActiveGift)
		{
			val = false;
		}
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.ActiveHighLight = val;
		}
		this.objGift.SetActive(val);
		if (!val)
		{
			if (ButOpenGift.instance != null)
			{
				ButOpenGift.instance.CanShowLabel = false;
			}
			if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.bannerObj.activeInHierarchy)
			{
				GiftBannerWindow.instance.ForceCloseAll();
			}
		}
		else
		{
			base.Invoke("WaitEndAnimShow", 1f);
		}
	}

	public void StartAnimForGetGift()
	{
		if (this._animator != null)
		{
			this._animator.SetBool("IsEnabled", true);
		}
	}

	public void StopAnimForGetGift()
	{
		this.CheckStateGift();
	}

	private void WaitEndAnimShow()
	{
		base.CancelInvoke("WaitEndAnimShow");
		if (ButOpenGift.instance != null)
		{
			ButOpenGift.instance.CanShowLabel = true;
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitOpenGift()
	{
		AnimationGift.u003cWaitOpenGiftu003ec__Iterator144 variable = null;
		return variable;
	}

	public static event Action onEndAnimOpen;
}