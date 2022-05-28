using System;

namespace ExitGames.Client.Photon
{
	public class SocketUdpNativeStatic : IPhotonSocket
	{
		public SocketUdpNativeStatic(PeerBase peerBase)
			: base(peerBase)
		{
		}

		public override bool Disconnect()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException("This class was compiled in an assembly WITH c# sockets. Another dll must be used for native sockets.");
		}

		public override PhotonSocketError Send(byte[] data, int length)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException("This class was compiled in an assembly WITH c# sockets. Another dll must be used for native sockets.");
		}

		public override PhotonSocketError Receive(out byte[] data)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException("This class was compiled in an assembly WITH c# sockets. Another dll must be used for native sockets.");
		}
	}
}
