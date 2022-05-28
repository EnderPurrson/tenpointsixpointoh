using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.DictionaryExtensions;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

internal sealed class QuestSystem : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHandleQuestCompleted_003Ec__AnonStorey2EF
	{
		internal object sender;

		internal QuestCompletedEventArgs e;

		internal void _003C_003Em__41C(EventHandler<QuestCompletedEventArgs> handler)
		{
			handler(sender, e);
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadQuestProgress_003Ec__AnonStorey2F0
	{
		internal Func<string, Version> createVersion;

		internal KeyValuePair<string, Version> _003C_003Em__41E(string k)
		{
			return new KeyValuePair<string, Version>(k, createVersion(k));
		}
	}

	internal const string QuestProgressKey = "QuestProgress";

	internal const string DefaultAvailabilityKey = "QuestSystem.DefaultAvailability";

	private const int _questConfigClientVersion = 28;

	private bool _enabled;

	private static readonly Lazy<QuestSystem> _instance = new Lazy<QuestSystem>(InitializeInstance);

	private Coroutine _getConfigLoopCoroutine;

	private Coroutine _getTutorialQuestsConfigLoopCoroutine;

	private QuestProgress _questProgress;

	private DateTime? _startupTimeAccordingToServer;

	[CompilerGenerated]
	private static Func<string, Version> _003C_003Ef__am_0024cache8;

	[CompilerGenerated]
	private static Func<KeyValuePair<string, Version>, KeyValuePair<string, Version>, KeyValuePair<string, Version>> _003C_003Ef__am_0024cache9;

	public static QuestSystem Instance
	{
		get
		{
			return _instance.Value;
		}
	}

	public QuestProgress QuestProgress
	{
		get
		{
			return _questProgress;
		}
	}

	internal bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			PlayerPrefs.SetInt("QuestSystem.DefaultAvailability", Convert.ToInt32(value));
			if (_enabled != value)
			{
				_enabled = value;
				if (value)
				{
					InitializeQuestProgress();
				}
				else if (_questProgress != null)
				{
					_questProgress.Dispose();
					_questProgress = null;
				}
				EventHandler updated = this.Updated;
				if (updated != null)
				{
					updated(this, EventArgs.Empty);
				}
			}
		}
	}

	public bool AnyActiveQuest
	{
		get
		{
			return Enabled && QuestProgress != null && QuestProgress.AnyActiveQuest;
		}
	}

	internal int QuestConfigClientVersion
	{
		get
		{
			return 28;
		}
	}

	public event EventHandler Updated;

	public event EventHandler<QuestCompletedEventArgs> QuestCompleted;

	public void Initialize()
	{
	}

	private void Start()
	{
		if (!Enabled)
		{
			Debug.Log("QuestSystem.Start(): disabled");
		}
		else
		{
			InitializeQuestProgress();
		}
	}

	private void InitializeQuestProgress()
	{
		_questProgress = LoadQuestProgress();
		if (_questProgress != null)
		{
			_questProgress.QuestCompleted += HandleQuestCompleted;
			if (!TutorialQuestManager.Instance.Received)
			{
				_getTutorialQuestsConfigLoopCoroutine.Do(base.StopCoroutine);
				_getTutorialQuestsConfigLoopCoroutine = StartCoroutine(GetTutorialQuestConfigLoopCoroutine());
			}
		}
		this.Updated.Do(_003CInitializeQuestProgress_003Em__41A);
		_getConfigLoopCoroutine = StartCoroutine(GetConfigLoopCoroutine(false));
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!Enabled)
		{
			Debug.LogFormat("QuestSystem.OnApplicationPause({0}): disabled", pauseStatus);
		}
		else if (pauseStatus)
		{
			SaveQuestProgress(_questProgress);
			TutorialQuestManager.Instance.SaveIfDirty();
		}
		else
		{
			_getConfigLoopCoroutine.Do(base.StopCoroutine);
			_getConfigLoopCoroutine = StartCoroutine(GetConfigLoopCoroutine(true));
		}
	}

	internal void DebugDecrementDay()
	{
		if (Enabled)
		{
			if (_questProgress != null)
			{
				_questProgress.DebugDecrementDay();
			}
			this.Updated.Do(_003CDebugDecrementDay_003Em__41B);
		}
	}

	internal void ForceGetConfig()
	{
		_getConfigLoopCoroutine.Do(base.StopCoroutine);
		_getConfigLoopCoroutine = StartCoroutine(GetConfigLoopCoroutine(false));
	}

	private void HandleQuestCompleted(object sender, QuestCompletedEventArgs e)
	{
		_003CHandleQuestCompleted_003Ec__AnonStorey2EF _003CHandleQuestCompleted_003Ec__AnonStorey2EF = new _003CHandleQuestCompleted_003Ec__AnonStorey2EF();
		_003CHandleQuestCompleted_003Ec__AnonStorey2EF.sender = sender;
		_003CHandleQuestCompleted_003Ec__AnonStorey2EF.e = e;
		if (!Enabled)
		{
			Debug.LogFormat("QuestSystem.HandleQuestCompleted('{0}'): disabled", _003CHandleQuestCompleted_003Ec__AnonStorey2EF.e.Quest.Id);
		}
		else
		{
			SaveQuestProgressIfDirty();
			TutorialQuestManager.Instance.SaveIfDirty();
			this.QuestCompleted.Do(_003CHandleQuestCompleted_003Ec__AnonStorey2EF._003C_003Em__41C);
		}
	}

	private Task<string> GetQuestConfig()
	{
		TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
		StartCoroutine(GetQuestConfigCoroutine(taskCompletionSource));
		return taskCompletionSource.Task;
	}

	private IEnumerator GetQuestConfigCoroutine(TaskCompletionSource<string> tcs)
	{
		WWW response = Tools.CreateWwwIfNotConnected(URLs.QuestConfig);
		if (response == null)
		{
			tcs.TrySetException(new InvalidOperationException("Skipped quest config request because the player is connected."));
			yield break;
		}
		yield return response;
		if (string.IsNullOrEmpty(response.error))
		{
			string responseText = ((response.text == null) ? string.Empty : URLs.Sanitize(response));
			tcs.TrySetResult(responseText);
		}
		else
		{
			tcs.TrySetException(new InvalidOperationException(response.error));
		}
	}

	private Task<string> GetConfigUpdate()
	{
		TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
		StartCoroutine(GetConfigUpdateCoroutine(taskCompletionSource));
		return taskCompletionSource.Task;
	}

	private IEnumerator GetConfigUpdateCoroutine(TaskCompletionSource<string> tcs)
	{
		while (true)
		{
			FriendsController sharedController = FriendsController.sharedController;
			if (_003CGetConfigUpdateCoroutine_003Ec__Iterator183._003C_003Ef__am_0024cache8 == null)
			{
				_003CGetConfigUpdateCoroutine_003Ec__Iterator183._003C_003Ef__am_0024cache8 = _003CGetConfigUpdateCoroutine_003Ec__Iterator183._003C_003Em__420;
			}
			if (!string.IsNullOrEmpty(sharedController.Map(_003CGetConfigUpdateCoroutine_003Ec__Iterator183._003C_003Ef__am_0024cache8)))
			{
				break;
			}
			yield return null;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_quest_version_info");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_quest_version_info"));
		WWW response = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty);
		if (response == null)
		{
			tcs.TrySetException(new InvalidOperationException("Cannot send request while connected."));
			yield break;
		}
		yield return response;
		if (string.IsNullOrEmpty(response.error))
		{
			string responseText = ((response.text == null) ? string.Empty : URLs.Sanitize(response));
			tcs.TrySetResult(responseText);
		}
		else
		{
			tcs.TrySetException(new InvalidOperationException(response.error));
		}
	}

	private IEnumerator GetTutorialQuestsConfigOnceCoroutine()
	{
		WWW response = Tools.CreateWwwIfNotConnected(URLs.TutorialQuestConfig);
		if (response == null)
		{
			yield break;
		}
		yield return response;
		if (!string.IsNullOrEmpty(response.error))
		{
			Debug.LogWarningFormat("Failed to load tutorial quests: {0}", response.error);
			yield break;
		}
		string responseText = ((response.text == null) ? string.Empty : URLs.Sanitize(response));
		Dictionary<string, object> config = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (config == null)
		{
			Debug.LogWarningFormat("Failed to parse config: '{0}'", responseText);
			yield break;
		}
		List<object> tutorialQuestJsons = config.TryGet("quests") as List<object>;
		if (_questProgress != null && !TutorialQuestManager.Instance.Received)
		{
			if (tutorialQuestJsons != null)
			{
				TutorialQuestManager.Instance.SetReceived();
			}
			_questProgress.FillTutorialQuests(tutorialQuestJsons);
			this.Updated.Do(((_003CGetTutorialQuestsConfigOnceCoroutine_003Ec__Iterator184)(object)this)._003C_003Em__421);
			SaveQuestProgressIfDirty();
			TutorialQuestManager.Instance.SaveIfDirty();
		}
	}

	private IEnumerator GetConfigOnceCoroutine(bool resumed)
	{
		if (!Enabled)
		{
			Debug.LogFormat("QuestSystem.GetConfigOnceCoroutine({0}): disabled", resumed);
			yield break;
		}
		Task<string> configUpdateRequest = GetConfigUpdate();
		while (!configUpdateRequest.IsCompleted)
		{
			yield return null;
		}
		float responceReceivedTime = Time.realtimeSinceStartup;
		if (configUpdateRequest.IsFaulted)
		{
			Debug.LogWarning(configUpdateRequest.Exception.InnerException);
			yield break;
		}
		Dictionary<string, object> response = Json.Deserialize(configUpdateRequest.Result) as Dictionary<string, object>;
		if (response == null)
		{
			Debug.LogWarning("GetConfigOnceCoroutine(): Bad update response: " + configUpdateRequest.Result);
			yield break;
		}
		string version2 = string.Empty;
		long day2 = 0L;
		float timeLeftSeconds2 = 0f;
		DateTime timestamp2 = default(DateTime);
		try
		{
			int serverVersion = Convert.ToInt32(response["version"]);
			version2 = string.Format(arg1: QuestConfigClientVersion, format: "{0}.{1}", arg0: serverVersion);
			day2 = Convert.ToInt64(response["day"]);
			timeLeftSeconds2 = (float)Convert.ToDouble(response["timeLeftSeconds"], CultureInfo.InvariantCulture);
			long timestampUnix = Convert.ToInt64(response["timestamp"], CultureInfo.InvariantCulture);
			timestamp2 = Tools.GetCurrentTimeByUnixTime(timestampUnix);
			_startupTimeAccordingToServer = timestamp2 - TimeSpan.FromSeconds(responceReceivedTime);
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.LogException(ex);
			yield break;
		}
		if (_questProgress != null && _questProgress.ConfigVersion == version2 && _questProgress.Day == day2)
		{
			yield break;
		}
		if (!Enabled)
		{
			Debug.LogFormat("QuestSystem.GetConfigOnceCoroutine({0}): disabled", resumed);
			yield break;
		}
		Task<string> questConfigRequest = GetQuestConfig();
		while (!questConfigRequest.IsCompleted)
		{
			yield return null;
		}
		if (questConfigRequest.IsFaulted)
		{
			Debug.LogWarning(questConfigRequest.Exception);
			yield break;
		}
		Dictionary<string, object> rawQuests = Json.Deserialize(questConfigRequest.Result) as Dictionary<string, object>;
		if (rawQuests == null)
		{
			Debug.LogWarning("GetConfigOnceCoroutine(): Bad config response: " + questConfigRequest.Result);
			yield break;
		}
		List<Difficulty> allowedDifficulties = new List<Difficulty>
		{
			Difficulty.Easy,
			Difficulty.Normal,
			Difficulty.Hard
		};
		if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 3 && allowedDifficulties.Remove(Difficulty.Hard))
		{
			allowedDifficulties.Add(Difficulty.Normal);
		}
		Lazy<IDictionary<int, List<QuestBase>>> newQuests = new Lazy<IDictionary<int, List<QuestBase>>>(((_003CGetConfigOnceCoroutine_003Ec__Iterator185)(object)this)._003C_003Em__422);
		if (_questProgress == null)
		{
			_questProgress = new QuestProgress(version2, day2, timestamp2, timeLeftSeconds2);
			_getTutorialQuestsConfigLoopCoroutine.Do(base.StopCoroutine);
			_getTutorialQuestsConfigLoopCoroutine = StartCoroutine(GetTutorialQuestConfigLoopCoroutine());
			_questProgress.QuestCompleted += HandleQuestCompleted;
			_questProgress.PopulateQuests(newQuests.Value, null);
			this.Updated.Do(((_003CGetConfigOnceCoroutine_003Ec__Iterator185)(object)this)._003C_003Em__423);
		}
		else if (!_questProgress.ConfigVersion.Equals(version2, StringComparison.Ordinal))
		{
			_questProgress.Dispose();
			_questProgress.QuestCompleted -= HandleQuestCompleted;
			_questProgress = new QuestProgress(version2, day2, timestamp2, timeLeftSeconds2, _questProgress);
			_getTutorialQuestsConfigLoopCoroutine.Do(base.StopCoroutine);
			_getTutorialQuestsConfigLoopCoroutine = StartCoroutine(GetTutorialQuestConfigLoopCoroutine());
			_questProgress.QuestCompleted += HandleQuestCompleted;
			_questProgress.PopulateQuests(newQuests.Value, null);
			this.Updated.Do(((_003CGetConfigOnceCoroutine_003Ec__Iterator185)(object)this)._003C_003Em__424);
		}
		else if (_questProgress.Day < day2)
		{
			_questProgress.UpdateQuests(day2, rawQuests, newQuests.Value);
			this.Updated.Do(((_003CGetConfigOnceCoroutine_003Ec__Iterator185)(object)this)._003C_003Em__425);
		}
		SaveQuestProgressIfDirty();
		TutorialQuestManager.Instance.SaveIfDirty();
	}

	public void SaveQuestProgressIfDirty()
	{
		if (_questProgress == null || !_questProgress.IsDirty())
		{
			return;
		}
		try
		{
			SaveQuestProgress(_questProgress);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	private IEnumerator GetConfigLoopCoroutine(bool resumed)
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		float delaySeconds = ((!Application.isEditor) ? 600f : 30f);
		Coroutine configCoroutine = null;
		while (Enabled)
		{
			if (configCoroutine != null)
			{
				StopCoroutine(configCoroutine);
			}
			configCoroutine = StartCoroutine(GetConfigOnceCoroutine(resumed));
			yield return new WaitForSeconds(delaySeconds);
		}
		Debug.LogFormat("QuestSystem.GetConfigLoopCoroutine({0}): disabled", resumed);
	}

	private IEnumerator GetTutorialQuestConfigLoopCoroutine()
	{
		float delaySeconds = ((!Application.isEditor) ? 600f : 30f);
		Coroutine configCoroutine = null;
		while (_questProgress == null || !TutorialQuestManager.Instance.Received)
		{
			if (!Enabled)
			{
				Debug.Log("QuestSystem.GetTutorialQuestConfigLoopCoroutine({0}): disabled");
				break;
			}
			if (configCoroutine != null)
			{
				StopCoroutine(configCoroutine);
			}
			configCoroutine = StartCoroutine(GetTutorialQuestsConfigOnceCoroutine());
			yield return new WaitForSeconds(delaySeconds);
		}
	}

	private QuestProgress LoadQuestProgress()
	{
		_003CLoadQuestProgress_003Ec__AnonStorey2F0 _003CLoadQuestProgress_003Ec__AnonStorey2F = new _003CLoadQuestProgress_003Ec__AnonStorey2F0();
		if (!Storager.hasKey("QuestProgress"))
		{
			return null;
		}
		string @string = Storager.getString("QuestProgress", false);
		if (string.IsNullOrEmpty(@string))
		{
			return null;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return null;
		}
		if (dictionary.Count == 0)
		{
			return null;
		}
		if (_003C_003Ef__am_0024cache8 == null)
		{
			_003C_003Ef__am_0024cache8 = _003CLoadQuestProgress_003Em__41D;
		}
		_003CLoadQuestProgress_003Ec__AnonStorey2F.createVersion = _003C_003Ef__am_0024cache8;
		string text;
		if (dictionary.Count == 1)
		{
			text = dictionary.Keys.First();
		}
		else
		{
			IEnumerable<KeyValuePair<string, Version>> source = dictionary.Keys.Select(_003CLoadQuestProgress_003Ec__AnonStorey2F._003C_003Em__41E);
			if (_003C_003Ef__am_0024cache9 == null)
			{
				_003C_003Ef__am_0024cache9 = _003CLoadQuestProgress_003Em__41F;
			}
			text = source.Aggregate(_003C_003Ef__am_0024cache9).Key;
		}
		string text2 = text;
		Dictionary<string, object> dictionary2 = dictionary[text2] as Dictionary<string, object>;
		if (dictionary2 == null)
		{
			return null;
		}
		object value;
		if (!dictionary2.TryGetValue("day", out value))
		{
			return null;
		}
		object value2;
		if (!dictionary2.TryGetValue("timeLeftSeconds", out value2))
		{
			return null;
		}
		object value3;
		if (!dictionary2.TryGetValue("timestamp", out value3))
		{
			return null;
		}
		QuestProgress questProgress = null;
		try
		{
			long day = Convert.ToInt64(value, CultureInfo.InvariantCulture);
			DateTime timestamp = Convert.ToDateTime(value3, CultureInfo.InvariantCulture);
			float timeLeftSeconds = (float)Convert.ToDouble(value2, CultureInfo.InvariantCulture);
			questProgress = new QuestProgress(text2, day, timestamp, timeLeftSeconds);
			Dictionary<string, object> dictionary3 = dictionary2["currentQuests"] as Dictionary<string, object>;
			if (dictionary3 == null)
			{
				return questProgress;
			}
			Dictionary<string, object> dictionary4 = dictionary2["previousQuests"] as Dictionary<string, object>;
			if (dictionary4 == null)
			{
				return questProgress;
			}
			IDictionary<int, List<QuestBase>> currentQuests = QuestProgress.RestoreQuests(dictionary3);
			IDictionary<int, List<QuestBase>> previousQuests = QuestProgress.RestoreQuests(dictionary4);
			questProgress.PopulateQuests(currentQuests, previousQuests);
			List<object> questJsons = dictionary2.TryGet("tutorialQuests") as List<object>;
			questProgress.FillTutorialQuests(questJsons);
			return questProgress;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return questProgress;
		}
	}

	private static void SaveQuestProgress(QuestProgress questProgress)
	{
		if (questProgress != null)
		{
			Dictionary<string, object> value = questProgress.ToJson();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(questProgress.ConfigVersion, value);
			Dictionary<string, object> obj = dictionary;
			string text = Json.Serialize(obj);
			if (questProgress.Count == 0)
			{
				Debug.LogWarning("SaveQuestProgress(): Bad progress: " + text);
				Storager.setString("QuestProgress", "{}", false);
			}
			else
			{
				Storager.setString("QuestProgress", text, false);
				questProgress.SetClean();
			}
		}
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
		QuestSystem questSystem2 = gameObject.AddComponent<QuestSystem>();
		int @int = PlayerPrefs.GetInt("QuestSystem.DefaultAvailability", 1);
		questSystem2._enabled = Convert.ToBoolean(@int);
		return questSystem2;
	}

	[CompilerGenerated]
	private void _003CInitializeQuestProgress_003Em__41A(EventHandler handler)
	{
		handler(this, EventArgs.Empty);
	}

	[CompilerGenerated]
	private void _003CDebugDecrementDay_003Em__41B(EventHandler handler)
	{
		handler(this, EventArgs.Empty);
	}

	[CompilerGenerated]
	private static Version _003CLoadQuestProgress_003Em__41D(string v)
	{
		//Discarded unreachable code: IL_000c, IL_0021
		try
		{
			return new Version(v);
		}
		catch
		{
			return new Version(0, 0, 0, 0);
		}
	}

	[CompilerGenerated]
	private static KeyValuePair<string, Version> _003CLoadQuestProgress_003Em__41F(KeyValuePair<string, Version> l, KeyValuePair<string, Version> r)
	{
		return (!(l.Value > r.Value)) ? r : l;
	}
}
