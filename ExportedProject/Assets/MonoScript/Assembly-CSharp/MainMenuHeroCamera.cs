using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainMenuHeroCamera : MonoBehaviour
{
	public Animator moveMenuAnimator;

	private string animGotcha = "MainMenuOpenGotcha";

	private string animNormal = "MainMenuCloseOptions";

	public bool IsAnimPlaying
	{
		get;
		private set;
	}

	private float YawForMainMenu
	{
		get
		{
			return (!MenuLeaderboardsView.IsNeedShow ? 6f : 0f);
		}
	}

	public MainMenuHeroCamera()
	{
	}

	private void EndAnimation(string nameAnim)
	{
		int num;
		string str = nameAnim;
		if (str != null)
		{
			if (MainMenuHeroCamera.u003cu003ef__switchu0024map11 == null)
			{
				MainMenuHeroCamera.u003cu003ef__switchu0024map11 = new Dictionary<string, int>(1)
				{
					{ "MainMenuOpenGotcha", 0 }
				};
			}
			if (MainMenuHeroCamera.u003cu003ef__switchu0024map11.TryGetValue(str, out num))
			{
				if (num == 0)
				{
					if (base.GetComponent<Animation>()[this.animGotcha].speed > 0f)
					{
						if (MainMenuHeroCamera.onEndOpenGift != null)
						{
							MainMenuHeroCamera.onEndOpenGift();
						}
					}
					else if (MainMenuHeroCamera.onEndCloseGift != null)
					{
						MainMenuHeroCamera.onEndCloseGift();
					}
				}
			}
		}
	}

	public void OnCloseGift()
	{
		base.GetComponent<Animation>()[this.animGotcha].speed = -1f;
		base.GetComponent<Animation>()[this.animGotcha].time = base.GetComponent<Animation>()[this.animGotcha].length;
		base.GetComponent<Animation>().Play(this.animGotcha);
		base.StartCoroutine(this.WaitAnimEnd(this.animGotcha));
	}

	public void OnCloseSingleModePanel()
	{
		this.moveMenuAnimator.enabled = true;
		this.PlayAnim(this.YawForMainMenu);
	}

	private void OnDisable()
	{
		ButOpenGift.onOpen -= new Action(this.OnShowGift);
		GiftBannerWindow.onClose -= new Action(this.OnCloseGift);
	}

	private void OnEnable()
	{
		ButOpenGift.onOpen += new Action(this.OnShowGift);
		GiftBannerWindow.onClose += new Action(this.OnCloseGift);
	}

	public void OnMainMenuCloseLeaderboards()
	{
		this.PlayAnim(6f);
	}

	public void OnMainMenuCloseOptions()
	{
		this.PlayAnim(this.YawForMainMenu);
		if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.menuLeaderboardsView != null && MenuLeaderboardsView.IsNeedShow)
		{
			MenuLeaderboardsController.sharedController.menuLeaderboardsView.Show(true, true);
		}
	}

	public void OnMainMenuOpenLeaderboards()
	{
		this.PlayAnim(0f);
	}

	public void OnMainMenuOpenOptions()
	{
		this.PlayAnim(10f);
		if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.menuLeaderboardsView != null)
		{
			MenuLeaderboardsController.sharedController.menuLeaderboardsView.Show(false, false);
		}
	}

	public void OnOpenSingleModePanel()
	{
		base.StopAllCoroutines();
		this.moveMenuAnimator.enabled = false;
	}

	public void OnShowGift()
	{
		base.GetComponent<Animation>()[this.animGotcha].speed = 1f;
		base.GetComponent<Animation>()[this.animGotcha].time = 0f;
		base.GetComponent<Animation>().Play(this.animGotcha);
		base.StartCoroutine(this.WaitAnimEnd(this.animGotcha));
	}

	private void PlayAnim(float endYaw)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.PlayAnimCoroutine(endYaw));
	}

	[DebuggerHidden]
	private IEnumerator PlayAnimCoroutine(float endYaw)
	{
		MainMenuHeroCamera.u003cPlayAnimCoroutineu003ec__Iterator1BB variable = null;
		return variable;
	}

	public void Start()
	{
		Vector3 vector3 = base.transform.rotation.eulerAngles;
		base.transform.rotation = Quaternion.Euler(new Vector3(vector3.x, this.YawForMainMenu, vector3.z));
	}

	[DebuggerHidden]
	private IEnumerator WaitAnimEnd(string nameAnim)
	{
		MainMenuHeroCamera.u003cWaitAnimEndu003ec__Iterator1BA variable = null;
		return variable;
	}

	public static event Action onEndCloseGift;

	public static event Action onEndOpenGift;
}