using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	internal struct CampaignProgressSynchronizerGpgFacade
	{
		public const string Filename = "CampaignProgress";

		private readonly static DummySavedGameClient _dummy;

		private static ISavedGameClient SavedGame
		{
			get
			{
				ISavedGameClient savedGame;
				try
				{
					if (PlayGamesPlatform.Instance == null)
					{
						savedGame = CampaignProgressSynchronizerGpgFacade._dummy;
					}
					else if (PlayGamesPlatform.Instance.SavedGame != null)
					{
						savedGame = PlayGamesPlatform.Instance.SavedGame;
					}
					else
					{
						savedGame = CampaignProgressSynchronizerGpgFacade._dummy;
					}
				}
				catch (NullReferenceException nullReferenceException)
				{
					savedGame = CampaignProgressSynchronizerGpgFacade._dummy;
				}
				return savedGame;
			}
		}

		static CampaignProgressSynchronizerGpgFacade()
		{
			CampaignProgressSynchronizerGpgFacade._dummy = new DummySavedGameClient("CampaignProgress");
		}

		public Task<GoogleSavedGameRequestResult<CampaignProgressMemento>> Pull()
		{
			Task<GoogleSavedGameRequestResult<CampaignProgressMemento>> task;
			string str = string.Concat(this.GetType().Name, ".Pull()");
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>>();
				ScopeLogger scopeLogger1 = new ScopeLogger(str, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild);
				try
				{
					CampaignProgressSynchronizerGpgFacade.PullCallback pullCallback = new CampaignProgressSynchronizerGpgFacade.PullCallback(taskCompletionSource);
					CampaignProgressSynchronizerGpgFacade.PullCallback pullCallback1 = pullCallback;
					CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(pullCallback.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
				}
				finally
				{
					scopeLogger1.Dispose();
				}
				task = taskCompletionSource.Task;
			}
			finally
			{
				scopeLogger.Dispose();
			}
			return task;
		}

		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(CampaignProgressMemento campaignProgress)
		{
			Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> task;
			string str = string.Format(CultureInfo.InvariantCulture, "{0}.Push({1})", new object[] { this.GetType().Name, campaignProgress.Levels.Count });
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				ScopeLogger scopeLogger1 = new ScopeLogger(str, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild);
				try
				{
					CampaignProgressSynchronizerGpgFacade.PushCallback pushCallback = new CampaignProgressSynchronizerGpgFacade.PushCallback(campaignProgress, taskCompletionSource);
					CampaignProgressSynchronizerGpgFacade.PushCallback pushCallback1 = pushCallback;
					CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(pushCallback.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
				}
				finally
				{
					scopeLogger1.Dispose();
				}
				task = taskCompletionSource.Task;
			}
			finally
			{
				scopeLogger.Dispose();
			}
			return task;
		}

		private abstract class Callback
		{
			protected CampaignProgressMemento? _resolved;

			protected abstract DataSource DefaultDataSource
			{
				get;
			}

			protected Callback()
			{
			}

			protected abstract void HandleAuthenticationCompleted(bool succeeded);

			internal abstract void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata);

			internal void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenConflict('{2}', '{3}')", new object[] { typeof(CampaignProgressSynchronizerGpgFacade).Name, this.GetType().Name, original.Description, unmerged.Description });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					CampaignProgressMemento campaignProgressMemento = CampaignProgressSynchronizerGpgFacade.Callback.Parse(originalData);
					CampaignProgressMemento campaignProgressMemento1 = CampaignProgressSynchronizerGpgFacade.Callback.Parse(unmergedData);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[CampaignProgress] original: {0}, unmerged: {1}", new object[] { campaignProgressMemento, campaignProgressMemento1 });
					}
					int num = campaignProgressMemento.Levels.Sum<LevelProgressMemento>((LevelProgressMemento l) => l.CoinCount + l.GemCount);
					resolver.ChooseMetadata((num < campaignProgressMemento1.Levels.Sum<LevelProgressMemento>((LevelProgressMemento l) => l.CoinCount + l.GemCount) ? unmerged : original));
					CampaignProgressMemento campaignProgressMemento2 = CampaignProgressMemento.Merge(campaignProgressMemento, campaignProgressMemento1);
					this._resolved = new CampaignProgressMemento?(this.MergeWithResolved(campaignProgressMemento2, true));
					CampaignProgressSynchronizerGpgFacade.Callback callback = this;
					CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(callback.HandleOpenCompleted));
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			protected CampaignProgressMemento MergeWithResolved(CampaignProgressMemento other, bool forceConflicted)
			{
				CampaignProgressMemento campaignProgressMemento = (!this._resolved.HasValue ? other : CampaignProgressMemento.Merge(this._resolved.Value, other));
				if (forceConflicted)
				{
					campaignProgressMemento.SetConflicted();
				}
				return campaignProgressMemento;
			}

			protected static CampaignProgressMemento Parse(byte[] data)
			{
				CampaignProgressMemento campaignProgressMemento;
				if (data == null || (int)data.Length <= 0)
				{
					return new CampaignProgressMemento();
				}
				string str = Encoding.UTF8.GetString(data, 0, (int)data.Length);
				if (string.IsNullOrEmpty(str))
				{
					return new CampaignProgressMemento();
				}
				try
				{
					campaignProgressMemento = JsonUtility.FromJson<CampaignProgressMemento>(str);
				}
				catch (ArgumentException argumentException1)
				{
					ArgumentException argumentException = argumentException1;
					Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[] { typeof(CampaignProgressMemento).Name, str });
					Debug.LogException(argumentException);
					campaignProgressMemento = new CampaignProgressMemento();
				}
				return campaignProgressMemento;
			}

			protected abstract void TrySetException(Exception ex);
		}

		private sealed class PullCallback : CampaignProgressSynchronizerGpgFacade.Callback
		{
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> _promise;

			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			public PullCallback(TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> promise)
			{
				this._promise = promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>>();
			}

			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[] { typeof(CampaignProgressSynchronizerGpgFacade).Name, base.GetType().Name, succeeded });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					if (succeeded)
					{
						CampaignProgressSynchronizerGpgFacade.PullCallback pullCallback = this;
						CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback.HandleOpenCompleted));
					}
					else
					{
						TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> taskCompletionSource = this._promise;
						CampaignProgressMemento campaignProgressMemento = new CampaignProgressMemento();
						taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(SavedGameRequestStatus.AuthenticationError, campaignProgressMemento));
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string str = (metadata == null ? string.Empty : metadata.Description);
				string str1 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[] { typeof(CampaignProgressSynchronizerGpgFacade).Name, base.GetType().Name, requestStatus, str });
				ScopeLogger scopeLogger = new ScopeLogger(str1, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
						case SavedGameRequestStatus.AuthenticationError:
						{
							GpgFacade instance = GpgFacade.Instance;
							CampaignProgressSynchronizerGpgFacade.PullCallback pullCallback = this;
							instance.Authenticate(new Action<bool>(pullCallback.HandleAuthenticationCompleted), true);
							break;
						}
						case SavedGameRequestStatus.InternalError:
						case 0:
						{
							TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> taskCompletionSource = this._promise;
							CampaignProgressMemento campaignProgressMemento = new CampaignProgressMemento();
							taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, campaignProgressMemento));
							break;
						}
						case SavedGameRequestStatus.TimeoutError:
						{
							CampaignProgressSynchronizerGpgFacade.PullCallback pullCallback1 = this;
							CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
							break;
						}
						case SavedGameRequestStatus.Success:
						{
							CampaignProgressSynchronizerGpgFacade.SavedGame.ReadBinaryData(metadata, new Action<SavedGameRequestStatus, byte[]>(this.HandleReadCompleted));
							break;
						}
						default:
						{
							goto case 0;
						}
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			private void HandleReadCompleted(SavedGameRequestStatus requestStatus, byte[] data)
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				object[] name = new object[] { typeof(CampaignProgressSynchronizerGpgFacade).Name, base.GetType().Name, requestStatus, null };
				name[3] = (data == null ? 0 : (int)data.Length);
				string str = string.Format(invariantCulture, "{0}.{1}.HandleReadCompleted('{2}', {3})", name);
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
						case SavedGameRequestStatus.AuthenticationError:
						{
							GpgFacade instance = GpgFacade.Instance;
							CampaignProgressSynchronizerGpgFacade.PullCallback pullCallback = this;
							instance.Authenticate(new Action<bool>(pullCallback.HandleAuthenticationCompleted), true);
							break;
						}
						case SavedGameRequestStatus.InternalError:
						case 0:
						{
							TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> taskCompletionSource = this._promise;
							CampaignProgressMemento campaignProgressMemento = new CampaignProgressMemento();
							taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, campaignProgressMemento));
							break;
						}
						case SavedGameRequestStatus.TimeoutError:
						{
							CampaignProgressSynchronizerGpgFacade.PullCallback pullCallback1 = this;
							CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
							break;
						}
						case SavedGameRequestStatus.Success:
						{
							CampaignProgressMemento campaignProgressMemento1 = CampaignProgressSynchronizerGpgFacade.Callback.Parse(data);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("[CampaignProgress] Incoming: {0}", new object[] { campaignProgressMemento1 });
							}
							CampaignProgressMemento campaignProgressMemento2 = base.MergeWithResolved(campaignProgressMemento1, false);
							this._promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, campaignProgressMemento2));
							break;
						}
						default:
						{
							goto case 0;
						}
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}
		}

		private sealed class PushCallback : CampaignProgressSynchronizerGpgFacade.Callback
		{
			private readonly CampaignProgressMemento _campaignProgressMemento;

			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> _promise;

			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			public PushCallback(CampaignProgressMemento campaignProgressMemento, TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> promise)
			{
				this._campaignProgressMemento = campaignProgressMemento;
				this._promise = promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
			}

			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[] { typeof(CampaignProgressSynchronizerGpgFacade).Name, base.GetType().Name, succeeded });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					if (succeeded)
					{
						CampaignProgressSynchronizerGpgFacade.PushCallback pushCallback = this;
						CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback.HandleOpenCompleted));
					}
					else
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(SavedGameRequestStatus.AuthenticationError, null));
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			private void HandleCommitCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string str = (metadata == null ? string.Empty : metadata.Description);
				string str1 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleCommitCompleted('{2}', '{3}')", new object[] { typeof(CampaignProgressSynchronizerGpgFacade).Name, base.GetType().Name, requestStatus, str });
				ScopeLogger scopeLogger = new ScopeLogger(str1, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
						case SavedGameRequestStatus.AuthenticationError:
						{
							GpgFacade instance = GpgFacade.Instance;
							CampaignProgressSynchronizerGpgFacade.PushCallback pushCallback = this;
							instance.Authenticate(new Action<bool>(pushCallback.HandleAuthenticationCompleted), true);
							break;
						}
						case SavedGameRequestStatus.InternalError:
						{
							GoogleSavedGameRequestResult<ISavedGameMetadata> googleSavedGameRequestResult = new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata);
							this._promise.TrySetResult(googleSavedGameRequestResult);
							break;
						}
						case SavedGameRequestStatus.TimeoutError:
						{
							CampaignProgressSynchronizerGpgFacade.PushCallback pushCallback1 = this;
							CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
							break;
						}
						default:
						{
							goto case SavedGameRequestStatus.InternalError;
						}
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string str;
				string str1 = (metadata == null ? string.Empty : metadata.Description);
				string str2 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[] { typeof(CampaignProgressSynchronizerGpgFacade).Name, base.GetType().Name, requestStatus, str1 });
				ScopeLogger scopeLogger = new ScopeLogger(str2, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
						case SavedGameRequestStatus.AuthenticationError:
						{
							GpgFacade instance = GpgFacade.Instance;
							CampaignProgressSynchronizerGpgFacade.PushCallback pushCallback = this;
							instance.Authenticate(new Action<bool>(pushCallback.HandleAuthenticationCompleted), true);
							break;
						}
						case SavedGameRequestStatus.InternalError:
						case 0:
						{
							this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata));
							break;
						}
						case SavedGameRequestStatus.TimeoutError:
						{
							CampaignProgressSynchronizerGpgFacade.PushCallback pushCallback1 = this;
							CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
							break;
						}
						case SavedGameRequestStatus.Success:
						{
							CampaignProgressMemento campaignProgressMemento = base.MergeWithResolved(this._campaignProgressMemento, false);
							if (!campaignProgressMemento.Conflicted)
							{
								str = (!this._resolved.HasValue ? "none" : "trivial");
							}
							else
							{
								str = "resolved";
							}
							string str3 = str;
							string str4 = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", new object[] { SystemInfo.deviceModel, str3 });
							SavedGameMetadataUpdate savedGameMetadataUpdate = (new SavedGameMetadataUpdate.Builder()).WithUpdatedDescription(str4).Build();
							string json = JsonUtility.ToJson(campaignProgressMemento);
							byte[] bytes = Encoding.UTF8.GetBytes(json);
							CampaignProgressSynchronizerGpgFacade.SavedGame.CommitUpdate(metadata, savedGameMetadataUpdate, bytes, new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleCommitCompleted));
							break;
						}
						default:
						{
							goto case 0;
						}
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}
		}
	}
}