using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rilisoft
{
	internal sealed class DictionaryLoadedListener
	{
		public DictionaryLoadedListener()
		{
		}

		internal static string MergeProgress(string localDataString, string serverDataString)
		{
			Dictionary<string, int> strs;
			Dictionary<string, int> strs1;
			int num;
			Dictionary<string, Dictionary<string, int>> strs2 = CampaignProgress.DeserializeProgress(localDataString) ?? new Dictionary<string, Dictionary<string, int>>();
			Dictionary<string, Dictionary<string, int>> strs3 = CampaignProgress.DeserializeProgress(serverDataString) ?? new Dictionary<string, Dictionary<string, int>>();
			Dictionary<string, Dictionary<string, int>> strs4 = new Dictionary<string, Dictionary<string, int>>();
			IEnumerator<string> enumerator = strs2.Keys.Concat<string>(strs3.Keys).Distinct<string>().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					strs4.Add(enumerator.Current, new Dictionary<string, int>());
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			foreach (KeyValuePair<string, Dictionary<string, int>> str in strs4)
			{
				if (strs2.TryGetValue(str.Key, out strs))
				{
					foreach (KeyValuePair<string, int> keyValuePair in strs)
					{
						str.Value.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
				if (!strs3.TryGetValue(str.Key, out strs1))
				{
					continue;
				}
				foreach (KeyValuePair<string, int> keyValuePair1 in strs1)
				{
					if (!str.Value.TryGetValue(keyValuePair1.Key, out num))
					{
						str.Value.Add(keyValuePair1.Key, keyValuePair1.Value);
					}
					else
					{
						str.Value[keyValuePair1.Key] = Math.Max(num, keyValuePair1.Value);
					}
				}
			}
			return CampaignProgress.SerializeProgress(strs4);
		}
	}
}