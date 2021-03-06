using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class MultiplayerParticipant : BaseReferenceHolder
	{
		private readonly static Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus> StatusConversion;

		static MultiplayerParticipant()
		{
			Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus> participantStatuses = new Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus>()
			{
				{ Types.ParticipantStatus.INVITED, Participant.ParticipantStatus.Invited },
				{ Types.ParticipantStatus.JOINED, Participant.ParticipantStatus.Joined },
				{ Types.ParticipantStatus.DECLINED, Participant.ParticipantStatus.Declined },
				{ Types.ParticipantStatus.LEFT, Participant.ParticipantStatus.Left },
				{ Types.ParticipantStatus.NOT_INVITED_YET, Participant.ParticipantStatus.NotInvitedYet },
				{ Types.ParticipantStatus.FINISHED, Participant.ParticipantStatus.Finished },
				{ Types.ParticipantStatus.UNRESPONSIVE, Participant.ParticipantStatus.Unresponsive }
			};
			GooglePlayGames.Native.PInvoke.MultiplayerParticipant.StatusConversion = participantStatuses;
		}

		internal MultiplayerParticipant(IntPtr selfPointer) : base(selfPointer)
		{
		}

		internal Participant AsParticipant()
		{
			GooglePlayGames.BasicApi.Multiplayer.Player player;
			NativePlayer nativePlayer = this.Player();
			string str = this.DisplayName();
			string str1 = this.Id();
			Participant.ParticipantStatus item = GooglePlayGames.Native.PInvoke.MultiplayerParticipant.StatusConversion[this.Status()];
			if (nativePlayer != null)
			{
				player = nativePlayer.AsPlayer();
			}
			else
			{
				player = null;
			}
			return new Participant(str, str1, item, player, this.IsConnectedToRoom());
		}

		internal static GooglePlayGames.Native.PInvoke.MultiplayerParticipant AutomatchingSentinel()
		{
			return new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(Sentinels.Sentinels_AutomatchingParticipant());
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Dispose(selfPointer);
		}

		internal string DisplayName()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_DisplayName(base.SelfPtr(), out_string, size));
		}

		internal static GooglePlayGames.Native.PInvoke.MultiplayerParticipant FromPointer(IntPtr pointer)
		{
			if (PInvokeUtilities.IsNull(pointer))
			{
				return null;
			}
			return new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(pointer);
		}

		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Id(base.SelfPtr(), out_string, size));
		}

		internal bool IsConnectedToRoom()
		{
			return (GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_IsConnectedToRoom(base.SelfPtr()) ? true : this.Status() == Types.ParticipantStatus.JOINED);
		}

		internal NativePlayer Player()
		{
			if (!GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_HasPlayer(base.SelfPtr()))
			{
				return null;
			}
			return new NativePlayer(GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Player(base.SelfPtr()));
		}

		internal Types.ParticipantStatus Status()
		{
			return GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Status(base.SelfPtr());
		}

		internal bool Valid()
		{
			return GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Valid(base.SelfPtr());
		}
	}
}