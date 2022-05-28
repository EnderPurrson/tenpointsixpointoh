using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public sealed class LeaderboardItemViewModel
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__AnonStorey2BB
		{
			internal string value;

			internal Texture2D _003C_003Em__346()
			{
				return CreateLogoFromBase64String(value);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__AnonStorey2BC
		{
			internal string currentClanLogo;

			internal Texture2D _003C_003Em__347()
			{
				return CreateLogoFromBase64String(currentClanLogo);
			}
		}

		private static LeaderboardItemViewModel _empty = new LeaderboardItemViewModel
		{
			Id = string.Empty,
			Nickname = string.Empty,
			ClanLogo = string.Empty
		};

		private string _clanLogo = string.Empty;

		private Lazy<Texture2D> _clanLogoTexture;

		[CompilerGenerated]
		private static Func<Texture2D> _003C_003Ef__am_0024cacheA;

		public string Id { get; set; }

		public int Rank { get; set; }

		public string Nickname { get; set; }

		public int WinCount { get; set; }

		public int Place { get; set; }

		public bool Highlight { get; set; }

		public string ClanName { get; set; }

		public string ClanLogo
		{
			get
			{
				return _clanLogo;
			}
			set
			{
				_003C_003Ec__AnonStorey2BB _003C_003Ec__AnonStorey2BB = new _003C_003Ec__AnonStorey2BB();
				_003C_003Ec__AnonStorey2BB.value = value;
				if (!(_003C_003Ec__AnonStorey2BB.value == _clanLogo))
				{
					_clanLogo = _003C_003Ec__AnonStorey2BB.value;
					_clanLogoTexture = new Lazy<Texture2D>(_003C_003Ec__AnonStorey2BB._003C_003Em__346);
				}
			}
		}

		public Texture2D ClanLogoTexture
		{
			get
			{
				if (_clanLogoTexture.Value == null)
				{
					_003C_003Ec__AnonStorey2BC _003C_003Ec__AnonStorey2BC = new _003C_003Ec__AnonStorey2BC();
					_003C_003Ec__AnonStorey2BC.currentClanLogo = ClanLogo;
					_clanLogoTexture = new Lazy<Texture2D>(_003C_003Ec__AnonStorey2BC._003C_003Em__347);
				}
				return _clanLogoTexture.Value;
			}
		}

		public static LeaderboardItemViewModel Empty
		{
			get
			{
				return _empty;
			}
		}

		public LeaderboardItemViewModel()
		{
			if (_003C_003Ef__am_0024cacheA == null)
			{
				_003C_003Ef__am_0024cacheA = _003C_clanLogoTexture_003Em__345;
			}
			_clanLogoTexture = new Lazy<Texture2D>(_003C_003Ef__am_0024cacheA);
			base._002Ector();
		}

		internal static Texture2D CreateLogoFromBase64String(string logo)
		{
			//Discarded unreachable code: IL_0045, IL_0059
			if (string.IsNullOrEmpty(logo))
			{
				return null;
			}
			try
			{
				byte[] data = Convert.FromBase64String(logo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
				texture2D.filterMode = FilterMode.Point;
				Texture2D texture2D2 = texture2D;
				texture2D2.LoadImage(data);
				texture2D2.Apply();
				return texture2D2;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return null;
			}
		}

		[CompilerGenerated]
		private static Texture2D _003C_clanLogoTexture_003Em__345()
		{
			return null;
		}
	}
}
