using GooglePlayGames;
using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Participant : IComparable<Participant>
	{
		private string mDisplayName = string.Empty;

		private string mParticipantId = string.Empty;

		private Participant.ParticipantStatus mStatus = Participant.ParticipantStatus.Unknown;

		private GooglePlayGames.BasicApi.Multiplayer.Player mPlayer;

		private bool mIsConnectedToRoom;

		public string DisplayName
		{
			get
			{
				return this.mDisplayName;
			}
		}

		public bool IsAutomatch
		{
			get
			{
				return this.mPlayer == null;
			}
		}

		public bool IsConnectedToRoom
		{
			get
			{
				return this.mIsConnectedToRoom;
			}
		}

		public string ParticipantId
		{
			get
			{
				return this.mParticipantId;
			}
		}

		public GooglePlayGames.BasicApi.Multiplayer.Player Player
		{
			get
			{
				return this.mPlayer;
			}
		}

		public Participant.ParticipantStatus Status
		{
			get
			{
				return this.mStatus;
			}
		}

		internal Participant(string displayName, string participantId, Participant.ParticipantStatus status, GooglePlayGames.BasicApi.Multiplayer.Player player, bool connectedToRoom)
		{
			this.mDisplayName = displayName;
			this.mParticipantId = participantId;
			this.mStatus = status;
			this.mPlayer = player;
			this.mIsConnectedToRoom = connectedToRoom;
		}

		public int CompareTo(Participant other)
		{
			return this.mParticipantId.CompareTo(other.mParticipantId);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != typeof(Participant))
			{
				return false;
			}
			Participant participant = (Participant)obj;
			return this.mParticipantId.Equals(participant.mParticipantId);
		}

		public override int GetHashCode()
		{
			return (this.mParticipantId == null ? 0 : this.mParticipantId.GetHashCode());
		}

		public override string ToString()
		{
			object[] str = new object[] { this.mDisplayName, this.mParticipantId, this.mStatus.ToString(), null, null };
			str[3] = (this.mPlayer != null ? this.mPlayer.ToString() : "NULL");
			str[4] = this.mIsConnectedToRoom;
			return string.Format("[Participant: '{0}' (id {1}), status={2}, player={3}, connected={4}]", str);
		}

		public enum ParticipantStatus
		{
			NotInvitedYet,
			Invited,
			Joined,
			Declined,
			Left,
			Finished,
			Unresponsive,
			Unknown
		}
	}
}