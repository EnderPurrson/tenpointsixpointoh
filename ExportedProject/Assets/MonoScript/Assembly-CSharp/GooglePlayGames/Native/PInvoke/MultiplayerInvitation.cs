using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class MultiplayerInvitation : BaseReferenceHolder
	{
		internal MultiplayerInvitation(IntPtr selfPointer) : base(selfPointer)
		{
		}

		internal Invitation AsInvitation()
		{
			Participant participant;
			Participant participant1;
			Invitation.InvType invType = GooglePlayGames.Native.PInvoke.MultiplayerInvitation.ToInvType(this.Type());
			string str = this.Id();
			int num = (int)this.Variant();
			using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = this.Inviter())
			{
				if (multiplayerParticipant != null)
				{
					participant1 = multiplayerParticipant.AsParticipant();
				}
				else
				{
					participant1 = null;
				}
				participant = participant1;
			}
			return new Invitation(invType, str, participant, num);
		}

		internal uint AutomatchingSlots()
		{
			return GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_AutomatchingSlotsAvailable(base.SelfPtr());
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Dispose(selfPointer);
		}

		internal static GooglePlayGames.Native.PInvoke.MultiplayerInvitation FromPointer(IntPtr selfPointer)
		{
			if (PInvokeUtilities.IsNull(selfPointer))
			{
				return null;
			}
			return new GooglePlayGames.Native.PInvoke.MultiplayerInvitation(selfPointer);
		}

		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Id(base.SelfPtr(), out_string, size));
		}

		internal GooglePlayGames.Native.PInvoke.MultiplayerParticipant Inviter()
		{
			GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_InvitingParticipant(base.SelfPtr()));
			if (multiplayerParticipant.Valid())
			{
				return multiplayerParticipant;
			}
			multiplayerParticipant.Dispose();
			return null;
		}

		internal uint ParticipantCount()
		{
			return GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Participants_Length(base.SelfPtr()).ToUInt32();
		}

		private static Invitation.InvType ToInvType(Types.MultiplayerInvitationType invitationType)
		{
			Types.MultiplayerInvitationType multiplayerInvitationType = invitationType;
			if (multiplayerInvitationType == Types.MultiplayerInvitationType.TURN_BASED)
			{
				return Invitation.InvType.TurnBased;
			}
			if (multiplayerInvitationType == Types.MultiplayerInvitationType.REAL_TIME)
			{
				return Invitation.InvType.RealTime;
			}
			Logger.d(string.Concat("Found unknown invitation type: ", invitationType));
			return Invitation.InvType.Unknown;
		}

		internal Types.MultiplayerInvitationType Type()
		{
			return GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Type(base.SelfPtr());
		}

		internal uint Variant()
		{
			return GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Variant(base.SelfPtr());
		}
	}
}