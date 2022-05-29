using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public struct Reward
	{
		public int Coins
		{
			get;
			set;
		}

		public int Experience
		{
			get;
			set;
		}

		public int Gems
		{
			get;
			set;
		}

		public static Reward Create(Dictionary<string, object> reward)
		{
			object obj;
			object obj1;
			object obj2;
			Reward num = new Reward();
			if (reward == null)
			{
				return num;
			}
			try
			{
				if (reward.TryGetValue("coins", out obj))
				{
					num.Coins = Convert.ToInt32(obj);
				}
				if (reward.TryGetValue("gems", out obj1))
				{
					num.Gems = Convert.ToInt32(obj1);
				}
				if (reward.TryGetValue("xp", out obj2))
				{
					num.Experience = Convert.ToInt32(obj2);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			return num;
		}

		public static Reward Create(List<object> reward)
		{
			Reward reward1 = new Reward();
			if (reward == null)
			{
				return reward1;
			}
			try
			{
				for (int i = 0; i != Math.Max(reward.Count, 3); i++)
				{
					int num = Convert.ToInt32(reward[i]);
					switch (i)
					{
						case 0:
						{
							reward1.Coins = num;
							break;
						}
						case 1:
						{
							reward1.Gems = num;
							break;
						}
						case 2:
						{
							reward1.Experience = num;
							break;
						}
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			return reward1;
		}

		public List<int> ToJson()
		{
			List<int> nums = new List<int>(3)
			{
				this.Coins,
				this.Gems,
				this.Experience
			};
			return nums;
		}
	}
}