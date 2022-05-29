using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeTurnBasedMatch : BaseReferenceHolder
	{
		internal NativeTurnBasedMatch(IntPtr selfPointer) : base(selfPointer)
		{
		}

		internal GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch AsTurnBasedMatch(string selfPlayerId)
		{
			List<Participant> participants = new List<Participant>();
			string str = null;
			string str1 = null;
			using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = this.PendingParticipant())
			{
				if (multiplayerParticipant != null)
				{
					str1 = multiplayerParticipant.Id();
				}
			}
			IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> enumerator = this.Participants().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GooglePlayGames.Native.PInvoke.MultiplayerParticipant current = enumerator.Current;
					using (current)
					{
						using (NativePlayer nativePlayer = current.Player())
						{
							if (nativePlayer != null && nativePlayer.Id().Equals(selfPlayerId))
							{
								str = current.Id();
							}
						}
						participants.Add(current.AsParticipant());
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
			bool flag = (this.MatchStatus() != Types.MatchStatus.COMPLETED ? false : !this.HasRematchId());
			return new GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch(this.Id(), this.Data(), flag, str, participants, this.AvailableAutomatchSlots(), str1, NativeTurnBasedMatch.ToTurnStatus(this.MatchStatus()), NativeTurnBasedMatch.ToMatchStatus(str1, this.MatchStatus()), this.Variant(), this.Version());
		}

		internal uint AvailableAutomatchSlots()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_AutomatchingSlotsAvailable(base.SelfPtr());
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Dispose(selfPointer);
		}

		internal ulong CreationTime()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_CreationTime(base.SelfPtr());
		}

		internal byte[] Data()
		{
			if (!GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_HasData(base.SelfPtr()))
			{
				Logger.d("Match has no data.");
				return null;
			}
			return PInvokeUtilities.OutParamsToArray<byte>((byte[] bytes, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Data(base.SelfPtr(), bytes, size));
		}

		internal string Description()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Description(base.SelfPtr(), out_string, size));
		}

		internal static NativeTurnBasedMatch FromPointer(IntPtr selfPointer)
		{
			if (PInvokeUtilities.IsNull(selfPointer))
			{
				return null;
			}
			return new NativeTurnBasedMatch(selfPointer);
		}

		internal bool HasRematchId()
		{
			string str = this.RematchId();
			return (string.IsNullOrEmpty(str) ? true : !str.Equals("(null)"));
		}

		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Id(base.SelfPtr(), out_string, size));
		}

		internal Types.MatchStatus MatchStatus()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Status(base.SelfPtr());
		}

		internal IEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> Participants()
		{
			return PInvokeUtilities.ToEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerParticipant>(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Participants_Length(base.SelfPtr()), (UIntPtr index) => new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Participants_GetElement(base.SelfPtr(), index)));
		}

		internal GooglePlayGames.Native.PInvoke.MultiplayerParticipant ParticipantWithId(string participantId)
		{
			GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant;
			IEnumerator<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> enumerator = this.Participants().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GooglePlayGames.Native.PInvoke.MultiplayerParticipant current = enumerator.Current;
					if (!current.Id().Equals(participantId))
					{
						current.Dispose();
					}
					else
					{
						multiplayerParticipant = current;
						return multiplayerParticipant;
					}
				}
				return null;
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			return multiplayerParticipant;
		}

		internal GooglePlayGames.Native.PInvoke.MultiplayerParticipant PendingParticipant()
		{
			GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_PendingParticipant(base.SelfPtr()));
			if (multiplayerParticipant.Valid())
			{
				return multiplayerParticipant;
			}
			multiplayerParticipant.Dispose();
			return null;
		}

		internal string RematchId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_RematchId(base.SelfPtr(), out_string, size));
		}

		internal GooglePlayGames.Native.PInvoke.ParticipantResults Results()
		{
			return new GooglePlayGames.Native.PInvoke.ParticipantResults(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_ParticipantResults(base.SelfPtr()));
		}

		private static GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus ToMatchStatus(string pendingParticipantId, Types.MatchStatus status)
		{
			switch (status)
			{
				case Types.MatchStatus.INVITED:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;
				}
				case Types.MatchStatus.THEIR_TURN:
				{
					return (pendingParticipantId != null ? GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active : GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.AutoMatching);
				}
				case Types.MatchStatus.MY_TURN:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;
				}
				case Types.MatchStatus.PENDING_COMPLETION:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Complete;
				}
				case Types.MatchStatus.COMPLETED:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Complete;
				}
				case Types.MatchStatus.CANCELED:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Cancelled;
				}
				case Types.MatchStatus.EXPIRED:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Expired;
				}
			}
			return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Unknown;
		}

		private static GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus ToTurnStatus(Types.MatchStatus status)
		{
			switch (status)
			{
				case Types.MatchStatus.INVITED:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Invited;
				}
				case Types.MatchStatus.THEIR_TURN:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.TheirTurn;
				}
				case Types.MatchStatus.MY_TURN:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.MyTurn;
				}
				case Types.MatchStatus.PENDING_COMPLETION:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
				}
				case Types.MatchStatus.COMPLETED:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
				}
				case Types.MatchStatus.CANCELED:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
				}
				case Types.MatchStatus.EXPIRED:
				{
					return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
				}
			}
			return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Unknown;
		}

		internal uint Variant()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Variant(base.SelfPtr());
		}

		internal uint Version()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Version(base.SelfPtr());
		}
	}
}