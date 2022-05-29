using I2.Loc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ButtonBannerHUD : MonoBehaviour
{
	private const string keyLastShowIndex = "keyLastShowIndex";

	private const string pathToBtnBanner = "ButtonBanners";

	private const float timeForNextScroll = 5f;

	public static ButtonBannerHUD instance;

	public UIScrollView scrollBanners;

	public UIWrapContent wrapBanners;

	public ButtonBannerBase curShowBanner;

	private ButtonBannerBase oldShowBanner;

	public Transform objAnchorNoActiveBanners;

	public List<ButtonBannerBase> listAllBanners = new List<ButtonBannerBase>();

	public List<ButtonBannerBase> listActiveBanners = new List<ButtonBannerBase>();

	[HideInInspector]
	public MyCenterOnChild centerScript;

	public int IndexShowBanner
	{
		get
		{
			return Load.LoadInt("keyLastShowIndex");
		}
		set
		{
			int num = value;
			if (num < 0)
			{
				num = 0;
			}
			if (num >= this.listActiveBanners.Count)
			{
				num = 0;
			}
			Save.SaveInt("keyLastShowIndex", num);
		}
	}

	static ButtonBannerHUD()
	{
	}

	public ButtonBannerHUD()
	{
	}

	private void AddNewActiveBanners()
	{
		foreach (ButtonBannerBase listAllBanner in this.listAllBanners)
		{
			if (!this.IsExistActiveBanner(listAllBanner))
			{
				if (listAllBanner.BannerIsActive())
				{
					listAllBanner.transform.parent = this.wrapBanners.transform;
					this.listActiveBanners.Add(listAllBanner);
				}
			}
		}
	}

	private void Awake()
	{
		ButtonBannerHUD.instance = this;
		this.LoadAllExistBanners();
	}

	private ButtonBannerBase GetNextActiveBanner()
	{
		int count = this.listActiveBanners.Count;
		ButtonBannerBase item = null;
		if (count > 0)
		{
			while (true)
			{
				ButtonBannerHUD indexShowBanner = this;
				indexShowBanner.IndexShowBanner = indexShowBanner.IndexShowBanner + 1;
				count--;
				item = this.listActiveBanners[this.IndexShowBanner];
				if (item.BannerIsActive())
				{
					return item;
				}
				if (count <= 0)
				{
					break;
				}
			}
			Debug.LogWarning("No next banner for show");
		}
		return item;
	}

	private bool IsExistActiveBanner(ButtonBannerBase needBanners)
	{
		return this.listActiveBanners.Contains(needBanners);
	}

	private void LoadAllExistBanners()
	{
		this.listAllBanners.Clear();
		UnityEngine.Object[] objArray = Resources.LoadAll("ButtonBanners");
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			UnityEngine.Object obj = objArray[i];
			if (obj != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
				gameObject.transform.parent = this.objAnchorNoActiveBanners;
				gameObject.transform.localScale = Vector3.one;
				ButtonBannerBase component = gameObject.GetComponent<ButtonBannerBase>();
				if (component != null)
				{
					this.listAllBanners.Add(component);
				}
			}
		}
	}

	public void LocalizeBanner()
	{
		if (this.curShowBanner != null)
		{
			this.curShowBanner.OnChangeLocalize();
		}
	}

	public void OnCenterBanner()
	{
		if (this.centerScript != null)
		{
			ButtonBannerBase component = this.centerScript.centeredObject.GetComponent<ButtonBannerBase>();
			if (this.oldShowBanner != null)
			{
				this.oldShowBanner.OnHide();
			}
			this.SetShowBanner(component, false);
			if (component)
			{
				this.ResetTimerNextBanner();
			}
		}
	}

	public void OnClickShowBanner()
	{
		if (this.curShowBanner != null)
		{
			this.curShowBanner.OnClickButton();
		}
	}

	private void OnDestroy()
	{
		if (this.centerScript != null)
		{
			this.centerScript.onFinished -= new SpringPanel.OnFinished(this.OnCenterBanner);
		}
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.LocalizeBanner));
		ButtonBannerHUD.instance = null;
	}

	public static void OnUpdateBanners()
	{
		if (ButtonBannerHUD.instance != null)
		{
			ButtonBannerHUD.instance.UpdateListBanners();
		}
	}

	private void RemoveAllNoActiveBanners()
	{
		if (this.curShowBanner != null && !this.curShowBanner.BannerIsActive())
		{
			this.curShowBanner = this.GetNextActiveBanner();
		}
		List<ButtonBannerBase> buttonBannerBases = new List<ButtonBannerBase>();
		foreach (ButtonBannerBase listActiveBanner in this.listActiveBanners)
		{
			if (listActiveBanner.BannerIsActive())
			{
				continue;
			}
			buttonBannerBases.Add(listActiveBanner);
		}
		foreach (ButtonBannerBase buttonBannerBasis in buttonBannerBases)
		{
			buttonBannerBasis.transform.parent = this.objAnchorNoActiveBanners;
			this.listActiveBanners.Remove(buttonBannerBasis);
		}
	}

	public void ResetTimerNextBanner()
	{
		this.StopTimerNextBanner();
		base.InvokeRepeating("ShowNextBanner", 5f, 5f);
	}

	private void SetShowBanner(ButtonBannerBase needBanner, bool auto = false)
	{
		if (this.curShowBanner != needBanner)
		{
			this.oldShowBanner = this.curShowBanner;
			this.curShowBanner = needBanner;
			if (this.curShowBanner != null)
			{
				this.IndexShowBanner = this.curShowBanner.indexBut;
				this.curShowBanner.OnShow();
				if (auto)
				{
					this.curShowBanner.OnUpdateParameter();
				}
			}
		}
	}

	public void ShowBanner(ButtonBannerBase needBanner)
	{
		if (needBanner == null || !needBanner.BannerIsActive())
		{
			return;
		}
		if (needBanner != null)
		{
			this.SetShowBanner(needBanner, true);
			this.centerScript.CenterOn(needBanner.transform);
		}
	}

	public void ShowNextBanner()
	{
		this.ShowBanner(this.GetNextActiveBanner());
	}

	private void SortByPriority()
	{
		this.listActiveBanners.Sort((ButtonBannerBase left, ButtonBannerBase right) => {
			if (left == null && right == null)
			{
				return 0;
			}
			if (left == null)
			{
				return -1;
			}
			if (right == null)
			{
				return 1;
			}
			return left.priorityShow.CompareTo(right.priorityShow);
		});
		string empty = string.Empty;
		for (int i = 0; i < this.listActiveBanners.Count; i++)
		{
			empty = i.ToString();
			if (i < 10)
			{
				empty = string.Concat("0", empty);
			}
			this.listActiveBanners[i].gameObject.name = empty;
			this.listActiveBanners[i].indexBut = i;
		}
		this.wrapBanners.SortAlphabetically();
		this.wrapBanners.WrapContent();
		this.ShowBanner(this.curShowBanner);
	}

	private void Start()
	{
		this.centerScript = this.wrapBanners.GetComponent<MyCenterOnChild>();
		if (this.centerScript != null)
		{
			this.centerScript.onFinished += new SpringPanel.OnFinished(this.OnCenterBanner);
		}
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.LocalizeBanner));
		this.UpdateListBanners();
		this.ResetTimerNextBanner();
	}

	public void StopTimerNextBanner()
	{
		base.CancelInvoke("ShowNextBanner");
	}

	private void UpdateListBanners()
	{
		this.RemoveAllNoActiveBanners();
		this.AddNewActiveBanners();
		this.SortByPriority();
	}
}