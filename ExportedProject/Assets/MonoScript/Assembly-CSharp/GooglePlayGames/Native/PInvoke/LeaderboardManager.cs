using System;
using System.Runtime.CompilerServices;
using AOT;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	internal class LeaderboardManager
	{
		[CompilerGenerated]
		private sealed class _003CLoadLeaderboardData_003Ec__AnonStorey26D
		{
			internal ScorePageToken token;

			internal string playerId;

			internal int rowCount;

			internal Action<LeaderboardScoreData> callback;

			internal LeaderboardManager _003C_003Ef__this;

			internal void _003C_003Em__12B(FetchResponse rsp)
			{
				_003C_003Ef__this.HandleFetch(token, rsp, playerId, rowCount, callback);
			}
		}

		[CompilerGenerated]
		private sealed class _003CHandleFetch_003Ec__AnonStorey26E
		{
			internal LeaderboardScoreData data;

			internal string selfPlayerId;

			internal int maxResults;

			internal ScorePageToken token;

			internal Action<LeaderboardScoreData> callback;

			internal LeaderboardManager _003C_003Ef__this;

			internal void _003C_003Em__12C(FetchScoreSummaryResponse rsp)
			{
				_003C_003Ef__this.HandleFetchScoreSummary(data, rsp, selfPlayerId, maxResults, token, callback);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadScorePage_003Ec__AnonStorey26F
		{
			internal LeaderboardScoreData data;

			internal ScorePageToken token;

			internal Action<LeaderboardScoreData> callback;

			internal LeaderboardManager _003C_003Ef__this;

			internal void _003C_003Em__12D(FetchScorePageResponse rsp)
			{
				_003C_003Ef__this.HandleFetchScorePage(data, token, rsp, callback);
			}
		}

		private readonly GameServices mServices;

		internal int LeaderboardMaxResults
		{
			get
			{
				return 25;
			}
		}

		internal LeaderboardManager(GameServices services)
		{
			mServices = Misc.CheckNotNull(services);
		}

		internal void SubmitScore(string leaderboardId, long score, string metadata)
		{
			Misc.CheckNotNull(leaderboardId, "leaderboardId");
			Logger.d("Native Submitting score: " + score + " for lb " + leaderboardId + " with metadata: " + metadata);
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_SubmitScore(mServices.AsHandle(), leaderboardId, (ulong)score, metadata ?? string.Empty);
		}

		internal void ShowAllUI(Action<CommonErrorStatus.UIStatus> callback)
		{
			Misc.CheckNotNull(callback);
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ShowAllUI(mServices.AsHandle(), Callbacks.InternalShowUICallback, Callbacks.ToIntPtr(callback));
		}

		internal void ShowUI(string leaderboardId, LeaderboardTimeSpan span, Action<CommonErrorStatus.UIStatus> callback)
		{
			Misc.CheckNotNull(callback);
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ShowUI(mServices.AsHandle(), leaderboardId, (Types.LeaderboardTimeSpan)span, Callbacks.InternalShowUICallback, Callbacks.ToIntPtr(callback));
		}

		public void LoadLeaderboardData(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, string playerId, Action<LeaderboardScoreData> callback)
		{
			_003CLoadLeaderboardData_003Ec__AnonStorey26D _003CLoadLeaderboardData_003Ec__AnonStorey26D = new _003CLoadLeaderboardData_003Ec__AnonStorey26D();
			_003CLoadLeaderboardData_003Ec__AnonStorey26D.playerId = playerId;
			_003CLoadLeaderboardData_003Ec__AnonStorey26D.rowCount = rowCount;
			_003CLoadLeaderboardData_003Ec__AnonStorey26D.callback = callback;
			_003CLoadLeaderboardData_003Ec__AnonStorey26D._003C_003Ef__this = this;
			NativeScorePageToken internalObject = new NativeScorePageToken(GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ScorePageToken(mServices.AsHandle(), leaderboardId, (Types.LeaderboardStart)start, (Types.LeaderboardTimeSpan)timeSpan, (Types.LeaderboardCollection)collection));
			_003CLoadLeaderboardData_003Ec__AnonStorey26D.token = new ScorePageToken(internalObject, leaderboardId, collection, timeSpan);
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_Fetch(mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, leaderboardId, InternalFetchCallback, Callbacks.ToIntPtr(_003CLoadLeaderboardData_003Ec__AnonStorey26D._003C_003Em__12B, FetchResponse.FromPointer));
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchCallback))]
		private static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
		}

		internal void HandleFetch(ScorePageToken token, FetchResponse response, string selfPlayerId, int maxResults, Action<LeaderboardScoreData> callback)
		{
			_003CHandleFetch_003Ec__AnonStorey26E _003CHandleFetch_003Ec__AnonStorey26E = new _003CHandleFetch_003Ec__AnonStorey26E();
			_003CHandleFetch_003Ec__AnonStorey26E.selfPlayerId = selfPlayerId;
			_003CHandleFetch_003Ec__AnonStorey26E.maxResults = maxResults;
			_003CHandleFetch_003Ec__AnonStorey26E.token = token;
			_003CHandleFetch_003Ec__AnonStorey26E.callback = callback;
			_003CHandleFetch_003Ec__AnonStorey26E._003C_003Ef__this = this;
			_003CHandleFetch_003Ec__AnonStorey26E.data = new LeaderboardScoreData(_003CHandleFetch_003Ec__AnonStorey26E.token.LeaderboardId, (ResponseStatus)response.GetStatus());
			if (response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID && response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				Logger.w("Error returned from fetch: " + response.GetStatus());
				_003CHandleFetch_003Ec__AnonStorey26E.callback(_003CHandleFetch_003Ec__AnonStorey26E.data);
			}
			else
			{
				_003CHandleFetch_003Ec__AnonStorey26E.data.Title = response.Leaderboard().Title();
				_003CHandleFetch_003Ec__AnonStorey26E.data.Id = _003CHandleFetch_003Ec__AnonStorey26E.token.LeaderboardId;
				GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummary(mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, _003CHandleFetch_003Ec__AnonStorey26E.token.LeaderboardId, (Types.LeaderboardTimeSpan)_003CHandleFetch_003Ec__AnonStorey26E.token.TimeSpan, (Types.LeaderboardCollection)_003CHandleFetch_003Ec__AnonStorey26E.token.Collection, InternalFetchSummaryCallback, Callbacks.ToIntPtr(_003CHandleFetch_003Ec__AnonStorey26E._003C_003Em__12C, FetchScoreSummaryResponse.FromPointer));
			}
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScoreSummaryCallback))]
		private static void InternalFetchSummaryCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchSummaryCallback", Callbacks.Type.Temporary, response, data);
		}

		internal void HandleFetchScoreSummary(LeaderboardScoreData data, FetchScoreSummaryResponse response, string selfPlayerId, int maxResults, ScorePageToken token, Action<LeaderboardScoreData> callback)
		{
			if (response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID && response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				Logger.w("Error returned from fetchScoreSummary: " + response);
				data.Status = (ResponseStatus)response.GetStatus();
				callback(data);
				return;
			}
			NativeScoreSummary scoreSummary = response.GetScoreSummary();
			data.ApproximateCount = scoreSummary.ApproximateResults();
			data.PlayerScore = scoreSummary.LocalUserScore().AsScore(data.Id, selfPlayerId);
			if (maxResults <= 0)
			{
				callback(data);
			}
			else
			{
				LoadScorePage(data, maxResults, token, callback);
			}
		}

		public void LoadScorePage(LeaderboardScoreData data, int maxResults, ScorePageToken token, Action<LeaderboardScoreData> callback)
		{
			_003CLoadScorePage_003Ec__AnonStorey26F _003CLoadScorePage_003Ec__AnonStorey26F = new _003CLoadScorePage_003Ec__AnonStorey26F();
			_003CLoadScorePage_003Ec__AnonStorey26F.data = data;
			_003CLoadScorePage_003Ec__AnonStorey26F.token = token;
			_003CLoadScorePage_003Ec__AnonStorey26F.callback = callback;
			_003CLoadScorePage_003Ec__AnonStorey26F._003C_003Ef__this = this;
			if (_003CLoadScorePage_003Ec__AnonStorey26F.data == null)
			{
				_003CLoadScorePage_003Ec__AnonStorey26F.data = new LeaderboardScoreData(_003CLoadScorePage_003Ec__AnonStorey26F.token.LeaderboardId);
			}
			NativeScorePageToken nativeScorePageToken = (NativeScorePageToken)_003CLoadScorePage_003Ec__AnonStorey26F.token.InternalObject;
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScorePage(mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, nativeScorePageToken.AsPointer(), (uint)maxResults, InternalFetchScorePage, Callbacks.ToIntPtr(_003CLoadScorePage_003Ec__AnonStorey26F._003C_003Em__12D, FetchScorePageResponse.FromPointer));
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScorePageCallback))]
		private static void InternalFetchScorePage(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchScorePage", Callbacks.Type.Temporary, response, data);
		}

		internal void HandleFetchScorePage(LeaderboardScoreData data, ScorePageToken token, FetchScorePageResponse rsp, Action<LeaderboardScoreData> callback)
		{
			data.Status = (ResponseStatus)rsp.GetStatus();
			if (rsp.GetStatus() != CommonErrorStatus.ResponseStatus.VALID && rsp.GetStatus() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				callback(data);
			}
			NativeScorePage scorePage = rsp.GetScorePage();
			if (!scorePage.Valid())
			{
				callback(data);
			}
			if (scorePage.HasNextScorePage())
			{
				data.NextPageToken = new ScorePageToken(scorePage.GetNextScorePageToken(), token.LeaderboardId, token.Collection, token.TimeSpan);
			}
			if (scorePage.HasPrevScorePage())
			{
				data.PrevPageToken = new ScorePageToken(scorePage.GetPreviousScorePageToken(), token.LeaderboardId, token.Collection, token.TimeSpan);
			}
			foreach (NativeScoreEntry item in scorePage)
			{
				data.AddScore(item.AsScore(data.Id));
			}
			callback(data);
		}
	}
}
