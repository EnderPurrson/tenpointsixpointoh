using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public sealed class QuestInfo
	{
		private readonly bool _forcedSkip;

		private readonly IList<QuestBase> _quests;

		private readonly Func<IList<QuestBase>> _skipMethod;

		public bool CanSkip
		{
			get
			{
				if (this._skipMethod == null)
				{
					return false;
				}
				if (this._quests.Count == 0)
				{
					return this._forcedSkip;
				}
				if (this._quests[0].Rewarded)
				{
					return false;
				}
				if (this._quests[0].CalculateProgress() >= new decimal(1))
				{
					return false;
				}
				if (this._quests.Count >= 2)
				{
					return true;
				}
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("_quests.Count < 2: {0}", new object[] { this._quests.Count });
				}
				return this._forcedSkip;
			}
		}

		public QuestBase Quest
		{
			get
			{
				return this._quests.FirstOrDefault<QuestBase>();
			}
		}

		internal QuestInfo(IEnumerable<QuestBase> quests, Func<IList<QuestBase>> skipMethod, bool forcedSkip = false)
		{
			if (quests == null)
			{
				throw new ArgumentNullException("quests");
			}
			this._forcedSkip = forcedSkip;
			this._quests = quests.ToList<QuestBase>();
			this._skipMethod = skipMethod;
		}

		public void Skip()
		{
			if (!this.CanSkip)
			{
				return;
			}
			IList<QuestBase> questBases = this._skipMethod();
			this._quests.Clear();
			if (questBases != null)
			{
				IEnumerator<QuestBase> enumerator = questBases.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						QuestBase current = enumerator.Current;
						this._quests.Add(current);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
			}
		}
	}
}