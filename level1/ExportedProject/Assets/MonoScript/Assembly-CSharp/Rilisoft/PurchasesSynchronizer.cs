using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class PurchasesSynchronizer
	{
		public const string Filename = "Purchases";

		private readonly List<string> _itemsToBeSaved = new List<string>();

		private static PurchasesSynchronizer _instance;

		private static IEnumerable<string> _allItemIds;

		public bool HasItemsToBeSaved
		{
			get
			{
				return _itemsToBeSaved.Count > 0;
			}
		}

		public ICollection<string> ItemsToBeSaved
		{
			get
			{
				return _itemsToBeSaved;
			}
		}

		public static PurchasesSynchronizer Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new PurchasesSynchronizer();
				}
				return _instance;
			}
		}

		public event EventHandler<PurchasesSavingEventArgs> PurchasesSavingStarted;

		public static IEnumerable<string> AllItemIds()
		{
			if (_allItemIds == null)
			{
				Dictionary<string, string>.ValueCollection values = WeaponManager.storeIDtoDefsSNMapping.Values;
				List<string> list = new List<string>();
				foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> item in Wear.wear)
				{
					foreach (List<string> item2 in item.Value)
					{
						list.AddRange(item2);
					}
				}
				IEnumerable<string> second = InAppData.inAppData.Values.Select((KeyValuePair<string, string> kv) => kv.Value);
				IEnumerable<string> second2 = from i in Enumerable.Range(1, ExperienceController.maxLevel)
					select "currentLevel" + i;
				string[] second3 = new string[6]
				{
					Defs.SkinsMakerInProfileBought,
					Defs.hungerGamesPurchasedKey,
					Defs.CaptureFlagPurchasedKey,
					Defs.smallAsAntKey,
					Defs.code010110_Key,
					Defs.UnderwaterKey
				};
				string[] second4 = new string[1] { "PayingUser" };
				string[] second5 = new string[1] { Defs.IsFacebookLoginRewardaGained };
				string[] second6 = new string[1] { Defs.IsTwitterLoginRewardaGained };
				_allItemIds = values.Concat(list).Concat(second).Concat(second2)
					.Concat(second3)
					.Concat(second4)
					.Concat(second5)
					.Concat(second6)
					.Concat(WeaponManager.GotchaGuns);
			}
			return _allItemIds;
		}

		public static IEnumerable<string> GetPurchasesIds()
		{
			IEnumerable<string> source = AllItemIds();
			return source.Where((string id) => Storager.getInt(id, false) != 0);
		}

		public void SynchronizeAmazonPurchases()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogWarning("SynchronizeAmazonPurchases() is not implemented for current target.");
				return;
			}
			if (!AGSClient.IsServiceReady())
			{
				Debug.LogWarning("SynchronizeAmazonPurchases(): service is not ready.");
				return;
			}
			AGSWhispersyncClient.Synchronize();
			using (AGSGameDataMap aGSGameDataMap = AGSWhispersyncClient.GetGameData())
			{
				if (aGSGameDataMap == null)
				{
					Debug.LogWarning("dataMap == null");
					return;
				}
				using (AGSSyncableStringSet aGSSyncableStringSet = aGSGameDataMap.GetStringSet("purchases"))
				{
					List<string> list = (from s in aGSSyncableStringSet.GetValues()
						select s.GetValue()).ToList();
					Debug.Log("Trying to sync purchases cloud -> local:    " + Json.Serialize(list));
					List<string> list2 = new List<string>();
					foreach (string item in list)
					{
						if (Storager.getInt(item, false) == 0 && (item == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(item)))
						{
							list2.Add(item);
						}
						_itemsToBeSaved.Add(item);
					}
					string[] array = GetPurchasesIds().ToArray();
					Debug.Log("Trying to sync purchases local -> cloud:    " + Json.Serialize(array));
					string[] array2 = array;
					foreach (string val in array2)
					{
						aGSSyncableStringSet.Add(val);
					}
					AGSWhispersyncClient.Synchronize();
					WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(list2);
				}
			}
		}

		public void AuthenticateAndSynchronize(Action<bool> callback, bool silent)
		{
			if (GpgFacade.Instance.IsAuthenticated())
			{
				Debug.LogFormat("Already authenticated: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
				Instance.SynchronizeIfAuthenticated(callback);
				return;
			}
			GpgFacade.Instance.Authenticate(delegate(bool succeeded)
			{
				bool value = !silent && !succeeded;
				PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(value));
				if (succeeded)
				{
					string message = string.Format("Authentication succeeded: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
					Debug.Log(message);
					Instance.SynchronizeIfAuthenticated(callback);
				}
				else
				{
					Debug.LogWarning("Authentication failed.");
				}
			}, silent);
		}

		private void HandleReadBinaryData(ISavedGameMetadata openMetadata, SavedGameRequestStatus readStatus, byte[] data, Action<bool> callback, List<string> traceContext)
		{
			traceContext.Add(string.Format("ReadBinaryData.Callback >: {0:F3}", Time.realtimeSinceStartup));
			try
			{
				data = data ?? new byte[0];
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				if (openMetadata != null)
				{
					Debug.LogFormat("====== Read '{0}' {4:F3}: {1} '{2}'    '{3}'", "Purchases", readStatus, openMetadata.GetDescription(), @string, Time.realtimeSinceStartup);
				}
				if (readStatus != SavedGameRequestStatus.Success)
				{
					traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(readStatus): {0:F3}", Time.realtimeSinceStartup));
					try
					{
						callback(false);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
					traceContext.Add(string.Format("< OpenWithManualConflictResolution.InnerCallback(readStatus): {0:F3}", Time.realtimeSinceStartup));
					return;
				}
				traceContext.Add(string.Format("> Deserializing JSON string, characters {0}: {1:F3}", @string.Length, Time.realtimeSinceStartup));
				List<object> list = (Json.Deserialize(@string) as List<object>) ?? new List<object>();
				IEnumerable<string> enumerable = from i in list.OfType<string>()
					where !string.IsNullOrEmpty(i)
					select i;
				traceContext.Add(string.Format("< Deserializing JSON string, items {0}: {1:F3}", list.Count, Time.realtimeSinceStartup));
				List<string> list2 = new List<string>();
				traceContext.Add(string.Format("> Prepare for saving: {0:F3}", Time.realtimeSinceStartup));
				float num = 0f;
				float num2 = 0f;
				int frameCount = Time.frameCount;
				foreach (string item in enumerable)
				{
					if (item == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(item))
					{
						float realtimeSinceStartup = Time.realtimeSinceStartup;
						int @int = Storager.getInt(item, false);
						num += Time.realtimeSinceStartup - realtimeSinceStartup;
						if (@int == 0)
						{
							list2.Add(item);
						}
					}
					_itemsToBeSaved.Add(item);
				}
				Debug.LogFormat("Items to be saved: {0}", _itemsToBeSaved.Count);
				traceContext.Add(string.Format("< Prepare for saving (r: {1:F3}, w: {2:F3}): {0:F3}", Time.realtimeSinceStartup, num, num2));
				Storager.RefreshWeaponDigestIfDirty();
				PlayerPrefs.Save();
				WeaponManager.RefreshExpControllers();
				WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(list2);
				HashSet<string> hashSet = new HashSet<string>(GetPurchasesIds());
				string outputString = Json.Serialize(hashSet.ToList());
				hashSet.ExceptWith(enumerable);
				string text = Json.Serialize(hashSet.ToList());
				Debug.LogFormat("====== Trying to send new items '{0}' {2:F3}: '{1}'...", "Purchases", text, Time.realtimeSinceStartup);
				if (hashSet.Count == 0)
				{
					Debug.LogFormat("====== Nothing to write '{0}' {1:F3}", "Purchases", Time.realtimeSinceStartup);
					traceContext.Add(string.Format("> ReadBinaryData.InnerCallback(true): {0:F3}", Time.realtimeSinceStartup));
					try
					{
						callback(true);
					}
					catch (Exception exception2)
					{
						Debug.LogException(exception2);
					}
					traceContext.Add(string.Format("< ReadBinaryData.InnerCallback(true): {0:F3}", Time.realtimeSinceStartup));
				}
				else
				{
					if (openMetadata == null)
					{
						return;
					}
					byte[] bytes = Encoding.UTF8.GetBytes(outputString);
					string description = string.Format("Added by '{0}': {1}", SystemInfo.deviceModel, text.Substring(0, Math.Min(32, text.Length)));
					traceContext.Add(string.Format("> CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
					SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
					PlayGamesPlatform.Instance.SavedGame.CommitUpdate(openMetadata, updateForMetadata, bytes, delegate(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
					{
						Debug.LogFormat("====== Written '{0}': {1} '{2}'    '{3}'", "Purchases", writeStatus, closeMetadata.GetDescription(), outputString);
						traceContext.Add(string.Format("CommitUpdate.Callback >: {0:F3}", Time.realtimeSinceStartup));
						try
						{
							callback(writeStatus == SavedGameRequestStatus.Success);
						}
						catch (Exception exception3)
						{
							Debug.LogException(exception3);
						}
						finally
						{
							traceContext.Add(string.Format("CommitUpdate.Callback <: {0:F3}", Time.realtimeSinceStartup));
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
							}
						}
					});
					traceContext.Add(string.Format("< CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
				}
			}
			finally
			{
				traceContext.Add(string.Format("ReadBinaryData.Callback <: {0:F3}", Time.realtimeSinceStartup));
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
				}
			}
		}

		private void SynchronizeIfAuthenticatedWithSavedGamesService(Action<bool> callback)
		{
			if (PlayGamesPlatform.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				callback(false);
			}
			List<string> traceContext = new List<string>();
			Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback = delegate(SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata)
			{
				traceContext.Add(string.Format("OpenWithManualConflictResolution.CompletedCallback >: {0:F3}", Time.realtimeSinceStartup));
				try
				{
					Debug.LogFormat("====== Open '{0}' {3:F3}: {1} '{2}'", "Purchases", openStatus, openMetadata.GetDescription(), Time.realtimeSinceStartup);
					if (openStatus != SavedGameRequestStatus.Success)
					{
						traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(openStatus): {0:F3}", Time.realtimeSinceStartup));
						callback(false);
						traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(openStatus): {0:F3}", Time.realtimeSinceStartup));
					}
					else
					{
						Debug.LogFormat("====== Trying to read '{0}' {2:F3}: '{1}'...", "Purchases", openMetadata.GetDescription(), Time.realtimeSinceStartup);
						traceContext.Add(string.Format("> ReadBinaryData: {0:F3}", Time.realtimeSinceStartup));
						PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(openMetadata, delegate(SavedGameRequestStatus readStatus, byte[] data)
						{
							HandleReadBinaryData(openMetadata, readStatus, data, callback, traceContext);
						});
						traceContext.Add(string.Format("< ReadBinaryData: {0:F3}", Time.realtimeSinceStartup));
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
						}
					}
				}
				finally
				{
					traceContext.Add(string.Format("OpenWithManualConflictResolution.CompletedCallback <: {0:F3}", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
					}
				}
			};
			ConflictCallback conflictCallback = delegate(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				traceContext.Add(string.Format("OpenWithManualConflictResolution.ConflictCallback: {0:F3} >", Time.realtimeSinceStartup));
				try
				{
					string @string = Encoding.UTF8.GetString(originalData);
					string string2 = Encoding.UTF8.GetString(unmergedData);
					HashSet<string> hashSet = new HashSet<string>(((Json.Deserialize(@string) as List<object>) ?? new List<object>()).Select(Convert.ToString));
					HashSet<string> hashSet2 = new HashSet<string>(((Json.Deserialize(string2) as List<object>) ?? new List<object>()).Select(Convert.ToString));
					if (hashSet.IsSupersetOf(hashSet2))
					{
						resolver.ChooseMetadata(original);
						Debug.LogFormat("====== Fully resolved using original metadata '{0}': '{1}'", "Purchases", original.GetDescription());
						callback(true);
					}
					else if (hashSet2.IsSupersetOf(hashSet))
					{
						resolver.ChooseMetadata(unmerged);
						Debug.LogFormat("====== Fully resolved using unmerged metadata '{0}': '{1}'", "Purchases", unmerged.GetDescription());
						callback(true);
					}
					else
					{
						ISavedGameMetadata savedGameMetadata = null;
						if (hashSet.Count > hashSet2.Count)
						{
							savedGameMetadata = original;
							resolver.ChooseMetadata(savedGameMetadata);
							Debug.LogFormat("====== Partially resolved using original metadata '{0}': '{1}'", "Purchases", original.GetDescription());
						}
						else
						{
							savedGameMetadata = unmerged;
							resolver.ChooseMetadata(savedGameMetadata);
							Debug.LogFormat("====== Partially resolved using unmerged metadata '{0}': '{1}'", "Purchases", unmerged.GetDescription());
						}
						HashSet<string> mergedItems = new HashSet<string>(hashSet);
						mergedItems.UnionWith(hashSet2);
						Dictionary<string, string> dictionary = null;
						try
						{
							dictionary = ItemDb.allRecords.Where((ItemRecord r) => r.StorageId != null && mergedItems.Contains(r.StorageId)).ToDictionary((ItemRecord rec) => rec.StorageId, (ItemRecord rec) => rec.Tag);
						}
						catch (Exception ex)
						{
							Debug.LogError("exception in initializing storageIdsToTagsOfItemsToBeSaved: " + ex);
						}
						List<string> list = new List<string>();
						foreach (string item in mergedItems)
						{
							int @int = Storager.getInt(item, false);
							Storager.setInt(item, 1, false);
							if (@int == 0 && (item == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(item)))
							{
								list.Add(item);
							}
							try
							{
								if (@int == 0 && dictionary.ContainsKey(item) && WeaponManager.RemoveGunFromAllTryGunRelated(dictionary[item]))
								{
									try
									{
										if (FriendsController.useBuffSystem)
										{
											BuffSystem.instance.RemoveGunBuff();
										}
										else
										{
											KillRateCheck.RemoveGunBuff();
										}
									}
									catch (Exception ex2)
									{
										Debug.LogError("exception in removing buff (storageIdsToTagsOfItemsToBeSaved): " + ex2);
									}
								}
							}
							catch (Exception ex3)
							{
								Debug.LogError("exception in remvoing try guns storageIdsToTagsOfItemsToBeSaved: " + ex3);
							}
						}
						PlayerPrefs.Save();
						int levelBefore = ((!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
						WeaponManager.RefreshExpControllers();
						ExperienceController.SendAnalyticsForLevelsFromCloud(levelBefore);
						WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(list);
						HashSet<string> source = new HashSet<string>(GetPurchasesIds());
						string outputString = Json.Serialize(source.ToArray());
						byte[] bytes = Encoding.UTF8.GetBytes(outputString);
						string description = string.Format("Merged by '{0}': '{1}' and '{2}'", SystemInfo.deviceModel, original.GetDescription(), unmerged.GetDescription());
						SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
						traceContext.Add(string.Format("> CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
						PlayGamesPlatform.Instance.SavedGame.CommitUpdate(savedGameMetadata, updateForMetadata, bytes, delegate(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
						{
							traceContext.Add(string.Format("CommitUpdate.Callback >: {0:F3}", Time.realtimeSinceStartup));
							try
							{
								Debug.LogFormat("====== Written '{0}': {1} '{2}'    '{3}'", "Purchases", writeStatus, closeMetadata.GetDescription(), outputString);
								callback(writeStatus == SavedGameRequestStatus.Success);
							}
							finally
							{
								traceContext.Add(string.Format("CommitUpdate.Callback <: {0:F3}", Time.realtimeSinceStartup));
								if (Defs.IsDeveloperBuild)
								{
									Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
								}
							}
						});
						traceContext.Add(string.Format("< CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
					}
				}
				finally
				{
					traceContext.Add(string.Format("OpenWithManualConflictResolution.ConflictCallback: {0:F3} <", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
					}
				}
			};
			Debug.LogFormat("====== Trying to open '{0}'...", "Purchases");
			traceContext.Add(string.Format("> OpenWithManualConflictResolution: {0:F3}", Time.realtimeSinceStartup));
			PlayGamesPlatform.Instance.SavedGame.OpenWithManualConflictResolution("Purchases", DataSource.ReadNetworkOnly, true, conflictCallback, completedCallback);
			traceContext.Add(string.Format("< OpenWithManualConflictResolution: {0:F3}", Time.realtimeSinceStartup));
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
			}
		}

		public IEnumerator SavePendingItemsToStorager()
		{
			TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
			EventHandler<PurchasesSavingEventArgs> handler = this.PurchasesSavingStarted;
			if (handler != null)
			{
				handler(this, new PurchasesSavingEventArgs(promise.get_Task()));
			}
			try
			{
				if (_itemsToBeSaved.Count <= 0)
				{
					yield break;
				}
				if (Application.isEditor)
				{
					yield return new WaitForSeconds(3f);
				}
				float writeTime = 0f;
				Dictionary<string, string> storageIdsToTagsOfItemsToBeSaved = null;
				try
				{
					storageIdsToTagsOfItemsToBeSaved = ItemDb.allRecords.Where((ItemRecord r) => r.StorageId != null && _itemsToBeSaved.Contains(r.StorageId)).ToDictionary((ItemRecord rec) => rec.StorageId, (ItemRecord rec) => rec.Tag);
				}
				catch (Exception e3)
				{
					Debug.LogError("exception in initializing storageIdsToTagsOfItemsToBeSaved: " + e3);
				}
				while (_itemsToBeSaved.Count > 0)
				{
					int index = _itemsToBeSaved.Count - 1;
					string item = _itemsToBeSaved[index];
					float startWrite = Time.realtimeSinceStartup;
					int valueBefore = Storager.getInt(item, false);
					Storager.setInt(item, 1, false);
					try
					{
						if (valueBefore == 0 && storageIdsToTagsOfItemsToBeSaved.ContainsKey(item) && WeaponManager.RemoveGunFromAllTryGunRelated(storageIdsToTagsOfItemsToBeSaved[item]))
						{
							try
							{
								if (FriendsController.useBuffSystem)
								{
									BuffSystem.instance.RemoveGunBuff();
								}
								else
								{
									KillRateCheck.RemoveGunBuff();
								}
							}
							catch (Exception ex)
							{
								Exception e2 = ex;
								Debug.LogError("exception in removing buff (storageIdsToTagsOfItemsToBeSaved): " + e2);
							}
						}
					}
					catch (Exception e)
					{
						Debug.LogError("exception in remove guns storageIdsToTagsOfItemsToBeSaved: " + e);
					}
					writeTime += Time.realtimeSinceStartup - startWrite;
					if (index % 2 == 1)
					{
						yield return null;
					}
					_itemsToBeSaved.RemoveAt(index);
				}
			}
			finally
			{
				promise.TrySetResult(true);
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
			SynchronizeIfAuthenticatedWithSavedGamesService(callback);
			return true;
		}

		internal IEnumerator SimulateSynchronization(Action<bool> callback)
		{
			Debug.Log("Waiting for syncing...");
			yield return new WaitForSeconds(3f);
			List<string> traceContext = new List<string> { string.Format("SimulateSynchronization >: {0:F3}", Time.realtimeSinceStartup) };
			try
			{
				List<string> simulatedInventory = new List<string> { "currentLevel1", "currentLevel2", "currentLevel3", "currentLevel4", "currentLevel5", "BerettaSN", "gravity_2", "IsFacebookLoginRewardaGained" };
				string inputString = Json.Serialize(simulatedInventory);
				byte[] data = Encoding.UTF8.GetBytes(inputString);
				HandleReadBinaryData(null, SavedGameRequestStatus.Success, data, callback, traceContext);
				callback(true);
			}
			finally
			{
				((_003CSimulateSynchronization_003Ec__Iterator17F)this)._003C_003E__Finally0();
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] SimulateSynchronization ({0}): {1}", "Purchases", Json.Serialize(traceContext));
			}
		}
	}
}