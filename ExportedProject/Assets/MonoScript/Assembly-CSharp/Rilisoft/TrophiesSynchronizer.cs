using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class TrophiesSynchronizer
	{
		private const string TrophiesNegativeKey = "RatingNegative";

		private const string TrophiesPositiveKey = "RatingPositive";

		private static readonly TrophiesSynchronizer _instance = new TrophiesSynchronizer();

		public static TrophiesSynchronizer Instance
		{
			get
			{
				return _instance;
			}
		}

		private bool Ready
		{
			get
			{
				return true;
			}
		}

		public event EventHandler Updated;

		private TrophiesSynchronizer()
		{
		}

		public Coroutine Pull()
		{
			if (!Ready)
			{
				return null;
			}
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
			if (!Ready)
			{
				return null;
			}
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
			if (!Ready)
			{
				return null;
			}
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
					if (aGSGameDataMap == null)
					{
						Debug.LogWarning("dataMap == null");
						return;
					}
					using (AGSGameDataMap aGSGameDataMap2 = aGSGameDataMap.GetMap("trophiesMap"))
					{
						if (aGSGameDataMap2 == null)
						{
							Debug.LogWarning("trophiesMap == null");
							return;
						}
						AGSSyncableNumber highestNumber = aGSGameDataMap2.GetHighestNumber("trophiesNegative");
						AGSSyncableNumber highestNumber2 = aGSGameDataMap2.GetHighestNumber("trophiesPositive");
						int num = ((highestNumber != null) ? highestNumber.AsInt() : 0);
						int num2 = ((highestNumber2 != null) ? highestNumber2.AsInt() : 0);
						TrophiesMemento trophiesMemento = new TrophiesMemento(num, num2);
						int @int = Storager.getInt("RatingNegative", false);
						int int2 = Storager.getInt("RatingPositive", false);
						TrophiesMemento trophiesMemento2 = new TrophiesMemento(@int, int2);
						TrophiesMemento trophiesMemento3 = TrophiesMemento.Merge(trophiesMemento2, trophiesMemento);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("Local trophies progress: {0}", JsonUtility.ToJson(trophiesMemento2));
							Debug.LogFormat("Cloud trophies progress: {0}", JsonUtility.ToJson(trophiesMemento));
							Debug.LogFormat("Merged trophies progress: {0}", JsonUtility.ToJson(trophiesMemento3));
						}
						if (@int < trophiesMemento3.TrophiesNegative || int2 < trophiesMemento3.TrophiesPositive)
						{
							Storager.setInt("RatingNegative", trophiesMemento3.TrophiesNegative, false);
							Storager.setInt("RatingPositive", trophiesMemento3.TrophiesPositive, false);
							EventHandler updated = this.Updated;
							if (updated != null)
							{
								updated(this, EventArgs.Empty);
							}
						}
						if (num < trophiesMemento3.TrophiesNegative || num2 < trophiesMemento3.TrophiesPositive)
						{
							highestNumber.Set(trophiesMemento3.TrophiesNegative);
							highestNumber2.Set(trophiesMemento3.TrophiesPositive);
						}
					}
					AGSWhispersyncClient.Synchronize();
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			if (!Ready)
			{
				yield break;
			}
			string thisName = string.Format(CultureInfo.InvariantCulture, "TrophiesSynchronizer.SyncGoogleCoroutine('{0}')", (!pullOnly) ? "sync" : "pull");
			ScopeLogger scopeLogger = new ScopeLogger(thisName, Defs.IsDeveloperBuild && !Application.isEditor);
			try
			{
				TrophiesSynchronizerGoogleSavedGameFacade googleSavedGamesFacade = default(TrophiesSynchronizerGoogleSavedGameFacade);
				WaitForSeconds delay = new WaitForSeconds(30f);
				int i = 0;
				while (true)
				{
					string callee = string.Format(CultureInfo.InvariantCulture, "Pull and wait ({0})", i);
					using (ScopeLogger logger = new ScopeLogger(thisName, callee, Defs.IsDeveloperBuild && !Application.isEditor))
					{
						Task<GoogleSavedGameRequestResult<TrophiesMemento>> future = googleSavedGamesFacade.Pull();
						while (!future.IsCompleted)
						{
							yield return null;
						}
						logger.Dispose();
						if (future.IsFaulted)
						{
							Exception ex = future.Exception.InnerExceptions.FirstOrDefault() ?? future.Exception;
							Debug.LogWarning("Failed to pull trophies with exception: " + ex.Message);
							yield return delay;
						}
						else
						{
							SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
							if (requestStatus == SavedGameRequestStatus.Success)
							{
								TrophiesMemento cloudTrophies = future.Result.Value;
								int localTrophiesNegative = Storager.getInt("RatingNegative", false);
								int localTrophiesPositive = Storager.getInt("RatingPositive", false);
								bool localDirty = cloudTrophies.TrophiesNegative > localTrophiesNegative || cloudTrophies.TrophiesPositive > localTrophiesPositive;
								if (cloudTrophies.TrophiesNegative > localTrophiesNegative)
								{
									Storager.setInt("RatingNegative", cloudTrophies.TrophiesNegative, false);
								}
								if (cloudTrophies.TrophiesPositive > localTrophiesPositive)
								{
									Storager.setInt("RatingPositive", cloudTrophies.TrophiesPositive, false);
								}
								EventHandler handler = this.Updated;
								if (localDirty && handler != null)
								{
									handler(this, EventArgs.Empty);
								}
								bool cloudDirty = cloudTrophies.TrophiesNegative < localTrophiesNegative || cloudTrophies.TrophiesPositive < localTrophiesPositive;
								if (Defs.IsDeveloperBuild)
								{
									Debug.LogFormat("[Trophies] Succeeded to pull trophies: {0}, 'pullOnly':{1}, 'conflicted':{2}, 'cloudDirty':{3}", cloudTrophies, pullOnly, cloudTrophies.Conflicted, cloudDirty);
								}
								if (pullOnly || (!cloudTrophies.Conflicted && !cloudDirty))
								{
									break;
								}
								ScopeLogger scopeLogger2 = new ScopeLogger("TrophiesSynchronizer.PullGoogleCoroutine()", "PushGoogleCoroutine(conflict)", Defs.IsDeveloperBuild && !Application.isEditor);
								try
								{
									IEnumerator enumerator = PushGoogleCoroutine();
									while (enumerator.MoveNext())
									{
										yield return null;
									}
									break;
								}
								finally
								{
									scopeLogger2.Dispose();
								}
							}
							Debug.LogWarning("Failed to push trophies with status: " + requestStatus);
							yield return delay;
						}
					}
					i++;
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private IEnumerator PushGoogleCoroutine()
		{
			if (!Ready)
			{
				yield break;
			}
			ScopeLogger scopeLogger = new ScopeLogger("TrophiesSynchronizer.PushGoogleCoroutine()", Defs.IsDeveloperBuild && !Application.isEditor);
			try
			{
				TrophiesSynchronizerGoogleSavedGameFacade googleSavedGamesFacade = default(TrophiesSynchronizerGoogleSavedGameFacade);
				WaitForSeconds delay = new WaitForSeconds(30f);
				int i = 0;
				while (true)
				{
					int trophiesNegative = Storager.getInt("RatingNegative", false);
					int trophiesPositive = Storager.getInt("RatingPositive", false);
					TrophiesMemento localTrophies = new TrophiesMemento(trophiesNegative, trophiesPositive);
					string callee = string.Format(CultureInfo.InvariantCulture, "Push and wait {0} ({1})", localTrophies, i);
					using (ScopeLogger logger = new ScopeLogger("TrophiesSynchronizer.PushGoogleCoroutine()", callee, Defs.IsDeveloperBuild && !Application.isEditor))
					{
						Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> future = googleSavedGamesFacade.Push(localTrophies);
						while (!future.IsCompleted)
						{
							yield return null;
						}
						logger.Dispose();
						if (future.IsFaulted)
						{
							Exception ex = future.Exception.InnerExceptions.FirstOrDefault() ?? future.Exception;
							Debug.LogWarning("Failed to push trophies with exception: " + ex.Message);
							yield return delay;
						}
						else
						{
							SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
							if (requestStatus == SavedGameRequestStatus.Success)
							{
								if (Defs.IsDeveloperBuild)
								{
									ISavedGameMetadata metadata = future.Result.Value;
									string description = ((metadata == null) ? string.Empty : metadata.Description);
									Debug.LogFormat("[Trophies] Succeeded to push trophies with status: '{0}'", description);
								}
								break;
							}
							Debug.LogWarning("Failed to push trophies with status: " + requestStatus);
							yield return delay;
						}
					}
					i++;
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}
	}
}