using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[Obsolete]
	internal sealed class CloudCleaner
	{
		private static CloudCleaner _instance;

		public static CloudCleaner Instance
		{
			get
			{
				if (CloudCleaner._instance == null)
				{
					CloudCleaner._instance = new CloudCleaner();
				}
				return CloudCleaner._instance;
			}
		}

		public CloudCleaner()
		{
		}

		public void CleanSavedGameFile(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentException("Filename should not be empty", filename);
			}
			if (PlayGamesPlatform.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				return;
			}
			Action<ISavedGameMetadata> action = (ISavedGameMetadata metadata) => {
				byte[] numArray = new byte[0];
				SavedGameMetadataUpdate savedGameMetadataUpdate = (new SavedGameMetadataUpdate.Builder()).WithUpdatedDescription(string.Format("Cleaned by '{0}'", SystemInfo.deviceModel)).Build();
				PlayGamesPlatform.Instance.SavedGame.CommitUpdate(metadata, savedGameMetadataUpdate, numArray, (SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata) => Debug.LogFormat("------ Cleaned after conflict '{0}': {1} '{2}'", new object[] { filename, writeStatus, closeMetadata.GetDescription() }));
			};
			Action<SavedGameRequestStatus, ISavedGameMetadata> action1 = (SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata) => {
				Debug.LogFormat("------ Open '{0}': {1} '{2}'", new object[] { filename, openStatus, openMetadata.GetDescription() });
				if (openStatus != SavedGameRequestStatus.Success)
				{
					return;
				}
				action(openMetadata);
			};
			ConflictCallback conflictCallback = (IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData) => {
				ISavedGameMetadata savedGameMetadatum = unmerged;
				resolver.ChooseMetadata(savedGameMetadatum);
				Debug.LogFormat("------ Partially resolved using unmerged metadata '{0}': '{1}'", new object[] { filename, unmerged.GetDescription() });
				action(savedGameMetadatum);
			};
			Debug.LogFormat("------ Trying to open '{0}'...", new object[] { filename });
			PlayGamesPlatform.Instance.SavedGame.OpenWithManualConflictResolution(filename, DataSource.ReadNetworkOnly, true, conflictCallback, action1);
		}
	}
}