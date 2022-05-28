using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

internal sealed class LeaderboardItemView : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CNewReset_003Ec__AnonStorey2BD
	{
		internal LeaderboardItemViewModel viewModel;

		internal UILabel.Effect effectStyle;

		internal void _003C_003Em__348(UISprite h)
		{
			h.gameObject.SetActive(viewModel.Highlight);
		}

		internal void _003C_003Em__349(UILabel l)
		{
			l.text = viewModel.Place.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effectStyle;
		}

		internal void _003C_003Em__34A(UILabel l)
		{
			l.text = viewModel.Nickname ?? string.Empty;
			l.effectStyle = effectStyle;
		}

		internal void _003C_003Em__34B(UILabel l)
		{
			l.text = viewModel.Rank.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effectStyle;
		}

		internal void _003C_003Em__34C(UITexture s)
		{
			LeaderboardScript.SetClanLogo(s, viewModel.ClanLogoTexture);
		}

		internal void _003C_003Em__34D(UILabel l)
		{
			l.text = ((!string.IsNullOrEmpty(viewModel.ClanName)) ? viewModel.ClanName : LocalizationStore.Get("Key_1500"));
			l.effectStyle = effectStyle;
		}

		internal void _003C_003Em__34E(UILabel l)
		{
			l.text = viewModel.WinCount.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effectStyle;
		}
	}

	[CompilerGenerated]
	private sealed class _003CHandleClick_003Ec__AnonStorey2BE
	{
		internal ClickedEventArgs e;

		internal LeaderboardItemView _003C_003Ef__this;

		internal void _003C_003Em__34F(EventHandler<ClickedEventArgs> handler)
		{
			handler(_003C_003Ef__this, e);
		}
	}

	[CompilerGenerated]
	private sealed class _003CReset_003Ec__AnonStorey2BF
	{
		internal LeaderboardItemViewModel viewModel;

		internal string _003C_003Em__350(object s)
		{
			string text = s.ToString();
			if (viewModel.Highlight)
			{
				text = string.Format("[{0}]{1}[-]", "FFFF00", text);
			}
			return text;
		}
	}

	private const string HighlightColor = "FFFF00";

	public string _id;

	public UISprite rankSprite;

	public UILabel nicknameLabel;

	public UILabel winCountLabel;

	public UILabel placeLabel;

	public UITexture clanLogo;

	public UILabel clanNameLabel;

	public UISprite highlightSprite;

	public UILabel levelLabel;

	public event EventHandler<ClickedEventArgs> Clicked;

	public void NewReset(LeaderboardItemViewModel viewModel)
	{
		_003CNewReset_003Ec__AnonStorey2BD _003CNewReset_003Ec__AnonStorey2BD = new _003CNewReset_003Ec__AnonStorey2BD();
		_003CNewReset_003Ec__AnonStorey2BD.viewModel = viewModel;
		LeaderboardItemViewModel leaderboardItemViewModel = _003CNewReset_003Ec__AnonStorey2BD.viewModel ?? LeaderboardItemViewModel.Empty;
		this.Clicked = null;
		_id = leaderboardItemViewModel.Id;
		_003CNewReset_003Ec__AnonStorey2BD.effectStyle = ((_003CNewReset_003Ec__AnonStorey2BD.viewModel.Highlight || (_003CNewReset_003Ec__AnonStorey2BD.viewModel.Place <= 3 && _003CNewReset_003Ec__AnonStorey2BD.viewModel.WinCount > 0)) ? UILabel.Effect.Outline : UILabel.Effect.None);
		highlightSprite.Do(_003CNewReset_003Ec__AnonStorey2BD._003C_003Em__348);
		placeLabel.Do(_003CNewReset_003Ec__AnonStorey2BD._003C_003Em__349);
		nicknameLabel.Do(_003CNewReset_003Ec__AnonStorey2BD._003C_003Em__34A);
		levelLabel.Do(_003CNewReset_003Ec__AnonStorey2BD._003C_003Em__34B);
		clanLogo.Do(_003CNewReset_003Ec__AnonStorey2BD._003C_003Em__34C);
		clanNameLabel.Do(_003CNewReset_003Ec__AnonStorey2BD._003C_003Em__34D);
		winCountLabel.Do(_003CNewReset_003Ec__AnonStorey2BD._003C_003Em__34E);
	}

	public void HandleClick()
	{
		_003CHandleClick_003Ec__AnonStorey2BE _003CHandleClick_003Ec__AnonStorey2BE = new _003CHandleClick_003Ec__AnonStorey2BE();
		_003CHandleClick_003Ec__AnonStorey2BE._003C_003Ef__this = this;
		_003CHandleClick_003Ec__AnonStorey2BE.e = new ClickedEventArgs(_id);
		this.Clicked.Do(_003CHandleClick_003Ec__AnonStorey2BE._003C_003Em__34F);
	}

	[Obsolete]
	public void Reset(LeaderboardItemViewModel viewModel)
	{
		_003CReset_003Ec__AnonStorey2BF _003CReset_003Ec__AnonStorey2BF = new _003CReset_003Ec__AnonStorey2BF();
		_003CReset_003Ec__AnonStorey2BF.viewModel = viewModel;
		LeaderboardItemViewModel leaderboardItemViewModel = _003CReset_003Ec__AnonStorey2BF.viewModel ?? LeaderboardItemViewModel.Empty;
		Func<object, string> func = _003CReset_003Ec__AnonStorey2BF._003C_003Em__350;
		if (rankSprite != null)
		{
			rankSprite.spriteName = "Rank_" + Mathf.Clamp(leaderboardItemViewModel.Rank, 1, ExperienceController.maxLevel);
		}
		if (clanLogo != null)
		{
			if (!string.IsNullOrEmpty(leaderboardItemViewModel.ClanLogo))
			{
				try
				{
					byte[] data = Convert.FromBase64String(leaderboardItemViewModel.ClanLogo ?? string.Empty);
					Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
					texture2D.LoadImage(data);
					texture2D.filterMode = FilterMode.Point;
					texture2D.Apply();
					Texture mainTexture = clanLogo.mainTexture;
					clanLogo.mainTexture = texture2D;
					if (mainTexture != null)
					{
						UnityEngine.Object.Destroy(mainTexture);
					}
				}
				catch
				{
					Texture mainTexture2 = clanLogo.mainTexture;
					clanLogo.mainTexture = null;
					if (mainTexture2 != null)
					{
						UnityEngine.Object.Destroy(mainTexture2);
					}
				}
			}
			else
			{
				Texture mainTexture3 = clanLogo.mainTexture;
				clanLogo.mainTexture = null;
				if (mainTexture3 != null)
				{
					UnityEngine.Object.Destroy(mainTexture3);
				}
			}
		}
		if (nicknameLabel != null)
		{
			string arg = leaderboardItemViewModel.Nickname ?? string.Empty;
			nicknameLabel.text = func(arg);
		}
		if (winCountLabel != null)
		{
			winCountLabel.text = ((leaderboardItemViewModel != LeaderboardItemViewModel.Empty) ? func((leaderboardItemViewModel.WinCount != int.MinValue) ? Math.Max(leaderboardItemViewModel.WinCount, 0).ToString() : "—") : string.Empty);
		}
		if (placeLabel != null)
		{
			placeLabel.text = ((leaderboardItemViewModel != LeaderboardItemViewModel.Empty) ? func((leaderboardItemViewModel.Place >= 0) ? leaderboardItemViewModel.Place.ToString() : LocalizationStore.Key_0588) : string.Empty);
		}
	}

	[Obsolete]
	public void Reset()
	{
		Reset(null);
	}
}
