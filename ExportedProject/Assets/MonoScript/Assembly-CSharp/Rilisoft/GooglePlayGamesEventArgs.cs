using System;

namespace Rilisoft
{
	internal sealed class GooglePlayGamesEventArgs : EventArgs
	{
		private string _data;

		private int _slot;

		private bool _succeeded;

		public string Data
		{
			get
			{
				return this._data;
			}
		}

		public int Slot
		{
			get
			{
				return this._slot;
			}
		}

		public bool Succeeded
		{
			get
			{
				return this._succeeded;
			}
		}

		public GooglePlayGamesEventArgs(bool succeeded, int slot, string data)
		{
			this._succeeded = succeeded;
			this._slot = slot;
			this._data = data ?? string.Empty;
		}

		public override string ToString()
		{
			return (!this._succeeded ? "<Failed>" : string.Format("Slot: {0}, Data: “{1}”", this._slot, this._data));
		}
	}
}