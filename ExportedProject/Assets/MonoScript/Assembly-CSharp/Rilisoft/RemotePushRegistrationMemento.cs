using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	internal struct RemotePushRegistrationMemento : IEquatable<RemotePushRegistrationMemento>
	{
		[SerializeField]
		private string registrationId;

		[SerializeField]
		private string registrationTime;

		[SerializeField]
		private string version;

		public string RegistrationId
		{
			get
			{
				return this.registrationId ?? string.Empty;
			}
		}

		public string RegistrationTime
		{
			get
			{
				return this.registrationTime ?? string.Empty;
			}
		}

		public string Version
		{
			get
			{
				return this.version ?? string.Empty;
			}
		}

		public RemotePushRegistrationMemento(string registrationId, DateTime registrationTime, string version)
		{
			this.registrationId = registrationId ?? string.Empty;
			this.registrationTime = registrationTime.ToString("s", CultureInfo.InvariantCulture);
			this.version = version ?? string.Empty;
		}

		public bool Equals(RemotePushRegistrationMemento other)
		{
			if (this.RegistrationTime != other.RegistrationTime)
			{
				return false;
			}
			if (this.Version != other.Version)
			{
				return false;
			}
			if (this.RegistrationId != other.RegistrationId)
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is RemotePushRegistrationMemento))
			{
				return false;
			}
			return this.Equals((RemotePushRegistrationMemento)obj);
		}

		public override int GetHashCode()
		{
			return this.RegistrationTime.GetHashCode() ^ this.Version.GetHashCode() ^ this.RegistrationId.GetHashCode();
		}
	}
}