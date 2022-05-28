using System.Collections.Generic;

public class PlayerEventScoreController
{
	private class ScoreEventInfo
	{
		public string eventId;

		public int eventScore;

		public string eventMessage;

		public string pictName;

		public string audioClipName;

		public ScoreEventInfo(string _eventId, int _eventScore, string _eventMessage, string _pictName)
		{
			eventId = _eventId;
			eventScore = _eventScore;
			eventMessage = _eventMessage;
			pictName = _pictName;
			audioClipName = string.Empty;
		}

		public ScoreEventInfo(string _eventId, int _eventScore, string _eventMessage, string _pictName, string _audioClipName)
		{
			eventId = _eventId;
			eventScore = _eventScore;
			eventMessage = _eventMessage;
			pictName = _pictName;
			audioClipName = _audioClipName;
		}
	}

	public enum ScoreEvent
	{
		damageBody = 0,
		damageHead = 1,
		damageMechBody = 2,
		damageMechHead = 3,
		damageTurret = 4,
		damageExplosion = 5,
		damageGrenade = 6,
		deadMech = 7,
		deadTurret = 8,
		dead = 9,
		deadHeadShot = 10,
		deadLongShot = 11,
		invisibleKill = 12,
		doubleHeadShot = 13,
		deadWithFlag = 14,
		killAssist = 15,
		teamKill = 16,
		revenge = 17,
		deathFromAbove = 18,
		duckHunt = 19,
		rocketJumpKill = 20,
		melee = 21,
		melee2 = 22,
		melee3 = 23,
		melee5 = 24,
		melee7 = 25,
		primary1 = 26,
		primary2 = 27,
		primary3 = 28,
		backup1 = 29,
		backup2 = 30,
		backup3 = 31,
		special1 = 32,
		special2 = 33,
		special3 = 34,
		sniper1 = 35,
		sniper2 = 36,
		sniper3 = 37,
		premium1 = 38,
		premium2 = 39,
		premium3 = 40,
		flagTouchDown = 41,
		flagTouchDouble = 42,
		flagTouchDownTriple = 43,
		multyKill2 = 44,
		multyKill3 = 45,
		multyKill4 = 46,
		multyKill5 = 47,
		multyKill6 = 48,
		multyKill10 = 49,
		multyKill20 = 50,
		multyKill50 = 51,
		killMultyKill2 = 52,
		killMultyKill3 = 53,
		killMultyKill4 = 54,
		killMultyKill5 = 55,
		killMultyKill6 = 56,
		killMultyKill10 = 57,
		killMultyKill20 = 58,
		killMultyKill50 = 59,
		deadGrenade = 60,
		deadExplosion = 61,
		teamCapturePoint = 62,
		mySpotPoint = 63,
		unstoppablePoint = 64,
		monopolyPoint = 65,
		houseKeeperPoint = 66,
		defenderPoint = 67,
		guardianPoint = 68,
		oneManArmyPoint = 69
	}

	private static List<ScoreEventInfo> _eventsScoreInfo;

	public static Dictionary<string, int> scoreOnEvent;

	public static Dictionary<string, string> messageOnEvent;

	public static Dictionary<string, string> pictureNameOnEvent;

	public static Dictionary<string, string> audioClipNameOnEvent;

	static PlayerEventScoreController()
	{
		_eventsScoreInfo = new List<ScoreEventInfo>();
		scoreOnEvent = new Dictionary<string, int>();
		messageOnEvent = new Dictionary<string, string>();
		pictureNameOnEvent = new Dictionary<string, string>();
		audioClipNameOnEvent = new Dictionary<string, string>();
		SetScoreEventInfo();
		SetLocalizeForScoreEvent();
		LocalizationStore.AddEventCallAfterLocalize(SetLocalizeForScoreEvent);
	}

	public static void SetScoreEventInfo()
	{
		_eventsScoreInfo.Clear();
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.damageBody.ToString(), 0, ScoreEvent.damageBody.ToString(), string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.damageHead.ToString(), 0, ScoreEvent.damageHead.ToString(), string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.damageMechBody.ToString(), 0, ScoreEvent.damageMechBody.ToString(), string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.damageMechHead.ToString(), 0, ScoreEvent.damageMechHead.ToString(), string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.damageTurret.ToString(), 0, ScoreEvent.damageTurret.ToString(), string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.damageExplosion.ToString(), 0, ScoreEvent.damageExplosion.ToString(), string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.damageGrenade.ToString(), 0, ScoreEvent.damageGrenade.ToString(), string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.deadMech.ToString(), 40, "Key_1129", "MechKill"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.deadTurret.ToString(), 40, "Key_1130", "TurretKill"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.dead.ToString(), 15, "Key_1127", "Kill"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.deadHeadShot.ToString(), 30, "Key_1128", "Headshot"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.deadLongShot.ToString(), 45, "Key_1133", string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.invisibleKill.ToString(), 30, "Key_1131", "InvisibleKill"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.doubleHeadShot.ToString(), 40, "Key_1132", "DoubleHeadshot"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.deadWithFlag.ToString(), 20, "Key_1144", "FlagKill"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.deathFromAbove.ToString(), 20, "Key_1215", "DeathFromAboue"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.duckHunt.ToString(), 20, "Key_1214", "DuckHunt"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.rocketJumpKill.ToString(), 20, "Key_1216", "RocketJumpKill"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.melee.ToString(), 30, "Key_1270", string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.melee2.ToString(), 30, "Key_1209", "Butcer"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.melee3.ToString(), 45, "Key_1210", "MadButcer"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.melee5.ToString(), 75, "Key_1211", "Slaughter"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.melee7.ToString(), 150, "Key_1212", "Massacre"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.backup1.ToString(), 45, "Key_2085", "Backup_1"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.backup2.ToString(), 75, "Key_2086", "Backup_2"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.backup3.ToString(), 150, "Key_2087", "Backup_3"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.premium1.ToString(), 45, "Key_2091", "Premium_1"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.premium2.ToString(), 75, "Key_2092", "Premium_2"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.premium3.ToString(), 150, "Key_2093", "Premium_3"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.primary1.ToString(), 45, "Key_2088", "Primary_1"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.primary2.ToString(), 75, "Key_2089", "Primary_2"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.primary3.ToString(), 150, "Key_2090", "Primary_3"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.sniper1.ToString(), 45, "Key_2097", "Sniper_1"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.sniper2.ToString(), 75, "Key_2098", "Sniper_2"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.sniper3.ToString(), 150, "Key_2099", "Sniper_3"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.special1.ToString(), 45, "Key_2094", "Special_1"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.special2.ToString(), 75, "Key_2095", "Special_2"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.special3.ToString(), 150, "Key_2096", "Special_3"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killAssist.ToString(), 5, "Key_1143", "KillAssist"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.teamKill.ToString(), 10, "Key_1147", string.Empty));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.revenge.ToString(), 30, "Key_1206", "Revenge"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.flagTouchDown.ToString(), 100, "Key_1145", "TouchDown"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.flagTouchDouble.ToString(), 300, "Key_1195", "DoubleTouchDown"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.flagTouchDownTriple.ToString(), 500, "Key_1146", "TripleTouchDown"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.multyKill2.ToString(), 20, "Key_1135", "kill_1"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.multyKill3.ToString(), 30, "Key_1136", "kill_2"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.multyKill4.ToString(), 40, "Key_1137", "kill_3"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.multyKill5.ToString(), 50, "Key_1138", "kill_4"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.multyKill6.ToString(), 60, "Key_1139", "kill_5"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.multyKill10.ToString(), 100, "Key_1140", "kill_9"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.multyKill20.ToString(), 350, "Key_1141", "kill_19"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.multyKill50.ToString(), 1000, "Key_1142", "kill_49"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killMultyKill2.ToString(), 10, "Key_1213", "Nemesis"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killMultyKill3.ToString(), 15, "Key_1213", "Nemesis"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killMultyKill4.ToString(), 20, "Key_1213", "Nemesis"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killMultyKill5.ToString(), 25, "Key_1213", "Nemesis"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killMultyKill6.ToString(), 30, "Key_1213", "Nemesis"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killMultyKill10.ToString(), 50, "Key_1213", "Nemesis"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killMultyKill20.ToString(), 175, "Key_1213", "Nemesis"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.killMultyKill50.ToString(), 500, "Key_1213", "Nemesis"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.deadGrenade.ToString(), 50, "Key_1134", "GrenadeKill"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.deadExplosion.ToString(), 15, "Key_1127", "Kill"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.teamCapturePoint.ToString(), 100, "Key_1271", "TeamCapture"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.mySpotPoint.ToString(), 200, "Key_1272", "MySpot"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.unstoppablePoint.ToString(), 500, "Key_1273", "Unstoppable"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.monopolyPoint.ToString(), 800, "Key_1274", "Monopoly"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.houseKeeperPoint.ToString(), 10, "Key_1275", "HouseKeeper"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.defenderPoint.ToString(), 30, "Key_1276", "Defender"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.guardianPoint.ToString(), 50, "Key_1277", "Guardian"));
		_eventsScoreInfo.Add(new ScoreEventInfo(ScoreEvent.oneManArmyPoint.ToString(), 100, "Key_1278", "OneManArmy"));
	}

	public static void SetLocalizeForScoreEvent()
	{
		scoreOnEvent.Clear();
		messageOnEvent.Clear();
		pictureNameOnEvent.Clear();
		audioClipNameOnEvent.Clear();
		foreach (ScoreEventInfo item in _eventsScoreInfo)
		{
			scoreOnEvent.Add(item.eventId, item.eventScore);
			messageOnEvent.Add(item.eventId, item.eventMessage);
			pictureNameOnEvent.Add(item.eventId, item.pictName);
			if (!string.IsNullOrEmpty(item.audioClipName) && !audioClipNameOnEvent.ContainsKey(item.pictName))
			{
				audioClipNameOnEvent.Add(item.pictName, item.audioClipName);
			}
		}
	}
}
