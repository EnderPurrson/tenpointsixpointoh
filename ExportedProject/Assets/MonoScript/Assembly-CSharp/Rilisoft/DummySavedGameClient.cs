using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class DummySavedGameClient : ISavedGameClient
	{
		[CompilerGenerated]
		private sealed class _003CCommitUpdate_003Ec__AnonStorey34E
		{
			internal Action<SavedGameRequestStatus, ISavedGameMetadata> callback;

			internal DummySavedGameClient _003C_003Ef__this;

			internal void _003C_003Em__57C()
			{
				callback(SavedGameRequestStatus.Success, _003C_003Ef__this._dummySavedGameMetadata);
			}
		}

		[CompilerGenerated]
		private sealed class _003CFetchAllSavedGames_003Ec__AnonStorey34F
		{
			internal Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback;

			internal void _003C_003Em__57D()
			{
				callback(SavedGameRequestStatus.Success, new List<ISavedGameMetadata>());
			}
		}

		[CompilerGenerated]
		private sealed class _003COpenWithAutomaticConflictResolution_003Ec__AnonStorey350
		{
			internal Action<SavedGameRequestStatus, ISavedGameMetadata> callback;

			internal DummySavedGameClient _003C_003Ef__this;

			internal void _003C_003Em__57E()
			{
				callback(SavedGameRequestStatus.Success, _003C_003Ef__this._dummySavedGameMetadata);
			}
		}

		[CompilerGenerated]
		private sealed class _003COpenWithManualConflictResolution_003Ec__AnonStorey351
		{
			internal ConflictCallback conflictCallback;

			internal Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback;

			internal DummySavedGameClient _003C_003Ef__this;

			internal void _003C_003Em__580()
			{
				completedCallback(SavedGameRequestStatus.Success, _003C_003Ef__this._dummySavedGameMetadata);
			}
		}

		[CompilerGenerated]
		private sealed class _003COpenWithManualConflictResolution_003Ec__AnonStorey352
		{
			internal byte[] data;

			internal _003COpenWithManualConflictResolution_003Ec__AnonStorey351 _003C_003Ef__ref_0024849;

			internal DummySavedGameClient _003C_003Ef__this;

			internal void _003C_003Em__57F()
			{
				_003C_003Ef__ref_0024849.conflictCallback(DummyConflictResolver.Instance, _003C_003Ef__this._dummySavedGameMetadata, data, _003C_003Ef__this._dummySavedGameMetadata, data);
			}
		}

		[CompilerGenerated]
		private sealed class _003CReadBinaryData_003Ec__AnonStorey353
		{
			internal Action<SavedGameRequestStatus, byte[]> completedCallback;

			internal byte[] binaryData;

			internal void _003C_003Em__581()
			{
				completedCallback(SavedGameRequestStatus.Success, binaryData);
			}
		}

		[CompilerGenerated]
		private sealed class _003CShowSelectSavedGameUI_003Ec__AnonStorey354
		{
			internal Action<SelectUIStatus, ISavedGameMetadata> callback;

			internal DummySavedGameClient _003C_003Ef__this;

			internal void _003C_003Em__582()
			{
				callback(SelectUIStatus.SavedGameSelected, _003C_003Ef__this._dummySavedGameMetadata);
			}
		}

		private int _conflictCounter;

		private readonly string _filename;

		private readonly DummySavedGameMetadata _dummySavedGameMetadata;

		public string Filename
		{
			get
			{
				return _filename;
			}
		}

		public DummySavedGameClient(string filename)
		{
			_filename = filename ?? string.Empty;
			_dummySavedGameMetadata = new DummySavedGameMetadata(_filename);
		}

		public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			_003CCommitUpdate_003Ec__AnonStorey34E _003CCommitUpdate_003Ec__AnonStorey34E = new _003CCommitUpdate_003Ec__AnonStorey34E();
			_003CCommitUpdate_003Ec__AnonStorey34E.callback = callback;
			_003CCommitUpdate_003Ec__AnonStorey34E._003C_003Ef__this = this;
			Debug.LogFormat("{0}('{1}').CommitUpdate()", GetType().Name, Filename);
			if (_003CCommitUpdate_003Ec__AnonStorey34E.callback != null)
			{
				Action action = _003CCommitUpdate_003Ec__AnonStorey34E._003C_003Em__57C;
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		public void Delete(ISavedGameMetadata metadata)
		{
			Debug.LogFormat("{0}('{1}').Delete()", GetType().Name, Filename);
		}

		public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
		{
			_003CFetchAllSavedGames_003Ec__AnonStorey34F _003CFetchAllSavedGames_003Ec__AnonStorey34F = new _003CFetchAllSavedGames_003Ec__AnonStorey34F();
			_003CFetchAllSavedGames_003Ec__AnonStorey34F.callback = callback;
			Debug.LogFormat("{0}('{1}').FetchAllSavedGames()", GetType().Name, Filename);
			if (_003CFetchAllSavedGames_003Ec__AnonStorey34F.callback != null)
			{
				Action action = _003CFetchAllSavedGames_003Ec__AnonStorey34F._003C_003Em__57D;
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			_003COpenWithAutomaticConflictResolution_003Ec__AnonStorey350 _003COpenWithAutomaticConflictResolution_003Ec__AnonStorey = new _003COpenWithAutomaticConflictResolution_003Ec__AnonStorey350();
			_003COpenWithAutomaticConflictResolution_003Ec__AnonStorey.callback = callback;
			_003COpenWithAutomaticConflictResolution_003Ec__AnonStorey._003C_003Ef__this = this;
			Debug.LogFormat("{0}('{1}').OpenWithAutomaticConflictResolution()", GetType().Name, Filename);
			if (_003COpenWithAutomaticConflictResolution_003Ec__AnonStorey.callback != null)
			{
				Action action = _003COpenWithAutomaticConflictResolution_003Ec__AnonStorey._003C_003Em__57E;
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			_003COpenWithManualConflictResolution_003Ec__AnonStorey351 _003COpenWithManualConflictResolution_003Ec__AnonStorey = new _003COpenWithManualConflictResolution_003Ec__AnonStorey351();
			_003COpenWithManualConflictResolution_003Ec__AnonStorey.conflictCallback = conflictCallback;
			_003COpenWithManualConflictResolution_003Ec__AnonStorey.completedCallback = completedCallback;
			_003COpenWithManualConflictResolution_003Ec__AnonStorey._003C_003Ef__this = this;
			bool flag = _conflictCounter % 2 == 0;
			Debug.LogFormat("{0}('{1}', {2}).OpenWithManualConflictResolution()", GetType().Name, Filename, flag);
			if (flag)
			{
				_003COpenWithManualConflictResolution_003Ec__AnonStorey352 _003COpenWithManualConflictResolution_003Ec__AnonStorey2 = new _003COpenWithManualConflictResolution_003Ec__AnonStorey352();
				_003COpenWithManualConflictResolution_003Ec__AnonStorey2._003C_003Ef__ref_0024849 = _003COpenWithManualConflictResolution_003Ec__AnonStorey;
				_003COpenWithManualConflictResolution_003Ec__AnonStorey2._003C_003Ef__this = this;
				if (_003COpenWithManualConflictResolution_003Ec__AnonStorey.conflictCallback == null)
				{
					return;
				}
				_003COpenWithManualConflictResolution_003Ec__AnonStorey2.data = Encoding.UTF8.GetBytes("{}");
				Action action = _003COpenWithManualConflictResolution_003Ec__AnonStorey2._003C_003Em__57F;
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
			else
			{
				if (_003COpenWithManualConflictResolution_003Ec__AnonStorey.completedCallback == null)
				{
					return;
				}
				Action action2 = _003COpenWithManualConflictResolution_003Ec__AnonStorey._003C_003Em__580;
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action2));
			}
			_conflictCounter++;
		}

		public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
		{
			_003CReadBinaryData_003Ec__AnonStorey353 _003CReadBinaryData_003Ec__AnonStorey = new _003CReadBinaryData_003Ec__AnonStorey353();
			_003CReadBinaryData_003Ec__AnonStorey.completedCallback = completedCallback;
			Debug.LogFormat("{0}('{1}').ReadBinaryData()", GetType().Name, Filename);
			if (_003CReadBinaryData_003Ec__AnonStorey.completedCallback != null)
			{
				_003CReadBinaryData_003Ec__AnonStorey.binaryData = Encoding.UTF8.GetBytes("{}");
				Action action = _003CReadBinaryData_003Ec__AnonStorey._003C_003Em__581;
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
		{
			_003CShowSelectSavedGameUI_003Ec__AnonStorey354 _003CShowSelectSavedGameUI_003Ec__AnonStorey = new _003CShowSelectSavedGameUI_003Ec__AnonStorey354();
			_003CShowSelectSavedGameUI_003Ec__AnonStorey.callback = callback;
			_003CShowSelectSavedGameUI_003Ec__AnonStorey._003C_003Ef__this = this;
			Debug.LogFormat("{0}('{1}').ShowSelectSavedGameUI()", GetType().Name, Filename);
			if (_003CShowSelectSavedGameUI_003Ec__AnonStorey.callback != null)
			{
				Action action = _003CShowSelectSavedGameUI_003Ec__AnonStorey._003C_003Em__582;
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		private IEnumerator WaitAndExecuteCoroutine(Action action)
		{
			yield return null;
			action();
		}
	}
}
