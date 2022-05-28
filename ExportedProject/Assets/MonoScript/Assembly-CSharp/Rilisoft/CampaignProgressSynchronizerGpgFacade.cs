using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	internal struct CampaignProgressSynchronizerGpgFacade
	{
		private abstract class Callback
		{
			protected CampaignProgressMemento? _resolved;

			[CompilerGenerated]
			private static Func<LevelProgressMemento, int> _003C_003Ef__am_0024cache1;

			[CompilerGenerated]
			private static Func<LevelProgressMemento, int> _003C_003Ef__am_0024cache2;

			protected abstract DataSource DefaultDataSource { get; }

			internal abstract void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata);

			protected abstract void HandleAuthenticationCompleted(bool succeeded);

			protected abstract void TrySetException(Exception ex);

			internal void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenConflict('{2}', '{3}')", typeof(CampaignProgressSynchronizerGpgFacade).Name, GetType().Name, original.Description, unmerged.Description);
				ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
				try
				{
					CampaignProgressMemento campaignProgressMemento = Parse(originalData);
					CampaignProgressMemento campaignProgressMemento2 = Parse(unmergedData);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[CampaignProgress] original: {0}, unmerged: {1}", campaignProgressMemento, campaignProgressMemento2);
					}
					List<LevelProgressMemento> levels = campaignProgressMemento.Levels;
					if (_003C_003Ef__am_0024cache1 == null)
					{
						_003C_003Ef__am_0024cache1 = _003CHandleOpenConflict_003Em__57A;
					}
					int num = levels.Sum(_003C_003Ef__am_0024cache1);
					List<LevelProgressMemento> levels2 = campaignProgressMemento2.Levels;
					if (_003C_003Ef__am_0024cache2 == null)
					{
						_003C_003Ef__am_0024cache2 = _003CHandleOpenConflict_003Em__57B;
					}
					int num2 = levels2.Sum(_003C_003Ef__am_0024cache2);
					ISavedGameMetadata savedGameMetadata;
					if (num >= num2)
					{
						savedGameMetadata = original;
					}
					else
					{
						savedGameMetadata = unmerged;
					}
					ISavedGameMetadata chosenMetadata = savedGameMetadata;
					resolver.ChooseMetadata(chosenMetadata);
					CampaignProgressMemento other = CampaignProgressMemento.Merge(campaignProgressMemento, campaignProgressMemento2);
					_resolved = MergeWithResolved(other, true);
					SavedGame.OpenWithManualConflictResolution("CampaignProgress", DefaultDataSource, true, HandleOpenConflict, HandleOpenCompleted);
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			protected CampaignProgressMemento MergeWithResolved(CampaignProgressMemento other, bool forceConflicted)
			{
				CampaignProgressMemento result = ((!_resolved.HasValue) ? other : CampaignProgressMemento.Merge(_resolved.Value, other));
				if (forceConflicted)
				{
					result.SetConflicted();
				}
				return result;
			}

			protected static CampaignProgressMemento Parse(byte[] data)
			{
				//Discarded unreachable code: IL_004e, IL_0091
				if (data == null || data.Length <= 0)
				{
					return default(CampaignProgressMemento);
				}
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				if (string.IsNullOrEmpty(@string))
				{
					return default(CampaignProgressMemento);
				}
				try
				{
					return JsonUtility.FromJson<CampaignProgressMemento>(@string);
				}
				catch (ArgumentException exception)
				{
					Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", typeof(CampaignProgressMemento).Name, @string);
					Debug.LogException(exception);
					return default(CampaignProgressMemento);
				}
			}

			[CompilerGenerated]
			private static int _003CHandleOpenConflict_003Em__57A(LevelProgressMemento l)
			{
				return l.CoinCount + l.GemCount;
			}

			[CompilerGenerated]
			private static int _003CHandleOpenConflict_003Em__57B(LevelProgressMemento l)
			{
				return l.CoinCount + l.GemCount;
			}
		}

		private sealed class PushCallback : Callback
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
				_campaignProgressMemento = campaignProgressMemento;
				_promise = promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
			}

			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", typeof(CampaignProgressSynchronizerGpgFacade).Name, GetType().Name, succeeded);
				ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
				try
				{
					if (!succeeded)
					{
						_promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(SavedGameRequestStatus.AuthenticationError, null));
					}
					else
					{
						SavedGame.OpenWithManualConflictResolution("CampaignProgress", DefaultDataSource, true, base.HandleOpenConflict, HandleOpenCompleted);
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			protected override void TrySetException(Exception ex)
			{
				_promise.TrySetException(ex);
			}

			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = ((metadata == null) ? string.Empty : metadata.Description);
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", typeof(CampaignProgressSynchronizerGpgFacade).Name, GetType().Name, requestStatus, text);
				ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
					case SavedGameRequestStatus.Success:
					{
						CampaignProgressMemento campaignProgressMemento = MergeWithResolved(_campaignProgressMemento, false);
						string text2 = (campaignProgressMemento.Conflicted ? "resolved" : ((!_resolved.HasValue) ? "none" : "trivial"));
						string description = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", SystemInfo.deviceModel, text2);
						SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
						string s = JsonUtility.ToJson(campaignProgressMemento);
						byte[] bytes = Encoding.UTF8.GetBytes(s);
						SavedGame.CommitUpdate(metadata, updateForMetadata, bytes, HandleCommitCompleted);
						break;
					}
					case SavedGameRequestStatus.TimeoutError:
						SavedGame.OpenWithManualConflictResolution("CampaignProgress", DefaultDataSource, true, base.HandleOpenConflict, HandleOpenCompleted);
						break;
					case SavedGameRequestStatus.AuthenticationError:
						GpgFacade.Instance.Authenticate(HandleAuthenticationCompleted, true);
						break;
					default:
						_promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata));
						break;
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			private void HandleCommitCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = ((metadata == null) ? string.Empty : metadata.Description);
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleCommitCompleted('{2}', '{3}')", typeof(CampaignProgressSynchronizerGpgFacade).Name, GetType().Name, requestStatus, text);
				ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
					case SavedGameRequestStatus.TimeoutError:
						SavedGame.OpenWithManualConflictResolution("CampaignProgress", DefaultDataSource, true, base.HandleOpenConflict, HandleOpenCompleted);
						break;
					case SavedGameRequestStatus.AuthenticationError:
						GpgFacade.Instance.Authenticate(HandleAuthenticationCompleted, true);
						break;
					default:
					{
						GoogleSavedGameRequestResult<ISavedGameMetadata> result = new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata);
						_promise.TrySetResult(result);
						break;
					}
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}
		}

		private sealed class PullCallback : Callback
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
				_promise = promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>>();
			}

			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", typeof(CampaignProgressSynchronizerGpgFacade).Name, GetType().Name, succeeded);
				ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
				try
				{
					if (!succeeded)
					{
						_promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(SavedGameRequestStatus.AuthenticationError, default(CampaignProgressMemento)));
					}
					else
					{
						SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, base.HandleOpenConflict, HandleOpenCompleted);
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			protected override void TrySetException(Exception ex)
			{
				_promise.TrySetException(ex);
			}

			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = ((metadata == null) ? string.Empty : metadata.Description);
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", typeof(CampaignProgressSynchronizerGpgFacade).Name, GetType().Name, requestStatus, text);
				ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
					case SavedGameRequestStatus.Success:
						SavedGame.ReadBinaryData(metadata, HandleReadCompleted);
						break;
					case SavedGameRequestStatus.TimeoutError:
						SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, base.HandleOpenConflict, HandleOpenCompleted);
						break;
					case SavedGameRequestStatus.AuthenticationError:
						GpgFacade.Instance.Authenticate(HandleAuthenticationCompleted, true);
						break;
					default:
						_promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, default(CampaignProgressMemento)));
						break;
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}

			private void HandleReadCompleted(SavedGameRequestStatus requestStatus, byte[] data)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleReadCompleted('{2}', {3})", typeof(CampaignProgressSynchronizerGpgFacade).Name, GetType().Name, requestStatus, (data != null) ? data.Length : 0);
				ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
				try
				{
					switch (requestStatus)
					{
					case SavedGameRequestStatus.Success:
					{
						CampaignProgressMemento campaignProgressMemento = Callback.Parse(data);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[CampaignProgress] Incoming: {0}", campaignProgressMemento);
						}
						CampaignProgressMemento value = MergeWithResolved(campaignProgressMemento, false);
						_promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, value));
						break;
					}
					case SavedGameRequestStatus.TimeoutError:
						SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, base.HandleOpenConflict, HandleOpenCompleted);
						break;
					case SavedGameRequestStatus.AuthenticationError:
						GpgFacade.Instance.Authenticate(HandleAuthenticationCompleted, true);
						break;
					default:
						_promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, default(CampaignProgressMemento)));
						break;
					}
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}
		}

		public const string Filename = "CampaignProgress";

		private static readonly DummySavedGameClient _dummy = new DummySavedGameClient("CampaignProgress");

		private static ISavedGameClient SavedGame
		{
			get
			{
				//Discarded unreachable code: IL_003f, IL_0050
				try
				{
					if (PlayGamesPlatform.Instance == null)
					{
						return _dummy;
					}
					if (PlayGamesPlatform.Instance.SavedGame == null)
					{
						return _dummy;
					}
					return PlayGamesPlatform.Instance.SavedGame;
				}
				catch (NullReferenceException)
				{
					return _dummy;
				}
			}
		}

		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(CampaignProgressMemento campaignProgress)
		{
			//Discarded unreachable code: IL_00b6
			string text = string.Format(CultureInfo.InvariantCulture, "{0}.Push({1})", GetType().Name, campaignProgress.Levels.Count);
			ScopeLogger scopeLogger = new ScopeLogger(text, Defs.IsDeveloperBuild);
			try
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				ScopeLogger scopeLogger2 = new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild);
				try
				{
					PushCallback pushCallback = new PushCallback(campaignProgress, taskCompletionSource);
					SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, pushCallback.HandleOpenConflict, pushCallback.HandleOpenCompleted);
				}
				finally
				{
					scopeLogger2.Dispose();
				}
				return taskCompletionSource.Task;
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		public Task<GoogleSavedGameRequestResult<CampaignProgressMemento>> Pull()
		{
			//Discarded unreachable code: IL_0093
			string text = GetType().Name + ".Pull()";
			ScopeLogger scopeLogger = new ScopeLogger(text, Defs.IsDeveloperBuild);
			try
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>>();
				ScopeLogger scopeLogger2 = new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild);
				try
				{
					PullCallback pullCallback = new PullCallback(taskCompletionSource);
					SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, pullCallback.HandleOpenConflict, pullCallback.HandleOpenCompleted);
				}
				finally
				{
					scopeLogger2.Dispose();
				}
				return taskCompletionSource.Task;
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}
	}
}
