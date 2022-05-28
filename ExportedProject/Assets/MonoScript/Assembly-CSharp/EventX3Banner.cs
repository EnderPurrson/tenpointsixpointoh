using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.NullExtensions;
using UnityEngine;

public class EventX3Banner : BannerWindow
{
	public GameObject amazonEventObject;

	public UILabel amazonEventCaptionLabel;

	public UILabel amazonEventTitleLabel;

	[CompilerGenerated]
	private static Func<UILabel, bool> _003C_003Ef__am_0024cache3;

	[CompilerGenerated]
	private static Func<PromoActionsManager, PromoActionsManager.AmazonEventInfo> _003C_003Ef__am_0024cache4;

	[CompilerGenerated]
	private static Func<PromoActionsManager.AmazonEventInfo, string> _003C_003Ef__am_0024cache5;

	[CompilerGenerated]
	private static Func<UILabel, bool> _003C_003Ef__am_0024cache6;

	[CompilerGenerated]
	private static Func<UILabel, UILabel[]> _003C_003Ef__am_0024cache7;

	[CompilerGenerated]
	private static Func<PromoActionsManager.AmazonEventInfo, float> _003C_003Ef__am_0024cache8;

	private void OnEnable()
	{
		bool isAmazonEventX3Active = PromoActionsManager.sharedManager.IsAmazonEventX3Active;
		amazonEventObject.SetActive(isAmazonEventX3Active);
		PromoActionsManager.EventAmazonX3Updated += OnAmazonEventUpdated;
		RefreshAmazonBonus();
	}

	private void OnDisable()
	{
		PromoActionsManager.EventAmazonX3Updated -= OnAmazonEventUpdated;
	}

	private void RefreshAmazonBonus()
	{
		UILabel[] componentsInChildren = amazonEventObject.GetComponentsInChildren<UILabel>();
		UILabel uILabel = amazonEventCaptionLabel;
		if ((object)uILabel == null)
		{
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003CRefreshAmazonBonus_003Em__257;
			}
			uILabel = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cache3);
		}
		UILabel uILabel2 = uILabel;
		PromoActionsManager sharedManager = PromoActionsManager.sharedManager;
		if (_003C_003Ef__am_0024cache4 == null)
		{
			_003C_003Ef__am_0024cache4 = _003CRefreshAmazonBonus_003Em__258;
		}
		PromoActionsManager.AmazonEventInfo o = sharedManager.Map(_003C_003Ef__am_0024cache4);
		if (uILabel2 != null)
		{
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003CRefreshAmazonBonus_003Em__259;
			}
			uILabel2.text = o.Map(_003C_003Ef__am_0024cache5) ?? string.Empty;
		}
		UILabel uILabel3 = amazonEventTitleLabel;
		if ((object)uILabel3 == null)
		{
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003CRefreshAmazonBonus_003Em__25A;
			}
			uILabel3 = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cache6);
		}
		UILabel o2 = uILabel3;
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = _003CRefreshAmazonBonus_003Em__25B;
		}
		UILabel[] array = o2.Map(_003C_003Ef__am_0024cache7) ?? new UILabel[0];
		if (_003C_003Ef__am_0024cache8 == null)
		{
			_003C_003Ef__am_0024cache8 = _003CRefreshAmazonBonus_003Em__25C;
		}
		float num = o.Map(_003C_003Ef__am_0024cache8);
		string text = LocalizationStore.Get("Key_1672");
		UILabel[] array2 = array;
		foreach (UILabel uILabel4 in array2)
		{
			uILabel4.text = ("Key_1672".Equals(text, StringComparison.OrdinalIgnoreCase) ? string.Empty : string.Format(text, num));
		}
	}

	private void OnAmazonEventUpdated()
	{
		amazonEventObject.SetActive(PromoActionsManager.sharedManager.IsAmazonEventX3Active);
		RefreshAmazonBonus();
	}

	[CompilerGenerated]
	private static bool _003CRefreshAmazonBonus_003Em__257(UILabel l)
	{
		return "CaptionLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase);
	}

	[CompilerGenerated]
	private static PromoActionsManager.AmazonEventInfo _003CRefreshAmazonBonus_003Em__258(PromoActionsManager p)
	{
		return p.AmazonEvent;
	}

	[CompilerGenerated]
	private static string _003CRefreshAmazonBonus_003Em__259(PromoActionsManager.AmazonEventInfo e)
	{
		return e.Caption;
	}

	[CompilerGenerated]
	private static bool _003CRefreshAmazonBonus_003Em__25A(UILabel l)
	{
		return "TitleLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase);
	}

	[CompilerGenerated]
	private static UILabel[] _003CRefreshAmazonBonus_003Em__25B(UILabel t)
	{
		return t.GetComponentsInChildren<UILabel>();
	}

	[CompilerGenerated]
	private static float _003CRefreshAmazonBonus_003Em__25C(PromoActionsManager.AmazonEventInfo e)
	{
		return e.Percentage;
	}
}
