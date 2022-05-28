using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class ProgressSynchronizer
	{
		[CompilerGenerated]
		private sealed class _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D0
		{
			internal bool silent;

			internal Action callback;

			internal void _003C_003Em__39D(bool succeeded)
			{
				bool value = !silent && !succeeded;
				PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(value));
				if (succeeded)
				{
					string message = string.Format("Authentication succeeded: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
					Debug.Log(message);
					Instance.SynchronizeIfAuthenticated(callback);
				}
				else if (!Application.isEditor)
				{
					Debug.LogWarning("Authentication failed.");
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D3
		{
			private sealed class _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D1
			{
				private sealed class _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D2
				{
					internal string outgoingProgressString;

					internal _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D3 _003C_003Ef__ref_0024723;

					internal _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D1 _003C_003Ef__ref_0024721;

					internal void _003C_003Em__3A4(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
					{
						Debug.Log(string.Format("****** Written '{0}': {1} '{2}'    '{3}'", "Progress", writeStatus, closeMetadata.GetDescription(), outgoingProgressString));
						_003C_003Ef__ref_0024723.callback();
					}
				}

				internal ISavedGameMetadata openMetadata;

				internal _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D3 _003C_003Ef__ref_0024723;

				private static Func<KeyValuePair<string, Dictionary<string, int>>, IEnumerable<string>> _003C_003Ef__am_0024cache2;

				private static Func<KeyValuePair<string, Dictionary<string, int>>, IEnumerable<string>> _003C_003Ef__am_0024cache3;

				internal void _003C_003Em__3A0(SavedGameRequestStatus readStatus, byte[] data)
				{
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D2 _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D = new _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D2();
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Ef__ref_0024723 = _003C_003Ef__ref_0024723;
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Ef__ref_0024721 = this;
					string @string = Encoding.UTF8.GetString(data ?? new byte[0]);
					Debug.Log(string.Format("****** Read '{0}': {1} '{2}'    '{3}'", "Progress", readStatus, openMetadata.GetDescription(), @string));
					if (readStatus != SavedGameRequestStatus.Success)
					{
						return;
					}
					Dictionary<string, Dictionary<string, int>> dictionary = CampaignProgress.DeserializeProgress(@string);
					if (dictionary == null)
					{
						Debug.LogWarning("serverProgress == null");
						return;
					}
					Dictionary<string, Dictionary<string, int>> boxesLevelsAndStars = CampaignProgress.boxesLevelsAndStars;
					if (_003C_003Ef__am_0024cache2 == null)
					{
						_003C_003Ef__am_0024cache2 = _003C_003Em__3A2;
					}
					HashSet<string> hashSet = new HashSet<string>(boxesLevelsAndStars.SelectMany(_003C_003Ef__am_0024cache2));
					if (_003C_003Ef__am_0024cache3 == null)
					{
						_003C_003Ef__am_0024cache3 = _003C_003Em__3A3;
					}
					hashSet.ExceptWith(dictionary.SelectMany(_003C_003Ef__am_0024cache3));
					string text = Json.Serialize(hashSet.ToArray());
					MergeUpdateLocalProgress(dictionary);
					CampaignProgress.ActualizeComicsViews();
					WeaponManager.ActualizeWeaponsForCampaignProgress();
					_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.outgoingProgressString = CampaignProgress.GetCampaignProgressString();
					Debug.Log(string.Format("****** Trying to write '{0}': '{1}'...", "Progress", _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.outgoingProgressString));
					byte[] bytes = Encoding.UTF8.GetBytes(_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.outgoingProgressString);
					string description = string.Format("Added levels by '{0}': {1}", SystemInfo.deviceModel, text.Substring(0, Math.Min(32, text.Length)));
					SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
					PlayGamesPlatform.Instance.SavedGame.CommitUpdate(openMetadata, updateForMetadata, bytes, _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Em__3A4);
				}

				private static IEnumerable<string> _003C_003Em__3A2(KeyValuePair<string, Dictionary<string, int>> kv)
				{
					return kv.Value.Keys;
				}

				private static IEnumerable<string> _003C_003Em__3A3(KeyValuePair<string, Dictionary<string, int>> kv)
				{
					return kv.Value.Keys;
				}
			}

			private sealed class _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D4
			{
				internal string mergedString;

				internal _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D3 _003C_003Ef__ref_0024723;

				internal void _003C_003Em__3A1(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
				{
					Debug.LogFormat("****** Written '{0}': {1} '{2}'    '{3}'", "Progress", writeStatus, closeMetadata.GetDescription(), mergedString);
					_003C_003Ef__ref_0024723.callback();
				}
			}

			internal Action callback;

			internal void _003C_003Em__39E(SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata)
			{
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D1 _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D = new _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D1();
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Ef__ref_0024723 = this;
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.openMetadata = openMetadata;
				Debug.LogFormat("****** Open '{0}': {1} '{2}'", "Progress", openStatus, _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.openMetadata.GetDescription());
				if (openStatus == SavedGameRequestStatus.Success)
				{
					Debug.LogFormat("****** Trying to read '{0}' '{1}'...", "Progress", _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.openMetadata.GetDescription());
					PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.openMetadata, _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Em__3A0);
				}
			}

			internal void _003C_003Em__39F(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D4 _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D = new _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D4();
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Ef__ref_0024723 = this;
				string @string = Encoding.UTF8.GetString(originalData);
				string string2 = Encoding.UTF8.GetString(unmergedData);
				ISavedGameMetadata savedGameMetadata = null;
				if (@string.Length > string2.Length)
				{
					savedGameMetadata = original;
					resolver.ChooseMetadata(savedGameMetadata);
					Debug.Log(string.Format("****** Partially resolved using original metadata '{0}': '{1}'", "Progress", original.GetDescription()));
				}
				else
				{
					savedGameMetadata = unmerged;
					resolver.ChooseMetadata(savedGameMetadata);
					Debug.Log(string.Format("****** Partially resolved using unmerged metadata '{0}': '{1}'", "Progress", unmerged.GetDescription()));
				}
				_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.mergedString = DictionaryLoadedListener.MergeProgress(@string, string2);
				Dictionary<string, Dictionary<string, int>> dictionary = CampaignProgress.DeserializeProgress(_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.mergedString);
				if (dictionary == null)
				{
					Debug.LogWarning("mergedProgress == null");
					return;
				}
				MergeUpdateLocalProgress(dictionary);
				CampaignProgress.ActualizeComicsViews();
				WeaponManager.ActualizeWeaponsForCampaignProgress();
				string description = string.Format("Merged by '{0}': '{1}' and '{2}'", SystemInfo.deviceModel, original.GetDescription(), unmerged.GetDescription());
				SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
				byte[] bytes = Encoding.UTF8.GetBytes(_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.mergedString);
				PlayGamesPlatform.Instance.SavedGame.CommitUpdate(savedGameMetadata, updateForMetadata, bytes, _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Em__3A1);
			}
		}

		public const string Filename = "Progress";

		private static ProgressSynchronizer _instance;

		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__am_0024cache1;

		public static ProgressSynchronizer Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ProgressSynchronizer();
				}
				return _instance;
			}
		}

		public void SynchronizeIosProgress()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer && !Storager.ICloudAvailable)
			{
			}
		}

		public void SynchronizeAmazonProgress()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogWarning("SynchronizeAmazonProgress() is not implemented for current target.");
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
				using (AGSGameDataMap aGSGameDataMap2 = aGSGameDataMap.GetMap("progressMap"))
				{
					if (aGSGameDataMap2 == null)
					{
						Debug.LogWarning("syncableProgressMap == null");
						return;
					}
					HashSet<string> mapKeys = aGSGameDataMap2.GetMapKeys();
					if (_003C_003Ef__am_0024cache1 == null)
					{
						_003C_003Ef__am_0024cache1 = _003CSynchronizeAmazonProgress_003Em__39C;
					}
					string[] array = mapKeys.Where(_003C_003Ef__am_0024cache1).ToArray();
					string message = string.Format("Trying to sync progress.    Local: {0}    Cloud keys: {1}", CampaignProgress.GetCampaignProgressString(), Json.Serialize(array));
					Debug.Log(message);
					string[] array2 = array;
					foreach (string text in array2)
					{
						Dictionary<string, int> value;
						if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(text, out value))
						{
							Debug.LogWarning("boxesLevelsAndStars doesn't contain “" + text + "”");
							value = new Dictionary<string, int>();
							CampaignProgress.boxesLevelsAndStars.Add(text, value);
						}
						else if (value == null)
						{
							Debug.LogWarning("localBox == null");
							value = new Dictionary<string, int>();
							CampaignProgress.boxesLevelsAndStars[text] = value;
						}
						using (AGSGameDataMap aGSGameDataMap3 = aGSGameDataMap2.GetMap(text))
						{
							if (aGSGameDataMap3 == null)
							{
								Debug.LogWarning("boxMap == null");
								continue;
							}
							string[] array3 = aGSGameDataMap3.GetHighestNumberKeys().ToArray();
							string message2 = string.Format("“{0}” levels: {1}", text, Json.Serialize(array3));
							Debug.Log(message2);
							string[] array4 = array3;
							foreach (string text2 in array4)
							{
								using (AGSSyncableNumber aGSSyncableNumber = aGSGameDataMap3.GetHighestNumber(text2))
								{
									if (aGSSyncableNumber == null)
									{
										Debug.LogWarning("syncableCloudValue == null");
										continue;
									}
									if (Debug.isDebugBuild)
									{
										Debug.Log("Synchronizing from cloud “" + text2 + "”...");
									}
									int num = aGSSyncableNumber.AsInt();
									int value2 = 0;
									if (value.TryGetValue(text2, out value2))
									{
										value[text2] = Math.Max(value2, num);
									}
									else
									{
										value.Add(text2, num);
									}
									if (Debug.isDebugBuild)
									{
										Debug.Log("Synchronized from cloud “" + text2 + "”...");
									}
								}
							}
						}
					}
					CampaignProgress.OpenNewBoxIfPossible();
					CampaignProgress.ActualizeComicsViews();
					WeaponManager.ActualizeWeaponsForCampaignProgress();
					Debug.Log("Trying to sync progress.    Merged: " + CampaignProgress.GetCampaignProgressString());
					foreach (KeyValuePair<string, Dictionary<string, int>> boxesLevelsAndStar in CampaignProgress.boxesLevelsAndStars)
					{
						if (Debug.isDebugBuild)
						{
							string message3 = string.Format("Synchronizing to cloud: “{0}”", boxesLevelsAndStar);
							Debug.Log(message3);
						}
						using (AGSGameDataMap aGSGameDataMap4 = aGSGameDataMap2.GetMap(boxesLevelsAndStar.Key))
						{
							if (aGSGameDataMap4 == null)
							{
								Debug.LogWarning("boxMap == null");
								continue;
							}
							Dictionary<string, int> dictionary = boxesLevelsAndStar.Value ?? new Dictionary<string, int>();
							foreach (KeyValuePair<string, int> item in dictionary)
							{
								using (AGSSyncableNumber aGSSyncableNumber2 = aGSGameDataMap4.GetHighestNumber(item.Key))
								{
									if (aGSSyncableNumber2 == null)
									{
										Debug.LogWarning("syncableCloudValue == null");
									}
									else
									{
										aGSSyncableNumber2.Set(item.Value);
									}
								}
							}
						}
					}
					AGSWhispersyncClient.Synchronize();
				}
			}
		}

		public void AuthenticateAndSynchronize(Action callback, bool silent)
		{
			_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D0 _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D = new _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D0();
			_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D.silent = silent;
			_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D.callback = callback;
			if (GpgFacade.Instance.IsAuthenticated())
			{
				Debug.LogFormat("Already authenticated: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
				Instance.SynchronizeIfAuthenticated(_003CAuthenticateAndSynchronize_003Ec__AnonStorey2D.callback);
			}
			else
			{
				Action<bool> callback2 = _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D._003C_003Em__39D;
				GpgFacade.Instance.Authenticate(callback2, _003CAuthenticateAndSynchronize_003Ec__AnonStorey2D.silent);
			}
		}

		private void SynchronizeIfAuthenticatedWithSavedGamesService(Action callback)
		{
			_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D3 _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D = new _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D3();
			_003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D.callback = callback;
			if (PlayGamesPlatform.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				return;
			}
			List<string> list = new List<string>();
			Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback = _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Em__39E;
			ConflictCallback conflictCallback = _003CSynchronizeIfAuthenticatedWithSavedGamesService_003Ec__AnonStorey2D._003C_003Em__39F;
			Debug.LogFormat("****** Trying to open '{0}'...", "Progress");
			PlayGamesPlatform.Instance.SavedGame.OpenWithManualConflictResolution("Progress", DataSource.ReadNetworkOnly, true, conflictCallback, completedCallback);
		}

		private static void MergeUpdateLocalProgress(IDictionary<string, Dictionary<string, int>> incomingProgress)
		{
			foreach (KeyValuePair<string, Dictionary<string, int>> item in incomingProgress)
			{
				Dictionary<string, int> value;
				if (CampaignProgress.boxesLevelsAndStars.TryGetValue(item.Key, out value))
				{
					foreach (KeyValuePair<string, int> item2 in item.Value)
					{
						int value2;
						if (value.TryGetValue(item2.Key, out value2))
						{
							value[item2.Key] = Math.Max(value2, item2.Value);
						}
						else
						{
							value.Add(item2.Key, item2.Value);
						}
					}
				}
				else
				{
					CampaignProgress.boxesLevelsAndStars.Add(item.Key, item.Value);
				}
			}
			CampaignProgress.OpenNewBoxIfPossible();
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
			using (new StopwatchLogger("SynchronizeIfAuthenticated(...)"))
			{
				SynchronizeIfAuthenticatedWithSavedGamesService(callback);
			}
		}

		[CompilerGenerated]
		private static bool _003CSynchronizeAmazonProgress_003Em__39C(string k)
		{
			return !string.IsNullOrEmpty(k);
		}
	}
}
