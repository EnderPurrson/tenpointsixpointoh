using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Reflection;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class DummyConflictResolver : IConflictResolver
	{
		private readonly static DummyConflictResolver s_instance;

		internal static DummyConflictResolver Instance
		{
			get
			{
				return DummyConflictResolver.s_instance;
			}
		}

		static DummyConflictResolver()
		{
			DummyConflictResolver.s_instance = new DummyConflictResolver();
		}

		private DummyConflictResolver()
		{
		}

		public void ChooseMetadata(ISavedGameMetadata chosenMetadata)
		{
			string str = (chosenMetadata == null ? string.Empty : chosenMetadata.Filename);
			Debug.LogFormat("{0}('{1}').ChooseMetadata()", new object[] { this.GetType().Name, str });
		}
	}
}