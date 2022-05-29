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
	internal sealed class CampaignProgressSynchronizer
	{
		private const int AttemptCountMax = 3;

		private readonly static WaitForSeconds s_delay;

		private readonly static CampaignProgressSynchronizer s_instance;

		public static CampaignProgressSynchronizer Instance
		{
			get
			{
				return CampaignProgressSynchronizer.s_instance;
			}
		}

		static CampaignProgressSynchronizer()
		{
			CampaignProgressSynchronizer.s_delay = new WaitForSeconds(30f);
			CampaignProgressSynchronizer.s_instance = new CampaignProgressSynchronizer();
		}

		private CampaignProgressSynchronizer()
		{
		}

		private void EnsureNotNull(object value, string name)
		{
			if (value == null)
			{
				throw new InvalidOperationException(name ?? string.Empty);
			}
		}

		internal CampaignProgressMemento LoadMemento()
		{
			LevelProgressMemento levelProgressMemento;
			LevelProgressMemento levelProgressMemento1;
			LevelProgressMemento levelProgressMemento2;
			CampaignProgressMemento campaignProgressMemento;
			string str = string.Format(CultureInfo.InvariantCulture, "{0}.LoadMemento()", new object[] { this.GetType().Name });
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				Dictionary<string, LevelProgressMemento> strs = new Dictionary<string, LevelProgressMemento>();
				string str1 = Storager.getString(Defs.LevelsWhereGetCoinS, false);
				string[] strArrays = str1.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string str2 = strArrays[i];
					if (!strs.TryGetValue(str2, out levelProgressMemento))
					{
						levelProgressMemento2 = new LevelProgressMemento(str2)
						{
							CoinCount = 1
						};
						levelProgressMemento = levelProgressMemento2;
						strs.Add(str2, levelProgressMemento);
					}
					else
					{
						levelProgressMemento.CoinCount = 1;
					}
				}
				string str3 = Storager.getString(Defs.LevelsWhereGotGems, false);
				List<object> objs = Json.Deserialize(str3) as List<object>;
				foreach (string str4 in (objs == null ? new List<string>() : objs.OfType<string>().ToList<string>()))
				{
					if (!strs.TryGetValue(str4, out levelProgressMemento1))
					{
						levelProgressMemento2 = new LevelProgressMemento(str4)
						{
							GemCount = 1
						};
						levelProgressMemento1 = levelProgressMemento2;
						strs.Add(str4, levelProgressMemento1);
					}
					else
					{
						levelProgressMemento1.GemCount = 1;
					}
				}
				CampaignProgressMemento campaignProgressMemento1 = new CampaignProgressMemento();
				campaignProgressMemento1.Levels.AddRange(strs.Values);
				campaignProgressMemento = campaignProgressMemento1;
			}
			finally
			{
				scopeLogger.Dispose();
			}
			return campaignProgressMemento;
		}

		internal void OverwriteMemento(CampaignProgressMemento campaignProgressMemento)
		{
			string[] array = (
				from l in campaignProgressMemento.Levels
				where l.CoinCount > 0
				select l.LevelId).ToArray<string>();
			string str = string.Join("#", array);
			Storager.setString(Defs.LevelsWhereGetCoinS, str, false);
			List<string> list = (
				from l in campaignProgressMemento.Levels
				where l.GemCount > 0
				select l.LevelId).ToList<string>();
			string str1 = Json.Serialize(list);
			Storager.setString(Defs.LevelsWhereGotGems, str1, false);
		}

		public Coroutine Pull()
		{
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
		private IEnumerator PushGoogleCoroutine()
		{
			CampaignProgressSynchronizer.u003cPushGoogleCoroutineu003ec__Iterator1CF variable = null;
			return variable;
		}

		public Coroutine Sync()
		{
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
			this.SyncCampaignBonusesIos();
			return null;
		}

		private void SyncAmazon()
		{
			LevelProgressMemento levelProgressMemento;
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
						using (AGSGameDataMap map = gameData.GetMap("campaignProgressMap"))
						{
							this.EnsureNotNull(map, "campaignProgressMap");
							CampaignProgressMemento campaignProgressMemento = this.LoadMemento();
							Dictionary<string, LevelProgressMemento> levelsAsDictionary = campaignProgressMemento.GetLevelsAsDictionary();
							CampaignProgressMemento campaignProgressMemento1 = new CampaignProgressMemento();
							using (AGSGameDataMap aGSGameDataMap = map.GetMap("levels"))
							{
								this.EnsureNotNull(aGSGameDataMap, "levelsMap");
								HashSet<string> mapKeys = aGSGameDataMap.GetMapKeys();
								mapKeys.UnionWith(levelsAsDictionary.Keys);
								foreach (string mapKey in mapKeys)
								{
									AGSGameDataMap map1 = aGSGameDataMap.GetMap(mapKey);
									if (map1 != null)
									{
										LevelProgressMemento levelProgressMemento1 = new LevelProgressMemento(mapKey);
										AGSSyncableNumber highestNumber = map1.GetHighestNumber("coinCount");
										levelProgressMemento1.CoinCount = (highestNumber == null ? 0 : highestNumber.AsInt());
										AGSSyncableNumber aGSSyncableNumber = map1.GetHighestNumber("gemCount");
										levelProgressMemento1.GemCount = (aGSSyncableNumber == null ? 0 : aGSSyncableNumber.AsInt());
										campaignProgressMemento1.Levels.Add(levelProgressMemento1);
										if (!levelsAsDictionary.TryGetValue(mapKey, out levelProgressMemento))
										{
											continue;
										}
										if (highestNumber != null && levelProgressMemento1.CoinCount < levelProgressMemento.CoinCount)
										{
											highestNumber.Set(levelProgressMemento.CoinCount);
											highestNumber.Dispose();
										}
										if (aGSSyncableNumber == null || levelProgressMemento1.GemCount >= levelProgressMemento.GemCount)
										{
											continue;
										}
										aGSSyncableNumber.Set(levelProgressMemento.GemCount);
										aGSSyncableNumber.Dispose();
									}
								}
							}
							CampaignProgressMemento campaignProgressMemento2 = CampaignProgressMemento.Merge(campaignProgressMemento, campaignProgressMemento1);
							if (Defs.IsDeveloperBuild)
							{
								UnityEngine.Debug.LogFormat("Local campaign progress: {0}", new object[] { campaignProgressMemento });
								UnityEngine.Debug.LogFormat("Cloud campaign progress: {0}", new object[] { campaignProgressMemento1 });
								UnityEngine.Debug.LogFormat("Merged campaign progress: {0}", new object[] { campaignProgressMemento2 });
							}
							this.OverwriteMemento(campaignProgressMemento2);
						}
						AGSWhispersyncClient.Synchronize();
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

		private void SyncCampaignBonusesIos()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				!Storager.ICloudAvailable;
			}
		}

		[DebuggerHidden]
		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			CampaignProgressSynchronizer.u003cSyncGoogleCoroutineu003ec__Iterator1CE variable = null;
			return variable;
		}
	}
}