using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	internal sealed class DummySavedGameMetadata : ISavedGameMetadata
	{
		private string _filename;

		public string CoverImageURL
		{
			get
			{
				return "http://example.com";
			}
		}

		public string Description
		{
			get
			{
				return this.GetType().Name;
			}
		}

		public string Filename
		{
			get
			{
				return this._filename;
			}
		}

		public bool IsOpen
		{
			get
			{
				return false;
			}
		}

		public DateTime LastModifiedTimestamp
		{
			get
			{
				return JustDecompileGenerated_get_LastModifiedTimestamp();
			}
			set
			{
				JustDecompileGenerated_set_LastModifiedTimestamp(value);
			}
		}

		private DateTime JustDecompileGenerated_LastModifiedTimestamp_k__BackingField;

		public DateTime JustDecompileGenerated_get_LastModifiedTimestamp()
		{
			return this.JustDecompileGenerated_LastModifiedTimestamp_k__BackingField;
		}

		private void JustDecompileGenerated_set_LastModifiedTimestamp(DateTime value)
		{
			this.JustDecompileGenerated_LastModifiedTimestamp_k__BackingField = value;
		}

		public TimeSpan TotalTimePlayed
		{
			get
			{
				return TimeSpan.Zero;
			}
		}

		public DummySavedGameMetadata(string filename)
		{
			this._filename = filename ?? string.Empty;
			this.LastModifiedTimestamp = DateTime.Now;
		}
	}
}