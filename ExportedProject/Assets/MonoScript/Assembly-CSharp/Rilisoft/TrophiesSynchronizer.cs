using GooglePlayGames.BasicApi.SavedGame;
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
	internal sealed class TrophiesSynchronizer
	{
		private const string TrophiesNegativeKey = "RatingNegative";

		private const string TrophiesPositiveKey = "RatingPositive";

		private readonly static TrophiesSynchronizer _instance;

		private EventHandler Updated;

		public static TrophiesSynchronizer Instance
		{
			get
			{
				return TrophiesSynchronizer._instance;
			}
		}

		private bool Ready
		{
			get
			{
				return true;
			}
		}

		static TrophiesSynchronizer()
		{
			TrophiesSynchronizer._instance = new TrophiesSynchronizer();
		}

		private TrophiesSynchronizer()
		{
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
		private IEnumerator PushGoogleCoroutine()
		{
			TrophiesSynchronizer.u003cPushGoogleCoroutineu003ec__Iterator1D4 variable = null;
			return variable;
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
			return null;
		}

		private void SyncAmazon()
		{
			string str = string.Format(CultureInfo.InvariantCulture, "{0}.SyncAmazon()", new object[] { this.GetType().Name });
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				AGSWhispersyncClient.Synchronize();
				using (AGSGameDataMap gameData = AGSWhispersyncClient.GetGameData())
				{
					if (gameData != null)
					{
						using (AGSGameDataMap map = gameData.GetMap("trophiesMap"))
						{
							if (map != null)
							{
								AGSSyncableNumber highestNumber = map.GetHighestNumber("trophiesNegative");
								AGSSyncableNumber aGSSyncableNumber = map.GetHighestNumber("trophiesPositive");
								int num = (highestNumber == null ? 0 : highestNumber.AsInt());
								int num1 = (aGSSyncableNumber == null ? 0 : aGSSyncableNumber.AsInt());
								TrophiesMemento trophiesMemento = new TrophiesMemento(num, num1);
								int num2 = Storager.getInt("RatingNegative", false);
								int num3 = Storager.getInt("RatingPositive", false);
								TrophiesMemento trophiesMemento1 = new TrophiesMemento(num2, num3);
								TrophiesMemento trophiesMemento2 = TrophiesMemento.Merge(trophiesMemento1, trophiesMemento);
								if (Defs.IsDeveloperBuild)
								{
									UnityEngine.Debug.LogFormat("Local trophies progress: {0}", new object[] { JsonUtility.ToJson(trophiesMemento1) });
									UnityEngine.Debug.LogFormat("Cloud trophies progress: {0}", new object[] { JsonUtility.ToJson(trophiesMemento) });
									UnityEngine.Debug.LogFormat("Merged trophies progress: {0}", new object[] { JsonUtility.ToJson(trophiesMemento2) });
								}
								if ((num2 < trophiesMemento2.TrophiesNegative ? true : num3 < trophiesMemento2.TrophiesPositive))
								{
									Storager.setInt("RatingNegative", trophiesMemento2.TrophiesNegative, false);
									Storager.setInt("RatingPositive", trophiesMemento2.TrophiesPositive, false);
									EventHandler updated = this.Updated;
									if (updated != null)
									{
										updated(this, EventArgs.Empty);
									}
								}
								if ((num < trophiesMemento2.TrophiesNegative ? true : num1 < trophiesMemento2.TrophiesPositive))
								{
									highestNumber.Set(trophiesMemento2.TrophiesNegative);
									aGSSyncableNumber.Set(trophiesMemento2.TrophiesPositive);
								}
							}
							else
							{
								UnityEngine.Debug.LogWarning("trophiesMap == null");
								return;
							}
						}
						AGSWhispersyncClient.Synchronize();
					}
					else
					{
						UnityEngine.Debug.LogWarning("dataMap == null");
						return;
					}
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
			TrophiesSynchronizer.u003cSyncGoogleCoroutineu003ec__Iterator1D3 variable = null;
			return variable;
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