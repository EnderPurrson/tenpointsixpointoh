using System;
using System.Runtime.CompilerServices;

namespace ExitGames.Client.Photon
{
	public abstract class PhotonPing : global::System.IDisposable
	{
		public string DebugString = "";

		public bool Successful;

		protected internal bool GotResult;

		protected internal int PingLength = 13;

		protected internal byte[] PingBytes;

		protected internal byte PingId;

		public virtual bool StartPing(string ip)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException();
		}

		public virtual bool Done()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException();
		}

		public virtual void Dispose()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException();
		}

		protected internal void Init()
		{
			GotResult = false;
			Successful = false;
			PingId = (byte)(Environment.get_TickCount() % 255);
		}

		protected PhotonPing()
		{
			byte[] array = new byte[13];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			PingBytes = array;
			base._002Ector();
		}
	}
}
