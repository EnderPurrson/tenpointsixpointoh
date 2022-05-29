using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public sealed class LeaderboardItemViewModel
	{
		private static LeaderboardItemViewModel _empty;

		private string _clanLogo = string.Empty;

		private Lazy<Texture2D> _clanLogoTexture = new Lazy<Texture2D>(new Func<Texture2D>(() => null));

		public string ClanLogo
		{
			get
			{
				return this._clanLogo;
			}
			set
			{
				if (value == this._clanLogo)
				{
					return;
				}
				this._clanLogo = value;
				this._clanLogoTexture = new Lazy<Texture2D>(() => LeaderboardItemViewModel.CreateLogoFromBase64String(value));
			}
		}

		public Texture2D ClanLogoTexture
		{
			get
			{
				if (this._clanLogoTexture.Value == null)
				{
					string clanLogo = this.ClanLogo;
					this._clanLogoTexture = new Lazy<Texture2D>(() => LeaderboardItemViewModel.CreateLogoFromBase64String(clanLogo));
				}
				return this._clanLogoTexture.Value;
			}
		}

		public string ClanName
		{
			get;
			set;
		}

		public static LeaderboardItemViewModel Empty
		{
			get
			{
				return LeaderboardItemViewModel._empty;
			}
		}

		public bool Highlight
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}

		public string Nickname
		{
			get;
			set;
		}

		public int Place
		{
			get;
			set;
		}

		public int Rank
		{
			get;
			set;
		}

		public int WinCount
		{
			get;
			set;
		}

		static LeaderboardItemViewModel()
		{
			LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel()
			{
				Id = string.Empty,
				Nickname = string.Empty,
				ClanLogo = string.Empty
			};
			LeaderboardItemViewModel._empty = leaderboardItemViewModel;
		}

		public LeaderboardItemViewModel()
		{
		}

		internal static Texture2D CreateLogoFromBase64String(string logo)
		{
			Texture2D texture2D;
			if (string.IsNullOrEmpty(logo))
			{
				return null;
			}
			try
			{
				byte[] numArray = Convert.FromBase64String(logo);
				Texture2D texture2D1 = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false)
				{
					filterMode = FilterMode.Point
				};
				Texture2D texture2D2 = texture2D1;
				texture2D2.LoadImage(numArray);
				texture2D2.Apply();
				texture2D = texture2D2;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				texture2D = null;
			}
			return texture2D;
		}
	}
}