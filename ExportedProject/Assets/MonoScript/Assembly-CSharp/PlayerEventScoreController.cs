using I2.Loc;
using System;
using System.Collections.Generic;

public class PlayerEventScoreController
{
	private static List<PlayerEventScoreController.ScoreEventInfo> _eventsScoreInfo;

	public static Dictionary<string, int> scoreOnEvent;

	public static Dictionary<string, string> messageOnEvent;

	public static Dictionary<string, string> pictureNameOnEvent;

	public static Dictionary<string, string> audioClipNameOnEvent;

	static PlayerEventScoreController()
	{
		PlayerEventScoreController._eventsScoreInfo = new List<PlayerEventScoreController.ScoreEventInfo>();
		PlayerEventScoreController.scoreOnEvent = new Dictionary<string, int>();
		PlayerEventScoreController.messageOnEvent = new Dictionary<string, string>();
		PlayerEventScoreController.pictureNameOnEvent = new Dictionary<string, string>();
		PlayerEventScoreController.audioClipNameOnEvent = new Dictionary<string, string>();
		PlayerEventScoreController.SetScoreEventInfo();
		PlayerEventScoreController.SetLocalizeForScoreEvent();
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(PlayerEventScoreController.SetLocalizeForScoreEvent));
	}

	public PlayerEventScoreController()
	{
	}

	public static void SetLocalizeForScoreEvent()
	{
		PlayerEventScoreController.scoreOnEvent.Clear();
		PlayerEventScoreController.messageOnEvent.Clear();
		PlayerEventScoreController.pictureNameOnEvent.Clear();
		PlayerEventScoreController.audioClipNameOnEvent.Clear();
		foreach (PlayerEventScoreController.ScoreEventInfo scoreEventInfo in PlayerEventScoreController._eventsScoreInfo)
		{
			PlayerEventScoreController.scoreOnEvent.Add(scoreEventInfo.eventId, scoreEventInfo.eventScore);
			PlayerEventScoreController.messageOnEvent.Add(scoreEventInfo.eventId, scoreEventInfo.eventMessage);
			PlayerEventScoreController.pictureNameOnEvent.Add(scoreEventInfo.eventId, scoreEventInfo.pictName);
			if (string.IsNullOrEmpty(scoreEventInfo.audioClipName) || PlayerEventScoreController.audioClipNameOnEvent.ContainsKey(scoreEventInfo.pictName))
			{
				continue;
			}
			PlayerEventScoreController.audioClipNameOnEvent.Add(scoreEventInfo.pictName, scoreEventInfo.audioClipName);
		}
	}

	public static void SetScoreEventInfo()
	{
		PlayerEventScoreController._eventsScoreInfo.Clear();
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageBody.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageBody.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageHead.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageHead.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageMechBody.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageMechBody.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageMechHead.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageMechHead.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageTurret.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageTurret.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageExplosion.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageExplosion.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageGrenade.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageGrenade.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadMech.ToString(), 40, "Key_1129", "MechKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadTurret.ToString(), 40, "Key_1130", "TurretKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.dead.ToString(), 15, "Key_1127", "Kill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadHeadShot.ToString(), 30, "Key_1128", "Headshot"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadLongShot.ToString(), 45, "Key_1133", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.invisibleKill.ToString(), 30, "Key_1131", "InvisibleKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.doubleHeadShot.ToString(), 40, "Key_1132", "DoubleHeadshot"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadWithFlag.ToString(), 20, "Key_1144", "FlagKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deathFromAbove.ToString(), 20, "Key_1215", "DeathFromAboue"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.duckHunt.ToString(), 20, "Key_1214", "DuckHunt"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.rocketJumpKill.ToString(), 20, "Key_1216", "RocketJumpKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee.ToString(), 30, "Key_1270", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee2.ToString(), 30, "Key_1209", "Butcer"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee3.ToString(), 45, "Key_1210", "MadButcer"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee5.ToString(), 75, "Key_1211", "Slaughter"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee7.ToString(), 150, "Key_1212", "Massacre"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.backup1.ToString(), 45, "Key_2085", "Backup_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.backup2.ToString(), 75, "Key_2086", "Backup_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.backup3.ToString(), 150, "Key_2087", "Backup_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.premium1.ToString(), 45, "Key_2091", "Premium_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.premium2.ToString(), 75, "Key_2092", "Premium_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.premium3.ToString(), 150, "Key_2093", "Premium_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.primary1.ToString(), 45, "Key_2088", "Primary_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.primary2.ToString(), 75, "Key_2089", "Primary_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.primary3.ToString(), 150, "Key_2090", "Primary_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.sniper1.ToString(), 45, "Key_2097", "Sniper_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.sniper2.ToString(), 75, "Key_2098", "Sniper_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.sniper3.ToString(), 150, "Key_2099", "Sniper_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.special1.ToString(), 45, "Key_2094", "Special_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.special2.ToString(), 75, "Key_2095", "Special_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.special3.ToString(), 150, "Key_2096", "Special_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killAssist.ToString(), 5, "Key_1143", "KillAssist"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.teamKill.ToString(), 10, "Key_1147", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.revenge.ToString(), 30, "Key_1206", "Revenge"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.flagTouchDown.ToString(), 100, "Key_1145", "TouchDown"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.flagTouchDouble.ToString(), 300, "Key_1195", "DoubleTouchDown"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.flagTouchDownTriple.ToString(), 500, "Key_1146", "TripleTouchDown"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill2.ToString(), 20, "Key_1135", "kill_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill3.ToString(), 30, "Key_1136", "kill_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill4.ToString(), 40, "Key_1137", "kill_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill5.ToString(), 50, "Key_1138", "kill_4"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill6.ToString(), 60, "Key_1139", "kill_5"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill10.ToString(), 100, "Key_1140", "kill_9"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill20.ToString(), 350, "Key_1141", "kill_19"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill50.ToString(), 1000, "Key_1142", "kill_49"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill2.ToString(), 10, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill3.ToString(), 15, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill4.ToString(), 20, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill5.ToString(), 25, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill6.ToString(), 30, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill10.ToString(), 50, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill20.ToString(), 175, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill50.ToString(), 500, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadGrenade.ToString(), 50, "Key_1134", "GrenadeKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadExplosion.ToString(), 15, "Key_1127", "Kill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.teamCapturePoint.ToString(), 100, "Key_1271", "TeamCapture"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.mySpotPoint.ToString(), 200, "Key_1272", "MySpot"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.unstoppablePoint.ToString(), 500, "Key_1273", "Unstoppable"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.monopolyPoint.ToString(), 800, "Key_1274", "Monopoly"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.houseKeeperPoint.ToString(), 10, "Key_1275", "HouseKeeper"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.defenderPoint.ToString(), 30, "Key_1276", "Defender"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.guardianPoint.ToString(), 50, "Key_1277", "Guardian"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.oneManArmyPoint.ToString(), 100, "Key_1278", "OneManArmy"));
	}

	public enum ScoreEvent
	{
		damageBody,
		damageHead,
		damageMechBody,
		damageMechHead,
		damageTurret,
		damageExplosion,
		damageGrenade,
		deadMech,
		deadTurret,
		dead,
		deadHeadShot,
		deadLongShot,
		invisibleKill,
		doubleHeadShot,
		deadWithFlag,
		killAssist,
		teamKill,
		revenge,
		deathFromAbove,
		duckHunt,
		rocketJumpKill,
		melee,
		melee2,
		melee3,
		melee5,
		melee7,
		primary1,
		primary2,
		primary3,
		backup1,
		backup2,
		backup3,
		special1,
		special2,
		special3,
		sniper1,
		sniper2,
		sniper3,
		premium1,
		premium2,
		premium3,
		flagTouchDown,
		flagTouchDouble,
		flagTouchDownTriple,
		multyKill2,
		multyKill3,
		multyKill4,
		multyKill5,
		multyKill6,
		multyKill10,
		multyKill20,
		multyKill50,
		killMultyKill2,
		killMultyKill3,
		killMultyKill4,
		killMultyKill5,
		killMultyKill6,
		killMultyKill10,
		killMultyKill20,
		killMultyKill50,
		deadGrenade,
		deadExplosion,
		teamCapturePoint,
		mySpotPoint,
		unstoppablePoint,
		monopolyPoint,
		houseKeeperPoint,
		defenderPoint,
		guardianPoint,
		oneManArmyPoint
	}

	private class ScoreEventInfo
	{
		public string eventId;

		public int eventScore;

		public string eventMessage;

		public string pictName;

		public string audioClipName;

		public ScoreEventInfo(string _eventId, int _eventScore, string _eventMessage, string _pictName)
		{
			this.eventId = _eventId;
			this.eventScore = _eventScore;
			this.eventMessage = _eventMessage;
			this.pictName = _pictName;
			this.audioClipName = string.Empty;
		}

		public ScoreEventInfo(string _eventId, int _eventScore, string _eventMessage, string _pictName, string _audioClipName)
		{
			this.eventId = _eventId;
			this.eventScore = _eventScore;
			this.eventMessage = _eventMessage;
			this.pictName = _pictName;
			this.audioClipName = _audioClipName;
		}
	}
}