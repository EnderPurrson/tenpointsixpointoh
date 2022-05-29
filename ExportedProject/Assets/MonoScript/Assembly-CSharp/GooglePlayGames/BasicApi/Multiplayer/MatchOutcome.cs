using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class MatchOutcome
	{
		public const uint PlacementUnset = 0;

		private List<string> mParticipantIds = new List<string>();

		private Dictionary<string, uint> mPlacements = new Dictionary<string, uint>();

		private Dictionary<string, MatchOutcome.ParticipantResult> mResults = new Dictionary<string, MatchOutcome.ParticipantResult>();

		public List<string> ParticipantIds
		{
			get
			{
				return this.mParticipantIds;
			}
		}

		public MatchOutcome()
		{
		}

		public uint GetPlacementFor(string participantId)
		{
			uint item;
			if (!this.mPlacements.ContainsKey(participantId))
			{
				item = 0;
			}
			else
			{
				item = this.mPlacements[participantId];
			}
			return item;
		}

		public MatchOutcome.ParticipantResult GetResultFor(string participantId)
		{
			return (!this.mResults.ContainsKey(participantId) ? MatchOutcome.ParticipantResult.Unset : this.mResults[participantId]);
		}

		public void SetParticipantResult(string participantId, MatchOutcome.ParticipantResult result, uint placement)
		{
			if (!this.mParticipantIds.Contains(participantId))
			{
				this.mParticipantIds.Add(participantId);
			}
			this.mPlacements[participantId] = placement;
			this.mResults[participantId] = result;
		}

		public void SetParticipantResult(string participantId, MatchOutcome.ParticipantResult result)
		{
			this.SetParticipantResult(participantId, result, 0);
		}

		public void SetParticipantResult(string participantId, uint placement)
		{
			this.SetParticipantResult(participantId, MatchOutcome.ParticipantResult.Unset, placement);
		}

		public override string ToString()
		{
			string str = "[MatchOutcome";
			foreach (string mParticipantId in this.mParticipantIds)
			{
				str = string.Concat(str, string.Format(" {0}->({1},{2})", mParticipantId, this.GetResultFor(mParticipantId), this.GetPlacementFor(mParticipantId)));
			}
			return string.Concat(str, "]");
		}

		public enum ParticipantResult
		{
			Unset = -1,
			None = 0,
			Win = 1,
			Loss = 2,
			Tie = 3
		}
	}
}