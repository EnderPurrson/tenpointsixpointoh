using System;
using System.Net.Sockets;

namespace ExitGames.Client.Photon
{
	public class PingMono : PhotonPing
	{
		private Socket sock;

		public override bool StartPing(string ip)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			Init();
			try
			{
				if (ip.Contains("."))
				{
					sock = new Socket((AddressFamily)2, (SocketType)2, (ProtocolType)17);
				}
				else
				{
					sock = new Socket((AddressFamily)23, (SocketType)2, (ProtocolType)17);
				}
				sock.set_ReceiveTimeout(5000);
				sock.Connect(ip, 5055);
				PingBytes[PingBytes.Length - 1] = PingId;
				sock.Send(PingBytes);
				PingBytes[PingBytes.Length - 1] = (byte)(PingId - 1);
			}
			catch (global::System.Exception ex)
			{
				sock = null;
				Console.WriteLine((object)ex);
			}
			return false;
		}

		public override bool Done()
		{
			if (GotResult || sock == null)
			{
				return true;
			}
			if (sock.get_Available() <= 0)
			{
				return false;
			}
			int num = sock.Receive(PingBytes, (SocketFlags)0);
			if (PingBytes[PingBytes.Length - 1] != PingId || num != PingLength)
			{
				DebugString += " ReplyMatch is false! ";
			}
			Successful = num == PingBytes.Length && PingBytes[PingBytes.Length - 1] == PingId;
			GotResult = true;
			return true;
		}

		public override void Dispose()
		{
			try
			{
				sock.Close();
			}
			catch
			{
			}
			sock = null;
		}
	}
}
