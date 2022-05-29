using AOT;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class LeaderboardManager
	{
		private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal int LeaderboardMaxResults
		{
			get
			{
				return 25;
			}
		}

		internal LeaderboardManager(GooglePlayGames.Native.PInvoke.GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
		}

		internal void HandleFetch(ScorePageToken token, FetchResponse response, string selfPlayerId, int maxResults, Action<LeaderboardScoreData> callback)
		{
			LeaderboardScoreData leaderboardScoreDatum = new LeaderboardScoreData(token.LeaderboardId, (ResponseStatus)response.GetStatus());
			if (response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID && response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				Logger.w(string.Concat("Error returned from fetch: ", response.GetStatus()));
				callback(leaderboardScoreDatum);
				return;
			}
			leaderboardScoreDatum.Title = response.Leaderboard().Title();
			leaderboardScoreDatum.Id = token.LeaderboardId;
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummary(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, token.LeaderboardId, (Types.LeaderboardTimeSpan)token.TimeSpan, (Types.LeaderboardCollection)token.Collection, new GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScoreSummaryCallback(GooglePlayGames.Native.PInvoke.LeaderboardManager.InternalFetchSummaryCallback), Callbacks.ToIntPtr<FetchScoreSummaryResponse>((FetchScoreSummaryResponse rsp) => this.HandleFetchScoreSummary(leaderboardScoreDatum, rsp, selfPlayerId, maxResults, token, callback), new Func<IntPtr, FetchScoreSummaryResponse>(FetchScoreSummaryResponse.FromPointer)));
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
			IEnumerator<NativeScoreEntry> enumerator = scorePage.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					NativeScoreEntry current = enumerator.Current;
					data.AddScore(current.AsScore(data.Id));
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			callback(data);
		}

		internal void HandleFetchScoreSummary(LeaderboardScoreData data, FetchScoreSummaryResponse response, string selfPlayerId, int maxResults, ScorePageToken token, Action<LeaderboardScoreData> callback)
		{
			if (response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID && response.GetStatus() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				Logger.w(string.Concat("Error returned from fetchScoreSummary: ", response));
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
				return;
			}
			this.LoadScorePage(data, maxResults, token, callback);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchCallback))]
		private static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScorePageCallback))]
		private static void InternalFetchScorePage(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchScorePage", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScoreSummaryCallback))]
		private static void InternalFetchSummaryCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("LeaderboardManager#InternalFetchSummaryCallback", Callbacks.Type.Temporary, response, data);
		}

		public void LoadLeaderboardData(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, string playerId, Action<LeaderboardScoreData> callback)
		{
			NativeScorePageToken nativeScorePageToken = new NativeScorePageToken(GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ScorePageToken(this.mServices.AsHandle(), leaderboardId, (Types.LeaderboardStart)start, (Types.LeaderboardTimeSpan)timeSpan, (Types.LeaderboardCollection)collection));
			ScorePageToken scorePageToken = new ScorePageToken(nativeScorePageToken, leaderboardId, collection, timeSpan);
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_Fetch(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, leaderboardId, new GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchCallback(GooglePlayGames.Native.PInvoke.LeaderboardManager.InternalFetchCallback), Callbacks.ToIntPtr<FetchResponse>((FetchResponse rsp) => this.HandleFetch(scorePageToken, rsp, playerId, rowCount, callback), new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer)));
		}

		public void LoadScorePage(LeaderboardScoreData data, int maxResults, ScorePageToken token, Action<LeaderboardScoreData> callback)
		{
			LeaderboardScoreData leaderboardScoreDatum = data ?? new LeaderboardScoreData(token.LeaderboardId);
			NativeScorePageToken internalObject = (NativeScorePageToken)token.InternalObject;
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScorePage(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, internalObject.AsPointer(), (uint)maxResults, new GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScorePageCallback(GooglePlayGames.Native.PInvoke.LeaderboardManager.InternalFetchScorePage), Callbacks.ToIntPtr<FetchScorePageResponse>((FetchScorePageResponse rsp) => this.HandleFetchScorePage(leaderboardScoreDatum, token, rsp, callback), new Func<IntPtr, FetchScorePageResponse>(FetchScorePageResponse.FromPointer)));
		}

		internal void ShowAllUI(Action<CommonErrorStatus.UIStatus> callback)
		{
			Misc.CheckNotNull<Action<CommonErrorStatus.UIStatus>>(callback);
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ShowAllUI(this.mServices.AsHandle(), new GooglePlayGames.Native.Cwrapper.LeaderboardManager.ShowAllUICallback(Callbacks.InternalShowUICallback), Callbacks.ToIntPtr(callback));
		}

		internal void ShowUI(string leaderboardId, LeaderboardTimeSpan span, Action<CommonErrorStatus.UIStatus> callback)
		{
			Misc.CheckNotNull<Action<CommonErrorStatus.UIStatus>>(callback);
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_ShowUI(this.mServices.AsHandle(), leaderboardId, (Types.LeaderboardTimeSpan)span, new GooglePlayGames.Native.Cwrapper.LeaderboardManager.ShowUICallback(Callbacks.InternalShowUICallback), Callbacks.ToIntPtr(callback));
		}

		internal void SubmitScore(string leaderboardId, long score, string metadata)
		{
			Misc.CheckNotNull<string>(leaderboardId, "leaderboardId");
			Logger.d(string.Concat(new object[] { "Native Submitting score: ", score, " for lb ", leaderboardId, " with metadata: ", metadata }));
			HandleRef handleRef = this.mServices.AsHandle();
			string str = leaderboardId;
			long num = score;
			string str1 = metadata ?? string.Empty;
			GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_SubmitScore(handleRef, str, (ulong)num, str1);
		}
	}
}