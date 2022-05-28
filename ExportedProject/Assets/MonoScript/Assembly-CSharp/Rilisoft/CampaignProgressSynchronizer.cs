using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class CampaignProgressSynchronizer
	{
		private const int AttemptCountMax = 3;

		private static readonly WaitForSeconds s_delay = new WaitForSeconds(30f);

		private static readonly CampaignProgressSynchronizer s_instance = new CampaignProgressSynchronizer();

		[CompilerGenerated]
		private static Func<LevelProgressMemento, bool> _003C_003Ef__am_0024cache2;

		[CompilerGenerated]
		private static Func<LevelProgressMemento, string> _003C_003Ef__am_0024cache3;

		[CompilerGenerated]
		private static Func<LevelProgressMemento, bool> _003C_003Ef__am_0024cache4;

		[CompilerGenerated]
		private static Func<LevelProgressMemento, string> _003C_003Ef__am_0024cache5;

		public static CampaignProgressSynchronizer Instance
		{
			get
			{
				return s_instance;
			}
		}

		private CampaignProgressSynchronizer()
		{
		}

		public Coroutine Pull()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return CoroutineRunner.Instance.StartCoroutine(SyncGoogleCoroutine(true));
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					SyncAmazon();
				}
			}
			return null;
		}

		public Coroutine Push()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return CoroutineRunner.Instance.StartCoroutine(PushGoogleCoroutine());
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					SyncAmazon();
				}
			}
			return null;
		}

		public Coroutine Sync()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return CoroutineRunner.Instance.StartCoroutine(SyncGoogleCoroutine(false));
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					SyncAmazon();
				}
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				SyncCampaignBonusesIos();
				return null;
			}
			return null;
		}

		private void SyncAmazon()
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.SyncAmazon()", GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
			try
			{
				AGSWhispersyncClient.Synchronize();
				using (AGSGameDataMap aGSGameDataMap = AGSWhispersyncClient.GetGameData())
				{
					EnsureNotNull(aGSGameDataMap, "dataMap");
					using (AGSGameDataMap aGSGameDataMap2 = aGSGameDataMap.GetMap("campaignProgressMap"))
					{
						EnsureNotNull(aGSGameDataMap2, "campaignProgressMap");
						CampaignProgressMemento campaignProgressMemento = LoadMemento();
						Dictionary<string, LevelProgressMemento> levelsAsDictionary = campaignProgressMemento.GetLevelsAsDictionary();
						CampaignProgressMemento campaignProgressMemento2 = default(CampaignProgressMemento);
						using (AGSGameDataMap aGSGameDataMap3 = aGSGameDataMap2.GetMap("levels"))
						{
							EnsureNotNull(aGSGameDataMap3, "levelsMap");
							HashSet<string> mapKeys = aGSGameDataMap3.GetMapKeys();
							mapKeys.UnionWith(levelsAsDictionary.Keys);
							foreach (string item in mapKeys)
							{
								AGSGameDataMap map = aGSGameDataMap3.GetMap(item);
								if (map == null)
								{
									continue;
								}
								LevelProgressMemento levelProgressMemento = new LevelProgressMemento(item);
								AGSSyncableNumber highestNumber = map.GetHighestNumber("coinCount");
								levelProgressMemento.CoinCount = ((highestNumber != null) ? highestNumber.AsInt() : 0);
								AGSSyncableNumber highestNumber2 = map.GetHighestNumber("gemCount");
								levelProgressMemento.GemCount = ((highestNumber2 != null) ? highestNumber2.AsInt() : 0);
								campaignProgressMemento2.Levels.Add(levelProgressMemento);
								LevelProgressMemento value;
								if (levelsAsDictionary.TryGetValue(item, out value))
								{
									if (highestNumber != null && levelProgressMemento.CoinCount < value.CoinCount)
									{
										highestNumber.Set(value.CoinCount);
										highestNumber.Dispose();
									}
									if (highestNumber2 != null && levelProgressMemento.GemCount < value.GemCount)
									{
										highestNumber2.Set(value.GemCount);
										highestNumber2.Dispose();
									}
								}
							}
						}
						CampaignProgressMemento campaignProgressMemento3 = CampaignProgressMemento.Merge(campaignProgressMemento, campaignProgressMemento2);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("Local campaign progress: {0}", campaignProgressMemento);
							Debug.LogFormat("Cloud campaign progress: {0}", campaignProgressMemento2);
							Debug.LogFormat("Merged campaign progress: {0}", campaignProgressMemento3);
						}
						OverwriteMemento(campaignProgressMemento3);
					}
					AGSWhispersyncClient.Synchronize();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void EnsureNotNull(object value, string name)
		{
			if (value == null)
			{
				throw new InvalidOperationException(name ?? string.Empty);
			}
		}

		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.SyncGoogleCoroutine('{1}')", GetType().Name, (!pullOnly) ? "sync" : "pull");
			ScopeLogger scopeLogger = new ScopeLogger(thisMethod, Defs.IsDeveloperBuild && !Application.isEditor);
			try
			{
				CampaignProgressSynchronizerGpgFacade googleSavedGamesFacade = default(CampaignProgressSynchronizerGpgFacade);
				for (int i = 0; i != 3; i++)
				{
					Task<GoogleSavedGameRequestResult<CampaignProgressMemento>> future = googleSavedGamesFacade.Pull();
					while (!future.IsCompleted)
					{
						yield return null;
					}
					if (future.IsFaulted)
					{
						Exception ex = future.Exception.InnerExceptions.FirstOrDefault() ?? future.Exception;
						Debug.LogWarning("Failed to pull campaign progress with exception: " + ex.Message);
						yield return s_delay;
						continue;
					}
					SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
					if (requestStatus != SavedGameRequestStatus.Success)
					{
						Debug.LogWarning("Failed to pull campaign progress with status: " + requestStatus);
						yield return s_delay;
						continue;
					}
					CampaignProgressMemento localCampaignProgress = LoadMemento();
					CampaignProgressMemento cloudCampaignProgress = future.Result.Value;
					CampaignProgressMemento mergedCampaignProgress = CampaignProgressMemento.Merge(localCampaignProgress, cloudCampaignProgress);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("Local campaign progress: {0}", JsonUtility.ToJson(localCampaignProgress));
						Debug.LogFormat("Cloud campaign progress: {0}", JsonUtility.ToJson(cloudCampaignProgress));
						Debug.LogFormat("Merged campaign progress: {0}", JsonUtility.ToJson(mergedCampaignProgress));
					}
					Dictionary<string, LevelProgressMemento> mergedLevels = mergedCampaignProgress.GetLevelsAsDictionary();
					Func<LevelProgressMemento, bool> dirtyCondition = ((_003CSyncGoogleCoroutine_003Ec__Iterator1CE)this)._003C_003Em__579;
					bool localDirty = localCampaignProgress.Levels.Count < mergedCampaignProgress.Levels.Count || localCampaignProgress.Levels.Any(dirtyCondition);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("Local progress is dirty: {0}", localDirty);
					}
					if (localDirty)
					{
						OverwriteMemento(mergedCampaignProgress);
					}
					if (pullOnly)
					{
						break;
					}
					bool cloudDirty = cloudCampaignProgress.Levels.Count < mergedCampaignProgress.Levels.Count || cloudCampaignProgress.Levels.Any(dirtyCondition);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("Cloud progress is dirty: {0}, conflicted: {1}", cloudDirty, cloudCampaignProgress.Conflicted);
					}
					if (cloudDirty || cloudCampaignProgress.Conflicted)
					{
						IEnumerator enumerator = PushGoogleCoroutine();
						while (enumerator.MoveNext())
						{
							yield return null;
						}
					}
					break;
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private IEnumerator PushGoogleCoroutine()
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.PushGoogleCoroutine()", GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(thisMethod, Defs.IsDeveloperBuild);
			try
			{
				CampaignProgressSynchronizerGpgFacade googleSavedGamesFacade = default(CampaignProgressSynchronizerGpgFacade);
				for (int i = 0; i != 3; i++)
				{
					CampaignProgressMemento localCampaignProgress = LoadMemento();
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("Local progress: {0}", JsonUtility.ToJson(localCampaignProgress));
					}
					Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> future = googleSavedGamesFacade.Push(localCampaignProgress);
					while (!future.IsCompleted)
					{
						yield return null;
					}
					if (future.IsFaulted)
					{
						Exception ex = future.Exception.InnerExceptions.FirstOrDefault() ?? future.Exception;
						Debug.LogWarning("Failed to push campaign progress with exception: " + ex.Message);
						yield return s_delay;
						continue;
					}
					SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
					if (requestStatus != SavedGameRequestStatus.Success)
					{
						Debug.LogWarning("Failed to push campaign progress with status: " + requestStatus);
						yield return s_delay;
						continue;
					}
					if (Defs.IsDeveloperBuild)
					{
						ISavedGameMetadata metadata = future.Result.Value;
						string description = ((metadata == null) ? string.Empty : metadata.Description);
						Debug.LogFormat("[CampaignProgress] Succeeded to push campaign progress {0}: '{1}'", localCampaignProgress, description);
					}
					break;
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		internal CampaignProgressMemento LoadMemento()
		{
			//Discarded unreachable code: IL_0183
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.LoadMemento()", GetType().Name);
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
			try
			{
				Dictionary<string, LevelProgressMemento> dictionary = new Dictionary<string, LevelProgressMemento>();
				string @string = Storager.getString(Defs.LevelsWhereGetCoinS, false);
				string[] array = @string.Split(new char[1] { '#' }, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				foreach (string text in array2)
				{
					LevelProgressMemento value;
					if (dictionary.TryGetValue(text, out value))
					{
						value.CoinCount = 1;
						continue;
					}
					LevelProgressMemento levelProgressMemento = new LevelProgressMemento(text);
					levelProgressMemento.CoinCount = 1;
					value = levelProgressMemento;
					dictionary.Add(text, value);
				}
				string string2 = Storager.getString(Defs.LevelsWhereGotGems, false);
				List<object> list = Json.Deserialize(string2) as List<object>;
				List<string> list2 = ((list == null) ? new List<string>() : list.OfType<string>().ToList());
				foreach (string item in list2)
				{
					LevelProgressMemento value2;
					if (dictionary.TryGetValue(item, out value2))
					{
						value2.GemCount = 1;
						continue;
					}
					LevelProgressMemento levelProgressMemento = new LevelProgressMemento(item);
					levelProgressMemento.GemCount = 1;
					value2 = levelProgressMemento;
					dictionary.Add(item, value2);
				}
				CampaignProgressMemento result = default(CampaignProgressMemento);
				result.Levels.AddRange(dictionary.Values);
				return result;
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		internal void OverwriteMemento(CampaignProgressMemento campaignProgressMemento)
		{
			List<LevelProgressMemento> levels = campaignProgressMemento.Levels;
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003COverwriteMemento_003Em__575;
			}
			IEnumerable<LevelProgressMemento> source = levels.Where(_003C_003Ef__am_0024cache2);
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003COverwriteMemento_003Em__576;
			}
			string[] value = source.Select(_003C_003Ef__am_0024cache3).ToArray();
			string val = string.Join("#", value);
			Storager.setString(Defs.LevelsWhereGetCoinS, val, false);
			List<LevelProgressMemento> levels2 = campaignProgressMemento.Levels;
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003COverwriteMemento_003Em__577;
			}
			IEnumerable<LevelProgressMemento> source2 = levels2.Where(_003C_003Ef__am_0024cache4);
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003COverwriteMemento_003Em__578;
			}
			List<string> obj = source2.Select(_003C_003Ef__am_0024cache5).ToList();
			string val2 = Json.Serialize(obj);
			Storager.setString(Defs.LevelsWhereGotGems, val2, false);
		}

		private void SyncCampaignBonusesIos()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer && !Storager.ICloudAvailable)
			{
			}
		}

		[CompilerGenerated]
		private static bool _003COverwriteMemento_003Em__575(LevelProgressMemento l)
		{
			return l.CoinCount > 0;
		}

		[CompilerGenerated]
		private static string _003COverwriteMemento_003Em__576(LevelProgressMemento l)
		{
			return l.LevelId;
		}

		[CompilerGenerated]
		private static bool _003COverwriteMemento_003Em__577(LevelProgressMemento l)
		{
			return l.GemCount > 0;
		}

		[CompilerGenerated]
		private static string _003COverwriteMemento_003Em__578(LevelProgressMemento l)
		{
			return l.LevelId;
		}
	}
}
