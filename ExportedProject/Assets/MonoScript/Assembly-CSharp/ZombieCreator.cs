using Rilisoft;
using Rilisoft.NullExtensions;
using RilisoftBot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class ZombieCreator : MonoBehaviour
{
	private GameObject boss;

	public GameObject weaponBonus;

	public static ZombieCreator sharedCreator;

	private static int _ZombiesInWave;

	public int currentWave;

	private static List<List<string>> _enemiesInWaves;

	private readonly static HashSet<string> _allEnemiesSurvival;

	public List<GameObject> waveZombiePrefabs = new List<GameObject>();

	public static Dictionary<int, int> bossesSurvival;

	private static List<List<string>> _WeaponsAddedInWaves;

	public static List<string> survivalAvailableWeapons;

	private bool _generatingZombiesIsStopped;

	private int totalNumOfKilledEnemies;

	private AudioClip bossMus;

	private static int? _enemyCountInSurvivalWave;

	public GUIStyle labelStyle;

	private int[] _intervalArr = new int[] { 6, 4, 3 };

	private int _genWithThisTimeInterval;

	private int _indexInTimesArray;

	private string _msg = string.Empty;

	private GameObject[] _teleports;

	public bool bossShowm;

	public bool stopGeneratingBonuses;

	public List<GameObject> zombiePrefabs = new List<GameObject>();

	private bool _isMultiplayer;

	private SaltedInt _numOfLiveZombies = 0;

	private SaltedInt _numOfDeadZombies = 0;

	private SaltedInt _numOfDeadZombsSinceLastFast = 0;

	public float curInterval = 10f;

	private GameObject[] _enemyCreationZones;

	private List<string[]> _enemies = new List<string[]>();

	public GameObject[] bossGuads
	{
		get;
		private set;
	}

	public static int DefaultEnemyCountInSurvivalWave
	{
		get
		{
			return ZombieCreator._ZombiesInWave;
		}
	}

	public static int EnemyCountInSurvivalWave
	{
		get
		{
			return (!ZombieCreator._enemyCountInSurvivalWave.HasValue ? ZombieCreator.DefaultEnemyCountInSurvivalWave : ZombieCreator._enemyCountInSurvivalWave.Value);
		}
		set
		{
			ZombieCreator._enemyCountInSurvivalWave = new int?(value);
		}
	}

	public bool IsLasTMonsRemains
	{
		get
		{
			return (this.NumOfDeadZombies + 1 != ZombieCreator.NumOfEnemisesToKill ? false : !this.bossShowm);
		}
	}

	public int NumOfDeadZombies
	{
		get
		{
			return this._numOfDeadZombies.Value;
		}
		set
		{
			if (this.bossShowm)
			{
				this.bossShowm = false;
				if (Defs.IsSurvival)
				{
					this.totalNumOfKilledEnemies++;
					ZombieCreator numOfLiveZombies = this;
					numOfLiveZombies.NumOfLiveZombies = numOfLiveZombies.NumOfLiveZombies - 1;
					this.NextWave();
					return;
				}
				if (ZombieCreator.BossKilled != null)
				{
					ZombieCreator.BossKilled();
				}
				if (LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
				{
					GameObject gameObject = (
						from g in (IEnumerable<GotToNextLevel>)UnityEngine.Object.FindObjectsOfType<GotToNextLevel>()
						select g.gameObject).FirstOrDefault<GameObject>();
					if (gameObject != null)
					{
						PlayerArrowToPortalController component = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
						if (component != null)
						{
							component.RemovePointOfInterest();
							component.SetPointOfInterest(gameObject.transform);
						}
					}
				}
				else
				{
					GameObject[] gameObjectArray = this._teleports;
					for (int i = 0; i < (int)gameObjectArray.Length; i++)
					{
						gameObjectArray[i].SetActive(true);
					}
					GameObject gameObject1 = this._teleports.Map<GameObject[], GameObject>((GameObject[] ts) => ts.FirstOrDefault<GameObject>());
					if (gameObject1 != null)
					{
						PlayerArrowToPortalController playerArrowToPortalController = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
						if (playerArrowToPortalController != null)
						{
							playerArrowToPortalController.RemovePointOfInterest();
							playerArrowToPortalController.SetPointOfInterest(gameObject1.transform);
						}
					}
				}
				return;
			}
			int num = value - this._numOfDeadZombies.Value;
			this._numOfDeadZombies = value;
			this.totalNumOfKilledEnemies += num;
			ZombieCreator zombieCreator = this;
			zombieCreator.NumOfLiveZombies = zombieCreator.NumOfLiveZombies - num;
			if (!Defs.IsSurvival && ZombieCreator.NumOfEnemisesToKill - this._numOfDeadZombies.Value <= 3 && Initializer.enemiesObj.Count > 0)
			{
				PlayerArrowToPortalController component1 = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
				Transform item = null;
				float single = Single.MaxValue;
				for (int j = 0; j < Initializer.enemiesObj.Count; j++)
				{
					if (Initializer.enemiesObj[j].GetComponent<BaseBot>().health > 0f)
					{
						Vector3 vector3 = WeaponManager.sharedManager.myPlayer.transform.position - Initializer.enemiesObj[j].transform.position;
						float single1 = vector3.sqrMagnitude;
						if (single1 < single)
						{
							item = Initializer.enemiesObj[j].transform;
							single = single1;
						}
					}
				}
				component1.RemovePointOfInterest();
				if (item != null)
				{
					component1.RemovePointOfInterest();
					component1.SetPointOfInterest(item);
				}
			}
			if (!Defs.IsSurvival)
			{
				this._numOfDeadZombsSinceLastFast = this._numOfDeadZombsSinceLastFast.Value + num;
				if (this._numOfDeadZombsSinceLastFast.Value == 12)
				{
					if (this.curInterval > 5f)
					{
						this.curInterval -= 5f;
					}
					this._numOfDeadZombsSinceLastFast = 0;
				}
				if (this.IsLasTMonsRemains && ZombieCreator.LastEnemy != null)
				{
					ZombieCreator.LastEnemy();
				}
			}
			if (Defs.IsSurvival && this.NumOfDeadZombies == ZombieCreator.NumOfEnemisesToKill - 1)
			{
				this.stopGeneratingBonuses = true;
			}
			if (this._numOfDeadZombies.Value >= ZombieCreator.NumOfEnemisesToKill)
			{
				if (Defs.IsSurvival)
				{
					if (!ZombieCreator.bossesSurvival.ContainsKey(this.currentWave))
					{
						this.NextWave();
					}
					else
					{
						this.CreateBoss();
					}
				}
				else if (CurrentCampaignGame.currentLevel != 0)
				{
					this.CreateBoss();
					if (this.bossMus != null)
					{
						GameObject gameObject2 = GameObject.FindGameObjectWithTag("BackgroundMusic");
						if (gameObject2 != null && gameObject2.GetComponent<AudioSource>())
						{
							gameObject2.GetComponent<AudioSource>().Stop();
							gameObject2.GetComponent<AudioSource>().clip = this.bossMus;
							if (Defs.isSoundMusic)
							{
								gameObject2.GetComponent<AudioSource>().Play();
							}
						}
					}
				}
				else
				{
					GameObject[] gameObjectArray1 = this._teleports;
					for (int k = 0; k < (int)gameObjectArray1.Length; k++)
					{
						GameObject gameObject3 = gameObjectArray1[k];
						if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
						{
							TrainingController.isNextStep = TrainingState.KillZombie;
						}
						gameObject3.SetActive(true);
					}
				}
			}
		}
	}

	public static int NumOfEnemisesToKill
	{
		get
		{
			return (!Defs.IsSurvival ? GlobalGameController.EnemiesToKill : ZombieCreator.EnemyCountInSurvivalWave);
		}
	}

	public int NumOfLiveZombies
	{
		get
		{
			return this._numOfLiveZombies.Value;
		}
		set
		{
			this._numOfLiveZombies = value;
		}
	}

	static ZombieCreator()
	{
		ZombieCreator.sharedCreator = null;
		ZombieCreator._ZombiesInWave = 45;
		ZombieCreator._enemiesInWaves = new List<List<string>>();
		ZombieCreator._allEnemiesSurvival = new HashSet<string>();
		ZombieCreator.bossesSurvival = new Dictionary<int, int>();
		ZombieCreator._WeaponsAddedInWaves = new List<List<string>>();
		ZombieCreator.survivalAvailableWeapons = new List<string>();
		List<string> strs = new List<string>()
		{
			WeaponManager.PistolWN,
			WeaponManager.ShotgunWN,
			WeaponManager.MP5WN
		};
		List<string> strs1 = new List<string>()
		{
			WeaponManager.AK47WN,
			WeaponManager.RevolverWN
		};
		List<string> strs2 = new List<string>()
		{
			WeaponManager.M16_2WN,
			WeaponManager.ObrezWN
		};
		List<string> strs3 = new List<string>()
		{
			WeaponManager.MachinegunWN
		};
		List<string> strs4 = new List<string>()
		{
			WeaponManager.AlienGunWN
		};
		ZombieCreator._WeaponsAddedInWaves.Add(strs);
		ZombieCreator._WeaponsAddedInWaves.Add(strs1);
		ZombieCreator._WeaponsAddedInWaves.Add(strs2);
		ZombieCreator._WeaponsAddedInWaves.Add(strs3);
		ZombieCreator._WeaponsAddedInWaves.Add(strs4);
	}

	public ZombieCreator()
	{
	}

	private Vector3 _createPos(GameObject spawnZone)
	{
		BoxCollider component = spawnZone.GetComponent<BoxCollider>();
		float single = component.size.x * spawnZone.transform.localScale.x;
		float single1 = component.size.z;
		Vector3 vector3 = spawnZone.transform.localScale;
		Vector2 vector2 = new Vector2(single, single1 * vector3.z);
		Vector3 vector31 = spawnZone.transform.position;
		float single2 = vector31.x - vector2.x / 2f;
		Vector3 vector32 = spawnZone.transform.position;
		Rect rect = new Rect(single2, vector32.z - vector2.y / 2f, vector2.x, vector2.y);
		float single3 = rect.x + UnityEngine.Random.Range(0f, rect.width);
		Vector3 vector33 = spawnZone.transform.position;
		Vector3 vector34 = new Vector3(single3, vector33.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		return vector34;
	}

	[DebuggerHidden]
	private IEnumerator _DrawFirstMessage()
	{
		return new ZombieCreator.u003c_DrawFirstMessageu003ec__Iterator1A3();
	}

	[DebuggerHidden]
	private IEnumerator _DrawWaveMessage(Action act)
	{
		ZombieCreator.u003c_DrawWaveMessageu003ec__Iterator1A4 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator _PrerenderBoss()
	{
		ZombieCreator.u003c_PrerenderBossu003ec__Iterator1A5 variable = null;
		return variable;
	}

	private void _ResetInterval()
	{
		this.curInterval = Mathf.Max(1f, this.curInterval);
	}

	private void _SetZombiePrefabs()
	{
		this.waveZombiePrefabs.Clear();
		int num = (this.currentWave < ZombieCreator._enemiesInWaves.Count ? this.currentWave : ZombieCreator._enemiesInWaves.Count - 1);
		foreach (GameObject zombiePrefab in this.zombiePrefabs)
		{
			string str = zombiePrefab.name.Substring("Enemy".Length).Substring(0, zombiePrefab.name.Substring("Enemy".Length).IndexOf("_"));
			if (!ZombieCreator._enemiesInWaves[num].Contains(str))
			{
				continue;
			}
			this.waveZombiePrefabs.Add(zombiePrefab);
		}
	}

	private void _UpdateAvailableWeapons()
	{
		if (this.currentWave < ZombieCreator._WeaponsAddedInWaves.Count)
		{
			foreach (string item in ZombieCreator._WeaponsAddedInWaves[this.currentWave])
			{
				ZombieCreator.survivalAvailableWeapons.Add(item);
			}
		}
	}

	private void _UpdateIntervalStructures()
	{
		this._genWithThisTimeInterval = 0;
		this._indexInTimesArray = 0;
		this.curInterval = (float)this._intervalArr[this._indexInTimesArray];
	}

	[DebuggerHidden]
	private IEnumerator AddZombies()
	{
		ZombieCreator.u003cAddZombiesu003ec__Iterator1A6 variable = null;
		return variable;
	}

	private void Awake()
	{
		string str = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", new object[] { base.GetType().Name });
		ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
		try
		{
			if (!Defs.isMulti)
			{
				if (Defs.IsSurvival)
				{
					ZombieCreator._enemiesInWaves.Clear();
					if (!SceneLoader.ActiveSceneName.Equals("Pizza"))
					{
						List<string> strs = new List<string>();
						List<string> strs1 = new List<string>();
						List<string> strs2 = new List<string>();
						List<string> strs3 = new List<string>();
						List<string> strs4 = new List<string>();
						List<string> strs5 = new List<string>();
						List<string> strs6 = new List<string>();
						List<string> strs7 = new List<string>();
						strs.Add("1");
						strs.Add("2");
						strs.Add("15");
						strs1.Add("1");
						strs1.Add("2");
						strs1.Add("15");
						strs1.Add("77");
						strs1.Add("12");
						strs2.Add("3");
						strs2.Add("9");
						strs2.Add("10");
						strs2.Add("11");
						strs2.Add("12");
						strs2.Add("57");
						strs3.Add("49");
						strs3.Add("9");
						strs3.Add("24");
						strs3.Add("29");
						strs3.Add("38");
						strs3.Add("74");
						strs3.Add("48");
						strs3.Add("10");
						strs4.Add("80");
						strs4.Add("81");
						strs4.Add("82");
						strs4.Add("83");
						strs4.Add("84");
						strs4.Add("85");
						strs4.Add("86");
						strs4.Add("87");
						strs4.Add("88");
						strs5.Add("37");
						strs5.Add("46");
						strs5.Add("47");
						strs5.Add("57");
						strs5.Add("24");
						strs5.Add("74");
						strs5.Add("50");
						strs5.Add("20");
						strs5.Add("51");
						strs6.Add("74");
						strs6.Add("57");
						strs6.Add("20");
						strs6.Add("66");
						strs6.Add("60");
						strs6.Add("50");
						strs6.Add("53");
						strs6.Add("33");
						strs6.Add("24");
						strs6.Add("46");
						strs7.Add("74");
						strs7.Add("57");
						strs7.Add("49");
						strs7.Add("66");
						strs7.Add("60");
						strs7.Add("50");
						strs7.Add("53");
						strs7.Add("59");
						strs7.Add("24");
						strs7.Add("46");
						ZombieCreator._enemiesInWaves.Add(strs);
						ZombieCreator._enemiesInWaves.Add(strs1);
						ZombieCreator._enemiesInWaves.Add(strs2);
						ZombieCreator._enemiesInWaves.Add(strs3);
						ZombieCreator._enemiesInWaves.Add(strs4);
						ZombieCreator._enemiesInWaves.Add(strs5);
						ZombieCreator._enemiesInWaves.Add(strs6);
						ZombieCreator._enemiesInWaves.Add(strs7);
					}
					else
					{
						List<string> strs8 = new List<string>();
						List<string> strs9 = new List<string>();
						List<string> strs10 = new List<string>();
						List<string> strs11 = new List<string>();
						List<string> strs12 = new List<string>();
						List<string> strs13 = new List<string>();
						strs8.Add("88");
						strs8.Add("85");
						strs8.Add("86");
						strs9.Add("85");
						strs9.Add("87");
						strs9.Add("82");
						strs9.Add("81");
						strs9.Add("88");
						strs10.Add("86");
						strs10.Add("82");
						strs10.Add("84");
						strs10.Add("81");
						strs10.Add("88");
						strs10.Add("87");
						strs11.Add("81");
						strs11.Add("82");
						strs11.Add("86");
						strs11.Add("80");
						strs11.Add("83");
						strs11.Add("87");
						strs11.Add("84");
						strs12.Add("81");
						strs12.Add("86");
						strs12.Add("88");
						strs12.Add("80");
						strs12.Add("83");
						strs12.Add("82");
						strs12.Add("84");
						strs12.Add("87");
						strs13.Add("81");
						strs13.Add("80");
						strs13.Add("83");
						strs13.Add("82");
						strs13.Add("84");
						strs13.Add("87");
						ZombieCreator._enemiesInWaves.Add(strs8);
						ZombieCreator._enemiesInWaves.Add(strs9);
						ZombieCreator._enemiesInWaves.Add(strs10);
						ZombieCreator._enemiesInWaves.Add(strs11);
						ZombieCreator._enemiesInWaves.Add(strs12);
						ZombieCreator._enemiesInWaves.Add(strs13);
					}
					foreach (List<string> _enemiesInWafe in ZombieCreator._enemiesInWaves)
					{
						foreach (string str1 in _enemiesInWafe)
						{
							ZombieCreator._allEnemiesSurvival.Add(str1);
						}
					}
				}
				this.stopGeneratingBonuses = false;
				ZombieCreator.sharedCreator = this;
				if (!Defs.IsSurvival && CurrentCampaignGame.currentLevel != 0)
				{
					string str2 = string.Concat("Boss", CurrentCampaignGame.currentLevel);
					this.boss = UnityEngine.Object.Instantiate(Resources.Load(ResPath.Combine("Bosses", str2))) as GameObject;
					this.TryCreateBossGuard(this.boss);
					this.boss.SetActive(false);
					if (LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
					{
						string item = LevelBox.weaponsFromBosses[Application.loadedLevelName];
						this.weaponBonus = BonusCreator._CreateBonusPrefab(item);
					}
					base.StartCoroutine(this._PrerenderBoss());
					this.bossMus = Resources.Load("Snd/boss_campaign") as AudioClip;
				}
				GlobalGameController.curThr = GlobalGameController.thrStep;
				this._enemies.Add(new string[] { "1", "2", "1", "11", "12", "13" });
				this._enemies.Add(new string[] { "30", "31", "32", "33", "34", "77" });
				this._enemies.Add(new string[] { "1", "2", "3", "9", "10", "12", "14", "15", "78" });
				this._enemies.Add(new string[] { "1", "2", "4", "11", "9", "16", "78" });
				this._enemies.Add(new string[] { "1", "2", "4", "9", "11", "10", "12" });
				this._enemies.Add(new string[] { "43", "44", "45", "46", "47", "73" });
				this._enemies.Add(new string[] { "6", "7", "7" });
				this._enemies.Add(new string[] { "1", "2", "8", "10", "11", "12", "76" });
				this._enemies.Add(new string[] { "18", "19", "20" });
				this._enemies.Add(new string[] { "21", "22", "23", "24", "25", "75" });
				this._enemies.Add(new string[] { "1", "15" });
				this._enemies.Add(new string[] { "1", "3", "9", "10", "14", "15", "16", "78" });
				this._enemies.Add(new string[] { "8", "21", "22", "79" });
				this._enemies.Add(new string[] { "26", "27", "28", "29", "57" });
				this._enemies.Add(new string[] { "35", "36", "37", "38", "48", "57" });
				this._enemies.Add(new string[] { "39", "40", "41", "42", "74" });
				this._enemies.Add(new string[] { "53", "55", "57", "61" });
				this._enemies.Add(new string[] { "59", "56", "54", "60" });
				this._enemies.Add(new string[] { "67", "68", "66", "69" });
				this._enemies.Add(new string[] { "70", "71", "72" });
				this._enemies.Add(new string[] { "58", "63", "64", "65" });
				this.UpdateBotPrefabs();
				if (Defs.IsSurvival)
				{
					this._SetZombiePrefabs();
				}
				ZombieCreator.survivalAvailableWeapons.Clear();
				this._UpdateAvailableWeapons();
				this._UpdateIntervalStructures();
				base.StartCoroutine(this._DrawFirstMessage());
			}
		}
		finally
		{
			scopeLogger.Dispose();
		}
	}

	public void BeganCreateEnemies()
	{
		if (Application.isEditor && Defs.IsSurvival && !SceneLoader.ActiveSceneName.Equals(Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % (int)Defs.SurvivalMaps.Length]))
		{
			return;
		}
		if (Defs.IsSurvival)
		{
			this.SetWaveNumberInGUI();
		}
		base.StartCoroutine(this.AddZombies());
	}

	private void CreateBoss()
	{
		GameObject gameObject = null;
		float single = Single.PositiveInfinity;
		GameObject gameObject1 = GameObject.FindGameObjectWithTag("Player");
		if (!gameObject1)
		{
			return;
		}
		GameObject[] gameObjectArray = this._enemyCreationZones;
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			GameObject gameObject2 = gameObjectArray[i];
			float single1 = Vector3.SqrMagnitude(gameObject1.transform.position - gameObject2.transform.position);
			float single2 = gameObject1.transform.position.y;
			Vector3 vector3 = gameObject2.transform.position;
			float single3 = Mathf.Abs(single2 - vector3.y);
			if (single1 > 225f && single1 < single && single3 < 2.5f)
			{
				single = single1;
				gameObject = gameObject2;
			}
		}
		if (!gameObject)
		{
			gameObject = this._enemyCreationZones[0];
		}
		Vector3 vector31 = this._createPos(gameObject);
		if (this.boss != null)
		{
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("BossRespawnPoint");
			if (gameObject3 != null)
			{
				vector31 = gameObject3.transform.position;
			}
			this.boss.transform.position = vector31;
			this.boss.transform.rotation = Quaternion.identity;
			this.boss.SetActive(true);
			this.ShowGuards(vector31);
		}
		else if (Defs.IsSurvival && ZombieCreator.bossesSurvival.ContainsKey(this.currentWave))
		{
			string str = string.Concat("Boss", ZombieCreator.bossesSurvival[this.currentWave]);
			this.boss = UnityEngine.Object.Instantiate(Resources.Load(ResPath.Combine("Bosses", str))) as GameObject;
			this.boss.transform.position = vector31;
			this.boss.transform.rotation = Quaternion.identity;
		}
		if (this.boss != null && !Defs.IsSurvival)
		{
			PlayerArrowToPortalController component = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
			if (component != null)
			{
				component.RemovePointOfInterest();
				component.SetPointOfInterest(this.boss.transform, Color.red);
			}
		}
		this.boss = null;
		this.bossShowm = true;
	}

	public static int GetCountMobsForLevel()
	{
		Dictionary<string, int> strs = Switcher.counCreateMobsInLevel;
		string str = CurrentCampaignGame.levelSceneName;
		if (!strs.ContainsKey(str))
		{
			return GlobalGameController.ZombiesInWave;
		}
		return strs[str];
	}

	public void NextWave()
	{
		Vector3 vector3;
		QuestMediator.NotifySurviveWaveInArena();
		this.currentWave++;
		StoreKitEventListener.State.Parameters.Clear();
		StoreKitEventListener.State.Parameters.Add("Waves", string.Concat((this.currentWave >= 10 ? string.Concat(new object[] { string.Empty, this.currentWave / 10 * 10, "-", (this.currentWave / 10 + 1) * 10 - 1 }) : string.Concat(string.Empty, this.currentWave + 1)), " In game"));
		base.StartCoroutine(this._DrawWaveMessage(() => {
			this._UpdateIntervalStructures();
			this._numOfDeadZombies = 0;
			this._numOfDeadZombsSinceLastFast = 0;
			this._SetZombiePrefabs();
			this._UpdateAvailableWeapons();
			this._generatingZombiesIsStopped = false;
			this.stopGeneratingBonuses = false;
			this.SetWaveNumberInGUI();
		}));
		vector3 = (!SceneLoader.ActiveSceneName.Equals("Pizza") ? new Vector3(0f, 1f, 0f) : new Vector3(-7.83f, 0.46f, -2.44f));
		GameObject gameObject = Initializer.CreateBonusAtPosition(vector3, VirtualCurrencyBonusType.Coin);
		if (gameObject == null)
		{
			return;
		}
		CoinBonus component = gameObject.GetComponent<CoinBonus>();
		if (component != null)
		{
			component.SetPlayer();
			return;
		}
		UnityEngine.Debug.LogErrorFormat("Cannot find component '{0}'", new object[] { component.GetType().Name });
	}

	private void OnDestroy()
	{
		ZombieCreator.sharedCreator = null;
		if (Defs.IsSurvival)
		{
			PlayerPrefs.SetInt(Defs.KilledZombiesSett, this.totalNumOfKilledEnemies);
			int num = PlayerPrefs.GetInt(Defs.KilledZombiesMaxSett, 0);
			if (this.totalNumOfKilledEnemies > num)
			{
				PlayerPrefs.SetInt(Defs.KilledZombiesMaxSett, this.totalNumOfKilledEnemies);
			}
			PlayerPrefs.SetInt(Defs.WavesSurvivedS, this.currentWave);
			int num1 = PlayerPrefs.GetInt(Defs.WavesSurvivedMaxS, 0);
			if (this.currentWave > num1)
			{
				PlayerPrefs.SetInt(Defs.WavesSurvivedMaxS, this.currentWave);
			}
		}
	}

	public static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		if (go == null)
		{
			return;
		}
		Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layerNumber;
		}
	}

	private void SetWaveNumberInGUI()
	{
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.SurvivalWaveNumber != null)
		{
			InGameGUI.sharedInGameGUI.SurvivalWaveNumber.text = string.Format("{0} {1}", LocalizationStore.Get("Key_0349"), this.currentWave + 1);
		}
	}

	private void ShowGuards(Vector3 bossPosition)
	{
		if (this.bossGuads == null)
		{
			return;
		}
		for (int i = 0; i < (int)this.bossGuads.Length; i++)
		{
			this.bossGuads[i].transform.position = BaseBot.GetPositionSpawnGuard(bossPosition);
			this.bossGuads[i].transform.rotation = Quaternion.identity;
			this.bossGuads[i].SetActive(true);
		}
	}

	private void Start()
	{
		if (Defs.IsSurvival)
		{
			StoreKitEventListener.State.Parameters.Clear();
			StoreKitEventListener.State.Parameters.Add("Waves", string.Concat(this.currentWave + 1, " In game"));
		}
		this.labelStyle.fontSize = Mathf.RoundToInt(50f * Defs.Coef);
		this.labelStyle.font = LocalizationStore.GetFontByLocalize("Key_04B_03");
		if (!Defs.isMulti)
		{
			this._isMultiplayer = false;
		}
		else
		{
			this._isMultiplayer = true;
		}
		this._teleports = GameObject.FindGameObjectsWithTag("Portal");
		GameObject[] gameObjectArray = this._teleports;
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			gameObjectArray[i].SetActive(false);
		}
		if (!this._isMultiplayer)
		{
			this._enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
			if (!Defs.IsSurvival)
			{
				this._ResetInterval();
			}
		}
	}

	public void SuppressDrawingWaveMessage()
	{
	}

	private void TryCreateBossGuard(GameObject bossObj)
	{
		this.bossGuads = null;
		BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(bossObj.transform);
		if (botScriptForObject == null)
		{
			return;
		}
		int length = (int)botScriptForObject.guards.Length;
		if (length == 0)
		{
			return;
		}
		this.bossGuads = new GameObject[length];
		for (int i = 0; i < length; i++)
		{
			GameObject gameObject = botScriptForObject.guards[i];
			this.bossGuads[i] = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			this.bossGuads[i].name = string.Format("{0}{1}", "BossGuard", i + 1);
			this.bossGuads[i].SetActive(false);
		}
	}

	private void UpdateBotPrefabs()
	{
		this.zombiePrefabs.Clear();
		string[] array = null;
		if (!Defs.IsSurvival)
		{
			array = (CurrentCampaignGame.currentLevel != 0 ? this._enemies[CurrentCampaignGame.currentLevel - 1] : new string[] { "1" });
		}
		else
		{
			array = ZombieCreator._allEnemiesSurvival.ToArray<string>();
		}
		string[] strArrays = array;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			string str = strArrays[i];
			GameObject gameObject = Resources.Load(string.Concat("Enemies/Enemy", str, "_go")) as GameObject;
			this.zombiePrefabs.Add(gameObject);
		}
	}

	public static event Action BossKilled;

	public static event Action LastEnemy;
}