using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ReviewHUDWindow : MonoBehaviour
{
	private static ReviewHUDWindow _instance;

	[Header("Добавить все звезды в список в их порядке активации при нажатии")]
	public StarReview[] arrStarByOrder;

	[Header("Окна")]
	public GameObject objWindowRating;

	public GameObject objWindowGoToStore;

	public GameObject objWindowEnterMsg;

	public GameObject objThanks;

	[Header("Другое")]
	public UIInput inputMsg;

	public UIButton btnSendMsg;

	public UILabel lbTitle5Stars;

	public static bool isShow;

	private bool _NeedShowThanks;

	public int countStarForReview;

	private bool isInputMsgForReview;

	private IDisposable _backSubscription;

	public static ReviewHUDWindow Instance
	{
		get
		{
			if (ReviewHUDWindow._instance != null)
			{
				return ReviewHUDWindow._instance;
			}
			ReviewHUDWindow._instance = InfoWindowController.Instance.gameObject.GetComponentInChildren<ReviewHUDWindow>();
			return ReviewHUDWindow._instance;
		}
	}

	public bool NeedShowThanks
	{
		get
		{
			return this._NeedShowThanks;
		}
		set
		{
			this._NeedShowThanks = value;
		}
	}

	public string TitleTextTranslate
	{
		get
		{
			return string.Empty;
		}
	}

	static ReviewHUDWindow()
	{
	}

	public ReviewHUDWindow()
	{
	}

	private void AddBackSubscription()
	{
		if (this._backSubscription == null)
		{
			this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClickClose), "Review HUD (Window with 5 stars)");
		}
	}

	private void Awake()
	{
		ReviewHUDWindow._instance = this;
		if (this.arrStarByOrder != null)
		{
			for (int i = 0; i < (int)this.arrStarByOrder.Length; i++)
			{
				this.arrStarByOrder[i].numOrderStar = i;
				int num = i + 1;
				this.arrStarByOrder[i].lbNumStar.text = num.ToString();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator Crt_OnShowThanks()
	{
		ReviewHUDWindow.u003cCrt_OnShowThanksu003ec__Iterator188 variable = null;
		return variable;
	}

	[ContextMenu("Find all stars")]
	private void FindStars()
	{
		this.arrStarByOrder = base.GetComponentsInChildren<StarReview>(true);
	}

	public void OnChangeMsgReview()
	{
		this.UpdateStateBtnSendMsg(true);
	}

	public void OnClickClose()
	{
		ReviewHUDWindow.isShow = false;
		if (this.countStarForReview != 0)
		{
			FlurryEvents.LogRateUsFake(true, this.countStarForReview, false);
		}
		else
		{
			FlurryEvents.LogRateUsFake(false, 0, false);
		}
		AnalyticsStuff.RateUsFake(this.countStarForReview != 0, this.countStarForReview, false);
		this.SendMsgReview(false);
	}

	public void OnClickStarRating()
	{
		if (this.countStarForReview <= 0 || this.countStarForReview > 4)
		{
			this.SendMsgReview(true);
		}
		else
		{
			this.OnShowWindowEnterMessage();
		}
	}

	private void OnCloseAllWindow()
	{
		ReviewHUDWindow.isShow = false;
		if (this.objWindowRating)
		{
			this.objWindowRating.SetActive(false);
		}
		if (this.objWindowEnterMsg)
		{
			this.objWindowEnterMsg.SetActive(false);
		}
		if (this.objWindowGoToStore)
		{
			this.objWindowGoToStore.SetActive(false);
		}
		if (this.objThanks)
		{
			this.objThanks.SetActive(false);
		}
		this.RemoveBackSubscription();
	}

	public void OnCloseThanks()
	{
		if (this.objThanks != null)
		{
			this.objThanks.SetActive(false);
		}
		BannerWindowController.firstScreen = false;
		ReviewHUDWindow.isShow = false;
		this.RemoveBackSubscription();
	}

	private void OnDestroy()
	{
		ReviewHUDWindow._instance = null;
	}

	private void OnEnable()
	{
		this.OnShowThanks();
	}

	public void OnSendMsgWithRating()
	{
		this.SendMsgReview(true);
	}

	private void OnShowThanks()
	{
		if (this.NeedShowThanks)
		{
			base.StartCoroutine(this.Crt_OnShowThanks());
		}
	}

	private void OnShowWidowRating()
	{
		ReviewHUDWindow.isShow = true;
		this.countStarForReview = 0;
		this.SelectStar(null);
		ReviewController.IsSendReview = true;
		ReviewController.IsNeedActive = false;
		if (this.lbTitle5Stars)
		{
			this.lbTitle5Stars.text = this.TitleTextTranslate;
		}
		if (this.objWindowRating)
		{
			this.objWindowRating.SetActive(true);
		}
		if (this.objWindowEnterMsg)
		{
			this.objWindowEnterMsg.SetActive(false);
		}
		if (this.objWindowGoToStore)
		{
			this.objWindowGoToStore.SetActive(false);
		}
		this.AddBackSubscription();
	}

	private void OnShowWindowEnterMessage()
	{
		this.UpdateStateBtnSendMsg(false);
		if (this.objWindowRating)
		{
			this.objWindowRating.SetActive(false);
		}
		if (this.objWindowEnterMsg)
		{
			this.objWindowEnterMsg.SetActive(true);
		}
	}

	private void OnShowWindowGoToStore()
	{
		if (this.objWindowRating)
		{
			this.objWindowRating.SetActive(false);
		}
		if (this.objWindowEnterMsg)
		{
			this.objWindowGoToStore.SetActive(true);
		}
	}

	private void RemoveBackSubscription()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	public void SelectStar(StarReview curStar)
	{
		if (curStar != null)
		{
			this.countStarForReview = curStar.numOrderStar + 1;
		}
		if (this.arrStarByOrder != null)
		{
			for (int i = 0; i < (int)this.arrStarByOrder.Length; i++)
			{
				if (!(curStar != null) || i > curStar.numOrderStar)
				{
					this.arrStarByOrder[i].SetActiveStar(false);
				}
				else
				{
					this.arrStarByOrder[i].SetActiveStar(true);
				}
			}
		}
	}

	private void SendMsgReview(bool isClickSend = true)
	{
		this.OnCloseAllWindow();
		if (this.countStarForReview > 0)
		{
			string empty = string.Empty;
			if (this.isInputMsgForReview)
			{
				empty = this.inputMsg.@value;
			}
			if (this.countStarForReview != 5)
			{
				FlurryEvents.LogRateUsFake(true, this.countStarForReview, this.isInputMsgForReview);
			}
			else
			{
				FlurryEvents.LogRateUsFake(true, 5, false);
			}
			AnalyticsStuff.RateUsFake(true, this.countStarForReview, (!this.isInputMsgForReview ? false : this.countStarForReview != 5));
			ReviewController.SendReview(this.countStarForReview, empty);
			if (isClickSend)
			{
				this.NeedShowThanks = true;
				ReviewHUDWindow.isShow = true;
				this.OnShowThanks();
			}
		}
	}

	public void ShowWindowRating()
	{
		ReviewController.CheckActiveReview();
		if (ReviewController.IsNeedActive)
		{
			this.OnShowWidowRating();
		}
	}

	[ContextMenu("Show window")]
	public void TestShow()
	{
		ReviewController.IsNeedActive = true;
		this.ShowWindowRating();
	}

	private void UpdateStateBtnSendMsg(bool val)
	{
		this.isInputMsgForReview = val;
		if (!this.isInputMsgForReview)
		{
			this.btnSendMsg.enabled = false;
			this.btnSendMsg.state = UIButtonColor.State.Disabled;
		}
		else
		{
			this.btnSendMsg.enabled = true;
			this.btnSendMsg.state = UIButtonColor.State.Normal;
		}
	}
}