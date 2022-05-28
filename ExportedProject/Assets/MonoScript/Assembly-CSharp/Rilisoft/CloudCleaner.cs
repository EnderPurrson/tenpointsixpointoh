using System;
using System.Runtime.CompilerServices;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	[Obsolete]
	internal sealed class CloudCleaner
	{
		[CompilerGenerated]
		private sealed class _003CCleanSavedGameFile_003Ec__AnonStorey2D5
		{
			internal string filename;

			internal Action<ISavedGameMetadata> commit;

			internal void _003C_003Em__3A5(ISavedGameMetadata metadata)
			{
				byte[] updatedBinaryData = new byte[0];
				SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(string.Format("Cleaned by '{0}'", SystemInfo.deviceModel)).Build();
				PlayGamesPlatform.Instance.SavedGame.CommitUpdate(metadata, updateForMetadata, updatedBinaryData, _003C_003Em__3A8);
			}

			internal void _003C_003Em__3A6(SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata)
			{
				Debug.LogFormat("------ Open '{0}': {1} '{2}'", filename, openStatus, openMetadata.GetDescription());
				if (openStatus == SavedGameRequestStatus.Success)
				{
					commit(openMetadata);
				}
			}

			internal void _003C_003Em__3A7(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				resolver.ChooseMetadata(unmerged);
				Debug.LogFormat("------ Partially resolved using unmerged metadata '{0}': '{1}'", filename, unmerged.GetDescription());
				commit(unmerged);
			}

			internal void _003C_003Em__3A8(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
			{
				Debug.LogFormat("------ Cleaned after conflict '{0}': {1} '{2}'", filename, writeStatus, closeMetadata.GetDescription());
			}
		}

		private static CloudCleaner _instance;

		public static CloudCleaner Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new CloudCleaner();
				}
				return _instance;
			}
		}

		public void CleanSavedGameFile(string filename)
		{
			_003CCleanSavedGameFile_003Ec__AnonStorey2D5 _003CCleanSavedGameFile_003Ec__AnonStorey2D = new _003CCleanSavedGameFile_003Ec__AnonStorey2D5();
			_003CCleanSavedGameFile_003Ec__AnonStorey2D.filename = filename;
			if (string.IsNullOrEmpty(_003CCleanSavedGameFile_003Ec__AnonStorey2D.filename))
			{
				throw new ArgumentException("Filename should not be empty", _003CCleanSavedGameFile_003Ec__AnonStorey2D.filename);
			}
			if (PlayGamesPlatform.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				return;
			}
			_003CCleanSavedGameFile_003Ec__AnonStorey2D.commit = _003CCleanSavedGameFile_003Ec__AnonStorey2D._003C_003Em__3A5;
			Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback = _003CCleanSavedGameFile_003Ec__AnonStorey2D._003C_003Em__3A6;
			ConflictCallback conflictCallback = _003CCleanSavedGameFile_003Ec__AnonStorey2D._003C_003Em__3A7;
			Debug.LogFormat("------ Trying to open '{0}'...", _003CCleanSavedGameFile_003Ec__AnonStorey2D.filename);
			PlayGamesPlatform.Instance.SavedGame.OpenWithManualConflictResolution(_003CCleanSavedGameFile_003Ec__AnonStorey2D.filename, DataSource.ReadNetworkOnly, true, conflictCallback, completedCallback);
		}
	}
}
