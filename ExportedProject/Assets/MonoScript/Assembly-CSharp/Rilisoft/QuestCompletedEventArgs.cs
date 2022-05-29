using System;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public sealed class QuestCompletedEventArgs : EventArgs
	{
		public QuestBase Quest
		{
			get;
			set;
		}

		public QuestCompletedEventArgs()
		{
		}
	}
}