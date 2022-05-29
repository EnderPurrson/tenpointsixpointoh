using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class FeedbackMenuController : MonoBehaviour
{
	private FeedbackMenuController.State _currentState;

	public UIButton faqButton;

	public UIButton termsFuseButton;

	public UILabel textLabel;

	public UIButton sendFeedbackButton;

	public UIButton backButton;

	public GameObject textFAQScroll;

	public GameObject textTermsOfUse;

	[SerializeField]
	private UILabel versionLabel;

	private IDisposable _backSubscription;

	public FeedbackMenuController.State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			if (this.faqButton != null)
			{
				this.faqButton.isEnabled = value != FeedbackMenuController.State.FAQ;
				Transform transforms = this.faqButton.gameObject.transform.FindChild("SpriteLabel");
				if (transforms != null)
				{
					transforms.gameObject.SetActive(value != FeedbackMenuController.State.FAQ);
				}
				Transform transforms1 = this.faqButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transforms1 != null)
				{
					transforms1.gameObject.SetActive(value == FeedbackMenuController.State.FAQ);
				}
				this.textFAQScroll.SetActive(value == FeedbackMenuController.State.FAQ);
			}
			if (this.termsFuseButton != null)
			{
				this.termsFuseButton.isEnabled = value != FeedbackMenuController.State.TermsFuse;
				Transform transforms2 = this.termsFuseButton.gameObject.transform.FindChild("SpriteLabel");
				if (transforms2 == null)
				{
					Debug.Log("_spriteLabel=null");
				}
				else
				{
					transforms2.gameObject.SetActive(value != FeedbackMenuController.State.TermsFuse);
				}
				Transform transforms3 = this.termsFuseButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transforms3 == null)
				{
					Debug.Log("_spriteLabel=null");
				}
				else
				{
					transforms3.gameObject.SetActive(value == FeedbackMenuController.State.TermsFuse);
				}
				this.textTermsOfUse.SetActive(value == FeedbackMenuController.State.TermsFuse);
			}
			this._currentState = value;
		}
	}

	public static FeedbackMenuController Instance
	{
		get;
		private set;
	}

	public FeedbackMenuController()
	{
	}

	private void Awake()
	{
		FeedbackMenuController.Instance = this;
	}

	private void BackButton()
	{
		base.gameObject.SetActive(false);
		if (MainMenuController.sharedController != null && MainMenuController.sharedController.settingsPanel != null)
		{
			MainMenuController.sharedController.settingsPanel.SetActive(true);
		}
	}

	private void HandleBackButton(object sender, EventArgs e)
	{
		this.BackButton();
	}

	private void HandleSendFeedback(object sender, EventArgs e)
	{
		FeedbackMenuController.ShowDialogWithCompletion(() => {
			string str = typeof(FeedbackMenuController).Assembly.GetName().Version.ToString();
			string str1 = string.Concat(new object[] { "mailto:pixelgun3D.supp0rt@gmail.com?subject=Feedback&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20", DateTime.Now.ToString(), "%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20", str, "%0D%0APlayerID:%20", FriendsController.sharedController.id, "%0D%0ACategory:%20Feedback%0D%0ADevice%20Type:%20", SystemInfo.deviceType, "%20", SystemInfo.deviceModel, "%0D%0AOS%20Version:%20", SystemInfo.operatingSystem, "%0D%0A------------------------" });
			str1 = str1.Replace(" ", "%20");
			Debug.Log(str1);
			FlurryPluginWrapper.LogEventWithParameterAndValue("User Feedback", "Menu", "User Support Menu");
			Application.OpenURL(str1);
		});
	}

	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (this.faqButton != null && gameObject == this.faqButton.gameObject)
		{
			this.CurrentState = FeedbackMenuController.State.FAQ;
		}
		else if (this.termsFuseButton != null && gameObject == this.termsFuseButton.gameObject)
		{
			this.CurrentState = FeedbackMenuController.State.TermsFuse;
		}
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.BackButton), "Feedback");
	}

	public static void ShowDialogWithCompletion(Action handler)
	{
		if (handler != null)
		{
			handler();
		}
	}

	private void Start()
	{
		this.CurrentState = FeedbackMenuController.State.FAQ;
		IEnumerable<UIButton> uIButtons = 
			from b in (IEnumerable<UIButton>)(new UIButton[] { this.faqButton, this.termsFuseButton })
			where b != null
			select b;
		IEnumerator<UIButton> enumerator = uIButtons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ButtonHandler component = enumerator.Current.GetComponent<ButtonHandler>();
				if (component == null)
				{
					continue;
				}
				component.Clicked += new EventHandler(this.HandleTabPressed);
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		if (this.sendFeedbackButton != null)
		{
			ButtonHandler buttonHandler = this.sendFeedbackButton.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.HandleSendFeedback);
			}
		}
		if (this.backButton != null)
		{
			ButtonHandler component1 = this.backButton.GetComponent<ButtonHandler>();
			if (component1 != null)
			{
				component1.Clicked += new EventHandler(this.HandleBackButton);
			}
		}
		string str = typeof(SettingsController).Assembly.GetName().Version.ToString();
		this.versionLabel.text = str;
	}

	public enum State
	{
		FAQ,
		TermsFuse
	}
}