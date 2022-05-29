using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GooglePlayGames.Native
{
	public class NativeRealtimeMultiplayerClient : IRealTimeMultiplayerClient
	{
		private readonly object mSessionLock = new object();

		private readonly NativeClient mNativeClient;

		private readonly RealtimeManager mRealtimeManager;

		private volatile NativeRealtimeMultiplayerClient.RoomSession mCurrentSession;

		internal NativeRealtimeMultiplayerClient(NativeClient nativeClient, RealtimeManager manager)
		{
			this.mNativeClient = Misc.CheckNotNull<NativeClient>(nativeClient);
			this.mRealtimeManager = Misc.CheckNotNull<RealtimeManager>(manager);
			this.mCurrentSession = this.GetTerminatedSession();
			PlayGamesHelperObject.AddPauseCallback(new Action<bool>(this.HandleAppPausing));
		}

		public void AcceptFromInbox(RealTimeMultiplayerListener listener)
		{
			object obj = this.mSessionLock;
			Monitor.Enter(obj);
			try
			{
				NativeRealtimeMultiplayerClient.RoomSession roomSession = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, listener);
				if (!this.mCurrentSession.IsActive())
				{
					this.mCurrentSession = roomSession;
					this.mCurrentSession.ShowingUI = true;
					this.mRealtimeManager.ShowRoomInboxUI((RealtimeManager.RoomInboxUIResponse response) => {
						this.mCurrentSession.ShowingUI = false;
						if (response.ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
						{
							Logger.d("User did not complete invitation screen.");
							roomSession.LeaveRoom();
							return;
						}
						GooglePlayGames.Native.PInvoke.MultiplayerInvitation multiplayerInvitation = response.Invitation();
						using (GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper realTimeEventListenerHelper = NativeRealtimeMultiplayerClient.HelperForSession(roomSession))
						{
							Logger.d(string.Concat("About to accept invitation ", multiplayerInvitation.Id()));
							roomSession.StartRoomCreation(this.mNativeClient.GetUserId(), () => this.mRealtimeManager.AcceptInvitation(multiplayerInvitation, realTimeEventListenerHelper, (RealtimeManager.RealTimeRoomResponse acceptResponse) => {
								using (multiplayerInvitation)
								{
									roomSession.HandleRoomResponse(acceptResponse);
									roomSession.SetInvitation(multiplayerInvitation.AsInvitation());
								}
							}));
						}
					});
				}
				else
				{
					Logger.e("Received attempt to accept invitation without cleaning up active session.");
					roomSession.LeaveRoom();
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}

		public void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener)
		{
			NativeRealtimeMultiplayerClient.u003cAcceptInvitationu003ec__AnonStorey23D variable;
			object obj = this.mSessionLock;
			Monitor.Enter(obj);
			try
			{
				NativeRealtimeMultiplayerClient.RoomSession roomSession = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, listener);
				if (!this.mCurrentSession.IsActive())
				{
					this.mCurrentSession = roomSession;
					this.mRealtimeManager.FetchInvitations((RealtimeManager.FetchInvitationsResponse response) => {
						if (!response.RequestSucceeded())
						{
							Logger.e("Couldn't load invitations.");
							roomSession.LeaveRoom();
							return;
						}
						IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = response.Invitations().GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								GooglePlayGames.Native.PInvoke.MultiplayerInvitation current = enumerator.Current;
								using (current)
								{
									if (current.Id().Equals(invitationId))
									{
										this.mCurrentSession.MinPlayersToStart = current.AutomatchingSlots() + current.ParticipantCount();
										Logger.d(string.Concat("Setting MinPlayersToStart with invitation to : ", this.mCurrentSession.MinPlayersToStart));
										using (GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper realTimeEventListenerHelper = NativeRealtimeMultiplayerClient.HelperForSession(roomSession))
										{
											roomSession.StartRoomCreation(this.mNativeClient.GetUserId(), () => this.mRealtimeManager.AcceptInvitation(current, realTimeEventListenerHelper, new Action<RealtimeManager.RealTimeRoomResponse>(roomSession.HandleRoomResponse)));
											return;
										}
									}
								}
							}
						}
						finally
						{
							if (enumerator == null)
							{
							}
							enumerator.Dispose();
						}
						Logger.e(string.Concat("Room creation failed since we could not find invitation with ID ", invitationId));
						roomSession.LeaveRoom();
					});
				}
				else
				{
					Logger.e("Received attempt to accept invitation without cleaning up active session.");
					roomSession.LeaveRoom();
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}

		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener)
		{
			this.CreateQuickGame(minOpponents, maxOpponents, variant, (ulong)0, listener);
		}

		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, RealTimeMultiplayerListener listener)
		{
			object obj = this.mSessionLock;
			Monitor.Enter(obj);
			try
			{
				NativeRealtimeMultiplayerClient.RoomSession roomSession = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, listener);
				if (!this.mCurrentSession.IsActive())
				{
					this.mCurrentSession = roomSession;
					Logger.d(string.Concat("QuickGame: Setting MinPlayersToStart = ", minOpponents));
					this.mCurrentSession.MinPlayersToStart = minOpponents;
					using (RealtimeRoomConfigBuilder realtimeRoomConfigBuilder = RealtimeRoomConfigBuilder.Create())
					{
						RealtimeRoomConfig realtimeRoomConfig = realtimeRoomConfigBuilder.SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetVariant(variant).SetExclusiveBitMask(exclusiveBitMask).Build();
						using (realtimeRoomConfig)
						{
							using (GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper realTimeEventListenerHelper = NativeRealtimeMultiplayerClient.HelperForSession(roomSession))
							{
								roomSession.StartRoomCreation(this.mNativeClient.GetUserId(), () => this.mRealtimeManager.CreateRoom(realtimeRoomConfig, realTimeEventListenerHelper, new Action<RealtimeManager.RealTimeRoomResponse>(roomSession.HandleRoomResponse)));
							}
						}
					}
				}
				else
				{
					Logger.e("Received attempt to create a new room without cleaning up the old one.");
					roomSession.LeaveRoom();
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, RealTimeMultiplayerListener listener)
		{
			object obj = this.mSessionLock;
			Monitor.Enter(obj);
			try
			{
				NativeRealtimeMultiplayerClient.RoomSession roomSession = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, listener);
				if (!this.mCurrentSession.IsActive())
				{
					this.mCurrentSession = roomSession;
					this.mCurrentSession.ShowingUI = true;
					this.mRealtimeManager.ShowPlayerSelectUI(minOpponents, maxOppponents, true, (PlayerSelectUIResponse response) => {
						this.mCurrentSession.ShowingUI = false;
						if (response.Status() != CommonErrorStatus.UIStatus.VALID)
						{
							Logger.d("User did not complete invitation screen.");
							roomSession.LeaveRoom();
							return;
						}
						this.mCurrentSession.MinPlayersToStart = response.MinimumAutomatchingPlayers() + response.Count<string>() + 1;
						using (RealtimeRoomConfigBuilder realtimeRoomConfigBuilder = RealtimeRoomConfigBuilder.Create())
						{
							realtimeRoomConfigBuilder.SetVariant(variant);
							realtimeRoomConfigBuilder.PopulateFromUIResponse(response);
							using (RealtimeRoomConfig realtimeRoomConfig = realtimeRoomConfigBuilder.Build())
							{
								using (GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper realTimeEventListenerHelper = NativeRealtimeMultiplayerClient.HelperForSession(roomSession))
								{
									roomSession.StartRoomCreation(this.mNativeClient.GetUserId(), () => this.mRealtimeManager.CreateRoom(realtimeRoomConfig, realTimeEventListenerHelper, new Action<RealtimeManager.RealTimeRoomResponse>(roomSession.HandleRoomResponse)));
								}
							}
						}
					});
				}
				else
				{
					Logger.e("Received attempt to create a new room without cleaning up the old one.");
					roomSession.LeaveRoom();
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}

		public void DeclineInvitation(string invitationId)
		{
			this.mRealtimeManager.FetchInvitations((RealtimeManager.FetchInvitationsResponse response) => {
				if (!response.RequestSucceeded())
				{
					Logger.e("Couldn't load invitations.");
					return;
				}
				IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = response.Invitations().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GooglePlayGames.Native.PInvoke.MultiplayerInvitation current = enumerator.Current;
						using (current)
						{
							if (current.Id().Equals(invitationId))
							{
								this.mRealtimeManager.DeclineInvitation(current);
							}
						}
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
			});
		}

		public void GetAllInvitations(Action<Invitation[]> callback)
		{
			this.mRealtimeManager.FetchInvitations((RealtimeManager.FetchInvitationsResponse response) => {
				if (!response.RequestSucceeded())
				{
					Logger.e("Couldn't load invitations.");
					callback(new Invitation[0]);
					return;
				}
				List<Invitation> invitations = new List<Invitation>();
				IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> enumerator = response.Invitations().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GooglePlayGames.Native.PInvoke.MultiplayerInvitation current = enumerator.Current;
						using (current)
						{
							invitations.Add(current.AsInvitation());
						}
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				callback(invitations.ToArray());
			});
		}

		public List<Participant> GetConnectedParticipants()
		{
			return this.mCurrentSession.GetConnectedParticipants();
		}

		public Invitation GetInvitation()
		{
			return this.mCurrentSession.GetInvitation();
		}

		public Participant GetParticipant(string participantId)
		{
			return this.mCurrentSession.GetParticipant(participantId);
		}

		public Participant GetSelf()
		{
			return this.mCurrentSession.GetSelf();
		}

		private NativeRealtimeMultiplayerClient.RoomSession GetTerminatedSession()
		{
			NativeRealtimeMultiplayerClient.RoomSession roomSession = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, new NativeRealtimeMultiplayerClient.NoopListener());
			roomSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(roomSession), false);
			return roomSession;
		}

		private void HandleAppPausing(bool paused)
		{
			if (paused)
			{
				Logger.d("Application is pausing, which disconnects the RTMP  client.  Leaving room.");
				this.LeaveRoom();
			}
		}

		private static GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper HelperForSession(NativeRealtimeMultiplayerClient.RoomSession session)
		{
			return GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.Create().SetOnDataReceivedCallback((NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant, byte[] data, bool isReliable) => session.OnDataReceived(room, participant, data, isReliable)).SetOnParticipantStatusChangedCallback((NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant) => session.OnParticipantStatusChanged(room, participant)).SetOnRoomConnectedSetChangedCallback((NativeRealTimeRoom room) => session.OnConnectedSetChanged(room)).SetOnRoomStatusChangedCallback((NativeRealTimeRoom room) => session.OnRoomStatusChanged(room));
		}

		public bool IsRoomConnected()
		{
			return this.mCurrentSession.IsRoomConnected();
		}

		public void LeaveRoom()
		{
			this.mCurrentSession.LeaveRoom();
		}

		public void SendMessage(bool reliable, string participantId, byte[] data)
		{
			this.mCurrentSession.SendMessage(reliable, participantId, data);
		}

		public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
		{
			this.mCurrentSession.SendMessage(reliable, participantId, data, offset, length);
		}

		public void SendMessageToAll(bool reliable, byte[] data)
		{
			this.mCurrentSession.SendMessageToAll(reliable, data);
		}

		public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
		{
			this.mCurrentSession.SendMessageToAll(reliable, data, offset, length);
		}

		public void ShowWaitingRoomUI()
		{
			object obj = this.mSessionLock;
			Monitor.Enter(obj);
			try
			{
				this.mCurrentSession.ShowWaitingRoomUI();
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}

		private static T WithDefault<T>(T presented, T defaultValue)
		where T : class
		{
			return (presented == null ? defaultValue : presented);
		}

		private class AbortingRoomCreationState : NativeRealtimeMultiplayerClient.State
		{
			private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

			internal AbortingRoomCreationState(NativeRealtimeMultiplayerClient.RoomSession session)
			{
				this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
			}

			internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				if (!response.RequestSucceeded())
				{
					this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
					this.mSession.OnGameThreadListener().RoomConnected(false);
					return;
				}
				this.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(this.mSession, response.Room(), () => this.mSession.OnGameThreadListener().RoomConnected(false)));
			}

			internal override bool IsActive()
			{
				return false;
			}
		}

		private class ActiveState : NativeRealtimeMultiplayerClient.MessagingEnabledState
		{
			internal ActiveState(NativeRealTimeRoom room, NativeRealtimeMultiplayerClient.RoomSession session) : base(session, room)
			{
			}

			internal override Participant GetParticipant(string participantId)
			{
				if (this.mParticipants.ContainsKey(participantId))
				{
					return this.mParticipants[participantId];
				}
				Logger.e(string.Concat("Attempted to retrieve unknown participant ", participantId));
				return null;
			}

			internal override Participant GetSelf()
			{
				Participant participant;
				Dictionary<string, Participant>.ValueCollection.Enumerator enumerator = this.mParticipants.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Participant current = enumerator.Current;
						if (current.Player == null || !current.Player.id.Equals(this.mSession.SelfPlayerId()))
						{
							continue;
						}
						participant = current;
						return participant;
					}
					return null;
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				return participant;
			}

			internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
				List<string> strs = new List<string>();
				List<string> list = new List<string>();
				Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> dictionary = room.Participants().ToDictionary<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, string>((GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) => p.Id());
				foreach (string key in this.mNativeParticipants.Keys)
				{
					GooglePlayGames.Native.PInvoke.MultiplayerParticipant item = dictionary[key];
					GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = this.mNativeParticipants[key];
					if (!item.IsConnectedToRoom())
					{
						list.Add(key);
					}
					if (multiplayerParticipant.IsConnectedToRoom() || !item.IsConnectedToRoom())
					{
						continue;
					}
					strs.Add(key);
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerParticipant value in this.mNativeParticipants.Values)
				{
					value.Dispose();
				}
				this.mNativeParticipants = dictionary;
				this.mParticipants = (
					from p in this.mNativeParticipants.Values
					select p.AsParticipant()).ToDictionary<Participant, string>((Participant p) => p.ParticipantId);
				Logger.d(string.Concat("Updated participant statuses: ", string.Join(",", (
					from p in this.mParticipants.Values
					select p.ToString()).ToArray<string>())));
				if (list.Contains(this.GetSelf().ParticipantId))
				{
					Logger.w("Player was disconnected from the multiplayer session.");
				}
				string participantId = this.GetSelf().ParticipantId;
				strs = (
					from peerId in strs
					where !peerId.Equals(participantId)
					select peerId).ToList<string>();
				list = (
					from peerId in list
					where !peerId.Equals(participantId)
					select peerId).ToList<string>();
				if (strs.Count > 0)
				{
					strs.Sort();
					this.mSession.OnGameThreadListener().PeersConnected((
						from peer in strs
						where !peer.Equals(participantId)
						select peer).ToArray<string>());
				}
				if (list.Count > 0)
				{
					list.Sort();
					this.mSession.OnGameThreadListener().PeersDisconnected((
						from peer in list
						where !peer.Equals(participantId)
						select peer).ToArray<string>());
				}
			}

			internal override bool IsRoomConnected()
			{
				return true;
			}

			internal override void LeaveRoom()
			{
				this.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(this.mSession, this.mRoom, () => this.mSession.OnGameThreadListener().LeftRoom()));
			}

			internal override void OnStateEntered()
			{
				if (this.GetSelf() == null)
				{
					Logger.e("Room reached active state with unknown participant for the player");
					this.LeaveRoom();
				}
			}
		}

		private class BeforeRoomCreateStartedState : NativeRealtimeMultiplayerClient.State
		{
			private readonly NativeRealtimeMultiplayerClient.RoomSession mContainingSession;

			internal BeforeRoomCreateStartedState(NativeRealtimeMultiplayerClient.RoomSession session)
			{
				this.mContainingSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
			}

			internal override void LeaveRoom()
			{
				Logger.d("Session was torn down before room was created.");
				this.mContainingSession.OnGameThreadListener().RoomConnected(false);
				this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mContainingSession));
			}
		}

		private class ConnectingState : NativeRealtimeMultiplayerClient.MessagingEnabledState
		{
			private const float InitialPercentComplete = 20f;

			private readonly static HashSet<Types.ParticipantStatus> FailedStatuses;

			private HashSet<string> mConnectedParticipants;

			private float mPercentComplete;

			private float mPercentPerParticipant;

			static ConnectingState()
			{
				HashSet<Types.ParticipantStatus> participantStatuses = new HashSet<Types.ParticipantStatus>();
				participantStatuses.Add(Types.ParticipantStatus.DECLINED);
				participantStatuses.Add(Types.ParticipantStatus.LEFT);
				NativeRealtimeMultiplayerClient.ConnectingState.FailedStatuses = participantStatuses;
			}

			internal ConnectingState(NativeRealTimeRoom room, NativeRealtimeMultiplayerClient.RoomSession session) : base(session, room)
			{
				this.mPercentPerParticipant = 80f / (float)((float)session.MinPlayersToStart);
			}

			internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
				HashSet<string> strs = new HashSet<string>();
				if ((room.Status() == Types.RealTimeRoomStatus.AUTO_MATCHING || room.Status() == Types.RealTimeRoomStatus.CONNECTING) && this.mSession.MinPlayersToStart <= room.ParticipantCount())
				{
					this.mSession.MinPlayersToStart = this.mSession.MinPlayersToStart + room.ParticipantCount();
					this.mPercentPerParticipant = 80f / (float)((float)this.mSession.MinPlayersToStart);
				}
				IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> enumerator = room.Participants().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GooglePlayGames.Native.PInvoke.MultiplayerParticipant current = enumerator.Current;
						using (current)
						{
							if (current.IsConnectedToRoom())
							{
								strs.Add(current.Id());
							}
						}
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				if (this.mConnectedParticipants.Equals(strs))
				{
					Logger.w("Received connected set callback with unchanged connected set!");
					return;
				}
				IEnumerable<string> strs1 = this.mConnectedParticipants.Except<string>(strs);
				if (room.Status() == Types.RealTimeRoomStatus.DELETED)
				{
					Logger.e(string.Concat("Participants disconnected during room setup, failing. Participants were: ", string.Join(",", strs1.ToArray<string>())));
					this.mSession.OnGameThreadListener().RoomConnected(false);
					this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
					return;
				}
				IEnumerable<string> strs2 = strs.Except<string>(this.mConnectedParticipants);
				Logger.d(string.Concat("New participants connected: ", string.Join(",", strs2.ToArray<string>())));
				if (room.Status() == Types.RealTimeRoomStatus.ACTIVE)
				{
					Logger.d("Fully connected! Transitioning to active state.");
					this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ActiveState(room, this.mSession));
					this.mSession.OnGameThreadListener().RoomConnected(true);
					return;
				}
				NativeRealtimeMultiplayerClient.ConnectingState connectingState = this;
				connectingState.mPercentComplete = connectingState.mPercentComplete + this.mPercentPerParticipant * (float)strs2.Count<string>();
				this.mConnectedParticipants = strs;
				this.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
			}

			internal override void HandleParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				if (!NativeRealtimeMultiplayerClient.ConnectingState.FailedStatuses.Contains(participant.Status()))
				{
					return;
				}
				this.mSession.OnGameThreadListener().ParticipantLeft(participant.AsParticipant());
				if (room.Status() != Types.RealTimeRoomStatus.CONNECTING && room.Status() != Types.RealTimeRoomStatus.AUTO_MATCHING)
				{
					this.LeaveRoom();
				}
			}

			internal override void LeaveRoom()
			{
				this.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(this.mSession, this.mRoom, () => this.mSession.OnGameThreadListener().RoomConnected(false)));
			}

			internal override void OnStateEntered()
			{
				this.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
			}

			internal override void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
			{
				this.mSession.ShowingUI = true;
				this.mSession.Manager().ShowWaitingRoomUI(this.mRoom, minimumParticipantsBeforeStarting, (RealtimeManager.WaitingRoomUIResponse response) => {
					this.mSession.ShowingUI = false;
					Logger.d(string.Concat("ShowWaitingRoomUI Response: ", response.ResponseStatus()));
					if (response.ResponseStatus() == CommonErrorStatus.UIStatus.VALID)
					{
						Logger.d(string.Concat(new object[] { "Connecting state ShowWaitingRoomUI: room pcount:", response.Room().ParticipantCount(), " status: ", response.Room().Status() }));
						if (response.Room().Status() == Types.RealTimeRoomStatus.ACTIVE)
						{
							this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ActiveState(response.Room(), this.mSession));
						}
					}
					else if (response.ResponseStatus() != CommonErrorStatus.UIStatus.ERROR_LEFT_ROOM)
					{
						this.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
					}
					else
					{
						this.LeaveRoom();
					}
				});
			}
		}

		private class LeavingRoom : NativeRealtimeMultiplayerClient.State
		{
			private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

			private readonly NativeRealTimeRoom mRoomToLeave;

			private readonly Action mLeavingCompleteCallback;

			internal LeavingRoom(NativeRealtimeMultiplayerClient.RoomSession session, NativeRealTimeRoom room, Action leavingCompleteCallback)
			{
				this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
				this.mRoomToLeave = Misc.CheckNotNull<NativeRealTimeRoom>(room);
				this.mLeavingCompleteCallback = Misc.CheckNotNull<Action>(leavingCompleteCallback);
			}

			internal override bool IsActive()
			{
				return false;
			}

			internal override void OnStateEntered()
			{
				this.mSession.Manager().LeaveRoom(this.mRoomToLeave, (CommonErrorStatus.ResponseStatus status) => {
					this.mLeavingCompleteCallback();
					this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
				});
			}
		}

		private abstract class MessagingEnabledState : NativeRealtimeMultiplayerClient.State
		{
			protected readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

			protected NativeRealTimeRoom mRoom;

			protected Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> mNativeParticipants;

			protected Dictionary<string, Participant> mParticipants;

			internal MessagingEnabledState(NativeRealtimeMultiplayerClient.RoomSession session, NativeRealTimeRoom room)
			{
				this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
				this.UpdateCurrentRoom(room);
			}

			internal sealed override List<Participant> GetConnectedParticipants()
			{
				List<Participant> list = (
					from p in this.mParticipants.Values
					where p.IsConnectedToRoom
					select p).ToList<Participant>();
				list.Sort();
				return list;
			}

			internal virtual void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
			}

			internal virtual void HandleParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
			}

			internal virtual void HandleRoomStatusChanged(NativeRealTimeRoom room)
			{
			}

			internal sealed override void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				this.HandleConnectedSetChanged(room);
				this.UpdateCurrentRoom(room);
			}

			internal override void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				this.mSession.OnGameThreadListener().RealTimeMessageReceived(isReliable, sender.Id(), data);
			}

			internal sealed override void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				this.HandleParticipantStatusChanged(room, participant);
				this.UpdateCurrentRoom(room);
			}

			internal sealed override void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				this.HandleRoomStatusChanged(room);
				this.UpdateCurrentRoom(room);
			}

			internal override void SendToAll(byte[] data, int offset, int length, bool isReliable)
			{
				byte[] subsetBytes = Misc.GetSubsetBytes(data, offset, length);
				if (!isReliable)
				{
					this.mSession.Manager().SendUnreliableMessageToAll(this.mRoom, subsetBytes);
				}
				else
				{
					foreach (string key in this.mNativeParticipants.Keys)
					{
						this.SendToSpecificRecipient(key, subsetBytes, 0, (int)subsetBytes.Length, true);
					}
				}
			}

			internal override void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
			{
				if (!this.mNativeParticipants.ContainsKey(recipientId))
				{
					Logger.e(string.Concat("Attempted to send message to unknown participant ", recipientId));
					return;
				}
				if (!isReliable)
				{
					RealtimeManager realtimeManager = this.mSession.Manager();
					NativeRealTimeRoom nativeRealTimeRoom = this.mRoom;
					List<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> multiplayerParticipants = new List<GooglePlayGames.Native.PInvoke.MultiplayerParticipant>()
					{
						this.mNativeParticipants[recipientId]
					};
					realtimeManager.SendUnreliableMessageToSpecificParticipants(nativeRealTimeRoom, multiplayerParticipants, Misc.GetSubsetBytes(data, offset, length));
				}
				else
				{
					this.mSession.Manager().SendReliableMessage(this.mRoom, this.mNativeParticipants[recipientId], Misc.GetSubsetBytes(data, offset, length), null);
				}
			}

			internal void UpdateCurrentRoom(NativeRealTimeRoom room)
			{
				if (this.mRoom != null)
				{
					this.mRoom.Dispose();
				}
				this.mRoom = Misc.CheckNotNull<NativeRealTimeRoom>(room);
				this.mNativeParticipants = this.mRoom.Participants().ToDictionary<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, string>((GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) => p.Id());
				this.mParticipants = (
					from p in this.mNativeParticipants.Values
					select p.AsParticipant()).ToDictionary<Participant, string>((Participant p) => p.ParticipantId);
			}
		}

		private class NoopListener : RealTimeMultiplayerListener
		{
			public NoopListener()
			{
			}

			public void OnLeftRoom()
			{
			}

			public void OnParticipantLeft(Participant participant)
			{
			}

			public void OnPeersConnected(string[] participantIds)
			{
			}

			public void OnPeersDisconnected(string[] participantIds)
			{
			}

			public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
			}

			public void OnRoomConnected(bool success)
			{
			}

			public void OnRoomSetupProgress(float percent)
			{
			}
		}

		private class OnGameThreadForwardingListener
		{
			private readonly RealTimeMultiplayerListener mListener;

			internal OnGameThreadForwardingListener(RealTimeMultiplayerListener listener)
			{
				this.mListener = Misc.CheckNotNull<RealTimeMultiplayerListener>(listener);
			}

			public void LeftRoom()
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnLeftRoom());
			}

			public void ParticipantLeft(Participant participant)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnParticipantLeft(participant));
			}

			public void PeersConnected(string[] participantIds)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnPeersConnected(participantIds));
			}

			public void PeersDisconnected(string[] participantIds)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnPeersDisconnected(participantIds));
			}

			public void RealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnRealTimeMessageReceived(isReliable, senderId, data));
			}

			public void RoomConnected(bool success)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnRoomConnected(success));
			}

			public void RoomSetupProgress(float percent)
			{
				PlayGamesHelperObject.RunOnGameThread(() => this.mListener.OnRoomSetupProgress(percent));
			}
		}

		private class RoomCreationPendingState : NativeRealtimeMultiplayerClient.State
		{
			private readonly NativeRealtimeMultiplayerClient.RoomSession mContainingSession;

			internal RoomCreationPendingState(NativeRealtimeMultiplayerClient.RoomSession session)
			{
				this.mContainingSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
			}

			internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				if (response.RequestSucceeded())
				{
					this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ConnectingState(response.Room(), this.mContainingSession));
					return;
				}
				this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mContainingSession));
				this.mContainingSession.OnGameThreadListener().RoomConnected(false);
			}

			internal override bool IsActive()
			{
				return true;
			}

			internal override void LeaveRoom()
			{
				Logger.d("Received request to leave room during room creation, aborting creation.");
				this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.AbortingRoomCreationState(this.mContainingSession));
			}
		}

		private class RoomSession
		{
			private readonly object mLifecycleLock;

			private readonly NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener mListener;

			private readonly RealtimeManager mManager;

			private volatile string mCurrentPlayerId;

			private volatile NativeRealtimeMultiplayerClient.State mState;

			private volatile bool mStillPreRoomCreation;

			private Invitation mInvitation;

			private volatile bool mShowingUI;

			private uint mMinPlayersToStart;

			internal uint MinPlayersToStart
			{
				get
				{
					return this.mMinPlayersToStart;
				}
				set
				{
					this.mMinPlayersToStart = value;
				}
			}

			internal bool ShowingUI
			{
				get
				{
					return this.mShowingUI;
				}
				set
				{
					this.mShowingUI = value;
				}
			}

			internal RoomSession(RealtimeManager manager, RealTimeMultiplayerListener listener)
			{
				this.mManager = Misc.CheckNotNull<RealtimeManager>(manager);
				this.mListener = new NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener(listener);
				this.EnterState(new NativeRealtimeMultiplayerClient.BeforeRoomCreateStartedState(this), false);
				this.mStillPreRoomCreation = true;
			}

			internal void EnterState(NativeRealtimeMultiplayerClient.State handler)
			{
				this.EnterState(handler, true);
			}

			internal void EnterState(NativeRealtimeMultiplayerClient.State handler, bool fireStateEnteredEvent)
			{
				object obj = this.mLifecycleLock;
				Monitor.Enter(obj);
				try
				{
					this.mState = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.State>(handler);
					if (fireStateEnteredEvent)
					{
						Logger.d(string.Concat("Entering state: ", handler.GetType().Name));
						this.mState.OnStateEntered();
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			internal List<Participant> GetConnectedParticipants()
			{
				return this.mState.GetConnectedParticipants();
			}

			public Invitation GetInvitation()
			{
				return this.mInvitation;
			}

			internal virtual Participant GetParticipant(string participantId)
			{
				return this.mState.GetParticipant(participantId);
			}

			internal virtual Participant GetSelf()
			{
				return this.mState.GetSelf();
			}

			internal void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				object obj = this.mLifecycleLock;
				Monitor.Enter(obj);
				try
				{
					this.mState.HandleRoomResponse(response);
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			internal bool IsActive()
			{
				return this.mState.IsActive();
			}

			internal virtual bool IsRoomConnected()
			{
				return this.mState.IsRoomConnected();
			}

			internal void LeaveRoom()
			{
				if (this.ShowingUI)
				{
					Logger.d("Not leaving room since showing UI");
				}
				else
				{
					object obj = this.mLifecycleLock;
					Monitor.Enter(obj);
					try
					{
						this.mState.LeaveRoom();
					}
					finally
					{
						Monitor.Exit(obj);
					}
				}
			}

			internal RealtimeManager Manager()
			{
				return this.mManager;
			}

			internal void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				object obj = this.mLifecycleLock;
				Monitor.Enter(obj);
				try
				{
					this.mState.OnConnectedSetChanged(room);
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			internal void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				this.mState.OnDataReceived(room, sender, data, isReliable);
			}

			internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener OnGameThreadListener()
			{
				return this.mListener;
			}

			internal void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				object obj = this.mLifecycleLock;
				Monitor.Enter(obj);
				try
				{
					this.mState.OnParticipantStatusChanged(room, participant);
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			internal void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				object obj = this.mLifecycleLock;
				Monitor.Enter(obj);
				try
				{
					this.mState.OnRoomStatusChanged(room);
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}

			internal string SelfPlayerId()
			{
				return this.mCurrentPlayerId;
			}

			internal void SendMessage(bool reliable, string participantId, byte[] data)
			{
				this.SendMessage(reliable, participantId, data, 0, (int)data.Length);
			}

			internal void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
			{
				this.mState.SendToSpecificRecipient(participantId, data, offset, length, reliable);
			}

			internal void SendMessageToAll(bool reliable, byte[] data)
			{
				this.SendMessageToAll(reliable, data, 0, (int)data.Length);
			}

			internal void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
			{
				this.mState.SendToAll(data, offset, length, reliable);
			}

			public void SetInvitation(Invitation invitation)
			{
				this.mInvitation = invitation;
			}

			internal void ShowWaitingRoomUI()
			{
				this.mState.ShowWaitingRoomUI(this.MinPlayersToStart);
			}

			internal void StartRoomCreation(string currentPlayerId, Action createRoom)
			{
				object obj = this.mLifecycleLock;
				Monitor.Enter(obj);
				try
				{
					if (!this.mStillPreRoomCreation)
					{
						Logger.e("Room creation started more than once, this shouldn't happen!");
					}
					else if (this.mState.IsActive())
					{
						this.mCurrentPlayerId = Misc.CheckNotNull<string>(currentPlayerId);
						this.mStillPreRoomCreation = false;
						this.EnterState(new NativeRealtimeMultiplayerClient.RoomCreationPendingState(this));
						createRoom();
					}
					else
					{
						Logger.w("Received an attempt to create a room after the session was already torn down!");
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}
		}

		private class ShutdownState : NativeRealtimeMultiplayerClient.State
		{
			private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

			internal ShutdownState(NativeRealtimeMultiplayerClient.RoomSession session)
			{
				this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
			}

			internal override bool IsActive()
			{
				return false;
			}

			internal override void LeaveRoom()
			{
				this.mSession.OnGameThreadListener().LeftRoom();
			}
		}

		internal abstract class State
		{
			protected State()
			{
			}

			internal virtual List<Participant> GetConnectedParticipants()
			{
				Logger.d(string.Concat(this.GetType().Name, ".GetConnectedParticipants: Returning empty connected participants"));
				return new List<Participant>();
			}

			internal virtual Participant GetParticipant(string participantId)
			{
				Logger.d(string.Concat(this.GetType().Name, ".GetSelf: Returning null participant."));
				return null;
			}

			internal virtual Participant GetSelf()
			{
				Logger.d(string.Concat(this.GetType().Name, ".GetSelf: Returning null self."));
				return null;
			}

			internal virtual void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				Logger.d(string.Concat(this.GetType().Name, ".HandleRoomResponse: Defaulting to no-op."));
			}

			internal virtual bool IsActive()
			{
				Logger.d(string.Concat(this.GetType().Name, ".IsNonPreemptable: Is preemptable by default."));
				return true;
			}

			internal virtual bool IsRoomConnected()
			{
				Logger.d(string.Concat(this.GetType().Name, ".IsRoomConnected: Returning room not connected."));
				return false;
			}

			internal virtual void LeaveRoom()
			{
				Logger.d(string.Concat(this.GetType().Name, ".LeaveRoom: Defaulting to no-op."));
			}

			internal virtual void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				Logger.d(string.Concat(this.GetType().Name, ".OnConnectedSetChanged: Defaulting to no-op."));
			}

			internal virtual void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				Logger.d(string.Concat(this.GetType().Name, ".OnDataReceived: Defaulting to no-op."));
			}

			internal virtual void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				Logger.d(string.Concat(this.GetType().Name, ".OnParticipantStatusChanged: Defaulting to no-op."));
			}

			internal virtual void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				Logger.d(string.Concat(this.GetType().Name, ".OnRoomStatusChanged: Defaulting to no-op."));
			}

			internal virtual void OnStateEntered()
			{
				Logger.d(string.Concat(this.GetType().Name, ".OnStateEntered: Defaulting to no-op."));
			}

			internal virtual void SendToAll(byte[] data, int offset, int length, bool isReliable)
			{
				Logger.d(string.Concat(this.GetType().Name, ".SendToApp: Defaulting to no-op."));
			}

			internal virtual void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
			{
				Logger.d(string.Concat(this.GetType().Name, ".SendToSpecificRecipient: Defaulting to no-op."));
			}

			internal virtual void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
			{
				Logger.d(string.Concat(this.GetType().Name, ".ShowWaitingRoomUI: Defaulting to no-op."));
			}
		}
	}
}