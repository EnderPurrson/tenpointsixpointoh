using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Rilisoft
{
	internal sealed class PurchasesSynchronizer
	{
		public const string Filename = "Purchases";

		private readonly List<string> _itemsToBeSaved = new List<string>();

		private static PurchasesSynchronizer _instance;

		private static IEnumerable<string> _allItemIds;

		private EventHandler<PurchasesSavingEventArgs> PurchasesSavingStarted;

		public bool HasItemsToBeSaved
		{
			get
			{
				return this._itemsToBeSaved.Count > 0;
			}
		}

		public static PurchasesSynchronizer Instance
		{
			get
			{
				if (PurchasesSynchronizer._instance == null)
				{
					PurchasesSynchronizer._instance = new PurchasesSynchronizer();
				}
				return PurchasesSynchronizer._instance;
			}
		}

		public ICollection<string> ItemsToBeSaved
		{
			get
			{
				return this._itemsToBeSaved;
			}
		}

		static PurchasesSynchronizer()
		{
		}

		public PurchasesSynchronizer()
		{
		}

		public static IEnumerable<string> AllItemIds()
		{
			if (PurchasesSynchronizer._allItemIds == null)
			{
				Dictionary<string, string>.ValueCollection values = WeaponManager.storeIDtoDefsSNMapping.Values;
				List<string> strs = new List<string>();
				foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
				{
					foreach (List<string> value in keyValuePair.Value)
					{
						strs.AddRange(value);
					}
				}
				IEnumerable<string> values1 = 
					from kv in InAppData.inAppData.Values
					select kv.Value;
				IEnumerable<string> strs1 = 
					from i in Enumerable.Range(1, ExperienceController.maxLevel)
					select string.Concat("currentLevel", i);
				string[] skinsMakerInProfileBought = new string[] { Defs.SkinsMakerInProfileBought, Defs.hungerGamesPurchasedKey, Defs.CaptureFlagPurchasedKey, Defs.smallAsAntKey, Defs.code010110_Key, Defs.UnderwaterKey };
				string[] strArrays = new string[] { "PayingUser" };
				string[] isFacebookLoginRewardaGained = new string[] { Defs.IsFacebookLoginRewardaGained };
				string[] isTwitterLoginRewardaGained = new string[] { Defs.IsTwitterLoginRewardaGained };
				PurchasesSynchronizer._allItemIds = values.Concat<string>(strs).Concat<string>(values1).Concat<string>(strs1).Concat<string>(skinsMakerInProfileBought).Concat<string>(strArrays).Concat<string>(isFacebookLoginRewardaGained).Concat<string>(isTwitterLoginRewardaGained).Concat<string>(WeaponManager.GotchaGuns);
			}
			return PurchasesSynchronizer._allItemIds;
		}

		public void AuthenticateAndSynchronize(Action<bool> callback, bool silent)
		{
			if (!GpgFacade.Instance.IsAuthenticated())
			{
				GpgFacade instance = GpgFacade.Instance;
				instance.Authenticate((bool succeeded) => {
					PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32((silent ? false : !succeeded)));
					if (!succeeded)
					{
						UnityEngine.Debug.LogWarning("Authentication failed.");
					}
					else
					{
						UnityEngine.Debug.Log(string.Format("Authentication succeeded: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state));
						PurchasesSynchronizer.Instance.SynchronizeIfAuthenticated(callback);
					}
				}, silent);
			}
			else
			{
				UnityEngine.Debug.LogFormat("Already authenticated: {0}, {1}, {2}", new object[] { Social.localUser.id, Social.localUser.userName, Social.localUser.state });
				PurchasesSynchronizer.Instance.SynchronizeIfAuthenticated(callback);
			}
		}

		public static IEnumerable<string> GetPurchasesIds()
		{
			IEnumerable<string> strs = 
				from id in PurchasesSynchronizer.AllItemIds()
				where Storager.getInt(id, false) != 0
				select id;
			return strs;
		}

		private void HandleReadBinaryData(ISavedGameMetadata openMetadata, SavedGameRequestStatus readStatus, byte[] data, Action<bool> callback, List<string> traceContext)
		{
			traceContext.Add(string.Format("ReadBinaryData.Callback >: {0:F3}", Time.realtimeSinceStartup));
			try
			{
				data = data ?? new byte[0];
				string str = Encoding.UTF8.GetString(data, 0, (int)data.Length);
				if (openMetadata != null)
				{
					UnityEngine.Debug.LogFormat("====== Read '{0}' {4:F3}: {1} '{2}'    '{3}'", new object[] { "Purchases", readStatus, openMetadata.GetDescription(), str, Time.realtimeSinceStartup });
				}
				if (readStatus == SavedGameRequestStatus.Success)
				{
					traceContext.Add(string.Format("> Deserializing JSON string, characters {0}: {1:F3}", str.Length, Time.realtimeSinceStartup));
					List<object> objs = Json.Deserialize(str) as List<object> ?? new List<object>();
					IEnumerable<string> strs = 
						from i in objs.OfType<string>()
						where !string.IsNullOrEmpty(i)
						select i;
					traceContext.Add(string.Format("< Deserializing JSON string, items {0}: {1:F3}", objs.Count, Time.realtimeSinceStartup));
					List<string> strs1 = new List<string>();
					traceContext.Add(string.Format("> Prepare for saving: {0:F3}", Time.realtimeSinceStartup));
					float single = 0f;
					float single1 = 0f;
					int num = Time.frameCount;
					IEnumerator<string> enumerator = strs.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current;
							if (current == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(current))
							{
								float single2 = Time.realtimeSinceStartup;
								int num1 = Storager.getInt(current, false);
								single = single + (Time.realtimeSinceStartup - single2);
								if (num1 == 0)
								{
									strs1.Add(current);
								}
							}
							this._itemsToBeSaved.Add(current);
						}
					}
					finally
					{
						if (enumerator == null)
						{
						}
						enumerator.Dispose();
					}
					UnityEngine.Debug.LogFormat("Items to be saved: {0}", new object[] { this._itemsToBeSaved.Count });
					traceContext.Add(string.Format("< Prepare for saving (r: {1:F3}, w: {2:F3}): {0:F3}", Time.realtimeSinceStartup, single, single1));
					Storager.RefreshWeaponDigestIfDirty();
					PlayerPrefs.Save();
					WeaponManager.RefreshExpControllers();
					WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(strs1);
					HashSet<string> strs2 = new HashSet<string>(PurchasesSynchronizer.GetPurchasesIds());
					string str1 = Json.Serialize(strs2.ToList<string>());
					strs2.ExceptWith(strs);
					string str2 = Json.Serialize(strs2.ToList<string>());
					UnityEngine.Debug.LogFormat("====== Trying to send new items '{0}' {2:F3}: '{1}'...", new object[] { "Purchases", str2, Time.realtimeSinceStartup });
					if (strs2.Count == 0)
					{
						UnityEngine.Debug.LogFormat("====== Nothing to write '{0}' {1:F3}", new object[] { "Purchases", Time.realtimeSinceStartup });
						traceContext.Add(string.Format("> ReadBinaryData.InnerCallback(true): {0:F3}", Time.realtimeSinceStartup));
						try
						{
							callback(true);
						}
						catch (Exception exception1)
						{
							UnityEngine.Debug.LogException(exception1);
						}
						traceContext.Add(string.Format("< ReadBinaryData.InnerCallback(true): {0:F3}", Time.realtimeSinceStartup));
					}
					else if (openMetadata != null)
					{
						byte[] bytes = Encoding.UTF8.GetBytes(str1);
						string str3 = string.Format("Added by '{0}': {1}", SystemInfo.deviceModel, str2.Substring(0, Math.Min(32, str2.Length)));
						traceContext.Add(string.Format("> CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
						SavedGameMetadataUpdate savedGameMetadataUpdate = (new SavedGameMetadataUpdate.Builder()).WithUpdatedDescription(str3).Build();
						PlayGamesPlatform.Instance.SavedGame.CommitUpdate(openMetadata, savedGameMetadataUpdate, bytes, (SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata) => {
							UnityEngine.Debug.LogFormat("====== Written '{0}': {1} '{2}'    '{3}'", new object[] { "Purchases", writeStatus, closeMetadata.GetDescription(), str1 });
							traceContext.Add(string.Format("CommitUpdate.Callback >: {0:F3}", Time.realtimeSinceStartup));
							try
							{
								try
								{
									callback(writeStatus == SavedGameRequestStatus.Success);
								}
								catch (Exception exception)
								{
									UnityEngine.Debug.LogException(exception);
								}
							}
							finally
							{
								traceContext.Add(string.Format("CommitUpdate.Callback <: {0:F3}", Time.realtimeSinceStartup));
								if (Defs.IsDeveloperBuild)
								{
									UnityEngine.Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[] { "Purchases", Json.Serialize(traceContext) });
								}
							}
						});
						traceContext.Add(string.Format("< CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
					}
				}
				else
				{
					traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(readStatus): {0:F3}", Time.realtimeSinceStartup));
					try
					{
						callback(false);
					}
					catch (Exception exception2)
					{
						UnityEngine.Debug.LogException(exception2);
					}
					traceContext.Add(string.Format("< OpenWithManualConflictResolution.InnerCallback(readStatus): {0:F3}", Time.realtimeSinceStartup));
				}
			}
			finally
			{
				traceContext.Add(string.Format("ReadBinaryData.Callback <: {0:F3}", Time.realtimeSinceStartup));
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[] { "Purchases", Json.Serialize(traceContext) });
				}
			}
		}

		[DebuggerHidden]
		public IEnumerator SavePendingItemsToStorager()
		{
			PurchasesSynchronizer.u003cSavePendingItemsToStorageru003ec__Iterator17E variable = null;
			return variable;
		}

		[DebuggerHidden]
		internal IEnumerator SimulateSynchronization(Action<bool> callback)
		{
			PurchasesSynchronizer.u003cSimulateSynchronizationu003ec__Iterator17F variable = null;
			return variable;
		}

		public void SynchronizeAmazonPurchases()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				UnityEngine.Debug.LogWarning("SynchronizeAmazonPurchases() is not implemented for current target.");
				return;
			}
			if (!AGSClient.IsServiceReady())
			{
				UnityEngine.Debug.LogWarning("SynchronizeAmazonPurchases(): service is not ready.");
				return;
			}
			AGSWhispersyncClient.Synchronize();
			using (AGSGameDataMap gameData = AGSWhispersyncClient.GetGameData())
			{
				if (gameData != null)
				{
					using (AGSSyncableStringSet stringSet = gameData.GetStringSet("purchases"))
					{
						List<string> list = (
							from s in stringSet.GetValues()
							select s.GetValue()).ToList<string>();
						UnityEngine.Debug.Log(string.Concat("Trying to sync purchases cloud -> local:    ", Json.Serialize(list)));
						List<string> strs = new List<string>();
						foreach (string str in list)
						{
							if (Storager.getInt(str, false) == 0 && (str == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(str)))
							{
								strs.Add(str);
							}
							this._itemsToBeSaved.Add(str);
						}
						string[] array = PurchasesSynchronizer.GetPurchasesIds().ToArray<string>();
						UnityEngine.Debug.Log(string.Concat("Trying to sync purchases local -> cloud:    ", Json.Serialize(array)));
						string[] strArrays = array;
						for (int i = 0; i < (int)strArrays.Length; i++)
						{
							stringSet.Add(strArrays[i]);
						}
						AGSWhispersyncClient.Synchronize();
						WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(strs);
					}
				}
				else
				{
					UnityEngine.Debug.LogWarning("dataMap == null");
				}
			}
		}

		public bool SynchronizeIfAuthenticated(Action<bool> callback)
		{
			if (!GpgFacade.Instance.IsAuthenticated())
			{
				return false;
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			this.SynchronizeIfAuthenticatedWithSavedGamesService(callback);
			return true;
		}

		private void SynchronizeIfAuthenticatedWithSavedGamesService(Action<bool> callback)
		{
			Func<ItemRecord, string> func = null;
			Func<ItemRecord, string> func1 = null;
			if (PlayGamesPlatform.Instance.SavedGame == null)
			{
				UnityEngine.Debug.LogWarning("Saved game client is null.");
				callback(false);
			}
			List<string> strs4 = new List<string>();
			Action<SavedGameRequestStatus, ISavedGameMetadata> action = (SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata) => {
				strs4.Add(string.Format("OpenWithManualConflictResolution.CompletedCallback >: {0:F3}", Time.realtimeSinceStartup));
				try
				{
					UnityEngine.Debug.LogFormat("====== Open '{0}' {3:F3}: {1} '{2}'", new object[] { "Purchases", openStatus, openMetadata.GetDescription(), Time.realtimeSinceStartup });
					if (openStatus == SavedGameRequestStatus.Success)
					{
						UnityEngine.Debug.LogFormat("====== Trying to read '{0}' {2:F3}: '{1}'...", new object[] { "Purchases", openMetadata.GetDescription(), Time.realtimeSinceStartup });
						strs4.Add(string.Format("> ReadBinaryData: {0:F3}", Time.realtimeSinceStartup));
						PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(openMetadata, (SavedGameRequestStatus readStatus, byte[] data) => this.HandleReadBinaryData(openMetadata, readStatus, data, callback, strs4));
						strs4.Add(string.Format("< ReadBinaryData: {0:F3}", Time.realtimeSinceStartup));
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[] { "Purchases", Json.Serialize(strs4) });
						}
					}
					else
					{
						strs4.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(openStatus): {0:F3}", Time.realtimeSinceStartup));
						callback(false);
						strs4.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(openStatus): {0:F3}", Time.realtimeSinceStartup));
					}
				}
				finally
				{
					strs4.Add(string.Format("OpenWithManualConflictResolution.CompletedCallback <: {0:F3}", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[] { "Purchases", Json.Serialize(strs4) });
					}
				}
			};
			ConflictCallback conflictCallback = (IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData) => {
				strs4.Add(string.Format("OpenWithManualConflictResolution.ConflictCallback: {0:F3} >", Time.realtimeSinceStartup));
				try
				{
					string str = Encoding.UTF8.GetString(originalData);
					string str1 = Encoding.UTF8.GetString(unmergedData);
					HashSet<string> strs = new HashSet<string>((Json.Deserialize(str) as List<object> ?? new List<object>()).Select<object, string>(new Func<object, string>(Convert.ToString)));
					HashSet<string> strs1 = new HashSet<string>((Json.Deserialize(str1) as List<object> ?? new List<object>()).Select<object, string>(new Func<object, string>(Convert.ToString)));
					if (strs.IsSupersetOf(strs1))
					{
						resolver.ChooseMetadata(original);
						UnityEngine.Debug.LogFormat("====== Fully resolved using original metadata '{0}': '{1}'", new object[] { "Purchases", original.GetDescription() });
						callback(true);
					}
					else if (!strs1.IsSupersetOf(strs))
					{
						ISavedGameMetadata savedGameMetadatum = null;
						if (strs.Count <= strs1.Count)
						{
							savedGameMetadatum = unmerged;
							resolver.ChooseMetadata(savedGameMetadatum);
							UnityEngine.Debug.LogFormat("====== Partially resolved using unmerged metadata '{0}': '{1}'", new object[] { "Purchases", unmerged.GetDescription() });
						}
						else
						{
							savedGameMetadatum = original;
							resolver.ChooseMetadata(savedGameMetadatum);
							UnityEngine.Debug.LogFormat("====== Partially resolved using original metadata '{0}': '{1}'", new object[] { "Purchases", original.GetDescription() });
						}
						HashSet<string> strs2 = new HashSet<string>(strs);
						strs2.UnionWith(strs1);
						Dictionary<string, string> dictionary = null;
						try
						{
							IEnumerable<ItemRecord> itemRecords = 
								from r in ItemDb.allRecords
								where (r.StorageId == null ? false : strs2.Contains(r.StorageId))
								select r;
							if (func == null)
							{
								func = (ItemRecord rec) => rec.StorageId;
							}
							Func<ItemRecord, string> u003cu003ef_amu0024cache3 = func;
							if (func1 == null)
							{
								func1 = (ItemRecord rec) => rec.Tag;
							}
							dictionary = itemRecords.ToDictionary<ItemRecord, string, string>(u003cu003ef_amu0024cache3, func1);
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogError(string.Concat("exception in initializing storageIdsToTagsOfItemsToBeSaved: ", exception));
						}
						List<string> strs3 = new List<string>();
						foreach (string str2 in strs2)
						{
							int num = Storager.getInt(str2, false);
							Storager.setInt(str2, 1, false);
							if (num == 0 && (str2 == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(str2)))
							{
								strs3.Add(str2);
							}
							try
							{
								if (num == 0 && dictionary.ContainsKey(str2) && WeaponManager.RemoveGunFromAllTryGunRelated(dictionary[str2]))
								{
									try
									{
										if (!FriendsController.useBuffSystem)
										{
											KillRateCheck.RemoveGunBuff();
										}
										else
										{
											BuffSystem.instance.RemoveGunBuff();
										}
									}
									catch (Exception exception1)
									{
										UnityEngine.Debug.LogError(string.Concat("exception in removing buff (storageIdsToTagsOfItemsToBeSaved): ", exception1));
									}
								}
							}
							catch (Exception exception2)
							{
								UnityEngine.Debug.LogError(string.Concat("exception in remvoing try guns storageIdsToTagsOfItemsToBeSaved: ", exception2));
							}
						}
						PlayerPrefs.Save();
						int num1 = (ExperienceController.sharedController == null ? 1 : ExperienceController.sharedController.currentLevel);
						WeaponManager.RefreshExpControllers();
						ExperienceController.SendAnalyticsForLevelsFromCloud(num1);
						WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(strs3);
						string str3 = Json.Serialize((new HashSet<string>(PurchasesSynchronizer.GetPurchasesIds())).ToArray<string>());
						byte[] bytes = Encoding.UTF8.GetBytes(str3);
						string str4 = string.Format("Merged by '{0}': '{1}' and '{2}'", SystemInfo.deviceModel, original.GetDescription(), unmerged.GetDescription());
						SavedGameMetadataUpdate savedGameMetadataUpdate = (new SavedGameMetadataUpdate.Builder()).WithUpdatedDescription(str4).Build();
						strs4.Add(string.Format("> CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
						PlayGamesPlatform.Instance.SavedGame.CommitUpdate(savedGameMetadatum, savedGameMetadataUpdate, bytes, (SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata) => {
							strs4.Add(string.Format("CommitUpdate.Callback >: {0:F3}", Time.realtimeSinceStartup));
							try
							{
								UnityEngine.Debug.LogFormat("====== Written '{0}': {1} '{2}'    '{3}'", new object[] { "Purchases", writeStatus, closeMetadata.GetDescription(), str3 });
								callback(writeStatus == SavedGameRequestStatus.Success);
							}
							finally
							{
								strs4.Add(string.Format("CommitUpdate.Callback <: {0:F3}", Time.realtimeSinceStartup));
								if (Defs.IsDeveloperBuild)
								{
									UnityEngine.Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[] { "Purchases", Json.Serialize(strs4) });
								}
							}
						});
						strs4.Add(string.Format("< CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
					}
					else
					{
						resolver.ChooseMetadata(unmerged);
						UnityEngine.Debug.LogFormat("====== Fully resolved using unmerged metadata '{0}': '{1}'", new object[] { "Purchases", unmerged.GetDescription() });
						callback(true);
					}
				}
				finally
				{
					strs4.Add(string.Format("OpenWithManualConflictResolution.ConflictCallback: {0:F3} <", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[] { "Purchases", Json.Serialize(strs4) });
					}
				}
			};
			UnityEngine.Debug.LogFormat("====== Trying to open '{0}'...", new object[] { "Purchases" });
			strs4.Add(string.Format("> OpenWithManualConflictResolution: {0:F3}", Time.realtimeSinceStartup));
			PlayGamesPlatform.Instance.SavedGame.OpenWithManualConflictResolution("Purchases", DataSource.ReadNetworkOnly, true, conflictCallback, action);
			strs4.Add(string.Format("< OpenWithManualConflictResolution: {0:F3}", Time.realtimeSinceStartup));
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[] { "Purchases", Json.Serialize(strs4) });
			}
		}

		public event EventHandler<PurchasesSavingEventArgs> PurchasesSavingStarted
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.PurchasesSavingStarted += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.PurchasesSavingStarted -= value;
			}
		}
	}
}