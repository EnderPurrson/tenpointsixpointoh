using System;
using UnityEngine;

public class ReviewController : MonoBehaviour
{
	public const string keyNeedActiveReview = "keyNeedActiveReview";

	public const string keyAlreadySendReview = "keyAlreadySendReview";

	public const string keyOldVersionForReview = "keyOldVersionForReview";

	public const string keyNeedSendMsgReview = "keyNeedSendMsgReview";

	public const string keyReviewSaveRating = "keyReviewSaveRating";

	public const string keyReviewSaveMsg = "keyReviewSaveMsg";

	public static ReviewController instance;

	public static int _IsNeedActive;

	public static int _ExistReviewForSend;

	public static bool isSending;

	public static bool ExistReviewForSend
	{
		get
		{
			if (ReviewController._ExistReviewForSend < 0)
			{
				ReviewController._ExistReviewForSend = (!Load.LoadBool("keyNeedSendMsgReview") ? 0 : 1);
			}
			return (ReviewController._ExistReviewForSend != 0 ? true : false);
		}
		set
		{
			Save.SaveBool("keyNeedSendMsgReview", value);
			ReviewController._ExistReviewForSend = (!value ? 0 : 1);
		}
	}

	public static bool IsNeedActive
	{
		get
		{
			if (ReviewController._IsNeedActive < 0)
			{
				ReviewController._IsNeedActive = (!Load.LoadBool("keyNeedActiveReview") ? 0 : 1);
			}
			return (ReviewController._IsNeedActive != 0 ? true : false);
		}
		set
		{
			if (ReviewController.ExistReviewForSend && value)
			{
				return;
			}
			if (ReviewController.IsNeedActive != value)
			{
				Save.SaveBool("keyNeedActiveReview", value);
			}
			ReviewController._IsNeedActive = (!value ? 0 : 1);
		}
	}

	public static bool IsSendReview
	{
		get
		{
			return Load.LoadBool("keyAlreadySendReview");
		}
		set
		{
			Save.SaveBool("keyAlreadySendReview", value);
		}
	}

	public static string ReviewMsg
	{
		get
		{
			return Load.LoadString("keyReviewSaveMsg");
		}
		set
		{
			Save.SaveString("keyReviewSaveMsg", value);
		}
	}

	public static int ReviewRating
	{
		get
		{
			return Load.LoadInt("keyReviewSaveRating");
		}
		set
		{
			Save.SaveInt("keyReviewSaveRating", value);
		}
	}

	static ReviewController()
	{
		ReviewController._IsNeedActive = -1;
		ReviewController._ExistReviewForSend = -1;
	}

	public ReviewController()
	{
	}

	private void Awake()
	{
		ReviewController.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void CheckActiveReview()
	{
		if (ExperienceController.sharedController == null)
		{
			return;
		}
		if (GlobalGameController.CountDaySessionInCurrentVersion > 2 && ExperienceController.sharedController.currentLevel >= 4 && !ReviewController.IsSendReview)
		{
			ReviewController.IsNeedActive = true;
		}
	}

	private void OnDestroy()
	{
		ReviewController.instance = null;
	}

	public static void SendReview(int rating, string msgReview)
	{
		ReviewController.ReviewRating = rating;
		ReviewController.ReviewMsg = msgReview;
		ReviewController.ExistReviewForSend = true;
		if (rating == 5)
		{
			Application.OpenURL(Defs2.ApplicationUrl);
		}
		FriendsController.StartSendReview();
	}
}