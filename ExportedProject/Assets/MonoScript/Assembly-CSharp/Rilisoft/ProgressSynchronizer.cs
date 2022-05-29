using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Rilisoft
{
	internal sealed class ProgressSynchronizer
	{
		public const string Filename = "Progress";

		private static ProgressSynchronizer _instance;

		public static ProgressSynchronizer Instance
		{
			get
			{
				if (ProgressSynchronizer._instance == null)
				{
					ProgressSynchronizer._instance = new ProgressSynchronizer();
				}
				return ProgressSynchronizer._instance;
			}
		}

		public ProgressSynchronizer()
		{
		}

		public void AuthenticateAndSynchronize(Action callback, bool silent)
		{
			if (!GpgFacade.Instance.IsAuthenticated())
			{
				Action<bool> action = (bool succeeded) => {
					PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32((silent ? false : !succeeded)));
					if (succeeded)
					{
						Debug.Log(string.Format("Authentication succeeded: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state));
						ProgressSynchronizer.Instance.SynchronizeIfAuthenticated(callback);
					}
					else if (!Application.isEditor)
					{
						Debug.LogWarning("Authentication failed.");
					}
				};
				GpgFacade.Instance.Authenticate(action, silent);
			}
			else
			{
				Debug.LogFormat("Already authenticated: {0}, {1}, {2}", new object[] { Social.localUser.id, Social.localUser.userName, Social.localUser.state });
				ProgressSynchronizer.Instance.SynchronizeIfAuthenticated(callback);
			}
		}

		private static void MergeUpdateLocalProgress(IDictionary<string, Dictionary<string, int>> incomingProgress)
		{
			Dictionary<string, int> strs;
			int num;
			IEnumerator<KeyValuePair<string, Dictionary<string, int>>> enumerator = incomingProgress.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, Dictionary<string, int>> current = enumerator.Current;
					if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(current.Key, out strs))
					{
						CampaignProgress.boxesLevelsAndStars.Add(current.Key, current.Value);
					}
					else
					{
						Dictionary<string, int>.Enumerator enumerator1 = current.Value.GetEnumerator();
						try
						{
							while (enumerator1.MoveNext())
							{
								KeyValuePair<string, int> keyValuePair = enumerator1.Current;
								if (!strs.TryGetValue(keyValuePair.Key, out num))
								{
									strs.Add(keyValuePair.Key, keyValuePair.Value);
								}
								else
								{
									strs[keyValuePair.Key] = Math.Max(num, keyValuePair.Value);
								}
							}
						}
						finally
						{
							((IDisposable)(object)enumerator1).Dispose();
						}
					}
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			CampaignProgress.OpenNewBoxIfPossible();
		}

		public void SynchronizeAmazonProgress()
		{
			Dictionary<string, int> strs;
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogWarning("SynchronizeAmazonProgress() is not implemented for current target.");
				return;
			}
			AGSWhispersyncClient.Synchronize();
			using (AGSGameDataMap gameData = AGSWhispersyncClient.GetGameData())
			{
				if (gameData != null)
				{
					using (AGSGameDataMap map = gameData.GetMap("progressMap"))
					{
						if (map != null)
						{
							string[] array = (
								from k in map.GetMapKeys()
								where !string.IsNullOrEmpty(k)
								select k).ToArray<string>();
							string str = string.Format("Trying to sync progress.    Local: {0}    Cloud keys: {1}", CampaignProgress.GetCampaignProgressString(), Json.Serialize(array));
							Debug.Log(str);
							string[] strArrays = array;
							for (int i = 0; i < (int)strArrays.Length; i++)
							{
								string str1 = strArrays[i];
								if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(str1, out strs))
								{
									Debug.LogWarning(string.Concat("boxesLevelsAndStars doesn't contain “", str1, "”"));
									strs = new Dictionary<string, int>();
									CampaignProgress.boxesLevelsAndStars.Add(str1, strs);
								}
								else if (strs == null)
								{
									Debug.LogWarning("localBox == null");
									strs = new Dictionary<string, int>();
									CampaignProgress.boxesLevelsAndStars[str1] = strs;
								}
								using (AGSGameDataMap aGSGameDataMap = map.GetMap(str1))
								{
									if (aGSGameDataMap != null)
									{
										string[] array1 = aGSGameDataMap.GetHighestNumberKeys().ToArray<string>();
										string str2 = string.Format("“{0}” levels: {1}", str1, Json.Serialize(array1));
										Debug.Log(str2);
										string[] strArrays1 = array1;
										for (int j = 0; j < (int)strArrays1.Length; j++)
										{
											string str3 = strArrays1[j];
											using (AGSSyncableNumber highestNumber = aGSGameDataMap.GetHighestNumber(str3))
											{
												if (highestNumber != null)
												{
													if (Debug.isDebugBuild)
													{
														Debug.Log(string.Concat("Synchronizing from cloud “", str3, "”..."));
													}
													int num = highestNumber.AsInt();
													int num1 = 0;
													if (!strs.TryGetValue(str3, out num1))
													{
														strs.Add(str3, num);
													}
													else
													{
														strs[str3] = Math.Max(num1, num);
													}
													if (Debug.isDebugBuild)
													{
														Debug.Log(string.Concat("Synchronized from cloud “", str3, "”..."));
													}
												}
												else
												{
													Debug.LogWarning("syncableCloudValue == null");
												}
											}
										}
									}
									else
									{
										Debug.LogWarning("boxMap == null");
									}
								}
							}
							CampaignProgress.OpenNewBoxIfPossible();
							CampaignProgress.ActualizeComicsViews();
							WeaponManager.ActualizeWeaponsForCampaignProgress();
							Debug.Log(string.Concat("Trying to sync progress.    Merged: ", CampaignProgress.GetCampaignProgressString()));
							foreach (KeyValuePair<string, Dictionary<string, int>> boxesLevelsAndStar in CampaignProgress.boxesLevelsAndStars)
							{
								if (Debug.isDebugBuild)
								{
									Debug.Log(string.Format("Synchronizing to cloud: “{0}”", boxesLevelsAndStar));
								}
								using (AGSGameDataMap map1 = map.GetMap(boxesLevelsAndStar.Key))
								{
									if (map1 != null)
									{
										foreach (KeyValuePair<string, int> value in boxesLevelsAndStar.Value ?? new Dictionary<string, int>())
										{
											using (AGSSyncableNumber aGSSyncableNumber = map1.GetHighestNumber(value.Key))
											{
												if (aGSSyncableNumber != null)
												{
													aGSSyncableNumber.Set(value.Value);
												}
												else
												{
													Debug.LogWarning("syncableCloudValue == null");
												}
											}
										}
									}
									else
									{
										Debug.LogWarning("boxMap == null");
									}
								}
							}
							AGSWhispersyncClient.Synchronize();
						}
						else
						{
							Debug.LogWarning("syncableProgressMap == null");
							return;
						}
					}
				}
				else
				{
					Debug.LogWarning("dataMap == null");
				}
			}
		}

		public void SynchronizeIfAuthenticated(Action callback)
		{
			if (!GpgFacade.Instance.IsAuthenticated())
			{
				return;
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			using (StopwatchLogger stopwatchLogger = new StopwatchLogger("SynchronizeIfAuthenticated(...)"))
			{
				this.SynchronizeIfAuthenticatedWithSavedGamesService(callback);
			}
		}

		private void SynchronizeIfAuthenticatedWithSavedGamesService(Action callback)
		{
			if (PlayGamesPlatform.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				return;
			}
			List<string> strs5 = new List<string>();
			Action<SavedGameRequestStatus, ISavedGameMetadata> action = (SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata) => {
				Func<KeyValuePair<string, Dictionary<string, int>>, IEnumerable<string>> keys = null;
				Func<KeyValuePair<string, Dictionary<string, int>>, IEnumerable<string>> func = null;
				Debug.LogFormat("****** Open '{0}': {1} '{2}'", new object[] { "Progress", openStatus, openMetadata.GetDescription() });
				if (openStatus != SavedGameRequestStatus.Success)
				{
					return;
				}
				Debug.LogFormat("****** Trying to read '{0}' '{1}'...", new object[] { "Progress", openMetadata.GetDescription() });
				PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(openMetadata, (SavedGameRequestStatus readStatus, byte[] data) => {
					string str = Encoding.UTF8.GetString(data ?? new byte[0]);
					Debug.Log(string.Format("****** Read '{0}': {1} '{2}'    '{3}'", new object[] { "Progress", readStatus, openMetadata.GetDescription(), str }));
					if (readStatus != SavedGameRequestStatus.Success)
					{
						return;
					}
					Dictionary<string, Dictionary<string, int>> strs = CampaignProgress.DeserializeProgress(str);
					if (strs == null)
					{
						Debug.LogWarning("serverProgress == null");
						return;
					}
					Dictionary<string, Dictionary<string, int>> strs1 = CampaignProgress.boxesLevelsAndStars;
					if (keys == null)
					{
						keys = (KeyValuePair<string, Dictionary<string, int>> kv) => kv.Value.Keys;
					}
					HashSet<string> strs2 = new HashSet<string>(strs1.SelectMany<KeyValuePair<string, Dictionary<string, int>>, string>(keys));
					HashSet<string> strs3 = strs2;
					Dictionary<string, Dictionary<string, int>> strs4 = strs;
					if (func == null)
					{
						func = (KeyValuePair<string, Dictionary<string, int>> kv) => kv.Value.Keys;
					}
					strs3.ExceptWith(strs4.SelectMany<KeyValuePair<string, Dictionary<string, int>>, string>(func));
					string str1 = Json.Serialize(strs2.ToArray<string>());
					ProgressSynchronizer.MergeUpdateLocalProgress(strs);
					CampaignProgress.ActualizeComicsViews();
					WeaponManager.ActualizeWeaponsForCampaignProgress();
					string campaignProgressString = CampaignProgress.GetCampaignProgressString();
					Debug.Log(string.Format("****** Trying to write '{0}': '{1}'...", "Progress", campaignProgressString));
					byte[] bytes = Encoding.UTF8.GetBytes(campaignProgressString);
					string str2 = string.Format("Added levels by '{0}': {1}", SystemInfo.deviceModel, str1.Substring(0, Math.Min(32, str1.Length)));
					SavedGameMetadataUpdate savedGameMetadataUpdate = (new SavedGameMetadataUpdate.Builder()).WithUpdatedDescription(str2).Build();
					PlayGamesPlatform.Instance.SavedGame.CommitUpdate(openMetadata, savedGameMetadataUpdate, bytes, (SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata) => {
						Debug.Log(string.Format("****** Written '{0}': {1} '{2}'    '{3}'", new object[] { "Progress", writeStatus, closeMetadata.GetDescription(), campaignProgressString }));
						callback();
					});
				});
			};
			ConflictCallback conflictCallback = (IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData) => {
				string str = Encoding.UTF8.GetString(originalData);
				string str1 = Encoding.UTF8.GetString(unmergedData);
				ISavedGameMetadata savedGameMetadatum = null;
				if (str.Length <= str1.Length)
				{
					savedGameMetadatum = unmerged;
					resolver.ChooseMetadata(savedGameMetadatum);
					Debug.Log(string.Format("****** Partially resolved using unmerged metadata '{0}': '{1}'", "Progress", unmerged.GetDescription()));
				}
				else
				{
					savedGameMetadatum = original;
					resolver.ChooseMetadata(savedGameMetadatum);
					Debug.Log(string.Format("****** Partially resolved using original metadata '{0}': '{1}'", "Progress", original.GetDescription()));
				}
				string str2 = DictionaryLoadedListener.MergeProgress(str, str1);
				Dictionary<string, Dictionary<string, int>> strs = CampaignProgress.DeserializeProgress(str2);
				if (strs == null)
				{
					Debug.LogWarning("mergedProgress == null");
					return;
				}
				ProgressSynchronizer.MergeUpdateLocalProgress(strs);
				CampaignProgress.ActualizeComicsViews();
				WeaponManager.ActualizeWeaponsForCampaignProgress();
				string str3 = string.Format("Merged by '{0}': '{1}' and '{2}'", SystemInfo.deviceModel, original.GetDescription(), unmerged.GetDescription());
				SavedGameMetadataUpdate savedGameMetadataUpdate = (new SavedGameMetadataUpdate.Builder()).WithUpdatedDescription(str3).Build();
				byte[] bytes = Encoding.UTF8.GetBytes(str2);
				PlayGamesPlatform.Instance.SavedGame.CommitUpdate(savedGameMetadatum, savedGameMetadataUpdate, bytes, (SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata) => {
					Debug.LogFormat("****** Written '{0}': {1} '{2}'    '{3}'", new object[] { "Progress", writeStatus, closeMetadata.GetDescription(), str2 });
					callback();
				});
			};
			Debug.LogFormat("****** Trying to open '{0}'...", new object[] { "Progress" });
			PlayGamesPlatform.Instance.SavedGame.OpenWithManualConflictResolution("Progress", DataSource.ReadNetworkOnly, true, conflictCallback, action);
		}

		public void SynchronizeIosProgress()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				!Storager.ICloudAvailable;
			}
		}
	}
}