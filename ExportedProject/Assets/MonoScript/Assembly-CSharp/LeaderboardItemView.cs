using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class LeaderboardItemView : MonoBehaviour
{
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

	private EventHandler<ClickedEventArgs> Clicked;

	public LeaderboardItemView()
	{
	}

	public void HandleClick()
	{
		ClickedEventArgs clickedEventArg = new ClickedEventArgs(this._id);
		this.Clicked.Do<EventHandler<ClickedEventArgs>>((EventHandler<ClickedEventArgs> handler) => handler(this, clickedEventArg));
	}

	public void NewReset(LeaderboardItemViewModel viewModel)
	{
		LeaderboardItemViewModel leaderboardItemViewModel = viewModel ?? LeaderboardItemViewModel.Empty;
		this.Clicked = null;
		this._id = leaderboardItemViewModel.Id;
		UILabel.Effect effect = (viewModel.Highlight || viewModel.Place <= 3 && viewModel.WinCount > 0 ? UILabel.Effect.Outline : UILabel.Effect.None);
		this.highlightSprite.Do<UISprite>((UISprite h) => h.gameObject.SetActive(viewModel.Highlight));
		this.placeLabel.Do<UILabel>((UILabel l) => {
			l.text = viewModel.Place.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effect;
		});
		this.nicknameLabel.Do<UILabel>((UILabel l) => {
			l.text = viewModel.Nickname ?? string.Empty;
			l.effectStyle = effect;
		});
		this.levelLabel.Do<UILabel>((UILabel l) => {
			l.text = viewModel.Rank.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effect;
		});
		this.clanLogo.Do<UITexture>((UITexture s) => LeaderboardScript.SetClanLogo(s, viewModel.ClanLogoTexture));
		this.clanNameLabel.Do<UILabel>((UILabel l) => {
			l.text = (!string.IsNullOrEmpty(viewModel.ClanName) ? viewModel.ClanName : LocalizationStore.Get("Key_1500"));
			l.effectStyle = effect;
		});
		this.winCountLabel.Do<UILabel>((UILabel l) => {
			l.text = viewModel.WinCount.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effect;
		});
	}

	[Obsolete]
	public void Reset(LeaderboardItemViewModel viewModel)
	{
		object obj;
		string empty;
		LeaderboardItemViewModel leaderboardItemViewModel = viewModel ?? LeaderboardItemViewModel.Empty;
		Func<object, string> func = (object s) => {
			string str = s.ToString();
			if (viewModel.Highlight)
			{
				str = string.Format("[{0}]{1}[-]", "FFFF00", str);
			}
			return str;
		};
		if (this.rankSprite != null)
		{
			this.rankSprite.spriteName = string.Concat("Rank_", Mathf.Clamp(leaderboardItemViewModel.Rank, 1, ExperienceController.maxLevel));
		}
		if (this.clanLogo != null)
		{
			if (string.IsNullOrEmpty(leaderboardItemViewModel.ClanLogo))
			{
				Texture texture = this.clanLogo.mainTexture;
				this.clanLogo.mainTexture = null;
				if (texture != null)
				{
					UnityEngine.Object.Destroy(texture);
				}
			}
			else
			{
				try
				{
					byte[] numArray = Convert.FromBase64String(leaderboardItemViewModel.ClanLogo ?? string.Empty);
					Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
					texture2D.LoadImage(numArray);
					texture2D.filterMode = FilterMode.Point;
					texture2D.Apply();
					Texture texture1 = this.clanLogo.mainTexture;
					this.clanLogo.mainTexture = texture2D;
					if (texture1 != null)
					{
						UnityEngine.Object.Destroy(texture1);
					}
				}
				catch
				{
					Texture texture2 = this.clanLogo.mainTexture;
					this.clanLogo.mainTexture = null;
					if (texture2 != null)
					{
						UnityEngine.Object.Destroy(texture2);
					}
				}
			}
		}
		if (this.nicknameLabel != null)
		{
			string nickname = leaderboardItemViewModel.Nickname ?? string.Empty;
			this.nicknameLabel.text = func(nickname);
		}
		if (this.winCountLabel != null)
		{
			UILabel uILabel = this.winCountLabel;
			if (leaderboardItemViewModel != LeaderboardItemViewModel.Empty)
			{
				Func<object, string> func1 = func;
				if (leaderboardItemViewModel.WinCount != -2147483648)
				{
					int num = Math.Max(leaderboardItemViewModel.WinCount, 0);
					obj = num.ToString();
				}
				else
				{
					obj = "—";
				}
				empty = func1(obj);
			}
			else
			{
				empty = string.Empty;
			}
			uILabel.text = empty;
		}
		if (this.placeLabel != null)
		{
			this.placeLabel.text = (leaderboardItemViewModel != LeaderboardItemViewModel.Empty ? func((leaderboardItemViewModel.Place >= 0 ? leaderboardItemViewModel.Place.ToString() : LocalizationStore.Key_0588)) : string.Empty);
		}
	}

	[Obsolete]
	public void Reset()
	{
		this.Reset(null);
	}

	public event EventHandler<ClickedEventArgs> Clicked
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.Clicked += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.Clicked -= value;
		}
	}
}