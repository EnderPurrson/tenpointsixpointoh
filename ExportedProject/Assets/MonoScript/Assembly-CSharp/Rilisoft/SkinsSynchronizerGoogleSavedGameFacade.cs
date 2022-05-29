using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	public struct SkinsSynchronizerGoogleSavedGameFacade
	{
		public const string Filename = "Skins";

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

		public Task<GoogleSavedGameRequestResult<SkinsMemento>> Pull()
		{
			Task<GoogleSavedGameRequestResult<SkinsMemento>> task;
			string str = string.Concat(this.GetType().Name, ".Pull()");
			ScopeLogger scopeLogger = new ScopeLogger(str, (!Defs.IsDeveloperBuild ? false : !Application.isEditor));
			try
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>>();
				if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
				{
					ScopeLogger scopeLogger1 = new ScopeLogger(str, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild);
					try
					{
						SkinsSynchronizerGoogleSavedGameFacade.PullCallback pullCallback = new SkinsSynchronizerGoogleSavedGameFacade.PullCallback(taskCompletionSource);
						SkinsSynchronizerGoogleSavedGameFacade.PullCallback pullCallback1 = pullCallback;
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(pullCallback.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
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

		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(SkinsMemento skins)
		{
			Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> task;
			string str = string.Format(CultureInfo.InvariantCulture, "{0}.Push({1})", new object[] { this.GetType().Name, skins });
			ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
			try
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
				{
					ScopeLogger scopeLogger1 = new ScopeLogger(str, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild);
					try
					{
						SkinsSynchronizerGoogleSavedGameFacade.PushCallback pushCallback = new SkinsSynchronizerGoogleSavedGameFacade.PushCallback(skins, taskCompletionSource);
						SkinsSynchronizerGoogleSavedGameFacade.PushCallback pushCallback1 = pushCallback;
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(pushCallback.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
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
			protected SkinsMemento? _resolved;

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
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenConflict('{2}', '{3}')", new object[] { typeof(SkinsSynchronizerGoogleSavedGameFacade).Name, this.GetType().Name, original.Description, unmerged.Description });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						SkinsMemento skinsMemento = SkinsSynchronizerGoogleSavedGameFacade.Callback.Parse(originalData);
						SkinsMemento skinsMemento1 = SkinsSynchronizerGoogleSavedGameFacade.Callback.Parse(unmergedData);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Skins] Original: {0}, unmerged: {1}", new object[] { skinsMemento, skinsMemento1 });
						}
						HashSet<string> strs = new HashSet<string>(
							from s in skinsMemento.Skins
							select s.Id);
						HashSet<string> strs1 = new HashSet<string>(
							from s in skinsMemento1.Skins
							select s.Id);
						if (strs.IsSupersetOf(strs1))
						{
							resolver.ChooseMetadata(original);
							this._resolved = new SkinsMemento?(this.MergeWithResolved(skinsMemento, false));
						}
						else if (!strs.IsProperSubsetOf(strs1))
						{
							resolver.ChooseMetadata((strs.Count < strs1.Count ? unmerged : original));
							SkinsMemento skinsMemento2 = SkinsMemento.Merge(skinsMemento, skinsMemento1);
							this._resolved = new SkinsMemento?(this.MergeWithResolved(skinsMemento2, true));
						}
						else
						{
							resolver.ChooseMetadata(unmerged);
							this._resolved = new SkinsMemento?(this.MergeWithResolved(skinsMemento1, false));
						}
						SkinsSynchronizerGoogleSavedGameFacade.Callback callback = this;
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(callback.HandleOpenCompleted));
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

			protected SkinsMemento MergeWithResolved(SkinsMemento skins, bool forceConflicted)
			{
				SkinsMemento skinsMemento = (!this._resolved.HasValue ? skins : SkinsMemento.Merge(this._resolved.Value, skins));
				if (!forceConflicted)
				{
					return skinsMemento;
				}
				return new SkinsMemento(skinsMemento.Skins, skinsMemento.DeletedSkins, skinsMemento.Cape, true);
			}

			protected static SkinsMemento Parse(byte[] data)
			{
				SkinsMemento skinsMemento;
				if (data == null || (int)data.Length <= 0)
				{
					return new SkinsMemento();
				}
				string str = Encoding.UTF8.GetString(data, 0, (int)data.Length);
				if (string.IsNullOrEmpty(str))
				{
					return new SkinsMemento();
				}
				try
				{
					skinsMemento = JsonUtility.FromJson<SkinsMemento>(str);
				}
				catch (ArgumentException argumentException1)
				{
					ArgumentException argumentException = argumentException1;
					Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[] { typeof(SkinsMemento).Name, str });
					Debug.LogException(argumentException);
					skinsMemento = new SkinsMemento();
				}
				return skinsMemento;
			}

			protected abstract void TrySetException(Exception ex);
		}

		private sealed class PullCallback : SkinsSynchronizerGoogleSavedGameFacade.Callback
		{
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> _promise;

			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			public PullCallback(TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> promise)
			{
				this._promise = promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>>();
			}

			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[] { typeof(SkinsSynchronizerGoogleSavedGameFacade).Name, base.GetType().Name, succeeded });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					if (!succeeded)
					{
						TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> taskCompletionSource = this._promise;
						SkinsMemento skinsMemento = new SkinsMemento();
						taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<SkinsMemento>(SavedGameRequestStatus.AuthenticationError, skinsMemento));
					}
					else if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						SkinsSynchronizerGoogleSavedGameFacade.PullCallback pullCallback = this;
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback.HandleOpenCompleted));
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
				string str1 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[] { typeof(SkinsSynchronizerGoogleSavedGameFacade).Name, base.GetType().Name, requestStatus, str });
				ScopeLogger scopeLogger = new ScopeLogger(str1, Defs.IsDeveloperBuild);
				try
				{
					if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						switch (requestStatus)
						{
							case SavedGameRequestStatus.AuthenticationError:
							{
								GpgFacade instance = GpgFacade.Instance;
								SkinsSynchronizerGoogleSavedGameFacade.PullCallback pullCallback = this;
								instance.Authenticate(new Action<bool>(pullCallback.HandleAuthenticationCompleted), true);
								break;
							}
							case SavedGameRequestStatus.InternalError:
							case 0:
							{
								TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> taskCompletionSource = this._promise;
								SkinsMemento skinsMemento = new SkinsMemento();
								taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<SkinsMemento>(requestStatus, skinsMemento));
								break;
							}
							case SavedGameRequestStatus.TimeoutError:
							{
								SkinsSynchronizerGoogleSavedGameFacade.PullCallback pullCallback1 = this;
								SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
								break;
							}
							case SavedGameRequestStatus.Success:
							{
								SkinsSynchronizerGoogleSavedGameFacade.SavedGame.ReadBinaryData(metadata, new Action<SavedGameRequestStatus, byte[]>(this.HandleReadCompleted));
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
				object[] name = new object[] { typeof(SkinsSynchronizerGoogleSavedGameFacade).Name, base.GetType().Name, requestStatus, null };
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
							SkinsSynchronizerGoogleSavedGameFacade.PullCallback pullCallback = this;
							instance.Authenticate(new Action<bool>(pullCallback.HandleAuthenticationCompleted), true);
							break;
						}
						case SavedGameRequestStatus.InternalError:
						case 0:
						{
							TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> taskCompletionSource = this._promise;
							SkinsMemento skinsMemento = new SkinsMemento();
							taskCompletionSource.TrySetResult(new GoogleSavedGameRequestResult<SkinsMemento>(requestStatus, skinsMemento));
							break;
						}
						case SavedGameRequestStatus.TimeoutError:
						{
							if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
							{
								SkinsSynchronizerGoogleSavedGameFacade.PullCallback pullCallback1 = this;
								SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pullCallback1.HandleOpenCompleted));
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
							SkinsMemento skinsMemento1 = SkinsSynchronizerGoogleSavedGameFacade.Callback.Parse(data);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("[Skins] Incoming: {0}", new object[] { skinsMemento1 });
							}
							SkinsMemento skinsMemento2 = base.MergeWithResolved(skinsMemento1, false);
							this._promise.TrySetResult(new GoogleSavedGameRequestResult<SkinsMemento>(requestStatus, skinsMemento2));
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

		private sealed class PushCallback : SkinsSynchronizerGoogleSavedGameFacade.Callback
		{
			private readonly SkinsMemento _skins;

			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> _promise;

			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			public PushCallback(SkinsMemento skins, TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> promise)
			{
				this._skins = skins;
				this._promise = promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
			}

			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[] { typeof(SkinsSynchronizerGoogleSavedGameFacade).Name, base.GetType().Name, succeeded });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(SavedGameRequestStatus.AuthenticationError, null));
					}
					else if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						SkinsSynchronizerGoogleSavedGameFacade.PushCallback pushCallback = this;
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback.HandleOpenCompleted));
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
				string str1 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleCommitCompleted('{2}', '{3}')", new object[] { typeof(SkinsSynchronizerGoogleSavedGameFacade).Name, base.GetType().Name, requestStatus, str });
				ScopeLogger scopeLogger = new ScopeLogger(str1, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
						case SavedGameRequestStatus.AuthenticationError:
						{
							GpgFacade instance = GpgFacade.Instance;
							SkinsSynchronizerGoogleSavedGameFacade.PushCallback pushCallback = this;
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
							if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
							{
								SkinsSynchronizerGoogleSavedGameFacade.PushCallback pushCallback1 = this;
								SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
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
				string str2 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[] { typeof(SkinsSynchronizerGoogleSavedGameFacade).Name, base.GetType().Name, requestStatus, str1 });
				ScopeLogger scopeLogger = new ScopeLogger(str2, Defs.IsDeveloperBuild);
				try
				{
					if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame != null)
					{
						switch (requestStatus)
						{
							case SavedGameRequestStatus.AuthenticationError:
							{
								GpgFacade instance = GpgFacade.Instance;
								SkinsSynchronizerGoogleSavedGameFacade.PushCallback pushCallback = this;
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
								SkinsSynchronizerGoogleSavedGameFacade.PushCallback pushCallback1 = this;
								SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(pushCallback1.HandleOpenCompleted));
								break;
							}
							case SavedGameRequestStatus.Success:
							{
								SkinsMemento skinsMemento = base.MergeWithResolved(this._skins, false);
								if (!skinsMemento.Conflicted)
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
								string json = JsonUtility.ToJson(skinsMemento);
								byte[] bytes = Encoding.UTF8.GetBytes(json);
								SkinsSynchronizerGoogleSavedGameFacade.SavedGame.CommitUpdate(metadata, savedGameMetadataUpdate, bytes, new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleCommitCompleted));
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