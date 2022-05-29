using Rilisoft;
using Rilisoft.DictionaryExtensions;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

internal sealed class QuestSystem : MonoBehaviour
{
	internal const string QuestProgressKey = "QuestProgress";

	internal const string DefaultAvailabilityKey = "QuestSystem.DefaultAvailability";

	private const int _questConfigClientVersion = 28;

	private bool _enabled;

	private readonly static Lazy<QuestSystem> _instance;

	private Coroutine _getConfigLoopCoroutine;

	private Coroutine _getTutorialQuestsConfigLoopCoroutine;

	private Rilisoft.QuestProgress _questProgress;

	private DateTime? _startupTimeAccordingToServer;

	private EventHandler Updated;

	private EventHandler<QuestCompletedEventArgs> QuestCompleted;

	public bool AnyActiveQuest
	{
		get
		{
			return (!this.Enabled || this.QuestProgress == null ? false : this.QuestProgress.AnyActiveQuest);
		}
	}

	internal bool Enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			PlayerPrefs.SetInt("QuestSystem.DefaultAvailability", Convert.ToInt32(value));
			if (this._enabled == value)
			{
				return;
			}
			this._enabled = value;
			if (value)
			{
				this.InitializeQuestProgress();
			}
			else if (this._questProgress != null)
			{
				this._questProgress.Dispose();
				this._questProgress = null;
			}
			EventHandler updated = this.Updated;
			if (updated != null)
			{
				updated(this, EventArgs.Empty);
			}
		}
	}

	public static QuestSystem Instance
	{
		get
		{
			return QuestSystem._instance.Value;
		}
	}

	internal int QuestConfigClientVersion
	{
		get
		{
			return 28;
		}
	}

	public Rilisoft.QuestProgress QuestProgress
	{
		get
		{
			return this._questProgress;
		}
	}

	static QuestSystem()
	{
		QuestSystem._instance = new Lazy<QuestSystem>(new Func<QuestSystem>(QuestSystem.InitializeInstance));
	}

	public QuestSystem()
	{
	}

	internal void DebugDecrementDay()
	{
		if (!this.Enabled)
		{
			return;
		}
		if (this._questProgress != null)
		{
			this._questProgress.DebugDecrementDay();
		}
		this.Updated.Do<EventHandler>((EventHandler handler) => handler(this, EventArgs.Empty));
	}

	internal void ForceGetConfig()
	{
		this._getConfigLoopCoroutine.Do<Coroutine>(new Action<Coroutine>(this.StopCoroutine));
		this._getConfigLoopCoroutine = base.StartCoroutine(this.GetConfigLoopCoroutine(false));
	}

	[DebuggerHidden]
	private IEnumerator GetConfigLoopCoroutine(bool resumed)
	{
		QuestSystem.u003cGetConfigLoopCoroutineu003ec__Iterator186 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetConfigOnceCoroutine(bool resumed)
	{
		QuestSystem.u003cGetConfigOnceCoroutineu003ec__Iterator185 variable = null;
		return variable;
	}

	private Task<string> GetConfigUpdate()
	{
		TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
		base.StartCoroutine(this.GetConfigUpdateCoroutine(taskCompletionSource));
		return taskCompletionSource.Task;
	}

	[DebuggerHidden]
	private IEnumerator GetConfigUpdateCoroutine(TaskCompletionSource<string> tcs)
	{
		QuestSystem.u003cGetConfigUpdateCoroutineu003ec__Iterator183 variable = null;
		return variable;
	}

	private Task<string> GetQuestConfig()
	{
		TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
		base.StartCoroutine(this.GetQuestConfigCoroutine(taskCompletionSource));
		return taskCompletionSource.Task;
	}

	[DebuggerHidden]
	private IEnumerator GetQuestConfigCoroutine(TaskCompletionSource<string> tcs)
	{
		QuestSystem.u003cGetQuestConfigCoroutineu003ec__Iterator182 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetTutorialQuestConfigLoopCoroutine()
	{
		QuestSystem.u003cGetTutorialQuestConfigLoopCoroutineu003ec__Iterator187 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator GetTutorialQuestsConfigOnceCoroutine()
	{
		QuestSystem.u003cGetTutorialQuestsConfigOnceCoroutineu003ec__Iterator184 variable = null;
		return variable;
	}

	private void HandleQuestCompleted(object sender, QuestCompletedEventArgs e)
	{
		if (!this.Enabled)
		{
			UnityEngine.Debug.LogFormat("QuestSystem.HandleQuestCompleted('{0}'): disabled", new object[] { e.Quest.Id });
			return;
		}
		this.SaveQuestProgressIfDirty();
		TutorialQuestManager.Instance.SaveIfDirty();
		this.QuestCompleted.Do<EventHandler<QuestCompletedEventArgs>>((EventHandler<QuestCompletedEventArgs> handler) => handler(sender, e));
	}

	public void Initialize()
	{
	}

	private static QuestSystem InitializeInstance()
	{
		QuestSystem questSystem = UnityEngine.Object.FindObjectOfType<QuestSystem>();
		if (questSystem != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(questSystem.gameObject);
			return questSystem;
		}
		GameObject gameObject = new GameObject("Rilisoft.QuestSystem");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		QuestSystem flag = gameObject.AddComponent<QuestSystem>();
		int num = PlayerPrefs.GetInt("QuestSystem.DefaultAvailability", 1);
		flag._enabled = Convert.ToBoolean(num);
		return flag;
	}

	private void InitializeQuestProgress()
	{
		this._questProgress = this.LoadQuestProgress();
		if (this._questProgress != null)
		{
			this._questProgress.QuestCompleted += new EventHandler<QuestCompletedEventArgs>(this.HandleQuestCompleted);
			if (!TutorialQuestManager.Instance.Received)
			{
				this._getTutorialQuestsConfigLoopCoroutine.Do<Coroutine>(new Action<Coroutine>(this.StopCoroutine));
				this._getTutorialQuestsConfigLoopCoroutine = base.StartCoroutine(this.GetTutorialQuestConfigLoopCoroutine());
			}
		}
		this.Updated.Do<EventHandler>((EventHandler handler) => handler(this, EventArgs.Empty));
		this._getConfigLoopCoroutine = base.StartCoroutine(this.GetConfigLoopCoroutine(false));
	}

	private Rilisoft.QuestProgress LoadQuestProgress()
	{
		object obj1;
		object obj2;
		object obj3;
		Rilisoft.QuestProgress questProgress;
		string key;
		if (!Storager.hasKey("QuestProgress"))
		{
			return null;
		}
		string str = Storager.getString("QuestProgress", false);
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null)
		{
			return null;
		}
		if (strs.Count == 0)
		{
			return null;
		}
		Func<string, Version> func = (string v) => {
			Version version;
			try
			{
				version = new Version(v);
			}
			catch
			{
				version = new Version(0, 0, 0, 0);
			}
			return version;
		};
		if (strs.Count != 1)
		{
			KeyValuePair<string, Version> keyValuePair = (
				from k in strs.Keys
				select new KeyValuePair<string, Version>(k, func(k))).Aggregate<KeyValuePair<string, Version>>((KeyValuePair<string, Version> l, KeyValuePair<string, Version> r) => (l.Value <= r.Value ? r : l));
			key = keyValuePair.Key;
		}
		else
		{
			key = strs.Keys.First<string>();
		}
		string str1 = key;
		Dictionary<string, object> item = strs[str1] as Dictionary<string, object>;
		if (item == null)
		{
			return null;
		}
		if (!item.TryGetValue("day", out obj1))
		{
			return null;
		}
		if (!item.TryGetValue("timeLeftSeconds", out obj2))
		{
			return null;
		}
		if (!item.TryGetValue("timestamp", out obj3))
		{
			return null;
		}
		Rilisoft.QuestProgress questProgress1 = null;
		try
		{
			long num = Convert.ToInt64(obj1, CultureInfo.InvariantCulture);
			DateTime dateTime = Convert.ToDateTime(obj3, CultureInfo.InvariantCulture);
			float single = (float)Convert.ToDouble(obj2, CultureInfo.InvariantCulture);
			questProgress1 = new Rilisoft.QuestProgress(str1, num, dateTime, single, null);
			Dictionary<string, object> item1 = item["currentQuests"] as Dictionary<string, object>;
			if (item1 != null)
			{
				Dictionary<string, object> strs1 = item["previousQuests"] as Dictionary<string, object>;
				if (strs1 != null)
				{
					IDictionary<int, List<QuestBase>> nums = Rilisoft.QuestProgress.RestoreQuests(item1);
					questProgress1.PopulateQuests(nums, Rilisoft.QuestProgress.RestoreQuests(strs1));
					questProgress1.FillTutorialQuests(item.TryGet("tutorialQuests") as List<object>);
					return questProgress1;
				}
				else
				{
					questProgress = questProgress1;
				}
			}
			else
			{
				questProgress = questProgress1;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			return questProgress1;
		}
		return questProgress;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!this.Enabled)
		{
			UnityEngine.Debug.LogFormat("QuestSystem.OnApplicationPause({0}): disabled", new object[] { pauseStatus });
			return;
		}
		if (!pauseStatus)
		{
			this._getConfigLoopCoroutine.Do<Coroutine>(new Action<Coroutine>(this.StopCoroutine));
			this._getConfigLoopCoroutine = base.StartCoroutine(this.GetConfigLoopCoroutine(true));
		}
		else
		{
			QuestSystem.SaveQuestProgress(this._questProgress);
			TutorialQuestManager.Instance.SaveIfDirty();
		}
	}

	private static void SaveQuestProgress(Rilisoft.QuestProgress questProgress)
	{
		if (questProgress == null)
		{
			return;
		}
		Dictionary<string, object> json = questProgress.ToJson();
		Dictionary<string, object> strs = new Dictionary<string, object>()
		{
			{ questProgress.ConfigVersion, json }
		};
		string str = Json.Serialize(strs);
		if (questProgress.Count != 0)
		{
			Storager.setString("QuestProgress", str, false);
			questProgress.SetClean();
			return;
		}
		UnityEngine.Debug.LogWarning(string.Concat("SaveQuestProgress(): Bad progress: ", str));
		Storager.setString("QuestProgress", "{}", false);
	}

	public void SaveQuestProgressIfDirty()
	{
		if (this._questProgress == null)
		{
			return;
		}
		if (!this._questProgress.IsDirty())
		{
			return;
		}
		try
		{
			QuestSystem.SaveQuestProgress(this._questProgress);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void Start()
	{
		if (!this.Enabled)
		{
			UnityEngine.Debug.Log("QuestSystem.Start(): disabled");
			return;
		}
		this.InitializeQuestProgress();
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

	public event EventHandler Updated
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.Updated += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.Updated -= value;
		}
	}
}