using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	internal static class MetadataExtensions
	{
		public static string GetDescription(this ISavedGameMetadata metadata)
		{
			if (metadata == null)
			{
				return "<null>";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1:s})", new object[] { metadata.Description, metadata.LastModifiedTimestamp });
		}
	}
}