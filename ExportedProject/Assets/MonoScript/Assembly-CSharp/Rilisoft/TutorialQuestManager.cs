using Rilisoft.DictionaryExtensions;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class TutorialQuestManager
	{
		private const string Key = "TutorialQuestManager";

		private readonly static Lazy<TutorialQuestManager> _instance;

		private bool _dirty;

		private readonly HashSet<string> _fulfilledQuests;

		private bool _received;

		public static TutorialQuestManager Instance
		{
			get
			{
				return TutorialQuestManager._instance.Value;
			}
		}

		public bool Received
		{
			get
			{
				return this._received;
			}
		}

		static TutorialQuestManager()
		{
			TutorialQuestManager._instance = new Lazy<TutorialQuestManager>(new Func<TutorialQuestManager>(TutorialQuestManager.Create));
		}

		private TutorialQuestManager()
		{
			this._fulfilledQuests = new HashSet<string>();
		}

		private TutorialQuestManager(TutorialQuestManager.Memento dto)
		{
			Debug.Log("> TutorialQuestManager.TutorialQuestManager()");
			try
			{
				try
				{
					this._fulfilledQuests = (dto.fulfilledQuests == null ? new HashSet<string>() : new HashSet<string>(dto.fulfilledQuests));
					this._received = dto.received;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Debug.LogWarningFormat("TutorialQuestManager.TutorialQuestManager(): Exception caught: {0}", new object[] { exception.GetType().Name });
					Debug.LogException(exception);
				}
			}
			finally
			{
				Debug.Log("< TutorialQuestManager.TutorialQuestManager()");
			}
		}

		public void AddFulfilledQuest(string questId)
		{
			if (questId == null)
			{
				return;
			}
			this._dirty = this._fulfilledQuests.Add(questId);
		}

		public bool CheckQuestIfFulfilled(string questId)
		{
			int num;
			if (questId == null)
			{
				return false;
			}
			if (this._fulfilledQuests.Contains(questId))
			{
				return true;
			}
			if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) == 1)
			{
				return true;
			}
			string str = questId;
			if (str != null)
			{
				if (TutorialQuestManager.u003cu003ef__switchu0024mapF == null)
				{
					Dictionary<string, int> strs = new Dictionary<string, int>(3)
					{
						{ "loginFacebook", 0 },
						{ "loginTwitter", 1 },
						{ "likeFacebook", 2 }
					};
					TutorialQuestManager.u003cu003ef__switchu0024mapF = strs;
				}
				if (TutorialQuestManager.u003cu003ef__switchu0024mapF.TryGetValue(str, out num))
				{
					switch (num)
					{
						case 0:
						{
							if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
							{
								return true;
							}
							return (!Storager.hasKey(Defs.IsFacebookLoginRewardaGained) ? false : Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1);
						}
						case 1:
						{
							if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
							{
								return true;
							}
							return Application.isEditor;
						}
						case 2:
						{
							if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
							{
								return true;
							}
							return Application.isEditor;
						}
					}
				}
			}
			return false;
		}

		private static TutorialQuestManager Create()
		{
			object obj;
			object obj1;
			TutorialQuestManager tutorialQuestManager;
			try
			{
				if (!Storager.hasKey("TutorialQuestManager"))
				{
					Storager.setString("TutorialQuestManager", "{}", false);
				}
				string str = Storager.getString("TutorialQuestManager", false);
				if (string.IsNullOrEmpty(str))
				{
					str = "{}";
				}
				Debug.LogFormat("TutorialQuestManager.Create(): parsing data transfer object: {0}", new object[] { str });
				TutorialQuestManager.Memento flag = new TutorialQuestManager.Memento();
				TutorialQuestManager.Memento strs = flag;
				strs.fulfilledQuests = new List<string>();
				flag = strs;
				Dictionary<string, object> strs1 = Json.Deserialize(str) as Dictionary<string, object>;
				if (strs1 != null)
				{
					if (strs1.TryGetValue("fulfilledQuests", out obj))
					{
						List<object> objs = obj as List<object>;
						flag.fulfilledQuests = (objs == null ? new List<string>() : objs.OfType<string>().ToList<string>());
					}
					if (strs1.TryGetValue("received", out obj1))
					{
						flag.received = Convert.ToBoolean(obj1);
					}
				}
				Debug.Log("TutorialQuestManager.Create(): data transfer object parsed.");
				tutorialQuestManager = new TutorialQuestManager(flag);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Debug.LogWarningFormat("TutorialQuestManager.Create(): Exception caught: {0}", new object[] { exception.GetType().Name });
				Debug.LogException(exception);
				tutorialQuestManager = new TutorialQuestManager();
			}
			return tutorialQuestManager;
		}

		private static QuestBase CreateQuestFromJson(Dictionary<string, object> questJson, long day)
		{
			object obj;
			QuestBase questBase;
			if (questJson == null)
			{
				throw new ArgumentNullException("questJson");
			}
			try
			{
				string str = questJson.TryGet("id") as string;
				if (str != null)
				{
					int num = Convert.ToInt32(questJson.TryGet("slot") ?? 0);
					Difficulty[] difficultyArray = new Difficulty[] { Difficulty.Easy, Difficulty.Normal, Difficulty.Hard };
					Difficulty difficulty = Difficulty.None;
					Dictionary<string, object> strs = null;
					Difficulty[] difficultyArray1 = difficultyArray;
					int num1 = 0;
					while (num1 < (int)difficultyArray1.Length)
					{
						Difficulty difficulty1 = difficultyArray1[num1];
						if (!questJson.TryGetValue(QuestConstants.GetDifficultyKey(difficulty1), out obj))
						{
							num1++;
						}
						else
						{
							difficulty = difficulty1;
							strs = obj as Dictionary<string, object>;
							break;
						}
					}
					if (strs != null)
					{
						Reward reward = Reward.Create(strs.TryGet("reward") as List<object>);
						int num2 = Convert.ToInt32(strs.TryGet("parameter") ?? 1);
						int num3 = questJson.TryGet("currentCount").Map<object, int>(new Func<object, int>(Convert.ToInt32));
						day = questJson.TryGet("day").Map<object, long>(new Func<object, long>(Convert.ToInt64), day);
						bool flag = questJson.TryGet("rewarded").Map<object, bool>(new Func<object, bool>(Convert.ToBoolean));
						bool flag1 = questJson.TryGet("active").Map<object, bool>(new Func<object, bool>(Convert.ToBoolean));
						SimpleAccumulativeQuest simpleAccumulativeQuest = new SimpleAccumulativeQuest(str, day, num, difficulty, reward, flag1, flag, num2, num3);
						questBase = simpleAccumulativeQuest;
					}
					else
					{
						Debug.LogWarningFormat("Failed to create quest, difficulty = null: {0}", new object[] { Json.Serialize(questJson) });
						questBase = null;
					}
				}
				else
				{
					Debug.LogWarningFormat("Failed to create quest, id = null: {0}", new object[] { Json.Serialize(questJson) });
					questBase = null;
				}
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception while creating quest object: {0}", new object[] { exception.Message });
				questBase = null;
			}
			return questBase;
		}

		public void FillTutorialQuests(IList<object> inputJsons, long day, IList<QuestBase> outputQuests)
		{
			if (inputJsons == null)
			{
				return;
			}
			if (outputQuests == null)
			{
				return;
			}
			IEnumerator<object> enumerator = inputJsons.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (current != null)
					{
						Dictionary<string, object> strs = current as Dictionary<string, object>;
						if (strs != null)
						{
							QuestBase questBase = TutorialQuestManager.CreateQuestFromJson(strs, day);
							if (questBase == null)
							{
								continue;
							}
							if (!questBase.Rewarded)
							{
								if (!this.CheckQuestIfFulfilled(questBase.Id) || !(questBase.CalculateProgress() < new decimal(1)))
								{
									outputQuests.Add(questBase);
								}
							}
						}
						else
						{
							Debug.LogWarningFormat("Skipping bad quest: {0}", new object[] { Json.Serialize(current) });
						}
					}
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

		public void SaveIfDirty()
		{
			if (!this._dirty)
			{
				return;
			}
			Storager.setString("TutorialQuestManager", this.ToString(), false);
			this._dirty = false;
		}

		public void SetReceived()
		{
			this._received = true;
			this._dirty = true;
		}

		public override string ToString()
		{
			TutorialQuestManager.Memento memento = new TutorialQuestManager.Memento();
			TutorialQuestManager.Memento list = memento;
			list.fulfilledQuests = this._fulfilledQuests.ToList<string>();
			list.received = this._received;
			memento = list;
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "fulfilledQuests", this._fulfilledQuests.ToList<string>() },
				{ "received", Convert.ToBoolean(this._received) }
			};
			return Json.Serialize(strs);
		}

		[Serializable]
		internal struct Memento
		{
			public List<string> fulfilledQuests;

			public bool received;
		}
	}
}