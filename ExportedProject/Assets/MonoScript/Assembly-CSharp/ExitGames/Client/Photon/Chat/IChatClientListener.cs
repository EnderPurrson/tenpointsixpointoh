using ExitGames.Client.Photon;
using System;

namespace ExitGames.Client.Photon.Chat
{
	public interface IChatClientListener
	{
		void DebugReturn(DebugLevel level, string message);

		void OnChatStateChange(ChatState state);

		void OnConnected();

		void OnDisconnected();

		void OnGetMessages(string channelName, string[] senders, object[] messages);

		void OnPrivateMessage(string sender, object message, string channelName);

		void OnStatusUpdate(string user, int status, bool gotMessage, object message);

		void OnSubscribed(string[] channels, bool[] results);

		void OnUnsubscribed(string[] channels);
	}
}