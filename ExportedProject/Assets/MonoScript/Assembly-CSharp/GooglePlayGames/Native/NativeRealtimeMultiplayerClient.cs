using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	public class NativeRealtimeMultiplayerClient : IRealTimeMultiplayerClient
	{
		private class NoopListener : RealTimeMultiplayerListener
		{
			public void OnRoomSetupProgress(float percent)
			{
			}

			public void OnRoomConnected(bool success)
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
		}

		private class RoomSession
		{
			private readonly object mLifecycleLock = new object();

			private readonly OnGameThreadForwardingListener mListener;

			private readonly RealtimeManager mManager;

			private volatile string mCurrentPlayerId;

			private volatile State mState;

			private volatile bool mStillPreRoomCreation;

			private Invitation mInvitation;

			private volatile bool mShowingUI;

			private uint mMinPlayersToStart;

			internal bool ShowingUI
			{
				get
				{
					return mShowingUI;
				}
				set
				{
					mShowingUI = value;
				}
			}

			internal uint MinPlayersToStart
			{
				get
				{
					return mMinPlayersToStart;
				}
				set
				{
					mMinPlayersToStart = value;
				}
			}

			internal RoomSession(RealtimeManager manager, RealTimeMultiplayerListener listener)
			{
				mManager = Misc.CheckNotNull(manager);
				mListener = new OnGameThreadForwardingListener(listener);
				EnterState(new BeforeRoomCreateStartedState(this), false);
				mStillPreRoomCreation = true;
			}

			internal RealtimeManager Manager()
			{
				return mManager;
			}

			internal bool IsActive()
			{
				return mState.IsActive();
			}

			internal string SelfPlayerId()
			{
				return mCurrentPlayerId;
			}

			public void SetInvitation(Invitation invitation)
			{
				mInvitation = invitation;
			}

			public Invitation GetInvitation()
			{
				return mInvitation;
			}

			internal OnGameThreadForwardingListener OnGameThreadListener()
			{
				return mListener;
			}

			internal void EnterState(State handler)
			{
				EnterState(handler, true);
			}

			internal void EnterState(State handler, bool fireStateEnteredEvent)
			{
				lock (mLifecycleLock)
				{
					mState = Misc.CheckNotNull(handler);
					if (fireStateEnteredEvent)
					{
						Logger.d("Entering state: " + handler.GetType().Name);
						mState.OnStateEntered();
					}
				}
			}

			internal void LeaveRoom()
			{
				if (!ShowingUI)
				{
					lock (mLifecycleLock)
					{
						mState.LeaveRoom();
						return;
					}
				}
				Logger.d("Not leaving room since showing UI");
			}

			internal void ShowWaitingRoomUI()
			{
				mState.ShowWaitingRoomUI(MinPlayersToStart);
			}

			internal void StartRoomCreation(string currentPlayerId, Action createRoom)
			{
				lock (mLifecycleLock)
				{
					if (!mStillPreRoomCreation)
					{
						Logger.e("Room creation started more than once, this shouldn't happen!");
						return;
					}
					if (!mState.IsActive())
					{
						Logger.w("Received an attempt to create a room after the session was already torn down!");
						return;
					}
					mCurrentPlayerId = Misc.CheckNotNull(currentPlayerId);
					mStillPreRoomCreation = false;
					EnterState(new RoomCreationPendingState(this));
					createRoom();
				}
			}

			internal void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				lock (mLifecycleLock)
				{
					mState.OnRoomStatusChanged(room);
				}
			}

			internal void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				lock (mLifecycleLock)
				{
					mState.OnConnectedSetChanged(room);
				}
			}

			internal void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				lock (mLifecycleLock)
				{
					mState.OnParticipantStatusChanged(room, participant);
				}
			}

			internal void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				lock (mLifecycleLock)
				{
					mState.HandleRoomResponse(response);
				}
			}

			internal void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				mState.OnDataReceived(room, sender, data, isReliable);
			}

			internal void SendMessageToAll(bool reliable, byte[] data)
			{
				SendMessageToAll(reliable, data, 0, data.Length);
			}

			internal void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
			{
				mState.SendToAll(data, offset, length, reliable);
			}

			internal void SendMessage(bool reliable, string participantId, byte[] data)
			{
				SendMessage(reliable, participantId, data, 0, data.Length);
			}

			internal void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
			{
				mState.SendToSpecificRecipient(participantId, data, offset, length, reliable);
			}

			internal List<Participant> GetConnectedParticipants()
			{
				return mState.GetConnectedParticipants();
			}

			internal virtual Participant GetSelf()
			{
				return mState.GetSelf();
			}

			internal virtual Participant GetParticipant(string participantId)
			{
				return mState.GetParticipant(participantId);
			}

			internal virtual bool IsRoomConnected()
			{
				return mState.IsRoomConnected();
			}
		}

		private class OnGameThreadForwardingListener
		{
			[CompilerGenerated]
			private sealed class _003CRoomSetupProgress_003Ec__AnonStorey241
			{
				internal float percent;

				internal OnGameThreadForwardingListener _003C_003Ef__this;

				internal void _003C_003Em__D6()
				{
					_003C_003Ef__this.mListener.OnRoomSetupProgress(percent);
				}
			}

			[CompilerGenerated]
			private sealed class _003CRoomConnected_003Ec__AnonStorey242
			{
				internal bool success;

				internal OnGameThreadForwardingListener _003C_003Ef__this;

				internal void _003C_003Em__D7()
				{
					_003C_003Ef__this.mListener.OnRoomConnected(success);
				}
			}

			[CompilerGenerated]
			private sealed class _003CPeersConnected_003Ec__AnonStorey243
			{
				internal string[] participantIds;

				internal OnGameThreadForwardingListener _003C_003Ef__this;

				internal void _003C_003Em__D9()
				{
					_003C_003Ef__this.mListener.OnPeersConnected(participantIds);
				}
			}

			[CompilerGenerated]
			private sealed class _003CPeersDisconnected_003Ec__AnonStorey244
			{
				internal string[] participantIds;

				internal OnGameThreadForwardingListener _003C_003Ef__this;

				internal void _003C_003Em__DA()
				{
					_003C_003Ef__this.mListener.OnPeersDisconnected(participantIds);
				}
			}

			[CompilerGenerated]
			private sealed class _003CRealTimeMessageReceived_003Ec__AnonStorey245
			{
				internal bool isReliable;

				internal string senderId;

				internal byte[] data;

				internal OnGameThreadForwardingListener _003C_003Ef__this;

				internal void _003C_003Em__DB()
				{
					_003C_003Ef__this.mListener.OnRealTimeMessageReceived(isReliable, senderId, data);
				}
			}

			[CompilerGenerated]
			private sealed class _003CParticipantLeft_003Ec__AnonStorey246
			{
				internal Participant participant;

				internal OnGameThreadForwardingListener _003C_003Ef__this;

				internal void _003C_003Em__DC()
				{
					_003C_003Ef__this.mListener.OnParticipantLeft(participant);
				}
			}

			private readonly RealTimeMultiplayerListener mListener;

			internal OnGameThreadForwardingListener(RealTimeMultiplayerListener listener)
			{
				mListener = Misc.CheckNotNull(listener);
			}

			public void RoomSetupProgress(float percent)
			{
				_003CRoomSetupProgress_003Ec__AnonStorey241 _003CRoomSetupProgress_003Ec__AnonStorey = new _003CRoomSetupProgress_003Ec__AnonStorey241();
				_003CRoomSetupProgress_003Ec__AnonStorey.percent = percent;
				_003CRoomSetupProgress_003Ec__AnonStorey._003C_003Ef__this = this;
				PlayGamesHelperObject.RunOnGameThread(_003CRoomSetupProgress_003Ec__AnonStorey._003C_003Em__D6);
			}

			public void RoomConnected(bool success)
			{
				_003CRoomConnected_003Ec__AnonStorey242 _003CRoomConnected_003Ec__AnonStorey = new _003CRoomConnected_003Ec__AnonStorey242();
				_003CRoomConnected_003Ec__AnonStorey.success = success;
				_003CRoomConnected_003Ec__AnonStorey._003C_003Ef__this = this;
				PlayGamesHelperObject.RunOnGameThread(_003CRoomConnected_003Ec__AnonStorey._003C_003Em__D7);
			}

			public void LeftRoom()
			{
				PlayGamesHelperObject.RunOnGameThread(_003CLeftRoom_003Em__D8);
			}

			public void PeersConnected(string[] participantIds)
			{
				_003CPeersConnected_003Ec__AnonStorey243 _003CPeersConnected_003Ec__AnonStorey = new _003CPeersConnected_003Ec__AnonStorey243();
				_003CPeersConnected_003Ec__AnonStorey.participantIds = participantIds;
				_003CPeersConnected_003Ec__AnonStorey._003C_003Ef__this = this;
				PlayGamesHelperObject.RunOnGameThread(_003CPeersConnected_003Ec__AnonStorey._003C_003Em__D9);
			}

			public void PeersDisconnected(string[] participantIds)
			{
				_003CPeersDisconnected_003Ec__AnonStorey244 _003CPeersDisconnected_003Ec__AnonStorey = new _003CPeersDisconnected_003Ec__AnonStorey244();
				_003CPeersDisconnected_003Ec__AnonStorey.participantIds = participantIds;
				_003CPeersDisconnected_003Ec__AnonStorey._003C_003Ef__this = this;
				PlayGamesHelperObject.RunOnGameThread(_003CPeersDisconnected_003Ec__AnonStorey._003C_003Em__DA);
			}

			public void RealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
				_003CRealTimeMessageReceived_003Ec__AnonStorey245 _003CRealTimeMessageReceived_003Ec__AnonStorey = new _003CRealTimeMessageReceived_003Ec__AnonStorey245();
				_003CRealTimeMessageReceived_003Ec__AnonStorey.isReliable = isReliable;
				_003CRealTimeMessageReceived_003Ec__AnonStorey.senderId = senderId;
				_003CRealTimeMessageReceived_003Ec__AnonStorey.data = data;
				_003CRealTimeMessageReceived_003Ec__AnonStorey._003C_003Ef__this = this;
				PlayGamesHelperObject.RunOnGameThread(_003CRealTimeMessageReceived_003Ec__AnonStorey._003C_003Em__DB);
			}

			public void ParticipantLeft(Participant participant)
			{
				_003CParticipantLeft_003Ec__AnonStorey246 _003CParticipantLeft_003Ec__AnonStorey = new _003CParticipantLeft_003Ec__AnonStorey246();
				_003CParticipantLeft_003Ec__AnonStorey.participant = participant;
				_003CParticipantLeft_003Ec__AnonStorey._003C_003Ef__this = this;
				PlayGamesHelperObject.RunOnGameThread(_003CParticipantLeft_003Ec__AnonStorey._003C_003Em__DC);
			}

			[CompilerGenerated]
			private void _003CLeftRoom_003Em__D8()
			{
				mListener.OnLeftRoom();
			}
		}

		internal abstract class State
		{
			internal virtual void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				Logger.d(GetType().Name + ".HandleRoomResponse: Defaulting to no-op.");
			}

			internal virtual bool IsActive()
			{
				Logger.d(GetType().Name + ".IsNonPreemptable: Is preemptable by default.");
				return true;
			}

			internal virtual void LeaveRoom()
			{
				Logger.d(GetType().Name + ".LeaveRoom: Defaulting to no-op.");
			}

			internal virtual void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
			{
				Logger.d(GetType().Name + ".ShowWaitingRoomUI: Defaulting to no-op.");
			}

			internal virtual void OnStateEntered()
			{
				Logger.d(GetType().Name + ".OnStateEntered: Defaulting to no-op.");
			}

			internal virtual void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				Logger.d(GetType().Name + ".OnRoomStatusChanged: Defaulting to no-op.");
			}

			internal virtual void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				Logger.d(GetType().Name + ".OnConnectedSetChanged: Defaulting to no-op.");
			}

			internal virtual void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				Logger.d(GetType().Name + ".OnParticipantStatusChanged: Defaulting to no-op.");
			}

			internal virtual void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				Logger.d(GetType().Name + ".OnDataReceived: Defaulting to no-op.");
			}

			internal virtual void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
			{
				Logger.d(GetType().Name + ".SendToSpecificRecipient: Defaulting to no-op.");
			}

			internal virtual void SendToAll(byte[] data, int offset, int length, bool isReliable)
			{
				Logger.d(GetType().Name + ".SendToApp: Defaulting to no-op.");
			}

			internal virtual List<Participant> GetConnectedParticipants()
			{
				Logger.d(GetType().Name + ".GetConnectedParticipants: Returning empty connected participants");
				return new List<Participant>();
			}

			internal virtual Participant GetSelf()
			{
				Logger.d(GetType().Name + ".GetSelf: Returning null self.");
				return null;
			}

			internal virtual Participant GetParticipant(string participantId)
			{
				Logger.d(GetType().Name + ".GetSelf: Returning null participant.");
				return null;
			}

			internal virtual bool IsRoomConnected()
			{
				Logger.d(GetType().Name + ".IsRoomConnected: Returning room not connected.");
				return false;
			}
		}

		private abstract class MessagingEnabledState : State
		{
			protected readonly RoomSession mSession;

			protected NativeRealTimeRoom mRoom;

			protected Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> mNativeParticipants;

			protected Dictionary<string, Participant> mParticipants;

			[CompilerGenerated]
			private static Func<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, string> _003C_003Ef__am_0024cache4;

			[CompilerGenerated]
			private static Func<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, Participant> _003C_003Ef__am_0024cache5;

			[CompilerGenerated]
			private static Func<Participant, string> _003C_003Ef__am_0024cache6;

			[CompilerGenerated]
			private static Func<Participant, bool> _003C_003Ef__am_0024cache7;

			internal MessagingEnabledState(RoomSession session, NativeRealTimeRoom room)
			{
				mSession = Misc.CheckNotNull(session);
				UpdateCurrentRoom(room);
			}

			internal void UpdateCurrentRoom(NativeRealTimeRoom room)
			{
				if (mRoom != null)
				{
					mRoom.Dispose();
				}
				mRoom = Misc.CheckNotNull(room);
				IEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> source = mRoom.Participants();
				if (_003C_003Ef__am_0024cache4 == null)
				{
					_003C_003Ef__am_0024cache4 = _003CUpdateCurrentRoom_003Em__DD;
				}
				mNativeParticipants = source.ToDictionary(_003C_003Ef__am_0024cache4);
				Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant>.ValueCollection values = mNativeParticipants.Values;
				if (_003C_003Ef__am_0024cache5 == null)
				{
					_003C_003Ef__am_0024cache5 = _003CUpdateCurrentRoom_003Em__DE;
				}
				IEnumerable<Participant> source2 = values.Select(_003C_003Ef__am_0024cache5);
				if (_003C_003Ef__am_0024cache6 == null)
				{
					_003C_003Ef__am_0024cache6 = _003CUpdateCurrentRoom_003Em__DF;
				}
				mParticipants = source2.ToDictionary(_003C_003Ef__am_0024cache6);
			}

			internal sealed override void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				HandleRoomStatusChanged(room);
				UpdateCurrentRoom(room);
			}

			internal virtual void HandleRoomStatusChanged(NativeRealTimeRoom room)
			{
			}

			internal sealed override void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				HandleConnectedSetChanged(room);
				UpdateCurrentRoom(room);
			}

			internal virtual void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
			}

			internal sealed override void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				HandleParticipantStatusChanged(room, participant);
				UpdateCurrentRoom(room);
			}

			internal virtual void HandleParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
			}

			internal sealed override List<Participant> GetConnectedParticipants()
			{
				Dictionary<string, Participant>.ValueCollection values = mParticipants.Values;
				if (_003C_003Ef__am_0024cache7 == null)
				{
					_003C_003Ef__am_0024cache7 = _003CGetConnectedParticipants_003Em__E0;
				}
				List<Participant> list = values.Where(_003C_003Ef__am_0024cache7).ToList();
				list.Sort();
				return list;
			}

			internal override void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
			{
				if (!mNativeParticipants.ContainsKey(recipientId))
				{
					Logger.e("Attempted to send message to unknown participant " + recipientId);
					return;
				}
				if (isReliable)
				{
					mSession.Manager().SendReliableMessage(mRoom, mNativeParticipants[recipientId], Misc.GetSubsetBytes(data, offset, length), null);
					return;
				}
				mSession.Manager().SendUnreliableMessageToSpecificParticipants(mRoom, new List<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> { mNativeParticipants[recipientId] }, Misc.GetSubsetBytes(data, offset, length));
			}

			internal override void SendToAll(byte[] data, int offset, int length, bool isReliable)
			{
				byte[] subsetBytes = Misc.GetSubsetBytes(data, offset, length);
				if (isReliable)
				{
					foreach (string key in mNativeParticipants.Keys)
					{
						SendToSpecificRecipient(key, subsetBytes, 0, subsetBytes.Length, true);
					}
					return;
				}
				mSession.Manager().SendUnreliableMessageToAll(mRoom, subsetBytes);
			}

			internal override void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				mSession.OnGameThreadListener().RealTimeMessageReceived(isReliable, sender.Id(), data);
			}

			[CompilerGenerated]
			private static string _003CUpdateCurrentRoom_003Em__DD(GooglePlayGames.Native.PInvoke.MultiplayerParticipant p)
			{
				return p.Id();
			}

			[CompilerGenerated]
			private static Participant _003CUpdateCurrentRoom_003Em__DE(GooglePlayGames.Native.PInvoke.MultiplayerParticipant p)
			{
				return p.AsParticipant();
			}

			[CompilerGenerated]
			private static string _003CUpdateCurrentRoom_003Em__DF(Participant p)
			{
				return p.ParticipantId;
			}

			[CompilerGenerated]
			private static bool _003CGetConnectedParticipants_003Em__E0(Participant p)
			{
				return p.IsConnectedToRoom;
			}
		}

		private class BeforeRoomCreateStartedState : State
		{
			private readonly RoomSession mContainingSession;

			internal BeforeRoomCreateStartedState(RoomSession session)
			{
				mContainingSession = Misc.CheckNotNull(session);
			}

			internal override void LeaveRoom()
			{
				Logger.d("Session was torn down before room was created.");
				mContainingSession.OnGameThreadListener().RoomConnected(false);
				mContainingSession.EnterState(new ShutdownState(mContainingSession));
			}
		}

		private class RoomCreationPendingState : State
		{
			private readonly RoomSession mContainingSession;

			internal RoomCreationPendingState(RoomSession session)
			{
				mContainingSession = Misc.CheckNotNull(session);
			}

			internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				if (!response.RequestSucceeded())
				{
					mContainingSession.EnterState(new ShutdownState(mContainingSession));
					mContainingSession.OnGameThreadListener().RoomConnected(false);
				}
				else
				{
					mContainingSession.EnterState(new ConnectingState(response.Room(), mContainingSession));
				}
			}

			internal override bool IsActive()
			{
				return true;
			}

			internal override void LeaveRoom()
			{
				Logger.d("Received request to leave room during room creation, aborting creation.");
				mContainingSession.EnterState(new AbortingRoomCreationState(mContainingSession));
			}
		}

		private class ConnectingState : MessagingEnabledState
		{
			private const float InitialPercentComplete = 20f;

			private static readonly HashSet<Types.ParticipantStatus> FailedStatuses = new HashSet<Types.ParticipantStatus>
			{
				Types.ParticipantStatus.DECLINED,
				Types.ParticipantStatus.LEFT
			};

			private HashSet<string> mConnectedParticipants = new HashSet<string>();

			private float mPercentComplete = 20f;

			private float mPercentPerParticipant;

			internal ConnectingState(NativeRealTimeRoom room, RoomSession session)
				: base(session, room)
			{
				mPercentPerParticipant = 80f / (float)session.MinPlayersToStart;
			}

			internal override void OnStateEntered()
			{
				mSession.OnGameThreadListener().RoomSetupProgress(mPercentComplete);
			}

			internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
				HashSet<string> hashSet = new HashSet<string>();
				if ((room.Status() == Types.RealTimeRoomStatus.AUTO_MATCHING || room.Status() == Types.RealTimeRoomStatus.CONNECTING) && mSession.MinPlayersToStart <= room.ParticipantCount())
				{
					mSession.MinPlayersToStart += room.ParticipantCount();
					mPercentPerParticipant = 80f / (float)mSession.MinPlayersToStart;
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerParticipant item in room.Participants())
				{
					using (item)
					{
						if (item.IsConnectedToRoom())
						{
							hashSet.Add(item.Id());
						}
					}
				}
				if (mConnectedParticipants.Equals(hashSet))
				{
					Logger.w("Received connected set callback with unchanged connected set!");
					return;
				}
				IEnumerable<string> source = mConnectedParticipants.Except(hashSet);
				if (room.Status() == Types.RealTimeRoomStatus.DELETED)
				{
					Logger.e("Participants disconnected during room setup, failing. Participants were: " + string.Join(",", source.ToArray()));
					mSession.OnGameThreadListener().RoomConnected(false);
					mSession.EnterState(new ShutdownState(mSession));
					return;
				}
				IEnumerable<string> source2 = hashSet.Except(mConnectedParticipants);
				Logger.d("New participants connected: " + string.Join(",", source2.ToArray()));
				if (room.Status() == Types.RealTimeRoomStatus.ACTIVE)
				{
					Logger.d("Fully connected! Transitioning to active state.");
					mSession.EnterState(new ActiveState(room, mSession));
					mSession.OnGameThreadListener().RoomConnected(true);
				}
				else
				{
					mPercentComplete += mPercentPerParticipant * (float)source2.Count();
					mConnectedParticipants = hashSet;
					mSession.OnGameThreadListener().RoomSetupProgress(mPercentComplete);
				}
			}

			internal override void HandleParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				if (FailedStatuses.Contains(participant.Status()))
				{
					mSession.OnGameThreadListener().ParticipantLeft(participant.AsParticipant());
					if (room.Status() != Types.RealTimeRoomStatus.CONNECTING && room.Status() != Types.RealTimeRoomStatus.AUTO_MATCHING)
					{
						LeaveRoom();
					}
				}
			}

			internal override void LeaveRoom()
			{
				mSession.EnterState(new LeavingRoom(mSession, mRoom, _003CLeaveRoom_003Em__E1));
			}

			internal override void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
			{
				mSession.ShowingUI = true;
				mSession.Manager().ShowWaitingRoomUI(mRoom, minimumParticipantsBeforeStarting, _003CShowWaitingRoomUI_003Em__E2);
			}

			[CompilerGenerated]
			private void _003CLeaveRoom_003Em__E1()
			{
				mSession.OnGameThreadListener().RoomConnected(false);
			}

			[CompilerGenerated]
			private void _003CShowWaitingRoomUI_003Em__E2(RealtimeManager.WaitingRoomUIResponse response)
			{
				mSession.ShowingUI = false;
				Logger.d("ShowWaitingRoomUI Response: " + response.ResponseStatus());
				if (response.ResponseStatus() == CommonErrorStatus.UIStatus.VALID)
				{
					Logger.d("Connecting state ShowWaitingRoomUI: room pcount:" + response.Room().ParticipantCount() + " status: " + response.Room().Status());
					if (response.Room().Status() == Types.RealTimeRoomStatus.ACTIVE)
					{
						mSession.EnterState(new ActiveState(response.Room(), mSession));
					}
				}
				else if (response.ResponseStatus() == CommonErrorStatus.UIStatus.ERROR_LEFT_ROOM)
				{
					LeaveRoom();
				}
				else
				{
					mSession.OnGameThreadListener().RoomSetupProgress(mPercentComplete);
				}
			}
		}

		private class ActiveState : MessagingEnabledState
		{
			[CompilerGenerated]
			private sealed class _003CHandleConnectedSetChanged_003Ec__AnonStorey247
			{
				internal string selfId;

				internal bool _003C_003Em__E7(string peerId)
				{
					return !peerId.Equals(selfId);
				}

				internal bool _003C_003Em__E8(string peerId)
				{
					return !peerId.Equals(selfId);
				}

				internal bool _003C_003Em__E9(string peer)
				{
					return !peer.Equals(selfId);
				}

				internal bool _003C_003Em__EA(string peer)
				{
					return !peer.Equals(selfId);
				}
			}

			[CompilerGenerated]
			private static Func<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, string> _003C_003Ef__am_0024cache0;

			[CompilerGenerated]
			private static Func<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, Participant> _003C_003Ef__am_0024cache1;

			[CompilerGenerated]
			private static Func<Participant, string> _003C_003Ef__am_0024cache2;

			[CompilerGenerated]
			private static Func<Participant, string> _003C_003Ef__am_0024cache3;

			internal ActiveState(NativeRealTimeRoom room, RoomSession session)
				: base(session, room)
			{
			}

			internal override void OnStateEntered()
			{
				if (GetSelf() == null)
				{
					Logger.e("Room reached active state with unknown participant for the player");
					LeaveRoom();
				}
			}

			internal override bool IsRoomConnected()
			{
				return true;
			}

			internal override Participant GetParticipant(string participantId)
			{
				if (!mParticipants.ContainsKey(participantId))
				{
					Logger.e("Attempted to retrieve unknown participant " + participantId);
					return null;
				}
				return mParticipants[participantId];
			}

			internal override Participant GetSelf()
			{
				foreach (Participant value in mParticipants.Values)
				{
					if (value.Player != null && value.Player.id.Equals(mSession.SelfPlayerId()))
					{
						return value;
					}
				}
				return null;
			}

			internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
				_003CHandleConnectedSetChanged_003Ec__AnonStorey247 _003CHandleConnectedSetChanged_003Ec__AnonStorey = new _003CHandleConnectedSetChanged_003Ec__AnonStorey247();
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				IEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> source = room.Participants();
				if (_003C_003Ef__am_0024cache0 == null)
				{
					_003C_003Ef__am_0024cache0 = _003CHandleConnectedSetChanged_003Em__E3;
				}
				Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> dictionary = source.ToDictionary(_003C_003Ef__am_0024cache0);
				foreach (string key in mNativeParticipants.Keys)
				{
					GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = dictionary[key];
					GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant2 = mNativeParticipants[key];
					if (!multiplayerParticipant.IsConnectedToRoom())
					{
						list2.Add(key);
					}
					if (!multiplayerParticipant2.IsConnectedToRoom() && multiplayerParticipant.IsConnectedToRoom())
					{
						list.Add(key);
					}
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerParticipant value in mNativeParticipants.Values)
				{
					value.Dispose();
				}
				mNativeParticipants = dictionary;
				Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant>.ValueCollection values = mNativeParticipants.Values;
				if (_003C_003Ef__am_0024cache1 == null)
				{
					_003C_003Ef__am_0024cache1 = _003CHandleConnectedSetChanged_003Em__E4;
				}
				IEnumerable<Participant> source2 = values.Select(_003C_003Ef__am_0024cache1);
				if (_003C_003Ef__am_0024cache2 == null)
				{
					_003C_003Ef__am_0024cache2 = _003CHandleConnectedSetChanged_003Em__E5;
				}
				mParticipants = source2.ToDictionary(_003C_003Ef__am_0024cache2);
				Dictionary<string, Participant>.ValueCollection values2 = mParticipants.Values;
				if (_003C_003Ef__am_0024cache3 == null)
				{
					_003C_003Ef__am_0024cache3 = _003CHandleConnectedSetChanged_003Em__E6;
				}
				Logger.d("Updated participant statuses: " + string.Join(",", values2.Select(_003C_003Ef__am_0024cache3).ToArray()));
				if (list2.Contains(GetSelf().ParticipantId))
				{
					Logger.w("Player was disconnected from the multiplayer session.");
				}
				_003CHandleConnectedSetChanged_003Ec__AnonStorey.selfId = GetSelf().ParticipantId;
				list = list.Where(_003CHandleConnectedSetChanged_003Ec__AnonStorey._003C_003Em__E7).ToList();
				list2 = list2.Where(_003CHandleConnectedSetChanged_003Ec__AnonStorey._003C_003Em__E8).ToList();
				if (list.Count > 0)
				{
					list.Sort();
					mSession.OnGameThreadListener().PeersConnected(list.Where(_003CHandleConnectedSetChanged_003Ec__AnonStorey._003C_003Em__E9).ToArray());
				}
				if (list2.Count > 0)
				{
					list2.Sort();
					mSession.OnGameThreadListener().PeersDisconnected(list2.Where(_003CHandleConnectedSetChanged_003Ec__AnonStorey._003C_003Em__EA).ToArray());
				}
			}

			internal override void LeaveRoom()
			{
				mSession.EnterState(new LeavingRoom(mSession, mRoom, _003CLeaveRoom_003Em__EB));
			}

			[CompilerGenerated]
			private static string _003CHandleConnectedSetChanged_003Em__E3(GooglePlayGames.Native.PInvoke.MultiplayerParticipant p)
			{
				return p.Id();
			}

			[CompilerGenerated]
			private static Participant _003CHandleConnectedSetChanged_003Em__E4(GooglePlayGames.Native.PInvoke.MultiplayerParticipant p)
			{
				return p.AsParticipant();
			}

			[CompilerGenerated]
			private static string _003CHandleConnectedSetChanged_003Em__E5(Participant p)
			{
				return p.ParticipantId;
			}

			[CompilerGenerated]
			private static string _003CHandleConnectedSetChanged_003Em__E6(Participant p)
			{
				return p.ToString();
			}

			[CompilerGenerated]
			private void _003CLeaveRoom_003Em__EB()
			{
				mSession.OnGameThreadListener().LeftRoom();
			}
		}

		private class ShutdownState : State
		{
			private readonly RoomSession mSession;

			internal ShutdownState(RoomSession session)
			{
				mSession = Misc.CheckNotNull(session);
			}

			internal override bool IsActive()
			{
				return false;
			}

			internal override void LeaveRoom()
			{
				mSession.OnGameThreadListener().LeftRoom();
			}
		}

		private class LeavingRoom : State
		{
			private readonly RoomSession mSession;

			private readonly NativeRealTimeRoom mRoomToLeave;

			private readonly Action mLeavingCompleteCallback;

			internal LeavingRoom(RoomSession session, NativeRealTimeRoom room, Action leavingCompleteCallback)
			{
				mSession = Misc.CheckNotNull(session);
				mRoomToLeave = Misc.CheckNotNull(room);
				mLeavingCompleteCallback = Misc.CheckNotNull(leavingCompleteCallback);
			}

			internal override bool IsActive()
			{
				return false;
			}

			internal override void OnStateEntered()
			{
				mSession.Manager().LeaveRoom(mRoomToLeave, _003COnStateEntered_003Em__EC);
			}

			[CompilerGenerated]
			private void _003COnStateEntered_003Em__EC(CommonErrorStatus.ResponseStatus status)
			{
				mLeavingCompleteCallback();
				mSession.EnterState(new ShutdownState(mSession));
			}
		}

		private class AbortingRoomCreationState : State
		{
			private readonly RoomSession mSession;

			internal AbortingRoomCreationState(RoomSession session)
			{
				mSession = Misc.CheckNotNull(session);
			}

			internal override bool IsActive()
			{
				return false;
			}

			internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				if (!response.RequestSucceeded())
				{
					mSession.EnterState(new ShutdownState(mSession));
					mSession.OnGameThreadListener().RoomConnected(false);
				}
				else
				{
					mSession.EnterState(new LeavingRoom(mSession, response.Room(), _003CHandleRoomResponse_003Em__ED));
				}
			}

			[CompilerGenerated]
			private void _003CHandleRoomResponse_003Em__ED()
			{
				mSession.OnGameThreadListener().RoomConnected(false);
			}
		}

		[CompilerGenerated]
		private sealed class _003CCreateQuickGame_003Ec__AnonStorey232
		{
			internal RoomSession newSession;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;
		}

		[CompilerGenerated]
		private sealed class _003CCreateQuickGame_003Ec__AnonStorey230
		{
			internal RealtimeRoomConfig config;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;
		}

		[CompilerGenerated]
		private sealed class _003CCreateQuickGame_003Ec__AnonStorey231
		{
			internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper;

			internal _003CCreateQuickGame_003Ec__AnonStorey232 _003C_003Ef__ref_0024562;

			internal _003CCreateQuickGame_003Ec__AnonStorey230 _003C_003Ef__ref_0024560;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__C8()
			{
				_003C_003Ef__this.mRealtimeManager.CreateRoom(_003C_003Ef__ref_0024560.config, helper, _003C_003Ef__ref_0024562.newSession.HandleRoomResponse);
			}
		}

		[CompilerGenerated]
		private sealed class _003CHelperForSession_003Ec__AnonStorey233
		{
			internal RoomSession session;

			internal void _003C_003Em__C9(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant, byte[] data, bool isReliable)
			{
				session.OnDataReceived(room, participant, data, isReliable);
			}

			internal void _003C_003Em__CA(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				session.OnParticipantStatusChanged(room, participant);
			}

			internal void _003C_003Em__CB(NativeRealTimeRoom room)
			{
				session.OnConnectedSetChanged(room);
			}

			internal void _003C_003Em__CC(NativeRealTimeRoom room)
			{
				session.OnRoomStatusChanged(room);
			}
		}

		[CompilerGenerated]
		private sealed class _003CCreateWithInvitationScreen_003Ec__AnonStorey235
		{
			internal uint variant;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;
		}

		[CompilerGenerated]
		private sealed class _003CCreateWithInvitationScreen_003Ec__AnonStorey234
		{
			private sealed class _003CCreateWithInvitationScreen_003Ec__AnonStorey236
			{
				internal RealtimeRoomConfig config;

				internal _003CCreateWithInvitationScreen_003Ec__AnonStorey234 _003C_003Ef__ref_0024564;
			}

			private sealed class _003CCreateWithInvitationScreen_003Ec__AnonStorey237
			{
				internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper;

				internal _003CCreateWithInvitationScreen_003Ec__AnonStorey234 _003C_003Ef__ref_0024564;

				internal _003CCreateWithInvitationScreen_003Ec__AnonStorey236 _003C_003Ef__ref_0024566;

				internal void _003C_003Em__D2()
				{
					_003C_003Ef__ref_0024564._003C_003Ef__this.mRealtimeManager.CreateRoom(_003C_003Ef__ref_0024566.config, helper, _003C_003Ef__ref_0024564.newRoom.HandleRoomResponse);
				}
			}

			internal RoomSession newRoom;

			internal _003CCreateWithInvitationScreen_003Ec__AnonStorey235 _003C_003Ef__ref_0024565;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__CD(PlayerSelectUIResponse response)
			{
				_003C_003Ef__this.mCurrentSession.ShowingUI = false;
				if (response.Status() != CommonErrorStatus.UIStatus.VALID)
				{
					Logger.d("User did not complete invitation screen.");
					newRoom.LeaveRoom();
					return;
				}
				_003C_003Ef__this.mCurrentSession.MinPlayersToStart = (uint)((int)response.MinimumAutomatchingPlayers() + response.Count() + 1);
				using (RealtimeRoomConfigBuilder realtimeRoomConfigBuilder = RealtimeRoomConfigBuilder.Create())
				{
					realtimeRoomConfigBuilder.SetVariant(_003C_003Ef__ref_0024565.variant);
					realtimeRoomConfigBuilder.PopulateFromUIResponse(response);
					_003CCreateWithInvitationScreen_003Ec__AnonStorey236 _003CCreateWithInvitationScreen_003Ec__AnonStorey = new _003CCreateWithInvitationScreen_003Ec__AnonStorey236();
					_003CCreateWithInvitationScreen_003Ec__AnonStorey._003C_003Ef__ref_0024564 = this;
					_003CCreateWithInvitationScreen_003Ec__AnonStorey.config = realtimeRoomConfigBuilder.Build();
					try
					{
						_003CCreateWithInvitationScreen_003Ec__AnonStorey237 _003CCreateWithInvitationScreen_003Ec__AnonStorey2 = new _003CCreateWithInvitationScreen_003Ec__AnonStorey237();
						_003CCreateWithInvitationScreen_003Ec__AnonStorey2._003C_003Ef__ref_0024564 = this;
						_003CCreateWithInvitationScreen_003Ec__AnonStorey2._003C_003Ef__ref_0024566 = _003CCreateWithInvitationScreen_003Ec__AnonStorey;
						_003CCreateWithInvitationScreen_003Ec__AnonStorey2.helper = HelperForSession(newRoom);
						try
						{
							newRoom.StartRoomCreation(_003C_003Ef__this.mNativeClient.GetUserId(), _003CCreateWithInvitationScreen_003Ec__AnonStorey2._003C_003Em__D2);
						}
						finally
						{
							if (_003CCreateWithInvitationScreen_003Ec__AnonStorey2.helper != null)
							{
								((IDisposable)_003CCreateWithInvitationScreen_003Ec__AnonStorey2.helper).Dispose();
							}
						}
					}
					finally
					{
						if (_003CCreateWithInvitationScreen_003Ec__AnonStorey.config != null)
						{
							((IDisposable)_003CCreateWithInvitationScreen_003Ec__AnonStorey.config).Dispose();
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetAllInvitations_003Ec__AnonStorey238
		{
			internal Action<Invitation[]> callback;

			internal void _003C_003Em__CE(RealtimeManager.FetchInvitationsResponse response)
			{
				if (!response.RequestSucceeded())
				{
					Logger.e("Couldn't load invitations.");
					callback(new Invitation[0]);
					return;
				}
				List<Invitation> list = new List<Invitation>();
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in response.Invitations())
				{
					using (item)
					{
						list.Add(item.AsInvitation());
					}
				}
				callback(list.ToArray());
			}
		}

		[CompilerGenerated]
		private sealed class _003CAcceptFromInbox_003Ec__AnonStorey239
		{
			private sealed class _003CAcceptFromInbox_003Ec__AnonStorey23A
			{
				internal GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation;

				internal _003CAcceptFromInbox_003Ec__AnonStorey239 _003C_003Ef__ref_0024569;
			}

			private sealed class _003CAcceptFromInbox_003Ec__AnonStorey23B
			{
				internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper;

				internal _003CAcceptFromInbox_003Ec__AnonStorey239 _003C_003Ef__ref_0024569;

				internal _003CAcceptFromInbox_003Ec__AnonStorey23A _003C_003Ef__ref_0024570;

				internal void _003C_003Em__D3()
				{
					_003C_003Ef__ref_0024569._003C_003Ef__this.mRealtimeManager.AcceptInvitation(_003C_003Ef__ref_0024570.invitation, helper, _003C_003Em__D4);
				}

				internal void _003C_003Em__D4(RealtimeManager.RealTimeRoomResponse acceptResponse)
				{
					using (_003C_003Ef__ref_0024570.invitation)
					{
						_003C_003Ef__ref_0024569.newRoom.HandleRoomResponse(acceptResponse);
						_003C_003Ef__ref_0024569.newRoom.SetInvitation(_003C_003Ef__ref_0024570.invitation.AsInvitation());
					}
				}
			}

			internal RoomSession newRoom;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__CF(RealtimeManager.RoomInboxUIResponse response)
			{
				_003CAcceptFromInbox_003Ec__AnonStorey23A _003CAcceptFromInbox_003Ec__AnonStorey23A = new _003CAcceptFromInbox_003Ec__AnonStorey23A();
				_003CAcceptFromInbox_003Ec__AnonStorey23A._003C_003Ef__ref_0024569 = this;
				_003C_003Ef__this.mCurrentSession.ShowingUI = false;
				if (response.ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
				{
					Logger.d("User did not complete invitation screen.");
					newRoom.LeaveRoom();
					return;
				}
				_003CAcceptFromInbox_003Ec__AnonStorey23A.invitation = response.Invitation();
				_003CAcceptFromInbox_003Ec__AnonStorey23B _003CAcceptFromInbox_003Ec__AnonStorey23B = new _003CAcceptFromInbox_003Ec__AnonStorey23B();
				_003CAcceptFromInbox_003Ec__AnonStorey23B._003C_003Ef__ref_0024569 = this;
				_003CAcceptFromInbox_003Ec__AnonStorey23B._003C_003Ef__ref_0024570 = _003CAcceptFromInbox_003Ec__AnonStorey23A;
				_003CAcceptFromInbox_003Ec__AnonStorey23B.helper = HelperForSession(newRoom);
				try
				{
					Logger.d("About to accept invitation " + _003CAcceptFromInbox_003Ec__AnonStorey23A.invitation.Id());
					newRoom.StartRoomCreation(_003C_003Ef__this.mNativeClient.GetUserId(), _003CAcceptFromInbox_003Ec__AnonStorey23B._003C_003Em__D3);
				}
				finally
				{
					if (_003CAcceptFromInbox_003Ec__AnonStorey23B.helper != null)
					{
						((IDisposable)_003CAcceptFromInbox_003Ec__AnonStorey23B.helper).Dispose();
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CAcceptInvitation_003Ec__AnonStorey23D
		{
			internal string invitationId;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;
		}

		[CompilerGenerated]
		private sealed class _003CAcceptInvitation_003Ec__AnonStorey23C
		{
			private sealed class _003CAcceptInvitation_003Ec__AnonStorey23E
			{
				internal GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation;

				internal _003CAcceptInvitation_003Ec__AnonStorey23D _003C_003Ef__ref_0024573;

				internal _003CAcceptInvitation_003Ec__AnonStorey23C _003C_003Ef__ref_0024572;
			}

			private sealed class _003CAcceptInvitation_003Ec__AnonStorey23F
			{
				internal GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper;

				internal _003CAcceptInvitation_003Ec__AnonStorey23C _003C_003Ef__ref_0024572;

				internal _003CAcceptInvitation_003Ec__AnonStorey23E _003C_003Ef__ref_0024574;

				internal void _003C_003Em__D5()
				{
					_003C_003Ef__ref_0024572._003C_003Ef__this.mRealtimeManager.AcceptInvitation(_003C_003Ef__ref_0024574.invitation, helper, _003C_003Ef__ref_0024572.newRoom.HandleRoomResponse);
				}
			}

			internal RoomSession newRoom;

			internal _003CAcceptInvitation_003Ec__AnonStorey23D _003C_003Ef__ref_0024573;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__D0(RealtimeManager.FetchInvitationsResponse response)
			{
				//Discarded unreachable code: IL_011e
				if (!response.RequestSucceeded())
				{
					Logger.e("Couldn't load invitations.");
					newRoom.LeaveRoom();
					return;
				}
				_003CAcceptInvitation_003Ec__AnonStorey23E _003CAcceptInvitation_003Ec__AnonStorey23E = new _003CAcceptInvitation_003Ec__AnonStorey23E();
				_003CAcceptInvitation_003Ec__AnonStorey23E._003C_003Ef__ref_0024573 = _003C_003Ef__ref_0024573;
				_003CAcceptInvitation_003Ec__AnonStorey23E._003C_003Ef__ref_0024572 = this;
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in response.Invitations())
				{
					_003CAcceptInvitation_003Ec__AnonStorey23E.invitation = item;
					using (_003CAcceptInvitation_003Ec__AnonStorey23E.invitation)
					{
						if (!_003CAcceptInvitation_003Ec__AnonStorey23E.invitation.Id().Equals(_003C_003Ef__ref_0024573.invitationId))
						{
							continue;
						}
						_003C_003Ef__this.mCurrentSession.MinPlayersToStart = _003CAcceptInvitation_003Ec__AnonStorey23E.invitation.AutomatchingSlots() + _003CAcceptInvitation_003Ec__AnonStorey23E.invitation.ParticipantCount();
						Logger.d("Setting MinPlayersToStart with invitation to : " + _003C_003Ef__this.mCurrentSession.MinPlayersToStart);
						_003CAcceptInvitation_003Ec__AnonStorey23F _003CAcceptInvitation_003Ec__AnonStorey23F = new _003CAcceptInvitation_003Ec__AnonStorey23F();
						_003CAcceptInvitation_003Ec__AnonStorey23F._003C_003Ef__ref_0024572 = this;
						_003CAcceptInvitation_003Ec__AnonStorey23F._003C_003Ef__ref_0024574 = _003CAcceptInvitation_003Ec__AnonStorey23E;
						_003CAcceptInvitation_003Ec__AnonStorey23F.helper = HelperForSession(newRoom);
						try
						{
							newRoom.StartRoomCreation(_003C_003Ef__this.mNativeClient.GetUserId(), _003CAcceptInvitation_003Ec__AnonStorey23F._003C_003Em__D5);
							return;
						}
						finally
						{
							if (_003CAcceptInvitation_003Ec__AnonStorey23F.helper != null)
							{
								((IDisposable)_003CAcceptInvitation_003Ec__AnonStorey23F.helper).Dispose();
							}
						}
					}
				}
				Logger.e("Room creation failed since we could not find invitation with ID " + _003C_003Ef__ref_0024573.invitationId);
				newRoom.LeaveRoom();
			}
		}

		[CompilerGenerated]
		private sealed class _003CDeclineInvitation_003Ec__AnonStorey240
		{
			internal string invitationId;

			internal NativeRealtimeMultiplayerClient _003C_003Ef__this;

			internal void _003C_003Em__D1(RealtimeManager.FetchInvitationsResponse response)
			{
				if (!response.RequestSucceeded())
				{
					Logger.e("Couldn't load invitations.");
					return;
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation item in response.Invitations())
				{
					using (item)
					{
						if (item.Id().Equals(invitationId))
						{
							_003C_003Ef__this.mRealtimeManager.DeclineInvitation(item);
						}
					}
				}
			}
		}

		private readonly object mSessionLock = new object();

		private readonly NativeClient mNativeClient;

		private readonly RealtimeManager mRealtimeManager;

		private volatile RoomSession mCurrentSession;

		internal NativeRealtimeMultiplayerClient(NativeClient nativeClient, RealtimeManager manager)
		{
			mNativeClient = Misc.CheckNotNull(nativeClient);
			mRealtimeManager = Misc.CheckNotNull(manager);
			mCurrentSession = GetTerminatedSession();
			PlayGamesHelperObject.AddPauseCallback(HandleAppPausing);
		}

		private RoomSession GetTerminatedSession()
		{
			RoomSession roomSession = new RoomSession(mRealtimeManager, new NoopListener());
			roomSession.EnterState(new ShutdownState(roomSession), false);
			return roomSession;
		}

		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener)
		{
			CreateQuickGame(minOpponents, maxOpponents, variant, 0uL, listener);
		}

		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, RealTimeMultiplayerListener listener)
		{
			lock (mSessionLock)
			{
				_003CCreateQuickGame_003Ec__AnonStorey232 _003CCreateQuickGame_003Ec__AnonStorey = new _003CCreateQuickGame_003Ec__AnonStorey232();
				_003CCreateQuickGame_003Ec__AnonStorey._003C_003Ef__this = this;
				_003CCreateQuickGame_003Ec__AnonStorey.newSession = new RoomSession(mRealtimeManager, listener);
				if (mCurrentSession.IsActive())
				{
					Logger.e("Received attempt to create a new room without cleaning up the old one.");
					_003CCreateQuickGame_003Ec__AnonStorey.newSession.LeaveRoom();
					return;
				}
				mCurrentSession = _003CCreateQuickGame_003Ec__AnonStorey.newSession;
				Logger.d("QuickGame: Setting MinPlayersToStart = " + minOpponents);
				mCurrentSession.MinPlayersToStart = minOpponents;
				using (RealtimeRoomConfigBuilder realtimeRoomConfigBuilder = RealtimeRoomConfigBuilder.Create())
				{
					_003CCreateQuickGame_003Ec__AnonStorey230 _003CCreateQuickGame_003Ec__AnonStorey2 = new _003CCreateQuickGame_003Ec__AnonStorey230();
					_003CCreateQuickGame_003Ec__AnonStorey2._003C_003Ef__this = this;
					_003CCreateQuickGame_003Ec__AnonStorey2.config = realtimeRoomConfigBuilder.SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetVariant(variant)
						.SetExclusiveBitMask(exclusiveBitMask)
						.Build();
					using (_003CCreateQuickGame_003Ec__AnonStorey2.config)
					{
						_003CCreateQuickGame_003Ec__AnonStorey231 _003CCreateQuickGame_003Ec__AnonStorey3 = new _003CCreateQuickGame_003Ec__AnonStorey231();
						_003CCreateQuickGame_003Ec__AnonStorey3._003C_003Ef__ref_0024562 = _003CCreateQuickGame_003Ec__AnonStorey;
						_003CCreateQuickGame_003Ec__AnonStorey3._003C_003Ef__ref_0024560 = _003CCreateQuickGame_003Ec__AnonStorey2;
						_003CCreateQuickGame_003Ec__AnonStorey3._003C_003Ef__this = this;
						_003CCreateQuickGame_003Ec__AnonStorey3.helper = HelperForSession(_003CCreateQuickGame_003Ec__AnonStorey.newSession);
						try
						{
							_003CCreateQuickGame_003Ec__AnonStorey.newSession.StartRoomCreation(mNativeClient.GetUserId(), _003CCreateQuickGame_003Ec__AnonStorey3._003C_003Em__C8);
						}
						finally
						{
							if (_003CCreateQuickGame_003Ec__AnonStorey3.helper != null)
							{
								((IDisposable)_003CCreateQuickGame_003Ec__AnonStorey3.helper).Dispose();
							}
						}
					}
				}
			}
		}

		private static GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper HelperForSession(RoomSession session)
		{
			_003CHelperForSession_003Ec__AnonStorey233 _003CHelperForSession_003Ec__AnonStorey = new _003CHelperForSession_003Ec__AnonStorey233();
			_003CHelperForSession_003Ec__AnonStorey.session = session;
			return GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.Create().SetOnDataReceivedCallback(_003CHelperForSession_003Ec__AnonStorey._003C_003Em__C9).SetOnParticipantStatusChangedCallback(_003CHelperForSession_003Ec__AnonStorey._003C_003Em__CA)
				.SetOnRoomConnectedSetChangedCallback(_003CHelperForSession_003Ec__AnonStorey._003C_003Em__CB)
				.SetOnRoomStatusChangedCallback(_003CHelperForSession_003Ec__AnonStorey._003C_003Em__CC);
		}

		private void HandleAppPausing(bool paused)
		{
			if (paused)
			{
				Logger.d("Application is pausing, which disconnects the RTMP  client.  Leaving room.");
				LeaveRoom();
			}
		}

		public void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, RealTimeMultiplayerListener listener)
		{
			_003CCreateWithInvitationScreen_003Ec__AnonStorey235 _003CCreateWithInvitationScreen_003Ec__AnonStorey = new _003CCreateWithInvitationScreen_003Ec__AnonStorey235();
			_003CCreateWithInvitationScreen_003Ec__AnonStorey.variant = variant;
			_003CCreateWithInvitationScreen_003Ec__AnonStorey._003C_003Ef__this = this;
			lock (mSessionLock)
			{
				_003CCreateWithInvitationScreen_003Ec__AnonStorey234 _003CCreateWithInvitationScreen_003Ec__AnonStorey2 = new _003CCreateWithInvitationScreen_003Ec__AnonStorey234();
				_003CCreateWithInvitationScreen_003Ec__AnonStorey2._003C_003Ef__ref_0024565 = _003CCreateWithInvitationScreen_003Ec__AnonStorey;
				_003CCreateWithInvitationScreen_003Ec__AnonStorey2._003C_003Ef__this = this;
				_003CCreateWithInvitationScreen_003Ec__AnonStorey2.newRoom = new RoomSession(mRealtimeManager, listener);
				if (mCurrentSession.IsActive())
				{
					Logger.e("Received attempt to create a new room without cleaning up the old one.");
					_003CCreateWithInvitationScreen_003Ec__AnonStorey2.newRoom.LeaveRoom();
				}
				else
				{
					mCurrentSession = _003CCreateWithInvitationScreen_003Ec__AnonStorey2.newRoom;
					mCurrentSession.ShowingUI = true;
					mRealtimeManager.ShowPlayerSelectUI(minOpponents, maxOppponents, true, _003CCreateWithInvitationScreen_003Ec__AnonStorey2._003C_003Em__CD);
				}
			}
		}

		public void ShowWaitingRoomUI()
		{
			lock (mSessionLock)
			{
				mCurrentSession.ShowWaitingRoomUI();
			}
		}

		public void GetAllInvitations(Action<Invitation[]> callback)
		{
			_003CGetAllInvitations_003Ec__AnonStorey238 _003CGetAllInvitations_003Ec__AnonStorey = new _003CGetAllInvitations_003Ec__AnonStorey238();
			_003CGetAllInvitations_003Ec__AnonStorey.callback = callback;
			mRealtimeManager.FetchInvitations(_003CGetAllInvitations_003Ec__AnonStorey._003C_003Em__CE);
		}

		public void AcceptFromInbox(RealTimeMultiplayerListener listener)
		{
			lock (mSessionLock)
			{
				_003CAcceptFromInbox_003Ec__AnonStorey239 _003CAcceptFromInbox_003Ec__AnonStorey = new _003CAcceptFromInbox_003Ec__AnonStorey239();
				_003CAcceptFromInbox_003Ec__AnonStorey._003C_003Ef__this = this;
				_003CAcceptFromInbox_003Ec__AnonStorey.newRoom = new RoomSession(mRealtimeManager, listener);
				if (mCurrentSession.IsActive())
				{
					Logger.e("Received attempt to accept invitation without cleaning up active session.");
					_003CAcceptFromInbox_003Ec__AnonStorey.newRoom.LeaveRoom();
				}
				else
				{
					mCurrentSession = _003CAcceptFromInbox_003Ec__AnonStorey.newRoom;
					mCurrentSession.ShowingUI = true;
					mRealtimeManager.ShowRoomInboxUI(_003CAcceptFromInbox_003Ec__AnonStorey._003C_003Em__CF);
				}
			}
		}

		public void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener)
		{
			_003CAcceptInvitation_003Ec__AnonStorey23D _003CAcceptInvitation_003Ec__AnonStorey23D = new _003CAcceptInvitation_003Ec__AnonStorey23D();
			_003CAcceptInvitation_003Ec__AnonStorey23D.invitationId = invitationId;
			_003CAcceptInvitation_003Ec__AnonStorey23D._003C_003Ef__this = this;
			lock (mSessionLock)
			{
				_003CAcceptInvitation_003Ec__AnonStorey23C _003CAcceptInvitation_003Ec__AnonStorey23C = new _003CAcceptInvitation_003Ec__AnonStorey23C();
				_003CAcceptInvitation_003Ec__AnonStorey23C._003C_003Ef__ref_0024573 = _003CAcceptInvitation_003Ec__AnonStorey23D;
				_003CAcceptInvitation_003Ec__AnonStorey23C._003C_003Ef__this = this;
				_003CAcceptInvitation_003Ec__AnonStorey23C.newRoom = new RoomSession(mRealtimeManager, listener);
				if (mCurrentSession.IsActive())
				{
					Logger.e("Received attempt to accept invitation without cleaning up active session.");
					_003CAcceptInvitation_003Ec__AnonStorey23C.newRoom.LeaveRoom();
				}
				else
				{
					mCurrentSession = _003CAcceptInvitation_003Ec__AnonStorey23C.newRoom;
					mRealtimeManager.FetchInvitations(_003CAcceptInvitation_003Ec__AnonStorey23C._003C_003Em__D0);
				}
			}
		}

		public Invitation GetInvitation()
		{
			return mCurrentSession.GetInvitation();
		}

		public void LeaveRoom()
		{
			mCurrentSession.LeaveRoom();
		}

		public void SendMessageToAll(bool reliable, byte[] data)
		{
			mCurrentSession.SendMessageToAll(reliable, data);
		}

		public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
		{
			mCurrentSession.SendMessageToAll(reliable, data, offset, length);
		}

		public void SendMessage(bool reliable, string participantId, byte[] data)
		{
			mCurrentSession.SendMessage(reliable, participantId, data);
		}

		public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
		{
			mCurrentSession.SendMessage(reliable, participantId, data, offset, length);
		}

		public List<Participant> GetConnectedParticipants()
		{
			return mCurrentSession.GetConnectedParticipants();
		}

		public Participant GetSelf()
		{
			return mCurrentSession.GetSelf();
		}

		public Participant GetParticipant(string participantId)
		{
			return mCurrentSession.GetParticipant(participantId);
		}

		public bool IsRoomConnected()
		{
			return mCurrentSession.IsRoomConnected();
		}

		public void DeclineInvitation(string invitationId)
		{
			_003CDeclineInvitation_003Ec__AnonStorey240 _003CDeclineInvitation_003Ec__AnonStorey = new _003CDeclineInvitation_003Ec__AnonStorey240();
			_003CDeclineInvitation_003Ec__AnonStorey.invitationId = invitationId;
			_003CDeclineInvitation_003Ec__AnonStorey._003C_003Ef__this = this;
			mRealtimeManager.FetchInvitations(_003CDeclineInvitation_003Ec__AnonStorey._003C_003Em__D1);
		}

		private static T WithDefault<T>(T presented, T defaultValue) where T : class
		{
			return (presented == null) ? defaultValue : presented;
		}
	}
}
