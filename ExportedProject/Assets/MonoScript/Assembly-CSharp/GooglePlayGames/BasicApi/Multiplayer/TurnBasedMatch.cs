using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class TurnBasedMatch
	{
		private string mMatchId;

		private byte[] mData;

		private bool mCanRematch;

		private uint mAvailableAutomatchSlots;

		private string mSelfParticipantId;

		private List<Participant> mParticipants;

		private string mPendingParticipantId;

		private TurnBasedMatch.MatchTurnStatus mTurnStatus;

		private TurnBasedMatch.MatchStatus mMatchStatus;

		private uint mVariant;

		private uint mVersion;

		public uint AvailableAutomatchSlots
		{
			get
			{
				return this.mAvailableAutomatchSlots;
			}
		}

		public bool CanRematch
		{
			get
			{
				return this.mCanRematch;
			}
		}

		public byte[] Data
		{
			get
			{
				return this.mData;
			}
		}

		public string MatchId
		{
			get
			{
				return this.mMatchId;
			}
		}

		public List<Participant> Participants
		{
			get
			{
				return this.mParticipants;
			}
		}

		public Participant PendingParticipant
		{
			get
			{
				Participant participant;
				if (this.mPendingParticipantId != null)
				{
					participant = this.GetParticipant(this.mPendingParticipantId);
				}
				else
				{
					participant = null;
				}
				return participant;
			}
		}

		public string PendingParticipantId
		{
			get
			{
				return this.mPendingParticipantId;
			}
		}

		public Participant Self
		{
			get
			{
				return this.GetParticipant(this.mSelfParticipantId);
			}
		}

		public string SelfParticipantId
		{
			get
			{
				return this.mSelfParticipantId;
			}
		}

		public TurnBasedMatch.MatchStatus Status
		{
			get
			{
				return this.mMatchStatus;
			}
		}

		public TurnBasedMatch.MatchTurnStatus TurnStatus
		{
			get
			{
				return this.mTurnStatus;
			}
		}

		public uint Variant
		{
			get
			{
				return this.mVariant;
			}
		}

		public uint Version
		{
			get
			{
				return this.mVersion;
			}
		}

		internal TurnBasedMatch(string matchId, byte[] data, bool canRematch, string selfParticipantId, List<Participant> participants, uint availableAutomatchSlots, string pendingParticipantId, TurnBasedMatch.MatchTurnStatus turnStatus, TurnBasedMatch.MatchStatus matchStatus, uint variant, uint version)
		{
			this.mMatchId = matchId;
			this.mData = data;
			this.mCanRematch = canRematch;
			this.mSelfParticipantId = selfParticipantId;
			this.mParticipants = participants;
			this.mParticipants.Sort();
			this.mAvailableAutomatchSlots = availableAutomatchSlots;
			this.mPendingParticipantId = pendingParticipantId;
			this.mTurnStatus = turnStatus;
			this.mMatchStatus = matchStatus;
			this.mVariant = variant;
			this.mVersion = version;
		}

		public Participant GetParticipant(string participantId)
		{
			Participant participant;
			List<Participant>.Enumerator enumerator = this.mParticipants.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Participant current = enumerator.Current;
					if (!current.ParticipantId.Equals(participantId))
					{
						continue;
					}
					participant = current;
					return participant;
				}
				Logger.w(string.Concat("Participant not found in turn-based match: ", participantId));
				return null;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return participant;
		}

		public override string ToString()
		{
			object[] objArray = new object[] { this.mMatchId, this.mData, this.mCanRematch, this.mSelfParticipantId, null, null, null, null, null, null };
			objArray[4] = string.Join(",", (
				from p in this.mParticipants
				select p.ToString()).ToArray<string>());
			objArray[5] = this.mPendingParticipantId;
			objArray[6] = this.mTurnStatus;
			objArray[7] = this.mMatchStatus;
			objArray[8] = this.mVariant;
			objArray[9] = this.mVersion;
			return string.Format("[TurnBasedMatch: mMatchId={0}, mData={1}, mCanRematch={2}, mSelfParticipantId={3}, mParticipants={4}, mPendingParticipantId={5}, mTurnStatus={6}, mMatchStatus={7}, mVariant={8}, mVersion={9}]", objArray);
		}

		public enum MatchStatus
		{
			Active,
			AutoMatching,
			Cancelled,
			Complete,
			Expired,
			Unknown,
			Deleted
		}

		public enum MatchTurnStatus
		{
			Complete,
			Invited,
			MyTurn,
			TheirTurn,
			Unknown
		}
	}
}