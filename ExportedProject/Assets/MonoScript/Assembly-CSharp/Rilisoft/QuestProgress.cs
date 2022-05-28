using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.DictionaryExtensions;
using Rilisoft.NullExtensions;
using UnityEngine;

namespace Rilisoft
{
	public sealed class QuestProgress : IDisposable
	{
		[CompilerGenerated]
		private sealed class _003CDebugDecrementDay_003Ec__AnonStorey2E6
		{
			internal long newDay;

			internal bool _003C_003Em__3D0(QuestBase q)
			{
				return newDay < q.Day;
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E7
		{
			internal int slot;

			internal QuestProgress _003C_003Ef__this;

			private static Func<QuestBase, int> _003C_003Ef__am_0024cache2;

			internal IList<QuestBase> _003C_003Em__3D5()
			{
				IEnumerable<int> first = _003C_003Ef__this._currentQuests.Keys.Concat(_003C_003Ef__this._previousQuests.Keys);
				List<QuestBase> tutorialQuests = _003C_003Ef__this._tutorialQuests;
				if (_003C_003Ef__am_0024cache2 == null)
				{
					_003C_003Ef__am_0024cache2 = _003C_003Em__419;
				}
				IEnumerable<int> enumerable = first.Concat(tutorialQuests.Select(_003C_003Ef__am_0024cache2)).Distinct();
				foreach (int item in enumerable)
				{
					List<QuestBase> activeQuestsBySlotReference = _003C_003Ef__this.GetActiveQuestsBySlotReference(item);
					if (activeQuestsBySlotReference != null && activeQuestsBySlotReference.Count > 0 && slot == item)
					{
						activeQuestsBySlotReference.RemoveAt(0);
						_003C_003Ef__this._dirty = true;
					}
					List<QuestBase> activeQuestsBySlotReference2 = _003C_003Ef__this.GetActiveQuestsBySlotReference(item, true);
					if (activeQuestsBySlotReference2.Count > 1)
					{
						activeQuestsBySlotReference2.RemoveRange(1, activeQuestsBySlotReference2.Count - 1);
						List<QuestBase> value;
						if (activeQuestsBySlotReference2[0].CalculateProgress() >= 1m && _003C_003Ef__this._currentQuests.TryGetValue(item, out value) && value.Count > 1)
						{
							value.RemoveRange(1, value.Count - 1);
						}
						_003C_003Ef__this._dirty = true;
					}
				}
				return _003C_003Ef__this.GetActiveQuestsBySlot(slot);
			}

			private static int _003C_003Em__419(QuestBase q)
			{
				return q.Slot;
			}
		}

		[CompilerGenerated]
		private sealed class _003CTryRemoveTutorialQuest_003Ec__AnonStorey2E8
		{
			internal string questId;

			internal bool _003C_003Em__3DA(QuestBase q)
			{
				return questId.Equals(q.Id, StringComparison.Ordinal);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetQuestById_003Ec__AnonStorey2E9
		{
			internal string id;

			internal bool _003C_003Em__3DE(QuestBase q)
			{
				return q.Id.Equals(id, StringComparison.Ordinal);
			}
		}

		[CompilerGenerated]
		private sealed class _003CFilterQuests_003Ec__AnonStorey2EA
		{
			internal string chosenDifficultyKey;

			internal Difficulty chosenDifficulty;

			internal bool _003C_003Em__3F5(KeyValuePair<string, Dictionary<string, object>> kv)
			{
				return kv.Value.ContainsKey(chosenDifficultyKey);
			}

			internal bool _003C_003Em__3FE(Difficulty d)
			{
				return d != chosenDifficulty;
			}
		}

		[CompilerGenerated]
		private sealed class _003CFilterQuests_003Ec__AnonStorey2EB
		{
			internal string existingQuestId;

			internal bool _003C_003Em__3F8(KeyValuePair<string, Dictionary<string, object>> kv)
			{
				return StringComparer.OrdinalIgnoreCase.Equals(kv.Key, existingQuestId);
			}
		}

		[CompilerGenerated]
		private sealed class _003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC
		{
			internal QuestBase quest;

			internal QuestProgress _003C_003Ef__this;

			internal void _003C_003Em__406(EventHandler<QuestCompletedEventArgs> handler)
			{
				handler(_003C_003Ef__this, new QuestCompletedEventArgs
				{
					Quest = quest
				});
			}
		}

		[CompilerGenerated]
		private sealed class _003CHandleKillOtherPlayer_003Ec__AnonStorey2ED
		{
			internal KillOtherPlayerEventArgs e;

			internal void _003C_003Em__407(ModeAccumulativeQuest quest)
			{
				quest.IncrementIf(e.Mode == quest.Mode);
			}

			internal void _003C_003Em__408(WeaponSlotAccumulativeQuest quest)
			{
				quest.IncrementIf(e.WeaponSlot == quest.WeaponSlot);
			}
		}

		[CompilerGenerated]
		private sealed class _003CHandleKillMonster_003Ec__AnonStorey2EE
		{
			internal KillMonsterEventArgs e;

			internal void _003C_003Em__40E(SimpleAccumulativeQuest quest)
			{
				quest.IncrementIf(e.Campaign);
			}

			internal void _003C_003Em__40F(WeaponSlotAccumulativeQuest quest)
			{
				quest.IncrementIf(e.WeaponSlot == quest.WeaponSlot);
			}
		}

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

		[CompilerGenerated]
		private static Func<KeyValuePair<int, QuestBase>, bool> _003C_003Ef__am_0024cacheD;

		[CompilerGenerated]
		private static Func<int, int> _003C_003Ef__am_0024cacheE;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cacheF;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache10;

		[CompilerGenerated]
		private static Func<KeyValuePair<int, List<QuestBase>>, IEnumerable<Difficulty>> _003C_003Ef__am_0024cache11;

		[CompilerGenerated]
		private static Func<List<QuestBase>, IEnumerable<QuestBase>> _003C_003Ef__am_0024cache12;

		[CompilerGenerated]
		private static Func<List<QuestBase>, IEnumerable<QuestBase>> _003C_003Ef__am_0024cache13;

		[CompilerGenerated]
		private static Func<KeyValuePair<int, QuestBase>, int> _003C_003Ef__am_0024cache15;

		[CompilerGenerated]
		private static Func<KeyValuePair<int, QuestBase>, List<QuestBase>> _003C_003Ef__am_0024cache16;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache17;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache18;

		[CompilerGenerated]
		private static Func<QuestBase, int> _003C_003Ef__am_0024cache19;

		[CompilerGenerated]
		private static Func<QuestInfo, bool> _003C_003Ef__am_0024cache1A;

		[CompilerGenerated]
		private static Func<QuestBase, int> _003C_003Ef__am_0024cache1B;

		[CompilerGenerated]
		private static Func<int, int> _003C_003Ef__am_0024cache1C;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache1D;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache1E;

		[CompilerGenerated]
		private static Func<QuestBase, int> _003C_003Ef__am_0024cache1F;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache20;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache21;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache22;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache23;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache24;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache25;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache26;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache27;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache28;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache29;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache2A;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache2B;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache2C;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache2D;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache2E;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache2F;

		[CompilerGenerated]
		private static Func<AccumulativeQuestBase, bool> _003C_003Ef__am_0024cache30;

		[CompilerGenerated]
		private static Func<List<QuestBase>, IEnumerable<QuestBase>> _003C_003Ef__am_0024cache31;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache32;

		[CompilerGenerated]
		private static Func<List<QuestBase>, IEnumerable<QuestBase>> _003C_003Ef__am_0024cache33;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache34;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache35;

		[CompilerGenerated]
		private static Func<List<QuestBase>, QuestBase> _003C_003Ef__am_0024cache36;

		[CompilerGenerated]
		private static Func<QuestBase, string> _003C_003Ef__am_0024cache37;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, object>, string> _003C_003Ef__am_0024cache38;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, object>, object> _003C_003Ef__am_0024cache39;

		[CompilerGenerated]
		private static Func<List<QuestBase>, bool> _003C_003Ef__am_0024cache3A;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, object>, string> _003C_003Ef__am_0024cache3B;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, object>, object> _003C_003Ef__am_0024cache3C;

		[CompilerGenerated]
		private static Func<ExperienceController, int> _003C_003Ef__am_0024cache3D;

		[CompilerGenerated]
		private static Func<ShopNGUIController.CategoryNames?, bool> _003C_003Ef__am_0024cache3E;

		[CompilerGenerated]
		private static Func<ShopNGUIController.CategoryNames?, ShopNGUIController.CategoryNames> _003C_003Ef__am_0024cache3F;

		[CompilerGenerated]
		private static Func<ConnectSceneNGUIController.RegimGame?, bool> _003C_003Ef__am_0024cache40;

		[CompilerGenerated]
		private static Func<ConnectSceneNGUIController.RegimGame?, ConnectSceneNGUIController.RegimGame> _003C_003Ef__am_0024cache41;

		[CompilerGenerated]
		private static Func<ExperienceController, int> _003C_003Ef__am_0024cache42;

		[CompilerGenerated]
		private static Predicate<QuestBase> _003C_003Ef__am_0024cache43;

		[CompilerGenerated]
		private static Func<SimpleAccumulativeQuest, string> _003C_003Ef__am_0024cache44;

		[CompilerGenerated]
		private static Func<SimpleAccumulativeQuest, SimpleAccumulativeQuest> _003C_003Ef__am_0024cache45;

		[CompilerGenerated]
		private static Action<SimpleAccumulativeQuest> _003C_003Ef__am_0024cache46;

		[CompilerGenerated]
		private static Func<SimpleAccumulativeQuest, string> _003C_003Ef__am_0024cache47;

		[CompilerGenerated]
		private static Func<SimpleAccumulativeQuest, SimpleAccumulativeQuest> _003C_003Ef__am_0024cache48;

		[CompilerGenerated]
		private static Action<SimpleAccumulativeQuest> _003C_003Ef__am_0024cache49;

		[CompilerGenerated]
		private static Action<SimpleAccumulativeQuest> _003C_003Ef__am_0024cache4A;

		[CompilerGenerated]
		private static Action<SimpleAccumulativeQuest> _003C_003Ef__am_0024cache4B;

		[CompilerGenerated]
		private static Action<SimpleAccumulativeQuest> _003C_003Ef__am_0024cache4C;

		[CompilerGenerated]
		private static Action<SimpleAccumulativeQuest> _003C_003Ef__am_0024cache4D;

		[CompilerGenerated]
		private static Func<KeyValuePair<int, List<QuestBase>>, IEnumerable<QuestBase>> _003C_003Ef__am_0024cache4E;

		[CompilerGenerated]
		private static Func<KeyValuePair<int, List<QuestBase>>, IEnumerable<QuestBase>> _003C_003Ef__am_0024cache4F;

		[CompilerGenerated]
		private static Func<QuestBase, Difficulty> _003C_003Ef__am_0024cache50;

		[CompilerGenerated]
		private static Func<QuestBase, bool> _003C_003Ef__am_0024cache51;

		public string ConfigVersion
		{
			get
			{
				return _configVersion;
			}
		}

		public long Day
		{
			get
			{
				return _day;
			}
		}

		public DateTime Timestamp
		{
			get
			{
				return _timestamp;
			}
		}

		public float TimeLeftSeconds
		{
			get
			{
				return _timeLeftSeconds;
			}
		}

		public bool AnyActiveQuest
		{
			get
			{
				IDictionary<int, QuestBase> activeQuests = GetActiveQuests();
				if (_003C_003Ef__am_0024cacheD == null)
				{
					_003C_003Ef__am_0024cacheD = _003Cget_AnyActiveQuest_003Em__3C8;
				}
				return activeQuests.Any(_003C_003Ef__am_0024cacheD);
			}
		}

		public int Count
		{
			get
			{
				return _currentQuests.Count + _previousQuests.Count;
			}
		}

		public bool Disposed
		{
			get
			{
				return _disposed;
			}
		}

		public event EventHandler<QuestCompletedEventArgs> QuestCompleted;

		public QuestProgress(string configVersion, long day, DateTime timestamp, float timeLeftSeconds, QuestProgress oldQuestProgress = null)
		{
			if (string.IsNullOrEmpty(configVersion))
			{
				throw new ArgumentException("ConfigId should not be empty.", "configVersion");
			}
			_events = QuestMediator.Events;
			_events.Win += HandleWin;
			_events.KillOtherPlayer += HandleKillOtherPlayer;
			_events.KillOtherPlayerWithFlag += HandleKillOtherPlayerWithFlag;
			_events.Capture += HandleCapture;
			_events.KillMonster += HandleKillMonster;
			_events.BreakSeries += HandleBreakSeries;
			_events.MakeSeries += HandleMakeSeries;
			_events.SurviveWaveInArena += HandleSurviveInArena;
			_events.GetGotcha += HandleGetGotcha;
			_events.SocialInteraction += HandleSocialInteraction;
			_configVersion = configVersion;
			_timestamp = timestamp;
			_timeLeftSeconds = timeLeftSeconds;
			_day = day;
			if (oldQuestProgress != null)
			{
				_tutorialQuests = oldQuestProgress._tutorialQuests;
				foreach (QuestBase tutorialQuest in _tutorialQuests)
				{
					tutorialQuest.Changed += OnQuestChangedCheckCompletion;
				}
			}
			UnityEngine.Random.seed = (int)Tools.CurrentUnixTime;
		}

		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, List<object>> dictionary = new Dictionary<string, List<object>>(3);
			foreach (KeyValuePair<int, List<QuestBase>> currentQuest in _currentQuests)
			{
				string key = currentQuest.Key.ToString(NumberFormatInfo.InvariantInfo);
				List<object> list = new List<object>(2);
				foreach (QuestBase item in currentQuest.Value)
				{
					list.Add(item.ToJson());
				}
				dictionary[key] = list;
			}
			Dictionary<string, List<object>> dictionary2 = new Dictionary<string, List<object>>(3);
			foreach (KeyValuePair<int, List<QuestBase>> previousQuest in _previousQuests)
			{
				string key2 = previousQuest.Key.ToString(NumberFormatInfo.InvariantInfo);
				List<object> list2 = new List<object>(2);
				foreach (QuestBase item2 in previousQuest.Value)
				{
					list2.Add(item2.ToJson());
				}
				dictionary2[key2] = list2;
			}
			List<object> list3 = new List<object>(_tutorialQuests.Count);
			foreach (QuestBase tutorialQuest in _tutorialQuests)
			{
				list3.Add(tutorialQuest.ToJson());
			}
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>(3);
			dictionary3.Add("day", _day);
			dictionary3.Add("timestamp", Timestamp.ToString("s", CultureInfo.InvariantCulture));
			dictionary3.Add("timeLeftSeconds", TimeLeftSeconds.ToString(CultureInfo.InvariantCulture));
			dictionary3.Add("tutorialQuests", list3);
			dictionary3.Add("previousQuests", dictionary2);
			dictionary3.Add("currentQuests", dictionary);
			return dictionary3;
		}

		public void UpdateQuests(long day, Dictionary<string, object> rawQuests, IDictionary<int, List<QuestBase>> newQuests)
		{
			if (newQuests == null)
			{
				return;
			}
			_day = day;
			IEnumerable<int> source = _previousQuests.Keys.Concat(_currentQuests.Keys).Distinct();
			if (_003C_003Ef__am_0024cacheE == null)
			{
				_003C_003Ef__am_0024cacheE = _003CUpdateQuests_003Em__3C9;
			}
			Dictionary<int, IList<QuestBase>> dictionary = source.ToDictionary(_003C_003Ef__am_0024cacheE, _003CUpdateQuests_003Em__3CA);
			ClearQuests(_previousQuests);
			foreach (KeyValuePair<int, IList<QuestBase>> item in dictionary)
			{
				int key = item.Key;
				IList<QuestBase> value = item.Value;
				foreach (QuestBase item2 in value)
				{
					item2.Changed -= OnQuestChangedCheckCompletion;
					if (!item2.Rewarded)
					{
						item2.Changed += OnQuestChangedCheckCompletion;
					}
				}
				IDictionary<int, List<QuestBase>> previousQuests = _previousQuests;
				if (_003C_003Ef__am_0024cacheF == null)
				{
					_003C_003Ef__am_0024cacheF = _003CUpdateQuests_003Em__3CB;
				}
				previousQuests[key] = new List<QuestBase>(value.Where(_003C_003Ef__am_0024cacheF));
			}
			ClearQuests(_currentQuests);
			foreach (KeyValuePair<int, List<QuestBase>> newQuest in newQuests)
			{
				int key2 = newQuest.Key;
				List<QuestBase> value2 = newQuest.Value;
				List<QuestBase> value3;
				if (!_previousQuests.TryGetValue(key2, out value3))
				{
					value3 = new List<QuestBase>();
				}
				QuestBase o = value3.FirstOrDefault();
				if (_003C_003Ef__am_0024cache10 == null)
				{
					_003C_003Ef__am_0024cache10 = _003CUpdateQuests_003Em__3CC;
				}
				if (o.Map(_003C_003Ef__am_0024cache10))
				{
					continue;
				}
				foreach (QuestBase item3 in value2)
				{
					item3.Changed -= OnQuestChangedCheckCompletion;
					item3.Changed += OnQuestChangedCheckCompletion;
				}
				_currentQuests[key2] = new List<QuestBase>(value2);
			}
			if (rawQuests != null)
			{
				IDictionary<int, List<QuestBase>> previousQuests2 = _previousQuests;
				if (_003C_003Ef__am_0024cache11 == null)
				{
					_003C_003Ef__am_0024cache11 = _003CUpdateQuests_003Em__3CD;
				}
				Difficulty[] allowedDifficulties = previousQuests2.SelectMany(_003C_003Ef__am_0024cache11).Distinct().ToArray();
				ParseQuests(rawQuests, day, allowedDifficulties, _previousQuests);
			}
			_dirty = true;
		}

		public void PopulateQuests(IDictionary<int, List<QuestBase>> currentQuests, IDictionary<int, List<QuestBase>> previousQuests)
		{
			if (currentQuests != null)
			{
				foreach (KeyValuePair<int, List<QuestBase>> currentQuest in currentQuests)
				{
					foreach (QuestBase item in currentQuest.Value)
					{
						item.Changed += OnQuestChangedCheckCompletion;
					}
					_currentQuests[currentQuest.Key] = new List<QuestBase>(currentQuest.Value);
				}
			}
			if (previousQuests != null)
			{
				foreach (KeyValuePair<int, List<QuestBase>> previousQuest in previousQuests)
				{
					foreach (QuestBase item2 in previousQuest.Value)
					{
						item2.Changed += OnQuestChangedCheckCompletion;
					}
					_previousQuests[previousQuest.Key] = new List<QuestBase>(previousQuest.Value);
				}
			}
			_dirty = true;
		}

		public void FillTutorialQuests(List<object> questJsons)
		{
			if (questJsons == null)
			{
				return;
			}
			TutorialQuestManager.Instance.FillTutorialQuests(questJsons, Day, _tutorialQuests);
			foreach (QuestBase tutorialQuest in _tutorialQuests)
			{
				tutorialQuest.Changed -= OnQuestChangedCheckCompletion;
				tutorialQuest.Changed += OnQuestChangedCheckCompletion;
			}
			_dirty = true;
		}

		public static IDictionary<int, List<QuestBase>> RestoreQuests(Dictionary<string, object> rawQuests)
		{
			Difficulty[] allowedDifficulties = new Difficulty[3]
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			return ParseQuests(rawQuests, null, allowedDifficulties);
		}

		public static IDictionary<int, List<QuestBase>> CreateQuests(Dictionary<string, object> rawQuests, long day, Difficulty[] allowedDifficulties)
		{
			if (allowedDifficulties == null)
			{
				allowedDifficulties = new Difficulty[3]
				{
					Difficulty.Easy,
					Difficulty.Normal,
					Difficulty.Hard
				};
			}
			return ParseQuests(rawQuests, day, allowedDifficulties);
		}

		internal void DebugDecrementDay()
		{
			_003CDebugDecrementDay_003Ec__AnonStorey2E6 _003CDebugDecrementDay_003Ec__AnonStorey2E = new _003CDebugDecrementDay_003Ec__AnonStorey2E6();
			_003CDebugDecrementDay_003Ec__AnonStorey2E.newDay = _day - 172800;
			ICollection<List<QuestBase>> values = _previousQuests.Values;
			if (_003C_003Ef__am_0024cache12 == null)
			{
				_003C_003Ef__am_0024cache12 = _003CDebugDecrementDay_003Em__3CE;
			}
			IEnumerable<QuestBase> first = values.SelectMany(_003C_003Ef__am_0024cache12);
			ICollection<List<QuestBase>> values2 = _currentQuests.Values;
			if (_003C_003Ef__am_0024cache13 == null)
			{
				_003C_003Ef__am_0024cache13 = _003CDebugDecrementDay_003Em__3CF;
			}
			IEnumerable<QuestBase> enumerable = first.Concat(values2.SelectMany(_003C_003Ef__am_0024cache13)).Concat(_tutorialQuests).Where(_003CDebugDecrementDay_003Ec__AnonStorey2E._003C_003Em__3D0);
			foreach (QuestBase item in enumerable)
			{
				item.DebugSetDay(_003CDebugDecrementDay_003Ec__AnonStorey2E.newDay);
			}
			_day = _003CDebugDecrementDay_003Ec__AnonStorey2E.newDay;
			_dirty = true;
		}

		private static IDictionary<int, List<QuestBase>> ParseQuests(Dictionary<string, object> rawQuests, long? dayOption, Difficulty[] allowedDifficulties)
		{
			Dictionary<int, List<QuestBase>> dictionary = new Dictionary<int, List<QuestBase>>(3);
			ParseQuests(rawQuests, dayOption, allowedDifficulties, dictionary);
			return dictionary;
		}

		private static HashSet<ShopNGUIController.CategoryNames> InitializeExcludedWeaponSlots(int slot)
		{
			HashSet<ShopNGUIController.CategoryNames> hashSet = new HashSet<ShopNGUIController.CategoryNames>();
			if (QuestSystem.Instance == null || QuestSystem.Instance.QuestProgress == null)
			{
				return hashSet;
			}
			QuestBase activeQuestBySlot = QuestSystem.Instance.QuestProgress.GetActiveQuestBySlot(slot);
			WeaponSlotAccumulativeQuest weaponSlotAccumulativeQuest = activeQuestBySlot as WeaponSlotAccumulativeQuest;
			if (weaponSlotAccumulativeQuest != null)
			{
				hashSet.Add(weaponSlotAccumulativeQuest.WeaponSlot);
			}
			return hashSet;
		}

		private static void ParseQuests(Dictionary<string, object> rawQuests, long? dayOption, Difficulty[] allowedDifficulties, IDictionary<int, List<QuestBase>> actualResult)
		{
			if (actualResult == null || rawQuests == null || rawQuests.Count == 0)
			{
				return;
			}
			if (allowedDifficulties == null)
			{
				throw new ArgumentNullException("allowedDifficulties");
			}
			bool flag = !dayOption.HasValue;
			Dictionary<int, List<QuestBase>> dictionary;
			if (QuestSystem.Instance.QuestProgress != null)
			{
				IDictionary<int, QuestBase> activeQuests = QuestSystem.Instance.QuestProgress.GetActiveQuests();
				if (_003C_003Ef__am_0024cache15 == null)
				{
					_003C_003Ef__am_0024cache15 = _003CParseQuests_003Em__3D1;
				}
				Func<KeyValuePair<int, QuestBase>, int> keySelector = _003C_003Ef__am_0024cache15;
				if (_003C_003Ef__am_0024cache16 == null)
				{
					_003C_003Ef__am_0024cache16 = _003CParseQuests_003Em__3D2;
				}
				dictionary = activeQuests.ToDictionary(keySelector, _003C_003Ef__am_0024cache16);
			}
			else
			{
				dictionary = new Dictionary<int, List<QuestBase>>();
			}
			IDictionary<int, List<QuestBase>> existingQuests = dictionary;
			IDictionary<int, List<Dictionary<string, object>>> dictionary3;
			if (flag)
			{
				IDictionary<int, List<Dictionary<string, object>>> dictionary2 = ExtractQuests(rawQuests);
				dictionary3 = dictionary2;
			}
			else
			{
				dictionary3 = FilterQuests(rawQuests, allowedDifficulties, existingQuests);
			}
			IDictionary<int, List<Dictionary<string, object>>> dictionary4 = dictionary3;
			Difficulty[] array = new Difficulty[3]
			{
				Difficulty.Easy,
				Difficulty.Normal,
				Difficulty.Hard
			};
			foreach (KeyValuePair<int, List<Dictionary<string, object>>> item5 in dictionary4)
			{
				int key = item5.Key;
				List<QuestBase> value;
				if (!actualResult.TryGetValue(key, out value))
				{
					value = new List<QuestBase>(2);
				}
				HashSet<ShopNGUIController.CategoryNames> hashSet = InitializeExcludedWeaponSlots(key);
				foreach (Dictionary<string, object> item6 in item5.Value)
				{
					string text = item6.TryGet("id") as string;
					if (text == null)
					{
						continue;
					}
					if (!QuestConstants.IsSupported(text))
					{
						Debug.LogWarning("Quest is not supported: " + text);
						continue;
					}
					Difficulty difficulty = Difficulty.None;
					object value2 = null;
					Difficulty[] array2 = array;
					foreach (Difficulty difficulty2 in array2)
					{
						if (item6.TryGetValue(QuestConstants.GetDifficultyKey(difficulty2), out value2))
						{
							difficulty = difficulty2;
							break;
						}
					}
					Dictionary<string, object> dictionary5 = value2 as Dictionary<string, object>;
					if (dictionary5 == null || difficulty == Difficulty.None)
					{
						continue;
					}
					try
					{
						List<object> reward = dictionary5["reward"] as List<object>;
						Reward reward2 = Reward.Create(reward);
						int requiredCount = Convert.ToInt32(dictionary5.TryGet("parameter") ?? ((object)1));
						object value3 = item6.TryGet("day");
						long day = ((!dayOption.HasValue) ? Convert.ToInt64(value3) : dayOption.Value);
						bool rewarded = item6.TryGet("rewarded").Map(Convert.ToBoolean);
						bool active = item6.TryGet("active").Map(Convert.ToBoolean);
						int initialCount = item6.TryGet("currentCount").Map(Convert.ToInt32);
						switch (text)
						{
						case "killInMode":
						case "winInMode":
						{
							ConnectSceneNGUIController.RegimGame? regimGame = ExtractModeFromQuestDescription(item6, flag, text);
							if (regimGame.HasValue)
							{
								ModeAccumulativeQuest item3 = new ModeAccumulativeQuest(text, day, key, difficulty, reward2, active, rewarded, requiredCount, regimGame.Value, initialCount);
								value.Add(item3);
							}
							break;
						}
						case "winInMap":
						{
							string text2 = ExtractMapFromQuestDescription(item6, flag);
							if (!string.IsNullOrEmpty(text2))
							{
								MapAccumulativeQuest item4 = new MapAccumulativeQuest(text, day, key, difficulty, reward2, active, rewarded, requiredCount, text2, initialCount);
								value.Add(item4);
							}
							break;
						}
						case "killWithWeapon":
						case "killNpcWithWeapon":
						{
							ShopNGUIController.CategoryNames? categoryNames = ExtractWeaponSlotFromQuestDescription(item6, flag, hashSet);
							if (categoryNames.HasValue)
							{
								hashSet.Add(categoryNames.Value);
								WeaponSlotAccumulativeQuest item2 = new WeaponSlotAccumulativeQuest(text, day, key, difficulty, reward2, active, rewarded, requiredCount, categoryNames.Value, initialCount);
								value.Add(item2);
							}
							break;
						}
						default:
						{
							SimpleAccumulativeQuest item = new SimpleAccumulativeQuest(text, day, key, difficulty, reward2, active, rewarded, requiredCount, initialCount);
							value.Add(item);
							break;
						}
						}
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
				actualResult[key] = value;
			}
		}

		public QuestBase GetActiveQuestBySlot(int slot)
		{
			QuestBase activeTutorialQuest = GetActiveTutorialQuest();
			if (activeTutorialQuest != null && activeTutorialQuest.Slot == slot)
			{
				return activeTutorialQuest;
			}
			List<QuestBase> value = null;
			_previousQuests.TryGetValue(slot, out value);
			List<QuestBase> o = value;
			if (_003C_003Ef__am_0024cache17 == null)
			{
				_003C_003Ef__am_0024cache17 = _003CGetActiveQuestBySlot_003Em__3D3;
			}
			QuestBase questBase = o.Map(_003C_003Ef__am_0024cache17);
			if (questBase != null && !questBase.Rewarded)
			{
				return questBase;
			}
			List<QuestBase> value2 = null;
			_currentQuests.TryGetValue(slot, out value2);
			List<QuestBase> o2 = value2;
			if (_003C_003Ef__am_0024cache18 == null)
			{
				_003C_003Ef__am_0024cache18 = _003CGetActiveQuestBySlot_003Em__3D4;
			}
			QuestBase questBase2 = o2.Map(_003C_003Ef__am_0024cache18);
			if (questBase2 != null)
			{
				return questBase2;
			}
			return questBase;
		}

		public QuestInfo GetActiveQuestInfoBySlot(int slot)
		{
			_003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E7 _003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E = new _003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E7();
			_003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E.slot = slot;
			_003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E._003C_003Ef__this = this;
			IList<QuestBase> activeQuestsBySlot = GetActiveQuestsBySlot(_003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E.slot);
			Func<IList<QuestBase>> skipMethod = _003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E._003C_003Em__3D5;
			bool forcedSkip = object.ReferenceEquals(_tutorialQuests, GetActiveQuestsBySlotReference(_003CGetActiveQuestInfoBySlot_003Ec__AnonStorey2E.slot));
			return new QuestInfo(activeQuestsBySlot, skipMethod, forcedSkip);
		}

		public QuestInfo GetRandomQuestInfo()
		{
			IEnumerable<int> first = _previousQuests.Keys.Concat(_currentQuests.Keys);
			List<QuestBase> tutorialQuests = _tutorialQuests;
			if (_003C_003Ef__am_0024cache19 == null)
			{
				_003C_003Ef__am_0024cache19 = _003CGetRandomQuestInfo_003Em__3D6;
			}
			IEnumerable<int> source = first.Concat(tutorialQuests.Select(_003C_003Ef__am_0024cache19)).Distinct();
			IEnumerable<QuestInfo> source2 = source.Select(GetActiveQuestInfoBySlot);
			if (_003C_003Ef__am_0024cache1A == null)
			{
				_003C_003Ef__am_0024cache1A = _003CGetRandomQuestInfo_003Em__3D7;
			}
			List<QuestInfo> list = source2.Where(_003C_003Ef__am_0024cache1A).ToList();
			if (list.Count < 1)
			{
				return null;
			}
			QuestInfo questInfo = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				QuestInfo questInfo2 = list[i];
				if (questInfo2.Quest == null)
				{
					continue;
				}
				if (questInfo.Quest == null)
				{
					questInfo = list[i];
					continue;
				}
				AccumulativeQuestBase accumulativeQuestBase = questInfo.Quest as AccumulativeQuestBase;
				AccumulativeQuestBase accumulativeQuestBase2 = questInfo2.Quest as AccumulativeQuestBase;
				if (accumulativeQuestBase != null && accumulativeQuestBase2 != null)
				{
					if (accumulativeQuestBase2.RequiredCount - accumulativeQuestBase2.CurrentCount < accumulativeQuestBase.RequiredCount - accumulativeQuestBase.CurrentCount)
					{
						questInfo = questInfo2;
					}
				}
				else if (questInfo.Quest.CalculateProgress() < questInfo2.Quest.CalculateProgress())
				{
					questInfo = questInfo2;
				}
			}
			return questInfo;
		}

		public IDictionary<int, QuestBase> GetActiveQuests()
		{
			IEnumerable<int> first = _previousQuests.Keys.Concat(_currentQuests.Keys);
			List<QuestBase> tutorialQuests = _tutorialQuests;
			if (_003C_003Ef__am_0024cache1B == null)
			{
				_003C_003Ef__am_0024cache1B = _003CGetActiveQuests_003Em__3D8;
			}
			IEnumerable<int> source = first.Concat(tutorialQuests.Select(_003C_003Ef__am_0024cache1B)).Distinct();
			if (_003C_003Ef__am_0024cache1C == null)
			{
				_003C_003Ef__am_0024cache1C = _003CGetActiveQuests_003Em__3D9;
			}
			return source.ToDictionary(_003C_003Ef__am_0024cache1C, GetActiveQuestBySlot);
		}

		internal bool TryRemoveTutorialQuest(string questId)
		{
			_003CTryRemoveTutorialQuest_003Ec__AnonStorey2E8 _003CTryRemoveTutorialQuest_003Ec__AnonStorey2E = new _003CTryRemoveTutorialQuest_003Ec__AnonStorey2E8();
			_003CTryRemoveTutorialQuest_003Ec__AnonStorey2E.questId = questId;
			if (_003CTryRemoveTutorialQuest_003Ec__AnonStorey2E.questId == null)
			{
				return false;
			}
			int num = _tutorialQuests.FindIndex(_003CTryRemoveTutorialQuest_003Ec__AnonStorey2E._003C_003Em__3DA);
			if (num < 0)
			{
				return false;
			}
			_tutorialQuests.RemoveAt(num);
			_dirty = true;
			return true;
		}

		private List<QuestBase> GetActiveQuestsBySlotReference(int slot, bool ignoreTutorialQuests = false)
		{
			if (!ignoreTutorialQuests)
			{
				QuestBase activeTutorialQuest = GetActiveTutorialQuest();
				if (activeTutorialQuest != null && activeTutorialQuest.Slot == slot)
				{
					return _tutorialQuests;
				}
			}
			List<QuestBase> value;
			_previousQuests.TryGetValue(slot, out value);
			List<QuestBase> o = value;
			if (_003C_003Ef__am_0024cache1D == null)
			{
				_003C_003Ef__am_0024cache1D = _003CGetActiveQuestsBySlotReference_003Em__3DB;
			}
			if (o.Map(_003C_003Ef__am_0024cache1D))
			{
				return value;
			}
			List<QuestBase> value2;
			if (_currentQuests.TryGetValue(slot, out value2))
			{
				List<QuestBase> o2 = value2;
				if (_003C_003Ef__am_0024cache1E == null)
				{
					_003C_003Ef__am_0024cache1E = _003CGetActiveQuestsBySlotReference_003Em__3DC;
				}
				if (o2.Map(_003C_003Ef__am_0024cache1E))
				{
					return value2;
				}
			}
			return value;
		}

		private IList<QuestBase> GetActiveQuestsBySlot(int slot, bool ignoreTutorialQuests = false)
		{
			List<QuestBase> activeQuestsBySlotReference = GetActiveQuestsBySlotReference(slot, ignoreTutorialQuests);
			if (activeQuestsBySlotReference == null)
			{
				return new List<QuestBase>();
			}
			return new List<QuestBase>(activeQuestsBySlotReference);
		}

		private QuestBase GetTutorialQuestById(string id)
		{
			if (id == null)
			{
				return null;
			}
			QuestBase activeTutorialQuest = GetActiveTutorialQuest();
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

		private QuestBase GetActiveTutorialQuest()
		{
			if (_tutorialQuests.Count == 0)
			{
				return null;
			}
			foreach (QuestBase tutorialQuest in _tutorialQuests)
			{
				if (tutorialQuest.Rewarded)
				{
					continue;
				}
				return tutorialQuest;
			}
			return null;
		}

		private QuestBase GetQuestById(string id)
		{
			_003CGetQuestById_003Ec__AnonStorey2E9 _003CGetQuestById_003Ec__AnonStorey2E = new _003CGetQuestById_003Ec__AnonStorey2E9();
			_003CGetQuestById_003Ec__AnonStorey2E.id = id;
			if (_003CGetQuestById_003Ec__AnonStorey2E.id == null)
			{
				throw new ArgumentNullException("id");
			}
			IEnumerable<int> first = _previousQuests.Keys.Concat(_currentQuests.Keys);
			List<QuestBase> tutorialQuests = _tutorialQuests;
			if (_003C_003Ef__am_0024cache1F == null)
			{
				_003C_003Ef__am_0024cache1F = _003CGetQuestById_003Em__3DD;
			}
			IEnumerable<int> source = first.Concat(tutorialQuests.Select(_003C_003Ef__am_0024cache1F)).Distinct();
			IEnumerable<QuestBase> source2 = source.Select(GetActiveQuestBySlot);
			return source2.FirstOrDefault(_003CGetQuestById_003Ec__AnonStorey2E._003C_003Em__3DE);
		}

		[Obsolete]
		public AccumulativeQuestBase GetRandomInProgressAccumQuest()
		{
			ICollection<List<QuestBase>> values = _previousQuests.Values;
			if (_003C_003Ef__am_0024cache20 == null)
			{
				_003C_003Ef__am_0024cache20 = _003CGetRandomInProgressAccumQuest_003Em__3DF;
			}
			IEnumerable<List<QuestBase>> source = values.Where(_003C_003Ef__am_0024cache20);
			if (_003C_003Ef__am_0024cache21 == null)
			{
				_003C_003Ef__am_0024cache21 = _003CGetRandomInProgressAccumQuest_003Em__3E0;
			}
			IEnumerable<QuestBase> source2 = source.Select(_003C_003Ef__am_0024cache21);
			if (_003C_003Ef__am_0024cache22 == null)
			{
				_003C_003Ef__am_0024cache22 = _003CGetRandomInProgressAccumQuest_003Em__3E1;
			}
			List<AccumulativeQuestBase> list = source2.Where(_003C_003Ef__am_0024cache22).OfType<AccumulativeQuestBase>().ToList();
			if (list.Count > 0)
			{
				return list[UnityEngine.Random.Range(0, list.Count)];
			}
			ICollection<List<QuestBase>> values2 = _previousQuests.Values;
			if (_003C_003Ef__am_0024cache23 == null)
			{
				_003C_003Ef__am_0024cache23 = _003CGetRandomInProgressAccumQuest_003Em__3E2;
			}
			IEnumerable<List<QuestBase>> source3 = values2.Where(_003C_003Ef__am_0024cache23);
			if (_003C_003Ef__am_0024cache24 == null)
			{
				_003C_003Ef__am_0024cache24 = _003CGetRandomInProgressAccumQuest_003Em__3E3;
			}
			IEnumerable<QuestBase> source4 = source3.Select(_003C_003Ef__am_0024cache24);
			if (_003C_003Ef__am_0024cache25 == null)
			{
				_003C_003Ef__am_0024cache25 = _003CGetRandomInProgressAccumQuest_003Em__3E4;
			}
			List<AccumulativeQuestBase> list2 = source4.Where(_003C_003Ef__am_0024cache25).OfType<AccumulativeQuestBase>().ToList();
			if (list2.Count > 0)
			{
				return list2[UnityEngine.Random.Range(0, list2.Count)];
			}
			ICollection<List<QuestBase>> values3 = _currentQuests.Values;
			if (_003C_003Ef__am_0024cache26 == null)
			{
				_003C_003Ef__am_0024cache26 = _003CGetRandomInProgressAccumQuest_003Em__3E5;
			}
			IEnumerable<List<QuestBase>> source5 = values3.Where(_003C_003Ef__am_0024cache26);
			if (_003C_003Ef__am_0024cache27 == null)
			{
				_003C_003Ef__am_0024cache27 = _003CGetRandomInProgressAccumQuest_003Em__3E6;
			}
			IEnumerable<QuestBase> source6 = source5.Select(_003C_003Ef__am_0024cache27);
			if (_003C_003Ef__am_0024cache28 == null)
			{
				_003C_003Ef__am_0024cache28 = _003CGetRandomInProgressAccumQuest_003Em__3E7;
			}
			AccumulativeQuestBase[] array = source6.Where(_003C_003Ef__am_0024cache28).OfType<AccumulativeQuestBase>().ToArray();
			if (array.Length > 0)
			{
				return array[UnityEngine.Random.Range(0, array.Length)];
			}
			ICollection<List<QuestBase>> values4 = _currentQuests.Values;
			if (_003C_003Ef__am_0024cache29 == null)
			{
				_003C_003Ef__am_0024cache29 = _003CGetRandomInProgressAccumQuest_003Em__3E8;
			}
			IEnumerable<List<QuestBase>> source7 = values4.Where(_003C_003Ef__am_0024cache29);
			if (_003C_003Ef__am_0024cache2A == null)
			{
				_003C_003Ef__am_0024cache2A = _003CGetRandomInProgressAccumQuest_003Em__3E9;
			}
			IEnumerable<QuestBase> source8 = source7.Select(_003C_003Ef__am_0024cache2A);
			if (_003C_003Ef__am_0024cache2B == null)
			{
				_003C_003Ef__am_0024cache2B = _003CGetRandomInProgressAccumQuest_003Em__3EA;
			}
			AccumulativeQuestBase[] array2 = source8.Where(_003C_003Ef__am_0024cache2B).OfType<AccumulativeQuestBase>().ToArray();
			if (array2.Length > 0)
			{
				return array2[UnityEngine.Random.Range(0, array2.Length)];
			}
			return null;
		}

		public bool HasUnrewaredAccumQuests()
		{
			ICollection<List<QuestBase>> values = _currentQuests.Values;
			if (_003C_003Ef__am_0024cache2C == null)
			{
				_003C_003Ef__am_0024cache2C = _003CHasUnrewaredAccumQuests_003Em__3EB;
			}
			IEnumerable<List<QuestBase>> source = values.Where(_003C_003Ef__am_0024cache2C);
			if (_003C_003Ef__am_0024cache2D == null)
			{
				_003C_003Ef__am_0024cache2D = _003CHasUnrewaredAccumQuests_003Em__3EC;
			}
			IEnumerable<QuestBase> first = source.Select(_003C_003Ef__am_0024cache2D);
			ICollection<List<QuestBase>> values2 = _previousQuests.Values;
			if (_003C_003Ef__am_0024cache2E == null)
			{
				_003C_003Ef__am_0024cache2E = _003CHasUnrewaredAccumQuests_003Em__3ED;
			}
			IEnumerable<List<QuestBase>> source2 = values2.Where(_003C_003Ef__am_0024cache2E);
			if (_003C_003Ef__am_0024cache2F == null)
			{
				_003C_003Ef__am_0024cache2F = _003CHasUnrewaredAccumQuests_003Em__3EE;
			}
			IEnumerable<QuestBase> second = source2.Select(_003C_003Ef__am_0024cache2F);
			IEnumerable<AccumulativeQuestBase> source3 = first.Concat(second).Concat(_tutorialQuests).OfType<AccumulativeQuestBase>();
			if (_003C_003Ef__am_0024cache30 == null)
			{
				_003C_003Ef__am_0024cache30 = _003CHasUnrewaredAccumQuests_003Em__3EF;
			}
			return source3.Any(_003C_003Ef__am_0024cache30);
		}

		public bool IsDirty()
		{
			int result;
			if (!_dirty)
			{
				ICollection<List<QuestBase>> values = _currentQuests.Values;
				if (_003C_003Ef__am_0024cache31 == null)
				{
					_003C_003Ef__am_0024cache31 = _003CIsDirty_003Em__3F0;
				}
				IEnumerable<QuestBase> source = values.SelectMany(_003C_003Ef__am_0024cache31);
				if (_003C_003Ef__am_0024cache32 == null)
				{
					_003C_003Ef__am_0024cache32 = _003CIsDirty_003Em__3F1;
				}
				if (!source.Any(_003C_003Ef__am_0024cache32))
				{
					ICollection<List<QuestBase>> values2 = _previousQuests.Values;
					if (_003C_003Ef__am_0024cache33 == null)
					{
						_003C_003Ef__am_0024cache33 = _003CIsDirty_003Em__3F2;
					}
					IEnumerable<QuestBase> source2 = values2.SelectMany(_003C_003Ef__am_0024cache33);
					if (_003C_003Ef__am_0024cache34 == null)
					{
						_003C_003Ef__am_0024cache34 = _003CIsDirty_003Em__3F3;
					}
					if (!source2.Any(_003C_003Ef__am_0024cache34))
					{
						List<QuestBase> tutorialQuests = _tutorialQuests;
						if (_003C_003Ef__am_0024cache35 == null)
						{
							_003C_003Ef__am_0024cache35 = _003CIsDirty_003Em__3F4;
						}
						result = (tutorialQuests.Any(_003C_003Ef__am_0024cache35) ? 1 : 0);
						goto IL_00de;
					}
				}
			}
			result = 1;
			goto IL_00de;
			IL_00de:
			return (byte)result != 0;
		}

		public void SetClean()
		{
			foreach (List<QuestBase> value in _currentQuests.Values)
			{
				foreach (QuestBase item in value)
				{
					item.SetClean();
				}
			}
			foreach (List<QuestBase> value2 in _previousQuests.Values)
			{
				foreach (QuestBase item2 in value2)
				{
					item2.SetClean();
				}
			}
			_dirty = false;
		}

		private void ClearQuests(IDictionary<int, List<QuestBase>> quests)
		{
			if (quests == null)
			{
				return;
			}
			foreach (KeyValuePair<int, List<QuestBase>> quest in quests)
			{
				quest.Value.Clear();
			}
			quests.Clear();
		}

		private static IDictionary<int, List<Dictionary<string, object>>> ExtractQuests(Dictionary<string, object> rawQuests)
		{
			Dictionary<int, List<Dictionary<string, object>>> dictionary = new Dictionary<int, List<Dictionary<string, object>>>();
			foreach (KeyValuePair<string, object> rawQuest in rawQuests)
			{
				int result;
				if (int.TryParse(rawQuest.Key, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
				{
					List<object> list = rawQuest.Value as List<object>;
					if (list == null)
					{
						dictionary[result] = new List<Dictionary<string, object>>();
					}
					else
					{
						List<Dictionary<string, object>> list3 = (dictionary[result] = list.OfType<Dictionary<string, object>>().ToList());
					}
				}
			}
			return dictionary;
		}

		private static IDictionary<int, List<Dictionary<string, object>>> FilterQuests(Dictionary<string, object> rawQuests, Difficulty[] allowedDifficulties, IDictionary<int, List<QuestBase>> existingQuests)
		{
			//Discarded unreachable code: IL_00e6
			if (allowedDifficulties == null)
			{
				throw new ArgumentNullException("allowedDifficulties");
			}
			if (allowedDifficulties.Length == 0)
			{
				throw new ArgumentException("List of difficulties should not be empty.", "allowedDifficulties");
			}
			if (existingQuests == null)
			{
				existingQuests = new Dictionary<int, List<QuestBase>>();
			}
			Dictionary<int, Dictionary<string, Dictionary<string, object>>> dictionary = new Dictionary<int, Dictionary<string, Dictionary<string, object>>>();
			foreach (KeyValuePair<string, object> rawQuest in rawQuests)
			{
				Dictionary<string, object> dictionary2 = rawQuest.Value as Dictionary<string, object>;
				object value;
				if (dictionary2 == null || !dictionary2.TryGetValue("slot", out value) || !QuestConstants.IsSupported(rawQuest.Key))
				{
					continue;
				}
				try
				{
					int key = Convert.ToInt32(value, NumberFormatInfo.InvariantInfo);
					Dictionary<string, Dictionary<string, object>> value2;
					if (!dictionary.TryGetValue(key, out value2))
					{
						value2 = (dictionary[key] = new Dictionary<string, Dictionary<string, object>>(3));
					}
					value2[rawQuest.Key] = dictionary2;
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			List<Difficulty> list = new List<Difficulty>(dictionary.Count);
			for (int i = 0; i != dictionary.Count; i++)
			{
				Difficulty item = allowedDifficulties[i % allowedDifficulties.Length];
				list.Add(item);
			}
			ShuffleInPlace(list);
			Dictionary<int, List<Dictionary<string, object>>> dictionary4 = new Dictionary<int, List<Dictionary<string, object>>>();
			Dictionary<int, Dictionary<string, Dictionary<string, object>>>.Enumerator enumerator2 = dictionary.GetEnumerator();
			List<Difficulty> list2 = new List<Difficulty>();
			list2.Add(Difficulty.Easy);
			list2.Add(Difficulty.Normal);
			list2.Add(Difficulty.Hard);
			List<Difficulty> source = list2;
			int num = 0;
			while (enumerator2.MoveNext())
			{
				_003CFilterQuests_003Ec__AnonStorey2EA _003CFilterQuests_003Ec__AnonStorey2EA = new _003CFilterQuests_003Ec__AnonStorey2EA();
				int key2 = enumerator2.Current.Key;
				Dictionary<string, Dictionary<string, object>> value3 = enumerator2.Current.Value;
				List<QuestBase> value4;
				bool flag = existingQuests.TryGetValue(key2, out value4);
				_003CFilterQuests_003Ec__AnonStorey2EA.chosenDifficulty = list[num];
				_003CFilterQuests_003Ec__AnonStorey2EA.chosenDifficultyKey = QuestConstants.GetDifficultyKey(_003CFilterQuests_003Ec__AnonStorey2EA.chosenDifficulty);
				List<KeyValuePair<string, Dictionary<string, object>>> list3 = value3.Where(_003CFilterQuests_003Ec__AnonStorey2EA._003C_003Em__3F5).ToList();
				if (list3.Count == 0)
				{
					value3.Clear();
				}
				else
				{
					if (list3.Count > 1)
					{
						_003CFilterQuests_003Ec__AnonStorey2EB _003CFilterQuests_003Ec__AnonStorey2EB = new _003CFilterQuests_003Ec__AnonStorey2EB();
						List<QuestBase> o = value4;
						if (_003C_003Ef__am_0024cache36 == null)
						{
							_003C_003Ef__am_0024cache36 = _003CFilterQuests_003Em__3F6;
						}
						QuestBase o2 = o.Map(_003C_003Ef__am_0024cache36);
						if (_003C_003Ef__am_0024cache37 == null)
						{
							_003C_003Ef__am_0024cache37 = _003CFilterQuests_003Em__3F7;
						}
						_003CFilterQuests_003Ec__AnonStorey2EB.existingQuestId = o2.Map(_003C_003Ef__am_0024cache37);
						list3.RemoveAll(_003CFilterQuests_003Ec__AnonStorey2EB._003C_003Em__3F8);
					}
					List<int> list4 = Enumerable.Range(0, list3.Count).ToList();
					ShuffleInPlace(list4);
					KeyValuePair<string, Dictionary<string, object>> keyValuePair = list3[list4[0]];
					keyValuePair.Value["id"] = keyValuePair.Key;
					value3.Clear();
					value3[keyValuePair.Key] = keyValuePair.Value;
					List<Dictionary<string, object>> list5 = new List<Dictionary<string, object>>(2);
					Dictionary<string, object> value5 = keyValuePair.Value;
					if (_003C_003Ef__am_0024cache38 == null)
					{
						_003C_003Ef__am_0024cache38 = _003CFilterQuests_003Em__3F9;
					}
					Func<KeyValuePair<string, object>, string> keySelector = _003C_003Ef__am_0024cache38;
					if (_003C_003Ef__am_0024cache39 == null)
					{
						_003C_003Ef__am_0024cache39 = _003CFilterQuests_003Em__3FA;
					}
					list5.Add(value5.ToDictionary(keySelector, _003C_003Ef__am_0024cache39));
					List<Dictionary<string, object>> list6 = list5;
					List<QuestBase> o3 = value4;
					if (_003C_003Ef__am_0024cache3A == null)
					{
						_003C_003Ef__am_0024cache3A = _003CFilterQuests_003Em__3FB;
					}
					if (o3.Map(_003C_003Ef__am_0024cache3A, true))
					{
						KeyValuePair<string, Dictionary<string, object>> keyValuePair2 = list3[list4[list4.Count - 1]];
						keyValuePair2.Value["id"] = keyValuePair2.Key;
						Dictionary<string, object> value6 = keyValuePair2.Value;
						if (_003C_003Ef__am_0024cache3B == null)
						{
							_003C_003Ef__am_0024cache3B = _003CFilterQuests_003Em__3FC;
						}
						Func<KeyValuePair<string, object>, string> keySelector2 = _003C_003Ef__am_0024cache3B;
						if (_003C_003Ef__am_0024cache3C == null)
						{
							_003C_003Ef__am_0024cache3C = _003CFilterQuests_003Em__3FD;
						}
						list6.Add(value6.ToDictionary(keySelector2, _003C_003Ef__am_0024cache3C));
					}
					dictionary4[key2] = list6;
					IEnumerable<Difficulty> enumerable = source.Where(_003CFilterQuests_003Ec__AnonStorey2EA._003C_003Em__3FE);
					foreach (Difficulty item2 in enumerable)
					{
						string difficultyKey = QuestConstants.GetDifficultyKey(item2);
						foreach (Dictionary<string, object> item3 in list6)
						{
							item3.Remove(difficultyKey);
						}
					}
				}
				num++;
			}
			return dictionary4;
		}

		private static string ExtractMapFromQuestDescription(Dictionary<string, object> q, bool restore)
		{
			if (q == null || q.Count == 0)
			{
				return string.Empty;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("map", out value))
				{
					return null;
				}
				return Convert.ToString(value);
			}
			string[] supportedMaps = GetSupportedMaps();
			object value2;
			if (!q.TryGetValue("maps", out value2))
			{
				return supportedMaps[UnityEngine.Random.Range(0, supportedMaps.Length - 1)];
			}
			List<object> list = value2 as List<object>;
			if (list == null)
			{
				return string.Empty;
			}
			string[] array = list.OfType<string>().Intersect(supportedMaps).ToArray();
			if (array.Length == 0)
			{
				return string.Empty;
			}
			return array[UnityEngine.Random.Range(0, array.Length - 1)];
		}

		private static string[] GetSupportedMaps()
		{
			if (_supportedMapsCache != null && _supportedMapsCache.IsAlive)
			{
				return (string[])_supportedMapsCache.Target;
			}
			ExperienceController sharedController = ExperienceController.sharedController;
			if (_003C_003Ef__am_0024cache3D == null)
			{
				_003C_003Ef__am_0024cache3D = _003CGetSupportedMaps_003Em__3FF;
			}
			int level = sharedController.Map(_003C_003Ef__am_0024cache3D, 1);
			HashSet<TypeModeGame> unlockedModesByLevel = SceneInfoController.GetUnlockedModesByLevel(level);
			unlockedModesByLevel.Remove(TypeModeGame.Dater);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (SceneInfo allScene in SceneInfoController.instance.allScenes)
			{
				if (allScene.isPremium || allScene.NameScene == "Developer_Scene")
				{
					continue;
				}
				foreach (TypeModeGame item in unlockedModesByLevel)
				{
					if (allScene.IsAvaliableForMode(item))
					{
						hashSet.Add(allScene.NameScene);
					}
				}
			}
			string[] array = hashSet.ToArray();
			_supportedMapsCache = new WeakReference(array, false);
			return array;
		}

		private static ShopNGUIController.CategoryNames? ExtractWeaponSlotFromQuestDescription(Dictionary<string, object> q, bool restore, HashSet<ShopNGUIController.CategoryNames> excluded)
		{
			if (q == null || q.Count == 0)
			{
				return null;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("weaponSlot", out value))
				{
					return null;
				}
				return QuestConstants.ParseWeaponSlot(Convert.ToString(value));
			}
			if (excluded == null)
			{
				excluded = new HashSet<ShopNGUIController.CategoryNames>();
			}
			List<ShopNGUIController.CategoryNames> list = Enum.GetValues(typeof(ShopNGUIController.CategoryNames)).Cast<ShopNGUIController.CategoryNames>().Where(ShopNGUIController.IsWeaponCategory)
				.ToList();
			object value2;
			if (!q.TryGetValue("weaponSlots", out value2))
			{
				List<ShopNGUIController.CategoryNames> list2 = list.Except(excluded).ToList();
				list2 = ((list2.Count <= 0) ? list : list2);
				return list2[UnityEngine.Random.Range(0, list2.Count - 1)];
			}
			List<object> list3 = value2 as List<object>;
			if (list3 == null)
			{
				return null;
			}
			IEnumerable<ShopNGUIController.CategoryNames?> source = list3.OfType<string>().Select(QuestConstants.ParseWeaponSlot);
			if (_003C_003Ef__am_0024cache3E == null)
			{
				_003C_003Ef__am_0024cache3E = _003CExtractWeaponSlotFromQuestDescription_003Em__400;
			}
			IEnumerable<ShopNGUIController.CategoryNames?> source2 = source.Where(_003C_003Ef__am_0024cache3E);
			if (_003C_003Ef__am_0024cache3F == null)
			{
				_003C_003Ef__am_0024cache3F = _003CExtractWeaponSlotFromQuestDescription_003Em__401;
			}
			List<ShopNGUIController.CategoryNames> list4 = source2.Select(_003C_003Ef__am_0024cache3F).Intersect(list).ToList();
			if (list4.Count == 0)
			{
				return null;
			}
			List<ShopNGUIController.CategoryNames> list5 = list4.Except(excluded).ToList();
			list5 = ((list5.Count <= 0) ? list4 : list5);
			return list5[UnityEngine.Random.Range(0, list5.Count - 1)];
		}

		private static ConnectSceneNGUIController.RegimGame? ExtractModeFromQuestDescription(Dictionary<string, object> q, bool restore, string questId)
		{
			if (q == null || q.Count == 0)
			{
				return null;
			}
			if (restore)
			{
				object value;
				if (!q.TryGetValue("mode", out value))
				{
					return null;
				}
				return QuestConstants.ParseMode(Convert.ToString(value));
			}
			List<ConnectSceneNGUIController.RegimGame> list = new List<ConnectSceneNGUIController.RegimGame>(GetSupportedModes());
			if ("killInMode".Equals(questId, StringComparison.OrdinalIgnoreCase))
			{
				list.Remove(ConnectSceneNGUIController.RegimGame.TimeBattle);
			}
			object value2;
			if (!q.TryGetValue("modes", out value2))
			{
				return (list.Count <= 0) ? null : new ConnectSceneNGUIController.RegimGame?(list[UnityEngine.Random.Range(0, list.Count - 1)]);
			}
			List<object> list2 = value2 as List<object>;
			if (list2 == null)
			{
				return null;
			}
			IEnumerable<ConnectSceneNGUIController.RegimGame?> source = list2.OfType<string>().Select(QuestConstants.ParseMode);
			if (_003C_003Ef__am_0024cache40 == null)
			{
				_003C_003Ef__am_0024cache40 = _003CExtractModeFromQuestDescription_003Em__402;
			}
			IEnumerable<ConnectSceneNGUIController.RegimGame?> source2 = source.Where(_003C_003Ef__am_0024cache40);
			if (_003C_003Ef__am_0024cache41 == null)
			{
				_003C_003Ef__am_0024cache41 = _003CExtractModeFromQuestDescription_003Em__403;
			}
			List<ConnectSceneNGUIController.RegimGame> list3 = source2.Select(_003C_003Ef__am_0024cache41).Intersect(list).ToList();
			if (list3.Count == 0)
			{
				return null;
			}
			return list3[UnityEngine.Random.Range(0, list3.Count - 1)];
		}

		private static ConnectSceneNGUIController.RegimGame[] GetSupportedModes()
		{
			if (_supportedModesCache != null && _supportedModesCache.IsAlive)
			{
				return (ConnectSceneNGUIController.RegimGame[])_supportedModesCache.Target;
			}
			ExperienceController sharedController = ExperienceController.sharedController;
			if (_003C_003Ef__am_0024cache42 == null)
			{
				_003C_003Ef__am_0024cache42 = _003CGetSupportedModes_003Em__404;
			}
			int level = sharedController.Map(_003C_003Ef__am_0024cache42, 1);
			HashSet<TypeModeGame> unlockedModesByLevel = SceneInfoController.GetUnlockedModesByLevel(level);
			HashSet<ConnectSceneNGUIController.RegimGame> source = SceneInfoController.SelectModes(unlockedModesByLevel);
			ConnectSceneNGUIController.RegimGame[] array = source.ToArray();
			_supportedModesCache = new WeakReference(array, false);
			return array;
		}

		private static void ShuffleInPlace<T>(List<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.Count >= 2)
			{
				for (int num = list.Count - 1; num >= 1; num--)
				{
					int index = UnityEngine.Random.Range(0, num);
					T value = list[index];
					list[index] = list[num];
					list[num] = value;
				}
			}
		}

		private static List<T> Shuffle<T>(IEnumerable<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			List<T> list2 = list.ToList();
			ShuffleInPlace(list2);
			return list2;
		}

		public void FilterFulfilledTutorialQuests()
		{
			List<QuestBase> tutorialQuests = _tutorialQuests;
			if (_003C_003Ef__am_0024cache43 == null)
			{
				_003C_003Ef__am_0024cache43 = _003CFilterFulfilledTutorialQuests_003Em__405;
			}
			tutorialQuests.RemoveAll(_003C_003Ef__am_0024cache43);
		}

		private void OnQuestChangedCheckCompletion(object sender, EventArgs e)
		{
			_003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC _003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC = new _003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC();
			_003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC._003C_003Ef__this = this;
			_003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC.quest = sender as QuestBase;
			if (_003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC.quest == null || !(_003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC.quest.CalculateProgress() >= 1m))
			{
				return;
			}
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.OnQuestChangedCheckCompletion({1})", GetType().Name, _003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC.quest.Id);
			ScopeLogger scopeLogger = new ScopeLogger(callee, Defs.IsDeveloperBuild);
			try
			{
				this.QuestCompleted.Do(_003COnQuestChangedCheckCompletion_003Ec__AnonStorey2EC._003C_003Em__406);
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		private void HandleWin(object sender, WinEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleWin(): " + e);
			}
			QuestBase questById = GetQuestById("winInMap");
			if (questById != null)
			{
				MapAccumulativeQuest mapAccumulativeQuest = questById as MapAccumulativeQuest;
				if (mapAccumulativeQuest != null)
				{
					mapAccumulativeQuest.IncrementIf(mapAccumulativeQuest.Map.Equals(e.Map, StringComparison.Ordinal));
				}
			}
			questById = GetQuestById("winInMode");
			if (questById != null)
			{
				ModeAccumulativeQuest modeAccumulativeQuest = questById as ModeAccumulativeQuest;
				if (modeAccumulativeQuest != null)
				{
					modeAccumulativeQuest.IncrementIf(modeAccumulativeQuest.Mode == e.Mode);
				}
			}
		}

		private void HandleKillOtherPlayer(object sender, KillOtherPlayerEventArgs e)
		{
			_003CHandleKillOtherPlayer_003Ec__AnonStorey2ED _003CHandleKillOtherPlayer_003Ec__AnonStorey2ED = new _003CHandleKillOtherPlayer_003Ec__AnonStorey2ED();
			_003CHandleKillOtherPlayer_003Ec__AnonStorey2ED.e = e;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillOtherPlayer(): " + _003CHandleKillOtherPlayer_003Ec__AnonStorey2ED.e);
			}
			QuestBase questById = GetQuestById("killInMode");
			if (questById != null)
			{
				(questById as ModeAccumulativeQuest).Do(_003CHandleKillOtherPlayer_003Ec__AnonStorey2ED._003C_003Em__407);
			}
			questById = GetQuestById("killWithWeapon");
			if (questById != null)
			{
				(questById as WeaponSlotAccumulativeQuest).Do(_003CHandleKillOtherPlayer_003Ec__AnonStorey2ED._003C_003Em__408);
			}
			string[] source = new string[3] { "killViaHeadshot", "killWithGrenade", "revenge" };
			IEnumerable<SimpleAccumulativeQuest> source2 = source.Select(GetQuestById).OfType<SimpleAccumulativeQuest>();
			if (_003C_003Ef__am_0024cache44 == null)
			{
				_003C_003Ef__am_0024cache44 = _003CHandleKillOtherPlayer_003Em__409;
			}
			Func<SimpleAccumulativeQuest, string> keySelector = _003C_003Ef__am_0024cache44;
			if (_003C_003Ef__am_0024cache45 == null)
			{
				_003C_003Ef__am_0024cache45 = _003CHandleKillOtherPlayer_003Em__40A;
			}
			Dictionary<string, SimpleAccumulativeQuest> dictionary = source2.ToDictionary(keySelector, _003C_003Ef__am_0024cache45);
			SimpleAccumulativeQuest value;
			if (dictionary.TryGetValue("killViaHeadshot", out value))
			{
				value.IncrementIf(_003CHandleKillOtherPlayer_003Ec__AnonStorey2ED.e.Headshot);
			}
			if (dictionary.TryGetValue("killWithGrenade", out value))
			{
				value.IncrementIf(_003CHandleKillOtherPlayer_003Ec__AnonStorey2ED.e.Grenade);
			}
			if (dictionary.TryGetValue("revenge", out value))
			{
				value.IncrementIf(_003CHandleKillOtherPlayer_003Ec__AnonStorey2ED.e.Revenge);
			}
		}

		private void HandleKillOtherPlayerWithFlag(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillOtherPlayerWithFlag(): " + e);
			}
			QuestBase questById = GetQuestById("killFlagCarriers");
			if (questById != null)
			{
				SimpleAccumulativeQuest o = questById as SimpleAccumulativeQuest;
				if (_003C_003Ef__am_0024cache46 == null)
				{
					_003C_003Ef__am_0024cache46 = _003CHandleKillOtherPlayerWithFlag_003Em__40B;
				}
				o.Do(_003C_003Ef__am_0024cache46);
			}
		}

		private void HandleCapture(object sender, CaptureEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleCapture(): " + e);
			}
			string[] source = new string[2] { "captureFlags", "capturePoints" };
			IEnumerable<SimpleAccumulativeQuest> source2 = source.Select(GetQuestById).OfType<SimpleAccumulativeQuest>();
			if (_003C_003Ef__am_0024cache47 == null)
			{
				_003C_003Ef__am_0024cache47 = _003CHandleCapture_003Em__40C;
			}
			Func<SimpleAccumulativeQuest, string> keySelector = _003C_003Ef__am_0024cache47;
			if (_003C_003Ef__am_0024cache48 == null)
			{
				_003C_003Ef__am_0024cache48 = _003CHandleCapture_003Em__40D;
			}
			Dictionary<string, SimpleAccumulativeQuest> dictionary = source2.ToDictionary(keySelector, _003C_003Ef__am_0024cache48);
			SimpleAccumulativeQuest value;
			if (dictionary.TryGetValue("capturePoints", out value))
			{
				value.IncrementIf(e.Mode == ConnectSceneNGUIController.RegimGame.CapturePoints);
			}
			if (dictionary.TryGetValue("captureFlags", out value))
			{
				value.IncrementIf(e.Mode == ConnectSceneNGUIController.RegimGame.FlagCapture);
			}
		}

		private void HandleKillMonster(object sender, KillMonsterEventArgs e)
		{
			_003CHandleKillMonster_003Ec__AnonStorey2EE _003CHandleKillMonster_003Ec__AnonStorey2EE = new _003CHandleKillMonster_003Ec__AnonStorey2EE();
			_003CHandleKillMonster_003Ec__AnonStorey2EE.e = e;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleKillMonster(): " + _003CHandleKillMonster_003Ec__AnonStorey2EE.e);
			}
			QuestBase questById = GetQuestById("killInCampaign");
			if (questById != null)
			{
				(questById as SimpleAccumulativeQuest).Do(_003CHandleKillMonster_003Ec__AnonStorey2EE._003C_003Em__40E);
			}
			questById = GetQuestById("killNpcWithWeapon");
			if (questById != null)
			{
				(questById as WeaponSlotAccumulativeQuest).Do(_003CHandleKillMonster_003Ec__AnonStorey2EE._003C_003Em__40F);
			}
		}

		private void HandleBreakSeries(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleBreakSeries()");
			}
			QuestBase questById = GetQuestById("breakSeries");
			if (questById != null)
			{
				SimpleAccumulativeQuest o = questById as SimpleAccumulativeQuest;
				if (_003C_003Ef__am_0024cache49 == null)
				{
					_003C_003Ef__am_0024cache49 = _003CHandleBreakSeries_003Em__410;
				}
				o.Do(_003C_003Ef__am_0024cache49);
			}
		}

		private void HandleMakeSeries(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleMakeSeries()");
			}
			QuestBase questById = GetQuestById("makeSeries");
			if (questById != null)
			{
				SimpleAccumulativeQuest o = questById as SimpleAccumulativeQuest;
				if (_003C_003Ef__am_0024cache4A == null)
				{
					_003C_003Ef__am_0024cache4A = _003CHandleMakeSeries_003Em__411;
				}
				o.Do(_003C_003Ef__am_0024cache4A);
			}
		}

		private void HandleSurviveInArena(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleSurviveInArena()");
			}
			QuestBase questById = GetQuestById("surviveWavesInArena");
			if (questById != null)
			{
				SimpleAccumulativeQuest o = questById as SimpleAccumulativeQuest;
				if (_003C_003Ef__am_0024cache4B == null)
				{
					_003C_003Ef__am_0024cache4B = _003CHandleSurviveInArena_003Em__412;
				}
				o.Do(_003C_003Ef__am_0024cache4B);
			}
		}

		private void HandleGetGotcha(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("HandleGetGotcha()");
			}
			QuestBase tutorialQuestById = GetTutorialQuestById("getGotcha");
			if (tutorialQuestById != null)
			{
				SimpleAccumulativeQuest o = tutorialQuestById as SimpleAccumulativeQuest;
				if (_003C_003Ef__am_0024cache4C == null)
				{
					_003C_003Ef__am_0024cache4C = _003CHandleGetGotcha_003Em__413;
				}
				o.Do(_003C_003Ef__am_0024cache4C);
			}
		}

		private void HandleSocialInteraction(object sender, SocialInteractionEventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("HandleSocialInteraction('{0}')", e.Kind);
			}
			QuestBase tutorialQuestById = GetTutorialQuestById(e.Kind);
			if (tutorialQuestById != null)
			{
				SimpleAccumulativeQuest o = tutorialQuestById as SimpleAccumulativeQuest;
				if (_003C_003Ef__am_0024cache4D == null)
				{
					_003C_003Ef__am_0024cache4D = _003CHandleSocialInteraction_003Em__414;
				}
				o.Do(_003C_003Ef__am_0024cache4D);
			}
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			foreach (QuestBase tutorialQuest in _tutorialQuests)
			{
				tutorialQuest.Changed -= OnQuestChangedCheckCompletion;
			}
			IDictionary<int, List<QuestBase>> currentQuests = _currentQuests;
			if (_003C_003Ef__am_0024cache4E == null)
			{
				_003C_003Ef__am_0024cache4E = _003CDispose_003Em__415;
			}
			foreach (QuestBase item in currentQuests.SelectMany(_003C_003Ef__am_0024cache4E))
			{
				item.Changed -= OnQuestChangedCheckCompletion;
			}
			IDictionary<int, List<QuestBase>> previousQuests = _previousQuests;
			if (_003C_003Ef__am_0024cache4F == null)
			{
				_003C_003Ef__am_0024cache4F = _003CDispose_003Em__416;
			}
			foreach (QuestBase item2 in previousQuests.SelectMany(_003C_003Ef__am_0024cache4F))
			{
				item2.Changed -= OnQuestChangedCheckCompletion;
			}
			_events.Win -= HandleWin;
			_events.KillOtherPlayer -= HandleKillOtherPlayer;
			_events.KillOtherPlayerWithFlag -= HandleKillOtherPlayerWithFlag;
			_events.Capture -= HandleCapture;
			_events.KillMonster -= HandleKillMonster;
			_events.BreakSeries -= HandleBreakSeries;
			_events.MakeSeries -= HandleMakeSeries;
			_events.SurviveWaveInArena -= HandleSurviveInArena;
			this.QuestCompleted = null;
			_disposed = true;
		}

		[CompilerGenerated]
		private static bool _003Cget_AnyActiveQuest_003Em__3C8(KeyValuePair<int, QuestBase> q)
		{
			return !q.Value.Rewarded;
		}

		[CompilerGenerated]
		private static int _003CUpdateQuests_003Em__3C9(int s)
		{
			return s;
		}

		[CompilerGenerated]
		private IList<QuestBase> _003CUpdateQuests_003Em__3CA(int s)
		{
			return GetActiveQuestsBySlot(s, true);
		}

		[CompilerGenerated]
		private static bool _003CUpdateQuests_003Em__3CB(QuestBase q)
		{
			return !q.Rewarded;
		}

		[CompilerGenerated]
		private static bool _003CUpdateQuests_003Em__3CC(QuestBase q)
		{
			return q.CalculateProgress() < 1m && !q.Rewarded;
		}

		[CompilerGenerated]
		private static IEnumerable<Difficulty> _003CUpdateQuests_003Em__3CD(KeyValuePair<int, List<QuestBase>> kv)
		{
			List<QuestBase> value = kv.Value;
			if (_003C_003Ef__am_0024cache50 == null)
			{
				_003C_003Ef__am_0024cache50 = _003CUpdateQuests_003Em__417;
			}
			return value.Select(_003C_003Ef__am_0024cache50);
		}

		[CompilerGenerated]
		private static IEnumerable<QuestBase> _003CDebugDecrementDay_003Em__3CE(List<QuestBase> q)
		{
			return q;
		}

		[CompilerGenerated]
		private static IEnumerable<QuestBase> _003CDebugDecrementDay_003Em__3CF(List<QuestBase> q)
		{
			return q;
		}

		[CompilerGenerated]
		private static int _003CParseQuests_003Em__3D1(KeyValuePair<int, QuestBase> kv)
		{
			return kv.Key;
		}

		[CompilerGenerated]
		private static List<QuestBase> _003CParseQuests_003Em__3D2(KeyValuePair<int, QuestBase> kv)
		{
			return new List<QuestBase> { kv.Value };
		}

		[CompilerGenerated]
		private static QuestBase _003CGetActiveQuestBySlot_003Em__3D3(List<QuestBase> ps)
		{
			return ps.FirstOrDefault();
		}

		[CompilerGenerated]
		private static QuestBase _003CGetActiveQuestBySlot_003Em__3D4(List<QuestBase> cs)
		{
			return cs.FirstOrDefault();
		}

		[CompilerGenerated]
		private static int _003CGetRandomQuestInfo_003Em__3D6(QuestBase q)
		{
			return q.Slot;
		}

		[CompilerGenerated]
		private static bool _003CGetRandomQuestInfo_003Em__3D7(QuestInfo qi)
		{
			return qi.Quest != null && !qi.Quest.Rewarded;
		}

		[CompilerGenerated]
		private static int _003CGetActiveQuests_003Em__3D8(QuestBase q)
		{
			return q.Slot;
		}

		[CompilerGenerated]
		private static int _003CGetActiveQuests_003Em__3D9(int s)
		{
			return s;
		}

		[CompilerGenerated]
		private static bool _003CGetActiveQuestsBySlotReference_003Em__3DB(List<QuestBase> qs)
		{
			int result;
			if (qs.Count > 0)
			{
				if (_003C_003Ef__am_0024cache51 == null)
				{
					_003C_003Ef__am_0024cache51 = _003CGetActiveQuestsBySlotReference_003Em__418;
				}
				result = (qs.All(_003C_003Ef__am_0024cache51) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		[CompilerGenerated]
		private static bool _003CGetActiveQuestsBySlotReference_003Em__3DC(List<QuestBase> qs)
		{
			return qs.Count > 0;
		}

		[CompilerGenerated]
		private static int _003CGetQuestById_003Em__3DD(QuestBase q)
		{
			return q.Slot;
		}

		[CompilerGenerated]
		private static bool _003CGetRandomInProgressAccumQuest_003Em__3DF(List<QuestBase> qs)
		{
			return qs.Count > 0;
		}

		[CompilerGenerated]
		private static QuestBase _003CGetRandomInProgressAccumQuest_003Em__3E0(List<QuestBase> qs)
		{
			return qs.First();
		}

		[CompilerGenerated]
		private static bool _003CGetRandomInProgressAccumQuest_003Em__3E1(QuestBase q)
		{
			return q.CalculateProgress() < 1m;
		}

		[CompilerGenerated]
		private static bool _003CGetRandomInProgressAccumQuest_003Em__3E2(List<QuestBase> qs)
		{
			return qs.Count > 0;
		}

		[CompilerGenerated]
		private static QuestBase _003CGetRandomInProgressAccumQuest_003Em__3E3(List<QuestBase> qs)
		{
			return qs.First();
		}

		[CompilerGenerated]
		private static bool _003CGetRandomInProgressAccumQuest_003Em__3E4(QuestBase q)
		{
			return !q.Rewarded;
		}

		[CompilerGenerated]
		private static bool _003CGetRandomInProgressAccumQuest_003Em__3E5(List<QuestBase> qs)
		{
			return qs.Count > 0;
		}

		[CompilerGenerated]
		private static QuestBase _003CGetRandomInProgressAccumQuest_003Em__3E6(List<QuestBase> qs)
		{
			return qs.First();
		}

		[CompilerGenerated]
		private static bool _003CGetRandomInProgressAccumQuest_003Em__3E7(QuestBase q)
		{
			return q.CalculateProgress() < 1m;
		}

		[CompilerGenerated]
		private static bool _003CGetRandomInProgressAccumQuest_003Em__3E8(List<QuestBase> qs)
		{
			return qs.Count > 0;
		}

		[CompilerGenerated]
		private static QuestBase _003CGetRandomInProgressAccumQuest_003Em__3E9(List<QuestBase> qs)
		{
			return qs.First();
		}

		[CompilerGenerated]
		private static bool _003CGetRandomInProgressAccumQuest_003Em__3EA(QuestBase q)
		{
			return !q.Rewarded;
		}

		[CompilerGenerated]
		private static bool _003CHasUnrewaredAccumQuests_003Em__3EB(List<QuestBase> qs)
		{
			return qs.Count > 0;
		}

		[CompilerGenerated]
		private static QuestBase _003CHasUnrewaredAccumQuests_003Em__3EC(List<QuestBase> qs)
		{
			return qs.First();
		}

		[CompilerGenerated]
		private static bool _003CHasUnrewaredAccumQuests_003Em__3ED(List<QuestBase> qs)
		{
			return qs.Count > 0;
		}

		[CompilerGenerated]
		private static QuestBase _003CHasUnrewaredAccumQuests_003Em__3EE(List<QuestBase> qs)
		{
			return qs.First();
		}

		[CompilerGenerated]
		private static bool _003CHasUnrewaredAccumQuests_003Em__3EF(AccumulativeQuestBase q)
		{
			return q.CalculateProgress() >= 1m && !q.Rewarded;
		}

		[CompilerGenerated]
		private static IEnumerable<QuestBase> _003CIsDirty_003Em__3F0(List<QuestBase> q)
		{
			return q;
		}

		[CompilerGenerated]
		private static bool _003CIsDirty_003Em__3F1(QuestBase q)
		{
			return q.Dirty;
		}

		[CompilerGenerated]
		private static IEnumerable<QuestBase> _003CIsDirty_003Em__3F2(List<QuestBase> q)
		{
			return q;
		}

		[CompilerGenerated]
		private static bool _003CIsDirty_003Em__3F3(QuestBase q)
		{
			return q.Dirty;
		}

		[CompilerGenerated]
		private static bool _003CIsDirty_003Em__3F4(QuestBase q)
		{
			return q.Dirty;
		}

		[CompilerGenerated]
		private static QuestBase _003CFilterQuests_003Em__3F6(List<QuestBase> l)
		{
			return l.FirstOrDefault();
		}

		[CompilerGenerated]
		private static string _003CFilterQuests_003Em__3F7(QuestBase q)
		{
			return q.Id;
		}

		[CompilerGenerated]
		private static string _003CFilterQuests_003Em__3F9(KeyValuePair<string, object> kv)
		{
			return kv.Key;
		}

		[CompilerGenerated]
		private static object _003CFilterQuests_003Em__3FA(KeyValuePair<string, object> kv)
		{
			return kv.Value;
		}

		[CompilerGenerated]
		private static bool _003CFilterQuests_003Em__3FB(List<QuestBase> l)
		{
			return l.Count == 0;
		}

		[CompilerGenerated]
		private static string _003CFilterQuests_003Em__3FC(KeyValuePair<string, object> kv)
		{
			return kv.Key;
		}

		[CompilerGenerated]
		private static object _003CFilterQuests_003Em__3FD(KeyValuePair<string, object> kv)
		{
			return kv.Value;
		}

		[CompilerGenerated]
		private static int _003CGetSupportedMaps_003Em__3FF(ExperienceController xp)
		{
			return xp.currentLevel;
		}

		[CompilerGenerated]
		private static bool _003CExtractWeaponSlotFromQuestDescription_003Em__400(ShopNGUIController.CategoryNames? w)
		{
			return w.HasValue;
		}

		[CompilerGenerated]
		private static ShopNGUIController.CategoryNames _003CExtractWeaponSlotFromQuestDescription_003Em__401(ShopNGUIController.CategoryNames? w)
		{
			return w.Value;
		}

		[CompilerGenerated]
		private static bool _003CExtractModeFromQuestDescription_003Em__402(ConnectSceneNGUIController.RegimGame? m)
		{
			return m.HasValue;
		}

		[CompilerGenerated]
		private static ConnectSceneNGUIController.RegimGame _003CExtractModeFromQuestDescription_003Em__403(ConnectSceneNGUIController.RegimGame? m)
		{
			return m.Value;
		}

		[CompilerGenerated]
		private static int _003CGetSupportedModes_003Em__404(ExperienceController xp)
		{
			return xp.currentLevel;
		}

		[CompilerGenerated]
		private static bool _003CFilterFulfilledTutorialQuests_003Em__405(QuestBase tq)
		{
			return TutorialQuestManager.Instance.CheckQuestIfFulfilled(tq.Id) && tq.CalculateProgress() < 1m;
		}

		[CompilerGenerated]
		private static string _003CHandleKillOtherPlayer_003Em__409(SimpleAccumulativeQuest q)
		{
			return q.Id;
		}

		[CompilerGenerated]
		private static SimpleAccumulativeQuest _003CHandleKillOtherPlayer_003Em__40A(SimpleAccumulativeQuest q)
		{
			return q;
		}

		[CompilerGenerated]
		private static void _003CHandleKillOtherPlayerWithFlag_003Em__40B(SimpleAccumulativeQuest quest)
		{
			quest.Increment();
		}

		[CompilerGenerated]
		private static string _003CHandleCapture_003Em__40C(SimpleAccumulativeQuest q)
		{
			return q.Id;
		}

		[CompilerGenerated]
		private static SimpleAccumulativeQuest _003CHandleCapture_003Em__40D(SimpleAccumulativeQuest q)
		{
			return q;
		}

		[CompilerGenerated]
		private static void _003CHandleBreakSeries_003Em__410(SimpleAccumulativeQuest quest)
		{
			quest.Increment();
		}

		[CompilerGenerated]
		private static void _003CHandleMakeSeries_003Em__411(SimpleAccumulativeQuest quest)
		{
			quest.Increment();
		}

		[CompilerGenerated]
		private static void _003CHandleSurviveInArena_003Em__412(SimpleAccumulativeQuest quest)
		{
			quest.Increment();
		}

		[CompilerGenerated]
		private static void _003CHandleGetGotcha_003Em__413(SimpleAccumulativeQuest quest)
		{
			quest.Increment();
		}

		[CompilerGenerated]
		private static void _003CHandleSocialInteraction_003Em__414(SimpleAccumulativeQuest quest)
		{
			quest.Increment();
		}

		[CompilerGenerated]
		private static IEnumerable<QuestBase> _003CDispose_003Em__415(KeyValuePair<int, List<QuestBase>> kv)
		{
			return kv.Value;
		}

		[CompilerGenerated]
		private static IEnumerable<QuestBase> _003CDispose_003Em__416(KeyValuePair<int, List<QuestBase>> kv)
		{
			return kv.Value;
		}

		[CompilerGenerated]
		private static Difficulty _003CUpdateQuests_003Em__417(QuestBase q)
		{
			return q.Difficulty;
		}

		[CompilerGenerated]
		private static bool _003CGetActiveQuestsBySlotReference_003Em__418(QuestBase q)
		{
			return !q.Rewarded;
		}
	}
}
