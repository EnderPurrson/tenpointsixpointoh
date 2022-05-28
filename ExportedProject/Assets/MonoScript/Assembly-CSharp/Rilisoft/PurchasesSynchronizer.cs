using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
		[CompilerGenerated]
		private sealed class _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D6
		{
			internal bool silent;

			internal Action<bool> callback;

			internal void _003C_003Em__3AD(bool succeeded)
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
			}
		}

		[CompilerGenerated]
		private sealed class _003CHandleReadBinaryData_003Ec__AnonStorey2D8
		{
			internal List<string> traceContext;

			internal Action<bool> callback;
		}

		[CompilerGenerated]
		private sealed class _003CHandleReadBinaryData_003Ec__AnonStorey2D7
		{
			internal string outputString;

			internal _003CHandleReadBinaryData_003Ec__AnonStorey2D8 _003C_003Ef__ref_0024728;

			internal void _003C_003Em__3AF(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
			{
				Debug.LogFormat("====== Written '{0}': {1} '{2}'    '{3}'", "Purchases", writeStatus, closeMetadata.GetDescription(), outputString);
				_003C_003Ef__ref_0024728.traceContext.Add(string.Format("CommitUpdate.Callback >: {0:F3}", Time.realtimeSinceStartup));
				try
				{
					_003C_003Ef__ref_0024728.callback(writeStatus == SavedGameRequestStatus.Success);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				finally
				{
					_003C_003Ef__ref_0024728.traceContext.Add(string.Format("CommitUpdate.Callback <: {0:F3}", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(_003C_003Ef__ref_0024728.traceContext));
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D9
		{
			private sealed class _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA
			{
				internal ISavedGameMetadata openMetadata;

				internal _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D9 _003C_003Ef__ref_0024729;

				internal void _003C_003Em__3B2(SavedGameRequestStatus readStatus, byte[] data)
				{
					_003C_003Ef__ref_0024729._003C_003Ef__this.HandleReadBinaryData(openMetadata, readStatus, data, _003C_003Ef__ref_0024729.callback, _003C_003Ef__ref_0024729.traceContext);
				}
			}

			private sealed class _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB
			{
				internal HashSet<string> mergedItems;

				internal string outputString;

				internal _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D9 _003C_003Ef__ref_0024729;

				internal bool _003C_003Em__3B3(ItemRecord r)
				{
					return r.StorageId != null && mergedItems.Contains(r.StorageId);
				}

				internal void _003C_003Em__3B6(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
				{
					_003C_003Ef__ref_0024729.traceContext.Add(string.Format("CommitUpdate.Callback >: {0:F3}", Time.realtimeSinceStartup));
					try
					{
						Debug.LogFormat("====== Written '{0}': {1} '{2}'    '{3}'", "Purchases", writeStatus, closeMetadata.GetDescription(), outputString);
						_003C_003Ef__ref_0024729.callback(writeStatus == SavedGameRequestStatus.Success);
					}
					finally
					{
						_003C_003Ef__ref_0024729.traceContext.Add(string.Format("CommitUpdate.Callback <: {0:F3}", Time.realtimeSinceStartup));
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(_003C_003Ef__ref_0024729.traceContext));
						}
					}
				}
			}

			internal List<string> traceContext;

			internal Action<bool> callback;

			internal PurchasesSynchronizer _003C_003Ef__this;

			private static Func<ItemRecord, string> _003C_003Ef__am_0024cache3;

			private static Func<ItemRecord, string> _003C_003Ef__am_0024cache4;

			internal void _003C_003Em__3B0(SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata)
			{
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA = new _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA();
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA._003C_003Ef__ref_0024729 = this;
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA.openMetadata = openMetadata;
				traceContext.Add(string.Format("OpenWithManualConflictResolution.CompletedCallback >: {0:F3}", Time.realtimeSinceStartup));
				try
				{
					Debug.LogFormat("====== Open '{0}' {3:F3}: {1} '{2}'", "Purchases", openStatus, _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA.openMetadata.GetDescription(), Time.realtimeSinceStartup);
					if (openStatus != SavedGameRequestStatus.Success)
					{
						traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(openStatus): {0:F3}", Time.realtimeSinceStartup));
						callback(false);
						traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(openStatus): {0:F3}", Time.realtimeSinceStartup));
						return;
					}
					Debug.LogFormat("====== Trying to read '{0}' {2:F3}: '{1}'...", "Purchases", _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA.openMetadata.GetDescription(), Time.realtimeSinceStartup);
					traceContext.Add(string.Format("> ReadBinaryData: {0:F3}", Time.realtimeSinceStartup));
					PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA.openMetadata, _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DA._003C_003Em__3B2);
					traceContext.Add(string.Format("< ReadBinaryData: {0:F3}", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
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
			}

			internal void _003C_003Em__3B1(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				traceContext.Add(string.Format("OpenWithManualConflictResolution.ConflictCallback: {0:F3} >", Time.realtimeSinceStartup));
				try
				{
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB = new _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB();
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB._003C_003Ef__ref_0024729 = this;
					string @string = Encoding.UTF8.GetString(originalData);
					string string2 = Encoding.UTF8.GetString(unmergedData);
					HashSet<string> hashSet = new HashSet<string>(((Json.Deserialize(@string) as List<object>) ?? new List<object>()).Select(Convert.ToString));
					HashSet<string> hashSet2 = new HashSet<string>(((Json.Deserialize(string2) as List<object>) ?? new List<object>()).Select(Convert.ToString));
					if (hashSet.IsSupersetOf(hashSet2))
					{
						resolver.ChooseMetadata(original);
						Debug.LogFormat("====== Fully resolved using original metadata '{0}': '{1}'", "Purchases", original.GetDescription());
						callback(true);
						return;
					}
					if (hashSet2.IsSupersetOf(hashSet))
					{
						resolver.ChooseMetadata(unmerged);
						Debug.LogFormat("====== Fully resolved using unmerged metadata '{0}': '{1}'", "Purchases", unmerged.GetDescription());
						callback(true);
						return;
					}
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
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB.mergedItems = new HashSet<string>(hashSet);
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB.mergedItems.UnionWith(hashSet2);
					Dictionary<string, string> dictionary = null;
					try
					{
						IEnumerable<ItemRecord> source = ItemDb.allRecords.Where(_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB._003C_003Em__3B3);
						if (_003C_003Ef__am_0024cache3 == null)
						{
							_003C_003Ef__am_0024cache3 = _003C_003Em__3B4;
						}
						Func<ItemRecord, string> keySelector = _003C_003Ef__am_0024cache3;
						if (_003C_003Ef__am_0024cache4 == null)
						{
							_003C_003Ef__am_0024cache4 = _003C_003Em__3B5;
						}
						dictionary = source.ToDictionary(keySelector, _003C_003Ef__am_0024cache4);
					}
					catch (Exception ex)
					{
						Debug.LogError("exception in initializing storageIdsToTagsOfItemsToBeSaved: " + ex);
					}
					List<string> list = new List<string>();
					foreach (string mergedItem in _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB.mergedItems)
					{
						int @int = Storager.getInt(mergedItem, false);
						Storager.setInt(mergedItem, 1, false);
						if (@int == 0 && (mergedItem == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(mergedItem)))
						{
							list.Add(mergedItem);
						}
						try
						{
							if (@int != 0 || !dictionary.ContainsKey(mergedItem) || !WeaponManager.RemoveGunFromAllTryGunRelated(dictionary[mergedItem]))
							{
								continue;
							}
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
					HashSet<string> source2 = new HashSet<string>(GetPurchasesIds());
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB.outputString = Json.Serialize(source2.ToArray());
					byte[] bytes = Encoding.UTF8.GetBytes(_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB.outputString);
					string description = string.Format("Merged by '{0}': '{1}' and '{2}'", SystemInfo.deviceModel, original.GetDescription(), unmerged.GetDescription());
					SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
					traceContext.Add(string.Format("> CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
					PlayGamesPlatform.Instance.SavedGame.CommitUpdate(savedGameMetadata, updateForMetadata, bytes, _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2DB._003C_003Em__3B6);
					traceContext.Add(string.Format("< CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
				}
				finally
				{
					traceContext.Add(string.Format("OpenWithManualConflictResolution.ConflictCallback: {0:F3} <", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(traceContext));
					}
				}
			}

			private static string _003C_003Em__3B4(ItemRecord rec)
			{
				return rec.StorageId;
			}

			private static string _003C_003Em__3B5(ItemRecord rec)
			{
				return rec.Tag;
			}
		}

		public const string Filename = "Purchases";

		private readonly List<string> _itemsToBeSaved = new List<string>();

		private static PurchasesSynchronizer _instance;

		private static IEnumerable<string> _allItemIds;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, string>, string> _003C_003Ef__am_0024cache4;

		[CompilerGenerated]
		private static Func<int, string> _003C_003Ef__am_0024cache5;

		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<AGSSyncableStringElement, string> _003C_003Ef__am_0024cache7;

		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__am_0024cache8;

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
				Dictionary<int, KeyValuePair<string, string>>.ValueCollection values2 = InAppData.inAppData.Values;
				if (_003C_003Ef__am_0024cache4 == null)
				{
					_003C_003Ef__am_0024cache4 = _003CAllItemIds_003Em__3A9;
				}
				IEnumerable<string> second = values2.Select(_003C_003Ef__am_0024cache4);
				IEnumerable<int> source = Enumerable.Range(1, ExperienceController.maxLevel);
				if (_003C_003Ef__am_0024cache5 == null)
				{
					_003C_003Ef__am_0024cache5 = _003CAllItemIds_003Em__3AA;
				}
				IEnumerable<string> second2 = source.Select(_003C_003Ef__am_0024cache5);
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
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003CGetPurchasesIds_003Em__3AB;
			}
			return source.Where(_003C_003Ef__am_0024cache6);
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
					HashSet<AGSSyncableStringElement> values = aGSSyncableStringSet.GetValues();
					if (_003C_003Ef__am_0024cache7 == null)
					{
						_003C_003Ef__am_0024cache7 = _003CSynchronizeAmazonPurchases_003Em__3AC;
					}
					List<string> list = values.Select(_003C_003Ef__am_0024cache7).ToList();
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
			_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D6 _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D = new _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D6();
			_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D.silent = silent;
			_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D.callback = callback;
			if (GpgFacade.Instance.IsAuthenticated())
			{
				Debug.LogFormat("Already authenticated: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
				Instance.SynchronizeIfAuthenticated(_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D.callback);
			}
			else
			{
				GpgFacade.Instance.Authenticate(_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D._003C_003Em__3AD, _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D.silent);
			}
		}

		private void HandleReadBinaryData(ISavedGameMetadata openMetadata, SavedGameRequestStatus readStatus, byte[] data, Action<bool> callback, List<string> traceContext)
		{
			_003CHandleReadBinaryData_003Ec__AnonStorey2D8 _003CHandleReadBinaryData_003Ec__AnonStorey2D = new _003CHandleReadBinaryData_003Ec__AnonStorey2D8();
			_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext = traceContext;
			_003CHandleReadBinaryData_003Ec__AnonStorey2D.callback = callback;
			_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("ReadBinaryData.Callback >: {0:F3}", Time.realtimeSinceStartup));
			try
			{
				_003CHandleReadBinaryData_003Ec__AnonStorey2D7 _003CHandleReadBinaryData_003Ec__AnonStorey2D2 = new _003CHandleReadBinaryData_003Ec__AnonStorey2D7();
				_003CHandleReadBinaryData_003Ec__AnonStorey2D2._003C_003Ef__ref_0024728 = _003CHandleReadBinaryData_003Ec__AnonStorey2D;
				data = data ?? new byte[0];
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				if (openMetadata != null)
				{
					Debug.LogFormat("====== Read '{0}' {4:F3}: {1} '{2}'    '{3}'", "Purchases", readStatus, openMetadata.GetDescription(), @string, Time.realtimeSinceStartup);
				}
				if (readStatus != SavedGameRequestStatus.Success)
				{
					_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(readStatus): {0:F3}", Time.realtimeSinceStartup));
					try
					{
						_003CHandleReadBinaryData_003Ec__AnonStorey2D.callback(false);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
					_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("< OpenWithManualConflictResolution.InnerCallback(readStatus): {0:F3}", Time.realtimeSinceStartup));
					return;
				}
				_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("> Deserializing JSON string, characters {0}: {1:F3}", @string.Length, Time.realtimeSinceStartup));
				List<object> list = (Json.Deserialize(@string) as List<object>) ?? new List<object>();
				IEnumerable<string> source = list.OfType<string>();
				if (_003C_003Ef__am_0024cache8 == null)
				{
					_003C_003Ef__am_0024cache8 = _003CHandleReadBinaryData_003Em__3AE;
				}
				IEnumerable<string> enumerable = source.Where(_003C_003Ef__am_0024cache8);
				_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("< Deserializing JSON string, items {0}: {1:F3}", list.Count, Time.realtimeSinceStartup));
				List<string> list2 = new List<string>();
				_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("> Prepare for saving: {0:F3}", Time.realtimeSinceStartup));
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
				_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("< Prepare for saving (r: {1:F3}, w: {2:F3}): {0:F3}", Time.realtimeSinceStartup, num, num2));
				Storager.RefreshWeaponDigestIfDirty();
				PlayerPrefs.Save();
				WeaponManager.RefreshExpControllers();
				WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(list2);
				HashSet<string> hashSet = new HashSet<string>(GetPurchasesIds());
				_003CHandleReadBinaryData_003Ec__AnonStorey2D2.outputString = Json.Serialize(hashSet.ToList());
				hashSet.ExceptWith(enumerable);
				string text = Json.Serialize(hashSet.ToList());
				Debug.LogFormat("====== Trying to send new items '{0}' {2:F3}: '{1}'...", "Purchases", text, Time.realtimeSinceStartup);
				if (hashSet.Count == 0)
				{
					Debug.LogFormat("====== Nothing to write '{0}' {1:F3}", "Purchases", Time.realtimeSinceStartup);
					_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("> ReadBinaryData.InnerCallback(true): {0:F3}", Time.realtimeSinceStartup));
					try
					{
						_003CHandleReadBinaryData_003Ec__AnonStorey2D.callback(true);
					}
					catch (Exception exception2)
					{
						Debug.LogException(exception2);
					}
					_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("< ReadBinaryData.InnerCallback(true): {0:F3}", Time.realtimeSinceStartup));
				}
				else if (openMetadata != null)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(_003CHandleReadBinaryData_003Ec__AnonStorey2D2.outputString);
					string description = string.Format("Added by '{0}': {1}", SystemInfo.deviceModel, text.Substring(0, Math.Min(32, text.Length)));
					_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("> CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
					SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
					PlayGamesPlatform.Instance.SavedGame.CommitUpdate(openMetadata, updateForMetadata, bytes, _003CHandleReadBinaryData_003Ec__AnonStorey2D2._003C_003Em__3AF);
					_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("< CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
				}
			}
			finally
			{
				_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext.Add(string.Format("ReadBinaryData.Callback <: {0:F3}", Time.realtimeSinceStartup));
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(_003CHandleReadBinaryData_003Ec__AnonStorey2D.traceContext));
				}
			}
		}

		private void SynchronizeIfAuthenticatedWithSavedGamesService(Action<bool> callback)
		{
			_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D9 _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D = new _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D9();
			_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.callback = callback;
			_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Ef__this = this;
			if (PlayGamesPlatform.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.callback(false);
			}
			_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.traceContext = new List<string>();
			Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback = _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Em__3B0;
			ConflictCallback conflictCallback = _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Em__3B1;
			Debug.LogFormat("====== Trying to open '{0}'...", "Purchases");
			_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.traceContext.Add(string.Format("> OpenWithManualConflictResolution: {0:F3}", Time.realtimeSinceStartup));
			PlayGamesPlatform.Instance.SavedGame.OpenWithManualConflictResolution("Purchases", DataSource.ReadNetworkOnly, true, conflictCallback, completedCallback);
			_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.traceContext.Add(string.Format("< OpenWithManualConflictResolution: {0:F3}", Time.realtimeSinceStartup));
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", "Purchases", Json.Serialize(_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.traceContext));
			}
		}

		public IEnumerator SavePendingItemsToStorager()
		{
			TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
			EventHandler<PurchasesSavingEventArgs> handler = this.PurchasesSavingStarted;
			if (handler != null)
			{
				handler(this, new PurchasesSavingEventArgs(promise.Task));
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
					IEnumerable<ItemRecord> source = ItemDb.allRecords.Where(((_003CSavePendingItemsToStorager_003Ec__Iterator17E)this)._003C_003Em__3B7);
					if (_003CSavePendingItemsToStorager_003Ec__Iterator17E._003C_003Ef__am_0024cacheF == null)
					{
						_003CSavePendingItemsToStorager_003Ec__Iterator17E._003C_003Ef__am_0024cacheF = _003CSavePendingItemsToStorager_003Ec__Iterator17E._003C_003Em__3B8;
					}
					Func<ItemRecord, string> _003C_003Ef__am_0024cacheF = _003CSavePendingItemsToStorager_003Ec__Iterator17E._003C_003Ef__am_0024cacheF;
					if (_003CSavePendingItemsToStorager_003Ec__Iterator17E._003C_003Ef__am_0024cache10 == null)
					{
						_003CSavePendingItemsToStorager_003Ec__Iterator17E._003C_003Ef__am_0024cache10 = _003CSavePendingItemsToStorager_003Ec__Iterator17E._003C_003Em__3B9;
					}
					storageIdsToTagsOfItemsToBeSaved = source.ToDictionary(_003C_003Ef__am_0024cacheF, _003CSavePendingItemsToStorager_003Ec__Iterator17E._003C_003Ef__am_0024cache10);
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

		[CompilerGenerated]
		private static string _003CAllItemIds_003Em__3A9(KeyValuePair<string, string> kv)
		{
			return kv.Value;
		}

		[CompilerGenerated]
		private static string _003CAllItemIds_003Em__3AA(int i)
		{
			return "currentLevel" + i;
		}

		[CompilerGenerated]
		private static bool _003CGetPurchasesIds_003Em__3AB(string id)
		{
			return Storager.getInt(id, false) != 0;
		}

		[CompilerGenerated]
		private static string _003CSynchronizeAmazonPurchases_003Em__3AC(AGSSyncableStringElement s)
		{
			return s.GetValue();
		}

		[CompilerGenerated]
		private static bool _003CHandleReadBinaryData_003Em__3AE(string i)
		{
			return !string.IsNullOrEmpty(i);
		}
	}
}
