using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class MobileAdManager
{
	internal const string TextInterstitialUnitId = "ca-app-pub-5590536419057381/7885668153";

	internal const string DefaultImageInterstitialUnitId = "ca-app-pub-5590536419057381/1950086558";

	internal const string DefaultVideoInterstitialUnitId = "ca-app-pub-5590536419057381/2096360557";

	private static byte[] _guid;

	private int _imageAdUnitIdIndex;

	private int _imageIdGroupIndex;

	private int _videoAdUnitIdIndex;

	private int _videoIdGroupIndex;

	private readonly static Lazy<MobileAdManager> _instance;

	private List<string> AdmobImageAdUnitIds
	{
		get
		{
			if (PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count == 0)
			{
				return PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds;
			}
			return PromoActionsManager.MobileAdvert.AdmobImageIdGroups[this._imageIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count];
		}
	}

	private List<string> AdmobVideoAdUnitIds
	{
		get
		{
			if (PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count == 0)
			{
				return PromoActionsManager.MobileAdvert.AdmobVideoAdUnitIds;
			}
			return PromoActionsManager.MobileAdvert.AdmobVideoIdGroups[this._videoIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count];
		}
	}

	internal static byte[] GuidBytes
	{
		get
		{
			if (MobileAdManager._guid != null && (int)MobileAdManager._guid.Length > 0)
			{
				return MobileAdManager._guid;
			}
			if (!PlayerPrefs.HasKey("Guid"))
			{
				Guid guid = Guid.NewGuid();
				MobileAdManager._guid = guid.ToByteArray();
				PlayerPrefs.SetString("Guid", guid.ToString("D"));
				PlayerPrefs.Save();
			}
			else
			{
				try
				{
					Guid guid1 = new Guid(PlayerPrefs.GetString("Guid"));
					MobileAdManager._guid = guid1.ToByteArray();
				}
				catch
				{
					Guid guid2 = Guid.NewGuid();
					MobileAdManager._guid = guid2.ToByteArray();
					PlayerPrefs.SetString("Guid", guid2.ToString("D"));
					PlayerPrefs.Save();
				}
			}
			return MobileAdManager._guid;
		}
	}

	public string ImageAdFailedToLoadMessage
	{
		get;
		private set;
	}

	internal int ImageAdUnitIndexClamped
	{
		get
		{
			if (this.AdmobImageAdUnitIds.Count == 0)
			{
				return -1;
			}
			return this._imageAdUnitIdIndex % this.AdmobImageAdUnitIds.Count;
		}
	}

	public MobileAdManager.State ImageInterstitialState
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	private string ImageInterstitialUnitId
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null || PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count == 0)
			{
				return "ca-app-pub-5590536419057381/1950086558";
			}
			string item = this.AdmobImageAdUnitIds[this._imageAdUnitIdIndex % this.AdmobImageAdUnitIds.Count];
			return item;
		}
	}

	public static MobileAdManager Instance
	{
		get
		{
			return MobileAdManager._instance.Value;
		}
	}

	internal bool SuppressShowOnReturnFromPause
	{
		get;
		set;
	}

	public string VideoAdFailedToLoadMessage
	{
		get;
		private set;
	}

	internal int VideoAdUnitIndexClamped
	{
		get
		{
			if (this.AdmobVideoAdUnitIds.Count == 0)
			{
				return -1;
			}
			return this._videoAdUnitIdIndex % this.AdmobVideoAdUnitIds.Count;
		}
	}

	public MobileAdManager.State VideoInterstitialState
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	private string VideoInterstitialUnitId
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return "ca-app-pub-5590536419057381/2096360557";
			}
			if (this.AdmobVideoAdUnitIds.Count == 0)
			{
				return (!string.IsNullOrEmpty(PromoActionsManager.MobileAdvert.AdmobVideoAdUnitId) ? PromoActionsManager.MobileAdvert.AdmobVideoAdUnitId : "ca-app-pub-5590536419057381/2096360557");
			}
			return this.AdmobVideoAdUnitIds[this._videoAdUnitIdIndex % this.AdmobVideoAdUnitIds.Count];
		}
	}

	static MobileAdManager()
	{
		MobileAdManager._guid = new byte[0];
		MobileAdManager._instance = new Lazy<MobileAdManager>(() => new MobileAdManager());
	}

	private MobileAdManager()
	{
		this.ImageAdFailedToLoadMessage = string.Empty;
		this.VideoAdFailedToLoadMessage = string.Empty;
	}

	internal static bool AdIsApplicable(MobileAdManager.Type adType)
	{
		return MobileAdManager.AdIsApplicable(adType, false);
	}

	internal static bool AdIsApplicable(MobileAdManager.Type adType, bool verbose)
	{
		if (PromoActionsManager.MobileAdvert != null)
		{
			return MobileAdManager.UserPredicate(adType, verbose, false, false);
		}
		if (verbose)
		{
			Debug.LogWarningFormat("AdIsApplicable ({0}): false, because PromoActionsManager.MobileAdvert == null", new object[] { adType });
		}
		return false;
	}

	public void DestroyImageInterstitial()
	{
	}

	private void DestroyImageInterstitialCore()
	{
	}

	public void DestroyVideoInterstitial()
	{
	}

	private void DestroyVideoInterstitialCore()
	{
	}

	internal static MobileAdManager.SampleGroup GetSempleGroup()
	{
		return (MobileAdManager.GuidBytes[0] % 2 != 0 ? MobileAdManager.SampleGroup.Video : MobileAdManager.SampleGroup.Image);
	}

	private static bool IsLongTimeShowBaner()
	{
		DateTime dateTime;
		string str = PlayerPrefs.GetString(Defs.LastTimeShowBanerKey, string.Empty);
		if (string.IsNullOrEmpty(str))
		{
			return true;
		}
		if (!DateTime.TryParse(str, out dateTime))
		{
			return false;
		}
		TimeSpan utcNow = DateTime.UtcNow - dateTime;
		return utcNow.TotalSeconds > (double)PromoActionsManager.MobileAdvert.TimeoutBetweenShowInterstitial;
	}

	private static bool IsNewUser()
	{
		if (PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1) > PromoActionsManager.MobileAdvert.CountSessionNewPlayer)
		{
			return false;
		}
		return true;
	}

	public static bool IsNewUserOldMetod()
	{
		DateTimeOffset dateTimeOffset;
		string str = PlayerPrefs.GetString("First Launch (Advertisement)", string.Empty);
		if (string.IsNullOrEmpty(str) || !DateTimeOffset.TryParse(str, out dateTimeOffset))
		{
			return true;
		}
		return (DateTimeOffset.Now - dateTimeOffset).TotalDays < 7;
	}

	public static bool IsPayingUser()
	{
		return FlurryPluginWrapper.IsPayingUser();
	}

	private void LogToFlurry(string eventName, string context)
	{
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Menu", context }
		};
		if (ExperienceController.sharedController != null)
		{
			strs.Add("Levels", ExperienceController.sharedController.currentLevel.ToString());
		}
		if (ExpController.Instance != null)
		{
			strs.Add("Tiers", ExpController.Instance.OurTier.ToString());
		}
		FlurryPluginWrapper.LogEventAndDublicateToConsole(eventName, strs, true);
	}

	internal static void RefreshBytes()
	{
		Guid guid = new Guid(MobileAdManager._guid);
		PlayerPrefs.SetString("Guid", guid.ToString("D"));
		PlayerPrefs.Save();
	}

	internal static string RemovePrefix(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		int num = s.IndexOf('/');
		return (num <= 0 ? s : s.Remove(0, num));
	}

	internal bool ResetImageAdUnitId()
	{
		int num = this._imageAdUnitIdIndex;
		string imageInterstitialUnitId = this.ImageInterstitialUnitId;
		int num1 = this._imageIdGroupIndex;
		this._imageAdUnitIdIndex = 0;
		this._imageIdGroupIndex = 0;
		if (Defs.IsDeveloperBuild)
		{
			string str = string.Format("Resetting image ad unit id from {0} to {1}; group index from {2} to 0", num, this._imageAdUnitIdIndex, num1);
			Debug.Log(str);
		}
		return true;
	}

	internal bool ResetVideoAdUnitId()
	{
		int num = this._videoAdUnitIdIndex;
		string videoInterstitialUnitId = this.VideoInterstitialUnitId;
		int num1 = this._videoIdGroupIndex;
		this._videoAdUnitIdIndex = 0;
		this._videoIdGroupIndex = 0;
		if (Defs.IsDeveloperBuild)
		{
			string str = string.Format("Resetting video group from {0} to {1}", num1, this._videoIdGroupIndex);
			Debug.Log(str);
		}
		return true;
	}

	public void ShowImageInterstitial(string context)
	{
	}

	public void ShowVideoInterstitial(string context, Action completeHandler)
	{
	}

	public void ShowVideoInterstitial(string context)
	{
		this.ShowVideoInterstitial(context, () => {
		});
	}

	internal bool SwitchImageAdUnitId()
	{
		int num = this._imageAdUnitIdIndex;
		string imageInterstitialUnitId = this.ImageInterstitialUnitId;
		this._imageAdUnitIdIndex++;
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Switching image ad unit id from {0} ({1}) to {2} ({3})", new object[] { num, MobileAdManager.RemovePrefix(imageInterstitialUnitId), this._imageAdUnitIdIndex, MobileAdManager.RemovePrefix(this.ImageInterstitialUnitId) }));
		}
		return (PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count == 0 ? true : this._imageAdUnitIdIndex % PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count == 0);
	}

	internal bool SwitchImageIdGroup()
	{
		int num = this._imageIdGroupIndex;
		List<string> list = this.AdmobImageAdUnitIds.Select<string, string>(new Func<string, string>(MobileAdManager.RemovePrefix)).ToList<string>();
		string str = Json.Serialize(list);
		this._imageIdGroupIndex++;
		this._imageAdUnitIdIndex = 0;
		List<string> strs = this.AdmobImageAdUnitIds.Select<string, string>(new Func<string, string>(MobileAdManager.RemovePrefix)).ToList<string>();
		string str1 = Json.Serialize(strs);
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Switching image id group from {0} ({1}) to {2} ({3})", new object[] { num, str, this._imageIdGroupIndex, str1 }));
		}
		return (PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count == 0 ? true : this._imageIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count == 0);
	}

	internal bool SwitchVideoAdUnitId()
	{
		int num = this._videoAdUnitIdIndex;
		string videoInterstitialUnitId = this.VideoInterstitialUnitId;
		this._videoAdUnitIdIndex++;
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Switching video ad unit id from {0} ({1}) to {2} ({3}); group index {4}", new object[] { num, MobileAdManager.RemovePrefix(videoInterstitialUnitId), this._videoAdUnitIdIndex, MobileAdManager.RemovePrefix(this.VideoInterstitialUnitId), this._videoIdGroupIndex }));
		}
		return (this.AdmobVideoAdUnitIds.Count == 0 ? true : this._videoAdUnitIdIndex % this.AdmobVideoAdUnitIds.Count == 0);
	}

	internal bool SwitchVideoIdGroup()
	{
		int num = this._videoIdGroupIndex;
		List<string> list = this.AdmobVideoAdUnitIds.Select<string, string>(new Func<string, string>(MobileAdManager.RemovePrefix)).ToList<string>();
		string str = Json.Serialize(list);
		this._videoIdGroupIndex++;
		this._videoAdUnitIdIndex = 0;
		List<string> strs = this.AdmobVideoAdUnitIds.Select<string, string>(new Func<string, string>(MobileAdManager.RemovePrefix)).ToList<string>();
		string str1 = Json.Serialize(strs);
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Switching video id group from {0} ({1}) to {2} ({3})", new object[] { num, str, this._videoIdGroupIndex, str1 }));
		}
		return (PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count == 0 ? true : this._videoIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count == 0);
	}

	public static bool UserPredicate(MobileAdManager.Type adType, bool verbose, bool showToPaying = false, bool showToNew = false)
	{
		bool flag;
		bool flag1;
		bool flag2 = MobileAdManager.IsNewUser();
		bool flag3 = MobileAdManager.IsPayingUser();
		if (adType != MobileAdManager.Type.Video)
		{
			bool flag4 = MobileAdManager.IsLongTimeShowBaner();
			flag = (PromoActionsManager.MobileAdvert == null || !PromoActionsManager.MobileAdvert.ImageEnabled || flag2 && !showToNew || flag3 && !showToPaying ? false : flag4);
			if (verbose)
			{
				Dictionary<string, bool> strs = new Dictionary<string, bool>(6)
				{
					{ "ImageEnabled", (PromoActionsManager.MobileAdvert == null ? false : PromoActionsManager.MobileAdvert.ImageEnabled) },
					{ "isNewUser", flag2 },
					{ "showToNew", showToNew },
					{ "isPayingUser", flag3 },
					{ "showToPaying", showToPaying },
					{ "longTimeShowBanner", flag4 }
				};
				Dictionary<string, bool> strs1 = strs;
				string str = string.Format("AdIsApplicable ({0}): {1}    Details: {2}", adType, flag, Json.Serialize(strs1));
				Debug.Log(str);
			}
		}
		else
		{
			int lobbyLevel = ExpController.LobbyLevel;
			bool flag5 = lobbyLevel >= 3;
			bool flag6 = (PromoActionsManager.MobileAdvert == null ? false : PromoActionsManager.MobileAdvert.VideoEnabled);
			bool flag7 = (PromoActionsManager.MobileAdvert == null ? false : PromoActionsManager.MobileAdvert.VideoShowPaying);
			bool flag8 = (PromoActionsManager.MobileAdvert == null ? false : PromoActionsManager.MobileAdvert.VideoShowNonpaying);
			if (!flag3 || !flag7)
			{
				flag1 = (flag3 ? false : flag8);
			}
			else
			{
				flag1 = true;
			}
			bool flag9 = flag1;
			bool num = PlayerPrefs.GetInt("CountRunMenu", 0) >= 3;
			flag = (!flag6 || !num || !flag9 ? false : flag5);
			if (verbose)
			{
				object[] objArray = new object[] { adType, flag, flag3, null, null, null };
				objArray[3] = (!flag3 ? flag8 : flag7);
				objArray[4] = num;
				objArray[5] = lobbyLevel;
				Debug.LogFormat("AdIsApplicable ({0}): {1}    Paying: {2},  Need to show: {3},  Session count satisfied: {4},  Lobby level: {5}", objArray);
			}
		}
		return flag;
	}

	internal enum SampleGroup
	{
		Unknown,
		Video,
		Image
	}

	public enum State
	{
		None,
		Idle,
		Loaded
	}

	public enum Type
	{
		Image,
		Video
	}
}