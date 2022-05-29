using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class SkinsSynchronizer
	{
		private readonly static SkinsSynchronizer s_instance;

		private EventHandler Updated;

		public static SkinsSynchronizer Instance
		{
			get
			{
				return SkinsSynchronizer.s_instance;
			}
		}

		internal bool Ready
		{
			get
			{
				return true;
			}
		}

		static SkinsSynchronizer()
		{
			SkinsSynchronizer.s_instance = new SkinsSynchronizer();
		}

		private SkinsSynchronizer()
		{
		}

		private void EnsureNotNull(object value, string name)
		{
			if (value == null)
			{
				throw new InvalidOperationException(name ?? string.Empty);
			}
		}

		private SkinsMemento LoadLocalSkins()
		{
			string empty;
			SkinsMemento skinsMemento;
			string str = string.Format(CultureInfo.InvariantCulture, "{0}.LoadLocalSkins()", new object[] { this.GetType().Name });
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				string str1 = PlayerPrefs.GetString("DeletedSkins", string.Empty);
				List<object> objs = Json.Deserialize(str1) as List<object>;
				List<string> strs = (objs == null ? new List<string>() : objs.OfType<string>().ToList<string>());
				string str2 = PlayerPrefs.GetString("User Skins", string.Empty);
				string str3 = PlayerPrefs.GetString("NewUserCape", string.Empty);
				CapeMemento capeMemento = Tools.DeserializeJson<CapeMemento>(str3);
				Dictionary<string, object> strs1 = Json.Deserialize(str2) as Dictionary<string, object> ?? new Dictionary<string, object>();
				List<SkinMemento> skinMementos = new List<SkinMemento>(strs1.Count);
				if (strs1.Count != 0)
				{
					string str4 = PlayerPrefs.GetString("User Name Skins", string.Empty);
					Dictionary<string, object> strs2 = Json.Deserialize(str4) as Dictionary<string, object> ?? new Dictionary<string, object>();
					foreach (KeyValuePair<string, object> keyValuePair in strs1)
					{
						string key = keyValuePair.Key;
						if (!strs2.TryGetValue<string>(key, out empty))
						{
							empty = string.Empty;
						}
						skinMementos.Add(new SkinMemento(key, empty, keyValuePair.Value as string ?? string.Empty));
					}
					skinsMemento = new SkinsMemento(skinMementos, strs, capeMemento);
				}
				else
				{
					UnityEngine.Debug.LogFormat("Deserialized skins are empty: {0}", new object[] { str2 });
					skinsMemento = new SkinsMemento(skinMementos, strs, capeMemento);
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
			return skinsMemento;
		}

		private void OverwriteLocalSkins(SkinsMemento localSkins, SkinsMemento cloudSkins)
		{
			HashSet<string> strs = new HashSet<string>(localSkins.DeletedSkins.Concat<string>(cloudSkins.DeletedSkins));
			Dictionary<string, string> skin = new Dictionary<string, string>(localSkins.Skins.Count);
			Dictionary<string, string> name = new Dictionary<string, string>(localSkins.Skins.Count);
			foreach (SkinMemento skinMemento in cloudSkins.Skins)
			{
				if (!strs.Contains(skinMemento.Id))
				{
					skin[skinMemento.Id] = skinMemento.Skin;
					name[skinMemento.Id] = skinMemento.Name;
				}
			}
			foreach (SkinMemento skin1 in localSkins.Skins)
			{
				if (!strs.Contains(skin1.Id))
				{
					skin[skin1.Id] = skin1.Skin;
					name[skin1.Id] = skin1.Name;
				}
			}
			PlayerPrefs.SetString("User Skins", Json.Serialize(skin));
			PlayerPrefs.SetString("User Name Skins", Json.Serialize(name));
			CapeMemento capeMemento = CapeMemento.ChooseCape(localSkins.Cape, cloudSkins.Cape);
			PlayerPrefs.SetString("NewUserCape", JsonUtility.ToJson(capeMemento));
			this.RefreshGui(skin, name, capeMemento);
			PlayerPrefs.Save();
		}

		public Coroutine Pull()
		{
			if (!this.Ready)
			{
				return null;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return CoroutineRunner.Instance.StartCoroutine(this.SyncGoogleCoroutine(true));
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					this.SyncAmazon();
				}
			}
			return null;
		}

		public Coroutine Push()
		{
			if (!this.Ready)
			{
				return null;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return CoroutineRunner.Instance.StartCoroutine(this.PushGoogleCoroutine());
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					this.SyncAmazon();
				}
			}
			return null;
		}

		[DebuggerHidden]
		internal IEnumerator PushGoogleCoroutine()
		{
			SkinsSynchronizer.u003cPushGoogleCoroutineu003ec__Iterator1D2 variable = null;
			return variable;
		}

		private void RefreshGui(Dictionary<string, string> skins, Dictionary<string, string> skinNames, CapeMemento cape)
		{
			if (ShopNGUIController.sharedShop == null)
			{
				return;
			}
			foreach (KeyValuePair<string, string> skin in skins)
			{
				if (!SkinsController.skinsForPers.ContainsKey(skin.Key))
				{
					Texture2D texture2D = SkinsController.TextureFromString(skin.Value, 64, 32);
					SkinsController.skinsForPers.Add(skin.Key, texture2D);
				}
			}
			foreach (KeyValuePair<string, string> skinName in skinNames)
			{
				SkinsController.skinsNamesForPers[skinName.Key] = skinName.Value;
			}
			SkinsController.capeUserTexture = SkinsController.TextureFromString(cape.Cape, 32, 32);
			ShopNGUIController.sharedShop.ReloadCarousel(null);
		}

		public Coroutine Sync()
		{
			if (!this.Ready)
			{
				return null;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return CoroutineRunner.Instance.StartCoroutine(this.SyncGoogleCoroutine(false));
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					this.SyncAmazon();
				}
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer)
			{
				return null;
			}
			this.SyncSkinsAndCapeIos();
			return null;
		}

		private void SyncAmazon()
		{
			string str = string.Format(CultureInfo.InvariantCulture, "{0}.SyncAmazon()", new object[] { this.GetType().Name });
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				try
				{
					AGSWhispersyncClient.Synchronize();
					using (AGSGameDataMap gameData = AGSWhispersyncClient.GetGameData())
					{
						this.EnsureNotNull(gameData, "dataMap");
						using (AGSSyncableString latestString = gameData.GetLatestString("skinsJson"))
						{
							this.EnsureNotNull(latestString, "skinsJson");
							SkinsMemento skinsMemento = JsonUtility.FromJson<SkinsMemento>(latestString.GetValue() ?? "{}");
							SkinsMemento skinsMemento1 = this.LoadLocalSkins();
							SkinsMemento skinsMemento2 = SkinsMemento.Merge(skinsMemento1, skinsMemento);
							if (Defs.IsDeveloperBuild)
							{
								UnityEngine.Debug.LogFormat("Local skins: {0}", new object[] { skinsMemento1 });
								UnityEngine.Debug.LogFormat("Cloud skins: {0}", new object[] { skinsMemento });
								UnityEngine.Debug.LogFormat("Merged skins: {0}", new object[] { skinsMemento2 });
							}
							int num = skinsMemento2.DeletedSkins.Distinct<string>().Count<string>();
							int num1 = (
								from s in skinsMemento2.Skins
								select s.Id).Distinct<string>().Count<string>();
							long id = skinsMemento2.Cape.Id;
							int num2 = skinsMemento1.DeletedSkins.Distinct<string>().Count<string>();
							int num3 = (
								from s in skinsMemento1.Skins
								select s.Id).Distinct<string>().Count<string>();
							long id1 = skinsMemento1.Cape.Id;
							if ((num2 < num || num3 < num1 ? true : id1 < id))
							{
								this.OverwriteLocalSkins(skinsMemento1, skinsMemento2);
								EventHandler updated = this.Updated;
								if (updated != null)
								{
									updated(this, EventArgs.Empty);
								}
							}
							int num4 = skinsMemento.DeletedSkins.Distinct<string>().Count<string>();
							int num5 = (
								from s in skinsMemento.Skins
								select s.Id).Distinct<string>().Count<string>();
							long id2 = skinsMemento.Cape.Id;
							if ((num4 < num || num5 < num1 ? true : id2 < id))
							{
								latestString.Set(JsonUtility.ToJson(skinsMemento2));
								AGSWhispersyncClient.Synchronize();
							}
						}
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		[DebuggerHidden]
		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			SkinsSynchronizer.u003cSyncGoogleCoroutineu003ec__Iterator1D1 variable = null;
			return variable;
		}

		private void SyncSkinsAndCapeIos()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				!Storager.ICloudAvailable;
			}
		}

		public event EventHandler Updated
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Updated += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Updated -= value;
			}
		}
	}
}