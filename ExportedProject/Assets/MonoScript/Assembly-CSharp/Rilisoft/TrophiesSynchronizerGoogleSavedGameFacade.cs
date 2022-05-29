using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	public struct TrophiesSynchronizerGoogleSavedGameFacade
	{
		public const string Filename = "Trophies";

		private const string SavedGameClientIsNullMessage = "SavedGameClient is null.";

		private static ISavedGameClient SavedGame
		{
			get
			{
				ISavedGameClient savedGame;
				try
				{
					if (PlayGamesPlatform.Instance != null)
					{
						savedGame = PlayGamesPlatform.Instance.SavedGame;
					}
					else
					{
						savedGame = null;
					}
				}
				catch (NullReferenceException nullReferenceException)
				{
					savedGame = null;
				}
				return savedGame;
			}
		}

		public Task<GoogleSavedGameRequestResult<TrophiesMemento>> Pull()
		{
			Task<GoogleSavedGameRequestResult<TrophiesMemento>> task;
			string str = string.Concat(this.GetType().Name, ".Pull()");
			ScopeLogger scopeLogger = new ScopeLogger(str, (!Defs.IsDeveloperBuild ? false : !Application.isEditor));
			try
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>>();
				TrophiesSynchronizerGoogleSavedGameFacade.PullCallback pullCallback = new TrophiesSynchronizerGoogleSavedGameFacade.PullCallback(taskCompletionSource);
				if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
				{
					ScopeLogger scopeLogger1 = new ScopeLogger(str, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild);
					try
					{
						TrophiesSynchronizerGoogleSavedGameFacade.PullCallback pullCallback1 = pullCallback;
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(pullCallback.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
					}
					finally
					{
						scopeLogger1.Dispose();
					}
					task = taskCompletionSource.Task;
				}
				else
				{
					taskCompletionSource.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					task = taskCompletionSource.Task;
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
			return task;
		}

		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(TrophiesMemento trophies)
		{
			Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> task;
			string str = string.Format(CultureInfo.InvariantCulture, "{0}.Push({1})", new object[] { this.GetType().Name, trophies });
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				TrophiesSynchronizerGoogleSavedGameFacade.PushCallback pushCallback = new TrophiesSynchronizerGoogleSavedGameFacade.PushCallback(trophies, taskCompletionSource);
				if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
				{
					ScopeLogger scopeLogger1 = new ScopeLogger(str, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild);
					try
					{
						TrophiesSynchronizerGoogleSavedGameFacade.PushCallback pushCallback1 = pushCallback;
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(pushCallback.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
					}
					finally
					{
						scopeLogger1.Dispose();
					}
					task = taskCompletionSource.Task;
				}
				else
				{
					taskCompletionSource.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					task = taskCompletionSource.Task;
				}
			}
			finally
			{
				scopeLogger.Dispose();
			}
			return task;
		}

		private abstract class Callback
		{
			protected TrophiesMemento? _resolvedTrophies;

			protected Callback()
			{
			}

			protected abstract void HandleAuthenticationCompleted(bool succeeded);

			internal abstract void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata);

			internal void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.HandleOpenConflict('{1}', '{2}')", new object[] { this.GetType().Name, original.Description, unmerged.Description });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						TrophiesMemento trophiesMemento = TrophiesSynchronizerGoogleSavedGameFacade.Callback.ParseTrophies(originalData);
						TrophiesMemento trophiesMemento1 = TrophiesSynchronizerGoogleSavedGameFacade.Callback.ParseTrophies(unmergedData);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Trophies] Original: {0}, unmerged: {1}", new object[] { trophiesMemento, trophiesMemento1 });
						}
						if (trophiesMemento.TrophiesNegative >= trophiesMemento1.TrophiesNegative && trophiesMemento.TrophiesPositive >= trophiesMemento1.TrophiesPositive)
						{
							resolver.ChooseMetadata(original);
							this._resolvedTrophies = new TrophiesMemento?(this.MergeWithResolved(trophiesMemento, false));
						}
						else if (trophiesMemento.TrophiesNegative > trophiesMemento1.TrophiesNegative || trophiesMemento.TrophiesPositive > trophiesMemento1.TrophiesPositive)
						{
							resolver.ChooseMetadata((trophiesMemento.Trophies < trophiesMemento1.Trophies ? unmerged : original));
							TrophiesMemento trophiesMemento2 = TrophiesMemento.Merge(trophiesMemento, trophiesMemento1);
							this._resolvedTrophies = new TrophiesMemento?(this.MergeWithResolved(trophiesMemento2, true));
						}
						else
						{
							resolver.ChooseMetadata(unmerged);
							this._resolvedTrophies = new TrophiesMemento?(this.MergeWithResolved(trophiesMemento1, false));
						}
						TrophiesSynchronizerGoogleSavedGameFacade.Callback callback = this;
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(callback.HandleOpenCompleted));
					}
					else
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			protected TrophiesMemento MergeWithResolved(TrophiesMemento trophies, bool forceConflicted)
			{
				TrophiesMemento trophiesMemento = (!this._resolvedTrophies.HasValue ? trophies : TrophiesMemento.Merge(this._resolvedTrophies.Value, trophies));
				if (!forceConflicted)
				{
					return trophiesMemento;
				}
				return new TrophiesMemento(trophiesMemento.TrophiesNegative, trophiesMemento.TrophiesPositive, true);
			}

			protected static TrophiesMemento ParseTrophies(byte[] data)
			{
				TrophiesMemento trophiesMemento;
				if (data == null || (int)data.Length <= 0)
				{
					return new TrophiesMemento();
				}
				string str = Encoding.UTF8.GetString(data, 0, (int)data.Length);
				if (string.IsNullOrEmpty(str))
				{
					return new TrophiesMemento();
				}
				try
				{
					trophiesMemento = JsonUtility.FromJson<TrophiesMemento>(str);
				}
				catch (ArgumentException argumentException1)
				{
					ArgumentException argumentException = argumentException1;
					Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[] { typeof(TrophiesMemento).Name, str });
					Debug.LogException(argumentException);
					trophiesMemento = new TrophiesMemento();
				}
				return trophiesMemento;
			}

			protected abstract void TrySetException(Exception ex);
		}

		private sealed class PullCallback : TrophiesSynchronizerGoogleSavedGameFacade.Callback
		{
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> _promise;

			public PullCallback(TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> promise)
			{
				this._promise = promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>>();
			}

			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.HandleAuthenticationCompleted({1})", new object[] { base.GetType().Name, succeeded });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					if (!succeeded)
					{
						TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> taskCompletionSource = this._promise;
						TrophiesMemento trophiesMemento = new TrophiesMemento();
						taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<TrophiesMemento>(SavedGameRequestStatus.AuthenticationError, trophiesMemento));
					}
					else if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						TrophiesSynchronizerGoogleSavedGameFacade.PullCallback pullCallback = this;
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback.HandleOpenCompleted));
					}
					else
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
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
				string str1 = string.Format(CultureInfo.InvariantCulture, "{0}.HandleOpenCompleted('{1}', '{2}')", new object[] { base.GetType().Name, requestStatus, str });
				ScopeLogger scopeLogger = new ScopeLogger(str1, Defs.IsDeveloperBuild);
				try
				{
					if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						switch (requestStatus)
						{
							case SavedGameRequestStatus.AuthenticationError:
							{
								GpgFacade instance = GpgFacade.Instance;
								TrophiesSynchronizerGoogleSavedGameFacade.PullCallback pullCallback = this;
								instance.Authenticate(new Action<bool>(pullCallback.HandleAuthenticationCompleted), true);
								break;
							}
							case SavedGameRequestStatus.InternalError:
							case 0:
							{
								TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> taskCompletionSource = this._promise;
								TrophiesMemento trophiesMemento = new TrophiesMemento();
								taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<TrophiesMemento>(requestStatus, trophiesMemento));
								break;
							}
							case SavedGameRequestStatus.TimeoutError:
							{
								TrophiesSynchronizerGoogleSavedGameFacade.PullCallback pullCallback1 = this;
								TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
								break;
							}
							case SavedGameRequestStatus.Success:
							{
								TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.ReadBinaryData(metadata, new Action<SavedGameRequestStatus, byte[]>(this.HandleReadCompleted));
								break;
							}
							default:
							{
								goto case 0;
							}
						}
					}
					else
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
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
				object[] name = new object[] { base.GetType().Name, requestStatus, null };
				name[2] = (data == null ? 0 : (int)data.Length);
				string str = string.Format(invariantCulture, "{0}.HandleReadCompleted('{1}', {2})", name);
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
						case SavedGameRequestStatus.AuthenticationError:
						{
							GpgFacade instance = GpgFacade.Instance;
							TrophiesSynchronizerGoogleSavedGameFacade.PullCallback pullCallback = this;
							instance.Authenticate(new Action<bool>(pullCallback.HandleAuthenticationCompleted), true);
							break;
						}
						case SavedGameRequestStatus.InternalError:
						case 0:
						{
							TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> taskCompletionSource = this._promise;
							TrophiesMemento trophiesMemento = new TrophiesMemento();
							taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<TrophiesMemento>(requestStatus, trophiesMemento));
							break;
						}
						case SavedGameRequestStatus.TimeoutError:
						{
							if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
							{
								TrophiesSynchronizerGoogleSavedGameFacade.PullCallback pullCallback1 = this;
								TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
								break;
							}
							else
							{
								this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
								return;
							}
						}
						case SavedGameRequestStatus.Success:
						{
							TrophiesMemento trophiesMemento1 = TrophiesSynchronizerGoogleSavedGameFacade.Callback.ParseTrophies(data);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("[Trophies] Incoming: {0}", new object[] { trophiesMemento1 });
							}
							TrophiesMemento trophiesMemento2 = base.MergeWithResolved(trophiesMemento1, false);
							this._promise.TrySetResult(new GoogleSavedGameRequestResult<TrophiesMemento>(requestStatus, trophiesMemento2));
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

		private sealed class PushCallback : TrophiesSynchronizerGoogleSavedGameFacade.Callback
		{
			private readonly TrophiesMemento _trophies;

			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> _promise;

			public PushCallback(TrophiesMemento trophies, TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> promise)
			{
				this._trophies = trophies;
				this._promise = promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
			}

			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.HandleAuthenticationCompleted({1})", new object[] { base.GetType().Name, succeeded });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(SavedGameRequestStatus.AuthenticationError, null));
					}
					else if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						TrophiesSynchronizerGoogleSavedGameFacade.PushCallback pushCallback = this;
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback.HandleOpenCompleted));
					}
					else
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
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
				string str1 = string.Format(CultureInfo.InvariantCulture, "{0}.HandleCommitCompleted('{1}', '{2}')", new object[] { base.GetType().Name, requestStatus, str });
				ScopeLogger scopeLogger = new ScopeLogger(str1, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
						case SavedGameRequestStatus.AuthenticationError:
						{
							GpgFacade instance = GpgFacade.Instance;
							TrophiesSynchronizerGoogleSavedGameFacade.PushCallback pushCallback = this;
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
							if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
							{
								TrophiesSynchronizerGoogleSavedGameFacade.PushCallback pushCallback1 = this;
								TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
								break;
							}
							else
							{
								this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
								return;
							}
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
				string str2 = string.Format(CultureInfo.InvariantCulture, "{0}.HandleOpenCompleted('{1}', '{2}')", new object[] { base.GetType().Name, requestStatus, str1 });
				ScopeLogger scopeLogger = new ScopeLogger(str2, Defs.IsDeveloperBuild);
				try
				{
					if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						switch (requestStatus)
						{
							case SavedGameRequestStatus.AuthenticationError:
							{
								GpgFacade instance = GpgFacade.Instance;
								TrophiesSynchronizerGoogleSavedGameFacade.PushCallback pushCallback = this;
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
								TrophiesSynchronizerGoogleSavedGameFacade.PushCallback pushCallback1 = this;
								TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
								break;
							}
							case SavedGameRequestStatus.Success:
							{
								TrophiesMemento trophiesMemento = base.MergeWithResolved(this._trophies, false);
								if (!trophiesMemento.Conflicted)
								{
									str = (!this._resolvedTrophies.HasValue ? "none" : "trivial");
								}
								else
								{
									str = "resolved";
								}
								string str3 = str;
								string str4 = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", new object[] { SystemInfo.deviceModel, str3 });
								SavedGameMetadataUpdate savedGameMetadataUpdate = (new SavedGameMetadataUpdate.Builder()).WithUpdatedDescription(str4).Build();
								string json = JsonUtility.ToJson(trophiesMemento);
								byte[] bytes = Encoding.UTF8.GetBytes(json);
								TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.CommitUpdate(metadata, savedGameMetadataUpdate, bytes, new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleCommitCompleted));
								break;
							}
							default:
							{
								goto case 0;
							}
						}
					}
					else
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
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