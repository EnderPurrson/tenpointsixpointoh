using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;

namespace Rilisoft
{
	public struct GoogleSavedGameRequestResult<TValue> : IEquatable<GoogleSavedGameRequestResult<TValue>>
	{
		private readonly SavedGameRequestStatus _requestStatus;

		private readonly TValue _value;

		public SavedGameRequestStatus RequestStatus
		{
			get
			{
				return this._requestStatus;
			}
		}

		public TValue Value
		{
			get
			{
				return this._value;
			}
		}

		public GoogleSavedGameRequestResult(SavedGameRequestStatus requestStatus, TValue value)
		{
			this._requestStatus = requestStatus;
			this._value = value;
		}

		public bool Equals(GoogleSavedGameRequestResult<TValue> other)
		{
			if (!this._requestStatus.Equals(other.RequestStatus))
			{
				return false;
			}
			if (!EqualityComparer<TValue>.Default.Equals(other.Value))
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is GoogleSavedGameRequestResult<TValue>))
			{
				return false;
			}
			return this.Equals((GoogleSavedGameRequestResult<TValue>)obj);
		}

		public override int GetHashCode()
		{
			return this.RequestStatus.GetHashCode() ^ this.Value.GetHashCode();
		}
	}
}