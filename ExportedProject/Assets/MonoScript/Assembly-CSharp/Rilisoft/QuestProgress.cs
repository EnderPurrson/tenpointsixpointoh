using Rilisoft.DictionaryExtensions;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public sealed class QuestProgress : IDisposable
	{
		private const bool TutorialQuestsSupported = true;

		private bool _disposed;

		private static WeakReference _supportedMapsCache;

		private static WeakReference _supportedModesCache;

		private readonly IDictionary<int, List<QuestBase>> _currentQuests = new Dictionary<int, List<QuestBase>>(3);

		private readonly IDictionary<int, List<QuestBase>> _previousQuests = new Dictionary<int, List<QuestBase>>(3);

		private readonly List<QuestBase> _tutorialQuests = new List<QuestBase>();

		private readonly QuestEvents _events;

		private readonly string _configVersion;

		private readonly DateTime _timestamp;

		private readonly float _timeLeftSeconds;

		private bool _dirty;

		private long _day;

		private EventHandler<QuestCompletedEventArgs> QuestCompleted;

		public bool AnyActiveQuest
		{
			get
			{
				return this.GetActiveQuests().Any<KeyValuePair<int, QuestBase>>((KeyValuePair<int, QuestBase> q) => !q.Value.Rewarded);
			}
		}

		public string ConfigVersion
		{
			get
			{
				return this._configVersion;
			}
		}

		public int Count
		{
			get
			{
				return this._currentQuests.Count + this._previousQuests.Count;
			}
		}

		public long Day
		{
			get
			{
				return this._day;
			}
		}

		public bool Disposed
		{
			get
			{
				return this._disposed;
			}
		}

		public float TimeLeftSeconds
		{
			get
			{
				return this._timeLeftSeconds;
			}
		}

		public DateTime Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		public QuestProgress(string configVersion, long day, DateTime timestamp, float timeLeftSeconds, QuestProgress oldQuestProgress = null)
		{
			if (string.IsNullOrEmpty(configVersion))
			{
				throw new ArgumentException("ConfigId should not be empty.", "configVersion");
			}
			this._events = QuestMediator.Events;
			this._events.Win += new EventHandler<WinEventArgs>(this.HandleWin);
			this._events.KillOtherPlayer += new EventHandler<KillOtherPlayerEventArgs>(this.HandleKillOtherPlayer);
			this._events.KillOtherPlayerWithFlag += new EventHandler(this.HandleKillOtherPlayerWithFlag);
			this._events.Capture += new EventHandler<CaptureEventArgs>(this.HandleCapture);
			this._events.KillMonster += new EventHandler<KillMonsterEventArgs>(this.HandleKillMonster);
			this._events.BreakSeries += new EventHandler(this.HandleBreakSeries);
			this._events.MakeSeries += new EventHandler(this.HandleMakeSeries);
			this._events.SurviveWaveInArena += new EventHandler(this.HandleSurviveInArena);
			this._events.GetGotcha += new EventHandler(this.HandleGetGotcha);
			this._events.SocialInteraction += new EventHandler<SocialInteractionEventArgs>(this.HandleSocialInteraction);
			this._configVersion = configVersion;
			this._timestamp = timestamp;
			this._timeLeftSeconds = timeLeftSeconds;
			this._day = day;
			if (oldQuestProgress != null)
			{
				this._tutorialQuests = oldQuestProgress._tutorialQuests;
				foreach (QuestBase _tutorialQuest in this._tutorialQuests)
				{
					_tutorialQuest.Changed += new EventHandler(this.OnQuestChangedCheckCompletion);
				}
			}
			UnityEngine.Random.seed = (int)Tools.CurrentUnixTime;
		}

		private void ClearQuests(IDictionary<int, List<QuestBase>> quests)
		{
			if (quests == null)
			{
				return;
			}
			IEnumerator<KeyValuePair<int, List<QuestBase>>> enumerator = quests.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Value.Clear();
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			quests.Clear();
		}

		public static IDictionary<int, List<QuestBase>> CreateQuests(Dictionary<string, object> rawQuests, long day, Difficulty[] allowedDifficulties)
		{
			if (allowedDifficulties == null)
			{
				allowedDifficulties = new Difficulty[] { Difficulty.Easy, Difficulty.Normal, Difficulty.Hard };
			}
			return QuestProgress.ParseQuests(rawQuests, new long?(day), allowedDifficulties);
		}

		internal void DebugDecrementDay()
		{
			long num = this._day - (long)172800;
			IEnumerable<QuestBase> questBases = 
				from q in this._previousQuests.Values.SelectMany<List<QuestBase>, QuestBase>((List<QuestBase> q) => q).Concat<QuestBase>(this._currentQuests.Values.SelectMany<List<QuestBase>, QuestBase>((List<QuestBase> q) => q)).Concat<QuestBase>(this._tutorialQuests)
				where num < q.Day
				select q;
			IEnumerator<QuestBase> enumerator = questBases.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.DebugSetDay(num);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			this._day = num;
			this._dirty = true;
		}

		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			foreach (QuestBase _tutorialQuest in this._tutorialQuests)
			{
				_tutorialQuest.Changed -= new EventHandler(this.OnQuestChangedCheckCompletion);
			}
			IEnumerator<QuestBase> enumerator = this._currentQuests.SelectMany<KeyValuePair<int, List<QuestBase>>, QuestBase>((KeyValuePair<int, List<QuestBase>> kv) => kv.Value).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Changed -= new EventHandler(this.OnQuestChangedCheckCompletion);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			IEnumerator<QuestBase> enumerator1 = this._previousQuests.SelectMany<KeyValuePair<int, List<QuestBase>>, QuestBase>((KeyValuePair<int, List<QuestBase>> kv) => kv.Value).GetEnumerator();
			try
			{
				while (enumerator1.MoveNext())
				{
					enumerator1.Current.Changed -= new EventHandler(this.OnQuestChangedCheckCompletion);
				}
			}
			finally
			{
				if (enumerator1 == null)
				{
				}
				enumerator1.Dispose();
			}
			this._events.Win -= new EventHandler<WinEventArgs>(this.HandleWin);
			this._events.KillOtherPlayer -= new EventHandler<KillOtherPlayerEventArgs>(this.HandleKillOtherPlayer);
			this._events.KillOtherPlayerWithFlag -= new EventHandler(this.HandleKillOtherPlayerWithFlag);
			this._events.Capture -= new EventHandler<CaptureEventArgs>(this.HandleCapture);
			this._events.KillMonster -= new EventHandler<KillMonsterEventArgs>(this.HandleKillMonster);
			this._events.BreakSeries -= new EventHandler(this.HandleBreakSeries);
			this._events.MakeSeries -= new EventHandler(this.HandleMakeSeries);
			this._events.SurviveWaveInArena -= new EventHandler(this.HandleSurviveInArena);
			this.QuestCompleted = null;
			this._disposed = true;
		}

		private static string ExtractMapFromQuestDescription(Dictionary<string, object> q, bool restore)
		{
			object obj;
			object obj1;
			if (q == null || q.Count == 0)
			{
				return string.Empty;
			}
			if (restore)
			{
				if (!q.TryGetValue("map", out obj))
				{
					return null;
				}
				return Convert.ToString(obj);
			}
			string[] supportedMaps = QuestProgress.GetSupportedMaps();
			if (!q.TryGetValue("maps", out obj1))
			{
				return supportedMaps[UnityEngine.Random.Range(0, (int)supportedMaps.Length - 1)];
			}
			List<object> objs = obj1 as List<object>;
			if (objs == null)
			{
				return string.Empty;
			}
			string[] array = objs.OfType<string>().Intersect<string>(supportedMaps).ToArray<string>();
			if ((int)array.Length == 0)
			{
				return string.Empty;
			}
			return array[UnityEngine.Random.Range(0, (int)array.Length - 1)];
		}

		private static ConnectSceneNGUIController.RegimGame? ExtractModeFromQuestDescription(Dictionary<string, object> q, bool restore, string questId)
		{
			object obj;
			object obj1;
			ConnectSceneNGUIController.RegimGame? nullable;
			if (q == null || q.Count == 0)
			{
				return null;
			}
			if (restore)
			{
				if (!q.TryGetValue("mode", out obj))
				{
					return null;
				}
				return QuestConstants.ParseMode(Convert.ToString(obj));
			}
			List<ConnectSceneNGUIController.RegimGame> regimGames = new List<ConnectSceneNGUIController.RegimGame>(QuestProgress.GetSupportedModes());
			if ("killInMode".Equals(questId, StringComparison.OrdinalIgnoreCase))
			{
				regimGames.Remove(ConnectSceneNGUIController.RegimGame.TimeBattle);
			}
			if (!q.TryGetValue("modes", out obj1))
			{
				if (regimGames.Count <= 0)
				{
					nullable = null;
				}
				else
				{
					nullable = new ConnectSceneNGUIController.RegimGame?(regimGames[UnityEngine.Random.Range(0, regimGames.Count - 1)]);
				}
				return nullable;
			}
			List<object> objs = obj1 as List<object>;
			if (objs == null)
			{
				return null;
			}
			List<ConnectSceneNGUIController.RegimGame> list = objs.OfType<string>().Select<string, ConnectSceneNGUIController.RegimGame?>(new Func<string, ConnectSceneNGUIController.RegimGame?>(QuestConstants.ParseMode)).Where<ConnectSceneNGUIController.RegimGame?>((ConnectSceneNGUIController.RegimGame? m) => m.HasValue).Select<ConnectSceneNGUIController.RegimGame?, ConnectSceneNGUIController.RegimGame>((ConnectSceneNGUIController.RegimGame? m) => m.Value).Intersect<ConnectSceneNGUIController.RegimGame>(regimGames).ToList<ConnectSceneNGUIController.RegimGame>();
			if (list.Count == 0)
			{
				return null;
			}
			return new ConnectSceneNGUIController.RegimGame?(list[UnityEngine.Random.Range(0, list.Count - 1)]);
		}

		private static IDictionary<int, List<Dictionary<string, object>>> ExtractQuests(Dictionary<string, object> rawQuests)
		{
			int num;
			Dictionary<int, List<Dictionary<string, object>>> nums = new Dictionary<int, List<Dictionary<string, object>>>();
			foreach (KeyValuePair<string, object> rawQuest in rawQuests)
			{
				if (int.TryParse(rawQuest.Key, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out num))
				{
					List<object> value = rawQuest.Value as List<object>;
					if (value != null)
					{
						List<Dictionary<string, object>> list = value.OfType<Dictionary<string, object>>().ToList<Dictionary<string, object>>();
						nums[num] = list;
					}
					else
					{
						nums[num] = new List<Dictionary<string, object>>();
					}
				}
			}
			return nums;
		}

		private static ShopNGUIController.CategoryNames? ExtractWeaponSlotFromQuestDescription(Dictionary<string, object> q, bool restore, HashSet<ShopNGUIController.CategoryNames> excluded)
		{
			object obj;
			object obj1;
			if (q == null || q.Count == 0)
			{
				return null;
			}
			if (restore)
			{
				if (!q.TryGetValue("weaponSlot", out obj))
				{
					return null;
				}
				return QuestConstants.ParseWeaponSlot(Convert.ToString(obj));
			}
			if (excluded == null)
			{
				excluded = new HashSet<ShopNGUIController.CategoryNames>();
			}
			List<ShopNGUIController.CategoryNames> list = Enum.GetValues(typeof(ShopNGUIController.CategoryNames)).Cast<ShopNGUIController.CategoryNames>().Where<ShopNGUIController.CategoryNames>(new Func<ShopNGUIController.CategoryNames, bool>(ShopNGUIController.IsWeaponCategory)).ToList<ShopNGUIController.CategoryNames>();
			if (!q.TryGetValue("weaponSlots", out obj1))
			{
				List<ShopNGUIController.CategoryNames> categoryNames = list.Except<ShopNGUIController.CategoryNames>(excluded).ToList<ShopNGUIController.CategoryNames>();
				categoryNames = (categoryNames.Count <= 0 ? list : categoryNames);
				return new ShopNGUIController.CategoryNames?(categoryNames[UnityEngine.Random.Range(0, categoryNames.Count - 1)]);
			}
			List<object> objs = obj1 as List<object>;
			if (objs == null)
			{
				return null;
			}
			List<ShopNGUIController.CategoryNames> list1 = objs.OfType<string>().Select<string, ShopNGUIController.CategoryNames?>(new Func<string, ShopNGUIController.CategoryNames?>(QuestConstants.ParseWeaponSlot)).Where<ShopNGUIController.CategoryNames?>((ShopNGUIController.CategoryNames? w) => w.HasValue).Select<ShopNGUIController.CategoryNames?, ShopNGUIController.CategoryNames>((ShopNGUIController.CategoryNames? w) => w.Value).Intersect<ShopNGUIController.CategoryNames>(list).ToList<ShopNGUIController.CategoryNames>();
			if (list1.Count == 0)
			{
				return null;
			}
			List<ShopNGUIController.CategoryNames> categoryNames1 = list1.Except<ShopNGUIController.CategoryNames>(excluded).ToList<ShopNGUIController.CategoryNames>();
			categoryNames1 = (categoryNames1.Count <= 0 ? list1 : categoryNames1);
			return new ShopNGUIController.CategoryNames?(categoryNames1[UnityEngine.Random.Range(0, categoryNames1.Count - 1)]);
		}

		public void FillTutorialQuests(List<object> questJsons)
		{
			if (questJsons == null)
			{
				return;
			}
			TutorialQuestManager.Instance.FillTutorialQuests(questJsons, this.Day, this._tutorialQuests);
			foreach (QuestBase _tutorialQuest in this._tutorialQuests)
			{
				_tutorialQuest.Changed -= new EventHandler(this.OnQuestChangedCheckCompletion);
				_tutorialQuest.Changed += new EventHandler(this.OnQuestChangedCheckCompletion);
			}
			this._dirty = true;
		}

		public void FilterFulfilledTutorialQuests()
		{
			this._tutorialQuests.RemoveAll((QuestBase tq) => (!TutorialQuestManager.Instance.CheckQuestIfFulfilled(tq.Id) ? false : tq.CalculateProgress() < new decimal(1)));
		}

		private static IDictionary<int, List<Dictionary<string, object>>> FilterQuests(Dictionary<string, object> rawQuests, Difficulty[] allowedDifficulties, IDictionary<int, List<QuestBase>> existingQuests)
		{
			object obj;
			Dictionary<string, Dictionary<string, object>> strs;
			List<QuestBase> questBases;
			if (allowedDifficulties == null)
			{
				throw new ArgumentNullException("allowedDifficulties");
			}
			if ((int)allowedDifficulties.Length == 0)
			{
				throw new ArgumentException("List of difficulties should not be empty.", "allowedDifficulties");
			}
			if (existingQuests == null)
			{
				existingQuests = new Dictionary<int, List<QuestBase>>();
			}
			Dictionary<int, Dictionary<string, Dictionary<string, object>>> nums = new Dictionary<int, Dictionary<string, Dictionary<string, object>>>();
			foreach (KeyValuePair<string, object> rawQuest in rawQuests)
			{
				Dictionary<string, object> value = rawQuest.Value as Dictionary<string, object>;
				if (value != null)
				{
					if (value.TryGetValue("slot", out obj))
					{
						if (QuestConstants.IsSupported(rawQuest.Key))
						{
							try
							{
								int num = Convert.ToInt32(obj, NumberFormatInfo.InvariantInfo);
								if (!nums.TryGetValue(num, out strs))
								{
									strs = new Dictionary<string, Dictionary<string, object>>(3);
									nums[num] = strs;
								}
								strs[rawQuest.Key] = value;
							}
							catch (Exception exception)
							{
								Debug.LogException(exception);
							}
						}
					}
				}
			}
			List<Difficulty> difficulties = new List<Difficulty>(nums.Count);
			for (int i = 0; i != nums.Count; i++)
			{
				difficulties.Add(allowedDifficulties[i % (int)allowedDifficulties.Length]);
			}
			QuestProgress.ShuffleInPlace<Difficulty>(difficulties);
			Dictionary<int, List<Dictionary<string, object>>> nums1 = new Dictionary<int, List<Dictionary<string, object>>>();
			Dictionary<int, Dictionary<string, Dictionary<string, object>>>.Enumerator enumerator = nums.GetEnumerator();
			List<Difficulty> difficulties1 = new List<Difficulty>()
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			int num1 = 0;
			while (enumerator.MoveNext())
			{
				int key = enumerator.Current.Key;
				Dictionary<string, Dictionary<string, object>> value1 = enumerator.Current.Value;
				existingQuests.TryGetValue(key, out questBases);
				Difficulty item = difficulties[num1];
				string difficultyKey = QuestConstants.GetDifficultyKey(item);
				List<KeyValuePair<string, Dictionary<string, object>>> list = (
					from kv in value1
					where kv.Value.ContainsKey(difficultyKey)
					select kv).ToList<KeyValuePair<string, Dictionary<string, object>>>();
				if (list.Count != 0)
				{
					if (list.Count > 1)
					{
						string str = questBases.Map<List<QuestBase>, QuestBase>((List<QuestBase> l) => l.FirstOrDefault<QuestBase>()).Map<QuestBase, string>((QuestBase q) => q.Id);
						list.RemoveAll((KeyValuePair<string, Dictionary<string, object>> kv) => StringComparer.OrdinalIgnoreCase.Equals(kv.Key, str));
					}
					List<int> list1 = Enumerable.Range(0, list.Count).ToList<int>();
					QuestProgress.ShuffleInPlace<int>(list1);
					KeyValuePair<string, Dictionary<string, object>> keyValuePair = list[list1[0]];
					keyValuePair.Value["id"] = keyValuePair.Key;
					value1.Clear();
					value1[keyValuePair.Key] = keyValuePair.Value;
					List<Dictionary<string, object>> dictionaries = new List<Dictionary<string, object>>(2)
					{
						keyValuePair.Value.ToDictionary<KeyValuePair<string, object>, string, object>((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value)
					};
					List<Dictionary<string, object>> dictionaries1 = dictionaries;
					if (questBases.Map<List<QuestBase>, bool>((List<QuestBase> l) => l.Count == 0, true))
					{
						KeyValuePair<string, Dictionary<string, object>> item1 = list[list1[list1.Count - 1]];
						item1.Value["id"] = item1.Key;
						dictionaries1.Add(item1.Value.ToDictionary<KeyValuePair<string, object>, string, object>((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value));
					}
					nums1[key] = dictionaries1;
					IEnumerable<Difficulty> difficulties2 = 
						from d in difficulties1
						where d != item
						select d;
					IEnumerator<Difficulty> enumerator1 = difficulties2.GetEnumerator();
					try
					{
						while (enumerator1.MoveNext())
						{
							string difficultyKey1 = QuestConstants.GetDifficultyKey(enumerator1.Current);
							List<Dictionary<string, object>>.Enumerator enumerator2 = dictionaries1.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									enumerator2.Current.Remove(difficultyKey1);
								}
							}
							finally
							{
								((IDisposable)(object)enumerator2).Dispose();
							}
						}
					}
					finally
					{
						if (enumerator1 == null)
						{
						}
						enumerator1.Dispose();
					}
				}
				else
				{
					value1.Clear();
				}
				num1++;
			}
			return nums1;
		}

		public QuestBase GetActiveQuestBySlot(int slot)
		{
			QuestBase activeTutorialQuest = this.GetActiveTutorialQuest();
			if (activeTutorialQuest != null && activeTutorialQuest.Slot == slot)
			{
				return activeTutorialQuest;
			}
			List<QuestBase> questBases = null;
			this._previousQuests.TryGetValue(slot, out questBases);
			QuestBase questBase = questBases.Map<List<QuestBase>, QuestBase>((List<QuestBase> ps) => ps.FirstOrDefault<QuestBase>());
			if (questBase != null && !questBase.Rewarded)
			{
				return questBase;
			}
			List<QuestBase> questBases1 = null;
			this._currentQuests.TryGetValue(slot, out questBases1);
			QuestBase questBase1 = questBases1.Map<List<QuestBase>, QuestBase>((List<QuestBase> cs) => cs.FirstOrDefault<QuestBase>());
			if (questBase1 != null)
			{
				return questBase1;
			}
			return questBase;
		}

		public QuestInfo GetActiveQuestInfoBySlot(int slot)
		{
			Func<QuestBase, int> func = null;
			IList<QuestBase> activeQuestsBySlot = this.GetActiveQuestsBySlot(slot, false);
			Func<IList<QuestBase>> func1 = () => {
				List<QuestBase> questBases;
				IEnumerable<int> nums = this._currentQuests.Keys.Concat<int>(this._previousQuests.Keys);
				List<QuestBase> u003cu003ef_this = this._tutorialQuests;
				if (func == null)
				{
					func = (QuestBase q) => q.Slot;
				}
				IEnumerator<int> enumerator = nums.Concat<int>(u003cu003ef_this.Select<QuestBase, int>(func)).Distinct<int>().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.Current;
						List<QuestBase> activeQuestsBySlotReference = this.GetActiveQuestsBySlotReference(current, false);
						if (activeQuestsBySlotReference != null && activeQuestsBySlotReference.Count > 0 && slot == current)
						{
							activeQuestsBySlotReference.RemoveAt(0);
							this._dirty = true;
						}
						List<QuestBase> activeQuestsBySlotReference1 = this.GetActiveQuestsBySlotReference(current, true);
						if (activeQuestsBySlotReference1.Count <= 1)
						{
							continue;
						}
						activeQuestsBySlotReference1.RemoveRange(1, activeQuestsBySlotReference1.Count - 1);
						if (activeQuestsBySlotReference1[0].CalculateProgress() >= new decimal(1) && this._currentQuests.TryGetValue(current, out questBases) && questBases.Count > 1)
						{
							questBases.RemoveRange(1, questBases.Count - 1);
						}
						this._dirty = true;
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				return this.GetActiveQuestsBySlot(slot, false);
			};
			bool flag = object.ReferenceEquals(this._tutorialQuests, this.GetActiveQuestsBySlotReference(slot, false));
			return new QuestInfo(activeQuestsBySlot, func1, flag);
		}

		public IDictionary<int, QuestBase> GetActiveQuests()
		{
			IEnumerable<int> nums = this._previousQuests.Keys.Concat<int>(this._currentQuests.Keys).Concat<int>(
				from q in this._tutorialQuests
				select q.Slot).Distinct<int>();
			Dictionary<int, QuestBase> dictionary = nums.ToDictionary<int, int, QuestBase>((int s) => s, new Func<int, QuestBase>(this.GetActiveQuestBySlot));
			return dictionary;
		}

		private IList<QuestBase> GetActiveQuestsBySlot(int slot, bool ignoreTutorialQuests = false)
		{
			List<QuestBase> activeQuestsBySlotReference = this.GetActiveQuestsBySlotReference(slot, ignoreTutorialQuests);
			if (activeQuestsBySlotReference == null)
			{
				return new List<QuestBase>();
			}
			return new List<QuestBase>(activeQuestsBySlotReference);
		}

		private List<QuestBase> GetActiveQuestsBySlotReference(int slot, bool ignoreTutorialQuests = false)
		{
			List<QuestBase> questBases;
			List<QuestBase> questBases1;
			if (!ignoreTutorialQuests)
			{
				QuestBase activeTutorialQuest = this.GetActiveTutorialQuest();
				if (activeTutorialQuest != null && activeTutorialQuest.Slot == slot)
				{
					return this._tutorialQuests;
				}
			}
			this._previousQuests.TryGetValue(slot, out questBases);
			if (questBases.Map<List<QuestBase>, bool>((List<QuestBase> qs) => (qs.Count <= 0 ? false : qs.All<QuestBase>((QuestBase q) => !q.Rewarded))))
			{
				return questBases;
			}
			if (this._currentQuests.TryGetValue(slot, out questBases1))
			{
				if (questBases1.Map<List<QuestBase>, bool>((List<QuestBase> qs) => qs.Count > 0))
				{
					return questBases1;
				}
			}
			return questBases;
		}

		private QuestBase GetActiveTutorialQuest()
		{
			QuestBase questBase;
			if (this._tutorialQuests.Count == 0)
			{
				return null;
			}
			List<QuestBase>.Enumerator enumerator = this._tutorialQuests.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					QuestBase current = enumerator.Current;
					if (!current.Rewarded)
					{
						questBase = current;
						return questBase;
					}
				}
				return null;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return questBase;
		}

		private QuestBase GetQuestById(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			IEnumerable<int> nums = this._previousQuests.Keys.Concat<int>(this._currentQuests.Keys).Concat<int>(
				from q in this._tutorialQuests
				select q.Slot).Distinct<int>();
			IEnumerable<QuestBase> questBases = nums.Select<int, QuestBase>(new Func<int, QuestBase>(this.GetActiveQuestBySlot));
			return questBases.FirstOrDefault<QuestBase>((QuestBase q) => q.Id.Equals(id, StringComparison.Ordinal));
		}

		[Obsolete]
		public AccumulativeQuestBase GetRandomInProgressAccumQuest()
		{
			List<AccumulativeQuestBase> list = (
				from qs in this._previousQuests.Values
				where qs.Count > 0
				select qs.First<QuestBase>() into q
				where q.CalculateProgress() < new decimal(1)
				select q).OfType<AccumulativeQuestBase>().ToList<AccumulativeQuestBase>();
			if (list.Count > 0)
			{
				return list[UnityEngine.Random.Range(0, list.Count)];
			}
			List<AccumulativeQuestBase> accumulativeQuestBases = (
				from qs in this._previousQuests.Values
				where qs.Count > 0
				select qs.First<QuestBase>() into q
				where !q.Rewarded
				select q).OfType<AccumulativeQuestBase>().ToList<AccumulativeQuestBase>();
			if (accumulativeQuestBases.Count > 0)
			{
				return accumulativeQuestBases[UnityEngine.Random.Range(0, accumulativeQuestBases.Count)];
			}
			AccumulativeQuestBase[] array = (
				from qs in this._currentQuests.Values
				where qs.Count > 0
				select qs.First<QuestBase>() into q
				where q.CalculateProgress() < new decimal(1)
				select q).OfType<AccumulativeQuestBase>().ToArray<AccumulativeQuestBase>();
			if ((int)array.Length > 0)
			{
				return array[UnityEngine.Random.Range(0, (int)array.Length)];
			}
			AccumulativeQuestBase[] accumulativeQuestBaseArray = (
				from qs in this._currentQuests.Values
				where qs.Count > 0
				select qs.First<QuestBase>() into q
				where !q.Rewarded
				select q).OfType<AccumulativeQuestBase>().ToArray<AccumulativeQuestBase>();
			if ((int)accumulativeQuestBaseArray.Length <= 0)
			{
				return null;
			}
			return accumulativeQuestBaseArray[UnityEngine.Random.Range(0, (int)accumulativeQuestBaseArray.Length)];
		}

		public QuestInfo GetRandomQuestInfo()
		{
			IEnumerable<int> nums = this._previousQuests.Keys.Concat<int>(this._currentQuests.Keys).Concat<int>(
				from q in this._tutorialQuests
				select q.Slot).Distinct<int>();
			List<QuestInfo> list = nums.Select<int, QuestInfo>(new Func<int, QuestInfo>(this.GetActiveQuestInfoBySlot)).Where<QuestInfo>((QuestInfo qi) => (qi.Quest == null ? false : !qi.Quest.Rewarded)).ToList<QuestInfo>();
			if (list.Count < 1)
			{
				return null;
			}
			QuestInfo item = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				QuestInfo questInfo = list[i];
				if (questInfo.Quest != null)
				{
					if (item.Quest != null)
					{
						AccumulativeQuestBase quest = item.Quest as AccumulativeQuestBase;
						AccumulativeQuestBase accumulativeQuestBase = questInfo.Quest as AccumulativeQuestBase;
						if (quest != null && accumulativeQuestBase != null)
						{
							if (accumulativeQuestBase.RequiredCount - accumulativeQuestBase.CurrentCount < quest.RequiredCount - quest.CurrentCount)
							{
								item = questInfo;
							}
						}
						else if (item.Quest.CalculateProgress() < questInfo.Quest.CalculateProgress())
						{
							item = questInfo;
						}
					}
					else
					{
						item = list[i];
					}
				}
			}
			return item;
		}

		private static string[] GetSupportedMaps()
		{
			if (QuestProgress._supportedMapsCache != null && QuestProgress._supportedMapsCache.IsAlive)
			{
				return (string[])QuestProgress._supportedMapsCache.Target;
			}
			int num = ExperienceController.sharedController.Map<ExperienceController, int>((ExperienceController xp) => xp.currentLevel, 1);
			HashSet<TypeModeGame> unlockedModesByLevel = SceneInfoController.GetUnlockedModesByLevel(num);
			unlockedModesByLevel.Remove(TypeModeGame.Dater);
			HashSet<string> strs = new HashSet<string>();
			foreach (SceneInfo allScene in SceneInfoController.instance.allScenes)
			{
				if (!allScene.isPremium)
				{
					if (allScene.NameScene != "Developer_Scene")
					{
						foreach (TypeModeGame typeModeGame in unlockedModesByLevel)
						{
							if (!allScene.IsAvaliableForMode(typeModeGame))
							{
								continue;
							}
							strs.Add(allScene.NameScene);
						}
					}
				}
			}
			string[] array = strs.ToArray<string>();
			QuestProgress._supportedMapsCache = new WeakReference(array, false);
			return array;
		}

		private static ConnectSceneNGUIController.RegimGame[] GetSupportedModes()
		{
			if (QuestProgress._supportedModesCache != null && QuestProgress._supportedModesCache.IsAlive)
			{
				return (ConnectSceneNGUIController.RegimGame[])QuestProgress._supportedModesCache.Target;
			}
			int num = ExperienceController.sharedController.Map<ExperienceController, int>((ExperienceController xp) => xp.currentLevel, 1);
			HashSet<TypeModeGame> unlockedModesByLevel = SceneInfoController.GetUnlockedModesByLevel(num);
			ConnectSceneNGUIController.RegimGame[] array = SceneInfoController.SelectModes(unlockedModesByLevel).ToArray<ConnectSceneNGUIController.RegimGame>();
			QuestProgress._supportedModesCache = new WeakReference(array, false);
			return array;
		}

		private QuestBase GetTutorialQuestById(string id)
		{
			if (id == null)
			{
				return null;
			}
			QuestBase activeTutorialQuest = this.GetActiveTutorialQuest();
			if (activeTutorialQuest == null)
			{
				return null;
			}
			if (!id.Equals(activeTutorialQuest.Id, StringComparison.Ordinal))
			{
				return null;
			}
			return activeTutorialQuest;
		}

		private void HandleBreakSeries(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleBreakSeries()");
			}
			QuestBase questById = this.GetQuestById("breakSeries");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do<SimpleAccumulativeQuest>((SimpleAccumulativeQuest quest) => quest.Increment(1));
			}
		}

		private void HandleCapture(object sender, CaptureEventArgs e)
		{
			SimpleAccumulativeQuest simpleAccumulativeQuest;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Concat("HandleCapture(): ", e));
			}
			string[] strArrays = new string[] { "captureFlags", "capturePoints" };
			Dictionary<string, SimpleAccumulativeQuest> dictionary = strArrays.Select<string, QuestBase>(new Func<string, QuestBase>(this.GetQuestById)).OfType<SimpleAccumulativeQuest>().ToDictionary<SimpleAccumulativeQuest, string, SimpleAccumulativeQuest>((SimpleAccumulativeQuest q) => q.Id, (SimpleAccumulativeQuest q) => q);
			if (dictionary.TryGetValue("capturePoints", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Mode == ConnectSceneNGUIController.RegimGame.CapturePoints, 1);
			}
			if (dictionary.TryGetValue("captureFlags", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Mode == ConnectSceneNGUIController.RegimGame.FlagCapture, 1);
			}
		}

		private void HandleGetGotcha(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleGetGotcha()");
			}
			QuestBase tutorialQuestById = this.GetTutorialQuestById("getGotcha");
			if (tutorialQuestById != null)
			{
				(tutorialQuestById as SimpleAccumulativeQuest).Do<SimpleAccumulativeQuest>((SimpleAccumulativeQuest quest) => quest.Increment(1));
			}
		}

		private void HandleKillMonster(object sender, KillMonsterEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Concat("HandleKillMonster(): ", e));
			}
			QuestBase questById = this.GetQuestById("killInCampaign");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do<SimpleAccumulativeQuest>((SimpleAccumulativeQuest quest) => quest.IncrementIf(e.Campaign, 1));
			}
			questById = this.GetQuestById("killNpcWithWeapon");
			if (questById != null)
			{
				(questById as WeaponSlotAccumulativeQuest).Do<WeaponSlotAccumulativeQuest>((WeaponSlotAccumulativeQuest quest) => quest.IncrementIf(e.WeaponSlot == quest.WeaponSlot, 1));
			}
		}

		private void HandleKillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			SimpleAccumulativeQuest simpleAccumulativeQuest;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Concat("HandleKillOtherPlayer(): ", e));
			}
			QuestBase questById = this.GetQuestById("killInMode");
			if (questById != null)
			{
				(questById as ModeAccumulativeQuest).Do<ModeAccumulativeQuest>((ModeAccumulativeQuest quest) => quest.IncrementIf(e.Mode == quest.Mode, 1));
			}
			questById = this.GetQuestById("killWithWeapon");
			if (questById != null)
			{
				(questById as WeaponSlotAccumulativeQuest).Do<WeaponSlotAccumulativeQuest>((WeaponSlotAccumulativeQuest quest) => quest.IncrementIf(e.WeaponSlot == quest.WeaponSlot, 1));
			}
			string[] strArrays = new string[] { "killViaHeadshot", "killWithGrenade", "revenge" };
			Dictionary<string, SimpleAccumulativeQuest> dictionary = strArrays.Select<string, QuestBase>(new Func<string, QuestBase>(this.GetQuestById)).OfType<SimpleAccumulativeQuest>().ToDictionary<SimpleAccumulativeQuest, string, SimpleAccumulativeQuest>((SimpleAccumulativeQuest q) => q.Id, (SimpleAccumulativeQuest q) => q);
			if (dictionary.TryGetValue("killViaHeadshot", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Headshot, 1);
			}
			if (dictionary.TryGetValue("killWithGrenade", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Grenade, 1);
			}
			if (dictionary.TryGetValue("revenge", out simpleAccumulativeQuest))
			{
				simpleAccumulativeQuest.IncrementIf(e.Revenge, 1);
			}
		}

		private void HandleKillOtherPlayerWithFlag(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Concat("HandleKillOtherPlayerWithFlag(): ", e));
			}
			QuestBase questById = this.GetQuestById("killFlagCarriers");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do<SimpleAccumulativeQuest>((SimpleAccumulativeQuest quest) => quest.Increment(1));
			}
		}

		private void HandleMakeSeries(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleMakeSeries()");
			}
			QuestBase questById = this.GetQuestById("makeSeries");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do<SimpleAccumulativeQuest>((SimpleAccumulativeQuest quest) => quest.Increment(1));
			}
		}

		private void HandleSocialInteraction(object sender, SocialInteractionEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("HandleSocialInteraction('{0}')", new object[] { e.Kind });
			}
			QuestBase tutorialQuestById = this.GetTutorialQuestById(e.Kind);
			if (tutorialQuestById != null)
			{
				(tutorialQuestById as SimpleAccumulativeQuest).Do<SimpleAccumulativeQuest>((SimpleAccumulativeQuest quest) => quest.Increment(1));
			}
		}

		private void HandleSurviveInArena(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleSurviveInArena()");
			}
			QuestBase questById = this.GetQuestById("surviveWavesInArena");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do<SimpleAccumulativeQuest>((SimpleAccumulativeQuest quest) => quest.Increment(1));
			}
		}

		private void HandleWin(object sender, WinEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Concat("HandleWin(): ", e));
			}
			QuestBase questById = this.GetQuestById("winInMap");
			if (questById != null)
			{
				MapAccumulativeQuest mapAccumulativeQuest = questById as MapAccumulativeQuest;
				if (mapAccumulativeQuest != null)
				{
					mapAccumulativeQuest.IncrementIf(mapAccumulativeQuest.Map.Equals(e.Map, StringComparison.Ordinal), 1);
				}
			}
			questById = this.GetQuestById("winInMode");
			if (questById != null)
			{
				ModeAccumulativeQuest modeAccumulativeQuest = questById as ModeAccumulativeQuest;
				if (modeAccumulativeQuest != null)
				{
					modeAccumulativeQuest.IncrementIf(modeAccumulativeQuest.Mode == e.Mode, 1);
				}
			}
		}

		public bool HasUnrewaredAccumQuests()
		{
			IEnumerable<QuestBase> values = 
				from qs in this._currentQuests.Values
				where qs.Count > 0
				select qs.First<QuestBase>();
			IEnumerable<QuestBase> questBases = 
				from qs in this._previousQuests.Values
				where qs.Count > 0
				select qs.First<QuestBase>();
			IEnumerable<AccumulativeQuestBase> accumulativeQuestBases = values.Concat<QuestBase>(questBases).Concat<QuestBase>(this._tutorialQuests).OfType<AccumulativeQuestBase>();
			return accumulativeQuestBases.Any<AccumulativeQuestBase>((AccumulativeQuestBase q) => (q.CalculateProgress() < new decimal(1) ? false : !q.Rewarded));
		}

		private static HashSet<ShopNGUIController.CategoryNames> InitializeExcludedWeaponSlots(int slot)
		{
			HashSet<ShopNGUIController.CategoryNames> categoryNames = new HashSet<ShopNGUIController.CategoryNames>();
			if (QuestSystem.Instance == null || QuestSystem.Instance.QuestProgress == null)
			{
				return categoryNames;
			}
			WeaponSlotAccumulativeQuest activeQuestBySlot = QuestSystem.Instance.QuestProgress.GetActiveQuestBySlot(slot) as WeaponSlotAccumulativeQuest;
			if (activeQuestBySlot != null)
			{
				categoryNames.Add(activeQuestBySlot.WeaponSlot);
			}
			return categoryNames;
		}

		public bool IsDirty()
		{
			bool flag;
			if (!this._dirty)
			{
				if (!this._currentQuests.Values.SelectMany<List<QuestBase>, QuestBase>((List<QuestBase> q) => q).Any<QuestBase>((QuestBase q) => q.Dirty))
				{
					if (this._previousQuests.Values.SelectMany<List<QuestBase>, QuestBase>((List<QuestBase> q) => q).Any<QuestBase>((QuestBase q) => q.Dirty))
					{
						flag = true;
						return flag;
					}
					flag = this._tutorialQuests.Any<QuestBase>((QuestBase q) => q.Dirty);
					return flag;
				}
			}
			flag = true;
			return flag;
		}

		private void OnQuestChangedCheckCompletion(object sender, EventArgs e)
		{
			QuestBase questBase = sender as QuestBase;
			if (questBase == null)
			{
				return;
			}
			if (questBase.CalculateProgress() >= new decimal(1))
			{
				string str = string.Format(CultureInfo.InvariantCulture, "{0}.OnQuestChangedCheckCompletion({1})", new object[] { this.GetType().Name, questBase.Id });
				ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
				try
				{
					this.QuestCompleted.Do<EventHandler<QuestCompletedEventArgs>>((EventHandler<QuestCompletedEventArgs> handler) => handler(this, new QuestCompletedEventArgs()
					{
						Quest = questBase
					}));
				}
				finally
				{
					scopeLogger.Dispose();
				}
			}
		}

		private static IDictionary<int, List<QuestBase>> ParseQuests(Dictionary<string, object> rawQuests, long? dayOption, Difficulty[] allowedDifficulties)
		{
			Dictionary<int, List<QuestBase>> nums = new Dictionary<int, List<QuestBase>>(3);
			QuestProgress.ParseQuests(rawQuests, dayOption, allowedDifficulties, nums);
			return nums;
		}

		private static void ParseQuests(Dictionary<string, object> rawQuests, long? dayOption, Difficulty[] allowedDifficulties, IDictionary<int, List<QuestBase>> actualResult)
		{
			List<QuestBase> questBases;
			int num;
			if (actualResult == null)
			{
				return;
			}
			if (rawQuests == null || rawQuests.Count == 0)
			{
				return;
			}
			if (allowedDifficulties == null)
			{
				throw new ArgumentNullException("allowedDifficulties");
			}
			bool hasValue = !dayOption.HasValue;
			IDictionary<int, List<QuestBase>> nums = (QuestSystem.Instance.QuestProgress == null ? new Dictionary<int, List<QuestBase>>() : QuestSystem.Instance.QuestProgress.GetActiveQuests().ToDictionary<KeyValuePair<int, QuestBase>, int, List<QuestBase>>((KeyValuePair<int, QuestBase> kv) => kv.Key, (KeyValuePair<int, QuestBase> kv) => new List<QuestBase>()
			{
				kv.Value
			}));
			IDictionary<int, List<Dictionary<string, object>>> nums1 = (!hasValue ? QuestProgress.FilterQuests(rawQuests, allowedDifficulties, nums) : QuestProgress.ExtractQuests(rawQuests));
			Difficulty[] difficultyArray = new Difficulty[] { Difficulty.Easy, Difficulty.Normal, Difficulty.Hard };
			IEnumerator<KeyValuePair<int, List<Dictionary<string, object>>>> enumerator = nums1.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, List<Dictionary<string, object>>> current = enumerator.Current;
					int key = current.Key;
					if (!actualResult.TryGetValue(key, out questBases))
					{
						questBases = new List<QuestBase>(2);
					}
					HashSet<ShopNGUIController.CategoryNames> categoryNames = QuestProgress.InitializeExcludedWeaponSlots(key);
					List<Dictionary<string, object>>.Enumerator enumerator1 = current.Value.GetEnumerator();
					try
					{
						while (enumerator1.MoveNext())
						{
							Dictionary<string, object> strs = enumerator1.Current;
							string str = strs.TryGet("id") as string;
							if (str != null)
							{
								if (QuestConstants.IsSupported(str))
								{
									Difficulty difficulty = Difficulty.None;
									object obj = null;
									Difficulty[] difficultyArray1 = difficultyArray;
									int num1 = 0;
									while (num1 < (int)difficultyArray1.Length)
									{
										Difficulty difficulty1 = difficultyArray1[num1];
										if (!strs.TryGetValue(QuestConstants.GetDifficultyKey(difficulty1), out obj))
										{
											num1++;
										}
										else
										{
											difficulty = difficulty1;
											break;
										}
									}
									Dictionary<string, object> strs1 = obj as Dictionary<string, object>;
									if (strs1 != null && difficulty != Difficulty.None)
									{
										try
										{
											Reward reward = Reward.Create(strs1["reward"] as List<object>);
											int num2 = Convert.ToInt32(strs1.TryGet("parameter") ?? 1);
											object obj1 = strs.TryGet("day");
											long num3 = (!dayOption.HasValue ? Convert.ToInt64(obj1) : dayOption.Value);
											bool flag = strs.TryGet("rewarded").Map<object, bool>(new Func<object, bool>(Convert.ToBoolean));
											bool flag1 = strs.TryGet("active").Map<object, bool>(new Func<object, bool>(Convert.ToBoolean));
											int num4 = strs.TryGet("currentCount").Map<object, int>(new Func<object, int>(Convert.ToInt32));
											string str1 = str;
											if (str1 != null)
											{
												if (QuestProgress.u003cu003ef__switchu0024mapE == null)
												{
													Dictionary<string, int> strs2 = new Dictionary<string, int>(5)
													{
														{ "killInMode", 0 },
														{ "winInMode", 0 },
														{ "winInMap", 1 },
														{ "killWithWeapon", 2 },
														{ "killNpcWithWeapon", 2 }
													};
													QuestProgress.u003cu003ef__switchu0024mapE = strs2;
												}
												if (QuestProgress.u003cu003ef__switchu0024mapE.TryGetValue(str1, out num))
												{
													switch (num)
													{
														case 0:
														{
															ConnectSceneNGUIController.RegimGame? nullable = QuestProgress.ExtractModeFromQuestDescription(strs, hasValue, str);
															if (nullable.HasValue)
															{
																ModeAccumulativeQuest modeAccumulativeQuest = new ModeAccumulativeQuest(str, num3, key, difficulty, reward, flag1, flag, num2, nullable.Value, num4);
																questBases.Add(modeAccumulativeQuest);
																goto Label0;
															}
															else
															{
																continue;
															}
														}
														case 1:
														{
															string str2 = QuestProgress.ExtractMapFromQuestDescription(strs, hasValue);
															if (!string.IsNullOrEmpty(str2))
															{
																MapAccumulativeQuest mapAccumulativeQuest = new MapAccumulativeQuest(str, num3, key, difficulty, reward, flag1, flag, num2, str2, num4);
																questBases.Add(mapAccumulativeQuest);
																goto Label0;
															}
															else
															{
																continue;
															}
														}
														case 2:
														{
															ShopNGUIController.CategoryNames? nullable1 = QuestProgress.ExtractWeaponSlotFromQuestDescription(strs, hasValue, categoryNames);
															if (nullable1.HasValue)
															{
																categoryNames.Add(nullable1.Value);
																WeaponSlotAccumulativeQuest weaponSlotAccumulativeQuest = new WeaponSlotAccumulativeQuest(str, num3, key, difficulty, reward, flag1, flag, num2, nullable1.Value, num4);
																questBases.Add(weaponSlotAccumulativeQuest);
																goto Label0;
															}
															else
															{
																continue;
															}
														}
													}
												}
											}
											SimpleAccumulativeQuest simpleAccumulativeQuest = new SimpleAccumulativeQuest(str, num3, key, difficulty, reward, flag1, flag, num2, num4);
											questBases.Add(simpleAccumulativeQuest);
										Label0:
										}
										catch (Exception exception)
										{
											Debug.LogException(exception);
										}
									}
								}
								else
								{
									Debug.LogWarning(string.Concat("Quest is not supported: ", str));
								}
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator1).Dispose();
					}
					actualResult[key] = questBases;
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

		public void PopulateQuests(IDictionary<int, List<QuestBase>> currentQuests, IDictionary<int, List<QuestBase>> previousQuests)
		{
			if (currentQuests != null)
			{
				IEnumerator<KeyValuePair<int, List<QuestBase>>> enumerator = currentQuests.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, List<QuestBase>> current = enumerator.Current;
						List<QuestBase>.Enumerator enumerator1 = current.Value.GetEnumerator();
						try
						{
							while (enumerator1.MoveNext())
							{
								enumerator1.Current.Changed += new EventHandler(this.OnQuestChangedCheckCompletion);
							}
						}
						finally
						{
							((IDisposable)(object)enumerator1).Dispose();
						}
						this._currentQuests[current.Key] = new List<QuestBase>(current.Value);
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
			if (previousQuests != null)
			{
				IEnumerator<KeyValuePair<int, List<QuestBase>>> enumerator2 = previousQuests.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, List<QuestBase>> questBases = enumerator2.Current;
						List<QuestBase>.Enumerator enumerator3 = questBases.Value.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								enumerator3.Current.Changed += new EventHandler(this.OnQuestChangedCheckCompletion);
							}
						}
						finally
						{
							((IDisposable)(object)enumerator3).Dispose();
						}
						this._previousQuests[questBases.Key] = new List<QuestBase>(questBases.Value);
					}
				}
				finally
				{
					if (enumerator2 == null)
					{
					}
					enumerator2.Dispose();
				}
			}
			this._dirty = true;
		}

		public static IDictionary<int, List<QuestBase>> RestoreQuests(Dictionary<string, object> rawQuests)
		{
			return QuestProgress.ParseQuests(rawQuests, null, new Difficulty[] { Difficulty.Easy, Difficulty.Normal, Difficulty.Hard });
		}

		public void SetClean()
		{
			IEnumerator<List<QuestBase>> enumerator = this._currentQuests.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					List<QuestBase>.Enumerator enumerator1 = enumerator.Current.GetEnumerator();
					try
					{
						while (enumerator1.MoveNext())
						{
							enumerator1.Current.SetClean();
						}
					}
					finally
					{
						((IDisposable)(object)enumerator1).Dispose();
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
			IEnumerator<List<QuestBase>> enumerator2 = this._previousQuests.Values.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					List<QuestBase>.Enumerator enumerator3 = enumerator2.Current.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							enumerator3.Current.SetClean();
						}
					}
					finally
					{
						((IDisposable)(object)enumerator3).Dispose();
					}
				}
			}
			finally
			{
				if (enumerator2 == null)
				{
				}
				enumerator2.Dispose();
			}
			this._dirty = false;
		}

		private static List<T> Shuffle<T>(IEnumerable<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			List<T> ts = list.ToList<T>();
			QuestProgress.ShuffleInPlace<T>(ts);
			return ts;
		}

		private static void ShuffleInPlace<T>(List<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.Count < 2)
			{
				return;
			}
			for (int i = list.Count - 1; i >= 1; i--)
			{
				int item = UnityEngine.Random.Range(0, i);
				T t = list[item];
				list[item] = list[i];
				list[i] = t;
			}
		}

		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, List<object>> strs = new Dictionary<string, List<object>>(3);
			IEnumerator<KeyValuePair<int, List<QuestBase>>> enumerator = this._currentQuests.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, List<QuestBase>> current = enumerator.Current;
					string str = current.Key.ToString(NumberFormatInfo.InvariantInfo);
					List<object> objs = new List<object>(2);
					List<QuestBase>.Enumerator enumerator1 = current.Value.GetEnumerator();
					try
					{
						while (enumerator1.MoveNext())
						{
							objs.Add(enumerator1.Current.ToJson());
						}
					}
					finally
					{
						((IDisposable)(object)enumerator1).Dispose();
					}
					strs[str] = objs;
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			Dictionary<string, List<object>> strs1 = new Dictionary<string, List<object>>(3);
			IEnumerator<KeyValuePair<int, List<QuestBase>>> enumerator2 = this._previousQuests.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, List<QuestBase>> keyValuePair = enumerator2.Current;
					string str1 = keyValuePair.Key.ToString(NumberFormatInfo.InvariantInfo);
					List<object> objs1 = new List<object>(2);
					List<QuestBase>.Enumerator enumerator3 = keyValuePair.Value.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							objs1.Add(enumerator3.Current.ToJson());
						}
					}
					finally
					{
						((IDisposable)(object)enumerator3).Dispose();
					}
					strs1[str1] = objs1;
				}
			}
			finally
			{
				if (enumerator2 == null)
				{
				}
				enumerator2.Dispose();
			}
			List<object> objs2 = new List<object>(this._tutorialQuests.Count);
			foreach (QuestBase _tutorialQuest in this._tutorialQuests)
			{
				objs2.Add(_tutorialQuest.ToJson());
			}
			Dictionary<string, object> strs2 = new Dictionary<string, object>(3)
			{
				{ "day", this._day }
			};
			DateTime timestamp = this.Timestamp;
			strs2.Add("timestamp", timestamp.ToString("s", CultureInfo.InvariantCulture));
			float timeLeftSeconds = this.TimeLeftSeconds;
			strs2.Add("timeLeftSeconds", timeLeftSeconds.ToString(CultureInfo.InvariantCulture));
			strs2.Add("tutorialQuests", objs2);
			strs2.Add("previousQuests", strs1);
			strs2.Add("currentQuests", strs);
			return strs2;
		}

		internal bool TryRemoveTutorialQuest(string questId)
		{
			if (questId == null)
			{
				return false;
			}
			int num = this._tutorialQuests.FindIndex((QuestBase q) => questId.Equals(q.Id, StringComparison.Ordinal));
			if (num < 0)
			{
				return false;
			}
			this._tutorialQuests.RemoveAt(num);
			this._dirty = true;
			return true;
		}

		public void UpdateQuests(long day, Dictionary<string, object> rawQuests, IDictionary<int, List<QuestBase>> newQuests)
		{
			List<QuestBase> questBases;
			if (newQuests == null)
			{
				return;
			}
			this._day = day;
			IEnumerable<int> nums = this._previousQuests.Keys.Concat<int>(this._currentQuests.Keys).Distinct<int>();
			Dictionary<int, IList<QuestBase>> dictionary = nums.ToDictionary<int, int, IList<QuestBase>>((int s) => s, (int s) => this.GetActiveQuestsBySlot(s, true));
			this.ClearQuests(this._previousQuests);
			foreach (KeyValuePair<int, IList<QuestBase>> keyValuePair in dictionary)
			{
				int key = keyValuePair.Key;
				IList<QuestBase> value = keyValuePair.Value;
				IEnumerator<QuestBase> enumerator = value.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						QuestBase current = enumerator.Current;
						current.Changed -= new EventHandler(this.OnQuestChangedCheckCompletion);
						if (current.Rewarded)
						{
							continue;
						}
						current.Changed += new EventHandler(this.OnQuestChangedCheckCompletion);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
				this._previousQuests[key] = new List<QuestBase>(
					from q in value
					where !q.Rewarded
					select q);
			}
			this.ClearQuests(this._currentQuests);
			IEnumerator<KeyValuePair<int, List<QuestBase>>> enumerator1 = newQuests.GetEnumerator();
			try
			{
				while (enumerator1.MoveNext())
				{
					KeyValuePair<int, List<QuestBase>> current1 = enumerator1.Current;
					int num = current1.Key;
					List<QuestBase> value1 = current1.Value;
					if (!this._previousQuests.TryGetValue(num, out questBases))
					{
						questBases = new List<QuestBase>();
					}
					if (!questBases.FirstOrDefault<QuestBase>().Map<QuestBase, bool>((QuestBase q) => (q.CalculateProgress() >= new decimal(1) ? false : !q.Rewarded)))
					{
						List<QuestBase>.Enumerator enumerator2 = value1.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								QuestBase questBase = enumerator2.Current;
								questBase.Changed -= new EventHandler(this.OnQuestChangedCheckCompletion);
								questBase.Changed += new EventHandler(this.OnQuestChangedCheckCompletion);
							}
						}
						finally
						{
							((IDisposable)(object)enumerator2).Dispose();
						}
						this._currentQuests[num] = new List<QuestBase>(value1);
					}
				}
			}
			finally
			{
				if (enumerator1 == null)
				{
				}
				enumerator1.Dispose();
			}
			if (rawQuests != null)
			{
				Difficulty[] array = this._previousQuests.SelectMany<KeyValuePair<int, List<QuestBase>>, Difficulty>((KeyValuePair<int, List<QuestBase>> kv) => 
					from q in kv.Value
					select q.Difficulty).Distinct<Difficulty>().ToArray<Difficulty>();
				QuestProgress.ParseQuests(rawQuests, new long?(day), array, this._previousQuests);
			}
			this._dirty = true;
		}

		public event EventHandler<QuestCompletedEventArgs> QuestCompleted
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.QuestCompleted += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.QuestCompleted -= value;
			}
		}
	}
}