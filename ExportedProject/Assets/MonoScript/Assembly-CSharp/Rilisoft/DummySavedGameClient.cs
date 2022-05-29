using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class DummySavedGameClient : ISavedGameClient
	{
		private int _conflictCounter;

		private readonly string _filename;

		private readonly DummySavedGameMetadata _dummySavedGameMetadata;

		public string Filename
		{
			get
			{
				return this._filename;
			}
		}

		public DummySavedGameClient(string filename)
		{
			this._filename = filename ?? string.Empty;
			this._dummySavedGameMetadata = new DummySavedGameMetadata(this._filename);
		}

		public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').CommitUpdate()", new object[] { this.GetType().Name, this.Filename });
			if (callback == null)
			{
				return;
			}
			Action action = () => callback(1, this._dummySavedGameMetadata);
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		public void Delete(ISavedGameMetadata metadata)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').Delete()", new object[] { this.GetType().Name, this.Filename });
		}

		public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').FetchAllSavedGames()", new object[] { this.GetType().Name, this.Filename });
			if (callback == null)
			{
				return;
			}
			Action action = () => callback(1, new List<ISavedGameMetadata>());
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').OpenWithAutomaticConflictResolution()", new object[] { this.GetType().Name, this.Filename });
			if (callback == null)
			{
				return;
			}
			Action action = () => callback(1, this._dummySavedGameMetadata);
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			bool flag = this._conflictCounter % 2 == 0;
			UnityEngine.Debug.LogFormat("{0}('{1}', {2}).OpenWithManualConflictResolution()", new object[] { this.GetType().Name, this.Filename, flag });
			if (!flag)
			{
				if (completedCallback == null)
				{
					return;
				}
				Action action = () => completedCallback(1, this._dummySavedGameMetadata);
				CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
			}
			else
			{
				if (conflictCallback == null)
				{
					return;
				}
				byte[] bytes = Encoding.UTF8.GetBytes("{}");
				Action action1 = () => conflictCallback(DummyConflictResolver.Instance, this._dummySavedGameMetadata, bytes, this._dummySavedGameMetadata, bytes);
				CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action1));
			}
			this._conflictCounter++;
		}

		public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').ReadBinaryData()", new object[] { this.GetType().Name, this.Filename });
			if (completedCallback == null)
			{
				return;
			}
			byte[] bytes = Encoding.UTF8.GetBytes("{}");
			Action action = () => completedCallback(1, bytes);
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').ShowSelectSavedGameUI()", new object[] { this.GetType().Name, this.Filename });
			if (callback == null)
			{
				return;
			}
			Action action = () => callback(1, this._dummySavedGameMetadata);
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		[DebuggerHidden]
		private IEnumerator WaitAndExecuteCoroutine(Action action)
		{
			DummySavedGameClient.u003cWaitAndExecuteCoroutineu003ec__Iterator1D0 variable = null;
			return variable;
		}
	}
}