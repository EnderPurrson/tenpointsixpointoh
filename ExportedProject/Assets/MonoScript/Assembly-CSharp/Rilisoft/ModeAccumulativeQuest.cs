using System;
using System.Collections.Generic;

namespace Rilisoft
{
	public sealed class ModeAccumulativeQuest : AccumulativeQuestBase
	{
		private readonly ConnectSceneNGUIController.RegimGame _mode;

		public ConnectSceneNGUIController.RegimGame Mode
		{
			get
			{
				return this._mode;
			}
		}

		public ModeAccumulativeQuest(string id, long day, int slot, Rilisoft.Difficulty difficulty, Rilisoft.Reward reward, bool active, bool rewarded, int requiredCount, ConnectSceneNGUIController.RegimGame mode, int initialCount = 0) : base(id, day, slot, difficulty, reward, active, rewarded, requiredCount, initialCount)
		{
			this._mode = mode;
		}

		protected override void AppendProperties(Dictionary<string, object> properties)
		{
			base.AppendProperties(properties);
			properties["mode"] = this._mode;
		}
	}
}