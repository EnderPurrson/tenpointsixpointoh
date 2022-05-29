using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace GooglePlayGames.Native
{
	internal class NativeSavedGameClient : ISavedGameClient
	{
		private readonly static Regex ValidFilenameRegex;

		private readonly GooglePlayGames.Native.PInvoke.SnapshotManager mSnapshotManager;

		static NativeSavedGameClient()
		{
			NativeSavedGameClient.ValidFilenameRegex = new Regex("\\A[a-zA-Z0-9-._~]{1,100}\\Z");
		}

		internal NativeSavedGameClient(GooglePlayGames.Native.PInvoke.SnapshotManager manager)
		{
			this.mSnapshotManager = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.SnapshotManager>(manager);
		}

		private static Types.SnapshotConflictPolicy AsConflictPolicy(ConflictResolutionStrategy strategy)
		{
			switch (strategy)
			{
				case ConflictResolutionStrategy.UseLongestPlaytime:
				{
					return Types.SnapshotConflictPolicy.LONGEST_PLAYTIME;
				}
				case ConflictResolutionStrategy.UseOriginal:
				{
					return Types.SnapshotConflictPolicy.LAST_KNOWN_GOOD;
				}
				case ConflictResolutionStrategy.UseUnmerged:
				{
					return Types.SnapshotConflictPolicy.MOST_RECENTLY_MODIFIED;
				}
			}
			throw new InvalidOperationException(string.Concat("Found unhandled strategy: ", strategy));
		}

		private static Types.DataSource AsDataSource(DataSource source)
		{
			DataSource dataSource = source;
			if (dataSource == DataSource.ReadCacheOrNetwork)
			{
				return Types.DataSource.CACHE_OR_NETWORK;
			}
			if (dataSource != DataSource.ReadNetworkOnly)
			{
				throw new InvalidOperationException(string.Concat("Found unhandled DataSource: ", source));
			}
			return Types.DataSource.NETWORK_ONLY;
		}

		private static NativeSnapshotMetadataChange AsMetadataChange(SavedGameMetadataUpdate update)
		{
			NativeSnapshotMetadataChange.Builder builder = new NativeSnapshotMetadataChange.Builder();
			if (update.IsCoverImageUpdated)
			{
				builder.SetCoverImageFromPngData(update.UpdatedPngCoverImage);
			}
			if (update.IsDescriptionUpdated)
			{
				builder.SetDescription(update.UpdatedDescription);
			}
			if (update.IsPlayedTimeUpdated)
			{
				TimeSpan? updatedPlayedTime = update.UpdatedPlayedTime;
				builder.SetPlayedTime((ulong)updatedPlayedTime.Value.TotalMilliseconds);
			}
			return builder.Build();
		}

		private static SavedGameRequestStatus AsRequestStatus(CommonErrorStatus.SnapshotOpenStatus status)
		{
			CommonErrorStatus.SnapshotOpenStatus snapshotOpenStatu = status;
			switch (snapshotOpenStatu)
			{
				case CommonErrorStatus.SnapshotOpenStatus.ERROR_TIMEOUT:
				{
					return SavedGameRequestStatus.TimeoutError;
				}
				case CommonErrorStatus.SnapshotOpenStatus.ERROR_NOT_AUTHORIZED:
				{
					return SavedGameRequestStatus.AuthenticationError;
				}
				default:
				{
					if (snapshotOpenStatu == CommonErrorStatus.SnapshotOpenStatus.VALID)
					{
						break;
					}
					else
					{
						Logger.e(string.Concat("Encountered unknown status: ", status));
						return SavedGameRequestStatus.InternalError;
					}
				}
			}
			return SavedGameRequestStatus.Success;
		}

		private static SavedGameRequestStatus AsRequestStatus(CommonErrorStatus.ResponseStatus status)
		{
			switch (status)
			{
				case CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
				{
					return SavedGameRequestStatus.TimeoutError;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
				case 0:
				{
					Logger.e(string.Concat("Unknown status: ", status));
					return SavedGameRequestStatus.InternalError;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_NOT_AUTHORIZED:
				{
					Logger.e("User was not authorized (they were probably not logged in).");
					return SavedGameRequestStatus.AuthenticationError;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_INTERNAL:
				{
					return SavedGameRequestStatus.InternalError;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
				{
					Logger.e("User attempted to use the game without a valid license.");
					return SavedGameRequestStatus.AuthenticationError;
				}
				case CommonErrorStatus.ResponseStatus.VALID:
				case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				{
					return SavedGameRequestStatus.Success;
				}
				default:
				{
					Logger.e(string.Concat("Unknown status: ", status));
					return SavedGameRequestStatus.InternalError;
				}
			}
		}

		private static SelectUIStatus AsUIStatus(CommonErrorStatus.UIStatus uiStatus)
		{
			switch (uiStatus)
			{
				case CommonErrorStatus.UIStatus.ERROR_CANCELED:
				{
					return SelectUIStatus.UserClosedUI;
				}
				case CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
				{
					return SelectUIStatus.TimeoutError;
				}
				case CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED:
				case CommonErrorStatus.UIStatus.VALID | CommonErrorStatus.UIStatus.ERROR_INTERNAL | CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED | CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED | CommonErrorStatus.UIStatus.ERROR_TIMEOUT | CommonErrorStatus.UIStatus.ERROR_CANCELED | CommonErrorStatus.UIStatus.ERROR_UI_BUSY | CommonErrorStatus.UIStatus.ERROR_LEFT_ROOM:
				case 0:
				{
					Logger.e(string.Concat("Encountered unknown UI Status: ", uiStatus));
					return SelectUIStatus.InternalError;
				}
				case CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED:
				{
					return SelectUIStatus.AuthenticationError;
				}
				case CommonErrorStatus.UIStatus.ERROR_INTERNAL:
				{
					return SelectUIStatus.InternalError;
				}
				case CommonErrorStatus.UIStatus.VALID:
				{
					return SelectUIStatus.SavedGameSelected;
				}
				default:
				{
					Logger.e(string.Concat("Encountered unknown UI Status: ", uiStatus));
					return SelectUIStatus.InternalError;
				}
			}
		}

		public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			Action<SavedGameRequestStatus, ISavedGameMetadata> onGameThread = callback;
			Misc.CheckNotNull<ISavedGameMetadata>(metadata);
			Misc.CheckNotNull<byte[]>(updatedBinaryData);
			Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(onGameThread);
			onGameThread = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(onGameThread);
			NativeSnapshotMetadata nativeSnapshotMetadatum = metadata as NativeSnapshotMetadata;
			if (nativeSnapshotMetadatum == null)
			{
				Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
				onGameThread(-4, null);
				return;
			}
			if (!nativeSnapshotMetadatum.IsOpen)
			{
				Logger.e("This method requires an open ISavedGameMetadata.");
				onGameThread(-4, null);
				return;
			}
			this.mSnapshotManager.Commit(nativeSnapshotMetadatum, NativeSavedGameClient.AsMetadataChange(updateForMetadata), updatedBinaryData, (GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse response) => {
				if (response.RequestSucceeded())
				{
					onGameThread(1, response.Data());
				}
				else
				{
					onGameThread(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
				}
			});
		}

		public void Delete(ISavedGameMetadata metadata)
		{
			Misc.CheckNotNull<ISavedGameMetadata>(metadata);
			this.mSnapshotManager.Delete((NativeSnapshotMetadata)metadata);
		}

		public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
		{
			Action<SavedGameRequestStatus, List<ISavedGameMetadata>> onGameThread = callback;
			Misc.CheckNotNull<Action<SavedGameRequestStatus, List<ISavedGameMetadata>>>(onGameThread);
			onGameThread = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, List<ISavedGameMetadata>>(onGameThread);
			this.mSnapshotManager.FetchAll(NativeSavedGameClient.AsDataSource(source), (GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse response) => {
				if (response.RequestSucceeded())
				{
					onGameThread(1, response.Data().Cast<ISavedGameMetadata>().ToList<ISavedGameMetadata>());
				}
				else
				{
					onGameThread(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), new List<ISavedGameMetadata>());
				}
			});
		}

		private void InternalManualOpen(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			this.mSnapshotManager.Open(filename, NativeSavedGameClient.AsDataSource(source), Types.SnapshotConflictPolicy.MANUAL, (GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse response) => {
				if (!response.RequestSucceeded())
				{
					completedCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
				}
				else if (response.ResponseStatus() == CommonErrorStatus.SnapshotOpenStatus.VALID)
				{
					completedCallback(1, response.Data());
				}
				else if (response.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					Logger.e("Unhandled response status");
					completedCallback(-2, null);
				}
				else
				{
					NativeSnapshotMetadata nativeSnapshotMetadatum = response.ConflictOriginal();
					NativeSnapshotMetadata nativeSnapshotMetadatum1 = response.ConflictUnmerged();
					NativeSavedGameClient.NativeConflictResolver nativeConflictResolver = new NativeSavedGameClient.NativeConflictResolver(this.mSnapshotManager, response.ConflictId(), nativeSnapshotMetadatum, nativeSnapshotMetadatum1, completedCallback, () => this.InternalManualOpen(filename, source, prefetchDataOnConflict, conflictCallback, completedCallback));
					if (!prefetchDataOnConflict)
					{
						conflictCallback(nativeConflictResolver, nativeSnapshotMetadatum, null, nativeSnapshotMetadatum1, null);
						return;
					}
					NativeSavedGameClient.Prefetcher prefetcher = new NativeSavedGameClient.Prefetcher((byte[] originalData, byte[] unmergedData) => conflictCallback(nativeConflictResolver, nativeSnapshotMetadatum, originalData, nativeSnapshotMetadatum1, unmergedData), completedCallback);
					this.mSnapshotManager.Read(nativeSnapshotMetadatum, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse>(prefetcher.OnOriginalDataRead));
					this.mSnapshotManager.Read(nativeSnapshotMetadatum1, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse>(prefetcher.OnUnmergedDataRead));
				}
			});
		}

		internal static bool IsValidFilename(string filename)
		{
			if (filename == null)
			{
				return false;
			}
			return NativeSavedGameClient.ValidFilenameRegex.IsMatch(filename);
		}

		public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			Action<SavedGameRequestStatus, ISavedGameMetadata> onGameThread = callback;
			Misc.CheckNotNull<string>(filename);
			Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(onGameThread);
			onGameThread = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(onGameThread);
			if (!NativeSavedGameClient.IsValidFilename(filename))
			{
				Logger.e(string.Concat("Received invalid filename: ", filename));
				onGameThread(-4, null);
				return;
			}
			this.OpenWithManualConflictResolution(filename, source, false, (IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData) => {
				switch (resolutionStrategy)
				{
					case ConflictResolutionStrategy.UseLongestPlaytime:
					{
						if (original.TotalTimePlayed < unmerged.TotalTimePlayed)
						{
							resolver.ChooseMetadata(unmerged);
						}
						else
						{
							resolver.ChooseMetadata(original);
						}
						return;
					}
					case ConflictResolutionStrategy.UseOriginal:
					{
						resolver.ChooseMetadata(original);
						return;
					}
					case ConflictResolutionStrategy.UseUnmerged:
					{
						resolver.ChooseMetadata(unmerged);
						return;
					}
				}
				Logger.e(string.Concat("Unhandled strategy ", resolutionStrategy));
				onGameThread(-2, null);
			}, onGameThread);
		}

		public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			Misc.CheckNotNull<string>(filename);
			Misc.CheckNotNull<ConflictCallback>(conflictCallback);
			Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completedCallback);
			conflictCallback = this.ToOnGameThread(conflictCallback);
			completedCallback = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(completedCallback);
			if (NativeSavedGameClient.IsValidFilename(filename))
			{
				this.InternalManualOpen(filename, source, prefetchDataOnConflict, conflictCallback, completedCallback);
				return;
			}
			Logger.e(string.Concat("Received invalid filename: ", filename));
			completedCallback(-4, null);
		}

		public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
		{
			Action<SavedGameRequestStatus, byte[]> onGameThread = completedCallback;
			Misc.CheckNotNull<ISavedGameMetadata>(metadata);
			Misc.CheckNotNull<Action<SavedGameRequestStatus, byte[]>>(onGameThread);
			onGameThread = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, byte[]>(onGameThread);
			NativeSnapshotMetadata nativeSnapshotMetadatum = metadata as NativeSnapshotMetadata;
			if (nativeSnapshotMetadatum == null)
			{
				Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
				onGameThread(-4, null);
				return;
			}
			if (!nativeSnapshotMetadatum.IsOpen)
			{
				Logger.e("This method requires an open ISavedGameMetadata.");
				onGameThread(-4, null);
				return;
			}
			this.mSnapshotManager.Read(nativeSnapshotMetadatum, (GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse response) => {
				if (response.RequestSucceeded())
				{
					onGameThread(1, response.Data());
				}
				else
				{
					onGameThread(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
				}
			});
		}

		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
		{
			Action<SelectUIStatus, ISavedGameMetadata> onGameThread = callback;
			Misc.CheckNotNull<string>(uiTitle);
			Misc.CheckNotNull<Action<SelectUIStatus, ISavedGameMetadata>>(onGameThread);
			onGameThread = NativeSavedGameClient.ToOnGameThread<SelectUIStatus, ISavedGameMetadata>(onGameThread);
			if (maxDisplayedSavedGames <= 0)
			{
				Logger.e("maxDisplayedSavedGames must be greater than 0");
				onGameThread(-4, null);
				return;
			}
			this.mSnapshotManager.SnapshotSelectUI(showCreateSaveUI, showDeleteSaveUI, maxDisplayedSavedGames, uiTitle, (GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse response) => onGameThread(NativeSavedGameClient.AsUIStatus(response.RequestStatus()), (!response.RequestSucceeded() ? null : response.Data())));
		}

		private ConflictCallback ToOnGameThread(ConflictCallback conflictCallback)
		{
			return (IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData) => {
				Logger.d("Invoking conflict callback");
				PlayGamesHelperObject.RunOnGameThread(() => conflictCallback(resolver, original, originalData, unmerged, unmergedData));
			};
		}

		private static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
		{
			return (T1 val1, T2 val2) => PlayGamesHelperObject.RunOnGameThread(() => toConvert(val1, val2));
		}

		private class NativeConflictResolver : IConflictResolver
		{
			private readonly GooglePlayGames.Native.PInvoke.SnapshotManager mManager;

			private readonly string mConflictId;

			private readonly NativeSnapshotMetadata mOriginal;

			private readonly NativeSnapshotMetadata mUnmerged;

			private readonly Action<SavedGameRequestStatus, ISavedGameMetadata> mCompleteCallback;

			private readonly Action mRetryFileOpen;

			internal NativeConflictResolver(GooglePlayGames.Native.PInvoke.SnapshotManager manager, string conflictId, NativeSnapshotMetadata original, NativeSnapshotMetadata unmerged, Action<SavedGameRequestStatus, ISavedGameMetadata> completeCallback, Action retryOpen)
			{
				this.mManager = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.SnapshotManager>(manager);
				this.mConflictId = Misc.CheckNotNull<string>(conflictId);
				this.mOriginal = Misc.CheckNotNull<NativeSnapshotMetadata>(original);
				this.mUnmerged = Misc.CheckNotNull<NativeSnapshotMetadata>(unmerged);
				this.mCompleteCallback = Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completeCallback);
				this.mRetryFileOpen = Misc.CheckNotNull<Action>(retryOpen);
			}

			public void ChooseMetadata(ISavedGameMetadata chosenMetadata)
			{
				NativeSnapshotMetadata nativeSnapshotMetadatum = chosenMetadata as NativeSnapshotMetadata;
				if (nativeSnapshotMetadatum != this.mOriginal && nativeSnapshotMetadatum != this.mUnmerged)
				{
					Logger.e("Caller attempted to choose a version of the metadata that was not part of the conflict");
					this.mCompleteCallback(-4, null);
					return;
				}
				this.mManager.Resolve(nativeSnapshotMetadatum, (new NativeSnapshotMetadataChange.Builder()).Build(), this.mConflictId, (GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse response) => {
					if (response.RequestSucceeded())
					{
						this.mRetryFileOpen();
						return;
					}
					this.mCompleteCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
				});
			}
		}

		private class Prefetcher
		{
			private readonly object mLock;

			private bool mOriginalDataFetched;

			private byte[] mOriginalData;

			private bool mUnmergedDataFetched;

			private byte[] mUnmergedData;

			private Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback;

			private readonly Action<byte[], byte[]> mDataFetchedCallback;

			internal Prefetcher(Action<byte[], byte[]> dataFetchedCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
			{
				this.mDataFetchedCallback = Misc.CheckNotNull<Action<byte[], byte[]>>(dataFetchedCallback);
				this.completedCallback = Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completedCallback);
			}

			private void MaybeProceed()
			{
				if (!this.mOriginalDataFetched || !this.mUnmergedDataFetched)
				{
					Logger.d(string.Concat(new object[] { "Not all data fetched - original:", this.mOriginalDataFetched, " unmerged:", this.mUnmergedDataFetched }));
				}
				else
				{
					Logger.d("Fetched data for original and unmerged, proceeding");
					this.mDataFetchedCallback(this.mOriginalData, this.mUnmergedData);
				}
			}

			internal void OnOriginalDataRead(GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse readResponse)
			{
				object obj = this.mLock;
				Monitor.Enter(obj);
				try
				{
					if (readResponse.RequestSucceeded())
					{
						Logger.d("Successfully fetched original data");
						this.mOriginalDataFetched = true;
						this.mOriginalData = readResponse.Data();
						this.MaybeProceed();
					}
					else
					{
						Logger.e("Encountered error while prefetching original data.");
						this.completedCallback(NativeSavedGameClient.AsRequestStatus(readResponse.ResponseStatus()), null);
						this.completedCallback = (SavedGameRequestStatus argument0, ISavedGameMetadata argument1) => {
						};
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			internal void OnUnmergedDataRead(GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse readResponse)
			{
				object obj = this.mLock;
				Monitor.Enter(obj);
				try
				{
					if (readResponse.RequestSucceeded())
					{
						Logger.d("Successfully fetched unmerged data");
						this.mUnmergedDataFetched = true;
						this.mUnmergedData = readResponse.Data();
						this.MaybeProceed();
					}
					else
					{
						Logger.e("Encountered error while prefetching unmerged data.");
						this.completedCallback(NativeSavedGameClient.AsRequestStatus(readResponse.ResponseStatus()), null);
						this.completedCallback = (SavedGameRequestStatus argument0, ISavedGameMetadata argument1) => {
						};
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}
		}
	}
}